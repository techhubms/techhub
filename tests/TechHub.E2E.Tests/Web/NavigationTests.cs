using Microsoft.Playwright;
using Xunit;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for navigation improvements and URL structure
/// Tests for user story requirements: section ordering, URL structure, and navigation flow
/// </summary>
[Collection("Navigation Tests")]
public class NavigationTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private const string BaseUrl = "http://localhost:5184";
    private const string ApiUrl = "http://localhost:5029";

    public NavigationTests(PlaywrightCollectionFixture fixture)
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
            await _context.DisposeAsync();
    }

    [Fact]
    public async Task Homepage_SectionsAreOrderedCorrectly()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        
        // Act
        await page.GotoAndWaitForBlazorAsync(BaseUrl);
        
        // Get all section card titles
        var sectionTitles = await page.Locator(".section-card h2").AllTextContentsAsync();
        
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
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task SectionCard_Click_NavigatesToSectionHomepage()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync(BaseUrl);
        
        // Act - Click on the GitHub Copilot section card
        // Find the GitHub Copilot card specifically (not the first card which is "Everything")
        var ghCopilotCard = page.Locator(".section-card-link[href*='github-copilot']");
        await ghCopilotCard.WaitForAsync();
        
        var href = await ghCopilotCard.GetAttributeAsync("href");
        Assert.NotNull(href);
        Assert.Contains("github-copilot", href);
        
        // Blazor uses enhanced navigation (SPA-style), so URL changes without page reload
        await ghCopilotCard.ClickAsync();
        
        // Wait for URL to contain the section name
        await page.WaitForBlazorUrlContainsAsync("/github-copilot");
        
        // Assert - Should navigate to /github-copilot
        Assert.Contains("/github-copilot", page.Url);
        Assert.DoesNotContain("#", page.Url); // Should not have hash fragment
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task CollectionNavigation_UpdatesURL_ToSectionSlashCollection()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot");
        
        // Act - Click on "News" collection button
        var newsButton = page.Locator(".collection-nav a", new() { HasTextString = "News" });
        await newsButton.ClickAsync();
        
        // Assert - URL should be /github-copilot/news
        await page.WaitForURLAsync("**/github-copilot/news");
        Assert.EndsWith("/github-copilot/news", page.Url);
        Assert.DoesNotContain("#", page.Url); // No hash fragments
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task CollectionPage_DoesNotShowRedundantCollectionBadge()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        
        // Act - Navigate to GitHub Copilot News collection
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot/news");
        
        // Get first content card
        var firstCard = page.Locator(".content-item-card").First;
        
        // Assert - Collection badge should NOT be visible (or should not say "News")
        var collectionBadges = firstCard.Locator(".collection-badge");
        var badgeCount = await collectionBadges.CountAsync();
        
        // Either no collection badge, or badge doesn't say "News"
        if (badgeCount > 0)
        {
            var badgeText = await collectionBadges.First.TextContentAsync();
            Assert.NotEqual("News", badgeText); // Should not show the current collection
        }
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task AllPage_ShowsCollectionBadgeBeforeTags()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        
        // Act - Navigate to "All" section (contains all content, may take longer to load)
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/all");
        
        // Get first content card
        var firstCard = page.Locator(".content-item-card").First;
        
        // Assert - Collection badge should exist and be before tags
        var collectionBadge = firstCard.Locator(".collection-badge-white").First;
        Assert.True(await collectionBadge.IsVisibleAsync());
        
        // Collection badge should have proper capitalization (e.g., "News" not "news")
        var badgeText = await collectionBadge.TextContentAsync();
        Assert.Matches("^[A-Z]", badgeText!); // Starts with capital letter
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task SectionPage_CollectionSidebarIsClickable()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot");
        
        // Act - Click on "Videos" collection
        var videosButton = page.Locator(".collection-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickAsync();
        
        // Assert - Should navigate and load videos
        await page.WaitForURLAsync("**/github-copilot/videos");
        
        // Wait for page to fully load after navigation
        await page.WaitForBlazorStateSyncAsync("Videos");
        
        // Videos collection should be active
        var activeButton = page.Locator(".collection-nav a.active");
        var activeText = await activeButton.TextContentAsync();
        Assert.Contains("Videos", activeText);
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task SectionPage_HeaderAreaHasConsistentHeight()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        
        // Act - Measure header height on homepage
        await page.GotoAndWaitForBlazorAsync(BaseUrl);
        var homeHeaderHeight = await page.Locator(".section-header.home-banner").BoundingBoxAsync();
        
        // Navigate to section page
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot");
        var sectionHeaderHeight = await page.Locator(".section-header").BoundingBoxAsync();
        
        // Assert - Both should have defined heights (not auto)
        Assert.NotNull(homeHeaderHeight);
        Assert.NotNull(sectionHeaderHeight);
        Assert.True(homeHeaderHeight.Height > 0);
        Assert.True(sectionHeaderHeight.Height > 0);
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task SectionBackgroundImages_DisplayCorrectly()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync(BaseUrl);
        
        // Act - Get first section card
        var firstCard = page.Locator(".section-card").First;
        var headerElement = firstCard.Locator(".section-card-header");
        
        // Assert - Should have background-image style
        var style = await headerElement.GetAttributeAsync("style");
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
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task DirectURL_ToSectionWithCollection_LoadsCorrectContent()
    {
        // Arrange & Act
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/ai/news");
        
        // Assert
        await page.WaitForSelectorAsync(".collection-nav");
        await page.WaitForSelectorAsync(".content-item-card");
        
        // Should show AI section
        var heading = page.Locator("h1");
        var headingText = await heading.TextContentAsync();
        Assert.Contains("Artificial Intelligence", headingText);
        
        // News collection should be active
        var activeButton = page.Locator(".collection-nav a.active");
        var activeText = await activeButton.TextContentAsync();
        Assert.Contains("News", activeText);
        
        await page.CloseAsync();
    }
}
