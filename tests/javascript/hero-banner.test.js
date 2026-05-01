import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/hero-banner.js';

describe('hero-banner.js', () => {
    beforeEach(() => {
        document.cookie = '';
        delete window.TechHub;
        vi.resetModules();
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    it('should expose TechHub.heroBanner on window', async () => {
        await import(MODULE_PATH);

        expect(window.TechHub).toBeDefined();
        expect(window.TechHub.heroBanner).toBeDefined();
        expect(typeof window.TechHub.heroBanner.setCollapsed).toBe('function');
        expect(typeof window.TechHub.heroBanner.setHash).toBe('function');
    });

    it('should set hero-banner-collapsed cookie to true', async () => {
        await import(MODULE_PATH);

        window.TechHub.heroBanner.setCollapsed(true);

        expect(document.cookie).toContain('hero-banner-collapsed=true');
    });

    it('should set hero-banner-collapsed cookie to false', async () => {
        await import(MODULE_PATH);

        window.TechHub.heroBanner.setCollapsed(false);

        expect(document.cookie).toContain('hero-banner-collapsed=false');
    });

    it('should set hero-banner-hash cookie with provided hash', async () => {
        await import(MODULE_PATH);

        window.TechHub.heroBanner.setHash('abc123');

        expect(document.cookie).toContain('hero-banner-hash=abc123');
    });

    it('should URL-encode special characters in hash', async () => {
        await import(MODULE_PATH);

        window.TechHub.heroBanner.setHash('hello world&foo=bar');

        expect(document.cookie).toContain('hero-banner-hash=hello%20world%26foo%3Dbar');
    });

    it('should persist cookie value across multiple calls', async () => {
        await import(MODULE_PATH);

        window.TechHub.heroBanner.setCollapsed(true);
        window.TechHub.heroBanner.setHash('hash1');

        // jsdom document.cookie getter only shows name=value (no attributes).
        // Verify both cookies are set with correct values.
        expect(document.cookie).toContain('hero-banner-collapsed=true');
        expect(document.cookie).toContain('hero-banner-hash=hash1');
    });

    it('should not clobber existing TechHub namespace properties', async () => {
        window.TechHub = { sidebar: { toggle: () => {} } };
        await import(MODULE_PATH);

        expect(window.TechHub.sidebar).toBeDefined();
        expect(window.TechHub.heroBanner).toBeDefined();
    });
});
