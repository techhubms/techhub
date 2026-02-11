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
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }

        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }
}
