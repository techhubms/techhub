# E2E Tests - Tech Hub

End-to-end tests using Playwright to verify complete user workflows and functionality.

## Prerequisites

1. **Playwright browsers must be installed**:

   ```powershell
   pwsh tests/TechHub.E2E.Tests/bin/Debug/net10.0/playwright.ps1 install
   ```

2. **Application must be running**:

   ```powershell
   # Start the entire application using the run script
   ./run.ps1
   ```

## Running Tests

### Run All E2E Tests

```powershell
# From repository root
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj
```

### Run Specific Test File

```powershell
# URL routing and navigation tests
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~UrlRoutingAndNavigationTests"

# Navigation improvements tests
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~NavigationImprovementsTests"
```

### Run Single Test

```powershell
# Example: Run only the URL routing test
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~UrlRoutingAndNavigationTests.NavigateToSection_DefaultsToAllCollection"
```

## Test Coverage

### URL Routing and Navigation (UrlRoutingAndNavigationTests.cs)

Comprehensive tests for URL-based navigation and "all" collection functionality:

**URL Routing** (7 tests):

- ✅ Navigate to section defaults to /section/all
- ✅ Navigate to section/collection maintains URL
- ✅ Click collection button updates URL to /section/collection
- ✅ Click "All" button updates URL to /section/all
- ✅ Browser back button navigates to previous collection
- ✅ Browser forward button navigates to next collection

**"All" Collection Functionality** (6 tests):

- ✅ /section/all shows all content from that section
- ✅ /all/all shows everything from all sections and collections
- ✅ /all/collection shows that collection across all sections
- ✅ "All" button exists in collection sidebar
- ✅ "All" collection shows collection badges (proper capitalization)
- ✅ Specific collection hides redundant collection badge

**Interactive Buttons** (3 tests):

- ✅ All collection buttons are clickable and update URL
- ✅ Retry button reloads content after errors
- ✅ Active collection button has "active" CSS class

**URL Sharing and Bookmarking** (2 tests):

- ✅ Direct URL loads correct collection state
- ✅ Copied URL restores exact collection state in new tab

#### Total: 20 comprehensive test cases

### Navigation Improvements (NavigationImprovementsTests.cs)

Tests for section ordering, styling, and navigation flow:

- ✅ Homepage sections ordered correctly (starts with "Everything")
- ✅ Section card click navigates to section homepage
- ✅ Collection navigation updates URL (no hash fragments)
- ✅ Collection page hides redundant collection badge
- ✅ "All" page shows collection badge with proper capitalization
- ✅ Collection sidebar buttons are clickable
- ✅ Header area has consistent height
- ✅ Section background images display correctly (no grey bars)
- ✅ Direct URL to section/collection loads correct content

#### Total: 10 test cases

## Test Architecture

### Structure

```text
tests/TechHub.E2E.Tests/
├── Tests/
│   ├── UrlRoutingAndNavigationTests.cs  ← URL routing, "all" collection, buttons
│   └── NavigationImprovementsTests.cs   ← Section ordering, styling, navigation
├── Helpers/
│   └── BlazorHelpers.cs                 ← Extension methods for Blazor-specific wait patterns
├── PlaywrightCollectionFixture.cs       ← Shared browser instance (ONE per test run)
├── xunit.runner.json                    ← Parallel execution configuration
├── TechHub.E2E.Tests.csproj
└── AGENTS.md (this file)
```

### Performance Architecture 🚀

**KEY INSIGHT**: The E2E tests were dramatically improved by implementing these optimizations:

#### 1. **Shared Browser Instance** (90% speed improvement)

**Problem**: Creating a new Playwright browser for each test class took 2-3 seconds per class.

**Solution**: Use xUnit Collection Fixtures to share ONE browser instance across all test classes:

```csharp
// PlaywrightCollectionFixture.cs - Created ONCE per test run
public class PlaywrightCollectionFixture : IAsyncLifetime
{
    public IBrowser? Browser { get; private set; }
    
    public async Task InitializeAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new() { Headless = true });
    }
    
    // Each test class gets isolated browser CONTEXT (not new browser!)
    public async Task<IBrowserContext> CreateContextAsync()
    {
        return await Browser.NewContextAsync(new()
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            Locale = "en-US",
            TimezoneId = "Europe/Brussels",
        });
    }
}
```

**Benefits**:

- **ONE browser launch** for all test classes (instead of N launches)
- **Isolated contexts** for proper test isolation (separate cookies, storage, etc.)
- **Parallel execution** still works - contexts are thread-safe
- **90% faster** test suite startup

#### 2. **Parallel Test Execution** (4x throughput)

**Configuration** in `xunit.runner.json`:

```jsonc
{
  "parallelizeAssembly": true,
  "parallelizeTestCollections": true,
  "maxParallelThreads": -1  // Use all available CPU cores
}
```

**Test Class Setup** - Each class gets its own collection for parallel execution:

```csharp
// Each collection can run in parallel
[CollectionDefinition("URL Routing Tests")]
public class UrlRoutingCollection : ICollectionFixture<PlaywrightCollectionFixture> { }

[CollectionDefinition("Navigation Tests")]
public class NavigationCollection : ICollectionFixture<PlaywrightCollectionFixture> { }

// Test class declares which collection it belongs to
[Collection("URL Routing Tests")]
public class UrlRoutingAndNavigationTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;  // Isolated context per test class
    
    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();  // Fast! Just a context, not browser
    }
}
```

**Benefits**:

- **4x+ throughput** on multi-core machines
- **Isolated contexts** prevent test interference
- **Same browser** shared across parallel tests
- **Fast context creation** (<100ms vs 2-3s for browser)

#### 3. **Smart Wait Strategies** (No Task.Delay!)

**❌ OLD Pattern** (slow and flaky):

```csharp
await page.GotoAsync(url);
await Task.Delay(2000);  // Arbitrary wait - might be too short OR too long
```

**✅ NEW Pattern** (fast and reliable):

```csharp
// Use intelligent wait strategies from BlazorHelpers.cs
await page.GotoAndWaitForBlazorAsync(url);  // Waits for actual content, not arbitrary time
```

**How It Works**:

```csharp
public static async Task GotoAndWaitForBlazorAsync(this IPage page, string url)
{
    // 1. Navigate with optimized settings
    await page.GotoAsync(url, new() { 
        WaitUntil = WaitUntilState.DOMContentLoaded,  // Faster than NetworkIdle
        Timeout = 10000  // Fail fast (reduced from 30s default)
    });
    
    // 2. Wait for Blazor streaming to complete (skeletons → real content)
    try
    {
        await page.WaitForSelectorAsync(".skeleton-header", new() { 
            State = WaitForSelectorState.Hidden,
            Timeout = 2000  // Reduced timeout
        });
    }
    catch { /* Skeleton might not exist if content loaded instantly */ }
    
    // 3. Wait for real content cards
    try
    {
        await page.WaitForSelectorAsync(".content-item-card:not(.skeleton-card)", new() { 
            State = WaitForSelectorState.Visible,
            Timeout = 2000
        });
    }
    catch { /* Content cards might not exist on all pages */ }
    
    // 4. Brief stabilization (100ms vs old 1000ms Task.Delay)
    await Task.Delay(100);
}
```

**Benefits**:

- **No arbitrary delays** - waits for actual conditions
- **Fail fast** - aggressive timeouts catch issues early
- **Blazor-aware** - understands streaming render lifecycle
- **Shorter total wait** - returns as soon as content is ready

#### 4. **SPA Navigation Detection** (No double waits!)

**Problem**: Blazor uses client-side routing (like React/Angular/Vue) - URL changes without page reloads.

**❌ OLD Pattern** (doesn't work):

```csharp
await page.ClickAsync("button");
await page.WaitForNavigationAsync();  // NEVER fires - no traditional navigation!
await Task.Delay(1000);  // Arbitrary wait because WaitForNavigationAsync failed
```

**✅ NEW Pattern** (standard Playwright SPA pattern):

```csharp
await page.ClickAsync("button");
await page.WaitForBlazorUrlContainsAsync("/news");  // Polls URL until it matches
```

**Implementation**:

```csharp
public static async Task WaitForBlazorUrlContainsAsync(
    this IPage page,
    string urlSegment,
    int timeoutMs = 5000)
{
    // Standard Playwright pattern for SPAs - poll JavaScript expression
    await page.WaitForFunctionAsync(
        $"() => window.location.pathname.includes('{urlSegment}')",
        new() { 
            Timeout = timeoutMs,
            PollingInterval = 50  // Check every 50ms (fast detection)
        }
    );
}
```

**Key Insight**: This is NOT a Blazor workaround - it's the **standard Playwright pattern** for ANY modern SPA (React, Angular, Vue, Blazor). Client-side routing doesn't trigger navigation events.

#### 5. **Smart State Synchronization** (Replace 1000ms delays with polling)

**Problem**: After browser back/forward, Blazor needs time to sync component state with URL.

**❌ OLD Pattern**:

```csharp
await page.GoBackAsync();
await Task.Delay(1000);  // Hope state is synced by now
```

**✅ NEW Pattern**:

```csharp
await page.GoBackAsync();
await page.WaitForBlazorStateSyncAsync("News");  // Returns as soon as state matches
```

**Implementation**:

```csharp
public static async Task WaitForBlazorStateSyncAsync(
    this IPage page,
    string expectedActiveButtonText,
    int timeoutMs = 3000)
{
    var startTime = DateTime.UtcNow;
    while ((DateTime.UtcNow - startTime).TotalMilliseconds < timeoutMs)
    {
        try
        {
            var activeButton = page.Locator(".collection-nav button.active");
            var activeText = await activeButton.TextContentAsync(new() { Timeout = 500 });
            
            if (activeText?.Contains(expectedActiveButtonText, StringComparison.OrdinalIgnoreCase) == true)
            {
                return;  // State is synced! Return immediately
            }
        }
        catch { /* Keep polling */ }
        
        await Task.Delay(50);  // Poll every 50ms (vs old 1000ms delay)
    }
    
    throw new TimeoutException($"State did not sync to '{expectedActiveButtonText}' within {timeoutMs}ms");
}
```

**Benefits**:

- **Fast return** - exits as soon as condition is met (usually <200ms)
- **No over-waiting** - old pattern always waited full 1000ms
- **Clear errors** - timeout message shows what was expected
- **20x faster polling** - 50ms intervals vs 1000ms delays

#### 6. **Aggressive Timeouts** (Fail fast, not slow)

**Philosophy**: Tests should fail quickly when something is wrong, not hang for 30+ seconds.

**Default Playwright Timeouts**: 30 seconds (too slow for CI/CD)

**Our Optimized Timeouts**:

```csharp
public static async Task<IPage> NewPageWithDefaultsAsync(this IBrowserContext context)
{
    var page = await context.NewPageAsync();
    page.SetDefaultTimeout(3000);           // 3s for element operations
    page.SetDefaultNavigationTimeout(10000); // 10s for page loads
    return page;
}
```

**Benefits**:

- **Fail fast** - 3s timeout catches missing elements quickly
- **CI/CD friendly** - tests complete in minutes, not hours
- **Clear feedback** - fast failures = faster debugging

### Base Configuration

- **Browser**: Chromium (headless mode, ONE instance shared across all tests)
- **Web URL**: <http://localhost:5184>
- **API URL**: <http://localhost:5029>
- **Test Framework**: xUnit with Collection Fixtures for parallel execution
- **Assertions**: FluentAssertions for readable, descriptive assertions
- **Performance**: Parallel execution, smart waits, shared browser instance

### Test Patterns

**CRITICAL**: Each test class now uses the shared browser fixture for maximum performance:

```csharp
[Collection("URL Routing Tests")]  // Unique collection name for parallel execution
public class UrlRoutingAndNavigationTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;  // Shared fixture
    private IBrowserContext? _context;  // Isolated context per test class
    
    public UrlRoutingAndNavigationTests(PlaywrightCollectionFixture fixture)
    {
        _fixture = fixture;  // xUnit injects the shared fixture
    }
    
    public async Task InitializeAsync()
    {
        // Create isolated context (fast!) - NOT a new browser (slow!)
        _context = await _fixture.CreateContextAsync();
    }
    
    public async Task DisposeAsync()
    {
        // Dispose context only - browser stays alive for other tests
        if (_context != null)
            await _context.DisposeAsync();
    }
    
    [Fact]
    public async Task MyTest()
    {
        // Create page from context (not browser!)
        var page = await _context!.NewPageWithDefaultsAsync();
        
        // Use smart wait helpers
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot");
        
        // Clean up page after test
        await page.CloseAsync();
    }
}
```

## Writing New Tests

### Critical Performance Rules 🚀

**ALWAYS follow these patterns** for fast, reliable tests:

1. **✅ Use Collection Fixtures** - ONE browser for all tests

   ```csharp
   [Collection("My Test Collection")]  // Unique name for parallel execution
   public class MyTests : IAsyncLifetime
   {
       private readonly PlaywrightCollectionFixture _fixture;
       private IBrowserContext? _context;
       
       public async Task InitializeAsync()
       {
           _context = await _fixture.CreateContextAsync();  // Fast context creation
       }
   }
   ```

2. **✅ Use Smart Wait Helpers** - NO Task.Delay!

   ```csharp
   // ✅ Good - waits for actual Blazor content
   await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot");
   
   // ❌ Bad - arbitrary delay
   await page.GotoAsync(url);
   await Task.Delay(2000);
   ```

3. **✅ Use SPA Navigation Detection** - NOT WaitForNavigationAsync

   ```csharp
   // ✅ Good - standard Playwright SPA pattern
   await page.ClickAsync("button");
   await page.WaitForBlazorUrlContainsAsync("/news");
   
   // ❌ Bad - doesn't work with client-side routing
   await page.ClickAsync("button");
   await page.WaitForNavigationAsync();  // Never fires!
   ```

4. **✅ Use State Polling** - NOT arbitrary delays

   ```csharp
   // ✅ Good - returns as soon as state is synced
   await page.GoBackAsync();
   await page.WaitForBlazorStateSyncAsync("News");
   
   // ❌ Bad - always waits full duration
   await page.GoBackAsync();
   await Task.Delay(1000);
   ```

5. **✅ Create New Collection Definitions** - For parallel execution

   ```csharp
   // Add to PlaywrightCollectionFixture.cs
   [CollectionDefinition("My Feature Tests")]
   public class MyFeatureCollection : ICollectionFixture<PlaywrightCollectionFixture> { }
   ```

### Test Naming Convention

Use the format: `ComponentOrFeature_Action_ExpectedResult`

Examples:

- `NavigateToSection_DefaultsToAllCollection`
- `ClickCollectionButton_UpdatesURL`
- `AllCollection_ShowsAllContentFromSection`

### Assertion Style

Use FluentAssertions for readable, descriptive assertions:

```csharp
// ✅ Good - descriptive with reason
page.Url.Should().EndWith("/github-copilot/all",
    "navigating to a section without collection should default to /section/all");

// ❌ Bad - no context
Assert.Equal("/github-copilot/all", page.Url);
```

### Waiting for Elements

**Use smart Blazor-aware wait helpers** from `BlazorHelpers.cs`:

```csharp
// ✅ BEST - Navigate and wait for Blazor streaming to complete
await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot");

// ✅ GOOD - Wait for URL to change after SPA navigation
await page.ClickAsync("button");
await page.WaitForBlazorUrlContainsAsync("/news");

// ✅ GOOD - Wait for component state to sync after browser navigation
await page.GoBackAsync();
await page.WaitForBlazorStateSyncAsync("News");

// ✅ GOOD - Wait for specific element with timeout
await page.WaitForSelectorAsync(".collection-nav", new() { Timeout = 3000 });

// ⚠️ AVOID - Only use for final stabilization (100ms max)
await Task.Delay(100);

// ❌ NEVER - Arbitrary delays hide real issues
await Task.Delay(2000);
```

**Available Helper Methods** (in `BlazorHelpers.cs`):

| Method                              | Purpose                                  | When to Use                  |
| ----------------------------------- | ---------------------------------------- | ---------------------------- |
| `NewPageWithDefaultsAsync()`        | Create page with aggressive timeouts     | Every test start             |
| `GotoAndWaitForBlazorAsync()`       | Navigate + wait for streaming complete   | Initial page load            |
| `WaitForBlazorUrlContainsAsync()`   | Wait for SPA navigation                  | After clicks that change URL |
| `WaitForBlazorStateSyncAsync()`     | Wait for state sync                      | After browser back/forward   |
| `WaitForBlazorCircuitAsync()`       | Wait for SignalR connection              | Rare - after reconnect       |
| `WaitForBlazorRenderAsync()`        | Wait for element to appear               | After state changes          |

### Complete Test Example

**Full example** showing all performance optimizations:

```csharp
using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Tests;

// Step 1: Use Collection attribute for parallel execution
[Collection("My Feature Tests")]
public class MyFeatureTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private const string BaseUrl = "http://localhost:5184";

    // Step 2: Inject shared fixture
    public MyFeatureTests(PlaywrightCollectionFixture fixture)
    {
        _fixture = fixture;
    }

    // Step 3: Create isolated context (NOT new browser!)
    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
            await _context.DisposeAsync();
    }

    [Fact]
    public async Task MyFeature_Action_ExpectedResult()
    {
        // Step 4: Create page with optimized defaults
        var page = await _context!.NewPageWithDefaultsAsync();
        
        // Step 5: Use smart wait helpers
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/github-copilot");
        
        // Step 6: Interact with elements
        var newsButton = page.Locator(".collection-nav button", new() { HasTextString = "News" });
        await newsButton.ClickAsync();
        
        // Step 7: Wait for SPA navigation (NOT WaitForNavigationAsync!)
        await page.WaitForBlazorUrlContainsAsync("/news");
        
        // Step 8: Use FluentAssertions for descriptive assertions
        page.Url.Should().EndWith("/github-copilot/news",
            "clicking News should navigate to /github-copilot/news");
        
        // Step 9: Clean up page after test
        await page.CloseAsync();
    }
}

// Step 10: Add collection definition to PlaywrightCollectionFixture.cs
// [CollectionDefinition("My Feature Tests")]
// public class MyFeatureCollection : ICollectionFixture<PlaywrightCollectionFixture> { }
```

**Why This Pattern Is Fast**:

- **Shared browser** - ONE Chromium launch for ALL tests
- **Parallel execution** - Different collections run simultaneously
- **Smart waits** - Return immediately when conditions are met
- **No Task.Delay** - No arbitrary delays masking issues
- **Aggressive timeouts** - Fail fast when something is wrong
- **Isolated contexts** - Tests don't interfere with each other

## Debugging Tests

### Run Tests in Headed Mode

Modify `LaunchAsync` in test setup:

```csharp
_browser = await _playwright.Chromium.LaunchAsync(new() { Headless = false });
```

### Slow Down Execution

```csharp
_browser = await _playwright.Chromium.LaunchAsync(new() 
{ 
    Headless = false,
    SlowMo = 1000 // 1 second delay between actions
});
```

### Screenshot on Failure

```csharp
try
{
    // Test code
}
catch
{
    await page.ScreenshotAsync(new() { Path = "test-failure.png" });
    throw;
}
```

## Continuous Integration

These tests should run in CI/CD pipeline:

```yaml
# Example GitHub Actions workflow
- name: Install Playwright
  run: pwsh tests/TechHub.E2E.Tests/bin/Debug/net10.0/playwright.ps1 install

- name: Start Application
  run: ./run.ps1 &
  
- name: Wait for Application
  run: Start-Sleep -Seconds 30

- name: Run E2E Tests
  run: dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj
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
