using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talkatives.Extensions.Messaging.Abstractions;

namespace Talkatives.InProcessMessageTestConsole
{
    public class GenericConsumer<T> : IInprocMessageSubscriber<T>
    {
        private readonly int _id;
        private readonly List<T> _messages;

        public GenericConsumer(int id, List<T> messages)
        {
            _id = id;
            _messages = messages;
        }

        public async Task OnNextAsync(T message)
        {
            _messages.Add(message);
            Console.WriteLine("just checking");
            await Task.CompletedTask;
        }
    }
}
