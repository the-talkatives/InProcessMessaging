using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Talkatives.Extensions.Messaging.Abstractions;

namespace Talkatives.Extensions.Messaging.Dataflow
{
    public class InprocBufferedMessageBus<T> : IInprocMessageBus<T>
    {
        private CancellationTokenSource _cancellationTokenSource;
        private BufferBlock<InprocMessage<T>> _bufferBlock;
        private readonly TimeSpan _publishTimeout;
        private readonly ILogger<InprocBufferedMessageBus<T>> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IDisposable _observableSubscription;

        #region .ctor

        public InprocBufferedMessageBus(IServiceProvider serviceProvider
            , InprocMessageBusConfiguration configuration
            , ILogger<InprocBufferedMessageBus<T>> logger)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _bufferBlock = new BufferBlock<InprocMessage<T>>(new DataflowBlockOptions
            {
                BoundedCapacity = configuration.PublisherQueueSize,
                CancellationToken = _cancellationTokenSource.Token
            });
            _publishTimeout = TimeSpan.FromMilliseconds(configuration.PublishTimeoutMSec);
            _logger = logger;
            _serviceProvider = serviceProvider;
            _ = SetSubscriberTask();
        }

        #endregion

        public bool IsDisposed { get; private set; }

        #region IInProcBus<T, TConfiguration>

        public bool Publish(T msg)
        {
            if (IsDisposed || _bufferBlock == null)
            {
                return false;
            }

            return SpinWait.SpinUntil(
                () => _bufferBlock.Post(new InprocMessage<T>(AmbientContext.GetContext(true), msg)), _publishTimeout);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            try
            {
                _bufferBlock?.Complete();
                _observableSubscription?.Dispose();
                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
            }

            _observableSubscription = null;
            _cancellationTokenSource = null;
            _bufferBlock = null;
            IsDisposed = true;
        }

        #endregion

        #region helpers

        private async Task SetSubscriberTask()
        {
            try
            {
                do
                {
                    try
                    {
                        var msgContainer = await _bufferBlock.ReceiveAsync();
                        var subscribers = _serviceProvider.GetServices<IInprocMessageSubscriber<T>>().ToList();
                        AmbientContext.Set(msgContainer.AmbientContextData);
                        if (!subscribers.Any())
                        {
                            _logger?.LogWarning(
                                $"no subscriber found for handling message of type: {msgContainer.GetType()}");
                            continue;
                        }

                        var tSubscribers = subscribers.Select(s => s.OnNextAsync(msgContainer.Message));
                        await Task.WhenAll(tSubscribers);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex.ToString());
                    }
                } while (await _bufferBlock.OutputAvailableAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}