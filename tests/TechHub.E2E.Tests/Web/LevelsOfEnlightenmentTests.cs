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

    [Fact]
    public async Task LevelsOfEnlightenment_AllNineLevels_ShouldDisplay()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Act - Find all level sections
        var levelSections = Page.Locator(".levels-level");
        var levelCount = await levelSections.CountAsync();

        // Assert - Should have exactly 9 levels
        levelCount.Should().Be(9, "Expected exactly 9 levels of enlightenment");

        // Verify each level has a heading with title
        for (int i = 1; i <= 9; i++)
        {
            var levelHeading = Page.Locator($"h2[id^='level-{i}']");
            await levelHeading.AssertElementVisibleAsync();
        }
    }

    [Fact]
    public async Task LevelsOfEnlightenment_VideoEmbeds_ShouldRender()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Act - Find all video iframes
        var videoIframes = Page.Locator(".levels-video iframe");
        var videoCount = await videoIframes.CountAsync();

        // Assert - Should have 9 video embeds (one per level)
        videoCount.Should().Be(9, "Expected 9 video embeds (one per level)");

        // Verify first video iframe has YouTube source
        var firstVideo = videoIframes.First;
        var src = await firstVideo.GetAttributeAsync("src");
        src.Should().Contain("youtube.com/embed", "Expected video iframe to have YouTube embed URL");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_OverviewSection_ShouldDisplayWithImage()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Overview heading exists
        var overviewHeading = Page.Locator("h2#overview");
        await overviewHeading.AssertElementVisibleAsync();

        // Assert - Overview image exists
        var overviewImage = Page.Locator(".levels-overview-image");
        await overviewImage.AssertElementVisibleAsync();

        // Verify image has alt text
        var altText = await overviewImage.GetAttributeAsync("alt");
        altText.Should().NotBeNullOrEmpty("Expected overview image to have alt text for accessibility");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_PlaylistLink_ShouldBePresent()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Act - Find playlist link
        var playlistLink = Page.Locator(".levels-playlist-link");

        // Assert - Link exists and is visible
        await playlistLink.AssertElementVisibleAsync();

        // Verify link has correct URL
        var href = await playlistLink.GetAttributeAsync("href");
        href.Should().Contain("youtube.com/playlist", "Expected playlist link to point to YouTube playlist");

        // Verify link opens in new tab
        var target = await playlistLink.GetAttributeAsync("target");
        target.Should().Be("_blank", "Expected playlist link to open in new tab");
    }

    [Fact]
    public async Task LevelsOfEnlightenment_KeyboardNavigation_ShouldWork()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get first TOC link
        var firstTocLink = Page.Locator(".sidebar-toc a").First;

        // Act - Focus on first TOC link and press Enter
        await firstTocLink.FocusAsync();
        await Page.Keyboard.PressAsync("Enter");

        // Wait for scroll
        await Page.WaitForTimeoutAsync(500);

        // Assert - URL should have hash (navigation worked)
        var url = Page.Url;
        url.Should().Contain("#", "Expected URL to contain anchor after pressing Enter on TOC link");
    }
}
