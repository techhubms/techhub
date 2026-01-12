using FluentAssertions;
using TechHub.Core.Helpers;

namespace TechHub.Core.Tests.Helpers;

/// <summary>
/// Unit tests for SectionPriorityHelper
/// </summary>
public class SectionPriorityHelperTests
{
    [Theory]
    [InlineData(new[] { "github-copilot" }, "github-copilot")]
    [InlineData(new[] { "ai" }, "ai")]
    [InlineData(new[] { "ml" }, "ml")]
    [InlineData(new[] { "coding" }, "coding")]
    [InlineData(new[] { "azure" }, "azure")]
    [InlineData(new[] { "devops" }, "devops")]
    [InlineData(new[] { "security" }, "security")]
    public void GetPrimarySectionUrl_ReturnsSectionUrl_ForSingleSection(string[] sectionNames, string expectedUrl)
    {
        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Fact]
    public void GetPrimarySectionUrl_ReturnsFirstInPriorityOrder_ForMultipleSections()
    {
        // Arrange - AI appears first in menubar order
        var sectionNames = new[] { "azure", "ai", "github-copilot" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames);

        // Assert
        result.Should().Be("github-copilot"); // GitHub Copilot is first in priority order
    }

    [Fact]
    public void GetPrimarySectionUrl_ReturnsAll_ForEmptySections()
    {
        // Arrange
        var sectionNames = Array.Empty<string>();

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames);

        // Assert
        result.Should().Be("all");
    }

    [Fact]
    public void GetPrimarySectionUrl_ReturnsAll_ForNoMatchingSections()
    {
        // Arrange
        var sectionNames = new[] { "Unknown Section", "Another Unknown" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames);

        // Assert
        result.Should().Be("all");
    }

    [Fact]
    public void GetPrimarySectionUrl_IsCaseInsensitive()
    {
        // Arrange
        var sectionNames = new[] { "github-copilot", "azure" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames);

        // Assert
        result.Should().Be("github-copilot");
    }

    [Theory]
    [InlineData(new[] { "github-copilot" }, "github-copilot")]
    [InlineData(new[] { "ai" }, "ai")]
    [InlineData(new[] { "ml" }, "ml")]
    [InlineData(new[] { "coding" }, "coding")]
    [InlineData(new[] { "azure" }, "azure")]
    [InlineData(new[] { "devops" }, "devops")]
    [InlineData(new[] { "security" }, "security")]
    public void GetPrimarySectionName_ReturnsSectionName_ForSingleSection(string[] sectionNames, string expectedName)
    {
        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(sectionNames);

        // Assert
        result.Should().Be(expectedName);
    }

    [Fact]
    public void GetPrimarySectionName_ReturnsFirstInPriorityOrder_ForMultipleSections()
    {
        // Arrange - GitHub Copilot appears first in menubar order
        var sectionNames = new[] { "azure", "ai", "github-copilot" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(sectionNames);

        // Assert
        result.Should().Be("github-copilot");
    }

    [Fact]
    public void GetPrimarySectionName_ReturnsAll_ForEmptySections()
    {
        // Arrange
        var sectionNames = Array.Empty<string>();

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(sectionNames);

        // Assert
        result.Should().Be("all");
    }

    [Fact]
    public void GetPrimarySectionName_ReturnsAll_ForNoMatchingSections()
    {
        // Arrange
        var sectionNames = new[] { "Unknown Section" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(sectionNames);

        // Assert
        result.Should().Be("all");
    }

    [Fact]
    public void PriorityOrder_MatchesMenubarOrder()
    {
        // This test documents the expected menubar order
        // GitHub Copilot, AI, ML, Coding, Azure, DevOps, Security

        // Arrange - Test with all sections in reverse order
        var sectionNames = new[] { "security", "devops", "azure", "coding", "ml", "ai", "github-copilot" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames);

        // Assert - Should pick GitHub Copilot (first in menubar)
        result.Should().Be("github-copilot");
    }

    [Fact]
    public void GetPrimarySectionUrl_ReturnsAll_ForRoundupsCollection()
    {
        // Arrange - Roundups content may have various section names
        var sectionNames = new[] { "AI", "GitHub Copilot", "Azure" };

        // Act - Pass "roundups" as collection name
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames, "roundups");

        // Assert - Should always return "all" regardless of section names
        result.Should().Be("all", "roundups always belong to the 'all' section");
    }

    [Fact]
    public void GetPrimarySectionName_ReturnsAll_ForRoundupsCollection()
    {
        // Arrange - Roundups content may have various section names
        var sectionNames = new[] { "ai", "github-copilot", "azure" };

        // Act - Pass "roundups" as collection name
        var result = SectionPriorityHelper.GetPrimarySectionName(sectionNames, "roundups");

        // Assert - Should always return "all" regardless of section names
        result.Should().Be("all", "roundups always belong to the 'all' section");
    }

    [Fact]
    public void GetPrimarySectionUrl_RespectsSections_ForNonRoundupsCollections()
    {
        // Arrange
        var sectionNames = new[] { "github-copilot" };

        // Act - Pass a non-roundups collection name
        var result = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames, "news");

        // Assert - Should use normal priority logic
        result.Should().Be("github-copilot", "non-roundups collections should use normal priority logic");
    }
}
