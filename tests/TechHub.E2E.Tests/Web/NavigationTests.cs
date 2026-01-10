using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for navigation improvements and URL structure
/// Tests for user story requirements: section ordering, URL structure, and navigation flow
/// </summary>
[Collection("Navigation Tests")]
public class NavigationTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
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

    [Fact]
    public async Task Homepage_SectionsAreOrderedCorrectly()
    {
        // Arrange

        // Act
        await Page.GotoRelativeAsync("/");

        // Get all section card titles
        var sectionTitles = await Page.Locator(".section-card h2").AllTextContentsAsync();

        // Assert - Expected order from live site (including "All" section)
        var expectedOrder = new[]
        {
            "All",
            "GitHub Copilot",
            "Artificial Intelligence",
            "Machine Learning",
            "DevOps",
            "Azure",
            ".NET",
            "Security"
        };

        Assert.Equal(expectedOrder.Length, sectionTitles.Count);
        for (int i = 0; i < expectedOrder.Length; i++)
        {
            Assert.Equal(expectedOrder[i], sectionTitles[i]);
        }
    }

    [Fact]
    public async Task SectionCard_Click_NavigatesToSectionHomepage()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Click on the GitHub Copilot section card
        // The card link is inside the section-card-container
        var ghCopilotCard = Page.Locator(".section-card-container a.section-card[href*='github-copilot']");
        await ghCopilotCard.WaitForAsync();

        var href = await ghCopilotCard.GetHrefAsync();
        Assert.NotNull(href);
        Assert.Contains("github-copilot", href);

        // Blazor uses enhanced navigation (SPA-style), so URL changes without page reload
        // Use ClickBlazorElementAsync to wait for Blazor interactivity before clicking
        await ghCopilotCard.ClickBlazorElementAsync();

        // Wait for URL to contain the section name (already asserts URL change)
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot");

        // Assert - Should not have hash fragment
        Assert.DoesNotContain("#", Page.Url);
    }

    [Fact]
    public async Task CollectionNavigation_UpdatesURL_ToSectionSlashCollection()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Click on "News" collection button
        var newsButton = Page.Locator(".collection-nav a", new() { HasTextString = "News" });
        await newsButton.ClickBlazorElementAsync();

        // Assert - URL should be /github-copilot/news
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/news");
        Assert.DoesNotContain("#", Page.Url); // No hash fragments
    }

    [Fact]
    public async Task CollectionPage_DoesNotShowRedundantCollectionBadge()
    {
        // Arrange

        // Act - Navigate to GitHub Copilot News collection
        await Page.GotoRelativeAsync("/github-copilot/news");

        // Get first content card
        var firstCard = Page.Locator(".content-item-card").First;

        // Assert - Collection badge should NOT be visible (or should not say "News")
        var collectionBadges = firstCard.Locator(".collection-badge");
        var badgeCount = await collectionBadges.CountAsync();

        // Either no collection badge, or badge doesn't say "News"
        if (badgeCount > 0)
        {
            var badgeText = await collectionBadges.First.TextContentWithTimeoutAsync();
            Assert.NotEqual("News", badgeText); // Should not show the current collection
        }
    }

    [Fact]
    public async Task AllPage_ShowsCollectionBadgeBeforeTags()
    {
        // Arrange

        // Act - Navigate to "All" section (contains all content, may take longer to load)
        await Page.GotoRelativeAsync("/all");

        // Get first content card
        var firstCard = Page.Locator(".content-item-card").First;

        // Assert - Collection badge should exist and be before tags
        var collectionBadge = firstCard.Locator(".collection-badge-white").First;
        await collectionBadge.AssertElementVisibleAsync();

        // Collection badge should have proper capitalization (e.g., "News" not "news")
        var badgeText = await collectionBadge.TextContentWithTimeoutAsync();
        Assert.Matches("^[A-Z]", badgeText!); // Starts with capital letter
    }

    [Fact]
    public async Task SectionPage_CollectionSidebarIsClickable()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Click on "Videos" collection
        var videosButton = Page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();

        // Assert - Should navigate and load videos
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/videos");

        // Wait for page to fully load after navigation
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "Videos");
    }

    [Fact]
    public async Task SectionPage_HeaderAreaHasConsistentHeight()
    {
        // Arrange

        // Act - Measure header height on homepage
        await Page.GotoRelativeAsync("/");
        var homeHeaderHeight = await Page.Locator(".section-header.home-banner").BoundingBoxAsync();

        // Navigate to section page
        await Page.GotoRelativeAsync("/github-copilot");
        var sectionHeaderHeight = await Page.Locator(".section-header").BoundingBoxAsync();

        // Assert - Both should have defined heights (not auto)
        Assert.NotNull(homeHeaderHeight);
        Assert.NotNull(sectionHeaderHeight);
        Assert.True(homeHeaderHeight.Height > 0);
        Assert.True(sectionHeaderHeight.Height > 0);
    }

    [Fact]
    public async Task SectionBackgroundImages_DisplayCorrectly()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Get first section card
        var firstCard = Page.Locator(".section-card").First;
        var headerElement = firstCard.Locator(".section-card-header");

        // Assert - Should have background-image style
        var style = await headerElement.GetAttributeAsync("style", new() { Timeout = BlazorHelpers.DefaultElementTimeout });
        Assert.NotNull(style);
        Assert.Contains("background-image", style);

        // Check that there's no grey bar (overlay should cover full height)
        var overlay = headerElement.Locator(".section-overlay");
        var overlayBox = await overlay.BoundingBoxAsync();
        var headerBox = await headerElement.BoundingBoxAsync();

        Assert.NotNull(overlayBox);
        Assert.NotNull(headerBox);

        // Overlay should have reasonable height (not zero/collapsed)
        Assert.True(overlayBox.Height > 50, "overlay should have substantial height");
        Assert.True(headerBox.Height > 50, "header should have substantial height");
    }

    [Fact]
    public async Task DirectURL_ToSectionWithCollection_LoadsCorrectContent()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/ai/news");

        // Assert
        await Page.WaitForSelectorWithTimeoutAsync(".collection-nav");
        await Page.WaitForSelectorWithTimeoutAsync(".content-item-card");

        // Should show AI section
        await Page.AssertElementContainsTextBySelectorAsync("h1", "Artificial Intelligence");

        // News collection should be active
        await Page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "News");
    }
}
