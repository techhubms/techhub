# Blazor Frontend Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Web/`. It complements the [Root AGENTS.md](../../AGENTS.md) and [src/AGENTS.md](../AGENTS.md).
> **RULE**: Follow the 10-step workflow in Root [AGENTS.md](../../AGENTS.md). Project principles are in [README.md](../../README.md). Follow **BOTH**.

## Overview

This project implements the Blazor frontend with server-side rendering (SSR) and optional WebAssembly interactivity. It consumes the TechHub.Api through TechHubApiClient and renders content using the Tech Hub design system.

**When to read this file**: When creating or modifying Blazor components, pages, layouts, or understanding frontend architecture.

**Testing this code**:

- Component tests: See [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md) for bUnit patterns
- E2E tests: See [tests/TechHub.E2E.Tests/AGENTS.md](../../tests/TechHub.E2E.Tests/AGENTS.md) for Playwright patterns

## Critical Rules

### ✅ Always Do

- **Use design tokens exclusively** - ALL colors, spacing, typography from `wwwroot/css/design-tokens.css` (see [Design Tokens section](#design-tokens---single-source-of-truth))
- **Server-side render initial content** - Use SSR for SEO and performance
- **Progressive enhancement** - Core functionality works without JavaScript
- **Use TechHubApiClient for all API calls** - Typed HTTP client in `Services/TechHubApiClient.cs`
- **Follow Blazor component patterns** - See [Root AGENTS.md](../../AGENTS.md) for .NET/Blazor framework-specific guidance
- **Follow semantic HTML structure** - Use `<main>`, `<section>`, `<article>`, `<aside>` instead of `<div>` (see Semantic HTML Structure section)
- **Fix all linting errors** - Check with `get_errors` tool after editing files
- **Add tests for components** - Use bUnit for component testing (see [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md))

### ⚠️ Ask First

- **Adding new component dependencies** - Verify no duplication exists
- **Changing global CSS** - May affect all components
- **Modifying TechHubApiClient** - May impact all pages
- **Breaking component APIs** - Changes to @Parameters or public methods

### 🚫 Never Do

- **Never hardcode colors, spacing, or typography** - Always use design tokens from `wwwroot/css/design-tokens.css`
- **Never use inline styles with hardcoded values** - Use CSS classes with design token references
- **Never use hex codes or rgba() directly in CSS** - Define new tokens in design-tokens.css first
- **Never duplicate component logic** - Extract to shared components
- **Never skip error handling** - Use try-catch and display user-friendly messages
- **Never create content without server-side rendering** - Initial load must show complete content
- **Never use /assets/ paths for images** - Use `/images/` convention (e.g., `/images/section-backgrounds/`)
- **Never use `<div>` for main content areas** - Use semantic HTML elements (`<main>`, `<section>`, `<article>`)

## Semantic HTML Structure

**CRITICAL**: All pages must use proper semantic HTML5 elements for accessibility, SEO, and maintainability.

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
    <section>                        <!-- Content items grid (styled via parent selector) -->
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
    <article>                        <!-- Individual content item (styled via parent selector) -->
  </main>
  <footer>                           <!-- Site footer -->
</html>
```

**Custom Pages** (`GitHubCopilotVSCodeUpdates.razor`, `GitHubCopilotLevels.razor`, etc.):

```html
<html>
  <header>                           <!-- Site navigation + section banner -->
  <main class="page-with-sidebar">   <!-- Main content wrapper -->
    <aside class="sidebar">          <!-- Video list, table of contents -->
    <article class="article-body">   <!-- Selected video/content with article styling -->
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

### Semantic Element Usage Guidelines

**`<header>`**: Site-wide navigation and page headers

- Used by Header.razor component
- Contains NavHeader, SectionBanner, SubNav
- **CRITICAL**: Uses `display: contents` to allow sticky positioning relative to `<body>` (see Sticky Header Architecture)

**`<main>`**: Primary content wrapper (one per page)

- Container for sidebar + main content
- Classes: `page-with-sidebar` (most pages) or `page-without-sidebar` (About page)

**`<aside>`**: Sidebar content (supplementary information)

- Tag clouds, RSS links, table of contents, latest items
- Always in left column with class `sidebar`

**`<section>`**: Thematic grouping of content

- Use for: Section cards grid, content items grid, team members, about content
- Has heading and represents thematic grouping
- Classes: `home-main-content` (homepage only); other pages use no class (styled via `.page-with-sidebar > :is(article, section)`)

**`<article>`**: Self-contained, independently distributable content

- Use for: Individual blog posts, videos, content item details
- Can be extracted and make sense on its own
- Classes: `article-body` (for article content styling); no class needed for layout (styled via `.page-with-sidebar > :is(article, section)`)

**`<footer>`**: Site footer

- Copyright, links, authorship information

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

❌ **Wrong**: `<div>content</div>` inside `<main class="page-with-sidebar">`  
✅ **Correct**: `<section>content</section>` (for grids) or `<article class="article-body">content</article>` (for detail pages)

❌ **Wrong**: `<div class="home-main-content">`  
✅ **Correct**: `<section class="home-main-content">`

❌ **Wrong**: `<main><div>content</div></main>`  
✅ **Correct**: `<main><section>content</section></main>` or `<main><article>content</article></main>`

## Sticky Header Architecture

**CRITICAL**: The sticky navigation uses `display: contents` to achieve proper sticky behavior.

### The Problem

By default, `position: sticky` elements only stick while their parent container is visible. With a traditional `<header>` wrapper, navigation would disappear once you scrolled past the section banner.

### The Solution

We use `display: contents` on the `<header>` element to "remove" it from the box tree, allowing sticky children to stick relative to `<body>` instead of `<header>`.

**Implementation** (Header.razor.css):

```css
header {
    display: contents;
    /* Removes header from box tree - nav/subnav become direct children of body for layout */
}
```

**NavHeader** (NavHeader.razor.css):

```css
.main-nav {
  position: sticky;
  top: 0;              /* Sticks to very top of viewport */
  z-index: 1000;       /* Above scrolling content */
}
```

**SubNav** (SubNav.razor.css):

```css
.sub-nav {
    position: sticky;
    top: 76px;          /* Sticks below main-nav (76px = main-nav height) */
    z-index: 999;       /* Slightly lower than main-nav */
}
```

### How It Works

1. **`display: contents`** makes the browser treat `<nav>`, `<section-banner>`, and `<subnav>` as direct children of `<body>` for layout purposes
2. **`position: sticky`** on navigation elements makes them stick relative to the viewport scroll
3. **Section banner scrolls normally** (no sticky positioning) and slides underneath the navigation
4. **Layered sticking** achieved through `top` offsets: main-nav at `0`, sub-nav at `76px` (main-nav height)

### Benefits

- Navigation remains visible during entire page scroll (not just header height)
- Section banner scrolls away naturally, revealing content
- Clean HTML structure maintained (semantic `<header>` wrapper)
- No JavaScript required

## Tech Hub Design System

### CSS Architecture

Tech Hub uses a **dual CSS strategy**: **Global CSS** for reusable design system components and **Component-Scoped CSS** for page-specific layouts.

#### Global CSS (wwwroot/css/)

Reusable design system components and site-wide styles:

```text
wwwroot/css/
├── design-tokens.css         # ALL colors, typography, spacing (single source of truth)
├── base.css                  # Reset, typography, links, focus states, utilities, forms
├── sidebar.css               # Shared sidebar component (used across multiple pages)
├── nav-helpers.css           # Navigation helpers (back to top, back to previous)
├── loading.css               # Skeleton loaders, loading states
├── page-container.css        # Page layout containers
├── tag-dropdown.css          # Tag dropdown component
├── date-slider.css           # Date range slider component
└── article.css               # Article content styling
```

#### Component-Scoped CSS (.razor.css files)

Page and component-specific styles that are automatically isolated by Blazor:

```text
Components/
├── Layout/
│   ├── MainLayout.razor.css       # Main layout styles
│   └── ReconnectModal.razor.css   # Reconnect modal styles
├── Pages/
│   ├── Home.razor.css             # Home page (currently minimal)
│   ├── Section.razor.css          # Section page layout (all content)
│   ├── SectionCollection.razor.css # Section collection page layout (filtered content)
│   ├── ContentItem.razor.css      # Content item detail page layout
│   └── About.razor.css            # About page team grid
├── Shared/
│   ├── PageHeader.razor.css       # Universal section header banner
│   ├── NavHeader.razor.css        # Site navigation header
│   ├── SidebarToc.razor.css       # Table of contents component
│   └── SidebarTagCloud.razor.css  # Tag cloud component
└── Root Components/
    ├── SectionCard.razor.css          # Section card styling
    ├── ContentItemCard.razor.css      # Content item card styling
    ├── SectionCardsGrid.razor.css     # Section cards grid layout
    └── ContentItemsGrid.razor.css     # Content items grid layout
```

#### How CSS is Loaded

**Development/Test Mode** (App.razor):

- Individual CSS files loaded separately for easy debugging
- Each file referenced directly via `<link>` tags
- No caching for instant updates during development

**Production Mode** (App.razor):

- All global CSS bundled into `bundle.css` (generated by WebOptimizer in Program.cs)
- Minified and optimized for performance
- Aggressive caching for faster load times

**Component-Scoped CSS** (All modes):

- Automatically bundled by Blazor into `TechHub.Web.styles.css`
- Loaded via `@Assets["TechHub.Web.styles.css"]` in App.razor
- Styles scoped to components using unique `b-{hash}` attributes

#### CSS Bundle Configuration

CSS files are defined once in `TechHub.Web.Configuration.CssFiles.All` and referenced by both App.razor and Program.cs.

**App.razor** - Uses CssFiles array in Development, bundle in Production:

```html
@if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    <!-- Development: Individual files for debugging -->
    @foreach (var cssFile in TechHub.Web.Configuration.CssFiles.All)
    {
        <link rel="stylesheet" href="@cssFile" />
    }
}
else
{
    <!-- Production: Bundled and minified -->
    <link rel="stylesheet" href="css/bundle.css" />
}
<!-- Component-scoped styles (all modes) -->
<link rel="stylesheet" href="@Assets["TechHub.Web.styles.css"]" />
```

**Program.cs** - WebOptimizer bundle uses same CssFiles array:

```csharp
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddWebOptimizer(pipeline =>
    {
        pipeline.AddCssBundle("/css/bundle.css", CssFiles.All);
    });
}
```

**CSS Bundling Pattern**:

- CSS files defined ONCE in `Configuration/CssFiles.cs`
- `AddWebOptimizer()` in Program.cs references `CssFiles.All`
- App.razor loops through `CssFiles.All` in Development
- Bundle path MUST match App.razor `<link>` reference

**See**: [Configuration/CssFiles.cs](Configuration/CssFiles.cs) for the single source of truth

**CRITICAL**: Add new CSS files to `CssFiles.All` array - App.razor and Program.cs automatically stay in sync.

#### When to Use Global vs Component-Scoped CSS

**🚨 CRITICAL CSS PLACEMENT RULE**:

- **Component-scoped CSS** (`.razor.css` files) for **page-specific** styles
- **Global CSS** (`wwwroot/css/`) for **reusable** styles across multiple components

**Use Component-Scoped CSS** (`.razor.css`) when:

- ✅ **Page-specific layouts** - Unique to one page (Home.razor.css, Section.razor.css, About.razor.css)
- ✅ **Component-specific styling** - Unique to that component (SectionCard.razor.css, PageHeader.razor.css)
- ✅ **One-off styles** - Will NOT be reused elsewhere
- ✅ **Style isolation** - Prevent accidental conflicts with other components
- ✅ **Component ownership** - Styles that logically belong to the component

**Use Global CSS** (`wwwroot/css/`) when:

- ✅ **Reusable design system components** - Cards, buttons, forms used across multiple pages
- ✅ **Site-wide styles** - Header, footer, navigation
- ✅ **Design tokens** - Colors, typography, spacing (design-tokens.css)
- ✅ **Multiple components use the same styles** - Any style repeated in 2+ components belongs in global CSS

**CSS File Placement Examples**:

**Component-Scoped CSS** (`.razor.css` files):

```text
Components/
├── Layout/MainLayout.razor.css        # Main layout-specific styles (error banner)
├── Pages/Home.razor.css               # Home page-specific styles (popular tags container)
├── Pages/Section.razor.css            # Section page skeleton loading states
├── Pages/SectionCollection.razor.css  # Collection page skeleton loading states
├── Pages/ContentItem.razor.css        # Content detail page (article tags, nav buttons)
├── Pages/About.razor.css              # About page team grid (unique to about page)
├── Shared/PageHeader.razor.css        # Header banner styles (unique to this component)
├── SectionCard.razor.css              # Section card styling (unique to this component)
└── ContentItemCard.razor.css          # Content item card styling (unique to this component)

Note: Most sidebar components use global styles from sidebar.css, but SidebarToc and SidebarTagCloud
      have component-scoped .razor.css files for their specific interactive patterns
```

**Global CSS** (wwwroot/css/):

```text
wwwroot/css/
├── design-tokens.css                  # Colors, typography, spacing (used everywhere)
├── base.css                           # Reset, typography, links, utilities, forms (used everywhere)
├── page-container.css                 # Page layout containers (.page-with-sidebar, .page-without-sidebar)
├── sidebar.css                        # Sidebar component styles (used on multiple pages)
├── loading.css                        # Skeleton loading states (used on multiple pages)
├── nav-helpers.css                    # Navigation helpers (back to top, back to previous)
├── tag-dropdown.css                   # Tag dropdown component
├── date-slider.css                    # Date range slider component
└── article.css                        # Article content styling
```

**🚨 CRITICAL RULE**: Shared layout classes (`.page-with-sidebar`, `.page-without-sidebar`) MUST be in global CSS (`page-container.css`), NEVER in component-scoped CSS, even if currently used by only one page. These are structural classes that define the fundamental page architecture.

**Decision Tree**:

```text
Is this style specific to ONE component/page?
├─ YES → Component-scoped CSS (.razor.css file)
└─ NO → Is it used in 2+ components?
    ├─ YES → Global CSS (wwwroot/css/)
    └─ NO → Component-scoped CSS (.razor.css file)
```

**Benefits of This Approach**:

- **Design system consistency** - Global CSS ensures consistent look and feel
- **Style isolation** - Component-scoped CSS prevents accidental conflicts
- **Performance** - Bundling in production reduces HTTP requests
- **Developer experience** - Individual files in dev mode for easy debugging
- **Automatic optimization** - Blazor handles component CSS bundling and scoping

### Styling Rules & Best Practices

#### Design Tokens - Single Source of Truth

**🚨 CRITICAL**: ALL colors, spacing, typography, and other design values MUST be defined in `wwwroot/css/design-tokens.css`. This is the single source of truth for the design system.

**File Location**: [wwwroot/css/design-tokens.css](wwwroot/css/design-tokens.css)

**Core Principle**: Never hardcode design values. Always use CSS custom properties (design tokens).

**✅ DO - Use design tokens**:

```css
/* ✅ CORRECT - Use design tokens */
.card {
    background: var(--color-bg-default);
    border: 1px solid var(--color-purple-border-subtle);
    padding: var(--spacing-3);
    border-radius: var(--radius-md);
    color: var(--color-text-primary);
}

.button:hover {
    background: var(--color-purple-medium);
    box-shadow: var(--shadow-md);
}
```

**🚫 DON'T - Hardcode values**:

```css
/* ❌ WRONG - Hardcoded hex colors */
.card {
    background: #161b22;
    border: 1px solid rgba(189, 147, 249, 0.2);
    padding: 24px;
    border-radius: 8px;
    color: #c9d1d9;
}

/* ❌ WRONG - Hardcoded rgba colors */
.button:hover {
    background: rgba(157, 114, 217, 1);
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.4);
}
```

**Adding New Design Tokens**:

If you need a new color, spacing, or other design value:

1. **Add to design-tokens.css** - Define the token in the appropriate section
2. **Use semantic naming** - Name describes purpose, not value (e.g., `--color-bg-elevated`, not `--color-gray-800`)
3. **Document usage** - Add comment explaining when to use it
4. **Check for duplicates** - Verify similar tokens don't already exist

**Example - Adding a new color**:

```css
/* In design-tokens.css */

/* Border Colors */
--color-border-default: #30363d;              /* Default borders */
--color-border-emphasis: #6e7681;             /* Emphasized borders */
--color-border-interactive: var(--color-purple-medium); /* NEW - Interactive element borders */
```

**Design Token Categories**:

- **Colors** - Backgrounds, text, borders, overlays, accents
- **Typography** - Font families, sizes, weights, line heights
- **Spacing** - Padding, margins, gaps (8px grid system: 0.5rem, 1rem, 1.5rem, 2rem, etc.)
- **Layout** - Breakpoints, container widths, z-index scale
- **Effects** - Shadows, transitions, border radius
- **Interactive** - Focus indicators, touch targets

**Why This Rule Exists**:

- **Consistency** - Ensures design system is applied uniformly
- **Maintainability** - Change once, updates everywhere
- **Theming** - Easy to create dark/light themes or rebrand
- **Performance** - CSS custom properties are optimized by browsers
- **Collaboration** - Designers and developers speak same language
- **Accessibility** - Centralized contrast ratios and spacing ensure WCAG compliance

**Common Violations to Avoid**:

```css
/* ❌ WRONG - Inline rgba/hex in hover states */
.element:hover {
    background: rgba(0, 0, 0, 0.3);  /* Define as --color-overlay-hover instead */
}

/* ❌ WRONG - Magic numbers for spacing */
.container {
    padding: 23px;  /* Use --spacing-3 (24px / 1.5rem) instead */
    margin-bottom: 17px;  /* Use --spacing-2 (16px / 1rem) instead */
}

/* ❌ WRONG - Random shadow values */
.card {
    box-shadow: 0 3px 9px rgba(0, 0, 0, 0.35);  /* Use --shadow-md instead */
}
```

**Enforcement**:

- Use `get_errors` tool after editing CSS files to check for hardcoded values
- Code review should flag any hex codes or rgba() not in design-tokens.css
- Linting rules should catch hardcoded values (if configured)

#### Hover Effects - NO Position/Size Changes

**🚨 CRITICAL**: Hover effects MUST NOT change element size or position. This prevents layout shifts and maintains a stable, professional user experience.

**✅ ALLOWED in hover effects**:

- `color` / `background-color` / `border-color` changes
- `opacity` changes
- `box-shadow` changes
- `filter` effects (brightness, blur, etc.)
- `rotate()` transforms (e.g., dropdown arrows)

**🚫 FORBIDDEN in hover effects**:

- `translateY()` / `translateX()` - Moves elements
- `scale()` - Changes element size
- ANY transform that changes visual position or size

**✅ ALLOWED transforms (non-hover)**:

- Animation transforms (modals enter/exit, mobile nav slide-in)
- Centering transforms (e.g., `translate(-50%, -50%)` for absolute positioning)
- Active/press states IF not combined with size/position changes

**Examples**:

```css
/* ✅ CORRECT - Color and shadow changes only */
.card:hover {
    box-shadow: var(--shadow-lg);
    border-color: var(--color-purple-medium);
}

.button:hover {
    background: var(--color-purple-vibrant);
    color: var(--color-text-on-emphasis);
}

/* ✅ CORRECT - Rotate is allowed (doesn't move element) */
.dropdown-arrow {
    transition: transform 0.2s ease;
}
.dropdown-arrow.open {
    transform: rotate(180deg);
}

/* ✅ CORRECT - Animation transform for modal entrance */
.modal-content {
    transform: translateY(30px) scale(0.95);
    transition: transform 0.3s ease;
}
.modal-content.visible {
    transform: translateY(0) scale(1);
}

/* 🚫 WRONG - translateY moves the element on hover */
.card:hover {
    transform: translateY(-4px);
    box-shadow: var(--shadow-lg);
}

/* 🚫 WRONG - scale changes element size on hover */
.button:hover {
    transform: scale(1.1);
}
```

**Why This Rule Exists**:

- **Accessibility** - Size/position changes on hover are disorienting
- **Touch devices** - No hover state, so layout should be stable
- **Performance** - Color/opacity changes are GPU-accelerated, position changes trigger reflow
- **Professional UX** - Subtle color/shadow changes feel polished, bouncing elements feel amateurish

#### Image Handling by Convention

**🚨 CRITICAL**: Images are resolved by naming convention, NOT stored in API models or configuration.

**Section Background Images**:

Section background images use a two-tier system:

1. **Thumbnails** (400x140px) for SectionCards - Fast page loads
2. **Full-size** images for SectionBanner - High quality hero images

**File Structure**:

```text
wwwroot/images/
├── section-backgrounds/         # Full-size images for banners (all formats)
│   ├── ai.jxl                  # JPEG XL (best compression, modern browsers)
│   ├── ai.webp                 # WebP (good compression, wide support)
│   ├── ai.jpg                  # JPEG (fallback, universal support)
│   ├── github-copilot.jxl
│   ├── github-copilot.webp
│   ├── github-copilot.jpg
│   └── ... (all sections in all three formats)
└── section-thumbnails/          # Thumbnails for cards (all formats)
    ├── ai.jxl                  # 400x140 (optimized)
    ├── ai.webp
    ├── ai.jpg
    └── ... (all sections in all three formats)
```

**CSS Classes**:

Images are applied via CSS background-image properties in component-scoped stylesheets. The CSS references WebP format, but browsers automatically select the best format when using the `<picture>` element with multiple sources.

**Note**: For CSS backgrounds, we use single format (WebP with JPG fallback in CSS). For `<img>` tags requiring multi-format support, use the ResponsiveImage component.

**SectionCard.razor.css** (thumbnails):

```css
.section-bg-ai { background-image: url('/images/section-thumbnails/ai.webp'); }
.section-bg-github-copilot { background-image: url('/images/section-thumbnails/github-copilot.webp'); }
.section-bg-azure { background-image: url('/images/section-thumbnails/azure.webp'); }
/* ... etc */
```

**SectionBanner.razor.css** (full-size):

```css
.section-banner-bg-ai { background-image: url('/images/section-backgrounds/ai.webp'); }
.section-banner-bg-github-copilot { background-image: url('/images/section-backgrounds/github-copilot.webp'); }
.section-banner-bg-azure { background-image: url('/images/section-backgrounds/azure.webp'); }
/* ... etc */
```

**ResponsiveImage Component** (for `<img>` tags with format fallbacks):

For inline images that need JPEG XL/WebP/JPG format fallbacks, use the ResponsiveImage component:

```razor
<!-- Standard image with format fallbacks -->
<ResponsiveImage ImagePath="/images/about/author.webp" 
                 Alt="Author photo" 
                 Loading="lazy" />

<!-- Above-the-fold image (eager loading) -->
<ResponsiveImage ImagePath="/images/hero.webp" 
                 Alt="Hero image" 
                 Loading="eager" />
```

The component automatically generates `<picture>` elements with JPEG XL, WebP, and JPEG sources for optimal format selection.

**Component Usage**:

```html
<!-- SectionCard.razor -->
<div class="section-card-header section-bg-@Section.Name">
  <!-- Content -->
</div>

<!-- SectionBanner.razor -->
<div class="section-banner section-banner-bg-@Section.Name">
  <!-- Content -->
</div>
```

**Adding a New Section**:

1. Create images: `{section-name}.webp` in both directories
2. Add CSS classes to both `.razor.css` files
3. ✅ NO API changes needed
4. ✅ NO configuration changes needed

**Benefits**:

- **Performance** - Thumbnails for cards, full-size for banners
- **Simplicity** - Images resolved purely by naming convention
- **Maintainability** - Add section = add images + CSS classes
- **No API dependency** - Frontend fully controls image presentation

**🚫 NEVER**:

- Store image paths in API models or DTOs
- Store image paths in appsettings.json
- Use inline `style="background-image: url(...)"`
- Mix thumbnail and full-size images (cards MUST use thumbnails)

#### CSS Organization Best Practices

**Component-Scoped vs Global Decision**:

✅ **Component-scoped** (`.razor.css`):

- Styles used ONLY by that component
- Layout specific to that page
- Component-specific hover states
- Even if only ONE component uses it currently

✅ **Global** (`wwwroot/css/`):

- Styles used by 2+ components
- Shared layout classes (`.page-with-sidebar`)
- Design system tokens (colors, spacing, typography)
- Reusable patterns (buttons, cards, tags)

**Class Naming Patterns**:

```css
/* Component-specific prefixes for clarity */
.section-card-container       /* SectionCard component */
.sidebar-tag-button           /* SidebarTagCloud component */
.nav-helper-btn               /* Navigation helpers */

/* Background image classes follow naming convention */
.section-bg-{name}            /* Card backgrounds (thumbnails) */
.section-banner-bg-{name}     /* Banner backgrounds (full-size) */
```

**File Organization**:

- Group related styles together (e.g., all sidebar components in one file)
- Use comments to mark sections clearly
- Keep files focused and cohesive
- Split when a file exceeds ~500 lines

**🚫 NEVER**:

- Put layout classes in component-scoped CSS
- Duplicate styles across files
- Use generic class names without component prefix
- Mix component logic with global patterns

### JavaScript Architecture

**CRITICAL PRINCIPLE**: Blazor handles most interactivity server-side. JavaScript is ONLY for:

1. **Browser-native features** Blazor can't access (scroll position, IntersectionObserver, history API)
2. **Enhanced navigation hooks** (Blazor `enhancedload` event for SPA-style page transitions)
3. **Third-party libraries** (Highlight.js, Mermaid) that require JavaScript execution

**JavaScript File Configuration**: See [Configuration/JsFiles.cs](Configuration/JsFiles.cs)

#### JavaScript Loading Strategies

| Loading Type          | Use When                | Example                                  | How                                      |
| --------------------- | ----------------------- | ---------------------------------------- | ---------------------------------------- |
| **Static**            | Every page needs it     | `nav-helpers.js`                         | `<script src="@Assets[...]" defer>`      |
| **Dynamic ES Module** | Only some pages need it | `toc-scroll-spy.js`, `custom-pages.js`   | `import('./js/file.js')` via ImportMap   |
| **External CDN**      | Third-party library     | Highlight.js, Mermaid                    | Dynamic `loadScript()` with SRI          |

#### Fingerprinting (Cache Busting)

**CRITICAL**: ALL local JavaScript files MUST use fingerprinted URLs for proper cache invalidation.

**Static scripts** - Use `@Assets["js/file.js"]`:

```html
<!-- ✅ CORRECT - Fingerprinted URL (cache busting works) -->
<script src="@Assets["js/nav-helpers.js"]" defer></script>

<!-- ❌ WRONG - Raw path (will NOT cache bust on updates) -->
<script src="js/nav-helpers.js" defer></script>
```

**Dynamic imports** - Use `import()` with ImportMap component:

```javascript
// ✅ CORRECT - ImportMap rewrites to fingerprinted path
await import('./js/toc-scroll-spy.js');

// ❌ WRONG - Bypasses fingerprinting
await loadScript('/js/file.js');
```

**How ImportMap Works**:

1. `<ImportMap />` in App.razor generates a `<script type="importmap">` block
2. Maps module specifiers to fingerprinted URLs (e.g., `./js/toc-scroll-spy.js` → `./js/toc-scroll-spy.abc123.js`)
3. Browser's `import()` uses the map to resolve fingerprinted paths
4. Result: Dynamic imports get proper cache busting

#### JavaScript Files Reference

**Files in `wwwroot/js/`**:

| File                 | Purpose                                     | Loading                                   | Format    |
| -------------------- | ------------------------------------------- | ----------------------------------------- | --------- |
| `nav-helpers.js`     | Back to top, back to previous buttons       | Static (every page)                       | IIFE      |
| `toc-scroll-spy.js`  | TOC scroll highlighting, history management | Dynamic (pages with TOC)                  | ES Module |
| `custom-pages.js`    | Collapsible sections for SDLC/DX pages      | Dynamic (pages with `[data-collapsible]`) | ES Module |

**Special file in `wwwroot/`**:

| File                            | Purpose                      | Loading                    |
| ------------------------------- | ---------------------------- | -------------------------- |
| `TechHub.Web.lib.module.js`     | Blazor lifecycle callbacks   | Auto-discovered by Blazor  |

**NEVER create**: Client-side filtering JavaScript. Tag filtering is 100% Blazor server-side (see `SidebarTagCloud.razor`).

#### External CDN Libraries

External libraries (Highlight.js, Mermaid) are loaded from CDNs for performance.
**All versions and SRI hashes are centralized** in [Configuration/CdnLibraries.cs](Configuration/CdnLibraries.cs).

**To update a CDN library version**:

1. Update the version in `CdnLibraries.cs` (e.g., `HighlightJs.Version`)
2. Generate new SRI hash from <https://www.srihash.org/>
3. Update the integrity hash in `CdnLibraries.cs`
4. Test locally to verify the library loads correctly

**Current libraries**:

- **Highlight.js**: Syntax highlighting for code blocks
- **Mermaid**: Diagram rendering (flowcharts, sequence diagrams, etc.)

#### Adding New JavaScript Files

1. **Add file** to `wwwroot/js/`
2. **Update** `Configuration/JsFiles.cs` for documentation
3. **Load correctly**:
   - Static (every page): Add `<script src="@Assets[\"js/file.js\"]" defer>` to App.razor
   - Dynamic (conditional): Add `await import('./js/file.js')` in the module script block
4. **Document** purpose in this file under "JavaScript Files Reference"

### Page Structure and Semantic HTML

**🚨 CRITICAL SEMANTIC HTML RULES**:

- **ONE `<main>` per page** - Main content landmark (WAI-ARIA requirement)
- **`<aside>` INSIDE `<main>`** when sidebar content is contextually related to main content (e.g., table of contents, related pages)
- **`<article>` for self-contained content** - Blog posts, documentation pages, content items
- **NO nested `<main>` elements** - Only one main landmark allowed per page

#### Page Layout Patterns

**Two standardized semantic structures**:

**1. Pages with Sidebar** (Section, SectionCollection, ContentItem, Custom Pages):

```razor
<Header SectionName="..." />

<main class="page-with-sidebar">
    <aside class="sidebar">
        <!-- Sidebar components: TOC, navigation, RSS links, tags -->
    </aside>
    
    <!-- For article content (blog posts, documentation): -->
    <article>
        <!-- Self-contained article content -->
    </article>
    
    <!-- OR for section/list content (grids, listings): -->
    <section>
        <!-- ContentItemsGrid, ContentItemDetail, etc. -->
    </section>
</main>
```

**2. Pages without Sidebar** (About, Error, NotFound):

```razor
<Header SectionName="..." />

<main class="page-without-sidebar">
    <!-- Centered content, no sidebar -->
</main>
```

**CSS Classes** (defined in `page-container.css`):

- **`.page-with-sidebar`** - Applied to `<main>` for two-column grid layout (300px sidebar + 1fr content)
- **`.page-without-sidebar`** - Applied to `<main>` for single-column centered layout
- **`.article-body`** - Applied to content container within `<article>` for typography styling (headings, code blocks, tables)

**Why `<aside>` is INSIDE `<main>`**:

Per WAI-ARIA best practices, complementary content (`<aside>`) that's directly related to the main content (like a table of contents or related pages navigation) should be nested within the `<main>` landmark. This semantic relationship helps assistive technologies understand the page structure.

### Sidebar Component Architecture

**🚨 CRITICAL RESPONSIBILITY RULE**: Pages define layout structure with `<aside class="sidebar">`, sidebar components only render their content.

**Pattern**: Composition-based sidebar design where pages control layout and components provide functionality.

#### Sidebar Layout Responsibility

**Pages** (`Section.razor`, `Home.razor`, `ContentItem.razor`):

- ✅ Use `.page-with-sidebar` container class
- ✅ Define the `<aside class="sidebar">` container
- ✅ Determine which sidebar components to render
- ✅ Control component order and composition
- ✅ Pass required parameters to components

**Sidebar Components** (`SidebarCollectionNav.razor`, `SidebarRssLinks.razor`, `SidebarTagCloud.razor`):

- ✅ Render their specific content (navigation, RSS links, tag clouds)
- ✅ Use semantic HTML appropriate to their purpose (`<nav>`, `<div>`, etc.)
- ✅ Apply shared sidebar styles (`.sidebar-section`, `.sidebar-link-button`, etc.)
- ❌ DO NOT wrap themselves in `<aside class="sidebar">`
- ❌ DO NOT define their own container positioning (sticky, grid, etc.)
- ❌ DO NOT have component-scoped `.razor.css` files (styles are global in sidebar.css)

#### Example: Section Page with Sidebar

**Section.razor** (Page defines semantic layout):

```razor
<Header SectionName="..." />

<main class="page-with-sidebar">
    <PageHeader Section="@sectionData" />
    
    @* Page defines sidebar container *@
    <aside class="sidebar">
        @* Page composes sidebar components *@
        <SidebarCollectionNav Section="@sectionData" SelectedCollection="all" />
        <SidebarRssLinks Links="@(new[] { new SidebarRssLinks.RssLink(\"RSS Feed\", $\"{sectionData.Url}/feed.xml\") })\" />
    </aside>
    
    @* Use section for listing/grid content *@
    <section>
        <ContentItemsGrid ... />
    </section>
</main>
```

**ContentItem.razor** (Page with article content):

```razor
<Header SectionName="..." />

<main class="page-with-sidebar">
    <aside class="sidebar">
        <SidebarToc HtmlContent="@contentItem.RenderedHtml" />
        <SidebarRssLinks ... />
    </aside>
    
    @* Use article for self-contained content *@
    <article>
        <ContentItemDetail Item="@contentItem" />
    </article>
</main>
```

**SidebarCollectionNav.razor** (Component renders content only):

```razor
@* Uses <nav> since this contains site navigation *@
<nav aria-label="Section collections and pages">
    <div class="sidebar-section">
        <h2 class="sidebar-h2">Collections</h2>
        <ul class="sidebar-list">
            @* Navigation links *@
        </ul>
    </div>
</nav>
```

**SidebarRssLinks.razor** (Component renders content only):

```razor
@* Uses <div> since this is not navigation, just external links *@
<div class="sidebar-section">
    <h2 class="sidebar-h2">Subscribe</h2>
    <ul class="sidebar-list">
        @* RSS feed links *@
    </ul>
</div>
```

**SidebarTagCloud.razor** (Component renders tag cloud):

```razor
@* Fetches tag cloud from API and renders with size-based visualization *@
<div class="sidebar-section">
    <h2 class="sidebar-h2">Tags</h2>
    <div class="tags-cloud">
        @if (tagCloudItems != null)
        {
            @foreach (var tag in tagCloudItems)
            {
                <a href="@GenerateTagUrl(tag.NormalizedName)"
                   class="sidebar-tag tag-size-@tag.TagSize @GetSelectedClass(tag.NormalizedName)"
                   style="font-size: @(tag.FontSizePercent)%">
                    @tag.DisplayName
                </a>
            }
        }
    </div>
</div>
```

**Key Features**:

- Fetches tag cloud data from API (`/api/tagcloud/{sectionName}` or `/api/tagcloud`)
- Uses quantile-based sizing (5 size categories)
- Supports tag selection from URL query parameters
- Automatically refreshes on navigation
- Located in Components/ (root), not Components/Shared/
- `CssClass`: Additional CSS classes for container

#### Sidebar Component Semantic HTML

**Each component chooses its own semantic wrapper**:

- `<nav>` - For site navigation (collections, pages)
- `<div>` - For non-navigational content (RSS feeds, metadata)
- `<aside>` - ONLY used by pages for the outer sidebar container

**Why component-level semantics?**

- ✅ Multiple `<nav>` elements per page are valid and recommended
- ✅ Screen readers can distinguish between different navigation sections
- ✅ Components are self-contained and semantically correct
- ✅ Easy to reuse components without breaking HTML structure

#### Shared Sidebar Styles

**Global CSS** (`wwwroot/css/sidebar.css`):

- `.sidebar` - Container with sticky positioning (used by pages)
- `.sidebar-section` - Section wrapper
- `.sidebar-h2` - Section headings
- `.sidebar-list` - List styling
- `.sidebar-link-button` - Navigation buttons (NO `.sidebar` parent scoping!)
- `.sidebar-content-button` - Content item buttons
- `.sidebar-tag-button` - Interactive tag buttons
- `.sidebar-section-tag` - Display-only section tags
- `.sidebar-content-tag` - Display-only content tags

**🚨 CRITICAL**: Sidebar button styles are **NOT scoped** to `.sidebar` parent - they work anywhere the class is used. This allows components to use these styles without requiring a `.sidebar` ancestor.

#### Sidebar Components

**SidebarCollectionNav** - Section collections and custom pages navigation

- **Used in**: Section pages, SectionCollection pages, ContentItem pages
- **Parameters**: `Section` (required), `SelectedCollection` (optional, default "all")
- **Renders**: Collections list, custom pages list (NO RSS links)

**SidebarRssLinks** - RSS feed and subscription links

- **Used in**: All pages with sidebars (Home, Section, ContentItem)
- **Parameters**: `Links` (required) - array of RssLink records
- **Supports**: Multiple feeds, custom icons per link
- **Example**:

  ```razor
  <SidebarRssLinks Links="@(new[] {
      new SidebarRssLinks.RssLink("RSS Feed - All Content", "/all/feed.xml"),
      new SidebarRssLinks.RssLink("Newsletter", "https://example.com", GetNewsletterIcon())
  })" />
  ```

**SidebarTagCloud** - Interactive tag cloud for filtering content

- **Location**: Components/SidebarTagCloud.razor (root Components/, standard Blazor convention)
- **Used in**: Section pages, ContentItem pages
- **Purpose**: Displays tag cloud with size-based visualization and interactive filtering
- **Data Source**: Fetches tag cloud from API with quantile-based sizing
- **Implementation**: Uses code-behind pattern (SidebarTagCloud.razor.cs) for complex logic

#### Benefits of This Architecture

- ✅ **Clear separation of concerns** - Pages control structure, components provide functionality
- ✅ **Reusable components** - SidebarCollectionNav, SidebarRssLinks, SidebarTagCloud used across multiple pages
- ✅ **Flexible composition** - Pages can mix and match sidebar components
- ✅ **Semantic HTML** - Each component uses appropriate semantic tags
- ✅ **No duplicate code** - Shared styles and components reduce duplication
- ✅ **Easy to extend** - New sidebar components follow the same pattern

### Color Palette

**Source**: [css/design-tokens.css](wwwroot/css/design-tokens.css)

**Primary Colors**:

```css
/* CSS Variables (defined in wwwroot/css/design-tokens.css) */
--bright-purple: #bd93f9   /* Interactive accents only (links, buttons, hover) */
--medium-purple: #9d72d9   /* Muted accents */
--darker-purple: #7f56d9   /* Background accents (prefer this for backgrounds) */
--muted-purple: #6b4fb8    /* Even more muted for subtle backgrounds */

--bright-blue: #58a6ff     /* Links, hover states */
--light-blue: #79c0ff      /* Light accents */
--medium-blue: #1f6feb     /* PRIMARY COLOR - buttons, CTAs */

--dark-navy: #1a1a2e       /* Background, headers */
--darker-navy: #16213e     /* Darker backgrounds */
--darkest-navy: #0f0f23    /* Darkest backgrounds */

--primary-color: var(--medium-blue)    /* #1f6feb */
--secondary-color: #28a745             /* Bright green for success states */

--text-primary: #333       /* Soft dark gray for body text (easier on eyes) */
--background-primary: #fafafa  /* Off-white background for content areas */
```

### Typography

**Font Stack**:

```css
font-family: ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont,
             "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
```

**Font Sizes**: Use relative units (rem/em) for accessibility

### Spacing & Breakpoints

**Spacing**: Use consistent spacing units (0.25rem, 0.5rem, 1rem, 1.5rem, 2rem, 3rem)

**Breakpoints**:

- Mobile: < 768px
- Tablet: 768px - 1024px
- Desktop: > 1024px

### Layout Specifications

**Content Widths**:

- **Listing pages** (home, section pages): Full-width responsive grid (no max-width constraint)
- **Article detail pages**: 800px max-width for optimal reading
- **Grid columns**: 1 column (mobile), 2 columns (tablet), 3 columns (desktop)

**Article Detail Page Layout**:

```text
┌─────────────────────────────────────────────────┐
│  Navigation Bar (full width)                    │
├──────────────┬──────────────────────────────────┤
│  Sidebar     │  Main Content                    │
│  (25-30%)    │  (70-75%)                        │
│              │                                  │
│  - Quick Nav │  Article title                   │
│  - Author    │  Article content (800px max)     │
│  - Metadata  │                                  │
│  - Related   │                                  │
│  - Share     │                                  │
└──────────────┴──────────────────────────────────┘
```

**Mobile Layout** (< 768px):

```text
┌─────────────────────────────────────────────────┐
│  Navigation Bar                                  │
├─────────────────────────────────────────────────┤
│  Metadata (date, tags, sections)                │
│  Quick Navigation (TOC)                          │
├─────────────────────────────────────────────────┤
│  Article title                                   │
│  Article content                                 │
├─────────────────────────────────────────────────┤
│  Author information                              │
│  Related articles                                │
│  Social share / back to section                  │
└─────────────────────────────────────────────────┘
```

## Component Patterns

### Razor Variable Naming Conflicts

**🚨 CRITICAL**: Certain variable names conflict with Razor directives and MUST be avoided in component code.

**The Problem**: Razor uses `@section` as a directive for defining named content sections in layouts. Using `section` as a variable name causes compilation errors because the Razor parser treats `@section.Property` as an invalid directive.

**Conflicting Variable Names**:

```razor
@code {
    // ❌ NEVER DO THIS - Conflicts with @section directive
    private SectionDto section;
    
    // ERROR: The 'section' directive must appear at the start of the line
    <PageTitle>@section.Title - Tech Hub</PageTitle>
}
```

**Recommended Alternatives** (for consistency across codebase):

| Avoid     | Use Instead      | Context                                                |
|-----------|------------------|--------------------------------------------------------|
| `section` | `sectionData`    | SectionDto objects in pages/components                 |
| `section` | `currentSection` | When emphasizing current vs. other sections            |
| `code`    | `codeBlock`      | When working with code snippets (conflicts with @code) |
| `page`    | `pageData`       | When working with page metadata (conflicts with @page) |
| `layout`  | `layoutData`     | When working with layout data (conflicts with @layout) |

**🚨 CRITICAL REMINDER**: Always use `sectionData` (not `section`) throughout your component code, including when passing parameters to other components. Using `@section` will cause undefined variable errors because `section` doesn't exist - only `sectionData` does.

**Correct Pattern**:

```razor
@page "/{sectionName}"
@using TechHub.Core.DTOs

@if (sectionData != null)
{
    <PageTitle>@sectionData.Title - Tech Hub</PageTitle>
    
    @* ✅ CORRECT - Pass sectionData to Header component *@
    <Header Section="@sectionData" />
    
    <div class="page-with-sidebar">
        <aside class="sidebar">
            @* ✅ CORRECT - Pass sectionData to sidebar components *@
            <SidebarCollectionNav Section="@sectionData" SelectedCollection="all" />
        </aside>
        
        <section>
            <ContentItemsGrid SectionName="@sectionData.Name" />
        </section>
    </div>
}

@code {
    [Parameter]
    public string SectionName { get; set; } = null!;
    
    // ✅ CORRECT - Use 'sectionData' to avoid @section directive conflict
    private SectionDto? sectionData;
    
    protected override async Task OnInitializedAsync()
    {
        sectionData = await ApiClient.GetSectionAsync(SectionName);
    }
}
```

**Why This Matters**:

- **Compilation errors** - Code won't build if variable names conflict with directives
- **Consistency** - Using standard alternatives makes code easier to understand
- **Searchability** - Consistent naming makes it easier to find related code
- **Maintenance** - Reduces confusion for developers working across multiple files

**Other Razor Directives to Avoid as Variable Names**:

- `@page` - Defines route templates
- `@layout` - Specifies layout for page
- `@code` - Defines component code block
- `@inject` - Dependency injection
- `@implements` - Interface implementation
- `@inherits` - Base class inheritance
- `@attribute` - Component attributes
- `@typeparam` - Generic type parameters

**Best Practice**: When working with DTO objects in Blazor components, append `Data` to the variable name (e.g., `sectionData`, `contentData`, `itemData`) to avoid conflicts and improve clarity.

### Client-Side Navigation Without Re-Renders

**Pattern**: Use JavaScript History API for URL updates without triggering Blazor navigation

**When to Use**: When you need to update the URL for a state change but don't want the entire component to re-render (e.g., switching tabs/collections within the same page).

**Example**: Section page with collection navigation

```razor
@page "/{sectionName}"
@page "/{sectionName}/{collectionName}"
@inject IJSRuntime JS
@implements IAsyncDisposable

<nav>
    @foreach (var collection in collections)
    {
        <button @onclick="() => SelectCollection(collection.Name)">
            @collection.Title
        </button>
    }
</nav>

<div class="content @(isLoadingContent ? "loading" : "")">
    @* Only this area updates *@
    <ContentList Items="@contentItems" />
</div>

@code {
    [Parameter]
    public string SectionName { get; set; } = null!;
    
    [Parameter]
    public string? CollectionName { get; set; }
    
    private string selectedCollection = string.Empty;
    private bool isLoadingContent = false;
    
    // Use JavaScript history API to update URL without Blazor navigation
    private async void SelectCollection(string collectionName)
    {
        // Update URL without triggering OnParametersSetAsync
        await JS.InvokeVoidAsync("history.pushState", null, "", 
            $"/{SectionName}/{collectionName}");
        
        // Update local state
        selectedCollection = collectionName;
        
        // Load new content (only this executes, not full component re-render)
        await LoadCollectionContent();
    }
    
    protected override async Task OnParametersSetAsync()
    {
        // This handles initial load and browser back/forward
        // NOT called when using history.pushState
        if (CollectionName != selectedCollection)
        {
            selectedCollection = CollectionName ?? "all";
            await LoadCollectionContent();
        }
    }
    
    private async Task LoadCollectionContent()
    {
        isLoadingContent = true;
        StateHasChanged(); // Trigger re-render for loading state
        
        try
        {
            contentItems = await ApiClient.GetContentAsync(SectionName, selectedCollection);
        }
        finally
        {
            isLoadingContent = false;
            StateHasChanged(); // Trigger re-render with new content
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        // Cleanup resources
    }
}
```

**CSS for Smooth Transitions**:

```css
.content {
    opacity: 1;
    transition: opacity 150ms ease-in-out;
}

.content.loading {
    opacity: 0.5;
    pointer-events: none; /* Prevent clicks during loading */
}
```

**Key Points**:

- **`history.pushState`** updates URL without triggering Blazor routing
- **`OnParametersSetAsync`** still handles browser back/forward buttons
- **`StateHasChanged()`** manually triggers re-render of content area only
- **CSS transitions** provide smooth visual feedback
- **Dispose pattern** ensures proper cleanup

**When NOT to Use**:

- Cross-page navigation (use `NavigationManager.NavigateTo` instead)
- When you need full page reload
- When SEO requires distinct page loads

### Skeleton Loading States

**Critical UX Principle**: Skeleton loading states provide **immediate visual feedback** that navigation completed successfully and content is being loaded.

**When to Use Skeletons**:

- ✅ **Content pages** (Section.razor, article detail pages) - Content loading can take time; users need immediate feedback
- ✅ **API-dependent components** - Show skeleton while waiting for potentially slow API responses
- ✅ **Heavy data pages** - Pages with many items, complex filtering, or large payloads

**When NOT to Use Skeletons**:

- ❌ **Home page** - Lightweight section data loads quickly; no skeleton needed
- ❌ **Static pages** - Pages without API calls or dynamic data
- ❌ **Fast-loading components** - If data loads in < 100ms, skeletons create visual flicker

**Why Skeletons Matter for Content Pages**:

1. **Immediate feedback** - Users know navigation completed and page is loading
2. **Page structure visible** - Shows layout (header, nav, content areas) immediately
3. **Better perceived performance** - Seeing structure loading feels faster than blank page
4. **Prevents layout shift** - Skeleton reserves space for content, preventing jumps

**Example**: Section.razor shows skeleton for header/nav while content loads (content can be slow due to API calls fetching many items).

### Skeleton Layout Architecture

**Pattern**: Three independent async components with stable CSS Grid positioning and skeleton placeholders

**When to Use**: When page areas load at different speeds and you want to prevent layout shift during loading

**Problem Solved**: Traditional layouts where child components load sequentially cause jarring visual shifts and poor UX. Users see empty space, then content appears causing the page to jump.

**Solution**: CSS Grid layout with three independently-loading components that maintain stable positions. Each component shows skeleton placeholders while loading.

**Example**: Section page with header, navigation, and content areas

**Parent Component (Section.razor)**:

```razor
@page "/{sectionName}"
@page "/{sectionName}/{collectionName}"
@using TechHub.Core.DTOs
@using TechHub.Web.Services
@inject TechHubApiClient ApiClient
@rendermode InteractiveServer

<PageTitle>@SectionName - Tech Hub</PageTitle>

<!-- Page container with sidebar layout -->
<main class="page-with-sidebar">
    @* Header loads independently *@
    <PageHeader Section="@section" />
    
    @* Sidebar container *@
    <aside class="sidebar">
        <SidebarCollectionNav Section="@section" 
                             SelectedCollection="@selectedCollection" />
        <SidebarRssLinks Links="@(new[] { new SidebarRssLinks.RssLink(\"RSS Feed\", $\"{section.Url}/feed.xml\") })\" />
    </aside>
    
    @* Main content area *@
    <section>
        <ContentItemsGrid ... />
    </section>
</main>

@code {
    [Parameter]
    public string SectionName { get; set; } = null!;

    [Parameter]
    public string? CollectionName { get; set; }

    private string selectedCollection = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        selectedCollection = CollectionName ?? "all";
        await LoadSectionMetadata();
    }

    private async Task HandleCollectionChange(string collectionName)
    {
        selectedCollection = collectionName;
        await JS.InvokeVoidAsync("history.pushState", null, "", 
            $"/{SectionName}/{collectionName}");
    }
}
```

**Page Layout CSS (page-container.css - global shared CSS)**:

```css
/* Standard page container with sidebar */
.page-with-sidebar {
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: var(--spacing-5);
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 var(--spacing-3) var(--spacing-6) var(--spacing-3);
}

/* Prevent grid blowout on content elements */
.page-with-sidebar > :is(article, section) {
  min-width: 0;
}

/* Standard page container without sidebar */
.page-without-sidebar {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 var(--spacing-3) var(--spacing-6) var(--spacing-3);
}

/* Responsive - Single column on smaller screens */
@media (max-width: 1024px) {
  .page-with-sidebar {
    grid-template-columns: 1fr;
    gap: var(--spacing-4);
  }
}

@media (max-width: 768px) {
  .page-with-sidebar {
    padding: 0 var(--spacing-2) var(--spacing-4) var(--spacing-2);
  }

  .page-without-sidebar {
    padding: 0 var(--spacing-2) var(--spacing-4) var(--spacing-2);
  }
}
```

**Component-Scoped Loading States (Section.razor.css)**:

```css
        90deg,
        rgba(255, 255, 255, 0.05) 0%,
        rgba(255, 255, 255, 0.1) 50%,
        rgba(255, 255, 255, 0.05) 100%
    );
    background-size: 1000px 100%;
    animation: shimmer 2s infinite;
    border-radius: 4px;
}

.skeleton-header {
    height: 120px;
    margin-bottom: 1rem;
}

.skeleton-nav-item {
    height: 40px;
    margin-bottom: 0.5rem;
}

.skeleton-card {
    height: 200px;
    margin-bottom: 1rem;
}
```

**Child Component Pattern (PageHeader.razor - in Shared/)**:

```razor
@using TechHub.Core.DTOs

@* Reusable banner component for all pages *@
<header class="section-header" style="@StyleAttribute">
    <div class="section-header-content">
        <div class="header-text-overlay">
            <h1 tabindex="-1">@DisplayTitle</h1>
            @if (!string.IsNullOrEmpty(DisplayDescription))
            {
                <p class="section-description">@DisplayDescription</p>
            }
        </div>
    </div>
</header>

@code {
    // For section pages - provide Section
    [Parameter]
    public SectionDto? Section { get; set; }
    
    // For static pages - provide Title, Description, BackgroundCssClass
    [Parameter]
    public string? Title { get; set; }
    
    [Parameter]
    public string? Description { get; set; }
    
    [Parameter]
    public string? BackgroundCssClass { get; set; }
    
    // Computed properties - prefer Section data if available
    private string DisplayTitle => Section?.Title ?? Title ?? string.Empty;
    private string? DisplayDescription => Section?.Description ?? Description;
    private string BackgroundClass => Section != null
        ? $"section-banner-bg-{Section.Name}"
        : BackgroundCssClass ?? "section-banner-bg-none";
}
```

**Key Architecture Principles**:

1. **CSS Grid Stability**: Grid areas have fixed positions that never change
2. **Independent Loading**: Each component loads its own data asynchronously
3. **Skeleton Placeholders**: Show loading state without layout shift
4. **@key Directive**: Force component re-render on navigation (e.g., `@key="@selectedCollection"`)
5. **EventCallback Communication**: Parent coordinates state (e.g., `OnCollectionChange`)

**Component Parameters**:

- **PageHeader**: `Section` (SectionDto) for section pages, or `Title`/`Description`/`BackgroundCssClass` for static pages
- **CollectionNav**: `SectionName` (string), `SelectedCollection` (string), `OnCollectionChange` (EventCallback)
- **CollectionContent**: `SectionName` (string), `CollectionName` (string)

**Testing Requirements**:

Test that page structure renders immediately even before API responses arrive. Use delayed `TaskCompletionSource` to simulate pending API calls. Verify skeleton layout structure exists with proper CSS classes. See [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md) for component testing patterns.

**Best Practices**:

- **Null Safety**: Always check if data is null before rendering
- **Loading States**: Use skeleton placeholders, not spinners (prevents layout shift)
- **Error Handling**: Show error message in same space as skeleton
- **Performance**: Each component loads independently (parallel HTTP requests)
- **SEO**: Initial server-side render shows skeleton structure

**When to Use This Pattern**:

- Multi-section pages with independent data sources
- Content that loads at different speeds
- When you want to prevent cumulative layout shift (CLS)
- Pages where parts can fail independently

**When NOT to Use**:

- Simple pages with single data source
- When all data loads from same endpoint
- Static pages without loading states

### Article Sidebar Component

**Pattern**: Sidebar with quick navigation, metadata, and related content

```razor
@* ArticleSidebar.razor *@
@inject ILogger<ArticleSidebar> Logger

<aside class="article-sidebar">
    @* Priority 1: Quick Navigation (Table of Contents) *@
    @if (Headings.Any())
    {
        <nav class="toc" aria-label="Table of Contents">
            <h3>Contents</h3>
            <ul>
                @foreach (var heading in Headings)
                {
                    <li><a href="#@heading.Id">@heading.Text</a></li>
                }
            </ul>
        </nav>
    }
    
    @* Priority 2: Author Information *@
    @if (!string.IsNullOrEmpty(Author))
    {
        <div class="author-info">
            <h3>Author</h3>
            <p>@Author</p>
        </div>
    }
    
    @* Priority 3: Article Metadata *@
    <div class="metadata">
        <h3>Details</h3>
        <time datetime="@PublishDate">@FormatDate(PublishDate)</time>
        @if (Tags.Any())
        {
            <div class="tags">
                @foreach (var tag in Tags)
                {
                    <span class="tag">@tag</span>
                }
            </div>
        }
    </div>
    
    @* Priority 4: Related Articles *@
    @if (RelatedArticles.Any())
    {
        <div class="related-articles">
            <h3>Related</h3>
            <ul>
                @foreach (var article in RelatedArticles)
                {
                    <li><a href="@article.Url">@article.Title</a></li>
                }
            </ul>
        </div>
    }
    
    @* Priority 5: Social Share / Back Link *@
    <div class="actions">
        <a href="@BackToSectionUrl" class="back-link">← Back to @SectionName</a>
    </div>
</aside>

@code {
    [Parameter, EditorRequired]
    public required List<Heading> Headings { get; set; }
    
    [Parameter]
    public string? Author { get; set; }
    
    [Parameter, EditorRequired]
    public required DateTime PublishDate { get; set; }
    
    [Parameter, EditorRequired]
    public required List<string> Tags { get; set; }
    
    [Parameter, EditorRequired]
    public required List<RelatedArticle> RelatedArticles { get; set; }
    
    [Parameter, EditorRequired]
    public required string BackToSectionUrl { get; set; }
    
    [Parameter, EditorRequired]
    public required string SectionName { get; set; }
    
    private string FormatDate(DateTime date)
    {
        // Format as "Month DD, YYYY"
        return date.ToString("MMMM dd, yyyy");
    }
}

public record Heading(string Id, string Text, int Level);
public record RelatedArticle(string Url, string Title);
```

### Basic Component Structure

```razor
@* SectionCard.razor *@
@inject ILogger<SectionCard> Logger

<div class="section-card">
    <h2>@Section.Title</h2>
    <p>@Section.Description</p>
</div>

@code {
    [Parameter, EditorRequired]
    public required SectionDto Section { get; set; }
}
```

### API Client Usage

**Pattern**: Inject TechHubApiClient and handle errors

```razor
@page "/sections/{id}"
@inject TechHubApiClient ApiClient
@inject ILogger<SectionPage> Logger

@if (section == null)
{
    <p>Loading...</p>
}
else if (errorMessage != null)
{
    <div class="error">@errorMessage</div>
}
else
{
    <h1>@section.Title</h1>
}

@code {
    [Parameter]
    public string Id { get; set; } = string.Empty;
    
    private SectionDto? section;
    private string? errorMessage;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            section = await ApiClient.GetSectionAsync(Id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load section {SectionId}", Id);
            errorMessage = "Unable to load section. Please try again.";
        }
    }
}
```

### RSS Feed Proxy Endpoints

**Purpose**: The Web frontend serves RSS feeds via proxy endpoints that call the secured API backend.

**Why Proxies?**: The API backend will be secured and not publicly accessible. User-facing RSS feeds are served from the Web frontend domain (`https://tech.hub.ms`).

**Pattern**: Minimal API endpoints in Program.cs that proxy to TechHubApiClient. See `Program.cs` for implementation.

**User-Facing URLs**:

- **Everything feed**: `https://tech.hub.ms/all/feed.xml`
- **Roundups only**: `https://tech.hub.ms/all/roundups/feed.xml`
- **Section feeds**: `https://tech.hub.ms/{sectionName}/feed.xml` (e.g., `/ai/feed.xml`, `/github-copilot/feed.xml`)

**Internal API Calls** (not publicly accessible):

- `/api/rss/all` - All content across all sections
- `/api/rss/collection/roundups` - Roundups collection
- `/api/rss/{sectionName}` - Section-specific content

**Content Type**: All feeds return `application/xml; charset=utf-8`

**Testing**: See [tests/TechHub.E2E.Tests/AGENTS.md](../../tests/TechHub.E2E.Tests/AGENTS.md) for RSS feed E2E tests

### Component with Background Image

**Pattern**: Use CSS classes for background images (defined in CSS with responsive image formats)

```razor
<div class="section-card section-bg-@Section.Name">
    <div class="section-card-overlay">
        <h2>@Section.Title</h2>
    </div>
</div>
```

**Why CSS Classes**: Background images use CSS classes (e.g., `section-bg-ai`, `section-banner-bg-ai`) which reference responsive image formats (WebP, JPEG XL) with proper fallbacks. This approach enables better performance through modern image formats while maintaining browser compatibility.

**CSS**:

```css
.section-card {
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    position: relative;
}

.section-card-overlay {
    background: rgba(26, 26, 46, 0.7); /* dark-navy with transparency */
    padding: 2rem;
}
```

### Date Formatting

**Pattern**: Use helper methods for relative dates ("Today", "Yesterday", "3 days ago", etc.) instead of absolute dates. Convert Unix epoch to `DateTimeOffset` and calculate difference from now.

See component code for implementation.

## File Structure

```text
src/TechHub.Web/
├── Components/              # Reusable components
│   ├── Layout/             # Layout components (MainLayout.razor, MainLayout.razor.css)
│   ├── Pages/              # Page components with @page directive (*.razor, *.razor.css)
│   ├── Shared/             # Shared components (PageHeader.razor.css, NavHeader.razor.css)
│   ├── SectionCard.razor   # Section display card
│   ├── ContentItemCard.razor # Content item display card
│   ├── Routes.razor        # Blazor router configuration
│   └── App.razor           # Application entry point
├── Services/               # Frontend services
│   └── TechHubApiClient.cs # Typed HTTP client for API
├── wwwroot/                # Static files
│   ├── css/                # Global CSS (design system)
│   │   ├── design-tokens.css
│   │   ├── base.css
│   │   ├── sidebar.css
│   │   ├── page-container.css
│   │   ├── nav-helpers.css
│   │   ├── loading.css
│   │   ├── tag-dropdown.css
│   │   ├── date-slider.css
│   │   └── article.css
│   └── images/             # Static images
│       └── section-backgrounds/ # Section header images
├── Program.cs              # Application configuration (CSS bundling)
└── appsettings.*.json      # Configuration files
```

**Component Organization**:

- **Layout/**: Application layout components
  - `MainLayout.razor` - Main application layout wrapper
  - `ReconnectModal.razor` - Server connection state UI (Interactive Server/Auto modes)
- **Pages/**: Routable pages (all have `@page` directive)
  - `Home.razor` - Homepage (`/`) - Shows all sections
  - `Section.razor` - Section page (`/{sectionName}`) - Shows all content in section
  - `SectionCollection.razor` - Collection page (`/{sectionName}/{collectionName}`) - Shows filtered content
  - `ContentItem.razor` - Detail page (`/{sectionName}/{collectionName}/{slug}`) - Shows single content item
  - `About.razor` - About page (`/about`)
  - `NotFound.razor` - 404 page
  - Custom pages - Feature-specific routable pages (GenAI courses, GitHub Copilot resources, etc.)
- **Components/ (root)**: Reusable non-routable components (standard Blazor convention)
  - **Navigation & Layout Components**:
    - `Header.razor` - Global site header wrapper
    - `NavHeader.razor` - Global site navigation header
    - `SectionBanner.razor` - Page banner with section background
    - `SubNav.razor` - Horizontal sub-navigation below page header
  - **Content Display Components**:
    - `SectionCard.razor` - Section display card (homepage)
    - `SectionCardsGrid.razor` - Grid of section cards (homepage)
    - `ContentItemCard.razor` - Content item display card (section/collection pages)
    - `ContentItemsGrid.razor` - Grid of content items with filtering
    - `ContentItemDetail.razor` - Full content rendering (detail page)
  - **Sidebar Components**:
    - `SidebarRssLinks.razor` - RSS subscription links
    - `SidebarTagCloud.razor` - Interactive tag cloud for filtering
    - `SidebarToc.razor` - Table of contents with scroll-spy
    - `SidebarPageInfo.razor` - Custom page metadata display
- **Framework Components** (must stay in root):
  - `Routes.razor` - Blazor router (framework requirement)
  - `App.razor` - Application entry point with global script loading (framework requirement)

**Why This Structure?**

This follows **Microsoft's official Blazor conventions** per ASP.NET Core documentation:

1. **Components/** (root) - Shared components used across the application
   - Per Microsoft: "Shared components are often placed at the root of the Components folder"

   - Navigation, content display, sidebar, and utility components all live here
   - No functional difference from Components/Shared/ - both are valid per Microsoft docs

2. **Components/Pages/** - Routable page components with `@page` directive
   - Per Microsoft: "Page components are usually placed in folders within the Components folder"
   - Each file represents a URL route

3. **Components/Layout/** - Application layout wrappers
   - Per Microsoft: "Layouts can be placed in the app's Shared or Layout folder"
   - MainLayout wraps all pages, ReconnectModal for server connection state

**Microsoft Guidance on Shared Components**:

> "Shared components are often placed at the root of the Components folder, while layout and page components are usually placed in folders within the Components folder."
>
> "However, layouts can be placed in any location accessible to the components that use it. For example, a layout can be placed in the same folder as the components that use it."

Source: [ASP.NET Core Blazor project structure](https://github.com/dotnet/aspnetcore.docs/blob/main/aspnetcore/blazor/project-structure.md) and [Blazor layouts](https://github.com/dotnet/aspnetcore.docs/blob/main/aspnetcore/blazor/components/layouts.md)

**Key Takeaway**: Both `Components/` (root) and `Components/Shared/` are valid locations for shared components. We chose the root location to align with Microsoft's documented pattern that "shared components are often placed at the root of the Components folder."

## Image Conventions

**Path Convention**: Use `/images/` prefix (NOT `/assets/`)

**Multi-Format Support**: All images provided in three formats for optimal performance:

- **JPEG XL (`.jxl`)**: Best compression, modern browsers (Chrome 109+, Edge 109+)
- **WebP (`.webp`)**: Good compression, wide browser support
- **JPEG (`.jpg`)**: Universal fallback, all browsers

**Section Backgrounds**: `/images/section-backgrounds/{section-name}.{format}`

**Examples**:

- `/images/section-backgrounds/ai.jxl` (best)
- `/images/section-backgrounds/ai.webp` (fallback)
- `/images/section-backgrounds/ai.jpg` (universal)

**Format Selection**:

- CSS backgrounds: Reference `.webp` (browsers use native format if supported)
- `<img>` tags: Use ResponsiveImage component for automatic `<picture>` generation with all formats

**Image Optimization**: Modern formats (JPEG XL, WebP) provide 30-50% better compression than JPEG while maintaining quality.

## Static Files & Browser Caching

**Implementation**: `StaticFilesCacheMiddleware` provides centralized cache control for all static files

**Middleware Architecture**:

- Placed **FIRST** in pipeline (before MapStaticAssets) to override built-in headers
- Uses `OnStarting` callback to set cache headers after response is prepared
- Detects fingerprinted files (e.g., `styles.abc123.css`) for immutable caching

**Cache Strategy**:

- **Fingerprinted assets**: Forever cache (`max-age=31536000,immutable`) - content change = new URL
- **Images/Fonts** (jpg, png, webp, jxl, svg, ico, woff, woff2, ttf, eot): 1 year cache (`max-age=31536000,immutable`)
- **CSS/JS** (non-fingerprinted): Short cache with revalidation (`max-age=3600,must-revalidate`)
- **Other files**: No cache headers set (browser defaults)

**Why This Works**:

- **`immutable`**: Tells browser the file will NEVER change at this URL
- **1 year cache**: Banner images, fonts, and other assets cached locally for maximum performance
- **No revalidation**: Browser uses cached version without asking server "has it changed?"
- **Fingerprinting detection**: Automatically recognizes versioned files for aggressive caching

**Benefits**:

- ✅ Banner images load instantly on subsequent page views (from disk cache)
- ✅ Zero HTTP requests for cached assets = faster page loads
- ✅ Reduces server bandwidth
- ✅ Better user experience on slow connections
- ✅ Automatic cache busting for fingerprinted assets

**Source**: See [StaticFilesCacheMiddleware.cs](Middleware/StaticFilesCacheMiddleware.cs) and configuration in [Program.cs](Program.cs)

## Testing Components

**See**: [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md) for bUnit testing patterns

**Test Pattern**:

- Arrange: Create test data (DTOs, models)
- Act: Render component with `RenderComponent<T>(parameters => ...)`
- Assert: Use `Find()`, `FindAll()` and FluentAssertions

## Common Patterns

### Error Boundary

```razor
<ErrorBoundary>
    <ChildContent>
        @* Component content *@
    </ChildContent>
    <ErrorContent Context="error">
        <div class="error">
            <p>An error occurred. Please refresh the page.</p>
        </div>
    </ErrorContent>
</ErrorBoundary>
```

### Loading States

```razor
@if (isLoading)
{
    <div class="loading">Loading...</div>
}
else if (data != null)
{
    @* Display data *@
}
```

### Responsive Grid

```css
.section-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 2rem;
    padding: 2rem;
}

/* Mobile */
@media (max-width: 768px) {
    .section-grid {
        grid-template-columns: 1fr;
        gap: 1rem;
        padding: 1rem;
    }
}
```

### Infinite Scroll Pagination

**Configuration**:

- **Items per batch**: 20 items
- **Prefetch trigger**: 80% scroll threshold (trigger when user is 80% down the page)
- **URL parameter preservation**: Maintain filters/search when loading more

**Pattern**:

```razor
@inject IJSRuntime JS

<div id="content-container">
    @foreach (var item in visibleItems)
    {
        <ContentItemCard Item="@item" Section="@section" />
    }
</div>

@if (hasMore)
{
    <div id="load-more-trigger" style="height: 1px;"></div>
}

@code {
    private List<ContentItemDto> visibleItems = new();
    private bool hasMore = true;
    private int currentPage = 1;
    private const int PageSize = 20;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Set up intersection observer at 80% threshold
            await JS.InvokeVoidAsync("setupInfiniteScroll", "load-more-trigger", 
                DotNetObjectReference.Create(this));
        }
    }
    
    [JSInvokable]
    public async Task LoadMore()
    {
        currentPage++;
        var nextBatch = await ApiClient.GetContentAsync(
            section: sectionName,
            page: currentPage,
            pageSize: PageSize);
        
        if (nextBatch.Count < PageSize)
            hasMore = false;
        
        visibleItems.AddRange(nextBatch);
        StateHasChanged();
    }
}
```

**JavaScript** (wwwroot/js/infinite-scroll.js):

```javascript
window.setupInfiniteScroll = (elementId, dotNetRef) => {
    const trigger = document.getElementById(elementId);
    const observer = new IntersectionObserver(async (entries) => {
        if (entries[0].isIntersecting) {
            await dotNetRef.invokeMethodAsync('LoadMore');
        }
    }, { threshold: 0.8 }); // 80% visible
    
    observer.observe(trigger);
};
```

### Conditional JavaScript Loading

**Pattern**: Only load heavy JavaScript libraries on pages that actually need them, not globally on every page.

**Problem**: Loading Highlight.js (~68KB + 8 language files), Mermaid diagrams, TOC scroll-spy, and custom page interactivity on every page significantly slows down initial page load for simple list/section pages that don't use them.

**Solution**: Global script loader in `App.razor` with element detection - loads libraries dynamically only when their target elements exist on the page.

#### Dynamic Script Loading

**Location**: [Components/App.razor](Components/App.razor)

**How It Works**:

1. **Element Detection**: On every page load and `enhancedload` event, checks for specific elements:
   - `pre code` → Load Highlight.js (syntax highlighting)
   - `.mermaid` → Load Mermaid (diagrams)
   - `[data-toc-scroll-spy]` → Initialize TOC scroll spy
   - `[data-collapsible]` → Load custom pages interactivity

2. **Dynamic Loading**: When elements are found, dynamically injects:
   - CSS `<link>` tags (for Highlight.js theme)
   - External `<script>` tags (for libraries)
   - ES6 module imports (for local scripts)

3. **One-Time Loading**: Tracks what's already loaded to avoid duplicate script injection

**Performance Benefits**:

- **Simple Pages** (Home, Section, Collection): ~0 extra JS (just element checks, <1ms)
- **Content Pages** (Handbook, Features, ContentItem): Libraries load only when needed
- **No Manual Parameters**: Pages don't need to declare what they use - automatic detection

**Example Flow**:

```javascript
// In App.razor - runs on every page load/navigation
async function loadScriptsForPage() {
    // 1. Check for code blocks
    if (document.querySelector('pre code')) {
        // Inject Highlight.js CSS + JS (only once)
        // Then call hljs.highlightAll()
    }
    
    // 2. Check for Mermaid diagrams
    if (document.querySelector('.mermaid')) {
        // Load Mermaid library (only once)
        // Then call mermaid.run()
    }
    
    // 3. Check for TOC
    if (document.querySelector('[data-toc-scroll-spy]')) {
        // Import toc-scroll-spy.js module
        // Then call initTocScrollSpy()
    }
    
    // 4. Check for collapsible sections
    if (document.querySelector('[data-collapsible]')) {
        // Load custom-pages.js (only once)
    }
}

        mermaid.initialize({ startOnLoad: true, theme: 'dark' });
    </script>
}

@if (LoadTocScrollSpy)
{
}
```

**Key Benefits**:

- ✅ **Automatic Detection** - No manual parameters needed on pages
- ✅ **Faster Initial Load** - Simple pages load zero extra JavaScript
- ✅ **Smart Loading** - Only loads libraries when elements exist
- ✅ **One-Time Injection** - Tracks loaded state to avoid duplicates
- ✅ **Clean Page Code** - Pages don't need script declarations
- ✅ **Progressive Enhancement** - Works on initial load and after navigation

### JavaScript Utilities

**See also**: [JavaScript Architecture](#javascript-architecture) for loading strategies, fingerprinting rules, and adding new files.

**Navigation Helpers** (wwwroot/js/nav-helpers.js):

Provides sticky bottom navigation buttons for improved user experience:

- **Back to Top**: Smooth scroll to top of page (appears after scrolling 300px)
- **Back to Previous**: Navigate to previous page in browser history

**Key Features**:

- Automatic show/hide based on scroll position (300px threshold)
- Blazor enhanced navigation support (pageshow event + MutationObserver)
- Proper cleanup and re-initialization after page navigation
- CSS fade-in/fade-out transitions

**Integration**: Automatically loaded in `App.razor`, no manual setup required.

**TOC Scroll-Spy** (wwwroot/js/toc-scroll-spy.js):

Automatically highlights table of contents links based on scroll position:

- **CRITICAL**: Uses `history.replaceState()` instead of `pushState()` to update URL hash
- This prevents polluting browser history with scroll positions
- Only actual TOC link clicks create history entries
- Enables clean "back to previous page" navigation

**Pattern**:

```javascript
// ❌ WRONG - Creates history entry for every scroll update
history.pushState(null, '', newUrl);

// ✅ CORRECT - Updates URL without creating history entry
history.replaceState(null, '', newUrl);
```

**Why This Matters**: When users navigate through anchors and scroll through content, only intentional navigation (clicking TOC links) should create history entries. Automatic scroll-spy updates should use `replaceState` so the back button takes users to the previous page, not the previous scroll position.

### Component Catalog Organization

**Layout Components**:

- `MainLayout.razor` - Primary application layout
- `NavMenu.razor` - Site navigation menu
- `NavHeader.razor` - Site header with logo and navigation
- `ReconnectModal.razor` - Blazor reconnection UI

**Page Components** (with `@page` directive):

- `Home.razor` - Homepage (`/`)
- `Section.razor` - Section index (`/{sectionName}`)
- `SectionCollection.razor` - Collection page (`/{sectionName}/{collectionName}`)
- `ContentItem.razor` - Detail page (`/{sectionName}/{collectionName}/{slug}`)
- `About.razor` - About page (`/about`)
- `NotFound.razor` - 404 page

**Shared Components**:

- `PageHeader.razor` - Universal section header banner
- `SidebarCollectionNav.razor` - Sidebar collection navigation
- `SidebarRssLinks.razor` - RSS feed links in sidebar
- `SidebarTagCloud.razor` - Interactive tag cloud with filtering (see [Tag Filtering Behavior](#tag-filtering-behavior) below)

**Content Components**:

- `SectionCard.razor` - Section display card (homepage)
- `SectionCardsGrid.razor` - Grid of section cards
- `ContentItemCard.razor` - Content item display card
- `ContentItemsGrid.razor` - Grid of content items with filtering
- `ContentItemDetail.razor` - Full content rendering

**Filter Components**:

- `SearchBox.razor` - Text search input
- `TagFilter.razor` - Tag selection filter
- `DateFilter.razor` - Date range filter

### Schema.org Structured Data

**Pattern**: Add JSON-LD structured data to content pages for SEO

```razor
@page "/{sectionName}/{collectionName}/{slug}"

<HeadContent>
    <script type="application/ld+json">
    {
        "@context": "https://schema.org",
        "@type": "Article",
        "headline": "@Item.Title",
        "description": "@Item.Description",
        "author": {
            "@type": "Person",
            "name": "@Item.Author"
        },
        "datePublished": "@GetIsoDate(Item.DateEpoch)",
        "publisher": {
            "@type": "Organization",
            "name": "Microsoft Tech Hub",
            "logo": {
                "@type": "ImageObject",
                "url": "https://tech.hub.ms/images/logo.png"
            }
        },
        "keywords": [@string.Join(", ", Item.Tags.Select(t => $"\"{t}\""))]
    }
    </script>
</HeadContent>

@code {
    private string GetIsoDate(long epochSeconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(epochSeconds)
            .ToString("yyyy-MM-ddTHH:mm:sszzz");
    }
}
```

### Render Mode Selection

**Criteria for choosing SSR vs WebAssembly**:

**Use SSR (Server-Side Rendering)** when:

- Content is static and doesn't change after initial render
- SEO is critical (search engines see complete HTML)
- Initial page load speed is priority
- No complex client-side interactivity needed

**Use WebAssembly (InteractiveWebAssembly)** when:

- Rich client-side interactivity required (filtering, search, infinite scroll)
- Reduced server load is important (processing moves to client)
- Real-time updates or complex UI state management
- Offline support or PWA features needed

**Hybrid Approach** (Recommended):

- Initial page render with SSR (fast load, SEO-friendly)
- Enhanced interactivity with WebAssembly for specific components
- Use `@rendermode InteractiveWebAssembly` on interactive components only

**Example**:

```razor
@* Page uses SSR by default *@
@page "/github-copilot"

<PageHeader Section="@section" />

@* Static content rendered server-side *@
<div class="section-description">
    @section.Description
</div>

@* Interactive filtering uses WebAssembly *@
<ContentItemsGrid Section="@section" @rendermode="InteractiveWebAssembly" />
```

### Custom Page Patterns

**GitHub Copilot Features Page**:

Features page with subscription tier filtering (Free, Business, Enterprise):

```razor
@page "/github-copilot/features"

<select @onchange="OnTierChanged">
    <option value="">All Features</option>
    <option value="free">Free</option>
    <option value="business">Business</option>
    <option value="enterprise">Enterprise</option>
</select>

@foreach (var feature in filteredFeatures)
{
    <div class="feature-card">
        <h3>@feature.Title</h3>
        <span class="tier-badge">@feature.Tier</span>
        @if (feature.GhesSupport)
        {
            <span class="ghes-badge">GHES Support</span>
        }
        @if (feature.ComingSoon)
        {
            <span class="coming-soon">Coming Soon</span>
        }
    </div>
}

@code {
    private string selectedTier = "";
    private List<Feature> filteredFeatures => allFeatures
        .Where(f => string.IsNullOrEmpty(selectedTier) || f.Tier == selectedTier)
        .ToList();
        
    private void OnTierChanged(ChangeEventArgs e)
    {
        selectedTier = e.Value?.ToString() ?? "";
    }
}
```

**Levels of Enlightenment Page**:

Progressive learning path visualization:

```razor
@page "/github-copilot/levels-of-enlightenment"

<div class="levels-container">
    @foreach (var level in levels)
    {
        <div class="level-card level-@level.Number">
            <h2>Level @level.Number: @level.Title</h2>
            <p>@level.Description</p>
            <ul>
                @foreach (var skill in level.Skills)
                {
                    <li>@skill</li>
                }
            </ul>
        </div>
    }
</div>
```

### Mobile Navigation (Hamburger Menu)

**Pattern**: Responsive navigation that shows hamburger menu on mobile (`<768px`)

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
        <a href="/github-copilot">GitHub Copilot</a>
        <a href="/azure">Azure</a>
        <a href="/about">About</a>
    </div>
</nav>

@code {
    private bool menuOpen = false;
    
    private void ToggleMenu()
    {
        menuOpen = !menuOpen;
    }
}
```

**CSS**:

```css
/* Desktop - normal nav */
.hamburger {
    display: none;
}

.nav-menu {
    display: flex;
    gap: 1rem;
}

/* Mobile - hamburger menu */
@media (max-width: 768px) {
    .hamburger {
        display: block;
        background: none;
        border: none;
        cursor: pointer;
    }
    
    .nav-menu {
        display: none;
        flex-direction: column;
        position: absolute;
        top: 60px;
        left: 0;
        right: 0;
        background: var(--dark-navy);
    }
    
    .nav-menu.open {
        display: flex;
    }
}
```

### Tag Filtering Behavior

**`SidebarTagCloud` Component - Interactive Tag Toggle**:

The `SidebarTagCloud` component provides interactive tag filtering with toggle behavior:

**Key Features**:

- **Visual active state**: Selected tags are highlighted with `.selected` CSS class (purple background/border)
- **Toggle behavior**: Clicking a tag toggles it on/off (add to filter or remove from filter)
- **URL state management**: Tags are stored in URL query parameter `?tags=tag1,tag2,tag3`
- **Duplicate prevention**: Tags are automatically deduplicated and normalized (lowercased) when parsing from URL
- **Case-insensitive matching**: Tag comparison uses `StringComparer.OrdinalIgnoreCase`

**Implementation Pattern**:

**Tag Selection State**:

- Store in `HashSet<string>` with `StringComparer.OrdinalIgnoreCase`
- Initialize from URL with deduplication and normalization (`.ToLowerInvariant()`)
- Toggle adds/removes tags from set
- Update URL after each toggle

**Page Integration**:

- Use `[SupplyParameterFromQuery(Name = "tags")]` for URL binding
- Parse comma-separated tags with `Uri.UnescapeDataString()`
- Normalize and deduplicate on parse
- Use `Distinct(StringComparer.OrdinalIgnoreCase)`

**See**: [Components/Shared/SidebarTagCloud.razor.cs](Components/Shared/SidebarTagCloud.razor.cs) for toggle implementation  
**See**: [Pages/Section.razor.cs](Pages/Section.razor.cs) for URL parameter parsing

**CSS Active State** (`SidebarTagCloud.razor.css`):

```css
.tag-cloud-item.selected {
    background: var(--color-purple-dark);
    border-color: var(--color-purple-bright);
    color: var(--color-text-on-emphasis);
}

.tag-cloud-item.selected:hover {
    background: var(--color-purple-medium);
    border-color: var(--color-purple-bright);
}
```

**E2E Testing**: See [tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs](../../tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs) for comprehensive tag toggle behavior tests.

## Related Documentation

- **[Root AGENTS.md](../../AGENTS.md)** - AI Assistant Workflow, .NET Tech Stack, Patterns & Examples, performance architecture, timezone handling
- **[src/AGENTS.md](../AGENTS.md)** - .NET development patterns across all src/ projects
- **[tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md)** - bUnit component testing patterns
