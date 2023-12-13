using CommunityToolkit.Mvvm.Messaging;
using GpxDeSerializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SegmentPlaner.Infrastructure;
using SegmentPlaner.Modules.Map;
using SegmentPlaner.Modules.Map.UI;
using SegmentPlaner.Modules.Segments;
using SegmentPlaner.UI.Windows.Shell;
using System;
using System.Configuration;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SegmentPlaner;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window _window;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        Services = ConfigureServices();

        InitializeComponent();

        LoadSettings();
    }

    private void LoadSettings()
    {
        var context = Services.GetRequiredService<Infrastructure.AppContext>();

        var appSettings = ConfigurationManager.AppSettings;

        context.SegmentsStorageDirectory = appSettings.GetValues(nameof(Infrastructure.AppContext.SegmentsStorageDirectory))[0];
    }

    public static IServiceProvider Services { get; private set; }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = Services.GetRequiredService<MainWindow>();
        _window.Activate();

        Services.GetRequiredService<MapViewCommand>().Execute();

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_window);
        Infrastructure.AppContext appContext = Services.GetRequiredService<Infrastructure.AppContext>();
        appContext.MainWindowHandle = hwnd;
    }

    private IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.UseSegments();
        services.UseMap();

        services.AddSingleton<Infrastructure.AppContext>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainViewModel>();

        services.AddSingleton(WeakReferenceMessenger.Default);
        services.AddSingleton<NavigationService>();
        services.AddSingleton<GpxDeserializer>();
        services.AddSingleton<GpxSerializer>();
        return services.BuildServiceProvider();
    }
}
