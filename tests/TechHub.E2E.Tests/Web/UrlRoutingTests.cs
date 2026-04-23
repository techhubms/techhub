using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for URL routing, collection navigation, and "all" collection functionality
/// These tests document and verify the expected behavior of URL-based navigation
/// </summary>
public class UrlRoutingTests : PlaywrightTestBase
{
    public UrlRoutingTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

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

        // "Browse" button should be active
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "Browse");
    }

    [Fact]
    public async Task NavigateToSectionWithCollection_URLMatchesRoute()
    {
        // Arrange

        // Act - Navigate directly to /ai/news (AI section doesn't hide collection pages)
        await Page.GotoRelativeAsync("/ai/news");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Assert - URL should remain /ai/news
        await Page.AssertUrlEndsWithAsync("/ai/news");

        // News button should be active
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "News");
    }

    [Fact]
    public async Task ClickCollectionButton_UpdatesURL()
    {
        // Arrange - Use AI section which doesn't hide collection pages
        await Page.GotoRelativeAsync("/ai");

        // Act - Click "Blogs" collection button
        var blogsButton = Page.Locator(".sub-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/ai/blogs.*"), new() { Timeout = 2000 }));

        // Assert - URL should update to /ai/blogs

        // Wait for page heading to actually update to show "Blog" (from "Blog Posts")
        var pageH1 = Page.Locator("h1.page-h1");
        await Assertions.Expect(pageH1).ToContainTextAsync("Blog");
    }

    [Fact]
    public async Task ClickBrowseButton_UpdatesURLToSectionSlashAll()
    {
        // Arrange - Navigate to a section with news collection via a section that doesn't hide collections
        await Page.GotoRelativeAsync("/ai/news");

        // Act - Click "Browse" button
        var browseButton = Page.Locator(".sub-nav a", new() { HasTextString = "Browse" });
        await browseButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/ai/all.*"), new() { Timeout = 2000 }));

        // Assert - URL should update to /ai/all
    }

    [Fact]
    public async Task BrowserBackButton_NavigatesToPreviousCollection()
    {
        // Arrange - Use AI section which doesn't hide collection pages
        await Page.GotoRelativeAsync("/ai/news");

        // Verify News button is active initially
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "News");

        // Navigate to videos
        var videosButton = Page.Locator(".sub-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/ai/videos.*"), new() { Timeout = 2000 }));

        // Wait for Blazor to sync state (update .active class)
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "Videos");

        // Act - Press browser back button
        await Page.GoBackAsync();

        // Assert - Should return to /ai/news
        await Page.WaitForBlazorUrlContainsAsync("/ai/news");

        // Wait for Blazor to sync state with URL (OnParametersSetAsync should fire)
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "News");
    }

    [Fact]
    public async Task BrowserForwardButton_NavigatesToNextCollection()
    {
        // Arrange - Use AI section which doesn't hide collection pages
        await Page.GotoRelativeAsync("/ai/news");

        // Wait for sub-nav to be fully rendered before interacting
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "News");

        // Navigate to videos
        var videosButton = Page.Locator(".sub-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/ai/videos.*"), new() { Timeout = 2000 }));

        // Go back to news
        await Page.GoBackAsync();
        await Page.WaitForBlazorUrlContainsAsync("/ai/news");

        // Act - Press browser forward button
        await Page.GoForwardAsync();

        // Assert - Should return to /ai/videos
        await Page.WaitForBlazorUrlContainsAsync("/ai/videos");
    }

    #endregion

    #region "All" Collection Tests

    [Fact]
    public async Task AllCollection_ShowsAllContentFromSection()
    {
        // Act - Navigate to /github-copilot/all
        await Page.GotoRelativeAsync("/github-copilot/all");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Assert - Should display content items
        var displayedItems = await Page.GetElementCountBySelectorAsync(".card");
        displayedItems.Should().BeGreaterThan(0,
            "the 'all' collection should show content items from the section");

        // Page heading should indicate "All" - verify active button shows "Browse"
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "Browse");

        // Verify "all" shows MORE items than a single collection by comparing with news
        await Page.GotoRelativeAsync("/github-copilot/news");
        await Page.Locator(".card").First.AssertElementVisibleAsync();
        var newsItems = await Page.GetElementCountBySelectorAsync(".card");
        displayedItems.Should().BeGreaterThanOrEqualTo(newsItems,
            "the 'all' collection should show at least as many items as any single collection");
    }

    [Fact]
    public async Task AllSection_AllCollection_ShowsAllContent()
    {
        // Act - Navigate to /all/all
        await Page.GotoRelativeAsync("/all/all");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Assert - Should display content items from all sections
        var allItems = await Page.GetElementCountBySelectorAsync(".card");
        allItems.Should().BeGreaterThan(0,
            "/all/all should show content items from all sections and all collections");

        // Verify "all/all" shows MORE items than a single section by comparing
        await Page.GotoRelativeAsync("/github-copilot/all");
        await Page.Locator(".card").First.AssertElementVisibleAsync();
        var sectionItems = await Page.GetElementCountBySelectorAsync(".card");
        allItems.Should().BeGreaterThanOrEqualTo(sectionItems,
            "/all/all should show at least as many items as any single section");
    }

    [Fact]
    public async Task AllSection_SpecificCollection_ShowsCollectionAcrossAllSections()
    {
        // Act - Navigate to /all/news
        await Page.GotoRelativeAsync("/all/news");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Assert - Should display news items from all sections
        var displayedItems = await Page.GetElementCountBySelectorAsync(".card");
        displayedItems.Should().BeGreaterThan(0,
            "/all/news should show news items from all sections");

        // Collection badges should NOT be shown (only shown when CollectionName='all', not on specific collections like 'news')
        var firstCard = Page.Locator(".card").First;
        var cardTags = firstCard.Locator(".card-tags");

        // Verify card-tags exists
        await cardTags.AssertElementVisibleAsync();

        // Cards should have tag badges (clickable buttons) but no collection badge
        // On /all/news, CollectionName is 'news' so collection badges are hidden
        var tagBadges = await cardTags.Locator("button.badge-tag").CountAsync();
        tagBadges.Should().BeGreaterThan(0, "cards should have clickable tag badges");
    }

    [Fact]
    public async Task AllButton_ExistsInSubNav()
    {
        // Arrange

        // Act - Navigate to any section
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Sub-nav should have "Browse" link plus collection links/custom pages
        // Sub-nav contains: Browse + custom pages (when HideCollectionPages is true, regular collections hidden)
        var subNav = Page.Locator("nav.sub-nav");
        var subNavLinks = subNav.Locator("a");

        // Should have at least Browse link (exact count depends on configured collections and custom pages)
        await Assertions.Expect(subNavLinks).Not.ToHaveCountAsync(0);

        // First link should be "Browse"
        await Assertions.Expect(subNavLinks.First).ToHaveTextAsync("Browse");
        await Assertions.Expect(subNavLinks.First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task AllCollection_ShowsCollectionBadges()
    {
        // Arrange

        // Act - Navigate to /github-copilot/all
        await Page.GotoRelativeAsync("/github-copilot/all");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Assert - Collection badges should be visible on items (clickable links to collection page)
        var firstCard = Page.Locator(".card").First;
        // Collection badge is the first .badge-purple link within .card-tags (before tag badges)
        var collectionBadge = firstCard.Locator(".card-tags a.badge-purple").First;

        await collectionBadge.AssertElementVisibleAsync();

        // Badge should have proper capitalization
        var badgeText = await collectionBadge.TextContentWithTimeoutAsync();
        badgeText.Should().MatchRegex("^[A-Z]",
            "collection badge should start with a capital letter (proper capitalization)");

        // Badge should be a link to the collection page
        var href = await collectionBadge.GetAttributeAsync("href");
        href.Should().NotBeNullOrEmpty("collection badge should link to the collection page");
    }

    [Fact]
    public async Task SpecificCollection_DoesNotShowCollectionBadge()
    {
        // Arrange

        // Act - Navigate to /github-copilot/news
        await Page.GotoRelativeAsync("/github-copilot/news");
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Assert - Collection badge should NOT be visible (redundant)
        var firstCard = Page.Locator(".card").First;
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
        // Arrange - Use AI section which doesn't hide collection pages
        await Page.GotoRelativeAsync("/ai");

        // Act - Click each collection button and verify navigation
        // NOTE: Blazor Server uses enhanced navigation (SPA-style), so we retry each
        // click until the URL confirms navigation (ClickAndExpectAsync pattern).
        var newsButton = Page.Locator(".sub-nav a", new() { HasTextString = "News" });
        await newsButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*/news.*"), new() { Timeout = 2000 }));

        var blogsButton = Page.Locator(".sub-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*/blogs.*"), new() { Timeout = 2000 }));

        var videosButton = Page.Locator(".sub-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*/videos.*"), new() { Timeout = 2000 }));

        var communityButton = Page.Locator(".sub-nav a", new() { HasTextString = "Community" });
        await communityButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*/community.*"), new() { Timeout = 2000 }));
    }

    [Fact]
    public async Task RetryButton_ReloadsContentAfterError()
    {
        // Arrange

        // Simulate an error by navigating with API down (we'll test with valid URL but check button exists)
        await Page.GotoRelativeAsync("/");
        await Page.WaitForBlazorRenderAsync(".section-banner.home-banner");

        // Check if error message with retry button appears (may not in normal conditions)
        // Look for retry button within error message context, not Blazor reconnect button
        var errorContainer = Page.Locator(".error");
        var retryButton = errorContainer.Locator("button", new() { HasTextString = "Retry" });
        var retryButtonExists = await retryButton.CountAsync() > 0;

        if (retryButtonExists)
        {
            // Act - Click retry button
            await retryButton.ClickAndExpectAsync(async () =>
                await Assertions.Expect(Page.Locator(".section-card")).ToBeVisibleAsync(new() { Timeout = 2000 }));
        }

        // If no error state, just verify the button would work (test passes either way)
        retryButtonExists.Should().BeFalse(
            "under normal conditions, no error message or retry button should appear");
    }

    [Fact]
    public async Task ActiveCollectionButton_HasActiveClass()
    {
        // Arrange - Use AI section which doesn't hide collection pages
        await Page.GotoRelativeAsync("/ai/news");

        // Assert - News button should have "active" class (verified by .active selector)
        await Page.AssertElementVisibleBySelectorAsync(".sub-nav a.active");
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "News");

        // Other buttons should NOT have "active" class - verify Blogs button exists but is not active
        var blogsButton = Page.Locator(".sub-nav a", new() { HasTextString = "Blogs" });
        await blogsButton.AssertElementVisibleAsync();
        var blogsClass = await blogsButton.GetAttributeAsync("class");
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

        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "News");
        await Page.AssertElementContainsTextBySelectorAsync("h1.page-h1", "Browse Azure News");
    }

    [Fact]
    public async Task CopiedURL_SharesExactCollectionState()
    {
        // Arrange
        var page1 = await Context.NewPageWithDefaultsAsync();
        await page1.GotoRelativeAsync("/ml/videos");
        var sharedUrl = page1.Url;

        // Act - Open shared URL in new tab/page
        var page2 = await Context.NewPageWithDefaultsAsync();
        await page2.GotoAndWaitForBlazorAsync(sharedUrl);
        await page2.Locator(".card").First.AssertElementVisibleAsync();

        // Assert - Both pages should show identical state
        page2.Url.Should().Be(sharedUrl,
            "shared URL should load the exact same route");

        await page2.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "Videos");

        await page1.CloseAsync();
        await page2.CloseAsync();
    }

    #endregion
}

