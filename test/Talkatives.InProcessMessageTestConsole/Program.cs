using System;
using System.Collections.Generic;
using System.Linq;
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
        private static IEnumerable<int> consumerIds;
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = configBuilder.Build();

            var services = new ServiceCollection();
            _messages = new List<string>();
            services.AddSingleton<List<string>>(_messages);

            services.RegisterGenericInProcPublisher(new InprocMessageBusConfiguration //_configuration.GetSection("") get from apsettings.json later
            {
                PublisherQueueSize = 100,
                PublishTimeoutMSec = 10000
            });

            _ = consumerIds.Select(id =>
            {
                return services.AddSingleton<IInprocMessageSubscriber<string>>(sp => new GenericConsumer<string>(id, sp.GetService<List<string>>()));
            }).ToList();

            _serviceProvider = services.BuildServiceProvider();
            //Configure(services);

            Console.WriteLine("Starting the process");

            //Run some tests using publisher and consumer
        }

      
    }
}
