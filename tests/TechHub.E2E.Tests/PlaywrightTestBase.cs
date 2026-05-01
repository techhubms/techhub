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
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        try
        {
            if (_cdpSession != null)
            {
                await _cdpSession.DetachAsync().WaitAsync(cts.Token);
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
                // Gracefully disconnect the Blazor circuit before closing the page.
                //
                // Blazor Server registers a pagehide listener that sends a disconnect beacon
                // (POST /_blazor/disconnect/) when the browser navigates or closes. When
                // Playwright calls CloseAsync() directly, it kills the TCP connection before
                // the beacon fires, and the server records a 499 (client closed connection)
                // in Application Insights — inflating the failure count and triggering alerts.
                //
                // Calling window.Blazor.disconnect() is the proper Blazor teardown path:
                // it sends the disconnect POST synchronously via the circuit, after which
                // WaitForLoadStateAsync(NetworkIdle) ensures the POST fully completes before
                // we close. Only called when the circuit is actually active.
                var isBlazorActive = await _page.EvaluateAsync<bool>(
                    "() => window.__blazorServerReady === true");

                if (isBlazorActive)
                {
                    await _page.EvaluateAsync(
                        "() => { try { window.Blazor?.disconnect?.(); } catch (_) {} }");

                    await _page.WaitForLoadStateAsync(
                        LoadState.NetworkIdle, new() { Timeout = 3000 }).WaitAsync(cts.Token);
                }
            }
        }
        catch (Exception)
        {
            // Best-effort: page may be on an error page, already closed, or Blazor not active
        }

        try
        {
            if (_page != null)
            {
                await _page.CloseAsync().WaitAsync(cts.Token);
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
            }
        }
        catch (Exception)
        {
            // Best-effort context cleanup
        }
    }
}
