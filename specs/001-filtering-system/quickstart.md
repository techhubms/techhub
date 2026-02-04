# Quickstart Guide: Sidebar Content Filtering

**Feature**: 001-filtering-system  
**Audience**: Users, Developers, Testers  
**Date**: 2026-01-16  
**Updated**: 2026-02-03

## Implementation Status

✅ **Currently Available**:

- Basic tag filtering via sidebar tag cloud
- Tag selection with URL parameters
- Multiple tag selection (OR logic)
- Tag cloud scoping (homepage/section/collection/content)
- Quantile-based tag sizing
- Tag toggle (click to select/deselect)

❌ **Coming Soon** (in development):

- **Dynamic tag counts** - tags will show item counts (e.g., "AI (901)")
- **Tag disabled state** - tags with 0 results will be grayed out
- **Date range slider** - filter by publication date
- **Excel-style tag dropdown** - search and browse all tags

## For Users: How to Filter Content

### Using the Sidebar Tag Cloud

The sidebar tag cloud shows the **top 20 most-used tags** from the last 90 days (or your selected date range), scoped to your current page:

1. **Navigate to a section page** (e.g., `/github-copilot`)
2. **View the tag cloud** in the sidebar
3. **Click a tag** to filter content - the tag highlights and content updates
4. **Click again** to deselect the tag
5. **Click multiple tags** - content shows items matching ANY selected tag (OR logic)

**Tag Cloud Scoping** ✅ Available Now:

- **Homepage** (`/`): Top 20 tags across entire website
- **Section page** (`/github-copilot`): Top 20 tags for that section only
- **Collection page** (`/github-copilot/videos`): Top 20 tags for that section+collection
- **Content item** (`/blogs/article-name`): Only that article's tags (not limited to 20)

**Tag Sizes** ✅ Available Now:

- **Large tags** (top 25%): Most popular
- **Medium tags** (middle 50%): Moderately popular
- **Small tags** (bottom 25%): Less popular

**Dynamic Tag Counts** ❌ Coming Soon:

- Each tag will show a count in parentheses: `AI (901)` means 901 items would show if you click this tag
- When you select a tag, other tags will update their counts to show the intersection
- Tags that would result in 0 items will become **disabled** (greyed out, not clickable)
- The date slider will affect these counts - change the date range and counts will recalculate

### Using the Excel-Style Tag Dropdown

❌ **Coming Soon** - Can't find your tag in the top 20? The dropdown filter will provide:

1. **Click "Filter by Tags"** dropdown below the date range selector
2. **Search for tags** using the search box (finds tags not in top 20)
3. **Select checkboxes** for multiple tags (OR logic)
4. **Click outside** or press Escape to close dropdown
5. **Tags automatically synchronized** with sidebar tag cloud

**Dropdown Features** (when implemented):

- **Search**: Type to filter tag list (e.g., "azure" shows "Azure", "Azure AI", "Azure Functions")
- **Dynamic Counts**: Each tag shows its intersection count - how many items would show if selected
- **Disabled Tags**: Tags that would result in 0 items are greyed out and disabled
- **Virtual scrolling**: Smooth scrolling even with 100+ tags
- **Select All / Clear All**: Quick actions for bulk selection

### Using Date Range Filters

❌ **Coming Soon** - Filter content by publication date:

1. **Select a preset** button:
   - Last 7 days
   - Last 30 days
   - **Last 90 days** (default)
   - All time

2. **Or use custom date range**:
   - Click "Custom range" to expand
   - Select "From" and "To" dates using date pickers
   - Click "Apply"

**Date Range + Tags** (when implemented):

- Date range **AND** tag filters work together
- Content must match the date range **AND** any selected tag

### Sharing Filtered Views

✅ **Available Now** - Filters are saved in the URL - you can:

- **Copy the URL** to share your filtered view with others
- **Bookmark the URL** to save your filter combination
- **Use browser back/forward** - filter state is preserved

**Example URLs**:

```text
/github-copilot?tags=vscode
/ai?tags=azure,openai
/github-copilot/videos?tags=tutorial
```

**Coming Soon** (with date range feature):

```text
/ai?tags=azure&tags=openai&from=2025-10-01
/github-copilot?from=2025-12-01&to=2026-01-16
/ai/news?tags=generative-ai&from=2025-11-01
```

### Clearing Filters

✅ **Available Now**:

- **Manually deselect** all tags by clicking them again
- **Or navigate to page without URL parameters**

❌ **Coming Soon**:

- **Click "Clear All Filters"** button in sidebar (will show when filters are active)

---

## For Developers: How to Implement Filtering

### Prerequisites

Read domain-specific AGENTS.md files:

- [src/AGENTS.md](/src/AGENTS.md) - .NET development patterns
- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [src/TechHub.Api/AGENTS.md](/src/TechHub.Api/AGENTS.md) - API endpoint patterns
- [src/TechHub.Infrastructure/AGENTS.md](/src/TechHub.Infrastructure/AGENTS.md) - Repository patterns
- [tests/AGENTS.md](/tests/AGENTS.md) - Testing strategies

### Architecture Overview

```text
┌─────────────┐     ┌──────────────┐     ┌─────────────────┐
│   Blazor    │────▶│  API Client  │────▶│   REST API      │
│  Component  │     │  (HttpClient)│     │  (Minimal API)  │
└─────────────┘     └──────────────┘     └─────────────────┘
      │                                            │
      ▼                                            ▼
┌─────────────┐                          ┌─────────────────┐
│   Filter    │                          │   Repository    │
│   State     │                          │  (File-Based)   │
│  Service    │                          └─────────────────┘
└─────────────┘                                   │
      │                                           ▼
      ▼                                  ┌─────────────────┐
┌─────────────┐                          │  Tag Services   │
│ Navigation  │                          │ - TagMatching   │
│  Manager    │                          │ - TagCloud      │
└─────────────┘                          └─────────────────┘
```

### Step 1: Add Filter Components to Page

```razor
@page "/sections/{SectionName}"
@inject FilterStateService FilterState
@inject NavigationManager Navigation
@inject TechHubApiClient ApiClient

<PageHeader Section="@section" />

<div class="section-content-layout">
    <aside class="sidebar">
        <FilterSidebar SectionName="@SectionName"
                       CollectionName="@null"
                       OnFilterChanged="@LoadFilteredContent" />
    </aside>

    <main class="content-grid">
        @if (isLoading)
        {
            <LoadingSkeleton />
        }
        else if (filteredItems.Count == 0)
        {
            <p>No results found. <button @onclick="FilterState.ClearFilters">Clear filters</button></p>
        }
        else
        {
            <ContentItemsGrid Items="@filteredItems" />
        }
    </main>
</div>

@code {
    [Parameter] public string SectionName { get; set; } = string.Empty;

    [SupplyParameterFromQuery(Name = "tags")]
    public string[]? SelectedTags { get; set; }

    [SupplyParameterFromQuery(Name = "from")]
    public string? DateFrom { get; set; }

    [SupplyParameterFromQuery(Name = "to")]
    public string? DateTo { get; set; }

    private Section? section;
    private IReadOnlyList<ContentItemDto> filteredItems = Array.Empty<ContentItemDto>();
    private bool isLoading = false;

    protected override async Task OnParametersSetAsync()
    {
        section = await ApiClient.GetSectionByNameAsync(SectionName);

        // Update filter state from URL
        FilterState.SetFilters(
            SelectedTags ?? Array.Empty<string>(),
            ParseDate(DateFrom),
            ParseDate(DateTo)
        );

        await LoadFilteredContent();
    }

    private async Task LoadFilteredContent()
    {
        isLoading = true;
        StateHasChanged();

        filteredItems = await ApiClient.FilterContentAsync(new FilterRequest
        {
            SelectedTags = FilterState.SelectedTags,
            DateFrom = FilterState.DateFrom,
            DateTo = FilterState.DateTo,
            SectionName = SectionName
        });

        isLoading = false;

        // Update URL with filter state
        Navigation.NavigateTo(Navigation.GetUriWithQueryParameters(FilterState.ToQueryParameters()));
    }

    private DateTimeOffset? ParseDate(string? dateStr) =>
        DateTimeOffset.TryParse(dateStr, out var date) ? date : null;
}
```

### Step 2: Register Services

```csharp
// Program.cs (TechHub.Web)
builder.Services.AddScoped<FilterStateService>();

// Program.cs (TechHub.Api)
builder.Services.AddSingleton<ITagCloudService, TagCloudService>();
builder.Services.AddSingleton<ITagMatchingService, TagMatchingService>();
```

### Step 3: Implement API Endpoints

```csharp
// FilterEndpoints.cs
public static class FilterEndpoints
{
    public static void MapFilterEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/content")
            .WithTags("Filtering")
            .WithOpenApi();

        group.MapGet("/filter", FilterContentAsync)
            .WithName("FilterContent")
            .WithSummary("Filter content by tags and date range");
    }

    private static async Task<IResult> FilterContentAsync(
        [FromQuery] string[]? tags,
        [FromQuery] string? from,
        [FromQuery] string? to,
        [FromQuery] string? section,
        [FromQuery] string? collection,
        IContentRepository repository,
        CancellationToken ct)
    {
        var request = new FilterRequest
        {
            SelectedTags = tags ?? Array.Empty<string>(),
            DateFrom = ParseDate(from),
            DateTo = ParseDate(to),
            SectionName = section,
            CollectionName = collection
        };

        var filtered = await repository.FilterAsync(request, ct);

        return Results.Ok(new FilterResponse
        {
            Items = filtered,
            TotalCount = filtered.Count,
            AppliedFilters = new FilterSummaryDto
            {
                Tags = request.SelectedTags,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                SectionName = request.SectionName,
                CollectionName = request.CollectionName
            }
        });
    }
}
```

### Step 4: Write Tests FIRST (TDD)

```csharp
// FilterEndpointsTests.cs (Integration Test)
public class FilterEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task FilterContent_WithTags_ReturnsMatchingItems()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/content/filter?tags=ai&tags=azure");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<FilterResponse>();
        result.Should().NotBeNull();
        result!.Items.Should().AllSatisfy(item =>
            item.Tags.Should().Contain(t =>
                t.Contains("ai", StringComparison.OrdinalIgnoreCase) ||
                t.Contains("azure", StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public async Task FilterContent_WithDateRange_ReturnsItemsInRange()
    {
        // Arrange
        var client = _factory.CreateClient();
        var from = "2025-10-01";
        var to = "2026-01-16";

        // Act
        var response = await client.GetAsync($"/api/content/filter?from={from}&to={to}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<FilterResponse>();
        result!.Items.Should().AllSatisfy(item =>
            item.PublishDate >= DateTimeOffset.Parse(from) &&
            item.PublishDate <= DateTimeOffset.Parse(to));
    }
}
```

### Step 5: Add E2E Tests (MANDATORY for UI changes)

```csharp
// FilteringTests.cs (E2E Test)
public class FilteringTests : PlaywrightTest
{
    [Test]
    public async Task UserCanFilterByTag()
    {
        // Arrange
        await Page.GotoAsync("https://localhost:5003/sections/ai");

        // Act
        await Page.ClickAsync("button:has-text('Azure')");  // Click tag in cloud
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page.Locator(".content-item-card")).Not.ToBeEmptyAsync();
        await Expect(Page).ToHaveURLAsync("**/sections/ai?tags=azure");
    }

    [Test]
    public async Task UserCanSelectDateRange()
    {
        // Arrange
        await Page.GotoAsync("https://localhost:5003/sections/ai");

        // Act
        await Page.ClickAsync("button:has-text('Last 30 days')");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync("**/sections/ai?from=*&to=*");
    }
}
```

---

## For Testers: Key Scenarios to Verify

### Smoke Tests (Critical Path)

1. **Basic tag filtering**:
   - Navigate to `/sections/ai`
   - Click any tag in sidebar cloud
   - Verify content updates, URL includes `?tags=tagname`
   - Verify tag is highlighted

2. **Date range filtering**:
   - Click "Last 30 days" preset
   - Verify content shows only recent items
   - Verify URL includes `?from=...&to=...`

3. **Combined filtering**:
   - Select tag "Azure"
   - Select "Last 90 days"
   - Verify content matches BOTH filters (AND logic)

4. **Clear filters**:
   - Apply multiple filters
   - Click "Clear All Filters"
   - Verify all filters reset, full content shows

### Tag Cloud Scoping Tests

| Page Type | URL | Expected Tag Cloud |
|-----------|-----|-------------------|
| Homepage | `/` | Top 20 tags across entire site |
| Section | `/sections/ai` | Top 20 tags for AI section only |
| Collection | `/sections/ai/news` | Top 20 tags for AI news only |
| Content | `/blogs/article-name` | Only that article's tags |

### Tag Matching Tests

Verify subset matching works correctly:

| Selected Tag | Should Match | Should NOT Match |
|--------------|--------------|------------------|
| "ai" | "AI", "Generative AI", "Azure AI" | "AIR", "PAIR" |
| "azure" | "Azure", "Azure-AI", "Azure Functions" | "Lazure" |
| "github copilot" | "GitHub Copilot", "Github-Copilot" | "GitHub" alone |

### Accessibility Tests

1. **Keyboard navigation**:
   - Tab through tag cloud (all tags focusable)
   - Space/Enter to select tag
   - Tab through date presets
   - Enter to select preset
   - Tab through tag dropdown
   - Arrow keys to navigate tag list
   - Space to toggle checkboxes

2. **Screen reader**:
   - Tag cloud announces "Filter by {tag}", "Selected" state
   - Date presets announce "Select last {N} days"
   - Dropdown announces "Filter by Tags, expanded/collapsed"
   - Result count announces "Showing {N} results"

3. **Focus visible**:
   - All interactive elements have visible focus indicators
   - Focus order is logical (tag cloud → date selector → dropdown)

### Performance Tests

| Operation | Target | Measurement |
|-----------|--------|-------------|
| Tag filter response | <50ms | DevTools Performance tab |
| Date filter response | <50ms | DevTools Performance tab |
| Tag dropdown search | <300ms | Debounce + filter time |
| Virtual scroll (100 tags) | 60fps | Scroll performance |

### Error Scenarios

1. **Invalid date range**:
   - Custom range: "From" > "To"
   - Expect: Error message "End date must be after start date"

2. **Future date**:
   - Custom range: "From" = tomorrow
   - Expect: Error message "Date cannot be in the future"

3. **No results**:
   - Select very specific tag + old date range
   - Expect: "No results found" + "Clear All Filters" button

4. **Slow network**:
   - Throttle to Slow 3G (DevTools)
   - Apply filters
   - Expect: Loading indicator, no UI blocking

---

## API Examples

### Filter Content

```bash
# Single tag
curl -k "https://localhost:5001/api/content/filter?tags=ai"

# Multiple tags (OR logic)
curl -k "https://localhost:5001/api/content/filter?tags=ai&tags=azure"

# Date range
curl -k "https://localhost:5001/api/content/filter?from=2025-10-01&to=2026-01-16"

# Combined filters
curl -k "https://localhost:5001/api/content/filter?tags=ai&from=2025-10-01&section=ai"
```

### Get Tag Cloud

```bash
# Homepage tag cloud
curl -k "https://localhost:5001/api/tags/cloud?scope=homepage&maxTags=20&lastDays=90"

# Section tag cloud
curl -k "https://localhost:5001/api/tags/cloud?scope=section&section=ai&maxTags=20"

# Collection tag cloud
curl -k "https://localhost:5001/api/tags/cloud?scope=collection&section=ai&collection=news"
```

### Get All Tags (for dropdown)

```bash
# All tags globally
curl -k "https://localhost:5001/api/tags/all"

# All tags in section
curl -k "https://localhost:5001/api/tags/all?section=ai"

# All tags in collection
curl -k "https://localhost:5001/api/tags/all?section=ai&collection=news"
```

---

## Troubleshooting

### Filters not working

1. **Check browser console** for JavaScript errors
2. **Verify URL parameters** are correct (`?tags=...&from=...`)
3. **Test API directly** using curl (see examples above)
4. **Clear browser cache** and reload

### Tag cloud not showing

1. **Verify tag calculation** - may be no tags with ≥5 uses in last 90 days
2. **Check scope** - content item pages show only article tags, not top 20
3. **Inspect API response** - `/api/tags/cloud?scope=...`

### Performance issues

1. **Check virtual scrolling** activates at 50+ tags (inspect DOM)
2. **Verify debouncing** on tag dropdown search (300ms delay)
3. **Test with production build** (`dotnet run -c Release`)
4. **Profile with DevTools** Performance tab

### Accessibility issues

1. **Test keyboard navigation** - Tab through all controls
2. **Verify ARIA labels** - Inspect elements in DevTools
3. **Check focus indicators** - All interactive elements visible on focus
4. **Run axe DevTools** - Automated accessibility scanning

---

## Quick Reference

**Tag Cloud**: Top 20 tags, last 90 days, scoped to page, quantile-sized  
**Tag Dropdown**: All tags, searchable, virtual scrolling at 50+  
**Date Presets**: 7/30/90 days, all time, default 90 days  
**Tag Matching**: Case-insensitive, word boundaries, punctuation-agnostic  
**Filter Logic**: Tags (OR), Date (AND), Combined (AND)  
**URL State**: Deep linking, browser history, shareable  

**API Endpoints**:

- `GET /api/content/filter` - Filter content
- `GET /api/tags/cloud` - Get tag cloud
- `GET /api/tags/all` - Get all tags

**Key Services**:

- `FilterStateService` (Blazor) - Client state management
- `TagCloudService` (API) - Tag cloud calculation
- `TagMatchingService` (API) - Subset matching logic

---

## Next Steps

1. ✅ Quickstart guide created
2. ⏭️ Update agent context (run update-agent-context.ps1)
3. ⏭️ Re-evaluate Constitution Check
4. ⏭️ Generate tasks.md (/speckit.tasks command)
