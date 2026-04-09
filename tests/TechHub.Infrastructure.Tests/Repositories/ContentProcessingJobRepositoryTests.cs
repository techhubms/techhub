using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.Admin;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="ContentProcessingJobRepository"/>.
/// Uses PostgreSQL via Testcontainers to verify Dapper column mapping and CRUD operations.
/// </summary>
public class ContentProcessingJobRepositoryTests
    : IClassFixture<DatabaseFixture<ContentProcessingJobRepositoryTests>>
{
    private readonly ContentProcessingJobRepository _repository;

    public ContentProcessingJobRepositoryTests(DatabaseFixture<ContentProcessingJobRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _repository = new ContentProcessingJobRepository(
            fixture.Connection,
            new PostgresConnectionFactory(fixture.ConnectionString),
            NullLogger<ContentProcessingJobRepository>.Instance);
    }

    // ── CreateAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ReturnsPositiveJobId()
    {
        // Act
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Assert
        jobId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync_SetsStartedAtToCurrentTime()
    {
        // Arrange
        var before = DateTimeOffset.UtcNow.AddSeconds(-2);

        // Act
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.StartedAt.Should().BeAfter(before);
        job.StartedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task CreateAsync_SetsStatusToRunning()
    {
        // Act
        var jobId = await _repository.CreateAsync("scheduled", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Running);
    }

    [Fact]
    public async Task CreateAsync_SetsTriggerType()
    {
        // Act
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.TriggerType.Should().Be("manual");
    }

    [Fact]
    public async Task CreateAsync_DefaultsJobTypeToContentProcessing()
    {
        // Act
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.JobType.Should().Be(ContentProcessingJobType.ContentProcessing);
    }

    [Fact]
    public async Task CreateAsync_WithRoundupJobType_SetsJobType()
    {
        // Act
        var jobId = await _repository.CreateAsync("scheduled", ContentProcessingJobType.RoundupGeneration, CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.JobType.Should().Be(ContentProcessingJobType.RoundupGeneration);
    }

    // ── CompleteAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task CompleteAsync_SetsAllStatistics()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        await _repository.CompleteAsync(jobId, feedsProcessed: 5, itemsAdded: 10,
            itemsSkipped: 3, errorCount: 2, transcriptsSucceeded: 0, transcriptsFailed: 0, logOutput: "test log", ct: CancellationToken.None);

        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Completed);
        job.FeedsProcessed.Should().Be(5);
        job.ItemsAdded.Should().Be(10);
        job.ItemsSkipped.Should().Be(3);
        job.ErrorCount.Should().Be(2);
        job.LogOutput.Should().Be("test log");
    }

    [Fact]
    public async Task CompleteAsync_SetsCompletedAtAndDuration()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("scheduled", ct: CancellationToken.None);

        // Act
        await _repository.CompleteAsync(jobId, 1, 0, 0, 0, 0, 0, "done", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.CompletedAt.Should().NotBeNull();
        job.CompletedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
        job.DurationMs.Should().NotBeNull();
        job.DurationMs.Should().BeGreaterThanOrEqualTo(0);
    }

    // ── FailAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task FailAsync_SetsStatusToFailed()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        await _repository.FailAsync(jobId, feedsProcessed: 2, itemsAdded: 3, itemsSkipped: 5, errorCount: 1, transcriptsSucceeded: 0, transcriptsFailed: 0, "error log", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Failed);
        job.LogOutput.Should().Be("error log");
        job.CompletedAt.Should().NotBeNull();
        job.DurationMs.Should().NotBeNull();
        job.FeedsProcessed.Should().Be(2);
        job.ItemsAdded.Should().Be(3);
        job.ItemsSkipped.Should().Be(5);
        job.ErrorCount.Should().Be(1);
    }

    // ── UpdateLogAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateLogAsync_ReplacesLogOutput()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        await _repository.UpdateLogAsync(jobId, "first flush", CancellationToken.None);

        // Act
        await _repository.UpdateLogAsync(jobId, "first flush\nsecond flush", CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.LogOutput.Should().Be("first flush\nsecond flush");
    }

    // ── GetRecentAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetRecentAsync_ReturnsJobsInDescendingOrder()
    {
        // Arrange — create multiple jobs
        var id1 = await _repository.CreateAsync("scheduled", ct: CancellationToken.None);
        var id2 = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        var jobs = await _repository.GetRecentAsync(10, CancellationToken.None);

        // Assert
        jobs.Should().HaveCountGreaterThanOrEqualTo(2);
        // Most recent job should be first
        var ids = jobs.Select(j => j.Id).ToList();
        ids.IndexOf(id2).Should().BeLessThan(ids.IndexOf(id1),
            "newer job should appear before older job");
    }

    [Fact]
    public async Task GetRecentAsync_AllFieldsMappedCorrectly()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        await _repository.CompleteAsync(jobId, 3, 7, 2, 1, 0, 0, "log content", ct: CancellationToken.None);

        // Act
        var jobs = await _repository.GetRecentAsync(10, CancellationToken.None);
        var job = jobs.FirstOrDefault(j => j.Id == jobId);

        // Assert — verify ALL fields are mapped (not defaulted)
        job.Should().NotBeNull();
        job!.StartedAt.Should().BeAfter(DateTimeOffset.MinValue, "started_at must be mapped from DB");
        job.CompletedAt.Should().NotBeNull("completed_at must be mapped from DB");
        job.DurationMs.Should().NotBeNull("duration_ms must be mapped from DB");
        job.Status.Should().Be("completed", "status must be mapped from DB");
        job.TriggerType.Should().Be("manual", "trigger_type must be mapped from DB");
        job.JobType.Should().Be(ContentProcessingJobType.ContentProcessing, "job_type must be mapped from DB");
        job.FeedsProcessed.Should().Be(3, "feeds_processed must be mapped from DB");
        job.ItemsAdded.Should().Be(7, "items_added must be mapped from DB");
        job.ItemsSkipped.Should().Be(2, "items_skipped must be mapped from DB");
        job.ErrorCount.Should().Be(1, "error_count must be mapped from DB");
    }

    // ── GetByIdAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Act
        var job = await _repository.GetByIdAsync(999999, CancellationToken.None);

        // Assert
        job.Should().BeNull();
    }

    // ── AbortRunningJobsAsync ──────────────────────────────────────────────

    [Fact]
    public async Task AbortJobAsync_MarksRunningJobAsAborted()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        await _repository.AbortJobAsync(jobId, feedsProcessed: 2, itemsAdded: 1, itemsSkipped: 0, errorCount: 0, transcriptsSucceeded: 0, transcriptsFailed: 0, "Cancelled by admin.", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Aborted);
        job.LogOutput.Should().Be("Cancelled by admin.");
        job.CompletedAt.Should().NotBeNull();
        job.DurationMs.Should().NotBeNull();
        job.FeedsProcessed.Should().Be(2);
        job.ItemsAdded.Should().Be(1);
    }

    [Fact]
    public async Task AbortJobAsync_DoesNotAffectCompletedJob()
    {
        // Arrange — complete the job first
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        await _repository.CompleteAsync(jobId, 1, 1, 0, 0, 0, 0, "done", ct: CancellationToken.None);

        // Act — try to abort a completed job
        await _repository.AbortJobAsync(jobId, feedsProcessed: 0, itemsAdded: 0, itemsSkipped: 0, errorCount: 0, transcriptsSucceeded: 0, transcriptsFailed: 0, "Should not apply.", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert — status should remain completed
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Completed);
        job.LogOutput.Should().Be("done");
    }

    [Fact]
    public async Task AbortRunningJobsAsync_MarksRunningJobsAsAborted()
    {
        // Arrange — create a running job
        var jobId = await _repository.CreateAsync("scheduled", ct: CancellationToken.None);

        // Act
        var aborted = await _repository.AbortRunningJobsAsync(CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        aborted.Should().BeGreaterThanOrEqualTo(1);
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Aborted);
        job.CompletedAt.Should().NotBeNull();
        job.DurationMs.Should().NotBeNull();
        job.LogOutput.Should().Contain("server was restarted");
    }

    [Fact]
    public async Task AbortRunningJobsAsync_DoesNotAffectCompletedJobs()
    {
        // Arrange — create a completed job
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        await _repository.CompleteAsync(jobId, 1, 1, 0, 0, 0, 0, "done", ct: CancellationToken.None);

        // Act
        var aborted = await _repository.AbortRunningJobsAsync(CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Completed);
    }

    // ── UpdateProgressAsync ────────────────────────────────────────────────

    [Fact]
    public async Task UpdateProgressAsync_UpdatesCountersForRunningJob()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        await _repository.UpdateProgressAsync(jobId, feedsProcessed: 3, itemsAdded: 5, itemsSkipped: 2, errorCount: 1, transcriptsSucceeded: 0, transcriptsFailed: 0, itemsFixed: 0, ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.FeedsProcessed.Should().Be(3);
        job.ItemsAdded.Should().Be(5);
        job.ItemsSkipped.Should().Be(2);
        job.ErrorCount.Should().Be(1);
        job.Status.Should().Be(ContentProcessingJobStatus.Running, "status should remain running");
    }

    [Fact]
    public async Task UpdateProgressAsync_DoesNotAffectCompletedJob()
    {
        // Arrange — complete the job first
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        await _repository.CompleteAsync(jobId, 1, 1, 0, 0, 0, 0, "done", ct: CancellationToken.None);

        // Act — try to update progress on a completed job
        await _repository.UpdateProgressAsync(jobId, feedsProcessed: 99, itemsAdded: 99, itemsSkipped: 99, errorCount: 99, transcriptsSucceeded: 0, transcriptsFailed: 0, itemsFixed: 0, ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert — counters should not change
        job.Should().NotBeNull();
        job!.FeedsProcessed.Should().Be(1);
        job.ItemsAdded.Should().Be(1);
    }

    // ── Transcript Counters ────────────────────────────────────────────────

    [Fact]
    public async Task CompleteAsync_WithTranscriptCounters_StoresValues()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        await _repository.CompleteAsync(jobId, feedsProcessed: 1, itemsAdded: 3, itemsSkipped: 0, errorCount: 0, transcriptsSucceeded: 2, transcriptsFailed: 1, logOutput: "done", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.TranscriptsSucceeded.Should().Be(2);
        job.TranscriptsFailed.Should().Be(1);
    }

    [Fact]
    public async Task FailAsync_WithTranscriptCounters_StoresValues()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        await _repository.FailAsync(jobId, feedsProcessed: 1, itemsAdded: 0, itemsSkipped: 0, errorCount: 1, transcriptsSucceeded: 0, transcriptsFailed: 3, logOutput: "crash", ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.TranscriptsSucceeded.Should().Be(0);
        job.TranscriptsFailed.Should().Be(3);
    }

    [Fact]
    public async Task UpdateProgressAsync_WithTranscriptCounters_StoresValues()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);

        // Act
        await _repository.UpdateProgressAsync(jobId, feedsProcessed: 1, itemsAdded: 2, itemsSkipped: 0, errorCount: 0, transcriptsSucceeded: 4, transcriptsFailed: 1, ct: CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.TranscriptsSucceeded.Should().Be(4);
        job.TranscriptsFailed.Should().Be(1);
    }

    [Fact]
    public async Task GetRecentAsync_ReturnsTranscriptCounters()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", ct: CancellationToken.None);
        await _repository.CompleteAsync(jobId, feedsProcessed: 1, itemsAdded: 1, itemsSkipped: 0, errorCount: 0, transcriptsSucceeded: 5, transcriptsFailed: 2, logOutput: "done", ct: CancellationToken.None);

        // Act
        var jobs = await _repository.GetRecentAsync(1, CancellationToken.None);

        // Assert
        jobs.Should().NotBeEmpty();
        jobs[0].TranscriptsSucceeded.Should().Be(5);
        jobs[0].TranscriptsFailed.Should().Be(2);
    }
}
