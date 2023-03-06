namespace SlotMachineApp.Model;

public interface ISpinnerSectionService
{
    int GetSectionSetCreditGain(ICollection<SpinnerSection> sections, int creditCount);
}