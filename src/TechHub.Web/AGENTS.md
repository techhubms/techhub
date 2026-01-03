# Blazor Frontend Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Web/`. It complements the [Root AGENTS.md](../../AGENTS.md), [src/AGENTS.md](../AGENTS.md), and [.github/agents/dotnet.md](../../.github/agents/dotnet.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Critical Rules

### ✅ Always Do

- **Reference Jekyll _sass for design system** - Colors, typography, and spacing from `jekyll/_sass/_colors.scss` and `jekyll/_sass/_settings.scss`
- **Use Tech Hub color palette** - #1f6feb (primary blue), #bd93f9 (bright purple), #1a1a2e (dark navy), #28a745 (secondary green)
- **Server-side render initial content** - Use SSR for SEO and performance
- **Progressive enhancement** - Core functionality works without JavaScript
- **Use TechHubApiClient for all API calls** - Typed HTTP client in `Services/TechHubApiClient.cs`
- **Follow Blazor component patterns** - See `.github/agents/dotnet.md` for framework-specific guidance
- **Fix all linting errors** - Check with `get_errors` tool after editing files
- **Add tests for components** - Use bUnit for component testing (see `tests/TechHub.Web.Tests/AGENTS.md`)

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

### Color Palette

**Source**: `jekyll/_sass/_colors.scss`

**Primary Colors**:

```css
/* CSS Variables (defined in wwwroot/styles.css) */
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

**Source**: `jekyll/_sass/_settings.scss`

**Font Stack**:

```css
font-family: ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont,
             "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
```

**Font Sizes**: Use relative units (rem/em) for accessibility

### Spacing & Breakpoints

**Spacing**: Use consistent spacing units (0.25rem, 0.5rem, 1rem, 1.5rem, 2rem, 3rem)

**Breakpoints** (from `jekyll/_sass/_settings.scss`):

- Mobile: < 768px
- Tablet: 768px - 1024px
- Desktop: > 1024px

### Layout Specifications

**Content Widths**:

- **Listing pages** (home, section pages): Full-width responsive grid (no max-width constraint)
- **Article detail pages**: 800px max-width for optimal reading (matching Jekyll $content-width)
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

<!-- CSS Grid with stable positioning -->
<div class="section-page-grid">
    @* Header loads independently *@
    <SectionHeader SectionName="@SectionName" />
    
    @* Navigation loads independently *@
    <CollectionNav SectionName="@SectionName" 
                   SelectedCollection="@selectedCollection"
                   OnCollectionChange="@HandleCollectionChange" />
    
    @* Content loads independently *@
    <CollectionContent SectionName="@SectionName"
                      CollectionName="@selectedCollection"
                      SectionCategory="@sectionCategory"
                      @key="@selectedCollection" />
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

**CSS Grid Layout (wwwroot/styles.css)**:

```css
/* Stable three-area grid that never shifts */
.section-page-grid {
    display: grid;
    grid-template-areas:
        "header"
        "nav"
        "content";
    grid-template-rows: auto auto 1fr;
    gap: 1.5rem;
    padding: 1rem;
    min-height: 100vh; /* Prevents collapse during loading */
}

.section-header { grid-area: header; }
.collection-nav { grid-area: nav; }
.collection-content { grid-area: content; }

/* Skeleton shimmer animation */
@keyframes shimmer {
    0% { background-position: -1000px 0; }
    100% { background-position: 1000px 0; }
}

.skeleton {
    background: linear-gradient(
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

**Child Component Pattern (SectionHeader.razor)**:

```razor
@using TechHub.Core.DTOs
@inject TechHubApiClient ApiClient
@rendermode InteractiveServer

<header class="section-header">
    @if (section == null)
    {
        @* Skeleton placeholder shown while loading *@
        <div class="skeleton skeleton-header"></div>
    }
    else
    {
        @* Actual content once loaded *@
        <h1>@section.Title</h1>
        <p>@section.Description</p>
    }
</header>

@code {
    [Parameter, EditorRequired]
    public required string SectionName { get; set; }

    private SectionDto? section;

    protected override async Task OnInitializedAsync()
    {
        // Load independently from parent and siblings
        section = await ApiClient.GetSectionAsync(SectionName);
    }
}
```

**Key Architecture Principles**:

1. **CSS Grid Stability**: Grid areas have fixed positions that never change
2. **Independent Loading**: Each component loads its own data asynchronously
3. **Skeleton Placeholders**: Show loading state without layout shift
4. **@key Directive**: Force component re-render on navigation (e.g., `@key="@selectedCollection"`)
5. **EventCallback Communication**: Parent coordinates state (e.g., `OnCollectionChange`)

**Component Parameters**:

- **SectionHeader**: `SectionName` (string) - Loads section metadata
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

    // Assert: Verify CSS Grid structure exists immediately
    cut.MarkupMatches(@"<div class=""section-page-grid"">
        <section-header diff:ignore></section-header>
        <collection-nav diff:ignore></collection-nav>
        <collection-content diff:ignore></collection-content>
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
│   ├── Layout/             # Layout components (MainLayout, NavMenu)
│   ├── Pages/              # Page components (@page directive)
│   ├── SectionCard.razor   # Section display card
│   └── ContentItemCard.razor # Content item display card
├── Services/               # Frontend services
│   └── TechHubApiClient.cs # Typed HTTP client for API
├── wwwroot/                # Static files
│   ├── styles.css          # Global CSS with design system
│   └── images/             # Static images
│       └── section-backgrounds/ # Section header images
├── Program.cs              # Application configuration
└── appsettings.*.json      # Configuration files
```

## Image Conventions

**Path Convention**: Use `/images/` prefix (NOT `/assets/`)

**Section Backgrounds**: `/images/section-backgrounds/{section-name}.jpg`

**Examples**:

- `/images/section-backgrounds/ai.jpg`
- `/images/section-backgrounds/github-copilot.jpg`
- `/images/section-backgrounds/azure.jpg`

**Image Sizes**: 743KB - 967KB per section background (optimized JPG)

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

- **[Root AGENTS.md](../../AGENTS.md)** - Generic principles and architecture
- **[.github/agents/dotnet.md](../../.github/agents/dotnet.md)** - .NET/Blazor framework patterns
- **[src/AGENTS.md](../AGENTS.md)** - .NET project structure and patterns
- **[Jekyll _sass](../../jekyll/_sass/)** - Design system source (colors, typography, spacing)
- **[tests/TechHub.Web.Tests/AGENTS.md](../../tests/TechHub.Web.Tests/AGENTS.md)** - Component testing patterns
