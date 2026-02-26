using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace TechHub.E2E.Tests.Helpers;

/// <summary>
/// Helper methods for working with Blazor Server in E2E tests.
///
/// PHILOSOPHY: Use Playwright's built-in auto-waiting and retry mechanisms wherever possible.
/// - Expect assertions auto-retry until condition is met or timeout
/// - Locator actions auto-wait for actionability (visible, stable, enabled, receives events)
/// - Avoid Task.Delay - use proper condition detection instead
///
/// KEY INSIGHT: Playwright's auto-waiting handles 90% of timing issues automatically.
/// The main Blazor-specific challenge is detecting when enhanced navigation completes,
/// since it doesn't trigger traditional navigation events.
///
/// TIMEOUT MANAGEMENT: All timeouts and polling intervals are centralized in constants
/// (DefaultTimeout, IncreasedTimeout, DefaultPollingInterval) to avoid relying on
/// Playwright's defaults which could change. Adjust these constants to tune test responsiveness.
///
/// USAGE EXAMPLES - Common Test Patterns:
///
/// 1. Click collection button and verify state:
///    await page.ClickAndNavigateAsync(".sub-nav a", text: "Videos",
///        expectedUrlSegment: "/videos", waitForActiveState: "Videos");
///
/// 2. Click section card on homepage:
///    await page.ClickAndNavigateAsync(".section-card-container a.section-card[href*='github-copilot']",
///        expectedUrlSegment: "/github-copilot");
///
/// 3. Click content card to detail page:
///    await page.ClickAndNavigateAsync(".card", nth: 0,
///        expectedUrlSegment: "/roundups/");
///
/// 4. Click navigation link:
///    await page.ClickAndNavigateAsync("a", text: "About", expectedUrlSegment: "/about",
///        customWait: async p => await Assertions.Expect(p.Locator("main")).ToBeVisibleAsync());
///
/// 5. Use Expect assertions instead of TextContentAsync + Should.Contain:
///    // DON'T: var text = await element.TextContentAsync(); text.Should().Contain("foo");
///    // DO: await Assertions.Expect(element).ToContainTextAsync("foo");
/// </summary>
public static class BlazorHelpers
{
    // ============================================================================
    // CONFIGURATION - Centralized timeout management
    // ============================================================================

    /// <summary>
    /// Default timeout for all operations (element waits, clicks, assertions, navigation).
    /// Set to 10s to handle concurrent test load during full Run and CI runners
    /// (unit + integration tests run before E2E, creating sustained CPU/IO pressure
    /// that slows Playwright-Chromium IPC). Most operations complete in &lt;500ms;
    /// the timeout only matters under peak load.
    /// </summary>
    internal const int DefaultTimeout = 10_000;

    /// <summary>
    /// Increased timeout for slow operations: initial page loads (SSR + database queries),
    /// heavy collection pages, and operations that chain multiple async steps
    /// (debounce + SignalR round-trip + Blazor re-render).
    /// Under full E2E suite load (~200 parallel tests), PostgreSQL can respond slowly
    /// for complex URLs with search/tag/date filters.
    /// Typical response: &lt;2s idle, 3-10s under CI load.
    /// </summary>
    internal const int IncreasedTimeout = 15_000;

    /// <summary>
    /// Default polling interval for WaitForFunctionAsync operations.
    /// Controls how frequently Playwright re-evaluates JavaScript conditions.
    /// 100ms provides a good balance between responsiveness and CPU usage.
    /// </summary>
    internal const int DefaultPollingInterval = 100;

    /// <summary>
    /// Timeout for browser launch operations.
    /// Separate from test timeouts as this is infrastructure initialization.
    /// </summary>
    internal const int BrowserLaunchTimeout = 30_000;

    /// <summary>Base URL for the Web frontend</summary>
    public const string BaseUrl = "https://localhost:5003";

    // ============================================================================
    // SAFE WaitForFunctionAsync WRAPPERS
    // ============================================================================
    // Playwright .NET has a SINGLE overload:
    //   WaitForFunctionAsync(string expression, object? arg = null, PageWaitForFunctionOptions? options = null)
    //
    // BUG TRAP: If you call WaitForFunctionAsync("...", new PageWaitForFunctionOptions { Timeout = BlazorHelpers.DefaultTimeout }),
    // the options object silently binds to the `arg` parameter (type object?), NOT `options`.
    // This causes all explicit timeouts to be ignored, falling back to the default timeout.
    //
    // These extension methods provide unambiguous 2-arg signatures that prevent this mistake.
    // USE THESE instead of calling WaitForFunctionAsync directly.
    // ============================================================================

    /// <summary>
    /// Waits for a JavaScript condition to become truthy. Safe wrapper that prevents
    /// the Playwright .NET arg-binding bug.
    /// Use this for expressions that take NO JavaScript arguments.
    /// </summary>
    /// <param name="page">The page to evaluate on</param>
    /// <param name="expression">JavaScript expression that returns a truthy/falsy value, e.g. "() => document.querySelector('.card') !== null"</param>
    /// <param name="options">Wait options (timeout, polling interval)</param>
    public static Task<IJSHandle> WaitForConditionAsync(
        this IPage page,
        string expression,
        PageWaitForFunctionOptions options) =>
        page.WaitForFunctionAsync(expression, null, options);

    /// <summary>
    /// Waits for a JavaScript condition to become truthy with default timeout.
    /// Convenience overload that uses DefaultTimeout.
    /// Use this for expressions that take NO JavaScript arguments.
    /// </summary>
    /// <param name="page">The page to evaluate on</param>
    /// <param name="expression">JavaScript expression that returns a truthy/falsy value, e.g. "() => document.querySelector('.card') !== null"</param>
    public static Task<IJSHandle> WaitForConditionAsync(
        this IPage page,
        string expression) =>
        page.WaitForFunctionAsync(expression, null, new PageWaitForFunctionOptions { Timeout = DefaultTimeout, PollingInterval = DefaultPollingInterval });



    /// <summary>
    /// Waits for a parameterized JavaScript condition to become truthy.
    /// Use this for expressions that accept a JavaScript argument, e.g.
    /// "(expectedCount) => document.querySelectorAll('.card').length >= expectedCount".
    /// The <paramref name="arg"/> is passed to the JavaScript function as its first parameter.
    /// </summary>
    /// <param name="page">The page to evaluate on</param>
    /// <param name="expression">JavaScript expression accepting one arg</param>
    /// <param name="arg">Value passed to the JavaScript function</param>
    /// <param name="options">Wait options (timeout, polling interval)</param>
    public static Task<IJSHandle> WaitForConditionAsync(
        this IPage page,
        string expression,
        object arg,
        PageWaitForFunctionOptions options) =>
        page.WaitForFunctionAsync(expression, arg, options);

    /// <summary>
    /// Waits for a parameterized JavaScript condition to become truthy with default timeout.
    /// Convenience overload that uses DefaultTimeout.
    /// </summary>
    /// <param name="page">The page to evaluate on</param>
    /// <param name="expression">JavaScript expression accepting one arg</param>
    /// <param name="arg">Value passed to the JavaScript function</param>
    public static Task<IJSHandle> WaitForConditionAsync(
        this IPage page,
        string expression,
        object arg) =>
        page.WaitForFunctionAsync(expression, arg, new PageWaitForFunctionOptions { Timeout = DefaultTimeout, PollingInterval = DefaultPollingInterval });

    // ============================================================================
    // INFINITE SCROLL - Reliable scroll-to-load pattern
    // ============================================================================

    /// <summary>
    /// Scrolls the page to trigger scroll-event-based infinite scroll and waits
    /// for the expected number of items to load.
    ///
    /// Uses <c>window.scrollTo</c> to scroll to the bottom of the page on each poll
    /// iteration. After scrolling, a synthetic <c>scroll</c> event is dispatched because
    /// headless Chrome does not fire scroll events from programmatic <c>scrollTo</c> calls.
    /// The infinite-scroll.js handler listens for these events and uses
    /// <c>getBoundingClientRect</c> to check whether the trigger element is near the
    /// viewport, invoking <c>LoadNextBatch</c> when it is.
    /// </summary>
    /// <param name="page">The page to scroll</param>
    /// <param name="expectedItemCount">Minimum number of items expected after scroll</param>
    /// <param name="timeoutMs">Total timeout for the operation</param>
    /// <param name="itemSelector">CSS selector for items to count (default: ".card")</param>
    public static async Task ScrollToLoadMoreAsync(
        this IPage page,
        int expectedItemCount,
        int timeoutMs = DefaultTimeout,
        string itemSelector = ".card",
        string triggerId = "scroll-trigger")
    {
        // Wait for the scroll listener to be attached before scrolling.
        // Readiness is scoped by triggerId so multiple concurrent listeners don't interfere.
        await page.WaitForConditionAsync(
            $"() => window.__scrollListenerReady?.['{triggerId}'] === true",
            new PageWaitForFunctionOptions { Timeout = timeoutMs, PollingInterval = DefaultPollingInterval });

        // On each poll: scroll to bottom and dispatch a synthetic scroll event.
        // Headless Chrome does not fire scroll events from programmatic scrollTo,
        // so the explicit dispatchEvent is required for the infinite-scroll.js handler
        // to detect the trigger element's position via getBoundingClientRect().
        await page.WaitForFunctionAsync(
            @"(expectedCount) => {
                window.scrollTo({ top: document.documentElement.scrollHeight, behavior: 'instant' });
                window.dispatchEvent(new Event('scroll'));
                return document.querySelectorAll('" + itemSelector + @"').length >= expectedCount;
            }",
            expectedItemCount,
            new PageWaitForFunctionOptions { Timeout = timeoutMs, PollingInterval = DefaultPollingInterval });
    }

    /// <summary>
    /// Scrolls repeatedly until an end-of-content element appears or timeout is reached.
    /// Used for testing that infinite scroll reaches the end of a finite collection.
    /// Uses the same <c>window.scrollTo</c> + synthetic event strategy as
    /// <see cref="ScrollToLoadMoreAsync"/>.
    /// <para>
    /// Handles two scenarios:
    /// <list type="bullet">
    /// <item><b>Small collection</b> (all items fit in one batch): The end-of-content marker
    /// is rendered immediately after the first load — no scroll trigger or scroll listener
    /// exists. The method detects this and returns without scrolling.</item>
    /// <item><b>Large collection</b> (multiple batches): The scroll listener is attached and
    /// the method scrolls repeatedly, dispatching synthetic scroll events, until all batches
    /// are loaded and the end-of-content marker appears.</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="page">The page to scroll</param>
    /// <param name="endSelector">CSS selector for the end-of-content marker (default: ".end-of-content")</param>
    /// <param name="timeoutMs">Total timeout for the operation</param>
    /// <param name="triggerId">The id of the scroll-trigger element used by infinite-scroll.js</param>
    public static async Task ScrollToEndOfContentAsync(
        this IPage page,
        string endSelector = ".end-of-content",
        int timeoutMs = DefaultTimeout,
        string triggerId = "scroll-trigger")
    {
        // Wait for EITHER:
        // 1. The end-of-content marker already present (small collection, all items in first batch,
        //    no scroll trigger was ever rendered, so no scroll listener exists), OR
        // 2. The scroll listener to be attached (large collection, multiple batches needed).
        await page.WaitForConditionAsync(
            $"() => document.querySelector('{endSelector}') !== null || window.__scrollListenerReady?.['{triggerId}'] === true",
            new PageWaitForFunctionOptions { Timeout = timeoutMs, PollingInterval = DefaultPollingInterval });

        // If end-of-content is already present, no scrolling needed — return immediately.
        var alreadyAtEnd = await page.EvaluateAsync<bool>(
            $"() => document.querySelector('{endSelector}') !== null");
        if (alreadyAtEnd)
        {
            return;
        }

        // On each poll: scroll to bottom and dispatch a synthetic scroll event.
        // See ScrollToLoadMoreAsync for why the explicit dispatchEvent is required.
        await page.WaitForFunctionAsync(
            @"() => {
                if (document.querySelector('" + endSelector + @"')) return true;
                window.scrollTo({ top: document.documentElement.scrollHeight, behavior: 'instant' });
                window.dispatchEvent(new Event('scroll'));
                return false;
            }",
            null, // no JS arg — must be explicit so options parameter is bound correctly
            new PageWaitForFunctionOptions { Timeout = timeoutMs, PollingInterval = DefaultPollingInterval });
    }

    // ============================================================================
    // PAGE CREATION
    // ============================================================================

    /// <summary>
    /// Creates a new page with optimized default settings for E2E tests.
    /// Uses aggressive timeouts to fail fast and avoid slow tests.
    /// </summary>
    public static async Task<IPage> NewPageWithDefaultsAsync(this IBrowserContext context)
    {
        var page = await context.NewPageAsync();
        page.SetDefaultTimeout(DefaultTimeout);
        page.SetDefaultNavigationTimeout(IncreasedTimeout);
        return page;
    }

    // ============================================================================
    // HIGH-LEVEL INTERACTION FLOWS - Use these in tests!
    //
    // These consolidate common patterns:
    // - Find element → Click → Wait for URL → Wait for state sync
    // ============================================================================

    /// <summary>
    /// Generic click-and-navigate pattern for all Blazor navigation scenarios.
    ///
    /// This consolidates the common pattern:
    /// 1. Find element by selector (with optional text filter or href filter)
    /// 2. Click the element
    /// 3. Wait for URL to change (optional)
    /// 4. Wait for UI state sync (optional - e.g., .active class update)
    /// 5. Wait for page-specific content (optional - e.g., detail page ready)
    ///
    /// Use this instead of writing custom click-wait sequences.
    ///
    /// Example (collection button):
    ///   await page.ClickAndNavigateAsync(".sub-nav a", text: "Videos",
    ///       expectedUrlSegment: "/videos", waitForActiveState: "Videos");
    ///
    /// Example (content card with href filter):
    ///   await page.ClickAndNavigateAsync(".card", filterByHref: "/roundups/",
    ///       customWait: p => p.WaitForContentDetailPageAsync());
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector for the element to click</param>
    /// <param name="text">Optional: Text to filter by (uses HasTextString)</param>
    /// <param name="filterByHref">Optional: Filter by href attribute containing this value</param>
    /// <param name="nth">Optional: Select nth matching element (0-based, default: first)</param>
    /// <param name="expectedUrlSegment">Optional: URL segment to wait for after click</param>
    /// <param name="waitForActiveState">Optional: Text that should appear in .active button after navigation</param>
    /// <param name="customWait">Optional: Custom wait function to call after navigation</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task ClickAndNavigateAsync(
        this IPage page,
        string selector,
        string? text = null,
        string? filterByHref = null,
        int nth = 0,
        string? expectedUrlSegment = null,
        string? waitForActiveState = null,
        Func<IPage, Task>? customWait = null,
        int timeoutMs = IncreasedTimeout)
    {
        // Step 1: Find the element
        var locator = page.Locator(selector);

        // Apply text filter if specified
        if (!string.IsNullOrEmpty(text))
        {
            locator = page.Locator(selector, new() { HasTextString = text });
        }

        // Apply href filter if specified
        if (!string.IsNullOrEmpty(filterByHref))
        {
            locator = locator.Filter(new() { Has = page.Locator($"[href*='{filterByHref}']") });
        }

        // Select nth element if needed
        if (nth > 0 || (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(filterByHref)))
        {
            locator = locator.Nth(nth);
        }

        // Step 2: Click the element
        await locator.ClickBlazorElementAsync(timeoutMs);

        // Step 3: Wait for URL to change (if specified)
        if (!string.IsNullOrEmpty(expectedUrlSegment))
        {
            await page.WaitForBlazorUrlContainsAsync(expectedUrlSegment, timeoutMs);
        }

        // Step 4: Wait for state sync (if specified)
        if (!string.IsNullOrEmpty(waitForActiveState))
        {
            await page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", waitForActiveState);
        }

        // Step 5: Custom wait (if specified)
        if (customWait != null)
        {
            await customWait(page);
        }
    }

    // ============================================================================
    // GENERIC ELEMENT ASSERTIONS
    // ============================================================================

    /// <summary>
    /// Asserts that an element is visible.
    /// Generic helper for any element visibility check.
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementVisibleAsync(
        this ILocator locator,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(locator)
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element has a specific CSS class.
    /// Supports both exact string matching and regex patterns.
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="className">The exact class name to check for</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertHasClassAsync(
        this ILocator locator,
        string className,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(locator)
            .ToHaveClassAsync(className, new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element has a CSS class matching a regex pattern.
    /// Useful for checking dynamic or partial class names.
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="pattern">The regex pattern to match against the class attribute</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertHasClassAsync(
        this ILocator locator,
        System.Text.RegularExpressions.Regex pattern,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(locator)
            .ToHaveClassAsync(pattern, new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that a locator matches a specific number of elements.
    /// Uses Playwright's auto-retrying assertion mechanism.
    /// </summary>
    /// <param name="locator">The locator to count</param>
    /// <param name="expectedCount">The expected number of matching elements</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertCountAsync(
        this ILocator locator,
        int expectedCount,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(locator)
            .ToHaveCountAsync(expectedCount, new() { Timeout = timeoutMs });
    }

    // ============================================================================
    // NAVIGATION - The core challenge with Blazor SPAs
    // ============================================================================

    /// <summary>
    /// Navigates to a URL and waits for Blazor to be ready.
    ///
    /// This handles the Blazor lifecycle:
    /// 1. Initial HTML renders (server-side)
    /// 2. Blazor JS loads and connects SignalR
    /// 3. Enhanced navigation handlers attach
    /// 4. Content becomes interactive
    ///
    /// Uses Playwright's built-in waiting where possible.
    /// </summary>
    public static async Task GotoAndWaitForBlazorAsync(
        this IPage page,
        string url,
        PageGotoOptions? options = null)
    {
        var gotoOptions = options ?? new PageGotoOptions();
        gotoOptions.WaitUntil ??= WaitUntilState.DOMContentLoaded;
        gotoOptions.Timeout ??= IncreasedTimeout;

        await page.GotoAsync(url, gotoOptions);

        // Wait for Blazor runtime - includes Mermaid diagram check if present
        // Playwright's auto-waiting handles element visibility/stability automatically
        await page.WaitForBlazorReadyAsync();
    }

    /// <summary>
    /// Navigates to a relative URL (prepends BaseUrl automatically).
    /// Always waits for Blazor to be ready.
    ///
    /// USE THIS instead of Page.GotoAsync() for all test navigation!
    ///
    /// Example: await page.GotoRelativeAsync("/ai/genai-basics")
    /// </summary>
    public static Task GotoRelativeAsync(
        this IPage page,
        string relativeUrl,
        string baseUrl = "https://localhost:5003",
        PageGotoOptions? options = null)
    {
        var fullUrl = $"{baseUrl}{relativeUrl}";
        return page.GotoAndWaitForBlazorAsync(fullUrl, options);
    }

    /// <summary>
    /// Waits for Blazor runtime to be loaded and interactive, including Mermaid diagrams if present.
    /// 
    /// Uses the official Blazor JavaScript initializer pattern:
    /// - TechHub.Web.lib.module.js exports afterServerStarted() which sets window.__blazorServerReady
    /// - This method waits for that flag, ensuring the SignalR circuit is fully established
    ///   and @onclick handlers are attached
    ///
    /// This approach is cleaner than Task.Delay as it uses Blazor's own lifecycle callbacks.
    /// 
    /// @see https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup
    /// INTERNAL USE: Called automatically by GotoAndWaitForBlazorAsync and ClickBlazorElementAsync.
    /// </summary>
    public static async Task WaitForBlazorReadyAsync(this IPage page, int timeoutMs = IncreasedTimeout)
    {
        try
        {
            // Single combined check for all readiness conditions INCLUDING Mermaid diagrams:
            // 1. Blazor runtime exists
            // 2. Interactive runtime is ready (Server/WASM circuit established, or SSR-only page)
            // 3. Page scripts finished loading (mermaid, highlight.js, custom-pages, etc.)
            // 4. Mermaid diagrams rendered (if present on page)
            //
            // Previously these were 3 separate WaitForConditionAsync calls, each requiring
            // its own browser round-trip and polling cycle. Combining them into one eliminates
            // ~100-200ms overhead per navigation across 200+ navigations in the E2E suite.
            //
            // Adding Mermaid check here (instead of separate call after) saves another ~50-150ms
            // on pages with diagrams by eliminating another browser round trip.
            await page.WaitForConditionAsync(@"
                () => {
                    // Step 1: Blazor runtime must exist
                    if (typeof window.Blazor === 'undefined') return false;

                    // Step 2: Interactive runtime or SSR must be ready
                    // Flags set by TechHub.Web.lib.module.js afterServerStarted/afterWebAssemblyStarted/afterWebStarted
                    if (window.__blazorServerReady !== true && 
                        window.__blazorWasmReady !== true && 
                        window.__blazorWebReady !== true) {
                        return false;
                    }

                    // Step 3: Page scripts must not be actively loading
                    // __scriptsLoading is set true by markScriptsLoading() when page scripts start,
                    // and set false by markScriptsReady() when they complete.
                    // Only block if scripts are ACTIVELY loading. If both flags are undefined,
                    // the page has no page scripts (e.g., SectionCollection.razor) — proceed immediately.
                    if (window.__scriptsLoading === true) return false;

                    // Step 4: Mermaid diagrams rendered (if present)
                    // Only wait if page has <pre class='mermaid'> elements that haven't been converted to SVG yet
                    const mermaidPres = document.querySelectorAll('pre.mermaid');
                    if (mermaidPres.length > 0) {
                        const renderedSvgs = document.querySelectorAll('svg[id^=""mermaid-""]');
                        if (renderedSvgs.length < mermaidPres.length) return false;
                    }

                    return true;
                }
            ", new PageWaitForFunctionOptions { Timeout = timeoutMs, PollingInterval = DefaultPollingInterval });
        }
        catch (TimeoutException)
        {
            // Static pages without Blazor - that's fine, continue
        }
    }

    // ============================================================================
    // ELEMENT INTERACTION - Use Playwright's built-in auto-waiting
    // ============================================================================

    /// <summary>
    /// Clicks a Blazor-enhanced element after ensuring it's ready for interaction.
    ///
    /// CRITICAL BLAZOR INSIGHT:
    /// Playwright's "stable" check waits for an element to stop moving/animating.
    /// However, Blazor Server with enhanced navigation continuously updates the DOM
    /// in subtle ways (SignalR heartbeats, streaming renders, etc.) that can
    /// prevent elements from ever being considered "stable" by Playwright.
    ///
    /// This method:
    /// 1. Waits for element to be visible (auto-retrying assertion)
    /// 2. Waits for Blazor interactivity to be ready (using official JS initializer callbacks)
    /// 3. Captures current URL before click
    /// 4. Uses Force=true to bypass stability checks (safe because we've already
    ///    verified Blazor is ready and the element is visible)
    /// 5. Waits for URL to change (Blazor Server updates URL via SignalR)
    /// 6. Waits for network to settle
    ///
    /// This is NOT a hack - it's the correct pattern for Blazor Server testing.
    /// The "stability" check is designed for CSS animations, not for Blazor's
    /// continuous DOM updates.
    /// </summary>
    /// <param name="locator">The element to click</param>
    /// <param name="timeoutMs">Maximum time to wait for interactivity and URL change</param>
    /// <param name="waitForUrlChange">Whether to wait for URL to change after click (default: true)</param>
    public static async Task ClickBlazorElementAsync(
        this ILocator locator,
        int timeoutMs = IncreasedTimeout,
        bool waitForUrlChange = true)
    {
        var page = locator.Page;
        var urlBeforeClick = page.Url;

        // Step 1: Wait for element to be visible using our centralized helper
        await locator.AssertElementVisibleAsync(timeoutMs);

        // Step 2: Ensure Blazor interactivity is ready (SignalR circuit established, event handlers attached)
        // This uses the official Blazor JS initializer callback - no arbitrary delays needed
        await page.WaitForBlazorReadyAsync(timeoutMs);

        // Step 3: Click with Force=true to bypass stability checks
        // This is necessary because Blazor's continuous DOM updates prevent
        // elements from being considered "stable" by Playwright's criteria.
        // We've already verified the element is visible and Blazor is ready,
        // so the click will work correctly.
        await locator.ClickAsync(new() { Force = true, Timeout = timeoutMs });

        // Step 4: Wait for URL to change (Blazor Server updates URL via SignalR/WebSocket)
        // This is the default because most interactive Blazor elements change the URL
        // (navigation links, tag toggles, collection buttons, etc.)
        if (waitForUrlChange)
        {
            // CRITICAL: Use WaitForConditionAsync instead of WaitForURLAsync!
            // Blazor Server updates URLs via pushState (history API), not HTTP navigation.
            // WaitForURLAsync waits for navigation events which don't fire with pushState.
            // WaitForConditionAsync polls the DOM directly, which works for any URL change.
            await page.WaitForConditionAsync(
                "expectedUrl => window.location.href !== expectedUrl",
                urlBeforeClick,
                new() { Timeout = timeoutMs });
        }

        // Mermaid diagram check is now integrated into WaitForBlazorReadyAsync above (Step 2)
    }

    /// <summary>
    /// Waits for a Blazor element to be fully interactive and actionable.
    /// Use this before custom interactions that don't use ClickBlazorElementAsync.
    /// </summary>
    public static async Task WaitForBlazorInteractivityAsync(
        this ILocator locator,
        int timeoutMs = DefaultTimeout)
    {
        // Wait for element to be visible using our centralized helper
        await locator.AssertElementVisibleAsync(timeoutMs);

        // Ensure Blazor is ready - uses its own default navigation timeout (2000ms)
        await locator.Page.WaitForBlazorReadyAsync();
    }

    // ============================================================================
    // URL DETECTION - For SPA navigation without page reloads
    // ============================================================================

    /// <summary>
    /// Waits for URL to contain a specific path segment after SPA navigation.
    ///
    /// Modern SPAs (React, Angular, Vue, Blazor) use client-side routing that
    /// changes URLs without triggering traditional navigation events.
    ///
    /// PREFERRED for Blazor: Uses Playwright's auto-retrying Expect assertion with regex.
    /// Much cleaner than manual polling with WaitForFunctionAsync!
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="urlSegment">The URL path segment to wait for (e.g., "news", "videos")</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task WaitForBlazorUrlContainsAsync(
        this IPage page,
        string urlSegment,
        int timeoutMs = IncreasedTimeout)
    {
        // Use Playwright's auto-retrying Expect assertion with regex - much cleaner!
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex($".*{System.Text.RegularExpressions.Regex.Escape(urlSegment)}.*"),
            new() { Timeout = timeoutMs }
        );
    }

    // ============================================================================
    // STATE SYNCHRONIZATION - For browser history navigation
    // ============================================================================

    /// <summary>
    /// Waits for a Blazor component to finish rendering a selector.
    /// Uses our centralized AssertElementVisibleAsync helper.
    /// </summary>
    public static async Task WaitForBlazorRenderAsync(
        this IPage page,
        string selector,
        int timeoutMs = DefaultTimeout)
    {
        await page.Locator(selector).AssertElementVisibleAsync(timeoutMs);
    }

    // ============================================================================
    // CENTRALIZED TIMEOUT METHODS - Single point of control
    // ============================================================================

    /// <summary>
    /// Waits for a selector with standard timeout.
    /// Centralized timeout management - change DefaultTimeout to affect all tests.
    /// </summary>
    public static async Task<ILocator> WaitForSelectorWithTimeoutAsync(
        this IPage page,
        string selector,
        PageWaitForSelectorOptions? options = null)
    {
        var opts = options ?? new PageWaitForSelectorOptions();
        opts.Timeout ??= DefaultTimeout;
        opts.State ??= WaitForSelectorState.Visible;

        await page.WaitForSelectorAsync(selector, opts);
        return page.Locator(selector);
    }

    /// <summary>
    /// Gets text content with standard timeout.
    /// Centralized timeout management - change DefaultTimeout to affect all tests.
    /// </summary>
    public static Task<string?> TextContentWithTimeoutAsync(
        this ILocator locator,
        LocatorTextContentOptions? options = null)
    {
        var opts = options ?? new LocatorTextContentOptions();
        opts.Timeout ??= DefaultTimeout;
        return locator.TextContentAsync(opts);
    }

    /// <summary>
    /// Waits for URL with standard timeout (glob pattern).
    /// Centralized timeout management.
    /// </summary>
    public static Task WaitForURLWithTimeoutAsync(
        this IPage page,
        string urlPattern,
        PageWaitForURLOptions? options = null)
    {
        var opts = options ?? new PageWaitForURLOptions();
        opts.Timeout ??= IncreasedTimeout;
        return page.WaitForURLAsync(urlPattern, opts);
    }

    /// <summary>
    /// Gets the href attribute value from a locator.
    /// Common pattern: Extract href from links for validation or navigation.
    /// </summary>
    /// <param name="locator">The element to get href from</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    /// <returns>The href attribute value, or null if not found</returns>
    public static async Task<string?> GetHrefAsync(
        this ILocator locator,
        int timeoutMs = DefaultTimeout)
    {
        return await locator.GetAttributeAsync("href", new() { Timeout = timeoutMs });
    }

    // ============================================================================
    // ARIA ROLE HELPERS - Accessible element location and assertions
    // ============================================================================

    /// <summary>
    /// Asserts that an element with the specified ARIA role and name is visible.
    /// Uses accessible role-based selection for better accessibility testing.
    /// Case-sensitive exact matching.
    ///
    /// Examples:
    ///   await page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Latest Content");
    ///   await page.AssertElementVisibleByRoleAsync(AriaRole.Link, "About");
    ///   await page.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Overview", level: 2);
    ///   await page.AssertElementVisibleByRoleAsync(AriaRole.Button, "Submit");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="role">The ARIA role to find</param>
    /// <param name="name">The exact accessible name (case-sensitive)</param>
    /// <param name="level">Optional: Heading level (1-6) when role is Heading</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementVisibleByRoleAsync(
        this IPage page,
        AriaRole role,
        string name,
        int? level = null,
        int timeoutMs = DefaultTimeout)
    {
        var options = new PageGetByRoleOptions
        {
            Name = name,
            Exact = true
        };

        if (level.HasValue)
        {
            options.Level = level.Value;
        }

        var element = page.GetByRole(role, options);
        await Assertions.Expect(element).ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element with the specified ARIA role and name is visible within a locator scope.
    /// Uses accessible role-based selection for better accessibility testing.
    /// Case-sensitive exact matching.
    ///
    /// Examples:
    ///   var section = page.Locator(".team-member");
    ///   await section.AssertElementVisibleByRoleAsync(AriaRole.Link, "GitHub");
    ///   await section.AssertElementVisibleByRoleAsync(AriaRole.Heading, "Team Lead", level: 3);
    /// </summary>
    /// <param name="locator">The locator to search within</param>
    /// <param name="role">The ARIA role to find</param>
    /// <param name="name">The exact accessible name (case-sensitive)</param>
    /// <param name="level">Optional: Heading level (1-6) when role is Heading</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementVisibleByRoleAsync(
        this ILocator locator,
        AriaRole role,
        string name,
        int? level = null,
        int timeoutMs = DefaultTimeout)
    {
        var options = new LocatorGetByRoleOptions
        {
            Name = name,
            Exact = true
        };

        if (level.HasValue)
        {
            options.Level = level.Value;
        }

        var element = locator.GetByRole(role, options);
        await Assertions.Expect(element).ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element with the specified alt text is visible.
    /// Uses accessible alt text selection for better accessibility testing.
    /// Case-sensitive exact matching.
    ///
    /// Example:
    ///   await page.AssertElementVisibleByAltTextAsync("Reinier van Maanen photo");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="altText">The exact alt text (case-sensitive)</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementVisibleByAltTextAsync(
        this IPage page,
        string altText,
        int timeoutMs = DefaultTimeout)
    {
        var element = page.GetByAltText(altText, new PageGetByAltTextOptions { Exact = true });
        await Assertions.Expect(element).ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element matching the selector is visible.
    /// Generic helper for any CSS selector.
    ///
    /// Examples:
    ///   await page.AssertElementVisibleBySelectorAsync(".sidebar");
    ///   await page.AssertElementVisibleBySelectorAsync("main article");
    ///   await page.AssertElementVisibleBySelectorAsync(".card");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementVisibleBySelectorAsync(
        this IPage page,
        string selector,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element matching the selector contains the expected text.
    /// Uses Playwright's auto-retrying assertions.
    ///
    /// Example:
    ///   await page.AssertElementContainsTextBySelectorAsync(".sub-nav a.active", "Videos");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="expectedText">Expected text content</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementContainsTextBySelectorAsync(
        this IPage page,
        string selector,
        string expectedText,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToContainTextAsync(expectedText, new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that the element count matches expected value.
    /// Uses Playwright's auto-retrying count assertion.
    ///
    /// Example:
    ///   await page.AssertElementCountBySelectorAsync(".card", 10);
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="expectedCount">Expected number of elements</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementCountBySelectorAsync(
        this IPage page,
        string selector,
        int expectedCount,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToHaveCountAsync(expectedCount, new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Gets an element attribute value by selector.
    ///
    /// Example:
    ///   var href = await page.GetElementAttributeBySelectorAsync("a.rss-link", "href");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="attributeName">Attribute name (e.g., "href", "class", "style")</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    /// <returns>Attribute value or null</returns>
    public static async Task<string?> GetElementAttributeBySelectorAsync(
        this IPage page,
        string selector,
        string attributeName,
        int timeoutMs = DefaultTimeout)
    {
        return await page.Locator(selector).GetAttributeAsync(attributeName, new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Gets an element's text content by selector.
    ///
    /// Example:
    ///   var heading = await page.GetElementTextBySelectorAsync("h1");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    /// <returns>Text content or null</returns>
    public static async Task<string?> GetElementTextBySelectorAsync(
        this IPage page,
        string selector,
        int timeoutMs = DefaultTimeout)
    {
        return await page.Locator(selector).TextContentAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Gets the count of elements matching the selector.
    /// Use this for conditional logic, not for assertions (use AssertElementCountBySelectorAsync for assertions).
    ///
    /// Example:
    ///   var count = await page.GetElementCountBySelectorAsync(".card");
    ///   if (count > 0) { /* do something */ }
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <returns>Number of matching elements</returns>
    public static async Task<int> GetElementCountBySelectorAsync(
        this IPage page,
        string selector)
    {
        return await page.Locator(selector).CountAsync();
    }

    /// <summary>
    /// Gets the count of elements matching the selector within a locator scope.
    /// Use this for conditional logic, not for assertions.
    ///
    /// Example:
    ///   var grid = page.Locator(".section-grid");
    ///   var count = await grid.GetElementCountBySelectorAsync("> .section-card-container");
    ///   if (count > 0) { /* do something */ }
    /// </summary>
    /// <param name="locator">The locator to search within</param>
    /// <param name="selector">CSS selector</param>
    /// <returns>Number of matching elements</returns>
    public static async Task<int> GetElementCountBySelectorAsync(
        this ILocator locator,
        string selector)
    {
        return await locator.Locator(selector).CountAsync();
    }

    /// <summary>
    /// Asserts that the current URL ends with the specified segment.
    /// Uses Playwright's auto-retrying Expect assertion.
    ///
    /// Example:
    ///   await page.AssertUrlEndsWithAsync("/github-copilot/news");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="urlSegment">The URL segment to check for at the end</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertUrlEndsWithAsync(
        this IPage page,
        string urlSegment,
        int timeoutMs = IncreasedTimeout)
    {
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex($".*{System.Text.RegularExpressions.Regex.Escape(urlSegment)}$"),
            new() { Timeout = timeoutMs }
        );
    }

    /// <summary>
    /// Clicks an element found by selector using Blazor-aware click handling.
    ///
    /// Example:
    ///   await page.ClickElementBySelectorAsync(".sub-nav a");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task ClickElementBySelectorAsync(
        this IPage page,
        string selector,
        int timeoutMs = DefaultTimeout)
    {
        await page.Locator(selector).ClickBlazorElementAsync(timeoutMs);
    }

    /// <summary>
    /// Clicks an element found by ARIA role using Blazor-aware click handling.
    ///
    /// Example:
    ///   await page.ClickElementByRoleAsync(AriaRole.Link, "About");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="role">ARIA role</param>
    /// <param name="name">Accessible name (case-sensitive)</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task ClickElementByRoleAsync(
        this IPage page,
        AriaRole role,
        string name,
        int timeoutMs = DefaultTimeout)
    {
        var options = new PageGetByRoleOptions
        {
            Name = name,
            Exact = true
        };
        await page.GetByRole(role, options).ClickBlazorElementAsync(timeoutMs);
    }

    /// <summary>
    /// Asserts that an element's href attribute equals the expected value.
    /// Uses Playwright's auto-retrying assertions.
    ///
    /// Example:
    ///   await locator.AssertHrefEqualsAsync("/github-copilot/feed.xml");
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="expectedHref">Expected href value (case-sensitive exact match)</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertHrefEqualsAsync(
        this ILocator locator,
        string expectedHref,
        int timeoutMs = DefaultTimeout)
    {
        await Assertions.Expect(locator)
            .ToHaveAttributeAsync("href", expectedHref, new() { Timeout = timeoutMs });
    }

    // ============================================================================
    // SCROLL HELPERS - Smart scrolling with automatic detection
    // ============================================================================

    /// <summary>
    /// Clicks an element that triggers scrolling (e.g., TOC link) and waits for scroll to complete.
    ///
    /// This is a smart wrapper that:
    /// 1. Sets up a scrollend event listener BEFORE clicking (same event TOC scroll-spy uses)
    /// 2. Clicks the element (which triggers anchor navigation/scrolling)
    /// 3. Waits for scrollend event (fired when scrolling stops)
    /// 4. Waits one additional animation frame for TOC scroll-spy to update active class
    /// 5. Returns when everything is complete or max timeout is reached
    ///
    /// Use this for TOC links, anchor links, or any clickable element that causes scrolling.
    ///
    /// Examples:
    ///   // Click TOC link and wait for scroll + TOC update
    ///   await tocLink.ClickAndWaitForScrollAsync();
    ///
    ///   // Click with custom timeout
    ///   await anchorLink.ClickAndWaitForScrollAsync(maxTimeoutMs: 500);
    /// </summary>
    /// <param name="locator">The element to click</param>
    /// <param name="maxTimeoutMs">Maximum time in ms to wait for scroll to complete (default: DefaultTimeout)</param>
    /// <returns>Task that completes when scrolling finishes and TOC updates, or timeout is reached</returns>
    public static async Task ClickAndWaitForScrollAsync(
        this ILocator locator,
        int maxTimeoutMs = DefaultTimeout)
    {
        // Use scrollend event (same as TOC scroll-spy) + RAF for TOC update
        // NOTE: When using EvaluateAsync on a locator, the element is passed as the first parameter,
        // and our options object as the second parameter
        await locator.EvaluateAsync($@"
            (element, {{ maxTimeout }}) => {{
                return new Promise((resolve) => {{
                    let maxTimer;
                    let scrollDetected = false;
                    
                    const cleanup = () => {{
                        window.removeEventListener('scrollend', onScrollEnd);
                        window.removeEventListener('scroll', onScroll);
                        clearTimeout(maxTimer);
                    }};
                    
                    const onScrollEnd = () => {{
                        // Wait TWO animation frames for TOC scroll-spy to update active class:
                        // 1st RAF: scrollend fires
                        // 2nd RAF: TOC scroll-spy's handleScroll() RAF callback executes updateActiveHeading()
                        requestAnimationFrame(() => {{
                            requestAnimationFrame(() => {{
                                cleanup();
                                resolve();
                            }});
                        }});
                    }};
                    
                    const onScroll = () => {{
                        scrollDetected = true;
                    }};
                    
                    // Set up maximum timeout
                    maxTimer = setTimeout(() => {{
                        cleanup();
                        resolve();
                    }}, maxTimeout);
                    
                    // Set up scroll listeners BEFORE clicking
                    window.addEventListener('scrollend', onScrollEnd, {{ once: true }});
                    window.addEventListener('scroll', onScroll, {{ passive: true }});
                    
                    // Click the element
                    element.click();
                    
                    // If no scroll detected after 50ms, assume no scroll (e.g., already at target)
                    setTimeout(() => {{
                        if (!scrollDetected) {{
                            cleanup();
                            resolve();
                        }}
                    }}, 50);
                }});
            }}
        ", new { maxTimeout = maxTimeoutMs });
    }

    /// <summary>
    /// Scrolls an element into view using JavaScript's scrollIntoView API.
    ///
    /// Unlike Playwright's built-in ScrollIntoViewIfNeededAsync, this method does NOT
    /// perform stability checks. Playwright's stability check waits for the element's
    /// bounding box to remain unchanged across two animation frames, which fails for
    /// elements that resize after scrolling (e.g., lazy-loaded images that go from 0×0
    /// to their natural dimensions once they enter the viewport).
    ///
    /// Use this method when scrolling to elements that may change size dynamically
    /// (lazy images, animated elements, elements with CSS transitions on visibility).
    /// </summary>
    /// <param name="locator">The element to scroll into view</param>
    /// <param name="block">Vertical alignment: "center", "start", "end", or "nearest"</param>
    public static async Task ScrollIntoViewAsync(
        this ILocator locator,
        string block = "center")
    {
        await locator.EvaluateAsync($"el => el.scrollIntoView({{ behavior: 'instant', block: '{block}' }})");
    }

    /// <summary>
    /// Waits for scrolling to complete by detecting scroll inactivity.
    ///
    /// This function uses a debounce pattern - it waits for NO scroll events
    /// to occur for the specified timeout duration. This ensures we only
    /// proceed after scrolling has truly finished.
    ///
    /// Use this after triggering scrolling via native Playwright methods
    /// like Mouse.WheelAsync() or ScrollIntoViewIfNeededAsync().
    ///
    /// Example:
    ///   await page.Mouse.WheelAsync(0, 1000);
    ///   await page.WaitForScrollEndAsync();
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="scrollEndTimeoutMs">Time in ms to wait for no scroll events (default: 300ms)</param>
    /// <returns>Task that completes when scrolling finishes</returns>
    public static async Task WaitForScrollEndAsync(
        this IPage page,
        int scrollEndTimeoutMs = 300)
    {
        // Use JavaScript to detect scroll end via debouncing
        // This returns a Promise that resolves when scrolling stops
        await page.EvaluateAsync($@"
            () => {{
                return new Promise((resolve) => {{
                    let scrollTimer;
                    const onScroll = () => {{
                        clearTimeout(scrollTimer);
                        scrollTimer = setTimeout(() => {{
                            window.removeEventListener('scroll', onScroll);
                            resolve();
                        }}, {scrollEndTimeoutMs});
                    }};
                    window.addEventListener('scroll', onScroll);
                    // Trigger initial timer in case scroll already finished
                    onScroll();
                }});
            }}
        ");
    }
}

/// <summary>
/// Custom exception for assertion failures with better error messages
/// </summary>
public class AssertionException : Exception
{
    public AssertionException(string message) : base(message)
    {
        ArgumentNullException.ThrowIfNull(message);
    }
}

