using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class BasicDetailsAdded
    {
        public BasicDetailsAdded(Guid entityId, string dateOfBirth, string countryOfResidence)
        {
            EntityId = entityId;
            DateOfBirth = dateOfBirth;
            CountryOfResidence = countryOfResidence;
        }

        public Guid EntityId { get; }
        public string DateOfBirth { get; }
        public string CountryOfResidence { get; }
    }
}
