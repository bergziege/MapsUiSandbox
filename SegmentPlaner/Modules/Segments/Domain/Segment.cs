using System;
using System.Collections.Generic;

namespace SegmentPlaner.Modules.Segments.Domain
{
    public class Segment
    {
        public Segment()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public IList<Guid> CycleRouteIds { get; set; } = new List<Guid>();

        public IList<Guid> SegmentClassificationIds { get; set; } = new List<Guid>();

        public IList<Guid> LabelIds { get; set; } = new List<Guid>();

        public void Update(string name, 
            IList<Guid> cycleRouteIds, 
            IList<Guid> segmentClassificationIds, 
            IList<Guid> labelIds)
        {
            Name = name;
            CycleRouteIds = cycleRouteIds;
            SegmentClassificationIds = segmentClassificationIds;
            LabelIds = labelIds;
        }
    }
}
