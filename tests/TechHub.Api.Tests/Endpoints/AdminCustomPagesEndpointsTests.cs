using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models.Admin;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Admin custom-pages endpoints.
/// Validates CRUD operations on the custom_page_data table.
/// Data is seeded from TestCollections/_custom/*.json during startup.
/// </summary>
public class AdminCustomPagesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    // The list endpoint returns a projection (Key, Description, UpdatedAt) without JsonData.
    private sealed record CustomPageSummary(string Key, string Description, DateTimeOffset UpdatedAt);
    private readonly HttpClient _client;

    public AdminCustomPagesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    // ── GET /api/admin/custom-pages ──────────────────────────────────────────

    [Fact]
    public async Task GetCustomPages_ReturnsOkWithEntries()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/custom-pages",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var entries = await response.Content.ReadFromJsonAsync<List<CustomPageSummary>>(
            TestContext.Current.CancellationToken);
        entries.Should().NotBeNull();
        entries!.Should().NotBeEmpty("custom page data is seeded from TestCollections/_custom/*.json");
        entries.Should().AllSatisfy(e =>
        {
            e.Key.Should().NotBeNullOrEmpty();
            e.Description.Should().NotBeNull();
        });
    }

    // ── GET /api/admin/custom-pages/{key} ────────────────────────────────────

    [Fact]
    public async Task GetCustomPageByKey_WithKnownKey_ReturnsEntry()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/custom-pages/features",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var entry = await response.Content.ReadFromJsonAsync<CustomPageEntry>(
            TestContext.Current.CancellationToken);
        entry.Should().NotBeNull();
        entry!.Key.Should().Be("features");
        entry.JsonData.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetCustomPageByKey_WithUnknownKey_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/custom-pages/does-not-exist",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── PUT /api/admin/custom-pages/{key} ────────────────────────────────────

    [Fact]
    public async Task UpdateCustomPage_WithValidJson_ReturnsUpdatedEntry()
    {
        // Arrange
        var newJson = """{"title":"Updated","items":[]}""";
        var request = new { JsonData = newJson };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/custom-pages/features",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<CustomPageEntry>(
            TestContext.Current.CancellationToken);
        updated.Should().NotBeNull();
        updated!.Key.Should().Be("features");
        updated.JsonData.Should().Be(newJson);
    }

    [Fact]
    public async Task UpdateCustomPage_WithInvalidJson_ReturnsBadRequest()
    {
        // Arrange
        var request = new { JsonData = "{ not valid json }" };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/custom-pages/features",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateCustomPage_WithUnknownKey_ReturnsNotFound()
    {
        // Arrange
        var request = new { JsonData = """{"title":"test"}""" };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/custom-pages/does-not-exist",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
