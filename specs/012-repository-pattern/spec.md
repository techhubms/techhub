# Repository Pattern Specification

> **Feature**: Repository pattern for data access with file-based implementation and caching

## Overview

The repository pattern provides an abstraction layer between domain models and data access logic. The initial implementation uses file-based storage (sections.json and markdown files in collections/) with in-memory caching for performance. The design is database-ready, allowing future migration to SQL Server, Cosmos DB, or other storage without changing business logic.

## Requirements

### Functional Requirements

**FR-1**: The system MUST provide repository interfaces for sections and content  
**FR-2**: The system MUST implement file-based repositories reading from sections.json and collections/  
**FR-3**: The system MUST support querying content by collection, category, and ID  
**FR-4**: The system MUST support filtering content by category (multi-location access)  
**FR-5**: The system MUST cache parsed data in memory for performance  
**FR-6**: The system MUST reload data when files change (development mode)  
**FR-7**: The system MUST validate section/category relationships  
**FR-8**: The system MUST sort content by date descending by default  
**FR-9**: The system MUST limit result sets to prevent memory issues  
**FR-10**: The system MUST handle concurrent reads safely  

### Non-Functional Requirements

**NFR-1**: Repository queries MUST complete in < 10ms (p95, cached)  
**NFR-2**: Cache hit rate MUST exceed 95% in production  
**NFR-3**: Repositories MUST be thread-safe  
**NFR-4**: Repository interfaces MUST support async/await  
**NFR-5**: Repository implementations MUST use CancellationToken  
**NFR-6**: File I/O MUST be asynchronous  
**NFR-7**: Memory cache MUST use sliding expiration (1 hour)  

## Repository Interfaces

### ISectionRepository

Provides access to section configuration data.

**Methods**:

```csharp
public interface ISectionRepository
{
    /// <summary>
    /// Get all sections defined in sections.json
    /// </summary>
    Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Get section by unique identifier
    /// </summary>
    Task<Section?> GetSectionByIdAsync(string id, CancellationToken ct = default);
    
    /// <summary>
    /// Get section by URL slug (e.g., "github-copilot")
    /// </summary>
    Task<Section?> GetSectionByUrlAsync(string url, CancellationToken ct = default);
    
    /// <summary>
    /// Check if a section contains content with the given category.
    /// Used for validating multi-location content URLs.
    /// </summary>
    Task<bool> SectionContainsCategoryAsync(
        string sectionUrl, 
        string category, 
        CancellationToken ct = default);
}
```

### IContentRepository

Provides access to content items across all collections.

**Methods**:

```csharp
public interface IContentRepository
{
    /// <summary>
    /// Get all items in a specific collection (news, blogs, videos, etc.)
    /// Sorted by date descending
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetItemsByCollectionAsync(
        string collection, 
        CancellationToken ct = default);
    
    /// <summary>
    /// Get all items with a specific category (ai, github-copilot, etc.)
    /// Supports multi-location content access
    /// Sorted by date descending
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetItemsByCategoryAsync(
        string category, 
        CancellationToken ct = default);
    
    /// <summary>
    /// Get content by ID, optionally filtered to a specific category.
    /// Returns null if item not found or doesn't have requested category.
    /// Supports multi-location access: same content viewable from different sections.
    /// </summary>
    Task<ContentItem?> GetItemByIdAsync(
        string id, 
        string? categoryFilter = null,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get most recent items, optionally filtered by category
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetLatestItemsAsync(
        int count, 
        string? category = null,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get items by collection within a specific section context.
    /// Filters to items that have the section's category.
    /// Example: Get all videos in the "ai" section
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetItemsByCollectionInSectionAsync(
        string collection,
        string sectionCategory,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get all items across all collections (for "everything" feed)
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetAllItemsAsync(CancellationToken ct = default);
}
```

## Implementation Details

### FileSectionRepository

```csharp
// TechHub.Infrastructure/Repositories/FileSectionRepository.cs
namespace TechHub.Infrastructure.Repositories;

public class FileSectionRepository : ISectionRepository
{
    private readonly string _sectionsJsonPath;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FileSectionRepository> _logger;
    
    private const string CacheKey = "sections_all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);
    
    public FileSectionRepository(
        IOptions<ContentOptions> options,
        IMemoryCache cache,
        ILogger<FileSectionRepository> logger)
    {
        _sectionsJsonPath = options.Value.SectionsJsonPath;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<IReadOnlyList<Section>> GetAllSectionsAsync(
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue<IReadOnlyList<Section>>(CacheKey, out var cached))
        {
            _logger.LogDebug("Cache hit for sections");
            return cached!;
        }
        
        _logger.LogInformation("Loading sections from {Path}", _sectionsJsonPath);
        
        var json = await File.ReadAllTextAsync(_sectionsJsonPath, ct);
        var sections = JsonSerializer.Deserialize<List<Section>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Failed to parse sections.json");
        
        _cache.Set(CacheKey, sections, new MemoryCacheEntryOptions
        {
            SlidingExpiration = CacheDuration
        });
        
        _logger.LogInformation("Loaded {Count} sections", sections.Count);
        
        return sections;
    }
    
    public async Task<Section?> GetSectionByUrlAsync(
        string url, 
        CancellationToken ct = default)
    {
        var sections = await GetAllSectionsAsync(ct);
        return sections.FirstOrDefault(s => 
            s.Url.Equals(url, StringComparison.OrdinalIgnoreCase));
    }
    
    public async Task<bool> SectionContainsCategoryAsync(
        string sectionUrl,
        string category,
        CancellationToken ct = default)
    {
        var section = await GetSectionByUrlAsync(sectionUrl, ct);
        return section is not null && 
               section.Category.Equals(category, StringComparison.OrdinalIgnoreCase);
    }
}
```

### FileContentRepository

```csharp
// TechHub.Infrastructure/Repositories/FileContentRepository.cs
namespace TechHub.Infrastructure.Repositories;

public class FileContentRepository : IContentRepository
{
    private readonly string _collectionsPath;
    private readonly IMarkdownProcessor _markdownProcessor;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FileContentRepository> _logger;
    
    private const string CacheKeyAll = "content_all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);
    
    // Collection directory names
    private static readonly string[] CollectionDirectories = 
    {
        "_news", "_blogs", "_videos", "_community", "_roundups"
    };
    
    public FileContentRepository(
        IOptions<ContentOptions> options,
        IMarkdownProcessor markdownProcessor,
        IMemoryCache cache,
        ILogger<FileContentRepository> logger)
    {
        _collectionsPath = options.Value.CollectionsPath;
        _markdownProcessor = markdownProcessor;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<IReadOnlyList<ContentItem>> GetAllItemsAsync(
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue<IReadOnlyList<ContentItem>>(CacheKeyAll, out var cached))
        {
            _logger.LogDebug("Cache hit for all content");
            return cached!;
        }
        
        _logger.LogInformation("Loading all content from {Path}", _collectionsPath);
        
        var allItems = new List<ContentItem>();
        
        foreach (var collectionDir in CollectionDirectories)
        {
            var dirPath = Path.Combine(_collectionsPath, collectionDir);
            if (!Directory.Exists(dirPath))
            {
                _logger.LogWarning("Collection directory not found: {Dir}", dirPath);
                continue;
            }
            
            var items = await LoadCollectionAsync(collectionDir, ct);
            allItems.AddRange(items);
        }
        
        // Sort by date descending
        var sortedItems = allItems
            .OrderByDescending(i => i.DateEpoch)
            .ToList();
        
        _cache.Set(CacheKeyAll, sortedItems, new MemoryCacheEntryOptions
        {
            SlidingExpiration = CacheDuration
        });
        
        _logger.LogInformation("Loaded {Count} total content items", sortedItems.Count);
        
        return sortedItems;
    }
    
    private async Task<List<ContentItem>> LoadCollectionAsync(
        string collectionDir,
        CancellationToken ct)
    {
        var items = new List<ContentItem>();
        var dirPath = Path.Combine(_collectionsPath, collectionDir);
        var files = Directory.GetFiles(dirPath, "*.md", SearchOption.AllDirectories);
        
        foreach (var file in files)
        {
            try
            {
                var item = await _markdownProcessor.ParseMarkdownFileAsync(file, ct);
                items.Add(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse markdown file: {File}", file);
            }
        }
        
        return items;
    }
    
    public async Task<IReadOnlyList<ContentItem>> GetItemsByCategoryAsync(
        string category,
        CancellationToken ct = default)
    {
        var allItems = await GetAllItemsAsync(ct);
        return allItems
            .Where(i => i.Categories.Contains(category, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }
    
    public async Task<ContentItem?> GetItemByIdAsync(
        string id,
        string? categoryFilter = null,
        CancellationToken ct = default)
    {
        var allItems = await GetAllItemsAsync(ct);
        var item = allItems.FirstOrDefault(i => 
            i.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        
        if (item is null) return null;
        
        // If category filter specified, verify item has that category
        if (categoryFilter is not null && 
            !item.Categories.Contains(categoryFilter, StringComparer.OrdinalIgnoreCase))
        {
            return null;
        }
        
        return item;
    }
    
    public async Task<IReadOnlyList<ContentItem>> GetItemsByCollectionInSectionAsync(
        string collection,
        string sectionCategory,
        CancellationToken ct = default)
    {
        var allItems = await GetAllItemsAsync(ct);
        return allItems
            .Where(i => i.Collection.Equals(collection, StringComparison.OrdinalIgnoreCase))
            .Where(i => i.Categories.Contains(sectionCategory, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }
}
```

## Configuration

```csharp
// TechHub.Core/Options/ContentOptions.cs
namespace TechHub.Core.Options;

public class ContentOptions
{
    /// <summary>
    /// Path to sections.json file (relative or absolute)
    /// </summary>
    public required string SectionsJsonPath { get; init; }
    
    /// <summary>
    /// Path to collections root directory
    /// </summary>
    public required string CollectionsPath { get; init; }
}
```

```json
// appsettings.json
{
  "Content": {
    "SectionsJsonPath": "../_data/sections.json",
    "CollectionsPath": "../collections"
  }
}
```

## Dependency Injection

```csharp
// Program.cs
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ISectionRepository, FileSectionRepository>();
builder.Services.AddSingleton<IContentRepository, FileContentRepository>();
```

## Use Cases

### UC-1: Load All Sections on Startup

**Flow**:
1. Application starts
2. First request triggers `GetAllSectionsAsync`
3. Repository checks cache (miss on first call)
4. Repository reads sections.json
5. Repository deserializes JSON to Section objects
6. Repository stores in cache with 1-hour expiration
7. Repository returns sections
8. Subsequent requests use cached data

### UC-2: Get Content for Section

**Flow**:
1. API receives request: `/api/content/section/ai`
2. Controller calls `GetSectionByUrlAsync("ai")`
3. Controller gets section category: "ai"
4. Controller calls `GetItemsByCategoryAsync("ai")`
5. Repository filters all items to those with "ai" category
6. Repository returns filtered items
7. Controller converts to DTOs with AI context

### UC-3: Validate Multi-Location Access

**Flow**:
1. User navigates to `/github-copilot/videos/vs-code-107.html`
2. API calls `GetItemByIdAsync("vs-code-107", "github-copilot")`
3. Repository finds item with ID
4. Repository checks if item.Categories contains "github-copilot"
5. If yes, returns item; if no, returns null
6. API returns 404 if null

## Testing Strategy

### Unit Tests

```csharp
public class FileSectionRepositoryTests
{
    [Fact]
    public async Task GetAllSectionsAsync_OnFirstCall_LoadsFromFile()
    {
        // Arrange
        var options = Options.Create(new ContentOptions
        {
            SectionsJsonPath = "test-sections.json"
        });
        var cache = new MemoryCache(new MemoryCacheOptions());
        var logger = NullLogger<FileSectionRepository>.Instance;
        
        var repo = new FileSectionRepository(options, cache, logger);
        
        // Act
        var sections = await repo.GetAllSectionsAsync();
        
        // Assert
        Assert.NotEmpty(sections);
    }
    
    [Fact]
    public async Task GetAllSectionsAsync_OnSecondCall_UsesCachedData()
    {
        // Verify cache is used on subsequent calls
    }
}
```

### Integration Tests

```csharp
public class ContentRepositoryIntegrationTests : IClassFixture<TestFixture>
{
    [Fact]
    public async Task GetItemByIdAsync_WithValidCategory_ReturnsItem()
    {
        // Arrange
        var repo = _fixture.ContentRepository;
        
        // Act
        var item = await repo.GetItemByIdAsync("test-item", "ai");
        
        // Assert
        Assert.NotNull(item);
        Assert.Contains("ai", item.Categories);
    }
    
    [Fact]
    public async Task GetItemsByCollectionInSectionAsync_FiltersCorrectly()
    {
        var repo = _fixture.ContentRepository;
        
        var items = await repo.GetItemsByCollectionInSectionAsync("videos", "ai");
        
        Assert.All(items, i => 
        {
            Assert.Equal("videos", i.Collection);
            Assert.Contains("ai", i.Categories);
        });
    }
}
```

## Performance Considerations

**Caching Strategy**:
- All data loaded into memory on first access
- Sliding expiration (1 hour) prevents stale data
- Cache eviction triggers reload on next access

**Memory Usage**:
- Typical site: ~1000 items × ~10KB = ~10MB
- Acceptable for in-memory caching
- Future: Add pagination for large datasets

**File Watching** (Development):
```csharp
// Optional: Watch for file changes in development
builder.Services.AddSingleton<IFileWatcher, FileWatcher>();
// On file change, evict cache to force reload
```

## Future Database Migration

The repository pattern allows seamless migration to database:

```csharp
// Future: TechHub.Infrastructure/Repositories/SqlContentRepository.cs
public class SqlContentRepository : IContentRepository
{
    private readonly DbContext _db;
    
    public async Task<IReadOnlyList<ContentItem>> GetAllItemsAsync(CancellationToken ct)
    {
        return await _db.ContentItems
            .OrderByDescending(i => i.DateEpoch)
            .ToListAsync(ct);
    }
    
    // Same interface, different implementation
}
```

## References

- `/specs/006-domain-models/spec.md` - Domain model definitions
- `/specs/.speckit/constitution.md` - Architecture principles
- `/AGENTS.md` - Development guidelines
