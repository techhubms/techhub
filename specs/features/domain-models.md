# Domain Models Specification

> **Feature**: Core domain models for sections, collections, and content items

## Overview

The domain models represent the core business entities of Tech Hub: Sections (topical areas), Collections (content types), and Content Items (individual articles/videos/posts). Models are implemented as C# records for immutability and use primitive types with validation. All models support multi-location content access where the same content can appear in multiple sections.

## Requirements

### Functional Requirements

**FR-1**: The system MUST represent sections with unique identifiers and URLs  
**FR-2**: The system MUST represent collections within sections  
**FR-3**: The system MUST represent content items with metadata and rendered HTML  
**FR-4**: The system MUST support multi-location content access (same item in multiple sections)  
**FR-5**: The system MUST track canonical URL for SEO (primary section context)  
**FR-6**: The system MUST store dates as Unix epoch timestamps for timezone-agnostic operations  
**FR-7**: The system MUST normalize tags for consistent filtering  
**FR-8**: The system MUST support both custom and auto-generated collections  
**FR-9**: The system MUST validate required fields at construction  
**FR-10**: The system MUST generate context-specific URLs for content items  

### Non-Functional Requirements

**NFR-1**: Models MUST be immutable (C# records)  
**NFR-2**: Models MUST use nullable reference types  
**NFR-3**: Models MUST use required properties for mandatory fields  
**NFR-4**: Models MUST serialize to JSON efficiently  
**NFR-5**: Models MUST support XML documentation comments  
**NFR-6**: DTOs MUST be separate from domain models  

## Domain Model Definitions

### Section Model

Represents a top-level content category (e.g., "AI", "GitHub Copilot").

**Properties**:
- `Id` (string, required): Unique identifier matching sections.json
- `Title` (string, required): Display title (e.g., "GitHub Copilot")
- `Description` (string, required): Section description for meta tags
- `Url` (string, required): URL-safe slug (e.g., "github-copilot")
- `Category` (string, required): Category identifier for content matching
- `Image` (string, required): Background image path
- `Collections` (IReadOnlyList<CollectionReference>, required): Associated collections

**Invariants**:
- `Url` must be URL-safe (lowercase, hyphens only)
- `Category` must match content item categories
- `Collections` must not be empty

### CollectionReference Model

Represents a collection within a section (e.g., "News", "Videos").

**Properties**:
- `Title` (string, required): Display title (e.g., "Latest News")
- `Url` (string, required): URL-safe slug (e.g., "news")
- `Collection` (string, required): Collection identifier (matches collection directories)
- `Description` (string, required): Collection description
- `IsCustom` (bool): Whether this is a custom collection (manually created page)

**Invariants**:
- `Collection` must match a valid collection directory (_news, _blogs, _videos, etc.)
- `Url` must be URL-safe

### ContentItem Model

Represents an individual piece of content (article, video, community post).

**Properties**:
- `Id` (string, required): Unique content identifier (slug from filename)
- `Title` (string, required): Content title
- `Description` (string, required): Brief description/excerpt
- `CanonicalUrl` (string, required): Primary URL for SEO (based on first category)
- `Author` (string, required): Content author name
- `DateEpoch` (long, required): Publication date as Unix timestamp
- `Collection` (string, required): Primary collection (news, blogs, videos, community, roundups)
- `Categories` (IReadOnlyList<string>, required): All categories this content belongs to
- `Tags` (IReadOnlyList<string>, required): Normalized tags for filtering
- `Content` (string, required): Rendered HTML content
- `Excerpt` (string, required): Rendered excerpt HTML
- `ExternalUrl` (string?, optional): External link if applicable
- `VideoId` (string?, optional): YouTube video ID if applicable
- `AltCollection` (string?, optional): Alternative collection for subfolder organization

**Invariants**:
- `DateEpoch` must be valid Unix timestamp
- `Categories` must not be empty
- `Tags` must be normalized (lowercase, trimmed)
- `CanonicalUrl` is derived from first category in `Categories` array
- If `VideoId` is provided, `ExternalUrl` should be YouTube URL

**Methods**:
- `GetUrlInSection(string sectionUrl)`: Generate URL for this content in a specific section context
  - Returns: `/{sectionUrl}/{collection}/{id}.html`
  - Example: `/ai/videos/vs-code-107.html` or `/github-copilot/videos/vs-code-107.html`

### ContentItemDto Model

Data Transfer Object for API responses, includes formatted display values.

**Properties**:
- `Id` (string, required): Content identifier
- `Title` (string, required): Content title
- `Description` (string, required): Description/excerpt
- `Url` (string, required): Context-specific URL for current section
- `CanonicalUrl` (string, required): Canonical URL for SEO <link rel="canonical">
- `Author` (string, required): Author name
- `Date` (string, required): Formatted display date (YYYY-MM-DD)
- `DateIso` (string, required): ISO 8601 formatted date for <time datetime>
- `DateEpoch` (long, required): Unix timestamp for client-side operations
- `Collection` (string, required): Collection identifier
- `Categories` (IReadOnlyList<string>, required): All categories
- `Tags` (IReadOnlyList<string>, required): Normalized tags
- `ExternalUrl` (string?, optional): External link
- `VideoId` (string?, optional): YouTube video ID

**Conversion**:
- Created from `ContentItem` via `.ToDto(string sectionUrl)` extension method
- `Url` is context-specific based on section
- `Date` is formatted in Europe/Brussels timezone
- `DateIso` uses ISO 8601 format

### SectionDto Model

Data Transfer Object for section API responses.

**Properties**:
- Same as `Section` model
- Used for API serialization

## Use Cases

### UC-1: Create Content Item from Markdown

**Actor**: Content Repository  
**Precondition**: Markdown file with valid YAML frontmatter  
**Trigger**: Application startup or content refresh  

**Flow**:
1. Repository reads Markdown file
2. Repository parses YAML frontmatter
3. Repository extracts: title, date, author, categories, tags
4. Repository converts date string to Unix epoch timestamp
5. Repository normalizes all tags (lowercase, trim)
6. Repository renders Markdown to HTML
7. Repository determines canonical URL from first category
8. Repository creates `ContentItem` with all properties
9. Repository validates all required fields are present

**Postcondition**: Valid `ContentItem` instance created

**Validation Rules**:
- Title must not be empty
- At least one category must be specified
- Date must be parseable
- All required fields must have values

### UC-2: Generate Context-Specific URLs

**Actor**: API Endpoint  
**Precondition**: Content item exists with multiple categories  
**Trigger**: API request for content in specific section  

**Flow**:
1. API receives request: `/api/content/ai/videos/vs-code-107`
2. Repository retrieves content item with id `vs-code-107`
3. Content item has categories: `["ai", "github-copilot"]`
4. API validates "ai" is in categories list
5. API calls `item.GetUrlInSection("ai")`
6. Method returns `/ai/videos/vs-code-107.html`
7. API creates DTO with context-specific URL
8. API returns DTO to client

**Postcondition**: DTO contains correct URL for section context

### UC-3: Determine Canonical URL

**Actor**: SEO Service  
**Precondition**: Content item has multiple categories  
**Trigger**: Page render for SEO meta tags  

**Flow**:
1. Service receives content item
2. Content item categories: `["ai", "github-copilot", "ml"]`
3. Service reads `CanonicalUrl` property
4. `CanonicalUrl` is derived from first category: `"ai"`
5. Canonical URL is `/ai/videos/vs-code-107.html`
6. Service renders: `<link rel="canonical" href="https://tech.hub.ms/ai/videos/vs-code-107.html">`

**Postcondition**: Correct canonical URL set for SEO

## Acceptance Criteria

**AC-1**: Given a valid Section, when constructed, then all required properties are set  
**AC-2**: Given a ContentItem with multiple categories, when GetUrlInSection called, then correct URL returned  
**AC-3**: Given a ContentItem, when converted to DTO, then display dates are formatted correctly  
**AC-4**: Given a ContentItem with invalid date, when constructed, then validation exception thrown  
**AC-5**: Given a Section without collections, when constructed, then validation exception thrown  
**AC-6**: Given a ContentItem, when canonical URL determined, then first category used  
**AC-7**: Given normalized tags, when compared, then case-insensitive matching works  

## Implementation Notes

### C# Record Syntax

```csharp
// TechHub.Core/Models/ContentItem.cs
namespace TechHub.Core.Models;

/// <summary>
/// Represents a content item (news, blog, video, etc.)
/// Supports multi-location access via categories array
/// </summary>
public record ContentItem
{
    /// <summary>
    /// Unique content identifier (slug)
    /// </summary>
    public required string Id { get; init; }
    
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Author { get; init; }
    
    /// <summary>
    /// Publication date as Unix epoch timestamp
    /// Stored as long for timezone-agnostic operations
    /// </summary>
    public required long DateEpoch { get; init; }
    
    /// <summary>
    /// Primary collection (news, blogs, videos, community, roundups)
    /// </summary>
    public required string Collection { get; init; }
    
    /// <summary>
    /// All categories this content belongs to (e.g., ["ai", "github-copilot"])
    /// First category determines canonical URL
    /// Supports multi-location content access
    /// </summary>
    public required IReadOnlyList<string> Categories { get; init; }
    
    public required IReadOnlyList<string> Tags { get; init; }
    public required string Content { get; init; }
    public required string Excerpt { get; init; }
    
    /// <summary>
    /// Canonical URL for SEO (primary section context)
    /// Derived from first category in Categories array
    /// </summary>
    public required string CanonicalUrl { get; init; }
    
    public string? ExternalUrl { get; init; }
    public string? VideoId { get; init; }
    public string? AltCollection { get; init; }
    
    /// <summary>
    /// Generate URL for this content in a specific section context.
    /// Supports multi-location access.
    /// Example: /ai/videos/vs-code-107.html or /github-copilot/videos/vs-code-107.html
    /// </summary>
    public string GetUrlInSection(string sectionUrl) => 
        $"/{sectionUrl}/{Collection}/{Id}.html";
}
```

### Date Handling

```csharp
// TechHub.Core/Utilities/DateUtils.cs
namespace TechHub.Core.Utilities;

public static class DateUtils
{
    private static readonly TimeZoneInfo BrusselsTimeZone = 
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
    
    /// <summary>
    /// Convert Unix epoch timestamp to DateTimeOffset
    /// </summary>
    public static DateTimeOffset FromEpoch(long epoch) =>
        DateTimeOffset.FromUnixTimeSeconds(epoch);
    
    /// <summary>
    /// Convert DateTimeOffset to Unix epoch timestamp
    /// </summary>
    public static long ToEpoch(DateTimeOffset date) =>
        date.ToUnixTimeSeconds();
    
    /// <summary>
    /// Format date for display in Europe/Brussels timezone
    /// Returns: YYYY-MM-DD
    /// </summary>
    public static string FormatDisplay(long epoch)
    {
        var dt = FromEpoch(epoch);
        var offset = BrusselsTimeZone.GetUtcOffset(dt);
        return dt.ToOffset(offset).ToString("yyyy-MM-dd");
    }
    
    /// <summary>
    /// Format date for ISO 8601 (for HTML <time datetime="">)
    /// </summary>
    public static string FormatIso(long epoch) =>
        FromEpoch(epoch).ToString("O");
}
```

### Extension Methods

```csharp
// TechHub.Core/Extensions/ContentItemExtensions.cs
namespace TechHub.Core.Extensions;

public static class ContentItemExtensions
{
    /// <summary>
    /// Convert ContentItem to DTO for specific section context
    /// </summary>
    public static ContentItemDto ToDto(this ContentItem item, string sectionUrl)
    {
        return new ContentItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Url = item.GetUrlInSection(sectionUrl),
            CanonicalUrl = item.CanonicalUrl,
            Author = item.Author,
            Date = DateUtils.FormatDisplay(item.DateEpoch),
            DateIso = DateUtils.FormatIso(item.DateEpoch),
            DateEpoch = item.DateEpoch,
            Collection = item.Collection,
            Categories = item.Categories,
            Tags = item.Tags,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId
        };
    }
    
    /// <summary>
    /// Convert ContentItem to detailed DTO (includes content and excerpt)
    /// </summary>
    public static ContentItemDetailDto ToDetailDto(this ContentItem item, string sectionUrl)
    {
        return new ContentItemDetailDto
        {
            // ... all properties from ToDto
            Content = item.Content,
            Excerpt = item.Excerpt
        };
    }
}
```

## Testing Strategy

### Unit Tests

```csharp
// TechHub.Core.Tests/Models/ContentItemTests.cs
public class ContentItemTests
{
    [Fact]
    public void GetUrlInSection_WithValidSection_ReturnsCorrectUrl()
    {
        // Arrange
        var item = new ContentItem
        {
            Id = "vs-code-107",
            Collection = "videos",
            Categories = new[] { "ai", "github-copilot" },
            // ... other required properties
        };
        
        // Act
        var url = item.GetUrlInSection("ai");
        
        // Assert
        Assert.Equal("/ai/videos/vs-code-107.html", url);
    }
    
    [Theory]
    [InlineData("ai", "/ai/videos/test.html")]
    [InlineData("github-copilot", "/github-copilot/videos/test.html")]
    [InlineData("ml", "/ml/videos/test.html")]
    public void GetUrlInSection_WithDifferentSections_ReturnsCorrectUrls(
        string section, 
        string expectedUrl)
    {
        var item = new ContentItem
        {
            Id = "test",
            Collection = "videos",
            // ...
        };
        
        Assert.Equal(expectedUrl, item.GetUrlInSection(section));
    }
    
    [Fact]
    public void CanonicalUrl_UsesFirstCategory()
    {
        var item = new ContentItem
        {
            Categories = new[] { "ai", "github-copilot", "ml" },
            CanonicalUrl = "/ai/videos/test.html",
            // ...
        };
        
        Assert.Equal("/ai/videos/test.html", item.CanonicalUrl);
    }
}
```

### Integration Tests

```csharp
// TechHub.Infrastructure.Tests/Repositories/ContentRepositoryTests.cs
public class ContentRepositoryTests
{
    [Fact]
    public async Task GetItemByIdAsync_WithCategoryFilter_ReturnsMatchingItem()
    {
        // Arrange
        var repo = new FileContentRepository(/* ... */);
        
        // Act
        var item = await repo.GetItemByIdAsync("vs-code-107", categoryFilter: "ai");
        
        // Assert
        Assert.NotNull(item);
        Assert.Contains("ai", item.Categories);
    }
    
    [Fact]
    public async Task GetItemByIdAsync_WithNonMatchingCategory_ReturnsNull()
    {
        var repo = new FileContentRepository(/* ... */);
        
        var item = await repo.GetItemByIdAsync("vs-code-107", categoryFilter: "azure");
        
        Assert.Null(item);
    }
}
```

## Migration Considerations

### From Jekyll Front Matter

**Jekyll YAML**:
```yaml
---
title: "VS Code 107"
date: 2025-01-15
author: "Microsoft"
categories: [ai, github-copilot]
tags: [vscode, copilot, demo]
youtube_id: "abc123"
---
```

**Converted to ContentItem**:
```csharp
new ContentItem
{
    Id = "vs-code-107",
    Title = "VS Code 107",
    DateEpoch = 1736899200, // 2025-01-15 00:00:00 Europe/Brussels
    Author = "Microsoft",
    Categories = new[] { "ai", "github-copilot" },
    Tags = new[] { "vscode", "copilot", "demo" },
    VideoId = "abc123",
    CanonicalUrl = "/ai/videos/vs-code-107.html", // First category
    // ...
}
```

## Open Questions

None - domain models are well-defined based on existing Jekyll structure.

## References

- `/AGENTS.md` - Root development guide
- `/specs/.speckit/constitution.md` - Project principles
- `/specs/current-site-analysis.md` - Current Jekyll behavior
- `/docs/dotnet-migration-plan.md` - Migration roadmap

