using Microsoft.Extensions.DependencyInjection;
using SegmentPlaner.Modules.Map.UI;

namespace SegmentPlaner.Modules.Map;

internal static class MapModule
{
    public static IServiceCollection UseMap(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<MapViewModel>();
        serviceCollection.AddSingleton<MapView>();

        serviceCollection.AddSingleton<MapViewCommand>();

        return serviceCollection;
    }
}
