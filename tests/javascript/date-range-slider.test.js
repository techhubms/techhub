import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/date-range-slider.js';

describe('date-range-slider.js', () => {
    let container;

    beforeEach(async () => {
        document.body.innerHTML = '';
        vi.resetModules();

        // Create a slider container with from/to sliders and fill
        container = document.createElement('div');
        container.className = 'slider-container';
        container.innerHTML = `
            <input type="range" class="slider-from" min="0" max="100" value="20">
            <input type="range" class="slider-to" min="0" max="100" value="80">
            <div class="slider-fill"></div>
        `;
        document.body.appendChild(container);
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    async function getModule() {
        return await import(MODULE_PATH);
    }

    it('should export initClamping function', async () => {
        const mod = await getModule();
        expect(typeof mod.initClamping).toBe('function');
    });

    it('should warn and return early if container is null', async () => {
        const mod = await getModule();
        const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});

        mod.initClamping(null);

        expect(warn).toHaveBeenCalledWith(
            '[DateRangeSlider] Container element not available, skipping clamping init'
        );
    });

    it('should warn if slider elements are missing', async () => {
        const mod = await getModule();
        const warn = vi.spyOn(console, 'warn').mockImplementation(() => {});

        const emptyContainer = document.createElement('div');
        mod.initClamping(emptyContainer);

        expect(warn).toHaveBeenCalledWith(
            '[DateRangeSlider] Could not find slider elements for clamping'
        );
    });

    it('should clamp from-slider to not exceed to-slider', async () => {
        const mod = await getModule();
        mod.initClamping(container);

        const fromSlider = container.querySelector('.slider-from');
        const toSlider = container.querySelector('.slider-to');

        // Set to-slider to 50, then try to move from-slider past it
        toSlider.value = '50';
        fromSlider.value = '60';

        // Dispatch input event on from-slider (capture listener on container fires with e.target = fromSlider)
        const event = new Event('input', { bubbles: true });
        fromSlider.dispatchEvent(event);

        expect(fromSlider.value).toBe('50');
    });

    it('should clamp to-slider to not go below from-slider', async () => {
        const mod = await getModule();
        mod.initClamping(container);

        const fromSlider = container.querySelector('.slider-from');
        const toSlider = container.querySelector('.slider-to');

        // Set from-slider to 40, then try to move to-slider below it
        fromSlider.value = '40';
        toSlider.value = '30';

        // Dispatch input event on to-slider (capture listener on container fires with e.target = toSlider)
        const event = new Event('input', { bubbles: true });
        toSlider.dispatchEvent(event);

        expect(toSlider.value).toBe('40');
    });

    it('should allow from-slider value equal to to-slider value', async () => {
        const mod = await getModule();
        mod.initClamping(container);

        const fromSlider = container.querySelector('.slider-from');
        const toSlider = container.querySelector('.slider-to');

        toSlider.value = '50';
        fromSlider.value = '50';

        const event = new Event('input', { bubbles: true });
        fromSlider.dispatchEvent(event);

        // Should not clamp — equal is allowed
        expect(fromSlider.value).toBe('50');
    });

    it('should update fill element position on input', async () => {
        const mod = await getModule();
        mod.initClamping(container);

        const fromSlider = container.querySelector('.slider-from');
        const fill = container.querySelector('.slider-fill');

        fromSlider.value = '25';

        const event = new Event('input', { bubbles: true });
        fromSlider.dispatchEvent(event);

        // from=25, to=80, max=100 → left=25%, width=55%
        expect(fill.style.left).toBe('25%');
        // Floating-point arithmetic: ((80-25)/100)*100 may produce 55.00000000000001
        expect(parseFloat(fill.style.width)).toBeCloseTo(55, 5);
    });

    it('should update fill when to-slider changes', async () => {
        const mod = await getModule();
        mod.initClamping(container);

        const toSlider = container.querySelector('.slider-to');
        const fill = container.querySelector('.slider-fill');

        toSlider.value = '60';

        const event = new Event('input', { bubbles: true });
        toSlider.dispatchEvent(event);

        // from=20, to=60, max=100 → left=20%, width=40%
        expect(fill.style.left).toBe('20%');
        expect(fill.style.width).toBe('40%');
    });

    it('should not clamp when values are within range', async () => {
        const mod = await getModule();
        mod.initClamping(container);

        const fromSlider = container.querySelector('.slider-from');
        const toSlider = container.querySelector('.slider-to');

        fromSlider.value = '30';
        toSlider.value = '70';

        const event = new Event('input', { bubbles: true });
        fromSlider.dispatchEvent(event);

        expect(fromSlider.value).toBe('30');
        expect(toSlider.value).toBe('70');
    });
});
