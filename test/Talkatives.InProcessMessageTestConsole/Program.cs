using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Talkatives.Extensions.Messaging.Abstractions;
using Talkatives.Extensions.Messaging.Dataflow;

namespace Talkatives.InProcessMessageTestConsole
{
    class Program
    {
        private static IConfiguration _configuration;
        private static List<string> _messages;
        private static readonly IEnumerable<int> _consumerIds = new List<int> { 1, 2, 3 };
        private static IServiceProvider _serviceProvider;
        private static bool _loop = true;

        static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = configBuilder.Build();

            var services = new ServiceCollection();
            _messages = new List<string>();
            services.AddSingleton<List<string>>(_messages);
            services.AddLogging();

            services.RegisterGenericInProcPublisher(new InprocMessageBusConfiguration //_configuration.GetSection("") get from apsettings.json later
            {
                PublisherQueueSize = 100,
                PublishTimeoutMSec = 10000
            });

            _ = _consumerIds.Select(id =>
            {
                return services.AddSingleton<IInprocMessageSubscriber<string>>(sp => new GenericConsumer<string>(id, sp.GetService<List<string>>()));
            }).ToList();

            _serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("Starting the process");
            PubSubPublishTest();
            Console.ReadKey();
            _loop = false;
        }

        private static async Task PubSubPublishTest()
        { 
            var bus = _serviceProvider.GetService<IInprocMessageBus<string>>();
            int i = 0;
            do
            {
                bus.Publish($"msg: {i++}");
                await Task.Delay(100);
            } while (_loop);
        }
    }
}
