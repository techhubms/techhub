using System.Data;
using Dapper;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Replaces the timeline events of an article inside a transaction owned by the caller, so an article write and its
/// events commit or roll back together.
/// </summary>
internal static class TimelineEventWriter
{
    public static async Task ReplaceAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        string collectionName,
        string slug,
        TimelineEventReplacement replacement,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(transaction);
        ArgumentNullException.ThrowIfNull(replacement);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);
        ArgumentNullException.ThrowIfNull(replacement.Events);
        ArgumentException.ThrowIfNullOrWhiteSpace(replacement.ExtractedFrom);

        // Serialize replacements per article: without the lock two concurrent replacements can both finish
        // their DELETE before either INSERTs, leaving both event sets stored.
        await connection.ExecuteAsync(new CommandDefinition(
            "SELECT 1 FROM content_items WHERE collection_name = @CollectionName AND slug = @Slug FOR NO KEY UPDATE",
            new { CollectionName = collectionName, Slug = slug },
            transaction,
            cancellationToken: ct));

        await connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM content_timeline_events WHERE collection_name = @CollectionName AND item_slug = @Slug",
            new { CollectionName = collectionName, Slug = slug },
            transaction,
            cancellationToken: ct));

        foreach (var timelineEvent in replacement.Events)
        {
            await connection.ExecuteAsync(new CommandDefinition(
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
                    replacement.ExtractedFrom
                },
                transaction,
                cancellationToken: ct));
        }

        await connection.ExecuteAsync(new CommandDefinition(
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
                replacement.PromptVersion,
                EventsFound = replacement.Events.Count,
                replacement.ExtractedFrom
            },
            transaction,
            cancellationToken: ct));
    }
}
