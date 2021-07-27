using System;

namespace Talkatives.Extensions.Messaging.Abstractions
{
    public interface IInprocMessageBus<in Tin> : IDisposable
    {
        bool Publish(Tin msg);
    }
}
