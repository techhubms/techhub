using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using TechHub.Core.Models.Admin;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="ContentReviewRepository"/>.
/// Uses PostgreSQL via Testcontainers; every test starts with an empty <c>content_reviews</c> table
/// because several operations (summary, approve all, reject all) act on the whole queue.
/// </summary>
public class ContentReviewRepositoryTests
    : IClassFixture<DatabaseFixture<ContentReviewRepositoryTests>>, IAsyncLifetime
{
    private const string Collection = "blogs";

    private readonly DatabaseFixture<ContentReviewRepositoryTests> _fixture;
    private readonly ContentReviewRepository _repository;

    public ContentReviewRepositoryTests(DatabaseFixture<ContentReviewRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _repository = new ContentReviewRepository(
            fixture.Connection,
            NullLogger<ContentReviewRepository>.Instance);
    }

    public async ValueTask InitializeAsync() =>
        await _fixture.Connection.ExecuteAsync("DELETE FROM content_reviews");

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    // ── CreateAsync & GetByIdAsync ─────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ValidReview_StoresPendingReviewWithJoinedItemData()
    {
        // Arrange
        var slug = await SeedContentItemAsync();

        // Act
        var id = await _repository.CreateAsync(
            slug, Collection, ContentReviewChangeType.Tags, ",Old,", ",New,", ct: TestContext.Current.CancellationToken);
        var review = await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken);

        // Assert
        id.Should().BeGreaterThan(0);
        review.Should().NotBeNull();
        review!.Slug.Should().Be(slug);
        review.CollectionName.Should().Be(Collection);
        review.ChangeType.Should().Be(ContentReviewChangeType.Tags);
        review.OriginalValue.Should().Be(",Old,");
        review.FixedValue.Should().Be(",New,");
        review.Status.Should().Be(ContentReviewStatus.Pending);
        review.JobId.Should().BeNull();
        review.ReviewedAt.Should().BeNull();
        review.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));
        review.PrimarySectionName.Should().Be("ai");
        review.ExternalUrl.Should().Be($"https://example.com/{slug}");
    }

    [Fact]
    public async Task CreateAsync_WithJobId_StoresJobId()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var jobRepository = new ContentProcessingJobRepository(
            _fixture.Connection,
            new PostgresConnectionFactory(_fixture.DataSource),
            NullLogger<ContentProcessingJobRepository>.Instance);
        var jobId = await jobRepository.CreateAsync("manual", ct: TestContext.Current.CancellationToken);

        // Act
        var id = await _repository.CreateAsync(
            slug, Collection, ContentReviewChangeType.Author, "a", "b", jobId, TestContext.Current.CancellationToken);
        var review = await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken);

        // Assert
        review!.JobId.Should().Be(jobId);
    }

    [Fact]
    public async Task CreateAsync_UnknownContentItem_ThrowsForeignKeyViolation()
    {
        // Act
        var act = () => _repository.CreateAsync(
            "does-not-exist", Collection, ContentReviewChangeType.Tags, "a", "b", ct: TestContext.Current.CancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<PostgresException>();
        exception.Which.SqlState.Should().Be(PostgresErrorCodes.ForeignKeyViolation);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        // Act
        var review = await _repository.GetByIdAsync(long.MaxValue, TestContext.Current.CancellationToken);

        // Assert
        review.Should().BeNull();
    }

    [Fact]
    public async Task DeletingContentItem_CascadesToItsReviews()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "a", "b");

        // Act
        await _fixture.Connection.ExecuteAsync(
            "DELETE FROM content_items WHERE collection_name = @Collection AND slug = @Slug", new { Collection, Slug = slug });

        // Assert
        (await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken)).Should().BeNull();
    }

    // ── GetByStatusAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetByStatusAsync_NoStatus_ReturnsAllReviewsNewestFirst()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var first = await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "1", "1b");
        var second = await CreateReviewAsync(slug, ContentReviewChangeType.Author, "2", "2b");
        var third = await CreateReviewAsync(slug, ContentReviewChangeType.Markdown, "3", "3b");
        await _repository.RejectAsync(second, TestContext.Current.CancellationToken);

        // Act
        var reviews = await _repository.GetByStatusAsync(ct: TestContext.Current.CancellationToken);

        // Assert
        reviews.Select(r => r.Id).Should().Equal(third, second, first);
    }

    [Fact]
    public async Task GetByStatusAsync_WithStatus_ReturnsOnlyMatchingReviews()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var pending = await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "1", "1b");
        var rejected = await CreateReviewAsync(slug, ContentReviewChangeType.Author, "2", "2b");
        await _repository.RejectAsync(rejected, TestContext.Current.CancellationToken);

        // Act
        var reviews = await _repository.GetByStatusAsync(ContentReviewStatus.Pending, ct: TestContext.Current.CancellationToken);

        // Assert
        reviews.Select(r => r.Id).Should().Equal(pending);
    }

    [Fact]
    public async Task GetByStatusAsync_LimitSmallerThanResultSet_ReturnsNewestReviewsOnly()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "1", "1b");
        var second = await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "2", "2b");
        var third = await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "3", "3b");

        // Act
        var reviews = await _repository.GetByStatusAsync(limit: 2, ct: TestContext.Current.CancellationToken);

        // Assert
        reviews.Select(r => r.Id).Should().Equal(third, second);
    }

    [Fact]
    public async Task GetByStatusAsync_EmptyQueue_ReturnsEmptyList()
    {
        // Act
        var reviews = await _repository.GetByStatusAsync(ct: TestContext.Current.CancellationToken);

        // Assert
        reviews.Should().BeEmpty();
    }

    // ── GetSummaryAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetSummaryAsync_EmptyQueue_ReturnsZeroCounts()
    {
        // Act
        var summary = await _repository.GetSummaryAsync(TestContext.Current.CancellationToken);

        // Assert
        summary.Pending.Should().Be(0);
        summary.Approved.Should().Be(0);
        summary.Rejected.Should().Be(0);
        summary.Total.Should().Be(0);
    }

    [Fact]
    public async Task GetSummaryAsync_MixedStatuses_CountsEachStatus()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "1", "1b");
        await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "2", "2b");
        var toApprove = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "3", "3b");
        var toReject = await CreateReviewAsync(slug, ContentReviewChangeType.Tags, "4", "4b");
        await _repository.ApproveAsync(toApprove, TestContext.Current.CancellationToken);
        await _repository.RejectAsync(toReject, TestContext.Current.CancellationToken);

        // Act
        var summary = await _repository.GetSummaryAsync(TestContext.Current.CancellationToken);

        // Assert
        summary.Pending.Should().Be(2);
        summary.Approved.Should().Be(1);
        summary.Rejected.Should().Be(1);
        summary.Total.Should().Be(4);
    }

    // ── ApproveAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task ApproveAsync_TagsReview_AppliesFixedTagsAndMarksApproved()
    {
        // Arrange
        var slug = await SeedContentItemAsync(tagsCsv: ",Old,");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Tags, ",Old,", ",AI,GitHub Copilot,");

        // Act
        var approved = await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        approved.Should().BeTrue();
        (await GetItemColumnAsync(slug, "tags_csv")).Should().Be(",AI,GitHub Copilot,");
        var review = await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken);
        review!.Status.Should().Be(ContentReviewStatus.Approved);
        review.ReviewedAt.Should().NotBeNull();
        review.ReviewedAt!.Value.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));
    }

    [Fact]
    public async Task ApproveAsync_MarkdownReview_AppliesFixedContent()
    {
        // Arrange
        var slug = await SeedContentItemAsync(content: "Original body");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Markdown, "Original body", "Fixed **body**");

        // Act
        var approved = await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        approved.Should().BeTrue();
        (await GetItemColumnAsync(slug, "content")).Should().Be("Fixed **body**");
    }

    [Fact]
    public async Task ApproveAsync_AuthorReview_AppliesFixedAuthor()
    {
        // Arrange
        var slug = await SeedContentItemAsync(author: "Original Author");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Author, "Original Author", "Fixed Author");

        // Act
        var approved = await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        approved.Should().BeTrue();
        (await GetItemColumnAsync(slug, "author")).Should().Be("Fixed Author");
    }

    [Fact]
    public async Task ApproveAsync_ValidationReview_MarksApprovedWithoutChangingContentItem()
    {
        // Arrange
        var slug = await SeedContentItemAsync(tagsCsv: ",Keep,", content: "Keep body", author: "Keep Author");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "issue", "n/a");

        // Act
        var approved = await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        approved.Should().BeTrue();
        (await GetItemColumnAsync(slug, "tags_csv")).Should().Be(",Keep,");
        (await GetItemColumnAsync(slug, "content")).Should().Be("Keep body");
        (await GetItemColumnAsync(slug, "author")).Should().Be("Keep Author");
        (await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken))!.Status.Should().Be(ContentReviewStatus.Approved);
    }

    [Fact]
    public async Task ApproveAsync_UnknownChangeType_ThrowsAndLeavesReviewPending()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var id = await CreateReviewAsync(slug, "not-a-change-type", "a", "b");

        // Act
        var act = () => _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*not-a-change-type*");
        (await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken))!.Status.Should().Be(ContentReviewStatus.Pending);
    }

    [Fact]
    public async Task ApproveAsync_UnknownId_ReturnsFalse()
    {
        // Act
        var approved = await _repository.ApproveAsync(long.MaxValue, TestContext.Current.CancellationToken);

        // Assert
        approved.Should().BeFalse();
    }

    [Fact]
    public async Task ApproveAsync_AlreadyApprovedReview_ReturnsFalseAndDoesNotReapplyChange()
    {
        // Arrange
        var slug = await SeedContentItemAsync(author: "Original Author");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Author, "Original Author", "Fixed Author");
        await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);
        await _fixture.Connection.ExecuteAsync(
            "UPDATE content_items SET author = 'Edited Later' WHERE collection_name = @Collection AND slug = @Slug",
            new { Collection, Slug = slug });

        // Act
        var approvedAgain = await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        approvedAgain.Should().BeFalse();
        (await GetItemColumnAsync(slug, "author")).Should().Be("Edited Later");
    }

    [Fact]
    public async Task ApproveAsync_RejectedReview_ReturnsFalseAndDoesNotApplyChange()
    {
        // Arrange
        var slug = await SeedContentItemAsync(author: "Original Author");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Author, "Original Author", "Fixed Author");
        await _repository.RejectAsync(id, TestContext.Current.CancellationToken);

        // Act
        var approved = await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        approved.Should().BeFalse();
        (await GetItemColumnAsync(slug, "author")).Should().Be("Original Author");
    }

    // ── RejectAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task RejectAsync_PendingReview_MarksRejectedWithoutChangingContentItem()
    {
        // Arrange
        var slug = await SeedContentItemAsync(author: "Original Author");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Author, "Original Author", "Fixed Author");

        // Act
        var rejected = await _repository.RejectAsync(id, TestContext.Current.CancellationToken);

        // Assert
        rejected.Should().BeTrue();
        (await GetItemColumnAsync(slug, "author")).Should().Be("Original Author");
        var review = await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken);
        review!.Status.Should().Be(ContentReviewStatus.Rejected);
        review.ReviewedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task RejectAsync_AlreadyApprovedReview_ReturnsFalseAndKeepsApproved()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "a", "b");
        await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Act
        var rejected = await _repository.RejectAsync(id, TestContext.Current.CancellationToken);

        // Assert
        rejected.Should().BeFalse();
        (await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken))!.Status.Should().Be(ContentReviewStatus.Approved);
    }

    [Fact]
    public async Task RejectAsync_UnknownId_ReturnsFalse()
    {
        // Act
        var rejected = await _repository.RejectAsync(long.MaxValue, TestContext.Current.CancellationToken);

        // Assert
        rejected.Should().BeFalse();
    }

    // ── ApproveAllAsync / RejectAllAsync ───────────────────────────────────

    [Fact]
    public async Task ApproveAllAsync_PendingReviews_AppliesEachChangeAndReturnsCount()
    {
        // Arrange
        var firstSlug = await SeedContentItemAsync(author: "First Original");
        var secondSlug = await SeedContentItemAsync(content: "Second original");
        var rejectedSlug = await SeedContentItemAsync(author: "Rejected Original");
        await CreateReviewAsync(firstSlug, ContentReviewChangeType.Author, "First Original", "First Fixed");
        await CreateReviewAsync(secondSlug, ContentReviewChangeType.Markdown, "Second original", "Second fixed");
        var rejectedId = await CreateReviewAsync(rejectedSlug, ContentReviewChangeType.Author, "Rejected Original", "Rejected Fixed");
        await _repository.RejectAsync(rejectedId, TestContext.Current.CancellationToken);

        // Act
        var count = await _repository.ApproveAllAsync(TestContext.Current.CancellationToken);

        // Assert
        count.Should().Be(2);
        (await GetItemColumnAsync(firstSlug, "author")).Should().Be("First Fixed");
        (await GetItemColumnAsync(secondSlug, "content")).Should().Be("Second fixed");
        (await GetItemColumnAsync(rejectedSlug, "author")).Should().Be("Rejected Original");
        var summary = await _repository.GetSummaryAsync(TestContext.Current.CancellationToken);
        summary.Pending.Should().Be(0);
        summary.Approved.Should().Be(2);
        summary.Rejected.Should().Be(1);
    }

    [Fact]
    public async Task ApproveAllAsync_NoPendingReviews_ReturnsZero()
    {
        // Act
        var count = await _repository.ApproveAllAsync(TestContext.Current.CancellationToken);

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public async Task RejectAllAsync_PendingReviews_RejectsOnlyPendingWithoutChangingContent()
    {
        // Arrange
        var slug = await SeedContentItemAsync(author: "Original Author");
        await CreateReviewAsync(slug, ContentReviewChangeType.Author, "Original Author", "Fixed 1");
        await CreateReviewAsync(slug, ContentReviewChangeType.Author, "Original Author", "Fixed 2");
        var approvedId = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "x", "y");
        await _repository.ApproveAsync(approvedId, TestContext.Current.CancellationToken);

        // Act
        var count = await _repository.RejectAllAsync(TestContext.Current.CancellationToken);

        // Assert
        count.Should().Be(2);
        (await GetItemColumnAsync(slug, "author")).Should().Be("Original Author");
        var summary = await _repository.GetSummaryAsync(TestContext.Current.CancellationToken);
        summary.Pending.Should().Be(0);
        summary.Approved.Should().Be(1);
        summary.Rejected.Should().Be(2);
    }

    [Fact]
    public async Task RejectAllAsync_NoPendingReviews_ReturnsZero()
    {
        // Act
        var count = await _repository.RejectAllAsync(TestContext.Current.CancellationToken);

        // Assert
        count.Should().Be(0);
    }

    // ── UpdateFixedValueAsync ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateFixedValueAsync_PendingReview_ReplacesFixedValueAndIsUsedOnApproval()
    {
        // Arrange
        var slug = await SeedContentItemAsync(author: "Original Author");
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Author, "Original Author", "Suggested Author");

        // Act
        var updated = await _repository.UpdateFixedValueAsync(id, "Admin Edited Author", TestContext.Current.CancellationToken);
        await _repository.ApproveAsync(id, TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeTrue();
        (await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken))!.FixedValue.Should().Be("Admin Edited Author");
        (await GetItemColumnAsync(slug, "author")).Should().Be("Admin Edited Author");
    }

    [Fact]
    public async Task UpdateFixedValueAsync_NonPendingReview_ReturnsFalseAndKeepsValue()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "a", "original fix");
        await _repository.RejectAsync(id, TestContext.Current.CancellationToken);

        // Act
        var updated = await _repository.UpdateFixedValueAsync(id, "too late", TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeFalse();
        (await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken))!.FixedValue.Should().Be("original fix");
    }

    [Fact]
    public async Task UpdateFixedValueAsync_UnknownId_ReturnsFalse()
    {
        // Act
        var updated = await _repository.UpdateFixedValueAsync(long.MaxValue, "x", TestContext.Current.CancellationToken);

        // Assert
        updated.Should().BeFalse();
    }

    // ── PurgeOldReviewsAsync ───────────────────────────────────────────────

    [Fact]
    public async Task PurgeOldReviewsAsync_OldReviewedAndPendingReviews_RemovesOnlyOldNonPending()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var oldRejected = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "1", "1");
        var oldApproved = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "2", "2");
        var oldPending = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "3", "3");
        var recentRejected = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "4", "4");
        await _repository.RejectAsync(oldRejected, TestContext.Current.CancellationToken);
        await _repository.ApproveAsync(oldApproved, TestContext.Current.CancellationToken);
        await _repository.RejectAsync(recentRejected, TestContext.Current.CancellationToken);
        await BackdateAsync(40, oldRejected, oldApproved, oldPending);

        // Act
        var purged = await _repository.PurgeOldReviewsAsync(30, TestContext.Current.CancellationToken);

        // Assert
        purged.Should().Be(2);
        (await _repository.GetByIdAsync(oldRejected, TestContext.Current.CancellationToken)).Should().BeNull();
        (await _repository.GetByIdAsync(oldApproved, TestContext.Current.CancellationToken)).Should().BeNull();
        (await _repository.GetByIdAsync(oldPending, TestContext.Current.CancellationToken)).Should().NotBeNull();
        (await _repository.GetByIdAsync(recentRejected, TestContext.Current.CancellationToken)).Should().NotBeNull();
    }

    [Fact]
    public async Task PurgeOldReviewsAsync_ReviewYoungerThanKeepDays_IsKept()
    {
        // Arrange
        var slug = await SeedContentItemAsync();
        var id = await CreateReviewAsync(slug, ContentReviewChangeType.Validation, "1", "1");
        await _repository.RejectAsync(id, TestContext.Current.CancellationToken);
        await BackdateAsync(10, id);

        // Act
        var purged = await _repository.PurgeOldReviewsAsync(30, TestContext.Current.CancellationToken);

        // Assert
        purged.Should().Be(0);
        (await _repository.GetByIdAsync(id, TestContext.Current.CancellationToken)).Should().NotBeNull();
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    private async Task<string> SeedContentItemAsync(
        string tagsCsv = ",AI,",
        string content = "Original body",
        string author = "Original Author")
    {
        var slug = $"review-test-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch,
                 primary_section_name, external_url, author, feed_name, tags_csv,
                 sections_bitmask, content_hash, is_ai)
            VALUES
                (@Slug, @Collection, 'Review Test Item', @Content, 'Excerpt', 1747699200,
                 'ai', 'https://example.com/' || @Slug, @Author, 'TechHub', @TagsCsv,
                 1, 'hash-review-test', TRUE)
            """, new { Slug = slug, Collection, Content = content, Author = author, TagsCsv = tagsCsv });
        return slug;
    }

    private async Task<long> CreateReviewAsync(string slug, string changeType, string original, string fixedValue) =>
        await _repository.CreateAsync(slug, Collection, changeType, original, fixedValue, ct: TestContext.Current.CancellationToken);

    private async Task<string?> GetItemColumnAsync(string slug, string column) =>
        await _fixture.Connection.ExecuteScalarAsync<string?>(
            $"SELECT {column} FROM content_items WHERE collection_name = @Collection AND slug = @Slug",
            new { Collection, Slug = slug });

    private async Task BackdateAsync(int days, params long[] reviewIds) =>
        await _fixture.Connection.ExecuteAsync(
            "UPDATE content_reviews SET created_at = NOW() - INTERVAL '1 day' * @Days WHERE id = ANY(@Ids)",
            new { Days = days, Ids = reviewIds });
}
