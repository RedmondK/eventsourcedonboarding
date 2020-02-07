using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateEntity
    {
        public CreateEntity(string entityName)
        {
            EntityId = Guid.NewGuid();
            EntityName = entityName;
        }

        public Guid EntityId { get; }

        public String EntityName { get; }
    }
}
