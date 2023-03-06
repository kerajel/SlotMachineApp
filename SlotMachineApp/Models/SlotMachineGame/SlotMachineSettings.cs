namespace SlotMachineApp.Model;

public class SlotMachineSettings
{
    public int DefaultCredits { get; set; }
    public int MaxCredits { get; set; }
    public Range<int> CreditsSpentPerSpinRange { get; set; }
    public bool SoundEnabled { get; set; }
}
