# E2E Tests - Tech Hub

End-to-end tests using Playwright to verify complete user workflows and functionality.

**Implementation being tested**: See [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) for Blazor component patterns and [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) for API endpoint patterns.

## Critical Rules

🚫 **NEVER use `Task.Delay()` or `Thread.Sleep()` in E2E tests**

These are anti-patterns that cause flaky tests and slow execution:

```csharp
// ❌ NEVER DO THIS
await Task.Delay(300); // Arbitrary wait - flaky and slow
await Task.Delay(100); // "Small delay" - still wrong!

// ✅ USE PLAYWRIGHT POLLING INSTEAD
// Wait for specific condition with WaitForFunctionAsync
await Page.WaitForFunctionAsync(
    "() => document.activeElement?.classList.contains('skip-link')",
    new() { Timeout = 2000, PollingInterval = 50 });

// Wait for element state with Expect assertions (auto-retry)
await Assertions.Expect(element).ToBeVisibleAsync();
await Assertions.Expect(element).ToHaveClassAsync(new Regex("active"));

// Wait for DOM condition
await Page.WaitForFunctionAsync(
    "() => document.querySelectorAll('.content-item-card').length > 0",
    new() { Timeout = 5000, PollingInterval = 100 });
```

**Why `Task.Delay` is harmful**:

- **Flaky**: Fixed delays may be too short on slow machines, causing intermittent failures
- **Slow**: Fixed delays are always too long when tests pass quickly
- **Unreadable**: Doesn't express what condition you're waiting for

**Use instead**: `WaitForFunctionAsync()`, `Expect().ToBeVisibleAsync()`, `Expect().ToHaveClassAsync()`, `WaitForSelectorAsync()`

## Understanding Timeout Failures

⚠️ **Timeouts are NOT performance issues**

When Playwright tests fail with timeout errors, this does **NOT** indicate:

- Server overload or slow response times
- Performance problems with the application
- Network latency issues

**What timeouts actually mean**: Playwright couldn't find the expected element on the page within the configured timeout period. The browser keeps polling/retrying until the timeout is reached.

**Root causes of timeout failures**:

1. **Test bugs**: The test is looking for the wrong selector, element text, or attribute
2. **Broken pages**: The application has a bug and isn't rendering the expected content
3. **Changed DOM structure**: The page structure changed but tests weren't updated
4. **Navigation issues**: The page redirected to an error page (e.g., `/not-found`)
5. **State mismatches**: The test expects content that depends on data not present in test fixtures

**How to debug timeout failures**:

```powershell
# Start servers without tests
Run -WithoutTests

# Then use Playwright MCP tools to:
# 1. Navigate to the failing page
# 2. Take a snapshot to see what's actually rendered
# 3. Compare actual DOM structure with test expectations
# 4. Check browser console for JavaScript errors
```

**Common fixes**:

- Update selectors to match current DOM structure
- Fix application bugs causing incorrect rendering
- Add missing test data to fixtures
- Update expected text/attributes to match current behavior

## Running Tests

### Recommended Approach

**Always use `Run`** - it handles server startup, test execution, and cleanup automatically:

```powershell
# Run all tests (clean build + all tests, then start servers)
Run

# Run only E2E tests (clean build + E2E tests, then keep servers running)
Run -TestProject E2E.Tests

# Run specific E2E tests by name pattern
Run -TestProject E2E.Tests -TestName UrlRouting
```

⚠️ **WARNING**: Direct `dotnet test` commands **WILL FAIL** because servers aren't running. Always use `Run` which handles server startup automatically.

### Interactive Debugging with Playwright MCP

For investigating bugs or exploring UI behavior interactively:

```powershell
# Start servers without running tests
Run -WithoutTests

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

## Test Architecture

### Structure

```text
tests/TechHub.E2E.Tests/
├── Web/                                 ← Playwright-based E2E tests
│   ├── Common Component Tests          ← Test shared components once
│   │   ├── SidebarTocTests.cs          ← TOC behavior (rendering, navigation, scroll spy, keyboard)
│   │   ├── MermaidTests.cs             ← Diagram rendering (tested on genai-basics)
│   │   ├── HighlightingTests.cs        ← Code syntax highlighting (tested on genai-advanced)
│   │   ├── TabHighlightingTests.cs     ← Focus visibility (WCAG AA accessibility)
│   │   └── TabOrderingTests.cs         ← Tab order verification (WCAG A accessibility)
│   ├── Page-Specific Tests             ← Test unique page features
│   │   ├── HandbookTests.cs            ← Handbook-specific (book info, hero, CTA)
│   │   ├── LevelsOfEnlightenmentTests.cs ← Levels-specific (9 levels, videos, playlist)
│   │   ├── VSCodeUpdatesTests.cs       ← VS Code updates page
│   │   ├── GenAIBasicsTests.cs         ← GenAI Basics (13 sections, FAQ blocks, resources)
│   │   ├── GenAIAdvancedTests.cs       ← GenAI Advanced page
│   │   ├── GenAIAppliedTests.cs        ← GenAI Applied page
│   │   ├── GitHubCopilotFeaturesTests.cs ← GitHub Copilot Features page
│   │   ├── DXSpaceTests.cs             ← Developer Experience Space page
│   │   ├── AISDLCTests.cs              ← AI SDLC page (no TOC test case)
│   │   ├── AboutPageTests.cs           ← About page (5 tests)
│   │   ├── HomePageTests.cs            ← Homepage (10 tests: 3 roundups + 7 sidebar)
│   │   └── ContentDetailTests.cs       ← Content detail page (6 tests: layout, nav, tags)
│   ├── Feature Tests                   ← Test specific features
│   │   ├── UrlRoutingTests.cs          ← URL routing, collections, buttons (18 tests)
│   │   ├── NavigationTests.cs          ← Section navigation, styling (8 tests)
│   │   ├── RssTests.cs                 ← RSS feeds (9 tests)
│   │   ├── SectionCardLayoutTests.cs   ← Section cards (3 tests)
│   │   ├── SectionPageKeyboardNavigationTests.cs ← Keyboard nav (5 tests)
│   │   └── TagFilteringTests.cs        ← Tag filtering
├── Api/                                 ← Direct API testing (no Playwright)
│   ├── ApiTestFactory.cs               ← Shared WebApplicationFactory for API tests
│   ├── SectionEndpointsE2ETests.cs     ← Section endpoints (4 tests)
│   ├── ContentEndpointsE2ETests.cs     ← Content endpoints (23 tests)
│   ├── TagEndpointsE2ETests.cs         ← Tag endpoints (15 tests)
│   └── ApiEndToEndTests.cs             ← Legacy test (1 test for backwards compatibility)
├── Helpers/
│   ├── BlazorHelpers.cs                ← Blazor-specific wait patterns
│   └── PlaywrightExtensions.cs         ← Page interaction helpers
├── PlaywrightCollectionFixture.cs      ← Shared browser configuration
└── xunit.runner.json                   ← Parallel execution settings
```

### Test Organization Strategy

**Common Components vs Page-Specific Features**:

To avoid test duplication and improve maintainability, tests are organized by scope:

**Common Component Tests** (test once on representative pages):

- **SidebarTocTests.cs**: Tests table of contents behavior on `/github-copilot/vscode-updates` and `/ai/genai-basics`
  - TOC rendering and visibility
  - Link navigation and scrolling
  - Active link updates on scroll
  - Last section detection (scroll spy edge case)
  - Keyboard accessibility
  - Anchor navigation (direct URL with hash)
  - Client-side navigation (TOC scroll-spy works after navigation)
  - Console error checks
- **MermaidTests.cs**: Tests diagram rendering on `/ai/genai-basics` (has 11+ diagrams)
  - SVG rendering verification
  - Multiple diagrams on same page
  - Client-side navigation scenarios (diagrams render after navigation)
  - Multiple navigation scenarios (diagrams persist)
  - Direct load vs navigation comparison
  - Console error checks
- **HighlightingTests.cs**: Tests code syntax highlighting on `/ai/genai-advanced`
  - highlight.js initialization
  - CSS class application
  - Console error checks
- **TabHighlightingTests.cs**: Tests keyboard focus visibility on `/ai/genai-basics`
  - Links show visible focus outline (WCAG 2.1 Level AA)
  - Buttons show visible focus outline
  - Tag buttons show visible focus outline
  - Skip link shows visible focus outline
- **TabOrderingTests.cs**: Tests logical tab ordering on `/ai/genai-basics`
  - Tab order starts with skip link (WCAG 2.1 Level A)
  - Navigation elements are in logical order
  - Main content is reachable via keyboard
  - Sidebar elements are reachable via keyboard
  - Overall tab order is predictable

**Why these test pages?**

- `/ai/genai-basics`: Has complex nested TOC (55 links), 11+ mermaid diagrams, AND code blocks with syntax highlighting
- Testing on 1 page provides sufficient coverage without duplication

**Page-Specific Tests** (test unique features only):

Each custom page has its own test file with tests for:

- **Page loading**: Title verification, successful HTTP response
- **Content display**: Main heading, paragraphs, page-specific elements
- **Unique features**: Page-specific components and functionality

**Test Files**:

- **HandbookTests.cs**: `/github-copilot/handbook` - Book-specific features (book cover, authors, Amazon link, hero section, CTA button)
- **LevelsOfEnlightenmentTests.cs**: `/github-copilot/levels-of-enlightenment` - Levels-specific features (9 levels, video embeds, overview image, playlist link)
- **VSCodeUpdatesTests.cs**: `/github-copilot/vscode-updates` - VS Code updates page (dynamic title, content display)
- **GenAIBasicsTests.cs**: `/ai/genai-basics` - GenAI-specific features (13 sections, FAQ blocks, resource links)
- **GenAIAdvancedTests.cs**: `/ai/genai-advanced` - GenAI Advanced page (page load, heading, console errors)
- **GenAIAppliedTests.cs**: `/ai/genai-applied` - GenAI Applied page (content sections, paragraphs)
- **GitHubCopilotFeaturesTests.cs**: `/github-copilot/features` - GitHub Copilot Features page
- **DXSpaceTests.cs**: `/devops/dx-space` - Developer Experience Space page (framework sections: DORA, SPACE, DevEx)
- **AISDLCTests.cs**: `/ai/sdlc` - AI SDLC page (test case for pages without TOC)
- **AboutPageTests.cs**: `/about` - About page (team members, photos, social links, navigation)
- **HomePageTests.cs**: `/` (homepage) - Latest roundup section (3 tests), sidebar sections (7 tests: display, latest items, tags, navigation)
- **ContentDetailTests.cs**: Content detail pages - Roundup detail page structure, layout, sub-nav, sidebar, tags, navigation from homepage

**Benefits**:

- **No duplication**: Common components tested once, not on every page
- **Faster test runs**: Fewer redundant tests
- **Easier maintenance**: Common component changes only require updating one test file
- **Clear separation**: Easy to find tests for specific features

**Keyboard Navigation Note**:

The `/github-copilot/vscode-updates` page has highlight.js which makes code blocks focusable (tabindex), changing tab order. This is expected behavior and doesn't affect keyboard accessibility of interactive elements. Keyboard navigation tests run on `/ai/genai-basics` instead.

### API Test Organization

API E2E tests are organized by endpoint group for maintainability:

- **ApiCollectionFixture.cs**: Shared WebApplicationFactory for all API E2E test classes
- **SectionEndpointsE2ETests.cs**: Tests for GET /api/sections and GET /api/sections/{name}
- **ContentEndpointsE2ETests.cs**: Tests for GET /api/content and GET /api/content/filter  
- **TagEndpointsE2ETests.cs**: Tests for GET /api/tags/all and GET /api/tags/cloud

**Pattern**: One test file per logical endpoint group, all sharing the same collection fixture.

**Test Class Structure**:

- `[Collection("API E2E Tests")]` attribute for factory sharing
- Primary constructor: Inject `ApiCollectionFixture`
- Create HttpClient: `fixture.Factory.CreateClient()`
- Test methods use FluentAssertions for validation
- Deserialize responses with `ReadFromJsonAsync<T>()`

**Performance**: Shared factory reduces API test time from ~12s to ~6.5s by avoiding redundant app startups.

**See**: [Api/TagEndpointsE2ETests.cs](Api/TagEndpointsE2ETests.cs), [Api/ContentEndpointsE2ETests.cs](Api/ContentEndpointsE2ETests.cs), [Api/SectionEndpointsE2ETests.cs](Api/SectionEndpointsE2ETests.cs) for complete examples

**Total**: 83 E2E test cases across all Web test files:

- Common Component Tests: 16 tests (2 Mermaid + 2 console + 3 navigation + 7 TOC + 2 TOC console)
- Page-Specific Tests: 48 tests
- Feature Tests: 45+ tests
- Accessibility Tests: 9 tests (4 tab highlighting + 5 tab ordering)

Note: Updated after test reorganization (merged homepage tests, added navigation tests to Mermaid/TOC, added GenAIAdvancedTests, added accessibility tests)

### Shared Page Pattern

**CRITICAL**: All test classes use a consistent shared page pattern:

**Key Elements**:

- `[Collection("Feature Name")]` attribute for browser sharing
- `IAsyncLifetime` interface for async setup/teardown  
- Primary constructor: Inject `PlaywrightCollectionFixture`
- Private fields: `_fixture`, `_context`, `_page`
- Property: `IPage Page => _page ?? throw ...` (clean access, no `!` operators)
- `InitializeAsync()`: Create context and page with `NewPageWithDefaultsAsync()`
- `DisposeAsync()`: Close page and dispose context

**Benefits**:

- Each test method gets its own fresh page
- Automatic cleanup even if test fails
- Clean test code - use `Page` property instead of `_page!` everywhere
- No manual page cleanup in test methods

**See**: [Web/UrlRoutingTests.cs](Web/UrlRoutingTests.cs), [Web/NavigationTests.cs](Web/NavigationTests.cs) for complete examples

### Browser Configuration

All browser launch options are centralized in [PlaywrightCollectionFixture.cs](PlaywrightCollectionFixture.cs):

**Launch Options**:

- `Headless: true` - No GUI
- `Channel: "chrome"` - Use Chrome (not chromium-headless-shell)
- `Timeout: 5000` - 5-second launch timeout
- DevContainer compatibility args: `--no-sandbox`, `--disable-setuid-sandbox`
- CORS for local testing: `--disable-web-security`

**Browser Context Settings**:

- Viewport: `1920x1080` (common desktop resolution)
- Locale: `en-US`
- Timezone: `Europe/Brussels` (matches application)
- `IgnoreHTTPSErrors: true` (allow self-signed certs)

**See**: [PlaywrightCollectionFixture.cs](PlaywrightCollectionFixture.cs) lines 40-60 for complete configuration

**Why these settings?**

- **Chrome channel**: Reliable headless testing + works with Playwright MCP tools
- **DevContainer compatibility**: `--no-sandbox` and `--disable-setuid-sandbox` required for containers
- **Local testing**: `--disable-web-security` allows CORS for localhost
- **Consistent viewport**: 1920x1080 matches common desktop resolution
- **Timezone**: Europe/Brussels matches application configuration

### Performance Optimizations

**1. Shared Test Infrastructure** (faster startup):

**Web Tests (Playwright)**:

- ONE browser launch for all Web test classes
- Each test gets isolated context (separate cookies, storage)
- Defined in [PlaywrightCollectionFixture.cs](PlaywrightCollectionFixture.cs), shared via xUnit Collection Fixtures

**API Tests (WebApplicationFactory)**:

- Shared factory instance per collection via [Api/ApiCollectionFixture.cs](Api/ApiCollectionFixture.cs)
- Reduces API test time from ~12s (3 factories × 4s each) to ~6.5s (parallel execution)
- All API test classes use `[Collection("API E2E Tests")]` to share fixture

**2. Parallel Test Execution** (4x throughput):

- Test collections run in parallel
- Configured in [xunit.runner.json](xunit.runner.json): `maxParallelThreads: 4` (fixed thread count)
- Each collection gets isolated resources (browser contexts, HTTP clients)

**3. Optimized Timeouts** (fail fast):

- Element operations: 5s (vs Playwright default 30s)
- Page loads: 10s (vs Playwright default 30s)
- Assertions: 5s (vs Playwright default 30s)
- Managed centrally via constants in `BlazorHelpers.cs`

### Blazor JavaScript Initializers (Ready Detection)

**The Challenge**: Blazor Server uses SignalR for interactivity. After the initial HTML loads, Blazor needs to:

1. Initialize the Blazor runtime (`window.Blazor`)
2. Establish the SignalR circuit (for Server rendering)
3. Attach event handlers (`@onclick`, `@onchange`, etc.)

Until step 3 completes, clicking buttons does nothing. Tests that click too early will fail.

**The Solution**: We use [Blazor JavaScript initializers](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup) to set flags when each stage completes.

**File**: [TechHub.Web.lib.module.js](../../src/TechHub.Web/wwwroot/TechHub.Web.lib.module.js) (MUST be in wwwroot root, not wwwroot/js)

**JavaScript Initializer Functions**:

- `afterWebStarted()` - Sets `window.__blazorWebReady = true`
- `afterServerStarted()` - Sets `window.__blazorServerReady = true`  
- `afterWebAssemblyStarted()` - Sets `window.__blazorWasmReady = true`

**Test Detection**: `BlazorHelpers.WaitForBlazorReadyAsync()` waits for these flags using `page.WaitForFunctionAsync()`

**Key Methods That Use This**:

- `GotoRelativeAsync()` - Navigates and waits for Blazor ready
- `ClickBlazorElementAsync()` - Clicks element after ensuring Blazor ready
- `ClickAndNavigateAsync()` - High-level click + navigate + verify pattern

**Why URL Waiting Is Default**: `ClickBlazorElementAsync()` waits for URL changes by default (`waitForUrlChange = true`) because:

- Blazor Server updates URLs via SignalR (not HTTP redirects)
- Standard `WaitForNavigationAsync()` doesn't detect SignalR URL changes
- Most Blazor element clicks (navigation, tag toggles, filters) DO change URLs

## Writing New Tests

### Test Naming Convention

Use the format: `ComponentOrFeature_Action_ExpectedResult`

Examples:

- `NavigateToSection_DefaultsToAllCollection`
- `ClickCollectionButton_UpdatesURL`
- `AllCollection_ShowsAllContentFromSection`

### Playwright Expect Assertions and Wait Patterns

**CRITICAL**: Use Playwright's built-in `Expect` assertions instead of explicit waits for better performance and reliability.

**✅ PREFER: Playwright Expect Assertions**

Playwright's `Expect` assertions have intelligent auto-waiting and polling built-in:

```csharp
// ✅ BEST - Auto-retrying assertion with smart waiting
await Assertions.Expect(element).ToBeVisibleAsync();
await Assertions.Expect(element).ToContainTextAsync("expected text");
await Assertions.Expect(element).ToHaveClassAsync(new Regex("active"));

// ❌ AVOID - Explicit wait then manual check
await element.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 5000 });
var isVisible = await element.IsVisibleAsync();
isVisible.Should().BeTrue();
```

**Why Expect is better**:

- Automatically polls until condition is met (smarter than fixed waits)
- Clearer test intent - assertion shows what you're verifying
- Better error messages when assertions fail
- Faster - only waits as long as needed, not fixed timeout

**🚫 AVOID: NetworkIdle Waits**

NetworkIdle waits add a **500ms delay** after ALL network activity stops - expensive and usually unnecessary:

```csharp
// ❌ SLOW - Waits 500ms after all network requests complete
await Page.WaitForURLAsync("**/ai", new() { WaitUntil = WaitUntilState.NetworkIdle });

// ✅ FAST - Default wait (DOMContentLoaded) is sufficient
await Page.WaitForURLAsync("**/ai");
```

**When NetworkIdle is needed**: Only if you specifically need to wait for ALL network requests to complete (rare - usually only for screenshot tests or network monitoring).

**Default Wait Behavior**:

Playwright's default wait strategy (`WaitUntilState.Load` or `WaitUntilState.DOMContentLoaded`) is sufficient for most navigation scenarios:

- Page content is loaded and ready
- Blazor JavaScript has initialized (via our `WaitForBlazorReadyAsync()`)
- Elements are interactive

**Performance Impact**:

- NetworkIdle wait: Adds fixed 500ms delay per occurrence
- Explicit WaitForAsync: Waits full timeout even if condition met earlier  
- Expect assertions: Polls intelligently, exits as soon as condition is met

**Examples from our codebase**:

```csharp
// Navigation without NetworkIdle (fast)
await Page.WaitForURLAsync("**/ai/genai-basics");

// Element visibility with Expect (smart polling)
await Assertions.Expect(tocElement).ToBeVisibleAsync();

// Class detection with polling (handles async DOM updates)
await Assertions.Expect(link).ToHaveClassAsync(new Regex("active"),
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 });
```

### Using BlazorHelpers

**ALWAYS use helper methods** from BlazorHelpers.cs for consistent timeout management:

**Navigation**: `GotoRelativeAsync()`, `WaitForBlazorUrlContainsAsync()`, `ClickAndNavigateAsync()`  
**Assertions**: `AssertUrlEndsWithAsync()`, `AssertElementVisibleAsync()`, `AssertElementContainsTextBySelectorAsync()`  
**Interactions**: `ClickBlazorElementAsync()`, `TextContentWithTimeoutAsync()`, `GetHrefAsync()`

**See**: [Helpers/BlazorHelpers.cs](Helpers/BlazorHelpers.cs) and [Helpers/PlaywrightExtensions.cs](Helpers/PlaywrightExtensions.cs) for complete method list

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

Use FluentAssertions with descriptive reasons:

```csharp
// ✅ Good - descriptive with reason
Page.Url.Should().EndWith("/github-copilot/all",
    "navigating to a section without collection should default to /section/all");

// ❌ Bad - no context
Assert.Equal("/github-copilot/all", Page.Url);
```

### Writing New Test Classes

**Required structure for E2E test classes**:

1. **Collection attribute**: `[Collection("Feature Name Tests")]` for browser sharing
2. **Primary constructor**: Inject `PlaywrightCollectionFixture`
3. **IAsyncLifetime**: Implement for async setup/teardown
4. **Page property**: `private IPage Page => _page ?? throw ...` for clean access
5. **InitializeAsync**: Create context and page with `NewPageWithDefaultsAsync()`
6. **DisposeAsync**: Close page and dispose context

**Add collection definition** to `PlaywrightCollectionFixture.cs`:

```csharp
[CollectionDefinition("Feature Name Tests")]
public class FeatureNameCollection : ICollectionFixture<PlaywrightCollectionFixture> { }
```

See existing test classes in `Web/` for complete examples.

## TOC Scroll Synchronization Patterns

**CRITICAL**: Testing Table of Contents (TOC) scroll-spy behavior requires careful timing synchronization between browser scrolling and JavaScript DOM updates.

### The Problem: Race Conditions

The TOC scroll-spy ([toc-scroll-spy.js](../../src/TechHub.Web/wwwroot/js/toc-scroll-spy.js)) uses `requestAnimationFrame` to update active classes:

1. User clicks TOC link or scrolls
2. Browser scrolls to target position
3. `scroll` event fires
4. `handleScroll()` schedules `requestAnimationFrame`
5. RAF callback calls `updateActiveHeading()`
6. Active class is updated in DOM

**The race**: If tests check for the active class immediately after clicking, the RAF callback may not have executed yet.

### Solution 1: Playwright Polling (Recommended)

Use Playwright's built-in `Expect().ToHaveClassAsync()` which automatically polls until the class appears:

```csharp
// ✅ CORRECT - Polls for active class (max 500ms)
await Assertions.Expect(secondLink).ToHaveClassAsync(
    new Regex("active"), 
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 }
);

// ❌ WRONG - Immediate check (race condition)
var hasActiveClass = await secondLink.EvaluateAsync<bool>("el => el.classList.contains('active')");
hasActiveClass.Should().BeTrue();
```

**Why this works**:

- Playwright polls every ~100ms checking for the class
- Handles timing variations across different machines/load
- Fails with clear timeout error if class never appears
- More reliable than trying to predict exact RAF timing

**Required imports**:

```csharp
using System.Text.RegularExpressions;  // For Regex
using Microsoft.Playwright;             // For Assertions
```

### Solution 2: Event-Based Waiting (Helper Methods)

The `ClickAndWaitForScrollAsync()` and `EvaluateAndWaitForScrollAsync()` helpers use browser's native `scrollend` event:

```csharp
// Click TOC link and wait for scroll to complete
await secondLink.ClickAndWaitForScrollAsync();

// Execute scroll and wait for completion
await Page.EvaluateAndWaitForScrollAsync("window.scrollBy(0, 500)");
```

**How it works**:

1. Set up `scrollend` event listener (fires when scrolling stops)
2. Set up `scroll` event listener (detects if scroll started)
3. Execute action (click link or run JavaScript)
4. Wait for `scrollend` event
5. Wait 2 RAF cycles for TOC scroll-spy to update
6. Timeout after 50ms if no scroll detected (already at target)

**Why 2 RAF cycles?**

The TOC scroll-spy schedules its update inside a RAF callback:

```javascript
handleScroll() {
    if (!this.ticking) {
        window.requestAnimationFrame(() => {
            this.updateActiveHeading();  // ← Happens in RAF callback
            this.ticking = false;
        });
        this.ticking = true;
    }
}
```

So we need:

- 1st RAF: `scrollend` event fires
- 2nd RAF: TOC scroll-spy's RAF callback executes

**IMPORTANT**: Even after `ClickAndWaitForScrollAsync()`, still use Playwright polling for assertions (Solution 1). The helper ensures scrolling completes, but the final class update timing can still vary.

### Pattern for TOC Tests

Complete pattern combining both solutions:

```csharp
[Fact]
public async Task CustomPage_TocLinks_ShouldScrollAndHighlight()
{
    // Arrange
    await Page.GotoRelativeAsync("/custom-page");
    var tocLinks = Page.Locator(".sidebar-toc a");
    var secondLink = tocLinks.Nth(1);
    
    // Act - Click and wait for scroll to complete
    await secondLink.ClickAndWaitForScrollAsync();
    
    // Assert - URL should update
    Page.Url.Should().Contain("#", "TOC link should add hash to URL");
    
    // Assert - Active class should appear (with polling)
    await Assertions.Expect(secondLink).ToHaveClassAsync(
        new Regex("active"),
        new LocatorAssertionsToHaveClassOptions { Timeout = 500 }
    );
}
```

### Common TOC Test Scenarios

**Scenario 1: Click TOC link and verify active class**

```csharp
var tocLink = Page.Locator(".sidebar-toc a").Nth(1);
await tocLink.ClickAndWaitForScrollAsync();
await Assertions.Expect(tocLink).ToHaveClassAsync(new Regex("active"), 
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 });
```

**Scenario 2: Scroll to bottom and verify last heading active**

```csharp
await Page.EvaluateAndWaitForScrollAsync("window.scrollTo(0, document.body.scrollHeight)");
var lastHeadingLink = Page.Locator(".sidebar-toc a").Last;
await Assertions.Expect(lastHeadingLink).ToHaveClassAsync(new Regex("active"),
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 });
```

**Scenario 3: Navigate between sections**

```csharp
// Click first link
await firstLink.ClickAndWaitForScrollAsync();
await Assertions.Expect(firstLink).ToHaveClassAsync(new Regex("active"),
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 });

// Click second link
await secondLink.ClickAndWaitForScrollAsync();
await Assertions.Expect(secondLink).ToHaveClassAsync(new Regex("active"),
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 });

// First link should no longer be active
await Assertions.Expect(firstLink).Not.ToHaveClassAsync(new Regex("active"),
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 });
```

### Debugging TOC Issues

**Enable debug mode** in browser console:

```javascript
toggleTocDebug()  // Shows red line at detection position (30% from top)
```

**Check timing in tests**:

```csharp
// Add delays to observe behavior (remove before committing)
await Page.WaitForTimeoutAsync(1000);  // 1 second delay

// Check class list manually
var classes = await link.GetAttributeAsync("class");
Console.WriteLine($"Classes: {classes}");
```

**Common issues**:

- **Test fails immediately**: Not using polling, checking too early
- **Test fails inconsistently**: Race condition, need Playwright polling
- **Test times out**: Element selector wrong, or scroll didn't trigger
- **Active class never appears**: TOC scroll-spy not initialized, JavaScript error

### Performance Notes

- Playwright polling adds ~100-500ms per assertion (acceptable for reliability)
- `scrollend` event is more reliable than arbitrary delays  
- TOC-specific tests: ~500-1000ms per test (legitimate - includes scrolling, RAF callbacks, and active class detection)
- General navigation tests: <200ms per test after removing NetworkIdle waits (see [Playwright Expect Assertions and Wait Patterns](#playwright-expect-assertions-and-wait-patterns))

### Related Files

- [toc-scroll-spy.js](../../src/TechHub.Web/wwwroot/js/toc-scroll-spy.js) - Production TOC scroll-spy implementation
- [BlazorHelpers.cs](Helpers/BlazorHelpers.cs) - Scroll synchronization helpers (`ClickAndWaitForScrollAsync`, `EvaluateAndWaitForScrollAsync`)
- [VSCodeUpdatesTests.cs](Web/VSCodeUpdatesTests.cs) - Reference implementation for TOC tests
- [LevelsOfEnlightenmentTests.cs](Web/LevelsOfEnlightenmentTests.cs) - Multiple TOC test scenarios
- [HandbookTests.cs](Web/HandbookTests.cs) - TOC keyboard navigation tests

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

- [Root AGENTS.md](../../AGENTS.md) - Overall development workflow and TDD approach
- [tests/AGENTS.md](../AGENTS.md) - Testing strategies across all layers
- [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) - Blazor component patterns
