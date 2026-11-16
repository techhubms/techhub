using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

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
    public async Task GetSectionFeatures_AfterAnAdminLinksContentToANewFeature_ShowsTheFeatureWithoutWaiting()
    {
        // Arrange: the first call fills the cache of the features page
        var ct = TestContext.Current.CancellationToken;
        var before = await _client.GetFromJsonAsync<List<SectionFeature>>("/api/sections/github-copilot/features", ct);
        var existing = before!.First().Content[0];
        var products = await _client.GetFromJsonAsync<List<ProductSummary>>("/api/admin/products", ct);
        var productId = products!.Single(p => p.Name == "GitHub Copilot").Id;
        var name = $"Cache test feature {Guid.NewGuid():N}";
        var created = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features", new { name }, ct);
        created.StatusCode.Should().Be(HttpStatusCode.Created);
        var featureId = (await created.Content.ReadFromJsonAsync<ProductFeatureOverview>(ct))!.Features.Single(f => f.Name == name).Id;

        // Act
        var link = await _client.PostAsJsonAsync(
            $"/api/admin/products/{productId}/features/{featureId}/content",
            new { collectionName = existing.CollectionName, itemSlug = existing.ItemSlug, isThumbnail = false }, ct);
        var after = await _client.GetFromJsonAsync<List<SectionFeature>>("/api/sections/github-copilot/features", ct);

        // Assert
        link.StatusCode.Should().Be(HttpStatusCode.OK);
        after!.Should().Contain(f => f.Name == name, "an admin write clears the cache of the features page");
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
