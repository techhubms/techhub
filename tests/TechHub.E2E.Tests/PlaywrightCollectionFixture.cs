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
        // CRITICAL: Clear PLAYWRIGHT_BROWSER_EXECUTABLE_PATH if set to empty string
        // Empty string causes Playwright to hang in DevContainer - let it auto-detect
        var execPath = Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSER_EXECUTABLE_PATH");
        if (execPath == string.Empty)
        {
            Environment.SetEnvironmentVariable("PLAYWRIGHT_BROWSER_EXECUTABLE_PATH", null);
        }

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        // CRITICAL FIX: Use 'chrome' channel instead of default 'chromium_headless_shell'
        // The chromium_headless_shell binary hangs when creating pages in DevContainer environments
        // Using 'chrome' channel launches the full Chrome browser in headless mode which works correctly
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Channel = "chrome",  // Use regular Chrome instead of headless_shell  
            Timeout = 5000, // 5 second timeout for browser launch (works fine in DevContainer)
            // Performance optimizations for DevContainer environment
            // See: tests/TechHub.E2E.Tests/PLAYWRIGHT-CONFIG.md for detailed explanation
            Args = new[]
            {
                "--no-sandbox",                // Required for Docker/DevContainer environments
                "--disable-setuid-sandbox",    // Required for Docker/DevContainer environments
                "--disable-web-security",      // Faster loading (test only!)
                "--disable-features=IsolateOrigins,site-per-process",
                "--disable-blink-features=AutomationControlled",
                "--disable-dev-shm-usage",     // Overcome limited resource problems
                "--disable-gpu"                // Disable GPU hardware acceleration
                // NOTE: --single-process REMOVED - causes test host crashes in .NET environments
                // NOTE: --no-zygote REMOVED - not needed with 'chrome' channel
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
            IgnoreHTTPSErrors = true, // Ignore HTTPS errors in test environment
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

[CollectionDefinition("RSS Tests")]
public class RssCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Content Detail Tests")]
public class ContentDetailCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("About Page Tests")]
public class AboutPageCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Home Page Roundups Tests")]
public class HomePageRoundupsCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Home Page Sidebar Tests")]
public class HomePageSidebarCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Custom Pages Tests")]
public class CustomPagesCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Section Card Layout Tests")]
public class SectionCardLayoutCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Section Page Keyboard Navigation Tests")]
public class SectionPageKeyboardNavigationCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}
