# Blazor Frontend Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Web/`. It complements the [Root AGENTS.md](../../AGENTS.md) and [src/AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

This project implements the Blazor frontend with server-side rendering (SSR) and optional WebAssembly interactivity. It consumes the TechHub.Api through TechHubApiClient and renders content using the Tech Hub design system.

**When to read this file**: When creating or modifying Blazor components, pages, layouts, or understanding frontend architecture.

**Testing this code**:

- Component tests: See [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md) for bUnit patterns
- E2E tests: See [tests/TechHub.E2E.Tests/AGENTS.md](../../tests/TechHub.E2E.Tests/AGENTS.md) for Playwright patterns

## Critical Rules

### ✅ Always Do

- **Use Tech Hub design system** - Colors, typography, and spacing defined in `wwwroot/css/site.css` (derived from legacy Jekyll _sass)
- **Use Tech Hub color palette** - #1f6feb (primary blue), #bd93f9 (bright purple), #1a1a2e (dark navy), #28a745 (secondary green)
- **Server-side render initial content** - Use SSR for SEO and performance
- **Progressive enhancement** - Core functionality works without JavaScript
- **Use TechHubApiClient for all API calls** - Typed HTTP client in `Services/TechHubApiClient.cs`
- **Follow Blazor component patterns** - See [Root AGENTS.md](../../AGENTS.md) for .NET/Blazor framework-specific guidance
- **Fix all linting errors** - Check with `get_errors` tool after editing files
- **Add tests for components** - Use bUnit for component testing (see [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md))

### ⚠️ Ask First

- **Adding new component dependencies** - Verify no duplication exists
- **Changing global CSS** - May affect all components
- **Modifying TechHubApiClient** - May impact all pages
- **Breaking component APIs** - Changes to @Parameters or public methods

### 🚫 Never Do

- **Never hardcode colors** - Always use CSS variables from Tech Hub design system
- **Never duplicate component logic** - Extract to shared components
- **Never skip error handling** - Use try-catch and display user-friendly messages
- **Never create content without server-side rendering** - Initial load must show complete content
- **Never use /assets/ paths for images** - Use `/images/` convention (e.g., `/images/section-backgrounds/`)

## Tech Hub Design System

### CSS Architecture

Tech Hub uses a **dual CSS strategy**: **Global CSS** for reusable design system components and **Component-Scoped CSS** for page-specific layouts.

#### Global CSS (wwwroot/css/)

Reusable design system components and site-wide styles:

```text
wwwroot/css/
├── design-tokens.css         # ALL colors, typography, spacing (single source of truth)
├── base.css                  # Reset, typography, links, focus states
├── layout.css                # Header, footer, navigation, grid
├── components/
│   ├── sidebar.css          # Shared sidebar component (used across multiple pages)
│   ├── buttons.css          # All button styles
│   ├── navigation.css       # Breadcrumb navigation only
│   ├── loading.css          # Skeleton loaders, loading states
│   ├── page-container.css   # Page layout containers
│   └── forms.css            # Form validation, Blazor errors
└── utilities.css             # Utility classes
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
│   └── NavHeader.razor.css        # Site navigation header
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

**App.razor** - Conditional loading based on environment:

```html
@if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" || 
     Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
{
    <!-- Development: Individual files for debugging -->
    <link rel="stylesheet" href="css/design-tokens.css" />
    <link rel="stylesheet" href="css/base.css" />
    <link rel="stylesheet" href="css/layout.css" />
    <link rel="stylesheet" href="css/components/sidebar.css" />
    <link rel="stylesheet" href="css/components/cards.css" />
    <link rel="stylesheet" href="css/components/buttons.css" />
    <link rel="stylesheet" href="css/components/navigation.css" />
    <link rel="stylesheet" href="css/components/loading.css" />
    <link rel="stylesheet" href="css/components/forms.css" />
    <link rel="stylesheet" href="css/utilities.css" />
}
else
{
    <!-- Production: Bundled and minified -->
    <link rel="stylesheet" href="css/bundle.css" />
}
<!-- Component-scoped styles (all modes) -->
<link rel="stylesheet" href="@Assets["TechHub.Web.styles.css"]" />
```

**Program.cs** - WebOptimizer bundle configuration:

```csharp
builder.Services.AddWebOptimizer(pipeline =>
{
    // Bundle all global CSS files - MUST match App.razor references
    pipeline.AddCssBundle("/css/bundle.css",
        "css/design-tokens.css",
        "css/base.css",
        "css/layout.css",
        "css/components/sidebar.css",
        "css/components/cards.css",
        "css/components/buttons.css",
        "css/components/navigation.css",
        "css/components/loading.css",
        "css/components/forms.css",
        "css/utilities.css"
    );
});
```

**CRITICAL**: App.razor and Program.cs MUST reference the exact same CSS files in the same order.

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

Note: Sidebar components (SidebarCollectionNav, SidebarRssLinks, SidebarTags) do NOT have 
      .razor.css files - they use global styles from sidebar.css
```

**Global CSS** (wwwroot/css/):

```text
wwwroot/css/
├── design-tokens.css                  # Colors, typography, spacing (used everywhere)
├── base.css                           # Reset, typography, links (used everywhere)
├── layout.css                         # Site header, footer, nav (used everywhere)
├── components/
│   ├── page-container.css            # Page layout containers (.page-with-sidebar, .page-without-sidebar)
│   ├── sidebar.css                   # Sidebar component styles (used on multiple pages)
│   ├── buttons.css                   # Button styles (used everywhere)
│   ├── forms.css                     # Form styles (used on multiple pages)
│   └── loading.css                   # Skeleton loading states (used on multiple pages)
└── utilities.css                      # Utility classes (used everywhere)
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

### Sidebar Component Architecture

**🚨 CRITICAL RESPONSIBILITY RULE**: Pages define layout structure with `<aside class="sidebar">`, sidebar components only render their content.

**Pattern**: Composition-based sidebar design where pages control layout and components provide functionality.

#### Page Layout Classes

**Two standardized layouts** (defined in `page-container.css`):

- **`.page-with-sidebar`** - Two-column grid layout (sidebar + main content)
  - Used by: Section, SectionCollection, Home, ContentItem pages
  - Grid: `300px 1fr` with responsive breakpoints
  - Includes `<aside class="sidebar">` and `<main class="page-main-content">`

- **`.page-without-sidebar`** - Single column centered layout
  - Used by: About, Error, NotFound pages
  - Max-width: `1400px`, centered with auto margins
  - Only `<main class="page-main-content">` (no sidebar)

#### Sidebar Layout Responsibility

**Pages** (`Section.razor`, `Home.razor`, `ContentItem.razor`):

- ✅ Use `.page-with-sidebar` container class
- ✅ Define the `<aside class="sidebar">` container
- ✅ Determine which sidebar components to render
- ✅ Control component order and composition
- ✅ Pass required parameters to components

**Sidebar Components** (`SidebarCollectionNav.razor`, `SidebarRssLinks.razor`, `SidebarTags.razor`):

- ✅ Render their specific content (navigation, RSS links, tag clouds)
- ✅ Use semantic HTML appropriate to their purpose (`<nav>`, `<div>`, etc.)
- ✅ Apply shared sidebar styles (`.sidebar-section`, `.sidebar-link-button`, etc.)
- ❌ DO NOT wrap themselves in `<aside class="sidebar">`
- ❌ DO NOT define their own container positioning (sticky, grid, etc.)
- ❌ DO NOT have component-scoped `.razor.css` files (styles are global in sidebar.css)

#### Example: Section Page with Sidebar

**Section.razor** (Page defines layout):

```razor
<div class="page-with-sidebar">
    <PageHeader Section="@sectionData" />
    
    @* Page defines sidebar container *@
    <aside class="sidebar">
        @* Page composes sidebar components *@
        <SidebarCollectionNav Section="@sectionData" SelectedCollection="all" />
        <SidebarRssLinks Links="@(new[] { new SidebarRssLinks.RssLink(\"RSS Feed\", $\"{sectionData.Url}/feed.xml\") })\" />
    </aside>
    
    <main class="page-main-content">
        <ContentItemsGrid ... />
    </main>
</div>
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

**SidebarTags.razor** (Component renders content only):

```razor
@* Uses <div> for tag cloud (not navigation) *@
<div class="sidebar-section">
    <h2 class="sidebar-h2">@Title</h2>
    <div class="tags-cloud">
        @foreach (var tag in displayTags)
        {
            <a href="@($"{BaseUrl}?tag={Uri.EscapeDataString(tag)}")"
               class="sidebar-tag">@tag</a>
        }
    </div>
</div>
```

**Parameters**:

- `Tags` (required): List of tag strings to display
- `Title`: Heading text (default: "Tags")
- `BaseUrl` (required): URL for tag filter links
- `MaxTags`: Limit number of tags shown (default: null/all)
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

**Global CSS** (`wwwroot/css/components/sidebar.css`):

- `.sidebar` - Container with sticky positioning (used by pages)
- `.sidebar-section` - Section wrapper
- `.sidebar-h2` - Section headings
- `.sidebar-list` - List styling
- `.sidebar-link-button` - Navigation buttons (NO `.sidebar` parent scoping!)
- `.sidebar-content-button` - Content item buttons
- `.sidebar-tag-button` - Interactive tag buttons
- `.sidebar-category-tag` - Display-only category tags
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

**SidebarTags** - Interactive tag cloud for filtering content

- **Used in**: Home page (popular tags), ContentItem pages (article tags)
- **Parameters**:
  - `Tags` (required) - Collection of tag strings
  - `Title` (optional, default "Tags") - Section heading
  - `BaseUrl` (required) - Base URL for tag filtering (e.g., "/ai" for section, "/all" for global)
  - `MaxTags` (optional) - Maximum tags to display
  - `CssClass` (optional) - Additional CSS class (e.g., "popular-tags", "article-tags")
- **Examples**:

  ```razor
  @* Article tags - filter within section *@
  <SidebarTags Tags="@item.Tags"
               Title="Tags"
               BaseUrl="@($"/{sectionName}")"
               CssClass="article-tags" />
  
  @* Popular tags - filter across all content *@
  <SidebarTags Tags="@popularTags"
               Title="Popular Tags"
               BaseUrl="/all"
               MaxTags="15"
               CssClass="popular-tags" />
  ```

#### Benefits of This Architecture

- ✅ **Clear separation of concerns** - Pages control structure, components provide functionality
- ✅ **Reusable components** - SidebarCollectionNav, SidebarRssLinks, SidebarTags used across multiple pages
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
│  Metadata (date, tags, category)                │
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
<div class="page-with-sidebar">
    @* Header loads independently *@
    <PageHeader Section="@section" />
    
    @* Sidebar container *@
    <aside class="sidebar">
        <SidebarCollectionNav Section="@section" 
                             SelectedCollection="@selectedCollection" />
        <SidebarRssLinks Links="@(new[] { new SidebarRssLinks.RssLink(\"RSS Feed\", $\"{section.Url}/feed.xml\") })\" />
    </aside>
    
    @* Main content area *@
    <main class="page-main-content">
        <ContentItemsGrid ... />
    </main>
</div>

@code {
    [Parameter]
    public string SectionName { get; set; } = null!;

    [Parameter]
    public string? CollectionName { get; set; }

    private string selectedCollection = string.Empty;
    private string? sectionCategory;

    protected override async Task OnInitializedAsync()
    {
        selectedCollection = CollectionName ?? "all";
        await LoadSectionMetadata(); // Loads category info
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

/* Main content area */
.page-main-content {
  min-width: 0;
  /* Prevent grid blowout */
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
    
    // For static pages - provide Title, Description, BackgroundImage
    [Parameter]
    public string? Title { get; set; }
    
    [Parameter]
    public string? Description { get; set; }
    
    [Parameter]
    public string? BackgroundImage { get; set; }
    
    // Computed properties - prefer Section data if available
    private string DisplayTitle => Section?.Title ?? Title ?? string.Empty;
    private string? DisplayDescription => Section?.Description ?? Description;
    private string? DisplayBackgroundImage => Section?.BackgroundImage ?? BackgroundImage;
    
    private string StyleAttribute => $"background-image: url('{DisplayBackgroundImage ?? "/images/section-backgrounds/home.jpg"}'); background-size: cover; background-position: center center; background-repeat: no-repeat;";
}
```

**Key Architecture Principles**:

1. **CSS Grid Stability**: Grid areas have fixed positions that never change
2. **Independent Loading**: Each component loads its own data asynchronously
3. **Skeleton Placeholders**: Show loading state without layout shift
4. **@key Directive**: Force component re-render on navigation (e.g., `@key="@selectedCollection"`)
5. **EventCallback Communication**: Parent coordinates state (e.g., `OnCollectionChange`)

**Component Parameters**:

- **PageHeader**: `Section` (SectionDto) for section pages, or `Title`/`Description`/`BackgroundImage` for static pages
- **CollectionNav**: `SectionName` (string), `SelectedCollection` (string), `OnCollectionChange` (EventCallback)
- **CollectionContent**: `SectionName` (string), `CollectionName` (string), `SectionCategory` (string)

**Testing Requirements**:

```csharp
// tests/TechHub.Web.Tests/Components/SectionTests.cs
[Fact]
public void Section_RendersWithSkeletonLayout()
{
    // Arrange: Mock API client with delayed response
    var mockApi = new Mock<TechHubApiClient>();
    var tcs = new TaskCompletionSource<SectionDto>();
    mockApi.Setup(x => x.GetSectionAsync(It.IsAny<string>(), default))
           .Returns(tcs.Task);

    using var ctx = new TestContext();
    ctx.Services.AddSingleton(mockApi.Object);

    // Act: Render component (API hasn't responded yet)
    var cut = ctx.RenderComponent<Section>(parameters => parameters
        .Add(p => p.SectionName, "ai"));

    // Assert: Verify page layout structure exists immediately
    cut.MarkupMatches(@"<div class=""page-with-sidebar"">
        <div diff:ignore></div> <!-- PageHeader -->
        <aside class=""sidebar"" diff:ignore></aside>
        <main class=""page-main-content"" diff:ignore></main>
    </div>");
}
```

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

### Component with Background Image

**Pattern**: Use inline styles for dynamic background images with fallback color

```razor
<div class="section-card"
     style="background-image: url('@Section.BackgroundImage'); background-color: var(--dark-navy);">
    <div class="section-card-overlay">
        <h2>@Section.Title</h2>
    </div>
</div>
```

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

**Pattern**: Use helper methods for relative dates

```csharp
private string FormatDate(long dateEpoch)
{
    var date = DateTimeOffset.FromUnixTimeSeconds(dateEpoch).DateTime;
    var now = DateTime.UtcNow;
    var diff = now - date;
    
    if (diff.TotalDays < 1) return "Today";
    if (diff.TotalDays < 2) return "Yesterday";
    if (diff.TotalDays < 7) return $"{(int)diff.TotalDays} days ago";
    if (diff.TotalDays < 30) return $"{(int)(diff.TotalDays / 7)} weeks ago";
    if (diff.TotalDays < 365) return $"{(int)(diff.TotalDays / 30)} months ago";
    return date.ToString("MMM dd, yyyy");
}
```

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
│   │   ├── layout.css
│   │   ├── components/     # Reusable component styles
│   │   └── utilities.css
│   └── images/             # Static images
│       └── section-backgrounds/ # Section header images
├── Program.cs              # Application configuration (CSS bundling)
└── appsettings.*.json      # Configuration files
```

**Component Organization**:

- **Layout/**: Application layout (MainLayout, NavMenu)
- **Pages/**: Routable pages
  - `Home.razor` - Homepage (`/`) - Shows all sections
  - `Section.razor` - Section page (`/{sectionName}`) - Shows all content in section
  - `SectionCollection.razor` - Collection page (`/{sectionName}/{collectionName}`) - Shows filtered content
  - `ContentItem.razor` - Detail page (`/{sectionName}/{collection}/{itemId}`) - Shows single content item
  - `About.razor` - About page (`/about`)
  - `NotFound.razor` - 404 page
- **Shared/**: Reusable UI components (PageHeader, NavHeader)
- **Root Components/**: Feature-specific components
  - `SectionCard.razor` - Section display card (used on homepage)
  - `SectionCardsGrid.razor` - Grid of section cards (used on homepage)
  - `ContentItemCard.razor` - Content item display card (used on section/collection pages)
  - `ContentItemsGrid.razor` - Grid of content items with filtering (used on section/collection pages)
  - `ContentItemDetail.razor` - Full content rendering (used on detail page)
  - `SidebarCollectionNav.razor` - Sidebar collection navigation (used on section/collection pages)
- **Routes.razor**: Blazor router (required by framework, stays in root)
- **App.razor**: Application entry point (required by framework, stays in root)

## Image Conventions

**Path Convention**: Use `/images/` prefix (NOT `/assets/`)

**Section Backgrounds**: `/images/section-backgrounds/{section-name}.jpg`

**Examples**:

- `/images/section-backgrounds/ai.jpg`
- `/images/section-backgrounds/github-copilot.jpg`
- `/images/section-backgrounds/azure.jpg`

**Image Sizes**: 743KB - 967KB per section background (optimized JPG)

## Static Files & Browser Caching

**Configuration**: `Program.cs` configures aggressive browser caching for static assets

**Cache Strategy**:

- **Images** (jpg, png, webp, svg, ico): 1 year cache (`max-age=31536000,immutable`)
- **Fonts** (woff, woff2, ttf, eot): 1 year cache (`max-age=31536000,immutable`)
- **CSS/JS** (via MapStaticAssets): 1 year cache with fingerprinting for cache busting
- **Other files**: 1 hour cache (`max-age=3600`)

**Why This Works**:

- **`immutable`**: Tells browser the file will NEVER change at this URL
- **1 year cache**: Banner images, fonts, and other assets cached locally for maximum performance
- **No revalidation**: Browser uses cached version without asking server "has it changed?"
- **Cache busting**: CSS/JS files get version fingerprint in filename (e.g., `styles.abc123.css`)

**Benefits**:

- ✅ Banner images load instantly on subsequent page views (from disk cache)
- ✅ Zero HTTP requests for cached assets = faster page loads
- ✅ Reduces server bandwidth
- ✅ Better user experience on slow connections

**Source**: See `UseStaticFiles` configuration in [Program.cs](Program.cs)

## Testing Components

**See**: `tests/TechHub.Web.Tests/AGENTS.md` for bUnit testing patterns

**Basic Test Pattern**:

```csharp
[Fact]
public void SectionCard_RendersTitle()
{
    // Arrange
    var section = new SectionDto { Title = "AI", /* ... */ };
    
    // Act
    var cut = RenderComponent<SectionCard>(parameters => parameters
        .Add(p => p.Section, section));
    
    // Assert
    cut.Find("h2").TextContent.Should().Be("AI");
}
```

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

## Related Documentation

- **[Root AGENTS.md](../../AGENTS.md)** - AI Assistant Workflow, .NET Tech Stack, Patterns & Examples, performance architecture, timezone handling
- **[src/AGENTS.md](../AGENTS.md)** - .NET development patterns across all src/ projects
- **[tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md)** - bUnit component testing patterns
