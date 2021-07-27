using System.Collections.Generic;

namespace Talkatives.Extensions.Messaging.Dataflow
{
    public class InprocMessage<T>
    {
        #region .ctor

        public InprocMessage(IDictionary<string, object> ambientData, T msg)
        {
            AmbientContextData = ambientData;
            Message = msg;
        }

        #endregion

        public IDictionary<string, object> AmbientContextData { get; private set; }

        public T Message { get; private set; }
    }
}
