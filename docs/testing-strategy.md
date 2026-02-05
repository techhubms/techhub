# Testing Strategy

Tech Hub uses a **testing diamond** approach that prioritizes integration tests as the most valuable layer, with the API boundary being the crucial point where functionality is exposed and validated.

## Testing Diamond

```text
        /\
       /  \     E2E Tests - Playwright .NET
      /____\    ← Slow, Fewer Tests, Critical User Journeys
     /      \
    /        \  Component Tests - bUnit
   /__________\ ← Medium Speed, UI Component Behavior
  /            \
 / INTEGRATION  \ Integration Tests - WebApplicationFactory
/________________\ ← MOST IMPORTANT - API boundary validation
 \              /
  \   UNIT     /  Unit Tests - xUnit
   \__________/   ← Fast, Edge Cases & Happy Paths
```

## Why Testing Diamond?

1. **API is the crucial boundary**: Our API defines what functionality is exposed to users and other systems
2. **Integration tests catch real issues**: They test how components work together at the API boundary
3. **Unit tests for edge cases**: If code paths aren't exposed via the API, they matter less
4. **All functionality must be covered**: Integration + E2E tests verify all exposed functionality
5. **Unit tests for quick feedback**: Test edge cases, boundary conditions, and happy paths quickly

## Test Distribution Philosophy

| Layer | Focus | Priority |
|-------|-------|----------|
| **Integration** (widest) | Every API endpoint, every feature exposed via API | Highest |
| **Unit** (narrower) | Edge cases, boundary conditions, complex business logic | High |
| **E2E** (focused) | Critical user journeys, complete workflows | High |
| **Component** | UI component behavior, rendering, interactions | Medium |

**Key Principle**: If a code path is NEVER exposed via the API, its test priority is lower. Focus testing effort on what users can actually trigger through the API.

## Database Strategy

Tech Hub uses different database backends for different testing scenarios:

| Test Type | Database | Rationale |
|-----------|----------|-----------|
| **Integration Tests** | SQLite in-memory | Fast, isolated, no cleanup needed |
| **E2E Tests** | PostgreSQL (docker-compose) | Tests production architecture |
| **Local Development** | PostgreSQL OR SQLite | User choice (persistent data) |
| **Production** | Azure PostgreSQL | Managed, scalable, production-grade |

## Test Layer Definitions

### E2E Tests (End-to-End)

**Goal**: Test critical user journeys through the complete system.

**What's Real**:

- Real API server running
- Real Web server running
- Real filesystem (actual markdown files)
- Real dependencies (all services, repositories)
- Real browser interactions (Playwright)

**What to Test**:

- Critical user journeys (most important workflows)
- Complete user workflows (navigation, filtering, search)
- Browser interactions (clicks, forms, navigation)
- Visual rendering and responsiveness

**Framework**: Playwright .NET

### Integration Tests

**Goal**: Test as real as possible at the API boundary - the most important testing layer.

**What's Real** (we control these):

- Filesystem (we control markdown files, test data)
- Internal services (real MarkdownService, real TagMatchingService)
- Real API pipeline (controllers, middleware, routing)
- Real data access (repository loading actual files)

**What's Stubbed/Mocked** (external to our control):

- Cloud services (Azure Storage, databases)
- Third-party APIs (external HTTP calls)
- Email services (SMTP, SendGrid)

**What to Test**:

- Every API endpoint (all functionality exposed via API must be tested)
- API endpoint contracts and responses
- HTTP pipeline (CORS, caching, security headers)
- Request validation and error handling
- Content loading from real markdown files

**Coverage Requirement**: All functionality exposed via the API must have integration test coverage.

**Framework**: xUnit + WebApplicationFactory

### Unit Tests

**Goal**: Test edge cases, boundary conditions, and happy paths quickly.

**Core Philosophy**: Unit tests are for quick feedback on specific scenarios:

1. **Edge cases** - Boundary conditions, null handling, empty collections
2. **Complex business logic** - Algorithms, calculations, transformations
3. **Happy paths** - Quick verification that basic functionality works
4. **Error handling** - Exception scenarios, validation failures

**What Unit Tests Are NOT For**:

- Testing every possible code path
- Duplicating integration test coverage
- Testing implementation details (test public APIs only)

**Key Principle**: If code can break in scenarios only reproducible via unit tests, but these paths are NEVER exposed via the API - what does it matter? Focus on scenarios that can actually happen through the API.

**Framework**: xUnit

### Component Tests

**Goal**: Test Blazor component rendering and behavior.

**What to Test**:

- Component renders correctly with various inputs
- User interactions trigger expected behaviors
- Loading and error states display properly
- Component parameters work as expected

**Framework**: bUnit

## Test Layer Mapping

| Layer | Framework | Projects | External Dependencies | Local Dependencies (Filesystem) |
|-------|-----------|----------|----------------------|--------------------------------|
| **Integration** | xUnit + WebApplicationFactory | Api | Stub/Mock | Real (we control it) |
| **Unit** | xUnit + Stubs | Core, Infrastructure | NEVER | NEVER |
| **E2E** | Playwright .NET + HttpClient | E2E | Real | Real |
| **Component** | bUnit | Web | Stub/Mock | Stub/Mock |
| **PowerShell** | Pester | powershell/ | Mock | Real (test files) |

## Test Doubles Terminology

| Type | Purpose | Verification |
|------|---------|--------------|
| **Stub** | Provides canned answers to calls | State verification (check final state) |
| **Mock** | Pre-programmed with call expectations | Behavior verification (verify calls made) |
| **Fake** | Working implementation with shortcuts | State verification |
| **Spy** | Stub that records how it was called | Hybrid approach |
| **Dummy** | Passed around but never used | N/A |

**Key Difference**: Stubs return data (state verification), Mocks verify behavior (behavior verification). Prefer stubs for simple data provision and mocks for verifying interactions.

## When to Use Real vs Stub/Mock

### Use REAL Implementations

- Simple classes without dependencies
- Pure functions (no side effects)
- Stateless services
- Domain models (entities, value objects)
- In-memory collections

### Stub/Mock (Only These Cases)

**Local Dependencies** (filesystem):

- File system access (read/write files)
- Directory operations (create/delete directories)

**External Dependencies** (outside our control):

- HTTP calls to external APIs
- Database access (SQL queries, Entity Framework)
- Cloud services (Azure Storage, AWS S3)
- Email services (SMTP, SendGrid)
- Third-party APIs

### NEVER Allowed in Unit Tests

- Actual filesystem I/O (`File.ReadAllText()`, `Directory.CreateDirectory()`)
- Actual HTTP requests
- Actual database queries
- Process spawning (`Process.Start()`)

## Testing Singleton Services

If a service is registered as **Singleton** in production, tests MUST verify it can safely be used as a Singleton.

**Pattern**: Use shared instance in tests that mirrors production Singleton registration.

**Why**: If someone adds mutable state to a Singleton service, tests should FAIL to catch the production-breaking bug.

**Services Registered as Singleton**:

- `MarkdownService` - Markdown rendering (stateless)
- `SectionRepository` - Section data access (uses caching)
- `ContentRepository` - Content data access (uses caching)
- `RssService` - RSS feed generation (stateless)

## Implementation Reference

- **Test patterns by project**: [tests/AGENTS.md](../tests/AGENTS.md)
- **API integration tests**: [tests/TechHub.Api.Tests/AGENTS.md](../tests/TechHub.Api.Tests/AGENTS.md)
- **E2E tests**: [tests/TechHub.E2E.Tests/AGENTS.md](../tests/TechHub.E2E.Tests/AGENTS.md)
- **Component tests**: [tests/TechHub.Web.Tests/AGENTS.md](../tests/TechHub.Web.Tests/AGENTS.md)
