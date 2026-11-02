using System.Data;
using Dapper;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IGhcFeatureRepository"/>. The features are the features of the GitHub Copilot
/// product (and its child products) in the catalog: <c>product_features</c>, with <c>product_feature_content</c> for the
/// content they link to. It also manages the <c>vscode_update_items</c> table.
/// </summary>
public sealed class GhcFeatureRepository : IGhcFeatureRepository
{
    private const string FeatureSelect = @"
SELECT
    f.slug         AS Slug,
    f.name         AS Title,
    f.description  AS Description,
    COALESCE(f.release_date, (
        SELECT EXTRACT(EPOCH FROM MIN(e.event_date))::bigint
        FROM content_timeline_events e
        WHERE e.feature_id = f.id AND e.kind IN ('launch', 'preview', 'ga', 'release'))) AS ReleaseDate,
    l.collection_name AS LinkCollectionName,
    l.item_slug       AS LinkItemSlug,
    l.is_thumbnail    AS LinkIsThumbnail,
    l.sort_order      AS LinkSortOrder,
    ci.title          AS LinkItemTitle,
    ci.external_url   AS LinkItemExternalUrl,
    ci.primary_section_name AS LinkItemPrimarySectionName
FROM product_features f
LEFT JOIN product_feature_content l ON l.feature_id = f.id
LEFT JOIN content_items ci
    ON ci.collection_name = l.collection_name
   AND ci.slug = l.item_slug";

    private const string InGitHubCopilot =
        "f.product_id IN (SELECT product_tree_ids(p.id) FROM products p WHERE lower(p.name) = 'github copilot')";

    private readonly IDbConnection _connection;

    public GhcFeatureRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    // ── GHC Features ──────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<IReadOnlyList<GhcFeature>> GetAllFeaturesAsync(CancellationToken ct = default)
    {
        const string Sql = FeatureSelect + @"
WHERE " + InGitHubCopilot + @"
ORDER BY ReleaseDate DESC NULLS LAST, f.created_at DESC, f.slug,
         l.sort_order, l.item_slug";

        return await QueryFeaturesAsync(Sql, null, ct);
    }

    /// <inheritdoc/>
    public async Task<GhcFeature?> GetFeatureBySlugAsync(string slug, CancellationToken ct = default)
    {
        const string Sql = FeatureSelect + @"
WHERE f.slug = @Slug AND " + InGitHubCopilot + @"
ORDER BY l.sort_order, l.item_slug";

        var features = await QueryFeaturesAsync(Sql, new { Slug = slug }, ct);
        return features.Count > 0 ? features[0] : null;
    }

    // ── VS Code Updates ────────────────────────────────────────────────────────

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

    // ── Private helpers ───────────────────────────────────────────────────────

    private sealed record FeatureRow(
        string Slug,
        string Title,
        string? Description,
        long? ReleaseDate,
        string? LinkCollectionName,
        string? LinkItemSlug,
        bool? LinkIsThumbnail,
        int? LinkSortOrder,
        string? LinkItemTitle,
        string? LinkItemExternalUrl,
        string? LinkItemPrimarySectionName
    );

    private async Task<IReadOnlyList<GhcFeature>> QueryFeaturesAsync(
        string sql,
        object? parameters,
        CancellationToken ct)
    {
        // Flatten feature + link rows, then group by feature slug.
        // Using a typed DTO avoids dynamic DapperRow case-sensitivity issues.
        // slugOrder tracks first-seen insertion order so we can return features
        // in SQL ORDER BY sequence — Dictionary<>.Values does not guarantee order.
        var featureDict = new Dictionary<string, (GhcFeature Feature, List<GhcFeatureContentLink> Links)>(StringComparer.OrdinalIgnoreCase);
        var slugOrder = new List<string>();
        var rows = await _connection.QueryAsync<FeatureRow>(
            new CommandDefinition(sql, parameters, cancellationToken: ct));

        foreach (var row in rows)
        {
            if (!featureDict.TryGetValue(row.Slug, out var entry))
            {
                var feature = new GhcFeature
                {
                    Slug = row.Slug,
                    Title = row.Title,
                    Description = row.Description ?? string.Empty,
                    ReleaseDate = row.ReleaseDate
                };

                entry = (feature, []);
                featureDict[row.Slug] = entry;
                slugOrder.Add(row.Slug);
            }

            if (row.LinkItemSlug is not null)
            {
                entry.Links.Add(new GhcFeatureContentLink
                {
                    FeatureSlug = row.Slug,
                    CollectionName = row.LinkCollectionName!,
                    ItemSlug = row.LinkItemSlug,
                    IsThumbnail = row.LinkIsThumbnail ?? false,
                    SortOrder = row.LinkSortOrder ?? 0,
                    ItemTitle = row.LinkItemTitle,
                    ItemExternalUrl = row.LinkItemExternalUrl,
                    ItemPrimarySectionName = row.LinkItemPrimarySectionName
                });
            }
        }

        return slugOrder
            .Select(slug =>
            {
                var e = featureDict[slug];
                return e.Feature with { ContentLinks = e.Links };
            })
            .ToList();
    }
}
