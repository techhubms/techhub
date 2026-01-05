using Microsoft.Playwright;
using Xunit;

namespace TechHub.E2E.Tests;

/// <summary>
/// Collection fixture for sharing Playwright browser instance across multiple test classes.
/// This significantly reduces overhead by creating the browser once per test collection.
/// Each test class gets an isolated browser context for proper test isolation.
/// </summary>
public class PlaywrightCollectionFixture : IAsyncLifetime
{
    public IPlaywright? Playwright { get; private set; }
    public IBrowser? Browser { get; private set; }
    
    public async Task InitializeAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            // Performance optimizations for test environment
            Args = new[]
            {
                "--disable-web-security",      // Faster loading (test only!)
                "--disable-features=IsolateOrigins,site-per-process",
                "--disable-blink-features=AutomationControlled",
            }
        });
    }
    
    public async Task DisposeAsync()
    {
        if (Browser != null)
            await Browser.CloseAsync();
        
        Playwright?.Dispose();
    }
    
    /// <summary>
    /// Creates a new isolated browser context for a test class.
    /// Contexts are lightweight and isolated from each other (separate cookies, storage, etc.).
    /// </summary>
    public async Task<IBrowserContext> CreateContextAsync()
    {
        if (Browser == null)
            throw new InvalidOperationException("Browser not initialized");
        
        return await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            Locale = "en-US",
            TimezoneId = "Europe/Brussels",
        });
    }
}

/// <summary>
/// xUnit collection definitions for E2E tests.
/// Each test class gets its own collection to enable parallel execution.
/// All collections share the same PlaywrightCollectionFixture instance (one browser).
/// </summary>
[CollectionDefinition("URL Routing Tests")]
public class UrlRoutingCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Navigation Tests")]
public class NavigationCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}
