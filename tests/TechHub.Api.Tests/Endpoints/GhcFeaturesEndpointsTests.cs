using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for GET /api/sections/{sectionName}/features: the features of the catalog that have content.
/// The test collections hold the ghc-features videos, which the test seeding turns into GitHub Copilot features with a thumbnail video.
/// </summary>
public class GhcFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public GhcFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetSectionFeatures_ForGitHubCopilot_ReturnsFeaturesThatAllHaveContent()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/github-copilot/features", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var features = await response.Content.ReadFromJsonAsync<List<SectionFeature>>(TestContext.Current.CancellationToken);
        features.Should().NotBeNullOrEmpty();
        features!.Should().OnlyContain(f => f.Content.Count > 0, "a feature without content is not shown");

        var feature = features.Single(f => f.Name == "Free Feature Without GHES");
        feature.ProductName.Should().Be("GitHub Copilot");
        feature.Content.Should().ContainSingle();
        feature.Content[0].IsThumbnail.Should().BeTrue();
        feature.Content[0].ExternalUrl.Should().Be("https://youtu.be/abc123test1");
        feature.Content[0].GetYouTubeVideoId().Should().Be("abc123test1");
    }

    [Fact]
    public async Task GetSectionFeatures_ForUnknownSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/does-not-exist/features", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
