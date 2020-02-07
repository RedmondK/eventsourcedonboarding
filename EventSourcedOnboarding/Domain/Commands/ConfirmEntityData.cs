using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class ConfirmEntityData
    {
        public ConfirmEntityData(Guid entityGuid)
        {
            EntityGuid = entityGuid;
        }

        public Guid EntityGuid { get; }
    }
}
