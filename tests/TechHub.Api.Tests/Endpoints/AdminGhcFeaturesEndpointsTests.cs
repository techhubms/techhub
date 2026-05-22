using System.Net;
using FluentAssertions;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the admin ghc-features endpoints.
/// Validates deleting ghc-features content items.
/// Test data seeded from TestCollections/_videos/ghc-features/*.md.
/// </summary>
public class AdminGhcFeaturesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AdminGhcFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
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
}
