using System.Data;
using Dapper;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Keeps <c>content_item_products</c> (the product names an article lists, linked to the catalog) in sync with the
/// <c>ai_metadata</c> of the article, inside a transaction owned by the caller.
/// </summary>
internal static class ProductLinkWriter
{
    /// <summary>
    /// Replaces the product links of an article with the products in its stored <c>ai_metadata</c>. Only the current
    /// metadata schema lists products; older or missing metadata leaves the article without product links.
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
                    CASE WHEN COALESCE((ci.ai_metadata ->> 'schema_version') ~ '^\d+$', FALSE)
                              AND (ci.ai_metadata ->> 'schema_version')::int >= 2
                              AND jsonb_typeof(ci.ai_metadata -> 'products') = 'array'
                         THEN ci.ai_metadata -> 'products'
                         ELSE '[]'::jsonb END) WITH ORDINALITY AS n (name, ordinal)
                WHERE ci.collection_name = @CollectionName AND ci.slug = @Slug
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
