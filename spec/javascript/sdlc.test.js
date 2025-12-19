const { openCard, toggleAll, toggleCardHeader } = require('../../assets/js/sdlc');

describe('SDLC interactivity', () => {
  beforeEach(() => {
    document.body.innerHTML = '';
  });

  test('toggleCardHeader toggles expanded state and icon', () => {
    document.body.innerHTML = `
      <div class="sdlc-step-card" id="sdlc-card-planning">
        <div class="sdlc-card-header">
          <span>Planning</span>
          <span class="sdlc-card-icon">▼</span>
        </div>
        <div class="sdlc-card-content"></div>
      </div>
    `;

    const header = document.querySelector('.sdlc-card-header');
    const content = document.querySelector('.sdlc-card-content');
    const icon = document.querySelector('.sdlc-card-icon');

    expect(content.classList.contains('expanded')).toBe(false);
    expect(icon.classList.contains('expanded')).toBe(false);

    const expanded = toggleCardHeader(header);

    expect(expanded).toBe(true);
    expect(content.classList.contains('expanded')).toBe(true);
    expect(icon.classList.contains('expanded')).toBe(true);

    const expandedAgain = toggleCardHeader(header);

    expect(expandedAgain).toBe(false);
    expect(content.classList.contains('expanded')).toBe(false);
    expect(icon.classList.contains('expanded')).toBe(false);
  });

  test('toggleAll expands when none expanded, collapses when any expanded', () => {
    document.body.innerHTML = `
      <div class="sdlc-step-card" id="sdlc-card-one">
        <div class="sdlc-card-header">
          <span>One</span>
          <span class="sdlc-card-icon">▼</span>
        </div>
        <div class="sdlc-card-content"></div>
      </div>
      <div class="sdlc-step-card" id="sdlc-card-two">
        <div class="sdlc-card-header">
          <span>Two</span>
          <span class="sdlc-card-icon">▼</span>
        </div>
        <div class="sdlc-card-content"></div>
      </div>
    `;

    const contents = Array.from(document.querySelectorAll('.sdlc-card-content'));

    expect(contents.every((el) => !el.classList.contains('expanded'))).toBe(true);

    const expanded = toggleAll(document);

    expect(expanded).toBe(true);
    expect(contents.every((el) => el.classList.contains('expanded'))).toBe(true);

    const collapsed = toggleAll(document);

    expect(collapsed).toBe(false);
    expect(contents.every((el) => !el.classList.contains('expanded'))).toBe(true);
  });

  test('openCard expands and scrolls the target card', () => {
    document.body.innerHTML = `
      <div class="sdlc-step-card" id="sdlc-card-planning">
        <div class="sdlc-card-header">
          <span>Planning</span>
          <span class="sdlc-card-icon">▼</span>
        </div>
        <div class="sdlc-card-content"></div>
      </div>
    `;

    const card = document.getElementById('sdlc-card-planning');
    card.scrollIntoView = jest.fn();

    const opened = openCard('planning', document);

    expect(opened).toBe(true);
    expect(document.querySelector('.sdlc-card-content').classList.contains('expanded')).toBe(true);
    expect(document.querySelector('.sdlc-card-icon').classList.contains('expanded')).toBe(true);
    expect(card.scrollIntoView).toHaveBeenCalledTimes(1);
  });
});
