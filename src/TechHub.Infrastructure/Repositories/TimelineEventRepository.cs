using System.Data;
using System.Globalization;
using Dapper;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
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
                   plans           AS Plans,
                   surfaces        AS Surfaces,
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
            Plans = r.Plans,
            Surfaces = r.Surfaces,
            Description = r.Description,
            EventDate = DateOnly.FromDateTime(r.EventDate),
            DatePrecision = r.DatePrecision,
            DateSource = r.DateSource,
            Quote = r.Quote,
            ExtractedFrom = r.ExtractedFrom
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TimelineEntry>> GetTimelineAsync(TimelineRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.SectionName);

        var sectionName = request.SectionName.ToLowerInvariant().Trim();
        var sectionBits = sectionName == "all" ? SectionBitmasks.AllSections : SectionBitmasks.For(sectionName);
        if (sectionBits == 0)
        {
            return [];
        }

        const string Sql = """
            SELECT e.id              AS Id,
                   e.kind            AS Kind,
                   e.event_date      AS EventDate,
                   e.date_precision  AS DatePrecision,
                   e.description     AS Description,
                   e.product_id      AS ProductId,
                   COALESCE(p.name, e.product) AS Product,
                   e.feature_id      AS FeatureId,
                   COALESCE(f.name, e.feature) AS Feature,
                   e.collection_name AS CollectionName,
                   e.item_slug       AS ItemSlug,
                   ci.title          AS ItemTitle,
                   ci.external_url   AS ItemExternalUrl,
                   ci.primary_section_name AS ItemPrimarySectionName,
                   ci.date_epoch     AS ItemDateEpoch
            FROM content_timeline_events e
            JOIN content_items ci ON ci.collection_name = e.collection_name AND ci.slug = e.item_slug
            LEFT JOIN products p ON p.id = e.product_id
            LEFT JOIN product_features f ON f.id = e.feature_id
            WHERE ci.collection_name <> 'roundups'
              AND (ci.sections_bitmask & @SectionBits) > 0
              AND (@From::date IS NULL OR e.event_date >= @From::date)
              AND (@To::date IS NULL OR e.event_date <= @To::date)
              AND (cardinality(@Kinds::text[]) = 0 OR e.kind = ANY(@Kinds::text[]))
            ORDER BY ci.date_epoch, e.id
            """;

        var rows = (await _connection.QueryAsync<TimelineRow>(new CommandDefinition(
            Sql,
            new
            {
                SectionBits = sectionBits,
                From = request.From?.ToDateTime(TimeOnly.MinValue),
                To = request.To?.ToDateTime(TimeOnly.MinValue),
                Kinds = (request.Kinds ?? []).ToArray()
            },
            cancellationToken: ct))).ToList();

        if (rows.Count == 0)
        {
            return [];
        }

        const string QualifiersSql = """
            SELECT eq.event_id AS EventId, q.kind AS Kind, q.name AS Name
            FROM content_timeline_event_qualifiers eq
            JOIN product_qualifiers q ON q.id = eq.qualifier_id
            WHERE eq.event_id = ANY(@EventIds)
            """;

        var qualifiers = (await _connection.QueryAsync<TimelineQualifierRow>(new CommandDefinition(
            QualifiersSql, new { EventIds = rows.Select(r => r.Id).ToArray() }, cancellationToken: ct)))
            .ToLookup(q => q.EventId);

        // The same event stated by several articles is one entry. Rows are ordered by article date, so the first row of a
        // group is the oldest article and supplies the description.
        return rows
            .GroupBy(r => (r.EventDate, r.Kind, Product: r.ProductId?.ToString(CultureInfo.InvariantCulture) ?? r.Product.ToLowerInvariant(), Feature: (r.FeatureId?.ToString(CultureInfo.InvariantCulture) ?? r.Feature?.ToLowerInvariant()) ?? string.Empty))
            .Select(g =>
            {
                var first = g.First();
                var groupQualifiers = g.SelectMany(r => qualifiers[r.Id]).ToList();
                return new TimelineEntry
                {
                    Date = DateOnly.FromDateTime(first.EventDate),
                    DatePrecision = first.DatePrecision,
                    Kind = first.Kind,
                    Product = first.Product,
                    ProductId = first.ProductId,
                    Feature = first.Feature,
                    Description = first.Description,
                    Plans = QualifierNames(groupQualifiers, "plan"),
                    Surfaces = QualifierNames(groupQualifiers, "surface"),
                    Sources = g
                        .Select(r => new TimelineEntrySource
                        {
                            CollectionName = r.CollectionName,
                            Slug = r.ItemSlug,
                            Title = r.ItemTitle,
                            ExternalUrl = r.ItemExternalUrl,
                            PrimarySectionName = r.ItemPrimarySectionName
                        })
                        .DistinctBy(s => (s.CollectionName, s.Slug))
                        .ToList()
                };
            })
            .OrderByDescending(e => e.Date)
            .ThenBy(e => e.Product, StringComparer.OrdinalIgnoreCase)
            .ThenBy(e => e.Feature, StringComparer.OrdinalIgnoreCase)
            .ThenBy(e => e.Kind, StringComparer.Ordinal)
            .ToList();
    }

    private static List<string> QualifierNames(IEnumerable<TimelineQualifierRow> qualifiers, string kind) =>
        qualifiers
            .Where(q => q.Kind == kind)
            .Select(q => q.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToList();

    private sealed class TimelineRow
    {
        public long Id { get; init; }
        public string Kind { get; init; } = string.Empty;
        public DateTime EventDate { get; init; }
        public string DatePrecision { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public long? ProductId { get; init; }
        public string Product { get; init; } = string.Empty;
        public long? FeatureId { get; init; }
        public string? Feature { get; init; }
        public string CollectionName { get; init; } = string.Empty;
        public string ItemSlug { get; init; } = string.Empty;
        public string ItemTitle { get; init; } = string.Empty;
        public string? ItemExternalUrl { get; init; }
        public string? ItemPrimarySectionName { get; init; }
        public long ItemDateEpoch { get; init; }
    }

    private sealed class TimelineQualifierRow
    {
        public long EventId { get; init; }
        public string Kind { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }

    private sealed class StoredTimelineEventRow
    {
        public long Id { get; init; }
        public string CollectionName { get; init; } = string.Empty;
        public string ItemSlug { get; init; } = string.Empty;
        public string Kind { get; init; } = string.Empty;
        public string Product { get; init; } = string.Empty;
        public string? Feature { get; init; }
        public string[] Plans { get; init; } = [];
        public string[] Surfaces { get; init; } = [];
        public string Description { get; init; } = string.Empty;
        public DateTime EventDate { get; init; }
        public string DatePrecision { get; init; } = string.Empty;
        public string DateSource { get; init; } = string.Empty;
        public string? Quote { get; init; }
        public string ExtractedFrom { get; init; } = string.Empty;
    }
}
