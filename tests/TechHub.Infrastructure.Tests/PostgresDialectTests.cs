using FluentAssertions;
using TechHub.Infrastructure.Data;

namespace TechHub.Infrastructure.Tests;

/// <summary>
/// Unit tests for PostgresDialect.TransformFullTextQuery.
/// Tests query transformation logic for full-text search including OR-based matching,
/// prefix support, and compound word expansion.
/// </summary>
public class PostgresDialectTests
{
    private readonly PostgresDialect _dialect = new();

    #region TransformFullTextQuery Tests

    [Fact]
    public void TransformFullTextQuery_SingleWord_ReturnsWordWithPrefix()
    {
        // Arrange & Act
        var result = _dialect.TransformFullTextQuery("copilot");

        // Assert
        result.Should().Contain("copilot:*");
    }

    [Fact]
    public void TransformFullTextQuery_MultipleWords_UsesOrLogic()
    {
        // Arrange & Act
        var result = _dialect.TransformFullTextQuery("vscode updates");

        // Assert - Should use OR (|) between terms for better recall
        result.Should().Contain("|", "Multi-word queries should use OR logic for better recall");
        result.Should().NotContain("&", "Should not use AND between terms");
    }

    [Fact]
    public void TransformFullTextQuery_CompoundWord_ExpandsToIncludeSubwords()
    {
        // Arrange & Act - "vscode" is a compound of "code" ("vs" too short for prefix matching)
        var result = _dialect.TransformFullTextQuery("vscode");

        // Assert - Should expand compound word so "vscode" also matches "code"
        result.Should().Contain("vscode:*", "Original term should be preserved");
        result.Should().Contain("code:*", "Should expand to include 'code' subword");
        result.Should().NotContain("vs:*", "Short parts (<3 chars) should be excluded to avoid overly broad matches");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void TransformFullTextQuery_EmptyOrWhitespace_ReturnsAsIs(string? query)
    {
        // Arrange & Act
        var result = _dialect.TransformFullTextQuery(query!);

        // Assert
        result.Should().Be(query);
    }

    [Fact]
    public void TransformFullTextQuery_MultipleWordsWithCompound_ExpandsCorrectly()
    {
        // Arrange & Act - "vscode updates" should expand "vscode" and keep "updates"
        var result = _dialect.TransformFullTextQuery("vscode updates");

        // Assert
        result.Should().Contain("updates:*", "Regular terms should be preserved");
        result.Should().Contain("vscode:*", "Compound term should be preserved");
        result.Should().Contain("code:*", "Compound 'vscode' should expand to include 'code'");
        result.Should().NotContain("vs:*", "Short parts (<3 chars) should be excluded");
    }

    [Fact]
    public void TransformFullTextQuery_AlreadySeparateWords_DoesNotDoubleExpand()
    {
        // Arrange & Act - "vs code" are already separate words
        var result = _dialect.TransformFullTextQuery("vs code");

        // Assert - Should have both terms with OR
        result.Should().Contain("vs:*");
        result.Should().Contain("code:*");
    }

    [Fact]
    public void TransformFullTextQuery_HyphenatedTerm_SplitsIntoSubterms()
    {
        // Arrange & Act
        // PostgreSQL to_tsquery treats "-" as NOT operator, so "auto-approval" without this fix
        // would mean "auto AND NOT approval" — returning zero results for content that has both words.
        var result = _dialect.TransformFullTextQuery("auto-approval");

        // Assert - hyphen must be treated as a word separator, not tsquery NOT operator
        result.Should().Contain("auto:*", "hyphen-separated prefix should become individual term");
        result.Should().Contain("approval:*", "hyphen-separated suffix should become individual term");
        result.Should().NotContain("auto-approval", "raw hyphenated form must not pass through to tsquery");
    }

    [Fact]
    public void TransformFullTextQuery_HyphenatedTerm_WorkflowNative_SplitsCorrectly()
    {
        // Arrange & Act
        var result = _dialect.TransformFullTextQuery("workflow-native");

        // Assert
        result.Should().Contain("workflow:*");
        result.Should().Contain("native:*");
        result.Should().NotContain("workflow-native", "raw hyphenated form must not pass through");
    }

    [Fact]
    public void TransformFullTextQuery_HyphenatedCompound_SplitsAndExpandsCompound()
    {
        // Arrange & Act - "vscode-extension": hyphen splits AND "vscode" is a known compound
        var result = _dialect.TransformFullTextQuery("vscode-extension");

        // Assert
        result.Should().Contain("vscode:*", "original compound term preserved");
        result.Should().Contain("code:*", "compound 'vscode' expands to 'code'");
        result.Should().Contain("extension:*", "hyphen-separated second part included");
        result.Should().NotContain("vscode-extension", "raw hyphenated form must not pass through");
    }

    [Fact]
    public void TransformFullTextQuery_SlashSeparated_SplitsIntoSubterms()
    {
        // Arrange & Act - slashes appear in URLs and path-like tech terms (e.g., "ASP/NET")
        var result = _dialect.TransformFullTextQuery("asp/net");

        // Assert
        result.Should().Contain("asp:*");
        result.Should().Contain("net:*");
        result.Should().NotContain("asp/net", "raw slash-separated form must not pass through");
    }

    [Fact]
    public void TransformFullTextQuery_UnderscoreSeparated_SplitsIntoSubterms()
    {
        // Arrange & Act
        var result = _dialect.TransformFullTextQuery("vscode_extension");

        // Assert
        result.Should().Contain("vscode:*");
        result.Should().Contain("extension:*");
    }

    [Fact]
    public void TransformFullTextQuery_TsqueryOperatorChars_AreStrippedNotPassedThrough()
    {
        // Arrange & Act - tsquery operators (&, |, !, :, *, (, )) must not reach to_tsquery raw
        var result = _dialect.TransformFullTextQuery("copilot!");

        // Assert - operator char stripped; only the word token remains
        result.Should().Contain("copilot:*");
        result.Should().NotContain("copilot!", "raw '!' must not appear in tsquery output");
    }

    [Fact]
    public void TransformFullTextQuery_DuplicateTermsFromSplitting_AreDeduplicatedInOutput()
    {
        // Arrange & Act - "code vs-code" produces "code" twice (once standalone, once from split)
        var result = _dialect.TransformFullTextQuery("code vs-code");

        // Assert - "code:*" appears exactly once
        var codeCount = result.Split('|')
            .Count(segment => segment.Trim() == "code:*");
        codeCount.Should().Be(1, "duplicate terms from splitting should be deduplicated");
    }

    [Fact]
    public void TransformFullTextQuery_SingleCharFragments_AreFiltered()
    {
        // Arrange & Act - splitting "a-z" produces single-char fragments that are too broad for prefix match
        var result = _dialect.TransformFullTextQuery("a-z");

        // Assert - single-char tokens filtered; method returns empty string (no valid terms extracted)
        // Callers treat empty string as "no FTS query" to avoid to_tsquery syntax errors
        result.Should().BeEmpty("single-character-only input produces no usable tokens");
        result.Should().NotContain("a:*", "single-character tokens are too broad for prefix matching");
        result.Should().NotContain("z:*", "single-character tokens are too broad for prefix matching");
    }

    [Theory]
    [InlineData("C#")]
    [InlineData("F#")]
    [InlineData("!")]
    [InlineData("#")]
    public void TransformFullTextQuery_NoTokenizableTerms_ReturnsEmpty(string query)
    {
        // Arrange & Act
        var result = _dialect.TransformFullTextQuery(query);

        // Assert - inputs that produce no usable alphanumeric tokens (≥2 chars) must return empty string
        // so callers can skip FTS entirely and avoid a to_tsquery syntax error (500 response)
        result.Should().BeEmpty($"'{query}' contains no tokenizable terms after operator/single-char stripping");
    }

    #endregion
}
