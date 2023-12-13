using SegmentPlaner.Modules.Segments.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Services;

public interface ISegmentService
{
    Task<IList<Segment>> GetAllSegmentsAsync();

    Task<Segment> CreateNewSegmentAsync(string sourceGpxFile);

    Task<Segment> UpdateAsync(Segment segmentToUpdate,
        string newName,
        IList<Guid> cycleRouteIds,
        IList<Guid> classificationIds,
        IList<Guid> labelIds);

    Task DeleteAsync(Segment segmentToDelete);
    Task<Segment> GetByIdAsync(Guid id);
}
