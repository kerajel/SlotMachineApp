using Microsoft.Extensions.Options;
using System.Text;

namespace SlotMachineApp.Model;

internal class SlotMachineGame : ISlotMachineGame
{
    private readonly SlotMachineSettings _settings;
    private readonly ISpinnerService _spinnerService;
    private readonly ISpinnerSectionService _spinnerSectionService;
    private readonly IOutputProvider _outputProvider;
    private readonly ISoundService _soundService;
    private readonly IInputSource _inputSource;

    private readonly Random _random = new();

    private int _credits;
    private int? _lastSpinResult;


    public bool GameIsOn { get; private set; }

    public SlotMachineGame(IOptions<SlotMachineSettings> settingsOptions,
        ISpinnerService spinnerService,
        ISpinnerSectionService spinnerSectionService,
        IOutputProvider outputProvider,
        IInputSource inputSource,
        ISoundService soundService)
    {
        _settings = settingsOptions.Value;
        _spinnerService = spinnerService;
        _spinnerSectionService = spinnerSectionService;
        _outputProvider = outputProvider;
        _inputSource = inputSource;
        _soundService = soundService;
    }

    public async Task SubmitCommand(CancellationToken ct = default)
    {
        var input = await _inputSource.GetInputAsync(ct);

        await OutputGameInfo(ct);

        await (input.Key switch
        {
            ConsoleKey.A => AddCredit(ct),
            ConsoleKey.M => MaxCredits(ct).ContinueWith(_ => Spin(ct)).Unwrap(),
            ConsoleKey.S => Spin(ct),
            ConsoleKey.P => TryCompleteGame(ct),
            _ => _outputProvider.WriteMessage("Unrecognized command", ct)
        });
    }

    public async Task StartGame(CancellationToken ct = default)
    {
        _credits = _settings.DefaultCredits;
        GameIsOn = true;
        await OutputGameInfo(ct);
    }

    private async Task Spin(CancellationToken ct = default)
    {
        if (_credits == 0)
        {
            await _outputProvider.WriteMessage("Not enough credits!", ct);
            return;
        }

        var creditsSpent = GetRandomCreditAmountToSpend();
        _credits -= creditsSpent;

        await _outputProvider.WriteMessage($"Off with {creditsSpent} credit spin!", ct);

        var sections = await _spinnerService.SpinAllSpinners(ct);

        var creditsGained = _spinnerSectionService.GetSectionSetCreditGain(sections, creditsSpent);
        _lastSpinResult = creditsGained;

        if (creditsGained > 0)
        {
            await _soundService.PlaySound("Fanfares", ct);
        }

        AddCredits(creditsGained);

        await _outputProvider.WriteMessage($"Your have rolled {creditsGained} credits!", ct);
        await _outputProvider.WriteMessage($"Credits remaining: {_credits}");
    }

    private async Task MaxCredits(CancellationToken ct = default)
    {
        _credits = _settings.MaxCredits;
        await _outputProvider.WriteMessage($"Credits are maxed! Total credits: {_credits}", ct);
    }

    private async Task AddCredit(CancellationToken ct = default)
    {
        AddCredits(1);
        await _outputProvider.WriteMessage($"Added 1 credit. Total credits: {_credits}", ct);
    }

    private async Task TryCompleteGame(CancellationToken ct = default)
    {
        if (_credits > 0)
        {
            await _outputProvider.WriteMessage($"You still have {_credits} to brave your luck!", ct);
            await _outputProvider.WriteMessage($"Are you sure you want to quit ? (Y / N)", ct);

            var inputKey = ConsoleKey.NoName;
            while (inputKey != ConsoleKey.Y && inputKey != ConsoleKey.N)
            {
                var input = await _inputSource.GetInputAsync(ct);
                inputKey = input.Key;
            }

            if (inputKey == ConsoleKey.N)
            {
                await OutputGameInfo(ct);
                return;
            }
        }

        await _outputProvider.WriteMessage($"Congratulations! You have won {_credits} credits!", ct);
        _credits = 0;
        GameIsOn = false;
    }

    private void AddCredits(int creditAmount)
    {
        var additionResult = _credits + creditAmount;
        _credits = additionResult > _settings.MaxCredits ? _settings.MaxCredits : additionResult;
    }

    private int GetRandomCreditAmountToSpend()
    {
        var creditsSpentRange = _settings.CreditsSpentPerSpinRange;
        var maxCreditsForThisSpin = creditsSpentRange.To > _credits ? _credits : creditsSpentRange.To;
        var creditsSpentForThisSpin = _random.Next(creditsSpentRange.From, maxCreditsForThisSpin + 1);
        return creditsSpentForThisSpin;
    }

    private async Task OutputGameInfo(CancellationToken ct = default)
    {
        var sb = new StringBuilder();

        sb.AppendLine("Welcome to Slot Machine Game!");
        sb.AppendLine();
        sb.AppendLine("Controls:");
        sb.AppendLine($"{ConsoleKey.S} - spin");
        sb.AppendLine($"{ConsoleKey.A} - add one credit");
        sb.AppendLine($"{ConsoleKey.M} - max credits and spin");
        sb.AppendLine($"{ConsoleKey.P} - payout and exit");
        sb.AppendLine();
        sb.AppendLine($"You have {_credits} credits");

        if (_lastSpinResult.HasValue)
        {
            sb.AppendLine($"Last spin {_lastSpinResult.Value} credits");
        }

        await _outputProvider.Clear(ct);
        await _outputProvider.ModifyMessage(sb.ToString(), ct);
    }
}
