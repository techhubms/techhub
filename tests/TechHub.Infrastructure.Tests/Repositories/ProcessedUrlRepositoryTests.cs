using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="ProcessedUrlRepository"/>.
/// Uses PostgreSQL via Testcontainers to verify dedup tracking and CRUD operations.
/// </summary>
public class ProcessedUrlRepositoryTests
    : IClassFixture<DatabaseFixture<ProcessedUrlRepositoryTests>>
{
    private readonly ProcessedUrlRepository _repository;
    private readonly ContentProcessingJobRepository _jobRepository;

    public ProcessedUrlRepositoryTests(DatabaseFixture<ProcessedUrlRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _repository = new ProcessedUrlRepository(
            fixture.Connection,
            NullLogger<ProcessedUrlRepository>.Instance);

        _jobRepository = new ContentProcessingJobRepository(
            fixture.Connection,
            new PostgresConnectionFactory(fixture.ConnectionString),
            NullLogger<ContentProcessingJobRepository>.Instance);
    }

    // ── ExistsAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task ExistsAsync_UnknownUrl_ReturnsFalse()
    {
        // Act
        var exists = await _repository.ExistsAsync(
            "https://example.com/never-seen", CancellationToken.None);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsAsync_AfterRecordSuccess_ReturnsTrue()
    {
        // Arrange
        const string Url = "https://example.com/exists-after-success";
        await _repository.RecordSuccessAsync(Url, ct: CancellationToken.None);

        // Act
        var exists = await _repository.ExistsAsync(Url, CancellationToken.None);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_AfterRecordFailure_ReturnsTrue()
    {
        // Arrange
        const string Url = "https://example.com/exists-after-failure";
        await _repository.RecordFailureAsync(Url, "test error", ct: CancellationToken.None);

        // Act
        var exists = await _repository.ExistsAsync(Url, CancellationToken.None);

        // Assert
        exists.Should().BeTrue();
    }

    // ── RecordSuccessAsync & GetAsync ──────────────────────────────────────

    [Fact]
    public async Task RecordSuccessAsync_StoresSucceededStatus()
    {
        // Arrange
        const string Url = "https://example.com/success-status";

        // Act
        await _repository.RecordSuccessAsync(Url, ct: CancellationToken.None);
        var record = await _repository.GetAsync(Url, CancellationToken.None);

        // Assert
        record.Should().NotBeNull();
        record!.ExternalUrl.Should().Be(Url);
        record.Status.Should().Be("succeeded");
        record.ErrorMessage.Should().BeNull();
        record.ProcessedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task RecordSuccessAsync_WithYouTubeTags_StoresTags()
    {
        // Arrange
        const string Url = "https://youtube.com/watch?v=tags123";
        var tags = new List<string> { "csharp", "dotnet", "tutorial" };

        // Act
        await _repository.RecordSuccessAsync(Url, tags, ct: CancellationToken.None);
        var record = await _repository.GetAsync(Url, CancellationToken.None);

        // Assert
        record.Should().NotBeNull();
        record!.YouTubeTags.Should().BeEquivalentTo(tags);
    }

    [Fact]
    public async Task RecordSuccessAsync_OnConflict_UpdatesStatus()
    {
        // Arrange — first record a failure
        const string Url = "https://example.com/conflict-update";
        await _repository.RecordFailureAsync(Url, "original error", ct: CancellationToken.None);

        // Act — then record success for the same URL
        await _repository.RecordSuccessAsync(Url, ct: CancellationToken.None);
        var record = await _repository.GetAsync(Url, CancellationToken.None);

        // Assert — status flipped to succeeded, error cleared
        record.Should().NotBeNull();
        record!.Status.Should().Be("succeeded");
        record.ErrorMessage.Should().BeNull();
    }

    // ── RecordFailureAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task RecordFailureAsync_StoresFailedStatusAndMessage()
    {
        // Arrange
        const string Url = "https://example.com/failure-record";
        const string ErrorMsg = "HTTP 503 Service Unavailable";

        // Act
        await _repository.RecordFailureAsync(Url, ErrorMsg, ct: CancellationToken.None);
        var record = await _repository.GetAsync(Url, CancellationToken.None);

        // Assert
        record.Should().NotBeNull();
        record!.Status.Should().Be("failed");
        record.ErrorMessage.Should().Be(ErrorMsg);
    }

    // ── GetAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAsync_NonExistentUrl_ReturnsNull()
    {
        // Act
        var record = await _repository.GetAsync(
            "https://example.com/does-not-exist", CancellationToken.None);

        // Assert
        record.Should().BeNull();
    }

    // ── PurgeFailedAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task PurgeFailedAsync_RemovesOldFailedRecords()
    {
        // Arrange — insert a failed record, then purge with zero retention
        const string Url = "https://example.com/purge-target";
        await _repository.RecordFailureAsync(Url, "will be purged", ct: CancellationToken.None);
        var before = await _repository.ExistsAsync(Url, CancellationToken.None);
        before.Should().BeTrue();

        // Act — purge anything older than 0 seconds (everything)
        await _repository.PurgeFailedAsync(TimeSpan.Zero, CancellationToken.None);

        // Assert
        var after = await _repository.ExistsAsync(Url, CancellationToken.None);
        after.Should().BeFalse();
    }

    [Fact]
    public async Task PurgeFailedAsync_DoesNotRemoveSucceededRecords()
    {
        // Arrange
        const string Url = "https://example.com/purge-survivor";
        await _repository.RecordSuccessAsync(Url, ct: CancellationToken.None);

        // Act — purge with zero retention
        await _repository.PurgeFailedAsync(TimeSpan.Zero, CancellationToken.None);

        // Assert — succeeded record survives
        var exists = await _repository.ExistsAsync(Url, CancellationToken.None);
        exists.Should().BeTrue();
    }

    // ── Feed Metadata ──────────────────────────────────────────────────────

    [Fact]
    public async Task RecordSuccessAsync_WithFeedMetadata_StoresFeedNameAndCollection()
    {
        // Arrange
        const string Url = "https://example.com/feed-metadata-success";

        // Act
        await _repository.RecordSuccessAsync(
            Url, feedName: "Test Feed", collectionName: "blogs", ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.FeedName.Should().Be("Test Feed");
        item.CollectionName.Should().Be("blogs");
    }

    [Fact]
    public async Task RecordFailureAsync_WithFeedMetadata_StoresFeedNameAndCollection()
    {
        // Arrange
        const string Url = "https://example.com/feed-metadata-failure";

        // Act
        await _repository.RecordFailureAsync(
            Url, "test error", feedName: "Broken Feed", collectionName: "videos", ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.FeedName.Should().Be("Broken Feed");
        item.CollectionName.Should().Be("videos");
    }

    // ── Reason ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task RecordSuccessAsync_WithReason_StoresReason()
    {
        // Arrange
        const string Url = "https://example.com/Reason-success";
        const string Reason = "Categories assigned as AI, DevOps";

        // Act
        await _repository.RecordSuccessAsync(
            Url, feedName: "Test Feed", collectionName: "blogs", reason: Reason, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.Reason.Should().Be(Reason);
    }

    [Fact]
    public async Task RecordFailureAsync_WithReason_StoresReason()
    {
        // Arrange
        const string Url = "https://example.com/Reason-failure";
        const string Reason = "AI categorization failed after 3 attempts";

        // Act
        await _repository.RecordFailureAsync(
            Url, "HTTP 500", feedName: "Test Feed", collectionName: "news", reason: Reason, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.Reason.Should().Be(Reason);
        item.ErrorMessage.Should().Be("HTTP 500");
    }

    [Fact]
    public async Task RecordSuccessAsync_OnConflict_PreservesReasonWhenNull()
    {
        // Arrange — first record with a Reason
        const string Url = "https://example.com/Reason-preserve";
        await _repository.RecordSuccessAsync(
            Url, reason: "Original reason", ct: CancellationToken.None);

        // Act — update without Reason (should preserve using COALESCE)
        await _repository.RecordSuccessAsync(Url, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Reason.Should().Be("Original reason");
    }

    // ── RecordSkippedAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task RecordSkippedAsync_StoresSkippedStatus()
    {
        // Arrange
        const string Url = "https://example.com/skipped-test";

        // Act
        await _repository.RecordSkippedAsync(Url, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Status.Should().Be("skipped");
    }

    [Fact]
    public async Task RecordSkippedAsync_WithFeedMetadataAndReason_StoresAllFields()
    {
        // Arrange
        const string Url = "https://example.com/skipped-full";
        const string Reason = "not relevant to any tracked section";

        // Act
        await _repository.RecordSkippedAsync(
            Url, feedName: "Some Feed", collectionName: "news", reason: Reason, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.Status.Should().Be("skipped");
        item.FeedName.Should().Be("Some Feed");
        item.CollectionName.Should().Be("news");
        item.Reason.Should().Be(Reason);
    }

    [Fact]
    public async Task GetPagedAsync_FilterBySkippedStatus_ReturnsOnlySkipped()
    {
        // Arrange
        var addedUrl = $"https://example.com/filter-added-{Guid.NewGuid():N}";
        var skippedUrl = $"https://example.com/filter-skipped-{Guid.NewGuid():N}";
        var failedUrl = $"https://example.com/filter-failed-{Guid.NewGuid():N}";
        await _repository.RecordSuccessAsync(addedUrl, ct: CancellationToken.None);
        await _repository.RecordSkippedAsync(skippedUrl, reason: "Not relevant", ct: CancellationToken.None);
        await _repository.RecordFailureAsync(failedUrl, "error", ct: CancellationToken.None);

        // Act
        var result = await _repository.GetPagedAsync(0, 100, status: "skipped", ct: CancellationToken.None);

        // Assert
        result.Items.Should().Contain(i => i.ExternalUrl == skippedUrl);
        result.Items.Should().NotContain(i => i.ExternalUrl == addedUrl);
        result.Items.Should().NotContain(i => i.ExternalUrl == failedUrl);
    }

    // ── HasTranscript ──────────────────────────────────────────────────────

    [Fact]
    public async Task RecordSuccessAsync_WithHasTranscriptTrue_StoresValue()
    {
        // Arrange
        const string Url = "https://youtube.com/watch?v=transcript-true";

        // Act
        await _repository.RecordSuccessAsync(Url, hasTranscript: true, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeTrue();
    }

    [Fact]
    public async Task RecordSuccessAsync_WithHasTranscriptFalse_StoresValue()
    {
        // Arrange
        const string Url = "https://youtube.com/watch?v=transcript-false";

        // Act
        await _repository.RecordSuccessAsync(Url, hasTranscript: false, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeFalse();
    }

    [Fact]
    public async Task RecordSuccessAsync_WithoutHasTranscript_StoresNull()
    {
        // Arrange
        const string Url = "https://example.com/no-transcript-field";

        // Act
        await _repository.RecordSuccessAsync(Url, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeNull();
    }

    [Fact]
    public async Task RecordFailureAsync_WithHasTranscript_StoresValue()
    {
        // Arrange
        const string Url = "https://youtube.com/watch?v=fail-transcript";

        // Act
        await _repository.RecordFailureAsync(Url, "transcript mandatory", hasTranscript: false, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeFalse();
    }

    // ── JobId ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task RecordSuccessAsync_WithJobId_StoresJobId()
    {
        // Arrange
        var jobId = await _jobRepository.CreateAsync("manual", ct: CancellationToken.None);
        var Url = $"https://example.com/jobid-success-{Guid.NewGuid():N}";

        // Act
        await _repository.RecordSuccessAsync(Url, jobId: jobId, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].JobId.Should().Be(jobId);
    }

    [Fact]
    public async Task RecordSkippedAsync_WithJobId_StoresJobId()
    {
        // Arrange
        var jobId = await _jobRepository.CreateAsync("manual", ct: CancellationToken.None);
        var Url = $"https://example.com/jobid-skipped-{Guid.NewGuid():N}";

        // Act
        await _repository.RecordSkippedAsync(Url, jobId: jobId, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].JobId.Should().Be(jobId);
    }

    [Fact]
    public async Task RecordFailureAsync_WithJobId_StoresJobId()
    {
        // Arrange
        var jobId = await _jobRepository.CreateAsync("manual", ct: CancellationToken.None);
        var Url = $"https://example.com/jobid-failure-{Guid.NewGuid():N}";

        // Act
        await _repository.RecordFailureAsync(Url, "test error", jobId: jobId, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].JobId.Should().Be(jobId);
    }

    [Fact]
    public async Task RecordSuccessAsync_WithoutJobId_StoresNull()
    {
        // Arrange
        var Url = $"https://example.com/jobid-null-{Guid.NewGuid():N}";

        // Act
        await _repository.RecordSuccessAsync(Url, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].JobId.Should().BeNull();
    }

    [Fact]
    public async Task GetPagedAsync_FilterByJobId_ReturnsOnlyMatchingItems()
    {
        // Arrange
        var jobId1 = await _jobRepository.CreateAsync("manual", ct: CancellationToken.None);
        var jobId2 = await _jobRepository.CreateAsync("manual", ct: CancellationToken.None);
        var url1 = $"https://example.com/jobfilter-1-{Guid.NewGuid():N}";
        var url2 = $"https://example.com/jobfilter-2-{Guid.NewGuid():N}";
        await _repository.RecordSuccessAsync(url1, jobId: jobId1, ct: CancellationToken.None);
        await _repository.RecordSuccessAsync(url2, jobId: jobId2, ct: CancellationToken.None);

        // Act
        var result = await _repository.GetPagedAsync(0, 100, jobId: jobId1, ct: CancellationToken.None);

        // Assert
        result.Items.Should().Contain(i => i.ExternalUrl == url1);
        result.Items.Should().NotContain(i => i.ExternalUrl == url2);
    }

    [Fact]
    public async Task RecordSuccessAsync_OnConflict_UpdatesJobId()
    {
        // Arrange — first record with one job
        var jobId1 = await _jobRepository.CreateAsync("manual", ct: CancellationToken.None);
        var jobId2 = await _jobRepository.CreateAsync("manual", ct: CancellationToken.None);
        var Url = $"https://example.com/jobid-update-{Guid.NewGuid():N}";
        await _repository.RecordSuccessAsync(Url, jobId: jobId1, ct: CancellationToken.None);

        // Act — re-process with a different job
        await _repository.RecordSuccessAsync(Url, jobId: jobId2, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: Url, ct: CancellationToken.None);

        // Assert — job_id updated to the newer job
        result.Items.Should().ContainSingle();
        result.Items[0].JobId.Should().Be(jobId2);
    }
}
