using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Playwright;
using TechHub.Core.Models;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for the section timeline page (/{section}/timeline): the dated events that the articles of a section state.
/// The events come from the database of the environment. The timeline is behind a feature toggle: on a local environment
/// <see cref="TimelineToggleFixture"/> opens it for these tests and restores it afterwards, so they never skip for it.
/// Only against a remote deployment, where the toggle cannot be changed, the tests that need the open state skip when it is closed.
/// The closed state (HTTP 404, no mention of logging in) is covered in the Web tests (<c>FeatureGatedPageEndpointTests</c>), which can set any toggle.
/// </summary>
public class SectionTimelineTests : PlaywrightTestBase, IClassFixture<TimelineToggleFixture>
{
    private readonly TimelineToggleFixture _toggle;

    public SectionTimelineTests(PlaywrightCollectionFixture fixture, TimelineToggleFixture toggle) : base(fixture)
    {
        _toggle = toggle;
    }

    private const string PageUrl = "/github-copilot/timeline";

    private const string KindFilterLinks = "nav[aria-label='Kinds of dates'] .timeline-filter-link";
    private const string FutureFilterLinks = "nav[aria-label='Future events'] .timeline-filter-link";

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
    public async Task SectionTimeline_ShouldOffer_KindFiltersFutureFilterAndTheEarlierWindow()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        var filters = Page.Locator(KindFilterLinks);
        await Assertions.Expect(filters).ToHaveCountAsync(4);
        await Assertions.Expect(filters.Nth(0)).ToHaveClassAsync(new Regex("active"));
        await Assertions.Expect(filters.Nth(2)).ToHaveTextAsync("Deprecations");
        // Future events are hidden until asked for
        var future = Page.Locator(FutureFilterLinks);
        await Assertions.Expect(future).ToHaveCountAsync(2);
        await Assertions.Expect(future.Nth(0)).ToHaveTextAsync("Hide");
        await Assertions.Expect(future.Nth(0)).ToHaveClassAsync(new Regex("active"));
        await Assertions.Expect(future.Nth(1)).ToHaveTextAsync("Show");
        await Assertions.Expect(Page.Locator(".timeline-page-window a")).ToHaveTextAsync("Show earlier dates");
    }

    [Fact]
    public async Task SectionTimeline_FutureFilter_ShouldSwitchBetweenHidingAndShowingFutureEvents()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        await Page.Locator(FutureFilterLinks, new() { HasText = "Show" }).ClickAsync();

        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@"/github-copilot/timeline\?future=show"));
        await Assertions.Expect(Page.Locator($"{FutureFilterLinks}.active")).ToHaveTextAsync("Show");
        await Assertions.Expect(Page.Locator(".timeline-notice")).ToHaveCountAsync(0);

        await Page.Locator(FutureFilterLinks, new() { HasText = "Hide" }).ClickAsync();

        await Assertions.Expect(Page).Not.ToHaveURLAsync(new Regex("future=show"));
        await Assertions.Expect(Page.Locator($"{FutureFilterLinks}.active")).ToHaveTextAsync("Hide");
    }

    // The API is private in deployed environments; locally it supplies the number of events after today from the real database.
    [Fact]
    [Trait("Category", "DevEnvironment")]
    public async Task SectionTimeline_FutureEvents_ShouldBeHiddenByDefaultBehindALinkThatShowsThem()
    {
        // Arrange
        await SkipUnlessTimelineIsOpenAsync();
        var tomorrow = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels")).DateTime).AddDays(1);
        var apiBaseUrl = (Environment.GetEnvironmentVariable("E2E_API_URL") ?? "https://localhost:5001").TrimEnd('/');
        var response = await Page.APIGetAsync($"{apiBaseUrl}/api/sections/azure/timeline?from={tomorrow:yyyy-MM-dd}");
        response.Ok.Should().BeTrue("the real timeline API supplies the number of future events");
        var futureCount = JsonSerializer.Deserialize<TimelineEntry[]>(await response.TextAsync(), JsonSerializerOptions.Web)!.Length;
        futureCount.Should().BeGreaterThan(0, "the restored database must contain azure events after today");
        var expectedLink = $"Hiding {(futureCount >= 200 ? "200+" : futureCount.ToString(CultureInfo.InvariantCulture))} future {(futureCount == 1 ? "event" : "events")}, click here to show";

        // Act
        await Page.GotoRelativeAsync("/azure/timeline");

        // Assert: the heading and the link stand where the first year would be, and no entry of the future is shown
        await Assertions.Expect(Page.Locator(".timeline-year-label").First).ToHaveTextAsync("Future events");
        var link = Page.Locator(".timeline-notice a");
        await Assertions.Expect(link).ToHaveTextAsync(expectedLink);
        await Assertions.Expect(Page.Locator(".timeline-year-group").First.Locator(".timeline-entry")).ToHaveCountAsync(0);

        // Act: show them
        await link.ClickAsync();

        // Assert: the timeline is the one it was before, with the future years
        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@"/azure/timeline\?future=show"));
        await Assertions.Expect(Page.Locator(".timeline-notice")).ToHaveCountAsync(0);
        await Assertions.Expect(Page.Locator(".timeline-year-label").First).Not.ToHaveTextAsync("Future events");
        await Assertions.Expect(Page.Locator(".timeline-entry").First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task SectionTimeline_KindFilter_ShouldNavigateAndMarkItActive()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        await Page.Locator(KindFilterLinks, new() { HasText = "Deprecations" }).ClickAsync();

        await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@"/github-copilot/timeline\?kinds=deprecation"));
        await Assertions.Expect(Page.Locator($"{KindFilterLinks}.active")).ToHaveTextAsync("Deprecations");
        // Whatever the page shows now is a deprecation, retirement, end-of-support or breaking change. The active filter is marked
        // before the filtered entries arrive, so this waits (retries) until no entry without a warning badge for its kind is left.
        await Assertions.Expect(Page.Locator(".timeline-entry:not(:has(.timeline-badges .badge-warning))")).ToHaveCountAsync(0);
        // An empty page would pass the check above, so the filter must also leave entries
        await Assertions.Expect(Page.Locator(".timeline-entry").First).ToBeVisibleAsync();
    }

    [Fact]
    public async Task SectionTimeline_ForGitHubCopilot_ShouldNotShowTheSubscriptionTiers()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync(PageUrl);

        // The subscriptions are on the features page. Wait for the timeline itself first: an absence check passes on a page that has not rendered yet.
        await Assertions.Expect(Page.Locator(".timeline-entry").First).ToBeVisibleAsync();
        await Assertions.Expect(Page.Locator(".features-tier-card")).ToHaveCountAsync(0);
    }

    // The API is private in deployed environments; locally it supplies the expected values from the real database.
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "DevEnvironment")]
    public async Task SectionTimeline_Event_ShouldShowTheApiHeadlineAndItsProductBadge(bool isSectionProduct)
    {
        // Arrange
        await SkipUnlessTimelineIsOpenAsync();
        var apiBaseUrl = (Environment.GetEnvironmentVariable("E2E_API_URL") ?? "https://localhost:5001").TrimEnd('/');
        var response = await Page.APIGetAsync($"{apiBaseUrl}/api/sections/github-copilot/timeline");
        response.Ok.Should().BeTrue("the real timeline API supplies the expected headlines and products");
        var entries = JsonSerializer.Deserialize<TimelineEntry[]>(await response.TextAsync(), JsonSerializerOptions.Web);
        entries.Should().NotBeNullOrEmpty("the restored database must contain timeline events");

        var index = Array.FindIndex(entries!, entry =>
            !string.IsNullOrWhiteSpace(entry.Title)
            && entry.Product.Equals("GitHub Copilot", StringComparison.OrdinalIgnoreCase) == isSectionProduct);
        index.Should().BeGreaterThanOrEqualTo(0,
            "the restored database must contain a titled event for {0}",
            isSectionProduct ? "GitHub Copilot itself" : "another product in the GitHub Copilot section");
        var expected = entries![index];

        // Act
        // The API list has the events after today as well, so they are shown to make the position in the list the id of the card
        await Page.GotoRelativeAsync($"{PageUrl}?since=all&future=show");
        var card = Page.Locator($"#timeline-entry-{index.ToString(CultureInfo.InvariantCulture)}");

        // Assert
        await Assertions.Expect(card).ToBeVisibleAsync();
        await Assertions.Expect(card.Locator(".timeline-name")).ToHaveTextAsync(expected.Title!);
        // The product is a badge for every event, also on the timeline of that product itself
        await Assertions.Expect(card.Locator(".timeline-badges .badge-tag")).ToHaveTextAsync(expected.Product);
    }

    [Fact]
    public async Task SectionTimeline_Entry_ShouldExpand_AndListItsSources()
    {
        await SkipUnlessTimelineIsOpenAsync();
        await Page.GotoRelativeAsync("/github-copilot/timeline?since=all");
        await Page.WaitForBlazorReadyAsync();

        var entries = Page.Locator(".timeline-entry");
        await Assertions.Expect(entries.First).ToBeVisibleAsync();

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
    // This test holds for either state; it checks that the page and the button agree with whatever the toggle is.
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

    private async Task<bool> IsTimelineOpenAsync()
    {
        var response = await Page.APIGetAsync($"{BlazorHelpers.BaseUrl}{PageUrl}");
        return response.Status == 200;
    }

    // On a local environment the fixture opened the toggle, so a closed page is a failure. Only a remote deployment can skip.
    private async Task SkipUnlessTimelineIsOpenAsync()
    {
        var isOpen = await IsTimelineOpenAsync();
        if (_toggle.CanChangeToggle)
        {
            isOpen.Should().BeTrue("the fixture opened the timeline toggle for everyone");
            return;
        }

        Assert.SkipUnless(isOpen, "The timeline is not open to everyone on this remote environment and its toggle cannot be changed");
    }
}
