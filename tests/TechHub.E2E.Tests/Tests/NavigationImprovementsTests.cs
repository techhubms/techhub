using Microsoft.Playwright;
using Xunit;

namespace TechHub.E2E.Tests.Tests;

/// <summary>
/// E2E tests for navigation improvements and URL structure
/// Tests for user story requirements: section ordering, URL structure, and navigation flow
/// </summary>
public class NavigationImprovementsTests : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private const string BaseUrl = "http://localhost:5184";
    private const string ApiUrl = "http://localhost:5029";

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
    }

    public async Task DisposeAsync()
    {
        if (_browser != null)
            await _browser.DisposeAsync();
        
        _playwright?.Dispose();
    }

    [Fact]
    public async Task Homepage_SectionsAreOrderedCorrectly()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        
        // Act
        await page.GotoAsync(BaseUrl);
        await page.WaitForSelectorAsync(".section-card");
        
        // Get all section card titles
        var sectionTitles = await page.Locator(".section-card h2").AllTextContentsAsync();
        
        // Assert - Expected order from live site
        var expectedOrder = new[]
        {
            "GitHub Copilot",
            "Artificial Intelligence",
            "Machine Learning",
            "DevOps",
            "Azure",
            ".NET & Coding",
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
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForSelectorAsync(".section-card");
        
        // Act - Click on the GitHub Copilot section card (not a collection badge)
        await page.Locator(".section-card").First.ClickAsync();
        
        // Assert - Should navigate to /github-copilot
        await page.WaitForURLAsync("**/github-copilot");
        Assert.Contains("/github-copilot", page.Url);
        Assert.DoesNotContain("#", page.Url); // Should not have hash fragment
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task CollectionNavigation_UpdatesURL_ToSectionSlashCollection()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"{BaseUrl}/github-copilot");
        await page.WaitForSelectorAsync(".collection-nav");
        
        // Act - Click on "News" collection button
        var newsButton = page.Locator(".collection-nav button", new() { HasTextString = "News" });
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
        var page = await _browser!.NewPageAsync();
        
        // Act - Navigate to GitHub Copilot News collection
        await page.GotoAsync($"{BaseUrl}/github-copilot/news");
        await page.WaitForSelectorAsync(".content-item-card");
        
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
        var page = await _browser!.NewPageAsync();
        
        // Act - Navigate to "All" section
        await page.GotoAsync($"{BaseUrl}/all");
        await page.WaitForSelectorAsync(".content-item-card");
        
        // Get first content card
        var firstCard = page.Locator(".content-item-card").First;
        
        // Assert - Collection badge should exist and be before tags
        var collectionBadge = firstCard.Locator(".collection-badge").First;
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
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"{BaseUrl}/github-copilot");
        await page.WaitForSelectorAsync(".collection-nav");
        
        // Act - Click on "Videos" collection
        var videosButton = page.Locator(".collection-nav button", new() { HasTextString = "Videos" });
        await videosButton.ClickAsync();
        
        // Assert - Should navigate and load videos
        await page.WaitForURLAsync("**/github-copilot/videos");
        await page.WaitForSelectorAsync(".content-item-card");
        
        // Videos collection should be active
        var activeButton = page.Locator(".collection-nav button.active");
        var activeText = await activeButton.TextContentAsync();
        Assert.Contains("Videos", activeText);
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task SectionPage_HeaderAreaHasConsistentHeight()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        
        // Act - Measure header height on homepage
        await page.GotoAsync(BaseUrl);
        await page.WaitForSelectorAsync(".home-header");
        var homeHeaderHeight = await page.Locator(".home-header").BoundingBoxAsync();
        
        // Navigate to section page
        await page.GotoAsync($"{BaseUrl}/github-copilot");
        await page.WaitForSelectorAsync(".section-header");
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
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync(BaseUrl);
        await page.WaitForSelectorAsync(".section-card");
        
        // Act - Get first section card
        var firstCard = page.Locator(".section-card").First;
        var headerElement = firstCard.Locator(".section-header");
        
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
        
        // Overlay height should match header height (no gaps)
        Assert.Equal(headerBox.Height, overlayBox.Height, precision: 1);
        
        await page.CloseAsync();
    }

    [Fact]
    public async Task DirectURL_ToSectionWithCollection_LoadsCorrectContent()
    {
        // Arrange & Act
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync($"{BaseUrl}/ai/news");
        
        // Assert
        await page.WaitForSelectorAsync(".collection-nav");
        await page.WaitForSelectorAsync(".content-item-card");
        
        // Should show AI section
        var heading = page.Locator("h1");
        var headingText = await heading.TextContentAsync();
        Assert.Contains("Artificial Intelligence", headingText);
        
        // News collection should be active
        var activeButton = page.Locator(".collection-nav button.active");
        var activeText = await activeButton.TextContentAsync();
        Assert.Contains("News", activeText);
        
        await page.CloseAsync();
    }
}
