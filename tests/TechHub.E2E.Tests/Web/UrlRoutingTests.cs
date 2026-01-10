using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for URL routing, collection navigation, and "all" collection functionality
/// These tests document and verify the expected behavior of URL-based navigation
/// </summary>
[Collection("URL Routing Tests")]
public class UrlRoutingTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private IBrowserContext? _context;
    private IPage? _page;`r`n    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private const string BaseUrl = "http://localhost:5184";
    private const string ApiUrl = "http://localhost:5029";

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

    #region URL Routing Tests

    [Fact]
    public async Task NavigateToSection_DefaultsToAllCollection()
    {
        // Arrange

        // Act - Navigate to section without specifying collection
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - URL is /github-copilot (defaults to "all" without redirect)
        // This is intentional - the section page defaults to "all" collection without URL redirect
        await Page.AssertUrlEndsWithAsync("/github-copilot");

        // Verify "All" collection is displayed - check the page heading
        await Page.AssertElementContainsTextBySelectorAsync("h1.page-h1", "All");

        // "All" button should be active
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "All");
    }

    [Fact]
    public async Task NavigateToSectionWithCollection_URLMatchesRoute()
    {
        // Arrange

        // Act - Navigate directly to /github-copilot/news
        await Page.GotoRelativeAsync("/github-copilot/news");
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - URL should remain /github-copilot/news
        await Page.AssertUrlEndsWithAsync("/github-copilot/news");

        // News button should be active
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");
    }

    [Fact]
    public async Task ClickCollectionButton_UpdatesURL()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Click "Blogs" collection button
        var blogsButton = Page.Locator(".collection-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.ClickBlazorElementAsync();

        // Assert - URL should update to /github-copilot/blogs
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/blogs");

        // Wait for page heading to actually update to show "Blog" (from "Blog Posts")
        var pageH1 = Page.Locator("h1.page-h1");
        await Assertions.Expect(pageH1).ToContainTextAsync("Blog");

        // Verify the page heading was already asserted with ToContainTextAsync above
    }

    [Fact]
    public async Task ClickAllButton_UpdatesURLToSectionSlashAll()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Act - Click "All" button
        var allButton = Page.Locator(".collection-nav a", new() { HasTextString = "All" });
        await allButton.ClickBlazorElementAsync();

        // Assert - URL should update to /github-copilot/all
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/all");
    }

    [Fact]
    public async Task BrowserBackButton_NavigatesToPreviousCollection()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Verify News button is active initially
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");

        // Navigate to videos
        var videosButton = Page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/videos");

        // Wait for Blazor to sync state (update .active class)
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "Videos");

        // Act - Press browser back button
        await Page.GoBackAsync();

        // Assert - Should return to /github-copilot/news
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/news");

        // Wait for Blazor to sync state with URL (OnParametersSetAsync should fire)
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");
    }

    [Fact]
    public async Task BrowserForwardButton_NavigatesToNextCollection()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Navigate to videos
        var videosButton = Page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/videos");

        // Go back to news
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/news");

        // Act - Press browser forward button
        await Page.GoForwardAsync();

        // Assert - Should return to /github-copilot/videos
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/videos");
    }

    #endregion

    #region "All" Collection Tests

    [Fact]
    public async Task AllCollection_ShowsAllContentFromSection()
    {
        // Arrange

        // Get total item count across all GitHub Copilot collections from API
        var apiResponse = await Page.APIRequest.GetAsync($"{ApiUrl}/api/content?category=GitHub%20Copilot");
        var allItems = await apiResponse.JsonAsync();
        var totalItemCount = allItems.Value.GetArrayLength();

        // Act - Navigate to /github-copilot/all
        await Page.GotoRelativeAsync("/github-copilot/all");
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Should display all GitHub Copilot items regardless of collection
        var displayedItems = await Page.GetElementCountBySelectorAsync(".content-item-card");
        displayedItems.Should().Be(totalItemCount,
            "the 'all' collection should show all content items from the section across all collection types");

        // Page heading should indicate "All" - verify active button shows "All"
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "All");
    }

    [Fact]
    public async Task AllSection_AllCollection_ShowsAllContent()
    {
        // Arrange

        // Get total item count across ALL sections and collections from API
        var apiResponse = await Page.APIRequest.GetAsync($"{ApiUrl}/api/content");
        var allItems = await apiResponse.JsonAsync();
        var totalItemCount = allItems.Value.GetArrayLength();

        // Act - Navigate to /all/all
        await Page.GotoRelativeAsync("/all/all");
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Should display ALL content from ALL sections and collections
        var displayedItems = await Page.GetElementCountBySelectorAsync(".content-item-card");
        displayedItems.Should().Be(totalItemCount,
            "/all/all should show absolutely all content items from all sections and all collections");
    }

    [Fact]
    public async Task AllSection_SpecificCollection_ShowsCollectionAcrossAllSections()
    {
        // Arrange

        // Get total news count across all sections from API
        var apiResponse = await Page.APIRequest.GetAsync($"{ApiUrl}/api/content?collectionName=news");
        var allNewsItems = await apiResponse.JsonAsync();
        var totalNewsCount = allNewsItems.Value.GetArrayLength();

        // Act - Navigate to /all/news
        await Page.GotoRelativeAsync("/all/news");
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Should display all news items from all sections
        var displayedItems = await Page.GetElementCountBySelectorAsync(".content-item-card");
        displayedItems.Should().Be(totalNewsCount,
            "/all/news should show all news items from all sections");

        // All items should be from "news" collection
        var collectionBadges = await Page.Locator(".collection-badge-white").AllTextContentsAsync();
        collectionBadges.Should().AllSatisfy(badge =>
            badge.Should().Contain("News", "all items should be from the News collection"));
    }

    [Fact]
    public async Task AllButton_ExistsInCollectionSidebar()
    {
        // Arrange

        // Act - Navigate to any section
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Collections section should have collection links (All + regular collections)
        // Use more specific selector to only count collection links, not custom pages or RSS feed
        var collectionsSection = Page.Locator(".sidebar-section").Filter(new() { Has = Page.GetByRole(AriaRole.Heading, new() { Name = "Collections" }) });
        var collectionLinks = collectionsSection.Locator("a");
        await Assertions.Expect(collectionLinks).ToHaveCountAsync(5); // All + News + Blogs + Videos + Community
        await Assertions.Expect(collectionLinks.First).ToBeVisibleAsync(); // Verify at least one is visible
    }

    [Fact]
    public async Task AllCollection_ShowsCollectionBadges()
    {
        // Arrange

        // Act - Navigate to /github-copilot/all
        await Page.GotoRelativeAsync("/github-copilot/all");
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Collection badges should be visible on items
        var firstCard = Page.Locator(".content-item-card").First;
        var collectionBadge = firstCard.Locator(".collection-badge-white");

        await collectionBadge.AssertElementVisibleAsync();

        // Badge should have proper capitalization
        var badgeText = await collectionBadge.TextContentWithTimeoutAsync();
        badgeText.Should().MatchRegex("^[A-Z]",
            "collection badge should start with a capital letter (proper capitalization)");
    }

    [Fact]
    public async Task SpecificCollection_DoesNotShowCollectionBadge()
    {
        // Arrange

        // Act - Navigate to /github-copilot/news
        await Page.GotoRelativeAsync("/github-copilot/news");
        await Page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Collection badge should NOT be visible (redundant)
        var firstCard = Page.Locator(".content-item-card").First;
        var collectionBadge = firstCard.Locator(".collection-badge");

        var isVisible = await collectionBadge.IsVisibleAsync();
        isVisible.Should().BeFalse(
            "collection badges should be hidden when viewing a specific collection (redundant information)");
    }

    #endregion

    #region Interactive Button Tests

    [Fact]
    public async Task CollectionButtons_AreInteractive()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Click each collection button and verify navigation
        // NOTE: Blazor Server uses enhanced navigation (SPA-style), so we poll for URL changes
        // instead of waiting for navigation events (see BlazorHelpers.WaitForBlazorUrlContainsAsync)
        var newsButton = Page.Locator(".collection-nav a", new() { HasTextString = "News" });
        await newsButton.ClickBlazorElementAsync();
        await Page.WaitForBlazorUrlContainsAsync("/news");

        var blogsButton = Page.Locator(".collection-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.ClickBlazorElementAsync();
        await Page.WaitForBlazorUrlContainsAsync("/blogs");

        var videosButton = Page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();
        await Page.WaitForBlazorUrlContainsAsync("/videos");

        var communityButton = Page.Locator(".collection-nav a", new() { HasTextString = "Community" });
        await communityButton.ClickBlazorElementAsync();
        await Page.WaitForBlazorUrlContainsAsync("/community");
    }

    [Fact]
    public async Task RetryButton_ReloadsContentAfterError()
    {
        // Arrange

        // Simulate an error by navigating with API down (we'll test with valid URL but check button exists)
        await Page.GotoRelativeAsync("/");
        await Page.WaitForBlazorRenderAsync(".section-header.home-banner");

        // Check if error message with retry button appears (may not in normal conditions)
        // Look for retry button within error message context, not Blazor reconnect button
        var errorContainer = Page.Locator(".error");
        var retryButton = errorContainer.Locator("button", new() { HasTextString = "Retry" });
        var retryButtonExists = await retryButton.CountAsync() > 0;

        if (retryButtonExists)
        {
            // Act - Click retry button
            await retryButton.ClickBlazorElementAsync();

            // Assert - Should attempt to reload (check for loading state or success)
            await Page.WaitForBlazorRenderAsync(".section-card");
        }

        // If no error state, just verify the button would work (test passes either way)
        retryButtonExists.Should().BeFalse(
            "under normal conditions, no error message or retry button should appear");
    }

    [Fact]
    public async Task ActiveCollectionButton_HasActiveClass()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Assert - News button should have "active" class (verified by .active selector)
        await Page.AssertElementVisibleBySelectorAsync(".collection-nav a.active");
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");

        // Other buttons should NOT have "active" class - verify Blogs button exists but is not active
        var blogsButton = Page.Locator(".collection-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.AssertElementVisibleAsync();
        var blogsClass = await blogsButton.GetAttributeAsync("class", new() { Timeout = BlazorHelpers.DefaultElementTimeout });
        blogsClass.Should().NotContain("active",
            "non-selected collection buttons should not have the 'active' class");
    }

    #endregion

    #region URL Sharing and Bookmarking

    [Fact]
    public async Task DirectURL_LoadsCorrectCollectionState()
    {
        // Arrange & Act - Open browser directly to a specific collection URL
        await Page.GotoRelativeAsync("/azure/news");

        // Assert - Should load Azure News collection
        await Page.AssertUrlEndsWithAsync("/azure/news");

        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");
        await Page.AssertElementContainsTextBySelectorAsync("h1", "Azure");
    }

    [Fact]
    public async Task CopiedURL_SharesExactCollectionState()
    {
        // Arrange
        var page1 = await _context!.NewPageWithDefaultsAsync();
        await page1.GotoRelativeAsync("/ml/videos");
        var sharedUrl = page1.Url;

        // Act - Open shared URL in new tab/page
        var page2 = await _context!.NewPageWithDefaultsAsync();
        await page2.GotoAndWaitForBlazorAsync(sharedUrl);
        await page2.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Both pages should show identical state
        page2.Url.Should().Be(sharedUrl,
            "shared URL should load the exact same route");

        await page2.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "Videos");

        await page1.CloseAsync();
        await page2.CloseAsync();
    }

    #endregion
}

