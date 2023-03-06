using SlotMachineApp.Model;

namespace SlotMachineAppUnitTests.SpinnerSectionServiceTests
{
    public class Tests
    {
        [Test]
        public void GetSectionSetCreditGain_WhenSetIsBaseLineUp_OutputsExpectedResults()
        {
            // Arrange
            var baseSection1 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.BaseBar)
                .WithValuePerFullSet(15)
                .Build();

            var baseSection2 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.Seven)
                .WithValuePerFullSet(5)
                .Build();

            var derivedSection1 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.BaseBar)
                .WithBaseSectionType(SpinnerSectionType.BaseBar)
                .Build();

            var derivedSection2 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.Blank)
                .WithBaseSectionType(SpinnerSectionType.BaseBar)
                .Build();

            var derivedSection3 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.Cherry)
                .WithBaseSectionType(SpinnerSectionType.BaseBar)
                .Build();

            var creditCount = 3;

            var sut = new SpinnerSectionService(new[] { baseSection1, baseSection2 });

            // Act
            var result = sut.GetSectionSetCreditGain(new[] { derivedSection1, derivedSection2, derivedSection3 }, creditCount);

            // Assert
            result.Should().Be(45);
        }

        [Test]
        public void GetSectionSetCreditGain_WhenSetIsDerivedLineUp_DoesNotUseBaseLineUp()
        {
            // Arrange
            var baseSection1 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.BaseBar)
                .WithValuePerFullSet(15)
                .Build();

            var baseSection2 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.Seven)
                .WithValuePerFullSet(5)
                .Build();

            var derivedSection1 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.DoubleBar)
                .WithValuePerFullSet(5)
                .WithBaseSectionType(SpinnerSectionType.BaseBar)
                .Build();

            var derivedSection2 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.DoubleBar)
                .WithValuePerFullSet(5)
                .WithBaseSectionType(SpinnerSectionType.BaseBar)
                .Build();

            var derivedSection3 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.DoubleBar)
                .WithValuePerFullSet(5)
                .WithBaseSectionType(SpinnerSectionType.BaseBar)
                .Build();

            var creditCount = 3;

            var sut = new SpinnerSectionService(new[] { baseSection1, baseSection2 });

            // Act
            var result = sut.GetSectionSetCreditGain(new[] { derivedSection1, derivedSection2, derivedSection3 }, creditCount);

            // Assert
            result.Should().Be(15);
        }

        [Test]
        public void GetSectionSetCreditGain_WhenItemsInSetHaveValuePerSingleItem_OutputsExpectedResults()
        {
            var derivedSection1 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.Seven)
                .WithBaseSectionType(SpinnerSectionType.Seven)
                .WithValuePerSingleItem(1)
                .Build();

            var derivedSection2 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.Cherry)
                .WithBaseSectionType(SpinnerSectionType.Cherry)
                .WithValuePerSingleItem(2)
                .Build();

            var derivedSection3 = new SpinnerSectionBuilder()
                .WithSectionType(SpinnerSectionType.Blank)
                .WithBaseSectionType(SpinnerSectionType.Blank)
                .WithValuePerSingleItem(3)
                .Build();

            var creditCount = 3;

            var sut = new SpinnerSectionService(Array.Empty<SpinnerSection>());

            // Act
            var result = sut.GetSectionSetCreditGain(new[] { derivedSection1, derivedSection2, derivedSection3 }, creditCount);

            // Assert
            result.Should().Be(18);
        }
    }
}