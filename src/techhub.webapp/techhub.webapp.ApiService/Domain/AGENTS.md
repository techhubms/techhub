# Domain Layer

## Purpose

The Domain layer contains the core business logic and entity models for the Tech Hub application. This layer is framework-agnostic and represents the business domain without dependencies on external concerns like databases or UI frameworks.

## Structure

```text
Domain/
├── AGENTS.md           # This file
├── Models/             # Entity classes and domain models
│   ├── AGENTS.md       # Models documentation
│   ├── ContentItem.cs  # Base content item entity
│   ├── Section.cs      # Site section entity
│   ├── Collection.cs   # Content collection entity
│   ├── Tag.cs          # Tag entity with normalization
│   ├── NewsArticle.cs  # News article entity
│   ├── BlogPost.cs     # Blog post entity
│   ├── Video.cs        # Video content entity
│   ├── CommunityPost.cs # Community post entity
│   ├── Event.cs        # Event entity
│   └── Roundup.cs      # Content roundup entity
└── Interfaces/         # Repository and service contracts
    ├── AGENTS.md       # Interfaces documentation
    ├── IContentRepository.cs    # Content data access contract
    ├── IFilteringService.cs     # Filtering logic contract
    ├── ISearchService.cs        # Search functionality contract
    ├── ITagService.cs           # Tag processing contract
    └── ISectionService.cs       # Section management contract
```

## Key Concepts

### Content Hierarchy

1. **Section**: Top-level organizational unit (AI, GitHub Copilot, ML, Azure, .NET, DevOps, Security)
2. **Collection**: Content type within a section (news, videos, community, events, posts, roundups)
3. **ContentItem**: Base class for all content with common properties
4. **Specific Content Types**: Inherit from ContentItem with type-specific properties

### Domain Rules

#### Content Item Requirements

- All content must have a title, description, author, and publication date
- Canonical URL must be provided for external content
- Categories array maps to sections (e.g., ["AI", "GitHub Copilot"])
- Tags array contains technology keywords (minimum 3, recommended 10+)
- Permalink follows pattern: `/YYYY-MM-DD-title-slug.html`

#### Tag Normalization

- Display tags: Original casing and formatting (e.g., "Visual Studio Code")
- Normalized tags: Lowercase, standardized (e.g., "visual studio code")
- Tag relationships support subset matching (e.g., "AI" matches "Generative AI")

#### Date Handling

- All dates stored as DateTimeOffset for timezone consistency
- Publication dates determine content visibility and filtering
- Date filters use epoch timestamps for efficient comparison
- Brussels timezone (Europe/Brussels) is the reference timezone

#### Content Limiting

- **"20 + Same-Day" Rule**: Load 20 items + all items from same day as 20th item
- **7-Day Recency Filter**: Only show content from last 7 days
- Applied per collection for fair representation
- Ensures complete daily coverage

### Filtering Rules

#### Date Filters (Exclusive)

- Only one date filter active at a time
- Options: Today, Last 3 days, Last 7 days, Last 30 days, etc.
- Date filter counts remain independent
- Uses epoch timestamps for calculations

#### Tag Filters (Inclusive)

- Multiple tag filters can be active simultaneously
- Uses AND logic (intersection, not union)
- Supports subset matching (e.g., "AI" matches "Azure AI")
- Three types: Section tags, Collection tags, Content tags

#### Text Search

- Real-time filtering with 300ms debounce
- Searches across titles, descriptions, meta info, and tags
- Case-insensitive, partial word matching
- Works additively with date and tag filters

## Design Principles

### Domain-Driven Design

- **Entities**: Objects with unique identity (Section, ContentItem, Tag)
- **Value Objects**: Immutable objects defined by attributes (TagNormalization)
- **Aggregates**: Clusters of entities treated as single unit (Section + Collections)
- **Repositories**: Data access abstraction (IContentRepository)
- **Services**: Domain operations spanning multiple entities (IFilteringService)

### Separation of Concerns

- Domain models contain only business logic
- No database or framework dependencies
- Validation rules in model classes
- Data access abstracted through interfaces

### Immutability Where Appropriate

- Tag normalization results are immutable
- Filter configurations are immutable
- Domain events are immutable (future implementation)

## Dependencies

- **None**: Domain layer has no external dependencies
- Models use only .NET base types
- Interfaces define contracts without implementation

## Testing Strategy

Domain models and logic are tested through:

1. **Unit Tests**: Model validation, business rules, tag normalization
2. **Property Tests**: Tag subset matching, date filtering logic
3. **Integration Tests**: Repository contracts, service behavior

## Related Documentation

- [Jekyll Site Overview](../../../../../docs/site-overview.md)
- [Filtering System](../../../../../docs/filtering-system.md)
- [Terminology](../../../../../docs/terminology.md)
- [Content Management](../../../../../docs/content-management.md)

## Future Enhancements

- Domain events for content publication
- Aggregate root for Section + Collections
- Specification pattern for complex queries
- Value objects for Email, Url, Markdown content
