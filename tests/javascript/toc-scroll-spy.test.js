import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/toc-scroll-spy.js';

function createTocAndContent() {
    // Create content element with headings
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

    // Create TOC element
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

describe('toc-scroll-spy.js', () => {
    let mod;

    beforeEach(async () => {
        document.body.innerHTML = '';
        delete window.__e2eSignal;
        delete window.__scrollRestoring;

        Object.defineProperty(window, 'innerHeight', {
            value: 800,
            writable: true,
            configurable: true,
        });

        Object.defineProperty(window, 'innerWidth', {
            value: 1400, // Desktop width (> 1292 tablet breakpoint)
            writable: true,
            configurable: true,
        });

        Object.defineProperty(window, 'scrollY', {
            value: 0,
            writable: true,
            configurable: true,
        });

        Object.defineProperty(window, 'location', {
            value: {
                pathname: '/handbook/testing',
                search: '',
                hash: '',
            },
            writable: true,
            configurable: true,
        });

        // Mock history.replaceState
        window.history.replaceState = vi.fn();

        // Mock scrollend support
        window.onscrollend = undefined;

        vi.resetModules();
        mod = await import(MODULE_PATH);
    });

    afterEach(() => {
        // Clean up any scroll spy instances
        const tocElements = document.querySelectorAll('[data-toc-scroll-spy]');
        tocElements.forEach(toc => {
            if (toc._tocScrollSpy) {
                toc._tocScrollSpy.destroy();
            }
        });
        vi.restoreAllMocks();
    });

    describe('TocScrollSpy class', () => {
        it('should export TocScrollSpy class and initTocScrollSpy function', () => {
            expect(mod.TocScrollSpy).toBeDefined();
            expect(mod.initTocScrollSpy).toBeDefined();
            expect(mod.cleanupAllTocScrollSpies).toBeDefined();
        });

        it('should initialize with TOC and content elements', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            expect(spy.initialized).toBe(true);
            expect(spy.headings.length).toBe(4); // 2 h2 + 2 h3

            spy.destroy();
        });

        it('should guard against duplicate initialization', () => {
            const { toc, content } = createTocAndContent();
            const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();
            spy.init(); // Second call should warn

            expect(warn).toHaveBeenCalledWith(
                'TocScrollSpy already initialized. Call destroy() first if re-initialization is needed.'
            );

            spy.destroy();
        });

        it('should not initialize if no headings found', () => {
            const content = document.createElement('div');
            content.id = 'empty-content';
            content.innerHTML = '<p>No headings here</p>';
            document.body.appendChild(content);

            const toc = document.createElement('nav');
            document.body.appendChild(toc);

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            expect(spy.initialized).toBe(false);
        });

        it('should record initialPagePath on init', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            expect(spy.initialPagePath).toBe('/handbook/testing');

            spy.destroy();
        });
    });

    describe('setActive', () => {
        it('should set active class on TOC link and heading', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('intro');

            const link = toc.querySelector('a[href="#intro"]');
            expect(link.classList.contains('active')).toBe(true);

            const heading = content.querySelector('#intro');
            expect(heading.classList.contains('toc-active-heading')).toBe(true);

            spy.destroy();
        });

        it('should remove active from previous element', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('intro');
            spy.setActive('features');

            const introLink = toc.querySelector('a[href="#intro"]');
            expect(introLink.classList.contains('active')).toBe(false);

            const featuresLink = toc.querySelector('a[href="#features"]');
            expect(featuresLink.classList.contains('active')).toBe(true);

            spy.destroy();
        });

        it('should call history.replaceState with heading hash', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('features');

            expect(window.history.replaceState).toHaveBeenCalledWith(
                null, '', '/handbook/testing#features'
            );

            spy.destroy();
        });

        it('should clear active state when null is passed', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('intro');
            spy.setActive(null);

            expect(spy.currentActiveId).toBeNull();

            spy.destroy();
        });

        it('should clear URL hash when active state is set to null and hash exists', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('intro');
            // Simulate that the hash was set
            window.location.hash = '#intro';
            window.history.replaceState.mockClear();

            spy.setActive(null);

            expect(window.history.replaceState).toHaveBeenCalledWith(
                null, '', '/handbook/testing'
            );

            spy.destroy();
        });

        it('should not call replaceState when clearing active state if no hash exists', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('intro');
            window.location.hash = '';
            window.history.replaceState.mockClear();

            spy.setActive(null);

            expect(window.history.replaceState).not.toHaveBeenCalled();

            spy.destroy();
        });
    });

    describe('updateCollapseState', () => {
        it('should expand the h2 section containing active heading', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('intro');

            const introItem = toc.querySelectorAll('.toc-depth-0')[0];
            expect(introItem.classList.contains('expanded')).toBe(true);

            spy.destroy();
        });

        it('should collapse other h2 sections', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('intro');
            spy.setActive('features');

            const introItem = toc.querySelectorAll('.toc-depth-0')[0];
            const featuresItem = toc.querySelectorAll('.toc-depth-0')[1];
            expect(introItem.classList.contains('expanded')).toBe(false);
            expect(featuresItem.classList.contains('expanded')).toBe(true);

            spy.destroy();
        });

        it('should expand parent h2 section when h3 is active', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            spy.setActive('sub-features');

            const featuresItem = toc.querySelectorAll('.toc-depth-0')[1];
            expect(featuresItem.classList.contains('expanded')).toBe(true);

            spy.destroy();
        });
    });

    describe('destroy', () => {
        it('should remove event listeners and reset state', () => {
            const { toc, content } = createTocAndContent();
            const removeSpy = vi.spyOn(window, 'removeEventListener');

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();
            spy.destroy();

            expect(spy.initialized).toBe(false);
            expect(removeSpy).toHaveBeenCalledWith('scroll', expect.any(Function));
            expect(removeSpy).toHaveBeenCalledWith('resize', expect.any(Function));
        });
    });

    describe('handleScroll - page change detection', () => {
        it('should auto-destroy when URL changes (enhanced navigation)', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            // Simulate URL change (enhanced navigation)
            window.location.pathname = '/different-page';

            spy.handleScroll();

            expect(spy.initialized).toBe(false);
        });

        it('should skip RAF scheduling while __scrollRestoring is set', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            // Simulate nav-helpers.js setting the flag on popstate
            window.__scrollRestoring = true;

            spy.handleScroll();
            window.dispatchEvent(new Event('scroll'));

            // Guard returns early — no RAF was scheduled, ticking stays false
            expect(spy.ticking).toBe(false);
        });

        it('should resume RAF scheduling once __scrollRestoring is cleared', () => {
            const { toc, content } = createTocAndContent();

            const spy = new mod.TocScrollSpy(toc, content);
            spy.init();

            window.__scrollRestoring = true;
            spy.handleScroll();
            expect(spy.ticking).toBe(false); // paused — no RAF

            // nav-helpers.js clears the flag after restore completes
            window.__scrollRestoring = false;
            spy.handleScroll();

            // Guard bypassed — RAF was scheduled
            expect(spy.ticking).toBe(true);
        });
    });

    describe('initTocScrollSpy', () => {
        it('should initialize scroll spy for desktop viewport', () => {
            createTocAndContent();

            mod.initTocScrollSpy();

            const toc = document.querySelector('[data-toc-scroll-spy]');
            expect(toc._tocScrollSpy).toBeDefined();
            expect(toc._tocScrollSpy.initialized).toBe(true);
        });

        it('should use mobile mode for narrow viewport', () => {
            Object.defineProperty(window, 'innerWidth', {
                value: 800, // Below 1292 breakpoint
                configurable: true,
            });

            createTocAndContent();

            mod.initTocScrollSpy();

            const toc = document.querySelector('[data-toc-scroll-spy]');
            expect(toc._tocScrollSpy).toBeUndefined();
            expect(toc.classList.contains('toc-mobile-mode')).toBe(true);
        });

        it('should handle missing data-content-selector gracefully', () => {
            const toc = document.createElement('nav');
            toc.setAttribute('data-toc-scroll-spy', '');
            // Missing data-content-selector
            document.body.appendChild(toc);

            const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});

            mod.initTocScrollSpy();

            expect(warn).toHaveBeenCalledWith('TOC element missing data-content-selector attribute');
        });

        it('should clean up existing instance before re-init', () => {
            createTocAndContent();

            mod.initTocScrollSpy();
            const toc = document.querySelector('[data-toc-scroll-spy]');
            const firstInstance = toc._tocScrollSpy;

            mod.initTocScrollSpy();
            const secondInstance = toc._tocScrollSpy;

            expect(firstInstance).not.toBe(secondInstance);
            expect(firstInstance.initialized).toBe(false); // destroyed
        });
    });

    describe('cleanupAllTocScrollSpies', () => {
        it('should destroy all scroll spy instances', () => {
            createTocAndContent();

            mod.initTocScrollSpy();
            const toc = document.querySelector('[data-toc-scroll-spy]');
            expect(toc._tocScrollSpy.initialized).toBe(true);

            mod.cleanupAllTocScrollSpies();
            expect(toc._tocScrollSpy).toBeNull();
        });
    });

    describe('mobile mode', () => {
        it('should toggle expanded on h2 click in mobile mode', () => {
            Object.defineProperty(window, 'innerWidth', {
                value: 800,
                configurable: true,
            });

            createTocAndContent();
            mod.initTocScrollSpy();

            const toc = document.querySelector('[data-toc-scroll-spy]');
            const h2Link = toc.querySelector('.toc-depth-0 > .toc-link');

            // Simulate click
            h2Link.click();

            const h2Item = h2Link.closest('.toc-depth-0');
            expect(h2Item.classList.contains('expanded')).toBe(true);

            // Click again to collapse
            h2Link.click();
            expect(h2Item.classList.contains('expanded')).toBe(false);
        });
    });
});
