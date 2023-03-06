namespace SlotMachineApp.Model;

public interface IOutputProvider
{
    Task Clear(CancellationToken ct = default);
    Task ModifyMessage(string message, CancellationToken ct = default);
    Task PlaySound(NoteSet set, CancellationToken ct = default);
    Task WriteMessage(string message, CancellationToken ct = default);
}