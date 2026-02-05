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

- **Always use design tokens exclusively** - ALL colors, spacing, typography from `wwwroot/css/design-tokens.css` (see [docs/design-system.md](../../docs/design-system.md))
- **Always server-side render initial content** - Use SSR for SEO and performance
- **Always progressive enhancement** - Core functionality works without JavaScript
- **Always use TechHubApiClient for all API calls** - Typed HTTP client in `Services/TechHubApiClient.cs`
- **Always follow Blazor component patterns** - See [Root AGENTS.md](../../AGENTS.md) for .NET/Blazor framework-specific guidance
- **Always follow semantic HTML structure** - Use `<main>`, `<section>`, `<article>`, `<aside>` instead of `<div>` (see [docs/page-structure.md](../../docs/page-structure.md))
- **Always fix all linting errors** - Check with `get_errors` tool after editing files
- **Always add tests for components** - Use bUnit for component testing (see [tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md))

### ⚠️ Ask First

- **Ask first before adding new component dependencies** - Verify no duplication exists
- **Ask first before changing global CSS** - May affect all components
- **Ask first before modifying TechHubApiClient** - May impact all pages
- **Ask first before breaking component APIs** - Changes to @Parameters or public methods

### 🚫 Never Do

- **Never hardcode colors, spacing, or typography** - Always use design tokens from `wwwroot/css/design-tokens.css`
- **Never use inline styles with hardcoded values** - Use CSS classes with design token references
- **Never use hex codes or rgba() directly in CSS** - Define new tokens in design-tokens.css first
- **Never duplicate component logic** - Extract to shared components
- **Never skip error handling** - Use try-catch and display user-friendly messages
- **Never create content without server-side rendering** - Initial load must show complete content
- **Never use /assets/ paths for images** - Use `/images/` convention (e.g., `/images/section-backgrounds/`)
- **Never use `<div>` for main content areas** - Use semantic HTML elements (`<main>`, `<section>`, `<article>`)

## Render Mode Strategy

📖 See [docs/render-modes.md](../../docs/render-modes.md) for complete render mode strategy (SSR vs Interactive Server).

**Testing Interactive Components**:

```csharp
// In your test setup for components using PersistentComponentState
this.AddBunitPersistentComponentState();
```

## Semantic HTML & Page Structure

📖 See [docs/page-structure.md](../../docs/page-structure.md) for semantic HTML requirements, layout classes, sticky header architecture, and sidebar patterns.

## Design System

📖 See [docs/design-system.md](../../docs/design-system.md) for design tokens, colors, typography, hover effects, and image handling.

### CSS Architecture

📖 See [docs/design-system.md](../../docs/design-system.md#css-architecture) for CSS organization rules (global vs component-scoped).

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

### Page Structure and Sidebar Components

📖 See [docs/page-structure.md](../../docs/page-structure.md) for semantic HTML and sidebar patterns.

**Available Sidebar Components**: `SidebarCollectionNav`, `SidebarRssLinks`, `SidebarTagCloud`, `SidebarToc`

## Component Patterns

### Razor Variable Naming Conflicts

**🚨 CRITICAL**: Certain variable names conflict with Razor directives and MUST be avoided in component code.

**The Problem**: Razor uses `@section` as a directive for defining named content sections in layouts. Using `section` as a variable name causes compilation errors because the Razor parser treats `@section.Property` as an invalid directive.

**Conflicting Variable Names**:

```razor
@code {
    // ❌ NEVER DO THIS - Conflicts with @section directive
    private Section section;
    
    // ERROR: The 'section' directive must appear at the start of the line
    <PageTitle>@section.Title - Tech Hub</PageTitle>
}
```

**Recommended Alternatives** (for consistency across codebase):

| Avoid     | Use Instead      | Context                                                |
|-----------|------------------|--------------------------------------------------------|
| `section` | `sectionData`    | Section objects in pages/components                    |
| `section` | `currentSection` | When emphasizing current vs. other sections            |
| `code`    | `codeBlock`      | When working with code snippets (conflicts with @code) |
| `page`    | `pageData`       | When working with page metadata (conflicts with @page) |
| `layout`  | `layoutData`     | When working with layout data (conflicts with @layout) |

**🚨 CRITICAL REMINDER**: Always use `sectionData` (not `section`) throughout your component code, including when passing parameters to other components. Using `@section` will cause undefined variable errors because `section` doesn't exist - only `sectionData` does.

**Correct Pattern**:

```razor
@page "/{sectionName}"
@using TechHub.Core.Models

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
    private Section? sectionData;
    
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

**Best Practice**: When working with model objects in Blazor components, append `Data` to the variable name (e.g., `sectionData`, `contentData`, `itemData`) to avoid conflicts and improve clarity.

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

📖 See [docs/page-structure.md - Skeleton Loading States](../../docs/page-structure.md#skeleton-loading-states) for when and how to use skeletons.

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
    public required Section Section { get; set; }
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
    
    private Section? section;
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

- Arrange: Create test data (models)
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
    private List<ContentItem> visibleItems = new();
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

#### SectionCard Custom Page Ordering and Expand Badges

**Feature**: Custom pages in `SectionCard` support ordering and expandable badge display.

**Custom Page Ordering**:

- Custom pages (collections with `Custom: true`) are ordered by:
  1. `Order` property (lower numbers first)
  2. `Title` (alphabetically, if Order values are equal)
- This ordering applies to:
  - Homepage section card badges
  - SubNav horizontal navigation bar

**Expand Badge Behavior**:

- **First custom page**: Always visible with `.badge-custom` styling
- **Additional custom pages**: Hidden behind "+X more" button
- **Click behavior**: Clicking "+X more" permanently reveals remaining custom pages inline
  - Button is removed after click
  - Hidden badges are displayed using `display: contents` for inline flow
  - No collapse functionality (use F5 to reset)
- **CSS**: `.custom-pages-expanded` uses `display: contents` to make badges flow inline with existing badges

**Configuration** (appsettings.json):

```json
{
  "Collections": {
    "features": {
      "Title": "Features",
      "Url": "/github-copilot/features",
      "Description": "GitHub Copilot features and capabilities overview",
      "Custom": true,
      "Order": 1
    },
    "handbook": {
      "Title": "The GitHub Copilot Handbook",
      "Url": "/github-copilot/handbook",
      "Description": "A practical guide to GitHub Copilot",
      "Custom": true,
      "Order": 3
    }
  }
}
```

**Code Example** (SectionCard.razor):

```razor
@* Show first custom page (ordered by Order, then Title) *@
@if (CustomPages.Any())
{
    var firstCustom = CustomPages.First();
    <a href="@firstCustom.Url" class="badge-custom">
        @firstCustom.Title
    </a>
    
    @* Show "+X more" button if additional custom pages exist *@
    @if (CustomPages.Count > 1)
    {
        <button class="badge-grey badge-expandable" data-expand-target="custom-pages-@Section.Name">
            +@(CustomPages.Count - 1) more
        </button>
        
        <span id="custom-pages-@Section.Name" class="custom-pages-expanded" hidden>
            @foreach (var customPage in CustomPages.Skip(1))
            {
                <a href="@customPage.Url" class="badge-custom">@customPage.Title</a>
            }
        </span>
    }
}

@code {
    private IReadOnlyList<Collection> CustomPages => 
        Section.Collections
            .Where(c => c.IsCustom)
            .OrderBy(c => c.Order)
            .ThenBy(c => c.Title)
            .ToList();
}
```

**JavaScript** (custom-pages.js):

```javascript
export function initExpandableBadges() {
    document.querySelectorAll('.badge-expandable[data-expand-target]').forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            
            const targetId = this.dataset.expandTarget;
            const targetElement = document.getElementById(targetId);
            
            if (targetElement) {
                targetElement.hidden = false; // Show hidden badges
                this.remove(); // Remove the expand button
            }
        });
    });
}
```

**E2E Testing**: See [tests/TechHub.E2E.Tests/Web/SectionCardCustomPagesTests.cs](../../tests/TechHub.E2E.Tests/Web/SectionCardCustomPagesTests.cs) for comprehensive tests covering:

- Badge visibility and positioning
- Custom page ordering
- Expand/reveal behavior
- Inline badge flow (not separate rows)
- Accessibility attributes
- No JavaScript errors

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

**See**: [Components/SidebarTagCloud.razor.cs](Components/SidebarTagCloud.razor.cs) for toggle implementation  
**See**: [Components/Pages/Section.razor](Components/Pages/Section.razor) for URL parameter parsing

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

### Functional Documentation (docs/)

- **[Design System](../../docs/design-system.md)** - Design tokens, colors, typography, CSS architecture
- **[Page Structure](../../docs/page-structure.md)** - Semantic HTML, layouts, sticky header, sidebar
- **[Render Modes](../../docs/render-modes.md)** - Blazor render mode strategy (SSR vs Interactive)
- **[Filtering](../../docs/filtering.md)** - Tag filtering system and tag cloud behavior

### Implementation Guides (AGENTS.md)

- **[Root AGENTS.md](../../AGENTS.md)** - AI Assistant Workflow, .NET Tech Stack, Patterns & Examples
- **[src/AGENTS.md](../AGENTS.md)** - .NET development patterns across all src/ projects
- **[tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md)** - bUnit component testing patterns
