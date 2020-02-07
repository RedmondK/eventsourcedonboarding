using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class CaseInitiatedEvent
    {
        public CaseInitiatedEvent(Guid caseId, CaseType caseType)
        {
            CaseId = caseId;
            CaseType = caseType;
        }

        public Guid CaseId { get; set; }

        public CaseType CaseType { get; set; }
    }
}
