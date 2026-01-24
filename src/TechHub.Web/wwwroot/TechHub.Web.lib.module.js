/**
 * TechHub.Web JavaScript Initializers
 * 
 * IMPORTANT: This file MUST be:
 * 1. Named "{ASSEMBLY NAME}.lib.module.js" (TechHub.Web.lib.module.js)
 * 2. Located in wwwroot/ root (NOT in wwwroot/js/)
 * 
 * This is a Blazor requirement - the framework auto-discovers and executes this file.
 * @see https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup
 * 
 * Other JavaScript files are in wwwroot/js/ and documented in Configuration/JsFiles.cs.
 * Those files are NOT Blazor initializers - they're loaded separately via App.razor.
 * 
 * This module exposes lifecycle callbacks used by:
 * - E2E tests (via BlazorHelpers.WaitForBlazorReadyAsync) to wait for interactivity
 * - Debug tooling to verify Blazor state
 */

/**
 * Called after the Blazor Web App has started (both SSR and interactive modes).
 * This is the main entry point for initialization after Blazor is ready.
 * 
 * @param {Object} blazor - The Blazor instance
 */
export function afterWebStarted(blazor) {
    // Mark that Blazor Web is ready (initial SSR rendering complete)
    window.__blazorWebReady = true;
    console.debug('[TechHub] Blazor Web started');
}

/**
 * Called after the Interactive Server runtime (SignalR circuit) is started.
 * This is the callback that fires when Blazor Server is fully interactive
 * and event handlers are attached.
 * 
 * @param {Object} blazor - The Blazor instance
 */
export function afterServerStarted(blazor) {
    // Mark that Blazor Server circuit is fully established
    // This is the signal that @onclick handlers are attached and ready
    window.__blazorServerReady = true;
    console.debug('[TechHub] Blazor Server circuit ready - event handlers attached');
}

/**
 * Called after the Interactive WebAssembly runtime is started.
 * 
 * @param {Object} blazor - The Blazor instance
 */
export function afterWebAssemblyStarted(blazor) {
    // Mark that Blazor WebAssembly is ready
    window.__blazorWasmReady = true;
    console.debug('[TechHub] Blazor WebAssembly started');
}

/**
 * Utility function to check if Blazor interactivity is ready.
 * Can be called from tests to wait for full interactivity.
 * 
 * Returns true if either Server or WebAssembly runtime is ready,
 * which means event handlers are attached and functional.
 */
window.__isBlazorInteractiveReady = function () {
    return window.__blazorServerReady === true || window.__blazorWasmReady === true;
};
