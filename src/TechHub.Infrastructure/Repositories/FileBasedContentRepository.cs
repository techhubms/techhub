using System.Text.Json;
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
/// </summary>
public sealed class FileBasedContentRepository : IContentRepository, IDisposable
{
    private readonly string _basePath;
    private readonly FrontMatterParser _frontMatterParser;
    private readonly IMarkdownService _markdownService;
    private IReadOnlyList<ContentItem>? _cachedAllItems;
    private readonly SemaphoreSlim _loadLock = new(1, 1);

    private static readonly string[] _validCollections =
    [
        "_news",
        "_videos",
        "_community",
        "_blogs",
        "_roundups",
        "_custom"
    ];

    public FileBasedContentRepository(
        IOptions<AppSettings> settings,
        IMarkdownService markdownService,
        IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(markdownService);
        ArgumentNullException.ThrowIfNull(environment);

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
        // Load all data into cache
        return await GetAllAsync(cancellationToken);
    }

    /// <summary>
    /// Get all content items across all collections.
    /// Loads from disk once at startup and caches in memory.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Return cached data if already loaded
        if (_cachedAllItems != null)
        {
            return _cachedAllItems;
        }

        // Thread-safe lazy loading
        await _loadLock.WaitAsync(cancellationToken);
        try
        {
            // Double-check after acquiring lock (required for thread-safety)
            // CA1508 false positive: another thread may have populated the cache while we waited for the lock
#pragma warning disable CA1508 // Avoid dead conditional code - this is a valid double-check pattern
            if (_cachedAllItems != null)
            {
                return _cachedAllItems;
            }
#pragma warning restore CA1508

            // Load all collections in parallel for faster startup
            var collectionTasks = _validCollections
                .Select(collection => LoadCollectionItemsAsync(collection, cancellationToken))
                .ToArray();

            var collectionResults = await Task.WhenAll(collectionTasks);
            var allItems = collectionResults.SelectMany(items => items).ToList();

            // Filter out future-dated content (DateEpoch > current time)
            var currentEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _cachedAllItems = [.. allItems
                .Where(x => x.DateEpoch <= currentEpoch)
                .OrderByDescending(x => x.DateEpoch)];

            return _cachedAllItems;
        }
        finally
        {
            _loadLock.Release();
        }
    }

    /// <summary>
    /// Get content items filtered by collection name.
    /// Filters from cached in-memory data.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetByCollectionAsync(
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(collectionName);
        // Normalize collection name (add _ prefix if missing)
        var normalizedCollection = collectionName.StartsWith('_') ? collectionName : $"_{collectionName}";

        // Load all items (from cache if available)
        var allItems = await GetAllAsync(cancellationToken);

        // Filter by collection
        return [.. allItems
            .Where(item => item.CollectionName.Equals(normalizedCollection.TrimStart('_'), StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Get content items filtered by section name.
    /// Matches against the SectionNames property which contains lowercase section names like "ai", "github-copilot".
    /// Filters from cached in-memory data.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// </summary>
    /// <param name="sectionName">Section name (lowercase, e.g., "ai", "github-copilot") - matches section.Name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionName,
        CancellationToken cancellationToken = default)
    {
        var allItems = await GetAllAsync(cancellationToken);
        return [.. allItems
            .Where(item => item.SectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Get a single content item by slug within a collection.
    /// Searches cached in-memory data.
    /// Slug is the filename without extension (e.g., "2026-01-01-my-article" from "2026-01-01-my-article.md")
    /// </summary>
    public async Task<ContentItem?> GetBySlugAsync(
        string collectionName,
        string slug,
        CancellationToken cancellationToken = default)
    {
        var items = await GetByCollectionAsync(collectionName, cancellationToken);
        return items.FirstOrDefault(item => item.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Search content items by text query (title, description, tags).
    /// Searches cached in-memory data.
    /// Case-insensitive search across multiple fields.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var allItems = await GetAllAsync(cancellationToken);
        var lowerQuery = query.ToLowerInvariant();

        return [.. allItems
            .Where(item =>
                item.Title.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                item.Description.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                item.Tags.Any(tag => tag.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(x => x.DateEpoch)];
    }

    /// <summary>
    /// Get all unique tags across all content.
    /// Computes from cached in-memory data.
    /// Returns normalized (lowercase) unique tags sorted alphabetically.
    /// </summary>
    public async Task<IReadOnlyList<string>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        var allItems = await GetAllAsync(cancellationToken);

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
        string collection,
        CancellationToken cancellationToken)
    {
        // Normalize collection name (add _ prefix if missing)
        var normalizedCollection = collection.StartsWith('_') ? collection : $"_{collection}";

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
        string collection,
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
        var description = _frontMatterParser.GetValue<string>(frontMatter, "description", string.Empty);
        var excerpt = _frontMatterParser.GetValue<string>(frontMatter, "excerpt", string.Empty);

        // Map 'categories' (regular content) or 'section' (custom pages) to lowercase section names
        var categories = _frontMatterParser.GetListValue(frontMatter, "categories");
        var sectionNames = categories.Select(c => c.ToLowerInvariant().Replace(" ", "-", StringComparison.Ordinal)).ToList();

        var tags = _frontMatterParser.GetListValue(frontMatter, "tags");
        var externalUrl = _frontMatterParser.GetValue<string>(frontMatter, "canonical_url", string.Empty);
        var videoId = _frontMatterParser.GetValue<string>(frontMatter, "youtube_video_id", string.Empty);
        var viewingMode = _frontMatterParser.GetValue<string>(frontMatter, "viewing_mode", "external");
        var altCollection = _frontMatterParser.GetValue<string>(frontMatter, "alt-collection", string.Empty);

        // Parse sidebar-info if present (dynamic JSON data for custom sidebars)
        JsonElement? sidebarInfo = null;
        if (frontMatter.TryGetValue("sidebar-info", out var sidebarData))
        {
            // Convert to JSON for flexible client-side consumption
            var json = System.Text.Json.JsonSerializer.Serialize(sidebarData);
            sidebarInfo = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);
        }

        // Generate ID from filename (without extension)
        var fileName = Path.GetFileNameWithoutExtension(filePath);

        // Generate excerpt if not provided in frontmatter
        if (string.IsNullOrWhiteSpace(excerpt))
        {
            excerpt = _markdownService.ExtractExcerpt(content);
        }

        // Use description if provided, otherwise use excerpt
        if (string.IsNullOrWhiteSpace(description))
        {
            description = excerpt;
        }

        // Process Jekyll/Liquid variables, YouTube embeds, and render markdown to HTML
        var processedMarkdown = _markdownService.ProcessJekyllVariables(content, frontMatter);
        processedMarkdown = _markdownService.ProcessYouTubeEmbeds(processedMarkdown);

        // Create temporary ContentItem to get primary section and URL
        var tempItem = new ContentItem
        {
            Slug = fileName,
            Title = title,
            Description = description,
            Author = author,
            DateEpoch = date.ToUnixTimeSeconds(),
            CollectionName = collection.TrimStart('_'),
            AltCollection = altCollection,
            SectionNames = sectionNames,
            Tags = tags,
            RenderedHtml = string.Empty, // Will be set below
            Excerpt = excerpt,
            ExternalUrl = externalUrl,
            VideoId = videoId,
            ViewingMode = viewingMode,
            SidebarInfo = sidebarInfo
        };

        // Get primary section and current page path using existing functionality
        var primarySection = tempItem.GetPrimarySectionUrl();
        var currentPagePath = tempItem.GetUrl();

        var renderedHtml = _markdownService.RenderToHtml(
            processedMarkdown,
            currentPagePath,
            primarySection,
            tempItem.CollectionName);

        var item = new ContentItem
        {
            Slug = fileName,
            Title = title,
            Description = description,
            Author = author,
            DateEpoch = date.ToUnixTimeSeconds(),
            CollectionName = collection.TrimStart('_'), // Store without _ prefix
            AltCollection = altCollection,
            SectionNames = sectionNames,
            Tags = tags,
            RenderedHtml = renderedHtml,
            Excerpt = excerpt,
            ExternalUrl = externalUrl,
            VideoId = videoId,
            ViewingMode = viewingMode,
            SidebarInfo = sidebarInfo
        };

        return item;
    }

    /// <summary>
    /// Dispose of resources
    /// </summary>
    public void Dispose()
    {
        _loadLock.Dispose();
        GC.SuppressFinalize(this);
    }
}
