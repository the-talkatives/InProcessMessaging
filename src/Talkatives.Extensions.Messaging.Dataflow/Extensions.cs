using Microsoft.Extensions.DependencyInjection;
using Talkatives.Extensions.Messaging.Abstractions;

namespace Talkatives.Extensions.Messaging.Dataflow
{
    public static class Extensions
    {
        /// <summary>
        /// Add the generic implementation for the in-proc service bus. Please note that this needs to be called to then get specific handlers for
        /// events based on this implementation.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterGenericInProcPublisher(this IServiceCollection services, InprocMessageBusConfiguration configuration)
        {
            services.AddSingleton(configuration);
            return services.AddSingleton(typeof(IInprocMessageBus<>), typeof(InprocBufferedMessageBus<>));
        }
    }
}
