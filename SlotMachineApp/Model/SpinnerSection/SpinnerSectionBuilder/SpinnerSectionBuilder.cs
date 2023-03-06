namespace SlotMachineApp.Model;

public class SpinnerSectionBuilder
{
    private SpinnerSection _spinnerSection;

    public SpinnerSectionBuilder()
    {
        _spinnerSection = new SpinnerSection();
    }

    public SpinnerSectionBuilder WithSectionType(SpinnerSectionType sectionType)
    {
        _spinnerSection.SectionType = sectionType;
        return this;
    }

    public SpinnerSectionBuilder WithGlyph(char glyph)
    {
        _spinnerSection.Glyph = glyph;
        return this;
    }

    public SpinnerSectionBuilder WithValuePerFullSet(int valuePerFullSet)
    {
        _spinnerSection.ValuePerFullSet = valuePerFullSet;
        return this;
    }

    public SpinnerSectionBuilder WithValuePerSingleItem(int valuePerSingleItem)
    {
        _spinnerSection.ValuePerSingleItem = valuePerSingleItem;
        return this;
    }

    public SpinnerSectionBuilder WithBaseSectionType(SpinnerSectionType? baseSectionType)
    {
        _spinnerSection.BaseSectionType = baseSectionType;
        return this;
    }

    public SpinnerSectionBuilder WithSectionCount(int sectionCount)
    {
        _spinnerSection.SectionCount = sectionCount;
        return this;
    }

    public SpinnerSection Build()
    {
        return _spinnerSection;
    }
}