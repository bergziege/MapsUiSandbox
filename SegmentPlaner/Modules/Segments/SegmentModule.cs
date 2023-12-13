using Microsoft.Extensions.DependencyInjection;
using SegmentPlaner.Modules.Segments.Persistence;
using SegmentPlaner.Modules.Segments.Services;
using SegmentPlaner.Modules.Segments.UI;

namespace SegmentPlaner.Modules.Segments;

internal static class SegmentModule
{
    public static IServiceCollection UseSegments(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISegmentDao, SegmentDao>();
        serviceCollection.AddSingleton<IGpxDao, GpxDao>();
        serviceCollection.AddSingleton<ISegmentService, SegmentService>();
        serviceCollection.AddSingleton<IGpxService, GpxService>();

        serviceCollection.AddTransient<SegmentListItemViewModel>();
        serviceCollection.AddTransient<SegmentViewModel>();
        serviceCollection.AddTransient<SegmentView>();

        return serviceCollection;
    }
}
