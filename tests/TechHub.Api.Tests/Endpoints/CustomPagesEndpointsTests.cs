using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

public class CustomPagesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly TechHubIntegrationTestApiFactory _factory;
    private readonly HttpClient _client;

    public CustomPagesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    // Tests for specific structured data endpoints
    [Theory]
    [InlineData("/api/custom-pages/features")]
    [InlineData("/api/custom-pages/genai-applied")]
    [InlineData("/api/custom-pages/handbook")]
    [InlineData("/api/custom-pages/levels")]
    [InlineData("/api/custom-pages/sdlc")]
    [InlineData("/api/custom-pages/genai-basics")]
    [InlineData("/api/custom-pages/genai-advanced")]
    [InlineData("/api/custom-pages/tool-tips")]
    public async Task GetSpecificCustomPage_ReturnsOk(string endpoint)
    {
        // Act
        var response = await _client.GetAsync(endpoint, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetDXSpaceData_ReturnsStructuredData()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages/dx-space", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<DXSpacePageData>(TestContext.Current.CancellationToken);
        data.Should().NotBeNull();
        data!.Title.Should().Be("Developer Experience Space");
        data.Dora.Should().NotBeNull();
        data.Dora.Metrics.Should().HaveCount(4);
        data.Space.Should().NotBeNull();
        data.Space.Dimensions.Should().HaveCount(5);
        data.DevEx.Should().NotBeNull();
        data.BestPractices.Should().NotBeNull();
    }

    [Fact]
    public async Task GetToolTipsData_ReturnsStructuredData()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages/tool-tips", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<ToolTipsPageData>(TestContext.Current.CancellationToken);
        data.Should().NotBeNull();
        data!.Title.Should().Be("GitHub Copilot Tool Tips");
        data.Tools.Should().NotBeEmpty();
        data.Tools[0].Name.Should().NotBeNullOrWhiteSpace();
        data.Tools[0].Url.Should().NotBeNullOrWhiteSpace();
        data.Tools[0].Category.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetFeaturesData_ReturnsStructuredData()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages/features", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<FeaturesPageData>(TestContext.Current.CancellationToken);
        data.Should().NotBeNull();
        data!.Title.Should().Be("GitHub Copilot Features");
        data.SubscriptionTiers.Should().HaveCount(5);
        data.SubscriptionTiers[0].Name.Should().Be("Free");
        data.SubscriptionTiers[0].Price.Should().BeNull();
        data.SubscriptionTiers[0].VideoAnchor.Should().NotBeNullOrWhiteSpace();
        data.FeatureSections.Should().HaveCount(3);
        data.FeatureSections[0].Title.Should().Be("Free Features");
        data.FeatureSections[0].Plans.Should().Contain("Free");
        data.VideoCollection.Should().Be("ghc-features");
    }

    [Fact]
    public async Task GetGhcFeaturesVideos_ReturnsVideosWithPlansAndGhesSupport()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/sections/github-copilot/collections/videos/items?subcollection=ghc-features&includeDraft=true&lastDays=0",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty("ghc-features collection should have video items");

        // Verify videos have plans that match feature section plan names
        var validPlanNames = new[] { "Free", "Pro", "Business", "Pro+", "Enterprise" };
        var nonDraftItems = result.Items.Where(i => !i.Draft).ToList();
        nonDraftItems.Should().NotBeEmpty("Should have non-draft videos");

        foreach (var item in nonDraftItems)
        {
            item.Plans.Should().NotBeEmpty($"Video '{item.Title}' should have plans assigned");
            item.Plans.Should().AllSatisfy(plan =>
                plan.Should().BeOneOf(validPlanNames, $"Video '{item.Title}' plan '{plan}' should be a valid plan name"));
        }

        // Verify at least some videos have GHES support
        nonDraftItems.Should().Contain(item => item.GhesSupport, "At least some videos should have GHES support");

        // Verify at least some videos have external URLs (YouTube)
        nonDraftItems.Should().Contain(item => !string.IsNullOrEmpty(item.ExternalUrl),
            "At least some videos should have external URLs for YouTube thumbnails");
    }
}
