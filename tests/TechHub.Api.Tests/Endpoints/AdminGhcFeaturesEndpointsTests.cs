using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
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

    public AdminGhcFeaturesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
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
}

