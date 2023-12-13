using System;

namespace SegmentPlaner.Modules.Map.Events;

internal class SegmentSelectedInMapEvent
{
    public SegmentSelectedInMapEvent(Guid? segmentId)
    {
        SegmentId = segmentId;
    }

    public Guid? SegmentId { get; }
}
