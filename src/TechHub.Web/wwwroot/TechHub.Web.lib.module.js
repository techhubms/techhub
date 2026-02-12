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

    // Enable browser's native scroll restoration for back/forward navigation
    // This prevents any JavaScript from interfering with scroll position restoration
    if ('scrollRestoration' in history) {
        history.scrollRestoration = 'auto';
    }

    // Set up focus scroll compensation for sticky headers
    // When a focusable element receives focus and is behind a sticky header,
    // scroll it into view with proper offset
    setupFocusScrollCompensation();
}

/**
 * Called after the Interactive Server runtime (SignalR circuit) is started.
 * This is the callback that fires when Blazor Server is fully interactive
 * and event handlers are attached.
 * 
 * With global InteractiveServer mode, this replaces DOMContentLoaded/enhancedload
 * as the trigger for loading page scripts (mermaid, highlight.js, custom pages, etc.).
 * Blazor may replace prerendered DOM elements during hydration, so scripts must
 * run AFTER the circuit is established to operate on the final DOM.
 * 
 * @param {Object} blazor - The Blazor instance
 */
export function afterServerStarted(blazor) {
    // Mark that Blazor Server circuit is fully established
    // This is the signal that @onclick handlers are attached and ready
    window.__blazorServerReady = true;
    console.debug('[TechHub] Blazor Server circuit ready - event handlers attached');

    // Page scripts (mermaid, highlight.js, custom pages, etc.) are loaded by
    // individual page components in their OnAfterRenderAsync. Each component
    // calls only the specific init functions it needs (e.g., initMermaid,
    // initHighlighting) — see js/page-scripts.js for the init functions.
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

/**
 * Sets up focus scroll compensation for sticky headers.
 * When tabbing backwards (Shift+Tab) and an element receives focus behind
 * a sticky header, this scrolls the element into view.
 * 
 * CSS scroll-margin-top handles most cases, but this provides a JavaScript
 * backup for edge cases and older browsers.
 */
function setupFocusScrollCompensation() {
    // Combined height of sticky elements (main-nav + sub-nav + padding)
    // This should match --sticky-header-height in design-tokens.css
    const STICKY_HEADER_HEIGHT = 146; // 76px + 54px + 16px padding

    document.addEventListener('focusin', (event) => {
        const element = event.target;
        if (!element) return;

        // Get the element's position relative to the viewport
        const rect = element.getBoundingClientRect();

        // Check if the element is behind the sticky header
        if (rect.top < STICKY_HEADER_HEIGHT) {
            // Calculate how much we need to scroll up
            const scrollOffset = STICKY_HEADER_HEIGHT - rect.top + 16; // Extra 16px padding

            // Scroll the window up smoothly
            window.scrollBy({
                top: -scrollOffset,
                behavior: 'smooth'
            });
        }
    });

    console.debug('[TechHub] Focus scroll compensation enabled');
}
