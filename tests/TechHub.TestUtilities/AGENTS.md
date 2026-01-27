# TechHub.TestUtilities

Shared test infrastructure for all test projects in the Tech Hub solution.

## Purpose

This project provides unified test utilities to avoid duplication across unit, integration, and E2E test projects.

## Test Data Strategy

Different test types use different data sources:

| Test Type | Repository | Data Source | Database |
|-----------|------------|-------------|----------|
| **Unit Tests** | `FileBasedContentRepository` | TestCollections folder | None (in-memory cache) |
| **Integration Tests** | `SqliteContentRepository` | TestCollections folder | SQLite in-memory |
| **E2E Tests** | `SqliteContentRepository` | Production `collections/` | SQLite file (`techhub.db`) |

### Unit Tests (Core/Infrastructure)

Use `FileBasedContentRepository` pointing to `TestCollections/`:

```csharp
var testCollectionsPath = "/workspaces/techhub/tests/TechHub.TestUtilities/TestCollections";
_repository = new FileBasedContentRepository(
    Options.Create(new AppSettings { Content = new ContentSettings { CollectionsPath = testCollectionsPath } }),
    markdownService, tagMatchingService, mockEnvironment.Object, cache);
```

**Why**: Fast, no database overhead, tests file parsing and business logic.

### Integration Tests (API)

Use `TechHubIntegrationTestApiFactory` with SQLite in-memory database:

```csharp
public class MyTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;
    public MyTests(TechHubIntegrationTestApiFactory factory) => _client = factory.CreateClient();
}
```

**Why**: Tests full API pipeline with real database operations, isolated per test run.

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
- Creates **in-memory SQLite database** for each factory instance
- Seeds database from `TestCollections/` folder
- Thread-safe: Master connection keeps DB alive, repository is transient
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
        var repository = new SqliteContentRepository(fixture.Connection, dialect, cache);
    }
}
```

**Features**:

- Creates in-memory SQLite database
- Runs migrations automatically
- Seeds from TestCollections
- Logs database setup and record counts

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
