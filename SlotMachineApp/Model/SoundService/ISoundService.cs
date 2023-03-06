namespace SlotMachineApp.Model;

public interface ISoundService
{
    Task PlaySound(string soundName, CancellationToken ct = default);
}