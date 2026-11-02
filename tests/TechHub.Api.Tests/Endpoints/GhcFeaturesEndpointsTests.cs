using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for GET /api/ghc-features (the features of GitHub Copilot in the catalog, public).
/// The features are edited through the admin product feature endpoints (see AdminFeaturesEndpointsTests).
/// Test data is seeded from TestCollections/_videos/ghc-features/*.md; the other tests create their own uniquely named
/// features and content items so tests are independent of each other.
/// </summary>
public class GhcFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public GhcFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _client = factory.CreateClient();
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetGhcFeatures_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/ghc-features/", Ct);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetGhcFeatures_ReturnsJsonArray()
    {
        // Act
        var features = await _client.GetFromJsonAsync<List<GhcFeature>>("/api/ghc-features/", Ct);

        // Assert
        features.Should().NotBeNull("endpoint should return a JSON array");
        features.Should().NotBeEmpty("endpoint should return seeded GHC feature entries from TestCollections");
    }

    [Fact]
    public async Task GetGhcFeatures_ShowsAFeatureOfGitHubCopilotWithTheDetailsAndContentSetOnTheProductPage()
    {
        // Arrange
        var productId = await GetGitHubCopilotProductIdAsync();
        var title = $"Edited feature {Guid.NewGuid():N}";
        var created = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features", new { name = title }, Ct);
        created.StatusCode.Should().Be(HttpStatusCode.Created);
        var feature = (await created.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!.Features.Single(f => f.Name == title);
        var video = $"ghc-link-{Guid.NewGuid():N}";
        await ContentItemSeeder.InsertAsync(_factory.Services, "videos", video, $"/all/videos/{video}", title: "Linked video");
        (await _client.PutAsJsonAsync($"/api/admin/products/{productId}/features/{feature.Id}/details", new { description = "Described", releaseDate = "2026-03-10" }, Ct))
            .StatusCode.Should().Be(HttpStatusCode.OK);
        (await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features/{feature.Id}/content", new { collectionName = "videos", itemSlug = video, isThumbnail = true }, Ct))
            .StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        var features = await _client.GetFromJsonAsync<List<GhcFeature>>("/api/ghc-features/", Ct);

        // Assert
        var found = features!.Should().ContainSingle(f => f.Title == title).Subject;
        found.Description.Should().Be("Described");
        found.ReleaseDate.Should().Be(new DateTimeOffset(2026, 3, 10, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds());
        found.ContentLinks.Should().ContainSingle().Which.ItemSlug.Should().Be(video);
        found.ThumbnailLink!.ItemTitle.Should().Be("Linked video");
    }

    [Fact]
    public async Task GetGhcFeatures_DoesNotShowTheFeaturesOfOtherProducts()
    {
        // Arrange
        var created = await _client.PostAsJsonAsync("/api/admin/products", new { name = $"Other product {Guid.NewGuid():N}" }, Ct);
        var otherProductId = (await created.Content.ReadFromJsonAsync<ProductDetail>(Ct))!.Id;
        var title = $"Other feature {Guid.NewGuid():N}";
        (await _client.PostAsJsonAsync($"/api/admin/products/{otherProductId}/features", new { name = title }, Ct)).StatusCode.Should().Be(HttpStatusCode.Created);

        // Act
        var features = await _client.GetFromJsonAsync<List<GhcFeature>>("/api/ghc-features/", Ct);

        // Assert
        features!.Should().NotContain(f => f.Title == title);
    }

    private async Task<long> GetGitHubCopilotProductIdAsync()
    {
        var products = await _client.GetFromJsonAsync<List<ProductSummary>>("/api/admin/products", Ct);
        return products!.Single(p => p.Name == "GitHub Copilot").Id;
    }
}
