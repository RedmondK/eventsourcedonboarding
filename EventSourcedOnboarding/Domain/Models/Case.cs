using Domain.Enums;
using Domain.Events;
using EventStoreFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Case : AggregateRoot
    {
        public string EntityName { get; set; }
        public CaseStatus CurrentStatus { get; set; }

        public Case(Guid entityId, string entityName) : this()
        {
            Id = Guid.NewGuid();

            Raise(new EntityCreatedEvent(entityId, entityName));
        }

        private Case()
        {
            Register<EntityCreatedEvent>(When);
        }

        public void When(EntityCreatedEvent e)
        {
            CurrentStatus = CaseStatus.InProgress;
            Id = e.EntityId;
            EntityName = e.EntityName;
        }
    }
}
