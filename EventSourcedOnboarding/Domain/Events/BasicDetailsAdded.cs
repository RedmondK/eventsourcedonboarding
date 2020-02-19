using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class BasicDetailsAdded
    {
        public BasicDetailsAdded(Guid caseId, Guid entityId, string dateOfBirth, string countryOfResidence)
        {
            CaseId = caseId;
            EntityId = entityId;
            DateOfBirth = dateOfBirth;
            CountryOfResidence = countryOfResidence;
        }

        public Guid CaseId { get; }
        public Guid EntityId { get; }
        public string DateOfBirth { get; }
        public string CountryOfResidence { get; }
    }
}
