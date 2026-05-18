# E2E Tests - Tech Hub

End-to-end tests using Playwright to verify complete user workflows against running servers (local or deployed).

Also contains database performance tests that run against local PostgreSQL only.

## Critical Rules

🚫 **NEVER use `Task.Delay()`, `Thread.Sleep()`, or `WaitForTimeoutAsync()` in E2E tests** — see [Wait Pattern Best Practices](#wait-pattern-best-practices).

## Base URL Configuration

The target server is configured via the `E2E_BASE_URL` environment variable:

- **Local development**: defaults to `https://localhost:5003` (start servers with `Run -WithoutTests`)
- **CI (PR preview)**: set to the PR preview URL — runs after PR environment deployment
- **Performance tests**: always run against local PostgreSQL, skipped if no database is available

## Understanding Timeout Failures

Timeouts are **NOT** performance issues. They mean Playwright couldn't find the expected element within the timeout period.

**Root causes**: Test bugs (wrong selector), broken pages, changed DOM structure, navigation to error pages, missing test data.

**Debug**: Start servers with `Run -WithoutTests`, then use Playwright MCP tools to navigate, snapshot, and compare actual DOM with expectations.

## Test Architecture

### Shared Page Pattern

All Web E2E test classes extend `PlaywrightTestBase`:

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

- `PlaywrightCollectionFixture` shared via **assembly fixture** (xUnit v3) — no `[Collection]` attribute needed
- Each test gets its own fresh `Page` and `Context` via `InitializeAsync()`/`DisposeAsync()` (return `ValueTask`)

### Browser Configuration

Centralized in [PlaywrightCollectionFixture.cs](PlaywrightCollectionFixture.cs): Headless Chrome, `--no-sandbox` + `--disable-setuid-sandbox` (devcontainer), `--disable-web-security` (CORS), 1920x1080 viewport, `Europe/Brussels` timezone, `IgnoreHTTPSErrors: true`.

### Network Throttling (CI Simulation)

Use the `-NetworkProfile` parameter on `Run` — it sets `E2E_NETWORK_THROTTLE` for the duration and clears it afterwards:

```powershell
Run -TestProject E2E -NetworkProfile wan
Run -TestProject E2E -NetworkProfile slow3g -TestName BackNavigation -RepeatTests 5
```

Profiles: `ci` (2x CPU throttle), `regular4g`, `fast3g`, `slow3g`, `wan` (150ms latency, simulates GitHub runner → remote Azure Container App). Uses CDP via `PlaywrightTestBase.InitializeAsync()`. See [Helpers/NetworkThrottling.cs](Helpers/NetworkThrottling.cs).

Alternatively, set the environment variable directly (it won't be auto-cleared):

```powershell
$env:E2E_NETWORK_THROTTLE = "wan"; Run -TestProject E2E
```

### E2E Lifecycle Counter (`window.__e2e`)

A monotonic lifecycle counter replaces independent `window.__*` flags. Every JS lifecycle hook calls `__e2eSignal(label)` which increments the counter. Test helpers capture counter before actions and wait for it to change.

**Signal sources**: `blazor-web-ready`, `blazor-server-ready`, `enhanced-nav` (from `TechHub.Web.lib.module.js`), `scripts-loading`/`scripts-ready`/`mermaid-rendered` (from `page-scripts.js`), `scroll-listener:{triggerId}`/`scroll-disposed:{triggerId}`/`toc-initialized`/`toc-active-updated` (from `scroll-manager.js`), `scroll-end` (browser `scrollend` event).

Signal history: Ring buffer of 20 entries. Timeout constants: `E2ETimeout` (scales by network profile: 10s local, 30s regular4g/wan/ci, 45s fast3g, 60s slow3g; 60s in CI without a profile), `E2EPollingInterval` (100ms) — centralized in `BlazorHelpers.cs`.

**Key public helpers**:

| Method | Purpose |
|---|---|
| `GotoRelativeAsync()` | Navigate + wait for page ready |
| `ClickAndExpectAsync(assert)` | **The only click helper.** Waits for Blazor interactivity, clicks **once**, then asserts. Do NOT pass explicit `Timeout` in the assertion lambda — rely on the global `E2ETimeout` set via `Assertions.SetDefaultExpectTimeout`. If the URL changes after the assertion, automatically waits for Blazor to be ready on the new page. |
| `ClickAndWaitForScrollAsync()` | Click + wait for `scroll-end` signal |
| `ScrollToAndWaitForTocUpdateAsync()` | Scroll + wait for `toc-active-updated` signal |
| `WaitForTocInitializedAsync()` | Wait for `toc-initialized` signal |
| `ScrollToPositionAsync(y)` | Scroll page to Y position (retries until page is tall enough) |
| `FillBlazorInputAsync(query)` | Fill input + wait for URL query param update |
| `WaitForConditionAsync(js, onTimeout?)` | Wait for JS condition (uses `E2ETimeout`; scales 10–60 s by network profile); optional `onTimeout` JS expression evaluated on timeout and appended to error message for diagnostics |
| `WaitForBlazorReadyAsync()` | Wait for Blazor ready + `__scriptsReady` (which already includes Mermaid via `allComponentsReady()`) |

## Wait Pattern Best Practices

### Golden Rule: Never Use Arbitrary Delays

| Waiting for... | ✅ Correct pattern |
|---|---|
| Element to appear | `Assertions.Expect(el).ToBeVisibleAsync()` |
| CSS class applied | `Assertions.Expect(el).ToHaveClassAsync(new Regex("expanded"))` |
| Console errors after load | `GotoRelativeAsync` already handles this — no extra wait |
| Blazor re-render | `Page.WaitForBlazorReadyAsync()` |
| DOM condition | `Page.WaitForConditionAsync("() => document.querySelectorAll('.card').length > 0")` |
| JS lib init | `Page.WaitForConditionAsync("() => document.querySelector('pre code.hljs') !== null")` |
| Scroll finish | `WaitForConditionAsync` polling `window.scrollY` |
| Expand/collapse | `Assertions.Expect(content).ToHaveClassAsync(new Regex("expanded"))` |
| No navigation | `WaitForBlazorReadyAsync()` then assert URL unchanged |
| Load more batch | `loadMoreBtn.ClickAndExpectAsync(async () => count.Should().BeGreaterThan(prev))` |
| End of content | `Assertions.Expect(Page.Locator(".end-of-content")).ToBeVisibleAsync()` |
| Focus after nav reset | `WaitForConditionAsync("() => document.activeElement && document.activeElement !== document.body")` |
| Search input fill | `searchInput.FillBlazorInputAsync("query")` |

### Key Patterns

**Console Error Tests**: `GotoRelativeAsync` already waits for Blazor ready + scripts loaded. No extra wait needed. Do NOT use `NetworkIdle` (Blazor's SignalR WebSocket prevents it from settling).

**Expand/Collapse**: Use `ClickAndExpectAsync` — it waits for Blazor interactivity before clicking and uses the full `E2ETimeout` for the assertion:

```csharp
await header.ClickAndExpectAsync(async () =>
    await Assertions.Expect(content).ToHaveClassAsync(new Regex("expanded")));
```

**Load More Button**: Click `.load-more-btn` using `ClickAndExpectAsync` and assert card count increases or end-of-content appears. No JS scroll events needed — it’s a pure Blazor button click.

**Lazy-Loaded Images**: `loading="lazy"` doesn't work reliably in headless Chrome. Fix in source: hero/above-fold images must use eager loading (omit `loading` attribute). Always include `width`/`height`.

**Focus Detection**: After Blazor enhanced navigation, `activeElement` resets to `<body>`. Use `WaitForConditionAsync` + `Page.EvaluateAsync` instead of `:focus` locator.

### Prefer Playwright Expect Assertions

```csharp
// ✅ BEST - Auto-retrying with smart polling
await Assertions.Expect(element).ToBeVisibleAsync();
await Assertions.Expect(element).ToContainTextAsync("expected text");
```

## TOC Scroll Synchronization

TOC scroll-spy uses `requestAnimationFrame` — race condition if checking active class immediately after scrolling.

**Solution 1 (Recommended)**: Playwright polling:

```csharp
await Assertions.Expect(link).ToHaveClassAsync(new Regex("active"), new() { Timeout = 500 });
```

**Solution 2**: Event-based — `ClickAndWaitForScrollAsync()` waits for `scroll-end` signal via lifecycle counter.

**Solution 3**: Programmatic scrolling — `window.scrollTo({ behavior: 'instant' })` fires scroll events in Chrome. Use `ScrollToAndWaitForTocUpdateAsync(targetY)` helper. `Mouse.WheelAsync` can hang under parallel Chrome load.

### Assertion Style

```csharp
// ✅ FluentAssertions with descriptive reasons
Page.Url.Should().EndWith("/github-copilot/all",
    "navigating to a section without collection should default to /section/all");
```

### Test Naming Convention

Format: `ComponentOrFeature_Action_ExpectedResult` (e.g., `NavigateToSection_DefaultsToAllCollection`)
