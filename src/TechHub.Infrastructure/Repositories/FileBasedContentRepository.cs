using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Repository for loading content from markdown files in collections directories
/// Reads from: collections/_news/, collections/_videos/, collections/_blogs/, etc.
/// Uses IMemoryCache for caching loaded content.
/// </summary>
public class FileBasedContentRepository : IContentRepository
{
    private readonly string _basePath;
    private readonly FrontMatterParser _frontMatterParser;
    private readonly IMarkdownService _markdownService;
    private readonly ITagMatchingService _tagMatchingService;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "ContentRepository:AllItems";

    private static readonly string[] _validCollections =
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
        ITagMatchingService tagMatchingService,
        IHostEnvironment environment,
        IMemoryCache cache)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(tagMatchingService);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(cache);

        var configuredPath = settings.Value.Content.CollectionsPath;

        // Resolve relative paths to absolute paths based on content root
        // In Development/Test: ContentRootPath = /workspaces/techhub/src/TechHub.Api
        // We need to go up to solution root to find collections/
        if (Path.IsPathRooted(configuredPath))
        {
            _basePath = configuredPath;
        }
        else
        {
            // Navigate up from API project to solution root, then to collections
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
        _markdownService = markdownService;
        _tagMatchingService = tagMatchingService;
        _cache = cache;
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

        // Fallback: assume startPath is already at solution root
        return startPath;
    }

    /// <summary>
    /// Initialize the repository by loading all data from disk.
    /// Should be called once at application startup.
    /// Returns the loaded collection for logging purposes.
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> InitializeAsync(CancellationToken cancellationToken = default)
    {
        // Load all data into cache (include drafts for initialization)
        return await GetAllAsync(includeDraft: true, cancellationToken);
    }

    /// <summary>
    /// Get all content items across all collections.
    /// Uses IMemoryCache to cache loaded data. Cache never expires (content is static).
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetAllAsync(
        bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        var allItems = await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            // Cache never expires - content is loaded once and never changes without restart
            entry.SetPriority(Microsoft.Extensions.Caching.Memory.CacheItemPriority.NeverRemove);

            // Load all collections in parallel for faster startup
            var collectionTasks = _validCollections
                .Select(collection => LoadCollectionItemsAsync(collection, cancellationToken))
                .ToArray();

            var collectionResults = await Task.WhenAll(collectionTasks);
            var allItems = collectionResults.SelectMany(items => items).ToList();

            // Filter out future-dated content UNLESS it's marked as draft
            // Draft items are included regardless of date (for "Coming Soon" features)
            var currentEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var cachedItems = allItems
                .Where(x => x.DateEpoch <= currentEpoch || x.Draft)
                .OrderByDescending(x => x.DateEpoch)
                .ToList();

            return (IReadOnlyList<ContentItem>)cachedItems;
        }) ?? [];

        // Filter drafts if needed (after getting from cache)
        if (!includeDraft)
        {
            allItems = [.. allItems.Where(item => !item.Draft)];
        }

        return allItems;
    }

    /// <summary>
    /// Get content items filtered by collection name.
    /// Filters from cached in-memory data.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// Special case: collectionName="all" returns all content across all collections.
    /// Includes both root collection items and subcollection items (e.g., videos + ghc-features).
    /// Draft items are excluded by default unless includeDraft=true.
    /// </summary>
    /// <param name="collectionName">Collection name to filter by</param>
    /// <param name="includeDraft">If true, include draft items in results. Default is false.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(collectionName);

        // Load all items (from cache if available) - already filtered by includeDraft
        var allItems = await GetAllAsync(includeDraft, cancellationToken);

        // Handle virtual "all" collection - return all content
        if (collectionName.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            // Already filtered by includeDraft parameter in GetAllAsync
            return [.. allItems.OrderByDescending(x => x.DateEpoch)];
        }

        // Normalize collection name (add _ prefix if missing)
        var normalizedCollection = collectionName.StartsWith('_') ? collectionName : $"_{collectionName}";

        // Filter by collection (includes both root and subcollections)
        // Already filtered by includeDraft parameter in GetAllAsync
        var collectionItems = allItems
            .Where(item => item.CollectionName.Equals(normalizedCollection.TrimStart('_'), StringComparison.OrdinalIgnoreCase));

        return [.. collectionItems.OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Get content items filtered by section name.
    /// Matches against the SectionNames property which contains lowercase section names like "ai", "github-copilot".
    /// Filters from cached in-memory data.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// Special case: sectionName="all" returns all content across all sections.
    /// </summary>
    /// <param name="sectionName">Section name (lowercase, e.g., "ai", "github-copilot") or "all" for all sections</param>
    /// <param name="includeDraft">Include draft items (default: false)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
        bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sectionName);

        var allItems = await GetAllAsync(includeDraft, cancellationToken);

        // Filter by section
        var sectionItems = sectionName.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? allItems
            : allItems.Where(item => item.SectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase));

        // Already filtered by includeDraft parameter in GetAllAsync
        return [.. sectionItems.OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Get a single content item by slug within a collection.
    /// Searches cached in-memory data.
    /// Slug is the filename without extension (e.g., "2026-01-01-my-article" from "2026-01-01-my-article.md")
    /// </summary>
    public async Task<ContentItem?> GetBySlugAsync(
        string collectionName,
        string slug,
        bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        var items = await GetByCollectionAsync(collectionName, includeDraft, cancellationToken);
        return items.FirstOrDefault(item => item.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Search content items by text query (title, excerpt, tags).
    /// Searches cached in-memory data.
    /// Case-insensitive search across multiple fields.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> SearchAsync(
        string query,
        bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var allItems = await GetAllAsync(includeDraft, cancellationToken);
        var lowerQuery = query.ToLowerInvariant();

        return [.. allItems
            .Where(item =>
                item.Title.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                item.Excerpt.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                item.Tags.Any(tag => tag.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Get all unique tags across all content.
    /// Computes from cached in-memory data.
    /// Returns normalized (lowercase) unique tags sorted alphabetically.
    /// </summary>
    public async Task<IReadOnlyList<string>> GetAllTagsAsync(
        bool includeDraft = false,
        CancellationToken cancellationToken = default)
    {
        var allItems = await GetAllAsync(includeDraft, cancellationToken);

        return [.. allItems
            .SelectMany(item => item.Tags)
            .Select(tag => tag.ToLowerInvariant())
            .Distinct()
            .OrderBy(tag => tag)];
    }

    /// <summary>
    /// Load all items from a collection directory.
    /// Helper method for initial data loading.
    /// </summary>
    private async Task<List<ContentItem>> LoadCollectionItemsAsync(
        string collectionName,
        CancellationToken cancellationToken)
    {
        // Normalize collection name (add _ prefix if missing)
        var normalizedCollection = collectionName.StartsWith('_') ? collectionName : $"_{collectionName}";

        var collectionPath = Path.Combine(_basePath, normalizedCollection);

        if (!Directory.Exists(collectionPath))
        {
            return [];
        }

        var markdownFiles = Directory.GetFiles(collectionPath, "*.md", SearchOption.AllDirectories);

        // Load files in parallel for faster processing
        var itemTasks = markdownFiles
            .Select(filePath => LoadContentItemAsync(filePath, normalizedCollection, cancellationToken))
            .ToArray();

        var items = await Task.WhenAll(itemTasks);

        // Filter out nulls (files that failed to parse or are invalid)
        return items.Where(item => item != null).ToList()!;
    }

    /// <summary>
    /// Load a single content item from a markdown file
    /// Parses frontmatter, extracts metadata, generates excerpt
    /// </summary>
    private async Task<ContentItem?> LoadContentItemAsync(
        string filePath,
        string collectionName,
        CancellationToken cancellationToken)
    {
        var fileContent = await File.ReadAllTextAsync(filePath, cancellationToken);
        var (frontMatter, content) = _frontMatterParser.Parse(fileContent);

        // Extract required fields (title, date)
        var title = _frontMatterParser.GetValue<string>(frontMatter, "title");
        var dateStr = _frontMatterParser.GetValue<string>(frontMatter, "date");

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(dateStr))
        {
            // Skip files without required frontmatter
            return null;
        }

        // Parse date (supports various formats)
        if (!DateTimeOffset.TryParse(dateStr, out var date))
        {
            return null;
        }

        // Extract optional fields
        var author = _frontMatterParser.GetValue<string>(frontMatter, "author", "Microsoft");
        var excerpt = _frontMatterParser.GetValue<string>(frontMatter, "excerpt", string.Empty);

        // Read section_names directly (ContentFixer has already normalized these to lowercase with hyphens)
        var sectionNames = _frontMatterParser.GetListValue(frontMatter, "section_names");

        var tags = _frontMatterParser.GetListValue(frontMatter, "tags");
        var externalUrl = _frontMatterParser.GetValue<string>(frontMatter, "external_url", string.Empty);
        var feedName = _frontMatterParser.GetValue<string>(frontMatter, "feed_name", string.Empty);

        // Parse GitHub Copilot features-specific fields
        var plans = _frontMatterParser.GetListValue(frontMatter, "plans");
        var ghesSupport = _frontMatterParser.GetValue<bool>(frontMatter, "ghes_support", false);
        var draft = _frontMatterParser.GetValue<bool>(frontMatter, "draft", false);
        var ghcFeature = _frontMatterParser.GetValue<bool>(frontMatter, "ghc_feature", false);

        // Derive subcollection from file path:
        // - collections/_videos/file.md → subcollection: null
        // - collections/_videos/vscode-updates/file.md → subcollection: "vscode-updates"
        // - collections/_news/file.md → subcollection: null
        var subcollectionName = DeriveSubcollectionFromPath(filePath);
        var derivedCollection = collectionName.TrimStart('_');

        // Read primary_section from frontmatter (computed by ContentFixer using priority order)
        var primarySectionName = _frontMatterParser.GetValue<string>(frontMatter, "primary_section");
        if (string.IsNullOrEmpty(primarySectionName))
        {
            // Fallback: compute at runtime if not in frontmatter (for backward compatibility)
            primarySectionName = Core.Helpers.SectionPriorityHelper.GetPrimarySectionName(sectionNames, derivedCollection);
        }

        // Parse sidebar-info if present (dynamic JSON data for custom sidebars)
        JsonElement? sidebarInfo = null;
        if (frontMatter.TryGetValue("sidebar-info", out var sidebarData))
        {
            // Convert to JSON for flexible client-side consumption
            var json = JsonSerializer.Serialize(sidebarData);
            sidebarInfo = JsonSerializer.Deserialize<JsonElement>(json);
        }

        // Generate slug from filename (without extension and without date prefix)
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var slug = StripDatePrefix(fileName);

        // Generate excerpt if not provided in frontmatter
        if (string.IsNullOrWhiteSpace(excerpt))
        {
            excerpt = _markdownService.ExtractExcerpt(content);
        }

        // Process YouTube embeds and render markdown to HTML
        var processedMarkdown = _markdownService.ProcessYouTubeEmbeds(content);

        // Calculate URL using primary_section (read from frontmatter or computed above)
        // URLs always use collection name, not subcollection (subcollections are for filtering only)
        // e.g., /github-copilot/videos/slug (not /github-copilot/ghc-features/slug)
        var currentPagePath = $"/{primarySectionName}/{derivedCollection}/{slug}".ToLowerInvariant();

        string renderedHtml;
        try
        {
            renderedHtml = _markdownService.RenderToHtml(
                processedMarkdown,
                currentPagePath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to render markdown for file: {filePath}. " +
                $"Slug: '{slug}', Collection: '{collectionName}', Sections: [{string.Join(", ", sectionNames)}]",
                ex);
        }

        var item = new ContentItem
        {
            Slug = slug,
            Title = title,
            Author = author,
            DateEpoch = date.ToUnixTimeSeconds(),
            DateIso = date.ToString("yyyy-MM-dd"),
            CollectionName = derivedCollection,
            SubcollectionName = subcollectionName,
            FeedName = feedName,
            SectionNames = sectionNames,
            PrimarySectionName = primarySectionName,
            Tags = tags,
            Plans = plans,
            GhesSupport = ghesSupport,
            Draft = draft,
            GhcFeature = ghcFeature,
            RenderedHtml = renderedHtml,
            Excerpt = excerpt,
            ExternalUrl = externalUrl,
            Url = currentPagePath,
            SidebarInfo = sidebarInfo
        };

        return item;
    }

    /// <summary>
    /// Strips YYYY-MM-DD- date prefix from slug if present.
    /// Example: "2026-01-12-what-quantum-safe-is" → "what-quantum-safe-is"
    /// </summary>
    /// <param name="slug">Original slug from filename</param>
    /// <returns>Slug with date prefix removed if it was present</returns>
    private static string StripDatePrefix(string slug)
    {
        // Pattern matches YYYY-MM-DD- at the start of the string
        // Example: 2026-01-12-slug-name → slug-name
        return System.Text.RegularExpressions.Regex.Replace(
            slug,
            @"^\d{4}-\d{2}-\d{2}-",
            string.Empty);
    }

    /// <summary>
    /// Filter content items by tags and/or date range with optional section/collection scoping
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> FilterAsync(
        FilterRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Start with all items or scoped items
        IReadOnlyList<ContentItem> items;

        if (!string.IsNullOrWhiteSpace(request.CollectionName) && !string.IsNullOrWhiteSpace(request.SectionName))
        {
            // Collection scope: filter by collection AND section (exclude drafts)
            var collectionItems = await GetByCollectionAsync(request.CollectionName, includeDraft: false, cancellationToken);
            items = [.. collectionItems.Where(item => item.SectionNames.Contains(request.SectionName, StringComparer.OrdinalIgnoreCase))];
        }
        else if (!string.IsNullOrWhiteSpace(request.SectionName))
        {
            // Section scope (exclude drafts)
            items = await GetBySectionAsync(request.SectionName, includeDraft: false, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(request.CollectionName))
        {
            // Collection scope only (exclude drafts)
            items = await GetByCollectionAsync(request.CollectionName, includeDraft: false, cancellationToken);
        }
        else
        {
            // No scope - all items (exclude drafts)
            items = await GetAllAsync(includeDraft: false, cancellationToken);
        }

        // Apply tag filter if tags are specified
        if (request.SelectedTags.Count > 0)
        {
            items = [.. items.Where(item => _tagMatchingService.MatchesAny(request.SelectedTags, item.Tags))];
        }

        // Apply date range filter
        if (request.DateFrom.HasValue)
        {
            var fromEpoch = request.DateFrom.Value.ToUnixTimeSeconds();
            items = [.. items.Where(item => item.DateEpoch >= fromEpoch)];
        }

        if (request.DateTo.HasValue)
        {
            var toEpoch = request.DateTo.Value.ToUnixTimeSeconds();
            items = [.. items.Where(item => item.DateEpoch <= toEpoch)];
        }

        // Results are already sorted by DateEpoch descending from GetAllAsync
        // If we filtered, maintain that order
        return [.. items.OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Derive subcollection name from file path based on subfolder structure
    /// - collections/_videos/file.md → null
    /// - collections/_videos/vscode-updates/file.md → "vscode-updates"
    /// - collections/_news/file.md → null
    /// </summary>
    private string? DeriveSubcollectionFromPath(string filePath)
    {
        // Normalize path separators
        var normalizedPath = filePath.Replace('\\', '/');

        // Get the relative path from the base path
        var relativePath = Path.GetRelativePath(_basePath, normalizedPath).Replace('\\', '/');

        // Split into segments: ["_videos", "vscode-updates", "file.md"] or ["_news", "file.md"]
        var segments = relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        // If the file is in a subdirectory (segments.Length > 2), return subdirectory name as subcollection
        // Otherwise return null
        if (segments.Length > 2)
        {
            // Use the subdirectory name (e.g., "vscode-updates" from ["_videos", "vscode-updates", "file.md"])
            return segments[1];
        }

        // File is in root of collection directory
        return null;
    }

    // ==================== Search Methods (Stub implementations) ====================
    // These will be properly implemented when migrating to SQLite/PostgreSQL

    /// <inheritdoc />
    public Task<SearchResults<ContentItem>> SearchAsync(
        SearchRequest request,
        CancellationToken ct = default)
    {
        // Stub implementation - not supported for file-based repository
        throw new NotSupportedException(
            "SearchAsync is not supported by FileBasedContentRepository. " +
            "Use SqliteContentRepository or PostgresContentRepository for search functionality.");
    }

    /// <inheritdoc />
    public Task<FacetResults> GetFacetsAsync(
        FacetRequest request,
        CancellationToken ct = default)
    {
        // Stub implementation - not supported for file-based repository
        throw new NotSupportedException(
            "GetFacetsAsync is not supported by FileBasedContentRepository. " +
            "Use SqliteContentRepository or PostgresContentRepository for facet functionality.");
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<ContentItem>> GetRelatedAsync(
        string articleId,
        int count = 5,
        CancellationToken ct = default)
    {
        // Stub implementation - not supported for file-based repository
        throw new NotSupportedException(
            "GetRelatedAsync is not supported by FileBasedContentRepository. " +
            "Use SqliteContentRepository or PostgresContentRepository for related articles functionality.");
    }
}
