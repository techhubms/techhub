# Testing Strategy for Tech Hub .NET

> **AI CONTEXT**: This is a **LEAF** context file for the `tests/` directory. It complements the [Root AGENTS.md](/AGENTS.md) and [.NET Development Expert](/.github/agents/dotnet.md).
> **RULE**: Global rules (Timezone, Performance, AI Assistant Workflow) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Critical Testing Rules

**⚠️ CRITICAL E2E TEST WARNING**:  
🚫 **NEVER run `dotnet test tests/TechHub.E2E.Tests` directly** - it **WILL FAIL** without servers running!  
✅ **ALWAYS use `./run.ps1 -OnlyTests`** which handles server startup, testing, and shutdown automatically.

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

| Layer         | Framework                   | Projects              | Purpose                                 |
| ------------- | --------------------------- | --------------------- | --------------------------------------- |
| **Unit**      | xUnit                       | Core, Infrastructure  | Domain logic, services                  |
| **Integration** | xUnit + WebApplicationFactory | Api                 | HTTP endpoints, full request/response   |
| **Component** | bUnit                       | Web (future)          | Blazor component rendering & logic      |
| **E2E**       | Playwright .NET             | E2E (future)          | Full user workflows                     |
| **PowerShell** | Pester                     | powershell/           | Automation scripts                      |

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
            ExternalUrl = "/ai/videos/vs-code-107"
        };
        
        // Act
        var url = item.GetUrlInSection("ai");
        
        // Assert
        url.Should().Be("/ai/videos/vs-code-107");
    }
    
    [Theory]
    [InlineData("ai", "/ai/videos/test")]
    [InlineData("github-copilot", "/github-copilot/videos/test")]
    [InlineData("ml", "/ml/videos/test")]
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
            ExternalUrl = $"/ai/{collection}/{id}"
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
result.Should().Match("/ai/*/test");

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
    public async Task GetSectionByName_ReturnsSection(string name, string expectedCategory)
    {
        // Act
        var response = await _client.GetAsync($"/api/sections/{name}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var section = await response.Content.ReadFromJsonAsync<SectionDto>();
        section.Should().NotBeNull();
        section!.Id.Should().Be(id);
        section.Category.Should().Be(expectedCategory);
    }
    
    [Fact]
    public async Task GetSectionByName_InvalidSection_Returns404()
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

## E2E Testing (Playwright .NET)

End-to-end tests verify complete user workflows across the full application stack using Playwright .NET.

### Current Status

- **Total Tests**: 47
- **Pass Rate**: 100%
- **Execution Time**: ~92 seconds
- **Framework**: Playwright .NET with xUnit
- **Browsers**: Chromium (headless)

### Structure

```text
TechHub.E2E.Tests/
├── Tests/
│   ├── NavigationImprovementsTests.cs     # Section ordering, URL structure
│   ├── UrlRoutingAndNavigationTests.cs    # URL routing, browser history
│   └── Api/
│       └── ApiEndToEndTests.cs            # API health checks
├── Helpers/
│   └── BlazorHelpers.cs                   # Blazor-specific test utilities
└── TechHub.E2E.Tests.csproj
```

### Blazor Server Testing Patterns

#### GotoAndWaitForBlazorAsync

Blazor Server requires special handling for page navigation due to circuit initialization:

```csharp
// ✅ CORRECT: Use GotoAndWaitForBlazorAsync for Blazor pages
await page.GotoAndWaitForBlazorAsync(BaseUrl);
await page.WaitForSelectorAsync(".section-card");

// ❌ WRONG: Plain GotoAsync doesn't wait for Blazor circuit
await page.GotoAsync(BaseUrl); // Circuit might not be ready!
```

**What GotoAndWaitForBlazorAsync does**:

1. Navigates to URL with extended 30s timeout
2. Waits for NetworkIdle (SignalR connection established)
3. Adds 500ms delay for first render to complete

#### WaitForBlazorUrlContainsAsync

Blazor Server uses **enhanced navigation** (SPA-style routing), just like React, Angular, and Vue. Traditional page load events don't fire because the page doesn't reload. This is intentional modern SPA behavior, not a limitation:

```csharp
// ✅ CORRECT: Use WaitForBlazorUrlContainsAsync for SPA navigation
// This polls JavaScript for URL changes - the standard pattern for SPAs
var newsButton = page.Locator(".collection-nav button", new() { HasTextString = "News" });
await newsButton.ClickAsync();
await page.WaitForBlazorUrlContainsAsync("/news");

// ❌ WRONG: WaitForURLAsync expects navigation events (load, commit, domcontentloaded)
// SPAs don't fire these events because the page doesn't reload
await page.WaitForURLAsync("**/news"); // Times out!
await page.WaitForURLAsync("**/news", new() { WaitUntil = WaitUntilState.Commit }); // Also times out!
```

**Why WaitForBlazorUrlContainsAsync is needed**:

- All modern SPAs (React, Angular, Vue, Blazor) change URLs without page reload
- Traditional navigation events ("load", "commit", "domcontentloaded") don't fire
- Playwright's `WaitForFunctionAsync` is the standard pattern for testing SPAs - it polls JavaScript
- This is NOT a Blazor limitation - it's how modern web apps work
- See [BlazorHelpers.cs](Helpers/BlazorHelpers.cs) for implementation

#### Browser History Testing

Testing browser back/forward buttons uses the standard Blazor `NavigationManager.LocationChanged` pattern:

```csharp
[Fact]
public async Task BrowserBackButton_NavigatesToPreviousCollection()
{
    var page = await _context!.NewPageAsync();
    await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot/news");
    
    // Navigate to videos
    var videosButton = page.Locator(".collection-nav button", new() { HasTextString = "Videos" });
    await videosButton.ClickAsync();
    await page.WaitForBlazorUrlContainsAsync("/videos");
    
    // Press browser back button
    await page.GoBackAsync();
    await page.WaitForURLAsync("**/github-copilot/news");
    
    // CRITICAL: Wait for Blazor to sync state (NavigationManager.LocationChanged event)
    await page.WaitForSelectorAsync(".collection-content");
    await Task.Delay(1000); // Blazor needs time to process parameter change
    
    // Verify active collection button updated
    var activeButton = page.Locator(".collection-nav button.active");
    var activeText = await activeButton.TextContentAsync();
    activeText.Should().Contain("News");
}
```

**Why the delay is needed**:

- `GoBackAsync()` changes URL immediately
- Blazor's `NavigationManager.LocationChanged` event fires asynchronously
- Component state update happens after event handler executes
- Without delay, assertion might run before UI updates

**Implementation Note**: See [Section.razor](../src/TechHub.Web/Components/Pages/Section.razor) for the `NavigationManager.LocationChanged` event handler. This is the **standard Blazor pattern** per [official documentation](https://learn.microsoft.com/aspnet/core/blazor/fundamentals/routing#uri-and-navigation-state-helpers).

### Common Test Patterns

```csharp
public class MyE2ETests : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private const string BaseUrl = "http://localhost:5184";
    
    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
        _context = await _browser.NewContextAsync();
        _context.SetDefaultTimeout(5000); // 5s default, fail fast if something is wrong
    }
    
    [Fact]
    public async Task Example_NavigationTest()
    {
        // Arrange
        var page = await _context!.NewPageAsync();
        await page.GotoAndWaitForBlazorAsync(BaseUrl);
        
        // Act
        var link = page.Locator(".section-card-link[href*='github-copilot']");
        await link.ClickAsync();
        await page.WaitForBlazorUrlContainsAsync("/github-copilot");
        
        // Assert
        page.Url.Should().Contain("/github-copilot");
        
        await page.CloseAsync();
    }
    
    public async Task DisposeAsync()
    {
        if (_context != null) await _context.DisposeAsync();
        if (_browser != null) await _browser.DisposeAsync();
        _playwright?.Dispose();
    }
}
```

### Locator Strategies

```csharp
// ✅ GOOD: Specific CSS selectors
page.Locator(".section-card-link[href*='github-copilot']")
page.Locator(".collection-nav button.active")
page.Locator(".content-item-card").First

// ✅ GOOD: Text-based (user-visible)
page.Locator(".collection-nav button", new() { HasTextString = "News" })

// ⚠️ CAREFUL: nth selectors are brittle
page.Locator(".section-card").Nth(2) // Breaks if order changes!

// ❌ BAD: XPath (prefer CSS)
page.Locator("xpath=//button[@class='active']")
```

### Debugging E2E Tests

```csharp
// Run in headed mode (see browser)
_browser = await _playwright.Chromium.LaunchAsync(new() { Headless = false });

// Slow down actions
_browser = await _playwright.Chromium.LaunchAsync(new() { 
    Headless = false,
    SlowMo = 500 // 500ms delay between actions
});

// Take screenshots on failure
try
{
    // test code
}
catch
{
    await page.ScreenshotAsync(new() { Path = "failure.png" });
    throw;
}

// Enable verbose logging
_context.SetDefaultTimeout(30000); // Longer timeout for debugging
```

### Running E2E Tests

**For Automated Testing** (verifying changes work):

```bash
# Run all tests (unit, integration, component, E2E) and exit
./run.ps1 -OnlyTests
```

**For Interactive Debugging** (AI agents AND humans using Playwright MCP):

```bash
# RECOMMENDED: Skip tests, start servers fast, use Playwright MCP interactively
./run.ps1 -SkipTests

# OR run tests first, then keep servers running for manual testing
./run.ps1
```

**AI Agents - Interactive Debugging is Powerful!**

- Use `./run.ps1 -SkipTests` to start servers quickly
- Use Playwright MCP tools in GitHub Copilot Chat to:
  - Navigate pages, take screenshots, capture snapshots
  - Click buttons, fill forms, test interactions
  - Investigate bugs interactively (faster than writing tests)
  - Verify UI behavior before writing automated tests

```bash
# Run specific E2E test (requires servers already running)
dotnet test tests/TechHub.E2E.Tests --filter "FullyQualifiedName~Web.UrlRoutingTests.NavigateToSection"
```

```bash
# Debug E2E test in headed mode
# First, modify test to set Headless = false, then run:
dotnet test tests/TechHub.E2E.Tests --filter "FullyQualifiedName~MyTest"
```

**Server Logs** (when running via `./run.ps1`):

- API: `.tmp/test-logs/api-e2e.log`
- Web: `.tmp/test-logs/web-e2e.log`

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

### Run with run.ps1 (Recommended)

The `run.ps1` script provides the easiest way to run all tests including E2E:

**For Automated Testing** (verifying all changes):

```bash
# Run all tests and exit
./run.ps1 -OnlyTests
```

**For Interactive Debugging** (AI agents AND humans with Playwright MCP):

```bash
# RECOMMENDED: Skip tests, start servers fast, debug interactively
./run.ps1 -SkipTests

# OR run tests first, then keep servers running
./run.ps1

# Clean build before debugging
./run.ps1 -Clean -SkipTests
```

**Why AI Agents Should Use Interactive Debugging**:

- **Faster exploration**: No need to write tests first - just navigate and interact
- **Better bug investigation**: See actual UI, take screenshots, inspect elements
- **Iterative testing**: Test → fix → test again without restarting
- **Playwright MCP in Chat**: All debugging happens in Copilot Chat, no terminal needed

**What Happens** (with `-OnlyTests`):

- Builds the solution
- Runs unit and integration tests
- Starts API and Web servers in `Test` environment (minimal logging via `appsettings.Test.json`)
- Server output redirected to `.tmp/test-logs/api-e2e.log` and `.tmp/test-logs/web-e2e.log`
- Runs E2E tests
- Stops servers automatically
- Exits with test results

**What Happens** (default behavior):

- Same as `-OnlyTests` but keeps servers running after tests complete
- Useful for manual testing or Playwright MCP interactive debugging

**Server Logs** (when running E2E tests):

- API logs: `.tmp/test-logs/api-e2e.log`
- Web logs: `.tmp/test-logs/web-e2e.log`

### Standard dotnet test

For running tests without E2E (no servers needed):

```bash
# All unit/integration/component tests (no E2E)
dotnet test --filter "FullyQualifiedName!~E2E"

# Specific test project
dotnet test tests/TechHub.Core.Tests
dotnet test tests/TechHub.Infrastructure.Tests
dotnet test tests/TechHub.Api.Tests
dotnet test tests/TechHub.Web.Tests

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
        ExternalUrl = $"/ai/{collection}/{id}"
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
    
    SectionRepository.GetByIdAsync("ai", Arg.Any<CancellationToken>())
        .Returns(sections[0]);
    
    SectionRepository.GetByIdAsync("github-copilot", Arg.Any<CancellationToken>())
        .Returns(sections[1]);
    
    SectionRepository.GetByIdAsync("invalid", Arg.Any<CancellationToken>())
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
[InlineData("ai", "/ai/videos/test")]
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
    var result = await repository.GetByIdAsync("ai", CancellationToken.None);
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
    var result = await repository.GetByIdAsync("ai", CancellationToken.None);
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
