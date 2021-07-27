using System.Collections.Generic;
using System.Threading;

namespace Talkatives.Extensions.Messaging.Abstractions
{
    public class AmbientContext
    {
        public const string EmailIdKey = "EmailId";
        private static readonly AsyncLocal<Dictionary<string, object>> Context;

        static AmbientContext()
        {
            Context = new AsyncLocal<Dictionary<string, object>>();
        }

        public static void Set(string key, object value)
        {
            Context.Value ??= new Dictionary<string, object>();
            Context.Value[key] = value;
        }

        public static void UnSet(string key)
        {
            if (Context.Value == null ||
                string.IsNullOrEmpty(key))
            {
                return;
            }
            Context.Value.Remove(key);
        }

        public static void Set(IDictionary<string, object> kvsParam)
        {
            if (kvsParam == null)
            {
                return;
            }
            var kvs = Context.Value ?? new Dictionary<string, object>();
            foreach (var (key, value) in kvsParam)
            {
                kvs[key] = value;
            }
            Context.Value = kvs;
        }

        public static IDictionary<string, object> GetContext(bool clone = false)
        {
            return clone
                ? new Dictionary<string, object>(Context.Value ?? new Dictionary<string, object>())
                : Context.Value ?? new Dictionary<string, object>();
        }
    }
}