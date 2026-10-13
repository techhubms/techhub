# Infrastructure Tests

> **RULE**: Follow [tests/AGENTS.md](../AGENTS.md) for shared testing rules and [Root AGENTS.md](../../AGENTS.md) for workflow.

Unit and integration tests for the Infrastructure layer. Validates repositories, parsers, services, and data access.

## What to Test

- ✅ YAML front matter parsing (valid/invalid/malformed)
- ✅ Markdown rendering
- ✅ Repository operations (GetAll, GetById, filtering, sorting)
- ✅ Database interactions (PostgreSQL via Testcontainers)
- ✅ Caching behavior
- ✅ Error handling (missing markers, malformed data)

## What NOT to Test Here

- ❌ Domain logic (belongs in Core tests)
- ❌ API endpoints (belongs in API integration tests)
- ❌ Markdig internals (third-party library)

## Key Rules

- **Parallel execution**: Services that must be safe under concurrent use (for example `MarkdownService`, `RssService`) are tested with shared instances plus a parallel execution test. They are registered as Transient in production, so this guards against shared mutable state rather than a singleton lifetime. See [tests/AGENTS.md](../AGENTS.md#testing-singleton-services) for the actual singleton list.
- Content MUST be sorted by `DateEpoch` descending
- Always test with `Europe/Brussels` timezone
- Use `DatabaseFixture` with Testcontainers for repository tests (lives in this project)
- Use `StubHttpMessageHandler` (in `Services/`) to test HTTP clients without a network; `MigrationDatabaseFixture` (in `Data/`) hands out a fresh empty database per test for migration tests

## Test Data

Repository tests use the content in `tests/TechHub.TestUtilities/TestCollections/`, seeded into the Testcontainers database by `TestCollectionsSeeder` (see [TechHub.TestUtilities/AGENTS.md](../TechHub.TestUtilities/AGENTS.md)).
