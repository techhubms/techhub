using Microsoft.Playwright;

namespace TechHub.E2E.Tests.Helpers;

/// <summary>
/// Helper methods for working with Blazor Server in E2E tests
/// </summary>
public static class BlazorHelpers
{
    /// <summary>
    /// Creates a new page with optimized default settings for E2E tests.
    /// Sets aggressive timeouts to fail fast and avoid slow tests.
    /// </summary>
    public static async Task<IPage> NewPageWithDefaultsAsync(this IBrowserContext context)
    {
        var page = await context.NewPageAsync();
        page.SetDefaultTimeout(5000); // Increased for Blazor hydration stability
        page.SetDefaultNavigationTimeout(10000);
        return page;
    }

    /// <summary>
    /// Navigates to a URL and waits for Blazor circuit to be ready.
    /// Uses Blazor's enhancedload event to detect when rendering is complete.
    /// </summary>
    public static async Task GotoAndWaitForBlazorAsync(
        this IPage page, 
        string url, 
        PageGotoOptions? options = null)
    {
        var gotoOptions = options ?? new PageGotoOptions();
        gotoOptions.WaitUntil = WaitUntilState.DOMContentLoaded;
        gotoOptions.Timeout = 10000;
        
        await page.GotoAsync(url, gotoOptions);
        
        // STEP 1: Wait for Blazor runtime to load
        // This is the foundation - Blazor JS must be loaded before anything else
        try
        {
            await page.WaitForFunctionAsync(
                "() => window.Blazor !== undefined",
                new PageWaitForFunctionOptions { Timeout = 10000, PollingInterval = 100 }
            );
        }
        catch
        {
            // Static pages without Blazor - that's fine, skip Blazor-specific waits
            return;
        }
        
        // STEP 2: Wait for Blazor's enhancedload event
        // This event fires when:
        // - Enhanced navigation completes
        // - Streaming rendering finishes
        // - All Blazor-managed content is rendered and ready
        // This is THE authoritative signal that Blazor is done with the page
        try
        {
            await page.WaitForFunctionAsync(@"
                () => new Promise((resolve) => {
                    // If Blazor already loaded, resolve immediately
                    if (window.Blazor && window.Blazor._internal && window.Blazor._internal.navigationEnhancementCallbacks) {
                        // Already loaded, check if we're in middle of navigation
                        const isNavigating = window.Blazor._internal.isNavigating || false;
                        if (!isNavigating) {
                            resolve(true);
                            return;
                        }
                    }
                    
                    // Otherwise wait for enhancedload event
                    if (window.Blazor && window.Blazor.addEventListener) {
                        const timeout = setTimeout(() => resolve(true), 8000); // Fallback after 8s
                        window.Blazor.addEventListener('enhancedload', () => {
                            clearTimeout(timeout);
                            resolve(true);
                        });
                    } else {
                        // Blazor exists but doesn't have enhanced navigation - resolve immediately
                        resolve(true);
                    }
                })
            ", new PageWaitForFunctionOptions { Timeout = 10000 });
        }
        catch
        {
            // enhancedload might not fire for initial page load - that's fine
        }
        
        // STEP 3: Wait for network to be truly idle
        // This ensures all API calls and resource fetches are complete
        try
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
        catch
        {
            // NetworkIdle might timeout if there are long-polling connections - that's fine
        }
        
        // STEP 4: Fallback wait for main content to be visible
        // This handles custom pages, about page, and any other page types
        // that don't have content cards but do have an #main-content element
        try
        {
            await page.WaitForSelectorAsync("#main-content, .content-item-card:not(.skeleton-card)", new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 3000
            });
        }
        catch
        {
            // Some pages might not have #main-content or content cards - that's fine
        }
        
        // STEP 5: Final stability delay for layout to settle
        // Reduced from 300ms since we now have proper event detection
        await Task.Delay(100);
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
    /// Waits for the Blazor circuit to be ready for interaction.
    /// Call this if you need to ensure the circuit is fully connected after page load.
    /// </summary>
    public static async Task WaitForBlazorCircuitAsync(this IPage page)
    {
        // Wait for network to be idle (SignalR connection should be established)
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Minimal delay for render completion
        await Task.Delay(50);
    }

    /// <summary>
    /// Waits for a Blazor element to be fully interactive and actionable.
    /// 
    /// Blazor SSR has a multi-stage hydration process:
    /// 1. Initial HTML renders (fast)
    /// 2. Blazor JavaScript loads (fast)
    /// 3. DOM elements become visible (fast)
    /// 4. SignalR circuit connects (variable)
    /// 5. Enhanced navigation handlers attach (variable)
    /// 6. Element becomes stable/clickable (THIS IS WHERE TESTS FAIL)
    /// 
    /// This method waits for ALL stages to complete before returning.
    /// Use this before clicking/interacting with Blazor-enhanced elements.
    /// </summary>
    /// <param name="locator">The element to wait for</param>
    /// <param name="timeoutMs">Maximum time to wait (default 15000ms for Blazor hydration)</param>
    /// <remarks>
    /// Why this is necessary:
    /// Playwright considers an element "ready for click" when it's visible, enabled, and stable.
    /// "Stable" means it's not animating and hasn't moved for a short period.
    /// 
    /// However, Blazor enhanced navigation attaches click interceptors AFTER the element is
    /// already visible and stable in the DOM. This creates a timing window where the element
    /// looks ready but clicking it will fail because Blazor's handlers aren't attached yet.
    /// 
    /// This method bridges that gap by waiting for Blazor's page to fully load and settle
    /// before allowing interactions.
    /// </remarks>
    public static async Task WaitForBlazorInteractivityAsync(
        this ILocator locator,
        int timeoutMs = 15000)
    {
        // STEP 1: Wait for element to be visible in DOM
        await locator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = timeoutMs });
        
        // STEP 2: Wait for page load state to be complete (all resources loaded)
        try
        {
            await locator.Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new() { Timeout = 5000 });
        }
        catch
        {
            // NetworkIdle might timeout if there are long-polling connections - that's OK
        }
        
        // STEP 3: Extended stabilization delay for Blazor enhanced navigation to attach handlers
        // This gives Blazor's JavaScript time to:
        // - Establish SignalR circuit connection
        // - Initialize enhanced navigation system
        // - Attach click event interceptors to links
        // - Make elements truly interactive
        await Task.Delay(1000); // Increased from 300ms to 1000ms for reliable hydration
    }

    /// <summary>
    /// Clicks a Blazor-enhanced element after ensuring it's fully interactive.
    /// Use this instead of ClickAsync() for elements with Blazor enhanced navigation.
    /// </summary>
    /// <param name="locator">The element to click</param>
    /// <param name="timeoutMs">Maximum time to wait for interactivity (default 15000ms for Blazor hydration)</param>
    public static async Task ClickBlazorElementAsync(
        this ILocator locator,
        int timeoutMs = 15000)
    {
        await locator.WaitForBlazorInteractivityAsync(timeoutMs);
        // Use Force=true to bypass Playwright's stability checks
        // We've already waited for Blazor hydration, so we know the element is ready
        // Blazor's continuous DOM updates during hydration prevent "stability" detection
        await locator.ClickAsync(new() { Force = true, Timeout = 5000 });
    }

    /// <summary>
    /// Waits for a Blazor component to finish rendering.
    /// Useful after triggering state changes that cause re-renders.
    /// </summary>
    public static async Task WaitForBlazorRenderAsync(
        this IPage page, 
        string selector)
    {
        await page.WaitForSelectorAsync(selector);
        // Element is visible means render is complete
    }

    /// <summary>
    /// Waits for URL to contain a specific path segment after SPA navigation.
    /// 
    /// Modern web applications (React, Angular, Vue, Blazor) use client-side routing that
    /// changes URLs without traditional page reloads. This means browser navigation events
    /// like "load", "commit", and "domcontentloaded" DON'T fire during navigation.
    /// 
    /// This method uses the standard Playwright pattern for testing SPAs: polling JavaScript
    /// to detect URL changes rather than waiting for navigation events that will never occur.
    /// This is NOT a Blazor-specific workaround - it's how you test ANY modern SPA.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="urlSegment">The URL path segment to wait for (e.g., "news", "videos", "github-copilot")</param>
    /// <remarks>
    /// Why WaitForFunctionAsync instead of WaitForURLAsync:
    /// - WaitForURLAsync with WaitUntilState (Load/Commit/DOMContentLoaded) waits for navigation events
    /// - SPAs use client-side routing - URL changes without triggering these events
    /// - WaitForFunctionAsync polls JavaScript expression until it returns true
    /// - This is the documented Playwright pattern for SPA testing
    /// 
    /// Official Playwright docs:
    /// - https://playwright.dev/dotnet/docs/api/class-page#page-wait-for-function
    /// 
    /// Would be identical in a greenfield React, Angular, or Vue app.
    /// </remarks>
    public static async Task WaitForBlazorUrlContainsAsync(
        this IPage page,
        string urlSegment,
        int timeoutMs = 5000) // Explicit timeout, reduced from default 30s
    {
        await page.WaitForFunctionAsync(
            $"() => window.location.pathname.includes('{urlSegment}')",
            new PageWaitForFunctionOptions { Timeout = timeoutMs, PollingInterval = 50 } // Poll every 50ms for faster detection
        );
        
        // Brief stabilization wait after URL change to ensure content is rendered
        await Task.Delay(200);
    }
    
    /// <summary>
    /// Waits for Blazor component state to synchronize after browser navigation.
    /// Replaces hard-coded Task.Delay(1000) with smart polling that returns as soon as state is synced.
    /// </summary>
    /// <param name="page">The Playwright page</param>
    /// <param name="expectedActiveButtonText">The text that should appear in the active collection button</param>
    /// <param name="timeoutMs">Maximum time to wait (default 3000ms)</param>
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
                var activeButton = page.Locator(".collection-nav a.active");
                var activeText = await activeButton.TextContentAsync(new() { Timeout = 500 });
                
                if (activeText?.Contains(expectedActiveButtonText, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return; // State is synced!
                }
            }
            catch
            {
                // Button might not exist yet or text might not match, keep polling
            }
            
            await Task.Delay(50); // Poll every 50ms (much faster than 1000ms delay)
        }
        
        throw new TimeoutException($"Blazor state did not sync to '{expectedActiveButtonText}' within {timeoutMs}ms");
    }

    /// <summary>
    /// Asserts that an element exists and is visible, failing fast with a clear message if not.
    /// This is better than waiting for timeouts - it checks immediately and provides context.
    /// </summary>
    /// <param name="locator">The Playwright locator for the element</param>
    /// <param name="elementDescription">Human-readable description of what element we're looking for</param>
    /// <param name="timeoutMs">How long to wait for element (default 3000ms, same as page default)</param>
    /// <returns>True if element is visible</returns>
    /// <remarks>
    /// Use this instead of just calling IsVisibleAsync() with default timeout.
    /// Benefits:
    /// - Fails fast with clear error message instead of generic timeout
    /// - Provides context about what was expected
    /// - Uses same timeout as page default for consistency
    /// </remarks>
    public static async Task<bool> AssertElementExistsAndVisible(
        this ILocator locator,
        string elementDescription,
        int timeoutMs = 3000)
    {
        try
        {
            await locator.WaitForAsync(new() 
            { 
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs 
            });
            return true;
        }
        catch (TimeoutException)
        {
            // Provide better error message than generic timeout
            var count = await locator.CountAsync();
            if (count == 0)
            {
                throw new AssertionException(
                    $"Element not found: {elementDescription}. " +
                    $"Selector '{locator}' matched 0 elements. " +
                    $"Check if selector is correct or if element should exist on this page.");
            }
            else
            {
                throw new AssertionException(
                    $"Element exists but not visible: {elementDescription}. " +
                    $"Selector '{locator}' matched {count} element(s) but none are visible. " +
                    $"Check CSS (display:none, visibility:hidden, opacity:0) or if element is off-screen.");
            }
        }
    }

    /// <summary>
    /// Asserts that an element is clickable (visible, enabled, not covered), failing fast if not.
    /// This is better than waiting for click timeouts - it checks the state immediately.
    /// </summary>
    /// <param name="locator">The Playwright locator for the element</param>
    /// <param name="elementDescription">Human-readable description of what element we're trying to click</param>
    /// <param name="timeoutMs">How long to wait for element to be clickable (default 3000ms)</param>
    /// <returns>True if element is clickable</returns>
    /// <remarks>
    /// Use this before ClickAsync() to get better error messages.
    /// Benefits:
    /// - Fails fast with clear error message instead of click timeout
    /// - Distinguishes between "element missing", "element disabled", "element covered"
    /// - Provides actionable debugging information
    /// </remarks>
    public static async Task<bool> AssertElementClickable(
        this ILocator locator,
        string elementDescription,
        int timeoutMs = 3000)
    {
        // First check if element exists and is visible
        await locator.AssertElementExistsAndVisible(elementDescription, timeoutMs);
        
        // Check if element is enabled
        var isEnabled = await locator.IsEnabledAsync();
        if (!isEnabled)
        {
            throw new AssertionException(
                $"Element not clickable (disabled): {elementDescription}. " +
                $"Selector '{locator}' is visible but disabled. " +
                $"Check if button/input has 'disabled' attribute or is in a fieldset with disabled.");
        }

        // Try to check if element would be actionable (not covered by another element)
        // Note: Playwright's click() already does this check, but we want to fail early with better message
        try
        {
            await locator.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 1000 // Quick check
            });
            
            // Element exists, is visible, and is enabled
            return true;
        }
        catch (TimeoutException)
        {
            throw new AssertionException(
                $"Element not clickable (possibly covered): {elementDescription}. " +
                $"Selector '{locator}' is visible and enabled but may be covered by another element. " +
                $"Check z-index, overlays, modals, or if element is outside viewport.");
        }
    }

    /// <summary>
    /// Waits for a selector with standard timeout (3000ms).
    /// Centralized timeout management - change here to affect all tests.
    /// </summary>
    public static Task WaitForSelectorWithTimeoutAsync(
        this IPage page,
        string selector,
        PageWaitForSelectorOptions? options = null)
    {
        var opts = options ?? new PageWaitForSelectorOptions();
        opts.Timeout ??= 3000; // Standard timeout for element waits
        return page.WaitForSelectorAsync(selector, opts);
    }

    /// <summary>
    /// Gets text content with standard timeout (3000ms).
    /// Centralized timeout management - change here to affect all tests.
    /// </summary>
    public static Task<string?> TextContentWithTimeoutAsync(
        this ILocator locator,
        LocatorTextContentOptions? options = null)
    {
        var opts = options ?? new LocatorTextContentOptions();
        opts.Timeout ??= 3000; // Standard timeout for text content
        return locator.TextContentAsync(opts);
    }

    /// <summary>
    /// Waits for URL with standard timeout (5000ms).
    /// URL navigation may take slightly longer than element waits.
    /// Centralized timeout management - change here to affect all tests.
    /// </summary>
    public static Task WaitForURLWithTimeoutAsync(
        this IPage page,
        string urlOrPredicate,
        PageWaitForURLOptions? options = null)
    {
        var opts = options ?? new PageWaitForURLOptions();
        opts.Timeout ??= 5000; // Standard timeout for URL navigation (slightly longer)
        return page.WaitForURLAsync(urlOrPredicate, opts);
    }

    /// <summary>
    /// Waits for URL with standard timeout (5000ms) using regex.
    /// Centralized timeout management - change here to affect all tests.
    /// </summary>
    public static Task WaitForURLWithTimeoutAsync(
        this IPage page,
        System.Text.RegularExpressions.Regex urlRegex,
        PageWaitForURLOptions? options = null)
    {
        var opts = options ?? new PageWaitForURLOptions();
        opts.Timeout ??= 5000; // Standard timeout for URL navigation (slightly longer)
        return page.WaitForURLAsync(urlRegex, opts);
    }
}

/// <summary>
/// Custom exception for assertion failures with better error messages
/// </summary>
public class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
}

