using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventStoreFramework
{
    public static class EventStoreHelper
    {
        public static object Deserialize(this ResolvedEvent resolvedEvent)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(resolvedEvent.Event.Data), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        }

        public static byte[] Serialize(this object e)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));
        }
    }
}
