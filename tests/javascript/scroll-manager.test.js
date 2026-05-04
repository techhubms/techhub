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

    beforeEach(async () => {
        document.body.innerHTML = '';
        document.documentElement.className = '';

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

        vi.resetModules();
        mod = await import(MODULE_PATH);
    });

    afterEach(() => {
        if (mod.dispose) mod.dispose();
        delete globalThis.Blazor;
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
        it('should save position on scroll events', () => {
            Object.defineProperty(window, 'scrollY', { value: 500, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));
            expect(window.__savedScrollPositions['/all']).toBe(500);
        });

        it('should save scroll position synchronously on pushState', () => {
            Object.defineProperty(window, 'scrollY', { value: 1500, writable: true, configurable: true });
            window.history.pushState(null, '', '/detail');
            window.markScriptsReady();
            expect(window.__savedScrollPositions['/all']).toBe(1500);
        });

        it('should save correct page key when pushState fires (old URL, not new)', () => {
            Object.defineProperty(window, 'scrollY', { value: 2000, writable: true, configurable: true });
            window.history.pushState(null, '', '/detail');
            window.markScriptsReady();
            window.location.pathname = '/detail';
            expect(window.__savedScrollPositions['/all']).toBe(2000);
            expect(window.__savedScrollPositions['/detail']).toBeUndefined();
        });

        it('should save position keyed by pathname + search (no hash)', () => {
            Object.defineProperty(window, 'scrollY', { value: 300, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            window.location.pathname = '/github-copilot';
            window.location.search = '?tags=news';
            window.location.hash = '#section';

            Object.defineProperty(window, 'scrollY', { value: 700, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            expect(window.__savedScrollPositions['/all']).toBe(300);
            expect(window.__savedScrollPositions['/github-copilot?tags=news']).toBe(700);
        });

        it('should expose __restoreScrollPosition globally', () => {
            expect(typeof window.__restoreScrollPosition).toBe('function');
        });

        it('should restore scroll position on traverse navigation', () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.__savedScrollPositions['/all'] = 450;
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

        it('should clear navigating flag after restoring position', () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBe(true);

            window.__savedScrollPositions['/other-page'] = 600;
            window.__restoreScrollPosition();
            expect(mod.isNavigating()).toBe(false);
        });

        it('should restore via popstate flag when Navigation API unavailable', () => {
            delete window.navigation;
            window.dispatchEvent(new Event('popstate'));
            window.__savedScrollPositions['/all'] = 800;
            const result = window.__restoreScrollPosition();
            expect(result).toBe(true);
            expect(window.scrollTo).toHaveBeenCalledWith(0, 800);
        });

        it('should defer scroll when page is too short and retry after DOM mutation', async () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 200, writable: true, configurable: true });

            window.__savedScrollPositions['/all'] = 2000;
            window.scrollTo.mockClear();

            const result = window.__restoreScrollPosition();
            expect(result).toBe(true);
            expect(window.scrollTo).not.toHaveBeenCalled();

            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 5000, writable: true, configurable: true });
            document.body.appendChild(document.createElement('div'));
            await new Promise(r => setTimeout(r, 60));

            expect(window.scrollTo).toHaveBeenCalledWith(0, 2000);
        });
    });

    // =========================================================================
    // Navigation State
    // =========================================================================

    describe('navigation state', () => {
        it('popstate sets isNavigating to true', () => {
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBe(true);
        });

        it('cross-page pushState sets isNavigating to true', () => {
            expect(mod.isNavigating()).toBe(false);
            window.history.pushState(null, '', '/github-copilot');
            window.markScriptsReady();
            expect(mod.isNavigating()).toBe(true);
        });

        it('hash-only pushState does NOT set isNavigating', () => {
            expect(mod.isNavigating()).toBe(false);
            window.history.pushState(null, '', '/all#section-one');
            expect(mod.isNavigating()).toBe(false);
        });

        it('restoreScrollPosition clears isNavigating', () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBe(true);

            window.__savedScrollPositions['/other-page'] = 400;
            window.__restoreScrollPosition();
            expect(mod.isNavigating()).toBe(false);
        });

        it('restoreScrollPosition deferred path clears isNavigating after MutationObserver fires', async () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 200, writable: true, configurable: true });

            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            window.__savedScrollPositions['/other-page'] = 2000;
            window.scrollTo.mockClear();

            window.__restoreScrollPosition();
            expect(mod.isNavigating()).toBe(true);

            Object.defineProperty(document.documentElement, 'scrollHeight', { value: 5000, writable: true, configurable: true });
            document.body.appendChild(document.createElement('div'));
            await new Promise(r => setTimeout(r, 60));

            expect(mod.isNavigating()).toBe(false);
        });

        it('no-position path clears isNavigating', () => {
            window.navigation = { currentEntry: { navigationType: 'traverse' } };
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));
            expect(mod.isNavigating()).toBe(true);

            const result = window.__restoreScrollPosition();
            expect(result).toBe(false);
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
            h2Link.click();

            const h2Item = h2Link.closest('.toc-depth-0');
            expect(h2Item.classList.contains('expanded')).toBe(true);

            h2Link.click();
            expect(h2Item.classList.contains('expanded')).toBe(false);
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

        it('should call LoadNextBatch immediately if trigger is in viewport', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();
            trigger.getBoundingClientRect = () => ({ top: 500 }); // 500 <= 800+300
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
        });

        it('should NOT call LoadNextBatch if trigger is far below viewport', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();
            trigger.getBoundingClientRect = () => ({ top: 2000 }); // 2000 > 800+300
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should call LoadNextBatch when scrolling brings trigger into margin', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();
            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();

            trigger.getBoundingClientRect = () => ({ top: 900 }); // 900 <= 800+300
            window.dispatchEvent(new Event('scroll'));
            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
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
            mod.dispose(); // Should not throw
        });

        it('should stop responding to scroll events after dispose', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();
            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            mod.dispose();

            trigger.getBoundingClientRect = () => ({ top: 500 });
            window.dispatchEvent(new Event('scroll'));
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should not fire during navigation', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();
            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // Trigger navigation state
            window.location = { ...window.location, pathname: '/other-page' };
            window.dispatchEvent(new PopStateEvent('popstate', { state: null }));

            trigger.getBoundingClientRect = () => ({ top: 500 });
            window.dispatchEvent(new Event('scroll'));
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
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
