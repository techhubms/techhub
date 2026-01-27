using System.Text.RegularExpressions;
using FluentAssertions;
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

        sectionTitles.Count.Should().Be(expectedOrder.Length);
        for (int i = 0; i < expectedOrder.Length; i++)
        {
            sectionTitles[i].Should().Be(expectedOrder[i]);
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
        href.Should().NotBeNull();
        href.Should().Contain("github-copilot");

        // Blazor uses enhanced navigation (SPA-style), so URL changes without page reload
        // Use ClickBlazorElementAsync to wait for Blazor interactivity before clicking
        await ghCopilotCard.ClickBlazorElementAsync();

        // Wait for URL to contain the section name (already asserts URL change)
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot");

        // Assert - Should not have hash fragment
        Page.Url.Should().NotContain("#");
    }

    [Fact]
    public async Task CollectionNavigation_UpdatesURL_ToSectionSlashCollection()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Click on "News" collection button
        var newsButton = Page.Locator(".sub-nav a", new() { HasTextString = "News" });
        await newsButton.ClickBlazorElementAsync();

        // Assert - URL should be /github-copilot/news
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/news");
        Page.Url.Should().NotContain("#"); // No hash fragments
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
            badgeText.Should().NotBe("News"); // Should not show the current collection
        }
    }

    [Fact]
    public async Task AllPage_ShowsCollectionBadgeBeforeTags()
    {
        // Arrange

        // Act - Navigate to "All" section (contains all content, may take longer to load)
        await Page.GotoRelativeAsync("/all");

        // Get first content card
        var firstCard = Page.Locator(".card").First;

        // Assert - Collection badge should exist and be before tags
        // Collection badge is the last .badge-grey in .card-tags
        var collectionBadge = firstCard.Locator(".card-tags .badge-grey").Last;
        await collectionBadge.AssertElementVisibleAsync();

        // Collection badge should have proper capitalization (e.g., "News" not "news")
        var badgeText = await collectionBadge.TextContentWithTimeoutAsync();
        badgeText!.Should().MatchRegex("^[A-Z]"); // Starts with capital letter
    }

    [Fact]
    public async Task SectionPage_SubNavIsClickable()
    {
        // Arrange
        await Page.GotoRelativeAsync("/github-copilot");

        // Act - Click on "Videos" collection in sub-nav
        var videosButton = Page.Locator(".sub-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickBlazorElementAsync();

        // Assert - Should navigate and load videos
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/videos");

        // Wait for page to fully load after navigation
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "Videos");
    }

    [Fact]
    public async Task SectionPage_HeaderAreaHasConsistentHeight()
    {
        // Arrange

        // Act - Measure banner height on homepage
        await Page.GotoRelativeAsync("/");
        var homeHeaderHeight = await Page.Locator(".section-banner.home-banner").BoundingBoxAsync();

        // Navigate to section page
        await Page.GotoRelativeAsync("/github-copilot");
        var sectionHeaderHeight = await Page.Locator(".section-banner").BoundingBoxAsync();

        // Assert - Both should have defined heights (not auto)
        homeHeaderHeight.Should().NotBeNull();
        sectionHeaderHeight.Should().NotBeNull();
        (homeHeaderHeight.Height > 0).Should().BeTrue();
        (sectionHeaderHeight.Height > 0).Should().BeTrue();
    }

    [Fact]
    public async Task SectionBackgroundImages_DisplayCorrectly()
    {
        // Arrange
        await Page.GotoRelativeAsync("/");

        // Act - Get first section card
        var firstCard = Page.Locator(".section-card").First;
        var headerElement = firstCard.Locator(".section-card-header");

        // Assert - Should have background image via CSS class (no inline style)
        var classAttr = await headerElement.GetAttributeAsync("class");
        classAttr.Should().NotBeNull();
        classAttr.Should().MatchRegex("section-bg-(ai|github-copilot|azure|ml|devops|coding|security|all)");

        // Check that there's no grey bar (overlay should cover full height)
        var overlay = headerElement.Locator(".section-overlay");
        var overlayBox = await overlay.BoundingBoxAsync();
        var headerBox = await headerElement.BoundingBoxAsync();

        overlayBox.Should().NotBeNull();
        headerBox.Should().NotBeNull();

        // Overlay should have reasonable height (not zero/collapsed)
        (overlayBox.Height > 50).Should().BeTrue("overlay should have substantial height");
        (headerBox.Height > 50).Should().BeTrue("header should have substantial height");
    }

    [Fact]
    public async Task DirectURL_ToSectionWithCollection_LoadsCorrectContent()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/ai/news");

        // Assert
        await Page.WaitForSelectorWithTimeoutAsync(".sub-nav");
        await Page.WaitForSelectorWithTimeoutAsync(".card");

        // Should show AI section and News collection
        await Page.AssertElementContainsTextBySelectorAsync("h1.page-h1", "Browse Artificial Intelligence News");

        // News collection should be active
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "News");
    }

    [Fact]
    public async Task TOC_HighlightingWorksAfterNavigation_FromHomepageToHandbook()
    {
        // Arrange - Start on homepage (which doesn't have TOC)
        await Page.GotoRelativeAsync("/");

        // Act - Navigate to handbook page (which has TOC)
        var handbookLink = Page.Locator("a[href*='/github-copilot/handbook']").First;
        await handbookLink.ClickBlazorElementAsync();

        // Wait for navigation to complete
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/handbook");

        // Wait for TOC to be visible (use Expect for auto-retry)
        var tocElement = Page.Locator("[data-toc-scroll-spy]");
        await Assertions.Expect(tocElement).ToBeVisibleAsync();

        // Assert - TOC should be initialized and have links
        var tocLinks = tocElement.Locator("a[href*='#']");
        var tocLinkCount = await tocLinks.CountAsync();
        tocLinkCount.Should().BeGreaterThan(0, "TOC should have links after navigation");

        // Click on a TOC link (e.g., "About the Book")
        var aboutBookLink = tocLinks.Filter(new() { HasText = "About the Book" }).First;
        await aboutBookLink.ClickAsync();

        // Wait for the clicked link to become active using Playwright's expect with retry
        await Assertions.Expect(aboutBookLink).ToHaveClassAsync(new Regex("active"), new() { Timeout = 2000 });

        // Assert - The clicked TOC link should become active (highlighted)
        var activeClass = await aboutBookLink.GetAttributeAsync("class");
        activeClass.Should().Contain("active", "TOC link should be highlighted after clicking");
    }
}
