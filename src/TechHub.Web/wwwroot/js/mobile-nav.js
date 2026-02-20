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
        document.body.style.position = 'fixed';
        document.body.style.top = `-${scrollY}px`;
        document.body.style.width = '100%';
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
