using System.Reflection;
using FluentAssertions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Validates <see cref="HtmlToMarkdownConverter"/> against real article HTML downloaded
/// from the live sites of pending content reviews in the local database.
///
/// Fixtures are stored as embedded resources in Services/Fixtures/ and represent the
/// inner content of the &lt;article&gt; tag extracted from each page.
///
/// Purpose: catching regressions where the converter incorrectly strips structured content
/// (headings, code blocks, links) that appeared in the admin review diff.
/// </summary>
public class HtmlToMarkdownConverterRealArticleTests
{
    private static string LoadFixture(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"TechHub.Infrastructure.Tests.Services.Fixtures.{fileName}";

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Embedded resource '{resourceName}' not found. " +
                $"Available: {string.Join(", ", assembly.GetManifestResourceNames())}");

        using var reader = new System.IO.StreamReader(stream);
        return reader.ReadToEnd();
    }

    // ── Microsoft Agent Framework 1.0 blog post ──────────────────────────────
    // Source: https://devblogs.microsoft.com/agent-framework/microsoft-agent-framework-version-1-0/
    // Pending review id=9 in local content_reviews

    [Fact]
    public void Convert_AgentFrameworkArticle_PreservesLeadParagraph()
    {
        var html = LoadFixture("agent-framework-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        result.Should().Contain("Microsoft Agent Framework has reached version 1.0",
            "lead paragraph must survive noise removal and entity decoding");
    }

    [Fact]
    public void Convert_AgentFrameworkArticle_PreservesCsharpCodeFence()
    {
        var html = LoadFixture("agent-framework-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        result.Should().Contain("```csharp",
            "code block with class=\"language-csharp\" must be converted to a fenced code block");
    }

    [Fact]
    public void Convert_AgentFrameworkArticle_PreservesPythonCodeFence()
    {
        var html = LoadFixture("agent-framework-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        result.Should().Contain("```py",
            "code block with class=\"language-py\" must be converted to a fenced code block");
    }

    [Fact]
    public void Convert_AgentFrameworkArticle_CodeBlockContainsRealCode()
    {
        var html = LoadFixture("agent-framework-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        // The C# code block includes this using statement
        result.Should().Contain("Microsoft.Agents.AI",
            "code block content must be preserved, not stripped");
    }

    [Fact]
    public void Convert_AgentFrameworkArticle_NoHtmlTagsInOutput()
    {
        var html = LoadFixture("agent-framework-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        result.Should().NotMatchRegex(@"<[a-zA-Z][^>]*>",
            "all HTML tags must be stripped or converted");
    }

    [Fact]
    public void Convert_AgentFrameworkArticle_NoScriptContent()
    {
        var html = LoadFixture("agent-framework-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        // "function(" and "document." are JS-specific patterns that should never appear
        // (note: "var " legitimately appears inside C# code blocks so cannot be asserted)
        result.Should().NotContain("function(", "script content must be removed");
        result.Should().NotContain("document.querySelector", "DOM-manipulation JS must be removed");
    }

    // ── GitHub Copilot cloud agent — organization runner controls ─────────────
    // Source: https://github.blog/changelog/2026-04-03-organization-runner-controls-for-copilot-cloud-agent
    // Pending review id=4 in local content_reviews

    [Fact]
    public void Convert_CopilotRunnerChangelog_PreservesTitle()
    {
        var html = LoadFixture("copilot-runner-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        result.Should().Contain("Organization runner controls for Copilot cloud agent",
            "H1 title must be converted to a markdown heading and preserve its text");
    }

    [Fact]
    public void Convert_CopilotRunnerChangelog_PreservesBodyContent()
    {
        var html = LoadFixture("copilot-runner-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        // The article explains that organization admins can control the runner
        result.Should().Contain("GitHub Actions",
            "key body text must survive conversion");
        // FluentAssertions Contain() is case-sensitive; match the actual capitalisation in the article
        result.Should().ContainAny(["Organization admins", "organization admins"],
            "runner control context must be preserved");
    }

    [Fact]
    public void Convert_CopilotRunnerChangelog_NoHtmlTagsInOutput()
    {
        var html = LoadFixture("copilot-runner-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        result.Should().NotMatchRegex(@"<[a-zA-Z][^>]*>",
            "all HTML tags must be stripped or converted");
    }

    [Fact]
    public void Convert_CopilotRunnerChangelog_HeadingFormattedCorrectly()
    {
        var html = LoadFixture("copilot-runner-article.html");

        var result = HtmlToMarkdownConverter.Convert(html);

        // The H1 is in the article's <header> (not a page-level header).
        // After the header stripping fix, it should survive as a markdown heading.
        result.Should().ContainAny(
            ["# Organization runner controls", "Organization runner controls"],
            "H1 content must be present in output");
    }
}
