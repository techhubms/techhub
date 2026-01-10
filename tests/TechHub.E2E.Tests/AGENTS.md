# E2E Tests - Tech Hub

End-to-end tests using Playwright to verify complete user workflows and functionality.

**Implementation being tested**: See [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) for Blazor component patterns and [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) for API endpoint patterns.

## Running Tests

### Recommended Approach

**Always use `./run.ps1`** - it handles server startup, test execution, and cleanup automatically:

```powershell
# Run all tests, then exit (recommended for verifying changes)
./run.ps1 -OnlyTests

# Run tests first, then keep servers running for debugging
./run.ps1
```

### Interactive Debugging with Playwright MCP

For investigating bugs or exploring UI behavior interactively:

```powershell
# Start servers without running tests
./run.ps1 -SkipTests

# Then use Playwright MCP tools directly in GitHub Copilot Chat to:
# - Navigate pages and inspect elements
# - Take snapshots and screenshots
# - Test interactions and verify behavior
# - Reproduce bugs before writing tests
```

**When to use Playwright MCP**:

- Faster than writing tests for initial exploration
- More powerful than curl/wget for UI testing
- Investigate bugs, test interactions, verify behavior interactively
- Afterwards, write E2E tests that reproduce the debugged issues

### Running Specific Tests

**Only when servers are already running** (via `./run.ps1 -SkipTests`):

```powershell
# Run all E2E tests
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj

# Run specific test file
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~Web.UrlRoutingTests"

# Run single test method
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~Web.UrlRoutingTests.NavigateToSection_DefaultsToAllCollection"
```

⚠️ **WARNING**: `dotnet test` requires servers at `localhost:5029` (API) and `localhost:5184` (Web). It **WILL FAIL** if servers aren't running. Always prefer `./run.ps1 -OnlyTests`.

## Test Architecture

### Structure

```text
tests/TechHub.E2E.Tests/
├── Web/                                 ← Playwright-based E2E tests
│   ├── UrlRoutingTests.cs              ← URL routing, collections, buttons (18 tests)
│   ├── NavigationTests.cs              ← Section navigation, styling (8 tests)
│   ├── RssTests.cs                     ← RSS feeds (9 tests)
│   ├── ContentDetailTests.cs           ← Content pages (8 tests)
│   ├── AboutPageTests.cs               ← About page (5 tests)
│   ├── HomePageRoundupsTests.cs        ← Homepage roundups (3 tests)
│   ├── HomePageSidebarTests.cs         ← Homepage sidebar (3 tests)
│   ├── CustomPagesTests.cs             ← Custom pages (10 tests)
│   ├── SectionCardLayoutTests.cs       ← Section cards (3 tests)
│   └── SectionPageKeyboardNavigationTests.cs ← Keyboard nav (5 tests)
├── Api/
│   └── ApiEndToEndTests.cs             ← Direct API testing (no Playwright)
├── Helpers/
│   ├── BlazorHelpers.cs                ← Blazor-specific wait patterns
│   └── PlaywrightExtensions.cs         ← Page interaction helpers
├── PlaywrightCollectionFixture.cs      ← Shared browser configuration
└── xunit.runner.json                   ← Parallel execution settings
```

**Total**: 72 E2E test cases across all Web test files

### Shared Page Pattern

**CRITICAL**: All test classes use a consistent shared page pattern:

```csharp
[Collection("My Feature Tests")]
public class MyFeatureTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");

    public MyFeatureTests(PlaywrightCollectionFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();
        if (_context != null)
            await _context.DisposeAsync();
    }

    [Fact]
    public async Task MyTest()
    {
        // Use Page property - clean, no ! everywhere
        await Page.GotoRelativeAsync("/github-copilot");
        await Page.AssertUrlEndsWithAsync("/github-copilot");
    }
}
```

**Benefits**:

- Each test method gets its own fresh page
- Automatic cleanup even if test fails
- Clean test code - use `Page` property instead of `_page!` everywhere
- No manual page cleanup in test methods

### Browser Configuration

All browser launch options are centralized in [PlaywrightCollectionFixture.cs](PlaywrightCollectionFixture.cs):

```csharp
Browser = await Playwright.Chromium.LaunchAsync(new()
{
    Headless = true,
    Channel = "chrome",              // Use Chrome browser (not chromium-headless-shell)
    Timeout = 5000,
    Args =
    [
        "--no-sandbox",              // Required for DevContainer
        "--disable-setuid-sandbox",  // Required for DevContainer
        "--disable-web-security",    // Allow CORS for local testing
        "--disable-features=IsolateOrigins,site-per-process",
        "--disable-blink-features=AutomationControlled",
        "--disable-dev-shm-usage",   // Prevent shared memory issues
        "--disable-gpu"              // Not needed in headless
    ]
});
```

**Browser Context Settings**:

```csharp
return await Browser.NewContextAsync(new()
{
    ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
    Locale = "en-US",
    TimezoneId = "Europe/Brussels",
    IgnoreHTTPSErrors = true
});
```

**Why these settings?**

- **Chrome channel**: Reliable headless testing + works with Playwright MCP tools
- **DevContainer compatibility**: `--no-sandbox` and `--disable-setuid-sandbox` required for containers
- **Local testing**: `--disable-web-security` allows CORS for localhost
- **Consistent viewport**: 1920x1080 matches common desktop resolution
- **Timezone**: Europe/Brussels matches application configuration

### Performance Optimizations

**1. Shared Browser Instance** (90% faster startup):

- ONE browser launch for all test classes
- Each test gets isolated context (separate cookies, storage)
- Defined in PlaywrightCollectionFixture.cs, shared via xUnit Collection Fixtures

**2. Parallel Test Execution** (4x throughput):

- Each test class runs in parallel
- Configured in xunit.runner.json: `maxParallelThreads: 4` (fixed thread count)
- Each collection gets its own browser context for isolation

**3. Optimized Timeouts** (fail fast):

- Element operations: 5s (vs Playwright default 30s)
- Page loads: 10s (vs Playwright default 30s)
- Assertions: 5s (vs Playwright default 30s)
- Managed centrally via constants in `BlazorHelpers.cs`

## Writing New Tests

### Test Naming Convention

Use the format: `ComponentOrFeature_Action_ExpectedResult`

Examples:

- `NavigateToSection_DefaultsToAllCollection`
- `ClickCollectionButton_UpdatesURL`
- `AllCollection_ShowsAllContentFromSection`

### Using BlazorHelpers

**ALWAYS use helper methods** from BlazorHelpers.cs for consistent timeout management:

```csharp
// Navigation
await Page.GotoRelativeAsync("/github-copilot");                    // Navigate with Blazor wait
await Page.WaitForBlazorUrlContainsAsync("/news");                  // Wait for SPA navigation

// High-level navigation pattern (consolidates click + wait + verify)
await Page.ClickAndNavigateAsync(".collection-nav a", text: "News",
    expectedUrlSegment: "/news", waitForActiveState: "News");        // Click + navigate + verify

// Assertions
await Page.AssertUrlEndsWithAsync("/github-copilot");               // Assert URL suffix
await element.AssertElementVisibleAsync();                           // Assert element visible
await Page.AssertElementVisibleBySelectorAsync(".sidebar");         // Assert by selector
await Page.AssertElementContainsTextBySelectorAsync("h1", "Title"); // Assert text content

// Interactions
await button.ClickBlazorElementAsync();                              // Click with Blazor handling
var text = await element.TextContentWithTimeoutAsync();              // Get text with timeout
var href = await link.GetHrefAsync();                                // Get href attribute
```

**Available Helper Methods**:

| Method | Purpose | Timeout |
| ------ | ------- | ------- |
| `NewPageWithDefaultsAsync()` | Create page with optimized timeouts | 5s/10s |
| `GotoRelativeAsync()` | Navigate to relative URL with Blazor wait | 10s |
| `ClickAndNavigateAsync()` | High-level click + navigate + verify pattern | 10s |
| `WaitForBlazorUrlContainsAsync()` | Wait for SPA navigation | 10s |
| `WaitForBlazorRenderAsync()` | Wait for element to appear | 5s |
| `ClickBlazorElementAsync()` | Click with Blazor-aware handling | 5s |
| `AssertElementVisibleAsync()` | Assert element is visible | 5s |
| `AssertUrlEndsWithAsync()` | Assert URL ends with segment | 10s |
| `AssertElementVisibleBySelectorAsync()` | Assert element visible by selector | 5s |
| `AssertElementContainsTextBySelectorAsync()` | Assert text content by selector | 5s |
| `AssertElementVisibleByRoleAsync()` | Assert element visible by ARIA role | 5s |
| `TextContentWithTimeoutAsync()` | Get text content with timeout | 5s |
| `GetHrefAsync()` | Get href attribute from element | 5s |
| `WaitForSelectorWithTimeoutAsync()` | Wait for selector with timeout | 5s |

### Assertion Style

Use FluentAssertions for readable assertions:

```csharp
// ✅ Good - descriptive with reason
Page.Url.Should().EndWith("/github-copilot/all",
    "navigating to a section without collection should default to /section/all");

// ❌ Bad - no context
Assert.Equal("/github-copilot/all", Page.Url);
```

### Complete Test Example

```csharp
using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

[Collection("My Feature Tests")]
public class MyFeatureTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private const string BaseUrl = "http://localhost:5184";

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();
        if (_context != null)
            await _context.DisposeAsync();
    }

    [Fact]
    public async Task MyFeature_Action_ExpectedResult()
    {
        // Navigate
        await Page.GotoRelativeAsync("/github-copilot");
        
        // Interact - Using high-level ClickAndNavigateAsync helper
        await Page.ClickAndNavigateAsync(".collection-nav a", text: "News",
            expectedUrlSegment: "/github-copilot/news",
            waitForActiveState: "News");
        
        // Assert - URL and active state already verified by ClickAndNavigateAsync
        Page.Url.Should().EndWith("/github-copilot/news",
            "clicking News should navigate to /github-copilot/news");
    }
}
```

**Don't forget**: Add collection definition to PlaywrightCollectionFixture.cs:

```csharp
[CollectionDefinition("My Feature Tests")]
public class MyFeatureCollection : ICollectionFixture<PlaywrightCollectionFixture> { }
```

## Maintenance

### When to Update Tests

- ✅ **ALWAYS** when changing URL routes or navigation behavior
- ✅ **ALWAYS** when modifying collection filtering logic
- ✅ **ALWAYS** when changing button behavior or interactivity
- ✅ **REQUIRED** when adding new collections or sections
- ⚠️ **CONSIDER** when changing CSS selectors (tests may break)

### Test Stability

All tests should be:

- **Deterministic**: Same input → same result
- **Isolated**: Tests don't depend on each other
- **Fast**: Complete in <30 seconds total
- **Clear**: Failures point to exact issue

## Related Documentation

- [Root AGENTS.md](/AGENTS.md) - Overall development workflow and TDD approach
- [tests/AGENTS.md](/tests/AGENTS.md) - Testing strategies across all layers
- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [specs/dotnet-migration/tasks.md](/specs/dotnet-migration/tasks.md) - Task tracking and completion status
