using GpxDeSerializer;
using GpxDeSerializer.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Persistence;

public class GpxDao : IGpxDao
{
    private readonly Infrastructure.AppContext appContext;
    private readonly GpxDeserializer gpxDeserializer;
    private readonly GpxSerializer gpxSerializer;

    public GpxDao(Infrastructure.AppContext appContext, GpxDeserializer gpxDeserializer, GpxSerializer gpxSerializer)
    {
        this.appContext = appContext;
        this.gpxDeserializer = gpxDeserializer;
        this.gpxSerializer = gpxSerializer;
    }

    public async Task CopyGpxAsync(string sourceFilePath, Guid destinationId)
    {
        if (File.Exists(sourceFilePath))
        {
            string destinationFilePath = CreateFilePath(destinationId);
            await Task.Run(() => File.Copy(sourceFilePath, destinationFilePath, true));
        }
    }

    public async Task DeleteGpxByIdAsync(Guid id)
    {
        string fileToDelete = CreateFilePath(id);

        if (File.Exists(fileToDelete))
        {
            await Task.Run(() => File.Delete(fileToDelete));
        }
    }

    public async Task<Gpx> GetGpxContentAsync(Guid id)
    {
        string gpxPath = CreateFilePath(id);
        return await gpxDeserializer.DeSerialize(gpxPath);
    }

    public async Task WriteGpxAsync(Gpx gpsData, string filePath)
    {
        await gpxSerializer.Serialize(gpsData, filePath);
    }

    public string CreateFilePath(Guid id)
    {
        return Path.Combine(appContext.SegmentsStorageDirectory, AsGpxFileName(id));
    }

    private string AsGpxFileName(Guid id)
    {
        return $"{id}.gpx";
    }
}
