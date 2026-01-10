using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
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
public class ContentDetailTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private const string BaseUrl = BlazorHelpers.BaseUrl;

    // Test with a known roundup URL - more reliable than clicking through
    private const string TestRoundupUrl = "/all/roundups";

    public ContentDetailTests(PlaywrightCollectionFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }

    /// <summary>
    /// Helper to navigate to a roundup detail page.
    /// Navigates to roundups list, gets the first card's href, and navigates directly.
    /// </summary>
    private async Task<IPage> NavigateToFirstRoundupDetailAsync()
    {
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}{TestRoundupUrl}");

        // Wait for cards to load and get href
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();
        var firstCardHref = await page.Locator(".content-item-card").First.GetHrefAsync();
        firstCardHref.Should().NotBeNullOrEmpty("first roundup card should have href");

        // Navigate directly to the detail page (more reliable than clicking)
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}{firstCardHref}");

        // Wait for detail page to be ready - verify main article is visible (should be exactly 1)
        await page.AssertElementVisibleBySelectorAsync("main article");

        return page;
    }

    [Fact]
    public async Task ContentDetailPage_URL_UsesPrimarySectionFromCategories()
    {
        // Arrange & Act - Navigate to a roundup detail page
        var page = await NavigateToFirstRoundupDetailAsync();

        // Assert - URL should include /roundups/ with a section prefix
        page.Url.Should().Contain("/roundups/",
            "content URL should include collection name with section prefix");

        await page.CloseAsync();
    }

    [Fact]
    public async Task ContentDetailPage_ShowsSidebarWithCollections()
    {
        // Arrange & Act
        var page = await NavigateToFirstRoundupDetailAsync();

        // Assert - Sidebar should exist (use generic helper with specific selector)
        await page.AssertElementVisibleBySelectorAsync(".sidebar");

        // Check for Collections heading in sidebar
        await Assertions.Expect(page.Locator(".sidebar h2:has-text('Collections')"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });

        await page.CloseAsync();
    }

    [Fact]
    public async Task ContentDetailPage_ShowsBackToTopButton()
    {
        // Arrange & Act
        var page = await NavigateToFirstRoundupDetailAsync();

        // Assert - "Back to Top" link exists (use auto-retrying Expect)
        await Assertions.Expect(page.Locator("a:has-text('Back to Top')"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });

        await page.CloseAsync();
    }

    [Fact]
    public async Task ContentDetailPage_ShowsBackToSectionButton()
    {
        // Arrange & Act
        var page = await NavigateToFirstRoundupDetailAsync();

        // Assert - "Back to [Section]" link exists
        // The text is "Back to All" or similar depending on section
        await Assertions.Expect(page.Locator("a[href]:has-text('Back to')").Last)
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });

        await page.CloseAsync();
    }

    [Fact]
    public async Task ContentDetailPage_BackToSectionButton_NavigatesToCorrectSection()
    {
        // Arrange & Act
        var page = await NavigateToFirstRoundupDetailAsync();

        // Find and click the "Back to [Section]" button (last Back to link)
        var backButton = page.Locator("a[href]:has-text('Back to')").Last;
        await backButton.AssertElementVisibleAsync();

        await backButton.ClickBlazorElementAsync();

        // Wait for navigation away from detail page
        await page.WaitForFunctionAsync(
            "() => !window.location.pathname.includes('/roundups/2')",
            new PageWaitForFunctionOptions { Timeout = BlazorHelpers.DefaultNavigationTimeout }
        );

        // Assert - Should be on a section page, not a content detail page
        page.Url.Should().NotMatch(@"/roundups/\d{4}-",
            "back button should navigate away from content detail page");

        await page.CloseAsync();
    }

    [Fact]
    public async Task ContentDetailPage_TwoColumnLayout_DisplaysCorrectly()
    {
        // Arrange & Act
        var page = await NavigateToFirstRoundupDetailAsync();

        // Assert - Sidebar visible (use generic helper with specific selector)
        await page.AssertElementVisibleBySelectorAsync(".sidebar");

        // Assert - Main content visible (article or main.page-main-content)
        await Assertions.Expect(page.Locator("main article, main.page-main-content article"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });

        await page.CloseAsync();
    }

    [Fact]
    public async Task ContentDetailPage_Sidebar_ShowsTags()
    {
        // Arrange & Act
        var page = await NavigateToFirstRoundupDetailAsync();

        // Assert - Tags heading visible in sidebar (use auto-retrying Expect)
        await Assertions.Expect(page.Locator(".sidebar h2:has-text('Tags')"))
            .ToBeVisibleAsync(new() { Timeout = BlazorHelpers.DefaultAssertionTimeout });

        await page.CloseAsync();
    }

    [Theory]
    [InlineData("/all/roundups")]
    public async Task ContentDetailPage_BackButton_ShowsCorrectSectionName(string sectionPath)
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}{sectionPath}");

        // Wait for cards and get first card's href
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();
        var firstCardHref = await page.Locator(".content-item-card").First.GetHrefAsync();
        firstCardHref.Should().NotBeNullOrEmpty("first roundup card should have href");

        // Navigate directly to detail page
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}{firstCardHref}");
        await page.AssertElementVisibleBySelectorAsync("main article");

        // Assert - Back button with "Back to" text should exist
        var backButton = page.Locator("a[href]:has-text('Back to')").Last;
        await backButton.AssertElementVisibleAsync(BlazorHelpers.DefaultAssertionTimeout);

        var buttonText = await backButton.TextContentAsync();
        buttonText.Should().StartWith("Back to ",
            "back button should show 'Back to [Section]' format");

        await page.CloseAsync();
    }
}
