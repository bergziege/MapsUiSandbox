using System;

namespace SegmentPlaner.Modules.Segments.Events;

internal class SegmentSelectedInListEvent
{
    public SegmentSelectedInListEvent(Guid? segmentId)
    {
        SegmentId = segmentId;
    }

    public Guid? SegmentId { get; }
}
