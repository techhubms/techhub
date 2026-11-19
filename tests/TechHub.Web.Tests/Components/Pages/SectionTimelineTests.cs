using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;
using TechHub.Web.Tests.TestSupport;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// Tests for SectionTimeline.razor - the timeline page of a section (/{section}/timeline).
/// </summary>
public class SectionTimelineTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _mockApiClient = new();
    private readonly FeatureGateSetup _features;

    public SectionTimelineTests()
    {
        var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        Services.AddSingleton<IHttpContextAccessor>(httpContextAccessor);
        Services.AddScoped<BrandingService>(_ => new BrandingService(httpContextAccessor, new ConfigurationBuilder().Build()));
        Services.AddSingleton<ILogger<SectionTimeline>>(Mock.Of<ILogger<SectionTimeline>>());
        Services.AddSingleton<ErrorService>();

        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new Section("ai", "Artificial Intelligence", "AI", "/ai", "AI",
            [
                new Collection("news", "News", "/ai/news", "News", "News"),
                new Collection("timeline", "Timeline", "/ai/timeline", "Timeline", "Timeline", isCustom: true)
            ]),
            new Section("github-copilot", "GitHub Copilot", "GitHub Copilot", "/github-copilot", "GHC",
            [
                new Collection("news", "News", "/github-copilot/news", "News", "News"),
                new Collection("timeline", "Timeline", "/github-copilot/timeline", "Timeline", "Timeline", isCustom: true)
            ])
        ]);
        Services.AddSingleton(sectionCache);
        _features = this.AddFeatureGate(isAdmin: false, (FeatureKeys.Timeline, FeatureToggleMode.Everyone));

        // Hidden events are requested separately from the visible window.
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, DateOnly? from, DateOnly? to, IReadOnlyList<string>? _, CancellationToken _) =>
                IsFutureCall(from, to) ? [] : [GaEntry()]);
        Services.AddSingleton(_mockApiClient.Object);

        AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));

        JSInterop.SetupVoid("markScriptsReady").SetVoidResult();
        JSInterop.SetupVoid("preventScrollOnExpand").SetVoidResult();
    }

    private static TimelineEntry GaEntry(string product = "GitHub Copilot", string? feature = "Agent mode", string kind = "ga", string precision = "day", DateOnly? date = null, string? title = "Agent mode generally available") => new()
    {
        Date = date ?? new DateOnly(2026, 3, 10),
        DatePrecision = precision,
        Kind = kind,
        Product = product,
        Feature = feature,
        Title = title,
        Description = $"{kind} of {feature ?? product}",
        Sources =
        [
            new TimelineEntrySource { CollectionName = "news", Slug = "agent-mode-ga", Title = "Agent mode is GA", ExternalUrl = "https://example.com/agent-mode", PrimarySectionName = "ai" },
            new TimelineEntrySource { CollectionName = "videos", Slug = "agent-mode-video", Title = "Agent mode video", PrimarySectionName = "ai" }
        ]
    };

    private static bool IsFutureCall(DateOnly? from, DateOnly? to) =>
        to is null && from.HasValue && from.Value > Today();

    private static DateOnly Today() =>
        DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels")).DateTime);

    private static TimelineEntry FutureEntry(int index) =>
        GaEntry(feature: $"Future {index}", date: Today().AddDays(30 + index));

    private void SetupFutureEntries(int count) =>
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, DateOnly? from, DateOnly? to, IReadOnlyList<string>? _, CancellationToken _) =>
                new[] { GaEntry() }.Concat(Enumerable.Range(0, count).Select(FutureEntry))
                    .Where(e => (!from.HasValue || e.Date >= from.Value) && (!to.HasValue || e.Date <= to.Value))
                    .OrderByDescending(e => e.Date)
                    .ToList());

    private IRenderedComponent<SectionTimeline> RenderPage(string query = "")
    {
        Services.GetRequiredService<NavigationManager>().NavigateTo($"/ai/timeline{query}");
        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "ai"));
        cut.WaitForState(() => cut.FindAll(".timeline-entry").Count > 0 || cut.FindAll(".timeline-empty").Count > 0);
        return cut;
    }

    private IRenderedComponent<SectionTimeline> RenderGitHubCopilotPage()
    {
        Services.GetRequiredService<NavigationManager>().NavigateTo("/github-copilot/timeline");
        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "github-copilot"));
        cut.WaitForState(() => cut.FindAll(".timeline-entry").Count > 0 || cut.FindAll(".timeline-empty").Count > 0);
        return cut;
    }

    [Fact]
    public void Render_ForGitHubCopilot_DoesNotLoadOrShowTheSubscriptionTiers()
    {
        // The subscriptions belong to the features page, not to the timeline
        var cut = RenderGitHubCopilotPage();

        cut.FindAll(".features-tiers-sidebar").Should().BeEmpty();
        cut.FindAll(".timeline-entry").Should().NotBeEmpty();
        _mockApiClient.Verify(x => x.GetFeaturesDataAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_UsesThePageWithSidebarLayoutAndShowsTheTitle()
    {
        var cut = RenderPage();

        cut.Find("main").ClassList.Should().Contain("page-with-sidebar");
        cut.Find("h1").TextContent.Should().Be("Timeline");
    }

    [Fact]
    public void Render_ShowsAnEntryAsACard()
    {
        var cut = RenderPage();

        cut.Find(".timeline-name").TextContent.Should().Be("Agent mode generally available");
        cut.Find(".timeline-entry-date").TextContent.Should().Be("10 Mar 2026");
        cut.Find(".timeline-badges .badge-success").TextContent.Should().Be("GA");
        cut.Find(".timeline-description").TextContent.Should().Be("ga of Agent mode");
    }

    [Theory]
    [InlineData("day", "10 Mar 2026")]
    [InlineData("month", "Mar 2026")]
    [InlineData("quarter", "Q1 2026")]
    public void Render_ShowsTheDateAsPreciselyAsTheArticleStatesIt(string precision, string expected)
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GaEntry(precision: precision, date: new DateOnly(2026, 3, precision == "day" ? 10 : 1))]);

        var cut = RenderPage();

        cut.Find(".timeline-entry-date").TextContent.Should().Be(expected);
    }

    [Fact]
    public void Render_EventWithATitle_UsesItAsTheCardTitleAndShowsTheProductAsABadge()
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GaEntry("Azure App Service", "Runtimes", kind: "end-of-support", title: "Java 8, 11 and 17 support ends")]);

        var cut = RenderPage();

        cut.Find(".timeline-name").TextContent.Should().Be("Java 8, 11 and 17 support ends");
        cut.Find(".timeline-badges .badge-tag").TextContent.Should().Be("Azure App Service");
        cut.Find(".timeline-badges .badge-warning").TextContent.Should().Be("End of support");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void Render_EventWithoutATitle_ShowsNoTitleButStillTheProductBadge(string? title)
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GaEntry("Azure App Service", "Runtimes", title: title)]);

        var cut = RenderPage();

        cut.Find(".timeline-name").TextContent.Trim().Should().BeEmpty();
        cut.Find(".timeline-badges .badge-tag").TextContent.Should().Be("Azure App Service");
    }

    [Fact]
    public void Render_Title_IsShownAsReturned()
    {
        var title = "Azure App Service: " + new string('x', 200);
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GaEntry("Azure App Service", "Runtimes", title: title)]);

        var cut = RenderPage();

        cut.Find(".timeline-name").TextContent.Should().Be(title);
    }

    [Fact]
    public void Render_ForTheProductOfTheSection_StillShowsTheProductBadge()
    {
        // The default entry is about GitHub Copilot, which is the section of this page
        var cut = RenderGitHubCopilotPage();

        cut.Find(".timeline-badges .badge-tag").TextContent.Should().Be("GitHub Copilot");
        cut.Find(".timeline-badges .badge-success").TextContent.Should().Be("GA");
    }
    [Fact]
    public void ExpandingACard_ListsItsSources_ExternalOnesOpeningInANewTab()
    {
        var cut = RenderPage();

        cut.Find(".timeline-card").Click();

        cut.WaitForAssertion(() => cut.FindAll(".timeline-sources a").Should().HaveCount(2));
        var links = cut.FindAll(".timeline-sources a");
        links.Select(l => l.TextContent).Should().Equal("Agent mode is GA", "Agent mode video");
        links[0].GetAttribute("href").Should().Be("https://example.com/agent-mode");
        links[0].GetAttribute("target").Should().Be("_blank");
        links[0].GetAttribute("rel").Should().Be("noopener noreferrer");
        links[1].GetAttribute("href").Should().Be("/ai/videos/agent-mode-video");
        links[1].GetAttribute("target").Should().BeNull();
        links.Should().OnlyContain(l => l.HasAttribute("data-content-link"), "content-links.js opens them in a new tab on a desktop");
    }

    [Fact]
    public void Render_ByDefault_AsksForTheLastTwelveMonthsAndNextSixMonthsWithLaterEventsApart()
    {
        RenderPage();

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(
            "ai",
            It.Is<DateOnly?>(d => d == Today().AddMonths(-12)),
            It.Is<DateOnly?>(d => d == Today().AddMonths(6)),
            null,
            It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(
            "ai",
            It.Is<DateOnly?>(d => d == Today().AddMonths(6).AddDays(1)),
            null,
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_WithSinceAll_AsksForEarlierDatesWithoutChangingTheUpcomingWindow()
    {
        var cut = RenderPage("?since=all");

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync("ai", null, It.Is<DateOnly?>(d => d == Today().AddMonths(6)), null, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".timeline-page-window a").TextContent.Should().Be("Show only the last 12 months");
        cut.Find(".timeline-page-window a").GetAttribute("href").Should().Be("/ai/timeline");
    }

    [Fact]
    public void Render_WithFutureHidden_HidesFutureEventsAndOffersToShowThem()
    {
        SetupFutureEntries(3);

        var cut = RenderPage("?future=hide");

        cut.FindAll(".timeline-year-label").Select(l => l.TextContent).Should().Equal("Future events", "2026");
        var link = cut.Find(".timeline-notice a");
        link.TextContent.Should().Be("Show all future events (3 hidden)");
        link.GetAttribute("href").Should().Be("/ai/timeline?future=show");
        cut.FindAll(".timeline-entry").Should().ContainSingle("only the event of the past is shown");
    }

    [Fact]
    public void Render_WithOneHiddenFutureEvent_ShowsItsCount()
    {
        SetupFutureEntries(1);

        var cut = RenderPage("?future=hide");

        cut.Find(".timeline-notice a").TextContent.Should().Be("Show all future events (1 hidden)");
    }

    [Fact]
    public void Render_WithoutFutureEvents_ShowsNoNotice()
    {
        var cut = RenderPage();

        cut.FindAll(".timeline-notice").Should().BeEmpty();
        cut.FindAll(".timeline-year-label").Select(l => l.TextContent).Should().Equal("2026");
    }

    [Fact]
    public void Render_WithOnlyFutureEvents_ShowsTheNoticeInsteadOfTheEmptyMessage()
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, DateOnly? from, DateOnly? to, IReadOnlyList<string>? _, CancellationToken _) =>
                IsFutureCall(from, to) ? [FutureEntry(0), FutureEntry(1)] : []);

        Services.GetRequiredService<NavigationManager>().NavigateTo("/ai/timeline?future=hide");
        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "ai"));
        cut.WaitForState(() => cut.FindAll(".timeline-notice").Count > 0);

        cut.Find(".timeline-notice a").TextContent.Should().Be("Show all future events (2 hidden)");
        cut.FindAll(".timeline-empty").Should().BeEmpty();
        cut.FindAll(".timeline-entry").Should().BeEmpty();
    }

    [Fact]
    public void Render_WithFutureShown_AsksOnceWithoutEndDateAndShowsAllEventsLikeBefore()
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([FutureEntry(0), GaEntry()]);

        var cut = RenderPage("?future=show");

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync("ai", It.IsAny<DateOnly?>(), null, null, It.IsAny<CancellationToken>()), Times.Once);
        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll(".timeline-notice").Should().BeEmpty();
        cut.FindAll(".timeline-entry").Should().HaveCount(2, "the future event is shown with the others");
    }

    [Fact]
    public void Render_FutureFilter_OffersAllThreeOptionsAndKeepsOtherFilters()
    {
        SetupFutureEntries(200);
        var cut = RenderPage("?kinds=pricing-change,event&since=all");

        var links = cut.FindAll("nav[aria-label='Future events'] .timeline-filter-link");
        links.Select(a => (a.TextContent, a.GetAttribute("href"))).Should().Equal(
            ("Next 6 months", "/ai/timeline?kinds=pricing-change,event&since=all"),
            ("All future events", "/ai/timeline?kinds=pricing-change,event&since=all&future=show"),
            ("Hide future events", "/ai/timeline?kinds=pricing-change,event&since=all&future=hide"));
        links[0].ClassList.Should().Contain("active");
        links[1].ClassList.Should().NotContain("active");
    }

    [Fact]
    public void Render_WithFutureShown_MarksShowActiveAndKeepsItInTheOtherLinks()
    {
        var cut = RenderPage("?future=show");

        cut.Find("nav[aria-label='Future events'] .timeline-filter-link.active").TextContent.Should().Be("All future events");
        cut.Find(".timeline-page-window a").GetAttribute("href").Should().Be("/ai/timeline?since=all&future=show");
        cut.FindAll("nav[aria-label='Kinds of dates'] .timeline-filter-link").Select(a => a.GetAttribute("href")).Should().Equal(
            "/ai/timeline?future=show",
            "/ai/timeline?kinds=announced,preview,ga,launch,release&future=show",
            "/ai/timeline?kinds=deprecation,retirement,end-of-support,breaking-change&future=show",
            "/ai/timeline?kinds=pricing-change,event&future=show");
    }

    [Fact]
    public void Render_ByDefault_OffersEarlierDates()
    {
        var cut = RenderPage();

        cut.Find(".timeline-page-window a").TextContent.Should().Be("Show earlier dates");
        cut.Find(".timeline-page-window a").GetAttribute("href").Should().Be("/ai/timeline?since=all");
    }

    [Theory]
    [InlineData("")]
    [InlineData("?future=upcoming")]
    [InlineData("?future=invalid")]
    public void Render_DefaultWindow_ShowsUpcomingThroughSixMonthsButNotTheDayAfter(string query)
    {
        // Arrange
        var cutoff = Today().AddMonths(6);
        var entries = new[]
        {
            GaEntry(date: cutoff.AddDays(1), title: "Later retirement"),
            GaEntry(date: cutoff, title: "Upcoming retirement"),
            GaEntry(date: Today().AddDays(1), title: "Tomorrow"),
            GaEntry(date: Today(), title: "Today"),
            GaEntry()
        };
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, DateOnly? from, DateOnly? to, IReadOnlyList<string>? _, CancellationToken _) =>
                entries.Where(e => (!from.HasValue || e.Date >= from.Value) && (!to.HasValue || e.Date <= to.Value)).ToList());

        // Act
        var cut = RenderPage(query);

        // Assert
        cut.FindAll(".timeline-name").Select(e => e.TextContent).Should().Equal("Upcoming retirement", "Tomorrow", "Today", "Agent mode generally available");
        cut.FindAll(".timeline-badges .badge-info").Count(e => e.TextContent == "Upcoming").Should().Be(2);
        cut.Find(".timeline-notice a").TextContent.Should().Be("Show all future events (1 beyond the next 6 months)");
        cut.Find("nav[aria-label='Future events'] .active").TextContent.Should().Be("Next 6 months");
    }

    [Fact]
    public void Render_WithoutLaterEvents_KeepsAllThreeOptionsVisible()
    {
        // Arrange
        SetupFutureEntries(2);

        // Act
        var cut = RenderPage();

        // Assert
        cut.FindAll(".timeline-entry").Should().HaveCount(3);
        cut.FindAll(".timeline-notice").Should().BeEmpty();
        cut.FindAll("nav[aria-label='Future events'] a").Select(a => a.TextContent)
            .Should().Equal("Next 6 months", "All future events", "Hide future events");
    }

    [Fact]
    public async Task Render_WhileSwitchingFromHiddenToAll_KeepsResultsAndIgnoresASupersededResponse()
    {
        // Arrange
        SetupFutureEntries(2);
        var all = new TaskCompletionSource<IReadOnlyList<TimelineEntry>>(TaskCreationOptions.RunContinuationsAsynchronously);
        _mockApiClient.Setup(x => x.GetSectionTimelineAsync("ai",
            It.Is<DateOnly?>(date => date == Today().AddMonths(-12)), null, null, It.IsAny<CancellationToken>()))
            .Returns(all.Task);
        var cut = RenderPage("?future=hide");
        cut.Find(".timeline-card").Click();
        cut.WaitForAssertion(() => cut.FindAll(".timeline-sources a").Should().HaveCount(2));
        var navigation = Services.GetRequiredService<NavigationManager>();

        // Act
        await cut.InvokeAsync(() => navigation.NavigateTo("/ai/timeline?future=show"));

        // Assert
        cut.WaitForAssertion(() =>
        {
            cut.Find("[aria-busy='true']");
            cut.Find(".timeline-notice a").TextContent.Should().Be("Show all future events (2 hidden)");
            cut.FindAll(".timeline-entry").Should().ContainSingle();
            cut.FindAll("nav[aria-label='Future events'] a").Should().HaveCount(3);
        });

        await cut.InvokeAsync(() => navigation.NavigateTo("/ai/timeline"));
        cut.WaitForAssertion(() => cut.FindAll(".timeline-entry").Should().HaveCount(3));
        cut.Find(".timeline-entry.expanded .timeline-name").TextContent.Should().Be("Agent mode generally available");

        var renderCount = cut.RenderCount;
        all.SetResult([GaEntry(title: "Outdated response")]);
        cut.WaitForState(() => cut.RenderCount > renderCount);
        cut.FindAll(".timeline-entry").Should().HaveCount(3);
        cut.FindAll(".timeline-notice").Should().BeEmpty();
        cut.FindAll("[aria-busy='true']").Should().BeEmpty();
    }

    [Fact]
    public void Render_WithFutureHidden_KeepsItHiddenWhenChangingOtherFilters()
    {
        // Act
        var cut = RenderPage("?future=hide");

        // Assert
        _mockApiClient.Verify(x => x.GetSectionTimelineAsync("ai", It.IsAny<DateOnly?>(), It.Is<DateOnly?>(d => d == Today()), null, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find("nav[aria-label='Future events'] .active").TextContent.Should().Be("Hide future events");
        cut.Find(".timeline-page-window a").GetAttribute("href").Should().Be("/ai/timeline?since=all&future=hide");
        cut.FindAll("nav[aria-label='Kinds of dates'] a").Should().OnlyContain(a => a.GetAttribute("href")!.Contains("future=hide"));
    }

    [Fact]
    public void Render_WithMaximumHiddenEvents_IndicatesTheCountIsALowerBound()
    {
        // Arrange
        SetupFutureEntries(200);

        // Act
        var cut = RenderPage("?future=hide");

        // Assert
        cut.Find(".timeline-notice a").TextContent.Should().Be("Show all future events (200+ hidden)");
    }

    [Fact]
    public void Render_WithKinds_PassesThemOnAndMarksTheFilterActive()
    {
        var cut = RenderPage("?kinds=deprecation,retirement,end-of-support,breaking-change");

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(
            "ai",
            It.IsAny<DateOnly?>(),
            null,
            It.Is<IReadOnlyList<string>?>(k => k != null && k.SequenceEqual(new[] { "deprecation", "retirement", "end-of-support", "breaking-change" })),
            It.IsAny<CancellationToken>()), Times.Once);
        cut.Find("nav[aria-label='Kinds of dates'] .timeline-filter-link.active").TextContent.Should().Be("Deprecations");
    }

    [Fact]
    public void Render_WithInvalidKinds_IgnoresThem()
    {
        var cut = RenderPage("?kinds=nonsense,ga");

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(
            "ai", It.IsAny<DateOnly?>(), null, It.Is<IReadOnlyList<string>?>(k => k != null && k.SequenceEqual(new[] { "ga" })), It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll("nav[aria-label='Kinds of dates'] .timeline-filter-link.active").Should().BeEmpty("a single kind is not one of the offered groups");
    }

    [Fact]
    public void Render_FilterLinks_KeepTheWindowAndOfferAllKinds()
    {
        var cut = RenderPage("?since=all");

        cut.FindAll("nav[aria-label='Kinds of dates'] .timeline-filter-link").Select(a => (a.TextContent, a.GetAttribute("href"))).Should().Equal(
            ("All", "/ai/timeline?since=all"),
            ("Releases", "/ai/timeline?kinds=announced,preview,ga,launch,release&since=all"),
            ("Deprecations", "/ai/timeline?kinds=deprecation,retirement,end-of-support,breaking-change&since=all"),
            ("Other", "/ai/timeline?kinds=pricing-change,event&since=all"));
        cut.Find("nav[aria-label='Kinds of dates'] .timeline-filter-link.active").TextContent.Should().Be("All");
    }

    [Fact]
    public void Render_WithoutEntries_ShowsTheEmptyMessage()
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var cut = RenderPage();

        cut.Find(".timeline-empty").TextContent.Should().Be("No dates found for this selection.");
    }

    [Fact]
    public void Render_WhenTheApiReturnsTheMaximum_SaysThatOnlyTheNewestAreShown()
    {
        var entries = Enumerable.Range(0, 200).Select(i => GaEntry(feature: $"Feature {i}", date: new DateOnly(2026, 3, 1).AddDays(-i))).ToList();
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entries);

        var cut = RenderPage();

        cut.Find(".timeline-page-window span").TextContent.Should().Be("Showing the newest 200 dates.");
    }

    [Fact]
    public void Render_ForAnUnknownSection_ShowsNotFoundAndDoesNotCallTheApi()
    {
        Services.GetRequiredService<NavigationManager>().NavigateTo("/nothing/timeline");

        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "nothing"));

        cut.FindAll("main.page-with-sidebar").Should().BeEmpty();
        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(FeatureToggleMode.Disabled, false)]
    [InlineData(FeatureToggleMode.Disabled, true)]
    [InlineData(FeatureToggleMode.Admins, false)]
    public void Render_WhenTheFeatureIsNotAvailableToTheVisitor_ShowsNotFoundAndDoesNotCallTheApi(FeatureToggleMode mode, bool isAdmin)
    {
        // Arrange
        _features.SetToggles((FeatureKeys.Timeline, mode));
        if (isAdmin)
        {
            _features.Authorization.SetAuthorized("admin@example.com");
        }

        Services.GetRequiredService<NavigationManager>().NavigateTo("/ai/timeline");

        // Act
        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "ai"));

        // Assert
        cut.FindAll("main.page-with-sidebar").Should().BeEmpty();
        cut.Markup.Should().Contain("Page Not Found");
        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_WhenTheFeatureIsForAdminsAndTheVisitorIsAnAdmin_ShowsTheTimeline()
    {
        // Arrange
        _features.SetToggles((FeatureKeys.Timeline, FeatureToggleMode.Admins));
        _features.Authorization.SetAuthorized("admin@example.com");
        Services.GetRequiredService<NavigationManager>().NavigateTo("/ai/timeline");

        // Act
        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "ai"));

        // Assert
        cut.FindAll("main.page-with-sidebar").Should().HaveCount(1);
        cut.Markup.Should().NotContain("Page Not Found");
    }

    [Fact]
    public void Render_WhenTheFeatureIsForAdminsAndTheVisitorIsNot_ShowsNotFound()
    {
        // Arrange
        _features.SetToggles((FeatureKeys.Timeline, FeatureToggleMode.Admins));
        Services.GetRequiredService<NavigationManager>().NavigateTo("/ai/timeline");

        // Act
        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "ai"));

        // Assert
        cut.FindAll("main.page-with-sidebar").Should().BeEmpty();
        cut.Markup.Should().Contain("Page Not Found");
    }
}
