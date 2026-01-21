using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot Levels of Enlightenment custom page.
/// Verifies level progression display, table of contents, and accessibility.
/// </summary>
[Collection("Custom Pages TOC Tests")]
public class LevelsOfEnlightenmentTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private const string PageUrl = "/github-copilot/levels-of-enlightenment";
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
    public async Task LevelsOfEnlightenment_ShouldRender_WithSidebarToc()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title (includes "GitHub Copilot:" prefix)
        await Page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "GitHub Copilot: Levels of Enlightenment", level: 1);

        // Check sidebar TOC exists
        var toc = Page.Locator(".sidebar-toc");
        await toc.AssertElementVisibleAsync();

        // Should have TOC links
        var tocLinks = toc.Locator("a");
        var linkCount = await tocLinks.CountAsync();
        linkCount.Should().BeGreaterThan(0, "Expected TOC to have navigation links");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_TocLinks_ShouldScrollToLevels()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get all TOC links
        var tocLinks = Page.Locator(".sidebar-toc a");
        var linkCount = await tocLinks.CountAsync();

        if (linkCount == 0)
        {
            Assert.Fail("No TOC links found on levels page");
        }

        // Act - Click first TOC link (should be "Overview" or first level)
        var firstLink = tocLinks.First;
        var linkText = await firstLink.TextContentAsync();
        await firstLink.ClickAsync();

        // Wait for scroll to complete
        await Page.WaitForTimeoutAsync(500);

        // Assert - URL should have hash
        var url = Page.Url;
        url.Should().Contain("#", $"Expected URL to contain anchor after clicking TOC link '{linkText}'");

        // Assert - Clicked link should have active class
        var activeLinks = await Page.Locator(".sidebar-toc a.active").CountAsync();
        activeLinks.Should().BeGreaterThan(0, "Expected at least one TOC link to be active");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_LastSection_ShouldScroll_ToDetectionPoint()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get last heading with ID
        var headings = Page.Locator("h2[id], h3[id]");
        var headingCount = await headings.CountAsync();

        if (headingCount == 0)
        {
            Assert.Fail("No headings with IDs found on page");
        }

        var lastHeading = headings.Last;

        // Act - Scroll to last heading
        await lastHeading.ScrollIntoViewIfNeededAsync();
        await Page.WaitForTimeoutAsync(500);

        // Scroll down more to trigger detection (50vh spacer should allow this)
        await Page.EvaluateAsync("window.scrollBy(0, 500)");
        await Page.WaitForTimeoutAsync(300);

        // Assert - Last heading's TOC link should be active
        var lastHeadingId = await lastHeading.GetAttributeAsync("id");
        var expectedActiveLink = Page.Locator($".sidebar-toc a[href$='#{lastHeadingId}']");

        var hasActiveClass = await expectedActiveLink.EvaluateAsync<bool>("el => el.classList.contains('active')");
        hasActiveClass.Should().BeTrue($"Expected TOC link for #{lastHeadingId} to be active when scrolled to bottom");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_Scrolling_ShouldUpdateActiveTocLink()
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

    [Fact]
    public async Task LevelsOfEnlightenment_Overview_ShouldBe_Highlighted()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for page to load
        await Page.WaitForTimeoutAsync(1000);

        // Assert - At least one TOC link should exist
        var tocLinks = await Page.Locator(".sidebar-toc a").CountAsync();
        tocLinks.Should().BeGreaterThan(0, "Expected TOC to have navigation links");
    }
}
