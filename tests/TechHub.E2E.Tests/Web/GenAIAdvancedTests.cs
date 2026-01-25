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
[Collection("Custom Pages TOC Tests")]
public class GenAIAdvancedTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string PageUrl = "/ai/genai-advanced";
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }

        if (_context != null)
        {
            await _context.CloseAsync();
        }
    }

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

        // Wait for page to fully load
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - No console errors
        var errors = consoleMessages
            .Where(m => m.Type == "error")
            .ToList();

        errors.Should().BeEmpty($"Expected no console errors, but found: {string.Join(", ", errors.Select(e => e.Text))}");
    }
}
