namespace SlotMachineApp.Model;

public class SpinnerSettings
{
    public Range<TimeSpan> SpinDurationRange { get; set; }
    public TimeSpan BaseSectionChangeInterval { get; set; }
    public TimeSpan MaxSectionChangeInterval { get; set; }
    public TimeSpan SpinnerTransitionDelay { get; set; }
    public int SpinnerCount { get; set; }
}
