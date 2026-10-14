# API Integration Tests

> **RULE**: Follow [tests/AGENTS.md](../AGENTS.md) for shared testing rules and [Root AGENTS.md](../../AGENTS.md) for workflow.

Integration tests for the REST API using xUnit and WebApplicationFactory, backed by PostgreSQL via Testcontainers.

## Test Infrastructure

- `TechHubApiFactory.cs` — Contains `TechHubApiFactoryBase` (shared `WebApplicationFactory<Program>` setup) and `TechHubIntegrationTestApiFactory` (adds the PostgreSQL Testcontainer and seeds test data). Lives in this project
- Test classes use `IClassFixture<TechHubIntegrationTestApiFactory>` to share the factory
- `TechHubStubbedExternalServicesApiFactory.cs` — Derived factory that stubs the article fetch client and AI categorization (no network); the outcome is selected by a marker in the URL
- Tests that need a different configuration derive from `TechHubIntegrationTestApiFactory` as a nested fixture class (see `AdminAuthorizationTests`, which enables Azure AD with a header-driven auth scheme to test the real `AdminOnly` policy)
- `TestSupport/` — `ContentItemSeeder` (per-test data) and `Eventually` (bounded poll helper for background work with no completion signal; prefer a `TaskCompletionSource` where a hook exists)

## Structure

- `Endpoints/` — One test class per endpoint group (content, admin, GHC features, custom pages, and so on)
- `Middleware/` — API middleware tests
- `Services/` — Service tests that need the API's DI setup
- `Telemetry/` — Telemetry filter tests
- `HealthCheckTests.cs`, `HttpsRedirectionTests.cs` — Pipeline-level tests at the project root

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
