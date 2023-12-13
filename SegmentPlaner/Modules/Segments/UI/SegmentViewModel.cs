using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SegmentPlaner.Modules.Map.Events;
using SegmentPlaner.Modules.SegmentClassifications.Events;
using SegmentPlaner.Modules.Segments.Domain;
using SegmentPlaner.Modules.Segments.Events;
using SegmentPlaner.Modules.Segments.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace SegmentPlaner.Modules.Segments.UI;

internal class SegmentViewModel : 
    ObservableObject,
    IRecipient<SegmentSelectedInMapEvent>
{
    private readonly IServiceProvider serviceProvider;
    private readonly ISegmentService segmentService;
    private readonly Infrastructure.AppContext appContext;
    private readonly WeakReferenceMessenger messenger;
    private SegmentListItemViewModel? selectedSegment;
    private bool isSelectingFromEvent;

    public SegmentViewModel(IServiceProvider serviceProvider, ISegmentService segmentService, Infrastructure.AppContext appContext, WeakReferenceMessenger messenger)
    {
        this.serviceProvider = serviceProvider;
        this.segmentService = segmentService;
        this.appContext = appContext;
        this.messenger = messenger;

        messenger.RegisterAll(this);

        RefreshCommand = new AsyncRelayCommand(OnRefresh);
        CreateCommand = new AsyncRelayCommand(OnCreate);

        Reload();
    }

    private async Task OnCreate()
    {
        FileOpenPicker fileOpenPicker = new FileOpenPicker();
        fileOpenPicker.FileTypeFilter.Add(".gpx");

        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, appContext.MainWindowHandle);

        Windows.Storage.StorageFile storageFile = await fileOpenPicker.PickSingleFileAsync();

        if (!string.IsNullOrWhiteSpace(storageFile.Path))
        {
            Segment newSegment = await segmentService.CreateNewSegmentAsync(storageFile.Path);
            SegmentListItemViewModel itemVm = serviceProvider.GetRequiredService<SegmentListItemViewModel>();
            await itemVm.Init(newSegment, this);
            Segments.Add(itemVm);
        }
    }

    public AsyncRelayCommand RefreshCommand { get; }
    public AsyncRelayCommand CreateCommand { get; }

    public ObservableCollection<SegmentListItemViewModel> Segments { get; } = new ObservableCollection<SegmentListItemViewModel>();

    public SegmentListItemViewModel? SelectedSegment
    {
        get => selectedSegment; set
        {
            SetProperty(ref selectedSegment, value);
            if (!isSelectingFromEvent)
            {
                messenger.Send(new SegmentSelectedInListEvent(value?.Segment.Id)); 
            }
        }
    }

    private async Task OnRefresh()
    {
        await Reload();
    }

    private async Task Reload()
    {
        Segments.Clear();

        IList<Domain.Segment> segments = (await segmentService.GetAllSegmentsAsync()).OrderBy(x => x.Name).ToList();

        foreach (Domain.Segment segment in segments)
        {
            SegmentListItemViewModel itemVm = serviceProvider.GetRequiredService<SegmentListItemViewModel>();
            await itemVm.Init(segment, this);
            Segments.Add(itemVm);
        }

        messenger.Send(new SegmentsLoadedEvent(Segments.Select(x => x.Segment).ToList()));
    }

    public void Receive(SegmentSelectedInMapEvent message)
    {
        isSelectingFromEvent = true;
        if (message.SegmentId.HasValue)
        {
            SegmentListItemViewModel? segmentToSelect = Segments.FirstOrDefault(x => x.Segment.Id == message.SegmentId);
            if (segmentToSelect != null)
            {
                SelectedSegment = segmentToSelect;
            }
        }
        else
        {
            SelectedSegment = null;
        }
        isSelectingFromEvent = false;
    }
}
