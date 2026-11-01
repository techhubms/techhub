import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/custom-pages.js';

describe('custom-pages.js', () => {
    beforeEach(() => {
        document.body.innerHTML = '';
        vi.resetModules();
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    describe('initCollapsibleCards', () => {
        it('should toggle SDLC phase content on header click', async () => {
            document.body.innerHTML = `
                <div class="sdlc-phase-header" aria-expanded="false">
                    <span class="sdlc-phase-toggle"></span>
                </div>
                <div class="sdlc-phase-content"></div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initCollapsibleCards();

            const header = document.querySelector('.sdlc-phase-header');
            header.click();

            expect(header.getAttribute('aria-expanded')).toBe('true');
            expect(header.nextElementSibling.classList.contains('expanded')).toBe(true);
            expect(header.querySelector('.sdlc-phase-toggle').classList.contains('expanded')).toBe(true);
        });

        it('should collapse SDLC phase on second click', async () => {
            document.body.innerHTML = `
                <div class="sdlc-phase-header" aria-expanded="false">
                    <span class="sdlc-phase-toggle"></span>
                </div>
                <div class="sdlc-phase-content"></div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initCollapsibleCards();

            const header = document.querySelector('.sdlc-phase-header');
            header.click(); // expand
            header.click(); // collapse

            expect(header.getAttribute('aria-expanded')).toBe('false');
            expect(header.nextElementSibling.classList.contains('expanded')).toBe(false);
        });

        it('should toggle SDLC card content on header click', async () => {
            document.body.innerHTML = `
                <div class="sdlc-card-header" aria-expanded="false">
                    <span class="sdlc-card-icon"></span>
                </div>
                <div class="sdlc-card-content"></div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initCollapsibleCards();

            const header = document.querySelector('.sdlc-card-header');
            header.click();

            expect(header.getAttribute('aria-expanded')).toBe('true');
            expect(header.nextElementSibling.classList.contains('expanded')).toBe(true);
            expect(header.querySelector('.sdlc-card-icon').classList.contains('expanded')).toBe(true);
        });

        it('should toggle DX card content on header click', async () => {
            document.body.innerHTML = `
                <div class="dx-card-header" aria-expanded="false">
                    <span class="dx-card-icon"></span>
                </div>
                <div class="dx-card-content"></div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initCollapsibleCards();

            const header = document.querySelector('.dx-card-header');
            header.click();

            expect(header.getAttribute('aria-expanded')).toBe('true');
            expect(header.nextElementSibling.classList.contains('expanded')).toBe(true);
        });

        it('should not initialize same header twice', async () => {
            document.body.innerHTML = `
                <div class="sdlc-phase-header" aria-expanded="false">
                    <span class="sdlc-phase-toggle"></span>
                </div>
                <div class="sdlc-phase-content"></div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initCollapsibleCards();
            mod.initCollapsibleCards(); // Second call should be idempotent

            const header = document.querySelector('.sdlc-phase-header');
            header.click();

            // Should toggle only once (not double-toggle back to false)
            expect(header.getAttribute('aria-expanded')).toBe('true');
        });
    });

    describe('initExpandableBadges', () => {
        it('should reveal hidden content and remove button on click', async () => {
            document.body.innerHTML = `
                <button class="badge-expandable" data-expand-target="hidden-badges">+3 more</button>
                <div id="hidden-badges" hidden>Extra badges</div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initExpandableBadges();

            const button = document.querySelector('.badge-expandable');
            button.click();

            expect(document.getElementById('hidden-badges').hidden).toBe(false);
            expect(document.querySelector('.badge-expandable')).toBeNull();
        });

        it('should not initialize same button twice', async () => {
            document.body.innerHTML = `
                <button class="badge-expandable" data-expand-target="hidden-badges">+3 more</button>
                <div id="hidden-badges" hidden>Extra badges</div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initExpandableBadges();
            mod.initExpandableBadges(); // Idempotent

            const button = document.querySelector('.badge-expandable');
            button.click();

            expect(document.getElementById('hidden-badges').hidden).toBe(false);
        });

        it('should handle missing target element gracefully', async () => {
            document.body.innerHTML = `
                <button class="badge-expandable" data-expand-target="nonexistent">+3 more</button>
            `;

            const mod = await import(MODULE_PATH);
            mod.initExpandableBadges();

            // Should not throw
            const button = document.querySelector('.badge-expandable');
            button.click();

            // Button should still be in DOM since target wasn't found
            expect(document.querySelector('.badge-expandable')).not.toBeNull();
        });

        it('should stop event propagation on click', async () => {
            document.body.innerHTML = `
                <div id="parent">
                    <button class="badge-expandable" data-expand-target="hidden-badges">+3 more</button>
                    <div id="hidden-badges" hidden>Extra badges</div>
                </div>
            `;

            const mod = await import(MODULE_PATH);
            mod.initExpandableBadges();

            const parentClickHandler = vi.fn();
            document.getElementById('parent').addEventListener('click', parentClickHandler);

            const button = document.querySelector('.badge-expandable');
            button.click();

            expect(parentClickHandler).not.toHaveBeenCalled();
        });
    });
});
