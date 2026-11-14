using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the GitHub Copilot features page (/github-copilot/features): the features of the catalog that have content,
/// shown as cards, with the subscriptions in the sidebar. The features come from the database of the environment, so these tests
/// need a database with features that have content (a restored production database has them) and fail without.
/// </summary>
public class GitHubCopilotFeaturesTests : PlaywrightTestBase
{
    public GitHubCopilotFeaturesTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/github-copilot/features";

    [Fact]
    public async Task Features_ShouldLoad_WithTitleAndSidebarLayout()
    {
        await Page.GotoRelativeAsync(PageUrl);

        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Features - Tech Hub"));
        await Page.Locator("main.page-with-sidebar").AssertElementVisibleAsync();
        await Assertions.Expect(Page.Locator("h1.page-h1")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Features_ShouldShowTheSubscriptionsInTheSidebar()
    {
        await Page.GotoRelativeAsync(PageUrl);

        var tiers = Page.Locator("aside.sidebar .features-tier-card");

        await Assertions.Expect(Page.Locator("aside.sidebar .features-tiers-sidebar h2")).ToHaveTextAsync("Subscriptions");
        (await tiers.CountAsync()).Should().BeGreaterThan(1, "the sidebar lists every subscription tier");
    }

    [Fact]
    public async Task Features_ShouldShowCardsInAGrid_NotATimeline()
    {
        await Page.GotoRelativeAsync(PageUrl);

        var cards = Page.Locator(".features-cards-grid .feature-card");

        await Assertions.Expect(cards.First).ToBeVisibleAsync();
        await Assertions.Expect(cards.First.Locator("h3 a")).ToHaveAttributeAsync("href", new Regex(".+"));
        await Assertions.Expect(Page.Locator(".timeline-entry")).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task Features_WithVideosFilter_ShouldOnlyKeepCardsWithAVideo()
    {
        await Page.GotoRelativeAsync(PageUrl);
        await Page.WaitForBlazorReadyAsync();

        var cards = Page.Locator(".feature-card");
        await Assertions.Expect(cards.First).ToBeVisibleAsync();

        var filter = Page.Locator("button[data-filter='videos']");
        await filter.ClickAndExpectAsync(async () =>
            await Assertions.Expect(filter).ToHaveAttributeAsync("aria-pressed", "true"));

        var withoutVideo = Page.Locator(".feature-card:not(:has(.badge-info))");
        await Assertions.Expect(withoutVideo).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task Features_ShouldBeLinked_FromTheSectionNavigation()
    {
        await Page.GotoRelativeAsync("/github-copilot");
        await Page.WaitForBlazorReadyAsync();

        (await Page.Locator($"a[href='{PageUrl}']").CountAsync()).Should().BeGreaterThan(0);
    }
}
