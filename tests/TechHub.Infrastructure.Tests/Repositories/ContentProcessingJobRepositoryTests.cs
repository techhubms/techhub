using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.Admin;
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
            NullLogger<ContentProcessingJobRepository>.Instance);
    }

    // ── CreateAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ReturnsPositiveJobId()
    {
        // Act
        var jobId = await _repository.CreateAsync("manual", CancellationToken.None);

        // Assert
        jobId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync_SetsStartedAtToCurrentTime()
    {
        // Arrange
        var before = DateTimeOffset.UtcNow.AddSeconds(-2);

        // Act
        var jobId = await _repository.CreateAsync("manual", CancellationToken.None);
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
        var jobId = await _repository.CreateAsync("scheduled", CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Running);
    }

    [Fact]
    public async Task CreateAsync_SetsTriggerType()
    {
        // Act
        var jobId = await _repository.CreateAsync("manual", CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.TriggerType.Should().Be("manual");
    }

    // ── CompleteAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task CompleteAsync_SetsAllStatistics()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", CancellationToken.None);

        // Act
        await _repository.CompleteAsync(jobId, feedsProcessed: 5, itemsAdded: 10,
            itemsSkipped: 3, errorCount: 2, logOutput: "test log", CancellationToken.None);

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
        var jobId = await _repository.CreateAsync("scheduled", CancellationToken.None);

        // Act
        await _repository.CompleteAsync(jobId, 1, 0, 0, 0, "done", CancellationToken.None);
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
        var jobId = await _repository.CreateAsync("manual", CancellationToken.None);

        // Act
        await _repository.FailAsync(jobId, "error log", CancellationToken.None);
        var job = await _repository.GetByIdAsync(jobId, CancellationToken.None);

        // Assert
        job.Should().NotBeNull();
        job!.Status.Should().Be(ContentProcessingJobStatus.Failed);
        job.LogOutput.Should().Be("error log");
        job.CompletedAt.Should().NotBeNull();
        job.DurationMs.Should().NotBeNull();
    }

    // ── UpdateLogAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateLogAsync_ReplacesLogOutput()
    {
        // Arrange
        var jobId = await _repository.CreateAsync("manual", CancellationToken.None);
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
        var id1 = await _repository.CreateAsync("scheduled", CancellationToken.None);
        var id2 = await _repository.CreateAsync("manual", CancellationToken.None);

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
        var jobId = await _repository.CreateAsync("manual", CancellationToken.None);
        await _repository.CompleteAsync(jobId, 3, 7, 2, 1, "log content", CancellationToken.None);

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
        job.FeedsProcessed.Should().Be(3, "feeds_processed must be mapped from DB");
        job.ItemsAdded.Should().Be(7, "items_added must be mapped from DB");
        job.ItemsSkipped.Should().Be(2, "items_skipped must be mapped from DB");
        job.ErrorCount.Should().Be(1, "error_count must be mapped from DB");
        job.LogOutput.Should().Be("log content", "log_output must be mapped from DB");
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
}
