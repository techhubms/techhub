import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/mobile-nav.js';

describe('mobile-nav.js', () => {
    let originalScrollY;

    beforeEach(() => {
        document.body.style.position = '';
        document.body.style.top = '';
        document.body.style.width = '';
        delete window.mobileNav;

        originalScrollY = 0;
        Object.defineProperty(window, 'scrollY', {
            get: () => originalScrollY,
            configurable: true,
        });

        window.scrollTo = vi.fn();
        vi.resetModules();
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    it('should expose mobileNav on window', async () => {
        await import(MODULE_PATH);

        expect(window.mobileNav).toBeDefined();
        expect(typeof window.mobileNav.lockScroll).toBe('function');
        expect(typeof window.mobileNav.unlockScroll).toBe('function');
        expect(typeof window.mobileNav.registerEscapeHandler).toBe('function');
    });

    describe('lockScroll', () => {
        it('should set body to fixed position', async () => {
            await import(MODULE_PATH);
            originalScrollY = 150;

            window.mobileNav.lockScroll();

            expect(document.body.style.position).toBe('fixed');
            expect(document.body.style.top).toBe('-150px');
            expect(document.body.style.width).toBe('100%');
        });

        it('should handle zero scroll position', async () => {
            await import(MODULE_PATH);
            originalScrollY = 0;

            window.mobileNav.lockScroll();

            expect(document.body.style.position).toBe('fixed');
            // Browser (and jsdom) normalizes -0px to 0px
            expect(document.body.style.top).toBe('0px');
        });
    });

    describe('unlockScroll', () => {
        it('should restore body styles and scroll position', async () => {
            await import(MODULE_PATH);

            // Simulate locked state
            document.body.style.position = 'fixed';
            document.body.style.top = '-200px';
            document.body.style.width = '100%';

            window.mobileNav.unlockScroll();

            expect(document.body.style.position).toBe('');
            expect(document.body.style.top).toBe('');
            expect(document.body.style.width).toBe('');
            expect(window.scrollTo).toHaveBeenCalledWith(0, 200);
        });

        it('should handle missing top style gracefully', async () => {
            await import(MODULE_PATH);

            document.body.style.position = 'fixed';
            document.body.style.top = '';

            window.mobileNav.unlockScroll();

            // parseInt('0') * -1 = -0, which is equivalent to 0
            expect(window.scrollTo).toHaveBeenCalledWith(0, -0);
        });
    });

    describe('registerEscapeHandler', () => {
        it('should call CloseMenuFromJs on Escape key', async () => {
            await import(MODULE_PATH);

            const dotNetHelper = {
                invokeMethodAsync: vi.fn().mockResolvedValue(undefined),
            };

            window.mobileNav.registerEscapeHandler(dotNetHelper);

            const event = new KeyboardEvent('keydown', { key: 'Escape' });
            document.dispatchEvent(event);

            expect(dotNetHelper.invokeMethodAsync).toHaveBeenCalledWith('CloseMenuFromJs');
        });

        it('should not call CloseMenuFromJs on other keys', async () => {
            await import(MODULE_PATH);

            const dotNetHelper = {
                invokeMethodAsync: vi.fn().mockResolvedValue(undefined),
            };

            window.mobileNav.registerEscapeHandler(dotNetHelper);

            const event = new KeyboardEvent('keydown', { key: 'Enter' });
            document.dispatchEvent(event);

            expect(dotNetHelper.invokeMethodAsync).not.toHaveBeenCalled();
        });

        it('should store handler reference for cleanup', async () => {
            await import(MODULE_PATH);

            const dotNetHelper = {
                invokeMethodAsync: vi.fn().mockResolvedValue(undefined),
            };

            window.mobileNav.registerEscapeHandler(dotNetHelper);

            expect(window.mobileNav._escapeHandler).toBeDefined();
            expect(typeof window.mobileNav._escapeHandler).toBe('function');
        });
    });
});
