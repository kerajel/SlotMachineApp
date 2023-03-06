namespace SlotMachineApp.Model;

public interface IInputSource
{
    Task<ConsoleKeyInfo> GetInputAsync(CancellationToken cancellationToken);
}
