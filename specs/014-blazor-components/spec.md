# Blazor Components Specification

> **Feature**: Reusable Blazor components for layout, navigation, and content display

## Overview

Blazor components provide the UI layer with server-side rendering (SSR) for SEO and optional interactivity via WebAssembly. Components are semantic, accessible, and optimized for performance. All initial rendering is server-side; client-side interactivity is added progressively for filtering and infinite scroll.

## Requirements

### Functional Requirements

**FR-1**: Components MUST render semantic HTML5 elements  
**FR-2**: Components MUST support server-side rendering (SSR)  
**FR-3**: Components MUST be accessible (WCAG 2.1 Level AA)  
**FR-4**: Components MUST use typed parameters  
**FR-5**: Components MUST include Schema.org structured data where applicable  
**FR-6**: Components MUST support code-behind for complex logic  
**FR-7**: Components MUST handle null states gracefully  
**FR-8**: Components MUST use proper ARIA attributes  
**FR-9**: Interactive components MUST use WebAssembly render mode  
**FR-10**: Components MUST be testable with bUnit  

### Non-Functional Requirements

**NFR-1**: Components MUST render in < 50ms (p95)  
**NFR-2**: Components MUST minimize JavaScript payload  
**NFR-3**: Components MUST support lazy loading where appropriate  
**NFR-4**: Components MUST follow Blazor best practices  

## Component Catalog

### Layout Components

**MainLayout.razor** - Root application layout
- Includes Header, Footer, and Body placeholder
- Sets up HeadOutlet for page-specific meta tags
- Provides common structure for all pages

**Header.razor** - Site header with navigation
- Semantic `<header role="banner">`
- Logo linking to home page
- Main navigation menu
- Mobile-responsive hamburger menu

**Footer.razor** - Site footer
- Copyright information
- RSS subscription link
- Privacy/legal links

**SectionHeader.razor** - Section-specific header
- Background image from section configuration
- Section title and description
- Breadcrumb navigation

**SectionNav.razor** - Section collection navigation
- Links to section collections (News, Blogs, Videos, etc.)
- Active state highlighting
- Keyboard accessible

### Content Components

**ContentList.razor** - List of content items
- Grid or list layout
- Supports infinite scroll (WASM interactivity)
- Empty state handling

**ItemCard.razor** - Individual content item card
- Schema.org Article markup
- Title, description, author, date
- Link to full content
- Video indicator for video items

**SectionCard.razor** - Section card for home page
- Section title and description
- Background image
- Link to section index
- Item count badge

**RoundupCard.razor** - Roundup-specific card
- Special styling for roundups
- Date prominence
- Link to roundup content

### Filter Components

**FilterControls.razor** - Filter UI controls (WASM)
- Date range filters
- Tag filters
- Collection filters
- Clear all button
- Active filter indicators

**FilterButton.razor** - Individual filter button
- Active/inactive states
- Item count badge
- Keyboard accessible
- ARIA pressed state

**SearchBox.razor** - Text search input (WASM)
- Debounced input (300ms)
- Clear button
- Keyboard shortcuts (Ctrl+K)
- ARIA search role

### Utility Components

**LoadingSpinner.razor** - Loading indicator
- Accessible loading announcement
- Semantic ARIA attributes

**ErrorBoundary.razor** - Error handling
- Graceful degradation
- User-friendly error messages

## Implementation Examples

### ItemCard.razor

```razor
@* TechHub.Web/Components/Content/ItemCard.razor *@

<article class="item-card" 
         itemscope 
         itemtype="https://schema.org/@(GetSchemaType())">
    <a href="@Item.Url" class="item-link">
        @if (Item.VideoId is not null)
        {
            <div class="video-indicator" aria-label="Video content">
                <svg><!-- video icon --></svg>
            </div>
        }
        
        <div class="item-content">
            <h3 itemprop="headline">@Item.Title</h3>
            
            <p itemprop="description" class="item-description">
                @Item.Description
            </p>
            
            <div class="item-meta">
                <span itemprop="author" itemscope itemtype="https://schema.org/Person">
                    <span itemprop="name">@Item.Author</span>
                </span>
                
                <time datetime="@Item.DateIso" 
                      itemprop="datePublished"
                      class="item-date">
                    @Item.Date
                </time>
            </div>
            
            @if (Item.Tags.Any())
            {
                <div class="item-tags" aria-label="Tags">
                    @foreach (var tag in Item.Tags.Take(5))
                    {
                        <span class="tag">@tag</span>
                    }
                </div>
            }
        </div>
    </a>
</article>

@code {
    [Parameter]
    public required ContentItemDto Item { get; set; }
    
    private string GetSchemaType() => Item.VideoId is not null
        ? "VideoObject"
        : "Article";
}
```

### SectionNav.razor

```razor
@* TechHub.Web/Components/Shared/SectionNav.razor *@

<nav class="section-nav" aria-label="Section navigation">
    <ul role="list">
        @foreach (var collection in Section.Collections)
        {
            <li>
                <a href="/@Section.Url/@collection.Url.html" 
                   class="@(IsActive(collection.Collection) ? "active" : "")"
                   aria-current="@(IsActive(collection.Collection) ? "page" : null)">
                    @collection.Title
                </a>
            </li>
        }
    </ul>
</nav>

@code {
    [Parameter]
    public required SectionDto Section { get; set; }
    
    [Parameter]
    public string? ActiveCollection { get; set; }
    
    private bool IsActive(string collection) =>
        string.Equals(ActiveCollection, collection, StringComparison.OrdinalIgnoreCase);
}
```

### FilterControls.razor (Interactive)

```razor
@* TechHub.Web/Components/Filters/FilterControls.razor *@
@rendermode InteractiveWebAssembly

@inject NavigationManager Navigation

<div class="filter-controls" role="search">
    <div class="filter-header">
        <h2>Filters</h2>
        <button @onclick="ClearAllFilters" 
                class="btn-clear" 
                disabled="@(!HasActiveFilters)">
            Clear All
        </button>
    </div>
    
    <div class="filter-section">
        <h3 id="date-filter-label">Date Range</h3>
        <div role="group" aria-labelledby="date-filter-label">
            @foreach (var filter in DateFilters)
            {
                <FilterButton 
                    Label="@filter.Label" 
                    Count="@filter.Count"
                    IsActive="@(ActiveDateFilter == filter.Value)"
                    OnClick="@(() => SetDateFilter(filter.Value))" />
            }
        </div>
    </div>
    
    <div class="filter-section">
        <label for="search-input">Search</label>
        <input type="search" 
               id="search-input"
               @bind="SearchText" 
               @bind:event="oninput"
               @bind:after="OnSearchChanged"
               placeholder="Search content..." 
               aria-label="Search content" />
    </div>
    
    <div class="filter-section">
        <h3 id="collection-filter-label">Collections</h3>
        <div role="group" aria-labelledby="collection-filter-label">
            @foreach (var collection in CollectionFilters)
            {
                <FilterButton 
                    Label="@collection.Label" 
                    Count="@collection.Count"
                    IsActive="@ActiveCollections.Contains(collection.Value)"
                    OnClick="@(() => ToggleCollection(collection.Value))" />
            }
        </div>
    </div>
    
    <div class="results-count" role="status" aria-live="polite">
        Showing @FilteredCount of @TotalCount items
    </div>
</div>

@code {
    [Parameter]
    public IReadOnlyList<ContentItemDto> AllItems { get; set; } = [];
    
    [Parameter]
    public EventCallback<IReadOnlyList<ContentItemDto>> OnFilterChanged { get; set; }
    
    private string SearchText = "";
    private string? ActiveDateFilter;
    private HashSet<string> ActiveCollections = new();
    private System.Threading.Timer? _searchDebounceTimer;
    
    private int FilteredCount => ApplyFilters().Count;
    private int TotalCount => AllItems.Count;
    private bool HasActiveFilters => 
        !string.IsNullOrWhiteSpace(SearchText) || 
        ActiveDateFilter is not null || 
        ActiveCollections.Count > 0;
    
    protected override void OnInitialized()
    {
        ParseUrlParameters();
    }
    
    private void OnSearchChanged()
    {
        // Debounce search
        _searchDebounceTimer?.Dispose();
        _searchDebounceTimer = new System.Threading.Timer(_ =>
        {
            InvokeAsync(() =>
            {
                ApplyFiltersAndNotify();
                UpdateUrl();
            });
        }, null, 300, Timeout.Infinite);
    }
    
    private void SetDateFilter(string? value)
    {
        ActiveDateFilter = ActiveDateFilter == value ? null : value;
        ApplyFiltersAndNotify();
        UpdateUrl();
    }
    
    private void ToggleCollection(string collection)
    {
        if (ActiveCollections.Contains(collection))
            ActiveCollections.Remove(collection);
        else
            ActiveCollections.Add(collection);
        
        ApplyFiltersAndNotify();
        UpdateUrl();
    }
    
    private void ClearAllFilters()
    {
        SearchText = "";
        ActiveDateFilter = null;
        ActiveCollections.Clear();
        ApplyFiltersAndNotify();
        UpdateUrl();
    }
    
    private List<ContentItemDto> ApplyFilters()
    {
        var filtered = AllItems.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(i => 
                i.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                i.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                i.Tags.Any(t => t.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
        }
        
        if (ActiveDateFilter is not null)
        {
            var cutoff = CalculateDateCutoff(ActiveDateFilter);
            filtered = filtered.Where(i => i.DateEpoch >= cutoff);
        }
        
        if (ActiveCollections.Count > 0)
        {
            filtered = filtered.Where(i => 
                ActiveCollections.Contains(i.Collection, StringComparer.OrdinalIgnoreCase));
        }
        
        return filtered.ToList();
    }
    
    private void ApplyFiltersAndNotify()
    {
        var filtered = ApplyFilters();
        OnFilterChanged.InvokeAsync(filtered);
    }
    
    private void UpdateUrl()
    {
        var queryParams = new Dictionary<string, string?>();
        
        if (!string.IsNullOrWhiteSpace(SearchText))
            queryParams["search"] = SearchText;
        
        if (ActiveDateFilter is not null)
            queryParams["date"] = ActiveDateFilter;
        
        if (ActiveCollections.Count > 0)
            queryParams["collections"] = string.Join(",", ActiveCollections);
        
        var newUri = Navigation.GetUriWithQueryParameters(queryParams);
        Navigation.NavigateTo(newUri, forceLoad: false);
    }
    
    public void Dispose()
    {
        _searchDebounceTimer?.Dispose();
    }
}
```

## Code-Behind Pattern

For complex components, use code-behind:

```csharp
// TechHub.Web/Components/Pages/SectionIndex.razor.cs
namespace TechHub.Web.Components.Pages;

public class SectionIndexBase : ComponentBase
{
    [Parameter]
    public required string SectionUrl { get; set; }
    
    [Inject]
    protected ITechHubApiClient ApiClient { get; set; } = default!;
    
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;
    
    protected SectionDto? Section { get; set; }
    protected IReadOnlyList<ContentItemDto> AllItems { get; set; } = [];
    protected IReadOnlyList<ContentItemDto> FilteredItems { get; set; } = [];
    protected bool IsLoading { get; set; } = true;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadSectionDataAsync();
    }
    
    protected async Task LoadSectionDataAsync()
    {
        IsLoading = true;
        
        try
        {
            Section = await ApiClient.GetSectionAsync(SectionUrl);
            if (Section is null)
            {
                Navigation.NavigateTo("/404");
                return;
            }
            
            AllItems = await ApiClient.GetContentAsync(SectionUrl);
            FilteredItems = AllItems;
        }
        catch (Exception ex)
        {
            // Log error
            Navigation.NavigateTo("/error");
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    protected void OnFilterChanged(IReadOnlyList<ContentItemDto> filtered)
    {
        FilteredItems = filtered;
        StateHasChanged();
    }
}
```

## Testing with bUnit

```csharp
// TechHub.Web.Tests/Components/ItemCardTests.cs
public class ItemCardTests : TestContext
{
    [Fact]
    public void ItemCard_RendersTitle()
    {
        // Arrange
        var item = new ContentItemDto
        {
            Id = "test",
            Title = "Test Article",
            Description = "Test description",
            Url = "/ai/news/test.html",
            CanonicalUrl = "/ai/news/test.html",
            Author = "Test Author",
            Date = "2025-01-15",
            DateIso = "2025-01-15T00:00:00+01:00",
            DateEpoch = 1736899200,
            Collection = "news",
            Categories = new[] { "ai" },
            Tags = Array.Empty<string>()
        };
        
        // Act
        var cut = RenderComponent<ItemCard>(parameters =>
            parameters.Add(p => p.Item, item));
        
        // Assert
        cut.Find("h3").TextContent.Should().Be("Test Article");
    }
    
    [Fact]
    public void ItemCard_WithVideoId_ShowsVideoIndicator()
    {
        var item = new ContentItemDto
        {
            // ... properties
            VideoId = "abc123"
        };
        
        var cut = RenderComponent<ItemCard>(parameters =>
            parameters.Add(p => p.Item, item));
        
        cut.Find(".video-indicator").Should().NotBeNull();
    }
}
```

## References

- bUnit documentation: https://bunit.dev/
- Blazor documentation: https://learn.microsoft.com/aspnet/core/blazor
- `/specs/013-api-endpoints/spec.md` - API client integration

