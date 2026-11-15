using Bunit;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Models;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// Tests for GitHubCopilotFeatures.razor - the cards of the features that have content, with the subscriptions next to them.
/// </summary>
public class GitHubCopilotFeaturesTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _mockApiClient = new();
    private readonly BunitPersistentComponentState _persistentState;

    public GitHubCopilotFeaturesTests()
    {
        var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        Services.AddSingleton<IHttpContextAccessor>(httpContextAccessor);
        Services.AddScoped<BrandingService>(_ => new BrandingService(httpContextAccessor, new ConfigurationBuilder().Build()));
        Services.AddSingleton<ILogger<GitHubCopilotFeatures>>(Mock.Of<ILogger<GitHubCopilotFeatures>>());
        Services.AddSingleton<ErrorService>();

        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new Section("github-copilot", "GitHub Copilot", "GitHub Copilot", "/github-copilot", "GitHub Copilot",
            [
                new Collection("features", "Features", "/github-copilot/features", "Features", "Features", isCustom: true)
            ])
        ]);
        Services.AddSingleton(sectionCache);

        _mockApiClient.Setup(x => x.GetFeaturesDataAsync(It.IsAny<CancellationToken>())).ReturnsAsync(PageData());
        _mockApiClient
            .Setup(x => x.GetSectionFeaturesAsync("github-copilot", It.IsAny<CancellationToken>()))
            .ReturnsAsync([VideoFeature(), ArticleFeature(), ChildProductFeature()]);
        Services.AddSingleton(_mockApiClient.Object);

        _persistentState = AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));

        JSInterop.SetupVoid("markScriptsReady").SetVoidResult();
    }

    private static FeaturesPageData PageData() => new()
    {
        Title = "GitHub Copilot Features",
        Description = "Features",
        Intro = "Intro of the page.",
        Links = new FeaturesLinks { Pricing = "https://example.com/pricing", PlanDetails = "https://example.com/plans" },
        SubscriptionTiers =
        [
            new SubscriptionTier { Id = "free", Name = "Free", Tagline = "Try it out", Features = ["Code completions"], VideoAnchor = "free" },
            new SubscriptionTier { Id = "pro", Name = "Pro", Tagline = "For individuals", Features = ["Agent mode", "Code review"], VideoAnchor = "pro" }
        ]
    };

    private static SectionFeatureContent Content(string collection, string slug, string title, string url = "", bool thumbnail = false) => new()
    {
        CollectionName = collection,
        ItemSlug = slug,
        Title = title,
        ExternalUrl = url,
        PrimarySectionName = "github-copilot",
        IsThumbnail = thumbnail
    };

    private static SectionFeature VideoFeature() => new()
    {
        Slug = "agent-mode",
        Name = "Agent mode",
        ProductName = "GitHub Copilot",
        Description = "Let Copilot work on its own.",
        ReleaseDate = new DateOnly(2025, 2, 6),
        Content =
        [
            Content("videos", "agent-mode-video", "Agent mode explained", "https://youtu.be/abc123test1", thumbnail: true),
            Content("blogs", "agent-mode-blog", "Agent mode in practice", "https://example.com/agent-mode"),
            Content("news", "agent-mode-news-1", "News 1", "https://example.com/n1"),
            Content("news", "agent-mode-news-2", "News 2", "https://example.com/n2"),
            Content("news", "agent-mode-news-3", "News 3", "https://example.com/n3")
        ]
    };

    private static SectionFeature ArticleFeature() => new()
    {
        Slug = "premium-requests",
        Name = "Premium requests",
        ProductName = "GitHub Copilot",
        Description = "Requests for the best models.",
        Content = [Content("news", "premium-requests-news", "Premium requests explained", "https://example.com/premium")]
    };

    private static SectionFeature ChildProductFeature() => new()
    {
        Slug = "cli-plan",
        Name = "Plan command",
        ProductName = "GitHub Copilot CLI",
        Content = [Content("videos", "cli-video", "CLI video", "https://www.youtube.com/watch?v=cliVideo_1")]
    };

    private IRenderedComponent<GitHubCopilotFeatures> RenderPage()
    {
        var cut = Render<GitHubCopilotFeatures>();
        cut.WaitForState(() => cut.FindAll(".feature-card").Count > 0 || cut.FindAll(".features-empty").Count > 0);
        return cut;
    }

    [Fact]
    public void Render_ShowsTheSubscriptionsInTheSidebar()
    {
        var cut = RenderPage();

        var sidebar = cut.Find("aside.sidebar");
        sidebar.QuerySelector(".features-tiers-sidebar h2")!.TextContent.Should().Be("Subscriptions");
        sidebar.QuerySelectorAll(".features-tier-card h3").Select(h => h.TextContent).Should().Equal("Free", "Pro");
        sidebar.QuerySelectorAll("#tier-pro .features-tier-features li").Select(li => li.TextContent).Should().Equal("Agent mode", "Code review");
    }

    [Fact]
    public void Render_TierTitles_LinkToThePricingPage()
    {
        var cut = RenderPage();

        var links = cut.FindAll(".features-tier-card h3 a");

        links.Select(a => a.TextContent).Should().Equal("Free", "Pro");
        links.Should().OnlyContain(a => a.GetAttribute("href") == "https://example.com/pricing"
            && a.GetAttribute("target") == "_blank" && a.GetAttribute("rel") == "noopener noreferrer");
    }

    [Fact]
    public void Render_ShowsATextFilterFieldNextToTheButtons_ForTheTextFilterScript()
    {
        var cut = RenderPage();

        var input = cut.Find(".features-filters input[type=search]");
        input.HasAttribute("data-text-filter").Should().BeTrue("text-filter.js acts on the marker");
        input.GetAttribute("data-filter-items").Should().Be(".feature-card");
        input.GetAttribute("aria-label").Should().NotBeNullOrWhiteSpace();
        cut.Find(input.GetAttribute("data-filter-empty")!).HasAttribute("hidden").Should().BeTrue("the message only shows when no card matches");
    }

    [Fact]
    public void Render_FeatureTitles_AreNotLinks()
    {
        var cut = RenderPage();

        cut.FindAll(".feature-card h3").Should().NotBeEmpty().And.OnlyContain(h => h.QuerySelector("a") == null,
            "a card can hold several articles, so a title that links to one of them is confusing");
    }

    [Fact]
    public void Render_ShowsTheIntroWithoutABillingNotice()
    {
        var cut = RenderPage();

        cut.Find(".custom-page-intro").TextContent.Should().Contain("Intro of the page.");
        cut.FindAll(".custom-page-billing-notice").Should().BeEmpty();
    }

    [Fact]
    public void Render_ShowsTheSubscriptionsAtFullHeightWithoutAScrollArea()
    {
        var cut = RenderPage();

        cut.FindAll(".features-tiers .features-tier-card").Should().HaveCount(2);
        cut.FindAll(".features-tiers-scroll").Should().BeEmpty();
    }

    [Fact]
    public void Render_ShowsACardPerFeatureAsAGridAndNotAsATimeline()
    {
        var cut = RenderPage();

        cut.FindAll(".features-cards-grid .feature-card h3").Select(h => h.TextContent)
            .Should().Equal("Agent mode", "Premium requests", "Plan command");
        cut.FindAll(".timeline-entry").Should().BeEmpty();
    }

    [Fact]
    public void Render_FeatureWithVideo_ShowsTheThumbnailAndLinksToTheVideo()
    {
        var cut = RenderPage();

        var card = cut.Find("#feature-agent-mode");
        card.QuerySelector(".feature-card-thumbnail img")!.GetAttribute("src").Should().Be("https://img.youtube.com/vi/abc123test1/mqdefault.jpg");
        card.QuerySelector(".feature-card-thumbnail")!.GetAttribute("href").Should().Be("/github-copilot/videos/agent-mode-video");
        card.QuerySelector(".feature-badge-video")!.TextContent.Should().Contain("Video");
        card.QuerySelector(".card-description")!.TextContent.Should().Be("Let Copilot work on its own.");
    }

    [Fact]
    public void Render_FeatureWithOnlyAnArticle_ShowsNoVideoAndListsTheArticleAsContent()
    {
        var cut = RenderPage();

        var card = cut.Find("#feature-premium-requests");
        card.QuerySelector(".feature-card-thumbnail").Should().BeNull();
        card.QuerySelector(".feature-badge-video").Should().BeNull();
        card.QuerySelectorAll(".feature-card-related li a").Select(a => a.TextContent)
            .Should().ContainSingle("a feature with one article is not an empty card: its content is listed")
            .Which.Should().Be("Premium requests explained");
        card.QuerySelector(".feature-card-related li a")!.GetAttribute("href").Should().Be("https://example.com/premium", "news items open the original article");
    }

    [Fact]
    public void Render_ContentLinks_AreMarkedAndKeepTheTargetOfATouchDevice()
    {
        var cut = RenderPage();

        var links = cut.FindAll(".feature-card-thumbnail, .feature-card-related li a");

        links.Should().NotBeEmpty();
        links.Should().OnlyContain(l => l.HasAttribute("data-content-link"), "content-links.js opens them in a new tab on a desktop");

        // The server renders what a touch device keeps: an article on another site in a new tab, a page of this site in the same tab.
        cut.Find("#feature-premium-requests .feature-card-related li a").GetAttribute("target").Should().Be("_blank");
        cut.Find("#feature-premium-requests .feature-card-related li a").GetAttribute("rel").Should().Be("noopener noreferrer");
        cut.Find("#feature-agent-mode .feature-card-thumbnail").HasAttribute("target").Should().BeFalse();
    }

    [Fact]
    public void Render_FeatureWithAVideo_DoesNotListTheVideoAgainInTheLinks()
    {
        var cut = RenderPage();

        var related = cut.Find("#feature-cli-plan").QuerySelectorAll(".feature-card-related li a");

        related.Should().BeEmpty("the only item is the video, which is shown as the thumbnail");
    }

    [Fact]
    public void Render_ListsAtMostThreeRelatedItemsAndCountsTheRest()
    {
        var cut = RenderPage();

        var related = cut.Find("#feature-agent-mode").QuerySelectorAll(".feature-card-related li");
        related.Select(li => li.TextContent.Trim()).Should().Equal("Agent mode in practice", "News 1", "News 2", "+ 1 more");
    }

    [Fact]
    public void MoreButton_ShowsTheRestOfTheLinks_AndShowLessCollapsesThemAgain()
    {
        var cut = RenderPage();

        cut.Find("#feature-agent-mode .feature-card-more-btn").Click();

        var expanded = cut.Find("#feature-agent-mode");
        expanded.QuerySelectorAll(".feature-card-related li a").Select(a => a.TextContent)
            .Should().Equal("Agent mode in practice", "News 1", "News 2", "News 3");
        expanded.QuerySelector(".feature-card-more-btn")!.TextContent.Should().Be("Show less");
        expanded.QuerySelector(".feature-card-more-btn")!.GetAttribute("aria-expanded").Should().Be("true");

        cut.Find("#feature-agent-mode .feature-card-more-btn").Click();

        cut.Find("#feature-agent-mode").QuerySelectorAll(".feature-card-related li a").Should().HaveCount(3);
        cut.Find("#feature-agent-mode .feature-card-more-btn").TextContent.Should().Be("+ 1 more");
    }

    [Fact]
    public void MoreButton_OnlyExpandsTheCardItBelongsTo()
    {
        var cut = RenderPage();

        cut.Find("#feature-agent-mode .feature-card-more-btn").Click();

        cut.Find("#feature-premium-requests").QuerySelectorAll(".feature-card-more-btn").Should().BeEmpty();
        cut.FindAll(".feature-card-more-btn").Should().ContainSingle();
    }

    [Fact]
    public void Render_ShowsTheProductOnlyForFeaturesOfAChildProduct()
    {
        var cut = RenderPage();

        cut.Find("#feature-cli-plan .feature-badge:not(.feature-badge-video)").TextContent.Should().Be("GitHub Copilot CLI");
        cut.FindAll("#feature-agent-mode .feature-badge:not(.feature-badge-video)").Select(b => b.TextContent).Should().Equal("Feb 2025");
    }

    [Fact]
    public void WithVideosFilter_HidesFeaturesWithoutAVideo_AndAllShowsThemAgain()
    {
        var cut = RenderPage();

        cut.Find("button[data-filter='videos']").Click();

        cut.FindAll(".feature-card h3").Select(h => h.TextContent).Should().Equal("Agent mode", "Plan command");
        cut.Find("button[data-filter='videos']").GetAttribute("aria-pressed").Should().Be("true");

        cut.Find("button[data-filter='all']").Click();

        cut.FindAll(".feature-card").Should().HaveCount(3);
    }

    [Fact]
    public void Render_WithoutFeatures_ShowsAnEmptyMessageButKeepsTheSubscriptions()
    {
        _mockApiClient
            .Setup(x => x.GetSectionFeaturesAsync("github-copilot", It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var cut = RenderPage();

        cut.Find(".features-empty").TextContent.Should().Contain("No features");
        cut.FindAll(".features-filter-btn").Should().BeEmpty();
        cut.FindAll(".features-tier-card").Should().HaveCount(2);
    }

    [Fact]
    public void Render_WithoutPageData_ShowsNotFound()
    {
        _mockApiClient.Setup(x => x.GetFeaturesDataAsync(It.IsAny<CancellationToken>())).ReturnsAsync((FeaturesPageData?)null);

        var cut = Render<GitHubCopilotFeatures>();

        cut.FindAll(".feature-card").Should().BeEmpty();
        cut.FindAll(".features-tier-card").Should().BeEmpty();
        cut.Find(".page-h1").TextContent.Should().Be("Page Not Found");
    }

    [Fact]
    public void Render_RestoredStateOfANotFoundPage_StillShowsNotFound()
    {
        _mockApiClient.Setup(x => x.GetFeaturesDataAsync(It.IsAny<CancellationToken>())).ReturnsAsync((FeaturesPageData?)null);
        Render<GitHubCopilotFeatures>();
        _persistentState.TriggerOnPersisting();

        // The interactive render restores what the prerender persisted; the API now would answer, but must not be asked.
        _mockApiClient.Setup(x => x.GetFeaturesDataAsync(It.IsAny<CancellationToken>())).ReturnsAsync(PageData());
        var restored = Render<GitHubCopilotFeatures>();

        restored.Find(".page-h1").TextContent.Should().Be("Page Not Found", "a restored not-found page must not turn into an empty page");
    }

    [Fact]
    public void Render_WhileTheFeaturesLoad_DoesNotSignalTheScriptsReadyUntilTheyArrive()
    {
        var features = new TaskCompletionSource<IReadOnlyList<SectionFeature>>();
        _mockApiClient
            .Setup(x => x.GetSectionFeaturesAsync("github-copilot", It.IsAny<CancellationToken>()))
            .Returns(features.Task);

        var cut = Render<GitHubCopilotFeatures>();

        JSInterop.VerifyNotInvoke("markScriptsReady");

        features.SetResult([VideoFeature()]);
        cut.WaitForState(() => cut.FindAll(".feature-card").Count == 1);
        cut.WaitForAssertion(() => JSInterop.VerifyInvoke("markScriptsReady"));
    }
}
