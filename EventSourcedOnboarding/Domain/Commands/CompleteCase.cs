using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CompleteCase
    {
        public CompleteCase(Guid caseGuid)
        {
            CaseGuid = caseGuid;
        }

        public Guid CaseGuid { get; }
    }
}
