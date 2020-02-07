using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class EntityCreatedEvent
    {
        public EntityCreatedEvent(Guid entityId, string entityName)
        {
            EntityId = entityId;
            EntityName = entityName;
        }

        public Guid EntityId { get; }
        public string EntityName { get; }
    }
}
