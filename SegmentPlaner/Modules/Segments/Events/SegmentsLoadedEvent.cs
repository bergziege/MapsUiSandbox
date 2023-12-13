using SegmentPlaner.Modules.Segments.Domain;
using System.Collections.Generic;

namespace SegmentPlaner.Modules.SegmentClassifications.Events
{
    internal class SegmentsLoadedEvent
    {
        public SegmentsLoadedEvent(IList<Segment> segments)
        {
            Segments = segments;
        }

        public IList<Segment> Segments { get; }
    }
}
