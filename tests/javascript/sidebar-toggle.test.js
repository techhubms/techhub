import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/sidebar-toggle.js';

describe('sidebar-toggle.js', () => {
    beforeEach(() => {
        document.documentElement.className = '';
        document.cookie = '';
        delete window.TechHub;
        vi.resetModules();
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    it('should expose TechHub.sidebar on window', async () => {
        await import(MODULE_PATH);

        expect(window.TechHub).toBeDefined();
        expect(window.TechHub.sidebar).toBeDefined();
        expect(typeof window.TechHub.sidebar.isCollapsed).toBe('function');
        expect(typeof window.TechHub.sidebar.toggle).toBe('function');
    });

    it('should report isCollapsed as false when class is not present', async () => {
        await import(MODULE_PATH);

        expect(window.TechHub.sidebar.isCollapsed()).toBe(false);
    });

    it('should report isCollapsed as true when class is present', async () => {
        document.documentElement.classList.add('sidebar-collapsed');
        await import(MODULE_PATH);

        expect(window.TechHub.sidebar.isCollapsed()).toBe(true);
    });

    it('should toggle sidebar-collapsed class on html element', async () => {
        await import(MODULE_PATH);

        window.TechHub.sidebar.toggle();
        expect(document.documentElement.classList.contains('sidebar-collapsed')).toBe(true);

        window.TechHub.sidebar.toggle();
        expect(document.documentElement.classList.contains('sidebar-collapsed')).toBe(false);
    });

    it('should set cookie to true when collapsing', async () => {
        await import(MODULE_PATH);

        window.TechHub.sidebar.toggle(); // Collapse

        expect(document.cookie).toContain('sidebar-collapsed=true');
    });

    it('should set cookie to false when expanding', async () => {
        document.documentElement.classList.add('sidebar-collapsed');
        await import(MODULE_PATH);

        window.TechHub.sidebar.toggle(); // Expand

        expect(document.cookie).toContain('sidebar-collapsed=false');
    });

    it('should set cookie value correctly on toggle', async () => {
        await import(MODULE_PATH);

        window.TechHub.sidebar.toggle();

        // jsdom document.cookie getter only exposes name=value pairs,
        // not attributes (SameSite, max-age, path). We verify the value is set.
        expect(document.cookie).toContain('sidebar-collapsed=true');
    });

    it('should not clobber existing TechHub namespace properties', async () => {
        window.TechHub = { existingProp: 'keep' };
        await import(MODULE_PATH);

        expect(window.TechHub.existingProp).toBe('keep');
        expect(window.TechHub.sidebar).toBeDefined();
    });
});
