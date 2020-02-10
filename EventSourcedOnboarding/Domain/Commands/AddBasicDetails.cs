using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class AddBasicDetails
    {
        public AddBasicDetails(Guid caseId, Guid entityId, String dateOfBirth, String countryOfResidence) 
        {
            CaseId = caseId;
            EntityId = entityId;
            DateOfBirth = dateOfBirth;
            CountryOfResidence = countryOfResidence;
        }

        public Guid CaseId { get; }

        public Guid EntityId { get; }

        public String DateOfBirth { get; }

        public String CountryOfResidence { get; }
    }
}
