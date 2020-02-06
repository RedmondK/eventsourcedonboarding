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
        public string EntityName { get; }
        public CaseStatus CurrentStatus { get; }

        public Case(Guid entityId, string entityName)
        {
            Raise(new EntityCreatedEvent(entityId, entityName));
        }
    }
}
