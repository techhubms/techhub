using System.Data;
using Dapper;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IGhcFeatureRepository"/>: manages the <c>vscode_update_items</c> table and reads the
/// features of the catalog that have content.
/// </summary>
public sealed class GhcFeatureRepository : IGhcFeatureRepository
{
    private readonly IDbConnection _connection;

    public GhcFeatureRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<SectionFeature>> GetFeaturesWithContentAsync(string productName, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productName);

        // The three sources of content are merged per item: the thumbnail and the admin's order win over a detected link. Roundups
        // state dates about everything, so they never count as content of a feature.
        const string Sql = """
            WITH tree AS (
                SELECT product_tree_ids(find_product_id(@ProductName)) AS id
            ),
            features AS (
                SELECT f.id, f.slug, f.name, f.description, p.name AS product_name,
                       (SELECT e.description FROM content_timeline_events e
                        WHERE e.feature_id = f.id AND btrim(e.description) <> ''
                        ORDER BY e.event_date DESC, e.id LIMIT 1) AS event_description,
                       COALESCE(f.release_date,
                                (SELECT EXTRACT(EPOCH FROM e.event_date)::bigint FROM content_timeline_events e
                                 WHERE e.feature_id = f.id
                                 ORDER BY e.event_date DESC, e.id LIMIT 1)) AS release_date
                FROM product_features f
                JOIN products p ON p.id = f.product_id
                WHERE f.product_id IN (SELECT id FROM tree)
            ),
            links AS (
                SELECT l.feature_id, l.collection_name, l.item_slug, l.is_thumbnail, l.sort_order, FALSE AS is_detected
                FROM product_feature_content l
                JOIN features f ON f.id = l.feature_id
                UNION ALL
                SELECT e.feature_id, e.collection_name, e.item_slug, FALSE, 2147483647, TRUE
                FROM content_timeline_events e
                JOIN features f ON f.id = e.feature_id
                WHERE e.collection_name <> 'roundups'
                UNION ALL
                SELECT m.feature_id, m.collection_name, m.item_slug, FALSE, 2147483647, TRUE
                FROM (SELECT cip.collection_name, cip.item_slug,
                             find_feature_id(cip.product_id, cip.feature_name) AS feature_id
                      FROM content_item_products cip
                      WHERE cip.feature_name <> ''
                        AND cip.collection_name <> 'roundups'
                        AND cip.product_id IN (SELECT id FROM tree)) m
                JOIN features f ON f.id = m.feature_id
            ),
            merged AS (
                SELECT feature_id, collection_name, item_slug,
                       bool_or(is_thumbnail) AS is_thumbnail,
                       min(sort_order) AS sort_order,
                       bool_and(is_detected) AS is_detected
                FROM links
                GROUP BY feature_id, collection_name, item_slug
            )
            SELECT f.slug AS Slug,
                   f.name AS Name,
                   f.product_name AS ProductName,
                   f.description AS Description,
                   f.event_description AS EventDescription,
                   f.release_date AS ReleaseDate,
                   m.collection_name AS CollectionName,
                   m.item_slug AS ItemSlug,
                   ci.title AS ItemTitle,
                   ci.excerpt AS ItemExcerpt,
                   ci.external_url AS ItemExternalUrl,
                   ci.primary_section_name AS ItemPrimarySectionName,
                   m.is_thumbnail AS IsThumbnail,
                   m.is_detected AS IsDetected
            FROM merged m
            JOIN features f ON f.id = m.feature_id
            JOIN content_items ci ON ci.collection_name = m.collection_name AND ci.slug = m.item_slug
            ORDER BY f.release_date DESC NULLS LAST, lower(f.name), f.slug,
                     m.is_thumbnail DESC, m.sort_order, ci.date_epoch DESC, m.item_slug
            """;

        var rows = await _connection.QueryAsync<FeatureContentRow>(
            new CommandDefinition(Sql, new { ProductName = productName }, cancellationToken: ct));

        // The query returns the rows of one feature together and in display order, so a group keeps the order of the rows.
        return rows
            .GroupBy(r => r.Slug)
            .Select(g =>
            {
                var first = g.First();
                return new SectionFeature
                {
                    Slug = first.Slug,
                    Name = first.Name,
                    ProductName = first.ProductName,
                    Description = DescriptionOf(g),
                    ReleaseDate = first.ReleaseDate is { } epoch
                        ? DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(epoch).UtcDateTime)
                        : null,
                    Content = g.Select(r => new SectionFeatureContent
                    {
                        CollectionName = r.CollectionName,
                        ItemSlug = r.ItemSlug,
                        Title = r.ItemTitle,
                        ExternalUrl = r.ItemExternalUrl ?? string.Empty,
                        PrimarySectionName = r.ItemPrimarySectionName,
                        IsThumbnail = r.IsThumbnail,
                        IsDetected = r.IsDetected
                    }).ToList()
                };
            })
            .ToList();
    }

    // A feature without a description of its own is described by its newest dated event, and failing that by the excerpt of its first item.
    private static string DescriptionOf(IEnumerable<FeatureContentRow> rows)
    {
        var first = rows.First();
        foreach (var candidate in new[] { first.Description, first.EventDescription, first.ItemExcerpt })
        {
            if (!string.IsNullOrWhiteSpace(candidate))
            {
                return candidate.Trim();
            }
        }

        return string.Empty;
    }

    private sealed class FeatureContentRow
    {
        public string Slug { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string ProductName { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? EventDescription { get; init; }
        public string? ItemExcerpt { get; init; }
        public long? ReleaseDate { get; init; }
        public string CollectionName { get; init; } = string.Empty;
        public string ItemSlug { get; init; } = string.Empty;
        public string ItemTitle { get; init; } = string.Empty;
        public string? ItemExternalUrl { get; init; }
        public string ItemPrimarySectionName { get; init; } = string.Empty;
        public bool IsThumbnail { get; init; }
        public bool IsDetected { get; init; }
    }

    /// <inheritdoc/>
    public async Task AddVscodeUpdateItemAsync(string collectionName, string slug, CancellationToken ct = default)
    {
        await _connection.ExecuteAsync(
            new CommandDefinition(
                "INSERT INTO vscode_update_items (collection_name, slug) VALUES (@CollectionName, @Slug) ON CONFLICT DO NOTHING",
                new { CollectionName = collectionName, Slug = slug },
                cancellationToken: ct));
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<VscodeUpdateListItem> Items, int TotalCount)> GetVscodeUpdateItemsAsync(
        int offset,
        int pageSize,
        string? search,
        CancellationToken ct = default)
    {
        var searchParam = string.IsNullOrWhiteSpace(search) ? null : $"%{search.Trim()}%";

        const string CountSql = @"
SELECT COUNT(*)
FROM vscode_update_items v
JOIN content_items ci ON ci.collection_name = v.collection_name AND ci.slug = v.slug
WHERE @Search IS NULL OR ci.title ILIKE @Search OR ci.slug ILIKE @Search";

        var totalCount = await _connection.ExecuteScalarAsync<int>(
            new CommandDefinition(CountSql, new { Search = searchParam }, cancellationToken: ct));

        const string ItemsSql = @"
SELECT
    v.collection_name AS CollectionName,
    v.slug            AS Slug,
    ci.title          AS Title,
    ci.external_url   AS ExternalUrl,
    ci.primary_section_name AS PrimarySectionName,
    ci.date_epoch     AS DateEpoch,
    ci.created_at     AS CreatedAt
FROM vscode_update_items v
JOIN content_items ci ON ci.collection_name = v.collection_name AND ci.slug = v.slug
WHERE @Search IS NULL OR ci.title ILIKE @Search OR ci.slug ILIKE @Search
ORDER BY ci.date_epoch DESC, ci.slug
LIMIT @PageSize OFFSET @Offset";

        var items = await _connection.QueryAsync<VscodeUpdateListItem>(
            new CommandDefinition(ItemsSql, new { Search = searchParam, PageSize = pageSize, Offset = offset }, cancellationToken: ct));

        return (items.ToList(), totalCount);
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveVscodeUpdateItemAsync(string collectionName, string slug, CancellationToken ct = default)
    {
        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM vscode_update_items WHERE collection_name = @CollectionName AND slug = @Slug",
                new { CollectionName = collectionName, Slug = slug },
                cancellationToken: ct));
        return rows > 0;
    }
}
