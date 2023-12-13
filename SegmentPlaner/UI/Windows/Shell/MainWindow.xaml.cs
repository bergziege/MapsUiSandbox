using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;
using Windows.System;
using WinRT; // required to support Window.As<ICompositionSupportsSystemBackdrop>()

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SegmentPlaner.UI.Windows.Shell;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
internal sealed partial class MainWindow : Window
{
    WindowsSystemDispatcherQueueHelper _wsdqHelper; // See below for implementation.
    MicaController _backdropController;
    SystemBackdropConfiguration _configurationSource;

    public MainViewModel ViewModel { get; }

    public Frame RootFrame => XamlRootFrame;

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        TrySetSystemBackdrop();
        ViewModel = viewModel;
    }

    bool TrySetSystemBackdrop()
    {
        if (MicaController.IsSupported())
        {
            _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Create the policy object.
            _configurationSource = new SystemBackdropConfiguration();
            Activated += Window_Activated;
            Closed += Window_Closed;
            ((FrameworkElement)Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            _configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            _backdropController = new MicaController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            _backdropController.AddSystemBackdropTarget(this.As<ICompositionSupportsSystemBackdrop>());
            _backdropController.SetSystemBackdropConfiguration(_configurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on this system
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        _configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        // Make sure any Mica/Acrylic controller is disposed
        // so it doesn't try to use this closed window.
        if (_backdropController != null)
        {
            _backdropController.Dispose();
            _backdropController = null;
        }
        Activated -= Window_Activated;
        _configurationSource = null;
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)this.Content).ActualTheme)
        {
            case ElementTheme.Dark: _configurationSource.Theme = SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: _configurationSource.Theme = SystemBackdropTheme.Light; break;
            case ElementTheme.Default: _configurationSource.Theme = SystemBackdropTheme.Default; break;
        }
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is NavigationViewItem navItem)
        {
            ViewModel.NavigateCommand.Execute(navItem.Tag);
        }
    }

    private void NavView_BackRequested(NavigationView sender,
                                   NavigationViewBackRequestedEventArgs args)
    {
        TryGoBack();
    }

    private bool TryGoBack()
    {
        if (!XamlRootFrame.CanGoBack)
            return false;

        // Don't go back if the nav pane is overlayed.
        if (NavView.IsPaneOpen &&
            (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
             NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
            return false;

        XamlRootFrame.GoBack();
        return true;
    }
}

class WindowsSystemDispatcherQueueHelper
{
    [StructLayout(LayoutKind.Sequential)]
    struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }

    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

    object m_dispatcherQueueController = null;
    public void EnsureWindowsSystemDispatcherQueueController()
    {
        if (DispatcherQueue.GetForCurrentThread() != null)
        {
            // one already exists, so we'll just use it.
            return;
        }

        if (m_dispatcherQueueController == null)
        {
            DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
            options.threadType = 2;    // DQTYPE_THREAD_CURRENT
            options.apartmentType = 2; // DQTAT_COM_STA

            CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
        }
    }
}