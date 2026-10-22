using System.Data;
using Dapper;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="ITimelineEventRepository"/>.
/// Manages <c>content_timeline_events</c> and <c>content_timeline_extractions</c>.
/// </summary>
public sealed class TimelineEventRepository : ITimelineEventRepository
{
    private const string RevisionExpression = TimelineEventWriter.RevisionExpression;

    private readonly IDbConnection _connection;

    public TimelineEventRepository(IDbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        _connection = connection;
    }

    /// <inheritdoc />
    public async Task ReplaceEventsAsync(
        string collectionName,
        string slug,
        IReadOnlyList<TimelineEvent> events,
        string extractedFrom,
        int promptVersion,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);
        ArgumentNullException.ThrowIfNull(events);
        ArgumentException.ThrowIfNullOrWhiteSpace(extractedFrom);

        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        using var transaction = _connection.BeginTransaction();

        try
        {
            await TimelineEventWriter.ReplaceAsync(
                _connection,
                transaction,
                collectionName,
                slug,
                new TimelineEventReplacement(events, extractedFrom, promptVersion),
                ct);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ReplaceEventsIfUnchangedAsync(
        string collectionName,
        string slug,
        IReadOnlyList<TimelineEvent> events,
        string extractedFrom,
        int promptVersion,
        long expectedRevision,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);
        ArgumentNullException.ThrowIfNull(events);
        ArgumentException.ThrowIfNullOrWhiteSpace(extractedFrom);

        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        using var transaction = _connection.BeginTransaction();

        try
        {
            // Lock the article first, then compare: an edit that committed meanwhile is visible and an edit that is
            // still running blocks here until it has committed.
            var currentRevision = await _connection.QuerySingleOrDefaultAsync<long?>(new CommandDefinition(
                $"SELECT {RevisionExpression} FROM content_items WHERE collection_name = @CollectionName AND slug = @Slug FOR NO KEY UPDATE",
                new { CollectionName = collectionName, Slug = slug },
                transaction,
                cancellationToken: ct));

            if (currentRevision != expectedRevision)
            {
                transaction.Rollback();
                return false;
            }

            await TimelineEventWriter.ReplaceAsync(
                _connection,
                transaction,
                collectionName,
                slug,
                new TimelineEventReplacement(events, extractedFrom, promptVersion),
                ct);

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TimelineBackfillCandidate>> GetBackfillCandidatesAsync(
        TimelineBackfillRequest request,
        int promptVersion,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        const string Sql = """
            SELECT ci.collection_name AS CollectionName,
                   ci.slug            AS Slug
            FROM content_items ci
            LEFT JOIN content_timeline_extractions x
              ON x.collection_name = ci.collection_name AND x.item_slug = ci.slug
            WHERE ci.collection_name <> 'roundups'
              AND (@Force OR x.item_slug IS NULL OR x.prompt_version < @PromptVersion)
              AND (@Collection::text IS NULL OR ci.collection_name = @Collection::text)
              AND (@AfterEpoch::bigint IS NULL OR ci.date_epoch >= @AfterEpoch::bigint)
            ORDER BY ci.date_epoch DESC, ci.slug
            LIMIT @Limit::int
            """;

        long? afterEpoch = null;
        if (request.PublishedAfter is { } after)
        {
            var brussels = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
            afterEpoch = new DateTimeOffset(after.ToDateTime(TimeOnly.MinValue), brussels.GetUtcOffset(after.ToDateTime(TimeOnly.MinValue))).ToUnixTimeSeconds();
        }

        var rows = await _connection.QueryAsync<TimelineBackfillCandidate>(new CommandDefinition(
            Sql,
            new
            {
                request.Force,
                PromptVersion = promptVersion,
                Collection = string.IsNullOrWhiteSpace(request.Collection) ? null : request.Collection.Trim().ToLowerInvariant(),
                AfterEpoch = afterEpoch,
                Limit = request.Limit
            },
            cancellationToken: ct));

        return rows.ToList();
    }

    /// <inheritdoc />
    public async Task<TimelineBackfillItem?> GetItemForExtractionAsync(string collectionName, string slug, CancellationToken ct = default)
    {
        const string Sql = $"""
            SELECT collection_name AS CollectionName,
                   slug            AS Slug,
                   title           AS Title,
                   excerpt         AS Excerpt,
                   content         AS Content,
                   external_url    AS ExternalUrl,
                   feed_name       AS FeedName,
                   date_epoch      AS DateEpoch,
                   {RevisionExpression} AS Revision
            FROM content_items
            WHERE collection_name = @CollectionName AND slug = @Slug
            """;

        var row = await _connection.QuerySingleOrDefaultAsync<ExtractionRow>(new CommandDefinition(
            Sql, new { CollectionName = collectionName, Slug = slug }, cancellationToken: ct));

        return row is null
            ? null
            : new TimelineBackfillItem
            {
                CollectionName = row.CollectionName,
                Slug = row.Slug,
                Title = row.Title,
                Excerpt = row.Excerpt ?? string.Empty,
                Content = row.Content,
                ExternalUrl = row.ExternalUrl ?? string.Empty,
                FeedName = row.FeedName ?? "TechHub",
                DateEpoch = row.DateEpoch,
                Revision = row.Revision
            };
    }

    private sealed class ExtractionRow
    {
        public string CollectionName { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string? Excerpt { get; init; }
        public string Content { get; init; } = string.Empty;
        public string? ExternalUrl { get; init; }
        public string? FeedName { get; init; }
        public long DateEpoch { get; init; }
        public long Revision { get; init; }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<StoredTimelineEvent>> GetEventsForItemAsync(
        string collectionName,
        string slug,
        CancellationToken ct = default)
    {
        const string Sql = """
            SELECT id              AS Id,
                   collection_name AS CollectionName,
                   item_slug       AS ItemSlug,
                   kind            AS Kind,
                   product         AS Product,
                   feature         AS Feature,
                   description     AS Description,
                   event_date      AS EventDate,
                   date_precision  AS DatePrecision,
                   date_source     AS DateSource,
                   quote           AS Quote,
                   extracted_from  AS ExtractedFrom
            FROM content_timeline_events
            WHERE collection_name = @CollectionName AND item_slug = @Slug
            ORDER BY event_date, id
            """;

        var rows = await _connection.QueryAsync<StoredTimelineEventRow>(new CommandDefinition(
            Sql, new { CollectionName = collectionName, Slug = slug }, cancellationToken: ct));

        return rows.Select(r => new StoredTimelineEvent
        {
            Id = r.Id,
            CollectionName = r.CollectionName,
            ItemSlug = r.ItemSlug,
            Kind = r.Kind,
            Product = r.Product,
            Feature = r.Feature,
            Description = r.Description,
            EventDate = DateOnly.FromDateTime(r.EventDate),
            DatePrecision = r.DatePrecision,
            DateSource = r.DateSource,
            Quote = r.Quote,
            ExtractedFrom = r.ExtractedFrom
        }).ToList();
    }

    private sealed class StoredTimelineEventRow
    {
        public long Id { get; init; }
        public string CollectionName { get; init; } = string.Empty;
        public string ItemSlug { get; init; } = string.Empty;
        public string Kind { get; init; } = string.Empty;
        public string Product { get; init; } = string.Empty;
        public string? Feature { get; init; }
        public string Description { get; init; } = string.Empty;
        public DateTime EventDate { get; init; }
        public string DatePrecision { get; init; } = string.Empty;
        public string DateSource { get; init; } = string.Empty;
        public string? Quote { get; init; }
        public string ExtractedFrom { get; init; } = string.Empty;
    }
}
