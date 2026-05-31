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
/// Tests for Home.razor component — homepage stats strip and section grid rendering.
/// </summary>
public class HomeTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _mockApiClient;
    private readonly HomepageStatsState _statsState;

    private static readonly List<Section> _defaultSections =
    [
        new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
            [new Collection("news", "News", "/ai/news", "Latest news", "News")]),
        new("github-copilot", "GitHub Copilot", "Copilot", "/github-copilot", "GitHub Copilot",
            [new Collection("news", "News", "/github-copilot/news", "Latest news", "News")]),
        new("cloud", "Cloud", "Cloud", "/cloud", "Cloud",
            [new Collection("news", "News", "/cloud/news", "Latest news", "News")]),
    ];

    public HomeTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        Services.AddSingleton<IHttpContextAccessor>(httpContextAccessor);

        var config = new ConfigurationBuilder().Build();
        Services.AddScoped<BrandingService>(_ => new BrandingService(httpContextAccessor, config));

        Services.AddSingleton<ILogger<Home>>(Mock.Of<ILogger<Home>>());
        Services.AddSingleton<ErrorService>();

        _statsState = new HomepageStatsState();
        Services.AddSingleton(_statsState);

        _mockApiClient = new Mock<ITechHubApiClient>();

        // Defaults — can be overridden per test
        _mockApiClient
            .Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_defaultSections);

        _mockApiClient
            .Setup(x => x.GetLatestItemsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _mockApiClient
            .Setup(x => x.GetLatestRoundupPerSectionAsync(
                It.IsAny<IEnumerable<Section>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Default: total = 1337, recent = 42
        _mockApiClient
            .Setup(x => x.GetCollectionItemsAsync(
                "all", "all",
                1, null, null, null, null, null, null, null,
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 1337));

        _mockApiClient
            .Setup(x => x.GetCollectionItemsAsync(
                "all", "all",
                1, null, null, null, null, It.IsNotNull<string>(), null, null,
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 42));

        Services.AddSingleton(_mockApiClient.Object);

        // Stub heavy child components
        ComponentFactories.AddStub<SeoMetaTags>();
        ComponentFactories.AddStub<SidebarToggle>();
        ComponentFactories.AddStub<MobileSidebarToolbar>();
        ComponentFactories.AddStub<SidebarTagCloud>();
        ComponentFactories.AddStub<SectionCardsGrid>();

        AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));
    }

    [Fact]
    public void Home_WithTotalCount_PopulatesStatsServiceTotalCount()
    {
        // Act
        var cut = Render<Home>();

        // Assert — Home.razor should call HomepageStatsState.Update() with the total count
        cut.WaitForAssertion(() => _statsState.TotalCount.Should().Be(1337));
    }

    [Fact]
    public void Home_WithRecentCount_PopulatesStatsServiceRecentCount()
    {
        // Act
        var cut = Render<Home>();

        // Assert — recent count populated from GetCollectionItemsAsync(lastDays: 7)
        cut.WaitForAssertion(() => _statsState.RecentCount.Should().Be(42));
    }

    [Fact]
    public void Home_WithZeroRecentCount_PopulatesZeroInStatsService()
    {
        // Arrange — override recent count to 0
        _mockApiClient
            .Setup(x => x.GetCollectionItemsAsync(
                "all", "all",
                1, null, null, null, null, It.IsNotNull<string>(), null, null,
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 0));

        // Act
        var cut = Render<Home>();

        // Assert — recentCount = 0, service should have RecentCount = 0
        cut.WaitForAssertion(() => _statsState.RecentCount.Should().Be(0));
    }

    [Fact]
    public void Home_PopulatesStatsService_WithSectionsCount()
    {
        // Act
        var cut = Render<Home>();

        // Assert — sections count matches _defaultSections.Count (3)
        cut.WaitForAssertion(() => _statsState.SectionsCount.Should().Be(_defaultSections.Count));
    }

    [Fact]
    public void Home_PopulatesStatsService_WithValidWeekAgoDate()
    {
        // Act
        var cut = Render<Home>();

        // Assert — WeekAgoDate is a valid yyyy-MM-dd date string
        cut.WaitForAssertion(() =>
        {
            _statsState.WeekAgoDate.Should().NotBeNullOrEmpty();
            DateTime.TryParseExact(_statsState.WeekAgoDate, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out _).Should().BeTrue("WeekAgoDate should be in yyyy-MM-dd format");
        });
    }

    [Fact]
    public void Home_StatsService_TotalCountReflectsApiResponse()
    {
        // Arrange — use a large number to verify value is passed through
        _mockApiClient
            .Setup(x => x.GetCollectionItemsAsync(
                "all", "all",
                1, null, null, null, null, null, null, null,
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 12345));

        // Act
        var cut = Render<Home>();

        // Assert — service receives exact total count from API
        cut.WaitForAssertion(() => _statsState.TotalCount.Should().Be(12345));
    }

    [Fact]
    public void Home_StatsService_RecentCountHref_DateIsEarlierThanToday()
    {
        // Act
        var cut = Render<Home>();

        // Assert — WeekAgoDate is in the past (roughly 7 days ago)
        cut.WaitForAssertion(() =>
        {
            _statsState.WeekAgoDate.Should().NotBeNullOrEmpty();
            var parsedDate = DateTime.ParseExact(_statsState.WeekAgoDate!, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);
            parsedDate.Should().BeBefore(DateTime.Today,
                "WeekAgoDate should be a past date");
            parsedDate.Should().BeAfter(DateTime.Today.AddDays(-10),
                "WeekAgoDate should be approximately 7 days ago");
        });
    }

    [Fact]
    public void Home_WithNullApiResponse_LeavesStatsServiceTotalCountNull()
    {
        // Arrange — both count calls return null (API error/no data)
        _mockApiClient
            .Setup(x => x.GetCollectionItemsAsync(
                "all", "all",
                1, null, null, null, null, null, null, null,
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CollectionItemsResponse?)null);

        _mockApiClient
            .Setup(x => x.GetCollectionItemsAsync(
                "all", "all",
                1, null, null, null, 7, null, null, null,
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CollectionItemsResponse?)null);

        // Act
        var cut = Render<Home>();

        // Assert — TotalCount stays null in service when API returns null
        cut.WaitForAssertion(() =>
        {
            // Sections have loaded so the page is done rendering
            _statsState.TotalCount.Should().BeNull(
                "service TotalCount should remain null when API returns null response");
        });
    }
}
