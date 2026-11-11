using System.Data;
using System.Text.Json;
using Dapper;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Replaces the timeline events of an article inside a transaction owned by the caller, so an article write and its
/// events commit or roll back together.
/// </summary>
internal static class TimelineEventWriter
{
    /// <summary>
    /// SQL expression for the opaque revision of a <c>content_items</c> row: microseconds since the epoch of
    /// <c>updated_at</c>, which every edit of an article sets to NOW().
    /// </summary>
    public const string RevisionExpression = "COALESCE((EXTRACT(EPOCH FROM updated_at) * 1000000)::bigint, 0)";

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

        // After the row lock, never before it: a writer that holds the shared catalog lock while it waits for an article
        // row can deadlock with a catalog change that waits for that writer and an article edit that holds the row.
        await CatalogRelink.LockForLinkWriteAsync(connection, transaction, ct);

        await connection.ExecuteAsync(new CommandDefinition(
            $"DELETE FROM content_timeline_events WHERE collection_name = @CollectionName AND item_slug = @Slug AND extracted_from <> '{ITimelineEventRepository.ExtractedFromCatalog}'",
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
            "SELECT relink_timeline_events(@CollectionName, @Slug)",
            new { CollectionName = collectionName, Slug = slug },
            transaction,
            cancellationToken: ct));

        if (replacement.Products is not null || replacement.ProductFeatures is not null)
        {
            await ReplaceCatalogMentionsAsync(connection, transaction, collectionName, slug, replacement, ct);
        }

        await ProductLinkWriter.ReplaceItemLinksAsync(connection, transaction, collectionName, slug, ct);

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

    /// <summary>
    /// Stores products and feature mentions in the <c>ai_metadata</c> of the article and relinks them to the catalog. An article without
    /// metadata gets an object with only the extracted names (unless there are none): it has no <c>schema_version</c> of the
    /// current schema, so the roundup metadata backfill still classifies it and replaces the object. Dropping the products
    /// instead would leave the article unlinked while the scan marker keeps later runs from naming them again.
    /// <c>updated_at</c> stays as it is, because the revision of the article must keep describing its text.
    /// </summary>
    private static async Task ReplaceCatalogMentionsAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        string collectionName,
        string slug,
        TimelineEventReplacement replacement,
        CancellationToken ct)
    {
        var productsJson = replacement.Products is null ? null : JsonSerializer.Serialize(replacement.Products);
        var productFeaturesJson = replacement.ProductFeatures is null
            ? null
            : JsonSerializer.Serialize(replacement.ProductFeatures.Select(feature => new { product = feature.Product, feature = feature.Feature }));
        await connection.ExecuteAsync(new CommandDefinition(
            """
            UPDATE content_items
               SET ai_metadata = COALESCE(ai_metadata, '{}'::jsonb)
                    || CASE WHEN @Products::jsonb IS NULL THEN '{}'::jsonb ELSE jsonb_build_object('products', @Products::jsonb) END
                    || CASE WHEN @ProductFeatures::jsonb IS NULL THEN '{}'::jsonb ELSE jsonb_build_object('product_features', @ProductFeatures::jsonb) END
             WHERE collection_name = @CollectionName
               AND slug = @Slug
               AND (ai_metadata IS NOT NULL
                    OR COALESCE(jsonb_array_length(@Products::jsonb), 0) > 0
                    OR COALESCE(jsonb_array_length(@ProductFeatures::jsonb), 0) > 0)
            """,
            new { CollectionName = collectionName, Slug = slug, Products = productsJson, ProductFeatures = productFeaturesJson },
            transaction,
            cancellationToken: ct));
    }
}
