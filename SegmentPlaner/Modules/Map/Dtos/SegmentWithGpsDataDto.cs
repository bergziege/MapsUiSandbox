using GpxDeSerializer.Model;
using SegmentPlaner.Modules.Segments.Domain;

namespace SegmentPlaner.Modules.Map.Dtos;

public class SegmentWithGpsDataDto
{
    public SegmentWithGpsDataDto(Segment segment, Gpx gpsData)
    {
        Segment = segment;
        GpsData = gpsData;
    }

    public Segment Segment { get; }
    public Gpx GpsData { get; }
}
