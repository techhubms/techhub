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

**Key Pattern**: Load sections from `appsettings.json` in constructor, cache in readonly field, return Task.FromResult.

**Implementation**: `ConfigurationBasedSectionRepository`

**Important Details**:

- Uses `IOptions<AppSettings>` to access configuration
- Converts `SectionConfig` to domain `Section` models in constructor
- No file I/O - all data loaded from configuration at startup
- Thread-safe via readonly collections
- Supports URL matching with and without leading slash

### File-Based Repository with Caching

**Key Pattern**: Check cache → Load from disk → Sort by date → Cache result → Return.

**Implementation**: `FileBasedContentRepository`

**Important Details**:

- **Cache expiration**: 30 minutes (configurable via `TimeSpan`)
- **Cache key**: `"all_content"` (constant string)
- **Sorting**: CRITICAL - All methods return content sorted by `DateEpoch` descending (newest first)
- **Error handling**: Log errors, continue processing other files (one bad file doesn't crash system)
- **Discovery**: Scans `collections/` for directories matching `_*` pattern
- **Filtering**: `GetBySectionAsync` filters cached content (no separate cache per section)

## Markdown Processing

### Frontmatter Parsing

**Key Pattern**: Extract YAML between `---` delimiters → Parse with YamlDotNet → Return metadata + content.

**Implementation**: `FrontMatterParser` (static Parse method)

**Important Details**:

- Uses `UnderscoredNamingConvention` to map `snake_case` YAML to `PascalCase` C# properties
- Returns empty `FrontMatter` object if no frontmatter found (graceful degradation)
- See [src/TechHub.Core/AGENTS.md](../TechHub.Core/AGENTS.md#markdown-frontmatter-mapping) for complete field mappings

### Markdown to HTML Conversion

**Key Pattern**: Read file → Parse frontmatter → Extract excerpt → Convert to HTML → Map to ContentItem.

**Implementation**: `MarkdownService`

**Important Details**:

- **Markdig pipeline**: Uses `UseAdvancedExtensions()`, `UseAutoIdentifiers()`, `UseEmojiAndSmiley()`, `UsePipeTables()`
- **Excerpt extraction**: Content before `<!--excerpt_end-->` marker, or first 200 words if no marker
- **Slug generation**: Derived from filename without extension
- **Date handling**: Converts dates to Unix epoch using configured timezone (`Europe/Brussels`)
- **Tag normalization**: Lowercase + replace spaces with hyphens + deduplicate
- **Error handling**: Returns null on error, logs exception details

### Tag Cloud Service

**Key Pattern**: Filter content by scope → Count tags → Filter by min usage → Take top N → Assign quantile sizes.

**Implementation**: `TagCloudService`

**Quantile Size Algorithm**:

- **Top 25%** (index 0-24%): `TagSize.Large`
- **Middle 50%** (index 25-74%): `TagSize.Medium`
- **Bottom 25%** (index 75-100%): `TagSize.Small`

**Important Details**:

- **Scoping**: Homepage (all), Section, Collection, or Content (section + collection)
- **Date filtering**: Optional `lastDays` parameter
- **Usage filtering**: `MinimumTagUses` (default: 5)
- **Top-N limiting**: `DefaultMaxTags` (default: 20)
- **Sorting**: By count descending, then alphabetically

**Why Quantiles?**:

- Consistent visual distribution (not all same size)
- Scalable (works with 5 or 500 tags)
- Predictable size distinctions

### Tag Matching Service

**Key Pattern**: Word boundary regex matching with OR logic between selected tags.

**Implementation**: `TagMatchingService`

**Matching Logic**:

- **Exact match**: `"AI"` == `"AI"`
- **Word boundary match**: Regex pattern `\b{escaped}\b` prevents partial matches
- **Examples**: `"AI"` matches `"Generative AI"` but NOT `"AIR"`

**Important Details**:

- **Normalization**: Trim + lowercase for case-insensitive matching
- **MatchesAny** (OR logic): ANY selected tag matches ANY item tag
- **Empty behavior**: Empty selected = show all, empty item = no match

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
builder.Services.AddSingleton<ITagMatchingService, TagMatchingService>();
builder.Services.AddSingleton<ITagCloudService, TagCloudService>();

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
