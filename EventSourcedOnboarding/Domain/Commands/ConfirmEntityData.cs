using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class ConfirmEntityDataCommand
    {
        public ConfirmEntityDataCommand(Guid entityGuid)
        {
            EntityGuid = entityGuid;
        }

        public Guid EntityGuid { get; }
    }
}
