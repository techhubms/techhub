import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/page-scripts.js';

describe('page-scripts.js', () => {
    beforeEach(() => {
        document.body.innerHTML = '';
        delete window.initHighlighting;
        delete window.initMermaid;
        delete window.initTocScrollSpy;
        delete window.initCustomPages;
        delete window.markScriptsLoading;
        delete window.markScriptsReady;
        delete window.__scriptsReady;
        delete window.__e2eSignal;

        // Provide CDN config that page-scripts.js expects
        window.TechHubCDN = {
            highlightJs: {
                cdnUrl: 'https://cdn.example.com/hljs',
                themeFile: 'styles/dark.min.css',
                languages: ['javascript', 'csharp'],
                thirdPartyLanguages: {},
            },
            mermaid: {
                cdnUrl: 'https://cdn.example.com/mermaid/mermaid.min.js',
            },
        };

        vi.resetModules();
    });

    afterEach(() => {
        delete window.TechHubCDN;
        vi.restoreAllMocks();
    });

    it('should export initHighlighting, initMermaid, initTocScrollSpy, initCustomPages', async () => {
        const mod = await import(MODULE_PATH);

        expect(typeof mod.initHighlighting).toBe('function');
        expect(typeof mod.initMermaid).toBe('function');
        expect(typeof mod.initTocScrollSpy).toBe('function');
        expect(typeof mod.initCustomPages).toBe('function');
    });

    it('should expose init functions globally on window', async () => {
        await import(MODULE_PATH);

        expect(typeof window.initHighlighting).toBe('function');
        expect(typeof window.initMermaid).toBe('function');
        expect(typeof window.initTocScrollSpy).toBe('function');
        expect(typeof window.initCustomPages).toBe('function');
    });

    describe('markScriptsLoading / markScriptsReady', () => {
        it('should expose markScriptsLoading and markScriptsReady', async () => {
            await import(MODULE_PATH);

            expect(typeof window.markScriptsLoading).toBe('function');
            expect(typeof window.markScriptsReady).toBe('function');
        });

        it('should set __scriptsReady=false on loading', async () => {
            await import(MODULE_PATH);

            window.markScriptsLoading();

            expect(window.__scriptsReady).toBe(false);
        });

        it('should set __scriptsReady=true on ready', async () => {
            await import(MODULE_PATH);

            window.markScriptsLoading();
            window.markScriptsReady();

            expect(window.__scriptsReady).toBe(true);
        });

        it('should fire e2eSignal events', async () => {
            await import(MODULE_PATH);

            const signals = [];
            window.__e2eSignal = (name) => signals.push(name);

            window.markScriptsLoading();
            window.markScriptsReady();

            expect(signals).toContain('scripts-loading');
            expect(signals).toContain('scripts-ready');
        });
    });

    describe('initHighlighting', () => {
        it('should return early if no pre code elements exist', async () => {
            const mod = await import(MODULE_PATH);

            // No <pre><code> in DOM — should not throw or load anything
            await mod.initHighlighting();

            // No scripts should have been appended
            const scripts = document.querySelectorAll('script');
            expect(scripts.length).toBe(0);
        });
    });

    describe('initMermaid', () => {
        it('should return early if no unprocessed mermaid elements exist', async () => {
            const mod = await import(MODULE_PATH);

            // No .mermaid elements
            await mod.initMermaid();

            const scripts = document.querySelectorAll('script');
            expect(scripts.length).toBe(0);
        });

        it('should skip already-processed mermaid elements', async () => {
            document.body.innerHTML = '<div class="mermaid" data-processed="true">graph TD</div>';

            const mod = await import(MODULE_PATH);
            await mod.initMermaid();

            // Should not attempt to load mermaid CDN
            const scripts = document.querySelectorAll('script');
            expect(scripts.length).toBe(0);
        });
    });

    describe('initTocScrollSpy', () => {
        it('should return early if no [data-toc-scroll-spy] element exists', async () => {
            const mod = await import(MODULE_PATH);

            // Should not throw
            await mod.initTocScrollSpy();
        });
    });

    describe('initCustomPages', () => {
        it('should return early if no collapsible or expandable elements exist', async () => {
            const mod = await import(MODULE_PATH);

            // Should not throw
            await mod.initCustomPages();
        });
    });
});
