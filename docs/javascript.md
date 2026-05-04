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
| **Static (plain)** | Every page, no module features needed | `mobile-nav.js`, `sidebar-toggle.js` | `<script src="@Assets[...]" defer>` |
| **Dynamic ES Module** | Only some pages need it | `custom-pages.js` | `import('./js/file.js')` via ImportMap |
| **External CDN** | Third-party library | Highlight.js, Mermaid | Dynamic `loadScript()` with SRI |

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
| `scroll-manager.js` | Navigation buttons, keyboard nav detection, scroll position save/restore, TOC scroll spy, infinite scroll | Static (`type="module"`) | ES Module |
| `sidebar-toggle.js` | Desktop sidebar collapse/expand with cookie persistence | Static (`defer`) | Script |
| `mobile-nav.js` | Mobile menu scroll lock and Escape key handler | Static (`defer`) | Script |
| `hero-banner.js` | Hero banner collapse/expand with cookie persistence | Static (`defer`) | IIFE |
| `custom-pages.js` | Collapsible sections for SDLC/DX pages, feature filters | Dynamic (pages with `[data-collapsible]`) | ES Module |
| `date-range-slider.js` | Client-side slider clamping (prevents handles crossing) | Dynamic (via Blazor JS interop) | ES Module |
| `page-scripts.js` | Orchestrator for CDN loading (Highlight.js, Mermaid), page init | Static (`type="module"`) | ES Module |

Special file in `wwwroot/`:

| File | Purpose | Loading |
|------|---------|---------|
| `TechHub.Web.lib.module.js` | Blazor lifecycle callbacks | Auto-discovered by Blazor |

## Conditional Loading

JavaScript libraries are loaded dynamically only when their target elements exist on the page.

### Element Detection

On every page load and `enhancedload` event, the system checks for:

- `pre code` → Load Highlight.js (syntax highlighting)
- `.mermaid` → Load Mermaid (diagrams)
- `[data-toc-scroll-spy]` → Initialize TOC scroll spy
- `[data-collapsible]` → Load custom pages interactivity

### Performance Benefits

- **Simple Pages** (Home, Section, Collection): ~0 extra JS (just element checks, <1ms)
- **Content Pages** (Handbook, Features, ContentItem): Libraries load only when needed
- **No Manual Parameters**: Pages don't need to declare what they use - automatic detection

## External CDN Libraries

External libraries are loaded from CDNs for performance. All versions and SRI hashes are centralized in `Configuration/CdnLibraries.cs`.

### Current Libraries

- **Highlight.js**: Syntax highlighting for code blocks
- **Mermaid**: Diagram rendering (flowcharts, sequence diagrams, etc.)

### Updating CDN Library Versions

1. Update the version in `CdnLibraries.cs` (e.g., `HighlightJs.Version`)
2. Generate new SRI hash from <https://www.srihash.org/>
3. Update the integrity hash in `CdnLibraries.cs`
4. Test locally to verify the library loads correctly

## Scroll Manager

`wwwroot/js/scroll-manager.js` is the unified module that handles all scroll-related behavior:

### Navigation Buttons

- **Back to Top**: Smooth scroll to top of page (appears after scrolling 300px)
- **Back to Previous**: Navigate to previous page in browser history
- Automatic show/hide based on scroll position (300px threshold)
- CSS fade-in/fade-out transitions

### Scroll Position Save/Restore

- **Save**: On every `scroll` event, saves `window.scrollY` keyed by `pathname + search` (no hash)
- **Restore**: On back/forward navigation (traverse), restores the saved position after `markScriptsReady` signals that all rendering is complete
- **Retry**: If the page isn't tall enough yet, a MutationObserver watches for DOM changes with a 50ms debounce and 1s hard deadline
- **Forward navigation**: Scrolls to top via `resetPagePosition()`
- **Same-page hash popstate**: Scrolls to the target element directly (no saved position lookup)

The scroll key intentionally excludes the hash. TOC `replaceState()` changes the hash freely during scrolling — if the hash were included in the key, restored positions would mismatch.

Detection uses the Navigation API (`currentEntry.navigationType === 'traverse'`) with a `popstate` fallback for browsers without it. Browser-native scroll restoration is disabled (`history.scrollRestoration = 'manual'`) in `TechHub.Web.lib.module.js`.

The restore is triggered by `markScriptsReady()` (called from every page's `OnAfterRenderAsync`) which invokes `window.__restoreScrollPosition()`. Every page **must** call `markScriptsReady` — this is enforced by a convention test.

### Navigation Gating

A single module-level `navigating` boolean gates all scroll work:

- **`scroll` handler**: Skips position save, button update, and infinite scroll check when `navigating = true`
- **`scrollend` handler**: Skips TOC highlight when `navigating = true`
- **`finishNavigation()`**: Sets `navigating = false` and calls `onScrollEnd()` once for the final position (explicit call, no synthetic events)

The `pushState` interceptor sets `navigating = true` for cross-page navigation. The `enhancedload` safety net clears it for forward navigation. For back/forward, `restoreScrollPosition()` → `finishNavigation()` clears it.

### Event Handling Split

| Event | Handlers | Why |
|-------|----------|-----|
| `scroll` (high frequency) | Save position, button visibility, infinite scroll check | Cheap comparisons, needs responsiveness |
| `scrollend` (or debounce fallback) | TOC highlight | Expensive `getBoundingClientRect` on all headings, avoids flicker |

### TOC Scroll-Spy

Highlights the active heading in the table of contents sidebar.

**Critical**: Uses `history.replaceState()` instead of `pushState()` to update URL hash. This prevents polluting browser history with scroll positions — only actual TOC link clicks create history entries.

```javascript
// ❌ WRONG - Creates history entry for every scroll update
history.pushState(null, '', newUrl);

// ✅ CORRECT - Updates URL without creating history entry
history.replaceState(null, '', newUrl);
```

Activated by calling `initTocScrollSpy()` (exported from scroll-manager.js, invoked by page-scripts.js when `[data-toc-scroll-spy]` elements exist).

### Infinite Scroll

Blazor's `ContentItemsGrid` component imports scroll-manager.js via `JSRuntime.InvokeAsync("import", "/js/scroll-manager.js")` and calls:

- `observeScrollTrigger(dotNetRef, "scroll-trigger")` — registers the trigger element
- `dispose()` — cleans up on component disposal

The infinite scroll check runs in the `scroll` event handler — it fires while the user is still scrolling so content arrives before they reach the bottom.

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
