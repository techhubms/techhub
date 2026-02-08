using Microsoft.Playwright;

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
            ExecutablePath = "/usr/bin/google-chrome",
            Timeout = 5000,
            Args =
            [
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-web-security",
                "--disable-features=IsolateOrigins,site-per-process",
                "--disable-blink-features=AutomationControlled",
                "--disable-dev-shm-usage",
                "--disable-gpu"
            ]
        });
    }

    public async Task DisposeAsync()
    {
        if (Browser != null)
        {
            await Browser.CloseAsync();
        }

        Playwright?.Dispose();
    }

    /// <summary>
    /// Creates a new isolated browser context for a test class.
    /// Contexts are lightweight and isolated from each other (separate cookies, storage, etc.).
    /// </summary>
    public async Task<IBrowserContext> CreateContextAsync()
    {
        if (Browser == null)
        {
            throw new InvalidOperationException("Browser not initialized");
        }

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

[CollectionDefinition("Home Page Tests")]
public class HomePageCollection : ICollectionFixture<PlaywrightCollectionFixture>
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

[CollectionDefinition("Tag Filtering Tests")]
public class TagFilteringCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Custom Pages TOC Tests")]
public class CustomPagesTocCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Sidebar TOC Tests")]
public class SidebarTocCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Mermaid Tests")]
public class MermaidCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Highlighting Tests")]
public class HighlightingCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Tab Highlighting Tests")]
public class TabHighlightingCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Tab Ordering Tests")]
public class TabOrderingCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Infinite Scroll Tests")]
public class InfiniteScrollCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}

[CollectionDefinition("Dynamic Tag Counts Tests")]
public class DynamicTagCountsCollection : ICollectionFixture<PlaywrightCollectionFixture>
{
}
