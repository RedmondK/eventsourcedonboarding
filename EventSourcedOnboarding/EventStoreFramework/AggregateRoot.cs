using System;
using System.Collections.Generic;
using System.Text;

namespace EventStoreFramework
{
    public class AggregateRoot : IAggregateRoot
    {
        readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();

        readonly List<object> _events = new List<object>();

        public Guid Id { get; protected set; }

        public int Version { get; protected set; } = -1;

        public List<object> GetEvents()
        {
            return _events;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public void Apply(object e)
        {
            Raise(e);
            Version++;
        }

        protected void Raise(object e)
        {
            _handlers[e.GetType()](e);
            _events.Add(e);
        }
    }
}
