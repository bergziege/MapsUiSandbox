using SegmentPlaner.Modules.Segments.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Persistence;

internal class SegmentDao : ISegmentDao
{
    private readonly Infrastructure.AppContext _appContext;

    public SegmentDao(Infrastructure.AppContext appContext)
    {
        _appContext = appContext;
    }

    public async Task DeleteSegmentAsync(Segment segment)
    {
        string jsonFilePath = Path.Combine(_appContext.SegmentsStorageDirectory, AsJsonFileName(segment));
        await Task.Run(() =>
        {
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }
        });
    }

    public async Task<IList<Segment>> GetAllSegmentsAsync()
    {
        string[] segmentJsons = Directory.GetFiles(_appContext.SegmentsStorageDirectory, "*.json");

        IList<Segment> segments = new List<Segment>();

        foreach (string segmentJson in segmentJsons)
        {
            using (FileStream jsonFileStream = File.OpenRead(segmentJson))
            {
                Segment deserializedRoute = await JsonSerializer.DeserializeAsync<Segment>(jsonFileStream);
                segments.Add(deserializedRoute);
            }
        }

        return segments;
    }

    public async Task<Segment> GetByIdAsync(Guid id)
    {
        string jsonFilePath = Path.Combine(_appContext.SegmentsStorageDirectory, AsJsonFileName(id));
        using (FileStream jsonFileStream = File.OpenRead(jsonFilePath))
        {
            return await JsonSerializer.DeserializeAsync<Segment>(jsonFileStream);
        }
    }

    public async Task SaveSegmentAsync(Segment segment)
    {
        using (FileStream segmentStream = File.Create(Path.Combine(_appContext.SegmentsStorageDirectory, AsJsonFileName(segment))))
        {
            await JsonSerializer.SerializeAsync(segmentStream, segment);
        }
    }

    private string AsJsonFileName(Segment segment)
    {
        return AsJsonFileName(segment.Id);
    }

    private string AsJsonFileName(Guid id)
    {
        return $"{id}.json";
    }
}
