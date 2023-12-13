using GpxDeSerializer.Model;

namespace SegmentPlaner.Modules.Segments.Dtos;

public class TrackpointDto
{
    public TrackpointDto(int index, Trkpt trackpoint)
    {
        Index = index;
        Trackpoint = trackpoint;
    }

    public int Index { get; }
    public Trkpt Trackpoint { get; }
}
