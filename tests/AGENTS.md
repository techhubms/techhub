# Testing Strategy for Tech Hub .NET

> **AI CONTEXT**: This is a **LEAF** context file for the `tests/` directory. It complements the [Root AGENTS.md](/AGENTS.md) and [.NET Development Expert](/.github/agents/dotnet.md).
> **RULE**: Global rules (Timezone, Performance, AI Assistant Workflow) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Critical Testing Rules

### ✅ Always Do

- **Write tests BEFORE or DURING implementation** (TDD) - Never after
- **Write regression test FIRST** for bugs, then fix it
- **Test real implementation** - NEVER duplicate production logic in tests
- **Mock only external dependencies** (file system, HTTP clients, external APIs)
- **Run tests after code changes**: `dotnet test`
- **Use `async Task` for async tests** - NEVER `async void`
- **Test public APIs** - Don't test implementation details
- **Dispose resources** in test cleanup
- **Fix or remove flaky tests** - NEVER ignore them
- **Use `CancellationToken.None`** in tests unless specifically testing cancellation

### ⚠️ Ask First

- **Adding new test dependencies** or frameworks
- **Changing test infrastructure** (WebApplicationFactory, test fixtures)
- **Skipping tests** for specific scenarios

### 🚫 Never Do

- **Never duplicate production logic** in tests
- **Never copy production code** into test files
- **Never test implementation details** (test public API)
- **Never mock what you're testing** (only mock dependencies)
- **Never share mutable state** between tests
- **Never assume test execution order**
- **Never commit failing tests**
- **Never write tests without assertions**
- **Never use `async void`** in tests (use `async Task`)
- **Never skip test cleanup** (dispose resources)
- **Never ignore flaky tests** (fix or remove)
- **Never remove tests without removing unused production code**

## Overview

You are a testing specialist for the Tech Hub .NET migration project. This directory contains all automated tests implementing a comprehensive testing pyramid strategy across multiple test layers: unit tests, integration tests, component tests, and end-to-end tests.

**🚨 Testing is Mandatory**: All testing requirements are defined in the [Root AGENTS.md - Step 6: Test & Validate](/AGENTS.md#6-test--validate) workflow step. Follow those instructions for when and how to test.

**Framework-Specific Guidelines**: This file provides .NET-specific testing patterns and tools for xUnit (unit & integration), bUnit (Blazor components), and Playwright (E2E).

## Tech Stack

- **.NET 10**: Latest LTS runtime
- **xUnit 2.9.3**: Unit and integration test framework
- **NSubstitute 5.3.0**: Mocking framework
- **FluentAssertions 7.0.0**: Assertion library
- **Microsoft.AspNetCore.Mvc.Testing 10.0.1**: API integration testing
- **bUnit 1.31.3**: Blazor component testing (future)
- **Playwright .NET**: End-to-end testing (future)
- **C# 13**: With nullable reference types enabled

## Directory Structure

```text
tests/
├── AGENTS.md                          # This file
├── Directory.Build.props              # Shared test project configuration
├── TechHub.Core.Tests/               # Domain model unit tests
│   ├── TechHub.Core.Tests.csproj
│   └── Models/
│       └── ContentItemTests.cs
├── TechHub.Infrastructure.Tests/      # Repository & service tests
│   ├── TechHub.Infrastructure.Tests.csproj
│   ├── FrontMatterParserTests.cs
│   ├── MarkdownServiceTests.cs
│   └── Repositories/
│       ├── SectionRepositoryTests.cs
│       └── ContentRepositoryTests.cs
├── TechHub.Api.Tests/                # API integration tests
│   ├── TechHub.Api.Tests.csproj
│   ├── TechHubApiFactory.cs          # WebApplicationFactory
│   └── Endpoints/
│       ├── SectionsEndpointsTests.cs
│       └── ContentEndpointsTests.cs
├── TechHub.Web.Tests/                # Blazor component tests (future)
│   ├── TechHub.Web.Tests.csproj
│   └── Components/
│       └── (bUnit tests - to be added)
├── TechHub.E2E.Tests/                # Playwright E2E tests (future)
│   ├── TechHub.E2E.Tests.csproj
│   └── Tests/
│       └── (Playwright tests - to be added)
└── powershell/                        # PowerShell script tests
    ├── AGENTS.md
    └── *.Tests.ps1
```

## Testing Strategy

### Testing Pyramid

```text
        /\
       /  \     E2E Tests (Future) - Playwright .NET
      /____\    ← Slow, Few Tests, High Value
     /      \   
    /        \  Component Tests (Future) - bUnit
   /__________\ ← Medium Speed, Some Tests
  /            \
 /   API Tests  \ Integration Tests - WebApplicationFactory
/________________\ ← Medium-Fast, Most Tests of All
 \              /
  \            /  Unit Tests - xUnit
   \__________/   ← Fast, Many Tests, Quick Feedback
```

### Test Layer Mapping

| Layer | Framework | Projects | Purpose |
|-------|-----------|----------|---------|
| **Unit** | xUnit | Core, Infrastructure | Domain logic, services |
| **Integration** | xUnit + WebApplicationFactory | Api | HTTP endpoints, full request/response |
| **Component** | bUnit | Web (future) | Blazor component rendering & logic |
| **E2E** | Playwright .NET | E2E (future) | Full user workflows |
| **PowerShell** | Pester | powershell/ | Automation scripts |

## Unit Testing (xUnit)

Unit tests verify isolated domain logic, services, and algorithms without external dependencies.

### File Organization

```text
TechHub.Core.Tests/
└── Models/
    └── ContentItemTests.cs

TechHub.Infrastructure.Tests/
├── FrontMatterParserTests.cs
├── MarkdownServiceTests.cs
└── Repositories/
    ├── SectionRepositoryTests.cs
    └── ContentRepositoryTests.cs
```

### Test Pattern

```csharp
namespace TechHub.Core.Tests.Models;

public class ContentItemTests
{
    [Fact]
    public void GetUrlInSection_ReturnsCorrectFormat()
    {
        // Arrange
        var item = new ContentItem
        {
            Id = "vs-code-107",
            Collection = "videos",
            Title = "Test",
            Description = "Test",
            Author = "Test",
            DateEpoch = 1234567890,
            Categories = [],
            Tags = [],
            Content = "Test",
            Excerpt = "Test",
            CanonicalUrl = "/ai/videos/vs-code-107.html"
        };
        
        // Act
        var url = item.GetUrlInSection("ai");
        
        // Assert
        url.Should().Be("/ai/videos/vs-code-107.html");
    }
    
    [Theory]
    [InlineData("ai", "/ai/videos/test.html")]
    [InlineData("github-copilot", "/github-copilot/videos/test.html")]
    [InlineData("ml", "/ml/videos/test.html")]
    public void GetUrlInSection_WithDifferentSections_ReturnsCorrectUrls(
        string sectionUrl,
        string expectedUrl)
    {
        // Arrange
        var item = CreateTestContentItem(collection: "videos", id: "test");
        
        // Act
        var url = item.GetUrlInSection(sectionUrl);
        
        // Assert
        url.Should().Be(expectedUrl);
    }
    
    private static ContentItem CreateTestContentItem(
        string collection = "news",
        string id = "test-item")
    {
        return new ContentItem
        {
            Id = id,
            Collection = collection,
            Title = "Test",
            Description = "Test",
            Author = "Test",
            DateEpoch = 1234567890,
            Categories = ["ai"],
            Tags = ["test"],
            Content = "Test",
            Excerpt = "Test",
            CanonicalUrl = $"/ai/{collection}/{id}.html"
        };
    }
}
```

### Testing with Mocks (NSubstitute)

```csharp
public class SectionServiceTests
{
    [Fact]
    public async Task GetAllSectionsAsync_ReturnsCachedData()
    {
        // Arrange
        var repository = Substitute.For<ISectionRepository>();
        var sections = new List<Section>
        {
            new() { Id = "ai", Title = "AI", /* ... */ }
        };
        repository.GetAllSectionsAsync(Arg.Any<CancellationToken>())
            .Returns(sections);
        
        var service = new SectionService(repository);
        
        // Act
        var result = await service.GetAllSectionsAsync(CancellationToken.None);
        
        // Assert
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(sections);
        await repository.Received(1).GetAllSectionsAsync(Arg.Any<CancellationToken>());
    }
}
```

### FluentAssertions Patterns

```csharp
// Collections
items.Should().NotBeNull();
items.Should().HaveCount(5);
items.Should().BeEmpty();
items.Should().Contain(x => x.Id == "test");
items.Should().NotContain(x => x.Id == "invalid");
items.Should().AllSatisfy(x => x.DateEpoch.Should().BeGreaterThan(0));

// Strings
result.Should().Be("expected");
result.Should().NotBeNullOrEmpty();
result.Should().Contain("substring");
result.Should().StartWith("prefix");
result.Should().Match("/ai/*/test.html");

// Booleans
result.Should().BeTrue();
result.Should().BeFalse();

// Nullability
result.Should().NotBeNull();
result.Should().BeNull();

// Exceptions
var act = () => Method();
act.Should().Throw<ArgumentNullException>()
    .WithParameterName("parameter");

// Objects
result.Should().BeEquivalentTo(expected);
result.Should().BeOfType<ContentItem>();

// Async
await act.Should().ThrowAsync<InvalidOperationException>();
```

## API Integration Testing (WebApplicationFactory)

Integration tests verify HTTP endpoints with mocked file system dependencies but real request/response pipeline.

### Current Test Coverage

- **Total Tests**: 40
- **Pass Rate**: 100%
- **Execution Time**: ~1.4 seconds

See [docs/testing-summary.md](/docs/testing-summary.md) for complete coverage details.

### TechHubApiFactory Pattern

```csharp
// TechHubApiFactory.cs
public class TechHubApiFactory : WebApplicationFactory<Program>
{
    public ISectionRepository SectionRepository { get; } = Substitute.For<ISectionRepository>();
    public IContentRepository ContentRepository { get; } = Substitute.For<IContentRepository>();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove real repositories
            services.RemoveAll<ISectionRepository>();
            services.RemoveAll<IContentRepository>();
            
            // Add mocked repositories
            services.AddSingleton(_ => SectionRepository);
            services.AddSingleton(_ => ContentRepository);
        });
    }
    
    public void SetupTestData()
    {
        // Set up sections
        var sections = new List<Section>
        {
            new()
            {
                Id = "ai",
                Title = "AI",
                Url = "ai",
                Category = "AI",
                Description = "AI content",
                Collections = ["news", "blogs", "videos"]
            },
            new()
            {
                Id = "github-copilot",
                Title = "GitHub Copilot",
                Url = "github-copilot",
                Category = "GitHub Copilot",
                Description = "GitHub Copilot content",
                Collections = ["news", "blogs"]
            }
        };
        
        SectionRepository.GetAllSectionsAsync(Arg.Any<CancellationToken>())
            .Returns(sections);
        
        // Set up content items
        var items = new List<ContentItem>
        {
            // Test data here
        };
        
        ContentRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(items);
    }
}
```

### Test Pattern

```csharp
namespace TechHub.Api.Tests.Endpoints;

public class SectionsEndpointsTests : IClassFixture<TechHubApiFactory>
{
    private readonly HttpClient _client;
    private readonly TechHubApiFactory _factory;
    
    public SectionsEndpointsTests(TechHubApiFactory factory)
    {
        _factory = factory;
        _factory.SetupTestData();
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllSections_Returns200_WithSections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var sections = await response.Content.ReadFromJsonAsync<List<SectionDto>>();
        sections.Should().NotBeNull();
        sections.Should().HaveCount(2);
        sections.Should().Contain(s => s.Id == "ai");
    }
    
    [Theory]
    [InlineData("ai", "AI")]
    [InlineData("github-copilot", "GitHub Copilot")]
    public async Task GetSectionById_ReturnsSection(string id, string expectedCategory)
    {
        // Act
        var response = await _client.GetAsync($"/api/sections/{id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var section = await response.Content.ReadFromJsonAsync<SectionDto>();
        section.Should().NotBeNull();
        section!.Id.Should().Be(id);
        section.Category.Should().Be(expectedCategory);
    }
    
    [Fact]
    public async Task GetSectionById_InvalidSection_Returns404()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/invalid");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
```

### Testing Complex Filters

```csharp
[Fact]
public async Task FilterContent_ComplexFilter_ReturnsFilteredItems()
{
    // Arrange - set up test data with specific characteristics
    var items = new List<ContentItem>
    {
        new()
        {
            Id = "item1",
            Categories = ["ai"],
            Collection = "news",
            Tags = ["copilot", "azure"],
            // ... other properties
        },
        new()
        {
            Id = "item2",
            Categories = ["ai", "github-copilot"],
            Collection = "blogs",
            Tags = ["copilot"],
            // ... other properties
        }
    };
    
    _factory.ContentRepository.GetAllAsync(Arg.Any<CancellationToken>())
        .Returns(items);
    
    // Act - complex filter: section AND collection AND tags
    var response = await _client.GetAsync(
        "/api/content/filter?sections=ai&collections=news&tags=copilot,azure"
    );
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    
    var result = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
    result.Should().HaveCount(1);
    result![0].Id.Should().Be("item1");
}
```

## Component Testing (bUnit - Future)

Component tests will verify Blazor component rendering, parameters, events, and interactions using bUnit.

### Planned Structure

```text
TechHub.Web.Tests/
├── Components/
│   ├── Layout/
│   │   └── SectionHeaderTests.cs
│   ├── Pages/
│   │   ├── SectionIndexTests.cs
│   │   └── HomeTests.cs
│   └── Shared/
│       ├── SectionCardTests.cs
│       └── ContentListTests.cs
└── TestContext.cs
```

### Planned Pattern (Reference)

```csharp
// Future bUnit test pattern
public class SectionCardTests : TestContext
{
    [Fact]
    public void SectionCard_RendersTitle()
    {
        // Arrange
        var section = new SectionDto
        {
            Title = "GitHub Copilot",
            Url = "github-copilot",
            // ... other properties
        };
        
        // Act
        var cut = RenderComponent<SectionCard>(parameters =>
            parameters.Add(p => p.Section, section));
        
        // Assert
        cut.Find("h2").TextContent.Should().Be("GitHub Copilot");
    }
    
    [Fact]
    public void SectionCard_LinksToSectionPage()
    {
        // Arrange
        var section = new SectionDto { Url = "ai", /* ... */ };
        
        // Act
        var cut = RenderComponent<SectionCard>(parameters =>
            parameters.Add(p => p.Section, section));
        
        // Assert
        var link = cut.Find("a");
        link.GetAttribute("href").Should().Be("/ai");
    }
}
```

## E2E Testing (Playwright .NET - Future)

End-to-end tests will verify complete user workflows across the full application stack using Playwright .NET.

### Planned Structure

```text
TechHub.E2E.Tests/
├── Tests/
│   ├── NavigationTests.cs
│   ├── FilteringTests.cs
│   └── SearchTests.cs
├── PageObjects/
│   ├── HomePage.cs
│   ├── SectionPage.cs
│   └── CollectionPage.cs
└── PlaywrightFixture.cs
```

### Planned Pattern (Reference)

```csharp
// Future Playwright .NET test pattern
[Collection("Playwright")]
public class NavigationTests : IAsyncLifetime
{
    private IPage _page = default!;
    private IBrowser _browser = default!;
    
    public async Task InitializeAsync()
    {
        var playwright = await Playwright.CreateAsync();
        _browser = await playwright.Chromium.LaunchAsync();
        _page = await _browser.NewPageAsync();
    }
    
    [Fact]
    public async Task HomePage_DisplaysSections()
    {
        // Navigate
        await _page.GotoAsync("https://localhost:5173");
        
        // Verify sections grid
        var sectionsGrid = await _page.QuerySelectorAsync(".category-grid");
        Assert.NotNull(sectionsGrid);
        
        // Verify section cards
        var sectionCards = await _page.QuerySelectorAllAsync(".section-card");
        sectionCards.Should().HaveCountGreaterThanOrEqualTo(7);
    }
    
    [Fact]
    public async Task FilteringSystem_FiltersContentByTag()
    {
        // Navigate to section
        await _page.GotoAsync("https://localhost:5173/ai");
        
        // Click tag filter
        await _page.ClickAsync("[data-filter='copilot']");
        
        // Verify filtering
        var visibleItems = await _page.QuerySelectorAllAsync(".content-item:visible");
        visibleItems.Should().NotBeEmpty();
        
        // Verify URL updated
        _page.Url.Should().Contain("filter=copilot");
    }
    
    public async Task DisposeAsync()
    {
        await _page.CloseAsync();
        await _browser.CloseAsync();
    }
}
```

## PowerShell Testing (Pester)

PowerShell tests verify automation scripts in `scripts/` directory using Pester v5.

See [tests/powershell/AGENTS.md](/tests/powershell/AGENTS.md) for detailed PowerShell testing guidelines.

### Quick Reference

```powershell
# Run all PowerShell tests

./scripts/run-powershell-tests.ps1

# Run specific test file

./scripts/run-powershell-tests.ps1 -TestFile "tests/powershell/MyScript.Tests.ps1"

# With coverage

./scripts/run-powershell-tests.ps1 -Coverage
```

## Running Tests

### Run All Tests

```bash
# All .NET tests (unit + integration)

dotnet test

# Specific test project

dotnet test tests/TechHub.Core.Tests
dotnet test tests/TechHub.Infrastructure.Tests
dotnet test tests/TechHub.Api.Tests

# With detailed output

dotnet test --logger "console;verbosity=detailed"
```

### Run Specific Tests

```bash
# Run specific test class

dotnet test --filter "FullyQualifiedName~SectionsEndpointsTests"

# Run specific test method

dotnet test --filter "FullyQualifiedName~GetAllSections_Returns200_WithSections"

# Run all tests in namespace

dotnet test --filter "FullyQualifiedName~TechHub.Api.Tests.Endpoints"
```

### Coverage Reports

```bash
# Collect coverage

dotnet test --collect:"XPlat Code Coverage"

# Coverage is output to

# tests/{ProjectName}/TestResults/{guid}/coverage.cobertura.xml

# Use ReportGenerator for HTML reports (future)

dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report"
```

### Watch Mode

```bash
# Auto-run tests on file changes (requires dotnet-watch)

dotnet watch test --project tests/TechHub.Core.Tests
```

## When to Use Which Test Type

### Unit Tests (xUnit)

**Use For**:

- Domain model behavior (`ContentItem.GetUrlInSection()`)
- Service algorithms (markdown processing, front matter parsing)
- Pure functions without external dependencies
- Validation logic
- Data transformations

**Avoid**:

- File system I/O (mock instead)
- HTTP calls (mock instead)
- Database operations (mock instead)
- UI rendering

**Current Projects**:

- TechHub.Core.Tests
- TechHub.Infrastructure.Tests

### API Integration Tests (WebApplicationFactory)

**Use For**:

- HTTP endpoint behavior
- Request/response validation
- Query parameter handling
- Filter combinations
- Error responses (404, 400, etc.)
- DTO serialization/deserialization
- Full request pipeline

**Avoid**:

- Complex domain logic (use unit tests)
- UI interactions (use E2E tests)
- Cross-browser testing

**Current Projects**:

- TechHub.Api.Tests

### Component Tests (bUnit - Future)

**Use For**:

- Blazor component rendering
- Parameter binding
- Event handling
- Component lifecycle
- CSS class application
- Conditional rendering

**Avoid**:

- Full page workflows (use E2E)
- API integration (use API tests)
- Browser-specific features

**Future Projects**:

- TechHub.Web.Tests

### E2E Tests (Playwright - Future)

**Use For**:

- Complete user workflows
- Cross-browser compatibility
- Navigation flows
- Form submissions
- Interactive filtering
- Performance validation
- Visual regression

**Avoid**:

- Unit logic
- Implementation details
- Isolated functions

**Future Projects**:

- TechHub.E2E.Tests

## Test Data Management

### In-Memory Test Data

```csharp
// Use factory methods for consistent test data
private static ContentItem CreateTestContentItem(
    string id = "test",
    string collection = "news",
    List<string>? categories = null,
    List<string>? tags = null)
{
    return new ContentItem
    {
        Id = id,
        Collection = collection,
        Categories = categories ?? ["ai"],
        Tags = tags ?? ["test"],
        Title = $"Test {id}",
        Description = $"Description for {id}",
        Author = "Test Author",
        DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        Content = "Test content",
        Excerpt = "Test excerpt",
        CanonicalUrl = $"/ai/{collection}/{id}.html"
    };
}
```

### Mock Repository Setup

```csharp
// Set up repository mocks with test data
public void SetupTestData()
{
    var sections = new List<Section>
    {
        new() { Id = "ai", Title = "AI", /* ... */ },
        new() { Id = "github-copilot", Title = "GitHub Copilot", /* ... */ }
    };
    
    SectionRepository.GetAllSectionsAsync(Arg.Any<CancellationToken>())
        .Returns(sections);
    
    SectionRepository.GetSectionByIdAsync("ai", Arg.Any<CancellationToken>())
        .Returns(sections[0]);
    
    SectionRepository.GetSectionByIdAsync("github-copilot", Arg.Any<CancellationToken>())
        .Returns(sections[1]);
    
    SectionRepository.GetSectionByIdAsync("invalid", Arg.Any<CancellationToken>())
        .Returns((Section?)null);
}
```

## Coverage Requirements

### Target Coverage

- **Unit Tests**: 80%+ line coverage
- **Integration Tests**: All endpoints covered
- **Component Tests**: Critical components (future)
- **E2E Tests**: Critical user paths (future)

### Current Coverage

See [docs/testing-summary.md](/docs/testing-summary.md) for latest coverage statistics:

- Infrastructure Tests: 52 tests (FrontMatter, Markdown, Repositories)
- API Integration Tests: 40 tests (Sections, Content, Filtering)
- Total: 92 tests, 100% pass rate

## Best Practices

### Test Naming Conventions

```csharp
// ✅ CORRECT: Method_Scenario_ExpectedBehavior
[Fact]
public void GetUrlInSection_WithValidSection_ReturnsCorrectUrl()

[Theory]
[InlineData("ai", "/ai/videos/test.html")]
public void GetUrlInSection_WithDifferentSections_ReturnsCorrectUrls(string section, string expected)

// ❌ WRONG: Vague test names
[Fact]
public void Test1()

[Fact]
public void Works()
```

### Arrange-Act-Assert Pattern

```csharp
[Fact]
public void ExampleTest()
{
    // Arrange - set up test data and dependencies
    var repository = Substitute.For<ISectionRepository>();
    var sections = new List<Section> { /* ... */ };
    repository.GetAllSectionsAsync(Arg.Any<CancellationToken>())
        .Returns(sections);
    
    // Act - execute the method under test
    var result = await service.GetAllSectionsAsync(CancellationToken.None);
    
    // Assert - verify the outcome
    result.Should().HaveCount(2);
    result.Should().Contain(s => s.Id == "ai");
}
```

### Test Independence

```csharp
// ✅ CORRECT: Each test is independent
public class IndependentTests : IClassFixture<TechHubApiFactory>
{
    private readonly HttpClient _client;
    
    public IndependentTests(TechHubApiFactory factory)
    {
        factory.SetupTestData(); // Fresh data for each test class
        _client = factory.CreateClient(); // Fresh client
    }
    
    [Fact]
    public void Test1() { /* ... */ }
    
    [Fact]
    public void Test2() { /* ... */ }
}

// ❌ WRONG: Tests depend on shared mutable state
private static int _counter = 0; // Shared state!

[Fact]
public void Test1()
{
    _counter++; // Modifies shared state
}

[Fact]
public void Test2()
{
    Assert.Equal(1, _counter); // Depends on Test1 running first!
}
```

### Async Test Patterns

```csharp
// ✅ CORRECT: Async tests return Task
[Fact]
public async Task GetSectionAsync_ReturnsSection()
{
    var result = await repository.GetSectionByIdAsync("ai", CancellationToken.None);
    result.Should().NotBeNull();
}

// ✅ CORRECT: Testing exceptions in async methods
[Fact]
public async Task GetSectionAsync_InvalidId_ThrowsException()
{
    var act = async () => await service.GetSectionAsync("invalid");
    await act.Should().ThrowAsync<NotFoundException>();
}

// ❌ WRONG: Async void tests
[Fact]
public async void GetSectionAsync_ReturnsSection() // Never use async void!
{
    var result = await repository.GetSectionByIdAsync("ai", CancellationToken.None);
}
```

## Debugging Tests

### Visual Studio / VS Code

```csharp
// Set breakpoints by clicking left of line number
// Right-click test → Debug Test

// Or use debugger statement (less common in C#)
System.Diagnostics.Debugger.Break(); // Breaks into debugger
```

### Test Output

```csharp
// Use ITestOutputHelper for test output
public class MyTests
{
    private readonly ITestOutputHelper _output;
    
    public MyTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void Test()
    {
        _output.WriteLine($"Debug value: {variable}");
        // Test continues...
    }
}
```

### Run Single Test

```bash
# Run only one test for debugging

dotnet test --filter "FullyQualifiedName~GetAllSections_Returns200_WithSections"

# Run with detailed output

dotnet test --logger "console;verbosity=detailed"
```

## CI/CD Integration

All tests run automatically in GitHub Actions:

```yaml
# .github/workflows/dotnet-tests.yml (future)

- name: Run All .NET Tests
  run: dotnet test --logger "trx;LogFileName=test-results.trx"

- name: Upload Test Results
  uses: actions/upload-artifact@v4
  if: always()
  with:
    name: test-results
    path: "**/TestResults/*.trx"
```

## Additional Resources

- **Source Code Patterns**: [src/AGENTS.md](/src/AGENTS.md) - Production code development patterns (test code is also source code!)
- **xUnit Documentation**: Use context7 MCP tool (`mcp_context7_query-docs` with library "/xunit/xunit")
- **FluentAssertions Docs**: Use context7 MCP tool
- **NSubstitute Docs**: Use context7 MCP tool
- **bUnit Documentation**: Use context7 MCP tool (future component testing)
- **Playwright .NET Docs**: Use context7 MCP tool (future E2E testing)
- **PowerShell Testing**: [tests/powershell/AGENTS.md](/tests/powershell/AGENTS.md)

---

**Remember**: Write tests BEFORE or DURING implementation, never after. Tests are not optional—they are part of the definition of "done".
