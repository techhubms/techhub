using System.Data;
using Dapper;
using Npgsql;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IGhcFeatureRepository"/>.
/// Manages ghc_features, ghc_feature_content, and vscode_update_items tables.
/// </summary>
public sealed class GhcFeatureRepository : IGhcFeatureRepository
{
    private readonly IDbConnection _connection;

    public GhcFeatureRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    // ── GHC Features ──────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<IReadOnlyList<GhcFeature>> GetAllFeaturesAsync(CancellationToken ct = default)
    {
        const string Sql = @"
SELECT
    f.slug         AS Slug,
    f.title        AS Title,
    f.description  AS Description,
    f.release_date AS ReleaseDate,
    f.plans        AS PlansCsv,
    f.ghes_support AS GhesSupport,
    l.collection_name AS LinkCollectionName,
    l.item_slug       AS LinkItemSlug,
    l.is_thumbnail    AS LinkIsThumbnail,
    l.sort_order      AS LinkSortOrder,
    ci.title          AS LinkItemTitle,
    ci.external_url   AS LinkItemExternalUrl,
    ci.primary_section_name AS LinkItemPrimarySectionName
FROM ghc_features f
LEFT JOIN ghc_feature_content l ON l.feature_slug = f.slug
LEFT JOIN content_items ci
    ON ci.collection_name = l.collection_name
   AND ci.slug = l.item_slug
ORDER BY f.release_date DESC NULLS LAST, f.created_at DESC, f.slug,
         l.sort_order, l.item_slug";

        return await QueryFeaturesAsync(Sql, null, ct);
    }

    /// <inheritdoc/>
    public async Task<GhcFeature?> GetFeatureBySlugAsync(string slug, CancellationToken ct = default)
    {
        const string Sql = @"
SELECT
    f.slug         AS Slug,
    f.title        AS Title,
    f.description  AS Description,
    f.release_date AS ReleaseDate,
    f.plans        AS PlansCsv,
    f.ghes_support AS GhesSupport,
    l.collection_name AS LinkCollectionName,
    l.item_slug       AS LinkItemSlug,
    l.is_thumbnail    AS LinkIsThumbnail,
    l.sort_order      AS LinkSortOrder,
    ci.title          AS LinkItemTitle,
    ci.external_url   AS LinkItemExternalUrl,
    ci.primary_section_name AS LinkItemPrimarySectionName
FROM ghc_features f
LEFT JOIN ghc_feature_content l ON l.feature_slug = f.slug
LEFT JOIN content_items ci
    ON ci.collection_name = l.collection_name
   AND ci.slug = l.item_slug
WHERE f.slug = @Slug
ORDER BY l.sort_order, l.item_slug";

        var features = await QueryFeaturesAsync(Sql, new { Slug = slug }, ct);
        return features.Count > 0 ? features[0] : null;
    }

    /// <inheritdoc/>
    public async Task<bool> UpsertFeatureAsync(GhcFeature feature, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(feature);
        var plansCsv = string.Join(",",
            (feature.Plans ?? Enumerable.Empty<string>())
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p)));
        const string Sql = @"
INSERT INTO ghc_features (slug, title, description, release_date, plans, ghes_support, updated_at)
VALUES (@Slug, @Title, @Description, @ReleaseDate, @PlansCsv, @GhesSupport, NOW())
ON CONFLICT (slug) DO UPDATE SET
    title        = EXCLUDED.title,
    description  = EXCLUDED.description,
    release_date = EXCLUDED.release_date,
    plans        = EXCLUDED.plans,
    ghes_support = EXCLUDED.ghes_support,
    updated_at   = NOW()";

        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(Sql,
                new
                {
                    feature.Slug,
                    feature.Title,
                    feature.Description,
                    feature.ReleaseDate,
                    PlansCsv = plansCsv,
                    feature.GhesSupport
                },
                cancellationToken: ct));
        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFeatureAsync(string slug, CancellationToken ct = default)
    {
        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM ghc_features WHERE slug = @Slug",
                new { Slug = slug },
                cancellationToken: ct));
        return rows > 0;
    }

    // ── Content links ─────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public async Task<bool> AddContentLinkAsync(
        string featureSlug,
        string collectionName,
        string itemSlug,
        bool isThumbnail,
        int sortOrder,
        CancellationToken ct = default)
    {
        using var tx = _connection.BeginTransaction();
        try
        {
            if (isThumbnail)
            {
                // Clear any existing thumbnail for this feature first
                await _connection.ExecuteAsync(
                    new CommandDefinition(
                        "UPDATE ghc_feature_content SET is_thumbnail = FALSE WHERE feature_slug = @FeatureSlug AND is_thumbnail = TRUE",
                        new { FeatureSlug = featureSlug },
                        transaction: tx,
                        cancellationToken: ct));
            }

            var rows = await _connection.ExecuteAsync(
                new CommandDefinition(
                    @"INSERT INTO ghc_feature_content (feature_slug, collection_name, item_slug, is_thumbnail, sort_order)
                      VALUES (@FeatureSlug, @CollectionName, @ItemSlug, @IsThumbnail, @SortOrder)
                      ON CONFLICT (feature_slug, collection_name, item_slug) DO UPDATE SET
                          is_thumbnail = EXCLUDED.is_thumbnail,
                          sort_order   = EXCLUDED.sort_order",
                    new
                    {
                        FeatureSlug = featureSlug,
                        CollectionName = collectionName,
                        ItemSlug = itemSlug,
                        IsThumbnail = isThumbnail,
                        SortOrder = sortOrder
                    },
                    transaction: tx,
                    cancellationToken: ct));

            tx.Commit();
            return rows > 0;
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            // Foreign key violation: either feature_slug or (collection_name, item_slug) does not exist.
            tx.Rollback();
            return false;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveContentLinkAsync(
        string featureSlug,
        string collectionName,
        string itemSlug,
        CancellationToken ct = default)
    {
        var rows = await _connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM ghc_feature_content WHERE feature_slug = @FeatureSlug AND collection_name = @CollectionName AND item_slug = @ItemSlug",
                new { FeatureSlug = featureSlug, CollectionName = collectionName, ItemSlug = itemSlug },
                cancellationToken: ct));
        return rows > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> SetThumbnailAsync(
        string featureSlug,
        string collectionName,
        string itemSlug,
        CancellationToken ct = default)
    {
        using var tx = _connection.BeginTransaction();
        try
        {
            // Clear existing thumbnail
            await _connection.ExecuteAsync(
                new CommandDefinition(
                    "UPDATE ghc_feature_content SET is_thumbnail = FALSE WHERE feature_slug = @FeatureSlug AND is_thumbnail = TRUE",
                    new { FeatureSlug = featureSlug },
                    transaction: tx,
                    cancellationToken: ct));

            // Set new thumbnail
            var rows = await _connection.ExecuteAsync(
                new CommandDefinition(
                    "UPDATE ghc_feature_content SET is_thumbnail = TRUE WHERE feature_slug = @FeatureSlug AND collection_name = @CollectionName AND item_slug = @ItemSlug",
                    new { FeatureSlug = featureSlug, CollectionName = collectionName, ItemSlug = itemSlug },
                    transaction: tx,
                    cancellationToken: ct));

            tx.Commit();
            return rows > 0;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
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
        string? PlansCsv,
        bool GhesSupport,
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
                var plans = string.IsNullOrWhiteSpace(row.PlansCsv)
                    ? (IReadOnlyList<string>)[]
                    : row.PlansCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var feature = new GhcFeature
                {
                    Slug = row.Slug,
                    Title = row.Title,
                    Description = row.Description ?? string.Empty,
                    ReleaseDate = row.ReleaseDate,
                    Plans = plans,
                    GhesSupport = row.GhesSupport
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
