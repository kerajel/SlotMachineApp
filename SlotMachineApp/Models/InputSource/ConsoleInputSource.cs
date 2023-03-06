using System.Diagnostics;

namespace SlotMachineApp.Model;
public class ConsoleInputSource : IInputSource
{
    public async Task<ConsoleKeyInfo> GetInputAsync(CancellationToken ct)
    {
        return await Task.Run(() =>
        {
            while (Console.KeyAvailable)
            {
                _ = Console.ReadKey(true);
            }

            return Console.ReadKey(true);
        }, ct);
    }
}
