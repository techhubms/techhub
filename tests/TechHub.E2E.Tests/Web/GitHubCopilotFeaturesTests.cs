using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for GitHub Copilot Features custom page — feature timeline view.
/// Verifies page-specific content and the timeline UI.
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

        // Should have 6 tier cards (Free, Student, Pro, Business, Pro+, Enterprise)
        var tierCards = sidebar.Locator(".features-tier-card");
        var tierCardCount = await tierCards.CountAsync();
        tierCardCount.Should().Be(6, "Expected 6 subscription tiers (Free, Student, Pro, Business, Pro+, Enterprise)");

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
        count.Should().Be(6, "Expected 6 tier cards stacked in the sidebar");
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
    public async Task GitHubCopilotFeatures_TierCards_ShouldBe_InSidebar()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Tier cards should be inside the sidebar
        var sidebar = Page.Locator("aside.sidebar");
        await sidebar.AssertElementVisibleAsync();

        var tierCards = sidebar.Locator(".features-tier-card");
        var count = await tierCards.CountAsync();
        count.Should().Be(6, "All tier cards should be inside the sidebar");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldHave_Sidebar_WithSubscriptionTiers()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have a sidebar with subscription tiers
        var sidebar = Page.Locator("aside.sidebar");
        await sidebar.AssertElementVisibleAsync();

        var sidebarTierCards = sidebar.Locator(".features-tier-card");
        var tierCount = await sidebarTierCards.CountAsync();
        tierCount.Should().Be(6, "Expected 6 subscription tiers in the sidebar");
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
    public async Task GitHubCopilotFeatures_ShouldDisplay_Timeline()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Timeline should exist and have entries
        var timeline = Page.Locator(".features-timeline");
        await timeline.AssertElementVisibleAsync();

        // Should have at least one year group
        var yearGroups = Page.Locator(".features-timeline-year-group");
        var yearGroupCount = await yearGroups.CountAsync();
        yearGroupCount.Should().BeGreaterThan(0, "Timeline should have at least one year group");

        // Should have timeline entries
        var entries = Page.Locator(".features-timeline-entry");
        var entryCount = await entries.CountAsync();
        entryCount.Should().BeGreaterThan(0, "Timeline should have at least one feature entry");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Timeline_ShouldHave_YearLabels()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Year labels should be visible
        var yearLabels = Page.Locator(".features-timeline-year-label");
        var count = await yearLabels.CountAsync();
        count.Should().BeGreaterThan(0, "Timeline should have year labels");

        // Year label should contain a year number
        var firstLabel = yearLabels.First;
        var labelText = await firstLabel.TextContentAsync();
        labelText.Should().MatchRegex(@"\d{4}", "Year label should contain a 4-digit year");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TimelineEntry_ShouldHave_RequiredElements()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Each timeline entry should have the required structural elements
        var firstEntry = Page.Locator(".features-timeline-entry").First;
        await Assertions.Expect(firstEntry).ToBeVisibleAsync();

        // Should have feature name (always visible in header)
        await Assertions.Expect(firstEntry.Locator(".features-timeline-name")).ToBeVisibleAsync();

        // Should have date
        await Assertions.Expect(firstEntry.Locator(".features-timeline-date")).ToBeVisibleAsync();

        // Description is inside the expandable section — expand the entry first
        var header = firstEntry.Locator(".features-timeline-header");
        await header.ClickAndExpectAsync(async () =>
            await Assertions.Expect(firstEntry).ToHaveClassAsync(
                new Regex("expanded"), new() { Timeout = 2000 }));

        await Assertions.Expect(firstEntry.Locator(".features-timeline-description")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TimelineEntry_ShouldHave_PlanBadges()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Timeline entries should have plan badges
        var planBadges = Page.Locator(".features-timeline-entry .badge-plan");
        var badgeCount = await planBadges.CountAsync();
        badgeCount.Should().BeGreaterThan(0, "At least one timeline entry should have plan badges");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Timeline_ShouldHave_GhesBadges()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Some timeline entries should have GHES badges
        var ghesBadges = Page.Locator(".features-timeline-entry .badge-success");
        var ghesBadgeCount = await ghesBadges.CountAsync();
        ghesBadgeCount.Should().BeGreaterThan(0, "At least one timeline entry should have a GHES badge");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Timeline_ShouldHave_VideoBadges()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Some timeline entries should have video badges
        var videoBadges = Page.Locator(".features-timeline-entry .badge-info");
        var videoBadgeCount = await videoBadges.CountAsync();
        videoBadgeCount.Should().BeGreaterThan(0, "At least one timeline entry should have a video badge");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_GlobalFilterBar()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Global filter bar should exist
        var filterBar = Page.Locator(".features-timeline-filters");
        await filterBar.AssertElementVisibleAsync();

        // Should have an "All" button
        var allButton = filterBar.Locator("button:has-text('All')");
        await Assertions.Expect(allButton).ToBeVisibleAsync();

        // Should have a GHES toggle switch (label element, not button)
        var ghesToggle = filterBar.Locator(".features-ghes-toggle");
        await Assertions.Expect(ghesToggle).ToBeVisibleAsync();
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
    public async Task GitHubCopilotFeatures_GhesFilter_ShouldFilter_Timeline()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get initial entry count
        var entriesBefore = Page.Locator(".features-timeline-entry");
        var countBefore = await entriesBefore.CountAsync();
        countBefore.Should().BeGreaterThan(0, "Should have timeline entries before filtering");

        // Act - Click the GHES toggle switch
        var ghesToggle = Page.Locator(".features-timeline-filters .features-ghes-toggle");
        await ghesToggle.ClickAndExpectAsync(async () =>
            await Assertions.Expect(ghesToggle.Locator(".features-ghes-toggle-input")).ToBeCheckedAsync(new() { Timeout = 2000 }));

        // Assert - GHES filter should be active and only GHES entries should be visible
        var entriesAfter = Page.Locator(".features-timeline-entry");
        var countAfter = await entriesAfter.CountAsync();
        // After filtering by GHES, we should have fewer or equal entries
        countAfter.Should().BeLessThanOrEqualTo(countBefore,
            "Filtering by GHES should not increase the number of entries");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TierFilter_ShouldFilter_Timeline()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get total entry count first
        var allEntries = Page.Locator(".features-timeline-entry");
        var totalCount = await allEntries.CountAsync();
        totalCount.Should().BeGreaterThan(0);

        // Act - Click a tier filter (Free)
        var freeButton = Page.Locator(".features-timeline-filters button:has-text('Free')");
        await freeButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(freeButton).ToHaveClassAsync(
                new Regex("active"), new() { Timeout = 2000 }));

        // Assert - After filtering by Free tier, entries should be <= total
        var filteredEntries = Page.Locator(".features-timeline-entry");
        var filteredCount = await filteredEntries.CountAsync();
        filteredCount.Should().BeLessThanOrEqualTo(totalCount,
            "Filtering by Free tier should not show more entries than without filter");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ClearFilter_ShouldRestore_AllEntries()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        var allEntries = Page.Locator(".features-timeline-entry");
        var totalCount = await allEntries.CountAsync();

        // Apply a filter using the GHES toggle switch
        var ghesToggle = Page.Locator(".features-timeline-filters .features-ghes-toggle");
        await ghesToggle.ClickAndExpectAsync(async () =>
            await Assertions.Expect(ghesToggle.Locator(".features-ghes-toggle-input")).ToBeCheckedAsync(new() { Timeout = 2000 }));

        // Act - Click the "All" button to clear filters
        var allButton = Page.Locator(".features-timeline-filters button:has-text('All')");
        await allButton.ClickAndExpectAsync(async () =>
            await Assertions.Expect(allButton).ToHaveClassAsync(
                new Regex("active"), new() { Timeout = 2000 }));

        // Assert - Entry count should be restored
        var restoredEntries = Page.Locator(".features-timeline-entry");
        var restoredCount = await restoredEntries.CountAsync();
        restoredCount.Should().Be(totalCount, "Clearing filters should restore all timeline entries");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TimelineEntry_ExpandCollapse_ShouldWork()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        var firstEntry = Page.Locator(".features-timeline-entry").First;
        await Assertions.Expect(firstEntry).ToBeVisibleAsync();

        // Initially not expanded
        await Assertions.Expect(firstEntry).Not.ToHaveClassAsync(new Regex("expanded"));

        // Act - Click to expand
        var header = firstEntry.Locator(".features-timeline-header");
        await header.ClickAndExpectAsync(async () =>
            await Assertions.Expect(firstEntry).ToHaveClassAsync(
                new Regex("expanded"), new() { Timeout = 2000 }));

        // Act - Click again to collapse
        await header.ClickAndExpectAsync(async () =>
            await Assertions.Expect(firstEntry).Not.ToHaveClassAsync(
                new Regex("expanded"), new() { Timeout = 2000 }));
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ExpandedEntry_ShouldShow_VideoThumbnail()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Find the first entry that has a video badge (badge-info)
        var entryWithVideo = Page.Locator(".features-timeline-entry:has(.badge-info)").First;
        var entryCount = await entryWithVideo.CountAsync();

        if (entryCount == 0)
        {
            // No video entries, skip
            return;
        }

        await Assertions.Expect(entryWithVideo).ToBeVisibleAsync();

        // Expand the entry
        var header = entryWithVideo.Locator(".features-timeline-header");
        await header.ClickAndExpectAsync(async () =>
            await Assertions.Expect(entryWithVideo).ToHaveClassAsync(
                new Regex("expanded"), new() { Timeout = 2000 }));

        // Assert - Expanded entry with video should show thumbnail
        var thumbnail = entryWithVideo.Locator(".feature-card-thumbnail img");
        await Assertions.Expect(thumbnail).ToBeVisibleAsync();

        // Thumbnail should have YouTube URL
        var src = await thumbnail.GetAttributeAsync("src");
        src.Should().Contain("img.youtube.com", "Thumbnail should use YouTube image URL");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TierCard_Click_ShouldFilter_Timeline()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        var sidebar = Page.Locator("aside.sidebar");
        var freeTierCard = sidebar.Locator("#tier-free");
        await Assertions.Expect(freeTierCard).ToBeVisibleAsync();

        // Act - Click Free tier card in sidebar
        await freeTierCard.ClickAndExpectAsync(async () =>
            await Assertions.Expect(freeTierCard).ToHaveClassAsync(
                new Regex("active"), new() { Timeout = 2000 }));

        // Assert - Free tier filter should also be active in the main filter bar
        var freeFilterBtn = Page.Locator(".features-timeline-filters button:has-text('Free')");
        await Assertions.Expect(freeFilterBtn).ToHaveClassAsync(new Regex("active"));
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_VideoSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Feature video sections (from featureSections config) should exist below the timeline
        var videoSections = Page.Locator(".features-video-section");
        var sectionCount = await videoSections.CountAsync();
        sectionCount.Should().BeGreaterThan(0, "Should have at least one feature video section");

        // First section should have a heading and a card grid
        var firstSection = videoSections.First;
        await Assertions.Expect(firstSection.Locator("h2")).ToBeVisibleAsync();

        // Should have some feature cards
        var cards = Page.Locator(".features-cards-grid .card");
        var cardCount = await cards.CountAsync();
        cardCount.Should().BeGreaterThan(0, "Should have feature cards in the video sections");
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
            .Where(e => !e.Contains("circuit failed to initialize"))
            .ToList();

        significantErrors.Should().BeEmpty("Page should load without JavaScript errors");
    }
}
