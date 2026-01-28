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
                c.primary_section_name AS PrimarySectionName,
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
        int limit,
        int offset,
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
                c.primary_section_name AS PrimarySectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft
            FROM content_items c
            WHERE c.draft = 0 OR @includeDraft = 1
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { includeDraft, limit, offset }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await HydrateRelationshipsAsync(itemsList, ct);
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
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, subcollectionName, includeDraft, limit, offset }, cancellationToken: ct));

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
        int limit,
        int offset,
        CancellationToken ct)
    {
        // Handle virtual "all" section - return all items
        if (sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllInternalAsync(includeDraft, limit, offset, ct);
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
                c.primary_section_name AS PrimarySectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft
            FROM content_items c
            INNER JOIN content_sections cs ON c.collection_name = cs.collection_name AND c.slug = cs.slug
            WHERE cs.section_name = @sectionName
              AND (c.draft = 0 OR @includeDraft = 1)
            ORDER BY c.date_epoch DESC
            LIMIT @limit OFFSET @offset";

        var items = await Connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { sectionName, includeDraft, limit, offset }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await HydrateRelationshipsAsync(itemsList, ct);
    }

    /// <summary>
    /// Hydrate relationship properties (tags, plans) from normalized database tables.
    /// Uses efficient multi-query approach to minimize database round-trips.
    /// For large result sets (>900 items), chunks queries to avoid SQLite's 999 parameter limit.
    /// </summary>
    protected override async Task<List<ContentItem>> HydrateRelationshipsAsync(IList<ContentItem> items, CancellationToken ct = default)
    {
        if (items.Count == 0)
        {
            return [];
        }

        // SQLite has a 999 parameter limit. We use chunks of 900 to be safe.
        const int chunkSize = 900;
        
        ILookup<string, string> tagLookup;
        ILookup<string, string> planLookup;

        if (items.Count <= chunkSize)
        {
            // Small batch: single query with IN clause
            var slugs = items.Select(i => i.Slug).ToList();

            // Load tags
            const string tagsSql = @"
                SELECT slug AS ContentId, tag AS Tag
                FROM content_tags
                WHERE slug IN @slugs
                ORDER BY tag";

            var tags = await Connection.QueryAsync<(string ContentId, string Tag)>(
                new CommandDefinition(tagsSql, new { slugs }, cancellationToken: ct));

            tagLookup = tags.ToLookup(t => t.ContentId, t => t.Tag);

            // Load plans
            const string plansSql = @"
                SELECT slug AS ContentId, plan_name AS PlanName
                FROM content_plans
                WHERE slug IN @slugs
                ORDER BY plan_name";

            var plans = await Connection.QueryAsync<(string ContentId, string PlanName)>(
                new CommandDefinition(plansSql, new { slugs }, cancellationToken: ct));

            planLookup = plans.ToLookup(p => p.ContentId, p => p.PlanName);
        }
        else
        {
            // Large batch: chunk into multiple queries to avoid parameter limit
            var allTags = new List<(string ContentId, string Tag)>();
            var allPlans = new List<(string ContentId, string PlanName)>();

            for (int i = 0; i < items.Count; i += chunkSize)
            {
                var chunk = items.Skip(i).Take(chunkSize).Select(item => item.Slug).ToList();

                // Load tags for this chunk
                const string tagsSql = @"
                    SELECT slug AS ContentId, tag AS Tag
                    FROM content_tags
                    WHERE slug IN @slugs
                    ORDER BY tag";

                var tags = await Connection.QueryAsync<(string ContentId, string Tag)>(
                    new CommandDefinition(tagsSql, new { slugs = chunk }, cancellationToken: ct));
                allTags.AddRange(tags);

                // Load plans for this chunk
                const string plansSql = @"
                    SELECT slug AS ContentId, plan_name AS PlanName
                    FROM content_plans
                    WHERE slug IN @slugs
                    ORDER BY plan_name";

                var plans = await Connection.QueryAsync<(string ContentId, string PlanName)>(
                    new CommandDefinition(plansSql, new { slugs = chunk }, cancellationToken: ct));
                allPlans.AddRange(plans);
            }

            tagLookup = allTags.ToLookup(t => t.ContentId, t => t.Tag);
            planLookup = allPlans.ToLookup(p => p.ContentId, p => p.PlanName);
        }

        // Create new instances with related data
        var result = new List<ContentItem>();
        foreach (var item in items)
        {
            var tags = tagLookup[item.Slug].ToList();
            var plans = planLookup[item.Slug].ToList();
            result.Add(new ContentItem(
                item.Slug, item.Title, item.Author, item.DateEpoch, item.CollectionName,
                item.FeedName, item.PrimarySectionName, tags, item.Excerpt,
                item.ExternalUrl, item.Draft, item.SubcollectionName, plans,
                item.GhesSupport, item.Content, item.RenderedHtml
            ));
        }

        return result;
    }
}
