using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class EntityCreated
    {
        public EntityCreated(Guid entityId, string entityName)
        {
            EntityId = entityId;
            EntityName = entityName;
        }

        public Guid EntityId { get; }

        public string EntityName { get; }
    }
}
