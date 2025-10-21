# Repository Implementations

## Purpose

This folder contains concrete implementations of the repository interfaces defined in `Domain/Interfaces`. These implementations provide the data access layer for the Tech Hub application.

## Current Implementation Strategy

### In-Memory Storage (Phase 1.2)

The initial implementation uses in-memory storage for rapid development and testing:

- **Simple List-based storage**: Uses `List<ContentItem>` for fast prototyping
- **Thread-safe operations**: Uses `lock` statements for concurrent access
- **Async-compliant**: All methods return `Task<T>` even though operations are synchronous
- **Full LINQ support**: Leverages LINQ for filtering and querying

### Future Database Migration (Phase 6)

The repository pattern enables easy migration to a real database:

- **Entity Framework Core**: Will replace in-memory storage
- **Same interface contracts**: No changes to consuming code
- **Database choice**: SQL Server, PostgreSQL, or CosmosDB
- **Migration strategy**: Swap implementation via dependency injection

## Repository Implementations

### InMemoryContentRepository

**Purpose**: Provides in-memory storage for all content types using the Repository Pattern.

**Storage Strategy**:
- Single `List<ContentItem>` stores all content (news, blogs, videos, etc.)
- Each content type (Video, Event, etc.) inherits from ContentItem
- LINQ queries filter by `CollectionType` property to separate content types

**Key Features**:
- **Thread Safety**: Uses `lock (_lock)` for concurrent access protection
- **LINQ Queries**: Efficient filtering using LINQ expressions
- **Validation**: Calls `item.Validate()` before adding/updating
- **ID Generation**: Simple incremental ID for demo purposes

**Methods Implemented**:
1. **GetByIdAsync**: Retrieves single item by ID
2. **GetAllAsync**: Returns all content items
3. **GetByCollectionAsync**: Filters by collection type (news, videos, etc.)
4. **GetBySectionAsync**: Filters by section category (AI, GitHub Copilot, etc.)
5. **GetByCategoriesAsync**: Filters by multiple categories (AND logic)
6. **GetByTagsAsync**: Filters by tags with subset matching support
7. **GetPublishedSinceAsync**: Date-based filtering from a start date
8. **GetPublishedBetweenAsync**: Date range filtering
9. **AddAsync**: Validates and adds new content
10. **UpdateAsync**: Validates and updates existing content
11. **DeleteAsync**: Removes content by ID
12. **ExistsAsync**: Checks if content exists by ID

**Example Usage**:
```csharp
// DI registration
services.AddSingleton<IContentRepository, InMemoryContentRepository>();

// Usage in service
public class ContentService
{
    private readonly IContentRepository _repository;
    
    public ContentService(IContentRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<ContentItem>> GetRecentNewsAsync()
    {
        var thirtyDaysAgo = DateTimeOffset.UtcNow.AddDays(-30);
        return await _repository.GetPublishedSinceAsync(thirtyDaysAgo);
    }
}
```

## Service Implementations

Service classes are located in `Domain/Services` and implement the service interfaces defined in `Domain/Interfaces`.

### FilteringService

**Purpose**: Implements all filtering logic including date filters, tag filters, content limiting, and recency filtering.

**Key Algorithms**:
- **"N + Same-Day" Rule**: Takes N items plus all items from the same day as the Nth item
- **7-Day Recency Filter**: Automatically excludes content older than 7 days
- **Tag Subset Matching**: Uses word boundary matching from `Tag.IsSubsetOf()`
- **Date Range Filtering**: Calculates eligible date filters based on content distribution

**Example Usage**:
```csharp
// Apply "20 + Same-Day" limiting
var limited = await _filteringService.ApplyContentLimiting(items, limit: 20);

// Apply tag filter with AND logic
var filtered = await _filteringService.ApplyTagFilter(items, ["ai", "github copilot"]);

// Apply 7-day recency filter
var recent = await _filteringService.ApplyRecencyFilter(items, days: 7);
```

### SearchService

**Purpose**: Provides full-text search across content items with indexing and real-time search capabilities.

**Search Strategy**:
- **Pre-indexed content**: Builds searchable strings on indexing
- **Case-insensitive**: All searches ignore case
- **Partial matching**: Supports substring matching
- **Multi-field search**: Searches across title, description, author, tags

**Example Usage**:
```csharp
// Full-text search
var results = await _searchService.SearchAsync("github copilot", items);

// Search by specific field
var titleResults = await _searchService.SearchByTitleAsync("azure", items);

// Index new content
await _searchService.IndexContentAsync(newItem);
```

### TagService

**Purpose**: Handles tag normalization, subset matching, and tag relationship building.

**Tag Processing**:
- **Normalization**: Converts tags to lowercase, handles special characters (C#, F#, +, ++)
- **Subset Matching**: Implements word boundary logic for "AI" matching "Azure AI"
- **Relationship Building**: Pre-calculates tag relationships for performance
- **Validation**: Ensures tags meet format requirements

**Example Usage**:
```csharp
// Normalize tag
var normalized = _tagService.NormalizeTag("C# Programming"); // "csharp programming"

// Check subset match
var isMatch = await _tagService.IsSubsetOf("ai", "azure ai"); // true

// Build tag relationships
var relationships = await _tagService.BuildTagRelationships(items);
```

### SectionService

**Purpose**: Manages section configuration, collection mappings, and validation.

**Section Management**:
- **Configuration Loading**: Loads from sections.json equivalent
- **Section Validation**: Ensures all sections have valid configuration
- **Collection Mapping**: Maps collections to appropriate sections
- **Category Mapping**: Links categories to sections

**Example Usage**:
```csharp
// Get all sections
var sections = await _sectionService.GetAllSectionsAsync();

// Get collections for a section
var collections = await _sectionService.GetCollectionsBySectionAsync("ai");

// Validate section structure
await _sectionService.ValidateSectionStructureAsync(section);
```

## Testing Strategy

### Unit Tests vs Integration Tests

**Unit Tests** (in `Tests/Domain/Models`):
- Test individual model validation
- Test tag normalization logic
- Test subset matching algorithms
- Mock dependencies, test in isolation

**Integration Tests** (in `Tests/Domain/Repositories`):
- Test repository CRUD operations
- Test service business logic
- Test repository + service interactions
- Use real implementations, test data flow

### Example Integration Test:
```csharp
[Fact]
public async Task GetByCollectionAsync_WithNewsCollection_ReturnsOnlyNews()
{
    // Arrange
    var repository = new InMemoryContentRepository();
    await repository.AddAsync(new NewsArticle { /* ... */ });
    await repository.AddAsync(new BlogPost { /* ... */ });
    
    // Act
    var news = await repository.GetByCollectionAsync("news");
    
    // Assert
    Assert.All(news, item => Assert.Equal("news", item.CollectionType));
}
```

## Dependency Injection Configuration

All repositories and services should be registered in `Program.cs`:

```csharp
// Repository registration
builder.Services.AddSingleton<IContentRepository, InMemoryContentRepository>();

// Service registration
builder.Services.AddScoped<IFilteringService, FilteringService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ISectionService, SectionService>();
```

**Lifetime Choices**:
- **Repository**: Singleton (shared in-memory storage)
- **Services**: Scoped (per-request lifecycle)

## Performance Considerations

### In-Memory Implementation

**Advantages**:
- ⚡ Extremely fast (no network/disk I/O)
- 🔄 Simple to understand and debug
- 🧪 Easy to test
- 🚀 Rapid prototyping

**Limitations**:
- 💾 Data lost on restart
- 📊 Not suitable for production scale
- 🔄 No persistence across deployments
- 🔒 Limited concurrent access patterns

### Migration to Database

When migrating to a real database:
1. Create new repository implementation (e.g., `EfCoreContentRepository`)
2. Implement same interface methods
3. Update DI registration in `Program.cs`
4. No changes needed to consuming code

**Example Migration**:
```csharp
// Before (Phase 1.2)
builder.Services.AddSingleton<IContentRepository, InMemoryContentRepository>();

// After (Phase 6)
builder.Services.AddScoped<IContentRepository, EfCoreContentRepository>();
builder.Services.AddDbContext<TechHubDbContext>(options =>
    options.UseSqlServer(connectionString));
```

## Best Practices

### Repository Pattern

1. **Single Responsibility**: Each repository manages one aggregate root
2. **Interface Segregation**: Repository interface defines only needed operations
3. **Dependency Inversion**: Consumers depend on IRepository, not concrete implementation

### Service Layer

1. **Business Logic**: Keep business rules in services, not repositories
2. **Orchestration**: Services coordinate multiple repository calls
3. **Validation**: Services validate before calling repositories

### Error Handling

1. **Validation Errors**: Throw `ArgumentException` for invalid input
2. **Not Found**: Return `null` for missing items (don't throw)
3. **Conflicts**: Throw `InvalidOperationException` for business rule violations

## Next Steps (Phase 1.3+)

After completing Phase 1.2:
1. **Phase 1.3**: Create API Controllers to expose repository operations via REST endpoints
2. **Phase 1.4**: Implement DTOs and mapping for API responses
3. **Phase 2+**: Build Blazor frontend to consume the API

The repository and service implementations in Phase 1.2 provide the foundation for all subsequent phases.
