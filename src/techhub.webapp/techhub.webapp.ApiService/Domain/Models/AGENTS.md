# Domain Models

## Purpose

This folder contains all entity classes and domain models that represent the core business concepts of the Tech Hub application.

## Models Overview

### Base Models

#### ContentItem (Abstract Base Class)

**Purpose**: Base class for all content types with common properties

**Key Properties**:
- `Id`: Unique identifier (GUID)
- `Title`: Content title (required, max 200 characters)
- `Description`: Brief summary (required, max 500 characters)
- `Author`: Content author name (required)
- `PublishedDate`: Publication date with timezone
- `CanonicalUrl`: Original source URL
- `Permalink`: Site-relative URL (/YYYY-MM-DD-title.html)
- `Categories`: Section mappings (["AI", "GitHub Copilot"])
- `Tags`: Technology keywords (display format)
- `TagsNormalized`: Lowercase, standardized tags
- `ViewingMode`: "internal" or "external"
- `FeedName`: RSS feed source name (optional)
- `FeedUrl`: RSS feed URL (optional)
- `ExcerptSeparator`: Markdown excerpt marker
- `CollectionType`: Collection name (news, videos, posts, etc.)

**Validation Rules**:
- Title cannot be empty
- Description cannot be empty
- Author cannot be empty
- PublishedDate must be valid DateTimeOffset
- Categories array cannot be empty
- Tags array must contain at least 3 items
- TagsNormalized must match Tags array length
- CanonicalUrl must be valid URL for external content
- Permalink must follow /YYYY-MM-DD-title-slug.html pattern

### Content Type Models

#### NewsArticle

**Inherits**: ContentItem

**Additional Properties**:
- None (uses all base properties)

**Purpose**: Represents news articles and announcements

**Validation**: Same as ContentItem

#### BlogPost

**Inherits**: ContentItem

**Additional Properties**:
- None (uses all base properties)

**Purpose**: Represents blog posts and technical articles

**Collection**: posts

#### Video

**Inherits**: ContentItem

**Additional Properties**:
- `VideoId`: YouTube video ID (required)
- `Duration`: Video duration in seconds (optional)
- `ThumbnailUrl`: Video thumbnail URL (auto-generated from YouTube)

**Purpose**: Represents video content from YouTube

**Validation**:
- VideoId must be valid YouTube ID format
- Duration must be positive if provided

**Special Cases**:
- GitHub Copilot feature videos in `ghc-features/` subfolder
- Feature videos include `Plans` array (Free, Pro, Business, Enterprise)
- Feature videos include `GhesSupport` boolean flag

#### CommunityPost

**Inherits**: ContentItem

**Additional Properties**:
- `MagazineDescriptionAi`: AI-generated magazine description (optional)
- `DownloadUrl`: PDF download link for magazines (optional)

**Purpose**: Represents community posts and XPRT Magazine editions

**Validation**:
- DownloadUrl must be valid URL if provided

#### Event

**Inherits**: ContentItem

**Additional Properties**:
- `StartDate`: Event start date/time (required)
- `EndDate`: Event end date/time (optional)
- `Location`: Event location or "Online" (required)
- `RegistrationUrl`: Event registration link (optional)

**Purpose**: Represents events, conferences, and meetups

**Validation**:
- StartDate cannot be in past (for upcoming events)
- EndDate must be after StartDate if provided
- Location cannot be empty

#### Roundup

**Inherits**: ContentItem

**Additional Properties**:
- `RoundupPeriod`: Time period covered (e.g., "Week of Jan 1-7, 2025")

**Purpose**: Represents weekly content summaries

**Collection**: roundups (shown on homepage)

### Structural Models

#### Section

**Purpose**: Represents top-level organizational units

**Properties**:
- `Id`: Unique identifier (GUID)
- `Key`: URL-safe key (e.g., "github-copilot")
- `Title`: Display name (e.g., "GitHub Copilot")
- `Description`: Section description
- `Url`: Section URL path (e.g., "/github-copilot")
- `Category`: Category name for filtering
- `ImageUrl`: Background image path
- `Collections`: List of collections in this section
- `Order`: Display order (for navigation)

**Validation**:
- Key must be URL-safe (lowercase, hyphens only)
- Title cannot be empty
- Url must start with /
- Category must match one of: AI, GitHub Copilot, ML, Azure, Coding, DevOps, Security
- Collections array cannot be empty

**Examples**:
```csharp
new Section
{
    Key = "github-copilot",
    Title = "GitHub Copilot",
    Category = "GitHub Copilot",
    Url = "/github-copilot",
    Collections = ["news", "posts", "videos", "community"]
}
```

#### Collection

**Purpose**: Represents content type groupings within sections

**Properties**:
- `Id`: Unique identifier (GUID)
- `Name`: Collection name (e.g., "news", "posts")
- `Title`: Display title (e.g., "News")
- `Description`: Collection description
- `Url`: Collection URL (e.g., "/github-copilot/news.html")
- `IsCustom`: Whether manually created (vs auto-generated)
- `SectionKey`: Parent section key
- `Order`: Display order within section

**Validation**:
- Name must be one of: news, posts, videos, community, events, roundups
- Title cannot be empty
- Url must follow pattern: /{section}/{collection}.html
- SectionKey must reference existing section

#### Tag

**Purpose**: Represents content tags with normalization

**Properties**:
- `Id`: Unique identifier (GUID)
- `Display`: Display format (e.g., "Visual Studio Code")
- `Normalized`: Normalized format (e.g., "visual studio code")
- `Count`: Number of times used
- `FirstSeen`: First appearance date
- `RelatedTags`: Tags that subset-match this tag

**Validation**:
- Display cannot be empty
- Normalized must be lowercase
- Count must be non-negative
- FirstSeen must be valid date

**Normalization Rules**:
- Convert to lowercase
- Replace special characters except hyphens with spaces
- Collapse multiple spaces to single space
- Trim whitespace
- Examples:
  - "C#" → "csharp"
  - "Tag++" → "tagplusplus"
  - "A . Tag" → "a dot tag"
  - "Visual Studio Code" → "visual studio code"

**Subset Matching**:
- Tag "AI" matches: "AI", "Generative AI", "Azure AI", "AI Agents"
- Tag "Visual Studio" matches: "Visual Studio", "Visual Studio Code", "Visual Studio 2022"
- Implemented via word boundary matching on normalized tags

## Design Patterns

### Inheritance Hierarchy

```text
ContentItem (abstract)
├── NewsArticle
├── BlogPost
├── Video
├── CommunityPost
├── Event
└── Roundup
```

### Value Objects (Future)

Consider extracting to value objects:
- `Email`: Author email with validation
- `Url`: URLs with validation and normalization
- `Markdown`: Markdown content with rendering
- `DateRange`: Start/end date pairs with validation

### Domain Events (Future)

Potential events for event sourcing:
- `ContentPublished`: When content is published
- `ContentUpdated`: When content is modified
- `TagAdded`: When new tag is created
- `ContentFiltered`: When filtering is applied (for analytics)

## Validation Strategy

### Property Validation

Each model class includes:
- Required field validation in constructors
- Format validation for strings
- Range validation for numbers
- Business rule validation

### Example Validation

```csharp
public ContentItem(string title, string description, string author)
{
    if (string.IsNullOrWhiteSpace(title))
        throw new ArgumentException("Title cannot be empty", nameof(title));
    
    if (title.Length > 200)
        throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));
    
    Title = title;
    // ... more validation
}
```

### Validation Extensions

Consider FluentValidation for complex rules:
- Cross-property validation
- Conditional validation
- Custom validators
- Async validation (for unique checks)

## Testing Approach

### Unit Tests

Test each model class:
- Constructor validation
- Property setters (if any)
- Business rule methods
- Equality comparisons
- Serialization/deserialization

### Test Examples

```csharp
[Fact]
public void ContentItem_ThrowsException_WhenTitleEmpty()
{
    var ex = Assert.Throws<ArgumentException>(() => 
        new NewsArticle("", "description", "author"));
    Assert.Equal("title", ex.ParamName);
}

[Fact]
public void Tag_Normalize_ConvertsToLowercase()
{
    var tag = new Tag("Visual Studio Code");
    Assert.Equal("visual studio code", tag.Normalized);
}
```

## Related Documentation

- [Domain Layer Overview](../AGENTS.md)
- [Filtering System](../../../../../../docs/filtering-system.md)
- [Terminology](../../../../../../docs/terminology.md)
