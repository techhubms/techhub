using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the section timeline page (/{section}/timeline): the dated events that the articles of a section state.
/// The events come from the database of the environment, so the tests that need an event skip when there is none.
/// </summary>
public class SectionTimelineTests : PlaywrightTestBase
{
    public SectionTimelineTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    private const string PageUrl = "/github-copilot/timeline";

    [Fact]
    public async Task SectionTimeline_ShouldLoad_WithTitleAndSidebarLayout()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Timeline - GitHub Copilot"));
        await Page.Locator("main.page-with-sidebar").AssertElementVisibleAsync();
        await Assertions.Expect(Page.Locator("h1.page-h1")).ToHaveTextAsync("Timeline");
    }

    [Fact]
    public async Task SectionTimeline_ShouldOffer_KindFiltersAndTheEarlierWindow()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        var filters = Page.Locator(".timeline-filter-link");
        await Assertions.Expect(filters).ToHaveCountAsync(4);
        await Assertions.Expect(filters.Nth(0)).ToHaveClassAsync(new Regex("active"));
        await Assertions.Expect(filters.Nth(2)).ToHaveTextAsync("Deprecations");
        await Assertions.Expect(Page.Locator(".timeline-page-window a")).ToHaveTextAsync("Show earlier dates");
    }

    [Fact]
    public async Task SectionTimeline_KindFilter_ShouldNavigateAndMarkItActive()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        await Page.Locator(".timeline-filter-link", new() { HasText = "Deprecations" }).ClickAsync();

        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@"/github-copilot/timeline\?kinds=deprecation"));
        await Assertions.Expect(Page.Locator(".timeline-filter-link.active")).ToHaveTextAsync("Deprecations");
        // Whatever the page shows now is a deprecation, retirement, end-of-support or breaking change.
        var badges = await Page.Locator(".timeline-entry .timeline-badges .badge-warning").CountAsync();
        var entries = await Page.Locator(".timeline-entry").CountAsync();
        badges.Should().Be(entries, "every entry of the Deprecations filter has a warning badge for its kind");
    }

    [Fact]
    public async Task SectionTimeline_ForGitHubCopilot_ShouldShowTheSubscriptionTiersInTheSidebar()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        var tiers = Page.Locator("aside.sidebar .features-tier-card");
        Assert.SkipWhen(await tiers.CountAsync() == 0, "No subscription tiers on this environment");

        await Assertions.Expect(Page.Locator("aside.sidebar .features-tiers-sidebar h2")).ToHaveTextAsync("Subscriptions");
        (await tiers.CountAsync()).Should().BeGreaterThan(1, "the sidebar lists every subscription tier");
    }

    [Fact]
    public async Task SectionTimeline_ForOtherSections_ShouldNotShowTheSubscriptionTiers()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync("/ai/timeline");

        await Assertions.Expect(Page.Locator(".features-tier-card")).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task OldFeaturesPage_ShouldRedirectToTheTimeline()
    {
        await Page.GotoRelativeAsync("/github-copilot/features");

        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@"/github-copilot/timeline$"));
    }

    [Fact]
    public async Task SectionTimeline_Entry_ShouldExpand_AndListItsSources()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync("/github-copilot/timeline?since=all");
        await Page.WaitForBlazorReadyAsync();

        var entries = Page.Locator(".timeline-entry");
        Assert.SkipWhen(await entries.CountAsync() == 0, "No timeline events found on this environment");

        var card = entries.First.Locator(".timeline-card");
        await card.Locator(".timeline-header").ClickAndExpectAsync(async () =>
            await Assertions.Expect(card).ToHaveAttributeAsync("aria-expanded", "true"));

        var sources = entries.First.Locator(".timeline-sources a[href]");
        (await sources.CountAsync()).Should().BeGreaterThan(0, "an expanded entry lists the articles that state it");
    }

    [Fact]
    public async Task SectionTimeline_ShouldBeLinked_FromTheSectionNavigation()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync("/github-copilot");

        var link = Page.Locator("a[href='/github-copilot/timeline']").First;
        await Assertions.Expect(link).ToBeAttachedAsync();
    }

    [Fact]
    public async Task SectionTimeline_ShouldHave_NoConsoleErrors()
    {
        await SkipUnlessTimelineIsOpenAsync();
        var consoleErrors = new List<string>();
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                consoleErrors.Add(msg.Text);
            }
        };

        await Page.GotoRelativeAsync(PageUrl);

        var significantErrors = consoleErrors
            .Where(e => !e.Contains("WebSocket"))
            .Where(e => !e.Contains("ERR_CONNECTION_REFUSED"))
            .Where(e => !e.Contains("circuit failed to initialize"))
            .ToList();

        significantErrors.Should().BeEmpty("Page should load without JavaScript errors");
    }

    // The timeline is behind a feature toggle: when it is not open to everyone, this anonymous browser must get a 404 and no buttons.
    // Both outcomes are valid for an environment, so the tests that need the page skip, and the two tests below hold either way.
    [Fact]
    public async Task SectionTimeline_PageAndNavigationButton_ShouldAgreeWithTheFeatureToggle()
    {
        var isOpen = await IsTimelineOpenAsync();

        await Page.GotoRelativeAsync("/github-copilot");
        await Page.WaitForBlazorReadyAsync();

        var link = Page.Locator("a[href='/github-copilot/timeline']");
        if (isOpen)
        {
            (await link.CountAsync()).Should().BeGreaterThan(0, "the feature is open to everyone, so the button is there");
        }
        else
        {
            (await link.CountAsync()).Should().Be(0, "the feature is not open to everyone, so a visitor sees no button");
        }
    }

    [Fact]
    public async Task SectionTimeline_WhenNotOpenToEveryone_ShouldAnswerWithNotFoundAndShowNothingAboutLoggingIn()
    {
        Assert.SkipWhen(await IsTimelineOpenAsync(), "The timeline is open to everyone on this environment");

        var response = await Page.APIGetAsync($"{BlazorHelpers.BaseUrl}{PageUrl}");
        response.Status.Should().Be(404);

        await Page.GotoRelativeAsync(PageUrl);
        await Assertions.Expect(Page.Locator("h1.page-h1")).ToHaveTextAsync("Page Not Found");
        (await Page.ContentAsync()).Should().NotContainEquivalentOf("log in").And.NotContainEquivalentOf("sign in");
    }

    private async Task<bool> IsTimelineOpenAsync()
    {
        var response = await Page.APIGetAsync($"{BlazorHelpers.BaseUrl}{PageUrl}");
        return response.Status == 200;
    }

    private async Task SkipUnlessTimelineIsOpenAsync() =>
        Assert.SkipUnless(await IsTimelineOpenAsync(), "The timeline is not open to everyone on this environment (feature toggle)");
}
