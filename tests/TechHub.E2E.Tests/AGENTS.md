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
│   │   ├── TabHighlightingTests.cs     ← Focus visibility (WCAG AA accessibility, keyboard-only outlines)
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
├── Api/                                 ← Direct API testing (no Playwright)
│   ├── ApiCollectionFixture.cs         ← Shared WebApplicationFactory for API tests
│   └── ContentEndpointsE2ETests.cs     ← Content API endpoints (4 tests)
├── Helpers/
│   ├── BlazorHelpers.cs                ← Blazor-specific wait patterns
│   └── NetworkThrottling.cs            ← CDP network/CPU throttling for CI simulation
├── PlaywrightCollectionFixture.cs      ← Shared browser (assembly fixture)
├── PlaywrightTestBase.cs               ← Abstract base class for Web E2E tests
└── testconfig.json                     ← Parallel execution settings (xUnit v3)
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

Note: Test count includes common component tests, page-specific tests, feature tests (infinite scroll, tag filtering, date range slider, dynamic tag counts, keyboard navigation), accessibility tests, and API E2E tests.

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

### Network Throttling (CI Simulation)

E2E tests can fail on CI runners due to constrained CPU and network resources. To reproduce these conditions locally, set the `E2E_NETWORK_THROTTLE` environment variable before running tests:

```powershell
# Simulate CI runner conditions (10 Mbps network + 2x CPU throttle)
$env:E2E_NETWORK_THROTTLE = "ci"
Run -TestProject E2E

# Simulate slow 3G network
$env:E2E_NETWORK_THROTTLE = "slow3g"
Run -TestProject E2E

# Disable throttling (default)
$env:E2E_NETWORK_THROTTLE = ""
```

**Available profiles**:

| Profile | Download | Upload | Latency | CPU Throttle |
|---------|----------|--------|---------|-------------|
| `ci` | — | — | — | 2x CPU slowdown |
| `regular4g` | 4 Mbps | 3 Mbps | 20ms | None |
| `fast3g` | 562 Kbps | 562 Kbps | 150ms | None |
| `slow3g` | 400 Kbps | 400 Kbps | 400ms | None |

**How it works**: Uses CDP (Chrome DevTools Protocol) `Network.emulateNetworkConditions` and `Emulation.setCPUThrottlingRate` via Playwright's `NewCDPSessionAsync`. Applied automatically in `PlaywrightTestBase.InitializeAsync()` per test page. Chromium-only (which is our default browser).

**See**: [Helpers/NetworkThrottling.cs](Helpers/NetworkThrottling.cs) for implementation

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

**2. Parallel Test Execution** (scales to CPU):

- Test collections run in parallel
- Configured via two `testconfig` files:
  - `testconfig.json` (default): `maxParallelThreads: "1x"` — conservative for CI runners (2 vCPUs)
  - `testconfig.localhost.json`: `maxParallelThreads: "2x"` — aggressive for local dev machines
  - The `Run` command automatically uses `testconfig.localhost.json` via `--config-file`
  - CI (`dotnet test`) uses the default `testconfig.json` with no extra flags
- Each collection gets isolated resources (browser contexts, HTTP clients)

**3. Timeout Constants** (single generous timeout for all environments):

- `E2ETimeout` (60s) — safety-net timeout for all `WaitForConditionAsync` / `WaitForFunctionAsync` calls
- `E2EPollingInterval` (100ms) — polling interval for condition checks
- No CI multiplier — the lifecycle counter makes tests event-driven, so timeouts rarely fire
- Managed centrally as `const` fields in `BlazorHelpers.cs`
- **No `timeoutMs` parameters on helper methods** — timeouts are fully centralized
- Prefer `WaitForConditionAsync` (wraps `WaitForFunctionAsync` with default timeout/polling) over raw `WaitForFunctionAsync`
- Most operations complete in &lt;500ms; timeouts only matter under peak load

### E2E Lifecycle Counter (`window.__e2e`)

**The Challenge**: Blazor Server uses SignalR for interactivity. After the initial HTML loads, Blazor needs to initialize its runtime, establish the SignalR circuit, and attach event handlers. Tests that interact too early will fail.

**The Solution**: A single monotonic lifecycle counter on `window.__e2e` replaces the previous 8+ independent `window.__*` flags. Every JS lifecycle hook calls `__e2eSignal(label)` which increments the counter. Test helpers capture the counter before an action and wait for it to change.

**How it works**:

1. [App.razor](../../src/TechHub.Web/Components/App.razor) injects an inline `<script>` (Development only) that initializes `window.__e2e = { counter, label, history[] }` and defines `__e2eSignal(label)`
2. [TechHub.Web.lib.module.js](../../src/TechHub.Web/wwwroot/TechHub.Web.lib.module.js) fires signals: `blazor-web-ready`, `blazor-server-ready`, `blazor-wasm-ready`, `enhanced-nav`
3. [page-scripts.js](../../src/TechHub.Web/wwwroot/js/page-scripts.js) fires: `scripts-loading`, `scripts-ready`, `mermaid-rendered`
4. [infinite-scroll.js](../../src/TechHub.Web/wwwroot/js/infinite-scroll.js) fires: `scroll-listener:{triggerId}`, `scroll-disposed:{triggerId}`
5. [toc-scroll-spy.js](../../src/TechHub.Web/wwwroot/js/toc-scroll-spy.js) fires: `toc-initialized`, `toc-active-updated`
6. The `scrollend` browser event fires: `scroll-end`

**Signal history**: A ring buffer of 20 entries allows helpers to check for specific signals.

**Timeout constants**: `E2ETimeout` (60s) and `E2EPollingInterval` (100ms) replace the old `IsCI`/`CiMultiplier` system. A single generous timeout works on both local and CI environments.

**Key internal helpers** (tests never call these directly):

- `GetE2ECounterAsync()` — Captures the current counter value
- `WaitForE2ECounterChangeAsync()` — Waits for counter to exceed a captured value
- `WaitForE2ESignalAsync()` — Waits for a specific signal label in the history
- `WaitForPageReadyAsync()` — Waits for counter to advance and scripts to finish loading

**Key public helpers that use the counter**:

- `GotoRelativeAsync()` / `GotoAndWaitForBlazorAsync()` — Navigates and waits for page ready via `WaitForPageReadyAsync(0)`
- `ClickBlazorElementAsync()` — Captures counter before click, waits for URL change + `WaitForBlazorReadyAsync`
- `ClickAndWaitForScrollAsync()` — Captures counter before click, waits for `scroll-end` signal
- `ScrollToAndWaitForTocUpdateAsync()` — Captures counter, scrolls, waits for `toc-active-updated` signal
- `WaitForTocInitializedAsync()` — Captures counter, waits for `toc-initialized` signal

**Legacy flags**: `WaitForBlazorReadyAsync()` is retained for backward compatibility. It checks the old `__blazorServerReady`/`__blazorWasmReady`/`__blazorWebReady` flags plus script loading state plus Mermaid rendering.

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
| Console errors after load | `WaitForTimeoutAsync(500)` | `GotoRelativeAsync` already waits for Blazor ready + scripts not loading — no extra wait needed |
| Blazor re-render complete | `WaitForTimeoutAsync(500)` | `Page.WaitForBlazorReadyAsync()` |
| DOM condition (card count) | `WaitForTimeoutAsync(500)` | `Page.WaitForFunctionAsync("() => document.querySelectorAll('.card').length > 0")` |
| JS lib init (highlight.js) | `WaitForTimeoutAsync(500)` | `Page.WaitForFunctionAsync("() => document.querySelector('pre code.hljs') !== null")` |
| Scroll to finish | `WaitForTimeoutAsync(200)` | `Page.WaitForFunctionAsync` polling scroll position |
| Expand/collapse toggle | `WaitForTimeoutAsync(1000)` | `Assertions.Expect(content).ToHaveClassAsync(new Regex("expanded"))` |
| No navigation occurred | `WaitForTimeoutAsync(200)` | `Page.WaitForBlazorReadyAsync()` then assert URL unchanged |
| Browser paint settle | `Task.Delay(100)` | `GotoRelativeAsync` / `WaitForBlazorReadyAsync()` (handles Mermaid rendering internally) |
| Infinite scroll next batch | `Mouse.WheelAsync()` or `loading="eager"` | `Page.ScrollToLoadMoreAsync(expectedCount)` |
| Infinite scroll end | Manual wheel scrolling loop | `Page.ScrollToEndOfContentAsync()` |
| Lazy-loaded element visible | `loading="eager"` attribute hack | Fix in source: remove `loading="lazy"` for above-fold/hero images (see Pattern 10) |
| Focus after navigation reset | `Locator(":focus").EvaluateAsync()` | `WaitForConditionAsync` + `Page.EvaluateAsync()` |
| Custom timeout value | `Timeout = 5000` (hardcoded) | `WaitForConditionAsync` (uses `E2ETimeout` 60s default) |
| Search input fill + URL update | `FillAsync` + `WaitForConditionAsync` | `searchInput.FillBlazorInputAsync("query")` |

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

`GotoRelativeAsync` already waits for Blazor readiness and ensures page scripts are not actively loading. No additional wait is needed — by the time it returns, any console errors from script loading have already been emitted.

```csharp
// ❌ WRONG - arbitrary delay
await Page.GotoRelativeAsync(url);
await Page.WaitForTimeoutAsync(500);

// ❌ WRONG - NetworkIdle is unreliable with Blazor Server's always-active WebSocket
await Page.GotoRelativeAsync(url);
await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

// ✅ CORRECT - GotoRelativeAsync already waits for Blazor ready + scripts not loading
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
await Page.WaitForConditionAsync(
    "() => document.querySelector('pre code.hljs') !== null");
```

#### Pattern 4: Scroll Position Stability

After scrolling with `Mouse.WheelAsync()`, wait for scroll position to reach the target:

```csharp
// ❌ WRONG
await Page.Mouse.WheelAsync(0, targetY);
await Page.WaitForTimeoutAsync(200);

// ✅ CORRECT - Poll for scroll position
await Page.Mouse.WheelAsync(0, targetY);
await Page.WaitForConditionAsync(
    @"(targetY) => Math.abs(window.scrollY - targetY) < 100",
    targetY);
```

For scrolling to the bottom of the page:

```csharp
await Page.Mouse.WheelAsync(0, scrollHeight);
await Page.WaitForConditionAsync(
    @"() => Math.abs((window.innerHeight + window.scrollY) - document.documentElement.scrollHeight) < 50");
```

#### Pattern 5: Browser Paint/Layout Settle

When the browser needs to complete a paint cycle after DOM mutations (e.g., after mermaid diagram rendering), rely on the existing readiness infrastructure rather than manual frame waits:

```csharp
// ❌ WRONG - arbitrary delay
await Task.Delay(100);

// ❌ WRONG - double requestAnimationFrame is a hack, not used anywhere in this codebase
await page.WaitForFunctionAsync(
    "() => new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(() => resolve(true))))");

// ✅ CORRECT - GotoRelativeAsync / WaitForBlazorReadyAsync already checks for Mermaid SVG rendering
await Page.GotoRelativeAsync(url);
// WaitForBlazorReadyAsync (called internally) waits for:
// 1. Blazor runtime ready
// 2. Page scripts finished loading
// 3. Mermaid diagrams rendered (checks SVG count matches <pre class="mermaid"> count)

// ✅ CORRECT - For post-navigation checks, use Playwright auto-retrying assertions
await Assertions.Expect(Page.Locator("svg[id^='mermaid-']").First).ToBeVisibleAsync();
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
await Page.WaitForConditionAsync(
    "() => document.querySelectorAll('.card').length > 0");
await Page.WaitForBlazorReadyAsync();
```

#### Pattern 8: Infinite Scroll (scroll event based)

Infinite scroll uses standard scroll events with `getBoundingClientRect()` — the same pattern as the TOC scroll-spy. When the user scrolls near the `#scroll-trigger` element (within 300px), the JS module calls Blazor's `LoadNextBatch`.

The **timing race** to watch for: Blazor re-attaches the scroll listener in `OnAfterRenderAsync` after each batch load, but tests may scroll before the new listener is attached. The `__scrollListenerReady[triggerId]` signal solves this (scoped per trigger element to avoid interference between concurrent listeners).

**⚠️ CRITICAL: `scrollTo()` does NOT fire scroll events in headless Chrome**

Unlike the TOC tests which use `Mouse.WheelAsync()`, infinite scroll helpers use `window.scrollTo()` for precise positioning. However, `window.scrollTo()` does **NOT** fire `scroll` events in headless Chrome/Playwright. The scroll helpers work around this by explicitly dispatching a scroll event after each `scrollTo()` call:

```javascript
// Inside BlazorHelpers scroll helpers:
window.scrollTo(0, targetY);
window.dispatchEvent(new Event('scroll'));  // ← REQUIRED for headless Chrome
```

This is already handled in `ScrollToLoadMoreAsync` and `ScrollToEndOfContentAsync`. If you ever write custom scroll logic for infinite scroll tests, you **MUST** include `window.dispatchEvent(new Event('scroll'))` after any `window.scrollTo()` call.

```csharp
// ❌ WRONG - scroll before listener is attached
await Page.Mouse.WheelAsync(0, scrollHeight);

// ✅ CORRECT - use ScrollToLoadMoreAsync (waits for __scrollListenerReady['scroll-trigger'])
await Page.ScrollToLoadMoreAsync(expectedItemCount: 40);

// ✅ CORRECT - use ScrollToEndOfContentAsync for end-of-collection tests
await Page.ScrollToEndOfContentAsync();
```

**How the scroll helpers work** (see `BlazorHelpers.cs`):

1. Wait for `window.__scrollListenerReady?.['scroll-trigger'] === true` (scroll listener attached)
2. Use `WaitForFunctionAsync` with `window.scrollTo()` + `window.dispatchEvent(new Event('scroll'))` on each poll iteration
3. The dispatched scroll event triggers the infinite-scroll JS handler
4. JS checks trigger position via `getBoundingClientRect()` → calls `LoadNextBatch`
5. Wait for new items to appear in the DOM
6. Loop until target count reached or end-of-content marker appears

**Why `dispatchEvent` instead of `Mouse.WheelAsync()`?** The infinite scroll helpers use `scrollTo()` for deterministic positioning (scroll to exact bottom), whereas TOC tests use `Mouse.WheelAsync()` because they need delta-based scrolling to specific headings. Both approaches work, but `scrollTo()` requires the explicit `dispatchEvent` workaround in headless browsers.

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

#### Pattern 10: Lazy-Loaded Images in Headless Browsers

`loading="lazy"` on `<img>` tags does **NOT** work reliably in headless Chrome when elements are scrolled into view programmatically. The browser's lazy loading heuristic requires real user scroll gestures and sufficient element dimensions to trigger.

**Root cause**: In headless mode, lazy loading depends on the IntersectionObserver internally, which may not fire for programmatic scrolls (even with explicit `width`/`height` attributes on the image).

**The fix is always in the source code, not in tests**:

```html
<!-- ❌ WRONG - Hero/above-fold images should never be lazy-loaded -->
<img src="image.jpg" loading="lazy" />

<!-- ✅ CORRECT - Eager load (default) + explicit dimensions for layout stability -->
<img src="image.jpg" width="1216" height="1500" />
```

**Rules**:

- **Hero images** and **above-the-fold content**: Always use eager loading (omit `loading` attribute entirely). These are LCP (Largest Contentful Paint) candidates and should load immediately for performance.
- **Below-the-fold images**: `loading="lazy"` is fine for production users, but be aware tests may need `ScrollIntoViewAsync()` to trigger loading.
- **Always include `width` and `height` attributes**: Prevents layout shift (CLS) and gives the browser dimensions for lazy loading decisions.

**Test pattern for eagerly-loaded images**:

```csharp
// Wait for image to fully load (eager images load with the page)
await Page.WaitForConditionAsync(
    "() => { const img = document.querySelector('.hero img');" +
    "return img && img.complete === true && img.naturalHeight > 0; }");
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
| `NewPageWithDefaultsAsync()` | Create page with optimized timeouts | 60s |
| `GotoRelativeAsync()` | Navigate to relative URL + wait for page ready | 60s |
| `ClickAndNavigateAsync()` | High-level click + navigate + verify pattern | 60s |
| `WaitForBlazorUrlContainsAsync()` | Wait for SPA navigation | 60s |
| `WaitForBlazorRenderAsync()` | Wait for element to appear | 60s |
| `ClickBlazorElementAsync()` | Click with URL change + Blazor ready wait | 60s |
| `ClickAndWaitForScrollAsync()` | Click + wait for `scroll-end` signal via counter | 60s |
| `ScrollToAndWaitForTocUpdateAsync()` | Scroll + wait for `toc-active-updated` signal | 60s |
| `WaitForTocInitializedAsync()` | Wait for `toc-initialized` signal via counter | 60s |
| `AssertElementVisibleAsync()` | Assert element is visible | 60s |
| `AssertUrlEndsWithAsync()` | Assert URL ends with segment | 60s |
| `WaitForConditionAsync()` | Wait for JS condition (string or options overloads) | 60s |
| `ScrollToLoadMoreAsync()` | Scroll infinite scroll until item count reached | 60s |
| `ScrollToEndOfContentAsync()` | Scroll infinite scroll until end-of-content marker | 60s |
| `ScrollIntoViewAsync()` | Scroll element into viewport via JS `scrollIntoView()` | - |
| `ScrollIntoViewIfNeededAsync()` | Scroll element into viewport (native Playwright) | - |
| `FillBlazorInputAsync()` | Fill input + wait for URL query param, with input event retry | 60s |

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

The `ClickAndWaitForScrollAsync()` helper uses the `__e2e` lifecycle counter to detect scroll completion:

```csharp
// Click TOC link and wait for scroll to complete
await secondLink.ClickAndWaitForScrollAsync();
```

**How it works**:

1. Captures the current `__e2e` counter value
2. Clicks the element (triggers anchor navigation/scrolling)
3. Waits for the `scroll-end` signal to appear in the `__e2e` history ring buffer
4. Returns when the signal is detected or max timeout is reached

**Why this works**: The `scrollend` browser event fires when scrolling stops. [App.razor](../../src/TechHub.Web/Components/App.razor) listens for `scrollend` and calls `__e2eSignal('scroll-end')`, which increments the lifecycle counter. The helper waits for this specific signal, making it event-driven rather than time-based.

The TOC scroll-spy also uses the same scroll events (with RAF throttling) to update the active heading. By the time `scroll-end` fires, the scroll-spy's RAF callback has typically already executed.

**IMPORTANT**: Even after `ClickAndWaitForScrollAsync()`, still use Playwright polling for assertions (Solution 1). The helper ensures scrolling completes, but the final class update timing can still vary.

### Solution 3: Programmatic Scrolling for Scroll-Spy Tests

There are two approaches for programmatic scrolling. Both work, with different trade-offs:

**Approach A: `window.scrollTo({ behavior: 'instant' })` (preferred for TOC tests)**

`window.scrollTo` with `behavior: 'instant'` **does** fire a `scroll` event in modern Chrome (including headless). This is what the actual TOC tests use:

```csharp
// ✅ CORRECT - scrollTo with behavior: 'instant' fires scroll events
var secondHeadingY = await Page.EvaluateAsync<int>(
    "document.querySelectorAll('h2[id], h3[id]')[1].getBoundingClientRect().top + window.scrollY - 150"
);
await Page.EvaluateAsync($"window.scrollTo({{ top: {secondHeadingY}, behavior: 'instant' }})");

// Wait for scroll position to stabilize
await Page.WaitForConditionAsync(
    @$"() => Math.abs(window.scrollY - {secondHeadingY}) < 100");

// Then verify with Playwright auto-retrying assertion
await activeTocLink.AssertHasClassAsync(
    new System.Text.RegularExpressions.Regex(".*active.*"));
```

Or use the convenience helper:

```csharp
// ✅ CORRECT - Helper captures counter, scrolls, waits for toc-active-updated signal
await Page.ScrollToAndWaitForTocUpdateAsync(targetY);
```

**Approach B: `Mouse.WheelAsync()` (simulates real user input)**

```csharp
// ✅ ALSO CORRECT - Simulates actual user mouse wheel input
await Page.Mouse.WheelAsync(0, secondHeadingY);

// Wait for scroll to reach target position
await Page.WaitForConditionAsync(
    @"(targetY) => Math.abs(window.scrollY - targetY) < 100",
    secondHeadingY);
```

**⚠️ Caveat**: `Mouse.WheelAsync` can hang under parallel Chrome load (multiple test collections running simultaneously). The actual TOC tests prefer `window.scrollTo` for this reason.

**When `window.scrollTo` does NOT fire scroll events**:

When called repeatedly to the **same position** inside a polling loop (e.g., in infinite scroll helpers), `scrollTo` won't fire a new `scroll` event because the scroll position doesn't change. In this case, you need `window.dispatchEvent(new Event('scroll'))`. This is already handled by `ScrollToLoadMoreAsync` and `ScrollToEndOfContentAsync` — see [Pattern 8](#pattern-8-infinite-scroll-scroll-event-based).

**Pattern for scrolling to bottom**:

```csharp
var scrollHeight = await Page.EvaluateAsync<int>(
    "document.documentElement.scrollHeight - window.innerHeight");
await Page.EvaluateAsync($"window.scrollTo({{ top: {scrollHeight}, behavior: 'instant' }})");
await Page.WaitForConditionAsync(
    @"() => Math.abs((window.innerHeight + window.scrollY) - document.documentElement.scrollHeight) < 50");
```

**When to use which**:

- ✅ `window.scrollTo({ behavior: 'instant' })` — TOC scroll-spy tests, precise positioning (preferred)
- ✅ `Mouse.WheelAsync()` — When you specifically need to simulate user wheel input
- ✅ `ClickAndWaitForScrollAsync()` — TOC link clicks (handles everything automatically)
- ✅ `ScrollToAndWaitForTocUpdateAsync()` — Programmatic scroll + wait for TOC update signal

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
await Page.WaitForConditionAsync(
    @"() => Math.abs((window.innerHeight + window.scrollY) - document.documentElement.scrollHeight) < 50");

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
- [infinite-scroll.js](../../src/TechHub.Web/wwwroot/js/infinite-scroll.js) - Scroll-event-based infinite scroll with `__scrollListenerReady[triggerId]` signal
- [BlazorHelpers.cs](Helpers/BlazorHelpers.cs) - Scroll synchronization helpers (`ClickAndWaitForScrollAsync`, `ScrollToLoadMoreAsync`, `ScrollToEndOfContentAsync`)
- [SidebarTocTests.cs](Web/SidebarTocTests.cs) - Reference implementation for TOC tests with native scrolling
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
