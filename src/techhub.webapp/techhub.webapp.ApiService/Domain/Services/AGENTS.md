# Service Implementations

## Purpose

This folder contains concrete implementations of the service interfaces defined in `Domain/Interfaces`. Services contain business logic and orchestrate repository operations.

## Service Layer Architecture

### Separation of Concerns

**Repositories** (Data Access):
- CRUD operations
- Data persistence
- Query execution
- No business logic

**Services** (Business Logic):
- Filtering algorithms
- Search logic
- Tag processing
- Business rules enforcement
- Orchestration of multiple repository calls

### Service Implementations

This folder contains implementations for:
- `FilteringService` - Content filtering and limiting logic
- `SearchService` - Full-text search functionality
- `TagService` - Tag normalization and relationship building
- `SectionService` - Section configuration and validation

For detailed information about each service, see the main [Repositories AGENTS.md](../Repositories/AGENTS.md) file.

## Dependency Injection

Services are registered as **Scoped** in the DI container:

```csharp
builder.Services.AddScoped<IFilteringService, FilteringService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ISectionService, SectionService>();
```

**Why Scoped?**
- Services typically work with per-request data
- Ensures clean state for each HTTP request
- Allows for request-specific caching if needed
- Follows ASP.NET Core best practices

## Testing

Service implementations can be tested with:
- Unit tests using mocked repositories
- Integration tests using real InMemoryContentRepository

Example:
```csharp
// Unit test with mock
var mockRepo = new Mock<IContentRepository>();
var service = new FilteringService(mockRepo.Object);

// Integration test with real repository
var repo = new InMemoryContentRepository();
var service = new FilteringService(repo);
```
