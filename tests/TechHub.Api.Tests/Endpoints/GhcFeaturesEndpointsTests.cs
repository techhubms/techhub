using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the /api/ghc-features endpoints (feature CRUD, content links, thumbnail).
/// GET is public; all writes require AdminOnly authorization (enforcement is covered by the admin authorization tests).
/// Test data is seeded from TestCollections/_videos/ghc-features/*.md; every write test creates
/// its own uniquely named feature / content items so tests are independent of each other.
/// </summary>
public class GhcFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private const string VideosCollection = "videos";

    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public GhcFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _client = factory.CreateClient();
    }

    // ── GET /api/ghc-features ────────────────────────────────────────────────

    [Fact]
    public async Task GetGhcFeatures_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/ghc-features/", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetGhcFeatures_ReturnsJsonArray()
    {
        // Act
        var features = await _client.GetFromJsonAsync<List<GhcFeature>>(
            "/api/ghc-features/", TestContext.Current.CancellationToken);

        // Assert
        features.Should().NotBeNull("endpoint should return a JSON array");
        features.Should().NotBeEmpty("endpoint should return seeded GHC feature entries from TestCollections");
    }

    // ── PUT /api/ghc-features/{slug} ─────────────────────────────────────────

    [Fact]
    public async Task UpsertGhcFeature_NewFeature_ReturnsNoContentAndPersistsAllFields()
    {
        // Arrange
        var slug = NewFeatureSlug();
        var feature = new GhcFeature
        {
            Slug = slug,
            Title = "Persisted Feature",
            Description = "Persisted description",
            ReleaseDate = 1700000000,
            Plans = ["Free", "Pro"],
            GhesSupport = true
        };

        // Act
        var response = await PutFeatureAsync(feature);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await GetFeatureAsync(slug);
        stored.Should().NotBeNull("the feature must be returned by GET after a successful PUT");
        stored!.Title.Should().Be("Persisted Feature");
        stored.Description.Should().Be("Persisted description");
        stored.ReleaseDate.Should().Be(1700000000);
        stored.Plans.Should().BeEquivalentTo(["Free", "Pro"]);
        stored.GhesSupport.Should().BeTrue();
    }

    [Fact]
    public async Task UpsertGhcFeature_ExistingFeature_UpdatesInPlace()
    {
        // Arrange
        var slug = await CreateFeatureAsync(title: "Original Title");
        var updated = new GhcFeature
        {
            Slug = slug,
            Title = "Updated Title",
            Description = "Updated description",
            Plans = ["Enterprise"],
            GhesSupport = true
        };

        // Act
        var response = await PutFeatureAsync(updated);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var all = await GetAllFeaturesAsync();
        var matches = all.Where(f => f.Slug == slug).ToList();
        matches.Should().ContainSingle("PUT on an existing slug must update, not duplicate");
        matches[0].Title.Should().Be("Updated Title");
        matches[0].Description.Should().Be("Updated description");
        matches[0].Plans.Should().BeEquivalentTo(["Enterprise"]);
        matches[0].GhesSupport.Should().BeTrue();
    }

    [Fact]
    public async Task UpsertGhcFeature_SlugMismatch_ReturnsBadRequestAndDoesNotPersist()
    {
        // Arrange
        var urlSlug = NewFeatureSlug();
        var bodySlug = NewFeatureSlug();
        var feature = new GhcFeature { Slug = bodySlug, Title = "Mismatch", Plans = [] };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/ghc-features/{urlSlug}", feature, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await GetFeatureAsync(urlSlug)).Should().BeNull();
        (await GetFeatureAsync(bodySlug)).Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpsertGhcFeature_BlankTitle_ReturnsBadRequestAndDoesNotPersist(string title)
    {
        // Arrange
        var slug = NewFeatureSlug();
        var feature = new GhcFeature { Slug = slug, Title = title, Plans = [] };

        // Act
        var response = await PutFeatureAsync(feature);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await GetFeatureAsync(slug)).Should().BeNull();
    }

    [Fact]
    public async Task UpsertGhcFeature_NullPlans_ReturnsBadRequestAndDoesNotPersist()
    {
        // Arrange — an explicit JSON null for Plans must be rejected (empty array is the valid "no plans" value)
        var slug = NewFeatureSlug();
        var body = new { Slug = slug, Title = "Null Plans", Plans = (string[]?)null };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/ghc-features/{slug}", body, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await GetFeatureAsync(slug)).Should().BeNull();
    }

    // ── DELETE /api/ghc-features/{slug} ──────────────────────────────────────

    [Fact]
    public async Task DeleteGhcFeature_ExistingFeature_ReturnsNoContentAndRemovesIt()
    {
        // Arrange
        var slug = await CreateFeatureAsync();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/ghc-features/{slug}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await GetFeatureAsync(slug)).Should().BeNull("a deleted feature must no longer be returned");

        var secondDelete = await _client.DeleteAsync(
            $"/api/ghc-features/{slug}", TestContext.Current.CancellationToken);
        secondDelete.StatusCode.Should().Be(HttpStatusCode.NotFound, "the feature no longer exists");
    }

    [Fact]
    public async Task DeleteGhcFeature_NonExistentSlug_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync(
            $"/api/ghc-features/{NewFeatureSlug()}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── POST /api/ghc-features/{slug}/content-links ────────────────────────────────

    [Theory]
    [InlineData("", "some-item")]
    [InlineData("videos", "")]
    [InlineData("  ", "  ")]
    public async Task AddContentLink_BlankFields_ReturnsBadRequest(string collectionName, string itemSlug)
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/ghc-features/{featureSlug}/content-links",
            new { CollectionName = collectionName, ItemSlug = itemSlug },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddContentLink_MissingItemSlug_ReturnsBadRequest()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();

        // Act — ItemSlug is a required member of the request DTO
        var response = await _client.PostAsJsonAsync(
            $"/api/ghc-features/{featureSlug}/content-links",
            new { CollectionName = VideosCollection },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddContentLink_UnknownFeature_ReturnsNotFound()
    {
        // Arrange
        var itemSlug = await SeedVideoAsync();

        // Act
        var response = await PostContentLinkAsync(NewFeatureSlug(), itemSlug);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddContentLink_UnknownContentItem_ReturnsNotFoundAndAddsNoLink()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();

        // Act
        var response = await PostContentLinkAsync(featureSlug, $"missing-{NewId()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var stored = await GetFeatureAsync(featureSlug);
        stored!.ContentLinks.Should().BeEmpty();
    }

    [Fact]
    public async Task AddContentLink_ExistingFeatureAndItem_ReturnsNoContentAndLinkAppearsOnFeature()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();
        var itemSlug = await SeedVideoAsync();

        // Act
        var response = await PostContentLinkAsync(featureSlug, itemSlug, isThumbnail: true, sortOrder: 3);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await GetFeatureAsync(featureSlug);
        var link = stored!.ContentLinks.Should().ContainSingle().Subject;
        link.CollectionName.Should().Be(VideosCollection);
        link.ItemSlug.Should().Be(itemSlug);
        link.IsThumbnail.Should().BeTrue();
        link.SortOrder.Should().Be(3);
        stored.ThumbnailLink!.ItemSlug.Should().Be(itemSlug);
    }

    [Fact]
    public async Task AddContentLink_SecondThumbnail_ReplacesFirstThumbnail()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();
        var first = await SeedVideoAsync();
        var second = await SeedVideoAsync();
        (await PostContentLinkAsync(featureSlug, first, isThumbnail: true, sortOrder: 1)).EnsureSuccessStatusCode();

        // Act
        var response = await PostContentLinkAsync(featureSlug, second, isThumbnail: true, sortOrder: 2);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await GetFeatureAsync(featureSlug);
        stored!.ContentLinks.Should().HaveCount(2);
        stored.ContentLinks.Single(l => l.ItemSlug == first).IsThumbnail.Should().BeFalse();
        stored.ContentLinks.Single(l => l.ItemSlug == second).IsThumbnail.Should().BeTrue();
    }

    // ── DELETE /api/ghc-features/{slug}/content-links ────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?itemSlug=some-item")]
    [InlineData("?collection=%20&itemSlug=%20")]
    public async Task RemoveContentLink_MissingOrBlankQuery_ReturnsBadRequest(string query)
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/ghc-features/{featureSlug}/content-links{query}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RemoveContentLink_LinkDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();
        var itemSlug = await SeedVideoAsync();

        // Act — the item exists but was never linked to this feature
        var response = await DeleteContentLinkAsync(featureSlug, itemSlug);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveContentLink_ExistingLink_ReturnsNoContentAndRemovesOnlyThatLink()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();
        var removed = await SeedVideoAsync();
        var kept = await SeedVideoAsync();
        (await PostContentLinkAsync(featureSlug, removed)).EnsureSuccessStatusCode();
        (await PostContentLinkAsync(featureSlug, kept)).EnsureSuccessStatusCode();

        // Act
        var response = await DeleteContentLinkAsync(featureSlug, removed);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await GetFeatureAsync(featureSlug);
        stored!.ContentLinks.Should().ContainSingle(l => l.ItemSlug == kept);
        stored.ContentLinks.Should().NotContain(l => l.ItemSlug == removed);
    }

    // ── PUT /api/ghc-features/{slug}/thumbnail ────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?itemSlug=some-item")]
    [InlineData("?collection=%20&itemSlug=%20")]
    public async Task SetThumbnail_MissingOrBlankQuery_ReturnsBadRequest(string query)
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();

        // Act
        var response = await _client.PutAsync(
            $"/api/ghc-features/{featureSlug}/thumbnail{query}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SetThumbnail_LinkDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();
        var itemSlug = await SeedVideoAsync();

        // Act — the item exists but is not linked to this feature
        var response = await SetThumbnailAsync(featureSlug, itemSlug);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetThumbnail_UnknownFeature_ReturnsNotFound()
    {
        // Act
        var response = await SetThumbnailAsync(NewFeatureSlug(), $"missing-{NewId()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetThumbnail_ExistingLink_ReturnsNoContentAndMovesThumbnail()
    {
        // Arrange
        var featureSlug = await CreateFeatureAsync();
        var current = await SeedVideoAsync();
        var next = await SeedVideoAsync();
        (await PostContentLinkAsync(featureSlug, current, isThumbnail: true, sortOrder: 1)).EnsureSuccessStatusCode();
        (await PostContentLinkAsync(featureSlug, next, isThumbnail: false, sortOrder: 2)).EnsureSuccessStatusCode();

        // Act
        var response = await SetThumbnailAsync(featureSlug, next);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await GetFeatureAsync(featureSlug);
        stored!.ThumbnailLink!.ItemSlug.Should().Be(next);
        stored.ContentLinks.Single(l => l.ItemSlug == current).IsThumbnail.Should().BeFalse();
    }

    // ── Helpers ─────────────────────────────────────────────────────────────────────────────

    private static string NewId() => Guid.NewGuid().ToString("N");

    private static string NewFeatureSlug() => $"ghc-test-{NewId()}";

    private async Task<string> CreateFeatureAsync(string title = "Test Feature")
    {
        var slug = NewFeatureSlug();
        var response = await PutFeatureAsync(new GhcFeature { Slug = slug, Title = title, Plans = [] });
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        return slug;
    }

    private async Task<string> SeedVideoAsync()
    {
        var slug = $"ghc-link-{NewId()}";
        await ContentItemSeeder.InsertAsync(
            _factory.Services, VideosCollection, slug, $"/all/videos/{slug}");
        return slug;
    }

    private Task<HttpResponseMessage> PutFeatureAsync(GhcFeature feature) =>
        _client.PutAsJsonAsync(
            $"/api/ghc-features/{feature.Slug}", feature, TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> PostContentLinkAsync(
        string featureSlug,
        string itemSlug,
        bool isThumbnail = false,
        int sortOrder = 0) =>
        _client.PostAsJsonAsync(
            $"/api/ghc-features/{featureSlug}/content-links",
            new { CollectionName = VideosCollection, ItemSlug = itemSlug, IsThumbnail = isThumbnail, SortOrder = sortOrder },
            TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> DeleteContentLinkAsync(string featureSlug, string itemSlug) =>
        _client.DeleteAsync(
            $"/api/ghc-features/{featureSlug}/content-links?collection={VideosCollection}&itemSlug={Uri.EscapeDataString(itemSlug)}",
            TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> SetThumbnailAsync(string featureSlug, string itemSlug) =>
        _client.PutAsync(
            $"/api/ghc-features/{featureSlug}/thumbnail?collection={VideosCollection}&itemSlug={Uri.EscapeDataString(itemSlug)}",
            null,
            TestContext.Current.CancellationToken);

    private async Task<List<GhcFeature>> GetAllFeaturesAsync()
    {
        var features = await _client.GetFromJsonAsync<List<GhcFeature>>(
            "/api/ghc-features/", TestContext.Current.CancellationToken);
        return features!;
    }

    private async Task<GhcFeature?> GetFeatureAsync(string slug)
    {
        var features = await GetAllFeaturesAsync();
        return features.SingleOrDefault(f => f.Slug == slug);
    }
}
