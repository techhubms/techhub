using System.Data;
using FluentAssertions;
using Moq;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

public class ContentFixerServiceTests
{
    // ── RepairMarkdown ─────────────────────────────────────────────────────

    [Fact]
    public void RepairMarkdown_TrailingWhitespace_RemovesIt()
    {
        // Arrange
        var service = CreateService();
        var input = "Hello   \nWorld  \n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Be("Hello\nWorld\n");
    }

    [Fact]
    public void RepairMarkdown_HeadingWithoutSpaceAfterHash_AddsSpace()
    {
        // Arrange
        var service = CreateService();
        var input = "#Hello\n##World\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Contain("# Hello");
        result.Should().Contain("## World");
    }

    [Fact]
    public void RepairMarkdown_MissingBlankLineBeforeHeading_AddsIt()
    {
        // Arrange
        var service = CreateService();
        var input = "Some text\n## Heading\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Contain("Some text\n\n## Heading");
    }

    [Fact]
    public void RepairMarkdown_TrailingColonOnHeading_RemovesIt()
    {
        // Arrange
        var service = CreateService();
        var input = "## My Heading:\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Contain("## My Heading\n");
    }

    [Fact]
    public void RepairMarkdown_NumberedListMissingSpace_AddsSpace()
    {
        // Arrange
        var service = CreateService();
        var input = "1.First item\n2.Second item\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Contain("1. First item");
        result.Should().Contain("2. Second item");
    }

    [Fact]
    public void RepairMarkdown_MultipleConsecutiveBlankLines_CollapsesToOne()
    {
        // Arrange
        var service = CreateService();
        var input = "Line 1\n\n\n\nLine 2\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Be("Line 1\n\nLine 2\n");
    }

    [Fact]
    public void RepairMarkdown_CodeFence_PreservesContentInside()
    {
        // Arrange
        var service = CreateService();
        var input = "Text before\n\n```csharp\n#No space here\n  trailing   \n```\n\nText after\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert — content inside code fence is preserved (only trailing whitespace trimmed)
        result.Should().Contain("#No space here");
        result.Should().Contain("  trailing");
    }

    [Fact]
    public void RepairMarkdown_BlankLineBeforeCodeFence_EnsuresIt()
    {
        // Arrange
        var service = CreateService();
        var input = "Some text\n```\ncode\n```\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Contain("Some text\n\n```");
    }

    [Fact]
    public void RepairMarkdown_BlankLineAfterOpeningFence_RemovesIt()
    {
        // Arrange — blank line immediately after opening fence (common in MarkdownSnippets output)
        var service = CreateService();
        var input = "Intro text\n\n```bash title=\"Terminal\"\n\ndotnet new console\n\n```\n\nNext paragraph\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert — blank lines inside code block are stripped; surrounding blank lines kept
        result.Should().Contain("```bash title=\"Terminal\"\ndotnet new console\n```");
        result.Should().Contain("Next paragraph");
    }

    [Fact]
    public void RepairMarkdown_BlankLineBeforeClosingFence_RemovesIt()
    {
        // Arrange — blank line immediately before closing fence
        var service = CreateService();
        var input = "Text\n\n```bash\ndotnet run\n\n```\n\nAfter\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Contain("```bash\ndotnet run\n```");
    }

    [Fact]
    public void RepairMarkdown_NullOrEmpty_ReturnsAsIs()
    {
        // Arrange
        var service = CreateService();

        // Act & Assert
        service.RepairMarkdown("").Should().Be("");
        service.RepairMarkdown("  ").Should().Be("  ");
    }

    [Fact]
    public void RepairMarkdown_CleanContent_ReturnsUnchanged()
    {
        // Arrange
        var service = CreateService();
        var input = "# Heading\n\nSome paragraph text.\n\n## Another heading\n\nMore text.\n";

        // Act
        var result = service.RepairMarkdown(input);

        // Assert
        result.Should().Be(input);
    }

    // ── DetectStructuralIssues (Markdig AST validation) ────────────────────

    [Fact]
    public void DetectStructuralIssues_CleanMarkdown_ReturnsNoIssues()
    {
        // Arrange
        var content = """
            # Title

            Some paragraph with a [link](https://example.com) and text.

            ## Section

            - Item 1
            - Item 2

            ```csharp
            var x = 1;
            ```
            """;

        // Act
        var issues = ContentFixerService.DetectStructuralIssues(content);

        // Assert
        issues.Should().BeEmpty();
    }

    [Fact]
    public void DetectStructuralIssues_EmptyHeading_DetectsIssue()
    {
        // Arrange
        var content = "# \n\nSome text\n";

        // Act
        var issues = ContentFixerService.DetectStructuralIssues(content);

        // Assert
        issues.Should().ContainSingle()
            .Which.Should().Contain("Empty heading");
    }

    [Fact]
    public void DetectStructuralIssues_LinkWithEmptyUrl_DetectsIssue()
    {
        // Arrange
        var content = "Check out [this link]() for more info.\n";

        // Act
        var issues = ContentFixerService.DetectStructuralIssues(content);

        // Assert
        issues.Should().ContainSingle()
            .Which.Should().Contain("Link with empty URL");
    }

    [Fact]
    public void DetectStructuralIssues_ImageWithEmptyUrl_DetectsIssue()
    {
        // Arrange
        var content = "Here is an image: ![alt text]()\n";

        // Act
        var issues = ContentFixerService.DetectStructuralIssues(content);

        // Assert
        issues.Should().ContainSingle()
            .Which.Should().Contain("Image with empty URL");
    }

    [Fact]
    public void DetectStructuralIssues_ValidLinksAndImages_NoIssues()
    {
        // Arrange
        var content = """
            Check [this](https://example.com) and ![img](https://example.com/img.png).
            """;

        // Act
        var issues = ContentFixerService.DetectStructuralIssues(content);

        // Assert
        issues.Should().BeEmpty();
    }

    [Fact]
    public void DetectStructuralIssues_MultipleIssues_ReportsAll()
    {
        // Arrange
        var content = """
            # 

            A [broken]() link.

            And a ![missing]() image.
            """;

        // Act
        var issues = ContentFixerService.DetectStructuralIssues(content);

        // Assert
        issues.Should().HaveCount(3);
        issues.Should().Contain(i => i.Contains("Empty heading"));
        issues.Should().Contain(i => i.Contains("Link with empty URL"));
        issues.Should().Contain(i => i.Contains("Image with empty URL"));
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static ContentFixerService CreateService()
    {
        return new ContentFixerService(
            Mock.Of<IDbConnection>(),
            null,
            Microsoft.Extensions.Logging.Abstractions.NullLogger<ContentFixerService>.Instance);
    }
}
