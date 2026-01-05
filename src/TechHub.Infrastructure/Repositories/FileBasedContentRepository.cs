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

    // Valid collection directories per Jekyll configuration
    private static readonly string[] ValidCollections = 
    {
        "_news",
        "_videos", 
        "_community",
        "_blogs",
        "_roundups"
    };

    public FileBasedContentRepository(
        IOptions<AppSettings> settings,
        IMarkdownService markdownService)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(markdownService);
        _basePath = settings.Value.Content.CollectionsPath;
        _frontMatterParser = new FrontMatterParser();
        _markdownService = markdownService;
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
            // Double-check after acquiring lock
            if (_cachedAllItems != null)
            {
                return _cachedAllItems;
            }

            var allItems = new List<ContentItem>();

            foreach (var collection in ValidCollections)
            {
                var items = await LoadCollectionItemsAsync(collection, cancellationToken);
                allItems.AddRange(items);
            }

            _cachedAllItems = allItems
                .OrderByDescending(x => x.DateEpoch)
                .ToList();
                
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
        return allItems
            .Where(item => item.CollectionName.Equals(normalizedCollection.TrimStart('_'), StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DateEpoch)
            .ToList();
    }

    /// <summary>
    /// Get content items filtered by category.
    /// Filters from cached in-memory data.
    /// Returns items sorted by date (DateEpoch) in descending order (newest first).
    /// </summary>
    public async Task<IReadOnlyList<ContentItem>> GetByCategoryAsync(
        string category,
        CancellationToken cancellationToken = default)
    {
        var allItems = await GetAllAsync(cancellationToken);
        return allItems
            .Where(item => item.Categories.Contains(category, StringComparer.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DateEpoch)
            .ToList();
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
            return Array.Empty<ContentItem>();
        }

        var allItems = await GetAllAsync(cancellationToken);
        var lowerQuery = query.ToLowerInvariant();

        return allItems
            .Where(item =>
                item.Title.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                item.Description.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                item.Tags.Any(tag => tag.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(x => x.DateEpoch)
            .ToList();
    }

    /// <summary>
    /// Get all unique tags across all content.
    /// Computes from cached in-memory data.
    /// Returns normalized (lowercase) unique tags sorted alphabetically.
    /// </summary>
    public async Task<IReadOnlyList<string>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        var allItems = await GetAllAsync(cancellationToken);
        
        return allItems
            .SelectMany(item => item.Tags)
            .Select(tag => tag.ToLowerInvariant())
            .Distinct()
            .OrderBy(tag => tag)
            .ToList();
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
            return new List<ContentItem>();
        }

        var items = new List<ContentItem>();
        var markdownFiles = Directory.GetFiles(collectionPath, "*.md", SearchOption.AllDirectories);

        foreach (var filePath in markdownFiles)
        {
            var item = await LoadContentItemAsync(filePath, normalizedCollection, cancellationToken);
            if (item != null)
            {
                items.Add(item);
            }
        }

        return items;
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
        try
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
            var categories = _frontMatterParser.GetListValue(frontMatter, "categories");
            var tags = _frontMatterParser.GetListValue(frontMatter, "tags");
            var canonicalUrl = _frontMatterParser.GetValue<string>(frontMatter, "canonical_url", string.Empty);
            var videoUrl = _frontMatterParser.GetValue<string>(frontMatter, "video_url", string.Empty);
            var altCollection = _frontMatterParser.GetValue<string>(frontMatter, "alt_collection", string.Empty);
            var viewingMode = _frontMatterParser.GetValue<string>(frontMatter, "viewing_mode", string.Empty);

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

            // Process YouTube embeds and render markdown to HTML
            var processedMarkdown = _markdownService.ProcessYouTubeEmbeds(content);
            var renderedHtml = _markdownService.RenderToHtml(processedMarkdown);
            
            // Extract video ID from URL if present
            string? videoId = null;
            if (!string.IsNullOrWhiteSpace(videoUrl))
            {
                var match = System.Text.RegularExpressions.Regex.Match(videoUrl, @"(?:youtube\.com/watch\?v=|youtu\.be/)([a-zA-Z0-9_-]+)");
                if (match.Success)
                {
                    videoId = match.Groups[1].Value;
                }
            }

            var item = new ContentItem
            {
                Slug = fileName,
                Title = title,
                Description = description,
                Author = author,
                DateEpoch = date.ToUnixTimeSeconds(),
                CollectionName = collection.TrimStart('_'), // Store without _ prefix
                AltCollection = altCollection,
                Categories = categories,
                Tags = tags,
                RenderedHtml = renderedHtml,
                Excerpt = excerpt,
                ExternalUrl = canonicalUrl,
                VideoId = videoId,
                ViewingMode = viewingMode
            };

            return item;
        }
        catch (Exception)
        {
            // Skip files that fail to parse
            return null;
        }
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
