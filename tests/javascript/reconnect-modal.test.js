import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/Components/Layout/ReconnectModal.razor.js';

function stateChanged(modal, state) {
    modal.dispatchEvent(new CustomEvent('components-reconnect-state-changed', { detail: { state } }));
}

// Lets the awaited Blazor calls inside the click handlers settle.
const flush = async () => {
    for (let i = 0; i < 5; i++) await Promise.resolve();
};

describe('ReconnectModal.razor.js', () => {
    let modal;
    let reload;
    let documentListeners;

    beforeEach(async () => {
        // The module registers document listeners that outlive the test; track them to remove them afterwards.
        documentListeners = [];
        const addEventListener = document.addEventListener.bind(document);
        vi.spyOn(document, 'addEventListener').mockImplementation((type, listener, options) => {
            documentListeners.push([type, listener, options]);
            addEventListener(type, listener, options);
        });

        document.body.innerHTML = `
            <dialog id="components-reconnect-modal">
                <button id="components-reconnect-button">Retry</button>
                <button id="components-reload-button">Reload</button>
                <button id="components-resume-button">Resume</button>
            </dialog>`;
        modal = document.getElementById('components-reconnect-modal');
        modal.showModal = vi.fn();
        modal.close = vi.fn();

        reload = vi.fn();
        vi.stubGlobal('location', { reload, href: 'http://localhost/page' });
        vi.stubGlobal('Blazor', { reconnect: vi.fn(), resumeCircuit: vi.fn() });

        vi.resetModules();
        await import(MODULE_PATH);
    });

    afterEach(() => {
        documentListeners.forEach(([type, listener, options]) => document.removeEventListener(type, listener, options));
        vi.useRealTimers();
        vi.unstubAllGlobals();
        vi.restoreAllMocks();
        document.body.innerHTML = '';
    });

    describe('state changes', () => {
        it('shows the modal on "show"', () => {
            stateChanged(modal, 'show');

            expect(modal.showModal).toHaveBeenCalledTimes(1);
        });

        it('fades out and closes the modal 250 ms after "hide"', () => {
            vi.useFakeTimers();

            stateChanged(modal, 'hide');

            expect(modal.style.opacity).toBe('0');
            expect(modal.close).not.toHaveBeenCalled();

            vi.advanceTimersByTime(249);
            expect(modal.close).not.toHaveBeenCalled();

            vi.advanceTimersByTime(1);
            expect(modal.close).toHaveBeenCalledTimes(1);
            expect(modal.style.opacity).toBe('');
            expect(modal.style.transition).toBe('');
        });

        it('reloads the page on "rejected"', () => {
            stateChanged(modal, 'rejected');

            expect(reload).toHaveBeenCalledTimes(1);
        });

        it('retries when the tab becomes visible after "failed"', async () => {
            Blazor.reconnect.mockResolvedValue(true);
            vi.spyOn(document, 'visibilityState', 'get').mockReturnValue('visible');

            stateChanged(modal, 'failed');
            document.dispatchEvent(new Event('visibilitychange'));
            await flush();

            expect(Blazor.reconnect).toHaveBeenCalledTimes(1);
        });

        it('does not retry while the tab is hidden after "failed"', async () => {
            vi.spyOn(document, 'visibilityState', 'get').mockReturnValue('hidden');

            stateChanged(modal, 'failed');
            document.dispatchEvent(new Event('visibilitychange'));
            await flush();

            expect(Blazor.reconnect).not.toHaveBeenCalled();
        });
    });

    describe('retry button', () => {
        const click = async () => {
            document.getElementById('components-reconnect-button').click();
            await flush();
        };

        it('does nothing further when the reconnect succeeds', async () => {
            Blazor.reconnect.mockResolvedValue(true);

            await click();

            expect(Blazor.resumeCircuit).not.toHaveBeenCalled();
            expect(reload).not.toHaveBeenCalled();
            expect(modal.close).not.toHaveBeenCalled();
        });

        it('resumes the circuit and closes the modal when the server rejected the reconnect', async () => {
            Blazor.reconnect.mockResolvedValue(false);
            Blazor.resumeCircuit.mockResolvedValue(true);

            await click();

            expect(modal.close).toHaveBeenCalledTimes(1);
            expect(reload).not.toHaveBeenCalled();
        });

        it('reloads when the circuit cannot be resumed', async () => {
            Blazor.reconnect.mockResolvedValue(false);
            Blazor.resumeCircuit.mockResolvedValue(false);

            await click();

            expect(reload).toHaveBeenCalledTimes(1);
        });

        it('waits for the tab to become visible again when the server is unreachable', async () => {
            Blazor.reconnect.mockRejectedValueOnce(new Error('unreachable')).mockResolvedValue(true);
            vi.spyOn(document, 'visibilityState', 'get').mockReturnValue('visible');

            await click();
            expect(Blazor.reconnect).toHaveBeenCalledTimes(1);

            document.dispatchEvent(new Event('visibilitychange'));
            await flush();

            expect(Blazor.reconnect).toHaveBeenCalledTimes(2);
            expect(reload).not.toHaveBeenCalled();
        });

        it('stops listening for visibility changes once a retry starts', async () => {
            Blazor.reconnect.mockResolvedValue(true);
            vi.spyOn(document, 'visibilityState', 'get').mockReturnValue('visible');
            stateChanged(modal, 'failed');

            await click();
            document.dispatchEvent(new Event('visibilitychange'));
            await flush();

            expect(Blazor.reconnect).toHaveBeenCalledTimes(1);
        });
    });

    describe('resume button', () => {
        const click = async () => {
            document.getElementById('components-resume-button').click();
            await flush();
        };

        it('does not reload when the circuit resumes', async () => {
            Blazor.resumeCircuit.mockResolvedValue(true);

            await click();

            expect(reload).not.toHaveBeenCalled();
        });

        it('reloads when the circuit cannot be resumed', async () => {
            Blazor.resumeCircuit.mockResolvedValue(false);

            await click();

            expect(reload).toHaveBeenCalledTimes(1);
        });

        it('reloads when resuming throws', async () => {
            Blazor.resumeCircuit.mockRejectedValue(new Error('boom'));

            await click();

            expect(reload).toHaveBeenCalledTimes(1);
        });
    });

    describe('reload button', () => {
        it('navigates to the current URL', () => {
            const assigned = vi.fn();
            vi.stubGlobal('location', {
                reload,
                get href() { return 'http://localhost/page'; },
                set href(value) { assigned(value); },
            });

            document.getElementById('components-reload-button').click();

            expect(assigned).toHaveBeenCalledWith('http://localhost/page');
        });
    });
});
