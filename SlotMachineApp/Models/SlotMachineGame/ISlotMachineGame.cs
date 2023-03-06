namespace SlotMachineApp.Model;

internal interface ISlotMachineGame
{
    bool GameIsOn { get; }

    Task StartGame(CancellationToken ct = default);
    Task SubmitCommand(CancellationToken ct = default);
}