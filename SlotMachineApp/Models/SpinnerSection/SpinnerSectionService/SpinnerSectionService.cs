namespace SlotMachineApp.Model;

public class SpinnerSectionService : ISpinnerSectionService
{
    private readonly IDictionary<SpinnerSectionType, SpinnerSection> _spinnerSectionsByType;

    public SpinnerSectionService(IEnumerable<SpinnerSection> spinnerSections)
    {
        _spinnerSectionsByType = spinnerSections.ToDictionary(x => x.SectionType, x => x);
    }

    public int GetSectionSetCreditGain(ICollection<SpinnerSection> sections, int creditCount)
    {
        if (sections.Count == 0)
        {
            return 0;
        }

        var firstSection = sections.First();

        var derivedLineUp = sections.All(x => x.SectionType == firstSection.SectionType);
        if (derivedLineUp)
        {
            return firstSection.ValuePerFullSet * creditCount;
        }

        var baseLineUp = firstSection.BaseSectionType.HasValue
            && sections.All(x => x.BaseSectionType == firstSection.BaseSectionType);
        if (baseLineUp)
        {
            var baseSectionType = firstSection.BaseSectionType!.Value;
            var baseSection = _spinnerSectionsByType[baseSectionType];
            return baseSection.ValuePerFullSet * creditCount;
        }

        return sections.Sum(x => x.ValuePerSingleItem) * creditCount;
    }
}