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

        // Assert - Should have subscription tiers section
        var tiersSection = Page.Locator(".features-tiers");
        await tiersSection.AssertElementVisibleAsync();

        // Should have tier cards
        var tierCards = Page.Locator(".features-tier-card");
        var tierCardCount = await tierCards.CountAsync();
        tierCardCount.Should().BeGreaterThanOrEqualTo(4, "Expected at least 4 subscription tiers (Free, Pro, Business, Enterprise)");

        // Each tier card should have key elements
        var firstTier = tierCards.First;
        await Assertions.Expect(firstTier.Locator("h3")).ToBeVisibleAsync();  // Tier name
        await Assertions.Expect(firstTier.Locator(".features-tier-tagline")).ToBeVisibleAsync();  // Tagline
        await Assertions.Expect(firstTier.Locator("ul.features-tier-features")).ToBeVisibleAsync();  // Features list
    }

    [Fact]
    public async Task GitHubCopilotFeatures_SubscriptionTiers_ShouldDisplay_PricingInfo()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Paid tiers should have pricing information
        var proPricing = Page.Locator("#tier-pro .features-tier-price");
        await proPricing.AssertElementVisibleAsync();

        // Should have price amount and period
        var priceAmount = proPricing.Locator(".price-amount");
        await priceAmount.AssertElementVisibleAsync();
        var priceText = await priceAmount.TextContentAsync();
        priceText.Should().NotBeNullOrEmpty("Pro tier should have a price amount");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_ShouldDisplay_FeatureSections()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Should have feature sections (Free Features, Pro Features, Enterprise Features)
        var featureSections = Page.Locator(".features-video-section");
        var sectionCount = await featureSections.CountAsync();
        sectionCount.Should().BeGreaterThan(0, "Expected at least one feature section with videos");

        // Each section should have a heading
        var firstSection = featureSections.First;
        await Assertions.Expect(firstSection.Locator("h2")).ToBeVisibleAsync();
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

        // Should have links to pricing and plan details
        var links = intro.Locator(".custom-page-links a");
        var linkCount = await links.CountAsync();
        linkCount.Should().BeGreaterThanOrEqualTo(2, "Expected links to pricing and plan details");
    }

    [Fact]
    public async Task GitHubCopilotFeatures_TierCards_ShouldBe_VisuallyDistinct()
    {
        // Arrange
        await Page.GotoRelativeAsync(PageUrl);

        // Assert - Tier cards should be in a grid layout
        var tiersGrid = Page.Locator(".features-tiers-grid");
        await tiersGrid.AssertElementVisibleAsync();

        // Wait for CSS isolation styles to load before checking computed style
        // Use WaitForFunctionAsync with explicit timeout to ensure CSS is applied
        await Page.WaitForFunctionAsync(
            @"() => {
                const el = document.querySelector('.features-tiers-grid');
                if (!el) return false;
                const style = window.getComputedStyle(el);
                return style && style.display === 'grid';
            }");

        var gridDisplay = await tiersGrid.EvaluateAsync<string>("el => window.getComputedStyle(el).display");
        gridDisplay.Should().Be("grid", "Tier cards should use CSS Grid layout");
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
