using SegmentPlaner.Infrastructure;

namespace SegmentPlaner.Modules.Map.UI;

internal class MapViewCommand
{
    private readonly NavigationService navService;

    public MapViewCommand(NavigationService navService)
    {
        this.navService = navService;
    }

    public void Execute()
    {
        navService.NavigateTo<MapView>();
    }
}
