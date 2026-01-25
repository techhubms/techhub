using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for AI-Powered SDLC custom page.
/// Reference implementation for custom pages with table of contents.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class AISDLCTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string PageUrl = "/ai/sdlc";
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
    public async Task AISDLC_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("AI SDLC"));
    }

    [Fact]
    public async Task AISDLC_ShouldRender_WithToc_ForPhaseSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Sidebar TOC should exist for this page with headings
        var toc = Page.Locator(".sidebar-toc");
        var tocExists = await toc.CountAsync();
        tocExists.Should().BeGreaterThan(0, "Expected TOC for page with SDLC phase headings");

        // Content should render - use main element specifically to avoid strict mode violation
        var mainContent = Page.Locator("main");
        await Assertions.Expect(mainContent).ToBeVisibleAsync(new() { Timeout = 5000 });
    }
}
