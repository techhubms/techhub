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
 *   - `scroll` event: save position, update button visibility, check infinite scroll
 *   - `scrollend` event (debounce fallback): update TOC highlight + URL hash
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

let navigating = false;           // true during navigation transitions
let lastPathname = location.pathname;
let lastSearch = location.search;
let lastPopstateAt = 0;           // >0 means back/forward nav in progress

/** Saved scroll positions keyed by pathname+search (no hash). */
const savedPositions = {};
window.__savedScrollPositions = savedPositions; // exposed for tests

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
    savedPositions[getScrollKey()] = window.scrollY;
}

/**
 * Restore scroll position after back/forward navigation.
 * Called by markScriptsReady when page rendering is complete.
 */
function restoreScrollPosition() {
    const isTraversal = window.navigation?.currentEntry?.navigationType === 'traverse'
        || lastPopstateAt > 0;
    if (!isTraversal) {
        // Forward nav: just unlock scroll handling
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
        finishNavigation();
        return false;
    }

    // Check if page is tall enough
    const maxScroll = document.documentElement.scrollHeight - window.innerHeight;
    if (maxScroll >= y - 5) {
        window.scrollTo(0, y);
        finishNavigation();
        return true;
    }

    // Page isn't tall enough — wait for content to arrive
    const deadline = Date.now() + 1000;
    let debounceTimer = null;
    let deadlineTimer = null;

    const observer = new MutationObserver(() => {
        if (debounceTimer != null) clearTimeout(debounceTimer);
        debounceTimer = setTimeout(tryScroll, 50);
    });

    function tryScroll() {
        const currentMax = document.documentElement.scrollHeight - window.innerHeight;
        if (currentMax >= y - 5 || Date.now() >= deadline) {
            cleanup();
            window.scrollTo(0, y);
            finishNavigation();
        }
    }

    function cleanup() {
        observer.disconnect();
        if (debounceTimer != null) clearTimeout(debounceTimer);
        if (deadlineTimer != null) clearTimeout(deadlineTimer);
    }

    deadlineTimer = setTimeout(() => {
        cleanup();
        window.scrollTo(0, y);
        finishNavigation();
    }, 1000);

    observer.observe(document.body, { childList: true, subtree: true });
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

function resetPagePosition() {
    if (window.navigation?.currentEntry?.navigationType === 'traverse') return;
    window.scrollTo(0, 0);
    requestAnimationFrame(() => {
        document.body.tabIndex = -1;
        document.body.focus();
        document.body.removeAttribute('tabindex');
    });
}

function checkForPageNavigation() {
    if (location.pathname === lastPathname) return;
    lastPathname = location.pathname;
    lastSearch = location.search;
    if (lastPopstateAt > 0) return;
    if (window.navigation?.currentEntry?.navigationType === 'traverse') return;
    resetPagePosition();
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
    // Save position synchronously before URL changes
    saveScrollPosition();

    const oldPathname = location.pathname;
    const oldSearch = location.search;
    originalPushState.apply(this, args);
    lastPopstateAt = 0;

    const newUrl = args[2] != null ? new URL(String(args[2]), location.href) : null;
    const isHashOnly = newUrl !== null &&
        newUrl.pathname === oldPathname &&
        newUrl.search === oldSearch;

    if (!isHashOnly) {
        // Cross-page navigation
        navigating = true;
        showNavSpinner();
    }
    // Hash-only: navigating stays false, scroll handlers keep running
};

// ============================================================================
// popstate handler
// ============================================================================

window.addEventListener('popstate', () => {
    lastPopstateAt = Date.now();
    navigating = true;

    // Same-page hash nav: pathname+search unchanged, only hash differs.
    // Blazor does NOT re-render, so markScriptsReady won't fire.
    if (location.pathname === lastPathname && location.search === lastSearch) {
        // Scroll to the hash target (or restore position if no hash)
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

    checkForPageNavigation();
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
        Blazor.addEventListener('enhancedload', () => {
            createButtonContainer();
            updateButtonVisibility();
        });
        Blazor.addEventListener('enhancedload', checkForPageNavigation);
        Blazor.addEventListener('enhancedload', hideNavSpinner);
        // Safety net: unlock scroll for forward nav if markScriptsReady fires late
        Blazor.addEventListener('enhancedload', () => {
            if (navigating && lastPopstateAt === 0) {
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

        if (isMobile) {
            tocElement.classList.add('toc-mobile-mode');
            const clickHandler = (e) => {
                const tocLink = e.target.closest('.toc-depth-0 > .toc-link');
                if (!tocLink) return;
                const tocItem = tocLink.closest('.toc-depth-0');
                if (!tocItem) return;
                const sublist = tocItem.querySelector('.toc-sublist');
                if (!sublist) return;
                e.preventDefault();
                tocItem.classList.toggle('expanded');
            };
            tocElement.addEventListener('click', clickHandler);
            tocElement._mobileClickHandler = clickHandler;
            // No scroll spy on mobile
            destroyToc();
        } else {
            tocElement.classList.remove('toc-mobile-mode');
            setupToc(tocElement, contentElement);
        }
    });
}

function setupToc(tocElement, contentElement) {
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
    };

    // Recalculate on resize
    window.addEventListener('resize', onTocResize, { passive: true });

    // Initial highlight (if not navigating)
    if (!navigating) {
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
    const { headings, headingElements, tocLinks, detectionLine, h2Items } = tocState;
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

    const { tocLinks, headingElements, h2Items } = tocState;

    // Remove current
    if (tocState.currentActiveId !== null) {
        const currentLink = tocLinks.get(tocState.currentActiveId);
        if (currentLink) currentLink.classList.remove('active');
        const currentHeading = headingElements.get(tocState.currentActiveId);
        if (currentHeading) currentHeading.classList.remove('toc-active-heading');
    }

    if (headingId === null) {
        tocState.currentActiveId = null;
        if (location.hash) {
            history.replaceState(null, '', location.pathname + location.search);
        }
        return;
    }

    const newLink = tocLinks.get(headingId);
    if (!newLink) return;

    newLink.classList.add('active');
    tocState.currentActiveId = headingId;

    // Update URL hash via replaceState (doesn't create history entry)
    history.replaceState(null, '', `${location.pathname}${location.search}#${headingId}`);

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
 */
export function observeScrollTrigger(helper, triggerElementId) {
    dispose();

    const trigger = document.getElementById(triggerElementId);
    if (!trigger) {
        console.warn('[InfiniteScroll] Trigger element not found:', triggerElementId);
        return;
    }

    infiniteScrollState = { helper, triggerElementId };

    // E2E test signals
    window.__scrollListenerReady ??= {};
    window.__scrollListenerReady[triggerElementId] = true;
    window.__scrollListenerVersion ??= {};
    const v = window.__scrollListenerVersion[triggerElementId] || 0;
    window.__scrollListenerVersion[triggerElementId] = v + 1;

    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scroll-listener:' + triggerElementId);
    console.debug('[InfiniteScroll] Scroll listener active for:', triggerElementId);

    // Check immediately if trigger is already in view
    if (!navigating) checkInfiniteScroll();
}

/**
 * Dispose infinite scroll listener. Called by Blazor on component dispose.
 */
export function dispose() {
    if (!infiniteScrollState) return;
    const { triggerElementId } = infiniteScrollState;
    window.__scrollListenerReady ??= {};
    window.__scrollListenerReady[triggerElementId] = false;
    if (typeof window.__e2eSignal === 'function') window.__e2eSignal('scroll-disposed:' + triggerElementId);
    infiniteScrollState = null;
}

function checkInfiniteScroll() {
    if (!infiniteScrollState) return;
    const { helper, triggerElementId } = infiniteScrollState;
    const el = document.getElementById(triggerElementId);
    if (!el) return;

    if (el.getBoundingClientRect().top <= window.innerHeight + TRIGGER_MARGIN_PX) {
        console.debug('[InfiniteScroll] Loading next batch');
        helper.invokeMethodAsync('LoadNextBatch');
    }
}

// ============================================================================
// Scroll Event Handlers
// ============================================================================

/**
 * Called on every scroll event (high frequency).
 * Does: save position, update button, check infinite scroll.
 * Does NOT do: TOC highlight (that's on scrollend only).
 */
function onScroll() {
    if (navigating) return;
    saveScrollPosition();
    updateButtonVisibility();
    checkInfiniteScroll();
}

/**
 * Called when scrolling stops (scrollend or debounce fallback).
 * Does: TOC highlight update.
 */
function onScrollEnd() {
    if (navigating) return;
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
