using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the POST /api/admin/urls/process endpoint.
/// Validates request validation (URL format, collection, ghc-feature rules).
/// Note: actual AI / transcript processing is not tested here — that requires a live AI service.
/// These tests cover the request boundary validation layer.
/// </summary>
public class AdminProcessUrlEndpointTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AdminProcessUrlEndpointTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    // ── POST /api/admin/urls/process ─────────────────────────────────────────

    [Fact]
    public async Task ProcessUrl_WithMissingUrl_ReturnsBadRequest()
    {
        // Arrange
        var request = new { Url = (string?)null, CollectionName = "videos", FeedName = "Test Feed" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessUrl_WithInvalidUrl_ReturnsBadRequest()
    {
        // Arrange — not a valid HTTP URL
        var request = new { Url = "not-a-url", CollectionName = "videos", FeedName = "Test Feed" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessUrl_WithInvalidCollection_ReturnsBadRequest()
    {
        // Arrange
        var request = new { Url = "https://www.youtube.com/watch?v=abc123", CollectionName = "invalid-collection", FeedName = "Test Feed" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessUrl_GhcFeatureWithNonYouTubeUrl_ReturnsBadRequest()
    {
        // Arrange — ghc-features requires a YouTube URL
        var request = new
        {
            Url = "https://github.com/some/article",
            CollectionName = "videos",
            FeedName = "Test Feed",
            IsGhcFeature = true,
            Plans = new[] { "Free" },
            GhesSupport = false
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessUrl_GhcFeatureWithEmptyPlans_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            Url = "https://www.youtube.com/watch?v=abc123",
            CollectionName = "videos",
            FeedName = "Test Feed",
            IsGhcFeature = true,
            Plans = Array.Empty<string>(),
            GhesSupport = false
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessUrl_GhcFeatureWithInvalidPlanName_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            Url = "https://www.youtube.com/watch?v=abc123",
            CollectionName = "videos",
            FeedName = "Test Feed",
            IsGhcFeature = true,
            Plans = new[] { "InvalidPlan" },
            GhesSupport = false
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessUrl_AlreadyExistingUrl_ReturnsConflict()
    {
        // Arrange — use a URL that is seeded from TestCollections/_videos/ghc-features/
        // The seed data uses youtu.be/abc123test1 as external_url for ghc-free-no-ghes
        var request = new
        {
            Url = "https://youtu.be/abc123test1",
            CollectionName = "videos",
            FeedName = "Test Feed"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ProcessUrl_WithEmptyFeedName_DefaultsToTechHub_ReturnsConflict()
    {
        // Arrange — empty FeedName should be accepted (defaults to "Tech Hub"); use a known duplicate URL
        var request = new { Url = "https://youtu.be/abc123test1", CollectionName = "videos", FeedName = "" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert — 409 means validation passed and the URL was found as a duplicate
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
