# TechHub.TestUtilities

Shared test infrastructure for all test projects in the Tech Hub solution.

## Purpose

This project provides unified test utilities to avoid duplication across unit, integration, and E2E test projects.

## Test Data Strategy

Different test types use different data sources:

| Test Type | Repository | Data Source | Database |
|-----------|------------|-------------|----------|
| **Unit Tests** | Mock/Stub or In-Memory | Test data in code or builders | None |
| **Integration Tests** | `ContentRepository` | TestCollections folder | PostgreSQL (Testcontainers) |
| **Infrastructure Tests** | `ContentRepository` | TestCollections folder | PostgreSQL (Testcontainers) |
| **E2E Tests** | `ContentRepository` | Production `collections/` | PostgreSQL (docker-compose) |

### Unit Tests (Core/Infrastructure)

Unit tests should use mocks, stubs, or test builders to avoid database dependencies:

```csharp
// Use test builders for creating test data
var item = A.ContentItem
    .WithCollectionName("news")
    .WithExternalUrl("https://example.com/news")
    .Build();
```

**Why**: Fast, no database overhead, isolated testing of business logic.

### Integration Tests (API)

Use `TechHubIntegrationTestApiFactory` with PostgreSQL Testcontainer:

```csharp
public class MyTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;
    public MyTests(TechHubIntegrationTestApiFactory factory) => _client = factory.CreateClient();
}
```

**Why**: Tests full API pipeline with production-like PostgreSQL database, isolated per test run via Testcontainers.

### E2E Tests

Use `TechHubE2ETestApiFactory` with Development environment:

```csharp
public class MyE2ETests
{
    private readonly HttpClient _client;
    public MyE2ETests()
    {
        var factory = new TechHubE2ETestApiFactory();
        _client = factory.CreateClient();
    }
}
```

**Why**: Tests against production-like configuration with real database file.

## TechHubApiFactory Classes

### TechHubIntegrationTestApiFactory

- Uses **IntegrationTest** environment
- Implements `IAsyncLifetime` for container lifecycle
- Spins up a **PostgreSQL Testcontainer** (`postgres:17-alpine`) per factory instance
- Overrides `Database:Provider` and `Database:ConnectionString` via `UseSetting`
- Seeds database from `TestCollections/` folder using production sync logic
- Logs seeding progress and record counts

### TechHubE2ETestApiFactory

- Uses **Development** environment
- Uses real database file (`techhub.db`) with production content
- No service overrides - uses production configuration

## TestCollectionsSeeder

Seeds database from markdown files using production `ContentSyncService`:

```csharp
// Basic usage
await TestCollectionsSeeder.SeedFromFilesAsync(connection);

// With custom path and logging
await TestCollectionsSeeder.SeedFromFilesAsync(connection, customPath, logger);
```

**Features**:

- Uses actual production sync logic for test consistency
- Logs seeding progress: files synced, duration
- Logs record counts for all database tables

## DatabaseFixture

Fixture for repository-level integration tests:

```csharp
public class MyRepoTests : IClassFixture<DatabaseFixture<MyRepoTests>>
{
    public MyRepoTests(DatabaseFixture<MyRepoTests> fixture)
    {
        var repository = new ContentRepository(fixture.Connection, new PostgresDialect(), cache, markdownService, appSettings);
    }
}
```

**Features**:

- Implements `IAsyncLifetime` for container lifecycle
- Spins up a PostgreSQL Testcontainer (`postgres:17-alpine`) per fixture
- Runs PostgreSQL migrations automatically
- Seeds from TestCollections using production sync logic
- Logs database setup and record counts
- Exposes `ConnectionString` for additional connections

## Configuration

### Integration Test Configuration

Uses `appsettings.IntegrationTest.json`:

- API: `src/TechHub.Api/appsettings.IntegrationTest.json`
- Web: `src/TechHub.Web/appsettings.IntegrationTest.json`
- Content path: `/workspaces/techhub/tests/TechHub.TestUtilities/TestCollections`

### E2E Test Configuration

Uses `appsettings.Development.json`:

- Content path: `/workspaces/techhub/collections`
- Database: `/workspaces/techhub/techhub.db`

## Dependencies

- **Microsoft.AspNetCore.Mvc.Testing**: WebApplicationFactory support
- **Microsoft.Extensions.Logging**: Console logging for test output
- **TechHub.Api**: API program for factory
- **TechHub.Core**: Domain models and interfaces
- **TechHub.Infrastructure**: Repository and sync service implementations

## Test Builders (`A` Pattern)

Use the `A` pattern for creating test data with sensible defaults:

```csharp
using TechHub.TestUtilities.Builders;

// Simple usage - fully valid object with defaults
var item = A.ContentItem.Build();

// Override specific properties
var news = A.ContentItem
    .WithCollectionName("news")
    .WithExternalUrl("https://example.com/news")
    .Build();

// Section and Collection builders
var section = A.Section
    .WithName("github-copilot")
    .WithTitle("GitHub Copilot")
    .Build();

var collection = A.Collection
    .WithName("videos")
    .WithTitle("Videos")
    .Build();
```

**Key Benefits**:

- Sensible defaults for all required properties
- Only specify what's different for your test
- Fluent API with IntelliSense support
- Guaranteed valid objects (constructor validation)

**Default Values**:

- `ContentItem`: roundups collection (no ExternalUrl required), ai section, valid slug/title/author
- `Section`: ai section with blogs and news collections
- `Collection`: blogs collection with standard properties

**Available Builders**:

- `A.ContentItem` - ContentItem objects
- `A.Section` - Section objects
- `A.Collection` - Collection objects

## Related Documentation

### Functional Documentation (docs/)

- **[Testing Strategy](../../docs/testing-strategy.md)** - Testing diamond and database strategy
- **[Database](../../docs/database.md)** - Database providers used in tests

### Implementation Guides (AGENTS.md)

- **[tests/AGENTS.md](../AGENTS.md)** - Complete testing strategy and patterns
- **[tests/TechHub.Api.Tests/AGENTS.md](../TechHub.Api.Tests/AGENTS.md)** - API integration tests using these factories
- **[tests/TechHub.E2E.Tests/AGENTS.md](../TechHub.E2E.Tests/AGENTS.md)** - E2E tests using these factories
- **[Root AGENTS.md](../../AGENTS.md)** - Complete workflow and principles
