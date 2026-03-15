using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GenAI Advanced custom page.
/// Verifies page-specific content and functionality.
/// 
/// Common component tests (TOC, mermaid, highlighting) are in their respective files:
/// - SidebarTocTests.cs: Table of contents behavior
/// - MermaidTests.cs: Diagram rendering
/// - HighlightingTests.cs: Code syntax highlighting
/// </summary>
public class GenAIAdvancedTests : PlaywrightTestBase
{
    public GenAIAdvancedTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/ai/genai-advanced";

    [Fact]
    public async Task GenAIAdvanced_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("GenAI Advanced"));
    }

    [Fact]
    public async Task GenAIAdvanced_ShouldDisplay_MainHeading()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Page title
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "GenAI Advanced", level: 1);
    }

    [Fact]
    public async Task GenAIAdvanced_ShouldNot_HaveConsoleErrors()
    {
        // Arrange - Collect console messages
        var consoleMessages = new List<IConsoleMessage>();
        Page.Console += (_, msg) => consoleMessages.Add(msg);

        // Act 
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - No console errors (filter infrastructure noise from Blazor/container environment)
        var errors = consoleMessages
            .Where(m => m.Type == "error")
            .Where(m => !m.Text.Contains("WebSocket"))
            .Where(m => !m.Text.Contains("ERR_CONNECTION_REFUSED"))
            .Where(m => !m.Text.Contains("ERR_NAME_NOT_RESOLVED"))
            .ToList();

        errors.Should().BeEmpty($"Expected no console errors, but found: {string.Join(", ", errors.Select(e => e.Text))}");
    }
}
