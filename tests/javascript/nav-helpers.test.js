import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/nav-helpers.js';

describe('nav-helpers.js', () => {
    let originalRAF;

    beforeEach(() => {
        document.body.innerHTML = '';
        document.documentElement.className = '';

        delete window.TechHub;
        delete window.__scrollRestoredAt;

        // Stub Blazor with addEventListener so nav-helpers.js can attach enhancedload
        // listeners immediately without entering the 200ms setInterval retry loop.
        // Without this stub, each import registers a long-lived interval/timeout pair
        // (200ms polling, 10s max) that keeps the Vitest process alive and makes the
        // suite slow and flaky.
        globalThis.Blazor = { addEventListener: vi.fn() };

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

        window.scrollTo = vi.fn();

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
});
