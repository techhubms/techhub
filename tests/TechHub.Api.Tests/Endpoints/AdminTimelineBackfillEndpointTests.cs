using System.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for POST /api/admin/timeline-backfill/trigger.
/// AI extraction is replaced by a stateless stub (see <see cref="TechHubStubbedExternalServicesApiFactory"/>).
/// </summary>
public class AdminTimelineBackfillEndpointTests : IClassFixture<TechHubStubbedExternalServicesApiFactory>
{
    private readonly HttpClient _client;
    private readonly TechHubStubbedExternalServicesApiFactory _factory;

    public AdminTimelineBackfillEndpointTests(TechHubStubbedExternalServicesApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("""{"publishedAfter":"31-12-2025"}""")]
    [InlineData("""{"publishedAfter":"yesterday"}""")]
    [InlineData("""{"limit":0}""")]
    [InlineData("""{"limit":-5}""")]
    [InlineData("""{"collection":"news; DROP TABLE content_items"}""")]
    public async Task Trigger_WithInvalidOptions_ReturnsBadRequest(string body)
    {
        using var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/admin/timeline-backfill/trigger", content, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Trigger_ReturnsAcceptedAndRunsATimelineBackfillJobThatStoresEvents()
    {
        // Act — a small run: two articles, newest first
        var response = await _client.PostAsJsonAsync(
            "/api/admin/timeline-backfill/trigger",
            new { limit = 2 },
            TestContext.Current.CancellationToken);

        // Assert — accepted immediately
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("message").GetString().Should().Be("Timeline backfill triggered");

        // Assert — the background service ran a manual timeline-backfill job to completion
        ContentProcessingJob? job = null;
        var finished = await Eventually.WaitAsync(
            async () =>
            {
                var jobs = await _client.GetFromJsonAsync<List<ContentProcessingJob>>(
                    "/api/admin/processing/jobs?count=100",
                    TestContext.Current.CancellationToken);
                job = jobs!.FirstOrDefault(j =>
                    j.JobType == ContentProcessingJobType.TimelineBackfill
                    && j.TriggerType == "manual"
                    && j.Status != ContentProcessingJobStatus.Running);
                return job is not null;
            },
            TimeSpan.FromSeconds(60),
            TestContext.Current.CancellationToken);

        finished.Should().BeTrue("the triggered timeline backfill should finish");
        var detail = await _client.GetFromJsonAsync<ContentProcessingJob>(
            $"/api/admin/processing/jobs/{job!.Id}", TestContext.Current.CancellationToken);
        detail!.Status.Should().Be(ContentProcessingJobStatus.Completed, "job log: {0}", detail.LogOutput);
        detail.ItemsAdded.Should().Be(2, "the stub reports one event per article");
        detail.LogOutput.Should().Contain("2 articles to scan");

        using var scope = _factory.Services.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var stored = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM content_timeline_events WHERE extracted_from = 'stored-content'");
        stored.Should().Be(2);
    }

    [Fact]
    public async Task Cancel_WithoutRunningJob_ReportsNothingToCancel()
    {
        var response = await _client.PostAsync("/api/admin/processing/cancel", null, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
