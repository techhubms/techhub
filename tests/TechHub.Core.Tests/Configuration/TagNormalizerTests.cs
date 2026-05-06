using FluentAssertions;
using TechHub.Core.Configuration;

namespace TechHub.Core.Tests.Configuration;

/// <summary>
/// Unit tests for TagNormalizer.
/// Validates casing normalization, noise filtering, deduplication, and edge cases.
/// </summary>
public class TagNormalizerTests
{
    // ── NormalizeTagCasing ──────────────────────────────────────────

    [Theory]
    [InlineData("mcp", "MCP")]
    [InlineData("Mcp", "MCP")]
    [InlineData("MCP", "MCP")]
    [InlineData("ai", "AI")]
    [InlineData("Ai", "AI")]
    [InlineData("csharp", "C#")]
    [InlineData("vscode", "VS Code")]
    [InlineData("github", "GitHub")]
    [InlineData("githubcopilot", "GitHub Copilot")]
    [InlineData("openai", "OpenAI")]
    [InlineData("dotnet", ".NET")]
    [InlineData("efcore", "EF Core")]
    [InlineData("grpc", "gRPC")]
    [InlineData("xunit", "xUnit")]
    [InlineData("npm", "npm")]
    [InlineData("k8s", "Kubernetes")]
    [InlineData("iac", "IaC")]
    [InlineData("modelcontextprotocol", "MCP")]
    public void NormalizeTagCasing_KnownMappings_ReturnsCorrectCasing(string input, string expected)
    {
        // Act
        var result = TagNormalizer.NormalizeTagCasing(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("azure openai", "Azure OpenAI")]
    [InlineData("Azure OpenAI", "Azure OpenAI")]
    [InlineData("github actions", "GitHub Actions")]
    [InlineData("semantic kernel", "Semantic Kernel")]
    [InlineData("copilot chat", "Copilot Chat")]
    public void NormalizeTagCasing_MultiWordWithMappedWords_AppliesPerWordMapping(string input, string expected)
    {
        // Act
        var result = TagNormalizer.NormalizeTagCasing(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Kubernetes", "Kubernetes")]
    [InlineData("Docker", "Docker")]
    [InlineData("Blazor", "Blazor")]
    public void NormalizeTagCasing_AlreadyCorrectCase_PreservesIt(string input, string expected)
    {
        // Act
        var result = TagNormalizer.NormalizeTagCasing(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("container apps", "Container Apps")]
    [InlineData("dependency injection", "Dependency Injection")]
    public void NormalizeTagCasing_UnmappedLowercaseWords_TitleCases(string input, string expected)
    {
        // Act
        var result = TagNormalizer.NormalizeTagCasing(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NormalizeTagCasing_EmptyOrNull_ReturnsSameValue(string? input)
    {
        // Act
        var result = TagNormalizer.NormalizeTagCasing(input!);

        // Assert
        result.Should().Be(input);
    }

    // ── NormalizeTags (full pipeline) ──────────────────────────────

    [Fact]
    public void NormalizeTags_FixesCasingAndDeduplicates()
    {
        // Arrange
        var tags = new[] { "mcp", "MCP", "Mcp", "ai", "AI" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().ContainSingle(t => t == "MCP");
        result.Should().ContainSingle(t => t == "AI");
        result.Should().HaveCount(2);
    }

    [Fact]
    public void NormalizeTags_RemovesDeprecatedTags()
    {
        // Arrange — "Cloud" and "Coding" are deprecated (dropped),
        // "Machine Learning" is remapped to "ML" via _tagMappings
        var tags = new[] { "Machine Learning", "Kubernetes", "Cloud", "Coding" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().Contain("Kubernetes");
        result.Should().Contain("ML", "Machine Learning should be remapped to ML");
        result.Should().NotContain("Machine Learning");
        result.Should().NotContain("Cloud");
        result.Should().NotContain("Coding");
    }

    [Fact]
    public void NormalizeTags_RemovesNoiseWords()
    {
        // Arrange
        var tags = new[] { "Kubernetes", "Post", "Update", "Announcement", "Docker" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().BeEquivalentTo(["Kubernetes", "Docker"]);
    }

    [Fact]
    public void NormalizeTags_RemovesNumericAndYearTags()
    {
        // Arrange
        var tags = new[] { "Azure", "2025", "12345", "42" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().ContainSingle(t => t == "Azure");
    }

    [Fact]
    public void NormalizeTags_ReplacesHyphensWithSpaces()
    {
        // Arrange
        var tags = new[] { "azure-openai", "infrastructure-as-code" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().Contain("Azure OpenAI");
        result.Should().Contain("IaC");
    }

    [Fact]
    public void NormalizeTags_StripsParenthesizedSuffixes()
    {
        // Arrange
        var tags = new[] { "Model Context Protocol (MCP)", "Large Language Model (LLM)", "Kubernetes (K8s)", "AKS (Azure Kubernetes Services)" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert — after stripping parentheses, the remaining text maps via _tagMappings
        result.Should().Contain("MCP");
        result.Should().Contain("LLM"); // "Large Language Model" maps via _tagMappings
        result.Should().Contain("Kubernetes");
        result.Should().Contain("AKS"); // abbreviation before parentheses also works
        result.Should().NotContain("Model Context Protocol (MCP)");
        result.Should().NotContain("Large Language Model (LLM)");
    }

    [Fact]
    public void NormalizeTags_StripsMultipleParenthesizedGroups()
    {
        // Arrange
        var tags = new[] { "Grafana (foo) (bar)" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert — both parenthesized groups removed, left with "Grafana"
        result.Should().ContainSingle(t => t == "Grafana");
    }

    [Fact]
    public void NormalizeTags_SplitsCommaAndSemicolonTags()
    {
        // Arrange
        var tags = new[] { "Kubernetes;Docker", "AI,ML" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().Contain("Kubernetes");
        result.Should().Contain("Docker");
        result.Should().Contain("AI");
        result.Should().Contain("ML");
    }

    [Fact]
    public void NormalizeTags_RemovesSingleCharAndEmptyTags()
    {
        // Arrange
        var tags = new[] { "", "  ", "x", "Azure" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().ContainSingle(t => t == "Azure");
    }

    [Fact]
    public void NormalizeTags_RemovesHexColorCodes()
    {
        // Arrange — "abc" (3-char hex) and "ff00ff" (6-char hex) should be removed,
        // but "AI" (2-char, in mapping) and "CSS" (3-char, in mapping) should stay
        var tags = new[] { "abc", "ff00ff", "AI", "CSS" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().Contain("AI");
        result.Should().Contain("CSS");
        result.Should().NotContain("abc");
        result.Should().NotContain("ff00ff");
    }

    [Fact]
    public void NormalizeTags_PreservesOrderAndKeepsFirstOccurrence()
    {
        // Arrange — "azure" comes first, "Azure" second → normalized both become "Azure", keep first
        var tags = new[] { "Kubernetes", "azure", "Azure", "Docker" };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().BeEquivalentTo(["Kubernetes", "Azure", "Docker"]);
        result[0].Should().Be("Kubernetes");
        result[1].Should().Be("Azure");
        result[2].Should().Be("Docker");
    }

    [Fact]
    public void NormalizeTags_ThrowsOnNullInput()
    {
        // Act
        var act = () => TagNormalizer.NormalizeTags(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void NormalizeTags_RealWorldAiOutputScenario()
    {
        // Arrange — simulates inconsistent AI output across multiple calls
        var tags = new[]
        {
            "Mcp", "agents", "GitHub Copilot", "vscode",
            "automation", "improvement", "company",
            "Semantic Kernel", "csharp", "2025"
        };

        // Act
        var result = TagNormalizer.NormalizeTags(tags);

        // Assert
        result.Should().Contain("MCP");
        result.Should().Contain("Agents");  // title-cased (not in filter words when standalone)
        result.Should().Contain("GitHub Copilot");
        result.Should().Contain("VS Code");
        result.Should().Contain("Semantic Kernel");
        result.Should().Contain("C#");
        result.Should().NotContain("automation");
        result.Should().NotContain("improvement");
        result.Should().NotContain("company");
        result.Should().NotContain("2025");
    }
}
