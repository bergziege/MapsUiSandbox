using CommunityToolkit.Mvvm.Input;
using SegmentPlaner.Modules.Map.UI;
using System.Windows.Input;

namespace SegmentPlaner.UI.Windows.Shell
{
    class MainViewModel
    {
        private ICommand _NavigateCommand;
        private readonly MapViewCommand mapViewCommand;

        public MainViewModel(MapViewCommand mapViewCommand)
        {
            this.mapViewCommand = mapViewCommand;
        }

        public ICommand NavigateCommand => this._NavigateCommand ?? (this._NavigateCommand =
            new RelayCommand<string>(OnItemInvoked));

        private void OnItemInvoked(string navItemTag)
        {
            switch (navItemTag) {
                case "Map":
                    mapViewCommand.Execute();
                    break;
            }
        }
    }
}
