using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Stores and retrieves content review records from PostgreSQL.
/// Reviews track proposed content fixer changes for admin approval.
/// </summary>
public sealed class ContentReviewRepository : IContentReviewRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ContentReviewRepository> _logger;

    public ContentReviewRepository(
        IDbConnection connection,
        ILogger<ContentReviewRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(logger);

        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(string slug, string collectionName, string changeType,
        string originalValue, string fixedValue, long? jobId = null, CancellationToken ct = default)
    {
        const string Sql = """
            INSERT INTO content_reviews (slug, collection_name, change_type, original_value, fixed_value, job_id)
            VALUES (@Slug, @CollectionName, @ChangeType, @OriginalValue, @FixedValue, @JobId)
            RETURNING id
            """;

        return await _connection.ExecuteScalarAsync<long>(
            new CommandDefinition(Sql, new
            {
                Slug = slug,
                CollectionName = collectionName,
                ChangeType = changeType,
                OriginalValue = originalValue,
                FixedValue = fixedValue,
                JobId = jobId
            }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<ContentReview?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        const string Sql = """
            SELECT r.id, r.slug, r.collection_name AS CollectionName, r.change_type AS ChangeType,
                   r.original_value AS OriginalValue, r.fixed_value AS FixedValue,
                   r.status, r.job_id AS JobId, r.created_at AS CreatedAt, r.reviewed_at AS ReviewedAt,
                   ci.primary_section_name AS PrimarySectionName,
                   ci.external_url AS ExternalUrl
            FROM content_reviews r
            LEFT JOIN content_items ci ON ci.collection_name = r.collection_name AND ci.slug = r.slug
            WHERE r.id = @Id
            """;

        return await _connection.QuerySingleOrDefaultAsync<ContentReview>(
            new CommandDefinition(Sql, new { Id = id }, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ContentReview>> GetByStatusAsync(string? status = null, int limit = 100, CancellationToken ct = default)
    {
        var statusClause = status != null ? "WHERE r.status = @Status" : string.Empty;

        var sql = $"""
            SELECT r.id, r.slug, r.collection_name AS CollectionName, r.change_type AS ChangeType,
                   r.original_value AS OriginalValue, r.fixed_value AS FixedValue,
                   r.status, r.job_id AS JobId, r.created_at AS CreatedAt, r.reviewed_at AS ReviewedAt,
                   ci.primary_section_name AS PrimarySectionName,
                   ci.external_url AS ExternalUrl
            FROM content_reviews r
            LEFT JOIN content_items ci ON ci.collection_name = r.collection_name AND ci.slug = r.slug
            {statusClause}
            ORDER BY r.created_at DESC
            LIMIT @Limit
            """;

        var results = await _connection.QueryAsync<ContentReview>(
            new CommandDefinition(sql, new { Status = status, Limit = limit }, cancellationToken: ct));

        return results.ToList();
    }

    /// <inheritdoc/>
    public async Task<ContentReviewSummary> GetSummaryAsync(CancellationToken ct = default)
    {
        const string Sql = """
            SELECT
                COUNT(*) FILTER (WHERE status = 'pending')  AS Pending,
                COUNT(*) FILTER (WHERE status = 'approved') AS Approved,
                COUNT(*) FILTER (WHERE status = 'rejected') AS Rejected
            FROM content_reviews
            """;

        return await _connection.QuerySingleAsync<ContentReviewSummary>(
            new CommandDefinition(Sql, cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<bool> ApproveAsync(long id, CancellationToken ct = default)
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        using var transaction = _connection.BeginTransaction();
        var pending = await GetPendingForApprovalAsync(id, 1, transaction, ct);
        var review = pending.SingleOrDefault();
        if (review is null)
        {
            return false;
        }

        // Apply the change depending on type
        await ApplyChangeAsync(review, transaction, ct);

        // Mark as approved
        const string Sql = """
            UPDATE content_reviews
            SET status = 'approved', reviewed_at = NOW()
            WHERE id = @Id AND status = 'pending'
            """;

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Id = id }, transaction: transaction, cancellationToken: ct));

        ct.ThrowIfCancellationRequested();
        transaction.Commit();

        if (rows > 0)
        {
            _logger.LogInformation("Approved review {Id} ({ChangeType}) for [{Collection}/{Slug}]",
                id, review.ChangeType, review.CollectionName, review.Slug);
        }

        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> RejectAsync(long id, CancellationToken ct = default)
    {
        const string Sql = """
            UPDATE content_reviews
            SET status = 'rejected', reviewed_at = NOW()
            WHERE id = @Id AND status = 'pending'
            """;

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Id = id }, cancellationToken: ct));

        if (rows > 0)
        {
            _logger.LogInformation("Rejected review {Id}", id);
        }

        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<int> ApproveAllAsync(CancellationToken ct = default)
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        using var transaction = _connection.BeginTransaction();
        var pending = await GetPendingForApprovalAsync(null, 10000, transaction, ct);

        foreach (var review in pending)
        {
            await ApplyChangeAsync(review, transaction, ct);
        }

        const string Sql = """
            UPDATE content_reviews
            SET status = 'approved', reviewed_at = NOW()
            WHERE status = 'pending' AND id = ANY(@Ids)
            """;

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Ids = pending.Select(r => r.Id).ToArray() },
                transaction: transaction, cancellationToken: ct));

        ct.ThrowIfCancellationRequested();
        transaction.Commit();

        _logger.LogInformation("Approved all {Count} pending reviews", rows);
        return rows;
    }

    /// <inheritdoc/>
    public async Task<int> RejectAllAsync(CancellationToken ct = default)
    {
        const string Sql = """
            UPDATE content_reviews
            SET status = 'rejected', reviewed_at = NOW()
            WHERE status = 'pending'
            """;

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, cancellationToken: ct));

        _logger.LogInformation("Rejected all {Count} pending reviews", rows);
        return rows;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateFixedValueAsync(long id, string fixedValue, CancellationToken ct = default)
    {
        const string Sql = """
            UPDATE content_reviews
            SET fixed_value = @FixedValue
            WHERE id = @Id AND status = 'pending'
            """;

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { Id = id, FixedValue = fixedValue }, cancellationToken: ct));

        if (rows > 0)
        {
            _logger.LogInformation("Updated fixed value for review {Id}", id);
        }

        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<int> PurgeOldReviewsAsync(int keepDays = 30, CancellationToken ct = default)
    {
        const string Sql = """
            DELETE FROM content_reviews
            WHERE status != 'pending'
              AND created_at < NOW() - INTERVAL '1 day' * @KeepDays
            """;

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql, new { KeepDays = keepDays }, cancellationToken: ct));

        if (rows > 0)
        {
            _logger.LogInformation("Purged {Count} old reviews (older than {Days} days)", rows, keepDays);
        }

        return rows;
    }

    // ── Internal helpers ─────────────────────────────────────────────────────

    private async Task<IReadOnlyList<ContentReview>> GetPendingForApprovalAsync(
        long? id, int limit, IDbTransaction transaction, CancellationToken ct)
    {
        // Lock the selected batch so concurrent approval, rejection, or edits cannot change it.
        const string Sql = """
            SELECT id, slug, collection_name AS CollectionName, change_type AS ChangeType,
                   original_value AS OriginalValue, fixed_value AS FixedValue, status
            FROM content_reviews
            WHERE status = 'pending' AND (@Id IS NULL OR id = @Id)
            ORDER BY created_at DESC, id DESC
            LIMIT @Limit
            FOR UPDATE
            """;

        return (await _connection.QueryAsync<ContentReview>(
            new CommandDefinition(Sql, new { Id = id, Limit = limit },
                transaction: transaction, cancellationToken: ct))).ToList();
    }

    private async Task ApplyChangeAsync(ContentReview review, IDbTransaction transaction, CancellationToken ct)
    {
        // Validation reviews are informational — approving acknowledges the issue,
        // but there is no automatic fix to apply.
        if (review.ChangeType == ContentReviewChangeType.Validation)
        {
            return;
        }

        var (column, value) = review.ChangeType switch
        {
            ContentReviewChangeType.Tags => ("tags_csv", review.FixedValue),
            ContentReviewChangeType.Markdown => ("content", review.FixedValue),
            ContentReviewChangeType.Author => ("author", review.FixedValue),
            _ => throw new InvalidOperationException($"Unknown change type: {review.ChangeType}")
        };

        var sql = $"""
            UPDATE content_items
            SET {column} = @Value, updated_at = NOW()
            WHERE collection_name = @Collection AND slug = @Slug
            """;

        await _connection.ExecuteAsync(
            new CommandDefinition(sql, new
            {
                Value = value,
                Collection = review.CollectionName,
                Slug = review.Slug
            }, transaction: transaction, cancellationToken: ct));

        if (review.ChangeType == ContentReviewChangeType.Tags)
        {
            await RebuildExpandedTagsAsync(review, transaction, ct);
        }
    }

    private async Task RebuildExpandedTagsAsync(ContentReview review, IDbTransaction transaction, CancellationToken ct)
    {
        var item = await _connection.QuerySingleOrDefaultAsync<TagFlagsRow>(new CommandDefinition(
            @"SELECT date_epoch AS DateEpoch, sections_bitmask AS SectionsBitmask,
                     is_ai AS IsAi, is_azure AS IsAzure, is_dotnet AS IsDotnet,
                     is_devops AS IsDevops, is_github_copilot AS IsGithubCopilot,
                     is_ml AS IsMl, is_security AS IsSecurity
              FROM content_items
              WHERE collection_name = @Collection AND slug = @Slug",
            new { Collection = review.CollectionName, Slug = review.Slug },
            transaction: transaction,
            cancellationToken: ct));

        if (item is null)
        {
            return;
        }

        await _connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM content_tags_expanded WHERE collection_name = @Collection AND slug = @Slug",
            new { Collection = review.CollectionName, Slug = review.Slug },
            transaction: transaction,
            cancellationToken: ct));

        var tags = (review.FixedValue ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .Where(t => t.Length > 0)
            .ToList();

        if (tags.Count == 0)
        {
            return;
        }

        var tagRows = ContentItemWriteRepository.BuildTagWords(
            tags, review.CollectionName, review.Slug, item.DateEpoch,
            item.IsAi, item.IsAzure, item.IsDotnet, item.IsDevops,
            item.IsGithubCopilot, item.IsMl, item.IsSecurity, item.SectionsBitmask);

        foreach (var row in tagRows)
        {
            await _connection.ExecuteAsync(new CommandDefinition(
                @"INSERT INTO content_tags_expanded
                    (collection_name, slug, tag_word, tag_display, is_full_tag,
                     date_epoch, is_ai, is_azure, is_dotnet, is_devops, is_github_copilot,
                     is_ml, is_security, sections_bitmask)
                  VALUES
                    (@CollectionName, @Slug, @TagWord, @TagDisplay, @IsFullTag,
                     @DateEpoch, @IsAi, @IsAzure, @IsDotnet, @IsDevops, @IsGhc,
                     @IsMl, @IsSecurity, @Bitmask)
                  ON CONFLICT DO NOTHING",
                row,
                transaction: transaction,
                cancellationToken: ct));
        }
    }

    private sealed class TagFlagsRow
    {
        public long DateEpoch { get; init; }
        public int SectionsBitmask { get; init; }
        public bool IsAi { get; init; }
        public bool IsAzure { get; init; }
        public bool IsDotnet { get; init; }
        public bool IsDevops { get; init; }
        public bool IsGithubCopilot { get; init; }
        public bool IsMl { get; init; }
        public bool IsSecurity { get; init; }
    }
}
