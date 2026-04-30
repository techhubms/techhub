using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Models;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for HeroBanner.razor - collapsible announcement banner shown above section content.
/// </summary>
public class HeroBannerTests : BunitContext
{
    private readonly HeroBannerCache _heroBannerCache;

    public HeroBannerTests()
    {
        _heroBannerCache = new HeroBannerCache();

        // Setup JS interop mocks for cookie operations
        JSInterop.SetupVoid("TechHub.heroBanner.setCollapsed", _ => true);
        JSInterop.SetupVoid("TechHub.heroBanner.setHash", _ => true);

        // Default: no cookies set (new visitor)
        Services.AddSingleton(_heroBannerCache);
        Services.AddSingleton<ILogger<HeroBanner>>(new Mock<ILogger<HeroBanner>>().Object);
        AddBunitPersistentComponentState();
    }

    /// <summary>
    /// Seed the <see cref="HeroBannerCache"/> with the supplied data.
    /// </summary>
    private void SetupCache(HeroBannerData? data) => _heroBannerCache.Initialize(data);

    /// <summary>
    /// Register an <see cref="IHttpContextAccessor"/> that returns the specified cookie values.
    /// </summary>
    private void RegisterHttpContextWithCookies(Dictionary<string, string>? cookies = null)
    {
        var mockCookies = new Mock<IRequestCookieCollection>();
        foreach (var (key, value) in cookies ?? [])
        {
            mockCookies.Setup(c => c[key]).Returns(value);
        }

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(r => r.Cookies).Returns(mockCookies.Object);

        var mockContext = new Mock<HttpContext>();
        mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

        var mockAccessor = new Mock<IHttpContextAccessor>();
        mockAccessor.Setup(a => a.HttpContext).Returns(mockContext.Object);

        Services.AddSingleton<IHttpContextAccessor>(mockAccessor.Object);
    }

    [Fact]
    public void HeroBanner_WithNoActiveCards_RendersNothing()
    {
        // Arrange — all cards are in the past
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Expired Event",
                    Description = "This event is over",
                    StartDate = "2020-01-01",
                    EndDate = "2020-01-02"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert — nothing rendered (no active cards)
        cut.Markup.Should().BeEmpty();
    }

    [Fact]
    public void HeroBanner_WithActiveCards_RendersAside()
    {
        // Arrange — cards active for a very long time (past and far future)
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Upcoming Event",
                    Description = "An exciting event",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31",
                    LinkUrl = "https://example.com",
                    LinkText = "Register"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert
        var aside = cut.Find("aside.hero-banner");
        aside.Should().NotBeNull();
        cut.Find(".hero-banner-card-title").TextContent.Should().Contain("Upcoming Event");
        cut.Find(".hero-banner-card-description").TextContent.Should().Contain("An exciting event");
    }

    [Fact]
    public void HeroBanner_WithActiveCards_ShowsRegisterLink()
    {
        // Arrange
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Event",
                    Description = "Description",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31",
                    LinkUrl = "https://example.com",
                    LinkText = "Sign up"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert
        var link = cut.Find("a.hero-banner-card-link");
        link.GetAttribute("href").Should().Be("https://example.com");
        link.TextContent.Trim().Should().Be("Sign up");
        link.GetAttribute("target").Should().Be("_blank");
        link.GetAttribute("rel").Should().Contain("noopener");
    }

    [Fact]
    public void HeroBanner_WithNullData_RendersNothing()
    {
        // Arrange
        RegisterHttpContextWithCookies();
        SetupCache(null);

        // Act
        var cut = Render<HeroBanner>();

        // Assert — nothing rendered
        cut.Markup.Should().BeEmpty();
    }

    [Fact]
    public void HeroBanner_WhenCollapsedCookieIsSet_RendersCollapsed()
    {
        // Arrange — collapsed cookie is set; hash matches so no auto-expand
        // The card title produces a hash; set the same hash in cookie to simulate "seen before"
        const string CardTitle = "Test Event";
        // Compute the same hash the component would compute
        var hash = 0u;
        foreach (var ch in CardTitle)
        {
            hash = hash * 31u + ch;
        }

        var hashString = hash.ToString(System.Globalization.CultureInfo.InvariantCulture);

        RegisterHttpContextWithCookies(new Dictionary<string, string>
        {
            { "hero-banner-collapsed", "true" },
            { "hero-banner-hash", hashString }
        });

        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = CardTitle,
                    Description = "Description",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert — banner rendered but collapsed
        var aside = cut.Find("aside.hero-banner");
        aside.ClassList.Should().Contain("hero-banner-collapsed");
    }

    [Fact]
    public void HeroBanner_ShowsToggleButton()
    {
        // Arrange
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Event",
                    Description = "Description",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert
        var toggleButton = cut.Find("button.hero-banner-toggle");
        toggleButton.Should().NotBeNull();
        toggleButton.GetAttribute("aria-expanded").Should().Be("true");
    }

    [Fact]
    public void HeroBanner_ShowsFindMoreLink()
    {
        // Arrange — banner data includes a find-more URL and text
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Label = "Featured Events",
            FindMoreUrl = "https://luma.com/githubcopilotdevdays",
            FindMoreText = "Find a GitHub Copilot Dev Day near you",
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Event",
                    Description = "Description",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert — find-more link uses URL and text from the data model
        var findMore = cut.Find("a.hero-banner-find-more");
        findMore.Should().NotBeNull();
        findMore.GetAttribute("href").Should().Be("https://luma.com/githubcopilotdevdays");
        findMore.GetAttribute("target").Should().Be("_blank");
        findMore.GetAttribute("rel").Should().Contain("noopener");
        findMore.QuerySelector(".hero-banner-find-more-text")!.TextContent.Should().Be("Find a GitHub Copilot Dev Day near you");
    }

    [Fact]
    public void HeroBanner_WithNoFindMoreUrl_DoesNotRenderFindMoreLink()
    {
        // Arrange — banner data has no find-more URL
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Announcement",
                    Description = "A generic announcement",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert — no find-more link rendered
        cut.FindAll("a.hero-banner-find-more").Should().BeEmpty();
    }

    [Fact]
    public void HeroBanner_WithLabel_ShowsLabelInHeader()
    {
        // Arrange
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Label = "Featured Events",
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Event",
                    Description = "Description",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31"
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>();

        // Assert — label rendered in header
        cut.Find(".hero-banner-label").TextContent.Should().Be("Featured Events");
    }

    [Fact]
    public void HeroBanner_CardWithMatchingSection_IsShown()
    {
        // Arrange — card restricted to "ai" section; banner rendered with SectionName="ai"
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "AI Event",
                    Description = "An AI-specific event",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31",
                    Sections = ["ai"]
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>(parameters => parameters.Add(p => p.SectionName, "ai"));

        // Assert — card is shown because section matches
        cut.Find("aside.hero-banner").Should().NotBeNull();
        cut.Find(".hero-banner-card-title").TextContent.Should().Contain("AI Event");
    }

    [Fact]
    public void HeroBanner_CardWithDifferentSection_IsHidden()
    {
        // Arrange — card restricted to "ai" section; banner rendered with SectionName="devops"
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "AI Event",
                    Description = "An AI-specific event",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31",
                    Sections = ["ai"]
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>(parameters => parameters.Add(p => p.SectionName, "devops"));

        // Assert — banner renders nothing because the card is not for this section
        cut.Markup.Should().BeEmpty();
    }

    [Fact]
    public void HeroBanner_CardWithNoSections_IsShownInAllSections()
    {
        // Arrange — card has no Sections restriction
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "Universal Announcement",
                    Description = "Shows everywhere",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31"
                }
            ]
        });

        // Act — render for an arbitrary section
        var cut = Render<HeroBanner>(parameters => parameters.Add(p => p.SectionName, "azure"));

        // Assert — card shows even though no explicit section filter
        cut.Find("aside.hero-banner").Should().NotBeNull();
        cut.Find(".hero-banner-card-title").TextContent.Should().Contain("Universal Announcement");
    }

    [Theory]
    [InlineData("ai", true)]
    [InlineData("cloud", false)]
    [InlineData("devops", false)]
    public void HeroBanner_SectionFiltering_OnParametersSet_CorrectlyFiltersPerSection(string sectionName, bool expectVisible)
    {
        // Arrange — card restricted to "ai" section only.
        // OnParametersSet fires on every render (including initial), so this validates
        // that section filtering applies correctly regardless of how the component was invoked.
        // In production, Blazor Server reuses the component instance on navigation, meaning
        // OnParametersSet is the only lifecycle method that fires after the first render.
        RegisterHttpContextWithCookies();
        SetupCache(new HeroBannerData
        {
            Cards =
            [
                new HeroBannerCard
                {
                    Title = "AI Event",
                    Description = "An AI-specific event",
                    StartDate = "2020-01-01",
                    EndDate = "2099-12-31",
                    Sections = ["ai"]
                }
            ]
        });

        // Act
        var cut = Render<HeroBanner>(parameters => parameters.Add(p => p.SectionName, sectionName));

        // Assert
        if (expectVisible)
        {
            cut.Find("aside.hero-banner").Should().NotBeNull();
            cut.Find(".hero-banner-card-title").TextContent.Should().Contain("AI Event");
        }
        else
        {
            cut.Markup.Should().BeEmpty($"section-specific card must not appear in '{sectionName}' section");
        }
    }
}
