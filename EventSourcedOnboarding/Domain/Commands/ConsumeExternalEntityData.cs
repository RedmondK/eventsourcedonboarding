using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class ConsumeExternalEntityData
    {
        public ConsumeExternalEntityData(Guid entityGuid, string externalEntityDataSet)
        {
            EntityGuid = entityGuid;
            ExternalEntityDataSet = externalEntityDataSet;
        }

        public Guid EntityGuid { get; }
        public string ExternalEntityDataSet { get; }
    }
}
