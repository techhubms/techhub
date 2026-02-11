# Page Structure

This document describes the semantic HTML structure, page layouts, and component architecture used across Tech Hub pages.

## Semantic HTML Structure

All pages use proper semantic HTML5 elements for accessibility, SEO, and maintainability.

### Required Structure by Page Type

**Homepage** (`Home.razor`):

```html
<html>
  <header>                           <!-- Site navigation (NavHeader + SectionBanner) -->
  <main class="page-with-sidebar">   <!-- Main content wrapper -->
    <aside class="sidebar">          <!-- Latest roundup, latest content, RSS links, tag cloud -->
    <section class="home-main-content">  <!-- Section cards grid -->
  </main>
  <footer>                           <!-- Site footer -->
</html>
```

**Section Pages** (`Section.razor`, `SectionCollection.razor`):

```html
<html>
  <header>                           <!-- Site navigation + section banner -->
  <main class="page-with-sidebar">   <!-- Main content wrapper -->
    <aside class="sidebar">          <!-- Tag cloud, RSS links -->
    <section>                        <!-- Content items grid -->
  </main>
  <footer>                           <!-- Site footer -->
</html>
```

**Content Item Pages** (`ContentItem.razor`):

```html
<html>
  <header>                           <!-- Site navigation + section banner -->
  <main class="page-with-sidebar">   <!-- Main content wrapper -->
    <aside class="sidebar">          <!-- Tag cloud, table of contents -->
    <article>                        <!-- Individual content item -->
  </main>
  <footer>                           <!-- Site footer -->
</html>
```

**Custom Pages** (handbook, levels, etc.):

```html
<html>
  <header>                           <!-- Site navigation + section banner -->
  <main class="page-with-sidebar">   <!-- Main content wrapper -->
    <aside class="sidebar">          <!-- Video list, table of contents -->
    <article class="article-body">   <!-- Selected video/content -->
  </main>
  <footer>                           <!-- Site footer -->
</html>
```

**About Page** (`About.razor`):

```html
<html>
  <header>                           <!-- Site navigation + section banner -->
  <main class="page-without-sidebar">  <!-- Main content wrapper (no sidebar) -->
    <section>                        <!-- Team member grid -->
  </main>
  <footer>                           <!-- Site footer -->
</html>
```

## Semantic Element Usage

### Element Guidelines

| Element | Purpose | Example Usage |
|---------|---------|---------------|
| `<header>` | Site-wide navigation and page headers | NavHeader, SectionBanner, SubNav |
| `<main>` | Primary content wrapper (one per page) | Container for sidebar + main content |
| `<aside>` | Sidebar content (supplementary) | Tag clouds, RSS links, table of contents |
| `<section>` | Thematic grouping of content | Section cards grid, content items grid |
| `<article>` | Self-contained, distributable content | Blog posts, videos, content item details |
| `<footer>` | Site footer | Copyright, links, authorship |

### Choosing Between `<section>` and `<article>`

**Use `<section>` when**:

- Content is a thematic grouping (grid of cards, list of items)
- Content depends on surrounding context
- Examples: Section cards grid, content items grid, team members grid

**Use `<article>` when**:

- Content is self-contained and independently distributable
- Could be extracted and still make sense
- Examples: Blog post detail, video detail, content item detail

### Common Mistakes to Avoid

| Wrong | Correct |
|-------|---------|
| `<div>content</div>` inside `<main>` | `<section>content</section>` or `<article>content</article>` |
| `<div class="home-main-content">` | `<section class="home-main-content">` |
| `<main><div>content</div></main>` | `<main><section>content</section></main>` |

## Page Layout Classes

**CSS Classes** (defined in `page-container.css`):

- **`.page-with-sidebar`** - Applied to `<main>` for two-column grid layout (300px sidebar + 1fr content)
- **`.page-without-sidebar`** - Applied to `<main>` for single-column centered layout
- **`.article-body`** - Applied to content container within `<article>` for typography styling

**Why `<aside>` is INSIDE `<main>`**: Per WAI-ARIA best practices, complementary content (`<aside>`) that's directly related to the main content should be nested within the `<main>` landmark. This helps assistive technologies understand the page structure.

## Sticky Header Architecture

The navigation uses `display: contents` to achieve proper sticky behavior across the full page scroll.

### The Problem

By default, `position: sticky` elements only stick while their parent container is visible. With a traditional `<header>` wrapper, navigation would disappear once you scrolled past the section banner.

### The Solution

Use `display: contents` on the `<header>` element to "remove" it from the box tree, allowing sticky children to stick relative to `<body>` instead of `<header>`.

```css
/* Header.razor.css */
header {
    display: contents;
    /* Removes header from box tree - nav/subnav become direct children of body */
}

/* NavHeader.razor.css */
.main-nav {
    position: sticky;
    top: 0;              /* Sticks to very top of viewport */
    z-index: 1000;       /* Above scrolling content */
}

/* SubNav.razor.css */
.sub-nav {
    position: sticky;
    top: 76px;          /* Sticks below main-nav (76px = main-nav height) */
    z-index: 999;       /* Slightly lower than main-nav */
}
```

### How It Works

1. `display: contents` makes the browser treat `<nav>`, `<section-banner>`, and `<subnav>` as direct children of `<body>` for layout purposes
2. `position: sticky` on navigation elements makes them stick relative to the viewport scroll
3. Section banner scrolls normally and slides underneath the navigation
4. Layered sticking achieved through `top` offsets: main-nav at `0`, sub-nav at `76px`

### Benefits

- Navigation remains visible during entire page scroll
- Section banner scrolls away naturally, revealing content
- Clean HTML structure maintained (semantic `<header>` wrapper)
- No JavaScript required

## Sidebar Component Architecture

Pages define layout structure with `<aside class="sidebar">`, sidebar components only render their content.

### Responsibility Pattern

**Pages** (`Section.razor`, `Home.razor`, `ContentItem.razor`):

- Use `.page-with-sidebar` container class
- Define the `<aside class="sidebar">` container
- Determine which sidebar components to render
- Control component order and composition
- Pass required parameters to components

**Sidebar Components** (`SidebarCollectionNav.razor`, `SidebarRssLinks.razor`, `SidebarTagCloud.razor`):

- Render their specific content (navigation, RSS links, tag clouds)
- Use semantic HTML appropriate to their purpose (`<nav>`, `<div>`, etc.)
- Apply shared sidebar styles (`.sidebar-section`, `.sidebar-link-button`, etc.)
- DO NOT wrap themselves in `<aside class="sidebar">`
- DO NOT define their own container positioning

### Example Page with Sidebar

```razor
<Header SectionName="..." />

<main class="page-with-sidebar">
    <PageHeader Section="@sectionData" />
    
    <!-- Page defines sidebar container -->
    <aside class="sidebar">
        <!-- Page composes sidebar components -->
        <SidebarCollectionNav Section="@sectionData" SelectedCollection="all" />
        <SidebarRssLinks Links="@(new[] { ... })" />
    </aside>
    
    <!-- Use section for listing/grid content -->
    <section>
        <ContentItemsGrid ... />
    </section>
</main>
```

### Available Sidebar Components

| Component | Purpose | Parameters |
|-----------|---------|------------|
| `SidebarCollectionNav` | Section collections and custom pages navigation | `Section`, `SelectedCollection` |
| `SidebarRssLinks` | RSS feed and subscription links | `Links` (array of RssLink) |
| `SidebarTagCloud` | Interactive tag cloud for filtering | `SectionName` (fetches from API) |
| `SidebarToc` | Table of contents from HTML headings | `HtmlContent` |

### Sidebar Component Semantic HTML

Each component chooses its own semantic wrapper:

- `<nav>` - For site navigation (collections, pages)
- `<div>` - For non-navigational content (RSS feeds, metadata)
- `<aside>` - ONLY used by pages for the outer sidebar container

Multiple `<nav>` elements per page are valid. Screen readers can distinguish between different navigation sections.

## Skeleton Loading States

Skeleton loading states provide immediate visual feedback that navigation completed and content is loading.

### When to Use Skeletons

| Use Case | Use Skeleton? |
|----------|---------------|
| Content pages (Section, article detail) | Yes |
| API-dependent components | Yes |
| Heavy data pages | Yes |
| Home page (lightweight data) | No |
| Static pages without API calls | No |
| Fast-loading components (<100ms) | No |

### Why Skeletons Matter

1. **Immediate feedback** - Users know navigation completed
2. **Page structure visible** - Shows layout immediately
3. **Better perceived performance** - Feels faster than blank page
4. **Prevents layout shift** - Skeleton reserves space for content

### Skeleton Layout Architecture

Use CSS Grid with three independently-loading components that maintain stable positions. Each component shows skeleton placeholders while loading.

**Key Principles**:

1. CSS Grid areas have fixed positions that never change
2. Each component loads its own data asynchronously
3. Show loading state without layout shift
4. Use `@key` directive to force component re-render on navigation

## Mobile Navigation

On mobile devices (< 768px), the layout adapts:

### Responsive Behavior

- **Sidebar**: Moves below main content or collapses into hamburger menu
- **Grid**: Changes from two-column to single-column layout
- **Touch targets**: Minimum 44px for accessibility
- **Navigation**: Remains sticky at top of viewport

### Hamburger Menu Pattern

```razor
<nav class="site-nav">
    <button class="hamburger" @onclick="ToggleMenu" aria-label="Toggle menu">
        <span></span>
        <span></span>
        <span></span>
    </button>
    
    <div class="nav-menu @(menuOpen ? "open" : "")">
        <a href="/">Home</a>
        <a href="/ai">AI</a>
        <!-- More links -->
    </div>
</nav>
```

### Mobile CSS

```css
/* Desktop - normal nav */
.hamburger { display: none; }
.nav-menu { display: flex; gap: 1rem; }

/* Mobile - hamburger menu */
@media (max-width: 768px) {
    .hamburger { display: block; }
    .nav-menu { display: none; flex-direction: column; }
    .nav-menu.open { display: flex; }
}
```

## Infinite Scroll Pagination

Long content lists use infinite scroll for progressive loading.

### Configuration

- **Items per batch**: 20 items
- **Trigger margin**: 300px above viewport bottom (loads next batch before user reaches the end)
- **URL parameter preservation**: Maintains filters/search when loading more

### Pattern

```razor
<div id="content-container">
    @foreach (var item in visibleItems)
    {
        <ContentItemCard Item="@item" Section="@section" />
    }
</div>

@if (hasMore)
{
    <div id="scroll-trigger" style="height: 1px;"></div>
}

@code {
    private List<ContentItem> visibleItems = new();
    private bool hasMore = true;
    private const int PageSize = 20;

    [JSInvokable]
    public async Task LoadNextBatch()
    {
        var nextBatch = await ApiClient.GetContentAsync(
            section: sectionName,
            page: currentPage++,
            pageSize: PageSize);

        if (nextBatch.Count < PageSize)
            hasMore = false;

        visibleItems.AddRange(nextBatch);
        StateHasChanged();
    }
}
```

### JavaScript

Uses scroll events and `getBoundingClientRect()` to detect when the trigger element
is near the viewport bottom (same pattern as the TOC scroll-spy). Sets
`window.__scrollListenerReady[triggerId] = true` when the listener is attached so
E2E tests can wait for it before scrolling. Readiness is scoped by trigger element ID
so multiple concurrent listeners don't interfere.

```javascript
function handleScroll() {
    const trigger = document.getElementById(triggerId);
    if (!trigger) return;

    const rect = trigger.getBoundingClientRect();
    if (rect.top <= window.innerHeight + 300) {
        dotnetHelper.invokeMethodAsync('LoadNextBatch');
    }
}

window.addEventListener('scroll', handleScroll, { passive: true });
```

## Implementation Reference

- **Page container CSS**: [src/TechHub.Web/wwwroot/css/page-container.css](../src/TechHub.Web/wwwroot/css/page-container.css)
- **Sidebar CSS**: [src/TechHub.Web/wwwroot/css/sidebar.css](../src/TechHub.Web/wwwroot/css/sidebar.css)
- **Blazor component patterns**: [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md)
