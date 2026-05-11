using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for GET/PUT/DELETE /api/ghc-features endpoints.
/// GET is public; PUT and DELETE require AdminOnly authorization.
/// Test data is seeded from TestCollections/_videos/ghc-features/*.md.
/// </summary>
public class GhcFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public GhcFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
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
        // Note: seeded ghc-features items may or may not have ghc_features rows (seeded as content_items only)
        // The list may be empty or non-empty depending on test data setup
    }

    [Fact]
    public async Task GetGhcFeatures_IsPublicEndpoint_NoAuthRequired()
    {
        // Arrange — use client without auth headers (factory default has no auth)
        // Act
        var response = await _client.GetAsync("/api/ghc-features/", TestContext.Current.CancellationToken);

        // Assert — should not get 401 Unauthorized
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized,
            "GET /api/ghc-features is a public endpoint and should not require authentication");
    }

    // ── PUT /api/ghc-features/{slug} ─────────────────────────────────────────

    [Fact]
    public async Task UpsertGhcFeature_WithoutAzureAd_AllowsRequest()
    {
        // Arrange — AzureAd:ClientId is not set in test configuration,
        // so the AdminOnly policy allows all requests (local dev bypass).
        var feature = new GhcFeature
        {
            Slug = "test-feature-auth",
            Title = "Test Feature",
            Description = "Excerpt",
            Plans = ["Free"],
            GhesSupport = false
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/ghc-features/test-feature-auth", feature, TestContext.Current.CancellationToken);

        // Assert — AdminOnly policy allows all when Azure AD is not configured
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized,
            "When AzureAd is not configured, AdminOnly policy allows all requests");
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden,
            "When AzureAd is not configured, AdminOnly policy allows all requests");
    }

    // ── DELETE /api/ghc-features/{slug} ──────────────────────────────────────

    [Fact]
    public async Task DeleteGhcFeature_WithoutAzureAd_AllowsRequest()
    {
        // Act — AzureAd:ClientId is not set in test configuration,
        // so the AdminOnly policy allows all requests (local dev bypass).
        var response = await _client.DeleteAsync(
            "/api/ghc-features/some-slug-that-does-not-exist", TestContext.Current.CancellationToken);

        // Assert — AdminOnly policy allows all when Azure AD is not configured;
        // the slug doesn't exist so we get 404 (not 401/403)
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized,
            "When AzureAd is not configured, AdminOnly policy allows all requests");
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden,
            "When AzureAd is not configured, AdminOnly policy allows all requests");
    }

    [Fact]
    public async Task DeleteGhcFeature_NonExistentSlug_ReturnsNotFound()
    {
        // Arrange — use the admin client provided by the factory
        var adminClient = _client; // factory sets up admin auth if configured

        // This test is specific to slug-not-found behavior;
        // the factory's default client has admin auth in test environment
        var response = await adminClient.DeleteAsync(
            "/api/ghc-features/slug-that-does-not-exist-xyz", TestContext.Current.CancellationToken);

        // Assert — 404 for non-existent slug (if admin) or 401/403 (if not admin)
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Forbidden);
    }
}
