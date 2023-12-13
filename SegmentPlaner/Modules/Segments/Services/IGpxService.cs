using SegmentPlaner.Modules.Map.Dtos;
using SegmentPlaner.Modules.Segments.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Services;

public interface IGpxService
{
    Task<IList<SegmentWithGpsDataDto>> GetGpsDataForSegmentsAsync(IList<Segment> segments);
}
