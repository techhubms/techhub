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

    #endregion
}
