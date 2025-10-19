# Domain Interfaces

## Purpose

This folder contains all interface definitions that define contracts for repositories and services in the Tech Hub application. These interfaces enable dependency inversion and testability.

## Interfaces Overview

### IContentRepository

**Purpose**: Defines data access contract for content items

**Key Methods**:

- `GetByIdAsync(Guid id)`: Retrieve single content item by ID
- `GetAllAsync()`: Retrieve all content items
- `GetByCollectionAsync(string collectionType)`: Get content by collection
- `GetBySectionAsync(string sectionKey)`: Get content by section
- `GetByCategoriesAsync(List<string> categories)`: Filter by categories
- `GetByTagsAsync(List<string> tags)`: Filter by tags
- `GetPublishedSinceAsync(DateTimeOffset since)`: Get recent content
- `GetPublishedBetweenAsync(DateTimeOffset start, DateTimeOffset end)`: Date range query
- `AddAsync(ContentItem item)`: Add new content
- `UpdateAsync(ContentItem item)`: Update existing content
- `DeleteAsync(Guid id)`: Delete content by ID
- `ExistsAsync(string canonicalUrl)`: Check if content exists by URL

**Implementation Notes**:

- Repository pattern for data access abstraction
- Async/await for all operations
- Returns `Task<ContentItem?>` for nullable results
- Uses Guid for primary keys
- Supports filtering by multiple criteria

### IFilteringService

**Purpose**: Implements the Tech Hub filtering logic

**Key Methods**:

- `ApplyDateFilter(IEnumerable<ContentItem> items, DateFilterOption option)`: Apply date range filtering
- `ApplyTagFilter(IEnumerable<ContentItem> items, List<string> tags)`: Apply tag-based filtering with subset matching
- `ApplySectionFilter(IEnumerable<ContentItem> items, string sectionKey)`: Filter by section
- `ApplyCollectionFilter(IEnumerable<ContentItem> items, string collectionType)`: Filter by collection
- `ApplyTextSearch(IEnumerable<ContentItem> items, string searchQuery)`: Apply text search
- `ApplyContentLimiting(IEnumerable<ContentItem> items, int limit)`: Apply "N + Same-Day" rule
- `ApplyRecencyFilter(IEnumerable<ContentItem> items, int days)`: Apply 7-day recency filter
- `GetEligibleDateFilters(IEnumerable<ContentItem> items)`: Determine which date filters to show
- `CountByTags(IEnumerable<ContentItem> items, List<string> tags)`: Calculate tag filter counts
- `CountByDateFilter(IEnumerable<ContentItem> items, DateFilterOption option)`: Calculate date filter counts

**Filtering Rules**:

- Date filters are exclusive (only one active)
- Tag filters are inclusive with AND logic
- Text search is additive with other filters
- Content limiting per collection for fair representation
- Subset matching for tags (e.g., "AI" matches "Azure AI")

### ISearchService

**Purpose**: Provides text search functionality

**Key Methods**:

- `SearchAsync(string query)`: Full-text search across all content
- `SearchByTitleAsync(string query)`: Search in titles only
- `SearchByDescriptionAsync(string query)`: Search in descriptions only
- `SearchByAuthorAsync(string query)`: Search by author name
- `SearchByTagsAsync(string query)`: Search in tags
- `IndexContentAsync(ContentItem item)`: Add content to search index
- `RemoveFromIndexAsync(Guid id)`: Remove content from search index
- `RebuildIndexAsync()`: Rebuild entire search index

**Search Features**:

- Case-insensitive matching
- Partial word matching
- Real-time search with debouncing (client-side)
- Pre-indexed content for fast searches
- Searches across titles, descriptions, meta info, and tags

### ITagService

**Purpose**: Handles tag processing and normalization

**Key Methods**:

- `NormalizeTag(string display)`: Convert display format to normalized format
- `GetRelatedTags(string tag)`: Find tags that subset-match
- `BuildTagRelationships(IEnumerable<ContentItem> items)`: Pre-calculate tag relationships
- `GetTagCounts(IEnumerable<ContentItem> items)`: Count tag usage
- `GetMostUsedTags(int count)`: Get most frequently used tags
- `ValidateTag(string tag)`: Validate tag format and content

**Tag Processing**:

- Normalization: lowercase, special character handling
- Subset matching: word boundary checking
- Relationship pre-calculation for performance
- Count aggregation for filter display

### ISectionService

**Purpose**: Manages section and collection configuration

**Key Methods**:

- `GetAllSectionsAsync()`: Retrieve all sections
- `GetSectionByKeyAsync(string key)`: Get specific section
- `GetCollectionsBySectionAsync(string sectionKey)`: Get collections for section
- `GetCollectionByNameAsync(string sectionKey, string collectionName)`: Get specific collection
- `ValidateSectionStructure()`: Validate sections.json configuration

**Configuration Management**:

- Loads from sections.json equivalent
- Validates structure and references
- Provides section/collection lookup
- Supports dynamic configuration updates

## Design Patterns

### Repository Pattern

```csharp
public interface IContentRepository
{
    Task<ContentItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<ContentItem>> GetAllAsync();
    Task AddAsync(ContentItem item);
    Task UpdateAsync(ContentItem item);
    Task DeleteAsync(Guid id);
}
```

Benefits:

- Abstracts data access implementation
- Enables unit testing with mocks
- Separates domain logic from data access
- Supports multiple data sources

### Service Layer Pattern

```csharp
public interface IFilteringService
{
    IEnumerable<ContentItem> ApplyDateFilter(
        IEnumerable<ContentItem> items,
        DateFilterOption option);
}
```

Benefits:

- Encapsulates business logic
- Coordinates multiple repositories
- Implements domain rules
- Testable in isolation

## Dependency Injection

All interfaces are registered in the DI container:

```csharp
// In Program.cs
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IFilteringService, FilteringService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ISectionService, SectionService>();
```

## Testing Strategy

### Unit Testing Interfaces

Use mocking frameworks to test consumers:

```csharp
[Fact]
public async Task GetContent_ReturnsFilteredResults()
{
    // Arrange
    var mockRepo = new Mock<IContentRepository>();
    mockRepo.Setup(r => r.GetBySectionAsync("ai"))
        .ReturnsAsync(GetTestContent());
    
    var service = new ContentService(mockRepo.Object);
    
    // Act
    var result = await service.GetSectionContentAsync("ai");
    
    // Assert
    Assert.NotEmpty(result);
}
```

### Integration Testing

Test actual implementations:

```csharp
[Fact]
public async Task ContentRepository_CanRetrieveAndFilter()
{
    var repo = new ContentRepository(dbContext);
    var items = await repo.GetBySectionAsync("github-copilot");
    Assert.All(items, item => 
        Assert.Contains("GitHub Copilot", item.Categories));
}
```

## Implementation Guidelines

### Async/Await

- All data access methods are async
- Use `Task<T>` for single results
- Use `Task<IEnumerable<T>>` for collections
- Use `Task` for void operations

### Nullable Reference Types

- Use `ContentItem?` for nullable returns
- Use `string?` for optional parameters
- Enable nullable reference types in project

### Error Handling

- Throw specific exceptions (ArgumentException, InvalidOperationException)
- Use validation methods before operations
- Document exceptions in XML comments

### Performance Considerations

- Pre-calculate relationships where possible
- Use IEnumerable for lazy evaluation
- Implement caching strategies in implementations
- Optimize database queries

## Related Documentation

- [Domain Layer Overview](../AGENTS.md)
- [Domain Models](../Models/AGENTS.md)
- [Filtering System](../../../../../../docs/filtering-system.md)
