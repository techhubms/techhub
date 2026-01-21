using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot VS Code Updates custom page.
/// Reference implementation for custom pages with table of contents.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class VSCodeUpdatesTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string BaseUrl = "https://localhost:5003";
    private const string PageUrl = "/github-copilot/vscode-updates";
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
    public async Task VSCodeUpdates_ShouldRender_WithSidebarToc()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check sidebar TOC exists
        var toc = Page.Locator(".sidebar-toc");
        await toc.AssertElementVisibleAsync();

        // Should have TOC links
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "Expected TOC to have navigation links");
    }

    [Fact]
    public async Task VSCodeUpdates_TocLinks_ShouldScrollToSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount < 2)
        {
            // Skip if not enough TOC links
            return;
        }

        // Act - Click second TOC link
        var secondLink = tocLinks.Nth(1);
        var linkText = await secondLink.TextContentAsync();
        await secondLink.ClickAsync();

        // Wait for scroll to complete
        await Page.WaitForTimeoutAsync(500);

        // Assert - URL should have hash
        var url = Page.Url;
        url.Should().Contain("#", $"Expected URL to contain anchor after clicking TOC link '{linkText}'");

        // Assert - Clicked link should have active class
        var hasActiveClass = await secondLink.EvaluateAsync<bool>("el => el.classList.contains('active')");
        hasActiveClass.Should().BeTrue($"Expected clicked TOC link '{linkText}' to have active class");
    }
}
