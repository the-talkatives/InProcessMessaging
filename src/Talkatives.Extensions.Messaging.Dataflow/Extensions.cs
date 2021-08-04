using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        //public static IServiceCollection AddInProcServiceBus<T>(this IServiceCollection services, int publisherQSize, TimeSpan publishTimeout)
        //{
        //    services.AddSingleton<IInProcBus<T>>(x => new InProcServiceBus<T>(x,
        //        publisherQSize,
        //        publishTimeout,
        //        x.GetService<ILogger>()));
        //    return services;
        //}
    }
}
