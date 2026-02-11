# E2E Tests - Tech Hub

End-to-end tests using Playwright to verify complete user workflows and functionality.

**Implementation being tested**: See [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) for Blazor component patterns and [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) for API endpoint patterns.

## Critical Rules

🚫 **NEVER use `Task.Delay()`, `Thread.Sleep()`, or `WaitForTimeoutAsync()` in E2E tests** — see [Wait Pattern Best Practices](#wait-pattern-best-practices) for what to use instead.

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

## Interactive Debugging with Playwright MCP

For investigating bugs or exploring UI behavior interactively:

```powershell
Run -WithoutTests  # Start servers without running tests
```

Then use Playwright MCP tools in GitHub Copilot Chat to navigate pages, take snapshots, test interactions, and verify behavior. Faster than writing tests for initial exploration.

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
│   │   ├── SectionCardCustomPagesTests.cs ← Custom page section cards
│   │   ├── SectionPageKeyboardNavigationTests.cs ← Keyboard nav (5 tests)
│   │   ├── TagFilteringTests.cs        ← Tag filtering
│   │   ├── DateRangeSliderTests.cs     ← Date range slider filtering
│   │   ├── DynamicTagCountsTests.cs    ← Dynamic tag count updates
│   │   ├── InfiniteScrollTests.cs      ← Infinite scroll pagination
│   │   └── InfiniteScrollWithTagsTests.cs ← Infinite scroll with tag filtering
│   └── Proof Tests                     ← Standalone verification tests
│       └── IntersectionObserverProofTests.cs ← IO works in headless Chrome (11 tests)
├── Api/                                 ← Direct API testing (no Playwright)
│   ├── ApiCollectionFixture.cs         ← Shared WebApplicationFactory for API tests
│   └── ContentEndpointsE2ETests.cs     ← Content API endpoints (4 tests)
├── Helpers/
│   └── BlazorHelpers.cs                ← Blazor-specific wait patterns
├── PlaywrightCollectionFixture.cs      ← Shared browser (assembly fixture)
├── PlaywrightTestBase.cs               ← Abstract base class for Web E2E tests
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
- **ContentEndpointsE2ETests.cs**: Tests for GET /api/sections and related content endpoints (10 tests)
  - Section endpoints (4 tests: list, get by name, 404, case-sensitivity)
  - Collection items with pagination and tag filtering (2 tests)
  - Content detail endpoint for internal and external collections (2 tests)
  - Tag cloud endpoint (1 test)
  - Custom pages endpoint (1 test)

**Pattern**: One test file per logical endpoint group, all sharing the same collection fixture.

**Test Class Structure**:

- `[Collection("API E2E Tests")]` attribute for factory sharing
- Primary constructor: Inject `ApiCollectionFixture`
- Create HttpClient: `fixture.Factory.CreateClient()`
- Test methods use FluentAssertions for validation
- Deserialize responses with `ReadFromJsonAsync<T>()`

**Performance**: Shared factory reduces API test time from ~12s to ~6.5s by avoiding redundant app startups.

**Coverage**: API E2E tests verify production integration (PostgreSQL, real markdown files, HTTP pipeline) for critical endpoints, complementing the comprehensive integration test suite.

**See**: [Api/ContentEndpointsE2ETests.cs](Api/ContentEndpointsE2ETests.cs) for complete examples

**Total**: 215 E2E test cases across all test files.

Note: Test count includes common component tests, page-specific tests, feature tests (infinite scroll, tag filtering, date range slider, dynamic tag counts, keyboard navigation), accessibility tests, API E2E tests, and IntersectionObserver proof tests.

### Shared Page Pattern

**CRITICAL**: All Web E2E test classes extend the `PlaywrightTestBase` abstract base class, which handles per-test browser context and page lifecycle.

**Key Elements**:

- Extend `PlaywrightTestBase` (no `[Collection]` attribute needed)
- `PlaywrightCollectionFixture` is shared via **assembly fixture** (`[assembly: AssemblyFixture(...)]` in xUnit v3)
- Primary constructor: call `base(fixture)`
- Protected properties: `Page` and `Context` for clean access (no `!` operators)
- `InitializeAsync()`: Creates context and page with `NewPageWithDefaultsAsync()`
- `DisposeAsync()`: Closes page and disposes context
- Both lifecycle methods return `ValueTask` (xUnit v3 requirement)

**Example**:

```csharp
public class MyFeatureTests : PlaywrightTestBase
{
    public MyFeatureTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Feature_Action_ExpectedResult()
    {
        await Page.GotoRelativeAsync("/my-page");
        // ... test code using Page property
    }
}
```

**Benefits**:

- Each test method gets its own fresh page
- Automatic cleanup even if test fails
- Clean test code — use `Page` property instead of `_page!` everywhere
- No manual page cleanup in test methods
- No boilerplate — constructor, fields, and lifecycle are handled by the base class
- `Context` property available for multi-page tests (e.g., `UrlRoutingTests`)

**See**: [PlaywrightTestBase.cs](PlaywrightTestBase.cs) for the base class, [Web/NavigationTests.cs](Web/NavigationTests.cs) for a complete example

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
- `PlaywrightCollectionFixture` is shared via **xUnit v3 assembly fixture** (`[assembly: AssemblyFixture(...)]`)
- All Web test classes extend `PlaywrightTestBase` — no `[Collection]` attributes needed

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

### Wait Pattern Best Practices

This section consolidates ALL guidance on waiting and timing. Follow these patterns to write stable, fast, non-flaky tests.

#### Golden Rule: Never Use Arbitrary Delays

```csharp
// ❌ NEVER - These are ALWAYS wrong
await Task.Delay(300);
await Page.WaitForTimeoutAsync(500);
await Thread.Sleep(100);

// ✅ ALWAYS - Wait for a specific condition
await Assertions.Expect(element).ToBeVisibleAsync();
await Page.WaitForFunctionAsync("() => condition", options);
await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
```

**Why arbitrary delays are harmful**:

- **Flaky**: Too short on slow machines → intermittent failures
- **Slow**: Always too long when tests pass quickly
- **Unclear**: Doesn't express what condition you're actually waiting for

#### Quick Reference: What To Use Instead

| Waiting for... | ❌ Anti-pattern | ✅ Correct pattern |
|---|---|---|
| Element to appear | `WaitForTimeoutAsync(500)` | `Assertions.Expect(el).ToBeVisibleAsync()` |
| CSS class applied | `WaitForTimeoutAsync(1000)` | `Assertions.Expect(el).ToHaveClassAsync(new Regex("expanded"))` |
| Console errors after load | `WaitForTimeoutAsync(500)` | `GotoRelativeAsync` already waits for `__scriptsReady` — no extra wait needed |
| Blazor re-render complete | `WaitForTimeoutAsync(500)` | `Page.WaitForBlazorReadyAsync()` |
| DOM condition (card count) | `WaitForTimeoutAsync(500)` | `Page.WaitForFunctionAsync("() => document.querySelectorAll('.card').length > 0")` |
| JS lib init (highlight.js) | `WaitForTimeoutAsync(500)` | `Page.WaitForFunctionAsync("() => document.querySelector('pre code.hljs') !== null")` |
| Scroll to finish | `WaitForTimeoutAsync(200)` | `Page.WaitForFunctionAsync` polling scroll position |
| Expand/collapse toggle | `WaitForTimeoutAsync(1000)` | `Assertions.Expect(content).ToHaveClassAsync(new Regex("expanded"))` |
| No navigation occurred | `WaitForTimeoutAsync(200)` | `Page.WaitForBlazorReadyAsync()` then assert URL unchanged |
| Browser paint settle | `Task.Delay(100)` | `WaitForFunctionAsync` with double `requestAnimationFrame` |
| Infinite scroll next batch | `Mouse.WheelAsync()` or `loading="eager"` | `Page.ScrollToLoadMoreAsync(expectedCount)` |
| Infinite scroll end | Manual wheel scrolling loop | `Page.ScrollToEndOfContentAsync()` |
| Lazy-loaded element visible | `loading="eager"` attribute hack | `element.ScrollIntoViewAsync()` or `element.ScrollIntoViewIfNeededAsync()` |
| Focus after navigation reset | `Locator(":focus").EvaluateAsync()` | `WaitForConditionAsync` + `Page.EvaluateAsync()` |
| Custom timeout value | `Timeout = 10000` (hardcoded) | `BlazorHelpers.DefaultAssertionTimeout` (5000ms) |

#### Pattern 1: Expand/Collapse Animations

Custom pages use `classList.toggle('expanded')` — an immediate class toggle with no CSS transition. Wait for the class directly:

```csharp
// ❌ WRONG
await header.ClickBlazorElementAsync(waitForUrlChange: false);
await Page.WaitForTimeoutAsync(1000);

// ✅ CORRECT - Wait for class to appear
await header.ClickBlazorElementAsync(waitForUrlChange: false);
await Assertions.Expect(content).ToHaveClassAsync(
    new Regex("expanded"),
    new() { Timeout = 3000 });
```

#### Pattern 2: Console Error Tests

`GotoRelativeAsync` already waits for `window.__scriptsReady` (set after all JS modules finish loading). No additional wait is needed — by the time it returns, any console errors from script loading have already been emitted.

```csharp
// ❌ WRONG - arbitrary delay
await Page.GotoRelativeAsync(url);
await Page.WaitForTimeoutAsync(500);

// ❌ WRONG - NetworkIdle is unreliable with Blazor Server's always-active WebSocket
await Page.GotoRelativeAsync(url);
await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

// ✅ CORRECT - GotoRelativeAsync already waits for __scriptsReady
await Page.GotoRelativeAsync(url);
// Just assert console errors here — scripts are already loaded
```

**Why NOT `NetworkIdle`**: Blazor Server maintains a persistent SignalR WebSocket connection. `NetworkIdle` waits for 500ms of zero network activity, which may never happen because the WebSocket counts as an active connection. This causes flaky timeouts.

#### Pattern 3: JavaScript Library Initialization

Wait for the library to apply its effects to the DOM:

```csharp
// ❌ WRONG
await Page.WaitForTimeoutAsync(500); // Hope highlight.js ran

// ✅ CORRECT - Wait for hljs classes to appear
await Page.WaitForFunctionAsync(
    "() => document.querySelector('pre code.hljs') !== null",
    new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });
```

#### Pattern 4: Scroll Position Stability

After scrolling with `Mouse.WheelAsync()`, wait for scroll position to reach the target:

```csharp
// ❌ WRONG
await Page.Mouse.WheelAsync(0, targetY);
await Page.WaitForTimeoutAsync(200);

// ✅ CORRECT - Poll for scroll position
await Page.Mouse.WheelAsync(0, targetY);
await Page.WaitForFunctionAsync(
    @"(targetY) => Math.abs(window.scrollY - targetY) < 100",
    targetY,
    new PageWaitForFunctionOptions { Timeout = 3000, PollingInterval = 50 });
```

For scrolling to the bottom of the page:

```csharp
await Page.Mouse.WheelAsync(0, scrollHeight);
await Page.WaitForFunctionAsync(
    @"() => Math.abs((window.innerHeight + window.scrollY) - document.documentElement.scrollHeight) < 50",
    new PageWaitForFunctionOptions { Timeout = 3000, PollingInterval = 50 });
```

#### Pattern 5: Browser Paint/Layout Settle

When the browser needs to complete a paint cycle after DOM mutations (e.g., after mermaid diagram rendering):

```csharp
// ❌ WRONG
await Task.Delay(100);

// ✅ CORRECT - Double requestAnimationFrame ensures paint completed
await page.WaitForFunctionAsync(
    "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(() => resolve(true))))",
    new PageWaitForFunctionOptions { Timeout = 2000 });
```

#### Pattern 6: Verifying No Navigation Occurred

After an action that should NOT navigate (e.g., clicking an expand button):

```csharp
// ❌ WRONG
await button.ClickBlazorElementAsync(waitForUrlChange: false);
await Page.WaitForTimeoutAsync(200);

// ✅ CORRECT - Wait for JS to complete, then check URL
var initialUrl = Page.Url;
await button.ClickBlazorElementAsync(waitForUrlChange: false);
await Page.WaitForBlazorReadyAsync();
Page.Url.Should().Be(initialUrl, "clicking expand should not navigate");
```

#### Pattern 7: Blazor Re-render After Filtering

After actions that trigger Blazor re-renders (e.g., tag filtering):

```csharp
// ❌ WRONG
await tagButton.ClickBlazorElementAsync();
await Page.WaitForTimeoutAsync(500);

// ✅ CORRECT - Wait for DOM to update
await tagButton.ClickBlazorElementAsync();
await Page.WaitForFunctionAsync(
    "() => document.querySelectorAll('.card').length > 0",
    new PageWaitForFunctionOptions { Timeout = 5000, PollingInterval = 100 });
await Page.WaitForBlazorReadyAsync();
```

#### Pattern 8: Infinite Scroll (IntersectionObserver)

IntersectionObserver fires callbacks during rendering frames. In headless Chrome, IO works correctly — flaky tests are caused by a **timing race**, not a capability issue. The race: Blazor re-attaches the observer in `OnAfterRenderAsync` after each batch load, but tests may scroll before the new observer is attached.

**Solution**: `infinite-scroll.js` sets `window.__ioObserverReady = true` after `observer.observe()` and resets it to `false` in `dispose()`. Tests wait for this signal before scrolling.

```csharp
// ❌ WRONG - scroll before IO observer is attached
await Page.Mouse.WheelAsync(0, scrollHeight);

// ❌ WRONG - use loading="eager" to bypass lazy loading
// This masks the real issue and disables the feature being tested

// ✅ CORRECT - use ScrollToLoadMoreAsync (waits for __ioObserverReady)
await Page.ScrollToLoadMoreAsync(expectedItemCount: 40);

// ✅ CORRECT - use ScrollToEndOfContentAsync for end-of-collection tests
await Page.ScrollToEndOfContentAsync();
```

**How the scroll helpers work** (see `BlazorHelpers.cs`):

1. Wait for `window.__ioObserverReady === true` (observer attached)
2. Scroll `#scroll-trigger` into view with `scrollIntoView({ behavior: 'instant' })`
3. Double `requestAnimationFrame` to ensure IO callback is delivered
4. Wait for new items to appear in the DOM
5. Loop until target count reached or end-of-content marker appears

**Key insight**: Never use `Mouse.WheelAsync()` for infinite scroll tests — use `scrollIntoView()` on the `#scroll-trigger` sentinel instead. `scrollIntoView` guarantees the sentinel enters the viewport, triggering the IO callback reliably.

#### Pattern 9: Focus Detection After Programmatic Focus Reset

After Blazor's enhanced navigation, `document.activeElement` resets to `<body>`. When testing that Tab restarts from the beginning, don't use `Locator(":focus").EvaluateAsync()` — the `:focus` locator will timeout because `<body>` doesn't match any element Playwright can find.

```csharp
// ❌ WRONG - :focus locator times out after focus reset to body
await Page.Keyboard.PressAsync("Tab");
var focusedTag = await Page.Locator(":focus").EvaluateAsync<string>("el => el.tagName");

// ✅ CORRECT - wait for real focus, then evaluate directly on the page
await Page.Keyboard.PressAsync("Tab");
await Page.WaitForConditionAsync(
    "() => document.activeElement && document.activeElement !== document.body");
var focusedTag = await Page.EvaluateAsync<string>("() => document.activeElement.tagName");
```

#### Prefer Playwright Expect Assertions

Playwright's `Expect` assertions have intelligent auto-waiting and polling built-in. Always prefer them over explicit waits followed by manual checks:

```csharp
// ✅ BEST - Auto-retrying with smart polling
await Assertions.Expect(element).ToBeVisibleAsync();
await Assertions.Expect(element).ToContainTextAsync("expected text");
await Assertions.Expect(element).ToHaveClassAsync(new Regex("active"));

// ❌ AVOID - Explicit wait then manual check
await element.WaitForAsync(new() { State = WaitForSelectorState.Visible });
var isVisible = await element.IsVisibleAsync();
isVisible.Should().BeTrue();
```

**Performance summary**:

- `Expect` assertions: Polls intelligently, exits as soon as condition is met (fastest)
- `WaitForFunctionAsync`: Polls at configurable intervals (good for custom conditions)
- `WaitForLoadStateAsync(NetworkIdle)`: Fixed ~500ms delay after network quiet (use sparingly)
- `WaitForTimeoutAsync` / `Task.Delay`: Fixed delay regardless of state (NEVER use)

### Using BlazorHelpers

**ALWAYS use helper methods** from BlazorHelpers.cs for consistent timeout management:

**Navigation**: `GotoRelativeAsync()`, `WaitForBlazorUrlContainsAsync()`, `ClickAndNavigateAsync()`  
**Assertions**: `AssertUrlEndsWithAsync()`, `AssertElementVisibleAsync()`, `AssertElementContainsTextBySelectorAsync()`  
**Interactions**: `ClickBlazorElementAsync()`, `TextContentWithTimeoutAsync()`, `GetHrefAsync()`

**See**: [Helpers/BlazorHelpers.cs](Helpers/BlazorHelpers.cs) for complete method list

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
| `WaitForConditionAsync()` | Wait for JS condition (string or options overloads) | 5s |
| `ScrollToLoadMoreAsync()` | Scroll infinite scroll until item count reached | 15s |
| `ScrollToEndOfContentAsync()` | Scroll infinite scroll until end-of-content marker | 30s |
| `ScrollIntoViewAsync()` | Scroll element into viewport via JS `scrollIntoView()` | - |
| `ScrollIntoViewIfNeededAsync()` | Scroll element into viewport (native Playwright) | - |

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

**Required structure for Web E2E test classes**:

1. **Extend `PlaywrightTestBase`**: Inherits page lifecycle, fixture injection, and cleanup
2. **Primary constructor**: Call `base(fixture)` with `PlaywrightCollectionFixture`
3. **No `[Collection]` attribute needed**: The fixture is shared via assembly fixture (xUnit v3)
4. **Use `Page` property**: Clean access to the current test's page
5. **Override `InitializeAsync`/`DisposeAsync`**: Only if extra setup/teardown is needed (call `base` first)

**Minimal test class**:

```csharp
public class MyFeatureTests : PlaywrightTestBase
{
    public MyFeatureTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Feature_Action_ExpectedResult()
    {
        await Page.GotoRelativeAsync("/my-page");
        // assertions...
    }
}
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

The `ClickAndWaitForScrollAsync()` helper uses browser's native `scrollend` event:

```csharp
// Click TOC link and wait for scroll to complete
await secondLink.ClickAndWaitForScrollAsync();
```

**How it works**:

1. Set up `scrollend` event listener (fires when scrolling stops)
2. Set up `scroll` event listener (detects if scroll started)
3. Execute action (click link)
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

### Solution 3: Native Scrolling for Programmatic Tests

**CRITICAL**: When testing scroll-spy behavior without user clicks, you **MUST** use native Playwright scrolling methods that fire scroll events. Programmatic JavaScript methods like `window.scrollTo()` do **NOT** fire scroll events in headless browsers.

**Why JavaScript scrolling fails**:

```csharp
// ❌ WRONG - Does NOT fire scroll events in headless Playwright
await Page.EvaluateAsync("window.scrollTo(0, 1000)");
// Result: Page scrolls but scroll-spy never updates (0 scroll events fired)
```

In headless browsers, `window.scrollTo()` executes synchronously and completes instantly without triggering scroll/scrollend events that the TOC scroll-spy depends on.

**Use native Playwright scrolling instead**:

```csharp
// ✅ CORRECT - Simulates real mouse wheel scrolling, fires scroll events
var secondHeadingY = await Page.EvaluateAsync<int>(
    "document.querySelectorAll('h2[id], h3[id]')[1].getBoundingClientRect().top + window.scrollY - 150"
);
await Page.Mouse.WheelAsync(0, secondHeadingY);

// Wait for scroll to reach target position
await Page.WaitForFunctionAsync(
    @"(targetY) => Math.abs(window.scrollY - targetY) < 100",
    secondHeadingY,
    new PageWaitForFunctionOptions { Timeout = 3000, PollingInterval = 50 });

// Then verify with polling
await Assertions.Expect(secondLink).ToHaveClassAsync(
    new Regex("active"),
    new LocatorAssertionsToHaveClassOptions { Timeout = 500 }
);
```

**How `Mouse.WheelAsync()` works**:

- Simulates actual user mouse wheel input
- Fires native browser scroll events (scroll, scrollend)
- Triggers TOC scroll-spy event handlers just like real user scrolling
- Takes `deltaX` and `deltaY` parameters (typically `deltaX=0`, `deltaY=scrollAmount`)

**Pattern for scrolling to specific elements**:

```csharp
// Calculate scroll position to element
var targetY = await Page.EvaluateAsync<int>(
    "element.getBoundingClientRect().top + window.scrollY - offsetFromTop"
);

// Scroll with native mouse wheel
await Page.Mouse.WheelAsync(0, targetY);

// Wait for scroll to reach target
await Page.WaitForFunctionAsync(
    @"(targetY) => Math.abs(window.scrollY - targetY) < 100",
    targetY,
    new PageWaitForFunctionOptions { Timeout = 3000, PollingInterval = 50 });
```

**Pattern for scrolling to bottom**:

```csharp
// Calculate total scrollable height
var scrollHeight = await Page.EvaluateAsync<int>(
    "document.documentElement.scrollHeight - window.innerHeight"
);

// Scroll to bottom with native mouse wheel
await Page.Mouse.WheelAsync(0, scrollHeight);

// Wait for scroll to reach bottom
await Page.WaitForFunctionAsync(
    @"() => Math.abs((window.innerHeight + window.scrollY) - document.documentElement.scrollHeight) < 50",
    new PageWaitForFunctionOptions { Timeout = 3000, PollingInterval = 50 });
```

**When to use native scrolling**:

- ✅ Testing scroll-spy updates when programmatically scrolling (without clicking)
- ✅ Testing "scroll to bottom" scenarios for last heading detection
- ✅ Any test that needs to trigger scroll event handlers
- ❌ **NOT** needed when using `ClickAndWaitForScrollAsync()` (already handles events)

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
// Calculate total scrollable height
var scrollHeight = await Page.EvaluateAsync<int>(
    "document.documentElement.scrollHeight - window.innerHeight"
);

// Scroll to bottom with native mouse wheel (fires scroll events)
await Page.Mouse.WheelAsync(0, scrollHeight);

// Wait for scroll to reach bottom
await Page.WaitForFunctionAsync(
    @"() => Math.abs((window.innerHeight + window.scrollY) - document.documentElement.scrollHeight) < 50",
    new PageWaitForFunctionOptions { Timeout = 3000, PollingInterval = 50 });

// Verify last heading is active
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
// Check class list manually for debugging
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
- General navigation tests: <200ms per test after removing NetworkIdle waits (see [Wait Pattern Best Practices](#wait-pattern-best-practices))

### Related Files

- [toc-scroll-spy.js](../../src/TechHub.Web/wwwroot/js/toc-scroll-spy.js) - Production TOC scroll-spy implementation
- [infinite-scroll.js](../../src/TechHub.Web/wwwroot/js/infinite-scroll.js) - IntersectionObserver-based infinite scroll with `__ioObserverReady` signal
- [BlazorHelpers.cs](Helpers/BlazorHelpers.cs) - Scroll synchronization helpers (`ClickAndWaitForScrollAsync`, `ScrollToLoadMoreAsync`, `ScrollToEndOfContentAsync`)
- [SidebarTocTests.cs](Web/SidebarTocTests.cs) - Reference implementation for TOC tests with native scrolling
- [IntersectionObserverProofTests.cs](Web/IntersectionObserverProofTests.cs) - Standalone proof that IO works in headless Chrome (11 tests)
- [InfiniteScrollTests.cs](Web/InfiniteScrollTests.cs) - Infinite scroll pagination tests
- [InfiniteScrollWithTagsTests.cs](Web/InfiniteScrollWithTagsTests.cs) - Infinite scroll with tag filtering
- [VSCodeUpdatesTests.cs](Web/VSCodeUpdatesTests.cs) - Additional TOC test examples
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
