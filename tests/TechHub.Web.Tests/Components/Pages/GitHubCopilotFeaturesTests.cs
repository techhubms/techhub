using Bunit;
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

        AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));

        JSInterop.SetupVoid("markScriptsReady").SetVoidResult();
    }

    private static FeaturesPageData PageData() => new()
    {
        Title = "GitHub Copilot Features",
        Description = "Features",
        Intro = "Intro of the page.",
        Links = new FeaturesLinks { Pricing = "https://example.com/pricing", PlanDetails = "https://example.com/plans" },
        BillingNotice = new BillingNotice
        {
            Text = "Billing changes.",
            Links = [new BillingNoticeLink { Label = "Billing docs", Url = "https://example.com/billing" }]
        },
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
    public void Render_ShowsTheIntroAndTheBillingNotice()
    {
        var cut = RenderPage();

        cut.Find(".custom-page-intro").TextContent.Should().Contain("Intro of the page.");
        cut.Find(".custom-page-billing-notice p").TextContent.Should().Be("Billing changes.");
        cut.Find(".custom-page-billing-notice a").GetAttribute("href").Should().Be("https://example.com/billing");
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
        card.QuerySelector("h3 a")!.GetAttribute("href").Should().Be("/github-copilot/videos/agent-mode-video");
        card.QuerySelector(".badge-info")!.TextContent.Should().Contain("Video");
        card.QuerySelector(".card-description")!.TextContent.Should().Be("Let Copilot work on its own.");
    }

    [Fact]
    public void Render_FeatureWithOnlyAnArticle_ShowsNoVideoAndLinksToTheArticle()
    {
        var cut = RenderPage();

        var card = cut.Find("#feature-premium-requests");
        card.QuerySelector(".feature-card-thumbnail").Should().BeNull();
        card.QuerySelector(".badge-info").Should().BeNull();
        card.QuerySelector("h3 a")!.GetAttribute("href").Should().Be("https://example.com/premium", "news items open the original article");
    }

    [Fact]
    public void Render_ListsAtMostThreeRelatedItemsAndCountsTheRest()
    {
        var cut = RenderPage();

        var related = cut.Find("#feature-agent-mode").QuerySelectorAll(".feature-card-related li");
        related.Select(li => li.TextContent.Trim()).Should().Equal("Agent mode in practice", "News 1", "News 2", "+ 1 more");
    }

    [Fact]
    public void Render_ShowsTheProductOnlyForFeaturesOfAChildProduct()
    {
        var cut = RenderPage();

        cut.Find("#feature-cli-plan .badge-muted").TextContent.Should().Be("GitHub Copilot CLI");
        cut.FindAll("#feature-agent-mode .badge-muted").Select(b => b.TextContent).Should().Equal("Feb 2025");
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
    }
}
