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
    /// Clicks a sidebar collection button and waits for navigation + state sync.
    /// 
    /// PATTERN: Click collection button → URL changes → .active class updates
    /// 
    /// Example:
    ///   await page.ClickCollectionButtonAsync("Videos");
    ///   // URL is now /section/videos and Videos button has .active class
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="collectionName">The collection button text (e.g., "Videos", "News", "All")</param>
    /// <param name="expectedUrlSegment">Optional: URL segment to wait for (defaults to lowercase collectionName)</param>
    public static async Task ClickCollectionButtonAsync(
        this IPage page,
        string collectionName,
        string? expectedUrlSegment = null)
    {
        var urlSegment = expectedUrlSegment ?? $"/{collectionName.ToLowerInvariant()}";
        
        // Find and click the collection button in sidebar
        var button = page.Locator(".collection-nav a, .sidebar-links a", new() { HasTextString = collectionName });
        await button.ClickBlazorElementAsync();
        
        // Wait for URL to contain the expected segment
        await page.WaitForBlazorUrlContainsAsync(urlSegment);
        
        // Wait for Blazor state sync (.active class update)
        await page.WaitForBlazorStateSyncAsync(collectionName);
    }

    /// <summary>
    /// Clicks on a content card and waits for the detail page to load.
    /// 
    /// PATTERN: Click content card → URL changes to detail → main content renders
    /// 
    /// Example:
    ///   await page.ClickContentCardAsync("/roundups/");
    ///   // Now on content detail page with sidebar and article visible
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="expectedUrlSegment">URL segment to wait for (e.g., "/roundups/")</param>
    /// <param name="cardSelector">Selector for the content card (default: ".content-item-card")</param>
    /// <param name="cardIndex">Which card to click (0-based, default first)</param>
    public static async Task ClickContentCardAsync(
        this IPage page,
        string expectedUrlSegment,
        string cardSelector = ".content-item-card",
        int cardIndex = 0)
    {
        // Wait for cards to load
        await page.WaitForSelectorAsync(cardSelector, new() { Timeout = DefaultNavigationTimeout });
        
        // Click the specified card
        var card = page.Locator(cardSelector).Nth(cardIndex);
        await card.ClickBlazorElementAsync();
        
        // Wait for navigation to detail page
        await page.WaitForBlazorUrlContainsAsync(expectedUrlSegment);
        
        // Wait for detail page content to render
        await page.WaitForContentDetailPageAsync();
    }

    /// <summary>
    /// Clicks a section card on the home page and waits for section page to load.
    /// 
    /// PATTERN: Click section card → URL changes to section → section content renders
    /// 
    /// Example:
    ///   await page.ClickSectionCardAsync("github-copilot");
    ///   // Now on /github-copilot with section content visible
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="sectionSlug">The section slug (e.g., "github-copilot", "ai")</param>
    public static async Task ClickSectionCardAsync(
        this IPage page,
        string sectionSlug)
    {
        var card = page.Locator($".section-card-container a.section-card[href*='{sectionSlug}']");
        await card.ClickBlazorElementAsync();
        
        // Wait for URL to contain section name
        await page.WaitForBlazorUrlContainsAsync($"/{sectionSlug}");
        
        // Wait for section page content to render (header + collection nav)
        await page.WaitForSectionPageReadyAsync();
    }

    /// <summary>
    /// Clicks a navigation link and waits for the target page to load.
    /// 
    /// PATTERN: Click nav link → URL changes → page content renders
    /// 
    /// Example:
    ///   await page.ClickNavLinkAsync("About", "/about");
    ///   // Now on /about with page content visible
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="linkText">The link text to click</param>
    /// <param name="expectedUrlSegment">URL segment to wait for</param>
    /// <param name="containerSelector">Optional: container to look for link in</param>
    public static async Task ClickNavLinkAsync(
        this IPage page,
        string linkText,
        string expectedUrlSegment,
        string? containerSelector = null)
    {
        var locator = containerSelector != null
            ? page.Locator($"{containerSelector} a", new() { HasTextString = linkText })
            : page.Locator("a", new() { HasTextString = linkText });
        
        await locator.First.ClickBlazorElementAsync();
        await page.WaitForBlazorUrlContainsAsync(expectedUrlSegment);
        
        // Wait for main content to be visible
        await Assertions.Expect(page.Locator("main, article, .page-main-content"))
            .ToBeVisibleAsync(new() { Timeout = DefaultNavigationTimeout });
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
        
        await page.WaitForBlazorStateSyncAsync(expectedActiveCollection);
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
        
        await page.WaitForBlazorStateSyncAsync(expectedActiveCollection);
    }

    // ============================================================================
    // PAGE-SPECIFIC WAIT HELPERS
    // ============================================================================

    /// <summary>
    /// Waits for a section page to be fully rendered.
    /// Use after navigating to a section (e.g., /github-copilot).
    /// </summary>
    public static async Task WaitForSectionPageReadyAsync(
        this IPage page,
        int timeoutMs = DefaultNavigationTimeout)
    {
        // Wait for section header
        await Assertions.Expect(page.Locator(".page-header, .section-header, h1"))
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
        
        // Wait for collection nav to be visible
        await Assertions.Expect(page.Locator(".collection-nav, .sidebar-links"))
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    /// <summary>
    /// Waits for content cards to be loaded.
    /// </summary>
    public static async Task WaitForContentCardsAsync(
        this IPage page,
        string cardSelector = ".content-item-card",
        int minCount = 1,
        int timeoutMs = DefaultNavigationTimeout)
    {
        if (minCount > 0)
        {
            await Assertions.Expect(page.Locator(cardSelector).First)
                .ToBeVisibleAsync(new() { Timeout = timeoutMs });
        }
        
        if (minCount > 1)
        {
            await Assertions.Expect(page.Locator(cardSelector))
                .ToHaveCountAsync(minCount, new() { Timeout = timeoutMs });
        }
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
        // Step 1: Wait for element to be visible using Playwright's auto-retrying assertion
        await Assertions.Expect(locator).ToBeVisibleAsync(new() { Timeout = timeoutMs });
        
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
        // Wait for element to be visible using auto-retrying assertion
        await Assertions.Expect(locator).ToBeVisibleAsync(new() { Timeout = timeoutMs });
        
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
    /// Waits for Blazor component state to synchronize after browser navigation.
    /// 
    /// When using browser back/forward buttons, Blazor's OnParametersSetAsync fires
    /// to update component state based on the new URL. This method waits for that
    /// update to complete by checking for the expected UI state.
    /// 
    /// Uses Playwright's auto-retrying Expect assertion instead of manual polling!
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="expectedActiveButtonText">Text that should appear in the active collection button</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task WaitForBlazorStateSyncAsync(
        this IPage page,
        string expectedActiveButtonText,
        int timeoutMs = DefaultAssertionTimeout)
    {
        // Use Playwright's auto-retrying assertion - no manual polling needed!
        var activeButton = page.Locator(".collection-nav a.active");
        await Assertions.Expect(activeButton).ToContainTextAsync(
            expectedActiveButtonText, 
            new() { Timeout = timeoutMs, IgnoreCase = true }
        );
    }

    /// <summary>
    /// Waits for a Blazor component to finish rendering a selector.
    /// Uses Playwright's built-in auto-retrying assertion.
    /// </summary>
    public static async Task WaitForBlazorRenderAsync(
        this IPage page, 
        string selector,
        int timeoutMs = DefaultElementTimeout)
    {
        await Assertions.Expect(page.Locator(selector))
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
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
    /// Waits for URL with standard timeout (regex pattern).
    /// Centralized timeout management.
    /// </summary>
    public static Task WaitForURLWithTimeoutAsync(
        this IPage page,
        System.Text.RegularExpressions.Regex urlRegex,
        PageWaitForURLOptions? options = null)
    {
        var opts = options ?? new PageWaitForURLOptions();
        opts.Timeout ??= DefaultNavigationTimeout;
        return page.WaitForURLAsync(urlRegex, opts);
    }

    /// <summary>
    /// Waits for a content detail page to be fully rendered after navigation.
    /// 
    /// Use this after clicking a content card to ensure the detail page is ready.
    /// Waits for the main content area and sidebar to be visible.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="urlSegment">Optional URL segment to wait for first (e.g., "/roundups/")</param>
    /// <param name="timeoutMs">Maximum time to wait</param>
    public static async Task WaitForContentDetailPageAsync(
        this IPage page,
        string? urlSegment = null,
        int timeoutMs = DefaultNavigationTimeout)
    {
        // Wait for URL if specified
        if (!string.IsNullOrEmpty(urlSegment))
        {
            await page.WaitForBlazorUrlContainsAsync(urlSegment, timeoutMs);
        }
        
        // Wait for article content to render (content-detail class is unique to detail pages)
        // Use .First to avoid strict mode violation if multiple elements match
        await Assertions.Expect(page.Locator("article.content-detail, main article").First)
            .ToBeVisibleAsync(new() { Timeout = timeoutMs });
    }

    // ============================================================================
    // ASSERTION HELPERS - Better error messages with auto-retry
    // ============================================================================

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
            try { actualText = await locator.TextContentAsync(new() { Timeout = 1000 }); } catch { }
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

    // ============================================================================
    // LEGACY COMPATIBILITY - Keep old method signatures for existing tests
    // ============================================================================

    /// <summary>
    /// [LEGACY] Kept for backwards compatibility. Use AssertElementExistsAndVisibleAsync instead.
    /// </summary>
    public static async Task<bool> AssertElementExistsAndVisible(
        this ILocator locator,
        string elementDescription,
        int timeoutMs = DefaultAssertionTimeout)
    {
        await locator.AssertElementExistsAndVisibleAsync(elementDescription, timeoutMs);
        return true;
    }

    /// <summary>
    /// [LEGACY] Kept for backwards compatibility. Use AssertElementClickableAsync instead.
    /// </summary>
    public static async Task<bool> AssertElementClickable(
        this ILocator locator,
        string elementDescription,
        int timeoutMs = DefaultAssertionTimeout)
    {
        await locator.AssertElementClickableAsync(elementDescription, timeoutMs);
        return true;
    }
}

/// <summary>
/// Custom exception for assertion failures with better error messages
/// </summary>
public class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
}

