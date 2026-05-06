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
/// TIMEOUT MANAGEMENT: All timeouts use a single E2ETimeout (60s) safety net with
/// E2EPollingInterval (100ms). Counter-based polling means timeouts never fire in
/// normal operation — they only catch genuine hangs.
///
/// USAGE EXAMPLES - Common Test Patterns:
///
/// 1. Click tag and wait for URL change:
///    await tagButton.ClickAndExpectAsync(async () =>
///        await Assertions.Expect(page).ToHaveURLAsync(new Regex(@".*tags=.*"), new() { Timeout = 2000 }));
///
/// 2. Click section card on homepage:
///    await sectionCard.ClickAndExpectAsync(async () =>
///        await Assertions.Expect(page).Not.ToHaveURLAsync(
///            new Regex($"^{Regex.Escape(BlazorHelpers.BaseUrl)}/?$"), new() { Timeout = 2000 }));
///
/// 3. Click content card to detail page:
///    await roundupLink.ClickAndExpectAsync(async () =>
///        await Assertions.Expect(page).ToHaveURLAsync(new Regex(@".*/roundups/.*"), new() { Timeout = 2000 }));
///
/// 4. Click a button that toggles a CSS class (no navigation):
///    await filterButton.ClickAndExpectAsync(async () =>
///        await Assertions.Expect(filterButton).Not.ToHaveClassAsync(
///            new Regex("active"), new() { Timeout = 2000 }));
///
/// 5. Use Expect assertions instead of TextContentAsync + Should.Contain:
///    // DON'T: var text = await element.TextContentAsync(); text.Should().Contain("foo");
///    // DO: await Assertions.Expect(element).ToContainTextAsync("foo");
/// </summary>
public static class BlazorHelpers
{
    // ============================================================================
    // CONFIGURATION - Centralized timeout management
    //
    // SINGLE TIMEOUT: All E2E operations use one timeout that scales with the active
    // network profile. Counter-based polling (100ms intervals) means this is a pure
    // safety net — it only fires on genuine hangs. The timeout is tuned per profile:
    //   no profile (local fast) → 10s
    //   regular4g / wan / ci   → 30s
    //   fast3g                 → 45s
    //   slow3g                 → 60s
    // CI without a profile also gets 60s (GitHub Actions sets CI=true).
    // ============================================================================

    /// <summary>
    /// Single timeout for all E2E operations. Scales with E2E_NETWORK_THROTTLE (set by
    /// Run -NetworkProfile) and CI environment. Counter-based polling at 100ms means
    /// this is a pure safety net that should never fire in normal operation.
    /// </summary>
    internal static int E2ETimeout { get; } = ResolveTimeout();

    /// <summary>
    /// Polling interval for WaitForFunctionAsync operations.
    /// 100ms = good balance between responsiveness and CPU.
    /// Checking one window property costs ~0.
    /// </summary>
    internal const int E2EPollingInterval = 100;

    /// <summary>
    /// Timeout for browser launch operations.
    /// Separate from test timeouts as this is infrastructure initialization.
    /// </summary>
    internal const int BrowserLaunchTimeout = 30_000;

    /// <summary>Base URL for the Web frontend. Override with E2E_BASE_URL env var for CI/staging.</summary>
    public static readonly string BaseUrl = (Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "https://localhost:5003").TrimEnd('/');

    private static int ResolveTimeout()
    {
        var profile = Environment.GetEnvironmentVariable("E2E_NETWORK_THROTTLE") ?? "";
        var isCI = string.Equals(Environment.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase);
        return profile switch
        {
            "slow3g" => 60_000,
            "fast3g" => 45_000,
            "regular4g" or "wan" or "ci" => 30_000,
            _ => isCI ? 60_000   // GitHub Actions (CI=true) without a profile
                      : 10_000,  // Local fast mode
        };
    }

    // ============================================================================
    // SAFE WaitForFunctionAsync WRAPPERS
    // ============================================================================
    // Playwright .NET has a SINGLE overload:
    //   WaitForFunctionAsync(string expression, object? arg = null, PageWaitForFunctionOptions? options = null)
    //
    // BUG TRAP: If you call WaitForFunctionAsync("...", new PageWaitForFunctionOptions { Timeout = BlazorHelpers.E2ETimeout }),
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
    /// Convenience overload that uses E2ETimeout.
    /// Use this for expressions that take NO JavaScript arguments.
    /// </summary>
    /// <param name="page">The page to evaluate on</param>
    /// <param name="expression">JavaScript expression that returns a truthy/falsy value, e.g. "() => document.querySelector('.card') !== null"</param>
    /// <param name="onTimeout">Optional JavaScript expression evaluated when the wait times out.
    /// Its result is appended to the timeout message for diagnostics, e.g.
    /// <c>"() => JSON.stringify({scrollY: window.scrollY})"</c>.</param>
    public static async Task<IJSHandle> WaitForConditionAsync(
        this IPage page,
        string expression,
        string? onTimeout = null)
    {
        try
        {
            return await page.WaitForFunctionAsync(expression, null, new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });
        }
        catch (TimeoutException ex) when (onTimeout != null)
        {
            var diag = await page.EvaluateAsync<string>($"() => String({onTimeout}())");
            throw new TimeoutException($"{ex.Message}\nDiagnostics: {diag}", ex);
        }
    }

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
    /// Convenience overload that uses E2ETimeout.
    /// </summary>
    /// <param name="page">The page to evaluate on</param>
    /// <param name="expression">JavaScript expression accepting one arg</param>
    /// <param name="arg">Value passed to the JavaScript function</param>
    /// <param name="onTimeout">Optional JavaScript expression evaluated when the wait times out.
    /// Its result is appended to the timeout message for diagnostics, e.g.
    /// <c>"() => JSON.stringify({scrollY: window.scrollY})"</c>.</param>
    public static async Task<IJSHandle> WaitForConditionAsync(
        this IPage page,
        string expression,
        object arg,
        string? onTimeout = null)
    {
        try
        {
            return await page.WaitForFunctionAsync(expression, arg, new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });
        }
        catch (TimeoutException ex) when (onTimeout != null)
        {
            var diag = await page.EvaluateAsync<string>($"() => String({onTimeout}())");
            throw new TimeoutException($"{ex.Message}\nDiagnostics: {diag}", ex);
        }
    }

    // ============================================================================
    // RETRY-UNTIL-PASS — The Playwright-idiomatic fix for flaky Blazor interactions
    //
    // Playwright's JavaScript API has `expect(fn).toPass()` which retries a whole
    // code block (action + assertions) until it passes or times out. The .NET API
    // does not expose this, so we implement the same pattern here. This is the
    // canonical solution for Blazor Server tests where a click may be silently
    // lost if the @onclick handler hasn't attached yet after SignalR hydration.
    //
    // See: https://playwright.dev/docs/test-assertions#expecttopass
    // ============================================================================

    /// <summary>
    /// Retries an async code block until it completes without throwing, or until
    /// <paramref name="totalTimeoutMs"/> is exceeded. .NET equivalent of Playwright's
    /// JS <c>expect(fn).toPass()</c>.
    ///
    /// Use for [action + assertion] blocks where the action may need to be repeated
    /// (e.g., a click that was lost because the event handler hadn't attached yet).
    /// Inner assertions should use short timeouts (1-3s) so a failed attempt fails
    /// fast and retries quickly; the outer <paramref name="totalTimeoutMs"/> is the
    /// overall budget.
    /// </summary>
    public static async Task RetryUntilPassAsync(
        Func<Task> action,
        int totalTimeoutMs = -1)
    {
        if (totalTimeoutMs < 0) totalTimeoutMs = E2ETimeout;
        // Progressive backoff matches Playwright's JS toPass default intervals.
        // This is retry backoff between genuine assertion attempts — not an
        // arbitrary "wait for something to happen" sleep.
        var intervals = new[] { 100, 250, 500, 1000, 1000 };
        var deadline = DateTime.UtcNow.AddMilliseconds(totalTimeoutMs);
        var attempt = 0;
        Exception? last;
        while (true)
        {
            try
            {
                await action();
                return;
            }
            catch (Exception ex)
            {
                last = ex;
            }

            var delayMs = intervals[Math.Min(attempt, intervals.Length - 1)];
            if (DateTime.UtcNow.AddMilliseconds(delayMs) >= deadline)
            {
                break;
            }

            await Task.Delay(delayMs);
            attempt++;
        }

        throw new TimeoutException(
            $"RetryUntilPassAsync: action did not pass within {totalTimeoutMs}ms. Last error:\n{last?.Message}",
            last);
    }

    // ============================================================================
    // E2E LIFECYCLE SIGNALS — for genuine async events (scroll, TOC render)
    //
    // App.razor injects window.__e2e = { counter, label, history[] } in Development
    // and Staging. These helpers wait for a specific named signal to appear in the
    // history, used by scroll / TOC helpers for events that have no visible DOM
    // state change to assert against.
    // ============================================================================

    /// <summary>
    /// Captures the current E2E lifecycle counter value.
    /// INTERNAL — only called by scroll / TOC helpers.
    /// </summary>
    internal static async Task<int> GetE2ECounterAsync(this IPage page) =>
        await page.EvaluateAsync<int>("() => window.__e2e?.counter ?? 0");

    /// <summary>
    /// Waits for a specific signal label to appear in the E2E history after
    /// <paramref name="afterValue"/>. Uses the ring buffer to find the label even
    /// if later signals overwrote <c>e.label</c>.
    /// INTERNAL — only called by scroll / TOC helpers.
    /// </summary>
    internal static async Task WaitForE2ESignalAsync(this IPage page, int afterValue, string label) =>
        await page.WaitForFunctionAsync(
            @"([prev, lbl]) => {
                const e = window.__e2e;
                if (!e || e.counter <= prev) return false;
                return e.history ? e.history.some(h => h.counter > prev && h.label === lbl) : e.label === lbl;
            }",
            new object[] { afterValue, label },
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });

    // ============================================================================
    // SCROLL POSITIONING — Reliable scroll-to-Y for tests
    //
    // Under slow networks, the page may not have reached its final height when
    // WaitForBlazorReadyAsync returns (images still loading, lazy content not yet
    // rendered). scrollTo(0, y) is capped by the browser to maxScroll = scrollHeight
    // - innerHeight. This helper retries scrollTo on each poll iteration until
    // scrollY actually reaches the target — naturally waiting for the page to grow.
    //
    // FUTURE: If Blazor adds native scroll positioning via NavigationManager, the
    // JS implementation here can be swapped without changing any call sites.
    // ============================================================================

    /// <summary>
    /// Scrolls to a specific Y position, retrying until the page is tall enough.
    /// Dispatches a synthetic <c>scroll</c> event after each attempt so that
    /// scroll-manager.js records the position (headless Chrome doesn't fire scroll
    /// events from programmatic <c>scrollTo</c>).
    ///
    /// <para>Use this instead of raw <c>Page.EvaluateAsync("window.scrollTo(...)")</c>
    /// in all test code that needs a specific scroll position.</para>
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="y">Target vertical scroll position in pixels</param>
    public static async Task ScrollToPositionAsync(
        this IPage page,
        int y)
    {
        await page.WaitForFunctionAsync(
            @"(targetY) => {
                window.scrollTo(0, targetY);
                window.dispatchEvent(new Event('scroll'));
                return window.scrollY === targetY;
            }",
            y,
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });

        // Lock the saved scroll position so that Playwright's scrollIntoViewIfNeeded
        // (which fires before click events) cannot overwrite it via scroll events.
        // scroll-manager.js reads this lock in saveScrollPosition() and clears it
        // when restoreScrollPosition() starts.
        await page.EvaluateAsync(
            @"(targetY) => {
                const key = location.pathname + location.search;
                window.__scrollSaveLock = { key, value: targetY };
            }",
            y);
    }

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
    /// The scroll-manager.js handler listens for these events and uses
    /// <c>getBoundingClientRect</c> to check whether the trigger element is near the
    /// viewport, invoking <c>LoadNextBatch</c> when it is.
    /// </summary>
    /// <param name="page">The page to scroll</param>
    /// <param name="expectedItemCount">Minimum number of items expected after scroll</param>
    /// <param name="itemSelector">CSS selector for items to count (default: ".card")</param>
    public static async Task ScrollToLoadMoreAsync(
        this IPage page,
        int expectedItemCount,
        string itemSelector = ".card",
        string triggerId = "scroll-trigger")
    {
        // Wait for the scroll listener to be attached AND the trigger element to exist.
        // Both conditions must be true simultaneously to guard against stale readiness:
        // - After a tag filter change, ContentItemsGrid.LoadInitialBatch() calls dispose()
        //   which sets __scrollListenerReady[triggerId] = false.
        // - The new listener is attached in OnAfterRenderAsync → SetupScrollListener().
        // - Checking BOTH the flag AND the DOM element prevents acting on a stale readiness
        //   flag if the component is mid-render and the trigger element hasn't been re-created yet.
        // Uses E2ETimeout: after a tag filter the page re-renders before the listener
        // is re-attached, which takes longer than a bare first load.
        await page.WaitForConditionAsync(
            $"() => window.__scrollListenerReady?.['{triggerId}'] === true && document.getElementById('{triggerId}') !== null",
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });

        // On each poll: scroll to bottom and dispatch a synthetic scroll event.
        // Headless Chrome does not fire scroll events from programmatic scrollTo,
        // so the explicit dispatchEvent is required for the scroll-manager.js handler
        // to detect the trigger element's position via getBoundingClientRect().
        // Uses E2ETimeout: loading the next batch requires an API round-trip.
        await page.WaitForFunctionAsync(
            @"(expectedCount) => {
                window.scrollTo({ top: document.documentElement.scrollHeight, behavior: 'instant' });
                window.dispatchEvent(new Event('scroll'));
                return document.querySelectorAll('" + itemSelector + @"').length >= expectedCount;
            }",
            expectedItemCount,
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });
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
    /// <param name="triggerId">The id of the scroll-trigger element used by scroll-manager.js</param>
    public static async Task ScrollToEndOfContentAsync(
        this IPage page,
        string endSelector = ".end-of-content",
        string triggerId = "scroll-trigger")
    {
        // Wait for EITHER:
        // 1. The end-of-content marker already present (small collection, all items in first batch,
        //    no scroll trigger was ever rendered, so no scroll listener exists), OR
        // 2. The scroll listener to be attached AND trigger element to exist in the DOM
        //    (large collection, multiple batches needed). Checking both the readiness flag
        //    AND the DOM element guards against stale readiness — see ScrollToLoadMoreAsync.
        await page.WaitForConditionAsync(
            $"() => document.querySelector('{endSelector}') !== null || (window.__scrollListenerReady?.['{triggerId}'] === true && document.getElementById('{triggerId}') !== null)",
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });

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
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });
    }

    /// <summary>
    /// Gets the current scroll listener version for a trigger element.
    /// Use this before performing an action that will re-attach the scroll listener
    /// (e.g., applying a tag filter), then pass the returned version to
    /// <see cref="WaitForScrollListenerReattachAsync"/> to wait for a fresh attachment.
    ///
    /// The version counter is incremented each time <c>observeScrollTrigger()</c> runs
    /// in scroll-manager.js. It is never reset, so comparing before/after values
    /// reliably detects whether a new listener was attached.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="triggerId">The scroll trigger element ID (default: "scroll-trigger")</param>
    /// <returns>Current version number (0 if no listener has ever been attached)</returns>
    public static async Task<int> GetScrollListenerVersionAsync(
        this IPage page,
        string triggerId = "scroll-trigger")
    {
        return await page.EvaluateAsync<int>(
            $"() => window.__scrollListenerVersion?.['{triggerId}'] || 0");
    }

    /// <summary>
    /// Waits for the scroll listener to be freshly re-attached after an action
    /// (tag filter, collection switch, etc.) by detecting that the version counter
    /// has incremented beyond the <paramref name="previousVersion"/>.
    ///
    /// This is immune to the stale-readiness race condition because the version
    /// counter only increases — it can never be stale from a previous load cycle.
    ///
    /// Example:
    ///   var version = await page.GetScrollListenerVersionAsync();
    ///   await tagButton.ClickAndExpectAsync(async () =>
    ///       await Assertions.Expect(Page).ToHaveURLAsync(new Regex(@".*tags=.*"), new() { Timeout = 2000 }));
    ///   await page.WaitForScrollListenerReattachAsync(version);
    ///   await page.ScrollToLoadMoreAsync(expectedItemCount: 21);
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="previousVersion">Version from <see cref="GetScrollListenerVersionAsync"/> before the action</param>
    /// <param name="triggerId">The scroll trigger element ID (default: "scroll-trigger")</param>
    public static async Task WaitForScrollListenerReattachAsync(
        this IPage page,
        int previousVersion,
        string triggerId = "scroll-trigger")
    {
        await page.WaitForConditionAsync(
            $"(prevVer) => (window.__scrollListenerVersion?.['{triggerId}'] || 0) > prevVer",
            previousVersion,
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });
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
        page.SetDefaultTimeout(E2ETimeout);
        page.SetDefaultNavigationTimeout(E2ETimeout);
        return page;
    }

    // ============================================================================
    // API REQUEST HELPERS
    // ============================================================================
    // NOTE: Page.APIRequest.GetAsync() does NOT inherit the page's SetDefaultTimeout().
    // The Playwright APIRequestContext has its own fixed 30s default. Always use
    // this helper instead of calling Page.APIRequest.GetAsync() directly.
    // ============================================================================

    /// <summary>
    /// Sends a GET request via Playwright's API request context with the standard
    /// E2E timeout. Use this instead of <c>Page.APIRequest.GetAsync()</c> directly
    /// because <c>page.SetDefaultTimeout()</c> does NOT apply to API requests.
    /// </summary>
    public static Task<IAPIResponse> APIGetAsync(this IPage page, string url) =>
        page.APIRequest.GetAsync(url, new APIRequestContextOptions { Timeout = E2ETimeout });

    // ============================================================================
    // GENERIC ELEMENT ASSERTIONS
    // ============================================================================

    /// <summary>
    /// Asserts that an element is visible.
    /// Generic helper for any element visibility check.
    /// </summary>
    /// <param name="locator">The element to check</param>
    public static async Task AssertElementVisibleAsync(
        this ILocator locator)
    {
        await Assertions.Expect(locator)
            .ToBeVisibleAsync(new() { Timeout = E2ETimeout });
    }

    /// <summary>
    /// Asserts that an element has a specific CSS class.
    /// Supports both exact string matching and regex patterns.
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="className">The exact class name to check for</param>
    public static async Task AssertHasClassAsync(
        this ILocator locator,
        string className)
    {
        await Assertions.Expect(locator)
            .ToHaveClassAsync(className, new() { Timeout = E2ETimeout });
    }

    /// <summary>
    /// Asserts that an element has a CSS class matching a regex pattern.
    /// Useful for checking dynamic or partial class names.
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="pattern">The regex pattern to match against the class attribute</param>
    public static async Task AssertHasClassAsync(
        this ILocator locator,
        System.Text.RegularExpressions.Regex pattern)
    {
        await Assertions.Expect(locator)
            .ToHaveClassAsync(pattern, new() { Timeout = E2ETimeout });
    }

    /// <summary>
    /// Asserts that a locator matches a specific number of elements.
    /// Uses Playwright's auto-retrying assertion mechanism.
    /// </summary>
    /// <param name="locator">The locator to count</param>
    /// <param name="expectedCount">The expected number of matching elements</param>
    public static async Task AssertCountAsync(
        this ILocator locator,
        int expectedCount)
    {
        await Assertions.Expect(locator)
            .ToHaveCountAsync(expectedCount, new() { Timeout = E2ETimeout });
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
        gotoOptions.Timeout ??= E2ETimeout;

        // Capture counter before navigation — full page load resets JS state,
        // so counter will restart from 0. We wait for it to become > 0.
        await page.GotoAsync(url, gotoOptions);

        // Wait for Blazor to be fully interactive (circuit established, scripts loaded).
        // WaitForBlazorReadyAsync correctly waits for __blazorServerReady/__blazorWasmReady
        // instead of the weaker __blazorWebReady which fires before the interactive circuit.
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
        string? baseUrl = null,
        PageGotoOptions? options = null)
    {
        var fullUrl = $"{baseUrl ?? BaseUrl}{relativeUrl}";
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
    /// INTERNAL USE: Called automatically by GotoAndWaitForBlazorAsync and ClickAndExpectAsync.
    /// </summary>
    public static async Task WaitForBlazorReadyAsync(this IPage page)
    {
        try
        {
            // Single combined check for all readiness conditions INCLUDING Mermaid diagrams:
            // 1. Blazor runtime exists
            // 2. Interactive runtime is ready (Server/WASM circuit established)
            // 3. Page scripts finished loading (mermaid, highlight.js, custom-pages, etc.)
            // Previously these were 3 separate WaitForConditionAsync calls. Combining into one
            // eliminates ~100-200ms overhead per navigation across 200+ navigations in the E2E suite.
            // Mermaid readiness is covered by __scriptsReady: markScriptsReady() defers until
            // __mermaidReady is true (set by initMermaid on all exit paths).
            await page.WaitForConditionAsync(@"
                () => {
                    // Step 1: Blazor runtime must exist
                    if (typeof window.Blazor === 'undefined') return false;

                    // Step 2: Interactive Server/WASM circuit must be established.
                    // __blazorServerReady is set by afterServerStarted() — SignalR circuit ready, event handlers attached.
                    // __blazorWasmReady  is set by afterWebAssemblyStarted() — WASM runtime ready.
                    // NOTE: __blazorWebReady (afterWebStarted) fires too early — before the interactive circuit
                    // is established — and must NOT be used here to avoid returning before @onclick handlers attach.
                    if (window.__blazorServerReady !== true && window.__blazorWasmReady !== true) {
                        return false;
                    }

                    // Step 3: Page scripts must be ready (or not applicable).
                    // __scriptsReady is set false by navigation handlers when scripts start loading,
                    // and set true by markScriptsReady() when ALL component flags are confirmed
                    // (scroll listener, date-range slider, mermaid). Only block if explicitly
                    // false (loading). If undefined (initial page load or page with no scripts),
                    // proceed immediately.
                    if (window.__scriptsReady === false) return false;

                    return true;
                }
            ", new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });
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
    /// Clicks an element and retries the [click + assertion] block until the assertion
    /// passes or the total timeout expires. The .NET equivalent of Playwright's JS
    /// <c>expect(fn).toPass()</c> pattern.
    ///
    /// <b>This is the single canonical click helper.</b> Use it for every Blazor click —
    /// whether the result is a URL change, a CSS class toggle, an element appearing, or
    /// anything else. Supply the assertion that describes what success looks like.
    ///
    /// <b>Why retry the whole block?</b> Under Blazor Server hydration, a click may be
    /// silently lost if the @onclick handler hasn't attached yet when the click fires.
    /// Retrying only the assertion will never succeed because the DOM won't change
    /// without a click. Retrying [click + assert] eventually lands a click on a fully-
    /// hydrated component. The same pattern fixes JS-driven interactions (custom-pages.js)
    /// where <c>data-initialized</c> may not yet be set when the first click fires.
    ///
    /// <b>After the assertion passes</b>, if the URL changed, <see cref="WaitForBlazorReadyAsync"/>
    /// is called automatically so callers don't need to wait for Blazor hydration on the
    /// new page themselves.
    ///
    /// Use short assertion timeouts (1-3s) inside the lambda so failed attempts are
    /// detected quickly. The outer <paramref name="totalTimeoutMs"/> is the overall budget
    /// across all retries.
    ///
    /// Example — state change (no URL change):
    /// <code>
    /// await toggle.ClickAndExpectAsync(async () =>
    ///     await Assertions.Expect(html).ToHaveClassAsync(
    ///         new Regex("sidebar-collapsed"), new() { Timeout = 2000 }));
    /// </code>
    ///
    /// Example — navigation (URL changes):
    /// <code>
    /// await blogsButton.ClickAndExpectAsync(async () =>
    ///     await Assertions.Expect(Page).ToHaveURLAsync(
    ///         new Regex(@".*/ai/blogs.*"), new() { Timeout = 2000 }));
    /// </code>
    /// </summary>
    public static async Task ClickAndExpectAsync(
        this ILocator locator,
        Func<Task> assertion,
        int totalTimeoutMs = -1)
    {
        if (totalTimeoutMs < 0) totalTimeoutMs = E2ETimeout;
        var page = locator.Page;
        var urlBefore = page.Url;

        await locator.WaitForBlazorInteractivityAsync();
        await RetryUntilPassAsync(async () =>
        {
            await locator.ClickAsync(new() { Timeout = 2000 });
            await assertion();
        }, totalTimeoutMs);

        // If a navigation happened, wait for Blazor to be ready on the new page
        // (SignalR circuit re-established, page scripts loaded).
        if (page.Url != urlBefore)
        {
            await page.WaitForBlazorReadyAsync();
        }
    }

    /// <summary>
    /// Waits for a Blazor element to be fully interactive and actionable.
    /// Use this before custom interactions that don't use ClickAndExpectAsync.
    /// </summary>
    public static async Task WaitForBlazorInteractivityAsync(
        this ILocator locator)
    {
        // Wait for element to be visible using our centralized helper
        await locator.AssertElementVisibleAsync();

        // Ensure page is interactive (server circuit established, scripts loaded).
        // WaitForBlazorReadyAsync waits for __blazorServerReady/__blazorWasmReady.
        await locator.Page.WaitForBlazorReadyAsync();
    }

    /// <summary>
    /// Fills a Blazor input element and waits for a URL query parameter to appear.
    ///
    /// Handles the Blazor Server race condition where <c>FillAsync</c> dispatches an
    /// <c>input</c> event before Blazor finishes attaching <c>@oninput</c> handlers
    /// to the DOM. The SignalR circuit may be established (<c>__blazorServerReady</c>)
    /// while handler attachment is still in progress.
    ///
    /// <b>How it works</b>:
    /// <list type="number">
    ///   <item>Waits for Blazor interactivity (element visible + circuit ready)</item>
    ///   <item>Fills the input value via Playwright</item>
    ///   <item>Polls for the expected URL query parameter, re-dispatching the
    ///         <c>input</c> event at ≥1 s intervals if the parameter hasn't appeared.
    ///         Retries are spaced &gt;1 s apart so the 300 ms debounce timer can fire
    ///         between them.</item>
    /// </list>
    ///
    /// This mirrors the pattern used by <see cref="ScrollToLoadMoreAsync"/> which
    /// re-dispatches scroll events on each poll iteration.
    /// </summary>
    /// <param name="locator">The search input element locator</param>
    /// <param name="value">The text to fill into the input</param>
    /// <param name="urlQueryParam">The URL query parameter name to wait for (default: "search")</param>
    public static async Task FillBlazorInputAsync(
        this ILocator locator,
        string value,
        string urlQueryParam = "search")
    {
        // Step 1: Ensure element is visible and Blazor circuit is ready
        await locator.WaitForBlazorInteractivityAsync();

        // Step 2: Fill the input value
        await locator.FillAsync(value);

        // Step 3: Wait for URL query parameter, re-dispatching input event if needed.
        // The __fillRetryTs variable is scoped per page lifetime; the retry interval
        // of 1 s ensures the 300 ms debounce timer fires between dispatches.
        //
        // CI RACE: Blazor may re-render SidebarSearch between steps 2 and 3, running
        // OnParametersSet which resets _searchQueryInternal (and therefore input.value)
        // to the current URL-bound parameter (empty). When that happens, the original
        // fill's debounce fires with "" and the URL never updates.
        //
        // The retry uses the native HTMLInputElement value setter (bypasses Blazor's
        // virtual-DOM diffing) to restore the value before re-dispatching, so that
        // Blazor's @oninput handler receives the correct value even after a reset.
        // The JS value escaping is safe because `value` is a test-supplied literal.
        var escapedValue = value.Replace("\\", "\\\\").Replace("'", "\\'");
        var inputSelector = await locator.EvaluateAsync<string>(
            "el => el.tagName.toLowerCase() + (el.type ? '[type=' + el.type + ']' : '')");
        await locator.Page.WaitForConditionAsync($@"
            () => {{
                if (window.location.href.includes('{urlQueryParam}=')) return true;
                const now = Date.now();
                if (!window.__fillRetryTs || (now - window.__fillRetryTs > 1000)) {{
                    window.__fillRetryTs = now;
                    // Find the VISIBLE input matching the selector. SidebarSearch may
                    // render in both desktop sidebar and mobile panel — querySelector
                    // returns the first in DOM order which may be the hidden mobile one.
                    const inputs = document.querySelectorAll('{inputSelector}');
                    let input = null;
                    for (const inp of inputs) {{
                        if (inp.offsetParent !== null || inp.getClientRects().length > 0) {{
                            input = inp;
                            break;
                        }}
                    }}
                    if (!input && inputs.length > 0) input = inputs[0];
                    if (input) {{
                        // Re-fill via native setter if Blazor re-render reset the value.
                        // This bypasses Blazor's virtual DOM so the DOM value is correct
                        // when the @oninput handler reads e.target.value.
                        if (input.value !== '{escapedValue}') {{
                            const setter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value')?.set;
                            if (setter) setter.call(input, '{escapedValue}');
                        }}
                        input.dispatchEvent(new Event('input', {{ bubbles: true }}));
                    }}
                }}
                return false;
            }}",
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });
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
    public static async Task WaitForBlazorUrlContainsAsync(
        this IPage page,
        string urlSegment)
    {
        // Use Playwright's auto-retrying Expect assertion with regex - much cleaner!
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex($".*{System.Text.RegularExpressions.Regex.Escape(urlSegment)}.*"),
            new() { Timeout = E2ETimeout }
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
        string selector)
    {
        await page.Locator(selector).AssertElementVisibleAsync();
    }

    // ============================================================================
    // CENTRALIZED TIMEOUT METHODS - Single point of control
    // ============================================================================

    /// <summary>
    /// Waits for a selector with standard timeout.
    /// Centralized timeout management - change E2ETimeout to affect all tests.
    /// </summary>
    public static async Task<ILocator> WaitForSelectorWithTimeoutAsync(
        this IPage page,
        string selector,
        PageWaitForSelectorOptions? options = null)
    {
        var opts = options ?? new PageWaitForSelectorOptions();
        opts.Timeout ??= E2ETimeout;
        opts.State ??= WaitForSelectorState.Visible;

        await page.WaitForSelectorAsync(selector, opts);
        return page.Locator(selector);
    }

    /// <summary>
    /// Gets text content with standard timeout.
    /// Centralized timeout management - change E2ETimeout to affect all tests.
    /// </summary>
    public static Task<string?> TextContentWithTimeoutAsync(
        this ILocator locator,
        LocatorTextContentOptions? options = null)
    {
        var opts = options ?? new LocatorTextContentOptions();
        opts.Timeout ??= E2ETimeout;
        return locator.TextContentAsync(opts);
    }

    /// <summary>
    /// Waits for URL with standard timeout (glob pattern).
    /// Centralized timeout management.
    /// </summary>
    /// <param name="page">The page to observe</param>
    /// <param name="urlPattern">Glob pattern the URL must match</param>
    /// <param name="options">Optional Playwright wait options</param>
    /// <param name="onTimeout">Optional JavaScript expression evaluated when the wait times out.
    /// Its result is appended to the timeout message for diagnostics.</param>
    public static async Task WaitForURLWithTimeoutAsync(
        this IPage page,
        string urlPattern,
        PageWaitForURLOptions? options = null,
        string? onTimeout = null)
    {
        var opts = options ?? new PageWaitForURLOptions();
        opts.Timeout ??= E2ETimeout;
        try
        {
            await page.WaitForURLAsync(urlPattern, opts);
        }
        catch (TimeoutException ex) when (onTimeout != null)
        {
            var diag = await page.EvaluateAsync<string>($"() => String({onTimeout}())");
            throw new TimeoutException($"{ex.Message}\nDiagnostics: {diag}", ex);
        }
    }

    /// <summary>
    /// Gets the href attribute value from a locator.
    /// Common pattern: Extract href from links for validation or navigation.
    /// </summary>
    /// <param name="locator">The element to get href from</param>
    /// <returns>The href attribute value, or null if not found</returns>
    public static async Task<string?> GetHrefAsync(
        this ILocator locator)
    {
        return await locator.GetAttributeAsync("href", new() { Timeout = E2ETimeout });
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
    public static async Task AssertElementVisibleByRoleAsync(
        this IPage page,
        AriaRole role,
        string name,
        int? level = null)
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
        await Assertions.Expect(element).ToBeVisibleAsync(new() { Timeout = E2ETimeout });
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
    public static async Task AssertElementVisibleByRoleAsync(
        this ILocator locator,
        AriaRole role,
        string name,
        int? level = null)
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
        await Assertions.Expect(element).ToBeVisibleAsync(new() { Timeout = E2ETimeout });
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
    public static async Task AssertElementVisibleByAltTextAsync(
        this IPage page,
        string altText)
    {
        var element = page.GetByAltText(altText, new PageGetByAltTextOptions { Exact = true });
        await Assertions.Expect(element).ToBeVisibleAsync(new() { Timeout = E2ETimeout });
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
    public static async Task AssertElementVisibleBySelectorAsync(
        this IPage page,
        string selector)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToBeVisibleAsync(new() { Timeout = E2ETimeout });
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
    public static async Task AssertElementContainsTextBySelectorAsync(
        this IPage page,
        string selector,
        string expectedText)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToContainTextAsync(expectedText, new() { Timeout = E2ETimeout });
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
    public static async Task AssertElementCountBySelectorAsync(
        this IPage page,
        string selector,
        int expectedCount)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToHaveCountAsync(expectedCount, new() { Timeout = E2ETimeout });
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
    /// <returns>Attribute value or null</returns>
    public static async Task<string?> GetElementAttributeBySelectorAsync(
        this IPage page,
        string selector,
        string attributeName)
    {
        return await page.Locator(selector).GetAttributeAsync(attributeName, new() { Timeout = E2ETimeout });
    }

    /// <summary>
    /// Gets an element's text content by selector.
    ///
    /// Example:
    ///   var heading = await page.GetElementTextBySelectorAsync("h1");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <returns>Text content or null</returns>
    public static async Task<string?> GetElementTextBySelectorAsync(
        this IPage page,
        string selector)
    {
        return await page.Locator(selector).TextContentAsync(new() { Timeout = E2ETimeout });
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
    public static async Task AssertUrlEndsWithAsync(
        this IPage page,
        string urlSegment)
    {
        await Assertions.Expect(page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex($".*{System.Text.RegularExpressions.Regex.Escape(urlSegment)}$"),
            new() { Timeout = E2ETimeout }
        );
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
    public static async Task AssertHrefEqualsAsync(
        this ILocator locator,
        string expectedHref)
    {
        await Assertions.Expect(locator)
            .ToHaveAttributeAsync("href", expectedHref, new() { Timeout = E2ETimeout });
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
    ///   await anchorLink.ClickAndWaitForScrollAsync();
    /// </summary>
    /// <param name="locator">The element to click</param>
    /// <returns>Task that completes when scrolling finishes and TOC updates, or timeout is reached</returns>
    public static async Task ClickAndWaitForScrollAsync(
        this ILocator locator)
    {
        var page = locator.Page;
        var counterBefore = await page.GetE2ECounterAsync();

        // Click the element (triggers anchor navigation / scrolling).
        // Do NOT use Force = true — let Playwright wait for the element to be visible
        // and actionable, which naturally handles slow rendering (e.g. slow3g).
        await locator.ClickAsync();

        // Wait for scroll-end signal (from scrollend event listener in App.razor)
        await page.WaitForE2ESignalAsync(counterBefore, "scroll-end");

        // Wait for TOC scroll-spy to update active heading (if TOC is present)
        // This is safe to call even without a TOC — WaitForE2ESignalAsync will
        // succeed immediately if the signal already appeared, or the scroll-end
        // gives the spy time to fire via rAF before the next assertion.
    }

    /// <summary>
    /// Waits for the page layout to stop changing — i.e., no DOM mutations or resize
    /// events for <paramref name="debounceMs"/> milliseconds.
    ///
    /// Uses the same ResizeObserver + MutationObserver debounce pattern as the
    /// <c>restoreScrollPosition</c> deferred path in scroll-manager.js: both observers
    /// share a single debounce timer that resets on every change. Once the page has
    /// been quiet for <paramref name="debounceMs"/> ms the returned task completes.
    ///
    /// Use this after an action that triggers async layout shifts (lazy-loading images,
    /// Mermaid diagrams, Blazor re-renders) before asserting element positions.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="debounceMs">Silence window in ms before resolving (default 150)</param>
    /// <param name="deadlineMs">Hard timeout in ms (default 5000)</param>
    public static Task WaitForLayoutSettledAsync(
        this IPage page,
        int debounceMs = 150,
        int deadlineMs = 5000) =>
        page.EvaluateAsync(@"
            ([debounce, deadline]) => new Promise((resolve) => {
                let timer = null;
                function reset() {
                    if (timer !== null) clearTimeout(timer);
                    timer = setTimeout(finish, debounce);
                }
                function finish() {
                    ro.disconnect();
                    mo.disconnect();
                    clearTimeout(deadlineTimer);
                    resolve();
                }
                const ro = new ResizeObserver(reset);
                const mo = new MutationObserver(reset);
                const deadlineTimer = setTimeout(finish, deadline);
                ro.observe(document.documentElement);
                mo.observe(document.body, { childList: true, subtree: true });
                // Kick off the initial debounce — if nothing changes it resolves naturally.
                reset();
            })",
            new[] { debounceMs, deadlineMs });

    /// <summary>
    /// Scrolls the page programmatically and waits for the TOC scroll-spy to update.
    /// Replaces the 3-line manual scroll + wait pattern in TOC tests.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="targetY">The vertical scroll position in pixels</param>
    public static async Task ScrollToAndWaitForTocUpdateAsync(
        this IPage page,
        double targetY)
    {
        var counterBefore = await page.GetE2ECounterAsync();
        await page.EvaluateAsync("top => window.scrollTo({ top, behavior: 'instant' })", targetY);
        await page.WaitForE2ESignalAsync(counterBefore, "toc-active-updated");
    }

    /// <summary>
    /// Waits for the TOC scroll-spy to be initialized.
    /// Replaces manual WaitForConditionAsync("toc._tocScrollSpy.initialized") calls.
    /// </summary>
    public static async Task WaitForTocInitializedAsync(this IPage page) =>
        await page.WaitForConditionAsync(
            @"() => {
                const e = window.__e2e;
                return e && e.history && e.history.some(h => h.label === 'toc-initialized');
            }",
            new PageWaitForFunctionOptions { Timeout = E2ETimeout, PollingInterval = E2EPollingInterval });

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

    /// <summary>
    /// Clicks a visible card link using JavaScript's native <c>.click()</c> instead of
    /// Playwright's <c>ClickAsync</c>.
    ///
    /// Playwright's <c>ClickAsync</c> calls <c>scrollIntoViewIfNeeded</c> before clicking,
    /// which fires a scroll event that can overwrite a saved scroll position in
    /// <c>scroll-manager.js</c>. Using JS <c>.click()</c> dispatches the click event
    /// directly — Blazor's router intercepts it for enhanced (SPA-style) navigation
    /// without any side-effect scrolling.
    ///
    /// The helper selects the first card link whose bounding rect is within the current
    /// viewport, falling back to index 0 if none is found.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="cardLinkSelector">CSS selector for the card links (default: ".card-link")</param>
    public static async Task ClickVisibleCardLinkAsync(
        this IPage page,
        string cardLinkSelector = ".card-link")
    {
        var visibleCardIndex = await page.EvaluateAsync<int>($@"() => {{
            const links = document.querySelectorAll('{cardLinkSelector}');
            for (let i = 0; i < links.length; i++) {{
                const rect = links[i].getBoundingClientRect();
                if (rect.top >= 0 && rect.top < window.innerHeight) return i;
            }}
            return 0;
        }}");
        await page.EvaluateAsync(
            "(idx) => document.querySelectorAll('" + cardLinkSelector + "')[idx].click()",
            visibleCardIndex);
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

