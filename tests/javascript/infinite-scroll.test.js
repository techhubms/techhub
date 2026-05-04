import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

// jsdom environment provides window, document, etc.

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/infinite-scroll.js';

function createTriggerElement(id = 'scroll-trigger') {
    const el = document.createElement('div');
    el.id = id;
    document.body.appendChild(el);
    return el;
}

function createMockHelper() {
    return {
        invokeMethodAsync: vi.fn().mockResolvedValue(undefined),
    };
}

describe('infinite-scroll.js', () => {
    let mod;

    beforeEach(async () => {
        // Reset DOM
        document.body.innerHTML = '';

        // Reset global state that persists across module reloads
        delete window.__scrollListenerReady;
        delete window.__scrollListenerVersion;
        delete window.__e2eSignal;
        delete window.__infiniteScrollPaused;
        // No navigation API leakage between tests
        delete window.navigation;

        // Reset location
        Object.defineProperty(window, 'location', {
            value: { pathname: '/all', search: '?types=videos' },
            writable: true,
            configurable: true,
        });

        // Mock window.innerHeight
        Object.defineProperty(window, 'innerHeight', {
            value: 800,
            writable: true,
            configurable: true,
        });

        // Mock scrollY
        Object.defineProperty(window, 'scrollY', {
            value: 0,
            writable: true,
            configurable: true,
        });

        // Mock scrollTo
        window.scrollTo = vi.fn((x, y) => {
            Object.defineProperty(window, 'scrollY', { value: y, writable: true, configurable: true });
        });

        // Reset module registry to get fresh module-level state (let variables)
        vi.resetModules();
        mod = await import(MODULE_PATH);
    });

    afterEach(() => {
        // Dispose to clean up event listeners
        mod.dispose();
        vi.restoreAllMocks();
    });

    describe('observeScrollTrigger', () => {
        it('should warn and return if trigger element not found', () => {
            const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});
            const helper = createMockHelper();

            mod.observeScrollTrigger(helper, 'nonexistent-trigger');

            expect(warn).toHaveBeenCalledWith(
                '[InfiniteScroll] Trigger element not found:',
                'nonexistent-trigger'
            );
        });

        it('should attach scroll listener when trigger element exists', () => {
            createTriggerElement();
            const helper = createMockHelper();
            const addSpy = vi.spyOn(window, 'addEventListener');

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            expect(addSpy).toHaveBeenCalledWith('scroll', expect.any(Function), { passive: true });
        });

        it('should set __scrollListenerReady to true', () => {
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

            // Re-attach (requires re-creating trigger since dispose removes reference)
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

            // Mock getBoundingClientRect to put trigger within viewport + margin
            trigger.getBoundingClientRect = () => ({
                top: 500, // within innerHeight (800) + TRIGGER_MARGIN_PX (300) = 1100
                bottom: 510,
                left: 0,
                right: 100,
                width: 100,
                height: 10,
            });

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
        });

        it('should NOT call LoadNextBatch if trigger is far below viewport', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // Trigger is far below viewport + margin
            trigger.getBoundingClientRect = () => ({
                top: 2000, // way beyond innerHeight (800) + 300 = 1100
                bottom: 2010,
                left: 0,
                right: 100,
                width: 100,
                height: 10,
            });

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should dispose previous listener before attaching new one', () => {
            createTriggerElement();
            const helper = createMockHelper();
            const removeSpy = vi.spyOn(window, 'removeEventListener');

            // First attach
            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // Second attach should dispose first
            mod.observeScrollTrigger(helper, 'scroll-trigger');

            expect(removeSpy).toHaveBeenCalledWith('scroll', expect.any(Function));
        });
    });

    describe('scroll event handling', () => {
        it('should call LoadNextBatch when scrolling brings trigger into margin', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // Start with trigger far away
            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger');

            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();

            // Now simulate scroll that brings trigger into margin
            trigger.getBoundingClientRect = () => ({ top: 900 }); // 900 <= 800 + 300
            window.dispatchEvent(new Event('scroll'));

            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
        });

        it('should auto-dispose when URL changes (enhanced navigation)', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // Simulate URL change (enhanced navigation)
            window.location = { pathname: '/different-page', search: '' };

            Object.defineProperty(window, 'scrollY', { value: 100, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            // Should have disposed
            expect(window.__scrollListenerReady['scroll-trigger']).toBe(false);
        });
    });

    describe('dispose', () => {
        it('should remove scroll event listener', () => {
            createTriggerElement();
            const helper = createMockHelper();
            const removeSpy = vi.spyOn(window, 'removeEventListener');

            mod.observeScrollTrigger(helper, 'scroll-trigger');
            mod.dispose();

            expect(removeSpy).toHaveBeenCalledWith('scroll', expect.any(Function));
        });

        it('should set __scrollListenerReady to false', () => {
            createTriggerElement();
            const helper = createMockHelper();

            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(window.__scrollListenerReady['scroll-trigger']).toBe(true);

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

        it('should be safe to call multiple times', () => {
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

            // Scroll after dispose should not trigger
            trigger.getBoundingClientRect = () => ({ top: 500 });
            window.dispatchEvent(new Event('scroll'));

            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });
    });

    describe('window.__infiniteScrollPaused flag (set by nav-helpers.js)', () => {
        it('should NOT call LoadNextBatch immediately when paused', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // Simulate nav-helpers.js setting the flag on popstate
            window.__infiniteScrollPaused = true;

            // Trigger in viewport
            trigger.getBoundingClientRect = () => ({ top: 900 }); // within 800+300=1100

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // Immediate handleScroll() is skipped because __infiniteScrollPaused
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should NOT call LoadNextBatch on scroll events while paused', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            window.__infiniteScrollPaused = true;
            trigger.getBoundingClientRect = () => ({ top: 900 });

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // Multiple scroll events while paused — none should trigger
            window.dispatchEvent(new Event('scroll'));
            window.dispatchEvent(new Event('scroll'));

            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should call LoadNextBatch once flag is cleared', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            window.__infiniteScrollPaused = true;
            trigger.getBoundingClientRect = () => ({ top: 900 });

            mod.observeScrollTrigger(helper, 'scroll-trigger');
            window.dispatchEvent(new Event('scroll'));
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();

            // Simulate nav-helpers.js clearing the flag when restore completes
            window.__infiniteScrollPaused = false;
            window.dispatchEvent(new Event('scroll'));

            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
        });

        it('should work normally when flag is not set (forward navigation)', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // No flag set (undefined) — treated as not paused
            trigger.getBoundingClientRect = () => ({ top: 900 });

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
        });

        it('flag stays paused across multiple scroll events until explicitly cleared', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            window.__infiniteScrollPaused = true;
            trigger.getBoundingClientRect = () => ({ top: 900 });

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // __scrollRestoring persists until nav-helpers.js clears it —
            // it covers the entire restore window, not just the first scroll event
            window.dispatchEvent(new Event('scroll'));
            window.dispatchEvent(new Event('scroll'));
            window.dispatchEvent(new Event('scroll'));

            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();

            window.__infiniteScrollPaused = false;
            window.dispatchEvent(new Event('scroll'));

            expect(helper.invokeMethodAsync).toHaveBeenCalledTimes(1);
        });
    });
});
