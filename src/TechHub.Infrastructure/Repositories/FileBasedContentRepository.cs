using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Repository for loading content from markdown files in collections directories.
/// Reads from: collections/_news/, collections/_videos/, collections/_blogs/, etc.
/// Extends ContentRepositoryBase which handles caching logic.
/// </summary>
public class FileBasedContentRepository : ContentRepositoryBase
{
    private readonly string _basePath;
    private readonly FrontMatterParser _frontMatterParser;

    // Local cache key for all items (file-based repo loads everything at once)
    private const string AllItemsCacheKey = "FileBasedRepository:AllItems";

    private static readonly string[] ValidCollections =
    [
        "_news",
        "_videos",
        "_community",
        "_blogs",
        "_roundups"
    ];

    public FileBasedContentRepository(
        IOptions<AppSettings> settings,
        IMarkdownService markdownService,
        IHostEnvironment environment,
        IMemoryCache cache)
        : base(cache, markdownService)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(environment);

        var configuredPath = settings.Value.Content.CollectionsPath;

        // Resolve relative paths to absolute paths based on content root
        if (Path.IsPathRooted(configuredPath))
        {
            _basePath = configuredPath;
        }
        else
        {
            var solutionRoot = FindSolutionRoot(environment.ContentRootPath);
            _basePath = Path.Combine(solutionRoot, configuredPath);
        }

        // Verify the path exists to prevent silent failures
        if (!Directory.Exists(_basePath))
        {
            throw new DirectoryNotFoundException(
                $"Collections directory not found: {_basePath}. " +
                $"ContentRootPath: {environment.ContentRootPath}, " +
                $"Configured path: {configuredPath}");
        }

        _frontMatterParser = new FrontMatterParser();
    }

    /// <summary>
    /// Find solution root by walking up directory tree looking for TechHub.slnx
    /// </summary>
    private static string FindSolutionRoot(string startPath)
    {
        var directory = new DirectoryInfo(startPath);
        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "TechHub.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return startPath;
    }

    // ==================== Overrides for ContentRepositoryBase ====================

    /// <summary>
    /// Initialize by pre-loading all content from disk.
    /// </summary>
    public override async Task<IReadOnlyList<ContentItem>> InitializeAsync(CancellationToken ct = default)
    {
        return await GetAllInternalAsync(includeDraft: true, ct);
    }

    /// <summary>
    /// Internal implementation: Load all content from markdown files.
    /// Results are cached in the local AllItems cache.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetAllInternalAsync(
        bool includeDraft,
        CancellationToken ct)
    {
        // Use local cache for all items (file-based loads everything at once)
        var allItems = await Cache.GetOrCreateAsync(AllItemsCacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);

            // Load all collections in parallel
            var collectionTasks = ValidCollections
                .Select(collection => LoadCollectionItemsAsync(collection, ct))
                .ToArray();

            var collectionResults = await Task.WhenAll(collectionTasks);
            var items = collectionResults.SelectMany(i => i).ToList();

            // Filter out future-dated content UNLESS it's marked as draft
            var currentEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var cachedItems = items
                .Where(x => x.DateEpoch <= currentEpoch || x.Draft)
                .OrderByDescending(x => x.DateEpoch)
                .ToList();

            return (IReadOnlyList<ContentItem>)cachedItems;
        }) ?? [];

        // Filter drafts if needed
        if (!includeDraft)
        {
            allItems = [.. allItems.Where(item => !item.Draft)];
        }

        return allItems;
    }

    /// <summary>
    /// Internal implementation: Get content by slug from cached data.
    /// </summary>
    protected override async Task<ContentItem?> GetBySlugInternalAsync(
        string collectionName,
        string slug,
        bool includeDraft,
        CancellationToken ct)
    {
        var items = await GetByCollectionInternalAsync(collectionName, includeDraft, ct);
        return items.FirstOrDefault(item => item.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Internal implementation: Get content by collection from cached data.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetByCollectionInternalAsync(
        string collectionName,
        bool includeDraft,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(collectionName);

        var allItems = await GetAllInternalAsync(includeDraft, ct);

        // Handle virtual "all" collection
        if (collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return [.. allItems.OrderByDescending(x => x.DateEpoch)];
        }

        // Normalize collection name
        var normalizedCollection = collectionName.StartsWith('_') ? collectionName : $"_{collectionName}";

        var collectionItems = allItems
            .Where(item => item.CollectionName.Equals(normalizedCollection.TrimStart('_'), StringComparison.OrdinalIgnoreCase));

        return [.. collectionItems.OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Internal implementation: Get content by section from cached data.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetBySectionInternalAsync(
        string sectionName,
        bool includeDraft,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(sectionName);

        var allItems = await GetAllInternalAsync(includeDraft, ct);

        var sectionItems = sectionName.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? allItems
            : allItems.Where(item => item.SectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase));

        return [.. sectionItems.OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// In-memory search implementation for file-based repository.
    /// Filters cached content by query, tags, sections, collections, and date range.
    /// </summary>
    protected override async Task<SearchResults<ContentItem>> SearchInternalAsync(
        SearchRequest request,
        CancellationToken ct)
    {
        var allItems = await GetAllInternalAsync(includeDraft: false, ct);
        IEnumerable<ContentItem> filtered = allItems;

        // Full-text search (naive: check if query appears in title, excerpt, or content)
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            var query = request.Query.ToLowerInvariant();
            filtered = filtered.Where(item =>
                (item.Title?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (item.Excerpt?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (item.Content?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        // Tag filtering (AND logic)
        if (request.Tags is { Count: > 0 })
        {
            foreach (var tag in request.Tags)
            {
                var normalizedTag = tag.ToLowerInvariant().Trim();
                filtered = filtered.Where(item =>
                    item.Tags.Any(t => t.ToLowerInvariant().Contains(normalizedTag)));
            }
        }

        // Section filtering (OR logic)
        if (request.Sections is { Count: > 0 })
        {
            filtered = filtered.Where(item =>
                item.SectionNames.Any(s => request.Sections.Contains(s, StringComparer.OrdinalIgnoreCase)));
        }

        // Collection filtering (OR logic)
        if (request.Collections is { Count: > 0 })
        {
            filtered = filtered.Where(item =>
                request.Collections.Contains(item.CollectionName, StringComparer.OrdinalIgnoreCase));
        }

        // Date range filtering
        if (request.DateFrom.HasValue)
        {
            var fromEpoch = request.DateFrom.Value.ToUnixTimeSeconds();
            filtered = filtered.Where(item => item.DateEpoch >= fromEpoch);
        }

        if (request.DateTo.HasValue)
        {
            var toEpoch = request.DateTo.Value.ToUnixTimeSeconds();
            filtered = filtered.Where(item => item.DateEpoch <= toEpoch);
        }

        // Order by date descending
        var orderedItems = filtered.OrderByDescending(item => item.DateEpoch).ToList();

        // Apply pagination
        var pagedItems = orderedItems.Take(request.Take).ToList();

        return new SearchResults<ContentItem>
        {
            Items = pagedItems,
            TotalCount = orderedItems.Count,
            Facets = new FacetResults
            {
                Facets = new Dictionary<string, IReadOnlyList<FacetValue>>(),
                TotalCount = orderedItems.Count
            },
            ContinuationToken = null
        };
    }

    /// <summary>
    /// In-memory facet computation for file-based repository.
    /// Computes facet counts from cached content.
    /// </summary>
    protected override async Task<FacetResults> GetFacetsInternalAsync(
        FacetRequest request,
        CancellationToken ct)
    {
        var allItems = await GetAllInternalAsync(includeDraft: false, ct);
        IEnumerable<ContentItem> filtered = allItems;

        // Apply filters
        if (request.Tags is { Count: > 0 })
        {
            foreach (var tag in request.Tags)
            {
                var normalizedTag = tag.ToLowerInvariant().Trim();
                filtered = filtered.Where(item =>
                    item.Tags.Any(t => t.ToLowerInvariant().Contains(normalizedTag)));
            }
        }

        if (request.Sections is { Count: > 0 })
        {
            filtered = filtered.Where(item =>
                item.SectionNames.Any(s => request.Sections.Contains(s, StringComparer.OrdinalIgnoreCase)));
        }

        if (request.Collections is { Count: > 0 })
        {
            filtered = filtered.Where(item =>
                request.Collections.Contains(item.CollectionName, StringComparer.OrdinalIgnoreCase));
        }

        if (request.DateFrom.HasValue)
        {
            var fromEpoch = request.DateFrom.Value.ToUnixTimeSeconds();
            filtered = filtered.Where(item => item.DateEpoch >= fromEpoch);
        }

        if (request.DateTo.HasValue)
        {
            var toEpoch = request.DateTo.Value.ToUnixTimeSeconds();
            filtered = filtered.Where(item => item.DateEpoch <= toEpoch);
        }

        var filteredList = filtered.ToList();
        var facets = new Dictionary<string, IReadOnlyList<FacetValue>>();

        // Compute tag facets
        if (request.FacetFields.Contains("tags"))
        {
            var tagCounts = filteredList
                .SelectMany(item => item.Tags)
                .GroupBy(tag => tag, StringComparer.OrdinalIgnoreCase)
                .Select(g => new FacetValue { Value = g.Key, Count = g.Count() })
                .OrderByDescending(f => f.Count)
                .ThenBy(f => f.Value, StringComparer.OrdinalIgnoreCase)
                .Take(request.MaxFacetValues)
                .ToList();

            facets["tags"] = tagCounts;
        }

        // Compute collection facets
        if (request.FacetFields.Contains("collections"))
        {
            var collectionCounts = filteredList
                .GroupBy(item => item.CollectionName, StringComparer.OrdinalIgnoreCase)
                .Select(g => new FacetValue { Value = g.Key, Count = g.Count() })
                .OrderByDescending(f => f.Count)
                .ThenBy(f => f.Value, StringComparer.OrdinalIgnoreCase)
                .ToList();

            facets["collections"] = collectionCounts;
        }

        // Compute section facets
        if (request.FacetFields.Contains("sections"))
        {
            var sectionCounts = filteredList
                .SelectMany(item => item.SectionNames)
                .GroupBy(section => section, StringComparer.OrdinalIgnoreCase)
                .Select(g => new FacetValue { Value = g.Key, Count = g.Count() })
                .OrderByDescending(f => f.Count)
                .ThenBy(f => f.Value, StringComparer.OrdinalIgnoreCase)
                .ToList();

            facets["sections"] = sectionCounts;
        }

        return new FacetResults
        {
            Facets = facets,
            TotalCount = filteredList.Count
        };
    }

    /// <summary>
    /// In-memory related articles computation for file-based repository.
    /// Finds articles with overlapping tags, ranked by shared tag count.
    /// </summary>
    protected override async Task<IReadOnlyList<ContentItem>> GetRelatedInternalAsync(
        string articleId,
        int count,
        CancellationToken ct)
    {
        var allItems = await GetAllInternalAsync(includeDraft: false, ct);

        // Find the source article
        var sourceArticle = allItems.FirstOrDefault(item => item.Slug == articleId);

        if (sourceArticle == null || sourceArticle.Tags.Count == 0)
        {
            // Fallback: return most recent items from the same collection
            return allItems
                .Where(item => item.Slug != articleId &&
                              item.CollectionName == sourceArticle?.CollectionName)
                .OrderByDescending(item => item.DateEpoch)
                .Take(count)
                .ToList();
        }

        var sourceTags = new HashSet<string>(sourceArticle.Tags, StringComparer.OrdinalIgnoreCase);

        // Find articles with tag overlap, ranked by shared tag count
        var relatedItems = allItems
            .Where(item => item.Slug != articleId)
            .Select(item => new
            {
                Item = item,
                SharedTagCount = item.Tags.Count(t => sourceTags.Contains(t))
            })
            .Where(x => x.SharedTagCount > 0)
            .OrderByDescending(x => x.SharedTagCount)
            .ThenByDescending(x => x.Item.DateEpoch)
            .Take(count)
            .Select(x => x.Item)
            .ToList();

        return relatedItems;
    }

    /// <summary>
    /// Get tag counts from in-memory items using LINQ GROUP BY.
    /// For file-based repository, this is efficient since items are already loaded.
    /// </summary>
    protected override async Task<IReadOnlyList<TagWithCount>> GetTagCountsInternalAsync(
        DateTimeOffset? dateFrom,
        DateTimeOffset? dateTo,
        string? sectionName,
        string? collectionName,
        int? maxTags,
        int minUses,
        CancellationToken ct)
    {
        var allItems = await GetAllInternalAsync(includeDraft: false, ct);

        // Apply filters
        var filtered = allItems.AsEnumerable();

        if (dateFrom.HasValue)
        {
            var fromEpoch = dateFrom.Value.ToUnixTimeSeconds();
            filtered = filtered.Where(item => item.DateEpoch >= fromEpoch);
        }

        if (dateTo.HasValue)
        {
            var toEpoch = dateTo.Value.ToUnixTimeSeconds();
            filtered = filtered.Where(item => item.DateEpoch <= toEpoch);
        }

        if (!string.IsNullOrWhiteSpace(sectionName) && !sectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(item => item.SectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(collectionName) && !collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(item => item.CollectionName.Equals(collectionName, StringComparison.OrdinalIgnoreCase));
        }

        // Count tags using LINQ GROUP BY
        var tagCounts = filtered
            .SelectMany(item => item.Tags)
            .GroupBy(tag => tag, StringComparer.OrdinalIgnoreCase)
            .Select(g => new TagWithCount { Tag = g.Key, Count = g.Count() })
            .Where(t => t.Count >= minUses)
            .OrderByDescending(t => t.Count)
            .ThenBy(t => t.Tag, StringComparer.OrdinalIgnoreCase);

        var result = maxTags.HasValue
            ? tagCounts.Take(maxTags.Value).ToList()
            : tagCounts.ToList();

        return result;
    }

    /// <summary>
    /// For file-based repository, items are already hydrated from frontmatter.
    /// Returns items as-is without additional processing.
    /// </summary>
    protected override Task<List<ContentItem>> HydrateRelationshipsAsync(
        IList<ContentItem> items,
        CancellationToken ct = default)
    {
        // File-based repo already has all data loaded from frontmatter - nothing to hydrate
        return Task.FromResult(items.ToList());
    }

    // ==================== Private File Loading Methods ====================

    /// <summary>
    /// Load all items from a collection directory.
    /// </summary>
    private async Task<List<ContentItem>> LoadCollectionItemsAsync(
        string collectionName,
        CancellationToken ct)
    {
        var normalizedCollection = collectionName.StartsWith('_') ? collectionName : $"_{collectionName}";
        var collectionPath = Path.Combine(_basePath, normalizedCollection);

        if (!Directory.Exists(collectionPath))
        {
            return [];
        }

        var markdownFiles = Directory.GetFiles(collectionPath, "*.md", SearchOption.AllDirectories);

        var itemTasks = markdownFiles
            .Select(filePath => LoadContentItemAsync(filePath, normalizedCollection, ct))
            .ToArray();

        var items = await Task.WhenAll(itemTasks);
        return items.Where(item => item != null).ToList()!;
    }

    /// <summary>
    /// Load a single content item from a markdown file.
    /// </summary>
    private async Task<ContentItem?> LoadContentItemAsync(
        string filePath,
        string collectionName,
        CancellationToken ct)
    {
        var fileContent = await File.ReadAllTextAsync(filePath, ct);
        var (frontMatter, content) = _frontMatterParser.Parse(fileContent);

        var title = _frontMatterParser.GetValue<string>(frontMatter, "title");
        var dateStr = _frontMatterParser.GetValue<string>(frontMatter, "date");

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(dateStr))
        {
            return null;
        }

        if (!DateTimeOffset.TryParse(dateStr, out var date))
        {
            return null;
        }

        var author = _frontMatterParser.GetValue<string>(frontMatter, "author", "Microsoft");
        var excerpt = _frontMatterParser.GetValue<string>(frontMatter, "excerpt", string.Empty);
        var sectionNames = _frontMatterParser.GetListValue(frontMatter, "section_names");
        var tags = _frontMatterParser.GetListValue(frontMatter, "tags");
        var externalUrl = _frontMatterParser.GetValue<string>(frontMatter, "external_url", string.Empty);
        var feedName = _frontMatterParser.GetValue<string>(frontMatter, "feed_name", string.Empty);
        var plans = _frontMatterParser.GetListValue(frontMatter, "plans");
        var ghesSupport = _frontMatterParser.GetValue<bool>(frontMatter, "ghes_support", false);
        var draft = _frontMatterParser.GetValue<bool>(frontMatter, "draft", false);

        var subcollectionName = DeriveSubcollectionFromPath(filePath);
        var derivedCollection = collectionName.TrimStart('_');

        // GhcFeature is derived from subcollection or collection name being "ghc-features"
        var ghcFeature = string.Equals(subcollectionName, "ghc-features", StringComparison.OrdinalIgnoreCase)
                      || string.Equals(derivedCollection, "ghc-features", StringComparison.OrdinalIgnoreCase);

        // PrimarySectionName is computed internally by ContentItem from SectionNames

        var filename = Path.GetFileNameWithoutExtension(filePath);
        var slug = StripDatePrefix(filename).ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(excerpt))
        {
            excerpt = MarkdownService.ExtractExcerpt(content);
        }

        // Compute primary section for URL - same logic as ContentItem uses internally
        var primarySectionName = ContentItem.ComputePrimarySectionName(sectionNames);
        var currentPagePath = $"/{primarySectionName}/{derivedCollection}/{slug}".ToLowerInvariant();

        // Return ContentItem with raw Content - base class will render to HTML
        // Note: PrimarySectionName is computed internally by ContentItem from SectionNames
        return new ContentItem
        {
            Slug = slug,
            Title = title,
            Author = author,
            DateEpoch = date.ToUnixTimeSeconds(),
            CollectionName = derivedCollection,
            SubcollectionName = subcollectionName,
            FeedName = feedName,
            SectionNames = sectionNames,
            Tags = tags,
            Plans = plans,
            GhesSupport = ghesSupport,
            Draft = draft,
            GhcFeature = ghcFeature,
            Content = content,  // Raw markdown - base class renders this
            Excerpt = excerpt,
            ExternalUrl = externalUrl,
            Url = currentPagePath
        };
    }

    /// <summary>
    /// Strips YYYY-MM-DD- date prefix from filename to create clean slug.
    /// </summary>
    private static string StripDatePrefix(string filename)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            filename,
            @"^\d{4}-\d{2}-\d{2}-",
            string.Empty);
    }

    /// <summary>
    /// Derive subcollection name from file path based on subfolder structure.
    /// </summary>
    private string? DeriveSubcollectionFromPath(string filePath)
    {
        var normalizedPath = filePath.Replace('\\', '/');
        var relativePath = Path.GetRelativePath(_basePath, normalizedPath).Replace('\\', '/');
        var segments = relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length > 2)
        {
            return segments[1];
        }

        return null;
    }
}
