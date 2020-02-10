using Domain.Commands;
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
        public Guid EntityId { get; set; }
        
        public string EntityName { get; set; }

        public string EntityDateOfBirth { get; set; }

        public string EntityCountryOfResidence { get; set; }

        public CaseStatus CurrentStatus { get; set; }
        public CaseType CaseType { get; set; }

        public Case(CaseType caseType, Guid entityId, string entityName) : this()
        {
            Raise(new CaseInitiated(Guid.NewGuid(), caseType));
            Raise(new EntityCreated(entityId, entityName));
        }

        private Case()
        {
            Register<CaseInitiated>(When);
            Register<EntityCreated>(When);
            Register<AddBasicDetails>(When);
        }

        public void AddBasicDetails(string dateOfBirth, string countryOfResidence)
        {
            Raise(new AddBasicDetails(Id, EntityId, dateOfBirth, countryOfResidence));
        }

        public void When(CaseInitiated e)
        {
            Id = e.CaseId;
            CaseType = e.CaseType;
            CurrentStatus = CaseStatus.InProgress;
        }

        public void When(EntityCreated e)
        {
            EntityId = e.EntityId;
            EntityName = e.EntityName;
        }

        public void When(AddBasicDetails e)
        {
            EntityDateOfBirth = e.DateOfBirth;
            EntityCountryOfResidence = e.CountryOfResidence;
        }
    }
}
