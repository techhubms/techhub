# TechHub.Core Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Core/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).
> **RULE**: All rules from parent AGENTS.md files apply. This file adds Core layer-specific patterns.

## Overview

This project contains domain models, DTOs, and interfaces with **zero external dependencies** (pure .NET). It defines the core business entities and contracts that all other layers depend on.

**When to read this file**: When creating or modifying domain models, DTOs, repository interfaces, or understanding the core domain.

**Testing this code**: See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md) for unit testing patterns.

## Project Structure

```text
TechHub.Core/
├── Models/                       # Domain entities
│   ├── Section.cs               # Section aggregate
│   ├── ContentItem.cs           # Content item entity
│   ├── Collection.cs            # Collection reference
│   └── FrontMatter.cs           # Markdown frontmatter
├── DTOs/                         # Data transfer objects
│   ├── SectionDto.cs            # Section DTO
│   ├── ContentItemDto.cs        # Content item DTO
│   └── CollectionDto.cs         # Collection DTO
├── Interfaces/                   # Repository contracts
│   ├── ISectionRepository.cs    # Section data access
│   ├── IContentRepository.cs    # Content data access
│   ├── IMarkdownService.cs      # Markdown processing
│   └── IRssService.cs           # RSS feed generation
└── TechHub.Core.csproj          # Project file (no dependencies!)
```

## Core Principles

### No External Dependencies

**CRITICAL**: TechHub.Core must remain framework-agnostic:

- ✅ **Use**: `System.*` namespaces only
- ✅ **Use**: Standard .NET types (string, int, DateTime, etc.)
- ✅ **Use**: Collections (`IReadOnlyList<T>`, `List<T>`, `Dictionary<K,V>`)
- 🚫 **NEVER**: Add NuGet packages or framework dependencies
- 🚫 **NEVER**: Reference ASP.NET Core, Entity Framework, or other frameworks

**Why**: Keeps domain logic clean, testable, and reusable across any application type.

### Immutability by Default

**Use `init` accessors and `required` properties**:

```csharp
public class Section
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<Collection> Collections { get; init; }
}
```

**Benefits**:

- Thread-safe by design
- Prevents accidental mutations
- Clear intent (values set once at creation)
- Works perfectly with record types

## Domain Model Patterns

### Entity Design

**Use classes for entities with identity**:

```csharp
namespace TechHub.Core.Models;

/// <summary>
/// Represents a thematic section grouping related content.
/// </summary>
public class Section
{
    /// <summary>
    /// Unique identifier (URL-friendly, e.g., "ai", "github-copilot").
    /// </summary>
    public required string Id { get; init; }
    
    /// <summary>
    /// Display title shown in UI.
    /// </summary>
    public required string Title { get; init; }
    
    /// <summary>
    /// Brief description of section purpose.
    /// </summary>
    public required string Description { get; init; }
    
    /// <summary>
    /// URL path for section (e.g., "/ai").
    /// </summary>
    public required string Url { get; init; }
    
    /// <summary>
    /// Background image path (e.g., "/images/section-backgrounds/ai.jpg").
    /// </summary>
    public required string BackgroundImage { get; init; }
    
    /// <summary>
    /// Collections within this section.
    /// </summary>
    public required IReadOnlyList<Collection> Collections { get; init; }
    
    /// <summary>
    /// Validates that all required properties are correctly formatted.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Id))
            throw new ArgumentException("Section ID cannot be empty", nameof(Id));
        
        if (!Id.All(c => char.IsLower(c) || c == '-'))
            throw new ArgumentException("Section ID must be lowercase with hyphens only", nameof(Id));
        
        if (!Url.StartsWith('/'))
            throw new ArgumentException("Section URL must start with '/'", nameof(Url));
        
        if (Collections.Count == 0)
            throw new ArgumentException("Section must have at least one collection", nameof(Collections));
    }
}
```

### Value Object Pattern

**Use records for value objects without identity**:

```csharp
namespace TechHub.Core.Models;

/// <summary>
/// Represents a collection reference within a section.
/// </summary>
public record Collection
{
    public required string Title { get; init; }
    public required string CollectionName { get; init; }
    public required string Url { get; init; }
    public required string Description { get; init; }
    public required bool IsCustom { get; init; }
}
```

**Benefits of records**:

- Value-based equality (two instances with same values are equal)
- Immutable by default
- Concise syntax with positional records
- Built-in `ToString()`, `GetHashCode()`, `Equals()`

### Content Item Model

**Complex domain entity with many properties**:

```csharp
namespace TechHub.Core.Models;

/// <summary>
/// Represents a content item (news, blog, video, community post).
/// </summary>
public class ContentItem
{
    /// <summary>
    /// URL-friendly slug derived from filename (e.g., "2025-01-15-article-title").
    /// </summary>
    public required string Slug { get; init; }
    
    /// <summary>
    /// Title of the content item.
    /// </summary>
    public required string Title { get; init; }
    
    /// <summary>
    /// Author name.
    /// </summary>
    public string? Author { get; init; }
    
    /// <summary>
    /// Publication date as Unix epoch timestamp (seconds since 1970-01-01 UTC).
    /// </summary>
    public required long DateEpoch { get; init; }
    
    /// <summary>
    /// Collection name (e.g., "news", "blogs", "videos").
    /// </summary>
    public required string CollectionName { get; init; }
    
    /// <summary>
    /// Section names (lowercase identifiers) this content belongs to (e.g., ["ai", "github-copilot"]).
    /// Mapped from frontmatter 'categories' field (which contains Section Titles like "AI", "GitHub Copilot").
    /// </summary>
    public required IReadOnlyList<string> SectionNames { get; init; }
    
    /// <summary>
    /// Tags (content-specific keywords, normalized to lowercase).
    /// </summary>
    public required IReadOnlyList<string> Tags { get; init; }
    
    /// <summary>
    /// Short excerpt (max 200 words, plain text).
    /// </summary>
    public string Excerpt { get; init; } = string.Empty;
    
    /// <summary>
    /// Full content rendered as HTML from markdown.
    /// </summary>
    public string RenderedHtml { get; init; } = string.Empty;
    
    /// <summary>
    /// External URL (original source, from canonical_url frontmatter).
    /// </summary>
    public string? ExternalUrl { get; init; }
    
    /// <summary>
    /// Viewing mode: "internal" (show on site) or "external" (link to source).
    /// </summary>
    public string ViewingMode { get; init; } = "external";
    
    /// <summary>
    /// YouTube video ID (for video content, from video_id frontmatter).
    /// </summary>
    public string? VideoId { get; init; }
    
    /// <summary>
    /// Alternative collection name for subfolder organization.
    /// </summary>
    public string? AltCollection { get; init; }
}
```

## Markdown Frontmatter Mapping

**Critical reference** for understanding how markdown frontmatter maps to domain properties:

**Markdown File Example**:

```markdown
---
title: "Getting Started with GitHub Copilot"
author: "John Doe"
date: 2026-01-07
categories: [ai, github-copilot]
tags: [machine-learning, productivity, vscode]
canonical_url: "https://example.com/original-article"
viewing_mode: "external"
video_id: "dQw4w9WgXcQ"
alt_collection: "ghc-features"
---

This is the excerpt that appears in content lists and RSS feeds.

<!--excerpt_end-->

# Full Article Content

The rest of the markdown content...
```

**Property Mappings**:

| Frontmatter Field           | Domain Property  | Type                    | Notes                                                                                   |
| --------------------------- | ---------------- | ----------------------- | --------------------------------------------------------------------------------------- |
| `title`                     | `Title`          | `string`                | Required                                                                                |
| `author`                    | `Author`         | `string?`               | Optional                                                                                |
| `date`                      | `DateEpoch`      | `long`                  | Converted to Unix timestamp in Europe/Brussels timezone                                 |
| `categories`                | `SectionNames`   | `IReadOnlyList<string>` | Frontmatter contains Section Titles ("AI"), mapped to lowercase section names ("ai")    |
| `tags`                      | `Tags`           | `IReadOnlyList<string>` | Normalized to lowercase, hyphen-separated                                               |
| `canonical_url`             | `ExternalUrl`    | `string?`               | Original source URL                                                                     |
| `viewing_mode`              | `ViewingMode`    | `string`                | "internal" or "external" (default: "external")                                          |
| `video_id`                  | `VideoId`        | `string?`               | YouTube video identifier                                                                |
| `alt_collection`            | `AltCollection`  | `string?`               | Subfolder categorization                                                                |
| Filename                    | `Slug`           | `string`                | `2025-01-15-article.md` → `2025-01-15-article`                                          |
| Before `<!--excerpt_end-->` | `Excerpt`        | `string`                | Plain text, max 200 words                                                               |
| Full markdown               | `RenderedHtml`   | `string`                | Processed with Markdig                                                                  |

**See [src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md)** for implementation details of frontmatter parsing.

## DTO Patterns

**Use DTOs for API responses and client communication**:

```csharp
namespace TechHub.Core.DTOs;

/// <summary>
/// Data transfer object for section API responses.
/// </summary>
public record SectionDto
{
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required string BackgroundImage { get; init; }
    public required IReadOnlyList<CollectionReferenceDto> Collections { get; init; }
}

/// <summary>
/// Data transfer object for content item API responses.
/// </summary>
public record ContentItemDto
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public string? Author { get; init; }
    public required long DateEpoch { get; init; }
    public required string CollectionName { get; init; }
    public required IReadOnlyList<string> SectionNames { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }
    public required string Excerpt { get; init; }
    public string? ExternalUrl { get; init; }
    public string ViewingMode { get; init; } = "external";
    public string? VideoId { get; init; }
}
```

**DTO vs Domain Model**:

- **Domain Model**: Rich behavior, validation, business rules
- **DTO**: Data transfer only, no behavior, optimized for serialization
- **Mapping**: Use extension methods or mapping libraries (AutoMapper, Mapster)

## Repository Interfaces

**Define contracts for data access**:

```csharp
namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for section data access.
/// </summary>
public interface ISectionRepository
{
    /// <summary>
    /// Initializes the repository (loads data from configuration/files).
    /// </summary>
    Task<IReadOnlyList<Section>> InitializeAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Gets all sections.
    /// </summary>
    Task<IReadOnlyList<Section>> GetAllAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Gets a section by its URL.
    /// </summary>
    Task<Section?> GetByUrlAsync(string sectionUrl, CancellationToken ct = default);
}

/// <summary>
/// Repository for content item data access.
/// </summary>
public interface IContentRepository
{
    /// <summary>
    /// Gets all content items across all collections, sorted by date (newest first).
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Gets content items for a specific section, sorted by date (newest first).
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetBySectionAsync(string sectionUrl, CancellationToken ct = default);
    
    /// <summary>
    /// Gets a single content item by its slug.
    /// </summary>
    Task<ContentItem?> GetBySlugAsync(string slug, CancellationToken ct = default);
}
```

**CRITICAL Repository Contract**:

- All `IContentRepository` methods **MUST** return content sorted by `DateEpoch` descending (newest first)
- This sorting happens at repository layer, before caching
- Clients should never need to sort content themselves

## URL Generation Methods

**ContentItem methods for URL generation**:

```csharp
namespace TechHub.Core.Models;

public class ContentItem
{
    // ... properties ...
    
    /// <summary>
    /// Generates the URL for this content item within a specific section.
    /// </summary>
    /// <param name="sectionName">The section URL (e.g., "github-copilot").</param>
    /// <returns>Full URL path (e.g., "/github-copilot/blogs/2024-01-15-article").</returns>
    public string GetUrlInSection(string sectionName)
    {
        if (!SectionNames.Contains(sectionName))
            throw new ArgumentException($"Content not in section '{sectionName}'", nameof(sectionName));
        
        return $"/{sectionName}/{Collection}/{Slug}";
    }
}
```

## Unix Epoch Timestamp Usage

**Always use Unix epoch for date storage and manipulation**:

**Storage**:

```csharp
public required long DateEpoch { get; init; }  // Seconds since Unix epoch
```

**Conversion from DateTime**:

```csharp
var date = new DateTime(2026, 1, 7, 10, 30, 0, DateTimeKind.Utc);
long epochSeconds = new DateTimeOffset(date).ToUnixTimeSeconds();
```

**Conversion from frontmatter string** (see Infrastructure/AGENTS.md):

```csharp
// Frontmatter: "date: 2026-01-07" or "date: 2026-01-07 14:30:00 +0100"
var dateString = "2026-01-07";
var date = DateTime.Parse(dateString);
var brusselsTime = TimeZoneInfo.ConvertTimeToUtc(date, 
    TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels"));
long epochSeconds = new DateTimeOffset(brusselsTime).ToUnixTimeSeconds();
```

**Conversion to DateTime**:

```csharp
var dateTime = DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch).DateTime;
```

**Why Unix Epoch?**:

- Timezone-agnostic storage (always UTC internally)
- Efficient comparison and sorting
- Compact representation (single long value)
- Standard format across all platforms

## DTO Conversion Extensions

**Pattern**: Use extension methods for converting domain models to DTOs

```csharp
namespace TechHub.Core.Extensions;

public static class ContentItemExtensions
{
    public static ContentItemDto ToDto(this ContentItem item)
    {
        return new ContentItemDto
        {
            Slug = item.Slug,
            Title = item.Title,
            Author = item.Author,
            Description = item.Description,
            Date = item.DateEpoch,
            SectionNames = item.SectionNames.ToList(),
            PrimarySectionName = item.SectionNames.FirstOrDefault() ?? "",
            Collection = item.Collection,
            Tags = item.Tags.ToList(),
            Excerpt = item.Excerpt,
            RenderedHtml = item.RenderedHtml,
            ExternalUrl = item.ExternalUrl,
            ViewingMode = item.ViewingMode,
            VideoId = item.VideoId
        };
    }
    
    public static IEnumerable<ContentItemDto> ToDtos(this IEnumerable<ContentItem> items)
    {
        return items.Select(i => i.ToDto());
    }
}
```

**Usage in repositories**:

```csharp
public async Task<ContentItemDto?> GetBySlugAsync(string slug, CancellationToken ct)
{
    var item = await _repository.GetBySlugAsync(slug, ct);
    return item?.ToDto();
}
```

**Usage in API endpoints**:

```csharp
public async Task<IResult> GetContent(string slug, IContentRepository repo)
{
    var item = await repo.GetBySlugAsync(slug);
    return item != null 
        ? Results.Ok(item.ToDto()) 
        : Results.NotFound();
}
```

## Validation Patterns

**Add validation methods to domain models**:

```csharp
public class ContentItem
{
    // ... properties ...
    
    /// <summary>
    /// Validates that all required properties are correctly formatted.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Slug))
            throw new ArgumentException("Slug cannot be empty", nameof(Slug));
        
        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("Title cannot be empty", nameof(Title));
        
        if (DateEpoch <= 0)
            throw new ArgumentException("DateEpoch must be positive", nameof(DateEpoch));
        
        if (SectionNames.Count == 0)
            throw new ArgumentException("Must have at least one section", nameof(SectionNames));
        
        if (ViewingMode is not "internal" and not "external")
            throw new ArgumentException("ViewingMode must be 'internal' or 'external'", nameof(ViewingMode));
    }
}
```

## Testing

**See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md)** for comprehensive unit testing patterns for domain models and business logic.

## Related Documentation

- **[src/AGENTS.md](../AGENTS.md)** - Shared .NET patterns and code quality standards
- **[src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md)** - Repository implementations that use these interfaces
- **[src/TechHub.Api/AGENTS.md](../TechHub.Api/AGENTS.md)** - API endpoints that consume these models
- **[tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md)** - Unit testing patterns
- **[Root AGENTS.md](../../AGENTS.md)** - Complete workflow and principles

---

**Remember**: Core layer stays clean and framework-agnostic. No dependencies, just pure domain logic.
