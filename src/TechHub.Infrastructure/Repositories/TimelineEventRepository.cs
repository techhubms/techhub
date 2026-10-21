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
            await _connection.ExecuteAsync(new CommandDefinition(
                "DELETE FROM content_timeline_events WHERE collection_name = @CollectionName AND item_slug = @Slug",
                new { CollectionName = collectionName, Slug = slug },
                transaction,
                cancellationToken: ct));

            foreach (var timelineEvent in events)
            {
                await _connection.ExecuteAsync(new CommandDefinition(
                    """
                    INSERT INTO content_timeline_events
                        (collection_name, item_slug, kind, product, feature, description,
                         event_date, date_precision, date_source, quote, extracted_from)
                    VALUES
                        (@CollectionName, @Slug, @Kind, @Product, @Feature, @Description,
                         @EventDate, @DatePrecision, @DateSource, @Quote, @ExtractedFrom)
                    """,
                    new
                    {
                        CollectionName = collectionName,
                        Slug = slug,
                        timelineEvent.Kind,
                        timelineEvent.Product,
                        timelineEvent.Feature,
                        timelineEvent.Description,
                        EventDate = timelineEvent.Date.ToDateTime(TimeOnly.MinValue),
                        timelineEvent.DatePrecision,
                        timelineEvent.DateSource,
                        timelineEvent.Quote,
                        ExtractedFrom = extractedFrom
                    },
                    transaction,
                    cancellationToken: ct));
            }

            await _connection.ExecuteAsync(new CommandDefinition(
                """
                INSERT INTO content_timeline_extractions
                    (collection_name, item_slug, prompt_version, events_found, extracted_from, extracted_at)
                VALUES
                    (@CollectionName, @Slug, @PromptVersion, @EventsFound, @ExtractedFrom, NOW())
                ON CONFLICT (collection_name, item_slug) DO UPDATE SET
                    prompt_version = EXCLUDED.prompt_version,
                    events_found   = EXCLUDED.events_found,
                    extracted_from = EXCLUDED.extracted_from,
                    extracted_at   = NOW()
                """,
                new
                {
                    CollectionName = collectionName,
                    Slug = slug,
                    PromptVersion = promptVersion,
                    EventsFound = events.Count,
                    ExtractedFrom = extractedFrom
                },
                transaction,
                cancellationToken: ct));

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
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
