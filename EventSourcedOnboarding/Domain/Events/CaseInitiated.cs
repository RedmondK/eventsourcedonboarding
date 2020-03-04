using Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class CaseInitiated
    {
        public CaseInitiated(Guid caseId, Guid entityId, CaseType caseType)
        {
            CaseId = caseId;
            EntityId = entityId;
            CaseType = caseType;
        }

        public Guid CaseId { get; set; }

        public Guid EntityId { get; set; }

        public CaseType CaseType { get; set; }
    }
}
