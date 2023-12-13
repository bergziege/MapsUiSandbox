using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SegmentPlaner.Modules.Segments.Domain;
using SegmentPlaner.Modules.Segments.Services;
using System.Threading.Tasks;

namespace SegmentPlaner.Modules.Segments.UI;

class SegmentListItemViewModel : ObservableObject
{
    private readonly WeakReferenceMessenger messenger;
    private readonly IGpxService gpxService;
    private string? segmentName;

    public SegmentListItemViewModel(WeakReferenceMessenger messenger, IGpxService gpxService)
    {
        this.messenger = messenger;
        this.gpxService = gpxService;
    }

    public string? SegmentName { get => segmentName; set =>SetProperty(ref segmentName, value); }
    public Segment Segment { get; set; }
    public SegmentViewModel Parent { get; private set; }

    public RelayCommand SelectForPlanningCommand { get; }
    public RelayCommand RequestShowWaypointsCommand { get; }
    public RelayCommand RequestSplitSegmentCommand { get; }

    public async Task Init(Segment segment, SegmentViewModel parent)
    {
        Segment = segment;
        Parent = parent;

        await UpdateFromModel();
    }

    private async Task UpdateFromModel()
    {
        SegmentName = Segment?.Name;
    }
}
