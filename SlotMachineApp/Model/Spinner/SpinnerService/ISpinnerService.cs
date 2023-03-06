namespace SlotMachineApp.Model;

public interface ISpinnerService
{
    Task<ICollection<SpinnerSection>> SpinAllSpinners(CancellationToken ct = default);
}