using Microsoft.Playwright;

namespace TechHub.E2E.Tests.Helpers;

/// <summary>
/// Helper methods for working with Blazor Server in E2E tests
/// </summary>
public static class BlazorHelpers
{
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
        
        // Blazor Server needs time to:
        // 1. Load initial HTML/CSS
        // 2. Download blazor.web.js
        // 3. Establish SignalR connection
        // 4. Initialize circuit
        // 5. Complete first render
        gotoOptions.Timeout ??= 10000; // 10 seconds is sufficient for local development
        gotoOptions.WaitUntil = WaitUntilState.NetworkIdle; // Wait for network to settle
        
        await page.GotoAsync(url, gotoOptions);
        
        // Wait for Blazor to fully initialize by checking for absence of loading states
        // This is more reliable than arbitrary delays
        try
        {
            await page.WaitForSelectorAsync(".loading", new() { State = WaitForSelectorState.Hidden, Timeout = 5000 });
        }
        catch
        {
            // Loading element might not exist if content loaded instantly - that's fine
        }
        
        // Brief pause to ensure render is complete
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
        string selector, 
        int timeout = 5000)
    {
        await page.WaitForSelectorAsync(selector, new() { Timeout = timeout });
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
    /// <param name="timeout">Timeout in milliseconds (default 5000ms)</param>
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
        int timeout = 5000)
    {
        await page.WaitForFunctionAsync(
            $"() => window.location.pathname.includes('{urlSegment}')",
            null,
            new() { Timeout = timeout }
        );
    }
}

