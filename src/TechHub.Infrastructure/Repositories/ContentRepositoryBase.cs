using Microsoft.Extensions.Caching.Memory;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Abstract base class for content repositories.
/// Provides shared caching logic and markdown rendering for all repository implementations.
/// Derived classes implement data access through abstract internal methods.
/// </summary>
public abstract class ContentRepositoryBase : IContentRepository
{
    protected readonly IMemoryCache Cache;
    protected readonly IMarkdownService MarkdownService;

    protected ContentRepositoryBase(IMemoryCache cache, IMarkdownService markdownService)
    {
        Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        MarkdownService = markdownService ?? throw new ArgumentNullException(nameof(markdownService));
    }

    /// <summary>
    /// Initialize repository. Override in derived classes for pre-loading if needed.
    /// </summary>
    public virtual Task<IReadOnlyList<ContentItem>> InitializeAsync(CancellationToken ct = default)
    {
        // Default: no preloading - cache is populated on first query
        return Task.FromResult<IReadOnlyList<ContentItem>>([]);
    }

    // ==================== Markdown Rendering ====================

    /// <summary>
    /// Renders the raw markdown content to HTML if Content is present and RenderedHtml is not already set.
    /// After rendering, clears the raw Content to save memory since it's no longer needed.
    /// All repository implementations should return ContentItem with Content populated,
    /// and this method handles the rendering uniformly.
    /// </summary>
    protected ContentItem RenderHtmlIfNeeded(ContentItem item)
    {
        // If RenderedHtml is already set, just clear Content to save memory
        if (item.RenderedHtml != null)
        {
            // Use constructor to create new instance with Content cleared
            return new ContentItem(
                item.Slug, item.Title, item.Author, item.DateEpoch, item.CollectionName,
                item.FeedName, item.PrimarySectionName, item.Tags, item.Excerpt,
                item.ExternalUrl, item.Draft, item.SubcollectionName, item.Plans,
                item.GhesSupport, null, item.RenderedHtml
            );
        }

        // If no raw content to render, return as-is
        if (string.IsNullOrEmpty(item.Content))
        {
            return item;
        }

        // Render the markdown to HTML
        var processedMarkdown = MarkdownService.ProcessYouTubeEmbeds(item.Content);
        var renderedHtml = MarkdownService.RenderToHtml(processedMarkdown, item.GetHref());

        // Return with rendered HTML and clear Content to save memory
        return new ContentItem(
            item.Slug, item.Title, item.Author, item.DateEpoch, item.CollectionName,
            item.FeedName, item.PrimarySectionName, item.Tags, item.Excerpt,
            item.ExternalUrl, item.Draft, item.SubcollectionName, item.Plans,
            item.GhesSupport, null, renderedHtml
        );
    }

    // ==================== Public Methods with Caching ====================

    /// <summary>
    /// Get a single content item by slug and collection.
    /// Results are cached in memory. Renders markdown to HTML if needed.
    /// </summary>
    public async Task<ContentItem?> GetBySlugAsync(
        string collectionName,
        string slug,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        var cacheKey = $"slug:{collectionName}:{slug}:{includeDraft}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            var item = await GetBySlugInternalAsync(collectionName, slug, includeDraft, ct);
            return item != null ? RenderHtmlIfNeeded(item) : null;
        });
    }

    /// <summary>
    /// Get all content items across all collections.
    /// Results are cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetAllAsync(
        bool includeDraft = false,
        int limit = 20,
        int offset = 0,
        CancellationToken ct = default)
    {
        var cacheKey = $"all:{includeDraft}:{limit}:{offset}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.Normal);
            return await GetAllInternalAsync(includeDraft, limit, offset, ct);
        }) ?? [];
    }

    /// <summary>
    /// Get all content items in a specific collection.
    /// Results are cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        string? subcollectionName = null,
        bool includeDraft = false,
        int limit = 20,
        int offset = 0,
        CancellationToken ct = default)
    {
        var cacheKey = $"collection:{collectionName}:{subcollectionName ?? "all"}:{includeDraft}:{limit}:{offset}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.Normal);
            return await GetByCollectionInternalAsync(collectionName, subcollectionName, includeDraft, limit, offset, ct);
        }) ?? [];
    }

    /// <summary>
    /// Get all content items in a specific section.
    /// Results are cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
        bool includeDraft = false,
        int limit = 20,
        int offset = 0,
        CancellationToken ct = default)
    {
        var cacheKey = $"section:{sectionName}:{includeDraft}:{limit}:{offset}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.Normal);
            return await GetBySectionInternalAsync(sectionName, includeDraft, limit, offset, ct);
        }) ?? [];
    }

    /// <summary>
    /// Full-text search with filtering.
    /// Results are cached in memory based on search parameters.
    /// </summary>
    public async Task<SearchResults<ContentItem>> SearchAsync(SearchRequest request, CancellationToken ct = default)
    {
        var cacheKey = BuildSearchCacheKey(request);
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await SearchInternalAsync(request, ct);
        }) ?? new SearchResults<ContentItem>
        {
            Items = [],
            TotalCount = 0,
            Facets = new FacetResults { Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(), TotalCount = 0 }
        };
    }

    /// <summary>
    /// Get facet counts for tags, collections, and sections.
    /// Results are cached in memory based on facet request parameters.
    /// </summary>
    public async Task<FacetResults> GetFacetsAsync(FacetRequest request, CancellationToken ct = default)
    {
        var cacheKey = BuildFacetCacheKey(request);
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
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
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetRelatedInternalAsync(articleId, count, ct);
        }) ?? [];
    }

    /// <summary>
    /// Get tag counts with optional filtering by date range, section, and collection.
    /// Uses efficient database GROUP BY instead of loading all items.
    /// </summary>
    public async Task<IReadOnlyList<TagWithCount>> GetTagCountsAsync(
        DateTimeOffset? dateFrom = null,
        DateTimeOffset? dateTo = null,
        string? sectionName = null,
        string? collectionName = null,
        int? maxTags = null,
        int minUses = 1,
        CancellationToken ct = default)
    {
        var cacheKey = BuildTagCountsCacheKey(dateFrom, dateTo, sectionName, collectionName, maxTags, minUses);
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetTagCountsInternalAsync(dateFrom, dateTo, sectionName, collectionName, maxTags, minUses, ct);
        }) ?? [];
    }

    // ==================== Abstract Internal Methods ====================
    // Derived classes implement these to provide data access logic

    /// <summary>
    /// Internal implementation for getting content by slug.
    /// </summary>
    protected abstract Task<ContentItem?> GetBySlugInternalAsync(
        string collectionName,
        string slug,
        bool includeDraft,
        CancellationToken ct);

    /// <summary>
    /// Internal implementation for getting all content.
    /// </summary>
    protected abstract Task<IReadOnlyList<ContentItem>> GetAllInternalAsync(
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct);

    /// <summary>
    /// Internal implementation for getting content by collection.
    /// Optionally filters by subcollection.
    /// </summary>
    protected abstract Task<IReadOnlyList<ContentItem>> GetByCollectionInternalAsync(
        string collectionName,
        string? subcollectionName,
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct);

    /// <summary>
    /// Internal implementation for getting content by section.
    /// </summary>
    protected abstract Task<IReadOnlyList<ContentItem>> GetBySectionInternalAsync(
        string sectionName,
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct);

    /// <summary>
    /// Internal implementation for full-text search.
    /// </summary>
    protected abstract Task<SearchResults<ContentItem>> SearchInternalAsync(
        SearchRequest request,
        CancellationToken ct);

    /// <summary>
    /// Internal implementation for getting facets.
    /// </summary>
    protected abstract Task<FacetResults> GetFacetsInternalAsync(
        FacetRequest request,
        CancellationToken ct);

    /// <summary>
    /// Internal implementation for getting related articles.
    /// </summary>
    protected abstract Task<IReadOnlyList<ContentItem>> GetRelatedInternalAsync(
        string articleId,
        int count,
        CancellationToken ct);

    /// <summary>
    /// Internal implementation for getting tag counts with GROUP BY.
    /// </summary>
    protected abstract Task<IReadOnlyList<TagWithCount>> GetTagCountsInternalAsync(
        DateTimeOffset? dateFrom,
        DateTimeOffset? dateTo,
        string? sectionName,
        string? collectionName,
        int? maxTags,
        int minUses,
        CancellationToken ct);

    /// <summary>
    /// Hydrate relationship properties (tags, sections, plans) for content items.
    /// For database repositories, loads from normalized relationship tables.
    /// For file-based repositories, items are already hydrated from frontmatter.
    /// </summary>
    protected abstract Task<List<ContentItem>> HydrateRelationshipsAsync(
        IList<ContentItem> items,
        CancellationToken ct = default);

    // ==================== Cache Key Builders ====================

    /// <summary>
    /// Build cache key for search requests based on all query parameters.
    /// </summary>
    protected static string BuildSearchCacheKey(SearchRequest request)
    {
        var parts = new List<string> { "search" };

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            parts.Add($"q:{request.Query}");
        }

        if (request.Tags?.Count > 0)
        {
            parts.Add($"t:{string.Join(",", request.Tags.OrderBy(x => x))}");
        }

        if (request.Sections?.Count > 0)
        {
            parts.Add($"s:{string.Join(",", request.Sections.OrderBy(x => x))}");
        }

        if (request.Collections?.Count > 0)
        {
            parts.Add($"c:{string.Join(",", request.Collections.OrderBy(x => x))}");
        }

        if (request.DateFrom.HasValue)
        {
            parts.Add($"df:{request.DateFrom.Value.ToUnixTimeSeconds()}");
        }

        if (request.DateTo.HasValue)
        {
            parts.Add($"dt:{request.DateTo.Value.ToUnixTimeSeconds()}");
        }

        parts.Add($"take:{request.Take}");

        if (!string.IsNullOrWhiteSpace(request.ContinuationToken))
        {
            parts.Add($"ct:{request.ContinuationToken}");
        }

        return string.Join("|", parts);
    }

    /// <summary>
    /// Build cache key for facet requests based on all filter parameters.
    /// </summary>
    protected static string BuildFacetCacheKey(FacetRequest request)
    {
        var parts = new List<string> { "facets" };

        if (request.Tags?.Count > 0)
        {
            parts.Add($"t:{string.Join(",", request.Tags.OrderBy(x => x))}");
        }

        if (request.Sections?.Count > 0)
        {
            parts.Add($"s:{string.Join(",", request.Sections.OrderBy(x => x))}");
        }

        if (request.Collections?.Count > 0)
        {
            parts.Add($"c:{string.Join(",", request.Collections.OrderBy(x => x))}");
        }

        if (request.DateFrom.HasValue)
        {
            parts.Add($"df:{request.DateFrom.Value.ToUnixTimeSeconds()}");
        }

        if (request.DateTo.HasValue)
        {
            parts.Add($"dt:{request.DateTo.Value.ToUnixTimeSeconds()}");
        }

        parts.Add($"fields:{string.Join(",", request.FacetFields.OrderBy(x => x))}");
        parts.Add($"max:{request.MaxFacetValues}");

        return string.Join("|", parts);
    }

    /// <summary>
    /// Build cache key for tag counts requests.
    /// </summary>
    protected static string BuildTagCountsCacheKey(
        DateTimeOffset? dateFrom,
        DateTimeOffset? dateTo,
        string? sectionName,
        string? collectionName,
        int? maxTags,
        int minUses)
    {
        var parts = new List<string> { "tagcounts" };

        if (dateFrom.HasValue)
        {
            parts.Add($"df:{dateFrom.Value.ToUnixTimeSeconds()}");
        }

        if (dateTo.HasValue)
        {
            parts.Add($"dt:{dateTo.Value.ToUnixTimeSeconds()}");
        }

        if (!string.IsNullOrWhiteSpace(sectionName))
        {
            parts.Add($"s:{sectionName}");
        }

        if (!string.IsNullOrWhiteSpace(collectionName))
        {
            parts.Add($"c:{collectionName}");
        }

        if (maxTags.HasValue)
        {
            parts.Add($"max:{maxTags.Value}");
        }

        parts.Add($"min:{minUses}");

        return string.Join("|", parts);
    }
}
