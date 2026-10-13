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
var cache = A.SectionCache.Build();                           // Ready, all 7 sections
var cache = A.SectionCache.WithSections("ai").Build();        // Custom sections
var empty = A.SectionCache.Empty();                           // Not-ready cache
```

**Defaults**: ContentItem → roundups collection, ai section. Section → ai with blogs/news. Collection → blogs. SectionCache → the 7 real sections (ai, ml, devops, azure, dotnet, security, github-copilot) × 5 standard collections (news, videos, community, blogs, roundups), ready state. The production `appsettings.json` also defines an eighth `all` section (virtual aggregate with the `authors` custom collection); `SectionCacheBuilder` does not include it by default.

## Shared Utilities

- `TestCollectionsSeeder` — seeds a database from the `TestCollections/` folder using production sync logic
- `TestDataConstants` — expected counts for test data (used by Api.Tests + Infrastructure.Tests)
- `ConfigurationHelper` — loads `appsettings.json` into `AppSettings` for tests that need real config

## Configuration

- **Integration tests**: `appsettings.IntegrationTest.json`, content from `tests/TechHub.TestUtilities/TestCollections`
- **E2E tests**: Playwright browser tests against running servers (local or deployed via `E2E_BASE_URL`)
