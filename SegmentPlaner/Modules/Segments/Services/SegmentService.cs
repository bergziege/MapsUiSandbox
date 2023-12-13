using SegmentPlaner.Modules.Segments.Domain;
using SegmentPlaner.Modules.Segments.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Services;

internal class SegmentService : ISegmentService
{
    private readonly ISegmentDao segmentDao;
    private readonly IGpxDao gpxDao;

    public SegmentService(ISegmentDao segmentDao, IGpxDao gpxDao)
    {
        this.segmentDao = segmentDao;
        this.gpxDao = gpxDao;
    }

    public async Task<Segment> CreateNewSegmentAsync(string sourceGpxFile)
    {
        string gpxName = Path.GetFileName(sourceGpxFile);
        Segment newSegment = new Segment() { Name = gpxName };
        await gpxDao.CopyGpxAsync(sourceGpxFile, newSegment.Id);
        await segmentDao.SaveSegmentAsync(newSegment);
        return newSegment;
    }

    public async Task DeleteAsync(Segment segmentToDelete)
    {
        await gpxDao.DeleteGpxByIdAsync(segmentToDelete.Id);
        await segmentDao.DeleteSegmentAsync(segmentToDelete);
    }

    public Task<IList<Segment>> GetAllSegmentsAsync()
    {
        return segmentDao.GetAllSegmentsAsync();
    }

    public Task<Segment> GetByIdAsync(Guid id)
    {
        return segmentDao.GetByIdAsync(id);
    }

    public async Task<Segment> UpdateAsync(Segment segmentToUpdate, string newName, IList<Guid> cycleRouteIds, IList<Guid> classificationIds, IList<Guid> labelIds)
    {
        segmentToUpdate.Update(newName, cycleRouteIds, classificationIds, labelIds);
        await segmentDao.SaveSegmentAsync(segmentToUpdate);
        return segmentToUpdate;
    }
}