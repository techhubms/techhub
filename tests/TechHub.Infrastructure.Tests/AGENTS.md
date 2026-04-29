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

- **Singleton services** (`MarkdownService`, `RssService`, etc.) MUST be tested with shared instances + parallel execution test. See [tests/AGENTS.md](../AGENTS.md#testing-singleton-services).
- Content MUST be sorted by `DateEpoch` descending
- Always test with `Europe/Brussels` timezone
- Use `DatabaseFixture` with Testcontainers for repository tests

## Test Data

Tests use real content from the `TestCollections/` folder.
