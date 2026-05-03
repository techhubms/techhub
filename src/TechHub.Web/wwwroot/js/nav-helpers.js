/**
 * Navigation Helpers - Back to Top, Back to Previous Page, Hash Link Fixes,
 * Keyboard Navigation Detection, and Scroll Position Management
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
 * Scroll Position Management:
 * Saves scroll position for every page on scroll events (throttled via rAF).
 * On back/forward navigation (traverse), restores the saved position after
 * markScriptsReady fires (signaling that all rendering is complete).
 * On forward navigation (push), scrolls to top.
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

        // Remove any #anchor from URL so F5 doesn't jump back down
        if (window.location.hash) {
            history.replaceState(null, '', window.location.pathname + window.location.search);
        }

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
    let lastPopstateAt = 0; // timestamp of last popstate (back/forward); cleared on pushState (forward)

    // ========================================================================
    // Scroll Position Management
    // Saves scroll position per page (pathname + search) on every scroll (rAF throttled).
    // On back/forward navigation (traverse), restores position after markScriptsReady.
    // On forward navigation (push), scrolls to top.
    // ========================================================================
    window.__savedScrollPositions ??= {};
    let scrollSaveScheduled = false;

    function getScrollKey() {
        return window.location.pathname + window.location.search;
    }

    function saveScrollPosition() {
        // Saves unconditionally. Post-restore scroll events save the correct restored
        // position, which is intentional and harmless.
        window.__savedScrollPositions[getScrollKey()] = window.scrollY;
    }

    // Throttled scroll save: fires at most once per animation frame.
    // This feels natural (saves ~60 times/sec max during active scroll) without
    // hammering the system. Passive listener ensures no jank.
    function onScrollSave() {
        if (scrollSaveScheduled) return;
        scrollSaveScheduled = true;
        window.requestAnimationFrame(() => {
            scrollSaveScheduled = false;
            saveScrollPosition();
        });
    }

    window.addEventListener('scroll', onScrollSave, { passive: true });

    /**
     * Restore scroll position for the current page.
     * Called by markScriptsReady (via window.__restoreScrollPosition) when all
     * rendering is complete. Only restores on back/forward navigation (traverse).
     * Uses retry logic: if the DOM isn't tall enough yet (e.g., Blazor circuit
     * still patching content under slow network), waits for DOM mutations via
     * MutationObserver (50ms debounce) and force-scrolls after a 1s timeout.
     * Returns true if position was restored or retry scheduled, false otherwise.
     */
    function restoreScrollPosition() {
        // Only restore on back/forward (traverse) navigation.
        // Forward navigations (link clicks / pushState) should start at top.
        const isTraversal = window.navigation?.currentEntry?.navigationType === 'traverse'
            || lastPopstateAt > 0;
        if (!isTraversal) return false;

        const key = getScrollKey();
        const y = window.__savedScrollPositions[key];
        if (y == null || y <= 0) return false;

        // Check if the page is already tall enough to scroll to the target.
        void document.documentElement.offsetHeight;
        const maxScroll = document.documentElement.scrollHeight - window.innerHeight;

        if (maxScroll >= y - 5) {
            // Page is tall enough — scroll immediately.
            window.scrollTo(0, y);
            window.__scrollRestoredAt = Date.now();
            return true;
        }

        // Page isn't tall enough yet (content still arriving via Blazor circuit).
        // Use MutationObserver to detect when DOM changes settle, then scroll.
        const deadline = Date.now() + 1000;
        let debounceTimer = null;
        const observer = new MutationObserver(() => {
            if (debounceTimer != null) clearTimeout(debounceTimer);
            debounceTimer = setTimeout(tryScroll, 50);
        });

        function tryScroll() {
            void document.documentElement.offsetHeight;
            const currentMax = document.documentElement.scrollHeight - window.innerHeight;

            if (currentMax >= y - 5) {
                // Page is now tall enough — scroll and stop observing.
                cleanup();
                window.scrollTo(0, y);
                window.__scrollRestoredAt = Date.now();
            } else if (Date.now() >= deadline) {
                // Deadline reached — scroll to best available position.
                cleanup();
                window.scrollTo(0, y);
                window.__scrollRestoredAt = Date.now();
            }
            // Otherwise keep observing — more mutations will come.
        }

        function cleanup() {
            observer.disconnect();
            if (debounceTimer != null) clearTimeout(debounceTimer);
            if (deadlineTimer != null) clearTimeout(deadlineTimer);
        }

        // Safety net: force scroll after 1s even if no mutations fire.
        const deadlineTimer = setTimeout(() => {
            cleanup();
            window.scrollTo(0, y);
            window.__scrollRestoredAt = Date.now();
        }, 1000);

        observer.observe(document.body, { childList: true, subtree: true });
        return true;
    }

    // Expose for markScriptsReady to call
    window.__restoreScrollPosition = restoreScrollPosition;

    function checkForPageNavigation() {
        const currentPathname = window.location.pathname;
        if (currentPathname === lastPathname) return;

        lastPathname = currentPathname;

        // Don't reset scroll on back/forward navigation — browser handles scroll restoration.
        // lastPopstateAt is set on every popstate (back/forward) and cleared on every pushState
        // (forward link click), so this guard is always active for back/forward regardless of
        // how long Blazor's enhancedload takes to fire after the popstate event.
        if (lastPopstateAt > 0) return;
        if (window.navigation?.currentEntry?.navigationType === 'traverse') return;

        resetPagePosition();
    }

    /**
     * Reset scroll and focus for forward navigation
     * This ensures Tab goes to skip link after navigating to a new page
     */
    function resetPagePosition() {
        // Primary guard: Navigation API (Chromium 102+). Back/forward navigations
        // have navigationType === 'traverse' — we must not reset scroll for those.
        if (window.navigation?.currentEntry?.navigationType === 'traverse') return;

        // Belt-and-suspenders: don't clobber a recently restored scroll position.
        // restoreScrollPosition sets __scrollRestoredAt on back-navigation.
        // This guards against the edge case where Blazor fires enhancedload *after* the
        // lastPopstateAt guard has expired, which would otherwise scroll back to top.
        if (window.__scrollRestoredAt && Date.now() - window.__scrollRestoredAt < 2000) return;

        window.scrollTo(0, 0);
        requestAnimationFrame(() => {
            document.body.tabIndex = -1;
            document.body.focus();
            document.body.removeAttribute('tabindex');
        });
    }

    // ========================================================================
    // Navigation Spinner
    // Shows a spinner during Blazor enhanced navigation (link click → page load).
    // A 500ms JS delay + 150ms CSS fade-in means it only appears for slow loads,
    // avoiding a flash on fast networks.
    //
    // Hide order (first one wins):
    //   1. markScriptsReady — fired by OnAfterRenderAsync after the circuit has
    //      fully rendered the new page (the authoritative signal).
    //   2. enhancedload — fired earlier (after DOM patch) as a fallback for pages
    //      that don't call markScriptsReady (e.g., simple pages with no scripts).
    //   3. 10s safety net.
    // ========================================================================
    const NAV_SPINNER_ID = 'nav-spinner';
    const NAV_SPINNER_SHOW_DELAY_MS = 500;  // wait before showing — hides on fast networks
    const NAV_SPINNER_SAFETY_MS = 10_000;   // force-hide if nothing else does it
    let navSpinnerShowTimeout = null;
    let navSpinnerSafetyTimeout = null;

    function createNavSpinner() {
        if (document.getElementById(NAV_SPINNER_ID)) return;
        const el = document.createElement('div');
        el.id = NAV_SPINNER_ID;
        el.className = 'nav-spinner';
        el.setAttribute('role', 'status');
        el.setAttribute('aria-label', 'Loading page');
        el.setAttribute('aria-live', 'polite');
        document.body.appendChild(el);
    }

    function showNavSpinner() {
        createNavSpinner();
        // Delay so fast navigations never flash the spinner
        navSpinnerShowTimeout = setTimeout(() => {
            navSpinnerShowTimeout = null;
            const el = document.getElementById(NAV_SPINNER_ID);
            if (el) el.classList.add('active');
        }, NAV_SPINNER_SHOW_DELAY_MS);
        // Safety net: force-hide after 10s if no other signal fires
        if (navSpinnerSafetyTimeout) clearTimeout(navSpinnerSafetyTimeout);
        navSpinnerSafetyTimeout = setTimeout(hideNavSpinner, NAV_SPINNER_SAFETY_MS);
    }

    function hideNavSpinner() {
        if (navSpinnerShowTimeout) { clearTimeout(navSpinnerShowTimeout); navSpinnerShowTimeout = null; }
        if (navSpinnerSafetyTimeout) { clearTimeout(navSpinnerSafetyTimeout); navSpinnerSafetyTimeout = null; }
        const el = document.getElementById(NAV_SPINNER_ID);
        if (el) el.classList.remove('active');
    }

    // Intercept pushState to detect Blazor Router navigation.
    // Clears lastPopstateAt so that a forward link click immediately after a back-navigation
    // correctly triggers resetPagePosition (scroll to top) on the new forward page.
    const originalPushState = history.pushState;
    history.pushState = function (...args) {
        // Save the current page's scroll position synchronously before the URL changes.
        // onScrollSave() is rAF-throttled, so on CI runners under load the rAF callback
        // may not have fired yet when the user clicks a link. Saving here guarantees the
        // most-recent scrollY is captured with the correct (pre-navigation) URL key,
        // regardless of rAF timing. This is the critical fix for scroll restoration after
        // back-navigation in production where network latency delays rAF execution.
        saveScrollPosition();
        originalPushState.apply(this, args);
        lastPopstateAt = 0;
        showNavSpinner();
        checkForPageNavigation();
    };

    // Track popstate for back/forward detection.
    // Sets lastPopstateAt so checkForPageNavigation suppresses scroll reset for the
    // duration of the back/forward navigation (until the next pushState clears it).
    window.addEventListener('popstate', () => {
        lastPopstateAt = Date.now();
        checkForPageNavigation();
    });

    // Patch markScriptsReady to also hide the spinner.
    // This is the authoritative 'page fully rendered' signal — called from
    // OnAfterRenderAsync after the Blazor circuit has completed its render.
    // We wrap rather than replace so the existing scroll-restore logic is unaffected.
    function patchMarkScriptsReady() {
        const original = window.markScriptsReady;
        window.markScriptsReady = function () {
            hideNavSpinner();
            if (typeof original === 'function') original();
        };
    }

    // markScriptsReady is defined in page-scripts.js which loads as a module.
    // Modules execute after the parser finishes, so it may not exist yet when
    // nav-helpers.js (defer) runs. Poll briefly until it appears.
    function tryPatchMarkScriptsReady() {
        if (typeof window.markScriptsReady === 'function') {
            patchMarkScriptsReady();
        } else {
            const id = setInterval(() => {
                if (typeof window.markScriptsReady === 'function') {
                    clearInterval(id);
                    patchMarkScriptsReady();
                }
            }, 50);
            setTimeout(() => clearInterval(id), 5000);
        }
    }
    tryPatchMarkScriptsReady();

    // Also listen for enhancedload if Blazor SSR is active
    function trySetupBlazorListeners() {
        if (typeof Blazor !== 'undefined' && Blazor.addEventListener) {
            Blazor.addEventListener('enhancedload', init);
            Blazor.addEventListener('enhancedload', checkForPageNavigation);
            // enhancedload is a fallback: fires before the circuit renders, so the
            // spinner may still be visible briefly after it. markScriptsReady (above)
            // is the primary hide trigger on pages that call it.
            Blazor.addEventListener('enhancedload', hideNavSpinner);
        } else {
            const retryId = setInterval(() => {
                if (typeof Blazor !== 'undefined' && Blazor.addEventListener) {
                    clearInterval(retryId);
                    Blazor.addEventListener('enhancedload', init);
                    Blazor.addEventListener('enhancedload', checkForPageNavigation);
                    Blazor.addEventListener('enhancedload', hideNavSpinner);
                }
            }, 200);
            setTimeout(() => clearInterval(retryId), 10000);
        }
    }
    trySetupBlazorListeners();

    // Expose scroll-to-top + hash clearing for Blazor JS interop.
    // Used by SubNav when clicking the already-active page link (e.g., clicking "Features"
    // while on /github-copilot/features#videos-pro). Must be in JS because Blazor Server's
    // NavigationManager.Uri doesn't include the hash fragment (fragments are client-side only).
    window.TechHub = window.TechHub || {};
    window.TechHub.scrollToTopAndClearHash = function () {
        window.scrollTo(0, 0);
        if (window.location.hash) {
            history.replaceState(null, '', window.location.pathname + window.location.search);
        }
    };
})();
