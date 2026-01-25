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

    // Set up focus scroll compensation for sticky headers
    // When a focusable element receives focus and is behind a sticky header,
    // scroll it into view with proper offset
    setupFocusScrollCompensation();

    // Set up focus management for enhanced navigation (accessibility)
    // When Blazor navigates via enhanced navigation (SPA-style), focus needs to reset
    // to the top of the page so keyboard users can navigate from the beginning.
    // This is a WCAG 2.1 requirement for consistent keyboard navigation after page changes.
    blazor.addEventListener('enhancedload', () => {
        // Reset focus to top of page for keyboard navigation
        // Use requestAnimationFrame to ensure DOM is fully updated
        requestAnimationFrame(() => {
            // Scroll to top of page
            window.scrollTo(0, 0);

            // To reset tab order, we need to move browser's internal focus tracking
            // to the top of the page. We do this by:
            // 1. Creating a temporary focusable element at the very top
            // 2. Focusing it (which resets browser's tab position)
            // 3. Removing focus and the element
            // This ensures next Tab press focuses the first element (skip-link)

            const tempFocus = document.createElement('span');
            tempFocus.setAttribute('tabindex', '-1');
            tempFocus.style.cssText = 'position:absolute;left:-9999px;top:0;';
            document.body.insertBefore(tempFocus, document.body.firstChild);

            // Focus without triggering scroll
            tempFocus.focus({ preventScroll: true });

            // Remove the element after a frame (focus position is now reset)
            requestAnimationFrame(() => {
                tempFocus.remove();
                console.debug('[TechHub] Focus reset after enhanced navigation');
            });
        });
    });
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
