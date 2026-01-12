# TechHub.Infrastructure Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Infrastructure/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).
> **RULE**: All rules from parent AGENTS.md files apply. This file adds Infrastructure layer-specific patterns.

## Overview

This project implements data access using the Repository pattern with file-based storage, markdown processing, caching, and other infrastructure concerns.

**When to read this file**: When implementing repositories, working with markdown files, adding caching, or understanding data access patterns.

**Testing this code**: See [tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md) for unit testing patterns.

## Project Structure

```text
TechHub.Infrastructure/
├── Repositories/                              # Repository implementations
│   ├── ConfigurationBasedSectionRepository.cs # Sections from appsettings.json
│   └── FileBasedContentRepository.cs          # Content from markdown files
├── Services/                                   # Infrastructure services
│   ├── FrontMatterParser.cs                   # YAML frontmatter parsing
│   ├── MarkdownService.cs                     # Markdown to HTML conversion
│   └── RssService.cs                          # RSS feed generation
└── TechHub.Infrastructure.csproj              # Project file
```

## Repository Patterns

### Configuration-Based Repository

**Load sections from appsettings.json** (no file I/O needed):

```csharp
namespace TechHub.Infrastructure.Repositories;

public sealed class ConfigurationBasedSectionRepository : ISectionRepository
{
    private readonly IReadOnlyList<Section> _sections;
    private readonly ILogger<ConfigurationBasedSectionRepository> _logger;
    
    public ConfigurationBasedSectionRepository(
        IOptions<AppSettings> settings,
        ILogger<ConfigurationBasedSectionRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(settings);
        _logger = logger;
        
        _sections = settings.Value.Content.Sections
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value))
            .ToList()
            .AsReadOnly();
        
        _logger.LogInformation("Loaded {Count} sections from configuration", _sections.Count);
    }
    
    public Task<IReadOnlyList<Section>> InitializeAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_sections);
    }
    
    public Task<IReadOnlyList<Section>> GetAllAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_sections);
    }
    
    public Task<Section?> GetByUrlAsync(string sectionUrl, CancellationToken ct = default)
    {
        var section = _sections.FirstOrDefault(s => 
            s.Url.Equals(sectionUrl, StringComparison.OrdinalIgnoreCase) ||
            s.Url.Equals($"/{sectionUrl}", StringComparison.OrdinalIgnoreCase));
        
        return Task.FromResult(section);
    }
    
    private static Section ConvertToSection(string key, SectionConfig config)
    {
        return new Section
        {
            Name = key,
            Title = config.Title,
            Description = config.Description,
            Url = config.Url,
            BackgroundImage = config.Image,
            Collections = config.Collections.Select(c => new CollectionReference
            {
                Title = c.Title,
                Name = c.Name,
                Url = c.Url,
                Description = c.Description,
                IsCustom = c.IsCustom
            }).ToList()
        };
    }
}
```

### File-Based Repository with Caching

**Load content from markdown files with memory caching**:

```csharp
namespace TechHub.Infrastructure.Repositories;

public sealed class FileBasedContentRepository : IContentRepository
{
    private readonly string _collectionsPath;
    private readonly IMemoryCache _cache;
    private readonly IMarkdownService _markdownService;
    private readonly ILogger<FileBasedContentRepository> _logger;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);
    
    private const string AllContentCacheKey = "all_content";
    
    public FileBasedContentRepository(
        IOptions<ContentOptions> options,
        IMemoryCache cache,
        IMarkdownService markdownService,
        ILogger<FileBasedContentRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        _collectionsPath = options.Value.CollectionsRootPath;
        _cache = cache;
        _markdownService = markdownService;
        _logger = logger;
    }
    
    public async Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken ct = default)
    {
        // Check cache first
        if (_cache.TryGetValue(AllContentCacheKey, out IReadOnlyList<ContentItem>? cached))
        {
            _logger.LogDebug("Returning {Count} content items from cache", cached!.Count);
            return cached;
        }
        
        // Load from disk
        _logger.LogInformation("Loading all content from disk");
        var content = await LoadAllContentAsync(ct);
        
        // Sort by date (newest first) - CRITICAL requirement
        var sorted = content
            .OrderByDescending(c => c.DateEpoch)
            .ToList()
            .AsReadOnly();
        
        // Cache the result
        _cache.Set(AllContentCacheKey, sorted, _cacheExpiration);
        _logger.LogInformation("Loaded and cached {Count} content items", sorted.Count);
        
        return sorted;
    }
    
    public async Task<IReadOnlyList<ContentItem>> GetBySectionAsync(
        string sectionUrl, 
        CancellationToken ct = default)
    {
        var allContent = await GetAllAsync(ct);
        
        // Filter by section URL (already sorted by GetAllAsync)
        var filtered = allContent
            .Where(c => c.SectionNames.Any(sectionName => 
                sectionName.Equals(sectionUrl.TrimStart('/'), StringComparison.OrdinalIgnoreCase)))
            .ToList()
            .AsReadOnly();
        
        _logger.LogDebug("Filtered to {Count} items for section {Section}", 
            filtered.Count, sectionUrl);
        
        return filtered;
    }
    
    public async Task<ContentItem?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var allContent = await GetAllAsync(ct);
        return allContent.FirstOrDefault(c => 
            c.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }
    
    private async Task<IReadOnlyList<ContentItem>> LoadAllContentAsync(CancellationToken ct)
    {
        var items = new List<ContentItem>();
        
        // Discover all collection directories
        var collectionDirs = Directory.GetDirectories(_collectionsPath, "_*");
        
        foreach (var collectionDir in collectionDirs)
        {
            var collectionName = Path.GetFileName(collectionDir);
            var markdownFiles = Directory.GetFiles(collectionDir, "*.md", SearchOption.AllDirectories);
            
            foreach (var filePath in markdownFiles)
            {
                try
                {
                    var content = await _markdownService.ProcessMarkdownFileAsync(filePath, collectionName, ct);
                    if (content is not null)
                    {
                        items.Add(content);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing markdown file: {FilePath}", filePath);
                    // Continue processing other files
                }
            }
        }
        
        return items;
    }
}
```

**CRITICAL Sorting Rule**: All repository methods return content sorted by `DateEpoch` descending (newest first). This sorting happens BEFORE caching so clients never need to sort.

## Markdown Processing

### Frontmatter Parsing

**Parse YAML frontmatter from markdown files**:

```csharp
namespace TechHub.Infrastructure.Services;

public class FrontMatterParser
{
    public static (FrontMatter metadata, string content) Parse(string markdownText)
    {
        // Check for frontmatter delimiters (---)
        if (!markdownText.StartsWith("---"))
        {
            return (new FrontMatter(), markdownText);
        }
        
        // Find end of frontmatter
        var endIndex = markdownText.IndexOf("---", 3, StringComparison.Ordinal);
        if (endIndex == -1)
        {
            return (new FrontMatter(), markdownText);
        }
        
        // Extract frontmatter and content
        var frontmatterYaml = markdownText[3..endIndex].Trim();
        var content = markdownText[(endIndex + 3)..].TrimStart();
        
        // Parse YAML
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        var metadata = deserializer.Deserialize<FrontMatter>(frontmatterYaml);
        
        return (metadata ?? new FrontMatter(), content);
    }
}

/// <summary>
/// Represents markdown frontmatter fields.
/// </summary>
public class FrontMatter
{
    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; }
    public DateTime? Date { get; set; }
    public List<string> Categories { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public string? CanonicalUrl { get; set; }
    public string ViewingMode { get; set; } = "external";
    public string? VideoId { get; set; }
    public string? AltCollection { get; set; }
}
```

### Markdown to HTML Conversion

**Convert markdown content to HTML with Markdig**:

```csharp
namespace TechHub.Infrastructure.Services;

public class MarkdownService : IMarkdownService
{
    private readonly MarkdownPipeline _pipeline;
    private readonly TimeProvider _timeProvider;
    private readonly string _timezone;
    private readonly ILogger<MarkdownService> _logger;
    
    public MarkdownService(
        IOptions<ContentOptions> options,
        TimeProvider timeProvider,
        ILogger<MarkdownService> logger)
    {
        _timeProvider = timeProvider;
        _timezone = options.Value.Timezone;
        _logger = logger;
        
        // Configure Markdig pipeline
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()    // Tables, task lists, etc.
            .UseAutoIdentifiers()       // Auto-generate heading IDs
            .UseEmojiAndSmiley()        // :emoji: support
            .UsePipeTables()            // GitHub-style tables
            .Build();
    }
    
    public async Task<ContentItem?> ProcessMarkdownFileAsync(
        string filePath,
        string collectionName,
        CancellationToken ct = default)
    {
        try
        {
            var markdownText = await File.ReadAllTextAsync(filePath, ct);
            
            // Parse frontmatter
            var (metadata, content) = FrontMatterParser.Parse(markdownText);
            
            // Extract excerpt (before <!--excerpt_end-->)
            var excerpt = ExtractExcerpt(content);
            
            // Convert markdown to HTML
            var html = Markdown.ToHtml(content, _pipeline);
            
            // Generate slug from filename
            var slug = Path.GetFileNameWithoutExtension(filePath);
            
            // Convert date to Unix epoch
            var dateEpoch = ConvertToEpoch(metadata.Date);
            
            return new ContentItem
            {
                Slug = slug,
                Title = metadata.Title,
                Author = metadata.Author,
                DateEpoch = dateEpoch,
                CollectionName = collectionName,
                SectionNames = metadata.Categories.AsReadOnly(),
                Tags = NormalizeTags(metadata.Tags).AsReadOnly(),
                Excerpt = excerpt,
                RenderedHtml = html,
                ExternalUrl = metadata.CanonicalUrl,
                ViewingMode = metadata.ViewingMode,
                VideoId = metadata.VideoId,
                AltCollection = metadata.AltCollection
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing markdown file: {FilePath}", filePath);
            return null;
        }
    }
    
    private static string ExtractExcerpt(string content)
    {
        var excerptEndIndex = content.IndexOf("<!--excerpt_end-->", StringComparison.Ordinal);
        if (excerptEndIndex == -1)
        {
            // No explicit excerpt marker, take first 200 words
            var words = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(' ', words.Take(200));
        }
        
        return content[..excerptEndIndex].Trim();
    }
    
    private long ConvertToEpoch(DateTime? date)
    {
        if (!date.HasValue)
            return _timeProvider.GetUtcNow().ToUnixTimeSeconds();
        
        // Convert to configured timezone
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timezone);
        var dateTime = TimeZoneInfo.ConvertTimeToUtc(date.Value, timeZone);
        
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
    
    private static List<string> NormalizeTags(List<string> tags)
    {
        return tags
            .Select(t => t.ToLowerInvariant().Replace(' ', '-'))
            .Distinct()
            .ToList();
    }
}
```

**See [src/TechHub.Core/AGENTS.md](../TechHub.Core/AGENTS.md#markdown-frontmatter-mapping)** for complete frontmatter field mappings.

## Caching Strategy

### Memory Cache Pattern

```csharp
// Check cache first
if (_cache.TryGetValue(cacheKey, out T? cached))
{
    _logger.LogDebug("Cache hit for key: {CacheKey}", cacheKey);
    return cached!;
}

// Load from source
_logger.LogDebug("Cache miss for key: {CacheKey}", cacheKey);
var data = await LoadDataAsync(ct);

// Store in cache with expiration
_cache.Set(cacheKey, data, TimeSpan.FromMinutes(30));

return data;
```

### Cache Invalidation

```csharp
public void InvalidateCache()
{
    _cache.Remove(AllContentCacheKey);
    _logger.LogInformation("Content cache invalidated");
}
```

**When to invalidate**:

- Content files change (file watcher, manual trigger)
- Configuration updates
- Administrative actions

## Service Lifetimes

**Register in Program.cs**:

```csharp
// Singleton - stateless, cache internally
builder.Services.AddSingleton<ISectionRepository, ConfigurationBasedSectionRepository>();
builder.Services.AddSingleton<IContentRepository, FileBasedContentRepository>();
builder.Services.AddSingleton<IMarkdownService, MarkdownService>();

// Scoped - per-request generation
builder.Services.AddScoped<IRssService, RssService>();

// Built-in services
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(TimeProvider.System);
```

## Error Handling

**Always log errors and handle gracefully**:

```csharp
try
{
    var content = await _markdownService.ProcessMarkdownFileAsync(filePath, collection, ct);
    if (content is not null)
    {
        items.Add(content);
    }
}
catch (IOException ex)
{
    _logger.LogError(ex, "I/O error reading file: {FilePath}", filePath);
    // Continue processing other files
}
catch (YamlException ex)
{
    _logger.LogError(ex, "Invalid YAML frontmatter in file: {FilePath}", filePath);
    // Continue processing other files
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error processing file: {FilePath}", filePath);
    // Continue processing other files
}
```

**Principle**: One bad file should not crash the entire system. Log the error and continue.

## Testing

**See [tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md)** for comprehensive testing patterns including:

- Repository testing with file system mocks
- Markdown parsing test cases
- Cache behavior verification
- Error handling scenarios

## Related Documentation

- **[src/AGENTS.md](../AGENTS.md)** - Shared .NET patterns and code quality standards
- **[src/TechHub.Core/AGENTS.md](../TechHub.Core/AGENTS.md)** - Domain models and interfaces
- **[src/TechHub.Api/AGENTS.md](../TechHub.Api/AGENTS.md)** - API endpoints that use these repositories
- **[tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md)** - Infrastructure testing patterns
- **[Root AGENTS.md](../../AGENTS.md)** - Complete workflow and principles

---

**Remember**: Infrastructure layer handles the "how" - file I/O, caching, external services. Keep it separate from domain logic.
