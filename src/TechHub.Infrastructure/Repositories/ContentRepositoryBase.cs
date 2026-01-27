using System.Data;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Base class for database-backed content repositories.
/// Provides shared Dapper-based database operations with memory caching for both SQLite and PostgreSQL.
/// </summary>
public abstract class ContentRepositoryBase : IContentRepository
{
    protected readonly IDbConnection _connection;
    protected readonly ISqlDialect _dialect;
    protected readonly IMemoryCache _cache;

    protected ContentRepositoryBase(IDbConnection connection, ISqlDialect dialect, IMemoryCache cache)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _dialect = dialect ?? throw new ArgumentNullException(nameof(dialect));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <summary>
    /// Initialize repository - no-op for database-backed repos since caching is on-demand.
    /// </summary>
    public virtual Task<IReadOnlyList<ContentItem>> InitializeAsync(CancellationToken ct = default)
    {
        // No preloading - cache is populated on first query
        return Task.FromResult<IReadOnlyList<ContentItem>>([]);
    }

    /// <summary>
    /// Get a single content item by slug and collection.
    /// Results are cached in memory.
    /// </summary>
    public async Task<ContentItem?> GetBySlugAsync(
        string collectionName,
        string slug,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        var cacheKey = $"slug:{collectionName}:{slug}:{includeDraft}";
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetBySlugInternalAsync(collectionName, slug, includeDraft, ct);
        });
    }

    /// <summary>
    /// Internal implementation for getting content by slug.
    /// </summary>
    protected virtual async Task<ContentItem?> GetBySlugInternalAsync(
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

        var item = await _connection.QuerySingleOrDefaultAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, slug, includeDraft }, cancellationToken: ct));

        if (item != null)
        {
            var itemsWithRelated = await LoadRelatedDataAsync([item], ct);
            return itemsWithRelated.FirstOrDefault();
        }

        return null;
    }

    /// <summary>
    /// Get all content items across all collections.
    /// Results are cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetAllAsync(
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        var cacheKey = $"all:{includeDraft}";
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetAllInternalAsync(includeDraft, ct);
        }) ?? [];
    }

    /// <summary>
    /// Internal implementation for getting all content.
    /// </summary>
    protected virtual async Task<IReadOnlyList<ContentItem>> GetAllInternalAsync(
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
            WHERE c.draft = 0 OR @includeDraft = 1
            ORDER BY c.date_epoch DESC";

        var items = await _connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { includeDraft }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await LoadRelatedDataAsync(itemsList, ct);
    }

    /// <summary>
    /// Get all content items in a specific collection.
    /// For "videos", returns all video subcollections (ghc-features, vscode-updates).
    /// Results are cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        var cacheKey = $"collection:{collectionName}:{includeDraft}";
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetByCollectionInternalAsync(collectionName, includeDraft, ct);
        }) ?? [];
    }

    /// <summary>
    /// Internal implementation for getting content by collection.
    /// </summary>
    protected virtual async Task<IReadOnlyList<ContentItem>> GetByCollectionInternalAsync(
        string collectionName,
        bool includeDraft,
        CancellationToken ct)
    {
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
                c.primary_section_name AS PrimarySectionName,
                c.feed_name AS FeedName,
                c.external_url AS ExternalUrl,
                c.author AS Author,
                c.ghes_support AS GhesSupport,
                c.draft AS Draft
            FROM content_items c
            WHERE {whereClause}
              AND (c.draft = 0 OR @includeDraft = 1)
            ORDER BY c.date_epoch DESC";

        var items = await _connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { collectionName, includeDraft }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await LoadRelatedDataAsync(itemsList, ct);
    }

    /// <summary>
    /// Get all content items in a specific section.
    /// Results are cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        var cacheKey = $"section:{sectionName}:{includeDraft}";
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetBySectionInternalAsync(sectionName, includeDraft, ct);
        }) ?? [];
    }

    /// <summary>
    /// Internal implementation for getting content by section.
    /// </summary>
    protected virtual async Task<IReadOnlyList<ContentItem>> GetBySectionInternalAsync(
        string sectionName,
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
            INNER JOIN content_sections cs ON c.slug = cs.slug
            WHERE cs.section_name = @sectionName
              AND (c.draft = 0 OR @includeDraft = 1)
            ORDER BY c.date_epoch DESC";

        var items = await _connection.QueryAsync<ContentItem>(
            new CommandDefinition(sql, new { sectionName, includeDraft }, cancellationToken: ct));

        var itemsList = items.ToList();
        return await LoadRelatedDataAsync(itemsList, ct);
    }

    /// <summary>
    /// Load tags, sections, and plans for content items.
    /// Uses efficient multi-query approach to minimize database round-trips.
    /// Returns new ContentItem instances with related data populated.
    /// </summary>
    protected virtual async Task<List<ContentItem>> LoadRelatedDataAsync(IList<ContentItem> items, CancellationToken ct = default)
    {
        if (items.Count == 0) return new List<ContentItem>();

        var slugs = items.Select(i => i.Slug).ToList();

        // Load tags
        const string tagsSql = @"
            SELECT slug AS ContentId, tag AS Tag
            FROM content_tags
            WHERE slug IN @slugs
            ORDER BY tag";

        var tags = await _connection.QueryAsync<(string ContentId, string Tag)>(
            new CommandDefinition(tagsSql, new { slugs }, cancellationToken: ct));

        var tagLookup = tags.ToLookup(t => t.ContentId, t => t.Tag);

        // Load sections
        const string sectionsSql = @"
            SELECT slug AS ContentId, section_name AS SectionName
            FROM content_sections
            WHERE slug IN @slugs
            ORDER BY section_name";

        var sections = await _connection.QueryAsync<(string ContentId, string SectionName)>(
            new CommandDefinition(sectionsSql, new { slugs }, cancellationToken: ct));

        var sectionLookup = sections.ToLookup(s => s.ContentId, s => s.SectionName);

        // Load plans
        const string plansSql = @"
            SELECT slug AS ContentId, plan_name AS PlanName
            FROM content_plans
            WHERE slug IN @slugs
            ORDER BY plan_name";

        var plans = await _connection.QueryAsync<(string ContentId, string PlanName)>(
            new CommandDefinition(plansSql, new { slugs }, cancellationToken: ct));

        var planLookup = plans.ToLookup(p => p.ContentId, p => p.PlanName);

        // Create new instances with related data
        var result = new List<ContentItem>();
        foreach (var item in items)
        {
            result.Add(item with
            {
                Tags = tagLookup[item.Slug].ToList(),
                SectionNames = sectionLookup[item.Slug].ToList(),
                Plans = planLookup[item.Slug].ToList()
            });
        }
        return result;
    }

    // ==================== Abstract Methods (Database-Specific Implementation) ====================

    /// <summary>
    /// Full-text search with filtering.
    /// Results are cached in memory based on search parameters.
    /// </summary>
    public async Task<SearchResults<ContentItem>> SearchAsync(SearchRequest request, CancellationToken ct = default)
    {
        var cacheKey = BuildSearchCacheKey(request);
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await SearchInternalAsync(request, ct);
        }) ?? new SearchResults<ContentItem> { Items = [], TotalCount = 0, Facets = new FacetResults { Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(), TotalCount = 0 } };
    }

    /// <summary>
    /// Get facet counts for tags, collections, and sections.
    /// Results are cached in memory based on facet request parameters.
    /// </summary>
    public async Task<FacetResults> GetFacetsAsync(FacetRequest request, CancellationToken ct = default)
    {
        var cacheKey = BuildFacetCacheKey(request);
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetFacetsInternalAsync(request, ct);
        }) ?? new FacetResults { Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(), TotalCount = 0 };
    }

    /// <summary>
    /// Get related articles.
    /// Results are cached in memory per article.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetRelatedAsync(string articleId, int count = 5, CancellationToken ct = default)
    {
        var cacheKey = $"related:{articleId}:{count}";
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetRelatedInternalAsync(articleId, count, ct);
        }) ?? [];
    }

    // Protected abstract methods for database-specific implementations
    protected abstract Task<SearchResults<ContentItem>> SearchInternalAsync(SearchRequest request, CancellationToken ct);
    protected abstract Task<FacetResults> GetFacetsInternalAsync(FacetRequest request, CancellationToken ct);
    protected abstract Task<IReadOnlyList<ContentItem>> GetRelatedInternalAsync(string articleId, int count, CancellationToken ct);

    // ==================== Cache Key Builders ====================

    /// <summary>
    /// Build cache key for search requests based on all query parameters.
    /// </summary>
    protected static string BuildSearchCacheKey(SearchRequest request)
    {
        var parts = new List<string> { "search" };
        
        if (!string.IsNullOrWhiteSpace(request.Query))
            parts.Add($"q:{request.Query}");
        
        if (request.Tags?.Count > 0)
            parts.Add($"t:{string.Join(",", request.Tags.OrderBy(x => x))}");
        
        if (request.Sections?.Count > 0)
            parts.Add($"s:{string.Join(",", request.Sections.OrderBy(x => x))}");
        
        if (request.Collections?.Count > 0)
            parts.Add($"c:{string.Join(",", request.Collections.OrderBy(x => x))}");
        
        if (request.DateFrom.HasValue)
            parts.Add($"df:{request.DateFrom.Value.ToUnixTimeSeconds()}");
        
        if (request.DateTo.HasValue)
            parts.Add($"dt:{request.DateTo.Value.ToUnixTimeSeconds()}");
        
        parts.Add($"take:{request.Take}");
        
        if (!string.IsNullOrWhiteSpace(request.ContinuationToken))
            parts.Add($"ct:{request.ContinuationToken}");
        
        return string.Join("|", parts);
    }

    /// <summary>
    /// Build cache key for facet requests based on all filter parameters.
    /// </summary>
    protected static string BuildFacetCacheKey(FacetRequest request)
    {
        var parts = new List<string> { "facets" };
        
        if (request.Tags?.Count > 0)
            parts.Add($"t:{string.Join(",", request.Tags.OrderBy(x => x))}");
        
        if (request.Sections?.Count > 0)
            parts.Add($"s:{string.Join(",", request.Sections.OrderBy(x => x))}");
        
        if (request.Collections?.Count > 0)
            parts.Add($"c:{string.Join(",", request.Collections.OrderBy(x => x))}");
        
        if (request.DateFrom.HasValue)
            parts.Add($"df:{request.DateFrom.Value.ToUnixTimeSeconds()}");
        
        if (request.DateTo.HasValue)
            parts.Add($"dt:{request.DateTo.Value.ToUnixTimeSeconds()}");
        
        parts.Add($"fields:{string.Join(",", request.FacetFields.OrderBy(x => x))}");
        parts.Add($"max:{request.MaxFacetValues}");
        
        return string.Join("|", parts);
    }
}
