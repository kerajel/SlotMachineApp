using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace SlotMachineApp.Model;

public class SpinnerService : ISpinnerService
{
    private readonly IEnumerable<ISpinner> _spinners;
    private readonly SpinnerSettings _settings;
    private readonly IOutputProvider _progressReporter;
    private readonly Random _random = new();

    public SpinnerService(IEnumerable<ISpinner> spinners, IOptions<SpinnerSettings> settingsOptions, IOutputProvider progressReporter)
    {
        _spinners = spinners;
        _settings = settingsOptions.Value;
        _progressReporter = progressReporter;
    }

    public async Task<ICollection<SpinnerSection>> SpinAllSpinners(CancellationToken ct = default)
    {
        var sections = new List<SpinnerSection>(_spinners.Count());
        var sw = new Stopwatch();
        var sb = new StringBuilder();

        await _progressReporter.WriteMessage("*****", ct);

        foreach (var spinner in _spinners)
        {
            var sectionEnumerator = spinner.GetSectionEnumerator();
            var section = sectionEnumerator.Current;
            var spinDuration = GetRandomSpinDuration();

            sw.Restart();
            while (sw.Elapsed < spinDuration)
            {
                var delay = GetDecreasingDelay(spinDuration, sw.Elapsed);
                await Task.Delay(delay, ct);
                sectionEnumerator.MoveNext();
                section = sectionEnumerator.Current;

                sb.Append(section.Glyph);
                await _progressReporter.ModifyMessage(sb.ToString(), ct);
                sb.Remove(sb.Length - 1, 1);
            }

            sections.Add(section);
            sb.Append(section.Glyph);
            sb.Append(' ');
            await Task.Delay(_settings.SpinnerTransitionDelay, ct);
        }

        await _progressReporter.WriteMessage("*****", ct);

        return sections;
    }

    private TimeSpan GetRandomSpinDuration()
    {
        var spinDurationRange = _settings.SpinDurationRange;
        var range = spinDurationRange.To.Ticks - spinDurationRange.From.Ticks;
        var ticks = (long)(_random.NextDouble() * range) + spinDurationRange.From.Ticks;
        var result = TimeSpan.FromTicks(ticks);
        return result;
    }

    private TimeSpan GetDecreasingDelay(TimeSpan spinDuration, TimeSpan elapsed)
    {
        var baseChangeInterval = _settings.BaseSectionChangeInterval;
        var maxChangeInterval = _settings.MaxSectionChangeInterval;
        var slowdownCoeff = (spinDuration - elapsed).TotalMilliseconds / spinDuration.TotalMilliseconds;
        var delayMs = baseChangeInterval.TotalMilliseconds / Math.Min(slowdownCoeff, 1);
        var delay = TimeSpan.FromMilliseconds(delayMs);
        var result = delay > maxChangeInterval ? maxChangeInterval : delay;
        return result;
    }
}
