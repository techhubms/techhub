using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the feature endpoints under /api/admin/products/{productId}/features. Names are unique per test
/// because the database is shared by the tests of this class.
/// </summary>
public class AdminFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private const string VideosCollection = "videos";

    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public AdminFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
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

    // ── Details ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateDetails_StoresTheDescriptionAndTheReleaseDate_AndNullClearsTheDate()
    {
        var (productId, feature) = await CreateFeatureAsync();

        var response = await _client.PutAsJsonAsync($"{FeatureUrl(productId, feature.Id)}/details", new { description = "  Described  ", releaseDate = "2026-03-10" }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stored = (await response.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!.Features.Single();
        stored.Description.Should().Be("Described");
        stored.ReleaseDate.Should().Be(new DateOnly(2026, 3, 10));

        var cleared = await _client.PutAsJsonAsync($"{FeatureUrl(productId, feature.Id)}/details", new { description = "", releaseDate = (string?)null }, Ct);

        var clearedFeature = (await cleared.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!.Features.Single();
        clearedFeature.Description.Should().BeEmpty();
        clearedFeature.ReleaseDate.Should().BeNull();
    }

    [Fact]
    public async Task UpdateDetails_WithATooLongDescription_ReturnsBadRequest()
    {
        var (productId, feature) = await CreateFeatureAsync();

        var response = await _client.PutAsJsonAsync($"{FeatureUrl(productId, feature.Id)}/details", new { description = new string('x', 4001) }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateDetails_OfAnUnknownFeature_ReturnsNotFound()
    {
        var productId = await CreateProductAsync();

        var response = await _client.PutAsJsonAsync($"{FeatureUrl(productId, long.MaxValue)}/details", new { description = "Text" }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Content ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task AddContent_LinksTheItemAndReturnsTheOverview_AndASecondThumbnailReplacesTheFirst()
    {
        var (productId, feature) = await CreateFeatureAsync();
        var first = await SeedVideoAsync();
        var second = await SeedVideoAsync();

        var firstOverview = await AddContentAsync(productId, feature.Id, first, isThumbnail: true);
        var secondOverview = await AddContentAsync(productId, feature.Id, second, isThumbnail: true);

        var link = firstOverview.Features.Single().Content.Should().ContainSingle().Subject;
        link.CollectionName.Should().Be(VideosCollection);
        link.ItemSlug.Should().Be(first);
        link.Title.Should().Be($"Video {first}");
        link.IsThumbnail.Should().BeTrue();
        var links = secondOverview.Features.Single().Content;
        links.Should().HaveCount(2);
        links.Single(l => l.ItemSlug == first).IsThumbnail.Should().BeFalse();
        links.Single(l => l.ItemSlug == second).IsThumbnail.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "some-item")]
    [InlineData("videos", "")]
    [InlineData("  ", "  ")]
    public async Task AddContent_WithBlankValues_ReturnsBadRequest(string collectionName, string itemSlug)
    {
        var (productId, feature) = await CreateFeatureAsync();

        var response = await _client.PostAsJsonAsync($"{FeatureUrl(productId, feature.Id)}/content", new { collectionName, itemSlug }, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddContent_ForAnUnknownItemOrFeature_ReturnsNotFoundAndLinksNothing()
    {
        var (productId, feature) = await CreateFeatureAsync();
        var video = await SeedVideoAsync();

        var unknownItem = await _client.PostAsJsonAsync($"{FeatureUrl(productId, feature.Id)}/content", new { collectionName = VideosCollection, itemSlug = $"missing-{Guid.NewGuid():N}" }, Ct);
        var unknownFeature = await _client.PostAsJsonAsync($"{FeatureUrl(productId, long.MaxValue)}/content", new { collectionName = VideosCollection, itemSlug = video }, Ct);

        unknownItem.StatusCode.Should().Be(HttpStatusCode.NotFound);
        unknownFeature.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _client.GetFromJsonAsync<ProductFeatureOverview>($"/api/admin/products/{productId}/features", Ct))!.Features.Single().Content.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?itemSlug=some-item")]
    [InlineData("?collection=%20&itemSlug=%20")]
    public async Task RemoveContent_MissingOrBlankQuery_ReturnsBadRequest(string query)
    {
        var (productId, feature) = await CreateFeatureAsync();

        var response = await _client.DeleteAsync($"{FeatureUrl(productId, feature.Id)}/content{query}", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RemoveContent_RemovesOnlyThatLink_AndAnUnlinkedItemReturnsNotFound()
    {
        var (productId, feature) = await CreateFeatureAsync();
        var removed = await SeedVideoAsync();
        var kept = await SeedVideoAsync();
        await AddContentAsync(productId, feature.Id, removed);
        await AddContentAsync(productId, feature.Id, kept);

        var response = await _client.DeleteAsync($"{FeatureUrl(productId, feature.Id)}/content?collection={VideosCollection}&itemSlug={Uri.EscapeDataString(removed)}", Ct);
        var again = await _client.DeleteAsync($"{FeatureUrl(productId, feature.Id)}/content?collection={VideosCollection}&itemSlug={Uri.EscapeDataString(removed)}", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        again.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _client.GetFromJsonAsync<ProductFeatureOverview>($"/api/admin/products/{productId}/features", Ct))!
            .Features.Single().Content.Select(c => c.ItemSlug).Should().Equal(kept);
    }

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?itemSlug=some-item")]
    [InlineData("?collection=%20&itemSlug=%20")]
    public async Task SetThumbnail_MissingOrBlankQuery_ReturnsBadRequest(string query)
    {
        var (productId, feature) = await CreateFeatureAsync();

        var response = await _client.PutAsync($"{FeatureUrl(productId, feature.Id)}/thumbnail{query}", null, Ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SetThumbnail_MovesTheThumbnail_AndAnUnlinkedItemReturnsNotFound()
    {
        var (productId, feature) = await CreateFeatureAsync();
        var current = await SeedVideoAsync();
        var next = await SeedVideoAsync();
        var unlinked = await SeedVideoAsync();
        await AddContentAsync(productId, feature.Id, current, isThumbnail: true);
        await AddContentAsync(productId, feature.Id, next);

        var unknown = await _client.PutAsync($"{FeatureUrl(productId, feature.Id)}/thumbnail?collection={VideosCollection}&itemSlug={Uri.EscapeDataString(unlinked)}", null, Ct);
        var moved = await _client.PutAsync($"{FeatureUrl(productId, feature.Id)}/thumbnail?collection={VideosCollection}&itemSlug={Uri.EscapeDataString(next)}", null, Ct);

        unknown.StatusCode.Should().Be(HttpStatusCode.NotFound);
        moved.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var content = (await _client.GetFromJsonAsync<ProductFeatureOverview>($"/api/admin/products/{productId}/features", Ct))!.Features.Single().Content;
        content.Single(c => c.IsThumbnail).ItemSlug.Should().Be(next);
    }

    [Fact]
    public async Task GetUnmatchedSummary_ReturnsAList()
    {
        var response = await _client.GetAsync("/api/admin/products/features/unmatched", Ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<ProductUnmatchedFeatures>>(Ct)).Should().NotBeNull();
    }

    private static string FeatureUrl(long productId, long id) => $"/api/admin/products/{productId}/features/{id}";

    private async Task<string> SeedVideoAsync()
    {
        var slug = $"feature-video-{Guid.NewGuid():N}";
        await ContentItemSeeder.InsertAsync(_factory.Services, VideosCollection, slug, $"/all/videos/{slug}", title: $"Video {slug}");
        return slug;
    }

    private async Task<ProductFeatureOverview> AddContentAsync(long productId, long id, string itemSlug, bool isThumbnail = false)
    {
        var response = await _client.PostAsJsonAsync($"{FeatureUrl(productId, id)}/content", new { collectionName = VideosCollection, itemSlug, isThumbnail }, Ct);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return (await response.Content.ReadFromJsonAsync<ProductFeatureOverview>(Ct))!;
    }
}
