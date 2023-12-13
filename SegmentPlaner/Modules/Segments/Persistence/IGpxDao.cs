using GpxDeSerializer.Model;
using System;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Persistence;

public interface IGpxDao
{
    Task DeleteGpxByIdAsync(Guid id);

    Task CopyGpxAsync(string originFilePath, Guid destinationId);

    Task<Gpx> GetGpxContentAsync(Guid id);

    Task WriteGpxAsync(Gpx gpsData, string filePath);
    string CreateFilePath(Guid id);
}
