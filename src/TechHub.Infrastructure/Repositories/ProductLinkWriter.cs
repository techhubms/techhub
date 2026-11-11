using System.Data;
using Dapper;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Keeps <c>content_item_products</c> (the product names an article lists, links through features, or names in events)
/// in sync with the article metadata and timeline events, inside a transaction owned by the caller.
/// </summary>
internal static class ProductLinkWriter
{
    /// <summary>
    /// Replaces the product links of an article with the products in its metadata, feature mentions and dated events.
    /// </summary>
    public static async Task ReplaceItemLinksAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        string collectionName,
        string slug,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(transaction);

        var parameters = new { CollectionName = collectionName, Slug = slug };

        await CatalogRelink.LockForLinkWriteAsync(connection, transaction, ct);

        await connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM content_item_products WHERE collection_name = @CollectionName AND item_slug = @Slug",
            parameters,
            transaction,
            cancellationToken: ct));

        await connection.ExecuteAsync(new CommandDefinition(
            """
            WITH src AS (
                SELECT regexp_replace(btrim(n.name), '\s+', ' ', 'g') AS name, n.ordinal
                FROM content_items ci
                CROSS JOIN LATERAL jsonb_array_elements_text(
                    CASE WHEN jsonb_typeof(ci.ai_metadata -> 'products') = 'array'
                         THEN ci.ai_metadata -> 'products'
                         ELSE '[]'::jsonb END) WITH ORDINALITY AS n (name, ordinal)
                WHERE ci.collection_name = @CollectionName AND ci.slug = @Slug
                UNION ALL
                SELECT regexp_replace(btrim(n.value ->> 'product'), '\s+', ' ', 'g') AS name, 1000 + n.ordinal
                FROM content_items ci
                CROSS JOIN LATERAL jsonb_array_elements(
                    CASE WHEN jsonb_typeof(ci.ai_metadata -> 'product_features') = 'array'
                         THEN ci.ai_metadata -> 'product_features'
                         ELSE '[]'::jsonb END) WITH ORDINALITY AS n (value, ordinal)
                WHERE ci.collection_name = @CollectionName AND ci.slug = @Slug
                  AND jsonb_typeof(n.value) = 'object'
                  AND btrim(COALESCE(n.value ->> 'product', '')) <> ''
                UNION ALL
                SELECT regexp_replace(btrim(e.product), '\s+', ' ', 'g') AS name,
                       100000 + ROW_NUMBER() OVER (ORDER BY e.event_date, e.product, e.feature)
                FROM content_timeline_events e
                WHERE e.collection_name = @CollectionName AND e.item_slug = @Slug
            ),
            firsts AS (
                SELECT DISTINCT ON (lower(name)) name, ordinal
                FROM src
                WHERE name <> ''
                ORDER BY lower(name), ordinal
            )
            INSERT INTO content_item_products (collection_name, item_slug, product_name, position)
            SELECT @CollectionName, @Slug, name,
                   (ROW_NUMBER() OVER (ORDER BY ordinal))::int - 1
            FROM firsts
            """,
            parameters,
            transaction,
            cancellationToken: ct));

        await connection.ExecuteAsync(new CommandDefinition(
            "SELECT relink_item_products(@CollectionName, @Slug)",
            parameters,
            transaction,
            cancellationToken: ct));
    }
}
