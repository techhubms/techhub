# Testing Strategy for Tech Hub .NET

> **AI CONTEXT**: This is a **LEAF** context file for the `tests/` directory. It complements the [Root AGENTS.md](../AGENTS.md) and [src/AGENTS.md](../src/AGENTS.md).
> **RULE**: Follow the 8-step workflow in Root [AGENTS.md](../AGENTS.md).

## Overview

This directory contains all automated tests implementing a **testing diamond strategy**: integration tests (widest, most important), unit tests, component tests, and E2E tests.

**Testing is Mandatory**: See [Root AGENTS.md - Step 4](../AGENTS.md#4-write-tests-before-implementing-changes-tdd) for when and how to test.

**Database Strategy**: Integration and infrastructure tests use PostgreSQL via Testcontainers (`postgres:17-alpine`). E2E tests use PostgreSQL via docker-compose. See [docs/testing-strategy.md](../docs/testing-strategy.md#database-strategy) for details.

## Core Testing Rules

**✅ Always Do**:

- Write tests BEFORE or DURING implementation (TDD) - Never after
- Write regression test FIRST for bugs, then fix it
- Test real implementation - NEVER duplicate production logic in tests
- Mock only external dependencies (file system, HTTP clients, external APIs)
- Run tests after code changes: `Run` or `Run -TestProject <project>` to scope tests
- Use `async Task` or `async ValueTask` for async tests - NEVER `async void`
- Test public APIs - Don't test implementation details
- Dispose resources in test cleanup
- Fix or remove flaky tests - NEVER ignore them
- Use `CancellationToken.None` in tests unless specifically testing cancellation
- Use AAA pattern (Arrange-Act-Assert) with explicit comments
- Name tests: `{MethodName}_{Scenario}_{ExpectedOutcome}`
- Use `[Theory]` with `[InlineData]` for parameterized scenarios
- Use FluentAssertions for readable assertions
- We prefer stubs (state verification) for data provision, mocks (behavior verification) for interaction verification

**⚠️ Ask First**:

- Adding new test dependencies or frameworks
- Changing test infrastructure (WebApplicationFactory, test fixtures)
- Skipping tests for specific scenarios

**🚫 Never Do**:

- Never duplicate production logic in tests
- Never test implementation details (test public API)
- Never mock what you're testing (only mock dependencies)
- Never share mutable state between tests
- Never assume test execution order
- Never commit failing tests
- Never remove tests without removing unused production code

## Testing Singleton Services

**CRITICAL**: If a service is registered as **Singleton** in production, tests MUST use a shared instance and include a parallel execution test to catch mutable state bugs.

**Singleton Services** (see `src/TechHub.Api/Program.cs`): `MarkdownService`, `SectionRepository`, `ContentRepository`, `RssService`

**Pattern**: Create shared instance in test constructor, add `ParallelExecution_ProducesConsistentResults` test. See `MarkdownServiceTests.cs` for the pattern.

## Test Fixtures

Use `IClassFixture<T>` for expensive shared setup (temp directories, databases). Fixture implements `IAsyncDisposable` for cleanup.

**xUnit v3 Assembly Fixtures**: Use `[assembly: AssemblyFixture(typeof(TFixture))]` to share a fixture across ALL test classes in an assembly.

## Testing Diamond

```text
        /\
       /  \     E2E Tests - Playwright .NET
      /____\    ← Critical User Journeys
     /      \
    /        \  Component Tests - bUnit
   /__________\ ← UI Component Behavior
  /            \
 / INTEGRATION  \ Integration Tests - WebApplicationFactory
/________________\ ← MOST IMPORTANT - API boundary validation
 \              /
  \   UNIT     /  Unit Tests - xUnit
   \__________/   ← Fast, Edge Cases & Happy Paths
```

**Key Principle**: Integration tests at the API boundary are the most important. All functionality exposed via API must have integration test coverage. Unit tests focus on edge cases. E2E tests validate critical user journeys.

### Test Layer Mapping

| Layer | Framework | Projects | Priority | What's Real | What's Mocked |
|---|---|---|---|---|---|
| **Integration** | xUnit v3 + WebApplicationFactory | Api | **HIGHEST** | Database, internal services, API pipeline | External APIs |
| **Unit** | xUnit v3 | Core, Infrastructure | High | Simple classes, pure functions, domain models | File I/O, HTTP, database |
| **E2E** | Playwright .NET + HttpClient | E2E | High | Everything (real servers, browser) | Nothing |
| **Component** | bUnit | Web | Medium | Component logic | All services |
| **JavaScript** | Vitest + jsdom | javascript/ | Medium | DOM interactions, module logic | Blazor interop, CDN scripts |
| **PowerShell** | Pester | powershell/ | Medium | Test files | External commands |

## What to Mock vs Use Real

**Use REAL implementations for**: Simple classes without dependencies, pure functions, stateless services, domain models.

**Mock/Stub ONLY**: File system I/O, HTTP calls to external APIs, database access (in unit tests — integration tests use real DB), cloud services, third-party APIs.

**NEVER in unit tests**: Actual filesystem I/O, HTTP requests, database queries, process spawning.

**Decision**: Does the class touch database or external systems? YES → use DatabaseFixture with Testcontainers. NO → use real implementation.

## Test Project AGENTS.md Files

Each test project has its own AGENTS.md with project-specific patterns:

- [TechHub.Core.Tests/AGENTS.md](TechHub.Core.Tests/AGENTS.md) — Domain model unit tests
- [TechHub.Infrastructure.Tests/AGENTS.md](TechHub.Infrastructure.Tests/AGENTS.md) — Repository & service tests
- [TechHub.Api.Tests/AGENTS.md](TechHub.Api.Tests/AGENTS.md) — API integration tests (WebApplicationFactory)
- [TechHub.Web.Tests/AGENTS.md](TechHub.Web.Tests/AGENTS.md) — Blazor component tests (bUnit)
- [TechHub.E2E.Tests/AGENTS.md](TechHub.E2E.Tests/AGENTS.md) — Playwright E2E tests
- [javascript/AGENTS.md](javascript/AGENTS.md) — Vitest JavaScript unit tests
- [powershell/AGENTS.md](powershell/AGENTS.md) — PowerShell Pester tests
- [TechHub.TestUtilities/AGENTS.md](TechHub.TestUtilities/AGENTS.md) — Shared test infrastructure, builders, factories
