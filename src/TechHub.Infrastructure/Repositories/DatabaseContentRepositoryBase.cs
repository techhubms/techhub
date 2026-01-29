using System.Data;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Base class for database-backed content repositories.
/// Extends ContentRepositoryBase with shared Dapper-based database operations for SQLite and PostgreSQL.
/// </summary>
public abstract class DatabaseContentRepositoryBase : ContentRepositoryBase
{
    protected IDbConnection Connection { get; }
    protected ISqlDialect Dialect { get; }

    static DatabaseContentRepositoryBase()
    {
        // Register custom type handler to convert SQLite's Int64 booleans to C# bool
        SqlMapper.AddTypeHandler(new BooleanTypeHandler());
    }

    /// <summary>
    /// Type handler to convert SQLite's Int64 (0/1) to C# bool.
    /// SQLite doesn't have a native boolean type, so it stores booleans as integers.
    /// </summary>
    private sealed class BooleanTypeHandler : SqlMapper.TypeHandler<bool>
    {
        public override bool Parse(object value)
        {
            return value switch
            {
                long l => l != 0,
                int i => i != 0,
                bool b => b,
                _ => throw new ArgumentException($"Cannot convert {value.GetType()} to bool")
            };
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, bool value)
        {
            parameter.Value = value ? 1 : 0;
        }
    }

    protected DatabaseContentRepositoryBase(
        IDbConnection connection,
        ISqlDialect dialect,
        IMemoryCache cache,
        IMarkdownService markdownService)
        : base(cache, markdownService)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(dialect);

        Connection = connection;
        Dialect = dialect;
    }

    /// <summary>
    /// Internal implementation for getting content by slug.
    /// Uses SQL to query the database.
    /// </summary>
    protected override async Task<ContentItem?> GetBySlugInternalAsync(
        string collectionName,
        string slug,
        bool includeDraft,
        CancellationToken ct)
    {
        const string Sql = @"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.content AS Content,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport
            FROM content_items c
            WHERE c.collection_name = @collectionName
              AND c.slug = @slug
              AND (c.draft = 0 OR @includeDraft = 1)";

        var item = await Connection.QuerySingleOrDefaultAsync<ContentItem>(
            new CommandDefinition(Sql, new { collectionName, slug, includeDraft }, cancellationToken: ct));

        if (item != null)
        {
            var hydratedItems = await HydrateTagsAsync([item], ct);
            return hydratedItems.FirstOrDefault();
        }

        return null;
    }

    /// <summary>
    /// Internal implementation for getting all content.
    /// Uses SQL to query the database.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetAllInternalAsync(
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        const string Sql = @"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.content AS Content,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport
            FROM content_items c
            WHERE c.draft = 0 OR @includeDraft = 1
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(Sql, new { includeDraft, limit, offset }, cancellationToken: ct));

        return await HydrateTagsAsync([.. items], ct);
    }

    /// <summary>
    /// Internal implementation for getting content by collection.
    /// Uses SQL to query the database.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetByCollectionInternalAsync(
        string collectionName,
        string? subcollectionName,
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(collectionName);

        // Handle virtual "all" collection - return all items
        if (collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllInternalAsync(includeDraft, limit, offset, ct);
        }

        // Build WHERE clause - filter by collection and optionally by subcollection
        var whereClause = !string.IsNullOrWhiteSpace(subcollectionName)
            ? "c.collection_name = @collectionName AND c.subcollection_name = @subcollectionName"
            : "c.collection_name = @collectionName";

        var sql = $@"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.content AS Content,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport
            FROM content_items c
            WHERE {whereClause}
              AND (c.draft = 0 OR @includeDraft = 1)
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, subcollectionName, includeDraft, limit, offset }, cancellationToken: ct));

        return await HydrateTagsAsync([.. items], ct);
    }

    /// <summary>
    /// Internal implementation for getting content by section.
    /// Optionally filter by collection and subcollection.
    /// Uses SQL to query the database.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetBySectionInternalAsync(
        string sectionName,
        string? collectionName,
        string? subcollectionName,
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        // Handle virtual "all" section - return all items
        if (sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllInternalAsync(includeDraft, limit, offset, ct);
        }

        // Build dynamic SQL based on filters
        var sql = @"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.author AS Author,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.feed_name AS FeedName,
                c.primary_section_name AS PrimarySectionName,
                c.excerpt AS Excerpt,
                c.external_url AS ExternalUrl,
                c.draft AS Draft,
                c.content AS Content,
                c.subcollection_name AS SubcollectionName,
                c.plans AS Plans,
                c.ghes_support AS GhesSupport
            FROM content_items c
            INNER JOIN content_sections cs ON c.collection_name = cs.collection_name AND c.slug = cs.slug
            WHERE cs.section_name = @sectionName
              AND (c.draft = 0 OR @includeDraft = 1)";

        // Add collection filter if specified
        if (!string.IsNullOrWhiteSpace(collectionName))
        {
            sql += " AND c.collection_name = @collectionName";
        }

        // Add subcollection filter if specified
        if (!string.IsNullOrWhiteSpace(subcollectionName))
        {
            sql += " AND c.subcollection_name = @subcollectionName";
        }

        sql += " ORDER BY c.date_epoch DESC LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { sectionName, collectionName, subcollectionName, includeDraft, limit, offset }, cancellationToken: ct));

        return await HydrateTagsAsync([.. items], ct);
    }

    /// <summary>
    /// Hydrate Tags property from normalized database table.
    /// Uses single batch query with composite key filtering for optimal performance.
    /// </summary>
    private async Task<List<ContentItem>> HydrateTagsAsync(IList<ContentItem> items, CancellationToken ct = default)
    {
        if (items.Count == 0)
        {
            return [];
        }

        // Build row value constructor: WHERE (slug, collection_name) IN ((val1, val2), (val3, val4), ...)
        var parameters = new DynamicParameters();
        var valuePairs = new List<string>();

        for (int i = 0; i < items.Count; i++)
        {
            var slugParam = $"slug{i}";
            var collectionParam = $"collection{i}";
            parameters.Add(slugParam, items[i].Slug);
            parameters.Add(collectionParam, items[i].CollectionName);
            valuePairs.Add($"(@{slugParam}, @{collectionParam})");
        }

        var valuesClause = string.Join(", ", valuePairs);
        var sql = $@"
            SELECT ct.slug, ct.collection_name, ct.tag
            FROM content_tags ct
            WHERE (ct.slug, ct.collection_name) IN ({valuesClause})
            ORDER BY ct.slug, ct.collection_name, ct.tag";

        var tags = await Connection.QueryAsync<(string Slug, string CollectionName, string Tag)>(
            new CommandDefinition(sql, parameters, cancellationToken: ct));

        // Build lookup for fast tag assignment
        var tagLookup = tags.ToLookup(t => (t.Slug, t.CollectionName), t => t.Tag);

        // Hydrate tags into items (in-place mutation via SetTags method)
        foreach (var item in items)
        {
            var itemTags = tagLookup[(item.Slug, item.CollectionName)].ToArray();
            if (itemTags.Length > 0)
            {
                item.SetTags(itemTags);
            }
        }

        return (List<ContentItem>)items;
    }

    /// <summary>
    /// Legacy method for compatibility. Calls HydrateTagsAsync since plans are now in the items.
    /// </summary>
    protected override Task<List<ContentItem>> HydrateRelationshipsAsync(IList<ContentItem> items, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(items);
        return HydrateTagsAsync(items, ct);
    }
}
