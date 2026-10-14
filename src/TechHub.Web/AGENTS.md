# Blazor Frontend Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Web/`. It complements the [Root AGENTS.md](../../AGENTS.md) and [src/AGENTS.md](../AGENTS.md).

## Overview

Blazor frontend with global InteractiveServer render mode and prerendering. Consumes TechHub.Api through `TechHubApiClient`. Renders content using the Tech Hub design system.

**Testing**: Component tests → [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md) | E2E tests → [tests/TechHub.E2E.Tests/AGENTS.md](../../tests/TechHub.E2E.Tests/AGENTS.md)

## Critical Rules

### ✅ Always Do

- **Always use design tokens exclusively** — ALL colors, spacing, typography from `wwwroot/css/design-tokens.css` (see [docs/design-system.md](../../docs/design-system.md))
- **Always use PersistentComponentState for pages with async data** — Prevents duplicate API calls during prerender→hydration
- **Always inherit `PageComponentBase` in every routable page** (`@inherits PageComponentBase`) — it runs the `markScriptsLoading` / init functions / `markScriptsReady` lifecycle that scroll position restoration on back/forward navigation depends on. Do not call `markScriptsReady` yourself. A build-time convention test (`PageMarkScriptsReadyConventionTests`) enforces the inheritance. See [Page Script Lifecycle](#page-script-lifecycle).
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

CSS files are defined once in `TechHub.Web.Configuration.CssFiles.All` and referenced by `App.razor` through `@Assets[...]`. `Program.cs` does not use the list; tests use it to verify every CSS file is referenced in `App.razor`.

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

| Loading Type          | Use When                          | How                                               |
| --------------------- | --------------------------------- | ------------------------------------------------- |
| **Static (module)**   | Every page, needs ES module scope | `<script type="module" src="@Assets[...]">` |
| **Static (plain)**    | Every page, no module features    | `<script src="@Assets[...]" defer>`           |
| **Dynamic ES Module** | Only some pages need it           | `import('./js/file.js')` via ImportMap             |
| **External CDN**      | Third-party library     | Dynamic `loadScript()` with SRI          |

### Fingerprinting (Cache Busting)

**ALL local JavaScript files MUST use fingerprinted URLs**:

- Static scripts: `@Assets["js/file.js"]`
- Dynamic imports: `import('./js/file.js')` (ImportMap rewrites to fingerprinted path)
- ImportMap in App.razor generates `<script type="importmap">` mapping to fingerprinted URLs

### JavaScript Files Reference

| File                  | Purpose                                           | Loading          |
| --------------------- | ------------------------------------------------- | ---------------- |
| `scroll-manager.js`   | Navigation, scroll position, TOC spy, buttons | Static ES module |
| `mobile-nav.js`       | Mobile navigation scroll lock and keyboard handling | Static (defer) |
| `sidebar-toggle.js`   | Desktop sidebar collapse/expand with cookie persistence | Static (defer)   |
| `hero-banner.js`      | Hero banner collapse/expand with cookie persistence | Static (defer) |
| `page-scripts.js`     | CDN loading, page init functions (mermaid, highlight.js, TOC, custom pages), script lifecycle | Static ES module (listed under `DynamicLoad` in `JsFiles.cs`) |
| `custom-pages.js`     | Collapsible sections for SDLC/DX pages, feature filters, expandable badges | Dynamic (imported by `initCustomPages`) |
| `date-range-slider.js` | Date range slider client-side clamping           | Dynamic          |

Special:

- `wwwroot/TechHub.Web.lib.module.js` — Blazor lifecycle callbacks (auto-discovered by Blazor)
- `Components/Layout/ReconnectModal.razor.js` — Reconnect dialog behavior, loaded by `ReconnectModal.razor`

See [docs/javascript.md](../../docs/javascript.md) for the window globals table, lifecycle hooks and `custom-pages.js` exports.

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
3. Static module: add `<script type="module" src="@Assets[\"js/file.js\"]">` to App.razor | Static plain: add `<script src="@Assets[\"js/file.js\"]" defer>` | Dynamic: use `import()`
4. Document in JavaScript Files Reference above

## Conditional JavaScript Loading

### Page Script Lifecycle

Every routable page declares `@inherits PageComponentBase` ([Components/PageComponentBase.cs](Components/PageComponentBase.cs)). The base class implements `OnAfterRenderAsync` and runs the JS lifecycle, so pages normally do **not** override `OnAfterRenderAsync` for scripts. If a page needs its own post-render logic, override it and call `await base.OnAfterRenderAsync(firstRender)` at the end.

Pages configure the lifecycle by overriding properties:

| Property | Default | Meaning |
|---|---|---|
| `PageScripts` | `[]` | JS function names invoked in order once data is ready (for example `["initHighlighting", "initTocScrollSpy"]`) |
| `IsPageDataReady` | `true` | `true` once async data has loaded (for example `pageData != null`); scripts do not run before |
| `IsNotFound` | `false` | `true` when the page rendered not-found content and data will never arrive; calls `markScriptsReady` so waiters do not hang |
| `ReinitializeScriptsOnRerender` | `false` | `true` re-runs scripts on every render where `IsPageDataReady` is true. Use for pages that swap content without a component teardown (`ContentItem`, `GenAI`, `GitHubCopilotVSCodeUpdates`) |

Flow per render: `IsNotFound` → `markScriptsReady`; not `IsPageDataReady` → wait for the next render; no `PageScripts` → `markScriptsReady`; otherwise (once, unless `ReinitializeScriptsOnRerender`) `markScriptsLoading` → each script → `markScriptsReady` (in a `finally`).

Available init functions (defined in `page-scripts.js`, all idempotent):

- `initHighlighting` → Highlight.js CDN
- `initMermaid` → Mermaid CDN
- `initTocScrollSpy` → TOC scroll-spy module
- `initCustomPages` → Collapsible cards, feature filters and expandable badges module (`custom-pages.js`)

CDN config is bridged from C# via an inline script in App.razor (`window.TechHubCDN`). `page-scripts.js` tracks loaded state to avoid duplicate CDN fetches.

### Init Functions per Page

| Component | `PageScripts` | Re-run on rerender |
|---|---|---|
| `Home.razor` | `initCustomPages` | No |
| `ContentItem.razor` | `initHighlighting`, `initMermaid`, `initTocScrollSpy` | Yes |
| `GenAI.razor` | `initHighlighting`, `initMermaid`, `initTocScrollSpy` | Yes |
| `DXSpace.razor` | `initTocScrollSpy` | No |
| `GitHubCopilotHandbook.razor` | `initCustomPages`, `initTocScrollSpy` | No |
| `GitHubCopilotLevels.razor` | `initCustomPages`, `initTocScrollSpy` | No |
| `GitHubCopilotGettingStarted.razor` | `initHighlighting`, `initTocScrollSpy` | No |
| `GitHubCopilotVSCodeUpdates.razor` | `initHighlighting`, `initMermaid`, `initTocScrollSpy` | Yes |
| `AISDLC.razor` | `initMermaid`, `initTocScrollSpy` | No |
| `GitHubCopilotFeatures.razor` | *(none in `PageScripts`)* — filters use Blazor `@onclick`; it calls the `scrollTierCardIntoView` and `preventScrollOnExpand` globals from `page-scripts.js` through JS interop | No |

All other pages (`SectionCollection`, `Authors`, `About`, `McpInfo`, `Newsletter*`, `Admin*`, `Error`, `NotFound`, and the `GitHubCopilotDiscord` and `GitHubCopilotToolTips` pages) declare no scripts, so the base class only calls `markScriptsReady`.

## Image Conventions

### Multi-Format Support

Images provided in three formats: JPEG XL (`.jxl`, best compression), WebP (`.webp`, wide support), JPEG (`.jpg`, universal fallback).

Section backgrounds: `/images/section-backgrounds/{section-name}.{format}`

- CSS backgrounds: Reference `.webp`
- `<img>` tags: Use the `ResponsiveImage` component for automatic `<picture>` generation (it exists but no page uses it today)

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

`scroll-manager.js` highlights TOC links based on scroll position using **RAF-throttled `scroll` events** (one update per frame — real-time as user scrolls), with a final pass on `scrollend` to settle at the exact resting position. The debounce fallback (150ms) is used on browsers without native `scrollend`.

**CRITICAL**: Uses `history.replaceState()` (not `pushState()`) to update URL hash — prevents polluting browser history with scroll positions. Only TOC link clicks create history entries.

## Load More Button

- **Items per batch**: 40 items
- **Load More button**: shown when `hasMoreContent: true` in the circuit cache; hidden when all content is loaded
- **End of content**: shown when `hasMoreContent: false` and at least one item is loaded
- Pure Blazor `@onclick` — no JavaScript or `JSRuntime` needed
- **Architecture deep-dive**: See [docs/scroll-system-architecture.md](../../docs/scroll-system-architecture.md) for the full scroll system (navigation lifecycle, save/restore, TOC, known edge cases, test coverage)

## Date Formatting

Use `DateHelper` methods from [Services/DateHelper.cs](Services/DateHelper.cs) (`FormatDateRelative`, `FormatDateFull`, `FormatDateIso`, `FormatTimestamp`) for consistent date display across the site. All dates use Europe/Brussels timezone.

## RSS Feed Proxy Endpoints

User-facing RSS feeds served by the Web frontend (proxied from internal API):

| URL | Description |
|---|---|
| `/all/feed.xml` | All content across all sections |
| `/{sectionName}/roundups/feed.xml` | Roundups for a specific section |
| `/{sectionName}/feed.xml` | Section-specific content |

## Mobile Navigation

Hamburger menu at ≤ 1292px breakpoint (`NavHeader.razor.css`, `SubNav.razor.css`, `page-container.css` and `sidebar.css` all switch at 1292px). Menu panel slides from right (320px / 85vw) and starts below the compact header (`--mobile-nav-height`, 64px). Features: section expand/collapse, overlay click-to-close, body scroll lock via `mobile-nav.js`, Escape key close, close on navigation.

**Z-index stacking**: Nav header (1002) > Menu panel (1001) > Overlay (1000)

**Sidebar on mobile**: `MobileSidebarToolbar` + `MobileSidebarPanel` — `display: contents` on desktop, a button bar with one toggleable panel at a time on mobile. Panels take `Label`, `Icon` and `HasActiveFilter`. `MobileSidebarCollapse` (a single collapse wrapper, tested but not used by any page) is the older pattern; use the toolbar and panels. Both use Blazor `@onclick` (not `<details>/<summary>`). Note: comments in `MobileSidebarCollapse.razor` and `MobileSidebarToolbar.razor` mention 1024px, but the CSS breakpoint is 1292px.

**Desktop sidebar collapse**: `SidebarToggle` (rendered at the top of every `<aside class="sidebar">`) calls `TechHub.sidebar.toggle()`. State lives in the `sidebar-collapsed` cookie; `App.razor` reads it during SSR and sets the `sidebar-collapsed` class on `<html>` (`HtmlClass`), so there is no flash. Layout is in `css/page-container.css`, hiding rules in `css/sidebar.css`.

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

Generated from the directory contents (`list_dir` on `Components/`). Re-check the directories when adding or removing components; do not trust this list over the code.

**Layout** (`Components/Layout/`): `MainLayout`, `AdminLayout`, `ReconnectModal` (+ `.razor.js`). Also in `Components/`: `App` (root document), `Routes`, `Header` (composes `NavHeader`, `SectionBanner`, `SubNav`), `NavHeader`, `SectionBanner`, `SubNav`, `SkipLink`, `SeoMetaTags`, `NotFoundContent`

**Base class**: `PageComponentBase` (`Components/PageComponentBase.cs`) — see [Page Script Lifecycle](#page-script-lifecycle)

**Pages** (`Components/Pages/`):

| Page | Route(s) |
|---|---|
| `Home` | `/` |
| `SectionCollection` | `/{sectionName}`, `/{sectionName}/{collectionName}` |
| `ContentItem` | `/{sectionName}/{collectionName}/{slug}` |
| `Authors` | `/all/authors`, `/all/authors/{authorName}` |
| `About` | `/about` |
| `McpInfo` | `/mcp` |
| `Error`, `NotFound` | `/Error`, `/not-found` |
| `GenAI` | `/ai/genai-basics`, `/ai/genai-applied`, `/ai/genai-advanced` |
| `AISDLC` | `/ai/sdlc` |
| `DXSpace` | `/devops/dx-space` |
| `GitHubCopilotFeatures`, `GitHubCopilotVSCodeUpdates` (also `/{VideoSlug}`), `GitHubCopilotGettingStarted`, `GitHubCopilotLevels`, `GitHubCopilotHandbook`, `GitHubCopilotToolTips`, `GitHubCopilotDiscord` | `/github-copilot/features`, `/vscode-updates`, `/getting-started`, `/levels-of-enlightenment`, `/handbook`, `/tool-tips`, `/discord` |
| `NewsletterSubscribe`, `NewsletterConfirm`, `NewsletterManage`, `NewsletterUnsubscribe` | `/newsletter/subscribe`, `/confirm`, `/manage`, `/unsubscribe` (see [docs/newsletter.md](../../docs/newsletter.md)) |
| `Admin*` (`Components/Pages/Admin/`) | `/admin` and `/admin/*`, rendered with `AdminLayout` (see [docs/admin.md](../../docs/admin.md) and [docs/admin-authentication.md](../../docs/admin-authentication.md)) |

Admin pages: `AdminDashboard`, `AdminContentItems`, `AdminContentPreview`, `AdminCopilotFeatures`, `AdminCustomPages`, `AdminFeeds`, `AdminLogin` (+ `AdminLoginContent`), `AdminNewsletter`, `AdminProcessedUrls`, `AdminReviews`, `AdminSettings`, `AdminStatistics`, `AdminVscodeUpdates`. They declare `@rendermode InteractiveServer` explicitly.

**Sidebar**: `SidebarToggle`, `MobileSidebarToolbar`, `MobileSidebarPanel`, `SidebarSearch`, `SidebarTagCloud`, `SidebarToc`, `DateRangeSlider`

**Content**: `SectionCard`, `SectionCardsGrid`, `ContentItemCard`, `ContentItemsGrid`, `ContentItemDetail`, `HeroBanner`, `SafeMarkup`, `ReviewDiff` (uses `Helpers/DiffHelper.cs`)

**Admin modals**: `AddContentModal`, `ContentItemEditorModal`, `JsonEditorModal`

**Unused components** (exist, but no page or component renders them; keep or delete deliberately): `ResponsiveImage`, `MobileSidebarCollapse` (only tests render it), `SidebarRssLinks`

**Services** (`Services/`): `TechHubApiClient` / `ITechHubApiClient`, `AdminTokenDelegatingHandler`, `BrandingService`, `FaviconService`, `ErrorService`, `DateHelper`, `SectionHelper`, `ContentGridStateCache`, `HomepageStatsState`, `SectionCacheRefreshService`, `SectionCacheHealthCheck` (the `SectionCache` itself lives in `TechHub.Core.Models`), `HeroBannerCache` (+ `HeroBannerCacheRefreshService`), `ApiHealthCheck`

**Other directories**: `Configuration/` (`CdnLibraries`, `CssFiles`, `JsFiles`), `Middleware/`, `Endpoints/` (RSS proxy), `MCP/`, `Telemetry/`, `Helpers/` (`DiffHelper` only)
