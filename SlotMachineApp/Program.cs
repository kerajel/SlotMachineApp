using Microsoft.Extensions.DependencyInjection;
using SlotMachineApp.Model;

namespace SlotMachineApp;

class Program
{
    static async Task Main()
    {
        var serviceProvider = Registry.GetServiceProvider();

        var game = serviceProvider.GetRequiredService<ISlotMachineGame>();

        var cancellationSource = new CancellationTokenSource();
        var ct = cancellationSource.Token;

        await game.StartGame(ct);

        while (game.GameIsOn)
        {
            await game.SubmitCommand(ct);
        }
    }
}