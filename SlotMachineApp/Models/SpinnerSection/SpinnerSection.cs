namespace SlotMachineApp.Model;

public class SpinnerSection
{
    public SpinnerSectionType SectionType { get; set; }
    public char Glyph { get; set; }
    public int ValuePerFullSet { get; set; }
    public int ValuePerSingleItem { get; set; }
    public SpinnerSectionType? BaseSectionType { get; set; }
    public int SectionCount { get; set; }
}
