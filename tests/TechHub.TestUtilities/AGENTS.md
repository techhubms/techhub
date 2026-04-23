# TechHub.TestUtilities

Shared test infrastructure for all test projects.

## Test Data Strategy

| Test Type | Data Source | Database |
|-----------|-------------|----------|
| **Unit Tests** | Test builders (`A.ContentItem`, etc.) | None |
| **Integration Tests** | `TestCollections/` folder | PostgreSQL (Testcontainers) |
| **Infrastructure Tests** | `TestCollections/` folder | PostgreSQL (Testcontainers) |
| **E2E Tests (Playwright)** | Real data from local/deployed database | PostgreSQL (local or staging) |
| **Performance Tests** | Real data (~4000+ items) | PostgreSQL (local, skipped if unavailable) |

## Test Builders (`A` Pattern)

```csharp
using TechHub.TestUtilities.Builders;

var item = A.ContentItem.Build();                              // Defaults
var news = A.ContentItem.WithCollectionName("news").Build();   // Override
var section = A.Section.WithName("github-copilot").Build();
var collection = A.Collection.WithName("videos").Build();
```

**Defaults**: ContentItem → roundups collection, ai section. Section → ai with blogs/news. Collection → blogs.

## Factory Classes

### TechHubIntegrationTestApiFactory

- `IntegrationTest` environment with PostgreSQL Testcontainer (`postgres:17-alpine`)
- Seeds database from `TestCollections/` folder
- Usage: `IClassFixture<TechHubIntegrationTestApiFactory>`

### DatabaseFixture

- Repository-level integration tests with PostgreSQL Testcontainer
- Runs migrations and seeds automatically
- Usage: `IClassFixture<DatabaseFixture<MyTest>>`

## Configuration

- **Integration tests**: `appsettings.IntegrationTest.json`, content from `tests/TechHub.TestUtilities/TestCollections`
- **E2E tests**: Playwright browser tests against running servers (local or deployed via `E2E_BASE_URL`)
