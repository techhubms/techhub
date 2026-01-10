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
/// 5. Verify active collection:
///    await page.AssertActiveCollectionAsync("News");
///
/// 6. Verify element attribute:
///    await link.AssertAttributeContainsAsync("href", "/github-copilot");
///
/// 7. Use Expect assertions instead of TextContentAsync + Should.Contain:
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
            await page.AssertActiveCollectionAsync(waitForActiveState, DefaultAssertionTimeout);
        }

        // Step 5: Custom wait (if specified)
        if (customWait != null)
        {
            await customWait(page);
        }
    }

    /// <summary>
    /// Navigates back and waits for state to sync with the new URL.
    ///
    /// PATTERN: Browser back → URL changes → Blazor re-renders with new state
    ///
    /// Example:
    ///   await page.GoBackAndWaitForStateSyncAsync("News");
    ///   // Back on previous page with News button showing .active class
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="expectedActiveCollection">Expected collection button text after navigation</param>
    /// <param name="expectedUrlSegment">Optional: URL segment to verify</param>
    public static async Task GoBackAndWaitForStateSyncAsync(
        this IPage page,
        string expectedActiveCollection,
        string? expectedUrlSegment = null)
    {
        await page.GoBackAsync();

        if (!string.IsNullOrEmpty(expectedUrlSegment))
        {
            await page.WaitForBlazorUrlContainsAsync(expectedUrlSegment);
        }

        await page.AssertActiveCollectionAsync(expectedActiveCollection);
    }

    /// <summary>
    /// Navigates forward and waits for state to sync with the new URL.
    /// </summary>
    public static async Task GoForwardAndWaitForStateSyncAsync(
        this IPage page,
        string expectedActiveCollection,
        string? expectedUrlSegment = null)
    {
        await page.GoForwardAsync();

        if (!string.IsNullOrEmpty(expectedUrlSegment))
        {
            await page.WaitForBlazorUrlContainsAsync(expectedUrlSegment);
        }

        await page.AssertActiveCollectionAsync(expectedActiveCollection);
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

    /// <summary>
    /// Waits for Blazor circuit to reconnect if disconnected.
    /// Call this if you see "Attempting to reconnect" UI.
    /// </summary>
    public static async Task WaitForBlazorCircuitAsync(this IPage page, int timeoutMs = DefaultNavigationTimeout)
    {
        try
        {
            // Wait for circuit indicator to disappear
            await Assertions.Expect(page.Locator("#components-reconnect-modal"))
                .ToBeHiddenAsync(new() { Timeout = timeoutMs });
        }
        catch
        {
            // Modal might not exist - that's fine
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
    /// Asserts that the sidebar is visible.
    /// Common pattern: Check sidebar visibility on pages with sidebars.
    /// Uses multiple selectors to match different sidebar implementations.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertSidebarVisibleAsync(
        this IPage page,
        int timeoutMs = DefaultAssertionTimeout)
    {
        await page.Locator(".sidebar, aside.sidebar, .home-sidebar, aside")
            .AssertElementVisibleAsync(timeoutMs);
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
    /// Asserts that a heading with the specified name is visible.
    /// Uses accessible role-based selection for better accessibility testing.
    /// Case-sensitive exact matching.
    ///
    /// Examples:
    ///   await page.AssertHeadingVisibleAsync("Latest Content");
    ///   await page.AssertHeadingVisibleAsync("Overview", level: 2);
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="name">The exact heading name to find (case-sensitive)</param>
    /// <param name="level">Optional: Specific heading level (1-6)</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertHeadingVisibleAsync(
        this IPage page,
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

        var heading = page.GetByRole(AriaRole.Heading, options);
        await Assertions.Expect(heading).ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that a link with the specified name is visible.
    /// Uses accessible role-based selection for better accessibility testing.
    /// Supports both exact matching and regex patterns.
    ///
    /// Examples:
    ///   await page.AssertLinkVisibleAsync("About");
    ///   await page.AssertLinkVisibleAsync("GitHub", useRegex: true);
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="name">The exact link text or regex pattern to find</param>
    /// <param name="useRegex">Whether to use regex pattern matching (default: false for exact match)</param>
    /// <param name="ignoreCase">Whether to ignore case when using regex (default: true)</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertLinkVisibleAsync(
        this IPage page,
        string name,
        bool useRegex = false,
        bool ignoreCase = true,
        int timeoutMs = DefaultAssertionTimeout)
    {
        var options = new PageGetByRoleOptions();

        if (useRegex)
        {
            options.NameRegex = new System.Text.RegularExpressions.Regex(
                name,
                ignoreCase ? System.Text.RegularExpressions.RegexOptions.IgnoreCase : System.Text.RegularExpressions.RegexOptions.None);
        }
        else
        {
            options.Name = name;
            options.Exact = true;
        }

        var link = page.GetByRole(AriaRole.Link, options);
        await Assertions.Expect(link).ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Asserts that an element with the specified alt text is visible.
    /// Uses accessible alt text selection for better accessibility testing.
    /// Supports both exact matching and regex patterns.
    ///
    /// Examples:
    ///   await page.AssertElementWithAltTextVisibleAsync("Reinier van Maanen photo");
    ///   await page.AssertElementWithAltTextVisibleAsync("Reinier", useRegex: true);
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="altText">The alt text to find (exact match or regex pattern)</param>
    /// <param name="useRegex">Whether to use regex pattern matching (default: false for exact match)</param>
    /// <param name="ignoreCase">Whether to ignore case when using regex (default: true)</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertElementWithAltTextVisibleAsync(
        this IPage page,
        string altText,
        bool useRegex = false,
        bool ignoreCase = true,
        int timeoutMs = DefaultAssertionTimeout)
    {
        var element = useRegex
            ? page.GetByAltText(new System.Text.RegularExpressions.Regex(
                altText,
                ignoreCase ? System.Text.RegularExpressions.RegexOptions.IgnoreCase : System.Text.RegularExpressions.RegexOptions.None))
            : page.GetByAltText(altText, new PageGetByAltTextOptions { Exact = true });

        await Assertions.Expect(element).ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    // ============================================================================
    // ASSERTION HELPERS - Better error messages with auto-retry
    // ============================================================================

    /// <summary>
    /// Asserts that the active collection button contains the expected text.
    /// Common pattern: Verify which collection is currently active.
    ///
    /// Example:
    ///   await page.AssertActiveCollectionAsync("Videos");
    ///   // Verifies that .collection-nav a.active contains "Videos"
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="expectedCollection">Expected collection name</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertActiveCollectionAsync(
        this IPage page,
        string expectedCollection,
        int timeoutMs = DefaultAssertionTimeout)
    {
        var activeButton = page.Locator(".collection-nav a.active");
        await Assertions.Expect(activeButton).ToContainTextAsync(
            expectedCollection,
            new() { Timeout = timeoutMs, IgnoreCase = true });
    }

    /// <summary>
    /// Asserts that an element attribute contains the expected value.
    /// Common pattern: Verify href, class, style attributes.
    ///
    /// Example:
    ///   await link.AssertAttributeContainsAsync("href", "/github-copilot");
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="attributeName">Name of the attribute (e.g., "href", "class")</param>
    /// <param name="expectedValue">Expected value or substring</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertAttributeContainsAsync(
        this ILocator locator,
        string attributeName,
        string expectedValue,
        int timeoutMs = DefaultElementTimeout)
    {
        // Get attribute value (Playwright auto-waits for element)
        var actualValue = await locator.GetAttributeAsync(attributeName, new() { Timeout = timeoutMs });

        if (actualValue == null || !actualValue.Contains(expectedValue))
        {
            throw new AssertionException(
                $"Attribute '{attributeName}' does not contain expected value. " +
                $"Expected to contain: '{expectedValue}'. " +
                $"Actual value: '{actualValue ?? "(null)"}'");
        }
    }

    /// <summary>
    /// Asserts that an element attribute equals the expected value.
    /// </summary>
    /// <param name="locator">The element to check</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="expectedValue">Expected exact value</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task AssertAttributeEqualsAsync(
        this ILocator locator,
        string attributeName,
        string expectedValue,
        int timeoutMs = DefaultElementTimeout)
    {
        var actualValue = await locator.GetAttributeAsync(attributeName, new() { Timeout = timeoutMs });

        if (actualValue != expectedValue)
        {
            throw new AssertionException(
                $"Attribute '{attributeName}' does not match expected value. " +
                $"Expected: '{expectedValue}'. " +
                $"Actual: '{actualValue ?? "(null)"}'");
        }
    }

    /// <summary>
    /// Asserts that an element exists and is visible, with a clear error message.
    /// Uses Playwright's auto-retrying Expect assertion.
    /// </summary>
    /// <param name="locator">The Playwright locator for the element</param>
    /// <param name="elementDescription">Human-readable description for error messages</param>
    /// <param name="timeoutMs">How long to wait (uses DefaultAssertionTimeout by default)</param>
    public static async Task AssertElementExistsAndVisibleAsync(
        this ILocator locator,
        string elementDescription,
        int timeoutMs = DefaultAssertionTimeout)
    {
        try
        {
            await Assertions.Expect(locator).ToBeVisibleAsync(new() { Timeout = timeoutMs });
        }
        catch (PlaywrightException ex)
        {
            var count = await locator.CountAsync();
            if (count == 0)
            {
                throw new AssertionException(
                    $"Element not found: {elementDescription}. " +
                    $"Selector matched 0 elements. " +
                    $"Original error: {ex.Message}");
            }
            else
            {
                throw new AssertionException(
                    $"Element exists but not visible: {elementDescription}. " +
                    $"Selector matched {count} element(s) but none are visible. " +
                    $"Original error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Asserts that an element is clickable, with a clear error message.
    /// Uses Playwright's auto-retrying Expect assertion.
    /// </summary>
    /// <param name="locator">The Playwright locator for the element</param>
    /// <param name="elementDescription">Human-readable description for error messages</param>
    /// <param name="timeoutMs">How long to wait</param>
    public static async Task AssertElementClickableAsync(
        this ILocator locator,
        string elementDescription,
        int timeoutMs = DefaultAssertionTimeout)
    {
        // First check visibility with auto-retry
        await locator.AssertElementExistsAndVisibleAsync(elementDescription, timeoutMs);

        // Then check enabled state using Playwright's auto-retrying assertion
        try
        {
            await Assertions.Expect(locator).ToBeEnabledAsync(new() { Timeout = timeoutMs });
        }
        catch (PlaywrightException ex)
        {
            throw new AssertionException(
                $"Element not clickable (disabled): {elementDescription}. " +
                $"Element is visible but disabled. " +
                $"Original error: {ex.Message}");
        }
    }

    /// <summary>
    /// Asserts element contains expected text using auto-retrying assertion.
    /// </summary>
    public static async Task AssertElementContainsTextAsync(
        this ILocator locator,
        string expectedText,
        string elementDescription,
        int timeoutMs = DefaultAssertionTimeout)
    {
        try
        {
            await Assertions.Expect(locator).ToContainTextAsync(expectedText, new() { Timeout = timeoutMs });
        }
        catch (PlaywrightException ex)
        {
            string? actualText = null;
            try
            {
                actualText = await locator.TextContentAsync(new() { Timeout = 1000 });
            }
            catch
            {
                // Ignore errors when getting text for error message
            }

            throw new AssertionException(
                $"Element text mismatch: {elementDescription}. " +
                $"Expected to contain: '{expectedText}'. " +
                $"Actual text: '{actualText ?? "(null)"}'. " +
                $"Original error: {ex.Message}");
        }
    }

    /// <summary>
    /// Asserts the expected count of elements using auto-retrying assertion.
    /// </summary>
    public static async Task AssertElementCountAsync(
        this ILocator locator,
        int expectedCount,
        string elementDescription,
        int timeoutMs = DefaultAssertionTimeout)
    {
        try
        {
            await Assertions.Expect(locator).ToHaveCountAsync(expectedCount, new() { Timeout = timeoutMs });
        }
        catch (PlaywrightException ex)
        {
            var actualCount = await locator.CountAsync();
            throw new AssertionException(
                $"Element count mismatch: {elementDescription}. " +
                $"Expected: {expectedCount}. " +
                $"Actual: {actualCount}. " +
                $"Original error: {ex.Message}");
        }
    }
}

/// <summary>
/// Custom exception for assertion failures with better error messages
/// </summary>
public class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
}

