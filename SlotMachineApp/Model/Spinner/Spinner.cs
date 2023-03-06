using SlotMachineApp.Helpers;

namespace SlotMachineApp.Model;

public class Spinner : ISpinner
{
    private readonly IEnumerable<SpinnerSection> _sections;
    private readonly SpinnerSection _blankSection;

    public Spinner(IEnumerable<SpinnerSection> spinnerSections)
    {
        _blankSection = spinnerSections.Single(x => x.SectionType == SpinnerSectionType.Blank);

        _sections = spinnerSections
            .Where(section => section != _blankSection)
            .SelectMany(section => Enumerable.Repeat(section, section.SectionCount))
            .OrderByRandom();
    }

    public IEnumerator<SpinnerSection> GetSectionEnumerator()
    {
        while (true)
        {
            foreach (var section in _sections)
            {
                yield return section;
                yield return _blankSection;
            }
        }
    }
}
