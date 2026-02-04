using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
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
    protected IMemoryCache Cache { get; }
    protected IMarkdownService MarkdownService { get; }
    private readonly AppSettings _settings;

    protected ContentRepositoryBase(
        IMemoryCache cache,
        IMarkdownService markdownService,
        IOptions<AppSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(settings);

        Cache = cache;
        MarkdownService = markdownService;
        _settings = settings.Value;
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
    /// </summary>
    protected ContentItemDetail RenderHtmlIfNeeded(ContentItemDetail item)
    {
        ArgumentNullException.ThrowIfNull(item);

        // If RenderedHtml is already set, return as-is (Content already nulled)
        if (item.RenderedHtml != null)
        {
            return item;
        }

        // If no raw content to render, something is wrong - item should have either RenderedHtml or Content
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new InvalidOperationException(
                $"ContentItemDetail '{item.Slug}' in collection '{item.CollectionName}' has no Content to render and no RenderedHtml. " +
                "Items must have either pre-rendered HTML or raw markdown content.");
        }

        // Render the markdown to HTML
        var processedMarkdown = MarkdownService.ProcessYouTubeEmbeds(item.Content);
        var renderedHtml = MarkdownService.RenderToHtml(processedMarkdown);

        // Set rendered HTML (this also clears Content to save memory)
        item.SetRenderedHtml(renderedHtml);
        return item;
    }

    // ==================== Public Methods with Caching ====================

    /// <summary>
    /// Get a single content item by slug and collection.
    /// Results are cached in memory. Renders markdown to HTML if needed.
    /// Returns ContentItemDetail which includes the full markdown content and rendered HTML.
    /// </summary>
    public async Task<ContentItemDetail?> GetBySlugAsync(
        string collectionName,
        string slug,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        ArgumentNullException.ThrowIfNull(slug);

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
        int limit,
        int offset = 0,
        bool includeDraft = false,
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
        int limit,
        string? subcollectionName = null,
        int offset = 0,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(collectionName);

        var cacheKey = $"collection:{collectionName}:{subcollectionName ?? "all"}:{includeDraft}:{limit}:{offset}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.Normal);
            return await GetByCollectionInternalAsync(collectionName, subcollectionName, includeDraft, limit, offset, ct);
        }) ?? [];
    }

    /// <summary>
    /// Get all content items in a specific section.
    /// Optionally filter by collection and subcollection.
    /// Results are cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
        int limit,
        int offset = 0,
        string? collectionName = null,
        string? subcollectionName = null,
        bool includeDraft = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sectionName);

        var cacheKey = $"section:{sectionName}:{collectionName}:{subcollectionName}:{includeDraft}:{limit}:{offset}";
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.Normal);
            return await GetBySectionInternalAsync(sectionName, collectionName, subcollectionName, includeDraft, limit, offset, ct);
        }) ?? [];
    }

    /// <summary>
    /// Full-text search with filtering.
    /// Results are cached in memory based on search parameters.
    /// </summary>
    public async Task<SearchResults<ContentItem>> SearchAsync(SearchRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

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
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = BuildFacetCacheKey(request);
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetFacetsInternalAsync(request, ct);
        }) ?? new FacetResults { Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(), TotalCount = 0 };
    }

    /// <summary>
    /// Get tag counts with optional filtering by date range, section, and collection.
    /// Returns top N tags (sorted by count descending) above minUses threshold.
    /// Results are cached - very fast for repeated calls with same filters.
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

    // ==================== Protected Helper Methods ====================

    /// <summary>
    /// Build a set of section and collection titles to exclude from tag clouds.
    /// Uses cached section configuration data.
    /// </summary>
    protected HashSet<string> BuildSectionCollectionExcludeSet()
    {
        var excludeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var sectionsConfig = _settings.Content.Sections;
        
        if (sectionsConfig != null)
        {
            foreach (var (_, sectionConfig) in sectionsConfig)
            {
                // Add section title
                if (!string.IsNullOrWhiteSpace(sectionConfig.Title))
                {
                    excludeSet.Add(sectionConfig.Title);
                }
                
                // Add collection titles
                if (sectionConfig.Collections != null)
                {
                    foreach (var (_, collectionConfig) in sectionConfig.Collections)
                    {
                        if (!string.IsNullOrWhiteSpace(collectionConfig.Title))
                        {
                            excludeSet.Add(collectionConfig.Title);
                        }
                    }
                }
            }
        }
        
        return excludeSet;
    }

    // ==================== Abstract Internal Methods ====================
    // Derived classes implement these to provide data access logic

    /// <summary>
    /// Internal implementation for getting content by slug.
    /// Returns ContentItemDetail which includes the full markdown content.
    /// </summary>
    protected abstract Task<ContentItemDetail?> GetBySlugInternalAsync(
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
    /// Optionally filter by collection and subcollection.
    /// </summary>
    protected abstract Task<IReadOnlyList<ContentItem>> GetBySectionInternalAsync(
        string sectionName,
        string? collectionName,
        string? subcollectionName,
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

    // ==================== Cache Key Builders ====================

    /// <summary>
    /// Build cache key for search requests based on all query parameters.
    /// </summary>
    protected static string BuildSearchCacheKey(SearchRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Optimization: Use StringBuilder instead of List<string> + string.Join
        // to reduce allocations and improve performance for cache key building
        var sb = new StringBuilder("search");

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            sb.Append("|q:").Append(request.Query);
        }

        if (request.Tags?.Count > 0)
        {
            sb.Append("|t:");
            var sortedTags = (IEnumerable<string>)(request.Tags.Count == 1 ? request.Tags : request.Tags.OrderBy(x => x));
            sb.AppendJoin(',', sortedTags);
        }

        if (request.Sections?.Count > 0)
        {
            sb.Append("|s:");
            var sortedSections = (IEnumerable<string>)(request.Sections.Count == 1 ? request.Sections : request.Sections.OrderBy(x => x));
            sb.AppendJoin(',', sortedSections);
        }

        if (request.Collections?.Count > 0)
        {
            sb.Append("|c:");
            var sortedCollections = (IEnumerable<string>)(request.Collections.Count == 1 ? request.Collections : request.Collections.OrderBy(x => x));
            sb.AppendJoin(',', sortedCollections);
        }

        if (request.DateFrom.HasValue)
        {
            sb.Append("|df:").Append(request.DateFrom.Value.ToUnixTimeSeconds());
        }

        if (request.DateTo.HasValue)
        {
            sb.Append("|dt:").Append(request.DateTo.Value.ToUnixTimeSeconds());
        }

        sb.Append("|take:").Append(request.Take);

        if (!string.IsNullOrWhiteSpace(request.ContinuationToken))
        {
            sb.Append("|ct:").Append(request.ContinuationToken);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Build cache key for facet requests based on all filter parameters.
    /// </summary>
    protected static string BuildFacetCacheKey(FacetRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

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
