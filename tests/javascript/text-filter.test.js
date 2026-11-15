import { describe, it, expect, beforeEach, vi } from 'vitest';

const MODULE_PATH = '../../src/TechHub.Web/wwwroot/js/text-filter.js';

function renderPage(cards) {
    document.body.innerHTML = `
        <div class="custom-page-container">
            <input type="search" data-text-filter data-filter-items=".feature-card" data-filter-empty="#empty">
            <p id="empty" hidden>Nothing</p>
            <div id="grid">${cards.map(text => `<article class="feature-card">${text}</article>`).join('')}</div>
        </div>`;
    return document.querySelector('input');
}

function type(input, value) {
    input.value = value;
    input.dispatchEvent(new Event('input', { bubbles: true }));
}

const hiddenCards = () => [...document.querySelectorAll('.feature-card')].filter(c => c.hidden).map(c => c.textContent);

describe('text-filter.js', () => {
    beforeEach(async () => {
        document.body.innerHTML = '';
        delete window.TechHub;
        vi.resetModules();
        await import(MODULE_PATH);
    });

    it('hides the items that do not contain the text, ignoring case', () => {
        const input = renderPage(['Agent mode', 'Code review', 'Memory']);

        type(input, 'CODE');

        expect(hiddenCards()).toEqual(['Agent mode', 'Memory']);
    });

    it('requires every word to occur, in any order', () => {
        const input = renderPage(['Agent mode in VS Code', 'Agent mode in the CLI']);

        type(input, 'code agent');

        expect(hiddenCards()).toEqual(['Agent mode in the CLI']);
    });

    it('shows every item again when the text is cleared', () => {
        const input = renderPage(['One', 'Two']);
        type(input, 'one');

        type(input, '  ');

        expect(hiddenCards()).toEqual([]);
    });

    it('shows the empty message only when the text matches nothing', () => {
        const input = renderPage(['One', 'Two']);
        const empty = document.getElementById('empty');

        type(input, 'zzz');
        expect(empty.hidden).toBe(false);

        type(input, 'one');
        expect(empty.hidden).toBe(true);

        type(input, '');
        expect(empty.hidden).toBe(true);
    });

    it('does not show the empty message when there are no items to filter', () => {
        const input = renderPage([]);

        type(input, 'zzz');

        expect(document.getElementById('empty').hidden).toBe(true);
    });

    it('filters items that are added later, like Blazor does after another filter button', async () => {
        const input = renderPage(['One']);
        type(input, 'two');

        const added = document.createElement('article');
        added.className = 'feature-card';
        added.textContent = 'Three';
        document.getElementById('grid').appendChild(added);
        await new Promise(resolve => setTimeout(resolve, 0));

        expect(hiddenCards()).toEqual(['One', 'Three']);
    });

    it('ignores input events of other fields', () => {
        renderPage(['One']);
        const other = document.createElement('input');
        document.body.appendChild(other);

        type(other, 'zzz');

        expect(hiddenCards()).toEqual([]);
    });
});
