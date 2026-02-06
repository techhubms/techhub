using System.Collections.Concurrent;
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
    // Tag split separators (same as ContentSyncService)
    private static readonly char[] TagSplitSeparators = [' ', '-', '_'];

    private readonly string _basePath;
    private readonly FrontMatterParser _frontMatterParser;
    private readonly AppSettings _settings;

    // Cache section_names by slug for filtering (not exposed on ContentItem model)
    private readonly ConcurrentDictionary<string, List<string>> _sectionNamesCache = new();

    // Local cache key for all items (file-based repo loads everything at once)
    private const string AllItemsCacheKey = "FileBasedRepository:AllItems";

    public FileBasedContentRepository(
        IOptions<AppSettings> settings,
        IMarkdownService markdownService,
        IHostEnvironment environment,
        IMemoryCache cache)
        : base(cache, markdownService, settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(environment);

        _settings = settings.Value;
        var configuredPath = _settings.Content.CollectionsPath;

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
        return await GetAllInternalAsync(includeDraft: true, limit: int.MaxValue, offset: 0, ct);
    }

    /// <summary>
    /// Internal helper: Load all content from markdown files.
    /// Results are cached in the local AllItems cache.
    /// Stores ContentItemDetail internally but returns as ContentItem for list views.
    /// </summary>
    private async Task<IReadOnlyList<ContentItem>> GetAllInternalAsync(
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        // Use local cache for all items (file-based loads everything at once)
        // Cache stores ContentItemDetail but returns as ContentItem for list views
        var allItems = await Cache.GetOrCreateAsync(AllItemsCacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);

            // Load all collections in parallel - get collection names from all sections
            var sections = await GetAllSectionsAsync(ct);
            var collectionNames = sections
                .SelectMany(s => s.Collections.Select(c => c.Name.StartsWith('_') ? c.Name : $"_{c.Name}"))
                .Distinct()
                .ToArray();

            var collectionTasks = collectionNames
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

            // Store as ContentItem for polymorphic access but actually holds ContentItemDetail
            return (IReadOnlyList<ContentItem>)[.. cachedItems.Cast<ContentItem>()];
        }) ?? [];

        // Filter drafts if needed
        if (!includeDraft)
        {
            allItems = [.. allItems.Where(item => !item.Draft)];
        }

        // Apply pagination
        return [.. allItems.Skip(offset).Take(limit)];
    }

    /// <summary>
    /// Internal implementation: Get content by slug from cached data.
    /// Returns ContentItemDetail with full content for rendering.
    /// </summary>
    protected override async Task<ContentItemDetail?> GetBySlugInternalAsync(
        string collectionName,
        string slug,
        bool includeDraft,
        CancellationToken ct)
    {
        var items = await GetByCollectionInternalAsync(collectionName, subcollectionName: null, includeDraft, int.MaxValue, 0, ct);
        var item = items.FirstOrDefault(item => item.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        // The cache stores ContentItemDetail objects, so cast back for detail view
        return item as ContentItemDetail;
    }

    /// <summary>
    /// Internal helper: Get content by collection from cached data.
    /// Optionally filters by subcollection.
    /// </summary>
    private async Task<IReadOnlyList<ContentItem>> GetByCollectionInternalAsync(
        string collectionName,
        string? subcollectionName,
        bool includeDraft,
        int limit,
        int offset,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(collectionName);

        var allItems = await GetAllInternalAsync(includeDraft, int.MaxValue, 0, ct);

        // Handle virtual "all" collection
        if (collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return [.. allItems.OrderByDescending(x => x.DateEpoch).Skip(offset).Take(limit)];
        }

        // Normalize collection name
        var normalizedCollection = collectionName.StartsWith('_') ? collectionName : $"_{collectionName}";

        var collectionItems = allItems
            .Where(item => item.CollectionName.Equals(normalizedCollection.TrimStart('_'), StringComparison.OrdinalIgnoreCase));

        // Optionally filter by subcollection
        if (!string.IsNullOrWhiteSpace(subcollectionName))
        {
            collectionItems = collectionItems.Where(item =>
                item.SubcollectionName != null &&
                item.SubcollectionName.Equals(subcollectionName, StringComparison.OrdinalIgnoreCase));
        }

        return [.. collectionItems.OrderByDescending(x => x.DateEpoch).Skip(offset).Take(limit)];
    }

    /// <summary>
    /// In-memory search implementation for file-based repository.
    /// Filters cached content by query, tags, sections, collections, and date range.
    /// </summary>
    protected override async Task<SearchResults<ContentItem>> SearchInternalAsync(
        SearchRequest request,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        var allItems = await GetAllInternalAsync(includeDraft: request.IncludeDraft, limit: int.MaxValue, offset: 0, ct);
        IEnumerable<ContentItem> filtered = allItems;

        // Full-text search (naive: check if query appears in title, excerpt, or content)
        // Items are stored as ContentItemDetail, so cast to access Content property
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            var query = request.Query.ToLowerInvariant();
            filtered = filtered.Where(item =>
                (item.Title?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (item.Excerpt?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                ((item as ContentItemDetail)?.Content?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        // Tag filtering (AND logic)
        if (request.Tags is { Count: > 0 })
        {
            foreach (var tag in request.Tags)
            {
                var normalizedTag = tag.Trim();
                filtered = filtered.Where(item =>
                    item.Tags.Any(t => t.Contains(normalizedTag, StringComparison.OrdinalIgnoreCase)));
            }
        }

        // Section filtering (OR logic) - use cached section_names from frontmatter ("all" means no filter)
        if (request.Sections is { Count: > 0 } &&
            !request.Sections.Any(s => s.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            filtered = filtered.Where(item =>
                _sectionNamesCache.TryGetValue(item.Slug, out var sections) &&
                sections.Any(s => request.Sections.Contains(s, StringComparer.OrdinalIgnoreCase)));
        }

        // Collection filtering (OR logic) ("all" means no filter)
        if (request.Collections is { Count: > 0 } &&
            !request.Collections.Any(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)))
        {
            filtered = filtered.Where(item =>
                request.Collections.Contains(item.CollectionName, StringComparer.OrdinalIgnoreCase));
        }

        // Subcollection filtering ("all" means no filter)
        if (!string.IsNullOrWhiteSpace(request.Subcollection) &&
            !request.Subcollection.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(item =>
                item.SubcollectionName != null &&
                item.SubcollectionName.Equals(request.Subcollection, StringComparison.OrdinalIgnoreCase));
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
        ArgumentNullException.ThrowIfNull(request);

        var allItems = await GetAllInternalAsync(includeDraft: false, limit: int.MaxValue, offset: 0, ct);
        IEnumerable<ContentItem> filtered = allItems;

        // Apply filters
        if (request.Tags is { Count: > 0 })
        {
            foreach (var tag in request.Tags)
            {
                var normalizedTag = tag.Trim();
                filtered = filtered.Where(item =>
                    item.Tags.Any(t => t.Contains(normalizedTag, StringComparison.OrdinalIgnoreCase)));
            }
        }

        if (request.Sections is { Count: > 0 })
        {
            filtered = filtered.Where(item =>
                _sectionNamesCache.TryGetValue(item.Slug, out var sections) &&
                sections.Any(s => request.Sections.Contains(s, StringComparer.OrdinalIgnoreCase)));
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
        // Expand tags into words for substring matching (like content_tags_expanded table)
        // For example, "GitHub Copilot" becomes "github" and "copilot" separately
        if (request.FacetFields.Contains("tags"))
        {
            var tagWordToSlugs = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in filteredList)
            {
                foreach (var tag in item.Tags)
                {
                    // Split tag into words (same logic as ContentSyncService)
                    var words = tag.Split(TagSplitSeparators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        var wordNormalized = word.Trim();
                        if (string.IsNullOrWhiteSpace(wordNormalized))
                        {
                            continue;
                        }

                        if (!tagWordToSlugs.TryGetValue(wordNormalized, out var slugs))
                        {
                            slugs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            tagWordToSlugs[wordNormalized] = slugs;
                        }

                        slugs.Add(item.Slug);
                    }
                }
            }

            var tagCounts = tagWordToSlugs
                .Select(kvp => new FacetValue { Value = kvp.Key, Count = kvp.Value.Count })
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

        // Compute section facets from cached section_names
        if (request.FacetFields.Contains("sections"))
        {
            var sectionCounts = filteredList
                .Where(item => _sectionNamesCache.ContainsKey(item.Slug))
                .SelectMany(item => _sectionNamesCache[item.Slug])
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
    /// Get tag counts from in-memory items using LINQ GROUP BY.
    /// For file-based repository, this is efficient since items are already loaded.
    /// Automatically excludes section and collection titles from tag counts.
    /// </summary>
    protected override async Task<IReadOnlyList<TagWithCount>> GetTagCountsInternalAsync(
        TagCountsRequest request,
        CancellationToken ct)
    {
        var allItems = await GetAllInternalAsync(includeDraft: false, limit: int.MaxValue, offset: 0, ct);

        // Apply filters
        var filtered = allItems.AsEnumerable();

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

        if (!string.IsNullOrWhiteSpace(request.SectionName) && !request.SectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(item =>
                _sectionNamesCache.TryGetValue(item.Slug, out var sections) &&
                sections.Contains(request.SectionName, StringComparer.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.CollectionName) && !request.CollectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            filtered = filtered.Where(item => item.CollectionName.Equals(request.CollectionName, StringComparison.OrdinalIgnoreCase));
        }

        // Build exclude set from section/collection titles
        var excludeSet = await BuildSectionCollectionExcludeSet();

        // Count tags, filter out excluded tags BEFORE grouping, sort, and limit
        var tagCounts = filtered
            .SelectMany(item => item.Tags)
            .Where(tag => !excludeSet.Contains(tag)) // Filter section/collection tags BEFORE grouping
            .GroupBy(tag => tag, StringComparer.OrdinalIgnoreCase)
            .Select(g => new TagWithCount { Tag = g.Key, Count = g.Count() })
            .Where(t => t.Count >= request.MinUses)
            .OrderByDescending(t => t.Count)
            .ThenBy(t => t.Tag, StringComparer.OrdinalIgnoreCase);

        var result = request.MaxTags.HasValue
            ? tagCounts.Take(request.MaxTags.Value).ToList()
            : [.. tagCounts];

        return result;
    }

    // ==================== Private File Loading Methods ====================

    /// <summary>
    /// Load all items from a collection directory.
    /// Returns ContentItemDetail with raw content for detail views.
    /// </summary>
    private async Task<List<ContentItemDetail>> LoadCollectionItemsAsync(
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
    /// Returns ContentItemDetail with raw content - base class will render to HTML.
    /// </summary>
    private async Task<ContentItemDetail?> LoadContentItemAsync(
        string filePath,
        string collectionName,
        CancellationToken ct)
    {
        var fileContent = await File.ReadAllTextAsync(filePath, ct);
        var (frontMatter, content) = _frontMatterParser.Parse(fileContent);

        var title = FrontMatterParser.GetValue<string>(frontMatter, "title");
        var dateStr = FrontMatterParser.GetValue<string>(frontMatter, "date");

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(dateStr))
        {
            throw new ArgumentException(
                $"Content item in collection '{collectionName}' is missing required frontmatter fields. " +
                $"File: {filePath}");
        }

        if (!DateTimeOffset.TryParse(dateStr, out var date))
        {
            throw new ArgumentException(
                $"Content item in collection '{collectionName}' has invalid date format in frontmatter. " +
                $"File: {filePath}, Date: {dateStr}");
        }

        var author = FrontMatterParser.GetValue<string>(frontMatter, "author", "Unknown");
        var sectionNames = FrontMatterParser.GetListValue(frontMatter, "section_names");
        var tags = FrontMatterParser.GetListValue(frontMatter, "tags");
        var externalUrl = FrontMatterParser.GetValue<string>(frontMatter, "external_url", string.Empty);
        var feedName = FrontMatterParser.GetValue<string>(frontMatter, "feed_name", string.Empty);
        var plans = FrontMatterParser.GetListValue(frontMatter, "plans");
        var ghesSupport = FrontMatterParser.GetValue<bool>(frontMatter, "ghes_support", false);
        var draft = FrontMatterParser.GetValue<bool>(frontMatter, "draft", false);
        var primarySectionName = FrontMatterParser.GetValue<string>(frontMatter, "primary_section", "all");
        var excerpt = MarkdownService.ExtractExcerpt(content);

        var filename = Path.GetFileNameWithoutExtension(filePath);
        var slug = StripDatePrefix(filename).ToLowerInvariant();

        // Cache section_names for filtering (not exposed on model)
        _sectionNamesCache[slug] = sectionNames;

        // Convert plans list to CSV for constructor
        var plansString = plans.Count > 0 ? string.Join(",", plans) : null;

        // Return ContentItemDetail with raw Content - base class will render to HTML
        var item = new ContentItemDetail(
            slug: slug,
            title: title,
            author: author,
            dateEpoch: date.ToUnixTimeSeconds(),
            collectionName: collectionName.TrimStart('_'),
            feedName: feedName,
            primarySectionName: primarySectionName,
            excerpt: excerpt,
            externalUrl: externalUrl,
            draft: draft,
            content: content,
            subcollectionName: DeriveSubcollectionFromPath(filePath),
            plans: plansString,
            ghesSupport: ghesSupport
        );

        if (tags.Count > 0)
        {
            item.SetTags(tags);
        }

        return item;
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
