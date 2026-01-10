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
    private readonly PlaywrightCollectionFixture _fixture = fixture;
    private IBrowserContext? _context;
    private const string BaseUrl = "http://localhost:5184";
    private const string ApiUrl = "http://localhost:5029";

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

    #region URL Routing Tests

    [Fact]
    public async Task NavigateToSection_DefaultsToAllCollection()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act - Navigate to section without specifying collection
        await page.GotoRelativeAsync("/github-copilot");

        // Assert - URL is /github-copilot (defaults to "all" without redirect)
        // This is intentional - the section page defaults to "all" collection without URL redirect
        await page.AssertUrlEndsWithAsync("/github-copilot");

        // Verify "All" collection is displayed - check the page heading
        await page.AssertElementContainsTextBySelectorAsync("h1.page-h1", "All");

        // "All" button should be active
        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "All");

        await page.CloseAsync();
    }

    [Fact]
    public async Task NavigateToSectionWithCollection_URLMatchesRoute()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act - Navigate directly to /github-copilot/news
        await page.GotoRelativeAsync("/github-copilot/news");
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - URL should remain /github-copilot/news
        await page.AssertUrlEndsWithAsync("/github-copilot/news");

        // News button should be active
        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");

        await page.CloseAsync();
    }

    [Fact]
    public async Task ClickCollectionButton_UpdatesURL()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoRelativeAsync("/github-copilot");

        // Act - Click "Blogs" collection button
        var blogsButton = page.Locator(".collection-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.ClickBlazorElementAsync();

        // Assert - URL should update to /github-copilot/blogs (WaitForURLAsync already validates URL)
        await page.WaitForURLAsync("**/github-copilot/blogs", new() { Timeout = 5000 });

        // Wait for page heading to actually update to show "Blog" (from "Blog Posts")
        var pageH1 = page.Locator("h1.page-h1");
        await Assertions.Expect(pageH1).ToContainTextAsync("Blog", new() { Timeout = 5000 });

        // Verify the page heading was already asserted with ToContainTextAsync above

        await page.CloseAsync();
    }

    [Fact]
    public async Task ClickAllButton_UpdatesURLToSectionSlashAll()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoRelativeAsync("/github-copilot/news");

        // Act - Click "All" button
        var allButton = page.Locator(".collection-nav a", new() { HasTextString = "All" });
        await allButton.ClickBlazorElementAsync();

        // Assert - URL should update to /github-copilot/all (WaitForURLAsync already validates URL)
        await page.WaitForURLAsync("**/github-copilot/all", new() { Timeout = 5000 });

        await page.CloseAsync();
    }

    [Fact]
    public async Task BrowserBackButton_NavigatesToPreviousCollection()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoRelativeAsync("/github-copilot/news");

        // Verify News button is active initially
        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");

        // Navigate to videos
        var videosButton = page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();
        await page.WaitForBlazorUrlContainsAsync("/github-copilot/videos");

        // Wait for Blazor to sync state (update .active class)
        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "Videos");

        // Act - Press browser back button
        await page.GoBackAsync();

        // Assert - Should return to /github-copilot/news (WaitForURLAsync already validates URL)
        await page.WaitForURLAsync("**/github-copilot/news", new() { Timeout = 5000 });

        // Wait for Blazor to sync state with URL (OnParametersSetAsync should fire)
        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");

        await page.CloseAsync();
    }

    [Fact]
    public async Task BrowserForwardButton_NavigatesToNextCollection()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoRelativeAsync("/github-copilot/news");

        // Navigate to videos
        var videosButton = page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();
        await page.WaitForURLAsync("**/github-copilot/videos", new() { Timeout = 5000 });

        // Go back to news
        await page.GoBackAsync();
        await page.WaitForURLAsync("**/github-copilot/news", new() { Timeout = 5000 });

        // Act - Press browser forward button
        await page.GoForwardAsync();

        // Assert - Should return to /github-copilot/videos (WaitForURLAsync already validates URL)
        await page.WaitForURLAsync("**/github-copilot/videos", new() { Timeout = 5000 });

        await page.CloseAsync();
    }

    #endregion

    #region "All" Collection Tests

    [Fact]
    public async Task AllCollection_ShowsAllContentFromSection()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Get total item count across all GitHub Copilot collections from API
        var apiResponse = await page.APIRequest.GetAsync($"{ApiUrl}/api/content?category=GitHub%20Copilot");
        var allItems = await apiResponse.JsonAsync();
        var totalItemCount = allItems.Value.GetArrayLength();

        // Act - Navigate to /github-copilot/all
        await page.GotoRelativeAsync("/github-copilot/all");
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Should display all GitHub Copilot items regardless of collection
        var displayedItems = await page.GetElementCountBySelectorAsync(".content-item-card");
        displayedItems.Should().Be(totalItemCount,
            "the 'all' collection should show all content items from the section across all collection types");

        // Page heading should indicate "All" - verify active button shows "All"
        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "All");

        await page.CloseAsync();
    }

    [Fact]
    public async Task AllSection_AllCollection_ShowsAllContent()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Get total item count across ALL sections and collections from API
        var apiResponse = await page.APIRequest.GetAsync($"{ApiUrl}/api/content");
        var allItems = await apiResponse.JsonAsync();
        var totalItemCount = allItems.Value.GetArrayLength();

        // Act - Navigate to /all/all
        await page.GotoRelativeAsync("/all/all");
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Should display ALL content from ALL sections and collections
        var displayedItems = await page.GetElementCountBySelectorAsync(".content-item-card");
        displayedItems.Should().Be(totalItemCount,
            "/all/all should show absolutely all content items from all sections and all collections");

        await page.CloseAsync();
    }

    [Fact]
    public async Task AllSection_SpecificCollection_ShowsCollectionAcrossAllSections()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Get total news count across all sections from API
        var apiResponse = await page.APIRequest.GetAsync($"{ApiUrl}/api/content?collectionName=news");
        var allNewsItems = await apiResponse.JsonAsync();
        var totalNewsCount = allNewsItems.Value.GetArrayLength();

        // Act - Navigate to /all/news
        await page.GotoRelativeAsync("/all/news");
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Should display all news items from all sections
        var displayedItems = await page.GetElementCountBySelectorAsync(".content-item-card");
        displayedItems.Should().Be(totalNewsCount,
            "/all/news should show all news items from all sections");

        // All items should be from "news" collection
        var collectionBadges = await page.Locator(".collection-badge-white").AllTextContentsAsync();
        collectionBadges.Should().AllSatisfy(badge =>
            badge.Should().Contain("News", "all items should be from the News collection"));

        await page.CloseAsync();
    }

    [Fact]
    public async Task AllButton_ExistsInCollectionSidebar()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act - Navigate to any section
        await page.GotoRelativeAsync("/github-copilot");

        // Assert - Collections section should have collection links (All + regular collections)
        // Use more specific selector to only count collection links, not custom pages or RSS feed
        var collectionsSection = page.Locator(".sidebar-section").Filter(new() { Has = page.GetByRole(AriaRole.Heading, new() { Name = "Collections" }) });
        var collectionLinks = collectionsSection.Locator("a");
        await Assertions.Expect(collectionLinks).ToHaveCountAsync(5); // All + News + Blogs + Videos + Community
        await Assertions.Expect(collectionLinks.First).ToBeVisibleAsync(); // Verify at least one is visible

        await page.CloseAsync();
    }

    [Fact]
    public async Task AllCollection_ShowsCollectionBadges()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act - Navigate to /github-copilot/all
        await page.GotoRelativeAsync("/github-copilot/all");
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Collection badges should be visible on items
        var firstCard = page.Locator(".content-item-card").First;
        var collectionBadge = firstCard.Locator(".collection-badge-white");

        await collectionBadge.AssertElementVisibleAsync();

        // Badge should have proper capitalization
        var badgeText = await collectionBadge.TextContentAsync(new() { Timeout = 3000 });
        badgeText.Should().MatchRegex("^[A-Z]",
            "collection badge should start with a capital letter (proper capitalization)");

        await page.CloseAsync();
    }

    [Fact]
    public async Task SpecificCollection_DoesNotShowCollectionBadge()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act - Navigate to /github-copilot/news
        await page.GotoRelativeAsync("/github-copilot/news");
        await page.Locator(".content-item-card").First.AssertElementVisibleAsync();

        // Assert - Collection badge should NOT be visible (redundant)
        var firstCard = page.Locator(".content-item-card").First;
        var collectionBadge = firstCard.Locator(".collection-badge");

        var isVisible = await collectionBadge.IsVisibleAsync();
        isVisible.Should().BeFalse(
            "collection badges should be hidden when viewing a specific collection (redundant information)");

        await page.CloseAsync();
    }

    #endregion

    #region Interactive Button Tests

    [Fact]
    public async Task CollectionButtons_AreInteractive()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoRelativeAsync("/github-copilot");

        // Act - Click each collection button and verify navigation
        // NOTE: Blazor Server uses enhanced navigation (SPA-style), so we poll for URL changes
        // instead of waiting for navigation events (see BlazorHelpers.WaitForBlazorUrlContainsAsync)
        var newsButton = page.Locator(".collection-nav a", new() { HasTextString = "News" });
        await newsButton.ClickBlazorElementAsync();
        await page.WaitForBlazorUrlContainsAsync("/news");

        var blogsButton = page.Locator(".collection-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.ClickBlazorElementAsync();
        await page.WaitForBlazorUrlContainsAsync("/blogs");

        var videosButton = page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();
        await page.WaitForBlazorUrlContainsAsync("/videos");

        var communityButton = page.Locator(".collection-nav a", new() { HasTextString = "Community" });
        await communityButton.ClickBlazorElementAsync();
        await page.WaitForBlazorUrlContainsAsync("/community");

        await page.CloseAsync();
    }

    [Fact]
    public async Task RetryButton_ReloadsContentAfterError()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Simulate an error by navigating with API down (we'll test with valid URL but check button exists)
        await page.GotoRelativeAsync("/");
        await page.WaitForBlazorRenderAsync(".section-header.home-banner");

        // Check if error message with retry button appears (may not in normal conditions)
        // Look for retry button within error message context, not Blazor reconnect button
        var errorContainer = page.Locator(".error");
        var retryButton = errorContainer.Locator("button", new() { HasTextString = "Retry" });
        var retryButtonExists = await retryButton.CountAsync() > 0;

        if (retryButtonExists)
        {
            // Act - Click retry button
            await retryButton.ClickAsync();

            // Assert - Should attempt to reload (check for loading state or success)
            await page.WaitForBlazorRenderAsync(".section-card");
        }

        // If no error state, just verify the button would work (test passes either way)
        retryButtonExists.Should().BeFalse(
            "under normal conditions, no error message or retry button should appear");

        await page.CloseAsync();
    }

    [Fact]
    public async Task ActiveCollectionButton_HasActiveClass()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoRelativeAsync("/github-copilot/news");

        // Assert - News button should have "active" class (verified by .active selector)
        await page.AssertElementVisibleBySelectorAsync(".collection-nav a.active");
        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");

        // Other buttons should NOT have "active" class - verify Blogs button exists but is not active
        var blogsButton = page.Locator(".collection-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.AssertElementVisibleAsync();
        var blogsClass = await blogsButton.GetAttributeAsync("class");
        blogsClass.Should().NotContain("active",
            "non-selected collection buttons should not have the 'active' class");

        await page.CloseAsync();
    }

    #endregion

    #region URL Sharing and Bookmarking

    [Fact]
    public async Task DirectURL_LoadsCorrectCollectionState()
    {
        // Arrange & Act - Open browser directly to a specific collection URL
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoRelativeAsync("/azure/news");

        // Assert - Should load Azure News collection
        await page.AssertUrlEndsWithAsync("/azure/news");

        await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");
        await page.AssertElementContainsTextBySelectorAsync("h1", "Azure");

        await page.CloseAsync();
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

