using System.Data;
using Dapper;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// PostgreSQL implementation of <see cref="IGhcFeatureRepository"/>: manages the <c>vscode_update_items</c> table.
/// </summary>
public sealed class GhcFeatureRepository : IGhcFeatureRepository
{
    private readonly IDbConnection _connection;

    public GhcFeatureRepository(IDbConnection connection)
    {
        _connection = connection;
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
