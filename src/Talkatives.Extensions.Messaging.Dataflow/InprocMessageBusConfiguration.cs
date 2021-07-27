namespace Talkatives.Extensions.Messaging.Dataflow
{
    public class InprocMessageBusConfiguration
    {
        public int PublisherQueueSize { get; set; } = 100;

        public int PublishTimeoutMSec { get; set; } = 3000;
    }
}
