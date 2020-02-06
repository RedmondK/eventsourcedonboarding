using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CompleteCaseCommand
    {
        public CompleteCaseCommand(Guid caseGuid)
        {
            CaseGuid = caseGuid;
        }

        public Guid CaseGuid { get; }
    }
}
