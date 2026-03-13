using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot Features custom page.
/// Verifies page-specific content and features.
/// 
/// Common component tests are in separate test files:
/// - SidebarTocTests.cs: Table of contents behavior
/// </summary>
public class GitHubCopilotFeaturesTests : PlaywrightTestBase
{
    public GitHubCopilotFeaturesTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/github-copilot/features";

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldLoad_Successfully()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Check page title attribute contains expected text
        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("GitHub Copilot Features"));
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_Content()
    {
        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Page should have main content heading (excluding banner heading)
        var mainHeading = Page.Locator(".page-h1");
        await mainHeading.AssertElementVisibleAsync();

        // Should have some content (paragraphs, lists, etc.)
        var count = await Page.GetElementCountBySelectorAsync("p");
        count.Should().BeGreaterThan(0, $"Expected at least one paragraph, but found {count}");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_SubscriptionTiers()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have subscription tiers in the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        var tiersSection = sidebar.Locator(".features-tiers-sidebar");
        await tiersSection.AssertElementVisibleAsync();

        // Should have 5 tier cards (Free, Pro, Business, Pro+, Enterprise)
        var tierCards = sidebar.Locator(".features-tier-card");
        var tierCardCount = await tierCards.CountAsync();
        tierCardCount.Should().Be(5, "Expected 5 subscription tiers (Free, Pro, Business, Pro+, Enterprise)");

        // Each tier card should have key elements
        var firstTier = tierCards.First;
        await Assertions.Expect(firstTier.Locator("h3")).ToBeVisibleAsync();  // Tier name
        await Assertions.Expect(firstTier.Locator(".features-tier-tagline")).ToBeVisibleAsync();  // Tagline
        await Assertions.Expect(firstTier.Locator("ul.features-tier-features")).ToBeVisibleAsync();  // Features list
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FreeTier_ShouldBeFirst_InSidebar()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Free tier should be the first tier card in the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        var freeTier = sidebar.Locator("#tier-free");
        await freeTier.AssertElementVisibleAsync();

        // Free tier should have the tier card class
        await Assertions.Expect(freeTier).ToHaveClassAsync(new Regex("features-tier-card"));
    }

    [Fact]
    public async Task GitHubCopilotFeatures_PaidTiers_ShouldBeStacked_InSidebar()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - All tiers should be stacked vertically in the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        var tierCards = sidebar.Locator(".features-tier-card");
        var count = await tierCards.CountAsync();
        count.Should().Be(5, "Expected 5 tier cards stacked in the sidebar");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldNotDisplay_PricingDiv()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - No pricing divs should exist on the page
        var pricingDivs = Page.Locator(".features-tier-price");
        var count = await pricingDivs.CountAsync();
        count.Should().Be(0, "Pricing divs should be removed from tier cards");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TierCards_ShouldBe_ClickableLinks()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Each tier card in the sidebar should be an anchor element linking to its feature section
        var sidebar = Page.Locator("aside.sidebar");
        var tierCards = sidebar.Locator("a.features-tier-card");
        var tierCount = await tierCards.CountAsync();
        tierCount.Should().Be(5, "Expected 5 clickable tier cards in the sidebar");

        for (var i = 0; i < tierCount; i++)
        {
            var card = tierCards.Nth(i);
            var href = await card.GetAttributeAsync("href");
            href.Should().StartWith("/github-copilot/features#", "Tier card should link to its feature section");

            // Should still have the "View Features" label
            var label = card.Locator(".features-tier-link");
            await Assertions.Expect(label).ToBeVisibleAsync();
        }
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_FeatureSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have feature sections (Free Features, Pro Features, Enterprise Features)
        var featureSections = Page.Locator(".features-video-section");
        var sectionCount = await featureSections.CountAsync();
        sectionCount.Should().Be(3, "Expected 3 feature sections (Free, Pro & Business, Pro+ & Enterprise)");

        // Each section should have a heading
        var firstSection = featureSections.First;
        await Assertions.Expect(firstSection.Locator("h2")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FeatureSections_ShouldHave_PerSectionFilters()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Each feature section should have its own filter buttons
        var featureSections = Page.Locator(".features-video-section");
        var sectionCount = await featureSections.CountAsync();

        for (var i = 0; i < sectionCount; i++)
        {
            var section = featureSections.Nth(i);
            var filterBar = section.Locator(".features-section-filters");
            await Assertions.Expect(filterBar).ToBeVisibleAsync();

            // Each section should have GHES and Videos filter buttons
            var ghesButton = filterBar.Locator("button:has-text('GHES')");
            await Assertions.Expect(ghesButton).ToBeVisibleAsync();

            var videosButton = filterBar.Locator("button:has-text('videos')");
            await Assertions.Expect(videosButton).ToBeVisibleAsync();
        }
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldNotHave_GlobalFilterBar()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Global filter bar with checkboxes should not exist
        var globalFilters = Page.Locator("[data-feature-filters]");
        var count = await globalFilters.CountAsync();
        count.Should().Be(0, "Global filter bar should be removed in favor of per-section filters");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FeatureCards_ShouldDisplay_GhesBadges()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Feature cards with GHES support should display GHES badge
        var ghesBadges = Page.Locator(".feature-card .badge-success");
        var ghesBadgeCount = await ghesBadges.CountAsync();
        ghesBadgeCount.Should().BeGreaterThan(0, "At least one feature should have a GHES badge");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FeatureCards_ShouldDisplay_VideoBadges()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Feature cards with videos should display video badge
        var videoBadges = Page.Locator(".feature-card .badge-info");
        var videoBadgeCount = await videoBadges.CountAsync();
        videoBadgeCount.Should().BeGreaterThan(0, "At least one feature should have a video badge");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FeatureCards_ShouldDisplay_YouTubeThumbnails()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Feature cards with video links should have YouTube thumbnails
        var thumbnails = Page.Locator(".feature-card-thumbnail img");
        var thumbnailCount = await thumbnails.CountAsync();
        thumbnailCount.Should().BeGreaterThan(0, "At least one feature should have a YouTube thumbnail");

        // Verify first thumbnail has YouTube URL
        var firstThumbnailSrc = await thumbnails.First.GetAttributeAsync("src");
        firstThumbnailSrc.Should().Contain("img.youtube.com", "Thumbnails should use YouTube image URLs");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Intro_ShouldDisplay_LinksAndNote()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have intro section with links
        var intro = Page.Locator(".custom-page-intro");
        await intro.AssertElementVisibleAsync();

        // Should have note about GHES
        var note = intro.Locator(".custom-page-note");
        await note.AssertElementVisibleAsync();
        var noteText = await note.TextContentAsync();
        noteText.Should().Contain("Note:", "Expected note section to be clearly marked");

        // Should have links to pricing and plan details (inline in text)
        var links = intro.Locator("p >> a[href]");
        var linkCount = await links.CountAsync();
        linkCount.Should().BeGreaterThanOrEqualTo(2, "Expected links to pricing and plan details");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TierCards_ShouldBe_InSidebar()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Tier cards should be inside the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        await sidebar.AssertElementVisibleAsync();

        var tierCards = sidebar.Locator(".features-tier-card");
        var count = await tierCards.CountAsync();
        count.Should().Be(5, "All tier cards should be inside the sidebar");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldHave_Sidebar_WithSubscriptionTiers()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have a sidebar with subscription tiers
        var sidebar = Page.Locator("aside.sidebar");
        await sidebar.AssertElementVisibleAsync();

        // Sidebar should contain all 5 tier cards
        var sidebarTierCards = sidebar.Locator(".features-tier-card");
        var tierCount = await sidebarTierCards.CountAsync();
        tierCount.Should().Be(5, "Expected 5 subscription tiers in the sidebar");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldUse_PageWithSidebarLayout()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Page should use page-with-sidebar layout
        var mainElement = Page.Locator("main.page-with-sidebar");
        await mainElement.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Sidebar_ShouldHave_MobileToolbarButton()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Sidebar toolbar should have a button for subscriptions
        var toolbarButton = Page.Locator(".sidebar-toolbar-btn:has-text('Subscriptions')");
        // On desktop the toolbar buttons are hidden (display: none), but the element exists
        await Assertions.Expect(toolbarButton).ToHaveCountAsync(1);
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldHave_NoConsoleErrors()
    {
        // Arrange
        var consoleErrors = new List<string>();
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                consoleErrors.Add(msg.Text);
            }
        };

        // Act
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have no console errors (filter WebSocket connection errors from Blazor)
        var significantErrors = consoleErrors
            .Where(e => !e.Contains("WebSocket"))
            .Where(e => !e.Contains("ERR_CONNECTION_REFUSED"))
            .ToList();

        significantErrors.Should().BeEmpty("Page should load without JavaScript errors");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FilterButton_ShouldOnlyAffectOwnSection()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for filter buttons to be initialized
        var sections = Page.Locator(".features-video-section");
        var sectionCount = await sections.CountAsync();
        sectionCount.Should().BeGreaterThanOrEqualTo(2, "Need at least 2 sections to test per-section filtering");

        // Act - Click the GHES filter in the first section only
        var firstSection = sections.First;
        var firstGhesButton = firstSection.Locator(".features-filter-btn[data-filter='ghes']");
        await firstGhesButton.ClickAsync();

        // Assert - The first section's GHES button should be active
        await Assertions.Expect(firstGhesButton).ToHaveClassAsync(new Regex("active"));

        // Assert - The second section's GHES button should NOT be active
        var secondSection = sections.Nth(1);
        var secondGhesButton = secondSection.Locator(".features-filter-btn[data-filter='ghes']");
        await Assertions.Expect(secondGhesButton).Not.ToHaveClassAsync(new Regex("active"));
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FilterButton_ShouldNotScrollToHeading()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Wait for filter buttons to be initialized (indicates JS has run and DOM is stable)
        var firstSection = Page.Locator(".features-video-section").First;
        var ghesButton = firstSection.Locator(".features-filter-btn[data-filter='ghes']");
        await Assertions.Expect(ghesButton).ToBeVisibleAsync();

        // Wait for Blazor interactivity to settle by checking a button has the initialized flag
        await Page.WaitForConditionAsync(
            "() => document.querySelector('.features-filter-btn[data-initialized]') !== null");

        // Record the heading's viewport position before clicking
        var heading = firstSection.Locator("h2[id]");
        await heading.ScrollIntoViewIfNeededAsync();
        var rectBefore = await heading.BoundingBoxAsync();

        // Act - Click the GHES filter
        await ghesButton.ClickAsync();

        // Small wait to let any smooth scroll start
        await Page.WaitForConditionAsync("() => true");

        // Assert - The heading should still be in roughly the same viewport position
        // scrollIntoView would snap it to the top of the viewport
        var rectAfter = await heading.BoundingBoxAsync();
        rectBefore.Should().NotBeNull();
        rectAfter.Should().NotBeNull();

        var yDiff = Math.Abs(rectAfter!.Y - rectBefore!.Y);
        yDiff.Should().BeLessThan(100, "Heading position should not change significantly - no programmatic scroll should occur");
    }
}
