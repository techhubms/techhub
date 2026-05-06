using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TechHub.Web.Components;

/// <summary>
/// Base class for Blazor page components that manages the markScriptsLoading /
/// markScriptsReady JS lifecycle required for scroll position restoration on
/// back/forward navigation.
///
/// Pages configure behaviour by overriding properties — most pages do not need
/// to override OnAfterRenderAsync at all. If a page needs pre-script logic it
/// can override OnAfterRenderAsync and call await base.OnAfterRenderAsync(firstRender)
/// at the end.
///
/// Three patterns:
///
///   1. Simple pages (no JS dependencies):
///      Leave all properties at their defaults. markScriptsReady is called on
///      every render (idempotent in JS).
///
///   2. Data-dependent pages (init-once, default):
///      Override PageScripts, IsPageDataReady (and IsNotFound if the page can 404).
///      Scripts run exactly once when data arrives; the _notFound path unblocks
///      WaitForBlazorReadyAsync when data will never arrive.
///
///   3. Pages whose content changes on in-component navigation (re-run):
///      Same as (2) but also override ReinitializeScriptsOnRerender => true.
///      Scripts re-run on every render where IsPageDataReady is true.
///      Use for pages like ContentItem/GenAI that reload content without a
///      component teardown (new item = new DOM = scripts must re-apply).
///
/// The convention test (PageMarkScriptsReadyConventionTests) accepts either a direct
/// InvokeVoidAsync("markScriptsReady") call in the .razor file OR @inherits PageComponentBase
/// as proof that the contract is fulfilled.
/// </summary>
public abstract class PageComponentBase : ComponentBase
{
    [Inject] protected IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// JS function names to invoke (in order) once page data is ready.
    /// Leave empty (default) for pages with no JS dependencies.
    /// </summary>
    protected virtual IReadOnlyList<string> PageScripts => [];

    /// <summary>
    /// Returns true when async data has loaded and scripts can be initialised.
    /// Default: true (pages with no async data dependency).
    /// </summary>
    protected virtual bool IsPageDataReady => true;

    /// <summary>
    /// Returns true when the page is in a 404 / error state and data will never
    /// arrive. Fires markScriptsReady so WaitForBlazorReadyAsync does not block
    /// indefinitely on not-found pages.
    /// </summary>
    protected virtual bool IsNotFound => false;

    /// <summary>
    /// When true, scripts re-run on every render where IsPageDataReady is true.
    /// Default: false (init-once). Set to true for pages that reload content
    /// without a component teardown so scripts re-apply to the new DOM.
    /// </summary>
    protected virtual bool ReinitializeScriptsOnRerender => false;

    private bool _scriptsInitialized;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        await RunScriptLifecycleAsync();
    }

    private async Task RunScriptLifecycleAsync()
    {
        // 404 / error: data will never arrive — unblock WaitForBlazorReadyAsync.
        if (IsNotFound)
        {
            await JS.InvokeVoidAsync("markScriptsReady");
            return;
        }

        // Data not yet available — wait for the next render.
        if (!IsPageDataReady)
        {
            return;
        }

        var scripts = PageScripts;

        // No scripts declared — signal ready directly (idempotent in JS).
        if (scripts.Count == 0)
        {
            await JS.InvokeVoidAsync("markScriptsReady");
            return;
        }

        // Guard for init-once pages: skip if already initialised.
        if (_scriptsInitialized && !ReinitializeScriptsOnRerender)
        {
            return;
        }

        _scriptsInitialized = true;
        try
        {
            await JS.InvokeVoidAsync("markScriptsLoading");
            foreach (var script in scripts)
            {
                await JS.InvokeVoidAsync(script);
            }
        }
        finally
        {
            await JS.InvokeVoidAsync("markScriptsReady");
        }
    }
}
