using SegmentPlaner.Modules.Segments.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Persistence;

internal interface ISegmentDao
{
    Task DeleteSegmentAsync(Segment segment);
    Task<IList<Segment>> GetAllSegmentsAsync();
    Task<Segment> GetByIdAsync(Guid id);
    Task SaveSegmentAsync(Segment segment);
}
