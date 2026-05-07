using System.Data;
using System.Net;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the admin ghc-features plans endpoints.
/// Validates reading, updating, and deleting ghc-features content items.
/// Test data seeded from TestCollections/_videos/ghc-features/*.md.
/// </summary>
public class AdminGhcFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private sealed record GhcFeaturePlansRequest(IReadOnlyList<string> Plans, bool GhesSupport, bool Draft = false);

    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public AdminGhcFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _client = factory.CreateClient();
    }

    // ── PUT /api/admin/ghc-features/{slug}/plans ─────────────────────────────

    [Fact]
    public async Task UpdateGhcFeaturePlans_WithValidSlug_ReturnsNoContent()
    {
        // Arrange — "ghc-free-no-ghes" is seeded from TestCollections/_videos/ghc-features/
        var request = new GhcFeaturePlansRequest(["Free", "Pro"], false);

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/ghc-features/ghc-free-no-ghes/plans",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateGhcFeaturePlans_PersistsChange_VerifiableViaPublicEndpoint()
    {
        // Arrange
        var request = new GhcFeaturePlansRequest(["Free", "Student", "Pro"], true);

        // Act — update
        var putResponse = await _client.PutAsJsonAsync(
            "/api/admin/ghc-features/ghc-free-no-ghes/plans",
            request,
            TestContext.Current.CancellationToken);

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert — verify via public content endpoint that plans were persisted
        var getResponse = await _client.GetAsync(
            "/api/sections/github-copilot/collections/videos/items?subcollection=ghc-features&includeDraft=true",
            TestContext.Current.CancellationToken);

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await getResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>(
            TestContext.Current.CancellationToken);
        var items = body.GetProperty("items").EnumerateArray().ToList();
        var updated = items.FirstOrDefault(i =>
            i.GetProperty("slug").GetString() == "ghc-free-no-ghes");

        updated.ValueKind.Should().NotBe(System.Text.Json.JsonValueKind.Undefined);
        var plans = updated.GetProperty("plans").EnumerateArray()
            .Select(p => p.GetString())
            .ToList();
        plans.Should().Contain("Free");
        plans.Should().Contain("Student");
        plans.Should().Contain("Pro");
        updated.GetProperty("ghesSupport").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task UpdateGhcFeaturePlans_WithDraftTrue_PersistsDraftFlag()
    {
        // Arrange — "ghc-pro-no-ghes" starts as non-draft
        var request = new GhcFeaturePlansRequest(["Pro"], false, Draft: true);

        // Act
        var putResponse = await _client.PutAsJsonAsync(
            "/api/admin/ghc-features/ghc-pro-no-ghes/plans",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify draft=true is persisted via the includeDraft endpoint
        var getResponse = await _client.GetAsync(
            "/api/sections/github-copilot/collections/videos/items?subcollection=ghc-features&includeDraft=true",
            TestContext.Current.CancellationToken);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await getResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>(
            TestContext.Current.CancellationToken);
        var items = body.GetProperty("items").EnumerateArray().ToList();
        var updated = items.FirstOrDefault(i => i.GetProperty("slug").GetString() == "ghc-pro-no-ghes");
        updated.ValueKind.Should().NotBe(System.Text.Json.JsonValueKind.Undefined);
        updated.GetProperty("draft").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task UpdateGhcFeaturePlans_WithNonExistentSlug_ReturnsNotFound()
    {
        // Arrange
        var request = new GhcFeaturePlansRequest(["Free"], false);

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/ghc-features/does-not-exist/plans",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateGhcFeaturePlans_WithEmptyPlans_ReturnsBadRequest()
    {
        // Arrange — plans list must not be empty for ghc-features items
        var request = new GhcFeaturePlansRequest([], false);

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/ghc-features/ghc-free-no-ghes/plans",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGhcFeaturePlans_WithInvalidPlanName_ReturnsBadRequest()
    {
        // Arrange
        var request = new GhcFeaturePlansRequest(["InvalidPlan"], false);

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/ghc-features/ghc-free-no-ghes/plans",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateGhcFeaturePlans_WithNullPlans_ReturnsBadRequest()
    {
        // Arrange — send null plans
        var request = new { Plans = (object?)null, GhesSupport = false };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/ghc-features/ghc-free-no-ghes/plans",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── DELETE /api/admin/ghc-features/{slug} ────────────────────────────────

    [Fact]
    public async Task DeleteGhcFeature_WithExistingSlug_ReturnsNoContent()
    {
        // Arrange — "ghc-enterprise-ghes" is seeded from TestCollections
        // Act
        var response = await _client.DeleteAsync(
            "/api/admin/ghc-features/ghc-enterprise-ghes",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteGhcFeature_WithNonExistentSlug_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync(
            "/api/admin/ghc-features/does-not-exist",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── POST /api/admin/ghc-features/{slug}/publish ───────────────────────────

    [Fact]
    public async Task PublishGhcDraft_WithMissingUrl_ReturnsBadRequest()
    {
        // Arrange
        var request = new { YoutubeUrl = "", Plans = new[] { "Free" }, GhesSupport = false };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/ghc-features/ghc-draft-feature/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PublishGhcDraft_WithNonYouTubeUrl_ReturnsBadRequest()
    {
        // Arrange
        var request = new { YoutubeUrl = "https://example.com/video", Plans = new[] { "Free" }, GhesSupport = false };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/ghc-features/ghc-draft-feature/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PublishGhcDraft_WithEmptyPlans_ReturnsBadRequest()
    {
        // Arrange
        var request = new { YoutubeUrl = "https://youtu.be/abc123", Plans = Array.Empty<string>(), GhesSupport = false };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/ghc-features/ghc-draft-feature/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PublishGhcDraft_WithInvalidPlanName_ReturnsBadRequest()
    {
        // Arrange
        var request = new { YoutubeUrl = "https://youtu.be/abc123", Plans = new[] { "InvalidPlan" }, GhesSupport = false };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/ghc-features/ghc-draft-feature/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PublishGhcDraft_WithTranscriptTooLong_ReturnsBadRequest()
    {
        // Arrange — transcript exceeds 50,000 character limit
        var request = new
        {
            YoutubeUrl = "https://youtu.be/abc123",
            Transcript = new string('x', 50_001),
            Plans = new[] { "Free" },
            GhesSupport = false
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/ghc-features/ghc-draft-feature/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PublishGhcDraft_WithNonExistentSlug_ReturnsNotFound()
    {
        // Arrange
        var request = new { YoutubeUrl = "https://youtu.be/abc123", Plans = new[] { "Free" }, GhesSupport = false };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/ghc-features/does-not-exist/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PublishGhcDraft_WithValidRequest_ReturnsOkAndSetsDraftFalse()
    {
        // Arrange — seed a fresh draft item so this test does not mutate shared seed data
        const string TestSlug = "ghc-publish-in-place-success-test";
        const string PlaceholderUrl = "https://youtu.be/publish-inplace-placeholder";
        const string YoutubeUrl = "https://www.youtube.com/watch?v=publishsuccessv";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO content_items
                    (slug, collection_name, subcollection_name, title, content, excerpt,
                     date_epoch, primary_section_name, external_url, author, feed_name,
                     tags_csv, is_github_copilot, sections_bitmask, content_hash, draft)
                  VALUES
                    (@Slug, 'videos', 'ghc-features', 'Publish In Place Test', '# Test', 'Test excerpt',
                     1717200000, 'github-copilot', @Url, 'Test Author', 'Test Feed',
                     ',GitHub Copilot,', TRUE, 16, 'testhash-publish', TRUE)
                  ON CONFLICT (external_url) DO NOTHING",
                new { Slug = TestSlug, Url = PlaceholderUrl });
        }

        // Mock AI categorization to return a successful result without calling a real API
        var mockAi = new Mock<IAiCategorizationService>();
        mockAi
            .Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult
            {
                Item = new ProcessedContentItem
                {
                    Slug = TestSlug,
                    Title = "Publish In Place Test",
                    Excerpt = "Test excerpt",
                    DateEpoch = 1717200000L,
                    CollectionName = "videos",
                    SubcollectionName = "ghc-features",
                    ExternalUrl = YoutubeUrl,
                    Author = "Test Author",
                    FeedName = "Test Feed",
                    Tags = ["GitHub Copilot"],
                    Sections = ["github-copilot"],
                    PrimarySectionName = "github-copilot",
                    ContentHash = "testhash-publish"
                },
                Explanation = "Test categorization",
                IsFailure = false
            });

        using var client = _factory
            .WithWebHostBuilder(b => b.ConfigureTestServices(services =>
                services.AddTransient<IAiCategorizationService>(_ => mockAi.Object)))
            .CreateClient();

        var request = new { YoutubeUrl, Plans = new[] { "Free" }, GhesSupport = false };

        // Act
        var response = await client.PostAsJsonAsync(
            $"/api/admin/ghc-features/{TestSlug}/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert — published successfully
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify the item is no longer a draft by confirming it appears without includeDraft
        var getResponse = await client.GetAsync(
            "/api/sections/github-copilot/collections/videos/items?subcollection=ghc-features",
            TestContext.Current.CancellationToken);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await getResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>(
            TestContext.Current.CancellationToken);
        var items = body.GetProperty("items").EnumerateArray().ToList();
        items.Any(i => i.GetProperty("slug").GetString() == TestSlug)
            .Should().BeTrue("published item should be visible without includeDraft");
    }

    [Fact]
    public async Task PublishGhcDraft_WithAlreadyClaimedUrl_ReturnsConflict()
    {
        // Arrange — seed a processed_urls row owned by a different slug so the conflict
        // check fires when we try to publish the draft with that same URL.
        // Use an existing seed slug as the owner to satisfy the FK constraint.
        const string ConflictUrl = "https://www.youtube.com/watch?v=conflict409test";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO processed_urls (external_url, status, feed_name, collection_name, slug)
                  VALUES (@Url, 'succeeded', 'Test Feed', 'videos', 'ghc-free-no-ghes')
                  ON CONFLICT (external_url) DO NOTHING",
                new { Url = ConflictUrl });
        }

        // ghc-draft-feature has placeholder URL; using ConflictUrl (different) triggers the check
        var request = new { YoutubeUrl = ConflictUrl, Plans = new[] { "Free" }, GhesSupport = false };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/ghc-features/ghc-draft-feature/publish",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}

