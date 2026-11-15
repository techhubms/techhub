import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/content-links.js';

/** Makes window.matchMedia report whether the device has a mouse or trackpad (a fine pointer with hover). */
function setDesktop(isDesktop) {
    window.matchMedia = vi.fn().mockImplementation(query => ({
        matches: isDesktop && query === '(hover: hover) and (pointer: fine)',
        media: query
    }));
}

function addLink({ contentLink = true, target = null, label = null, text = 'An article' } = {}) {
    const link = document.createElement('a');
    link.href = '/ai/videos/an-article';
    link.textContent = text;
    if (contentLink) link.setAttribute('data-content-link', '');
    if (target) link.target = target;
    if (label) link.setAttribute('aria-label', label);
    document.body.appendChild(link);
    return link;
}

/** Dispatches a click like a browser does: bubbling, cancelable, with the given details. */
function click(element, init = {}) {
    const event = new MouseEvent('click', { bubbles: true, cancelable: true, button: 0, ...init });
    element.dispatchEvent(event);
    return event;
}

describe('content-links.js', () => {
    beforeEach(async () => {
        document.body.innerHTML = '';
        delete window.TechHub;
        vi.resetModules();
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    // The script adds its listeners to the real document for the lifetime of the page, so they are recorded and removed after each test.
    async function loadOnDocument(isDesktop) {
        setDesktop(isDesktop);
        const added = [];
        const original = document.addEventListener.bind(document);
        vi.spyOn(document, 'addEventListener').mockImplementation((type, listener, options) => {
            added.push([type, listener, options]);
            original(type, listener, options);
        });
        await import(MODULE_PATH);
        return () => added.forEach(([type, listener, options]) => document.removeEventListener(type, listener, options));
    }

    it('should expose TechHub.contentLinks.opensInNewTab on window', async () => {
        const unload = await loadOnDocument(true);

        expect(typeof window.TechHub.contentLinks.opensInNewTab).toBe('function');
        unload();
    });

    it('should report a new tab on a device with a mouse or trackpad and not on a touch device', async () => {
        let unload = await loadOnDocument(true);
        expect(window.TechHub.contentLinks.opensInNewTab()).toBe(true);
        unload();
        vi.restoreAllMocks();

        vi.resetModules();
        unload = await loadOnDocument(false);
        expect(window.TechHub.contentLinks.opensInNewTab()).toBe(false);
        unload();
    });

    describe('on a desktop', () => {
        let unload;
        beforeEach(async () => { unload = await loadOnDocument(true); });
        afterEach(() => unload());

        it('should open an internal content link in a new tab', () => {
            const link = addLink();

            click(link);

            expect(link.target).toBe('_blank');
            expect(link.rel).toBe('noopener noreferrer');
        });

        it('should open a link whose click happened on an element inside it', () => {
            const link = addLink();
            const inner = document.createElement('span');
            link.appendChild(inner);

            click(inner);

            expect(link.target).toBe('_blank');
        });

        it('should not touch a link that is not a content link', () => {
            const link = addLink({ contentLink: false });

            click(link);

            expect(link.target).toBe('');
        });

        it.each([
            ['Ctrl', { ctrlKey: true }],
            ['Cmd', { metaKey: true }],
            ['Shift', { shiftKey: true }],
            ['Alt', { altKey: true }],
            ['the middle button', { button: 1 }]
        ])('should leave a click with %s to the browser', (_name, init) => {
            const link = addLink();

            click(link, init);

            expect(link.target).toBe('');
        });

        it('should leave a click that was already cancelled alone', () => {
            const link = addLink();
            // A listener on window runs before the capture listener of the script on document.
            const cancel = e => e.preventDefault();
            window.addEventListener('click', cancel, true);

            const event = click(link);
            window.removeEventListener('click', cancel, true);

            expect(event.defaultPrevented).toBe(true);
            expect(link.target).toBe('');
        });

        it('should announce the new tab when the link is pointed at', () => {
            const link = addLink({ label: 'An article' });

            link.dispatchEvent(new MouseEvent('pointerover', { bubbles: true }));

            expect(link.getAttribute('aria-label')).toBe('An article - opens in new tab');
        });

        it('should use the text of a link without a label and announce only once', () => {
            const link = addLink({ text: 'Plain link' });

            link.dispatchEvent(new FocusEvent('focusin', { bubbles: true }));
            link.dispatchEvent(new FocusEvent('focusin', { bubbles: true }));

            expect(link.getAttribute('aria-label')).toBe('Plain link - opens in new tab');
        });

        it('should not repeat the note on a link that already announces it', () => {
            const link = addLink({ label: 'An article - opens in new tab' });

            link.dispatchEvent(new MouseEvent('pointerover', { bubbles: true }));

            expect(link.getAttribute('aria-label')).toBe('An article - opens in new tab');
        });
    });

    describe('on a touch device', () => {
        let unload;
        beforeEach(async () => { unload = await loadOnDocument(false); });
        afterEach(() => unload());

        it('should keep an internal content link in the same tab', () => {
            const link = addLink();

            click(link);

            expect(link.target).toBe('');
        });

        it('should keep the target the server rendered for an external link', () => {
            const link = addLink({ target: '_blank' });

            click(link);

            expect(link.target).toBe('_blank');
        });

        it('should not change the label of a link', () => {
            const link = addLink({ label: 'An article' });

            link.dispatchEvent(new MouseEvent('pointerover', { bubbles: true }));

            expect(link.getAttribute('aria-label')).toBe('An article');
        });
    });

    it('should do nothing when matchMedia is not available', async () => {
        const unload = await loadOnDocument(true);
        delete window.matchMedia;
        const link = addLink();

        click(link);

        expect(link.target).toBe('');
        unload();
    });
});
