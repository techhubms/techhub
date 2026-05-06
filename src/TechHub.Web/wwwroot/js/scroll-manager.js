/**
 * Scroll Manager — Unified navigation, scroll position, TOC, and infinite scroll.
 *
 * Single module that handles:
 *   - Scroll position save/restore across navigations
 *   - Back-to-top / back-to-prev buttons
 *   - TOC scroll spy (highlight active heading in sidebar)
 *   - Infinite scroll (trigger next batch load)
 *   - Navigation spinner
 *   - Keyboard navigation detection
 *
 * Architecture:
 *   - `scroll` event: update button visibility + RAF-throttled TOC highlight + is-scrolling guard
 *   - `scrollend` event (debounce fallback): track settled position + final TOC highlight pass
 *   - `beforeenhancedload` (Blazor): scroll to top immediately before DOM patch (eliminates forward-nav jerk)
 *   - pushState intercept: fallback scroll-to-top + navigating='forward' for non-Blazor callers
 *   - popstate handler: save leaving page position, trigger traverse navigation
 *   - Explicit navigation lifecycle: onNavigationStart → onNavigationEnd
 *   - No plugin registry, no synthetic events, no RAF-inside-event races
 *
 * Scroll key uses pathname+search only (no hash). Same-page hash navigation
 * scrolls to the target element rather than restoring a saved Y position.
 *
 * @see {@link /workspaces/techhub/src/TechHub.Web/AGENTS.md} for integration patterns
 */

'use strict';

// ============================================================================
// State
// ============================================================================

let navigating = false;           // false | 'forward' | 'traverse'
let lastPathname = location.pathname;
let lastSearch = location.search;
let lastSettledScrollY = window.scrollY; // updated on scrollend — the user's true resting position

/** Saved scroll positions keyed by pathname+search (no hash). */
const savedPositions = {};
window.__savedScrollPositions = savedPositions; // exposed for tests

// RAF throttle flag for TOC updates during scroll (one update per frame).
let tocRafPending = false;

// ============================================================================
// Keyboard Navigation Detection
// ============================================================================

(function setupKeyboardNav() {
    const html = document.documentElement;
    document.addEventListener('keydown', e => {
        if (e.key === 'Tab') html.classList.add('keyboard-nav');
    }, true);
    document.addEventListener('pointerdown', () => {
        html.classList.remove('keyboard-nav');
        const active = document.activeElement;
        if (active && active !== document.body) {
            const tag = active.tagName;
            if (tag !== 'INPUT' && tag !== 'TEXTAREA' && tag !== 'SELECT') {
                active.blur();
            }
        }
    }, true);
})();

// ============================================================================
// Back-to-Top / Back-to-Prev Buttons
// ============================================================================

const SCROLL_THRESHOLD = 300;
const BUTTON_CONTAINER_ID = 'nav-helper-buttons';

function createButtonContainer() {
    if (document.getElementById(BUTTON_CONTAINER_ID)) return;
    const container = document.createElement('div');
    container.id = BUTTON_CONTAINER_ID;
    container.className = 'nav-helper-buttons';
    container.setAttribute('aria-label', 'Navigation helpers');

    const backToPrevBtn = document.createElement('button');
    backToPrevBtn.className = 'nav-helper-btn nav-helper-btn-prev';
    backToPrevBtn.setAttribute('aria-label', 'Back to previous page');
    backToPrevBtn.setAttribute('title', 'Back to previous page');
    backToPrevBtn.innerHTML = `<svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true"><path d="M12 4L6 10L12 16" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>`;
    backToPrevBtn.addEventListener('click', () => {
        if (history.length > 1) history.back();
        else location.href = '/';
    });

    const backToTopBtn = document.createElement('button');
    backToTopBtn.className = 'nav-helper-btn nav-helper-btn-top';
    backToTopBtn.setAttribute('aria-label', 'Back to top');
    backToTopBtn.setAttribute('title', 'Back to top');
    backToTopBtn.innerHTML = `<svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true"><path d="M10 16L10 4M10 4L4 10M10 4L16 10" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>`;
    backToTopBtn.addEventListener('click', scrollToTop);

    container.appendChild(backToPrevBtn);
    container.appendChild(backToTopBtn);
    document.body.appendChild(container);
}

function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    if (location.hash) {
        history.replaceState(null, '', location.pathname + location.search);
    }
    setTimeout(() => {
        document.body.focus();
        if (document.activeElement !== document.body) {
            document.body.setAttribute('tabindex', '-1');
            document.body.focus();
            document.body.removeAttribute('tabindex');
        }
    }, 500);
}

function updateButtonVisibility() {
    const container = document.getElementById(BUTTON_CONTAINER_ID);
    if (!container) return;
    if (window.scrollY > SCROLL_THRESHOLD) {
        container.classList.add('visible');
    } else {
        container.classList.remove('visible');
    }
}

// Expose for Blazor JS interop (SubNav clicks on active page link)
window.TechHub = window.TechHub || {};
window.TechHub.scrollToTopAndClearHash = function () {
    window.scrollTo(0, 0);
    if (location.hash) {
        history.replaceState(null, '', location.pathname + location.search);
    }
};

// ============================================================================
// Navigation Spinner
// ============================================================================

const NAV_SPINNER_ID = 'nav-spinner';
let navSpinnerShowTimeout = null;
let navSpinnerSafetyTimeout = null;

function showNavSpinner() {
    if (!document.getElementById(NAV_SPINNER_ID)) {
        const el = document.createElement('div');
        el.id = NAV_SPINNER_ID;
        el.className = 'nav-spinner';
        el.setAttribute('role', 'status');
        el.setAttribute('aria-label', 'Loading page');
        el.setAttribute('aria-live', 'polite');
        document.body.appendChild(el);
    }
    // Clear any in-flight show-delay so a rapid second call doesn't leave a stale
    // timeout that re-activates the spinner after hideNavSpinner() has already run.
    if (navSpinnerShowTimeout) { clearTimeout(navSpinnerShowTimeout); navSpinnerShowTimeout = null; }
    navSpinnerShowTimeout = setTimeout(() => {
        navSpinnerShowTimeout = null;
        const el = document.getElementById(NAV_SPINNER_ID);
        if (el) el.classList.add('active');
    }, 500);
    if (navSpinnerSafetyTimeout) clearTimeout(navSpinnerSafetyTimeout);
    navSpinnerSafetyTimeout = setTimeout(hideNavSpinner, 10_000);
}

function hideNavSpinner() {
    if (navSpinnerShowTimeout) { clearTimeout(navSpinnerShowTimeout); navSpinnerShowTimeout = null; }
    if (navSpinnerSafetyTimeout) { clearTimeout(navSpinnerSafetyTimeout); navSpinnerSafetyTimeout = null; }
    const el = document.getElementById(NAV_SPINNER_ID);
    if (el) el.classList.remove('active');
}

// ============================================================================
// Scroll Position Save / Restore
// ============================================================================

function getScrollKey() {
    // Key excludes hash: TOC replaceState changes hash freely, and same-page hash
    // navigation should scroll to the element rather than restore a saved Y.
    return location.pathname + location.search;
}

function saveScrollPosition() {
    const key = getScrollKey();
    const lock = window.__scrollSaveLock;
    // Priority: lock (tests) > lastSettledScrollY (user's true resting position).
    // lastSettledScrollY is immune to scrollIntoView shifts that happen between
    // the user's last scroll stop and the click/Enter that triggers pushState.
    const y = (lock && lock.key === key) ? lock.value : lastSettledScrollY;
    savedPositions[key] = y;
}

/**
 * Restore scroll position after back/forward navigation.
 * Called by markScriptsReady when page rendering is complete.
 *
 * Uses requestAnimationFrame(finishNavigation) rather than setTimeout(fn, 0) to
 * unlock scroll after scrollTo. The rendering pipeline processes IntersectionObserver
 * callbacks at step 13.10 and rAF callbacks at step 13.14 — within the same frame,
 * IO always fires BEFORE rAF. This guarantees that if scrollTo puts the infinite-scroll
 * trigger within the rootMargin, the IO fires while navigating='traverse' (→ ignored),
 * and only after that does rAF fire to set navigating=false. Without rAF, setTimeout(0)
 * races against IO and sometimes wins, causing a cascade of batch loads on back-nav.
 */
function restoreScrollPosition() {
    // Clear lock — it has served its purpose (saved the correct value in pushState).
    window.__scrollSaveLock = null;

    // Already restored or no navigation in progress — no-op.
    if (!navigating) return false;

    if (navigating !== 'traverse') {
        // Forward nav: unlock immediately. No cascading-load risk because
        // forward nav scrolls to top (trigger is at the bottom of the page).
        finishNavigation();
        return false;
    }

    const key = getScrollKey();
    const y = savedPositions[key];
    if (y == null || y <= 0) {
        // No saved position — if there's a hash, scroll to it; otherwise top.
        if (location.hash) {
            scrollToHash(location.hash);
        } else {
            window.scrollTo(0, 0);
        }
        // Use rAF instead of setTimeout(0). In the rendering pipeline, IntersectionObserver
        // callbacks fire at step 13.10 and rAF fires at step 13.14 — IO always fires BEFORE
        // rAF in the same frame. This guarantees the IO callback sees navigating='traverse'
        // before finishNavigation() resets it to false, preventing a cascade on back-nav.
        requestAnimationFrame(finishNavigation);
        return false;
    }

    // Check if page is tall enough
    const maxScroll = document.documentElement.scrollHeight - window.innerHeight;
    if (maxScroll >= y - 5) {
        window.scrollTo(0, y);
        // Use rAF instead of setTimeout(0) — see comment above for rationale.
        requestAnimationFrame(finishNavigation);
        return true;
    }

    // Page isn't tall enough — wait for layout to stabilize before scrolling.
    // Two observers run in parallel, both feeding a shared debounce timer:
    //   - ResizeObserver: fires when page height changes (e.g., images loading)
    //   - MutationObserver: fires when DOM elements are added/removed
    // Once NEITHER observer has fired for 150ms, the page is "settled" and we scroll.
    // A 30s hard deadline ensures we never hang indefinitely.
    //
    // Capture the current scroll key so that if a new navigation starts before
    // tryScroll fires, we bail out instead of scrolling the wrong page and
    // incorrectly calling finishNavigation() for the new navigation.
    const restoreKey = getScrollKey();
    let debounceTimer = null;
    let deadlineTimer = null;

    const resizeObserver = new ResizeObserver(() => {
        resetDebounce();
    });

    const mutationObserver = new MutationObserver(() => {
        resetDebounce();
    });

    function resetDebounce() {
        if (debounceTimer != null) clearTimeout(debounceTimer);
        debounceTimer = setTimeout(tryScroll, 150);
    }

    function tryScroll() {
        cleanup();
        // Bail if navigation has moved on — do not scroll the new page.
        if (getScrollKey() !== restoreKey) return;
        window.scrollTo(0, y);
        // Use rAF instead of setTimeout(0) — see comment above for rationale.
        requestAnimationFrame(finishNavigation);
    }

    function cleanup() {
        resizeObserver.disconnect();
        mutationObserver.disconnect();
        if (debounceTimer != null) clearTimeout(debounceTimer);
        if (deadlineTimer != null) clearTimeout(deadlineTimer);
    }

    // Hard deadline: after 30 seconds, scroll to whatever position is available.
    deadlineTimer = setTimeout(() => {
        cleanup();
        if (getScrollKey() !== restoreKey) return;
        window.scrollTo(0, y);
        // Use rAF instead of setTimeout(0) — see comment above for rationale.
        requestAnimationFrame(finishNavigation);
    }, 30_000);

    resizeObserver.observe(document.documentElement);
    mutationObserver.observe(document.body, { childList: true, subtree: true });
    return true;
}

window.__restoreScrollPosition = restoreScrollPosition;

// ============================================================================
// Navigation Lifecycle
// ============================================================================

/**
 * Called when navigation completes (page rendered, scroll restored).
 * Unlocks scroll handling and triggers one round of scroll-end work.
 */
function finishNavigation() {
    navigating = false;
    // Run scroll-end work once at the final position (TOC highlight, etc.)
    onScrollEnd();
}

/**
 * Scroll to a hash target element with offset for sticky header.
 */
function scrollToHash(hash) {
    const id = hash.replace('#', '');
    if (!id) return;
    const el = document.getElementById(id);
    if (el) {
        // scroll-margin-top in CSS handles the offset
        el.scrollIntoView({ behavior: 'instant' });
    } else {
        window.scrollTo(0, 0);
    }
}

// ============================================================================
// pushState intercept
// ============================================================================

const originalPushState = history.pushState;
history.pushState = function (...args) {
    // Save position synchronously before URL changes.
    // This is the ONLY place we save on forward navigation — not on every scroll event.
    // If __scrollSaveLock is set, saveScrollPosition uses the locked value.
    saveScrollPosition();

    const oldPathname = location.pathname;
    const oldSearch = location.search;
    originalPushState.apply(this, args);

    const newUrl = args[2] != null ? new URL(String(args[2]), location.href) : null;
    const isHashOnly = newUrl !== null &&
        newUrl.pathname === oldPathname &&
        newUrl.search === oldSearch;

    if (!isHashOnly) {
        // Update tracking immediately so popstate can reliably detect same-page vs cross-page.
        lastPathname = location.pathname;
        lastSearch = location.search;
        // beforeenhancedload fires before this for Blazor links and already handles
        // the early scroll + navigating setup. This branch is the fallback for
        // non-Blazor pushState callers or for the rare case the event didn't fire.
        if (navigating !== 'forward') {
            document.documentElement.classList.remove('is-scrolling');
            window.scrollTo({ top: 0, behavior: 'instant' });
            lastSettledScrollY = 0;
            navigating = 'forward';
            // Block WaitForBlazorReadyAsync immediately — don't wait for enhancedload.
            window.__scriptsReady = false;
            showNavSpinner();
        }
    }
    // Hash-only: navigating stays false, scroll handlers keep running
};

// ============================================================================
// popstate handler
// ============================================================================

window.addEventListener('popstate', () => {
    // Save the position of the page we're LEAVING.
    // At this point, location has already changed to the destination, but
    // lastPathname/lastSearch still identify the page we're navigating away from.
    // Use lastSettledScrollY for the same reason as pushState: immune to late shifts.
    const leavingKey = lastPathname + lastSearch;
    savedPositions[leavingKey] = lastSettledScrollY;

    navigating = 'traverse';

    // Same-page hash nav: pathname+search unchanged, only hash differs.
    // Blazor does NOT re-render, so markScriptsReady won't fire.
    if (location.pathname === lastPathname && location.search === lastSearch) {
        if (location.hash) {
            scrollToHash(location.hash);
        } else {
            // Back to the page without a hash — restore saved position
            const y = savedPositions[getScrollKey()];
            if (y != null && y > 0) {
                window.scrollTo(0, y);
            } else {
                window.scrollTo(0, 0);
            }
        }
        finishNavigation();
        return;
    }

    // Cross-page back/forward navigation.
    lastPathname = location.pathname;
    lastSearch = location.search;

    // Block WaitForBlazorReadyAsync immediately.
    window.__scriptsReady = false;
});

// ============================================================================
// Blazor lifecycle hooks
// ============================================================================

function patchMarkScriptsReady() {
    const original = window.markScriptsReady;
    window.markScriptsReady = function () {
        hideNavSpinner();
        if (typeof original === 'function') original();
    };
}

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

function setupBlazorListeners() {
    if (typeof Blazor !== 'undefined' && Blazor.addEventListener) {
        attach();
    } else {
        const retryId = setInterval(() => {
            if (typeof Blazor !== 'undefined' && Blazor.addEventListener) {
                clearInterval(retryId);
                attach();
            }
        }, 200);
        setTimeout(() => clearInterval(retryId), 10000);
    }

    function attach() {
        // beforeenhancedload fires BEFORE Blazor patches the DOM and BEFORE pushState
        // is called (Blazor Interactive Server calls pushState after the circuit update).
        // Scrolling to top here gives the user an instant visual jump on link click,
        // eliminating the jerk where the old content scrolled while DOM was being patched.
        Blazor.addEventListener('beforeenhancedload', () => {
            // traversal (back/forward) sets navigating='traverse' via popstate before
            // this event fires, so the guard prevents us resetting scroll on back-nav.
            if (navigating) return;
            document.documentElement.classList.remove('is-scrolling');
            window.scrollTo({ top: 0, behavior: 'instant' });
            lastSettledScrollY = 0;
            navigating = 'forward';
            window.__scriptsReady = false;
            showNavSpinner();
        });

        Blazor.addEventListener('enhancedload', () => {
            createButtonContainer();
            updateButtonVisibility();
            hideNavSpinner();
            // Forward nav completion: scroll was already done in beforeenhancedload
            // (or pushState fallback). Only move focus for accessibility.
            if (navigating === 'forward') {
                requestAnimationFrame(() => {
                    document.body.tabIndex = -1;
                    document.body.focus();
                    document.body.removeAttribute('tabindex');
                });
                finishNavigation();
            }
        });
    }
}
setupBlazorListeners();

// ============================================================================
// TOC Scroll Spy
// ============================================================================

const STICKY_HEADER_OFFSET = 106;
const DETECTION_LINE_PERCENT = 0.30;
const TABLET_BREAKPOINT = 1292;

let tocState = null; // non-null when TOC is active

/**
 * Activate TOC scroll spy. Called by page-scripts.js / Blazor interop.
 */
export function initTocScrollSpy() {
    const isMobile = window.innerWidth <= TABLET_BREAKPOINT;
    const tocElements = document.querySelectorAll('[data-toc-scroll-spy]');

    tocElements.forEach(tocElement => {
        const contentSelector = tocElement.getAttribute('data-content-selector');
        if (!contentSelector) return;
        const contentElement = document.querySelector(contentSelector);
        if (!contentElement) return;

        // Clean up previous
        if (tocElement._mobileClickHandler) {
            tocElement.removeEventListener('click', tocElement._mobileClickHandler);
            tocElement._mobileClickHandler = null;
        }
        if (tocElement._desktopClickHandler) {
            tocElement.removeEventListener('click', tocElement._desktopClickHandler);
            tocElement._desktopClickHandler = null;
        }

        if (isMobile) {
            tocElement.classList.add('toc-mobile-mode');
            const clickHandler = (e) => {
                const link = e.target.closest('a[href]');
                if (!link) return;
                // Match bare hash links (#id) and same-page full-path hash links (/path#id).
                // Bare hash links are always same-page; full-path links need URL comparison.
                const hrefAttr = link.getAttribute('href');
                if (!hrefAttr || !hrefAttr.includes('#')) return;
                let hash;
                if (hrefAttr.startsWith('#')) {
                    hash = hrefAttr;
                } else {
                    try {
                        const url = new URL(hrefAttr, location.href);
                        if (url.origin !== location.origin) return;
                        if (url.pathname !== location.pathname || url.search !== location.search) return;
                        if (!url.hash) return;
                        hash = url.hash;
                    } catch {
                        return;
                    }
                }
                e.preventDefault();
                // Top-level link with a sublist: toggle expand/collapse, don't scroll.
                const tocItem = link.closest('.toc-depth-0');
                if (link.matches('.toc-depth-0 > .toc-link') && tocItem?.querySelector('.toc-sublist')) {
                    tocItem.classList.toggle('expanded');
                    return;
                }
                // All other TOC anchor clicks (h2 without children, h3 sub-items):
                // use replaceState so they don't push history entries.
                history.replaceState(null, '', location.pathname + location.search + hash);
                scrollToHash(hash);
                // Directly activate the clicked link by ID — more reliable than position
                // detection (updateTocHighlight), which may select a nearby heading instead.
                const clickedId = hash.replace('#', '');
                requestAnimationFrame(() => setTocActive(clickedId));
            };
            tocElement.addEventListener('click', clickHandler);
            tocElement._mobileClickHandler = clickHandler;
            setupToc(tocElement, contentElement, false);
        } else {
            tocElement.classList.remove('toc-mobile-mode');
            // TOC clicks are "scroll to section", not page navigations — use replaceState
            // so they don't accumulate history entries and break the back button.
            const desktopClickHandler = (e) => {
                const link = e.target.closest('a[href]');
                if (!link) return;
                // Match bare hash links (#id) and same-page full-path hash links (/path#id).
                // SidebarToc.razor generates full-path hrefs like "/github-copilot/handbook#about-book".
                // Our handler fires before Blazor's document-level interceptor (event bubbling),
                // so e.preventDefault() prevents Blazor from calling pushState.
                const hrefAttr = link.getAttribute('href');
                if (!hrefAttr || !hrefAttr.includes('#')) return;
                let hash;
                if (hrefAttr.startsWith('#')) {
                    hash = hrefAttr;
                } else {
                    try {
                        const url = new URL(hrefAttr, location.href);
                        if (url.origin !== location.origin) return;
                        if (url.pathname !== location.pathname || url.search !== location.search) return;
                        if (!url.hash) return;
                        hash = url.hash;
                    } catch {
                        return;
                    }
                }
                e.preventDefault();
                history.replaceState(null, '', location.pathname + location.search + hash);
                scrollToHash(hash);
                // Directly activate the clicked link by ID — more reliable than position
                // detection (updateTocHighlight), which may select a nearby heading instead.
                const clickedId = hash.replace('#', '');
                requestAnimationFrame(() => setTocActive(clickedId));
            };
            tocElement.addEventListener('click', desktopClickHandler);
            tocElement._desktopClickHandler = desktopClickHandler;
            setupToc(tocElement, contentElement);
        }
    });
}

function setupToc(tocElement, contentElement, initialHighlight = true) {
    // Destroy previous if any
    destroyToc();

    const headings = Array.from(contentElement.querySelectorAll('h2[id], h3[id]'));
    if (headings.length === 0) return;

    const tocLinks = new Map();
    const headingElements = new Map();
    headings.forEach(heading => {
        const id = heading.getAttribute('id');
        const link = tocElement.querySelector(`a[href="#${id}"], a[href$="#${id}"]`);
        if (link) {
            tocLinks.set(id, link);
            headingElements.set(id, heading);
        }
    });

    const h2Items = Array.from(tocElement.querySelectorAll('.toc-depth-0'));
    const contentHeight = window.innerHeight - STICKY_HEADER_OFFSET;
    const detectionLine = STICKY_HEADER_OFFSET + (contentHeight * DETECTION_LINE_PERCENT);

    tocState = {
        tocElement,
        contentElement,
        headings,
        tocLinks,
        headingElements,
        h2Items,
        detectionLine,
        currentActiveId: null,
        currentActiveH2Id: null,
        // Snapshot the page at init time. replaceState is skipped if we've
        // navigated away before scrollend fires (prevents hash leaking onto
        // the next page when the user clicks Back mid-scroll).
        pageKey: location.pathname + location.search,
    };

    // Recalculate on resize
    window.addEventListener('resize', onTocResize, { passive: true });

    // Initial highlight (if not navigating and caller requested it).
    // Mobile mode passes initialHighlight=false so the expand/collapse state
    // is not pre-set before any user interaction.
    if (!navigating && initialHighlight) {
        updateTocHighlight();
    }

    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('toc-initialized');
}

function destroyToc() {
    if (!tocState) return;
    window.removeEventListener('resize', onTocResize);
    tocState = null;
}

function onTocResize() {
    if (!tocState) return;
    const contentHeight = window.innerHeight - STICKY_HEADER_OFFSET;
    tocState.detectionLine = STICKY_HEADER_OFFSET + (contentHeight * DETECTION_LINE_PERCENT);
    updateTocHighlight();
}

function updateTocHighlight() {
    if (!tocState) return;
    const { headings, headingElements, tocLinks, detectionLine } = tocState;
    const tolerance = 5;

    // Bottom-of-page: activate last heading
    const scrollBottom = window.innerHeight + window.scrollY;
    const pageHeight = document.documentElement.scrollHeight;
    if (pageHeight - scrollBottom < 50 && headings.length > 0) {
        const lastHeading = headings[headings.length - 1];
        const lastId = lastHeading.getAttribute('id');
        if (lastId && tocLinks.has(lastId)) {
            setTocActive(lastId);
            return;
        }
    }

    // Find heading closest to detection line (above it)
    let targetId = null;
    let closestDistance = Infinity;

    for (const [headingId, heading] of headingElements) {
        const top = heading.getBoundingClientRect().top;
        if (top <= detectionLine + tolerance) {
            const distance = Math.abs(detectionLine - top);
            if (distance < closestDistance) {
                closestDistance = distance;
                targetId = headingId;
            }
        }
    }

    if (!targetId) {
        if (tocState.currentActiveId !== null) {
            setTocActive(null);
        }
        return;
    }

    setTocActive(targetId);
}

function setTocActive(headingId) {
    if (!tocState) return;
    if (tocState.currentActiveId === headingId) return;

    const { tocLinks, headingElements } = tocState;

    // Remove current
    if (tocState.currentActiveId !== null) {
        const currentLink = tocLinks.get(tocState.currentActiveId);
        if (currentLink) currentLink.classList.remove('active');
        const currentHeading = headingElements.get(tocState.currentActiveId);
        if (currentHeading) currentHeading.classList.remove('toc-active-heading');
    }

    if (headingId === null) {
        tocState.currentActiveId = null;
        // Only clear the hash if we're still on the page where the TOC was
        // initialized — guard against a late scrollend firing after navigation.
        if (location.hash && location.pathname + location.search === tocState.pageKey) {
            history.replaceState(null, '', location.pathname + location.search);
        }
        return;
    }

    const newLink = tocLinks.get(headingId);
    if (!newLink) return;

    newLink.classList.add('active');
    tocState.currentActiveId = headingId;

    // Update URL hash — but only if we're still on the page where the TOC was
    // initialized. If the user clicked a link and a late scrollend fires just
    // before the DOM swap, we must not overwrite the new page's URL with a hash
    // from the previous page (which would then prevent scroll-position restore).
    if (location.pathname + location.search === tocState.pageKey) {
        history.replaceState(null, '', `${location.pathname}${location.search}#${headingId}`);
    }

    // Add active class to heading
    const newHeading = headingElements.get(headingId);
    if (newHeading) newHeading.classList.add('toc-active-heading');

    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('toc-active-updated');

    // Update collapse/expand state
    updateTocCollapseState(headingId);
}

function updateTocCollapseState(activeHeadingId) {
    if (!tocState) return;
    const { headings, headingElements, tocLinks, h2Items } = tocState;

    const activeHeading = headingElements.get(activeHeadingId);
    if (!activeHeading) return;

    const headingLevel = parseInt(activeHeading.tagName.substring(1));
    let targetH2Id = activeHeadingId;

    if (headingLevel === 3) {
        const activeIndex = headings.indexOf(activeHeading);
        if (activeIndex === -1) return;
        for (let i = activeIndex - 1; i >= 0; i--) {
            if (parseInt(headings[i].tagName.substring(1)) === 2) {
                targetH2Id = headings[i].getAttribute('id');
                break;
            }
        }
    }

    const activeTocLink = tocLinks.get(targetH2Id);
    if (!activeTocLink) return;

    if (tocState.currentActiveH2Id === targetH2Id) return;

    const activeItem = activeTocLink.closest('.toc-depth-0');
    if (activeItem) activeItem.classList.add('expanded');

    h2Items.forEach(item => {
        if (item !== activeItem) item.classList.remove('expanded');
    });

    tocState.currentActiveH2Id = targetH2Id;
}

// ============================================================================
// Infinite Scroll
// ============================================================================

let infiniteScrollState = null; // non-null when active

const TRIGGER_MARGIN_PX = 300;

/**
 * Set up infinite scroll monitoring. Called by Blazor ContentItemsGrid component.
 *
 * Uses IntersectionObserver for edge-triggered detection: the callback fires
 * exactly once when the trigger element ENTERS the viewport margin. After firing,
 * the observer is disconnected. Blazor re-calls this after each batch render,
 * creating a fresh observer — so cascade is architecturally impossible:
 * the trigger must leave and re-enter the zone for another batch to load.
 */
export function observeScrollTrigger(helper, triggerElementId) {
    dispose(); // disconnect any previous observer

    const trigger = document.getElementById(triggerElementId);
    if (!trigger) {
        console.warn('[InfiniteScroll] Trigger element not found:', triggerElementId);
        return;
    }

    const observer = new IntersectionObserver(entries => {
        const entry = entries[entries.length - 1];
        if (!entry.isIntersecting) return;
        if (navigating) return; // nav in progress — back/forward nav; observer stays active
        // Trigger entered the viewport+margin — disconnect immediately so we
        // cannot fire again while the batch loads. observeScrollTrigger will be
        // called again by Blazor after the next render, creating a new observer.
        observer.disconnect();
        infiniteScrollState = null;
        helper.invokeMethodAsync('LoadNextBatch');
    }, { rootMargin: `0px 0px ${TRIGGER_MARGIN_PX}px 0px` });

    observer.observe(trigger);
    infiniteScrollState = { helper, triggerElementId, observer };

    // E2E test signals
    window.__scrollListenerReady ??= {};
    window.__scrollListenerReady[triggerElementId] = true;
    window.__scrollListenerVersion ??= {};
    const v = window.__scrollListenerVersion[triggerElementId] || 0;
    window.__scrollListenerVersion[triggerElementId] = v + 1;

    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scroll-listener:' + triggerElementId);
}

/**
 * Dispose infinite scroll listener. Called by Blazor on component dispose.
 */
export function dispose() {
    if (!infiniteScrollState) return;
    const { triggerElementId, observer } = infiniteScrollState;
    observer.disconnect();
    window.__scrollListenerReady ??= {};
    window.__scrollListenerReady[triggerElementId] = false;
    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scroll-disposed:' + triggerElementId);
    infiniteScrollState = null;
}

// ============================================================================
// Scroll Event Handlers
// ============================================================================

/**
 * Called on every scroll event (high frequency).
 * Does: update button visibility, RAF-throttled TOC highlight, pointer-events guard for cards.
 * Does NOT do: save position (only saved in pushState).
 */
function onScroll() {
    if (navigating) return;
    // Suppress card hover visuals during scroll (CSS resets them while
    // is-scrolling is present). Pointer-events stay on so the browser tracks
    // the cursor; when is-scrolling is removed in onScrollEnd the correct
    // card highlights immediately without requiring a mouse move.
    document.documentElement.classList.add('is-scrolling');
    updateButtonVisibility();
    // RAF-throttled TOC update — one update per frame, matching the old
    // toc-scroll-spy.js behaviour. scrollend-only was the regression.
    if (tocState && !tocRafPending) {
        tocRafPending = true;
        requestAnimationFrame(() => {
            tocRafPending = false;
            if (!navigating) updateTocHighlight();
        });
    }
}

/**
 * Called when scrolling stops (scrollend or debounce fallback).
 * Does: remove is-scrolling guard, update lastSettledScrollY, final TOC highlight pass.
 */
function onScrollEnd() {
    if (navigating) return;
    document.documentElement.classList.remove('is-scrolling');
    lastSettledScrollY = window.scrollY;
    updateTocHighlight();
}

// Single scroll listener
window.addEventListener('scroll', onScroll, { passive: true });

// scrollend for TOC (fires once when scrolling stops — perfect for TOC)
let scrollEndDebounceTimer = null;
if ('onscrollend' in window) {
    window.addEventListener('scrollend', onScrollEnd, { passive: true });
} else {
    // Fallback: debounced scroll → simulate scrollend
    window.addEventListener('scroll', () => {
        if (navigating) return;
        if (scrollEndDebounceTimer) clearTimeout(scrollEndDebounceTimer);
        scrollEndDebounceTimer = setTimeout(onScrollEnd, 150);
    }, { passive: true });
}

// ============================================================================
// Exports for Blazor / page-scripts.js
// ============================================================================

/**
 * Returns true while navigation is in progress.
 * Exported for tests and potential future use.
 */
export function isNavigating() {
    return navigating;
}

// ============================================================================
// Init
// ============================================================================

function init() {
    createButtonContainer();
    updateButtonVisibility();
}

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
} else {
    init();
}

window.addEventListener('pageshow', init);
