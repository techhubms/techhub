using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for GET /api/admin/statistics and POST /api/admin/processing/cancel.
/// No test in this class starts a background job, so the cancel endpoint always observes an idle system.
/// </summary>
public class AdminJobControlEndpointTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AdminJobControlEndpointTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    // ── POST /api/admin/processing/cancel ────────────────────────────────────

    [Fact]
    public async Task CancelRunningJob_WhenNoJobIsRunning_ReturnsOkWithNothingToCancelMessage()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/processing/cancel",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("message").GetString().Should().Be("No running job to cancel");
    }

    // ── GET /api/admin/statistics ────────────────────────────────────────────

    [Fact]
    public async Task GetStatistics_ReturnsStatisticsForSeededDatabase()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/statistics",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stats = await response.Content.ReadFromJsonAsync<DatabaseStatistics>(
            TestContext.Current.CancellationToken);
        stats.Should().NotBeNull();
        stats!.TotalContentItems.Should().BeGreaterThan(0, "the integration database is seeded from TestCollections");
        stats.ItemsByCollection.Sum(c => c.Count).Should().Be(stats.TotalContentItems,
            "every content item belongs to exactly one collection");
        stats.ItemsByCollection.Should().Contain(c => c.CollectionName == "blogs");
        stats.LatestItems.Should().NotBeEmpty();
        stats.LatestItems.Count.Should().BeLessThanOrEqualTo(10);
        stats.DatabaseSize.DatabaseName.Should().Be("techhub_test");
        stats.GeneratedAt.Should().BeAfter(DateTimeOffset.UtcNow.AddMinutes(-5));
    }
}
