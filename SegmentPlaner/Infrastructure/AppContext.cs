namespace SegmentPlaner.Infrastructure;

public class AppContext
{
    public string SegmentsStorageDirectory { get; set; }
    public nint MainWindowHandle { get; internal set; }
}
