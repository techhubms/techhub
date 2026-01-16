using FluentAssertions;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for TagMatchingService
/// Tests word boundary matching, normalization, and subset logic
/// </summary>
public class TagMatchingServiceTests
{
    private readonly TagMatchingService _service = new();

    #region Normalize Tests

    [Theory]
    [InlineData("AI", "ai")]
    [InlineData("GitHub Copilot", "github copilot")]
    [InlineData("  Azure  ", "azure")]
    [InlineData("Machine-Learning", "machine-learning")]
    [InlineData("VS Code", "vs code")]
    public void Normalize_VariousInputs_ReturnsLowercaseTrimmed(string input, string expected)
    {
        // Act
        var result = _service.Normalize(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Matches - Word Boundary Tests

    [Theory]
    [InlineData("AI", "AI", true, "exact match")]
    [InlineData("AI", "ai", true, "case insensitive")]
    [InlineData("AI", "Generative AI", true, "subset with word boundary")]
    [InlineData("AI", "Azure AI", true, "subset at end")]
    [InlineData("AI", "AI Model", true, "subset at start")]
    [InlineData("AI", "AIR", false, "no word boundary - should NOT match")]
    [InlineData("AI", "AIML", false, "prefix without boundary")]
    [InlineData("AI", "CHAIR", false, "substring without boundary")]
    [InlineData("Visual Studio", "Visual Studio", true, "multi-word exact match")]
    [InlineData("Visual Studio", "Visual Studio Code", true, "multi-word subset")]
    [InlineData("Studio", "Visual Studio", true, "single word in multi-word")]
    [InlineData("Code", "VS Code", true, "word at end")]
    [InlineData("VS", "VS Code", true, "word at start")]
    [InlineData("Copilot", "GitHub Copilot", true, "subset match")]
    [InlineData("GitHub", "GitHub Copilot", true, "prefix match with boundary")]
    [InlineData("Git", "GitHub", false, "partial word without boundary")]
    public void Matches_VariousScenarios_ReturnsExpectedResult(string searchTag, string itemTag, bool expected, string because)
    {
        // Act
        var result = _service.Matches(searchTag, itemTag);

        // Assert
        result.Should().Be(expected, because);
    }

    [Fact]
    public void Matches_EmptySearchTag_ReturnsFalse()
    {
        // Act
        var result = _service.Matches("", "AI");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Matches_EmptyItemTag_ReturnsFalse()
    {
        // Act
        var result = _service.Matches("AI", "");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Matches_BothEmpty_ReturnsFalse()
    {
        // Act
        var result = _service.Matches("", "");

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("AI-ML", "AI-ML", true)]
    [InlineData("Visual Studio", "Visual Studio", true)]
    [InlineData("Visual Studio", "Visual Studio Code", true)]
    public void Matches_SpecialCharacters_HandlesCorrectly(string searchTag, string itemTag, bool expected)
    {
        // Act
        var result = _service.Matches(searchTag, itemTag);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region MatchesAny Tests

    [Fact]
    public void MatchesAny_OneTagMatches_ReturnsTrue()
    {
        // Arrange
        var selectedTags = new[] { "AI", "Azure" };
        var itemTags = new[] { "Machine Learning", "AI Model", "Python" };

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeTrue("AI matches 'AI Model'");
    }

    [Fact]
    public void MatchesAny_MultipleTagsMatch_ReturnsTrue()
    {
        // Arrange
        var selectedTags = new[] { "AI", "Azure" };
        var itemTags = new[] { "Azure AI", "Cloud" };

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeTrue("both AI and Azure match 'Azure AI'");
    }

    [Fact]
    public void MatchesAny_NoTagsMatch_ReturnsFalse()
    {
        // Arrange
        var selectedTags = new[] { "AI", "Azure" };
        var itemTags = new[] { "DevOps", "Security", "Productivity" };

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void MatchesAny_EmptySelectedTags_ReturnsTrue()
    {
        // Arrange
        var selectedTags = Array.Empty<string>();
        var itemTags = new[] { "AI", "Azure" };

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeTrue("no selected tags means no filter active, show all items");
    }

    [Fact]
    public void MatchesAny_EmptyItemTags_ReturnsFalse()
    {
        // Arrange
        var selectedTags = new[] { "AI", "Azure" };
        var itemTags = Array.Empty<string>();

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void MatchesAny_BothEmpty_ReturnsTrue()
    {
        // Arrange
        var selectedTags = Array.Empty<string>();
        var itemTags = Array.Empty<string>();

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeTrue("no selected tags means no filter active, show all items including those with no tags");
    }

    [Fact]
    public void MatchesAny_CaseInsensitive_Matches()
    {
        // Arrange
        var selectedTags = new[] { "ai", "AZURE" };
        var itemTags = new[] { "AI", "azure" };

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesAny_PartialWordBoundary_DoesNotMatch()
    {
        // Arrange
        var selectedTags = new[] { "AI" };
        var itemTags = new[] { "AIR", "AIML" }; // AI without word boundary

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeFalse("AI should not match AIR or AIML without word boundary");
    }

    [Fact]
    public void MatchesAny_RealWorldScenario_FiltersCorrectly()
    {
        // Arrange - User selects "AI" and "Copilot"
        var selectedTags = new[] { "AI", "Copilot" };

        // Content items with various tags
        var item1Tags = new[] { "Generative AI", "Azure", "Machine Learning" }; // Should match (AI)
        var item2Tags = new[] { "GitHub Copilot", "Productivity", "VS Code" }; // Should match (Copilot)
        var item3Tags = new[] { "AIR", "AIML", "Automation" }; // Should NOT match (no word boundary)
        var item4Tags = new[] { "DevOps", "Security" }; // Should NOT match

        // Act & Assert
        _service.MatchesAny(selectedTags, item1Tags).Should().BeTrue();
        _service.MatchesAny(selectedTags, item2Tags).Should().BeTrue();
        _service.MatchesAny(selectedTags, item3Tags).Should().BeFalse();
        _service.MatchesAny(selectedTags, item4Tags).Should().BeFalse();
    }

    #endregion

    #region Performance Tests

    [Fact]
    public void Matches_LargeTags_PerformsWell()
    {
        // Arrange
        var searchTag = "AI";
        var itemTag = new string('A', 1000) + " AI " + new string('Z', 1000);

        // Act
        var result = _service.Matches(searchTag, itemTag);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesAny_ManyTags_PerformsWell()
    {
        // Arrange
        var selectedTags = Enumerable.Range(1, 100).Select(i => $"Tag{i}").ToArray();
        var itemTags = Enumerable.Range(50, 100).Select(i => $"Tag{i}").ToArray();

        // Act
        var result = _service.MatchesAny(selectedTags, itemTags);

        // Assert
        result.Should().BeTrue("tags overlap in range 50-100");
    }

    #endregion
}
