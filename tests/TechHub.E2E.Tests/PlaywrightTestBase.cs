using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests;

/// <summary>
/// Base class for Playwright-based E2E Web tests.
/// Provides per-test browser context and page lifecycle management.
///
/// Each test class gets an isolated browser context (separate cookies, storage, etc.)
/// while sharing the same browser instance via the assembly-scoped <see cref="PlaywrightCollectionFixture"/>.
/// </summary>
public abstract class PlaywrightTestBase : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private IPage? _page;
    private ICDPSession? _cdpSession;

    /// <summary>
    /// Gets the Playwright page for the current test.
    /// </summary>
    protected IPage Page => _page ?? throw new InvalidOperationException("Page not initialized. Ensure InitializeAsync has completed.");

    /// <summary>
    /// Gets the browser context for the current test.
    /// Use this to create additional pages within the same isolated context.
    /// </summary>
    protected IBrowserContext Context => _context ?? throw new InvalidOperationException("Context not initialized. Ensure InitializeAsync has completed.");

    protected PlaywrightTestBase(PlaywrightCollectionFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    public virtual async ValueTask InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
        _cdpSession = await NetworkThrottling.ApplyIfConfiguredAsync(_page);
    }

    public virtual async ValueTask DisposeAsync()
    {
        // Per Playwright docs: BrowserContext.CloseAsync() gracefully closes all pages
        // belonging to the context, ensuring page close events fire properly.
        // This must happen before Browser.CloseAsync() in the fixture.
        // DisposeAsync() releases underlying resources after graceful close.
        await CleanupBrowserResourcesAsync();
    }

    /// <summary>
    /// Default number of retry attempts for <see cref="WithRetryAsync"/> on CI.
    /// CI runners experience resource pressure that causes transient timeouts
    /// which don't reproduce locally. Total attempts = 1 + DefaultCIMaxRetries.
    /// </summary>
    private const int DefaultCIMaxRetries = 2;

    /// <summary>
    /// Retries the test body with a fresh browser context on each attempt.
    /// Use this for tests that are flaky due to transient CI conditions (resource pressure,
    /// slow SignalR connections, etc.) rather than deterministic bugs.
    ///
    /// On each retry, the previous browser context and page are disposed and new ones are
    /// created, ensuring a completely clean slate (fresh cookies, storage, DOM, and SignalR circuit).
    ///
    /// Example:
    ///   await WithRetryAsync(async () =>
    ///   {
    ///       await Page.GotoRelativeAsync("/github-copilot/news");
    ///       await Page.ScrollToLoadMoreAsync(expectedItemCount: 21);
    ///   });
    /// </summary>
    /// <param name="testBody">The test logic to execute (and potentially retry)</param>
    /// <param name="maxRetries">Maximum number of retry attempts (default: 2 on CI, 0 locally).
    /// Total attempts = 1 + maxRetries.</param>
    protected async Task WithRetryAsync(Func<Task> testBody, int? maxRetries = null)
    {
        var retries = maxRetries ?? (BlazorHelpers.IsCI ? DefaultCIMaxRetries : 0);
        Exception? lastException = null;

        for (var attempt = 0; attempt <= retries; attempt++)
        {
            try
            {
                if (attempt > 0)
                {
                    // Dispose old context and create a fresh one for the retry
                    await CleanupBrowserResourcesAsync();
                    _context = await _fixture.CreateContextAsync();
                    _page = await _context.NewPageWithDefaultsAsync();
                    _cdpSession = await NetworkThrottling.ApplyIfConfiguredAsync(_page);
                }

                await testBody();
                return; // Success — exit immediately
            }
            catch (TimeoutException ex) when (attempt < retries)
            {
                lastException = ex;
                // Only retry on TimeoutException — other failures are likely deterministic bugs
            }
            catch (PlaywrightException ex) when (attempt < retries && ex.Message.Contains("Timeout"))
            {
                lastException = ex;
                // Playwright wraps timeouts in PlaywrightException
            }
        }

        throw lastException!;
    }

    private async Task CleanupBrowserResourcesAsync()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        try
        {
            if (_cdpSession != null)
            {
                await _cdpSession.DetachAsync().WaitAsync(cts.Token);
                _cdpSession = null;
            }
        }
        catch (Exception)
        {
            // Best-effort CDP session cleanup
        }

        try
        {
            if (_page != null)
            {
                await _page.CloseAsync().WaitAsync(cts.Token);
                _page = null;
            }
        }
        catch (Exception)
        {
            // Best-effort context cleanup
        }

        try
        {
            if (_context != null)
            {
                await _context.CloseAsync().WaitAsync(cts.Token);
                await _context.DisposeAsync().AsTask().WaitAsync(cts.Token);
                _context = null;
            }
        }
        catch (Exception)
        {
            // Best-effort context cleanup
        }
    }
}
