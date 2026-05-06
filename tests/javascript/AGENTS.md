# JavaScript Test Suite

> **RULE**: Follow [Root AGENTS.md](../../AGENTS.md) for workflow and [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) for frontend context.

Vitest + jsdom unit tests for client-side JavaScript in `src/TechHub.Web/wwwroot/js/`.

## Framework

- **Vitest** v4 — Fast ES module test runner with native `import()` support
- **jsdom** — DOM simulation environment (configured in `vitest.config.js`)
- Tests run in Node.js with jsdom providing `window`, `document`, etc.

## Running Tests

```powershell
# Via Run command (integrated into standard workflow)
Run -TestProject javascript

# Direct npm
npm test           # Single run
npm run test:watch # Watch mode
```

## Structure

- All test files: `tests/javascript/*.test.js`
- One test file per source file (e.g., `scroll-manager.test.js` tests `scroll-manager.js`)
- Config: `/vitest.config.js` (root)
- Dependencies: `/package.json` devDependencies

## Key Patterns

### Module-level state isolation

ES modules with `let` variables retain state between imports. Use `vi.resetModules()` + dynamic `import()` to get fresh state per test:

```javascript
beforeEach(async () => {
    vi.resetModules();
    mod = await import(MODULE_PATH);
});
```

### Self-executing modules

Scripts like `scroll-manager.js` execute on import (set up listeners, create DOM). Simply `await import(MODULE_PATH)` — they attach to `window` automatically.

### DOM setup

Create needed DOM elements in `beforeEach`, clean with `document.body.innerHTML = ''`.

### jsdom limitations

- `document.cookie` getter only returns `name=value` pairs (no attributes like `SameSite`, `max-age`)
- `getBoundingClientRect()` returns zeros unless mocked
- No real layout engine — `scrollHeight`, `offsetHeight` must be mocked
- `requestAnimationFrame` is available but may need synchronous override for tests

## What to Test

| Source File | Test File | Key Behaviors |
|-------------|-----------|---------------|
| `scroll-manager.js` | `scroll-manager.test.js` | Buttons, keyboard nav, scroll save/restore, TOC highlight, infinite scroll, navigation gating |
| `sidebar-toggle.js` | `sidebar-toggle.test.js` | Toggle class, cookie persistence |
| `hero-banner.js` | `hero-banner.test.js` | Cookie persistence for collapsed/hash |
| `mobile-nav.js` | `mobile-nav.test.js` | Scroll lock/unlock, escape handler |
| `date-range-slider.js` | `date-range-slider.test.js` | Clamping, fill position |
| `custom-pages.js` | `custom-pages.test.js` | Collapsible cards, filters, expandable badges |
| `page-scripts.js` | `page-scripts.test.js` | Global exposure, script loading flags |

## Key Rules

- **Test public API** — exported functions and window-exposed globals
- **Mock external dependencies** — `DotNetObjectReference`, CDN scripts, Blazor globals
- **Never test internal implementation** — test behavior, not how it's achieved
- **Clean up event listeners** — call `dispose()`/`destroy()` in `afterEach`
- **Use `vi.fn()` for Blazor interop** — mock `invokeMethodAsync` and similar
- **Floating-point assertions** — use `toBeCloseTo()` for percentage calculations

## CI/CD Integration

JavaScript tests run as a separate job (`test-javascript`) in both CI and CD pipelines, gated by the quality gate. They also run locally via `Run` (Phase 1.5, after PowerShell tests).
