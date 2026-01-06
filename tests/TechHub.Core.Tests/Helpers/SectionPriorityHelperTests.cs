using FluentAssertions;
using TechHub.Core.Helpers;

namespace TechHub.Core.Tests.Helpers;

/// <summary>
/// Unit tests for SectionPriorityHelper
/// </summary>
public class SectionPriorityHelperTests
{
    [Theory]
    [InlineData(new[] { "GitHub Copilot" }, "github-copilot")]
    [InlineData(new[] { "AI" }, "ai")]
    [InlineData(new[] { "ML" }, "ml")]
    [InlineData(new[] { "Coding" }, "coding")]
    [InlineData(new[] { "Azure" }, "azure")]
    [InlineData(new[] { "DevOps" }, "devops")]
    [InlineData(new[] { "Security" }, "security")]
    public void GetPrimarySectionUrl_ReturnsSectionUrl_ForSingleCategory(string[] categories, string expectedUrl)
    {
        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(categories);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Fact]
    public void GetPrimarySectionUrl_ReturnsFirstInPriorityOrder_ForMultipleCategories()
    {
        // Arrange - AI appears first in menubar order
        var categories = new[] { "Azure", "AI", "GitHub Copilot" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(categories);

        // Assert
        result.Should().Be("github-copilot"); // GitHub Copilot is first in priority order
    }

    [Fact]
    public void GetPrimarySectionUrl_ReturnsAll_ForEmptyCategories()
    {
        // Arrange
        var categories = Array.Empty<string>();

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(categories);

        // Assert
        result.Should().Be("all");
    }

    [Fact]
    public void GetPrimarySectionUrl_ReturnsAll_ForNoMatchingCategories()
    {
        // Arrange
        var categories = new[] { "Unknown Category", "Another Unknown" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(categories);

        // Assert
        result.Should().Be("all");
    }

    [Fact]
    public void GetPrimarySectionUrl_IsCaseInsensitive()
    {
        // Arrange
        var categories = new[] { "github copilot", "AZURE" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(categories);

        // Assert
        result.Should().Be("github-copilot");
    }

    [Theory]
    [InlineData(new[] { "GitHub Copilot" }, "GitHub Copilot")]
    [InlineData(new[] { "AI" }, "AI")]
    [InlineData(new[] { "ML" }, "ML")]
    [InlineData(new[] { "Coding" }, "Coding")]
    [InlineData(new[] { "Azure" }, "Azure")]
    [InlineData(new[] { "DevOps" }, "DevOps")]
    [InlineData(new[] { "Security" }, "Security")]
    public void GetPrimarySectionName_ReturnsSectionName_ForSingleCategory(string[] categories, string expectedName)
    {
        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(categories);

        // Assert
        result.Should().Be(expectedName);
    }

    [Fact]
    public void GetPrimarySectionName_ReturnsFirstInPriorityOrder_ForMultipleCategories()
    {
        // Arrange - GitHub Copilot appears first in menubar order
        var categories = new[] { "Azure", "AI", "GitHub Copilot" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(categories);

        // Assert
        result.Should().Be("GitHub Copilot");
    }

    [Fact]
    public void GetPrimarySectionName_ReturnsAll_ForEmptyCategories()
    {
        // Arrange
        var categories = Array.Empty<string>();

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(categories);

        // Assert
        result.Should().Be("All");
    }

    [Fact]
    public void GetPrimarySectionName_ReturnsAll_ForNoMatchingCategories()
    {
        // Arrange
        var categories = new[] { "Unknown Category" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionName(categories);

        // Assert
        result.Should().Be("All");
    }

    [Fact]
    public void PriorityOrder_MatchesMenubarOrder()
    {
        // This test documents the expected menubar order
        // GitHub Copilot, AI, ML, Coding, Azure, DevOps, Security

        // Arrange - Test with all categories in reverse order
        var categories = new[] { "Security", "DevOps", "Azure", "Coding", "ML", "AI", "GitHub Copilot" };

        // Act
        var result = SectionPriorityHelper.GetPrimarySectionUrl(categories);

        // Assert - Should pick GitHub Copilot (first in menubar)
        result.Should().Be("github-copilot");
    }
}
