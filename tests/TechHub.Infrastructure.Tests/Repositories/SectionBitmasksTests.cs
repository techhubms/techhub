using FluentAssertions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

public class SectionBitmasksTests
{
    [Theory]
    [InlineData("ai", 1)]
    [InlineData("azure", 2)]
    [InlineData("dotnet", 4)]
    [InlineData("devops", 8)]
    [InlineData("github-copilot", 16)]
    [InlineData("ml", 32)]
    [InlineData("security", 64)]
    [InlineData("  GitHub-Copilot ", 16)]
    [InlineData("all", 0)]
    [InlineData("unknown", 0)]
    [InlineData("", 0)]
    public void For_Section_ReturnsItsBit(string section, int expected)
    {
        SectionBitmasks.For(section).Should().Be(expected);
    }

    [Fact]
    public void For_Sections_CombinesTheBitsAndIgnoresUnknownOnes()
    {
        SectionBitmasks.For(["ai", "Security", "nonsense", "ai"]).Should().Be(1 | 64);
        SectionBitmasks.For([]).Should().Be(0);
    }

    [Fact]
    public void All_HasEveryBitOnceAndMatchesAllSections()
    {
        SectionBitmasks.All.Select(s => s.Bit).Should().OnlyHaveUniqueItems();
        SectionBitmasks.All.Select(s => s.Bit).Aggregate(0, (a, b) => a | b).Should().Be(SectionBitmasks.AllSections);
        SectionBitmasks.All.Should().OnlyContain(s => SectionBitmasks.For(s.Name) == s.Bit);
    }

    [Fact]
    public void Has_ReportsWhetherTheBitIsSet()
    {
        SectionBitmasks.Has(1 | 16, SectionBitmasks.GithubCopilot).Should().BeTrue();
        SectionBitmasks.Has(1 | 16, SectionBitmasks.Ml).Should().BeFalse();
    }
}
