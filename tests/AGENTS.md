# Testing Strategy for Tech Hub .NET

> **AI CONTEXT**: This is a **LEAF** context file for the `tests/` directory. It complements the [Root AGENTS.md](../AGENTS.md) and [src/AGENTS.md](../src/AGENTS.md).
> **RULE**: Follow the 8-step workflow in Root [AGENTS.md](../AGENTS.md).

## Overview

You are a testing specialist for the Tech Hub .NET project. This directory contains all automated tests implementing a comprehensive **testing diamond strategy** across multiple test layers: unit tests, **integration tests** (the widest and most important layer), component tests, and end-to-end tests.

**🚨 Testing is Mandatory**: All testing requirements are defined in [Root AGENTS.md - Step 6: Write Tests First](../AGENTS.md#step-6-write-tests-first-tdd). Follow those instructions for when and how to test.

**See Project-Specific Test Patterns**: Each test project has its own AGENTS.md file with detailed patterns and examples. This file provides shared testing principles and navigation.

## Database Strategy

📖 **Full documentation**: See [docs/testing-strategy.md](../docs/testing-strategy.md#database-strategy) for database backends by test type.

**Summary**: Integration tests use SQLite in-memory for speed. E2E tests use SQLite on disk by default or PostgreSQL via docker-compose. Production uses Azure PostgreSQL.

## Core Testing Rules

These apply to ALL tests across all layers:

**✅ Always Do**:

- Write tests BEFORE or DURING implementation (TDD) - Never after
- Write regression test FIRST for bugs, then fix it
- Test real implementation - NEVER duplicate production logic in tests
- Mock only external dependencies (file system, HTTP clients, external APIs)
- Run tests after code changes: `Run` or `Run -TestProject <project>` to scope tests
- Use `async Task` for async tests - NEVER `async void`
- Test public APIs - Don't test implementation details
- Dispose resources in test cleanup
- Fix or remove flaky tests - NEVER ignore them
- Use `CancellationToken.None` in tests unless specifically testing cancellation

**⚠️ Ask First**:

- Adding new test dependencies or frameworks
- Changing test infrastructure (WebApplicationFactory, test fixtures)
- Skipping tests for specific scenarios

**🚫 Never Do**:

- Never duplicate production logic in tests
- Never copy production code into test files
- Never test implementation details (test public API)
- Never mock what you're testing (only mock dependencies)
- Never share mutable state between tests
- Never assume test execution order
- Never commit failing tests
- Never write tests without assertions
- Never use `async void` in tests (use `async Task`)
- Never skip test cleanup (dispose resources)
- Never ignore flaky tests (fix or remove)
- Never remove tests without removing unused production code

## Test Doubles Terminology

**Test Double** is the generic term for any pretend object used in place of a real object for testing purposes (like a stunt double in movies). There are several types:

**Stub** provides canned answers to calls made during tests. Stubs use **state verification** - you check the final state after the test runs. Example: `FileBasedContentRepository` pointing to TestCollections returns predefined test data and you assert on the results.

**Mock** is pre-programmed with expectations about which calls it should receive. Mocks use **behavior verification** - you verify that specific methods were called with expected parameters. Example: Using Moq to verify a method was called exactly once.

**Fake** has a working implementation but takes shortcuts unsuitable for production (e.g., in-memory database instead of real database).

**Spy** is a stub that also records information about how it was called (a hybrid approach).

**Dummy** is passed around but never actually used (just fills parameter lists).

**Key Difference**: **Stubs return data** (state verification), **Mocks verify behavior** (behavior verification). We prefer stubs for simple data provision and mocks for verifying interactions.

**Learn More**: [Mocks Aren't Stubs](https://martinfowler.com/articles/mocksArentStubs.html) by Martin Fowler

## Unit Testing Patterns

### AAA Pattern (Arrange-Act-Assert)

**ALWAYS structure tests with explicit AAA sections**:

```csharp
[Fact]
public void GetUrlInSection_ValidSection_ReturnsCorrectUrl()
{
    // Arrange
    var item = new ContentItem
    {
        Id = "2024-01-15-test-article",
        Title = "Test Article",
        SectionNames = new[] { "ai", "github-copilot" },
        Collection = "blogs",
        DateEpoch = 1705276800
    };
    
    // Act
    var url = item.GetUrlInSection("github-copilot");
    
    // Assert
    url.Should().Be("/github-copilot/blogs/2024-01-15-test-article");
}
```

**Key Rules**:

- Always use comments to mark AAA sections
- Arrange: Set up test data and dependencies
- Act: Execute ONE action being tested
- Assert: Verify ONE expected outcome (or multiple related assertions)

### Test Naming Convention

**Pattern**: `{MethodName}_{Scenario}_{ExpectedOutcome}`

**Examples**:

- `GetByIdAsync_ExistingId_ReturnsContent()`
- `GetByIdAsync_NonExistentId_ReturnsNull()`
- `GetBySectionAsync_EmptySection_ReturnsEmptyList()`
- `ParseFrontMatter_InvalidYaml_ThrowsFormatException()`

### Theory Tests for Parameterization

**Use `[Theory]` with `[InlineData]` for testing multiple scenarios with the same logic**. Reduces test duplication while maintaining clarity.

**When to use**:

- Testing the same method with different inputs
- Verifying boundary conditions
- Testing multiple valid/invalid cases

See actual tests in the codebase for examples.

### Testing Singleton Services

**🚨 CRITICAL**: If a service is registered as **Singleton** in production, tests MUST verify it can safely be used as a Singleton.

**Pattern**: Shared instance in tests mirrors production Singleton registration

**Why**: If someone adds mutable state to a Singleton service, tests should FAIL to catch the production-breaking bug.

**Example**: Testing `MarkdownService` (registered as Singleton in Program.cs)

```csharp
public class MarkdownServiceTests
{
    // Shared instance mirrors Singleton registration in production
    // If someone adds mutable state, parallel tests will fail
    private readonly MarkdownService _service;

    public MarkdownServiceTests()
    {
        // INTENTIONAL: Shared instance mimics production Singleton behavior
        // This will catch bugs if someone adds state to the service
        _service = new MarkdownService();
    }

    [Fact]
    public void RenderToHtml_BasicMarkdown_ConvertsToHtml()
    {
        // Arrange: Use shared instance
        var markdown = "# Heading";

        // Act: Shared instance must be stateless
        var html = _service.RenderToHtml(markdown);

        // Assert
        html.Should().Contain("<h1");
    }

    /// <summary>
    /// CRITICAL: This test verifies the service is stateless
    /// If someone adds mutable state, this test will fail randomly
    /// </summary>
    [Fact]
    public async Task RenderToHtml_ParallelExecution_ProducesConsistentResults()
    {
        // Arrange: Same markdown for all parallel calls
        var markdown = "# Test Heading";
        var expectedHtml = _service.RenderToHtml(markdown);

        // Act: Execute in parallel (would fail if service has mutable state)
        var tasks = Enumerable.Range(0, 100).Select(async _ =>
        {
            await Task.Yield(); // Force async execution
            return _service.RenderToHtml(markdown);
        });

        var results = await Task.WhenAll(tasks);

        // Assert: All results identical (proves stateless)
        results.Should().AllSatisfy(html => html.Should().Be(expectedHtml));
    }
}
```

**Key Rules**:

- ✅ **Mirror production registration** - Singleton in production = shared in tests
- ✅ **Add parallel execution test** - Catches mutable state bugs
- ✅ **Document why instance is shared** - Explain it mirrors production
- ❌ **Never add state to Singleton services** - Tests will fail

**Services Registered as Singleton** (see `src/TechHub.Api/Program.cs`):

- `MarkdownService` - Markdown rendering (stateless)
- `SectionRepository` - Section data access (uses caching)
- `ContentRepository` - Content data access (uses caching)
- `RssService` - RSS feed generation (stateless)

### Test Fixtures (IClassFixture\<T\>)

**Use fixtures for expensive setup shared across tests**.

**When to use**:

- Creating temp directories with test files
- Setting up shared caches or databases
- Any setup that's expensive and can be safely shared

**Key Rules**:

- Fixture class implements `IDisposable` for cleanup
- Test class uses `IClassFixture<TFixture>` interface
- Fixture is injected via constructor
- Each test class gets ONE shared fixture instance

See xUnit documentation for implementation details.

## Integration Testing Patterns

### Test Data File Conventions

**Pattern**: Use `*.test.json` files for test configuration:

```text
tests/TechHub.Api.Tests/
├── TestData/
│   ├── sections.test.json        # Section configuration for tests
│   ├── sample-content/           # Markdown files with frontmatter
│   │   ├── 2024-01-15-test-article.md
│   │   └── 2024-01-20-another-test.md
│   └── appsettings.test.json     # Test-specific app config
```

**sections.test.json structure**:

```json
{
  "Sections": [
    {
      "Name": "ai",
      "Title": "AI",
      "Description": "Test section for AI content",
      "Url": "ai",
      "Collections": ["news", "blogs", "videos"]
    }
  ]
}
```

### HTTP Pipeline Testing

Integration tests should verify HTTP pipeline configuration:

- **CORS headers**: Verify `Access-Control-Allow-Origin` is set for allowed origins
- **Cache headers**: Verify `Cache-Control` has appropriate `max-age` values
- **Security headers**: Verify `X-Content-Type-Options: nosniff` and other security headers

These tests send HTTP requests and assert on response headers. See actual tests for examples.

## Directory Structure

```text
tests/
├── AGENTS.md                           # This file - testing overview and navigation
├── Directory.Build.props               # Shared test project configuration
├── TechHub.Core.Tests/                 # Domain model unit tests
│   ├── AGENTS.md                       # Unit testing patterns for Core
│   └── Models/
├── TechHub.Infrastructure.Tests/       # Repository & service tests
│   ├── AGENTS.md                       # Unit testing patterns for Infrastructure
│   ├── FrontMatterParserTests.cs
│   ├── MarkdownServiceTests.cs
│   └── Repositories/
├── TechHub.Api.Tests/                  # API integration tests
│   ├── AGENTS.md                       # Integration testing patterns
│   ├── TechHubApiFactory.cs            # WebApplicationFactory
│   └── Endpoints/
├── TechHub.Web.Tests/                  # Blazor component tests
│   ├── AGENTS.md                       # Component testing patterns (bUnit)
│   └── Components/
├── TechHub.E2E.Tests/                  # Playwright E2E tests
│   ├── AGENTS.md                       # E2E testing patterns (Playwright)
│   └── Tests/
└── powershell/                         # PowerShell script tests
    ├── AGENTS.md                       # PowerShell testing patterns (Pester)
    └── *.Tests.ps1
```

## Testing Strategy

### Testing Diamond

The **testing diamond** approach prioritizes integration tests as the most valuable layer, with the API boundary being the crucial point where functionality is exposed and validated.

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

**Why Testing Diamond?**

1. **API is the crucial boundary**: Our API defines what functionality is exposed to users and other systems
2. **Integration tests catch real issues**: They test how components work together at the API boundary
3. **Unit tests for edge cases**: If code paths aren't exposed via the API, they matter less
4. **All functionality must be covered**: Integration + E2E tests verify all exposed functionality
5. **Unit tests for quick feedback**: Test edge cases, boundary conditions, and happy paths quickly

**Test Distribution Philosophy**:

- **Integration tests (widest layer)**: Every API endpoint, every feature exposed via API
- **Unit tests (narrower)**: Edge cases, boundary conditions, complex business logic, error handling
- **E2E tests (focused)**: Critical user journeys, complete workflows
- **Component tests**: UI component behavior, rendering, interactions

**Key Principle**: If a code path is NEVER exposed via the API, its test priority is lower. Focus testing effort on what users can actually trigger through the API.

### Test Layer Mapping

| Layer           | Framework                     | Projects             | Purpose                                              | Priority | External Dependencies | Local Dependencies (Filesystem) |
|-----------------|-------------------------------|----------------------|------------------------------------------------------|----------|----------------------|---------------------------------|
| **Integration** | xUnit + WebApplicationFactory | Api                  | **API endpoints with real internal services**        | **🔥 HIGHEST** | ❌ Stub/Mock         | ✅ Real (we control it)         |
| **Unit**        | xUnit + Stubs                 | Core, Infrastructure | Edge cases, complex logic, quick feedback            | **High** | ❌ NEVER             | ❌ NEVER                        |
| **E2E**         | Playwright .NET + HttpClient  | E2E                  | Critical user journeys, complete workflows           | **High** | ✅ Real              | ✅ Real                         |
| **Component**   | bUnit                         | Web                  | Blazor component rendering & logic                   | Medium | ❌ Stub/Mock         | ❌ Stub/Mock                    |
| **PowerShell**  | Pester                        | powershell/          | Automation scripts                                   | Medium | ❌ Mock              | ✅ Real (test files)            |

**Priority Explanation**:

- **Integration tests are the most important**: They validate the API boundary - what users and systems can actually access
- **Unit tests remain valuable**: Quick feedback for edge cases and complex business logic
- **E2E tests for critical paths**: Verify complete user workflows work end-to-end
- **All exposed functionality must be covered by integration + E2E tests**

## Understanding Test Layers - Detailed Definitions

### E2E Tests (End-to-End)

**Goal**: Test **critical user journeys** through the complete system to verify end-to-end workflows.

**Priority**: E2E tests validate **complete user workflows** and should focus on the most important user journeys rather than comprehensive coverage.

**What's Real**:

- ✅ Real API server running
- ✅ Real Web server running
- ✅ Real filesystem (actual markdown files)
- ✅ Real dependencies (all services, repositories)
- ✅ Real browser interactions (Playwright)

**What to Test**:

- ✅ **Critical user journeys** - Most important workflows users perform
- ✅ **Complete user workflows** - Navigation, filtering, search from start to finish
- ✅ **Browser interactions** - Clicks, forms, navigation
- ✅ **API endpoints with full dependency chain** - End-to-end validation
- ✅ **Visual rendering and responsiveness** - UI behaves correctly

**Coverage Philosophy**: **All functionality should be covered by integration + E2E tests combined**. E2E tests focus on critical paths while integration tests validate all API endpoints.

**Example**: `NavigationTests.cs` - User navigates from homepage → AI section → filters by tags → views content

**Location**: [tests/TechHub.E2E.Tests/](TechHub.E2E.Tests/)

---

### Integration Tests

**Goal**: Test **as real as possible** at the API boundary - the most important testing layer in the testing diamond.

**Why Integration Tests Are Most Important**:

1. **API is the contract**: The API defines what functionality is exposed to users and other systems
2. **Real component interaction**: Tests verify how services, repositories, and middleware work together
3. **Catch integration bugs**: Issues that only appear when components interact are caught here
4. **If it's not exposed via API, it's less critical**: Code paths never reachable through the API have lower priority

**✅ What's Real** (we control these):

- ✅ **Filesystem** - We control markdown files, test data
- ✅ **Internal services** - Real MarkdownService, real TagMatchingService
- ✅ **Real API pipeline** - Controllers, middleware, routing
- ✅ **Real data access** - FileBasedContentRepository loading actual files

**❌ What's Stubbed/Mocked** (external to our control):

- ❌ **Cloud services** - Azure Storage, databases (would use stubs)
- ❌ **Third-party APIs** - External HTTP calls (would use mocks)
- ❌ **Email services** - SMTP, SendGrid (would use mocks)

**Key Principle**: "External" means cloud or other systems outside our control. Filesystem is **NOT external** - we control it in tests.

**What to Test**:

- ✅ **Every API endpoint** - All functionality exposed via API must be tested
- ✅ **API endpoint contracts and responses** - Verify request/response structure
- ✅ **HTTP pipeline** - CORS, caching, security headers
- ✅ **Request validation and error handling** - Validate inputs, handle errors gracefully
- ✅ **Content loading from real markdown files** - Test with actual data
- ✅ **Integration of all internal services** - Verify components work together

**Coverage Requirement**: **All functionality exposed via the API must have integration test coverage**. If a feature can be accessed through an API endpoint, it must be tested at the integration layer.

**Example**: `SectionsEndpointTests.cs` - API endpoint loads real markdown files, returns sections

**Location**: [tests/TechHub.Api.Tests/](TechHub.Api.Tests/)

---

### Unit Tests

**Goal**: Test edge cases, boundary conditions, and happy paths quickly - NOT to test every code path.

**Core Philosophy**: **Unit tests are for quick feedback on specific scenarios**. Focus on:

1. **Edge cases** - Boundary conditions, null handling, empty collections
2. **Complex business logic** - Algorithms, calculations, transformations
3. **Happy paths** - Quick verification that basic functionality works
4. **Error handling** - Exception scenarios, validation failures

**What Unit Tests Are NOT For**:

- ❌ **Testing every possible code path** - If a path is never exposed via API, it's less critical
- ❌ **Duplicating integration test coverage** - Don't test the same thing twice
- ❌ **Testing implementation details** - Test public APIs only

**Key Principle**: **If code can break in scenarios only reproducible via unit tests, but these paths are NEVER exposed via the API - what does it matter?** Focus unit testing effort on scenarios that can actually happen in production through the API.

**Priority**: Unit tests provide **quick feedback** and test **specific edge cases**, but integration tests at the API boundary are more important for overall system validation.

**🎯 When to Use REAL Implementations**:

- ✅ **Simple classes without dependencies** - Any class that's just logic (e.g., `MarkdownService`, `TagMatchingService`)
- ✅ **Pure functions** - Methods with no side effects
- ✅ **Stateless services** - Services with no mutable state
- ✅ **Domain models** - Entities, value objects
- ✅ **In-memory collections** - Lists, dictionaries, etc.

**❌ When to Stub/Mock (ONLY These Cases)**:

**Local Dependencies** (filesystem):

- ❌ **File system access** - Classes that read/write files → Stub the repository/service that does I/O
- ❌ **Directory operations** - Classes that create/delete directories → Stub the abstraction

**External Dependencies** (systems outside our control):

- ❌ **HTTP calls** - HttpClient requests to external APIs → Mock HttpClient or use stub responses
- ❌ **Database access** - SQL queries, Entity Framework → Stub the repository
- ❌ **Cloud services** - Azure Storage, AWS S3 → Mock the SDK or use stub
- ❌ **Email services** - SMTP, SendGrid → Mock the email service
- ❌ **Third-party APIs** - Any external service → Mock or stub

**❌ NEVER Allowed in Unit Tests**:

- ❌ **Actual filesystem I/O** - NO `File.ReadAllText()`, `Directory.CreateDirectory()`
- ❌ **Actual HTTP requests** - NO real network calls
- ❌ **Actual database queries** - NO real SQL execution
- ❌ **Process spawning** - NO `Process.Start()`

**Key Principle**: **Test real code with real implementations**. Only create test doubles (stubs/mocks) at the **boundary** where your code would touch filesystem or external systems.

**Correct Example**: `TagCloudServiceTests.cs` tests `TagCloudService`:

```csharp
// ✅ CORRECT: Real service being tested
var service = new TagCloudService(repository, options);

// ✅ CORRECT: FileBasedContentRepository with TestCollections provides canned data
var repository = new FileBasedContentRepository(
    Options.Create(settings),  // ✅ CORRECT: Points to TestCollections directory
    markdownService,           // ✅ CORRECT: Real simple service
    tagMatchingService,        // ✅ CORRECT: Real simple service  
    environment,               // ✅ CORRECT: Real environment (or mock if needed)
    cache                      // ✅ CORRECT: Real MemoryCache
);
```

**Why**: We use `FileBasedContentRepository` pointing to TestCollections because it provides consistent test data without manual file creation. We use real `MarkdownService` and `TagMatchingService` because they're just simple classes with no filesystem/external access.

**Wrong Example** - Over-mocking:

```csharp
// ❌ WRONG: Mocking simple services unnecessarily
var mockMarkdownService = new Mock<IMarkdownService>();
var mockTagMatchingService = new Mock<ITagMatchingService>();

// ❌ WRONG: This makes tests fragile and tests nothing useful
mockMarkdownService.Setup(m => m.RenderToHtml(It.IsAny<string>()))
    .Returns("<p>Test</p>");
```

**Why Wrong**: `MarkdownService` is a simple class with no dependencies - just use the real one! Mocking it means you're not testing the actual Markdown rendering logic.

**Decision Tree - Should I Mock This?**:

```text
Does this class touch filesystem or external systems?
│
├─ YES → Use FileBasedContentRepository with TestCollections for IContentRepository
│
└─ NO → Use real implementation (e.g., MarkdownService)
    │
    ├─ Is it complex with many dependencies?
    │   └─ Still use real if dependencies are also simple
    │
    └─ Is it a simple class/function?
        └─ ALWAYS use real implementation
```

**Location**: [tests/TechHub.Core.Tests/](TechHub.Core.Tests/), [tests/TechHub.Infrastructure.Tests/](TechHub.Infrastructure.Tests/)

---

### Component Tests (Blazor)

**Goal**: Test Blazor component rendering and logic in isolation.

**What's Stubbed**: All services and repositories (use bUnit's dependency injection to provide stubs)

**What to Test**:

- Component markup rendering
- Parameter binding and cascading values
- Event handlers and callbacks
- Conditional rendering

**Example**: `SectionCardTests.cs` - Verifies section card renders with correct title, description, link

**Location**: [tests/TechHub.Web.Tests/](TechHub.Web.Tests/)

---

### PowerShell Tests

**Goal**: Test PowerShell scripts with mocked external commands.

**What's Real**: Test files on filesystem for validation

**What's Mocked**: External commands (git, dotnet, etc.)

**Location**: [tests/powershell/](powershell/)

## Test Project Navigation

**Domain Model Tests** ([TechHub.Core.Tests/AGENTS.md](TechHub.Core.Tests/AGENTS.md)):

- Testing entity validation and business rules
- Testing value objects and models
- Testing domain model behavior

**Repository & Service Tests** ([TechHub.Infrastructure.Tests/AGENTS.md](TechHub.Infrastructure.Tests/AGENTS.md)):

- Testing file-based repositories
- Testing markdown parsing and frontmatter
- Testing caching strategies
- Mocking file system dependencies

**API Integration Tests** ([TechHub.Api.Tests/AGENTS.md](TechHub.Api.Tests/AGENTS.md)):

- Testing API endpoints with **mocked dependencies** (repositories, services)
- Testing request/response contracts
- Using WebApplicationFactory with dependency injection overrides
- Testing error handling and resilience
- **Key difference from E2E**: Uses mocked repositories/services, not real file system

**Blazor Component Tests** ([TechHub.Web.Tests/AGENTS.md](TechHub.Web.Tests/AGENTS.md)):

- Testing component rendering
- Testing component parameters and cascading values
- Testing component events and callbacks
- Using bUnit test context

**End-to-End Tests** ([TechHub.E2E.Tests/AGENTS.md](TechHub.E2E.Tests/AGENTS.md)):

- Testing complete user workflows with **real dependencies** (actual file system, real data)
- Browser automation with Playwright for UI testing
- API tests using HttpClient against running API (real repositories, real services)
- Page object pattern
- Visual regression testing
- **Key difference from Integration**: Uses real file system, real repositories, real services

**PowerShell Script Tests** ([powershell/AGENTS.md](powershell/AGENTS.md)):

- Testing PowerShell automation scripts
- Using Pester framework
- Mocking external commands

## Cross-References to Source Code

When testing code from source projects, refer to implementation patterns:

- **Testing API code**: See [src/TechHub.Api/AGENTS.md](../src/TechHub.Api/AGENTS.md) for endpoint patterns
- **Testing Core code**: See [src/TechHub.Core/AGENTS.md](../src/TechHub.Core/AGENTS.md) for domain models
- **Testing Infrastructure code**: See [src/TechHub.Infrastructure/AGENTS.md](../src/TechHub.Infrastructure/AGENTS.md) for repository patterns
- **Testing Web code**: See [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) for Blazor component patterns

## Shared Testing Utilities

### Directory.Build.props

Shared configuration for all test projects (package versions, nullable reference types):

```xml
<Project>
  <Import Project="../Directory.Build.props" />
  
  <ItemGroup>
    <PackageReference Include="xunit" />
    <PackageReference Include="xunit.runner.visualstudio" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" />
    <PackageReference Include="FluentAssertions" />
    <PackageReference Include="Moq" />
  </ItemGroup>
</Project>
```

### Common Test Helpers

**Creating Test Data**: Use builder/factory patterns in test helper classes:

```csharp
// See TechHub.Core.Tests/AGENTS.md for entity creation patterns
// See TechHub.Infrastructure.Tests/AGENTS.md for file system test helpers
// See TechHub.Api.Tests/AGENTS.md for WebApplicationFactory configuration
```

## Related Documentation

### Functional Documentation (docs/)

- **[Testing Strategy](../docs/testing-strategy.md)** - Testing diamond, layer definitions, database strategy
- **[Database](../docs/database.md)** - Database providers for different test scenarios

### External Resources

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Moq Documentation](https://github.com/devlooped/moq)
- [bUnit Documentation](https://bunit.dev/)
- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [Pester Documentation](https://pester.dev/)
