/**
 * Text Filter - hides the items that do not contain the text a reader types in a filter field.
 *
 * Markup (see GitHubCopilotFeatures.razor):
 *   <input type="search" data-text-filter data-filter-items=".feature-card" data-filter-empty="#id-of-the-empty-message">
 *
 * - `data-filter-items`: selector of the items to filter, searched in the closest `.custom-page-container` (else the document).
 * - `data-filter-empty`: optional selector of a message that is shown while the text matches no item.
 *
 * Every word the reader types has to occur in the text of an item (case-insensitive), in any order. Items are hidden with the
 * `hidden` attribute, so it works next to Blazor: Blazor leaves the attribute alone when it re-renders an item, and a
 * MutationObserver filters the items that Blazor adds afterwards (for example when another filter button changes the list).
 *
 * @see {@link /workspaces/techhub/src/TechHub.Web/Components/Pages/GitHubCopilotFeatures.razor} for the markup
 */

'use strict';

(function () {
    const SCOPE_SELECTOR = '.custom-page-container';
    const observed = new WeakSet();

    /**
     * The words of the text a reader typed.
     * @param {string} value
     * @returns {string[]}
     */
    function wordsOf(value) {
        return value.toLowerCase().split(/\s+/).filter(Boolean);
    }

    /**
     * Whether the text of an item contains every word.
     * @param {Element} item
     * @param {string[]} words
     * @returns {boolean}
     */
    function matches(item, words) {
        const text = (item.textContent ?? '').toLowerCase();
        return words.every(word => text.includes(word));
    }

    /**
     * @param {HTMLInputElement} input
     * @returns {Element|Document}
     */
    function scopeOf(input) {
        return input.closest(SCOPE_SELECTOR) ?? document;
    }

    /**
     * Shows the items that match the text of the input and hides the others.
     * @param {HTMLInputElement} input
     */
    function apply(input) {
        const selector = input.dataset.filterItems;
        if (!selector) {
            return;
        }

        const words = wordsOf(input.value);
        const items = [...scopeOf(input).querySelectorAll(selector)];
        let shown = 0;

        for (const item of items) {
            const isMatch = matches(item, words);
            if (item.hidden === isMatch) {
                item.hidden = !isMatch;
            }
            if (isMatch) {
                shown++;
            }
        }

        const emptySelector = input.dataset.filterEmpty;
        const empty = emptySelector ? scopeOf(input).querySelector(emptySelector) : null;
        if (empty) {
            empty.hidden = !(words.length > 0 && items.length > 0 && shown === 0);
        }
    }

    /**
     * Filter the items Blazor adds or replaces later on, as long as the input is on the page.
     * @param {HTMLInputElement} input
     */
    function keepApplying(input) {
        if (observed.has(input)) {
            return;
        }
        observed.add(input);

        new MutationObserver(() => {
            if (input.isConnected) {
                apply(input);
            }
        }).observe(scopeOf(input), { childList: true, subtree: true });
    }

    document.addEventListener('input', event => {
        const input = event.target;
        if (input instanceof HTMLInputElement && 'textFilter' in input.dataset) {
            keepApplying(input);
            apply(input);
        }
    });

    window.TechHub = window.TechHub || {};
    window.TechHub.textFilter = { apply };
})();
