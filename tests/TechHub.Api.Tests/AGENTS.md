# API Integration Tests

> **RULE**: Follow [tests/AGENTS.md](../AGENTS.md) for shared testing rules and [Root AGENTS.md](../../AGENTS.md) for workflow.

Integration tests for the REST API using xUnit and WebApplicationFactory, backed by PostgreSQL via Testcontainers.

## Test Infrastructure

- `TechHubApiFactory.cs` — Custom `WebApplicationFactory<Program>` with PostgreSQL Testcontainer (lives in this project)
- Test classes use `IClassFixture<TechHubIntegrationTestApiFactory>` to share the factory

## What to Test

- ✅ HTTP status codes (200, 404, 400)
- ✅ Response body structure (JSON deserialization)
- ✅ Query parameter handling (filtering, pagination)
- ✅ Error responses (validation errors, not found)
- ✅ HTTP pipeline (CORS, cache, security headers)

## What NOT to Test Here

- ❌ Business logic (belongs in Core unit tests)
- ❌ Repository internals (belongs in Infrastructure tests)
- ❌ UI rendering (belongs in E2E tests)

## Key Rules

- Don't mock repositories — use real implementations with Testcontainers
- Don't share state between tests
- Always verify HTTP status codes
