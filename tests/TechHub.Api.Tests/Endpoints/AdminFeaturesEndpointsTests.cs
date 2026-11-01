using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the feature endpoints under /api/admin/products/{productId}/features. Names are unique per test
/// because the database is shared by the tests of this class.
/// </summary>
public class AdminFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AdminFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

    private async Task<long> CreateProductAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/admin/products", new { name = UniqueName("Product") }, Ct);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        return (await response.Content.ReadFromJsonAsync<ProductDetail>(Ct))!.Id;
    }

    private async Task<(long ProductId, ProductFeature Feature)> CreateFeatureAsync(string? name = null)
    {
        var productId = await CreateProductAsync();
        var response = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features", new { name = name ?? UniqueName("Feature") }, Ct);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var overview = (await response.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!;
        return (productId, overview.Features.Single());
    }

    [Fact]
    public async Task GetFeatures_OfAProductWithoutFeatures_ReturnsEmptyLists()
    {
        var productId = await CreateProductAsync();

        var overview = await _client.GetFromJsonAsync<ProductFeatureOverview>($"/api/admin/products/{productId}/features", Ct);

        overview!.Features.Should().BeEmpty();
        overview.Unmatched.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateFeature_ReturnsCreatedOverviewWithLocation()
    {
        var productId = await CreateProductAsync();
        var name = UniqueName("Created");

        var response = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features", new { name = $"  {name}  " }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var overview = await response.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct);
        var feature = overview!.Features.Should().ContainSingle().Subject;
        feature.Name.Should().Be(name);
        response.Headers.Location!.ToString().Should().Be($"/api/admin/products/{productId}/features/{feature.Id}");
    }

    [Theory]
    [InlineData("""{"name":""}""")]
    [InlineData("""{"name":"   "}""")]
    [InlineData("""{}""")]
    public async Task CreateFeature_WithoutName_ReturnsBadRequest(string body)
    {
        var productId = await CreateProductAsync();
        using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"/api/admin/products/{productId}/features", content, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFeature_ForUnknownProduct_ReturnsNotFound()
    {
        var response = await _client.PostAsJsonAsync("/api/admin/products/999999999/features", new { name = "Feature" }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateFeature_WithExistingName_ReturnsConflictWithMessage()
    {
        var (productId, feature) = await CreateFeatureAsync();

        var response = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features", new { name = feature.Name.ToUpperInvariant() }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await response.Content.ReadAsStringAsync(Ct)).Should().Contain("already used");
    }

    [Fact]
    public async Task RenameFeature_KeepsTheOldNameAsAlias()
    {
        var (productId, feature) = await CreateFeatureAsync();
        var newName = UniqueName("Renamed");

        var response = await _client.PutAsJsonAsync($"/api/admin/products/{productId}/features/{feature.Id}", new { name = newName }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var renamed = (await response.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!.Features.Single();
        renamed.Name.Should().Be(newName);
        renamed.Aliases.Select(a => a.Alias).Should().Equal(feature.Name);
    }

    [Fact]
    public async Task RenameFeature_ThroughAnotherProduct_ReturnsNotFound()
    {
        var (_, feature) = await CreateFeatureAsync();
        var other = await CreateProductAsync();

        var response = await _client.PutAsJsonAsync($"/api/admin/products/{other}/features/{feature.Id}", new { name = "Renamed" }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddAndRemoveAlias_UpdatesTheFeature()
    {
        var (productId, feature) = await CreateFeatureAsync();
        var alias = UniqueName("Alias");

        var added = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features/{feature.Id}/aliases", new { alias }, Ct);

        added.StatusCode.Should().Be(HttpStatusCode.OK);
        var aliasId = (await added.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!.Features.Single().Aliases.Should().ContainSingle(a => a.Alias == alias).Subject.Id;

        var removed = await _client.DeleteAsync($"/api/admin/products/{productId}/features/{feature.Id}/aliases/{aliasId}", Ct);

        removed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.DeleteAsync($"/api/admin/products/{productId}/features/{feature.Id}/aliases/{aliasId}", Ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddAlias_UsedByAnotherFeatureOfTheProduct_ReturnsConflict()
    {
        var (productId, first) = await CreateFeatureAsync();
        var second = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features", new { name = UniqueName("Second") }, Ct);
        var secondId = (await second.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!.Features.Single(f => f.Id != first.Id).Id;

        var response = await _client.PostAsJsonAsync($"/api/admin/products/{productId}/features/{secondId}/aliases", new { alias = first.Name }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task DeleteFeature_RemovesIt_AndUnknownReturnsNotFound()
    {
        var (productId, feature) = await CreateFeatureAsync();

        var deleted = await _client.DeleteAsync($"/api/admin/products/{productId}/features/{feature.Id}", Ct);
        var again = await _client.DeleteAsync($"/api/admin/products/{productId}/features/{feature.Id}", Ct);

        deleted.StatusCode.Should().Be(HttpStatusCode.NoContent);
        again.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _client.GetFromJsonAsync<ProductFeatureOverview>($"/api/admin/products/{productId}/features", Ct))!.Features.Should().BeEmpty();
    }
}
