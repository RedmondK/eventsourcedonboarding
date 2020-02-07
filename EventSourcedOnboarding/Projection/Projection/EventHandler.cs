using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectionFramework
{
    public class EventHandler
    {
        public string EventType { get; set; }

        public Action<object> Handler { get; set; }
    }
}