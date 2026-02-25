/**
 * Navigation Helpers - Back to Top, Back to Previous Page, Hash Link Fixes,
 * and Keyboard Navigation Detection
 * 
 * Provides sticky bottom buttons for:
 * - Back to top: Smooth scroll to top of page
 * - Back to previous page: Navigate to previous page in history
 * 
 * Also handles hash-only link navigation to work around <base href="/"> issues.
 * When a hash-only link (#section) is clicked, the browser resolves it relative
 * to the base URL instead of the current page. This handler intercepts those
 * clicks and navigates correctly.
 * 
 * Keyboard Navigation Detection:
 * Adds/removes 'keyboard-nav' class on <html> to distinguish keyboard (Tab)
 * from pointer/touch input. Focus outlines are only shown in keyboard-nav mode
 * via CSS scoping. This prevents lingering focus rings on mobile after tapping
 * buttons, while preserving WCAG-compliant keyboard accessibility.
 * 
 * Buttons appear when user scrolls down (300px threshold) and are hidden at top.
 * 
 * Browser Compatibility:
 * - Uses modern APIs (requestAnimationFrame, MutationObserver)
 * - Requires ES6+ support
 * 
 * @see {@link /workspaces/techhub/src/TechHub.Web/AGENTS.md} for architecture details
 */

(function () {
    'use strict';

    // ========================================================================
    // Keyboard Navigation Detection
    // Adds 'keyboard-nav' class to <html> when Tab key is pressed,
    // removes it on pointer/touch interaction.
    // All focus outline CSS is scoped to html.keyboard-nav so outlines
    // only appear during keyboard navigation.
    // ========================================================================
    function setupKeyboardNavDetection() {
        const html = document.documentElement;

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Tab') {
                html.classList.add('keyboard-nav');
            }
        }, true);

        document.addEventListener('pointerdown', function (e) {
            html.classList.remove('keyboard-nav');

            // Blur focused buttons/links after pointer interaction to prevent
            // lingering focus state on mobile. Skip text inputs, textareas,
            // and selects so they remain focusable for typing.
            var active = document.activeElement;
            if (active && active !== document.body) {
                var tag = active.tagName;
                if (tag !== 'INPUT' && tag !== 'TEXTAREA' && tag !== 'SELECT') {
                    active.blur();
                }
            }
        }, true);
    }

    // Set up keyboard detection immediately (before DOM ready)
    setupKeyboardNavDetection();

    // Configuration
    const SCROLL_THRESHOLD = 300; // Show buttons after scrolling 300px
    const BUTTON_CONTAINER_ID = 'nav-helper-buttons';

    // Create and inject the button container
    function createButtonContainer() {
        const container = document.createElement('div');
        container.id = BUTTON_CONTAINER_ID;
        container.className = 'nav-helper-buttons';
        container.setAttribute('aria-label', 'Navigation helpers');

        // Back to previous page button (left arrow)
        const backToPrevBtn = document.createElement('button');
        backToPrevBtn.className = 'nav-helper-btn nav-helper-btn-prev';
        backToPrevBtn.setAttribute('aria-label', 'Back to previous page');
        backToPrevBtn.setAttribute('title', 'Back to previous page');
        backToPrevBtn.innerHTML = `
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
                <path d="M12 4L6 10L12 16" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        `;
        backToPrevBtn.addEventListener('click', goToPreviousPage);

        // Back to top button (up arrow)
        const backToTopBtn = document.createElement('button');
        backToTopBtn.className = 'nav-helper-btn nav-helper-btn-top';
        backToTopBtn.setAttribute('aria-label', 'Back to top');
        backToTopBtn.setAttribute('title', 'Back to top');
        backToTopBtn.innerHTML = `
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
                <path d="M10 16L10 4M10 4L4 10M10 4L16 10" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        `;
        backToTopBtn.addEventListener('click', scrollToTop);

        container.appendChild(backToPrevBtn);
        container.appendChild(backToTopBtn);
        document.body.appendChild(container);

        return container;
    }

    // Scroll to top smoothly and reset focus
    function scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });

        // Reset focus to body after scroll completes
        // This ensures the next Tab press will naturally focus the skip-link
        // with proper :focus-visible styling (programmatic focus doesn't trigger focus-visible)
        setTimeout(() => {
            document.body.focus();
            // If body isn't focusable, make it temporarily focusable
            if (document.activeElement !== document.body) {
                document.body.setAttribute('tabindex', '-1');
                document.body.focus();
                document.body.removeAttribute('tabindex');
            }
        }, 500); // Wait for smooth scroll to complete
    }

    /**
     * Navigate to previous page in browser history
     * 
     * Simple history.back() navigation since TOC scroll-spy uses replaceState
     * instead of pushState, preventing history pollution from scroll positions.
     */
    function goToPreviousPage() {
        // Check if there's history to go back to
        if (window.history.length > 1) {
            window.history.back();
        } else {
            // If no history, go to homepage
            window.location.href = '/';
        }
    }

    // Show/hide buttons based on scroll position
    function handleScroll() {
        const container = document.getElementById(BUTTON_CONTAINER_ID);
        if (!container) return;

        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        if (scrollTop > SCROLL_THRESHOLD) {
            container.classList.add('visible');
        } else {
            container.classList.remove('visible');
        }
    }

    // Scroll handler with debouncing
    let scrollTimeout;
    function onScroll() {
        if (scrollTimeout) {
            window.cancelAnimationFrame(scrollTimeout);
        }
        scrollTimeout = window.requestAnimationFrame(handleScroll);
    }

    // Initialize
    function init() {
        // Only create container if it doesn't exist (avoid duplicates)
        if (!document.getElementById(BUTTON_CONTAINER_ID)) {
            createButtonContainer();
        }

        // Always check scroll position on init
        handleScroll();
    }

    // Listen for scroll events (set up once globally)
    window.addEventListener('scroll', onScroll, { passive: true });

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // Re-initialize after page shows (handles full page back/forward navigation)
    window.addEventListener('pageshow', init);

    // Navigation detection for scroll/focus reset.
    // Only resets on pathname changes (actual page navigation), not query/hash changes
    // (tag filters, scroll spy). The JS initializer handles re-running page scripts.
    let lastPathname = window.location.pathname;
    let isPopstateNavigation = false;

    function checkForPageNavigation() {
        const currentPathname = window.location.pathname;
        if (currentPathname === lastPathname) return;

        lastPathname = currentPathname;

        // Don't reset on back/forward — browser handles scroll restoration
        if (isPopstateNavigation) return;
        if (window.navigation?.currentEntry?.navigationType === 'traverse') return;

        resetPagePosition();
    }

    /**
     * Reset scroll and focus for forward navigation
     * This ensures Tab goes to skip link after navigating to a new page
     */
    function resetPagePosition() {
        window.scrollTo(0, 0);
        requestAnimationFrame(() => {
            document.body.tabIndex = -1;
            document.body.focus();
            document.body.removeAttribute('tabindex');
        });
    }

    // Intercept pushState to detect Blazor Router navigation
    const originalPushState = history.pushState;
    history.pushState = function (...args) {
        originalPushState.apply(this, args);
        checkForPageNavigation();
    };

    // Track popstate for back/forward detection
    window.addEventListener('popstate', () => {
        isPopstateNavigation = true;
        setTimeout(() => { isPopstateNavigation = false; }, 100);
        checkForPageNavigation();
    });

    // Also listen for enhancedload if Blazor SSR is active
    function trySetupBlazorListeners() {
        if (typeof Blazor !== 'undefined' && Blazor.addEventListener) {
            Blazor.addEventListener('enhancedload', init);
            Blazor.addEventListener('enhancedload', checkForPageNavigation);
        } else {
            const retryId = setInterval(() => {
                if (typeof Blazor !== 'undefined' && Blazor.addEventListener) {
                    clearInterval(retryId);
                    Blazor.addEventListener('enhancedload', init);
                    Blazor.addEventListener('enhancedload', checkForPageNavigation);
                }
            }, 200);
            setTimeout(() => clearInterval(retryId), 10000);
        }
    }
    trySetupBlazorListeners();
})();
