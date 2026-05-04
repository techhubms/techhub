import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/nav-helpers.js';

describe('nav-helpers.js', () => {
    let originalRAF;

    beforeEach(() => {
        document.body.innerHTML = '';
        document.documentElement.className = '';

        delete window.TechHub;
        delete window.__scrollRestoredAt;
        delete window.__savedScrollPositions;
        delete window.__restoreScrollPosition;
        delete window.__scrollRestoring;
        // Reset Navigation API so tests that set window.navigation don't leak state
        // into subsequent tests and cause order-dependent failures.
        delete window.navigation;

        // Stub Blazor with addEventListener so nav-helpers.js can attach enhancedload
        // listeners immediately without entering the 200ms setInterval retry loop.
        // Without this stub, each import registers a long-lived interval/timeout pair
        // (200ms polling, 10s max) that keeps the Vitest process alive and makes the
        // suite slow and flaky.
        globalThis.Blazor = { addEventListener: vi.fn() };

        // Pre-stub markScriptsReady so tryPatchMarkScriptsReady() wraps it immediately
        // during module import instead of creating a 50ms polling setInterval (+ 5s
        // safety timeout). Prevents those timers — and the navSpinner timers started by
        // history.pushState() in scroll tests — from keeping the Vitest event loop alive.
        window.markScriptsReady = vi.fn();

        Object.defineProperty(window, 'scrollY', {
            value: 0,
            writable: true,
            configurable: true,
        });

        Object.defineProperty(window, 'pageYOffset', {
            value: 0,
            writable: true,
            configurable: true,
        });

        window.scrollTo = vi.fn((_x, y) => {
            // Simulate browser behavior: scrollTo updates scrollY when page is tall enough
            Object.defineProperty(window, 'scrollY', { value: y, writable: true, configurable: true });
        });

        // Make the DOM "tall enough" so scroll restore logic doesn't retry infinitely.
        Object.defineProperty(document.documentElement, 'scrollHeight', {
            value: 10000,
            writable: true,
            configurable: true,
        });

        // Mock history
        window.history.replaceState = vi.fn();
        window.history.back = vi.fn();
        Object.defineProperty(window.history, 'length', {
            value: 5,
            writable: true,
            configurable: true,
        });

        // Mock location
        Object.defineProperty(window, 'location', {
            value: {
                pathname: '/all',
                search: '',
                hash: '',
                href: 'https://localhost/all',
            },
            writable: true,
            configurable: true,
        });

        // Ensure rAF executes synchronously for tests
        originalRAF = window.requestAnimationFrame;
        window.requestAnimationFrame = (cb) => { cb(); return 1; };
        window.cancelAnimationFrame = vi.fn();

        vi.resetModules();
    });

    afterEach(() => {
        window.requestAnimationFrame = originalRAF;
        delete globalThis.Blazor;
        vi.restoreAllMocks();
    });

    describe('keyboard navigation detection', () => {
        it('should add keyboard-nav class on Tab key', async () => {
            await import(MODULE_PATH);

            const event = new KeyboardEvent('keydown', { key: 'Tab', bubbles: true });
            document.dispatchEvent(event);

            expect(document.documentElement.classList.contains('keyboard-nav')).toBe(true);
        });

        it('should remove keyboard-nav class on pointerdown', async () => {
            await import(MODULE_PATH);

            // First, add keyboard-nav
            document.dispatchEvent(new KeyboardEvent('keydown', { key: 'Tab', bubbles: true }));
            expect(document.documentElement.classList.contains('keyboard-nav')).toBe(true);

            // Then, pointer interaction removes it
            document.dispatchEvent(new Event('pointerdown', { bubbles: true }));

            expect(document.documentElement.classList.contains('keyboard-nav')).toBe(false);
        });

        it('should not add keyboard-nav for non-Tab keys', async () => {
            await import(MODULE_PATH);

            document.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true }));

            expect(document.documentElement.classList.contains('keyboard-nav')).toBe(false);
        });
    });

    describe('button creation', () => {
        it('should create nav-helper-buttons container', async () => {
            await import(MODULE_PATH);

            const container = document.getElementById('nav-helper-buttons');
            expect(container).not.toBeNull();
            expect(container.className).toBe('nav-helper-buttons');
        });

        it('should create back-to-top button', async () => {
            await import(MODULE_PATH);

            const btn = document.querySelector('.nav-helper-btn-top');
            expect(btn).not.toBeNull();
            expect(btn.getAttribute('aria-label')).toBe('Back to top');
        });

        it('should create back-to-previous button', async () => {
            await import(MODULE_PATH);

            const btn = document.querySelector('.nav-helper-btn-prev');
            expect(btn).not.toBeNull();
            expect(btn.getAttribute('aria-label')).toBe('Back to previous page');
        });

        it('should not create duplicate containers', async () => {
            await import(MODULE_PATH);

            // Simulate re-init (e.g., after enhancedload)
            window.dispatchEvent(new Event('pageshow'));

            const containers = document.querySelectorAll('#nav-helper-buttons');
            expect(containers.length).toBe(1);
        });
    });

    describe('scroll visibility', () => {
        it('should show buttons when scrolled past threshold (300px)', async () => {
            await import(MODULE_PATH);

            Object.defineProperty(window, 'pageYOffset', { value: 350, configurable: true });
            Object.defineProperty(document.documentElement, 'scrollTop', { value: 350, configurable: true });

            window.dispatchEvent(new Event('scroll'));

            const container = document.getElementById('nav-helper-buttons');
            expect(container.classList.contains('visible')).toBe(true);
        });

        it('should hide buttons when scrolled to top', async () => {
            await import(MODULE_PATH);

            // First scroll past threshold
            Object.defineProperty(window, 'pageYOffset', { value: 350, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            // Then scroll back to top
            Object.defineProperty(window, 'pageYOffset', { value: 50, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            const container = document.getElementById('nav-helper-buttons');
            expect(container.classList.contains('visible')).toBe(false);
        });
    });

    describe('back-to-top', () => {
        it('should scroll to top when clicked', async () => {
            await import(MODULE_PATH);

            const btn = document.querySelector('.nav-helper-btn-top');
            btn.click();

            expect(window.scrollTo).toHaveBeenCalledWith({
                top: 0,
                behavior: 'smooth',
            });
        });

        it('should clear hash from URL when scrolling to top', async () => {
            window.location.hash = '#section1';
            await import(MODULE_PATH);

            const btn = document.querySelector('.nav-helper-btn-top');
            btn.click();

            expect(window.history.replaceState).toHaveBeenCalledWith(
                null, '', '/all'
            );
        });
    });

    describe('back-to-previous', () => {
        it('should call history.back when history exists', async () => {
            await import(MODULE_PATH);

            const btn = document.querySelector('.nav-helper-btn-prev');
            btn.click();

            expect(window.history.back).toHaveBeenCalled();
        });

        it('should navigate to homepage when no history', async () => {
            Object.defineProperty(window.history, 'length', { value: 1, configurable: true });
            await import(MODULE_PATH);

            const btn = document.querySelector('.nav-helper-btn-prev');
            btn.click();

            expect(window.location.href).toBe('/');
        });
    });

    describe('TechHub.scrollToTopAndClearHash', () => {
        it('should expose scrollToTopAndClearHash globally', async () => {
            await import(MODULE_PATH);

            expect(typeof window.TechHub.scrollToTopAndClearHash).toBe('function');
        });

        it('should scroll to top and clear hash', async () => {
            window.location.hash = '#something';
            await import(MODULE_PATH);

            window.TechHub.scrollToTopAndClearHash();

            expect(window.scrollTo).toHaveBeenCalledWith(0, 0);
            expect(window.history.replaceState).toHaveBeenCalledWith(
                null, '', '/all'
            );
        });
    });

    describe('scroll position management', () => {
        it('should save position on scroll events (rAF-throttled)', async () => {
            await import(MODULE_PATH);

            Object.defineProperty(window, 'scrollY', { value: 500, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            expect(window.__savedScrollPositions['/all']).toBe(500);
        });

        it('should save scroll position synchronously on pushState', async () => {
            await import(MODULE_PATH);

            // User scrolls to mid-page; rAF mock fires synchronously so position is saved.
            Object.defineProperty(window, 'scrollY', { value: 1234, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            // User scrolls further before the next rAF fires, then clicks a link.
            // pushState must capture the latest position immediately without waiting for rAF.
            Object.defineProperty(window, 'scrollY', { value: 1500, writable: true, configurable: true });
            window.history.pushState('/detail', '', '/detail');
            // Call markScriptsReady to clear the navSpinner timers started by pushState
            // (500ms show + 10s safety) so they don't keep the Vitest event loop alive.
            window.markScriptsReady();

            // The synchronous save in the pushState interceptor captures 1500 for '/all'.
            expect(window.__savedScrollPositions['/all']).toBe(1500);
        });

        it('should save correct page key when pushState fires (old URL, not new URL)', async () => {
            await import(MODULE_PATH);

            // Current page is /all, user is scrolled to 2000
            Object.defineProperty(window, 'scrollY', { value: 2000, writable: true, configurable: true });

            // Fire pushState to navigate to /detail
            window.history.pushState('/detail', '', '/detail');
            // Clear navSpinner timers (500ms show + 10s safety) started by pushState.
            window.markScriptsReady();
            // Explicitly update the location stub to reflect the URL change — makes the
            // test deterministic regardless of how jsdom handles pushState on plain objects.
            window.location.pathname = '/detail';
            window.location.search = '';
            expect(window.location.pathname).toBe('/detail');

            // Save must use the OLD URL key '/all' (captured before URL changes)
            expect(window.__savedScrollPositions['/all']).toBe(2000);
            // The new page '/detail' should not have been saved yet
            expect(window.__savedScrollPositions['/detail']).toBeUndefined();
        });

        it('should save position keyed by pathname + search + hash', async () => {
            await import(MODULE_PATH);

            Object.defineProperty(window, 'scrollY', { value: 300, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            // Change location and scroll again
            window.location.pathname = '/github-copilot';
            window.location.search = '?tags=news';
            window.location.hash = '#section';

            Object.defineProperty(window, 'scrollY', { value: 700, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            expect(window.__savedScrollPositions['/all']).toBe(300);
            expect(window.__savedScrollPositions['/github-copilot?tags=news#section']).toBe(700);
        });

        it('should expose __restoreScrollPosition globally', async () => {
            await import(MODULE_PATH);

            expect(typeof window.__restoreScrollPosition).toBe('function');
        });

        it('should restore scroll position on traverse navigation', async () => {
            // Set up navigation API to indicate traverse
            window.navigation = {
                currentEntry: { navigationType: 'traverse' },
            };

            await import(MODULE_PATH);

            // Pre-save a position
            window.__savedScrollPositions['/all'] = 450;

            const result = window.__restoreScrollPosition();

            expect(result).toBe(true);
            expect(window.scrollTo).toHaveBeenCalledWith(0, 450);
        });

        it('should NOT restore on push navigation', async () => {
            // No traverse flag, no popstate
            window.navigation = {
                currentEntry: { navigationType: 'push' },
            };

            await import(MODULE_PATH);

            window.__savedScrollPositions['/all'] = 450;

            const result = window.__restoreScrollPosition();

            expect(result).toBe(false);
            // scrollTo may have been called by init logic, but not with 450
            const calls = window.scrollTo.mock.calls;
            const hasRestoredTo450 = calls.some(c => c[0] === 0 && c[1] === 450);
            expect(hasRestoredTo450).toBe(false);
        });

        it('should return false when no saved position exists', async () => {
            window.navigation = {
                currentEntry: { navigationType: 'traverse' },
            };

            await import(MODULE_PATH);

            const result = window.__restoreScrollPosition();

            expect(result).toBe(false);
        });

        it('should set __scrollRestoredAt after restoring', async () => {
            window.navigation = {
                currentEntry: { navigationType: 'traverse' },
            };

            await import(MODULE_PATH);

            window.__savedScrollPositions['/all'] = 600;
            const before = Date.now();

            window.__restoreScrollPosition();

            expect(window.__scrollRestoredAt).toBeGreaterThanOrEqual(before);
            expect(window.__scrollRestoredAt).toBeLessThanOrEqual(Date.now());
        });

        it('should restore via popstate flag when Navigation API unavailable', async () => {
            // No Navigation API
            delete window.navigation;

            await import(MODULE_PATH);

            // Simulate popstate (back button)
            window.dispatchEvent(new Event('popstate'));

            window.__savedScrollPositions['/all'] = 800;

            const result = window.__restoreScrollPosition();

            expect(result).toBe(true);
            expect(window.scrollTo).toHaveBeenCalledWith(0, 800);
        });

        it('should defer scroll when page is too short and retry after DOM mutation', async () => {
            window.navigation = {
                currentEntry: { navigationType: 'traverse' },
            };

            // Page starts too short — scrollHeight < target
            Object.defineProperty(document.documentElement, 'scrollHeight', {
                value: 200,
                writable: true,
                configurable: true,
            });
            Object.defineProperty(window, 'innerHeight', {
                value: 800,
                writable: true,
                configurable: true,
            });

            await import(MODULE_PATH);

            window.__savedScrollPositions['/all'] = 2000;
            window.scrollTo.mockClear();

            const result = window.__restoreScrollPosition();

            // Should return true (deferred restore scheduled) but NOT scroll yet
            expect(result).toBe(true);
            expect(window.scrollTo).not.toHaveBeenCalled();

            // Simulate DOM growing tall enough
            Object.defineProperty(document.documentElement, 'scrollHeight', {
                value: 5000,
                writable: true,
                configurable: true,
            });

            // Trigger a DOM mutation (MutationObserver fires)
            const el = document.createElement('div');
            document.body.appendChild(el);

            // Wait for the 50ms debounce
            await new Promise(r => setTimeout(r, 60));

            expect(window.scrollTo).toHaveBeenCalledWith(0, 2000);
            expect(window.__scrollRestoredAt).toBeGreaterThan(0);
        });

        it('popstate sets __scrollRestoring = true', async () => {
            await import(MODULE_PATH); // captures lastPathname = '/all' (from beforeEach mock)

            // Simulate back-nav from a different page: change window.location so that
            // pathname differs from lastPathname, triggering the cross-page branch.
            // (window.history.replaceState is mocked as vi.fn() and won't update location.)
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));

            expect(window.__scrollRestoring).toBe(true);
        });

        it('pushState clears __scrollRestoring', async () => {
            await import(MODULE_PATH); // captures lastPathname = '/all' (from beforeEach mock)

            // Simulate cross-page back-nav to set __scrollRestoring = true.
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(window.__scrollRestoring).toBe(true);

            window.history.pushState('/detail', '', '/detail');
            window.markScriptsReady(); // clear navSpinner timers

            expect(window.__scrollRestoring).toBe(false);
        });

        it('restoreScrollPosition immediate path clears __scrollRestoring and dispatches scroll', async () => {
            window.navigation = {
                currentEntry: { navigationType: 'traverse' },
            };
            await import(MODULE_PATH);

            window.__scrollRestoring = true;
            window.__savedScrollPositions['/all'] = 400;

            const scrollEvents = [];
            window.addEventListener('scroll', () => scrollEvents.push(window.scrollY), { passive: true });

            window.__restoreScrollPosition();

            expect(window.__scrollRestoring).toBe(false);
            // finishScrollRestore dispatches a synthetic scroll event
            expect(scrollEvents.length).toBeGreaterThanOrEqual(1);
        });

        it('restoreScrollPosition deferred path clears __scrollRestoring after MutationObserver fires', async () => {
            window.navigation = {
                currentEntry: { navigationType: 'traverse' },
            };

            // Page starts too short
            Object.defineProperty(document.documentElement, 'scrollHeight', {
                value: 200,
                writable: true,
                configurable: true,
            });
            Object.defineProperty(window, 'innerHeight', {
                value: 800,
                writable: true,
                configurable: true,
            });

            await import(MODULE_PATH);

            window.__scrollRestoring = true;
            window.__savedScrollPositions['/all'] = 2000;
            window.scrollTo.mockClear();

            window.__restoreScrollPosition();
            expect(window.__scrollRestoring).toBe(true); // still paused

            // DOM grows tall enough
            Object.defineProperty(document.documentElement, 'scrollHeight', {
                value: 5000,
                writable: true,
                configurable: true,
            });
            document.body.appendChild(document.createElement('div'));
            await new Promise(r => setTimeout(r, 60));

            expect(window.__scrollRestoring).toBe(false);
        });

        it('no-position path clears __scrollRestoring', async () => {
            window.navigation = {
                currentEntry: { navigationType: 'traverse' },
            };
            await import(MODULE_PATH);

            window.__scrollRestoring = true;
            // No saved position for this page
            const result = window.__restoreScrollPosition();

            expect(result).toBe(false);
            expect(window.__scrollRestoring).toBe(false);
        });
    });

    describe('navigation spinner', () => {
        it('hash-only pushState does not create the nav spinner (TOC links)', async () => {
            await import(MODULE_PATH);

            // Simulate clicking a TOC anchor link — same path, only the fragment changes.
            window.history.pushState(null, '', '/all#section-one');

            // showNavSpinner must NOT have been called: the spinner element is created
            // lazily inside createNavSpinner() which only runs from showNavSpinner().
            expect(document.getElementById('nav-spinner')).toBeNull();
        });

        it('path-changing pushState creates the nav spinner element', async () => {
            await import(MODULE_PATH);

            // Simulate a real page navigation — different path.
            window.history.pushState(null, '', '/github-copilot');
            window.markScriptsReady(); // clear the 500ms show + 10s safety timers

            expect(document.getElementById('nav-spinner')).not.toBeNull();
        });

        it('hash-with-query-change pushState does not create the nav spinner', async () => {
            // Current page already has a query string
            window.location.pathname = '/all';
            window.location.search = '?tags=news';
            window.location.href = 'https://localhost/all?tags=news';

            await import(MODULE_PATH);

            // Same path+query, different fragment — still hash-only.
            window.history.pushState(null, '', '/all?tags=news#section-two');

            expect(document.getElementById('nav-spinner')).toBeNull();
        });
    });
});
