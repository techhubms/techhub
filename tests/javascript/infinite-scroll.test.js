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
        delete window.__gridScrollPositions;
        delete window.__scrollListenerReady;
        delete window.__scrollListenerVersion;
        delete window.__scrollRestoredAt;
        delete window.__e2eSignal;

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

            mod.observeScrollTrigger(helper, 'nonexistent-trigger', 'key1');

            expect(warn).toHaveBeenCalledWith(
                '[InfiniteScroll] Trigger element not found:',
                'nonexistent-trigger'
            );
        });

        it('should attach scroll listener when trigger element exists', () => {
            createTriggerElement();
            const helper = createMockHelper();
            const addSpy = vi.spyOn(window, 'addEventListener');

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

            expect(addSpy).toHaveBeenCalledWith('scroll', expect.any(Function), { passive: true });
        });

        it('should set __scrollListenerReady to true', () => {
            createTriggerElement();
            const helper = createMockHelper();

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

            expect(window.__scrollListenerReady['scroll-trigger']).toBe(true);
        });

        it('should increment __scrollListenerVersion', () => {
            createTriggerElement();
            const helper = createMockHelper();

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');
            expect(window.__scrollListenerVersion['scroll-trigger']).toBe(1);

            // Re-attach (requires re-creating trigger since dispose removes reference)
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');
            expect(window.__scrollListenerVersion['scroll-trigger']).toBe(2);
        });

        it('should fire e2eSignal when attached', () => {
            createTriggerElement();
            const helper = createMockHelper();
            window.__e2eSignal = vi.fn();

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

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

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

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

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should dispose previous listener before attaching new one', () => {
            createTriggerElement();
            const helper = createMockHelper();
            const removeSpy = vi.spyOn(window, 'removeEventListener');

            // First attach
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

            // Second attach should dispose first
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key2');

            expect(removeSpy).toHaveBeenCalledWith('scroll', expect.any(Function));
        });
    });

    describe('scroll event handling', () => {
        it('should save scroll position on scroll', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // Trigger far away so LoadNextBatch doesn't fire
            trigger.getBoundingClientRect = () => ({ top: 5000 });

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'my-state-key');

            // Simulate scroll
            Object.defineProperty(window, 'scrollY', { value: 450, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            expect(window.__gridScrollPositions['my-state-key']).toBe(450);
        });

        it('should call LoadNextBatch when scrolling brings trigger into margin', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // Start with trigger far away
            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

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
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

            // Simulate URL change (enhanced navigation)
            window.location = { pathname: '/different-page', search: '' };

            Object.defineProperty(window, 'scrollY', { value: 100, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            // Should NOT have updated the position to 100 (the corrupted scroll)
            // The initial handleScroll() at attach time saved 0, that stays.
            expect(window.__gridScrollPositions['key1']).toBe(0);
            // Should have disposed
            expect(window.__scrollListenerReady['scroll-trigger']).toBe(false);
        });

        it('should not save position with null stateKey', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger', null);

            Object.defineProperty(window, 'scrollY', { value: 200, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            // __gridScrollPositions should remain empty
            expect(Object.keys(window.__gridScrollPositions)).toHaveLength(0);
        });
    });

    describe('dispose', () => {
        it('should remove scroll event listener', () => {
            createTriggerElement();
            const helper = createMockHelper();
            const removeSpy = vi.spyOn(window, 'removeEventListener');

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');
            mod.dispose();

            expect(removeSpy).toHaveBeenCalledWith('scroll', expect.any(Function));
        });

        it('should set __scrollListenerReady to false', () => {
            createTriggerElement();
            const helper = createMockHelper();

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');
            expect(window.__scrollListenerReady['scroll-trigger']).toBe(true);

            mod.dispose();
            expect(window.__scrollListenerReady['scroll-trigger']).toBe(false);
        });

        it('should fire e2eSignal on dispose', () => {
            createTriggerElement();
            const helper = createMockHelper();
            window.__e2eSignal = vi.fn();

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');
            window.__e2eSignal.mockClear();

            mod.dispose();

            expect(window.__e2eSignal).toHaveBeenCalledWith('scroll-disposed:scroll-trigger');
        });

        it('should be safe to call multiple times', () => {
            createTriggerElement();
            const helper = createMockHelper();

            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');
            mod.dispose();
            mod.dispose(); // Should not throw
        });

        it('should stop responding to scroll events after dispose', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');
            mod.dispose();

            // Scroll after dispose should not save position
            Object.defineProperty(window, 'scrollY', { value: 999, writable: true, configurable: true });
            trigger.getBoundingClientRect = () => ({ top: 500 });
            window.dispatchEvent(new Event('scroll'));

            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
            // The position saved at attach time (scrollY=0) persists, but no new writes happen
            expect(window.__gridScrollPositions['key1']).toBe(0);
        });
    });

    describe('restoreScrollPosition', () => {
        it('should return false if no saved position', () => {
            const result = mod.restoreScrollPosition('unknown-key');
            expect(result).toBe(false);
        });

        it('should return false if saved position is 0', () => {
            window.__gridScrollPositions = { 'key1': 0 };
            const result = mod.restoreScrollPosition('key1');
            expect(result).toBe(false);
        });

        it('should scroll to saved position and return true', () => {
            window.__gridScrollPositions = { 'key1': 750 };

            const result = mod.restoreScrollPosition('key1');

            expect(result).toBe(true);
            expect(window.scrollTo).toHaveBeenCalledWith(0, 750);
        });

        it('should set __scrollRestoredAt timestamp', () => {
            window.__gridScrollPositions = { 'key1': 500 };
            const before = Date.now();

            mod.restoreScrollPosition('key1');

            expect(window.__scrollRestoredAt).toBeGreaterThanOrEqual(before);
            expect(window.__scrollRestoredAt).toBeLessThanOrEqual(Date.now());
        });

        it('should work with different state keys independently', () => {
            window.__gridScrollPositions = {
                'page-a': 100,
                'page-b': 2000,
            };

            mod.restoreScrollPosition('page-a');
            expect(window.scrollTo).toHaveBeenCalledWith(0, 100);

            mod.restoreScrollPosition('page-b');
            expect(window.scrollTo).toHaveBeenCalledWith(0, 2000);
        });
    });

    describe('scroll position persistence across lifecycle', () => {
        it('should persist scroll positions across dispose/re-attach cycles', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

            // Scroll to position
            Object.defineProperty(window, 'scrollY', { value: 600, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));

            expect(window.__gridScrollPositions['key1']).toBe(600);

            // Dispose and re-attach (simulates navigation away and back)
            mod.dispose();

            // Position should still be in global state
            expect(window.__gridScrollPositions['key1']).toBe(600);

            // Restore should work
            const restored = mod.restoreScrollPosition('key1');
            expect(restored).toBe(true);
            expect(window.scrollTo).toHaveBeenCalledWith(0, 600);
        });

        it('should update position on subsequent scrolls', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            trigger.getBoundingClientRect = () => ({ top: 5000 });
            mod.observeScrollTrigger(helper, 'scroll-trigger', 'key1');

            // First scroll
            Object.defineProperty(window, 'scrollY', { value: 200, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));
            expect(window.__gridScrollPositions['key1']).toBe(200);

            // Second scroll further down
            Object.defineProperty(window, 'scrollY', { value: 800, writable: true, configurable: true });
            window.dispatchEvent(new Event('scroll'));
            expect(window.__gridScrollPositions['key1']).toBe(800);
        });
    });

    describe('__gridScrollPositions global initialization', () => {
        it('should initialize as empty object if not already set', () => {
            // The module initializes this at import time
            expect(window.__gridScrollPositions).toBeDefined();
            expect(typeof window.__gridScrollPositions).toBe('object');
        });
    });
});
