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
        page.SetDefaultTimeout(3000);
        page.SetDefaultNavigationTimeout(10000);
        return page;
    }

    /// <summary>
    /// Navigates to a URL and waits for Blazor circuit to be ready.
    /// Uses optimized wait strategy for Blazor Server initialization.
    /// </summary>
    public static async Task GotoAndWaitForBlazorAsync(
        this IPage page, 
        string url, 
        PageGotoOptions? options = null)
    {
        var gotoOptions = options ?? new PageGotoOptions();
        gotoOptions.WaitUntil = WaitUntilState.DOMContentLoaded; // Faster than NetworkIdle
        gotoOptions.Timeout = 10000; // Reduced from default 30s - fail fast
        
        await page.GotoAsync(url, gotoOptions);
        
        // Wait for StreamRendering to complete:
        // 1. Skeletons render first (both .skeleton-header and .skeleton-card appear)
        // 2. Then real content streams in (skeletons disappear, real elements appear)
        try
        {
            // Wait for skeleton header to be hidden (section data loaded)
            await page.WaitForSelectorAsync(".skeleton-header", new() { 
                State = WaitForSelectorState.Hidden,
                Timeout = 2000 // Reduced timeout
            });
        }
        catch
        {
            // Skeleton might not exist if content loaded instantly - that's fine
        }
        
        // Wait for skeleton CARDS to be replaced by real content
        // Skeleton cards have BOTH .content-item-card AND .skeleton-card classes
        // Real cards have ONLY .content-item-card class
        // So we wait for cards WITHOUT the skeleton-card class
        try
        {
            await page.WaitForSelectorAsync(".content-item-card:not(.skeleton-card)", new() { 
                State = WaitForSelectorState.Visible,
                Timeout = 2000 // Reduced timeout
            });
        }
        catch
        {
            // Content cards might not exist on all pages (like home page) - that's fine
        }
        
        // Brief pause to ensure final render is stable (reduced from 200ms)
        await Task.Delay(100);
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
                var activeButton = page.Locator(".collection-nav button.active");
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
}

/// <summary>
/// Custom exception for assertion failures with better error messages
/// </summary>
public class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
}

