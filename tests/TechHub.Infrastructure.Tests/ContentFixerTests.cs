using System.Text.RegularExpressions;
using FluentAssertions;

namespace TechHub.Infrastructure.Tests;

/// <summary>
/// Tests for ContentFixer functionality that processes markdown files.
/// NOTE: These tests simulate the ContentFixer logic for URL fixing.
/// The actual ContentFixer is in src/TechHub.ContentFixer/Program.cs
/// </summary>
public class ContentFixerTests
{
    [Theory]
    [InlineData("[Link](/blog/2025/04/01/GitHub-Copilot-Change-the-Narrative)", "[Link](/blog/github-copilot-change-the-narrative)")]
    [InlineData("[Link](/videos/2025-12-01-Reimagine-Migration-Agentic-Platform-Landing-Zone-with-Azure-Migrate-Video.html)", "[Link](/videos/reimagine-migration-agentic-platform-landing-zone-with-azure-migrate-video)")]
    [InlineData("[Link](/github-copilot/videos/Using-the-GitHub-Copilot-SDK-with-Python)", "[Link](/github-copilot/videos/using-the-github-copilot-sdk-with-python)")]
    [InlineData("[Link](/AI/videos/Deep-Dive-into-Foundry-IQ-and-Azure-AI-Search)", "[Link](/ai/videos/deep-dive-into-foundry-iq-and-azure-ai-search)")]
    [InlineData("[External](https://example.com/Path/To/Resource)", "[External](https://example.com/Path/To/Resource)")]
    [InlineData("[Relative](../docs/file.md)", "[Relative](../docs/file.md)")]
    public void InternalUrls_ShouldBeLowercase_AndWithoutDatesInPath(string input, string expected)
    {
        // Arrange
        var content = $"---\ntitle: Test\n---\n\n{input}";

        // Act
        var result = FixInternalUrls(content);

        // Assert
        result.Should().Contain(expected);
    }

    [Theory]
    [InlineData("[Link](/blog/2025/04/01/test \"Title\")", "[Link](/blog/test \"Title\")")]
    [InlineData("[Multiple](/ai/videos/Video-One) and [Another](/dotnet/blogs/Blog-Two)", "[Multiple](/ai/videos/video-one)", "[Another](/dotnet/blogs/blog-two)")]
    public void InternalUrls_ShouldPreserveStructure_WhenLowercasing(string input, params string[] expectedParts)
    {
        // Arrange  
        var content = $"---\ntitle: Test\n---\n\n{input}";

        // Act
        var result = FixInternalUrls(content);

        // Assert
        foreach (var part in expectedParts)
        {
            result.Should().Contain(part);
        }
    }

    [Fact]
    public void InternalUrls_WithDateInPath_ShouldRemoveDateSegments()
    {
        // Arrange
        var content = @"---
title: Test
---

Check out [this article](/blog/2025/04/01/GitHub-Copilot-Change-the-Narrative) for more details.
Also see [this video](/videos/2024/12/15/Some-Video-Title).
";

        // Act
        var result = FixInternalUrls(content);

        // Assert
        result.Should().Contain("/blog/github-copilot-change-the-narrative");
        result.Should().Contain("/videos/some-video-title");
        result.Should().NotContain("/blog/2025/04/01/");
        result.Should().NotContain("/videos/2024/12/15/");
    }

    /// <summary>
    /// Fix internal URLs by removing date segments from the path and lowercasing everything.
    /// This duplicates the logic from ContentFixer for testing purposes.
    /// Examples:
    /// - /blog/2025/04/01/Title → /blog/title
    /// - /AI/videos/Video-Title → /ai/videos/video-title
    /// - /github-copilot/blogs/Title → /github-copilot/blogs/title
    /// Does NOT modify:
    /// - External URLs (containing ://)
    /// - Relative paths (starting with ..)
    /// </summary>
    private static string FixInternalUrls(string markdown)
    {
        // Pattern to match markdown links with internal URLs: [text](/path/to/resource) or [text](/path "title")
        var linkPattern = @"\[([^\]]+)\]\((/[^\s)""#]+)(?:\s+""([^""]*)"")?\)";

        return Regex.Replace(markdown, linkPattern, match =>
        {
            var text = match.Groups[1].Value;
            var url = match.Groups[2].Value;
            var title = match.Groups[3].Success ? match.Groups[3].Value : null;

            // Remove date segments from path: /collection/YYYY/MM/DD/slug → /collection/slug
            // Also handle: /collection/YYYY-MM-DD-slug → /collection/slug
            var urlWithoutDates = Regex.Replace(url, @"/\d{4}/\d{2}/\d{2}/", "/");
            urlWithoutDates = Regex.Replace(urlWithoutDates, @"/\d{4}-\d{2}-\d{2}-", "/");

            // Remove .html extension if present
            if (urlWithoutDates.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            {
                urlWithoutDates = urlWithoutDates[..^5];
            }

            // Lowercase the entire URL for consistency
            var fixedUrl = urlWithoutDates.ToLowerInvariant();

            // Rebuild markdown link
            var result = $"[{text}]({fixedUrl}";
            if (!string.IsNullOrEmpty(title))
            {
                result += $" \"{title}\"";
            }

            result += ")";
            return result;
        });
    }
}
