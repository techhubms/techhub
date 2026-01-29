using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for content detail page functionality.
/// Tests PrimarySection URL routing, sub-nav display, and navigation buttons.
///
/// PATTERN: These tests navigate directly to content URLs (not click-through) since
/// roundup detail pages are the main testable internal content pages.
/// </summary>
[Collection("Content Detail Tests")]
public class ContentDetailTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public ContentDetailTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private const string BaseUrl = BlazorHelpers.BaseUrl;

    // Test with a known roundup URL - more reliable than clicking through
    private const string TestRoundupUrl = "/all/roundups";

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
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
            await _context.DisposeAsync();
        }
    }

    /// <summary>
    /// Helper to navigate to a roundup detail Page.
    /// Navigates to roundups list, gets the first card's href, and navigates directly.
    /// </summary>
    private async Task NavigateToFirstRoundupDetailAsync()
    {
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}{TestRoundupUrl}");

        // Wait for cards to load and get href
        await Page.Locator(".card").First.AssertElementVisibleAsync();
        var firstCardHref = await Page.Locator(".card").First.GetHrefAsync();
        firstCardHref.Should().NotBeNullOrEmpty("first roundup card should have href");

        // Navigate directly to the detail page (more reliable than clicking)
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}{firstCardHref}");

        // Wait for detail page to be ready - verify main article is visible (should be exactly 1)
        await Page.AssertElementVisibleBySelectorAsync("main article");
    }

    [Fact]
    public async Task ContentDetailPage_URL_IncludesCollectionName()
    {
        // Arrange & Act - Navigate to a roundup detail page
        await NavigateToFirstRoundupDetailAsync();

        // Assert - URL should include collection name (roundups)
        Page.Url.Should().Contain("/roundups/",
            "content URL should include collection name");
    }

    [Fact]
    public async Task ContentDetailPage_ShowsSubNavWithCollections()
    {
        // Arrange & Act
        await NavigateToFirstRoundupDetailAsync();

        // Assert - Sub-nav should exist with collection links
        await Page.AssertElementVisibleBySelectorAsync(".sub-nav");

        // Check for collection links in sub-nav (All + regular collections)
        await Assertions.Expect(Page.Locator(".sub-nav a"))
            .Not.ToHaveCountAsync(0, new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });
    }

    [Fact]
    public async Task ContentDetailPage_TwoColumnLayout_DisplaysCorrectly()
    {
        // Arrange & Act
        await NavigateToFirstRoundupDetailAsync();

        // Assert - Sidebar visible (use generic helper with specific selector)
        await Page.AssertElementVisibleBySelectorAsync(".sidebar");

        // Assert - Main content visible (article or main.page-main-content)
        await Assertions.Expect(Page.Locator("main article, main.page-main-content article"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });
    }

    [Fact]
    public async Task ContentDetailPage_Sidebar_ShowsTags()
    {
        // Arrange & Act
        await NavigateToFirstRoundupDetailAsync();

        // Assert - Tags heading visible in sidebar (use auto-retrying Expect)
        await Assertions.Expect(Page.Locator(".sidebar h2:has-text('Tags')"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });
    }

    [Fact]
    public async Task RoundupLinks_ShouldNavigate_ToRoundupDetailPage()
    {
        // Arrange - Start from homepage where latest roundup is featured
        await Page.GotoRelativeAsync("/");

        // Act - Click roundup link (finds links with date format like "Dec 29, 2025")
        var roundupLinks = Page.Locator("a").Filter(new() { HasTextRegex = new Regex(@"[A-Z][a-z]{2}\s+\d{1,2},\s+\d{4}", RegexOptions.None) });
        var count = await roundupLinks.CountAsync();

        if (count > 0)
        {
            await roundupLinks.First.ClickBlazorElementAsync();

            // Assert - Should navigate to roundup detail page
            await Page.WaitForBlazorUrlContainsAsync("/roundups/");
            Page.Url.Should().Contain("/roundups/", "clicking roundup link should navigate to roundup detail page");
        }
    }

    [Fact]
    public async Task VideoDetailPage_URL_DoesNotIncludeDatePrefix()
    {
        // Arrange - Navigate to videos collection
        await Page.GotoRelativeAsync("/ai/videos");

        // Get first video card href
        await Page.Locator(".card").First.AssertElementVisibleAsync();
        var firstCardHref = await Page.Locator(".card").First.GetHrefAsync();
        firstCardHref.Should().NotBeNullOrEmpty("first video card should have href");

        // Act - Navigate to video detail page
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}{firstCardHref}");

        // Assert - URL should NOT contain date prefix pattern (YYYY-MM-DD-)
        Page.Url.Should().Contain("/videos/", "URL should include collection name");

        // Extract slug from URL (everything after /videos/)
        var urlPattern = new Regex(@"/videos/([^/?#]+)");
        var match = urlPattern.Match(Page.Url);
        match.Success.Should().BeTrue("URL should match /videos/{slug} pattern");

        var slug = match.Groups[1].Value;
        slug.Should().NotMatchRegex(@"^\d{4}-\d{2}-\d{2}-",
            "video URL slug should NOT start with date prefix (YYYY-MM-DD-)");
    }

    [Fact]
    public async Task ContentDetailPage_OldDatePrefixedURL_Returns404()
    {
        // Arrange - Try to access a URL with old date prefix format
        // This URL pattern should no longer work: /ai/videos/2026-01-12-slug
        var oldFormatUrl = "/ai/videos/2026-01-12-what-quantum-safe-is-and-why-we-need-it";

        // Act & Assert - Should get 404 or redirect behavior
        var response = await Page.GotoAsync($"{BaseUrl}{oldFormatUrl}");

        // Either 404 status code or redirected away from the old URL pattern
        // If we get a response, it should be 404
        response?.Status.Should().Be(404, "old date-prefixed URLs should return 404");

        // Or we should be redirected to a different page (not the old format)
        Page.Url.Should().NotContain("/2026-01-12-",
            "should not remain on old date-prefixed URL");
    }
}

