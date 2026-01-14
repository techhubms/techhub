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
public class ContentDetailTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private const string BaseUrl = BlazorHelpers.BaseUrl;

    // Test with a known roundup URL - more reliable than clicking through
    private const string TestRoundupUrl = "/all/roundups";

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
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();
        var firstCardHref = await Page.Locator(".content-item-card").First.GetHrefAsync();
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
    public async Task ContentDetailPage_ShowsBackToTopButton()
    {
        // Arrange & Act
        await NavigateToFirstRoundupDetailAsync();

        // Assert - "Back to Top" link exists (use auto-retrying Expect)
        await Assertions.Expect(Page.Locator("a:has-text('Back to Top')"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });
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

}

