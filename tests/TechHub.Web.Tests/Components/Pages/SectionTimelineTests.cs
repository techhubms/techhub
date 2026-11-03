using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Models;
using TechHub.Web.Components;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// Tests for SectionTimeline.razor - the timeline page of a section (/{section}/timeline).
/// </summary>
public class SectionTimelineTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _mockApiClient = new();

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
            ])
        ]);
        Services.AddSingleton(sectionCache);

        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GaEntry()]);
        Services.AddSingleton(_mockApiClient.Object);

        AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));

        JSInterop.SetupVoid("markScriptsReady").SetVoidResult();
        JSInterop.SetupVoid("preventScrollOnExpand").SetVoidResult();
    }

    private static TimelineEntry GaEntry(string product = "GitHub Copilot", string? feature = "Agent mode", string kind = "ga", string precision = "day", DateOnly? date = null) => new()
    {
        Date = date ?? new DateOnly(2026, 3, 10),
        DatePrecision = precision,
        Kind = kind,
        Product = product,
        Feature = feature,
        Description = $"{kind} of {feature ?? product}",
        Plans = ["Pro"],
        Surfaces = ["Visual Studio Code"],
        Sources =
        [
            new TimelineEntrySource { CollectionName = "news", Slug = "agent-mode-ga", Title = "Agent mode is GA", ExternalUrl = "https://example.com/agent-mode", PrimarySectionName = "ai" },
            new TimelineEntrySource { CollectionName = "videos", Slug = "agent-mode-video", Title = "Agent mode video", PrimarySectionName = "ai" }
        ]
    };

    private IRenderedComponent<SectionTimeline> RenderPage(string query = "")
    {
        Services.GetRequiredService<NavigationManager>().NavigateTo($"/ai/timeline{query}");
        var cut = Render<SectionTimeline>(p => p.Add(x => x.SectionName, "ai"));
        cut.WaitForState(() => cut.FindAll(".timeline-entry").Count > 0 || cut.FindAll(".timeline-empty").Count > 0);
        return cut;
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

        cut.Find(".timeline-name").TextContent.Should().Be("GitHub Copilot: Agent mode");
        cut.Find(".timeline-entry-date").TextContent.Should().Be("10 Mar 2026");
        cut.Find(".timeline-badges .badge-success").TextContent.Should().Be("GA");
        cut.FindAll(".timeline-badges .badge-muted").Select(b => b.TextContent).Should().Equal("Pro", "Visual Studio Code");
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
    public void Render_ForAProductWithoutAFeature_UsesTheProductAsTheTitle()
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([GaEntry(product: "Azure Kubernetes Service", feature: null, kind: "retirement")]);

        var cut = RenderPage();

        cut.Find(".timeline-name").TextContent.Should().Be("Azure Kubernetes Service");
        cut.Find(".timeline-badges .badge-warning").TextContent.Should().Be("Retirement");
    }

    [Fact]
    public void Render_ForTheProductOfTheSection_LeavesTheProductOutOfTheTitle()
    {
        _mockApiClient
            .Setup(x => x.GetSectionTimelineAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<IReadOnlyList<string>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                GaEntry(product: "Artificial Intelligence", feature: "Agent mode", date: new DateOnly(2026, 3, 10)),
                GaEntry(product: "Artificial Intelligence", feature: null, date: new DateOnly(2026, 3, 9)),
                GaEntry(product: "Microsoft Foundry", feature: "Agent Service", date: new DateOnly(2026, 3, 8))
            ]);

        var cut = RenderPage();

        cut.FindAll(".timeline-name").Select(n => n.TextContent).Should().Equal(
            "Agent mode",
            "Artificial Intelligence",
            "Microsoft Foundry: Agent Service");
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
    }

    [Fact]
    public void Render_ByDefault_AsksForTheLastTwelveMonths()
    {
        RenderPage();

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(
            "ai",
            It.Is<DateOnly?>(d => d.HasValue && Math.Abs(d.Value.DayNumber - DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12)).DayNumber) <= 1),
            null,
            null,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_WithSinceAll_AsksForEveryDateAndOffersTheShorterWindow()
    {
        var cut = RenderPage("?since=all");

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync("ai", null, null, null, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".timeline-page-window a").TextContent.Should().Be("Show only the last 12 months");
        cut.Find(".timeline-page-window a").GetAttribute("href").Should().Be("/ai/timeline");
    }

    [Fact]
    public void Render_ByDefault_OffersEarlierDates()
    {
        var cut = RenderPage();

        cut.Find(".timeline-page-window a").TextContent.Should().Be("Show earlier dates");
        cut.Find(".timeline-page-window a").GetAttribute("href").Should().Be("/ai/timeline?since=all");
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
        cut.Find(".timeline-filter-link.active").TextContent.Should().Be("Deprecations");
    }

    [Fact]
    public void Render_WithInvalidKinds_IgnoresThem()
    {
        var cut = RenderPage("?kinds=nonsense,ga");

        _mockApiClient.Verify(x => x.GetSectionTimelineAsync(
            "ai", It.IsAny<DateOnly?>(), null, It.Is<IReadOnlyList<string>?>(k => k != null && k.SequenceEqual(new[] { "ga" })), It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll(".timeline-filter-link.active").Should().BeEmpty("a single kind is not one of the offered groups");
    }

    [Fact]
    public void Render_FilterLinks_KeepTheWindowAndOfferAllKinds()
    {
        var cut = RenderPage("?since=all");

        cut.FindAll(".timeline-filter-link").Select(a => (a.TextContent, a.GetAttribute("href"))).Should().Equal(
            ("All", "/ai/timeline?since=all"),
            ("Releases", "/ai/timeline?kinds=announced,preview,ga,launch,release&since=all"),
            ("Deprecations", "/ai/timeline?kinds=deprecation,retirement,end-of-support,breaking-change&since=all"),
            ("Other", "/ai/timeline?kinds=pricing-change,event&since=all"));
        cut.Find(".timeline-filter-link.active").TextContent.Should().Be("All");
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
}
