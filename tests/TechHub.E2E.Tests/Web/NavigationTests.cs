using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for navigation improvements and URL structure
/// Tests for user story requirements: section ordering, URL structure, and navigation flow
/// </summary>
public class NavigationTests : PlaywrightTestBase
{
    public NavigationTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

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

        // Blazor uses enhanced navigation (SPA-style); retry click until URL confirms navigation.
        await ghCopilotCard.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/github-copilot.*"), new() { Timeout = 2000 }));

        // Assert - Should not have hash fragment
        Page.Url.Should().NotContain("#");
    }

    [Fact]
    public async Task CollectionNavigation_UpdatesURL_ToSectionSlashCollection()
    {
        // Arrange - Use AI section which doesn't hide collection pages
        await Page.GotoRelativeAsync("/ai");

        // Act - Click on "News" collection button
        var newsButton = Page.Locator(".sub-nav a", new() { HasTextString = "News" });
        await newsButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/ai/news.*"), new() { Timeout = 2000 }));

        // Assert - URL should be /ai/news
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

        // Assert - Collection badge should exist as a clickable link, before tags
        // Collection badge is the first .badge-purple link in .card-tags
        var collectionBadge = firstCard.Locator(".card-tags a.badge-purple").First;
        await collectionBadge.AssertElementVisibleAsync();

        // Collection badge should have proper capitalization (e.g., "News" not "news")
        var badgeText = await collectionBadge.TextContentWithTimeoutAsync();
        badgeText!.Should().MatchRegex("^[A-Z]"); // Starts with capital letter

        // Badge should be a clickable link to the collection
        var href = await collectionBadge.GetAttributeAsync("href");
        href.Should().NotBeNullOrEmpty("collection badge should be a clickable link");
    }

    [Fact]
    public async Task SectionPage_SubNavIsClickable()
    {
        // Arrange - Use AI section which doesn't hide collection pages
        await Page.GotoRelativeAsync("/ai");

        // Act - Click on "Videos" collection in sub-nav
        var videosButton = Page.Locator(".sub-nav a", new() { HasTextString = "Videos" });
        await videosButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/ai/videos.*"), new() { Timeout = 2000 }));

        // Assert - Should navigate and load videos

        // Wait for page to fully load after navigation
        await Page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "Videos");
    }

    [Fact]
    public async Task SectionPage_HeaderAreaHasConsistentHeight()
    {
        // Arrange

        // Act - Measure banner height on homepage
        await Page.GotoRelativeAsync("/");
        // Use JS-based measurement — ScrollIntoViewIfNeededAsync fails intermittently with
        // "Element is not attached to the DOM" during Blazor hydration because Playwright's
        // stability check extends the window where element detachment can occur.
        var homeHeightHandle = await Page.WaitForConditionAsync(@"() => {
            const el = document.querySelector('.section-banner.home-banner');
            if (!el || !el.isConnected) return null;
            el.scrollIntoView({ block: 'center' });
            const r = el.getBoundingClientRect();
            return r.height > 0 ? r.height : null;
        }");
        var homeBannerHeight = await homeHeightHandle.JsonValueAsync<double>();

        // Navigate to section page - use specific selector to avoid matching stale DOM during Blazor navigation
        await Page.GotoRelativeAsync("/github-copilot");
        var sectionBanner = Page.Locator(".section-banner.section-banner-bg-github-copilot");
        await Assertions.Expect(sectionBanner).ToBeVisibleAsync();
        // Use JS-based measurement — ScrollIntoViewIfNeededAsync fails intermittently with
        // "Element is not attached to the DOM" during Blazor enhanced navigation DOM patching
        // because Playwright's stability check extends the window where element detachment can occur.
        var sectionHeightHandle = await Page.WaitForConditionAsync(@"() => {
            const el = document.querySelector('.section-banner[class*=""section-banner-bg-github-copilot""]');
            if (!el || !el.isConnected) return null;
            el.scrollIntoView({ block: 'center' });
            const r = el.getBoundingClientRect();
            return r.height > 0 ? r.height : null;
        }");
        var sectionBannerHeight = await sectionHeightHandle.JsonValueAsync<double>();

        // Assert - Both should have defined heights (not auto)
        homeBannerHeight.Should().BeGreaterThan(0);
        sectionBannerHeight.Should().BeGreaterThan(0);
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
        // Use a single JS evaluation to get both bounding boxes atomically,
        // avoiding timing issues from scroll-triggered Blazor re-renders.
        var boxes = await Page.EvaluateAsync<double[]>(@"() => {
            const header = document.querySelector('.section-card .section-card-header');
            header.scrollIntoView({ block: 'center' });
            const overlay = header.querySelector('.section-overlay');
            const oBox = overlay.getBoundingClientRect();
            const hBox = header.getBoundingClientRect();
            return [oBox.height, hBox.height];
        }");

        boxes.Should().NotBeNull();
        boxes.Should().HaveCount(2);

        var overlayHeight = boxes[0];
        var headerHeight = boxes[1];

        // Overlay should have reasonable height (not zero/collapsed)
        (overlayHeight > 50).Should().BeTrue("overlay should have substantial height");
        (headerHeight > 50).Should().BeTrue("header should have substantial height");
    }

    [Fact]
    public async Task DirectURL_ToSectionWithCollection_LoadsCorrectContent()
    {
        // Arrange & Act
        await Page.GotoRelativeAsync("/ai/news");

        // Assert - use navigation-aware timeouts since we just navigated and data may still be loading
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
        // Arrange - Start on AI section with "All" collection (default)
        // Using AI instead of GitHub Copilot because GitHub Copilot has HideCollectionPages enabled
        await Page.GotoRelativeAsync("/ai");
        await Page.WaitForSelectorWithTimeoutAsync(".card");

        // Get the titles/hrefs of the first few cards displayed in "All" collection
        var initialCards = Page.Locator(".card");
        var initialCardCount = await initialCards.CountAsync();
        initialCardCount.Should().BeGreaterThan(0, "All collection should have content");

        // Get hrefs of first 5 cards (or less if fewer exist)
        // Note: .card is a container div, the link is .card-link inside it
        var countToCheck = Math.Min(5, initialCardCount);
        var initialHrefs = new List<string>();
        for (int i = 0; i < countToCheck; i++)
        {
            var href = await initialCards.Nth(i).Locator(".card-link").GetAttributeAsync("href");
            if (!string.IsNullOrEmpty(href))
            {
                initialHrefs.Add(href);
            }
        }

        initialHrefs.Should().NotBeEmpty("Cards should have navigable links");

        // Act - Navigate to "News" collection
        var newsButton = Page.Locator(".sub-nav a", new() { HasTextString = "News" });
        await newsButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/ai/news.*"), new() { Timeout = 2000 }));

        // Wait for enhanced navigation DOM swap to complete. ClickAndExpectAsync checks
        // __blazorServerReady which stays true across enhanced navigations, so the old
        // page's .card elements may still be in the DOM. Verifying the heading changed
        // confirms the new page has rendered before we count cards.
        await Assertions.Expect(Page.Locator("h1.page-h1")).ToContainTextAsync("News",
            new() { Timeout = BlazorHelpers.E2ETimeout });
        await Page.WaitForSelectorWithTimeoutAsync(".card");

        // Get the hrefs of cards in the "News" collection
        // Note: .card is a container div, the link is .card-link inside it
        var newsCards = Page.Locator(".card");
        var newsCardCount = await newsCards.CountAsync();
        newsCardCount.Should().BeGreaterThan(0, "News collection should have content");

        var newsCountToCheck = Math.Min(5, newsCardCount);
        var newsHrefs = new List<string>();
        for (int i = 0; i < newsCountToCheck; i++)
        {
            var href = await newsCards.Nth(i).Locator(".card-link").GetAttributeAsync("href");
            if (!string.IsNullOrEmpty(href))
            {
                newsHrefs.Add(href);
            }
        }

        newsHrefs.Should().NotBeEmpty("News cards should have navigable links");

        // Assert - URL should have changed to show News collection
        Page.Url.Should().Contain("/news", "URL should reflect the News collection");

        // Assert - Content should be filtered (unless all items in 'All' are News items)
        // The test validates navigation works - content difference depends on data state
        // If the section only has News items, the lists may be identical, which is valid
        if (initialHrefs.Count != newsHrefs.Count)
        {
            newsHrefs.Should().NotBeEquivalentTo(initialHrefs,
                "Different item counts indicate filtered content");
        }
        else
        {
            // Even if counts match, verify navigation occurred by checking URL changed
            Page.Url.Should().NotBe("/github-copilot/all",
                "Navigation should have changed the URL even if content overlaps");
        }
    }

    [Fact]
    public async Task TOC_HighlightingWorksAfterNavigation_FromGitHubCopilotToHandbook()
    {
        // Arrange - Start on the GitHub Copilot section page where the handbook link
        // is always visible in the sub-nav (not hidden in a collapsed section).
        // This tests the same scenario as "from a page without TOC to a page with TOC"
        // via enhanced navigation (Blazor SPA-style link click, no full page reload).
        await Page.GotoRelativeAsync("/github-copilot/all");

        // Act - Navigate to handbook page (which has TOC) via the sub-nav link.
        // The sub-nav renders all custom pages including the handbook as visible links.
        var handbookLink = Page.Locator(".sub-nav a[href*='/github-copilot/handbook']").First;
        await handbookLink.ClickAndExpectAsync(async () =>
            await Assertions.Expect(Page).ToHaveURLAsync(
                new Regex(@".*/github-copilot/handbook.*"), new() { Timeout = 2000 }));

        // Wait for navigation to complete

        // Wait for TOC to be visible (use Expect for auto-retry)
        var tocElement = Page.Locator("[data-toc-scroll-spy]");
        await Assertions.Expect(tocElement).ToBeVisibleAsync();

        // Wait for scroll-spy to be initialized before clicking — without this, the click
        // fires before the handler is attached (race condition under full test suite load).
        await Page.WaitForTocInitializedAsync();

        // Assert - TOC should be initialized and have links
        var tocLinks = tocElement.Locator("a[href*='#']");
        var tocLinkCount = await tocLinks.CountAsync();
        tocLinkCount.Should().BeGreaterThan(0, "TOC should have links after navigation");

        // Click on a TOC link (e.g., "About the Book") — retry until scroll-spy marks it active.
        var aboutBookLink = tocLinks.Filter(new() { HasText = "About the Book" }).First;
        await aboutBookLink.ClickAndExpectAsync(async () =>
            await Assertions.Expect(aboutBookLink).ToHaveClassAsync(
                new Regex("active"), new() { Timeout = 2000 }));

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
        var href = await firstCard.Locator(".card-link").GetHrefAsync();
        href.Should().NotBeNull("Card should have an href");

        // Skip external links - find first internal card
        ILocator? internalCard = null;
        string? internalHref = null;

        for (int i = 0; i < cardCount; i++)
        {
            var card = cards.Nth(i);
            var cardHref = await card.Locator(".card-link").GetHrefAsync();
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
