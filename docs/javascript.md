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
| `scroll-manager.js` | Navigation buttons, keyboard nav detection, scroll position save/restore, TOC scroll spy | Static (`type="module"`) | ES Module |
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
