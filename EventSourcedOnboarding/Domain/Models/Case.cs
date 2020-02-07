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
        public CaseType CaseType { get; set; }

        public Case(CaseType caseType, Guid entityId, string entityName) : this()
        {
            Raise(new CaseInitiatedEvent(Guid.NewGuid(), caseType));
            Raise(new EntityCreatedEvent(entityId, entityName));
        }

        private Case()
        {
            Register<CaseInitiatedEvent>(When);
            Register<EntityCreatedEvent>(When);
        }

        public void When(CaseInitiatedEvent e)
        {
            Id = e.CaseId;
            CaseType = e.CaseType;
            CurrentStatus = CaseStatus.InProgress;
        }

        public void When(EntityCreatedEvent e)
        {
            EntityName = e.EntityName;
        }
    }
}
