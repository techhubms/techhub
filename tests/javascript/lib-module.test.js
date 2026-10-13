import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/TechHub.Web.lib.module.js';
const FLAGS = ['__blazorWebReady', '__blazorWebReadyAt', '__blazorServerReady', '__blazorWasmReady', '__scriptsReady', '__e2eSignal'];

function createBlazor() {
    const target = new EventTarget();
    return {
        addEventListener: vi.fn((name, handler) => target.addEventListener(name, handler)),
        dispatch: name => target.dispatchEvent(new Event(name)),
    };
}

describe('TechHub.Web.lib.module.js', () => {
    let mod;
    let documentListeners;

    beforeEach(async () => {
        // The module registers a document focusin listener that outlives the test; track it to remove it afterwards.
        documentListeners = [];
        const addEventListener = document.addEventListener.bind(document);
        vi.spyOn(document, 'addEventListener').mockImplementation((type, listener, options) => {
            documentListeners.push([type, listener, options]);
            addEventListener(type, listener, options);
        });
        FLAGS.forEach(flag => delete window[flag]);
        delete window.__isBlazorInteractiveReady;
        document.documentElement.className = '';
        vi.spyOn(console, 'debug').mockImplementation(() => { });
        vi.resetModules();
        mod = await import(MODULE_PATH);
    });

    afterEach(() => {
        documentListeners.forEach(([type, listener, options]) => document.removeEventListener(type, listener, options));
        vi.restoreAllMocks();
        FLAGS.forEach(flag => delete window[flag]);
    });

    describe('beforeWebStarted', () => {
        it('configures the circuit SignalR timeouts for background tab throttling', () => {
            const options = {};
            const builder = {};

            mod.beforeWebStarted(options);
            options.circuit.configureSignalR(builder);

            expect(builder.serverTimeoutInMilliseconds).toBe(120000);
            expect(builder.keepAliveIntervalInMilliseconds).toBe(15000);
        });

        it('keeps the server timeout at least four times the keep-alive interval', () => {
            const options = {};
            const builder = {};

            mod.beforeWebStarted(options);
            options.circuit.configureSignalR(builder);

            expect(builder.serverTimeoutInMilliseconds).toBeGreaterThanOrEqual(builder.keepAliveIntervalInMilliseconds * 4);
        });
    });

    describe('afterWebStarted', () => {
        it('marks Blazor Web ready with a timestamp', () => {
            const before = Date.now();

            mod.afterWebStarted(createBlazor());

            expect(window.__blazorWebReady).toBe(true);
            expect(window.__blazorWebReadyAt).toBeGreaterThanOrEqual(before);
            expect(window.__blazorWebReadyAt).toBeLessThanOrEqual(Date.now());
        });

        it('takes over scroll restoration from the browser', () => {
            history.scrollRestoration = 'auto';

            mod.afterWebStarted(createBlazor());

            expect(history.scrollRestoration).toBe('manual');
        });

        it('signals blazor-web-ready to the e2e hook when present', () => {
            window.__e2eSignal = vi.fn();

            mod.afterWebStarted(createBlazor());

            expect(window.__e2eSignal).toHaveBeenCalledWith('blazor-web-ready');
        });

        it('does not require an e2e hook', () => {
            expect(() => mod.afterWebStarted(createBlazor())).not.toThrow();
        });

        it('resets __scriptsReady and signals enhanced-nav on enhancedload', () => {
            const blazor = createBlazor();
            window.__e2eSignal = vi.fn();
            mod.afterWebStarted(blazor);
            window.__scriptsReady = true;

            blazor.dispatch('enhancedload');

            expect(blazor.addEventListener).toHaveBeenCalledWith('enhancedload', expect.any(Function));
            expect(window.__scriptsReady).toBe(false);
            expect(window.__e2eSignal).toHaveBeenCalledWith('enhanced-nav');
        });

        it('resets __scriptsReady on every enhanced navigation', () => {
            const blazor = createBlazor();
            mod.afterWebStarted(blazor);

            window.__scriptsReady = true;
            blazor.dispatch('enhancedload');
            window.__scriptsReady = true;
            blazor.dispatch('enhancedload');

            expect(window.__scriptsReady).toBe(false);
        });
    });

    describe('focus scroll compensation', () => {
        let scrollBy;

        function focusIn(element, top) {
            element.getBoundingClientRect = () => ({ top });
            element.dispatchEvent(new FocusEvent('focusin', { bubbles: true }));
        }

        beforeEach(() => {
            document.body.innerHTML = '<header><button id="nav">Nav</button></header><main><button id="target">Go</button></main>';
            scrollBy = vi.fn();
            window.scrollBy = scrollBy;
            mod.afterWebStarted(createBlazor());
        });

        afterEach(() => {
            document.body.innerHTML = '';
        });

        it('scrolls up when a keyboard-focused element is behind the sticky header', () => {
            document.documentElement.classList.add('keyboard-nav');

            focusIn(document.getElementById('target'), 20);

            // 142 (sticky header) - 20 (top) + 16 (padding)
            expect(scrollBy).toHaveBeenCalledWith({ top: -138, behavior: 'smooth' });
        });

        it('does not scroll when the element is already below the sticky header', () => {
            document.documentElement.classList.add('keyboard-nav');

            focusIn(document.getElementById('target'), 300);

            expect(scrollBy).not.toHaveBeenCalled();
        });

        it('does not scroll for pointer interaction (no keyboard-nav class)', () => {
            focusIn(document.getElementById('target'), 20);

            expect(scrollBy).not.toHaveBeenCalled();
        });

        it('ignores elements inside the sticky header itself', () => {
            document.documentElement.classList.add('keyboard-nav');

            focusIn(document.getElementById('nav'), 20);

            expect(scrollBy).not.toHaveBeenCalled();
        });
    });

    describe('afterServerStarted', () => {
        it('gates readiness on markScriptsReady and flags the circuit as ready', () => {
            window.__scriptsReady = true;

            mod.afterServerStarted(createBlazor());

            expect(window.__scriptsReady).toBe(false);
            expect(window.__blazorServerReady).toBe(true);
        });

        it('signals blazor-server-ready to the e2e hook when present', () => {
            window.__e2eSignal = vi.fn();

            mod.afterServerStarted(createBlazor());

            expect(window.__e2eSignal).toHaveBeenCalledWith('blazor-server-ready');
        });
    });

    describe('afterWebAssemblyStarted', () => {
        it('flags the WebAssembly runtime as ready and signals the e2e hook', () => {
            window.__e2eSignal = vi.fn();

            mod.afterWebAssemblyStarted(createBlazor());

            expect(window.__blazorWasmReady).toBe(true);
            expect(window.__e2eSignal).toHaveBeenCalledWith('blazor-wasm-ready');
        });
    });

    describe('__isBlazorInteractiveReady', () => {
        it('is false before any interactive runtime has started', () => {
            expect(window.__isBlazorInteractiveReady()).toBe(false);
        });

        it('is false after only the SSR web start', () => {
            mod.afterWebStarted(createBlazor());

            expect(window.__isBlazorInteractiveReady()).toBe(false);
        });

        it('is true once the server circuit is ready', () => {
            mod.afterServerStarted(createBlazor());

            expect(window.__isBlazorInteractiveReady()).toBe(true);
        });

        it('is true once WebAssembly is ready', () => {
            mod.afterWebAssemblyStarted(createBlazor());

            expect(window.__isBlazorInteractiveReady()).toBe(true);
        });
    });
});
