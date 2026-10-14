# JavaScript Architecture

Tech Hub uses a minimal JavaScript approach where Blazor handles most interactivity server-side. JavaScript is only used for browser-native features that Blazor cannot access.

## When JavaScript Is Required

JavaScript is ONLY for:

1. **Browser-native features** Blazor can't access (scroll position, scroll events, history API)
2. **Enhanced navigation hooks** (Blazor `enhancedload` event for SPA-style page transitions)
3. **Third-party libraries** (Highlight.js, Mermaid) that require JavaScript execution

**Never create**: Client-side filtering JavaScript. Tag filtering is 100% Blazor server-side.

## Loading Strategies

| Loading Type | Use When | Example | How |
|--------------|----------|---------|-----|
| **Static (module)** | Every page, needs ES module scope | `scroll-manager.js`, `page-scripts.js` | `<script type="module" src="@Assets[...]">` |
| **Static (plain)** | Every page, no module features needed | `mobile-nav.js`, `sidebar-toggle.js`, `hero-banner.js` | `<script src="@Assets[...]" defer>` |
| **Dynamic ES Module** | Only some pages need it | `custom-pages.js`, `date-range-slider.js` | `import('./js/file.js')` via ImportMap |
| **Component module** | Script belongs to a single component | `ReconnectModal.razor.js` | `<script type="module" src="@Assets[...]">` inside the component |
| **External CDN** | Third-party library | Highlight.js, Mermaid | Dynamic `loadScript()` with fallback URLs and retries |

## Fingerprinting (Cache Busting)

All local JavaScript files MUST use fingerprinted URLs for proper cache invalidation.

### Static Scripts

Use `@Assets["js/file.js"]`:

```html
<!-- ✅ CORRECT - Fingerprinted URL, ES module (scroll-manager.js) -->
<script type="module" src="@Assets["js/scroll-manager.js"]"></script>

<!-- ✅ CORRECT - Fingerprinted URL, deferred plain script -->
<script src="@Assets["js/mobile-nav.js"]" defer></script>

<!-- ❌ WRONG - Raw path (will NOT cache bust on updates) -->
<script src="js/scroll-manager.js" defer></script>
```

### Dynamic Imports

Use `import()` with ImportMap component:

```javascript
// ✅ CORRECT - ImportMap rewrites to fingerprinted path
await import('./js/custom-pages.js');

// ❌ WRONG - Bypasses fingerprinting
await loadScript('/js/file.js');
```

### How ImportMap Works

1. `<ImportMap />` in App.razor generates a `<script type="importmap">` block
2. Maps module specifiers to fingerprinted URLs (e.g., `./js/custom-pages.js` → `./js/custom-pages.abc123.js`)
3. Browser's `import()` uses the map to resolve fingerprinted paths
4. Result: Dynamic imports get proper cache busting

## Local JavaScript Files

Files in `wwwroot/js/`:

| File | Purpose | Loading | Format |
|------|---------|---------|--------|
| `scroll-manager.js` | Navigation buttons (back-to-top / back-to-previous), navigation spinner, keyboard nav detection, scroll position save/restore, TOC scroll spy | Static (`type="module"`) | ES Module |
| `sidebar-toggle.js` | Desktop sidebar collapse/expand with cookie persistence | Static (`defer`) | Script |
| `mobile-nav.js` | Mobile menu scroll lock and Escape key handler | Static (`defer`) | Script |
| `hero-banner.js` | Hero banner collapse/expand with cookie persistence | Static (`defer`) | IIFE |
| `custom-pages.js` | Collapsible sections for SDLC/DX pages, feature filters, expandable badges | Dynamic (loaded by `initCustomPages`) | ES Module |
| `date-range-slider.js` | Client-side slider clamping (prevents handles crossing) | Dynamic (via Blazor JS interop) | ES Module |
| `page-scripts.js` | Orchestrator for CDN loading (Highlight.js, Mermaid), page init functions, script lifecycle | Static (`type="module"`) | ES Module |

`Configuration/JsFiles.cs` lists `page-scripts.js` under its "dynamic" group for bookkeeping, but `App.razor` loads it with a static module tag.

Other script files:

| File | Purpose | Loading |
|------|---------|---------|
| `wwwroot/TechHub.Web.lib.module.js` | Blazor startup and lifecycle hooks (see [Blazor Lifecycle Hooks](#blazor-lifecycle-hooks)) | Auto-discovered by Blazor |
| `Components/Layout/ReconnectModal.razor.js` | Reconnect dialog behavior: shows and hides the dialog on circuit state changes, handles retry, resume and reload buttons | Module script rendered by `ReconnectModal.razor` |

## Page Script Lifecycle

Page components inherit `PageComponentBase`, which runs the script lifecycle after each render:

1. `markScriptsLoading()` sets `window.__scriptsReady = false`, which blocks E2E waits and defers scroll restoration
2. Each init function the page declares in `PageScripts` runs in order
3. `markScriptsReady()` sets `window.__scriptsReady = true` and restores the scroll position on back/forward navigation

Pages declare which init functions they need, so a page without code blocks, diagrams or collapsible cards loads none of the related libraries. Pages without scripts only call `markScriptsReady()`. A convention test enforces that every page inherits the base class. Pages that cannot load data (404) still signal ready so waiting code does not hang.

The init functions are idempotent: they only process DOM elements that have not been processed yet, so re-running them after in-component navigation is safe.

See [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) for the base class properties and the per-page init function table.

## Blazor Lifecycle Hooks

`TechHub.Web.lib.module.js` implements the Blazor startup callbacks:

| Hook | What it does |
|------|--------------|
| `beforeWebStarted` | Raises the SignalR server timeout (120s) and keep-alive interval (15s) so background tabs do not show a spurious "connection lost" overlay |
| `afterWebStarted` | Sets `__blazorWebReady`, switches `history.scrollRestoration` to `manual` (scroll restoration is handled by `scroll-manager.js`), resets `__scriptsReady` on every `enhancedload`, and sets up focus scroll compensation for sticky headers |
| `afterServerStarted` | Sets `__blazorServerReady` and resets `__scriptsReady` so waiting code blocks until the first page render cycle completes |
| `afterWebAssemblyStarted` | Sets `__blazorWasmReady` (unused today, kept for completeness) |

It also defines `window.__isBlazorInteractiveReady()`, which returns true when either the Server or WebAssembly runtime is ready.

## Window Globals

Scripts expose a small set of globals for Blazor JS interop and tests.

| Global | Defined in | Purpose |
|--------|-----------|---------|
| `initHighlighting`, `initMermaid`, `initTocScrollSpy`, `initCustomPages` | `page-scripts.js` | Page init functions invoked by `PageComponentBase` |
| `markScriptsLoading`, `markScriptsReady` | `page-scripts.js` (`markScriptsReady` is wrapped by `scroll-manager.js`) | Script lifecycle signals |
| `scrollTierCardIntoView`, `preventScrollOnExpand` | `page-scripts.js` | Used by the GitHub Copilot Features page to keep scroll stable when filtering and expanding cards |
| `TechHub.sidebar` (`isCollapsed`, `toggle`) | `sidebar-toggle.js` | Desktop sidebar collapse, called by `SidebarToggle` |
| `TechHub.heroBanner` (`setCollapsed`, `setHash`) | `hero-banner.js` | Cookie persistence for `HeroBanner` |
| `TechHub.scrollToTopAndClearHash` | `scroll-manager.js` | Scroll to top and remove the URL hash |
| `mobileNav` (`lockScroll`, `unlockScroll`, `registerEscapeHandler`) | `mobile-nav.js` | Mobile menu scroll lock, called by `NavHeader` |
| `__restoreScrollPosition`, `__savedScrollPositions` | `scroll-manager.js` | Scroll restoration entry point and saved positions (exposed for tests) |
| `__scriptsReady`, `__mermaidReady`, `__dateRangeSliderReady` | `page-scripts.js`, `date-range-slider.js` | Readiness flags checked by `markScriptsReady` |
| `__blazorWebReady`, `__blazorServerReady`, `__isBlazorInteractiveReady()` | `TechHub.Web.lib.module.js` | Blazor readiness for tests |
| `__e2e`, `__e2eSignal(label)` | Inline script in `App.razor` | E2E lifecycle counter (see [E2E lifecycle counter](#e2e-lifecycle-counter)) |
| `TechHubCDN` | Inline script in `App.razor` | CDN versions, URLs and SRI hashes bridged from `Configuration/CdnLibraries.cs` |

## E2E Lifecycle Counter

An inline script in `App.razor` defines `window.__e2e`, a single monotonic counter that lifecycle hooks increment through `__e2eSignal(label)`, keeping the last 20 signals in a history. E2E tests capture the counter before an action and wait for it to change, which replaces many independent flags with one wait pattern. It is always enabled because the overhead is trivial, so tests behave the same in every environment. Labels include `blazor-web-ready`, `blazor-server-ready`, `enhanced-nav`, `scripts-loading`, `scripts-ready`, `mermaid-rendered`, `toc-initialized` and `scroll-end`.

## Conditional Loading

JavaScript libraries are loaded dynamically only when their target elements exist on the page.

### Init Functions

Each page declares which init functions it needs (see [Page Script Lifecycle](#page-script-lifecycle)). Each function also checks the DOM before doing any work:

- `initHighlighting` → loads Highlight.js when `pre code` exists
- `initMermaid` → loads Mermaid when unprocessed `.mermaid` elements exist
- `initTocScrollSpy` → activates TOC scroll spy when a `[data-toc-scroll-spy]` element exists
- `initCustomPages` → loads `custom-pages.js` when `[data-collapsible]`, un-initialized `[data-expand-target]` or feature filter sections exist

### Performance Benefits

- **Simple Pages** (Home, Section, Collection): No CDN libraries and at most one small dynamic import
- **Content Pages** (Handbook, Features, ContentItem): Libraries load only on pages that declare them, and only once per session
- **Explicit per page**: The page component decides which init functions run; the DOM check is a second guard

### Mermaid and Highlight.js Details

- **Mermaid expand**: Every rendered diagram gets an expand button that opens it in a fullscreen modal (closed with the close button, backdrop click or Escape)
- **Mermaid readiness**: `__mermaidReady` is false while diagrams render so scroll restoration waits for the final layout, and the page re-scrolls to the URL hash afterwards because rendering shifts the layout
- **Terraform highlighting**: Highlight.js core does not ship Terraform, so a third-party language plugin is loaded from the CDN and registered as `terraform`, `tf` and `hcl`

## External CDN Libraries

External libraries are loaded from CDNs for performance. All versions and SRI hashes are centralized in `Configuration/CdnLibraries.cs`.

### Current Libraries

- **Highlight.js**: Syntax highlighting for code blocks
- **Mermaid**: Diagram rendering (flowcharts, sequence diagrams, etc.)

SRI hashes for Highlight.js are defined in `CdnLibraries.cs` and bridged to `window.TechHubCDN`, but `page-scripts.js` does not currently pass them to `loadScript()` (which supports an `integrity` option). Mermaid is loaded from a list of fallback CDN URLs without SRI.

### Updating CDN Library Versions

1. Update the version in `CdnLibraries.cs` (e.g., `HighlightJs.Version`)
2. Generate new SRI hash from <https://www.srihash.org/>
3. Update the integrity hash in `CdnLibraries.cs`
4. Test locally to verify the library loads correctly

## Scroll Manager

`wwwroot/js/scroll-manager.js` is the unified module handling scroll position
save/restore, navigation lifecycle, TOC scroll-spy, and
back-to-top/back-to-prev buttons.

For the full architecture (sequence diagrams, known bugs, `navigating` flag,
`allComponentsReady` gate, test coverage), see
[scroll-system-architecture.md](scroll-system-architecture.md).

### Quick API Reference

| Export | Called by | Purpose |
|--------|-----------|---------|
| `initTocScrollSpy()` | page-scripts.js | Activate TOC highlighting |
| `isNavigating()` | Tests | Check navigation state |

Besides its exports, the module installs side effects on load: the `pushState` and `popstate` interceptors, Blazor navigation listeners, the keyboard-navigation detector (adds `keyboard-nav` on Tab, removes it on pointer input), the back-to-top and back-to-previous buttons (`#nav-helper-buttons`) and the navigation spinner. It also exposes the globals listed under [Window Globals](#window-globals).

### custom-pages.js Exports

| Export | Purpose |
|--------|---------|
| `initCollapsibleCards()` | Click handlers for SDLC phase blocks, SDLC cards and DX Space cards (also calls `initFeatureFilters`) |
| `initFeatureFilters()` | Toggle filters (GHES, videos) on the GitHub Copilot Features page |
| `initExpandableBadges()` | "+X more" buttons on section cards that reveal hidden custom page badges |

Every handler is guarded by a `data-initialized` attribute so it is attached only once. Elements whose click is handled by Blazor `@onclick` are marked `data-initialized="blazor"` so the JS skips them.

### Key Rules

- **Always `replaceState`** for TOC hash updates (never `pushState` — see arch doc)
- **Scroll key excludes hash** — TOC `replaceState` changes hash freely during scroll
- **`beforeenhancedload`** scrolls to top before DOM patch (eliminates forward-nav jerk)
- **Blazor imports** via `import("./js/scroll-manager.js")` (ImportMap resolves fingerprint)

## Adding New JavaScript Files

1. **Add file** to `wwwroot/js/`
2. **Update** `Configuration/JsFiles.cs` for documentation
3. **Load correctly**:
   - Static module (needs ES module scope): Add `<script type="module" src="@Assets["js/file.js"]">` to App.razor
   - Static plain: Add `<script src="@Assets["js/file.js"]" defer>` to App.razor
   - Dynamic (conditional): Add `await import('./js/file.js')` in the module script block
4. **Document** purpose in this file under "Local JavaScript Files"

## Implementation Reference

- JavaScript configuration: [src/TechHub.Web/Configuration/JsFiles.cs](../src/TechHub.Web/Configuration/JsFiles.cs)
- CDN library versions: [src/TechHub.Web/Configuration/CdnLibraries.cs](../src/TechHub.Web/Configuration/CdnLibraries.cs)
- Scroll manager: [src/TechHub.Web/wwwroot/js/scroll-manager.js](../src/TechHub.Web/wwwroot/js/scroll-manager.js)

## Testing

All client-side JavaScript is unit-tested with **Vitest** + **jsdom** in `tests/javascript/`.

### Running JavaScript Tests

```powershell
# Via the standard Run command
Run -TestProject javascript

# Direct npm commands
npm test           # Single run (CI mode)
npm run test:watch # Watch mode (development)
```

### Test Coverage

Every file in `wwwroot/js/` has a corresponding `*.test.js` file. Tests verify:

- Exported function behavior and return values
- DOM manipulation (class toggling, element creation)
- Event listener registration and cleanup
- Cookie persistence (value only — jsdom limitation)
- Module lifecycle (init/dispose patterns)
- Page navigation guards (URL change detection)

### CI/CD Integration

JavaScript tests run as a dedicated `test-javascript` job in both CI and CD pipelines. They are part of the quality gate — failures block PR merge and deployment.

See [tests/javascript/AGENTS.md](../tests/javascript/AGENTS.md) for patterns and conventions.
