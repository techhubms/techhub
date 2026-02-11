using Microsoft.Playwright;

[assembly: AssemblyFixture(typeof(TechHub.E2E.Tests.PlaywrightCollectionFixture))]

namespace TechHub.E2E.Tests;

/// <summary>
/// Assembly fixture for sharing a single Playwright browser instance across all Web E2E test classes.
/// This significantly reduces overhead by creating the browser once per test assembly.
/// Each test class gets an isolated browser context for proper test isolation.
/// </summary>
public class PlaywrightCollectionFixture : IAsyncLifetime
{
    public IPlaywright? Playwright { get; private set; }
    public IBrowser? Browser { get; private set; }

    public async ValueTask InitializeAsync()
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

    public async ValueTask DisposeAsync()
    {
        if (Browser != null)
        {
            try
            {
                await Browser.CloseAsync();
            }
            catch (Exception)
            {
                // Best-effort browser cleanup
            }
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
