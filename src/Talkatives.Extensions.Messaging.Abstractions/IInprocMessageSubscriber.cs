using System.Threading.Tasks;

namespace Talkatives.Extensions.Messaging.Abstractions
{
    public interface IInprocMessageSubscriber<T>
    {
        Task OnNextAsync(T message);
    }
}
