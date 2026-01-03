# Data Model: Tech Hub .NET Migration

**Generated**: 2026-01-02  
**Source**: Feature specification requirements (FR-031 through FR-035)

## Overview

This document defines all domain entities, DTOs, and data structures for the Tech Hub .NET application. The model follows the repository pattern with clear separation between domain entities (internal) and DTOs (API contracts).

---

## Domain Entities (TechHub.Core/Models)

### Section

Represents a top-level organizational unit grouping related content by topic.

```csharp
public class Section
{
    public required string Id { get; init; }                    // e.g., "ai", "github-copilot"
    public required string Title { get; init; }                 // e.g., "AI"
    public required string Description { get; init; }           // Brief description
    public required string Url { get; init; }                   // e.g., "/ai"
    public required string Category { get; init; }              // e.g., "ai"
    public required string BackgroundImage { get; init; }       // Path to section header image
    public required IReadOnlyList<CollectionReference> Collections { get; init; }
}
```

**Validation Rules**:

- `Id` must be lowercase with hyphens only
- `Url` must start with `/` and match `Id`
- `Category` must match valid categories from sections.json
- `Collections` must not be empty

---

### CollectionReference

Reference to a content collection within a section.

```csharp
public class CollectionReference
{
    public required string Title { get; init; }                 // e.g., "Latest News"
    public required string Collection { get; init; }            // e.g., "news"
    public required string Url { get; init; }                   // e.g., "/ai/news"
    public required string Description { get; init; }           // Brief description
    public bool IsCustom { get; init; }                         // true if manually created
}
```

**Validation Rules**:

- `Collection` must match a valid collection directory (_news, _blogs, _videos, _community, _roundups)
- `Url` must be valid URL path
- `IsCustom` defaults to false

---

### ContentItem

Represents individual content (article, blog, video, etc.) with metadata and rendered content.

```csharp
public class ContentItem
{
    public required string Id { get; init; }                    // Slug from filename
    public required string Title { get; init; }                 // Article title
    public required string Description { get; init; }           // Brief summary
    public string? Author { get; init; }                        // Optional author
    public required long DateEpoch { get; init; }               // Unix timestamp
    public required string Collection { get; init; }            // e.g., "news", "blogs"
    public string? AltCollection { get; init; }                 // Optional alt-collection for subfolders
    public required IReadOnlyList<string> Categories { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }   // Normalized tags
    public required string RenderedHtml { get; init; }          // Full markdown rendered to HTML
    public required string Excerpt { get; init; }               // Content before <!--excerpt_end-->
    public string? ExternalUrl { get; init; }                   // Optional external link
    public string? VideoId { get; init; }                       // Optional YouTube video ID
    
    // Computed properties
    public DateTime DateUtc => DateTimeOffset.FromUnixTimeSeconds(DateEpoch).UtcDateTime;
    public string DateIso => DateUtc.ToString("yyyy-MM-dd");
    
    // Methods
    public string GetUrlInSection(string sectionUrl) => $"{sectionUrl}/{Collection}/{Id}.html";
}
```

**Validation Rules**:

- `Title` must not be empty
- `DateEpoch` must be valid Unix timestamp (> 0)
- `Categories` must not be empty
- `Tags` must be normalized (lowercase, hyphens)
- `RenderedHtml` must be valid HTML
- `Excerpt` length should be reasonable (<1000 chars)

---

### FilterState

Client-side state for content filtering (not persisted, URL-synced only).

```csharp
public class FilterState
{
    public string? SearchText { get; set; }                     // Text search query
    public DateRange? DateRange { get; set; }                   // Selected date range
    public IReadOnlyList<string> SelectedTags { get; set; } = Array.Empty<string>();
    public IReadOnlyList<string> SelectedCollections { get; set; } = Array.Empty<string>();
    
    // Computed
    public bool HasActiveFilters =>
        !string.IsNullOrWhiteSpace(SearchText) ||
        DateRange != null ||
        SelectedTags.Any() ||
        SelectedCollections.Any();
        
    public void Clear()
    {
        SearchText = null;
        DateRange = null;
        SelectedTags = Array.Empty<string>();
        SelectedCollections = Array.Empty<string>();
    }
}

public enum DateRange
{
    Last7Days,
    Last30Days,
    Last90Days,
    Last365Days,
    AllTime
}
```

---

## Data Transfer Objects (TechHub.Core/DTOs)

### SectionDto

API response for section data.

```csharp
public record SectionDto(
    string Id,
    string Title,
    string Description,
    string Url,
    string Category,
    string BackgroundImage,
    IReadOnlyList<CollectionReferenceDto> Collections
);
```

---

### CollectionReferenceDto

API response for collection reference.

```csharp
public record CollectionReferenceDto(
    string Title,
    string Collection,
    string Url,
    string Description,
    bool IsCustom
);
```

---

### ContentItemDto

API response for content item (optimized for client consumption).

```csharp
public record ContentItemDto(
    string Id,
    string Title,
    string Description,
    string? Author,
    string DateIso,                                             // ISO 8601 format
    long DateEpoch,                                             // Unix timestamp for client-side sorting
    string Collection,
    string? AltCollection,
    string CanonicalUrl,                                        // Full canonical URL
    IReadOnlyList<string> Categories,
    IReadOnlyList<string> Tags,
    string Excerpt,
    string? ExternalUrl,
    string? VideoId
);
```

**Notes**:

- `RenderedHtml` is NOT included in list views (only in detail view)
- `DateIso` provided for `<time datetime>` attributes
- `CanonicalUrl` includes full URL for SEO

---

### ContentItemDetailDto

Extended DTO for content detail pages (includes rendered HTML).

```csharp
public record ContentItemDetailDto(
    string Id,
    string Title,
    string Description,
    string? Author,
    string DateIso,
    long DateEpoch,
    string Collection,
    string? AltCollection,
    string CanonicalUrl,
    IReadOnlyList<string> Categories,
    IReadOnlyList<string> Tags,
    string RenderedHtml,                                        // Full content
    string Excerpt,
    string? ExternalUrl,
    string? VideoId
);
```

---

### PagedResultDto<T>

Generic paged result for infinite scroll.

```csharp
public record PagedResultDto<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int PageSize,
    int CurrentPage,
    bool HasNextPage
);
```

---

### RssChannelDto

RSS feed channel metadata.

```csharp
public record RssChannelDto(
    string Title,
    string Description,
    string Link,
    string Language,
    DateTime LastBuildDate,
    IReadOnlyList<RssItemDto> Items
);
```

---

### RssItemDto

RSS feed item.

```csharp
public record RssItemDto(
    string Title,
    string Description,
    string Link,
    string Guid,
    DateTime PubDate,
    string? Author,
    IReadOnlyList<string> Categories
);
```

---

## Entity Relationships

```
Section (1) ─── (n) CollectionReference
                        │
                        │ (references)
                        ▼
                    Collection Name (string)
                        │
                        │ (contains)
                        ▼
ContentItem (n) ─── (n) Categories (via many-to-many through frontmatter)
ContentItem (n) ─── (n) Tags (via many-to-many through frontmatter)
```

**Key Relationships**:

- One Section has many CollectionReferences
- CollectionReferences point to collection names (not entities)
- ContentItems belong to one primary Collection
- ContentItems can have multiple Categories (tags in frontmatter)
- ContentItems can have multiple Tags (normalized from frontmatter)

---

## Data Flow

### Content Loading (Startup)

```
1. Repository reads sections.json
   → Parses into List<Section>
   → Stores in IMemoryCache (sliding expiration 30min)

2. Repository scans collections/ directory
   → Reads all markdown files
   → Parses YAML frontmatter
   → Renders markdown to HTML (Markdig)
   → Creates ContentItem entities
   → Sorts by DateEpoch descending (newest first)
   → Stores in IMemoryCache (sliding expiration 30min)

3. API endpoints serve cached data
   → Convert entities to DTOs
   → Return JSON responses (pre-sorted by date desc)
```

### Client-Side Filtering

```
1. Blazor Web loads full section content from API
   → GET /api/sections/{sectionName}
   → Receives SectionDto with all ContentItemDto[]

2. Client-side filter state tracks selections
   → User interacts with filter controls
   → FilterState updated in Blazor component

3. Client-side filtering applies logic
   → Filter by DateRange (compare DateEpoch)
   → Filter by SelectedTags (OR logic)
   → Filter by SearchText (match title/description/tags)
   → Return filtered IEnumerable<ContentItemDto>

4. Infinite scroll paginates results
   → Take next 20 items from filtered results
   → Render in UI
```

---

## Storage Format

### sections.json Structure

```json
{
  "sections": [
    {
      "id": "ai",
      "title": "AI",
      "description": "Artificial Intelligence news and resources",
      "url": "/ai",
      "category": "ai",
      "background": "/assets/section-backgrounds/ai.jpg",
      "collections": [
        {
          "title": "Latest News",
          "collection": "news",
          "url": "/ai/news",
          "description": "AI news and announcements",
          "isCustom": false
        }
      ]
    }
  ]
}
```

### Markdown File Structure

```markdown
---
title: "Example Article Title"
author: "Author Name"
date: 2026-01-02
categories: [ai, github-copilot]
tags: [machine-learning, azure-openai]
external_url: "https://example.com/article"
video_id: "dQw4w9WgXcQ"
alt_collection: "ghc-features"
---

This is the excerpt that appears in list views.

<!--excerpt_end-->

# Full Article Content

The rest of the markdown content rendered to HTML...
```

---

## Validation Rules Summary

| Entity | Field | Validation |
|--------|-------|------------|
| Section | Id | Lowercase, hyphens only, matches Url |
| Section | Collections | Not empty |
| CollectionReference | Collection | Must match valid collection directory |
| ContentItem | Title | Not empty |
| ContentItem | DateEpoch | Valid Unix timestamp (> 0) |
| ContentItem | Categories | Not empty |
| ContentItem | Tags | Normalized (lowercase, hyphens) |
| FilterState | SearchText | Optional, max 200 chars |

---

## Repository Behavior

### Sorting

**CRITICAL**: All repository methods return content sorted by `DateEpoch` in **descending order** (newest first).

This sorting is applied:

- At the repository layer (not in controllers)
- To all methods: `GetAllAsync()`, `GetByCollectionAsync()`, `GetByCategoryAsync()`, `SearchAsync()`
- Before caching (cached results are pre-sorted)

**Rationale**: Consistent sorting order across all endpoints, reduces client-side sorting burden, matches user expectations (newest content first).

**Implementation Note**: For database migration, add `ORDER BY DateEpoch DESC` to all queries. Current file-based implementation uses `.OrderByDescending(x => x.DateEpoch)`.

---

## Extension Methods

Helper methods for entity/DTO conversion (TechHub.Core/Extensions):

```csharp
public static class ContentItemExtensions
{
    public static ContentItemDto ToDto(this ContentItem item, string sectionUrl)
    {
        return new ContentItemDto(
            item.Id,
            item.Title,
            item.Description,
            item.Author,
            item.DateIso,
            item.DateEpoch,
            item.Collection,
            item.AltCollection,
            $"{sectionUrl}/{item.Collection}/{item.Id}.html",
            item.Categories,
            item.Tags,
            item.Excerpt,
            item.ExternalUrl,
            item.VideoId
        );
    }
    
    public static ContentItemDetailDto ToDetailDto(this ContentItem item, string sectionUrl)
    {
        return new ContentItemDetailDto(
            item.Id,
            item.Title,
            item.Description,
            item.Author,
            item.DateIso,
            item.DateEpoch,
            item.Collection,
            item.AltCollection,
            $"{sectionUrl}/{item.Collection}/{item.Id}.html",
            item.Categories,
            item.Tags,
            item.RenderedHtml,
            item.Excerpt,
            item.ExternalUrl,
            item.VideoId
        );
    }
}

public static class SectionExtensions
{
    public static SectionDto ToDto(this Section section)
    {
        return new SectionDto(
            section.Id,
            section.Title,
            section.Description,
            section.Url,
            section.Category,
            section.BackgroundImage,
            section.Collections.Select(c => new CollectionReferenceDto(
                c.Title,
                c.Collection,
                c.Url,
                c.Description,
                c.IsCustom
            )).ToList()
        );
    }
}
```

---

## Performance Considerations

### Caching Strategy

- **Sections**: Load once at startup, cache for 1 hour (absolute expiration)
- **Content Items**: Load once at startup, cache for 30 minutes (sliding expiration)
- **RSS Feeds**: Generate on-demand, cache for 30 minutes (absolute expiration)
- **Cache Invalidation**: Clear all caches on container restart (acceptable for Git-based updates)

### Memory Footprint

Estimated memory usage for typical content volume:

- 1000 content items × ~10 KB average = ~10 MB
- 7 sections × ~1 KB = ~7 KB  
- Total cached data: ~10-15 MB

**Acceptable for current scale** - No distributed cache needed until traffic exceeds 10k concurrent users.

---

## Future Database Schema (When Migrating)

When content volume exceeds ~1000 items, migrate to database with this schema:

```sql
-- Sections table
CREATE TABLE Sections (
    Id VARCHAR(50) PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    Url VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    BackgroundImage VARCHAR(200)
);

-- ContentItems table
CREATE TABLE ContentItems (
    Id VARCHAR(100) PRIMARY KEY,
    Title VARCHAR(500) NOT NULL,
    Description TEXT,
    Author VARCHAR(200),
    DateEpoch BIGINT NOT NULL,
    Collection VARCHAR(50) NOT NULL,
    AltCollection VARCHAR(50),
    RenderedHtml TEXT NOT NULL,
    Excerpt TEXT,
    ExternalUrl VARCHAR(500),
    VideoId VARCHAR(50),
    INDEX idx_date (DateEpoch DESC),
    INDEX idx_collection (Collection),
    INDEX idx_alt_collection (AltCollection)
);

-- Categories table (many-to-many)
CREATE TABLE ContentItemCategories (
    ContentItemId VARCHAR(100),
    Category VARCHAR(100),
    PRIMARY KEY (ContentItemId, Category),
    FOREIGN KEY (ContentItemId) REFERENCES ContentItems(Id)
);

-- Tags table (many-to-many)
CREATE TABLE ContentItemTags (
    ContentItemId VARCHAR(100),
    Tag VARCHAR(100),
    PRIMARY KEY (ContentItemId, Tag),
    FOREIGN KEY (ContentItemId) REFERENCES ContentItems(Id),
    INDEX idx_tag (Tag)
);
```

Repository pattern allows seamless swap from file-based to database implementation without changing API contracts.

---

**Status**: ✅ Data model complete - Ready for API contract generation
