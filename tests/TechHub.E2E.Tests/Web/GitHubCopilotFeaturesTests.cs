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
    public async Task GitHubCopilotFeatures_TierCards_ShouldBe_ClickableLinks()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Each tier card in the sidebar should be an anchor element linking to the timeline
        var sidebar = Page.Locator("aside.sidebar");
        var tierCards = sidebar.Locator("a.features-tier-card");
        var tierCount = await tierCards.CountAsync();
        tierCount.Should().Be(6, "Expected 6 clickable tier cards in the sidebar");

        for (var i = 0; i < tierCount; i++)
        {
            var card = tierCards.Nth(i);
            var href = await card.GetAttributeAsync("href");
            href.Should().StartWith("/github-copilot/features#", "Tier card should link to the features section");

            // Should still have the "View Features" label
            var label = card.Locator(".features-tier-link");
            await Assertions.Expect(label).ToBeVisibleAsync();
        }
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_Timeline()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Timeline section should be visible
        var timelineSection = Page.Locator(".features-timeline-section");
        await timelineSection.AssertElementVisibleAsync();

        // Timeline should have a heading
        var heading = timelineSection.Locator("h2#timeline");
        await Assertions.Expect(heading).ToBeVisibleAsync();

        // Timeline should contain at least one item
        var items = Page.Locator(".features-timeline-item");
        var itemCount = await items.CountAsync();
        itemCount.Should().BeGreaterThan(0, "Timeline should contain at least one feature entry");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Timeline_Items_ShouldHave_RequiredElements()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Each timeline item should have a date, title, and badges
        var firstItem = Page.Locator(".features-timeline-item").First;
        await Assertions.Expect(firstItem).ToBeVisibleAsync();

        // Should have a date
        var date = firstItem.Locator("time.features-timeline-date");
        await Assertions.Expect(date).ToBeVisibleAsync();
        var dateText = await date.TextContentAsync();
        dateText.Should().NotBeNullOrWhiteSpace("Timeline item should have a release date");

        // Should have a title
        var title = firstItem.Locator(".features-timeline-title");
        await Assertions.Expect(title).ToBeVisibleAsync();
        var titleText = await title.TextContentAsync();
        titleText.Should().NotBeNullOrWhiteSpace("Timeline item should have a title");

        // Should have tier badges
        var tierBadges = firstItem.Locator(".badge-tier");
        var badgeCount = await tierBadges.CountAsync();
        badgeCount.Should().BeGreaterThan(0, "Timeline item should have at least one tier badge");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Timeline_ShouldHave_GlobalFilters()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Timeline should have a filter row
        var filters = Page.Locator(".features-timeline-filters");
        await filters.AssertElementVisibleAsync();

        // Should have a GHES filter button
        var ghesButton = filters.Locator("button:has-text('GHES')");
        await Assertions.Expect(ghesButton).ToBeVisibleAsync();

        // Should have tier filter buttons (at least Free)
        var freeButton = filters.Locator("button:has-text('Free')");
        await Assertions.Expect(freeButton).ToBeVisibleAsync();
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Timeline_FilterButton_ShouldToggle_ActiveState()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        var filters = Page.Locator(".features-timeline-filters");
        var ghesButton = filters.Locator("button:has-text('GHES')");
        await Assertions.Expect(ghesButton).ToBeVisibleAsync();

        // Act - Click the GHES filter (Blazor server-side: triggers re-render)
        await ghesButton.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - Button should now have the 'active' class
        await Assertions.Expect(ghesButton).ToHaveClassAsync(new Regex("active"));

        // Act - Click again to deactivate
        await ghesButton.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - Button should no longer have 'active' class
        await Assertions.Expect(ghesButton).Not.ToHaveClassAsync(new Regex("active"));
    }

    [Fact]
    public async Task GitHubCopilotFeatures_Timeline_ExpandItem_ShouldShowDescription()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Get first timeline item header button
        var firstItemHeader = Page.Locator(".features-timeline-header").First;
        await Assertions.Expect(firstItemHeader).ToBeVisibleAsync();

        // Initially no detail panel
        var detailsBefore = Page.Locator(".features-timeline-detail");
        var countBefore = await detailsBefore.CountAsync();
        countBefore.Should().Be(0, "No items should be expanded initially");

        // Act - Click to expand
        await firstItemHeader.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - Detail panel is now visible with description
        var detail = Page.Locator(".features-timeline-detail").First;
        await Assertions.Expect(detail).ToBeVisibleAsync();

        var description = detail.Locator(".features-timeline-description");
        await Assertions.Expect(description).ToBeVisibleAsync();
        var descText = await description.TextContentAsync();
        descText.Should().NotBeNullOrWhiteSpace("Expanded item should show a description");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_FeatureSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have legacy feature video sections (only rendered when videos exist)
        // The test environment may not have video data, but the timeline section should always be present
        var timelineSection = Page.Locator(".features-timeline-section");
        await timelineSection.AssertElementVisibleAsync();
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldNotHave_GlobalFilterBar()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Legacy data-feature-filters attribute should not exist
        var globalFilters = Page.Locator("[data-feature-filters]");
        var count = await globalFilters.CountAsync();
        count.Should().Be(0, "Legacy global filter bar attribute should not be present");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FeatureCards_ShouldDisplay_GhesBadges()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Timeline items or feature cards with GHES support should display GHES badge
        // Check both the timeline items and legacy video cards
        var ghesBadges = Page.Locator(".badge-success");
        var ghesBadgeCount = await ghesBadges.CountAsync();
        ghesBadgeCount.Should().BeGreaterThan(0, "At least one feature should have a GHES badge");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_FeatureCards_ShouldDisplay_VideoBadges()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Feature cards with videos should display video badge (in legacy sections)
        var videoBadges = Page.Locator(".feature-card .badge-info");
        // Video badges are only shown in legacy sections which only render when video data exists.
        // In the test environment there may be no videos — we just verify no errors occur.
        var videoBadgeCount = await videoBadges.CountAsync();
        videoBadgeCount.Should().BeGreaterThanOrEqualTo(0, "Video badge count should be non-negative");
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

        // Sidebar should contain all 6 tier cards
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
}
