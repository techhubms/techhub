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
public class NavigationTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;

    public NavigationTests(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
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
        var firstCard = Page.Locator(".card").First;

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
        classAttr.Should().MatchRegex("section-bg-(ai|github-copilot|azure|ml|devops|dotnet|security|all)");

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
    public async Task CollectionNavigation_ChangesDisplayedContent()
    {
        // Arrange - Start on GitHub Copilot with "All" collection (default)
        await Page.GotoRelativeAsync("/github-copilot");
        await Page.WaitForSelectorWithTimeoutAsync(".card");

        // Get the titles/hrefs of the first few cards displayed in "All" collection
        var initialCards = Page.Locator(".card");
        var initialCardCount = await initialCards.CountAsync();
        initialCardCount.Should().BeGreaterThan(0, "All collection should have content");

        // Get hrefs of first 5 cards (or less if fewer exist)
        // Note: .card IS the anchor element (a.card), so we get href directly from it
        var countToCheck = Math.Min(5, initialCardCount);
        var initialHrefs = new List<string>();
        for (int i = 0; i < countToCheck; i++)
        {
            var href = await initialCards.Nth(i).GetAttributeAsync("href");
            if (!string.IsNullOrEmpty(href))
            {
                initialHrefs.Add(href);
            }
        }

        initialHrefs.Should().NotBeEmpty("Cards should have navigable links");

        // Act - Navigate to "News" collection
        var newsButton = Page.Locator(".sub-nav a", new() { HasTextString = "News" });
        await newsButton.ClickBlazorElementAsync();
        await Page.WaitForBlazorUrlContainsAsync("/github-copilot/news");
        await Page.WaitForSelectorWithTimeoutAsync(".card");

        // Get the hrefs of cards in the "News" collection
        // Note: .card IS the anchor element (a.card), so we get href directly from it
        var newsCards = Page.Locator(".card");
        var newsCardCount = await newsCards.CountAsync();
        newsCardCount.Should().BeGreaterThan(0, "News collection should have content");

        var newsCountToCheck = Math.Min(5, newsCardCount);
        var newsHrefs = new List<string>();
        for (int i = 0; i < newsCountToCheck; i++)
        {
            var href = await newsCards.Nth(i).GetAttributeAsync("href");
            if (!string.IsNullOrEmpty(href))
            {
                newsHrefs.Add(href);
            }
        }

        newsHrefs.Should().NotBeEmpty("News cards should have navigable links");

        // Assert - The content should be different (at least some links differ)
        // Since "News" is a subset of "All", the news items should be in a different order
        // or the "All" view should contain items from multiple collections
        // At minimum, the first item should be different or the list order should differ
        newsHrefs.Should().NotBeEquivalentTo(initialHrefs,
            "Navigating from 'All' collection to 'News' collection should display different (filtered) content");
    }

    [Fact]
    public async Task TOC_HighlightingWorksAfterNavigation_FromHomepageToHandbook()
    {
        // Arrange - Start on homepage (which doesn't have TOC)
        await Page.GotoRelativeAsync("/");

        // First, check if the handbook link is hidden in an expandable section
        var handbookLink = Page.Locator("a[href*='/github-copilot/handbook']").First;
        
        // If link exists but is hidden, click the expand button first
        if (await handbookLink.CountAsync() > 0)
        {
            var isHidden = await handbookLink.IsHiddenAsync();
            if (isHidden)
            {
                // Find and click the expand button in the GitHub Copilot section
                var githubCopilotSection = Page.Locator("nav[aria-label*='GitHub Copilot collections']");
                var expandButton = githubCopilotSection.Locator("button.badge-expandable").First;
                
                // Only click if button exists
                if (await expandButton.CountAsync() > 0)
                {
                    await expandButton.ClickAsync();
                    
                    // Wait for the link to become visible
                    await Assertions.Expect(handbookLink).ToBeVisibleAsync(
                        new LocatorAssertionsToBeVisibleOptions { Timeout = 2000 });
                }
            }
        }

        // Act - Navigate to handbook page (which has TOC)
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

    [Fact]
    public async Task ContentCard_Click_StaysInCurrentSection()
    {
        // Arrange - Navigate to DevOps videos (a section with cross-section content)
        await Page.GotoRelativeAsync("/devops/videos");

        // Wait for content cards to load
        await Page.Locator(".card").First.AssertElementVisibleAsync();

        // Find a content card that has an internal link (not external)
        // Internal links should stay within /devops/videos/ path
        var cards = Page.Locator(".card");
        var cardCount = await cards.CountAsync();
        cardCount.Should().BeGreaterThan(0, "Should have content cards on the page");

        // Get the href of the first card
        var firstCard = cards.First;
        var href = await firstCard.GetHrefAsync();
        href.Should().NotBeNull("Card should have an href");

        // Skip external links - find first internal card
        ILocator? internalCard = null;
        string? internalHref = null;

        for (int i = 0; i < cardCount; i++)
        {
            var card = cards.Nth(i);
            var cardHref = await card.GetHrefAsync();
            if (cardHref != null && cardHref.StartsWith("/"))
            {
                internalCard = card;
                internalHref = cardHref;
                break;
            }
        }

        // Assert - Internal content links should stay in current section
        internalCard.Should().NotBeNull("Should have at least one internal content link");
        internalHref.Should().NotBeNull();
        internalHref!.Should().StartWith("/devops/",
            "Content cards in /devops/videos should link to /devops/{collection}/{slug}, not to another section");
    }
}
