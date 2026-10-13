using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for POST /api/admin/content-fixer/trigger.
/// Kept in its own class (own database) because the fixer rewrites content in the background.
/// </summary>
public class AdminContentFixerEndpointTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AdminContentFixerEndpointTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TriggerContentFixer_ReturnsAcceptedAndBackgroundServiceRunsCleanupJob()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/content-fixer/trigger",
            null,
            TestContext.Current.CancellationToken);

        // Assert — the endpoint accepts immediately
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("message").GetString().Should().Be("Content fixer run triggered");

        // Assert — the background service picked up the signal and ran a manual cleanup job to completion.
        // The service exposes no completion hook, so poll the job history (bounded).
        ContentProcessingJob? job = null;
        var finished = await Eventually.WaitAsync(
            async () =>
            {
                var jobs = await _client.GetFromJsonAsync<List<ContentProcessingJob>>(
                    "/api/admin/processing/jobs?count=100",
                    TestContext.Current.CancellationToken);
                job = jobs!.FirstOrDefault(j =>
                    j.JobType == ContentProcessingJobType.ContentCleanup
                    && j.TriggerType == "manual"
                    && j.Status != ContentProcessingJobStatus.Running);
                return job is not null;
            },
            TimeSpan.FromSeconds(30),
            TestContext.Current.CancellationToken);

        finished.Should().BeTrue("the triggered content fixer run should finish");
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Completed, "job log: {0}", job.LogOutput);
    }
}
