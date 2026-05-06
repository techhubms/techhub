using FluentAssertions;
using TechHub.Core.Configuration;

namespace TechHub.Core.Tests.Configuration;

public class EnsureSectionTagsTests
{
    [Fact]
    public void EnsureSectionTags_AddsTagsForKnownSections()
    {
        // Arrange
        var tags = new[] { "Kubernetes" };
        var sections = new[] { "ai", "azure" };

        // Act
        var result = TagNormalizer.EnsureSectionTags(tags, sections);

        // Assert
        result.Should().Contain("Kubernetes");
        result.Should().Contain("AI");
        result.Should().Contain("Azure");
    }

    [Fact]
    public void EnsureSectionTags_DoesNotDuplicateExistingTags()
    {
        // Arrange
        var tags = new[] { "AI", "Kubernetes" };
        var sections = new[] { "ai" };

        // Act
        var result = TagNormalizer.EnsureSectionTags(tags, sections);

        // Assert
        result.Count(t => t.Equals("AI", StringComparison.OrdinalIgnoreCase)).Should().Be(1);
    }

    [Fact]
    public void EnsureSectionTags_GithubCopilotSection_AlsoAddsAi()
    {
        // Arrange
        var tags = new[] { "MCP" };
        var sections = new[] { "github-copilot" };

        // Act
        var result = TagNormalizer.EnsureSectionTags(tags, sections);

        // Assert
        result.Should().Contain("GitHub Copilot");
        result.Should().Contain("AI");
    }

    [Fact]
    public void EnsureSectionTags_UnknownSection_IsIgnored()
    {
        // Arrange
        var tags = new[] { "Docker" };
        var sections = new[] { "unknown-section" };

        // Act
        var result = TagNormalizer.EnsureSectionTags(tags, sections);

        // Assert
        result.Should().BeEquivalentTo(["Docker"]);
    }

    [Fact]
    public void EnsureSectionTags_AllSections_AddsMappedTags()
    {
        // Arrange
        var tags = new List<string>();
        var sections = new[] { "ai", "azure", "dotnet", "devops", "github-copilot", "ml", "security" };

        // Act
        var result = TagNormalizer.EnsureSectionTags(tags, sections);

        // Assert
        result.Should().Contain("AI");
        result.Should().Contain("Azure");
        result.Should().Contain(".NET");
        result.Should().Contain("DevOps");
        result.Should().Contain("GitHub Copilot");
        result.Should().Contain("ML");
        result.Should().Contain("Security");
    }
}
