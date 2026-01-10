using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for content detail page functionality.
/// Tests PrimarySection URL routing, sidebar display, and navigation buttons.
///
/// PATTERN: These tests navigate directly to content URLs (not click-through) since
/// roundup detail pages are the main testable internal content pages.
/// </summary>
[Collection("Content Detail Tests")]
public class ContentDetailTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private IBrowserContext? _context;
    private IPage? _page;`r`n    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
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
    public async Task ContentDetailPage_URL_UsesPrimarySectionFromCategories()
    {
        // Arrange & Act - Navigate to a roundup detail page
        await NavigateToFirstRoundupDetailAsync();

        // Assert - URL should include /roundups/ with a section prefix
        Page.Url.Should().Contain("/roundups/",
            "content URL should include collection name with section prefix");
    }

    [Fact]
    public async Task ContentDetailPage_ShowsSidebarWithCollections()
    {
        // Arrange & Act
        await NavigateToFirstRoundupDetailAsync();

        // Assert - Sidebar should exist (use generic helper with specific selector)
        await Page.AssertElementVisibleBySelectorAsync(".sidebar");

        // Check for Collections heading in sidebar
        await Assertions.Expect(Page.Locator(".sidebar h2:has-text('Collections')"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });
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
    public async Task ContentDetailPage_ShowsBackToSectionButton()
    {
        // Arrange & Act
        await NavigateToFirstRoundupDetailAsync();

        // Assert - "Back to [Section]" link exists
        // The text is "Back to All" or similar depending on section
        await Assertions.Expect(Page.Locator("a[href]:has-text('Back to')").Last)
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });
    }

    [Fact]
    public async Task ContentDetailPage_BackToSectionButton_NavigatesToCorrectSection()
    {
        // Arrange & Act
        await NavigateToFirstRoundupDetailAsync();

        // Find and click the "Back to [Section]" button (last Back to link)
        var backButton = Page.Locator("a[href]:has-text('Back to')").Last;
        await backButton.AssertElementVisibleAsync();

        await backButton.ClickBlazorElementAsync();

        // Wait for navigation away from detail page
        await Page.WaitForFunctionAsync(
            "() => !window.location.pathname.includes('/roundups/2')",
            new PageWaitForFunctionOptions { Timeout = BlazorHelpers.DefaultNavigationTimeout }
        );

        // Assert - Should be on a section page, not a content detail page
        Page.Url.Should().NotMatch(@"/roundups/\d{4}-",
            "back button should navigate away from content detail page");
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

    [Theory]
    [InlineData("/all/roundups")]
    public async Task ContentDetailPage_BackButton_ShowsCorrectSectionName(string sectionPath)
    {
        // Arrange
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}{sectionPath}");

        // Wait for cards and get first card's href
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();
        var firstCardHref = await Page.Locator(".content-item-card").First.GetHrefAsync();
        firstCardHref.Should().NotBeNullOrEmpty("first roundup card should have href");

        // Navigate directly to detail page
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}{firstCardHref}");
        await Page.AssertElementVisibleBySelectorAsync("main article");

        // Assert - Back button with "Back to" text should exist
        var backButton = Page.Locator("a[href]:has-text('Back to')").Last;
        await backButton.AssertElementVisibleAsync(BlazorHelpers.DefaultAssertionTimeout);

        var buttonText = await backButton.TextContentWithTimeoutAsync();
        buttonText.Should().StartWith("Back to ",
            "back button should show 'Back to [Section]' format");
    }
}
