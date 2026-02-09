using System.Collections.ObjectModel;
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
    /// Initialize sections from configuration.
    /// Converts configuration to Section models and applies ordering.
    /// </summary>
    private static ReadOnlyCollection<Section> InitializeSections(AppSettings settings)
    {
        // Define section display order (matches live site - starts with "all")
        var sectionOrder = new[]
        {
            "all", "github-copilot", "ai", "ml", "devops", "azure", "dotnet", "security"
        };

        // Convert configuration to Section models
        var sectionsDict = settings.Content.Sections
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value))
            .ToDictionary(s => s.Name);

        // Order sections according to defined order, then any remaining alphabetically
        return sectionOrder
            .Where(name => sectionsDict.ContainsKey(name))
            .Select(name => sectionsDict[name])
            .Concat(sectionsDict.Values.Where(s => !sectionOrder.Contains(s.Name)).OrderBy(s => s.Title))
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Convert SectionConfig from appsettings.json to Section model.
    /// </summary>
    private static Section ConvertToSection(string sectionName, SectionConfig config)
    {
        var collections = config.Collections
            .Select(kvp =>
            {
                // Use GetTagFromName for display name (e.g., "blogs" -> "Blogs", "vscode-updates" -> "Vscode Updates")
                var displayName = Collection.GetTagFromName(kvp.Key);
                return new Collection(
                    kvp.Key,
                    kvp.Value.Title,
                    kvp.Value.Url,
                    kvp.Value.Description,
                    displayName,
                    kvp.Value.Custom,
                    kvp.Value.Order);
            })
            .ToList();

        return new Section(sectionName, config.Title, config.Description, config.Url, config.Tag, collections);
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
    /// Search content with filters, facets, and pagination.
    /// Results are cached in memory based on search parameters.
    /// </summary>
    public async Task<SearchResults<ContentItem>> SearchAsync(SearchRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = request.GetCacheKey();
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

        var cacheKey = request.GetCacheKey();
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetFacetsInternalAsync(request, ct);
        }) ?? new FacetResults { Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(), TotalCount = 0 };
    }

    /// <summary>
    /// Get tag counts with optional filtering.
    /// Returns top N tags (sorted by count descending) above minUses threshold.
    /// Results are cached - very fast for repeated calls with same filters.
    /// </summary>
    public async Task<IReadOnlyList<TagWithCount>> GetTagCountsAsync(
        TagCountsRequest request,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = request.GetCacheKey();
        return await Cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await GetTagCountsInternalAsync(request, ct);
        }) ?? [];
    }

    // ==================== Protected Helper Methods ====================

    // High-frequency tags excluded from all tag clouds.
    // These appear on most content items and don't provide filtering value.
    private static readonly string[] HighFrequencyExcludedTags = ["github", "copilot", "microsoft"];

    /// <summary>
    /// Get the cached set of tags to exclude from tag clouds.
    /// Cached for the lifetime of the application since it's based on static configuration.
    /// Returns a case-insensitive HashSet for efficient contains checks and SQL filtering.
    /// Includes: section tags, collection tags, and high-frequency terms (github, copilot, microsoft).
    /// </summary>
    protected async Task<HashSet<string>> GetExcludeTagsSetAsync()
    {
        return await Cache.GetOrCreateAsync("excludetags:set", async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return await BuildExcludeTagsSetAsync();
        }) ?? [];
    }

    /// <summary>
    /// Build a set of section and collection tags to exclude from tag clouds.
    /// Uses section Tags from configuration and programmatically generated collection tags.
    /// These are structural tags added by ContentFixer for search purposes,
    /// but shouldn't appear in tag clouds as they're redundant (users already filter by section/collection).
    /// </summary>
    private async Task<HashSet<string>> BuildExcludeTagsSetAsync()
    {
        var excludeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var sections = await GetAllSectionsAsync();

        foreach (var section in sections)
        {
            // Add section tag from configuration (e.g., "AI" for ai section, "All" for all section)
            if (!string.IsNullOrWhiteSpace(section.Tag))
            {
                excludeSet.Add(section.Tag);
            }

            // Add collection tags generated from collection names
            foreach (var collection in section.Collections)
            {
                // Generate tag from collection name (e.g., "blogs" -> "Blogs", "community" -> "Community")
                var collectionTag = Collection.GetTagFromName(collection.Name);
                if (!string.IsNullOrWhiteSpace(collectionTag))
                {
                    excludeSet.Add(collectionTag);
                }
            }
        }

        // Add high-frequency tags (already defined in HighFrequencyExcludedTags array)
        foreach (var tag in HighFrequencyExcludedTags)
        {
            excludeSet.Add(tag);
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
        TagCountsRequest request,
        CancellationToken ct);

    // ==================== Section Methods ====================

    /// <summary>
    /// Get all sections defined in configuration.
    /// Sections are loaded lazily and cached in memory.
    /// </summary>
    public async Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default)
    {
        return await Cache.GetOrCreateAsync("sections:all", entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            return Task.FromResult(InitializeSections(_settings));
        }) ?? [];
    }

    /// <summary>
    /// Get a single section by name.
    /// Sections are loaded lazily and cached in memory.
    /// </summary>
    public async Task<Section?> GetSectionByNameAsync(string name, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        var sections = await GetAllSectionsAsync(ct);
        return sections.FirstOrDefault(s => s.Name == name);
    }
}
