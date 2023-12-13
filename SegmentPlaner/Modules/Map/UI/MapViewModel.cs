using CommunityToolkit.Mvvm.Messaging;
using GpxDeSerializer.Model;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Nts.Providers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using NetTopologySuite.Geometries;
using SegmentPlaner.Modules.Map.Dtos;
using SegmentPlaner.Modules.Map.Events;
using SegmentPlaner.Modules.SegmentClassifications.Events;
using SegmentPlaner.Modules.Segments.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SegmentPlaner.Modules.Map.UI
{
    internal class MapViewModel : 
        IRecipient<SegmentsLoadedEvent>, 
        IRecipient<Segments.Events.SegmentSelectedInListEvent>
    {
        private readonly IGpxService gpxService;
        private readonly WeakReferenceMessenger messenger;
        private Mapsui.Map map;
        private readonly Layer segmentLayer = new Layer() { IsMapInfoLayer = true };
        private readonly RasterizingLayer rasterizedSegmentLayer;
        private object colors;
        private Guid? lastHighlightedSegmentId;
        private List<GeometryFeature> segmentFeatures;

        public MapViewModel(
            Segments.UI.SegmentViewModel segmentViewModel, 
            IGpxService gpxService, 
            WeakReferenceMessenger messenger)
        {
            SegmentViewModel = segmentViewModel;
            this.gpxService = gpxService;
            this.messenger = messenger;
            messenger.RegisterAll(this);

            rasterizedSegmentLayer = new RasterizingLayer(segmentLayer);
        }

        public Segments.UI.SegmentViewModel SegmentViewModel { get; set; }

        internal void RegisterMap(Mapsui.Map map)
        {
            this.map = map;
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            //map.Layers.Add(segmentLayer);
            map.Layers.Add(rasterizedSegmentLayer);
        }

        public static LineString CreateLineString(Gpx gpsData)
        {
            return new LineString(gpsData.Trk.Trkseg.Trkpt.Select(x => SphericalMercator.FromLonLat(x.Lon, x.Lat).ToCoordinate()).ToArray());
        }

        public static IStyle CreateLineStringStyle(IList<Windows.UI.Color> colors)
        {
            Mapsui.Styles.Color lineColor = Mapsui.Styles.Color.Gray;
            if (colors.Any())
            {
                lineColor = Mapsui.Styles.Color.FromArgb(colors[0].A, colors[0].R, colors[0].G, colors[0].B);
            }

            return new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = new Mapsui.Styles.Pen {  Color = lineColor, Width = 5 }
            };
        }

        public async void Receive(SegmentsLoadedEvent message)
        {
            segmentFeatures = new List<GeometryFeature>();

            IList<SegmentWithGpsDataDto> segmentWithDataDtos = await gpxService.GetGpsDataForSegmentsAsync(message.Segments);

            // https://raw.githubusercontent.com/Mapsui/Mapsui/master/Samples/Mapsui.Samples.Common/Maps/Geometries/LineStringSample.cs

            foreach (var segmentWithDataDto in segmentWithDataDtos)
            {
                GeometryFeature geometryFeature = new GeometryFeature()
                {
                    Geometry = CreateLineString(segmentWithDataDto.GpsData)
                };

                geometryFeature["segmentId"] = segmentWithDataDto.Segment.Id;
                geometryFeature["segment"] = segmentWithDataDto.Segment;

                AddDefaultRouteStyle(geometryFeature);

                segmentFeatures.Add(geometryFeature);
            }

            GeometrySimplifyProvider simplifyProvider = new GeometrySimplifyProvider(new MemoryProvider(segmentFeatures));

            segmentLayer.DataSource = simplifyProvider;
            map.RefreshData();
        }

        private void AddDefaultRouteStyle(GeometryFeature geometryFeature)
        {
            VectorStyle bgStyle = new VectorStyle { Line = new Mapsui.Styles.Pen(Mapsui.Styles.Color.Black, 7) { PenStyle = PenStyle.Solid, PenStrokeCap = PenStrokeCap.Butt, StrokeJoin = StrokeJoin.Bevel, StrokeMiterLimit = 1 } };
            geometryFeature.Styles.Add(bgStyle);

            VectorStyle style = new VectorStyle { Line = new Mapsui.Styles.Pen(Mapsui.Styles.Color.Gray, 5) { PenStyle = PenStyle.Solid, PenStrokeCap = PenStrokeCap.Butt } };
            geometryFeature.Styles.Add(style);
        }

        public void Receive(Segments.Events.SegmentSelectedInListEvent message)
        {
            RemoveHighlight();
            if (message.SegmentId.HasValue)
            {
                Highlight(message.SegmentId.Value);
            }
            segmentLayer.DataHasChanged();
        }

        private void Highlight(Guid segmentId)
        {
            ICollection<IStyle> styles = GetStyleBySegmentId(segmentId);
            if (styles.ElementAt(0) is VectorStyle bgStyle)
            {
                bgStyle.Line = new Mapsui.Styles.Pen(Mapsui.Styles.Color.Lime, 15);
            }
            lastHighlightedSegmentId = segmentId;
        }

        private void RemoveHighlight()
        {
            if (!lastHighlightedSegmentId.HasValue)
            {
                return;
            }

            ICollection<IStyle> styles = GetStyleBySegmentId(lastHighlightedSegmentId.Value);
            if (styles.ElementAt(0) is VectorStyle bgStyle)
            {
                bgStyle.Line = new Mapsui.Styles.Pen(Mapsui.Styles.Color.Black, 7);
            }

            lastHighlightedSegmentId = null;
        }

        private ICollection<IStyle> GetStyleBySegmentId(Guid lastHighlightedSegmentId)
        {
            GeometryFeature segment = GetFeatureBySegmenId(lastHighlightedSegmentId);
            return segment.Styles;
        }

        private GeometryFeature? GetFeatureBySegmenId(Guid lastHighlightedSegmentId)
        {
            return segmentFeatures.First(x => (Guid)x["segmentId"] == lastHighlightedSegmentId) as GeometryFeature;
        }

        internal void NotifySegmentSelected(Guid? guid)
        {
            messenger.Send(new SegmentSelectedInMapEvent(guid));

            RemoveHighlight();
            if (guid.HasValue)
            {
                Highlight(guid.Value);
            }
            segmentLayer.DataHasChanged();
        }
    }
}
