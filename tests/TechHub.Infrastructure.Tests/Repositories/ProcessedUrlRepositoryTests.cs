using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
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

    public ProcessedUrlRepositoryTests(DatabaseFixture<ProcessedUrlRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _repository = new ProcessedUrlRepository(
            fixture.Connection,
            NullLogger<ProcessedUrlRepository>.Instance);
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
        const string url = "https://example.com/exists-after-success";
        await _repository.RecordSuccessAsync(url, ct: CancellationToken.None);

        // Act
        var exists = await _repository.ExistsAsync(url, CancellationToken.None);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_AfterRecordFailure_ReturnsTrue()
    {
        // Arrange
        const string url = "https://example.com/exists-after-failure";
        await _repository.RecordFailureAsync(url, "test error", ct: CancellationToken.None);

        // Act
        var exists = await _repository.ExistsAsync(url, CancellationToken.None);

        // Assert
        exists.Should().BeTrue();
    }

    // ── RecordSuccessAsync & GetAsync ──────────────────────────────────────

    [Fact]
    public async Task RecordSuccessAsync_StoresSucceededStatus()
    {
        // Arrange
        const string url = "https://example.com/success-status";

        // Act
        await _repository.RecordSuccessAsync(url, ct: CancellationToken.None);
        var record = await _repository.GetAsync(url, CancellationToken.None);

        // Assert
        record.Should().NotBeNull();
        record!.ExternalUrl.Should().Be(url);
        record.Status.Should().Be("succeeded");
        record.ErrorMessage.Should().BeNull();
        record.ProcessedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task RecordSuccessAsync_WithYouTubeTags_StoresTags()
    {
        // Arrange
        const string url = "https://youtube.com/watch?v=tags123";
        var tags = new List<string> { "csharp", "dotnet", "tutorial" };

        // Act
        await _repository.RecordSuccessAsync(url, tags, ct: CancellationToken.None);
        var record = await _repository.GetAsync(url, CancellationToken.None);

        // Assert
        record.Should().NotBeNull();
        record!.YouTubeTags.Should().BeEquivalentTo(tags);
    }

    [Fact]
    public async Task RecordSuccessAsync_OnConflict_UpdatesStatus()
    {
        // Arrange — first record a failure
        const string url = "https://example.com/conflict-update";
        await _repository.RecordFailureAsync(url, "original error", ct: CancellationToken.None);

        // Act — then record success for the same URL
        await _repository.RecordSuccessAsync(url, ct: CancellationToken.None);
        var record = await _repository.GetAsync(url, CancellationToken.None);

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
        const string url = "https://example.com/failure-record";
        const string errorMsg = "HTTP 503 Service Unavailable";

        // Act
        await _repository.RecordFailureAsync(url, errorMsg, ct: CancellationToken.None);
        var record = await _repository.GetAsync(url, CancellationToken.None);

        // Assert
        record.Should().NotBeNull();
        record!.Status.Should().Be("failed");
        record.ErrorMessage.Should().Be(errorMsg);
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
        const string url = "https://example.com/purge-target";
        await _repository.RecordFailureAsync(url, "will be purged", ct: CancellationToken.None);
        var before = await _repository.ExistsAsync(url, CancellationToken.None);
        before.Should().BeTrue();

        // Act — purge anything older than 0 seconds (everything)
        await _repository.PurgeFailedAsync(TimeSpan.Zero, CancellationToken.None);

        // Assert
        var after = await _repository.ExistsAsync(url, CancellationToken.None);
        after.Should().BeFalse();
    }

    [Fact]
    public async Task PurgeFailedAsync_DoesNotRemoveSucceededRecords()
    {
        // Arrange
        const string url = "https://example.com/purge-survivor";
        await _repository.RecordSuccessAsync(url, ct: CancellationToken.None);

        // Act — purge with zero retention
        await _repository.PurgeFailedAsync(TimeSpan.Zero, CancellationToken.None);

        // Assert — succeeded record survives
        var exists = await _repository.ExistsAsync(url, CancellationToken.None);
        exists.Should().BeTrue();
    }

    // ── Feed Metadata ──────────────────────────────────────────────────────

    [Fact]
    public async Task RecordSuccessAsync_WithFeedMetadata_StoresFeedNameAndCollection()
    {
        // Arrange
        const string url = "https://example.com/feed-metadata-success";

        // Act
        await _repository.RecordSuccessAsync(
            url, feedName: "Test Feed", collectionName: "blogs", ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: url, ct: CancellationToken.None);

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
        const string url = "https://example.com/feed-metadata-failure";

        // Act
        await _repository.RecordFailureAsync(
            url, "test error", feedName: "Broken Feed", collectionName: "videos", ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: url, ct: CancellationToken.None);

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
        const string url = "https://example.com/reason-success";
        const string reason = "Content included: Categories assigned as AI, DevOps";

        // Act
        await _repository.RecordSuccessAsync(
            url, feedName: "Test Feed", collectionName: "blogs", reason: reason, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.Reason.Should().Be(reason);
    }

    [Fact]
    public async Task RecordFailureAsync_WithReason_StoresReason()
    {
        // Arrange
        const string url = "https://example.com/reason-failure";
        const string reason = "AI categorization failed after 3 attempts";

        // Act
        await _repository.RecordFailureAsync(
            url, "HTTP 500", feedName: "Test Feed", collectionName: "news", reason: reason, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.Reason.Should().Be(reason);
        item.ErrorMessage.Should().Be("HTTP 500");
    }

    [Fact]
    public async Task RecordSuccessAsync_OnConflict_PreservesReasonWhenNull()
    {
        // Arrange — first record with a reason
        const string url = "https://example.com/reason-preserve";
        await _repository.RecordSuccessAsync(
            url, reason: "Original reason", ct: CancellationToken.None);

        // Act — update without reason (should preserve using COALESCE)
        await _repository.RecordSuccessAsync(url, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Reason.Should().Be("Original reason");
    }

    // ── RecordSkippedAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task RecordSkippedAsync_StoresSkippedStatus()
    {
        // Arrange
        const string url = "https://example.com/skipped-test";

        // Act
        await _repository.RecordSkippedAsync(url, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        result.Items[0].Status.Should().Be("skipped");
    }

    [Fact]
    public async Task RecordSkippedAsync_WithFeedMetadataAndReason_StoresAllFields()
    {
        // Arrange
        const string url = "https://example.com/skipped-full";
        const string reason = "Content excluded: not relevant to any tracked section";

        // Act
        await _repository.RecordSkippedAsync(
            url, feedName: "Some Feed", collectionName: "news", reason: reason, ct: CancellationToken.None);
        var result = await _repository.GetPagedAsync(0, 10, search: url, ct: CancellationToken.None);

        // Assert
        result.Items.Should().ContainSingle();
        var item = result.Items[0];
        item.Status.Should().Be("skipped");
        item.FeedName.Should().Be("Some Feed");
        item.CollectionName.Should().Be("news");
        item.Reason.Should().Be(reason);
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
}
