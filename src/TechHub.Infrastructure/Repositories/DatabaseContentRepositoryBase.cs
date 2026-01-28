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
    protected readonly IDbConnection Connection;
    protected readonly ISqlDialect Dialect;

    protected DatabaseContentRepositoryBase(
        IDbConnection connection,
        ISqlDialect dialect,
        IMemoryCache cache,
        IMarkdownService markdownService)
        : base(cache, markdownService)
    {
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        Dialect = dialect ?? throw new ArgumentNullException(nameof(dialect));
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
        const string sql = @"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.content AS Content,
                c.excerpt AS Excerpt,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.subcollection_name AS SubcollectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft
            FROM content_items c
            WHERE c.collection_name = @collectionName
              AND c.slug = @slug
              AND (c.draft = 0 OR @includeDraft = 1)";

        var item = await Connection.QuerySingleOrDefaultAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, slug, includeDraft }, cancellationToken: ct));

        if (item != null)
        {
            var hydratedItems = await HydrateRelationshipsAsync([item], ct);
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
        CancellationToken ct)
    {
        const string sql = @"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.content AS Content,
                c.excerpt AS Excerpt,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.subcollection_name AS SubcollectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft
            FROM content_items c
            WHERE c.draft = 0 OR @includeDraft = 1
            ORDER BY c.date_epoch DESC";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { includeDraft }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await HydrateRelationshipsAsync(itemsList, ct);
    }

    /// <summary>
    /// Internal implementation for getting content by collection.
    /// Uses SQL to query the database.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetByCollectionInternalAsync(
        string collectionName,
        bool includeDraft,
        CancellationToken ct)
    {
        // Handle virtual "all" collection - return all items
        if (collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllInternalAsync(includeDraft, ct);
        }

        // Special handling for "videos" collection - includes all video subcollections
        var whereClause = collectionName.Equals("videos", StringComparison.OrdinalIgnoreCase)
            ? "c.collection_name IN ('ghc-features', 'vscode-updates', 'videos')"
            : "c.collection_name = @collectionName";

        var sql = $@"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.content AS Content,
                c.excerpt AS Excerpt,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.subcollection_name AS SubcollectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft
            FROM content_items c
            WHERE {whereClause}
              AND (c.draft = 0 OR @includeDraft = 1)
            ORDER BY c.date_epoch DESC";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, includeDraft }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await HydrateRelationshipsAsync(itemsList, ct);
    }

    /// <summary>
    /// Internal implementation for getting content by section.
    /// Uses SQL to query the database.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetBySectionInternalAsync(
        string sectionName,
        bool includeDraft,
        CancellationToken ct)
    {
        // Handle virtual "all" section - return all items
        if (sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllInternalAsync(includeDraft, ct);
        }

        const string sql = @"
            SELECT 
                c.slug AS Slug,
                c.title AS Title,
                c.content AS Content,
                c.excerpt AS Excerpt,
                c.date_epoch AS DateEpoch,
                c.collection_name AS CollectionName,
                c.subcollection_name AS SubcollectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft
            FROM content_items c
            INNER JOIN content_sections cs ON c.slug = cs.slug
            WHERE cs.section_name = @sectionName
              AND (c.draft = 0 OR @includeDraft = 1)
            ORDER BY c.date_epoch DESC";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { sectionName, includeDraft }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await HydrateRelationshipsAsync(itemsList, ct);
    }

    /// <summary>
    /// Hydrate relationship properties (tags, sections, plans) from normalized database tables.
    /// Uses efficient multi-query approach to minimize database round-trips.
    /// </summary>
    protected override async Task<List<ContentItem>> HydrateRelationshipsAsync(IList<ContentItem> items, CancellationToken ct = default)
    {
        if (items.Count == 0)
        {
            return [];
        }

        var slugs = items.Select(i => i.Slug).ToList();

        // Load tags
        const string tagsSql = @"
            SELECT slug AS ContentId, tag AS Tag
            FROM content_tags
            WHERE slug IN @slugs
            ORDER BY tag";

        var tags = await Connection.QueryAsync<(string ContentId, string Tag)>(
            new CommandDefinition(tagsSql, new { slugs }, cancellationToken: ct));

        var tagLookup = tags.ToLookup(t => t.ContentId, t => t.Tag);

        // Load sections
        const string sectionsSql = @"
            SELECT slug AS ContentId, section_name AS SectionName
            FROM content_sections
            WHERE slug IN @slugs
            ORDER BY section_name";

        var sections = await Connection.QueryAsync<(string ContentId, string SectionName)>(
            new CommandDefinition(sectionsSql, new { slugs }, cancellationToken: ct));

        var sectionLookup = sections.ToLookup(s => s.ContentId, s => s.SectionName);

        // Load plans
        const string plansSql = @"
            SELECT slug AS ContentId, plan_name AS PlanName
            FROM content_plans
            WHERE slug IN @slugs
            ORDER BY plan_name";

        var plans = await Connection.QueryAsync<(string ContentId, string PlanName)>(
            new CommandDefinition(plansSql, new { slugs }, cancellationToken: ct));

        var planLookup = plans.ToLookup(p => p.ContentId, p => p.PlanName);

        // Create new instances with related data and computed Url
        var result = new List<ContentItem>();
        foreach (var item in items)
        {
            // Compute Url from item properties: /{primarySectionName}/{collectionName}/{slug}
            var url = $"/{item.PrimarySectionName}/{item.CollectionName}/{item.Slug}".ToLowerInvariant();

            // GhcFeature is derived from subcollection or collection name being "ghc-features"
            var ghcFeature = string.Equals(item.SubcollectionName, "ghc-features", StringComparison.OrdinalIgnoreCase)
                          || string.Equals(item.CollectionName, "ghc-features", StringComparison.OrdinalIgnoreCase);

            result.Add(item with
            {
                Tags = tagLookup[item.Slug].ToList(),
                SectionNames = sectionLookup[item.Slug].ToList(),
                Plans = planLookup[item.Slug].ToList(),
                Url = url,
                GhcFeature = ghcFeature
            });
        }

        return result;
    }
}
