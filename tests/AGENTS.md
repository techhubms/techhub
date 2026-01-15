# Testing Strategy for Tech Hub .NET

> **AI CONTEXT**: This is a **LEAF** context file for the `tests/` directory. It complements the [Root AGENTS.md](../AGENTS.md) and [src/AGENTS.md](../src/AGENTS.md).
> **RULE**: Global rules (Timezone, Performance, AI Assistant Workflow) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

You are a testing specialist for the Tech Hub .NET migration project. This directory contains all automated tests implementing a comprehensive testing pyramid strategy across multiple test layers: unit tests, integration tests, component tests, and end-to-end tests.

**🚨 Testing is Mandatory**: All testing requirements are defined in [Root AGENTS.md - Step 6: Write Tests First](../AGENTS.md#6-write-tests-first-tdd). Follow those instructions for when and how to test.

**See Project-Specific Test Patterns**: Each test project has its own AGENTS.md file with detailed patterns and examples. This file provides shared testing principles and navigation.

## Tech Stack

- **.NET 10**: Latest LTS runtime
- **xUnit 2.9.3**: Unit and integration test framework
- **Moq 4.20.72**: Mocking framework
- **FluentAssertions 7.0.0**: Assertion library
- **Microsoft.AspNetCore.Mvc.Testing 10.0.1**: API integration testing
- **bUnit 1.31.3**: Blazor component testing
- **Playwright .NET**: End-to-end testing
- **Pester**: PowerShell script testing
- **C# 13**: With nullable reference types enabled

## Running Tests

**ALWAYS refer to [Root AGENTS.md](../AGENTS.md#starting--stopping-the-website)** for complete instructions on:

- Running all tests with `Run -OnlyTests`
- Interactive debugging with `Run -SkipTests`
- Using Playwright MCP tools for testing
- Proper terminal management

**Quick command reference** (see root AGENTS.md for full details):

```powershell
Run -OnlyTests      # Run all tests and exit (for verification)
Run -SkipTests      # Skip tests, start servers (for debugging)
Run                 # Run tests, then keep servers running
```

**⚠️ CRITICAL E2E TEST WARNING**:

🚫 **NEVER run `dotnet test tests/TechHub.E2E.Tests` directly** - it **WILL FAIL** without servers running!
✅ **ALWAYS use `Run -OnlyTests`** which handles server startup, testing, and shutdown automatically.

## Core Testing Rules

These apply to ALL tests across all layers:

**✅ Always Do**:

- Write tests BEFORE or DURING implementation (TDD) - Never after
- Write regression test FIRST for bugs, then fix it
- Test real implementation - NEVER duplicate production logic in tests
- Mock only external dependencies (file system, HTTP clients, external APIs)
- Run tests after code changes: `Run -OnlyTests`
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

**Use [Theory] for testing multiple scenarios with same logic**:

```csharp
[Theory]
[InlineData("ai", "/ai/blogs/2024-01-15-test")]
[InlineData("github-copilot", "/github-copilot/blogs/2024-01-15-test")]
[InlineData("azure", "/azure/blogs/2024-01-15-test")]
public void GetUrlInSection_DifferentSections_ReturnsCorrectUrl(string sectionName, string expectedUrl)
{
    // Arrange
    var item = new ContentItem
    {
        Id = "2024-01-15-test",
        SectionNames = new[] { "ai", "github-copilot", "azure" },
        Collection = "blogs"
    };
    
    // Act
    var url = item.GetUrlInSection(sectionName);
    
    // Assert
    url.Should().Be(expectedUrl);
}
```

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
- `RssService` - RSS feed generation (stateless)### Test Fixtures (IClassFixture\<T\>)

**Use fixtures for expensive setup shared across tests**:

```csharp
// Fixture class - expensive setup
public class ContentRepositoryFixture : IDisposable
{
    public string TestDataPath { get; }
    public IMemoryCache Cache { get; }
    
    public ContentRepositoryFixture()
    {
        TestDataPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(TestDataPath);
        
        // Create test data files
        File.WriteAllText(
            Path.Combine(TestDataPath, "test-article.md"),
            "---\ntitle: Test\n---\nContent");
        
        Cache = new MemoryCache(new MemoryCacheOptions());
    }
    
    public void Dispose()
    {
        if (Directory.Exists(TestDataPath))
            Directory.Delete(TestDataPath, recursive: true);
        
        Cache.Dispose();
    }
}

// Test class using fixture
public class ContentRepositoryTests : IClassFixture<ContentRepositoryFixture>
{
    private readonly ContentRepositoryFixture _fixture;
    
    public ContentRepositoryTests(ContentRepositoryFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task GetAllAsync_WithTestData_ReturnsContent()
    {
        // Arrange
        var repo = new FileBasedContentRepository(
            _fixture.TestDataPath,
            _fixture.Cache,
            Mock.Of<ILogger<FileBasedContentRepository>>());
        
        // Act
        var items = await repo.GetAllAsync(CancellationToken.None);
        
        // Assert
        items.Should().HaveCount(1);
    }
}
```

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
      "BackgroundImage": "/images/ai.jpg",
      "Collections": ["news", "blogs", "videos"]
    }
  ]
}
```

### CORS Testing Pattern

**Verify CORS headers are properly configured**:

```csharp
[Fact]
public async Task GetSections_FromBrowserOrigin_IncludesCorsHeaders()
{
    // Arrange
    var request = new HttpRequestMessage(HttpMethod.Get, "/api/sections");
    request.Headers.Add("Origin", "https://tech.hub.ms");
    
    // Act
    var response = await _client.SendAsync(request);
    
    // Assert
    response.Should().BeSuccessful();
    response.Headers.Should().ContainKey("Access-Control-Allow-Origin");
    response.Headers.GetValues("Access-Control-Allow-Origin")
        .Should().Contain("https://tech.hub.ms");
}
```

### Response Caching Testing

**Verify cache headers are set correctly**:

```csharp
[Fact]
public async Task GetSections_CachedEndpoint_IncludesCacheHeaders()
{
    // Arrange
    // Act
    var response = await _client.GetAsync("/api/sections");
    
    // Assert
    response.Should().BeSuccessful();
    response.Headers.CacheControl.Should().NotBeNull();
    response.Headers.CacheControl!.MaxAge.Should().BeGreaterThan(TimeSpan.Zero);
}
```

### Middleware Pipeline Testing

**Test middleware is properly configured**:

```csharp
[Fact]
public async Task Api_Request_IncludesSecurityHeaders()
{
    // Arrange
    // Act
    var response = await _client.GetAsync("/api/sections");
    
    // Assert
    response.Headers.Should().ContainKey("X-Content-Type-Options");
    response.Headers.GetValues("X-Content-Type-Options")
        .Should().Contain("nosniff");
}
```

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

### Testing Pyramid

```text
        /\
       /  \     E2E Tests - Playwright .NET
      /____\    ← Slow, Few Tests, High Value
     /      \   
    /        \  Component Tests - bUnit
   /__________\ ← Medium Speed, Some Tests
  /            \
 /   API Tests  \ Integration Tests - WebApplicationFactory
/________________\ ← Medium-Fast, Most Tests of All
 \              /
  \            /  Unit Tests - xUnit
   \__________/   ← Fast, Many Tests, Quick Feedback
```

### Test Layer Mapping

| Layer           | Framework                     | Projects             | Purpose                                              | Dependencies |
|-----------------|-------------------------------|----------------------|------------------------------------------------------|--------------|
| **Unit**        | xUnit + Moq                   | Core, Infrastructure | Domain logic, services                               | Mocked       |
| **Integration** | xUnit + WebApplicationFactory | Api                  | API endpoints with mocked repositories/services      | Mocked       |
| **Component**   | bUnit                         | Web                  | Blazor component rendering & logic                   | Mocked       |
| **E2E**         | Playwright .NET + HttpClient  | E2E                  | Full user workflows, API tests with real dependencies| Real         |
| **PowerShell**  | Pester                        | powershell/          | Automation scripts                                   | Mocked       |

## Test Project Navigation

**Domain Model Tests** ([TechHub.Core.Tests/AGENTS.md](TechHub.Core.Tests/AGENTS.md)):

- Testing entity validation and business rules
- Testing value objects and DTOs
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
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
    <PackageReference Include="FluentAssertions" Version="7.0.0" />
    <PackageReference Include="Moq" Version="4.20.72" />
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

## Additional Resources

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Moq Documentation](https://github.com/devlooped/moq)
- [bUnit Documentation](https://bunit.dev/)
- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [Pester Documentation](https://pester.dev/)
