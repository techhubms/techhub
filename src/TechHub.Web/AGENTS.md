# Blazor Frontend Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Web/`. It complements the [Root AGENTS.md](../../AGENTS.md) and [src/AGENTS.md](../AGENTS.md).

## Overview

Blazor frontend with global InteractiveServer render mode and prerendering. Consumes TechHub.Api through `TechHubApiClient`. Renders content using the Tech Hub design system.

**Testing**: Component tests → [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md) | E2E tests → [tests/TechHub.E2E.Tests/AGENTS.md](../../tests/TechHub.E2E.Tests/AGENTS.md)

## Critical Rules

### ✅ Always Do

- **Always use design tokens exclusively** — ALL colors, spacing, typography from `wwwroot/css/design-tokens.css` (see [docs/design-system.md](../../docs/design-system.md))
- **Always use PersistentComponentState for pages with async data** — Prevents duplicate API calls during prerender→hydration
- **Always call `markScriptsReady` in every page's `OnAfterRenderAsync`** — Required for scroll position restoration on back/forward navigation. Pages with scripts call it after init; pages without call it directly. A build-time test enforces this rule.
- **Always progressive enhancement** — Core functionality works without JavaScript
- **Always use TechHubApiClient for all API calls** — Typed HTTP client in `Services/TechHubApiClient.cs`
- **Always follow semantic HTML structure** — `<main>`, `<section>`, `<article>`, `<aside>` (see [docs/page-structure.md](../../docs/page-structure.md))
- **Always add tests for components** — bUnit for component testing (see [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md))
- **Always sanitize user-controlled strings for logging** — `.Sanitize()` from `TechHub.Core.Logging`. In `TechHubApiClient`, overwrite parameters at entry. In Blazor components, call `.Sanitize()` inline. See [docs/input-validation-and-sanitization.md](../../docs/input-validation-and-sanitization.md).

### ⚠️ Ask First

- Adding new component dependencies, changing global CSS, modifying TechHubApiClient, breaking component APIs

### 🚫 Never Do

- **Never hardcode colors/spacing/typography** — Use design tokens from `wwwroot/css/design-tokens.css`
- **Never use inline styles with hardcoded values** — Use CSS classes with design token references
- **Never use hex codes or rgba() directly in CSS** — Define tokens in design-tokens.css first
- **Never duplicate component logic** — Extract to shared components
- **Never skip error handling** — Use try-catch and display user-friendly messages
- **Never create content without server-side rendering** — Initial load must show complete content
- **Never use /assets/ paths for images** — Use `/images/` convention
- **Never use `<div>` for main content areas** — Use semantic HTML elements

## Key Documentation References

- **Render Modes**: [docs/render-modes.md](../../docs/render-modes.md) — SSR vs Interactive Server strategy
- **Page Structure**: [docs/page-structure.md](../../docs/page-structure.md) — Semantic HTML, layouts, sticky header, sidebar, skeleton loading
- **Design System**: [docs/design-system.md](../../docs/design-system.md) — Design tokens, colors, typography, CSS architecture

## Razor Variable Naming Conflicts

**🚨 CRITICAL**: Certain variable names conflict with Razor directives.

`@section` is a Razor directive — using `section` as a variable name causes compilation errors. Always use `sectionData` instead.

| Avoid     | Use Instead      | Reason                              |
|-----------|------------------|-------------------------------------|
| `section` | `sectionData`    | Conflicts with `@section` directive |
| `code`    | `codeBlock`      | Conflicts with `@code`              |
| `page`    | `pageData`       | Conflicts with `@page`              |
| `layout`  | `layoutData`     | Conflicts with `@layout`            |

## CSS Architecture

### CSS Bundle Configuration

CSS files are defined once in `TechHub.Web.Configuration.CssFiles.All` and referenced by both App.razor and Program.cs.

- `@Assets["path"]` produces fingerprinted URLs (e.g., `base.e3opgp91n1.css`)
- `@Assets` requires string literals for compile-time fingerprint resolution
- **When adding CSS files**: Add to both `CssFiles.All` array AND an `@Assets` reference in App.razor

### Component-Scoped CSS

- Component `.razor.css` files for component-specific styles
- Global CSS in `wwwroot/css/` for shared styles
- See [docs/design-system.md](../../docs/design-system.md#css-architecture) for organization rules

## JavaScript Architecture

**CRITICAL PRINCIPLE**: JavaScript is ONLY for browser-native features (scroll, history API), enhanced navigation hooks, and third-party libraries. Blazor handles all other interactivity server-side.

### Loading Strategies

| Loading Type          | Use When                | How                                      |
| --------------------- | ----------------------- | ---------------------------------------- |
| **Static**            | Every page needs it     | `<script src="@Assets[...]" defer>`      |
| **Dynamic ES Module** | Only some pages need it | `import('./js/file.js')` via ImportMap   |
| **External CDN**      | Third-party library     | Dynamic `loadScript()` with SRI          |

### Fingerprinting (Cache Busting)

**ALL local JavaScript files MUST use fingerprinted URLs**:

- Static scripts: `@Assets["js/file.js"]`
- Dynamic imports: `import('./js/file.js')` (ImportMap rewrites to fingerprinted path)
- ImportMap in App.razor generates `<script type="importmap">` mapping to fingerprinted URLs

### JavaScript Files Reference

| File                 | Purpose                                     | Loading             |
| -------------------- | ------------------------------------------- | ------------------- |
| `nav-helpers.js`     | Back to top, back to previous, scroll pos   | Static (every page) |
| `page-scripts.js`    | CDN loading, init functions, scroll restore | Static ES module    |
| `toc-scroll-spy.js`  | TOC scroll highlighting, history management | Dynamic (TOC pages) |
| `custom-pages.js`    | Collapsible sections for SDLC/DX pages      | Dynamic             |
| `infinite-scroll.js` | Scroll-event-based infinite pagination      | Dynamic             |
| `mobile-nav.js`      | Mobile hamburger menu scroll lock           | Dynamic             |

Special: `TechHub.Web.lib.module.js` — Blazor lifecycle callbacks (auto-discovered by Blazor)

**NEVER create**: Client-side filtering JavaScript. Tag filtering is 100% Blazor server-side.

### Blazor @onclick vs. Vanilla JS Event Listeners

**Use Blazor `@onclick` for any interactive element inside a Blazor component.**

Vanilla JS event listeners attached via `addEventListener` or `initXxx()` functions are lost when Blazor patches the DOM during re-renders (e.g., state updates, parameter changes). Blazor `@onclick` bindings survive DOM patching because they are managed by the SignalR circuit.

**Rule**: If a button or interactive element lives in a `.razor` file (or in a child component rendered by one), handle its click with `@onclick`, not with a JS init function.

**When JS init functions are still appropriate**:

- Elements on **static HTML pages** or content rendered server-side outside of Blazor
- **Browser-native features** with no C# equivalent (scroll events, resize, history API)
- **Third-party widget initialization** (e.g., Highlight.js, Mermaid)

**Coordination with existing JS init functions**: If an element is handled by Blazor `@onclick` but a JS init function would also try to attach a listener, add `data-initialized="blazor"` to the element. The existing `data-initialized` guard in `initExpandableBadges` and `initFeatureFilters` will skip it.

### External CDN Libraries

Versions and SRI hashes centralized in [Configuration/CdnLibraries.cs](Configuration/CdnLibraries.cs). To update: change version → generate new SRI hash from srihash.org → update integrity hash → test locally.

### Adding New JavaScript Files

1. Add file to `wwwroot/js/`
2. Update `Configuration/JsFiles.cs`
3. Static: add `<script src="@Assets[\"js/file.js\"]" defer>` to App.razor | Dynamic: use `import()`
4. Document in JavaScript Files Reference above

## Conditional JavaScript Loading

Each page component calls only the init functions it needs from `OnAfterRenderAsync`:

- `initHighlighting()` → Highlight.js CDN
- `initMermaid()` → Mermaid CDN
- `initTocScrollSpy()` → TOC scroll-spy module
- `initCustomPages()` → Collapsible cards module
- `markScriptsLoading()` / `markScriptsReady()` → E2E test signals

CDN config bridged from C# via inline script in App.razor (`window.TechHubCDN`). `page-scripts.js` tracks loaded state to avoid duplicate CDN fetches.

| Component | Init Functions |
|---|---|
| `Home.razor` | `initCustomPages` |
| `ContentItem.razor` | `initHighlighting`, `initMermaid`, `initTocScrollSpy` |
| `GenAI.razor` | `initHighlighting`, `initMermaid`, `initTocScrollSpy` |
| `DXSpace.razor` | `initCustomPages`, `initTocScrollSpy` |
| `GitHubCopilotHandbook.razor` | `initCustomPages`, `initTocScrollSpy` |
| `GitHubCopilotFeatures.razor` | *(none — filters use Blazor `@onclick`)* |
| `GitHubCopilotLevels.razor` | `initCustomPages`, `initTocScrollSpy` |
| `GitHubCopilotVSCodeUpdates.razor` | `initHighlighting`, `initMermaid`, `initTocScrollSpy` |
| `AISDLC.razor` | `initCustomPages`, `initTocScrollSpy` |

## Image Conventions

### Multi-Format Support

Images provided in three formats: JPEG XL (`.jxl`, best compression), WebP (`.webp`, wide support), JPEG (`.jpg`, universal fallback).

Section backgrounds: `/images/section-backgrounds/{section-name}.{format}`

- CSS backgrounds: Reference `.webp`
- `<img>` tags: Use ResponsiveImage component for automatic `<picture>` generation

### Loading Rules

- **Hero/above-fold images**: Always eager load (omit `loading` attribute). These are LCP candidates.
- **Below-fold images**: `loading="lazy"` is fine for production
- **Always include `width` and `height` attributes**: Prevents CLS

## Static Files & Browser Caching

`StaticFilesCacheMiddleware` provides centralized cache control. Placed first in pipeline.

| Asset Type | Cache Strategy |
|---|---|
| Fingerprinted assets | `max-age=31536000,immutable` |
| Images/Fonts | `max-age=31536000,immutable` |
| CSS/JS (non-fingerprinted) | `max-age=3600,must-revalidate` |
| Other files | Browser defaults |

See [Middleware/StaticFilesCacheMiddleware.cs](Middleware/StaticFilesCacheMiddleware.cs) and [Program.cs](Program.cs).

## TOC Scroll-Spy

`toc-scroll-spy.js` highlights TOC links based on scroll position.

**CRITICAL**: Uses `history.replaceState()` (not `pushState()`) to update URL hash — prevents polluting browser history with scroll positions. Only TOC link clicks create history entries.

## Infinite Scroll

- **Items per batch**: 20 items
- **Prefetch trigger**: 300px margin
- **Sentinel element**: `#scroll-trigger` (removed when no more items)
- **Ready signal**: `window.__scrollListenerReady[triggerId]` (scoped per trigger to avoid interference)
- Uses `scroll` events + `getBoundingClientRect()` — deliberately no `requestAnimationFrame` throttling (rAF callbacks not delivered in headless Chrome with `--disable-gpu`)

## Date Formatting

Use `DateHelper` methods from `Helpers/DateHelper.cs` for consistent date display across the site. All dates use Europe/Brussels timezone.

## RSS Feed Proxy Endpoints

User-facing RSS feeds served by the Web frontend (proxied from internal API):

| URL | Description |
|---|---|
| `/feed` or `/feed/all` | All content |
| `/feed/{sectionName}` | Section-specific |
| `/feed/collection/{collectionName}` | Collection-specific |

## Mobile Navigation

Hamburger menu at ≤ 1024px breakpoint. Menu panel slides from right (320px / 85vw). Features: section expand/collapse, overlay click-to-close, body scroll lock via `mobile-nav.js`, Escape key close, close on navigation.

**Z-index stacking**: Nav header (1002) > Menu panel (1001) > Overlay (1000)

**Sidebar collapse**: `MobileSidebarCollapse` component — `display: contents` on desktop, toggle button on mobile. Uses `div` + Blazor `@onclick` (not `<details>/<summary>`).

## Tag Filtering

- `SidebarTagCloud`: Toggle tags on/off, stored in URL `?tags=tag1,tag2,tag3`
- Tags deduplicated and normalized (lowercased), matched with `StringComparer.OrdinalIgnoreCase`
- `ContentItemCard` badges highlight when matching active filter tags (`.badge-tag-active`)
- Active badges toggle: click removes filter instead of adding it

## Custom Page Ordering (SectionCard)

Custom pages (`IsCustom = true`) ordered by `Order` then `Title`. First 2 visible, rest behind "+X more" button. Expand is permanent (no collapse). CSS: `.custom-pages-expanded` uses `display: contents`.

## Client-Side Navigation Without Re-Renders

Use `history.pushState` for URL updates without triggering Blazor navigation (e.g., switching collections within a page). `OnParametersSetAsync` still handles browser back/forward. Call `StateHasChanged()` manually for partial re-renders.

## Component Catalog

**Layout**: `MainLayout`, `NavMenu`, `NavHeader`, `ReconnectModal`

**Pages**: `Home` (`/`), `SectionCollection` (`/{sectionName}`, `/{sectionName}/{collectionName}`), `ContentItem` (`/{sectionName}/{collectionName}/{slug}`), `About` (`/about`), `NotFound` (404)

**Shared**: `PageHeader`, `SidebarCollectionNav`, `SidebarRssLinks`, `SidebarTagCloud`

**Content**: `SectionCard`, `SectionCardsGrid`, `ContentItemCard`, `ContentItemsGrid`, `ContentItemDetail`

**Filter**: `SearchBox`, `TagFilter`, `DateFilter`
