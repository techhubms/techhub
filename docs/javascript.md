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
| **Static (module)** | Every page, needs ES module scope | `nav-helpers.js`, `page-scripts.js` | `<script type="module" src="@Assets[...]">` |
| **Static (plain)** | Every page, no module features needed | `mobile-nav.js`, `sidebar-toggle.js` | `<script src="@Assets[...]" defer>` |
| **Dynamic ES Module** | Only some pages need it | `toc-scroll-spy.js`, `custom-pages.js` | `import('./js/file.js')` via ImportMap |
| **External CDN** | Third-party library | Highlight.js, Mermaid | Dynamic `loadScript()` with SRI |

## Fingerprinting (Cache Busting)

All local JavaScript files MUST use fingerprinted URLs for proper cache invalidation.

### Static Scripts

Use `@Assets["js/file.js"]`:

```html
<!-- ✅ CORRECT - Fingerprinted URL, ES module (nav-helpers.js) -->
<script type="module" src="@Assets["js/nav-helpers.js"]"></script>

<!-- ✅ CORRECT - Fingerprinted URL, deferred plain script -->
<script src="@Assets["js/mobile-nav.js"]" defer></script>

<!-- ❌ WRONG - Raw path (will NOT cache bust on updates) -->
<script src="js/nav-helpers.js" defer></script>
```

### Dynamic Imports

Use `import()` with ImportMap component:

```javascript
// ✅ CORRECT - ImportMap rewrites to fingerprinted path
await import('./js/toc-scroll-spy.js');

// ❌ WRONG - Bypasses fingerprinting
await loadScript('/js/file.js');
```

### How ImportMap Works

1. `<ImportMap />` in App.razor generates a `<script type="importmap">` block
2. Maps module specifiers to fingerprinted URLs (e.g., `./js/toc-scroll-spy.js` → `./js/toc-scroll-spy.abc123.js`)
3. Browser's `import()` uses the map to resolve fingerprinted paths
4. Result: Dynamic imports get proper cache busting

## Local JavaScript Files

Files in `wwwroot/js/`:

| File | Purpose | Loading | Format |
|------|---------|---------|--------|
| `nav-helpers.js` | Back to top, back to previous buttons, keyboard nav detection, scroll position management, `window.__scrollRestoring` ownership | Static (`type="module"`) | ES Module |
| `sidebar-toggle.js` | Desktop sidebar collapse/expand with cookie persistence | Static (`defer`) | Script |
| `mobile-nav.js` | Mobile menu scroll lock and Escape key handler | Static (`defer`) | Script |
| `hero-banner.js` | Hero banner collapse/expand with cookie persistence | Static (`defer`) | IIFE |
| `infinite-scroll.js` | Scroll-based infinite loading trigger | Dynamic (via Blazor JS interop) | ES Module |
| `toc-scroll-spy.js` | TOC scroll highlighting, history management | Dynamic (pages with TOC) | ES Module |
| `custom-pages.js` | Collapsible sections for SDLC/DX pages, feature filters | Dynamic (pages with `[data-collapsible]`) | ES Module |
| `date-range-slider.js` | Client-side slider clamping (prevents handles crossing) | Dynamic (via Blazor JS interop) | ES Module |
| `page-scripts.js` | Orchestrator for CDN loading (Highlight.js, Mermaid), page init, and scroll restore trigger | Static (`type="module"`) | ES Module |

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

## Navigation Helpers

`wwwroot/js/nav-helpers.js` provides sticky bottom navigation buttons:

- **Back to Top**: Smooth scroll to top of page (appears after scrolling 300px)
- **Back to Previous**: Navigate to previous page in browser history

Features:

- Automatic show/hide based on scroll position (300px threshold)
- Blazor enhanced navigation support (pageshow event + MutationObserver)
- Proper cleanup and re-initialization after page navigation
- CSS fade-in/fade-out transitions

### Scroll Position Management

`nav-helpers.js` also owns centralized scroll position save/restore for all pages:

- **Save**: On every scroll event (debounced via boolean flag), saves `window.scrollY` keyed by `pathname + search`
- **Restore**: On back/forward navigation (traverse), restores the saved position after `markScriptsReady` signals that all rendering is complete
- **Retry**: If the page isn't tall enough yet (slow network / Blazor circuit still streaming content), a MutationObserver watches for DOM changes with a 50ms debounce and 1s hard deadline
- **Forward navigation**: Scrolls to top via `resetPagePosition()`

Detection uses the Navigation API (`currentEntry.navigationType === 'traverse'`) with a `popstate` fallback for browsers without it. Browser-native scroll restoration is disabled (`history.scrollRestoration = 'manual'`) in `TechHub.Web.lib.module.js` to prevent races with Blazor's enhanced navigation.

The restore is triggered by `markScriptsReady()` (in `page-scripts.js`) calling `window.__restoreScrollPosition()`. Every page **must** call `markScriptsReady` in its `OnAfterRenderAsync` — this is enforced by a convention test (`PageMarkScriptsReadyConventionTests`).

### Cascade Prevention (`window.__scrollRestoring`)

During back/forward navigation, `window.__scrollRestoring` coordinates all scroll-event listeners to prevent spurious triggers while the page is scrolling to the saved position:

- **Set to `true`**: On `popstate` (immediately, before any rendering)
- **Cleared to `false`**: In `finishScrollRestore()`, called from every `restoreScrollPosition()` exit path
- **After clearing**: A synthetic `scroll` event is dispatched so all paused listeners fire once with the final restored position

Listeners that check this flag:

| Listener | What it prevents |
|----------|------------------|
| `infinite-scroll.js` `handleScroll` | Loading extra content batches while restore is in progress |
| `toc-scroll-spy.js` `handleScroll` | Calling `replaceState` with a stale heading hash that would corrupt the restored URL |
| `nav-helpers.js` `onScrollSave` | Overwriting the saved position with `scrollY=0` (the browser-reset value before restore) |

The `pushState` interceptor also clears `__scrollRestoring` immediately for forward navigation, ensuring a previous interrupted back-nav cannot leave the flag stuck `true`.

## TOC Scroll-Spy

`wwwroot/js/toc-scroll-spy.js` highlights table of contents links based on scroll position.

**Critical**: Uses `history.replaceState()` instead of `pushState()` to update URL hash. This prevents polluting browser history with scroll positions - only actual TOC link clicks create history entries.

```javascript
// ❌ WRONG - Creates history entry for every scroll update
history.pushState(null, '', newUrl);

// ✅ CORRECT - Updates URL without creating history entry
history.replaceState(null, '', newUrl);
```

**Why This Matters**: When users scroll through content, only intentional navigation (clicking TOC links) should create history entries. The back button takes users to the previous page, not the previous scroll position.

`toc-scroll-spy.js` also checks `window.__scrollRestoring` in `handleScroll` and returns early when set. This prevents it from calling `replaceState` during scroll restoration, which would append a stale heading hash to the restored URL and trigger Blazor's hash-scroll handler — resetting `scrollY` to 0 and cascading into infinite-scroll loads.

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
- Navigation helpers: [src/TechHub.Web/wwwroot/js/nav-helpers.js](../src/TechHub.Web/wwwroot/js/nav-helpers.js)
- TOC scroll-spy: [src/TechHub.Web/wwwroot/js/toc-scroll-spy.js](../src/TechHub.Web/wwwroot/js/toc-scroll-spy.js)

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
