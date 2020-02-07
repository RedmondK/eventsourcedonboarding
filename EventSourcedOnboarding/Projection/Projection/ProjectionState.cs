using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectionFramework
{
    public class ProjectionState
    {
        public string Id { get; set; }

        public long CommitPosition { get; set; }

        public long PreparePosition { get; set; }
    }
}