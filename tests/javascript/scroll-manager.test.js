import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/scroll-manager.js';

function createTocAndContent() {
    const content = document.createElement('div');
    content.id = 'content';
    content.innerHTML = `
        <h2 id="intro">Introduction</h2>
        <p>Some text about intro</p>
        <h3 id="sub-intro">Sub Introduction</h3>
        <p>More text</p>
        <h2 id="features">Features</h2>
        <p>Feature content</p>
        <h3 id="sub-features">Sub Features</h3>
        <p>Sub feature content</p>
    `;
    document.body.appendChild(content);

    const toc = document.createElement('nav');
    toc.setAttribute('data-toc-scroll-spy', '');
    toc.setAttribute('data-content-selector', '#content');
    toc.innerHTML = `
        <div class="toc-depth-0">
            <a class="toc-link" href="#intro">Introduction</a>
            <div class="toc-sublist">
                <div class="toc-depth-1">
                    <a class="toc-link" href="#sub-intro">Sub Introduction</a>
                </div>
            </div>
        </div>
        <div class="toc-depth-0">
            <a class="toc-link" href="#features">Features</a>
            <div class="toc-sublist">
                <div class="toc-depth-1">
                    <a class="toc-link" href="#sub-features">Sub Features</a>
                </div>
            </div>
        </div>
    `;
    document.body.appendChild(toc);
    return { toc, content };
}

function createTriggerElement(id = 'scroll-trigger') {
    const el = document.createElement('div');
    el.id = id;
    document.body.appendChild(el);
    return el;
}

function createMockHelper() {
    return { invokeMethodAsync: vi.fn().mockResolvedValue(undefined) };
}

describe('scroll-manager.js', () => {
    let mod;
    let originalPushState;

    beforeEach(async () => {
        document.body.innerHTML = '';
        document.documentElement.className = '';

        // Capture the real pushState before the module overwrites it, so afterEach
        // can restore it and prevent wrapper accumulation across tests.
        originalPushState = window.history.pushState;

        delete window.TechHub;
        delete window.__savedScrollPositions;
        delete window.__restoreScrollPosition;
        delete window.__scrollListenerReady;
        delete window.__scrollListenerVersion;
        delete window.__e2eSignal;
        delete window.navigation;

        globalThis.Blazor = { addEventListener: vi.fn() };
        window.markScriptsReady = vi.fn();

        Object.defineProperty(window, 'scrollY', {
            value: 0, writable: true, configurable: true,
        });
        Object.defineProperty(window, 'innerHeight', {
            value: 800, writable: true, configurable: true,
        });
        Object.defineProperty(window, 'innerWidth', {
            value: 1400, writable: true, configurable: true,
        });
        Object.defineProperty(document.documentElement, 'scrollHeight', {
            value: 10000, writable: true, configurable: true,
        });

        window.scrollTo = vi.fn((xOrOptions, y) => {
            const val = typeof xOrOptions === 'object' ? (xOrOptions.top ?? 0) : (y ?? 0);
            Object.defineProperty(window, 'scrollY', { value: val, writable: true, configurable: true });
        });

        window.history.replaceState = vi.fn();
        window.history.back = vi.fn();
        Object.defineProperty(window.history, 'length', {
            value: 5, writable: true, configurable: true,
        });

        Object.defineProperty(window, 'location', {
            value: { pathname: '/all', search: '', hash: '', href: 'https://localhost/all' },
            writable: true, configurable: true,
        });

        window.requestAnimationFrame = (cb) => { cb(); return 1; };
        window.cancelAnimationFrame = vi.fn();

        // IntersectionObserver mock: stores the last created instance so tests
        // can fire intersection callbacks manually via mockObserver.trigger(isIntersecting).
        window.__mockObservers = [];
        window.IntersectionObserver = class MockIntersectionObserver {
            constructor(callback, options) {
                this.callback = callback;
                this.options = options;
                this.targets = [];
                this.disconnected = false;
                window.__mockObservers.push(this);
            }
            observe(el) { this.targets.push(el); }
            unobserve(el) { this.targets = this.targets.filter(t => t !== el); }
            disconnect() { this.disconnected = true; this.targets = []; }
            /** Fire the IO callback with a synthetic entry. */
            trigger(isIntersecting) {
                if (this.disconnected) return;
                const target = this.targets[0];
                this.callback([{ isIntersecting, target }]);
            }
        };

        // ResizeObserver mock: stores instances so tests can trigger resize callbacks.
        window.__mockResizeObservers = [];
        window.ResizeObserver = class MockResizeObserver {
            constructor(callback) {
                this.callback = callback;
                this.targets = [];
                this.disconnected = false;
                window.__mockResizeObservers.push(this);
            }
            observe(el) { this.targets.push(el); }
            unobserve(el) { this.targets = this.targets.filter(t => t !== el); }
            disconnect() { this.disconnected = true; this.targets = []; }
            /** Fire the RO callback to simulate a resize. */
            trigger() {
                if (this.disconnected) return;
                this.callback([{ target: this.targets[0] }]);
            }
        };

        vi.resetModules();
        mod = await import(MODULE_PATH);
    });

    afterEach(() => {
        if (mod.dispose) mod.dispose();
        // Restore the original pushState to prevent wrapper accumulation across tests.
        window.history.pushState = originalPushState;
        delete globalThis.Blazor;
        delete window.__mockResizeObservers;
        vi.restoreAllMocks();
    });

    // =========================================================================
    // Keyboard Navigation Detection
    // =========================================================================

    describe('keyboard navigation detection', () => {
        it('should add keyboard-nav class on Tab key', () => {
            document.dispatchEvent(new KeyboardEvent('keydown', { key: 'Tab', bubbles: true }));
            expect(document.documentElement.classList.contains('keyboard-nav')).toBe(true);
        });

        it('should remove keyboard-nav class on pointerdown', () => {
            document.dispatchEvent(new KeyboardEvent('keydown', { key: 'Tab', bubbles: true }));
            document.dispatchEvent(new Event('pointerdown', { bubbles: true }));
            expect(document.documentElement.classList.contains('keyboard-nav')).toBe(false);
        });

        it('should not add keyboard-nav for non-Tab keys', () => {
            document.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true }));
            expect(document.documentElement.classList.contains('keyboard-nav')).toBe(false);
        });
    });

    // =========================================================================
    // Navigation Buttons
    // =========================================================================

    describe('button creation', () => {
        it('should create nav-helper-buttons container', () => {
            const container = document.getElementById('nav-helper-buttons');
            expect(container).not.toBeNull();
            expect(container.className).toBe('nav-helper-buttons');
        });

        it('should create back-to-top button', () => {
            const btn = document.querySelector('.nav-helper-btn-top');
            expect(btn).not.toBeNull();
            expect(btn.getAttribute('aria-label')).toBe('Back to top');
        });

        it('should create back-to-previous button', () => {
            const btn = document.querySelector('.nav-helper-btn-prev');
            expect(btn).not.toBeNull();
            expect(btn.getAttribute('aria-label')).toBe('Back to previous page');
        });

        it('should not create duplicate containers', () => {
            window.dispatchEvent(new Event('pageshow'));
            const containers = document.querySelectorAll('#nav-helper-buttons');
            expect(containers.length).toBe(1);
        });
    });

    describe('scroll visibility', () => {
        it('should show buttons when scrolled past threshold (300px)', () => {
            Object.defineProperty(window, 'scrollY', { value: 350, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));
            const container = document.getElementById('nav-helper-buttons');
            expect(container.classList.contains('visible')).toBe(true);
        });

        it('should hide buttons when scrolled to top', () => {
            Object.defineProperty(window, 'scrollY', { value: 350, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));
            Object.defineProperty(window, 'scrollY', { value: 50, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));
            const container = document.getElementById('nav-helper-buttons');
            expect(container.classList.contains('visible')).toBe(false);
        });
    });

    describe('back-to-top', () => {
        it('should scroll to top when clicked', () => {
            const btn = document.querySelector('.nav-helper-btn-top');
            btn.click();
            expect(window.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: 'smooth' });
        });

        it('should clear hash from URL when scrolling to top', () => {
            window.location.hash = '#section1';
            const btn = document.querySelector('.nav-helper-btn-top');
            btn.click();
            expect(window.history.replaceState).toHaveBeenCalledWith(null, '', '/all');
        });
    });

    describe('back-to-previous', () => {
        it('should call history.back when history exists', () => {
            const btn = document.querySelector('.nav-helper-btn-prev');
            btn.click();
            expect(window.history.back).toHaveBeenCalled();
        });

        it('should navigate to homepage when no history', () => {
            Object.defineProperty(window.history, 'length', { value: 1, configurable: true });
            // Need fresh module since button was already created with length=5
            const btn = document.querySelector('.nav-helper-btn-prev');
            btn.click();
            // With length=1, falls through to location.href = '/'
            expect(window.location.href).toBe('/');
        });
    });

    describe('TechHub.scrollToTopAndClearHash', () => {
        it('should expose scrollToTopAndClearHash globally', () => {
            expect(typeof window.TechHub.scrollToTopAndClearHash).toBe('function');
        });

        it('should scroll to top and clear hash', () => {
            window.location.hash = '#something';
            window.TechHub.scrollToTopAndClearHash();
            expect(window.scrollTo).toHaveBeenCalledWith(0, 0);
            expect(window.history.replaceState).toHaveBeenCalledWith(null, '', '/all');
        });
    });

    // =========================================================================
    // Scroll Position Management
    // =========================================================================

    describe('scroll position management', () => {
        it('should save settled position on scrollend for use by pushState', () => {
            Object.defineProperty(window, 'scrollY', { value: 500, writable: true, configurable: true });
            window.dispatchEvent(new Event('scrollend'));
            // scrollend updates lastSettledScrollY; pushState uses it to save
            window.history.pushState(null, '', '/detail');
            window.markScriptsReady();
            expect(window.__savedScrollPositions['/all']).toBe(500);
        });

        it('should save scroll position synchronously on pushState', () => {
            Object.defineProperty(window, 'scrollY', { value: 1500, writable: true, configurable: true });
            window.dispatchEvent(new Event('scrollend'));
            window.history.pushState(null, '', '/detail');
            window.markScriptsReady();
            expect(window.__savedScrollPositions['/all']).toBe(1500);
        });

        it('should save correct page key when pushState fires (old URL, not new)', () => {
            Object.defineProperty(window, 'scrollY', { value: 2000, writable: true, configurable: true });
            window.dispatchEvent(new Event('scrollend'));
            window.history.pushState(null, '', '/detail');
            window.markScriptsReady();
            window.location.pathname = '/detail';
            expect(window.__savedScrollPositions['/all']).toBe(2000);
            expect(window.__savedScrollPositions['/detail']).toBeUndefined();
        });

        it('should save position keyed by pathname + search (no hash)', () => {
            // Settle at 300 on /all, then navigate away
            Object.defineProperty(window, 'scrollY', { value: 300, writable: true, configurable: true });
            window.dispatchEvent(new Event('scrollend'));
            window.history.pushState(null, '', '/github-copilot?tags=news#section');
            window.markScriptsReady();

            // Now on the new page: update location and settle at 700
            window.location.pathname = '/github-copilot';
            window.location.search = '?tags=news';
            window.location.hash = '#section';

            // Simulate forward nav finishing (clears navigating flag so scrollend works)
            window.__restoreScrollPosition();

            Object.defineProperty(window, 'scrollY', { value: 700, writable: true, configurable: true });
            window.dispatchEvent(new Event('scrollend'));
            window.history.pushState(null, '', '/another-page');
            window.markScriptsReady();

            expect(window.__savedScrollPositions['/all']).toBe(300);
            expect(window.__savedScrollPositions['/github-copilot?tags=news']).toBe(700);
        });

        it('should expose __restoreScrollPosition globally', () => {
            expect(typeof window.__restoreScrollPosition).toBe('function');
        });

        it('should restore scroll position on traverse navigation', () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.location = { ...window.location, pathname: '/other-page', href: 'https://localhost/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            window.__savedScrollPositions['/other-page'] = 450;
            const result = window.__restoreScrollPosition();
            expect(result).toBe(true);
            expect(window.scrollTo).toHaveBeenCalledWith(0, 450);
        });

        it('should NOT restore on push navigation', () => {
            window.navigation = { currentEntry: { navigationType: 'push' } };
            window.__savedScrollPositions['/all'] = 450;
            window.scrollTo.mockClear();
            const result = window.__restoreScrollPosition();
            expect(result).toBe(false);
        });

        it('should return false when no saved position exists', () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            const result = window.__restoreScrollPosition();
            expect(result).toBe(false);
        });

        it('should clear navigating flag after restoring position', async () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBeTruthy();

            window.__savedScrollPositions['/other-page'] = 600;
            window.__restoreScrollPosition();
            await new Promise(r => setTimeout(r, 0));
            expect(mod.isNavigating()).toBe(false);
        });

        it('should restore via popstate flag when Navigation API unavailable', () => {
            delete window.navigation;
            window.location = { ...window.location, pathname: '/other-page', href: 'https://localhost/other-page' };
            window.dispatchEvent(new Event('popstate'));
            window.__savedScrollPositions['/other-page'] = 800;
            const result = window.__restoreScrollPosition();
            expect(result).toBe(true);
            expect(window.scrollTo).toHaveBeenCalledWith(0, 800);
        });

        it('should defer scroll when page is too short and retry after layout stabilizes', async () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 200, writable: true, configurable: true });

            window.location = { ...window.location, pathname: '/other-page', href: 'https://localhost/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            window.__savedScrollPositions['/other-page'] = 2000;
            window.scrollTo.mockClear();

            const result = window.__restoreScrollPosition();
            expect(result).toBe(true);
            expect(window.scrollTo).not.toHaveBeenCalled();

            // Simulate page height increasing via ResizeObserver (e.g., images loaded)
            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 5000, writable: true, configurable: true });
            const ro = window.__mockResizeObservers.find(o => !o.disconnected);
            ro.trigger();

            // Wait for the 150ms debounce to settle
            await new Promise(r => setTimeout(r, 200));

            expect(window.scrollTo).toHaveBeenCalledWith(0, 2000);
        });
    });

    // =========================================================================
    // Navigation State
    // =========================================================================

    describe('navigation state', () => {
        it('popstate sets isNavigating to traverse', () => {
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBe('traverse');
        });

        it('cross-page pushState sets isNavigating to forward', () => {
            expect(mod.isNavigating()).toBe(false);
            window.history.pushState(null, '', '/github-copilot');
            window.markScriptsReady();
            expect(mod.isNavigating()).toBe('forward');
        });

        it('hash-only pushState does NOT set isNavigating', () => {
            expect(mod.isNavigating()).toBe(false);
            window.history.pushState(null, '', '/all#section-one');
            expect(mod.isNavigating()).toBe(false);
        });

        it('restoreScrollPosition clears isNavigating', async () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBe('traverse');

            window.__savedScrollPositions['/other-page'] = 400;
            window.__restoreScrollPosition();
            await new Promise(r => setTimeout(r, 0));
            expect(mod.isNavigating()).toBe(false);
        });

        it('restoreScrollPosition deferred path clears isNavigating after layout stabilizes', async () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 200, writable: true, configurable: true });

            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            window.__savedScrollPositions['/other-page'] = 2000;
            window.scrollTo.mockClear();

            window.__restoreScrollPosition();
            expect(mod.isNavigating()).toBe('traverse');

            // Simulate page height increasing (e.g., images loaded) via ResizeObserver
            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 5000, writable: true, configurable: true });
            const ro = window.__mockResizeObservers.find(o => !o.disconnected);
            ro.trigger();

            // Wait for the 150ms debounce to settle + rAF
            await new Promise(r => setTimeout(r, 200));

            expect(mod.isNavigating()).toBe(false);
        });

        it('no-position path clears isNavigating', async () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBe('traverse');

            const result = window.__restoreScrollPosition();
            expect(result).toBe(false);
            await new Promise(r => setTimeout(r, 0));
            expect(mod.isNavigating()).toBe(false);
        });
    });

    // =========================================================================
    // Navigation Spinner
    // =========================================================================

    describe('navigation spinner', () => {
        it('hash-only pushState does not create the nav spinner', () => {
            window.history.pushState(null, '', '/all#section-one');
            expect(document.getElementById('nav-spinner')).toBeNull();
        });

        it('path-changing pushState creates the nav spinner element', () => {
            window.history.pushState(null, '', '/github-copilot');
            window.markScriptsReady();
            expect(document.getElementById('nav-spinner')).not.toBeNull();
        });

        it('cross-page pushState scrolls to top immediately', () => {
            window.scrollTo = vi.fn();
            window.history.pushState(null, '', '/github-copilot');
            expect(window.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: 'instant' });
        });

        it('hash-only pushState does not scroll to top', () => {
            window.scrollTo = vi.fn();
            window.history.pushState(null, '', '/all#section-one');
            expect(window.scrollTo).not.toHaveBeenCalledWith(0, 0);
        });
    });

    // =========================================================================
    // TOC Scroll Spy
    // =========================================================================

    describe('TOC scroll spy', () => {
        it('should export initTocScrollSpy function', () => {
            expect(typeof mod.initTocScrollSpy).toBe('function');
        });

        it('should initialize for desktop viewport when TOC elements exist', () => {
            createTocAndContent();
            mod.initTocScrollSpy();
            // Should fire e2e signal on successful init
            // No crash = success
        });

        it('should use mobile mode for narrow viewport', () => {
            Object.defineProperty(window, 'innerWidth', { value: 800, configurable: true });
            createTocAndContent();
            mod.initTocScrollSpy();

            const toc = document.querySelector('[data-toc-scroll-spy]');
            expect(toc.classList.contains('toc-mobile-mode')).toBe(true);
        });

        it('should toggle expanded on h2 click in mobile mode', () => {
            Object.defineProperty(window, 'innerWidth', { value: 800, configurable: true });
            createTocAndContent();
            mod.initTocScrollSpy();

            const toc = document.querySelector('[data-toc-scroll-spy]');
            const h2Link = toc.querySelector('.toc-depth-0 > .toc-link');
            const h2Item = h2Link.closest('.toc-depth-0');

            // Capture initial state (mobile mode skips initial highlight, starts collapsed)
            const initialExpanded = h2Item.classList.contains('expanded');

            h2Link.click();
            expect(h2Item.classList.contains('expanded')).toBe(!initialExpanded);

            h2Link.click();
            expect(h2Item.classList.contains('expanded')).toBe(initialExpanded);
        });

        it('should not crash when no TOC elements exist', () => {
            mod.initTocScrollSpy(); // Should not throw
        });

        it('should update TOC highlight on scrollend', () => {
            const { content } = createTocAndContent();
            // Mock heading position
            const heading = content.querySelector('#intro');
            heading.getBoundingClientRect = () => ({ top: 150 });
            const featuresHeading = content.querySelector('#features');
            featuresHeading.getBoundingClientRect = () => ({ top: 900 });
            const subIntro = content.querySelector('#sub-intro');
            subIntro.getBoundingClientRect = () => ({ top: 500 });
            const subFeatures = content.querySelector('#sub-features');
            subFeatures.getBoundingClientRect = () => ({ top: 1200 });

            mod.initTocScrollSpy();

            // Simulate scrollend
            if ('onscrollend' in window) {
                window.dispatchEvent(new Event('scrollend'));
            } else {
                // Trigger via scroll + debounce timeout
                window.dispatchEvent(new Event('scroll'));
            }

            // TOC link should have active class
            const toc = document.querySelector('[data-toc-scroll-spy]');
            const introLink = toc.querySelector('a[href="#intro"]');
            expect(introLink.classList.contains('active')).toBe(true);
        });

        it('should NOT update URL hash when page has changed since TOC init', () => {
            const { content } = createTocAndContent();
            content.querySelector('#intro').getBoundingClientRect = () => ({ top: 150 });
            content.querySelector('#features').getBoundingClientRect = () => ({ top: 900 });
            content.querySelector('#sub-intro').getBoundingClientRect = () => ({ top: 500 });
            content.querySelector('#sub-features').getBoundingClientRect = () => ({ top: 1200 });

            mod.initTocScrollSpy(); // pageKey = '/all'; may call replaceState for initial highlight

            // Reset the mock to only observe calls that happen after navigation
            window.history.replaceState.mockClear();

            // Simulate navigating away before scrollend fires (user clicked Back mid-scroll)
            window.location = { ...window.location, pathname: '/other', search: '', hash: '' };

            window.dispatchEvent(new Event('scrollend'));

            // replaceState must NOT have been called with a hash from the old page
            const calls = window.history.replaceState.mock.calls;
            const hashCalls = calls.filter(c => typeof c[2] === 'string' && c[2].includes('#'));
            expect(hashCalls).toHaveLength(0);
        });

        it('should update URL hash when still on the same page', () => {
            const { content } = createTocAndContent();
            content.querySelector('#intro').getBoundingClientRect = () => ({ top: 150 });
            content.querySelector('#features').getBoundingClientRect = () => ({ top: 900 });
            content.querySelector('#sub-intro').getBoundingClientRect = () => ({ top: 500 });
            content.querySelector('#sub-features').getBoundingClientRect = () => ({ top: 1200 });

            mod.initTocScrollSpy(); // pageKey = '/all'

            window.dispatchEvent(new Event('scrollend'));

            const calls = window.history.replaceState.mock.calls;
            const hashCalls = calls.filter(c => typeof c[2] === 'string' && c[2].includes('#'));
            expect(hashCalls.length).toBeGreaterThan(0);
        });
    });

    // =========================================================================
    // Infinite Scroll
    // =========================================================================

    describe('infinite scroll', () => {
        it('should export observeScrollTrigger and dispose', () => {
            expect(typeof mod.observeScrollTrigger).toBe('function');
            expect(typeof mod.dispose).toBe('function');
        });

        it('should warn and return if trigger element not found', () => {
            const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'nonexistent-trigger');
            expect(warn).toHaveBeenCalledWith('[InfiniteScroll] Trigger element not found:', 'nonexistent-trigger');
        });

        it('should set __scrollListenerReady when trigger exists', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(window.__scrollListenerReady['scroll-trigger']).toBe(true);
        });

        it('should increment __scrollListenerVersion', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(window.__scrollListenerVersion['scroll-trigger']).toBe(1);
            // Simulating re-attach: first dispose (called inside observeScrollTrigger), then re-attach
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(window.__scrollListenerVersion['scroll-trigger']).toBe(2);
        });

        it('should fire e2eSignal when attached', () => {
            createTriggerElement();
            const helper = createMockHelper();
            window.__e2eSignal = vi.fn();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(window.__e2eSignal).toHaveBeenCalledWith('scroll-listener:scroll-trigger');
        });

        it('should call LoadNextBatch when trigger enters intersection margin', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const observer = window.__mockObservers.at(-1);
            observer.trigger(true);
            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
        });

        it('should NOT call LoadNextBatch when trigger exits intersection margin', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const observer = window.__mockObservers.at(-1);
            observer.trigger(false); // exit event
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should disconnect observer after LoadNextBatch is called (prevents cascade)', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const observer = window.__mockObservers.at(-1);
            observer.trigger(true);
            expect(observer.disconnected).toBe(true);
        });

        it('should NOT call LoadNextBatch during navigation', () => {
            createTriggerElement();
            const helper = createMockHelper();

            // Trigger back-nav (sets navigating = 'traverse')
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));

            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const observer = window.__mockObservers.at(-1);
            observer.trigger(true); // fires but navigating is truthy
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
            expect(observer.disconnected).toBe(false); // stays active for when nav completes
        });

        it('should call LoadNextBatch after navigation completes if trigger was intersecting during traverse', () => {
            createTriggerElement();
            const helper = createMockHelper();

            // Trigger back-nav (sets navigating = 'traverse')
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));

            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const observer = window.__mockObservers.at(-1);

            // IO fires while navigating — should record pending, not call LoadNextBatch yet
            observer.trigger(true);
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();

            // finishNavigation via restoreScrollPosition (rAF is sync in tests)
            window.__restoreScrollPosition();

            // Now that navigating=false, the pending intersect should have been flushed
            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
            expect(observer.disconnected).toBe(true);
        });

        it('should create a new observer on re-attach after batch load', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const first = window.__mockObservers.at(-1);
            first.trigger(true); // fires → disconnects

            // Simulate Blazor re-attaching after batch render
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const second = window.__mockObservers.at(-1);

            expect(second).not.toBe(first);
            expect(first.disconnected).toBe(true);
        });

        it('should set __scrollListenerReady to false on dispose', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            mod.dispose();
            expect(window.__scrollListenerReady['scroll-trigger']).toBe(false);
        });

        it('should fire e2eSignal on dispose', () => {
            createTriggerElement();
            const helper = createMockHelper();
            window.__e2eSignal = vi.fn();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            window.__e2eSignal.mockClear();
            mod.dispose();
            expect(window.__e2eSignal).toHaveBeenCalledWith('scroll-disposed:scroll-trigger');
        });

        it('should be safe to call dispose multiple times', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            mod.dispose();
            mod.dispose(); // should not throw
        });

        it('should be safe to dispose after IO already disconnected on batch load', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const observer = window.__mockObservers.at(-1);
            observer.trigger(true); // disconnects observer, clears infiniteScrollState
            mod.dispose(); // should be a no-op, not throw
            expect(helper.invokeMethodAsync).toHaveBeenCalledTimes(1);
        });

        it('should not call LoadNextBatch after dispose', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            mod.dispose();
            // After dispose, the observer is disconnected — trigger() has no effect
            const observer = window.__mockObservers.at(-1);
            observer.trigger(true);
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should use rootMargin matching TRIGGER_MARGIN_PX for pre-loading', () => {
            createTriggerElement();
            const helper = createMockHelper();
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            const observer = window.__mockObservers.at(-1);
            // rootMargin should extend 300px below the viewport
            expect(observer.options.rootMargin).toBe('0px 0px 300px 0px');
        });
    });

    // =========================================================================
    // isNavigating export
    // =========================================================================

    describe('isNavigating export', () => {
        it('should export isNavigating function', () => {
            expect(typeof mod.isNavigating).toBe('function');
        });

        it('should return false initially', () => {
            expect(mod.isNavigating()).toBe(false);
        });
    });
});
