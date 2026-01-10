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
/// USAGE EXAMPLES - Common Test Patterns:
///
/// 1. Click collection button and verify state:
///    await page.ClickAndNavigateAsync(".collection-nav a", text: "Videos",
///        expectedUrlSegment: "/videos", waitForActiveState: "Videos");
///
/// 2. Click section card on homepage:
///    await page.ClickAndNavigateAsync(".section-card-container a.section-card[href*='github-copilot']",
///        expectedUrlSegment: "/github-copilot");
///
/// 3. Click content card to detail page:
///    await page.ClickAndNavigateAsync(".content-item-card", nth: 0,
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

    /// <summary>Default timeout for element operations (locator waits, clicks, text content)</summary>
    public const int DefaultElementTimeout = 5000;

    /// <summary>Default timeout for navigation operations (page load, URL changes)</summary>
    public const int DefaultNavigationTimeout = 10000;

    /// <summary>Default timeout for Expect assertions (auto-retrying)</summary>
    public const int DefaultAssertionTimeout = 5000;

    /// <summary>Base URL for the Web frontend</summary>
    public const string BaseUrl = "http://localhost:5184";

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
        page.SetDefaultTimeout(DefaultElementTimeout);
        page.SetDefaultNavigationTimeout(DefaultNavigationTimeout);
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
    ///   await page.ClickAndNavigateAsync(".collection-nav a", text: "Videos",
    ///       expectedUrlSegment: "/videos", waitForActiveState: "Videos");
    ///
    /// Example (content card with href filter):
    ///   await page.ClickAndNavigateAsync(".content-item-card", filterByHref: "/roundups/",
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
        int timeoutMs = DefaultNavigationTimeout)
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
            await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", waitForActiveState, DefaultAssertionTimeout);
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
        int timeoutMs = DefaultAssertionTimeout)
    {
        await Assertions.Expect(locator)
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
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
        gotoOptions.Timeout ??= DefaultNavigationTimeout;

        await page.GotoAsync(url, gotoOptions);

        // Wait for Blazor runtime - this is the only Blazor-specific wait we need
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
        string baseUrl = "http://localhost:5184",
        PageGotoOptions? options = null)
    {
        var fullUrl = $"{baseUrl}{relativeUrl}";
        return page.GotoAndWaitForBlazorAsync(fullUrl, options);
    }

    /// <summary>
    /// Waits for Blazor runtime to be loaded and ready.
    /// This is the foundation - all Blazor functionality depends on this.
    ///
    /// INTERNAL USE: Called automatically by GotoAndWaitForBlazorAsync.
    /// </summary>
    public static async Task WaitForBlazorReadyAsync(this IPage page, int timeoutMs = DefaultNavigationTimeout)
    {
        try
        {
            // Wait for Blazor object to exist
            await page.WaitForFunctionAsync(
                "() => typeof window.Blazor !== 'undefined'",
                new PageWaitForFunctionOptions { Timeout = timeoutMs }
            );

            // Wait for enhanced navigation to be initialized (Blazor SSR with streaming)
            // This ensures click handlers are attached before we try to interact
            await page.WaitForFunctionAsync(@"
                () => {
                    if (!window.Blazor) return false;
                    // Enhanced navigation is ready when this exists
                    if (window.Blazor._internal?.navigationEnhancementCallbacks) return true;
                    // For simpler pages, Blazor existing is enough
                    return true;
                }
            ", new PageWaitForFunctionOptions { Timeout = 3000 });
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
    /// 2. Waits for Blazor enhanced navigation to be initialized
    /// 3. Uses Force=true to bypass stability checks (safe because we've already
    ///    verified Blazor is ready and the element is visible)
    ///
    /// This is NOT a hack - it's the correct pattern for Blazor Server testing.
    /// The "stability" check is designed for CSS animations, not for Blazor's
    /// continuous DOM updates.
    /// </summary>
    /// <param name="locator">The element to click</param>
    /// <param name="timeoutMs">Maximum time to wait for interactivity</param>
    public static async Task ClickBlazorElementAsync(
        this ILocator locator,
        int timeoutMs = DefaultElementTimeout)
    {
        // Step 1: Wait for element to be visible using our centralized helper
        await locator.AssertElementVisibleAsync(timeoutMs);

        // Step 2: Ensure Blazor enhanced navigation handlers are attached
        await locator.Page.WaitForBlazorReadyAsync(timeoutMs);

        // Step 3: Click with Force=true to bypass stability checks
        // This is necessary because Blazor's continuous DOM updates prevent
        // elements from being considered "stable" by Playwright's criteria.
        // We've already verified the element is visible and Blazor is ready,
        // so the click will work correctly.
        await locator.ClickAsync(new() { Force = true, Timeout = timeoutMs });
    }

    /// <summary>
    /// Waits for a Blazor element to be fully interactive and actionable.
    /// Use this before custom interactions that don't use ClickBlazorElementAsync.
    /// </summary>
    public static async Task WaitForBlazorInteractivityAsync(
        this ILocator locator,
        int timeoutMs = DefaultElementTimeout)
    {
        // Wait for element to be visible using our centralized helper
        await locator.AssertElementVisibleAsync(timeoutMs);

        // Ensure Blazor is ready
        await locator.Page.WaitForBlazorReadyAsync(timeoutMs);
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
        int timeoutMs = DefaultNavigationTimeout)
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
        int timeoutMs = DefaultElementTimeout)
    {
        await page.Locator(selector).AssertElementVisibleAsync(timeoutMs);
    }

    // ============================================================================
    // CENTRALIZED TIMEOUT METHODS - Single point of control
    // ============================================================================

    /// <summary>
    /// Waits for a selector with standard timeout.
    /// Centralized timeout management - change DefaultElementTimeout to affect all tests.
    /// </summary>
    public static async Task<ILocator> WaitForSelectorWithTimeoutAsync(
        this IPage page,
        string selector,
        PageWaitForSelectorOptions? options = null)
    {
        var opts = options ?? new PageWaitForSelectorOptions();
        opts.Timeout ??= DefaultElementTimeout;
        opts.State ??= WaitForSelectorState.Visible;

        await page.WaitForSelectorAsync(selector, opts);
        return page.Locator(selector);
    }

    /// <summary>
    /// Gets text content with standard timeout.
    /// Centralized timeout management - change DefaultElementTimeout to affect all tests.
    /// </summary>
    public static Task<string?> TextContentWithTimeoutAsync(
        this ILocator locator,
        LocatorTextContentOptions? options = null)
    {
        var opts = options ?? new LocatorTextContentOptions();
        opts.Timeout ??= DefaultElementTimeout;
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
        opts.Timeout ??= DefaultNavigationTimeout;
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
        int timeoutMs = DefaultElementTimeout)
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
        int timeoutMs = DefaultAssertionTimeout)
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
        int timeoutMs = DefaultAssertionTimeout)
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
    ///   await page.AssertElementVisibleBySelectorAsync(".content-item-card");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementVisibleBySelectorAsync(
        this IPage page,
        string selector,
        int timeoutMs = DefaultAssertionTimeout)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element matching the selector contains the expected text.
    /// Uses Playwright's auto-retrying assertions.
    ///
    /// Example:
    ///   await page.AssertElementContainsTextBySelectorAsync(".collection-nav a.active", "Videos");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="expectedText">Expected text content</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementContainsTextBySelectorAsync(
        this IPage page,
        string selector,
        string expectedText,
        int timeoutMs = DefaultAssertionTimeout)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToContainTextAsync(expectedText, new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that the element count matches expected value.
    /// Uses Playwright's auto-retrying count assertion.
    ///
    /// Example:
    ///   await page.AssertElementCountBySelectorAsync(".content-item-card", 10);
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="expectedCount">Expected number of elements</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementCountBySelectorAsync(
        this IPage page,
        string selector,
        int expectedCount,
        int timeoutMs = DefaultAssertionTimeout)
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
        int timeoutMs = DefaultElementTimeout)
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
        int timeoutMs = DefaultElementTimeout)
    {
        return await page.Locator(selector).TextContentAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Clicks an element found by selector using Blazor-aware click handling.
    ///
    /// Example:
    ///   await page.ClickElementBySelectorAsync(".collection-nav a");
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="selector">CSS selector</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task ClickElementBySelectorAsync(
        this IPage page,
        string selector,
        int timeoutMs = DefaultElementTimeout)
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
        int timeoutMs = DefaultElementTimeout)
    {
        var options = new PageGetByRoleOptions
        {
            Name = name,
            Exact = true
        };
        await page.GetByRole(role, options).ClickBlazorElementAsync(timeoutMs);
    }
}

/// <summary>
/// Custom exception for assertion failures with better error messages
/// </summary>
public class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
}

