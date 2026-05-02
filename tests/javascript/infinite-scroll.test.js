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

    describe('setSuppressNextTriggerCheck', () => {
        it('should NOT call LoadNextBatch on immediate check after setSuppressNextTriggerCheck', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // Set suppress flag (as ContentItemsGrid does on back-navigation)
            mod.setSuppressNextTriggerCheck();

            // Now attach scroll listener with trigger IN viewport (simulating layout
            // differences where trigger ends up within the 300px margin after back-nav)
            trigger.getBoundingClientRect = () => ({
                top: 900, // within innerHeight (800) + TRIGGER_MARGIN_PX (300) = 1100
                bottom: 910,
                left: 0,
                right: 100,
                width: 100,
                height: 10,
            });

            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // The immediate handleScroll() call should NOT trigger LoadNextBatch
            // because suppressNextTriggerCheck was set
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should allow LoadNextBatch on subsequent scroll after suppression', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            mod.setSuppressNextTriggerCheck();

            // Trigger in viewport — immediate handleScroll in observeScrollTrigger
            // consumes the suppressNextTriggerCheck flag (no LoadNextBatch call)
            trigger.getBoundingClientRect = () => ({ top: 900 });
            mod.observeScrollTrigger(helper, 'scroll-trigger');
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();

            // Subsequent user scroll: flag is already consumed, should trigger normally
            window.dispatchEvent(new Event('scroll'));
            expect(helper.invokeMethodAsync).toHaveBeenCalledWith('LoadNextBatch');
        });

        it('suppressNextTriggerCheck survives dispose/re-attach cycle', () => {
            const trigger = createTriggerElement();
            const helper = createMockHelper();

            // Set the flag
            mod.setSuppressNextTriggerCheck();

            // Dispose (as happens in observeScrollTrigger's first line)
            mod.dispose();

            // Re-attach with trigger in viewport
            trigger.getBoundingClientRect = () => ({
                top: 900,
                bottom: 910,
                left: 0,
                right: 100,
                width: 100,
                height: 10,
            });
            mod.observeScrollTrigger(helper, 'scroll-trigger');

            // Should still be suppressed
            expect(helper.invokeMethodAsync).not.toHaveBeenCalled();
        });
    });
});
