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
    private const string BaseUrl = "https://localhost:5003";
    private const string PageUrl = "/ai-powered-sdlc";
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
    public async Task AISDLC_ShouldRender_WithoutToc_WhenNoHeadings()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - No sidebar TOC should exist for pages without headings
        var toc = Page.Locator(".sidebar-toc");
        var tocExists = await toc.CountAsync();
        tocExists.Should().Be(0, "Expected no TOC for custom page without headings");

        // Content should still render
        var contentArea = Page.Locator(".custom-page-content, .page-content, main");
        await contentArea.AssertElementVisibleAsync();
    }
}
