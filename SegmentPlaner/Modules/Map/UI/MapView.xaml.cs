using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using Windows.System;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SegmentPlaner.Modules.Map.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapView : Page
    {
        public MapView()
        {
            this.InitializeComponent();

            MapViewModel viewModel = App.Services.GetRequiredService<MapViewModel>();
            viewModel.RegisterMap(MyMap.Map);

            MapViewModel = viewModel;
        }

        MapViewModel MapViewModel { get; set; }

        private void MyMapInfo(object sender, Mapsui.MapInfoEventArgs e)
        {
            bool? isSegmentFeature = e.MapInfo?.Feature?.Fields.Contains("segment");
            if (isSegmentFeature is true) 
            {
                MapViewModel.NotifySegmentSelected((Guid)e.MapInfo.Feature["segmentId"]);
            }
            else
            {
                MapViewModel.NotifySegmentSelected(null);
            }
        }

        static bool IsKeyDown(VirtualKey key)
        {
            return InputKeyboardSource
                .GetKeyStateForCurrentThread(key)
                .HasFlag(CoreVirtualKeyStates.Down);
        }
    }
}
