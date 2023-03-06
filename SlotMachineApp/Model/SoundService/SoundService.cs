using Microsoft.Extensions.Options;

namespace SlotMachineApp.Model;

public class SoundService : ISoundService
{
    private readonly SoundSettings _soundSettings;
    private readonly IOutputProvider _outputProvider;
    private readonly INoteSetProvider _noteSetProvider;

    public SoundService(IOptions<SoundSettings> soundSettingsOptions, IOutputProvider outputProvider, INoteSetProvider noteSetProvider)
    {
        _soundSettings = soundSettingsOptions.Value;
        _outputProvider = outputProvider;
        _noteSetProvider = noteSetProvider;
    }

    public Task PlaySound(string soundName, CancellationToken ct = default)
    {
        if (_soundSettings.SoundEnabled)
        {
            var noteSet = _noteSetProvider.GetNoteSet(soundName);
            _ = _outputProvider.PlaySound(noteSet, ct);
        }
        return Task.CompletedTask;
    }
}
