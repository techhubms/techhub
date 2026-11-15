/**
 * Content Links - open links to content in a new tab on a desktop.
 *
 * Every link to a content item (the cards, the latest content of the home page, the cards of the features page, the sources of
 * the timeline) has `data-content-link`. The server renders the behaviour of a touch device: the original article of an external
 * collection opens in a new tab, a page of this site in the same tab. On a device with a mouse or trackpad every one of those links
 * opens in a new tab instead, because closing a tab (Ctrl+W or a click) is easy there and it brings the reader straight back to the
 * list, the scroll position and the filters they came from. Phones and tablets keep the same-tab behaviour.
 *
 * It is decided in the browser, when the link is clicked: the server cannot know the pointer of the device. Setting `target` in a
 * capture-phase listener happens before Blazor's own link handling, which leaves links with a target alone.
 *
 * To let a reader choose, `opensInNewTab()` is the one place to ask: a stored preference would come before the media query.
 *
 * @see {@link /workspaces/techhub/src/TechHub.Web/Components/ContentItemCard.razor} for the markup
 */

'use strict';

(function () {
    // A mouse or trackpad: a desktop or laptop. A touch screen (phone, tablet) matches neither condition.
    const FINE_POINTER_WITH_HOVER = '(hover: hover) and (pointer: fine)';
    const NEW_TAB_NOTE = ' - opens in new tab';

    /**
     * Whether content links open in a new tab on this device.
     * @returns {boolean}
     */
    function opensInNewTab() {
        return typeof window.matchMedia === 'function' && window.matchMedia(FINE_POINTER_WITH_HOVER).matches;
    }

    /**
     * The content link an event happened on, if any.
     * @param {Event} event
     * @returns {HTMLAnchorElement|null}
     */
    function contentLinkOf(event) {
        return event.target instanceof Element ? event.target.closest('a[data-content-link]') : null;
    }

    function onClick(event) {
        // Leave alone what the browser already handles: a click that was cancelled, other mouse buttons and Ctrl/Cmd/Shift/Alt clicks.
        if (event.defaultPrevented || event.button !== 0 || event.ctrlKey || event.metaKey || event.shiftKey || event.altKey) {
            return;
        }

        const link = contentLinkOf(event);
        if (link && opensInNewTab()) {
            link.target = '_blank';
            link.rel = 'noopener noreferrer';
        }
    }

    /**
     * Tell assistive technology that the link opens in a new tab, as soon as the reader points at or focuses it.
     * @param {Event} event
     */
    function onApproach(event) {
        const link = contentLinkOf(event);
        if (!link || !opensInNewTab()) {
            return;
        }

        const label = link.getAttribute('aria-label') ?? link.textContent.trim();
        if (!label.endsWith(NEW_TAB_NOTE)) {
            link.setAttribute('aria-label', label + NEW_TAB_NOTE);
        }
    }

    document.addEventListener('click', onClick, true);
    document.addEventListener('pointerover', onApproach, true);
    document.addEventListener('focusin', onApproach, true);

    window.TechHub = window.TechHub || {};
    window.TechHub.contentLinks = { opensInNewTab };
})();
