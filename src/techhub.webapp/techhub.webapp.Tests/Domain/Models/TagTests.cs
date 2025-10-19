using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.Tests.Domain.Models;

public class TagTests
{
    [Theory]
    [InlineData("Visual Studio Code", "visual studio code")]
    [InlineData("GitHub Copilot", "github copilot")]
    [InlineData("AI", "ai")]
    [InlineData("Azure DevOps", "azure devops")]
    public void NormalizeTag_WithStandardTags_ReturnsLowercase(string display, string expected)
    {
        // Act
        var normalized = Tag.NormalizeTag(display);

        // Assert
        Assert.Equal(expected, normalized);
    }

    [Theory]
    [InlineData("C#", "csharp")]
    [InlineData("F#", "fsharp")]
    public void NormalizeTag_WithSharpSymbol_ReplacesWithLanguageName(string display, string expected)
    {
        // Act
        var normalized = Tag.NormalizeTag(display);

        // Assert
        Assert.Equal(expected, normalized);
    }

    [Theory]
    [InlineData("AI+", "aiplus")]
    [InlineData("AI++", "aiplusplus")]
    [InlineData("C++", "cplusplus")]
    public void NormalizeTag_WithPlusSymbols_ReplacesWithWord(string display, string expected)
    {
        // Act
        var normalized = Tag.NormalizeTag(display);

        // Assert
        Assert.Equal(expected, normalized);
    }

    [Theory]
    [InlineData("Tag-With-Hyphens", "tag-with-hyphens")]
    [InlineData("kebab-case-tag", "kebab-case-tag")]
    public void NormalizeTag_WithHyphens_PreservesHyphens(string display, string expected)
    {
        // Act
        var normalized = Tag.NormalizeTag(display);

        // Assert
        Assert.Equal(expected, normalized);
    }

    [Theory]
    [InlineData("Tag@With!Special$Chars", "tag with special chars")]
    [InlineData("Tag.With.Dots", "tag with dots")]
    public void NormalizeTag_WithSpecialCharacters_ReplacesWithSpaces(string display, string expected)
    {
        // Act
        var normalized = Tag.NormalizeTag(display);

        // Assert
        Assert.Equal(expected, normalized);
    }

    [Fact]
    public void NormalizeTag_WithMultipleSpaces_CollapsesToSingleSpace()
    {
        // Arrange
        var display = "Tag   With    Multiple     Spaces";

        // Act
        var normalized = Tag.NormalizeTag(display);

        // Assert
        Assert.Equal("tag with multiple spaces", normalized);
    }

    [Theory]
    [InlineData("AI", "Azure AI", true)]
    [InlineData("AI", "Generative AI", true)]
    [InlineData("AI", "AI Agents", true)]
    [InlineData("Visual Studio", "Visual Studio Code", true)]
    [InlineData("Visual Studio", "Visual Studio 2022", true)]
    [InlineData("Azure", "Azure DevOps", true)]
    [InlineData("Azure", "Azure AI", true)]
    public void IsSubsetOf_WithValidSubsets_ReturnsTrue(string subset, string superset, bool expected)
    {
        // Arrange
        var tag = new Tag { Display = subset };

        // Act
        var result = tag.IsSubsetOf(superset);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("AI", "Daily", false)]
    [InlineData("AI", "Brain", false)]
    [InlineData("Test", "Testing", false)]
    public void IsSubsetOf_WithPartialMatches_ReturnsFalse(string subset, string superset, bool expected)
    {
        // Arrange
        var tag = new Tag { Display = subset };

        // Act
        var result = tag.IsSubsetOf(superset);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsSubsetOf_WithSameTag_ReturnsTrue()
    {
        // Arrange
        var tag = new Tag { Display = "Visual Studio Code" };

        // Act
        var result = tag.IsSubsetOf("Visual Studio Code");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Normalized_ComputedProperty_ReturnsNormalizedValue()
    {
        // Arrange
        var tag = new Tag { Display = "GitHub Copilot" };

        // Act
        var normalized = tag.Normalized;

        // Assert
        Assert.Equal("github copilot", normalized);
    }

    [Fact]
    public void Validate_WithValidData_Succeeds()
    {
        // Arrange
        var tag = new Tag
        {
            Display = "Valid Tag",
            Count = 10,
            FirstSeen = DateTimeOffset.UtcNow
        };

        // Act & Assert
        tag.Validate(); // Should not throw
    }

    [Fact]
    public void Validate_WithEmptyDisplay_ThrowsException()
    {
        // Arrange
        var tag = new Tag
        {
            Display = "",
            Count = 10,
            FirstSeen = DateTimeOffset.UtcNow
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => tag.Validate());
        Assert.Contains("Display cannot be empty", ex.Message);
    }

    [Fact]
    public void Validate_WithNegativeCount_ThrowsException()
    {
        // Arrange
        var tag = new Tag
        {
            Display = "Valid Tag",
            Count = -1,
            FirstSeen = DateTimeOffset.UtcNow
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => tag.Validate());
        Assert.Contains("Count cannot be negative", ex.Message);
    }
}
