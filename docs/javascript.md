# JavaScript Architecture

Tech Hub uses a minimal JavaScript approach where Blazor handles most interactivity server-side. JavaScript is only used for browser-native features that Blazor cannot access.

## When JavaScript Is Required

JavaScript is ONLY for:

1. **Browser-native features** Blazor can't access (scroll position, IntersectionObserver, history API)
2. **Enhanced navigation hooks** (Blazor `enhancedload` event for SPA-style page transitions)
3. **Third-party libraries** (Highlight.js, Mermaid) that require JavaScript execution

**Never create**: Client-side filtering JavaScript. Tag filtering is 100% Blazor server-side.

## Loading Strategies

| Loading Type | Use When | Example | How |
|--------------|----------|---------|-----|
| **Static** | Every page needs it | `nav-helpers.js` | `<script src="@Assets[...]" defer>` |
| **Dynamic ES Module** | Only some pages need it | `toc-scroll-spy.js`, `custom-pages.js` | `import('./js/file.js')` via ImportMap |
| **External CDN** | Third-party library | Highlight.js, Mermaid | Dynamic `loadScript()` with SRI |

## Fingerprinting (Cache Busting)

All local JavaScript files MUST use fingerprinted URLs for proper cache invalidation.

### Static Scripts

Use `@Assets["js/file.js"]`:

```html
<!-- ✅ CORRECT - Fingerprinted URL (cache busting works) -->
<script src="@Assets["js/nav-helpers.js"]" defer></script>

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
| `nav-helpers.js` | Back to top, back to previous buttons | Static (every page) | IIFE |
| `toc-scroll-spy.js` | TOC scroll highlighting, history management | Dynamic (pages with TOC) | ES Module |
| `custom-pages.js` | Collapsible sections for SDLC/DX pages | Dynamic (pages with `[data-collapsible]`) | ES Module |

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

## Adding New JavaScript Files

1. **Add file** to `wwwroot/js/`
2. **Update** `Configuration/JsFiles.cs` for documentation
3. **Load correctly**:
   - Static (every page): Add `<script src="@Assets["js/file.js"]" defer>` to App.razor
   - Dynamic (conditional): Add `await import('./js/file.js')` in the module script block
4. **Document** purpose in this file under "Local JavaScript Files"

## Implementation Reference

- JavaScript configuration: [src/TechHub.Web/Configuration/JsFiles.cs](../src/TechHub.Web/Configuration/JsFiles.cs)
- CDN library versions: [src/TechHub.Web/Configuration/CdnLibraries.cs](../src/TechHub.Web/Configuration/CdnLibraries.cs)
- Navigation helpers: [src/TechHub.Web/wwwroot/js/nav-helpers.js](../src/TechHub.Web/wwwroot/js/nav-helpers.js)
- TOC scroll-spy: [src/TechHub.Web/wwwroot/js/toc-scroll-spy.js](../src/TechHub.Web/wwwroot/js/toc-scroll-spy.js)
