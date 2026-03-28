# TechHub.Core Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Core/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).
> **RULE**: All rules from parent AGENTS.md files apply. This file adds Core layer-specific patterns.

## Overview

This project contains domain models and interfaces with **zero external dependencies** (pure .NET). It defines the core business entities and contracts that all other layers depend on.

**When to read this file**: When creating or modifying domain models, repository interfaces, or understanding the core domain.

**Testing this code**: See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md) for unit testing patterns.

## Project Structure

```text
TechHub.Core/
├── Models/                       # Domain models organized by feature
│   ├── Core/                    # Core domain entities
│   │   ├── Section.cs           # Section model (AI, Security, DevOps, etc.)
│   │   ├── ContentItem.cs       # Content item (list + detail views)
│   │   └── Collection.cs        # Collection definition
│   ├── Facets/                  # Facet aggregation models
│   │   ├── FacetRequest.cs      # Facet aggregation request
│   │   ├── FacetResults.cs      # Facet aggregation response
│   │   └── FacetValue.cs        # Single facet value with count
│   ├── Filter/                  # Filtering models
│   │   ├── FilterRequest.cs     # Filter request parameters
│   │   ├── FilterResponse.cs    # Filter response with items
│   │   └── FilterSummary.cs     # Filter summary metadata
│   ├── PageData/                # Page-specific data models
│   │   ├── DXSpacePageData.cs   # DX Space page data
│   │   ├── FeaturesPageData.cs  # Features page data
│   │   ├── GenAIPageData.cs     # GenAI page data
│   │   ├── HandbookPageData.cs  # Handbook page data
│   │   ├── LevelsPageData.cs    # Levels page data
│   │   └── SDLCPageData.cs      # SDLC page data
│   ├── Rss/                     # RSS feed models
│   │   ├── RssChannel.cs        # RSS channel (feed metadata)
│   │   └── RssItem.cs           # RSS item (feed entry)
│   ├── Search/                  # Search models
│   │   ├── SearchRequest.cs     # Search request parameters
│   │   └── SearchResults.cs     # Search response with pagination
│   ├── Tags/                    # Tag-related models
│   │   ├── AllTagsResponse.cs   # Response for all tags API
│   │   ├── TagCloudItem.cs      # Tag cloud item with size
│   │   ├── TagWithCount.cs      # Tag with usage count
├── Interfaces/                   # Repository contracts
│   ├── IContentRepository.cs    # Content & Section data access
│   ├── IDbConnectionFactory.cs  # Database connection factory
│   ├── IMarkdownService.cs      # Markdown processing
│   ├── IRssService.cs           # RSS feed generation
│   └── ISqlDialect.cs           # SQL dialect abstraction
└── TechHub.Core.csproj          # Project file (zero dependencies!)
```

## Core Principles

### No External Dependencies

**CRITICAL**: TechHub.Core must remain framework-agnostic:

- ✅ **Use**: `System.*` namespaces only
- ✅ **Use**: `Microsoft.Extensions.*.Abstractions` (logging, DI interfaces)
- ✅ **Use**: Standard .NET types (string, int, DateTime, etc.)
- ✅ **Use**: Collections (`IReadOnlyList<T>`, `List<T>`, `Dictionary<K,V>`)
- 🚫 **NEVER**: Add NuGet packages with implementation logic
- 🚫 **NEVER**: Reference ASP.NET Core, Entity Framework, or other frameworks

**Why**: Keeps domain logic clean, testable, and reusable across any application type.

### Immutability by Default

**Use `init` accessors and `required` properties** for all domain models:

- Properties use `init` instead of `set`
- Mark mandatory fields with `required`
- Use `IReadOnlyList<T>` for collections

**Benefits**:

- Thread-safe by design
- Prevents accidental mutations
- Clear intent (values set once at creation)
- Works perfectly with record types

**See**: [Models/Core/Section.cs](Models/Core/Section.cs) for implementation

## Domain Model Patterns

### Entity Design

**Use classes for entities with identity** (can be mutated over lifetime).

**Example**: `Section`

**Key Properties**:

- `required` for mandatory fields
- `init` for immutability after construction
- Validation methods for business rules
- Avoid public setters

### Value Object Pattern

**Use records for value objects** (identity defined by values, not reference).

**Example**: `CollectionReference`, `TagCloudItem`, `ContentItem`, `CustomPage`

**Benefits**: Value equality, immutability, concise syntax, built-in methods.

### Unified Model Pattern (No Separate DTOs)

**Pattern**: Single model serves both list and detail views.

**ContentItem** ([Models/Core/ContentItem.cs](Models/Core/ContentItem.cs)):

- Used for both list views (summary) and detail views (full HTML)
- List views: `RenderedHtml` is null
- Detail views: `RenderedHtml` getter throws if accessed when null (fail-fast)
- **Why**: Eliminates duplication, simpler serialization, type-safe
- Same pattern applies to custom page data models in `Models/PageData/`

**Benefits**:

- No property duplication between summary/detail models
- Single source of truth for model structure
- Simpler serialization (same type, different data)
- Clear intent via property access (throws if misused)

### Content Item Model

**ContentItem** ([Models/Core/ContentItem.cs](Models/Core/ContentItem.cs)) represents content in both list and detail views:

**List View Properties** (always populated):

- Metadata: `Slug`, `Title`, `Author`, `DateEpoch`
- Categorization: `SectionNames`, `PrimarySectionName`, `CollectionName`, `Tags`
- Display: `Excerpt`, `Url`, `ExternalUrl`
- Feature flags: `Plans`, `GhesSupport`, `Draft`

**Detail View Properties** (only for full content):

- `RenderedHtml` - Full HTML content (throws if accessed in list view)

**Methods**:

- Navigation: `LinksExternally()`, `GetHref()`, `GetTarget()`, `GetRel()`, `GetAriaLabel()`, `GetDataEnhanceNav()`
- URL generation: `GetUrlInSection()`, `GetPrimarySectionUrl()`
- Validation: `Validate()` - Business rule checks
- Computed: `DateUtc` - Convert epoch to DateTime

See [Markdown Frontmatter Mapping](#markdown-frontmatter-mapping) for complete field mappings.

## Markdown Frontmatter Mapping

📖 **Full documentation**: See [docs/frontmatter.md](../../docs/frontmatter.md#frontmatter-to-domain-model-mapping) for complete field definitions and mappings.

**Key Implementation Notes**:

- `date` → `DateEpoch`: Converted to Unix timestamp in Europe/Brussels timezone
- `section_names` → `SectionNames`: Multi-section support (array)
- `tags` → `Tags`: Normalized to lowercase, hyphen-separated
- Filename → `Slug`: `2025-01-15-article.md` becomes `2025-01-15-article`
- Content before `<!--excerpt_end-->` → `Excerpt`: Plain text, max 200 words

**See [src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md)** for implementation details of frontmatter parsing.

## Model Patterns

**Use records for all models** (unified model layer - no separate DTOs).

**Model Structure**:

- Use `record` types for value equality and immutability
- Mark mandatory fields with `required`
- Use `init` accessors
- Add XML documentation comments
- Use inheritance for detail/summary relationships

**Key Models**:

- `Section` - Section data with collections ([Models/Core/Section.cs](Models/Core/Section.cs))
- `ContentItem` - Content item (list + detail) ([Models/Core/ContentItem.cs](Models/Core/ContentItem.cs))
- `Collection` - Collection definition with custom page ordering ([Models/Core/Collection.cs](Models/Core/Collection.cs))
- `RssChannel` - RSS feed metadata ([Models/Rss/RssChannel.cs](Models/Rss/RssChannel.cs))
- `RssItem` - RSS feed entry ([Models/Rss/RssItem.cs](Models/Rss/RssItem.cs))
- `SearchRequest` - Search parameters ([Models/Search/SearchRequest.cs](Models/Search/SearchRequest.cs))
- `SearchResults<T>` - Search response ([Models/Search/SearchResults.cs](Models/Search/SearchResults.cs))
- `FacetResults` - Facet aggregations ([Models/Facets/FacetResults.cs](Models/Facets/FacetResults.cs))
- `TagCloudItem` - Tag cloud display item ([Models/Tags/TagCloudItem.cs](Models/Tags/TagCloudItem.cs))
- `TagWithCount` - Tag with usage count ([Models/Tags/TagWithCount.cs](Models/Tags/TagWithCount.cs))

### Collection Model - Custom Page Ordering

**Collection** ([Models/Core/Collection.cs](Models/Core/Collection.cs)) represents a collection of content with optional custom page ordering:

**Properties**:

- `Name` - Collection identifier (lowercase with hyphens)
- `Title` - Display title
- `Url` - Collection URL
- `Description` - Collection description
- `DisplayName` - Display name for page titles
- `IsCustom` - Whether this is a custom page (not a content collection)
- `Order` - Display order for custom pages (lower values appear first)

**Custom Page Ordering**:

Custom pages (`IsCustom = true`) support explicit ordering via the `Order` property:

- Lower `Order` values appear first
- If `Order` values are equal, sort alphabetically by `Title`
- Regular collections (IsCustom = false) don't use `Order` (defaults to 0)
- Applied in:
  - `SectionCard.razor` - Homepage section badges
  - `SubNav.razor` - Horizontal navigation bar

**Configuration Example** (appsettings.json):

```json
{
  "Collections": {
    "features": {
      "Title": "Features",
      "Custom": true,
      "Order": 1
    },
    "levels-of-enlightenment": {
      "Title": "Levels of Enlightenment",
      "Custom": true,
      "Order": 2
    },
    "handbook": {
      "Title": "The GitHub Copilot Handbook",
      "Custom": true,
      "Order": 3
    }
  }
}
```

**Usage Pattern**:

```csharp
// Get ordered custom pages
var orderedCustomPages = section.Collections
    .Where(c => c.IsCustom)
    .OrderBy(c => c.Order)
    .ThenBy(c => c.Title)
    .ToList();
```

**See**: [src/TechHub.Web/AGENTS.md - SectionCard Custom Page Ordering](../TechHub.Web/AGENTS.md#sectioncard-custom-page-ordering-and-expand-badges) for UI implementation details.

## Repository Interfaces

**Define contracts for data access**:

**Key Repository Interfaces**:

- `IContentRepository` - Section and content data access ([Interfaces/IContentRepository.cs](Interfaces/IContentRepository.cs))
- `IMarkdownService` - Markdown processing ([Interfaces/IMarkdownService.cs](Interfaces/IMarkdownService.cs))
- `IRssService` - RSS feed generation ([Interfaces/IRssService.cs](Interfaces/IRssService.cs))

**Repository Method Patterns**:

- All methods async with `CancellationToken ct = default`
- Return `Task<IReadOnlyList<T>>` for collections
- Return `Task<T?>` for single items (null if not found)
- Include XML documentation for each method

**CRITICAL Repository Contract**:

- All `IContentRepository` methods **MUST** return content sorted by `DateEpoch` descending (newest first)
- Methods return `ContentItem` - caller determines if detail view is needed by checking `RenderedHtml != null`
- This sorting happens at repository layer, before caching
- Clients should never need to sort content themselves

**See**: [Interfaces/](Interfaces/) for complete interface definitions

## URL Generation Methods

**ContentItem includes URL generation methods**:

**`GetUrlInSection(string sectionName)`**:

- Generates full URL path for content item
- Pattern: `/{sectionName}/{collectionName}/{slug}`
- Validates section name exists in item's `SectionNames`
- Throws `ArgumentException` if section not found

**Example**: `GetUrlInSection("github-copilot")` → `"/github-copilot/blogs/2024-01-15-article"`

**See**: [Models/Core/ContentItem.cs](Models/Core/ContentItem.cs) for implementation

## Unix Epoch Timestamp Usage

**Always use Unix epoch for date storage and manipulation**:

**Storage**:

```csharp
public required long DateEpoch { get; init; }  // Seconds since Unix epoch
```

**Conversion Examples**:

- **From DateTime**: `new DateTimeOffset(date).ToUnixTimeSeconds()`
- **To DateTime**: `DateTimeOffset.FromUnixTimeSeconds(epochSeconds).DateTime`
- **From frontmatter**: See [src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md) for parsing logic

**Why Unix Epoch?**:

- Timezone-agnostic storage (always UTC internally)
- Efficient comparison and sorting
- Compact representation (single long value)
- Standard format across all platforms

## Model Conversion Extensions

**Legacy Note**: Previously had `ContentItemExtensions` and `SectionExtensions` for DTO conversion.

**Current State**: Extensions removed since models are now unified (no separate DTOs).

**Migration**: If you need to convert between different representations, create specific extension methods for those use cases.

## Validation Patterns

**Add validation methods to domain models**:

**Validation Method Pattern**:

- Method name: `Validate()`
- Throws `ArgumentException` for invalid state
- Validates required fields are not empty
- Validates numeric fields are in valid range
- Validates enum-like fields have allowed values

**Common Validations**:

- String fields: Check `IsNullOrWhiteSpace`
- Numeric fields: Check positive values
- Collections: Check minimum count
- Enums: Check allowed values

**See**: [Models/Core/ContentItem.cs](Models/Core/ContentItem.cs) `Validate()` method for implementation

## Testing

**See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md)** for comprehensive unit testing patterns for domain models and business logic.

## Related Documentation

### Functional Documentation (docs/)

- **[Frontmatter](../../docs/frontmatter.md)** - Frontmatter schema and field mappings to domain models
- **[Content API](../../docs/content-api.md)** - REST API contracts using these domain models
- **[Custom Pages](../../docs/custom-pages.md)** - Custom page ordering and configuration

### Implementation Guides (AGENTS.md)

- **[src/AGENTS.md](../AGENTS.md)** - Shared .NET patterns and code quality standards
- **[src/TechHub.Infrastructure/AGENTS.md](../TechHub.Infrastructure/AGENTS.md)** - Repository implementations that use these interfaces
- **[src/TechHub.Api/AGENTS.md](../TechHub.Api/AGENTS.md)** - API endpoints that consume these models
- **[tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md)** - Unit testing patterns
- **[Root AGENTS.md](../../AGENTS.md)** - Complete workflow and principles

---

**Remember**: Core layer stays clean and framework-agnostic. No dependencies, just pure domain logic.
