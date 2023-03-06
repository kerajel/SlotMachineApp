namespace SlotMachineApp.Model;

public class ConsoleOutputProvider : IOutputProvider
{
    public Task ModifyMessage(string message, CancellationToken ct = default)
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(message.PadRight(Console.WindowWidth - 1));
        return Task.CompletedTask;
    }

    public Task WriteMessage(string message, CancellationToken ct = default)
    {
        Console.WriteLine();
        Console.WriteLine(message);
        return Task.CompletedTask;
    }

    public Task Clear(CancellationToken ct = default)
    {
        Console.Clear();
        return Task.CompletedTask;
    }

    public async Task PlaySound(NoteSet set, CancellationToken ct = default)
    {
        await Task.Run(() =>
        {
            foreach (var note in set.Notes)
            {
                Console.Beep(note.Frequency, note.Duration);
            }
        }, ct);
    }
}
