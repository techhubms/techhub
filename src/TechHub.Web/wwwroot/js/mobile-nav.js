// ============================================================================
// Mobile Navigation JavaScript
//
// Handles body scroll lock when hamburger menu is open and Escape key to close.
// Called from NavHeader.razor via JS interop.
// ============================================================================

window.mobileNav = {
    /** Lock body scroll when mobile menu is open (preserves scroll position) */
    lockScroll: () => {
        const scrollY = window.scrollY;
        // Set top before position:fixed to prevent a Safari paint flash where the
        // body briefly renders at position 0 before the scroll offset is applied.
        document.body.style.top = `-${scrollY}px`;
        document.body.style.position = 'fixed';
        // Use explicit pixel width matching the current layout width so the fixed
        // body does not expand by the scrollbar width on desktop (where html has a
        // permanent scrollbar via overflow-y: scroll).
        document.body.style.width = `${document.documentElement.clientWidth}px`;
    },

    /** Unlock body scroll when mobile menu is closed (restores scroll position) */
    unlockScroll: () => {
        const top = document.body.style.top;
        document.body.style.position = '';
        document.body.style.top = '';
        document.body.style.width = '';
        window.scrollTo(0, parseInt(top || '0') * -1);
    },

    /**
     * Register Escape key handler to close the mobile menu.
     * @param {DotNetObjectReference} dotNetHelper - Blazor component reference
     */
    registerEscapeHandler: (dotNetHelper) => {
        const handler = (e) => {
            if (e.key === 'Escape') {
                dotNetHelper.invokeMethodAsync('CloseMenuFromJs');
            }
        };
        document.addEventListener('keydown', handler);

        // Store handler for potential cleanup
        window.mobileNav._escapeHandler = handler;
    }
};
