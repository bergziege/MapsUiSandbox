using GpxDeSerializer.Model;
using SegmentPlaner.Modules.Map.Dtos;
using SegmentPlaner.Modules.Segments.Domain;
using SegmentPlaner.Modules.Segments.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.Services
{
    internal class GpxService : IGpxService
    {
        private readonly IGpxDao gpxDao;

        public GpxService(IGpxDao gpxDao)
        {
            this.gpxDao = gpxDao;
        }

        public async Task<IList<SegmentWithGpsDataDto>> GetGpsDataForSegmentsAsync(IList<Segment> segments)
        {
            IList<SegmentWithGpsDataDto> dtos = new List<SegmentWithGpsDataDto>();

            foreach (var segment in segments)
            {
                Gpx gpsData = await gpxDao.GetGpxContentAsync(segment.Id);
                dtos.Add(new SegmentWithGpsDataDto(segment, gpsData));
            }

            return dtos;
        }
    }
}
