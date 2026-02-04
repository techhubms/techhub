# Data Model: Sidebar Content Filtering

**Feature**: 001-filtering-system  
**Date**: 2026-01-16  
**Updated**: 2026-02-03  
**Phase**: Phase 1 - Design & Contracts

## Implementation Status

| Model | Status | Location |
|-------|--------|----------|
| FilterRequest | ✅ Complete | `src/TechHub.Core/Models/Filter/` |
| FilterResponse | ✅ Complete | `src/TechHub.Core/Models/Filter/` |
| FilterSummary | ✅ Complete | `src/TechHub.Core/Models/Filter/` |
| TagCloudItem | ✅ Complete | `src/TechHub.Core/Models/Tags/` |
| TagCloudRequest | ✅ Complete | `src/TechHub.Core/Models/Tags/` |
| AllTagsResponse | ✅ Complete | `src/TechHub.Core/Models/Tags/` |
| TagWithCount | ✅ Complete | `src/TechHub.Core/Models/Tags/` |
| FacetRequest | ✅ Complete | `src/TechHub.Core/Models/Facets/` |
| FacetResults | ✅ Complete | `src/TechHub.Core/Models/Facets/` |
| TagCountsResponse | ❌ TODO | For dynamic counts API endpoint |
| TagWithCountAndState | ❌ TODO | Tag with count + disabled flag |
| DateRangePreset | ❌ TODO | Enum for date range presets |

> **Note**: Implementation uses `Models/` namespace, not `DTOs/` as originally specified.

## Domain Entities

### FilterRequest (Model - API Input)

**File**: `src/TechHub.Core/Models/Filter/FilterRequest.cs` ✅

```csharp
namespace TechHub.Core.Models.Filter;

/// <summary>
/// Request DTO for content filtering operations
/// </summary>
public record FilterRequest
{
    /// <summary>
    /// Selected tags for filtering (OR logic within tags)
    /// </summary>
    public string[] SelectedTags { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Start of date range filter (inclusive)
    /// </summary>
    public DateTimeOffset? DateFrom { get; init; }

    /// <summary>
    /// End of date range filter (inclusive)
    /// </summary>
    public DateTimeOffset? DateTo { get; init; }

    /// <summary>
    /// Section scope for filtering (optional)
    /// </summary>
    public string? SectionName { get; init; }

    /// <summary>
    /// Collection scope for filtering (optional)
    /// </summary>
    public string? CollectionName { get; init; }
}
```

**Validation Rules**:

- `SelectedTags`: Each tag must be non-empty, max 50 characters, alphanumeric + spaces/hyphens
- `DateFrom` / `DateTo`: If both provided, DateFrom must be ≤ DateTo
- `DateFrom` / `DateTo`: Cannot be in the future (validated against `Europe/Brussels` timezone)
- `SectionName`: Must exist in configured sections (from appsettings.json)
- `CollectionName`: Must exist in configured collections for the section

---

### FilterResponse (Model - API Output)

**File**: `src/TechHub.Core/Models/Filter/FilterResponse.cs` ✅

```csharp
namespace TechHub.Core.Models.Filter;

/// <summary>
/// Response DTO containing filtered content items and metadata
/// </summary>
public record FilterResponse
{
    /// <summary>
    /// Filtered content items matching the criteria
    /// </summary>
    public required IReadOnlyList<ContentItemDto> Items { get; init; }

    /// <summary>
    /// Total count of filtered items
    /// </summary>
    public required int TotalCount { get; init; }

    /// <summary>
    /// Summary of applied filters for display
    /// </summary>
    public required FilterSummaryDto AppliedFilters { get; init; }
}
```

---

### FilterSummary (Model - Filter Metadata)

**File**: `src/TechHub.Core/Models/Filter/FilterSummary.cs` ✅

```csharp
namespace TechHub.Core.Models.Filter;

/// <summary>
/// Summary of filters applied to the current result set
/// </summary>
public record FilterSummaryDto
{
    /// <summary>
    /// Tags used in filtering
    /// </summary>
    public required string[] Tags { get; init; }

    /// <summary>
    /// Date range start (if applied)
    /// </summary>
    public DateTimeOffset? DateFrom { get; init; }

    /// <summary>
    /// Date range end (if applied)
    /// </summary>
    public DateTimeOffset? DateTo { get; init; }

    /// <summary>
    /// Section scope (if applied)
    /// </summary>
    public string? SectionName { get; init; }

    /// <summary>
    /// Collection scope (if applied)
    /// </summary>
    public string? CollectionName { get; init; }

    /// <summary>
    /// Whether any filters are active
    /// </summary>
    public bool HasActiveFilters => Tags.Length > 0 || DateFrom.HasValue || DateTo.HasValue;
}
```

---

### TagCloudItem (Model - Tag Display)

**File**: `src/TechHub.Core/Models/Tags/TagCloudItem.cs` ✅

```csharp
namespace TechHub.Core.Models.Tags;

/// <summary>
/// Tag display information for tag cloud rendering
/// </summary>
public record TagCloudItem
{
    /// <summary>
    /// Tag name (normalized for display)
    /// </summary>
    public required string Tag { get; init; }

    /// <summary>
    /// Number of content items with this tag in current scope
    /// </summary>
    public required int Count { get; init; }

    /// <summary>
    /// Visual size category for tag cloud display
    /// </summary>
    public required TagSize Size { get; init; }
}

/// <summary>
/// Tag size categories for quantile-based sizing
/// </summary>
public enum TagSize
{
    /// <summary>Bottom 25% of tag cloud (least popular within top 20)</summary>
    Small = 0,

    /// <summary>Middle 50% of tag cloud (moderately popular)</summary>
    Medium = 1,

    /// <summary>Top 25% of tag cloud (most popular)</summary>
    Large = 2
}
```

---

### TagCloudRequest (Model - Tag Cloud Query)

**File**: `src/TechHub.Core/Models/Tags/TagCloudRequest.cs` ✅

```csharp
namespace TechHub.Core.Models.Tags;

/// <summary>
/// Request DTO for tag cloud calculation
/// </summary>
public record TagCloudRequest
{
    /// <summary>
    /// Scope of tag cloud calculation
    /// </summary>
    public required TagCloudScope Scope { get; init; }

    /// <summary>
    /// Section name (required for Section/Collection/Content scopes)
    /// </summary>
    public string? SectionName { get; init; }

    /// <summary>
    /// Collection name (required for Collection scope)
    /// </summary>
    public string? CollectionName { get; init; }

    /// <summary>
    /// Content item slug (required for Content scope)
    /// </summary>
    public string? Slug { get; init; }

    /// <summary>
    /// Maximum number of tags to return (default: 20)
    /// </summary>
    public int MaxTags { get; init; } = 20;

    /// <summary>
    /// Minimum usage count for tags (default: 5)
    /// </summary>
    public int MinUses { get; init; } = 5;

    /// <summary>
    /// Include only content from last N days (default: 90)
    /// Null = all time
    /// </summary>
    public int? LastDays { get; init; } = 90;
}

/// <summary>
/// Tag cloud scoping strategy
/// </summary>
public enum TagCloudScope
{
    /// <summary>Top tags across entire website</summary>
    Homepage = 0,

    /// <summary>Top tags for specific section</summary>
    Section = 1,

    /// <summary>Top tags for section/collection combination</summary>
    Collection = 2,

    /// <summary>All tags for specific content item (no limits)</summary>
    Content = 3
}
```

**Validation Rules**:

- `Scope = Section`: SectionName required
- `Scope = Collection`: SectionName + CollectionName required
- `Scope = Content`: SectionName + CollectionName + ContentId required
- `MaxTags`: Range 1-100
- `MinUses`: Range 1-1000
- `LastDays`: Range 1-365 or null

---

### AllTagsResponse (Model - Tag Dropdown Data)

**File**: `src/TechHub.Core/Models/Tags/AllTagsResponse.cs` ✅

```csharp
namespace TechHub.Core.Models.Tags;

/// <summary>
/// Response containing all available tags with counts for dropdown filter
/// </summary>
public record AllTagsResponse
{
    /// <summary>
    /// All available tags in the current scope (sorted alphabetically)
    /// </summary>
    public required IReadOnlyList<TagWithCount> Tags { get; init; }

    /// <summary>
    /// Total number of unique tags
    /// </summary>
    public required int TotalTagCount { get; init; }
}

/// <summary>
/// Tag with usage count for dropdown display
/// </summary>
public record TagWithCount
{
    /// <summary>
    /// Tag name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Number of items with this tag
    /// </summary>
    public required int Count { get; init; }
}
```

---

### TagCountsResponse (Model - Dynamic Tag Counts)

**File**: `src/TechHub.Core/Models/Tags/TagCountsResponse.cs` ❌ TODO

> **New in 2026-02-03**: Supports dynamic tag counts feature where each tag shows how many items would remain if that tag is selected, based on current filter state.

```csharp
namespace TechHub.Core.Models.Tags;

/// <summary>
/// Response containing dynamic tag counts based on current filter state.
/// Used for Excel-style dropdown and tag cloud with counts (e.g., "AI (901)").
/// </summary>
public record TagCountsResponse
{
    /// <summary>
    /// Tags with their dynamic counts based on current filter state.
    /// Count = number of items that would remain if this tag is selected.
    /// </summary>
    public required IReadOnlyList<DynamicTagCount> Tags { get; init; }

    /// <summary>
    /// Total items matching current filters (before any new tag selection)
    /// </summary>
    public required int CurrentTotal { get; init; }

    /// <summary>
    /// Section scope for the calculation
    /// </summary>
    public string? SectionName { get; init; }

    /// <summary>
    /// Collection scope for the calculation
    /// </summary>
    public string? CollectionName { get; init; }
}

/// <summary>
/// Tag with dynamic count showing intersection with current filter state.
/// </summary>
public record DynamicTagCount
{
    /// <summary>
    /// Tag name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Number of items that would remain if this tag is added to current filters.
    /// For active tags: shows current filtered count.
    /// For inactive tags: shows intersection count (items with this tag AND current filter).
    /// </summary>
    public required int Count { get; init; }

    /// <summary>
    /// Whether this tag is currently selected/active
    /// </summary>
    public required bool IsSelected { get; init; }

    /// <summary>
    /// Whether this tag is disabled (would result in 0 items)
    /// </summary>
    public required bool IsDisabled { get; init; }
}
```

**Calculation Rules**:

- **No filters active**: Count = total items with this tag in scope
- **Tags selected**: Count = items with (selected tags OR this tag) AND date range
- **Date range active**: Count = items with this tag within date range
- **IsDisabled**: `true` when Count = 0 (clicking would show no results)

**API Endpoint**: `GET /api/sections/{section}/collections/{collection}/tagcounts`

**Query Parameters**:

| Parameter | Type | Description |
|-----------|------|-------------|
| `selectedTags` | string[] | Currently selected tags (comma-separated) |
| `dateFrom` | DateTimeOffset? | Start of date range filter |
| `dateTo` | DateTimeOffset? | End of date range filter |

**Example Response**:

```json
{
  "tags": [
    { "name": "AI", "count": 901, "isSelected": true, "isDisabled": false },
    { "name": "GitHub Copilot", "count": 245, "isSelected": false, "isDisabled": false },
    { "name": "Obsolete Framework", "count": 0, "isSelected": false, "isDisabled": true }
  ],
  "currentTotal": 901,
  "sectionName": "ai",
  "collectionName": "blogs"
}
```

---

## Component Models (Blazor Frontend)

### FilterState (Client-Side State Model)

```csharp
namespace TechHub.Web.Models;

/// <summary>
/// Client-side filter state with UI metadata
/// </summary>
public class FilterState
{
    public string[] SelectedTags { get; set; } = Array.Empty<string>();
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    public string? SectionName { get; set; }
    public string? CollectionName { get; set; }

    // UI State
    public bool IsLoading { get; set; }
    public string? ErrorMessage { get; set; }
    public int ResultCount { get; set; }

    // Computed properties
    public bool HasActiveFilters => SelectedTags.Length > 0 || DateFrom.HasValue || DateTo.HasValue;
    public string ActiveFiltersText => HasActiveFilters 
        ? $"{SelectedTags.Length} tag(s), {GetDateRangeText()}" 
        : "No filters";

    private string GetDateRangeText()
    {
        if (!DateFrom.HasValue && !DateTo.HasValue) return "All time";
        if (DateFrom.HasValue && DateTo.HasValue) return $"{DateFrom:MMM d} - {DateTo:MMM d}";
        if (DateFrom.HasValue) return $"From {DateFrom:MMM d, yyyy}";
        return $"Until {DateTo:MMM d, yyyy}";
    }

    public Dictionary<string, object?> ToQueryParameters()
    {
        return new Dictionary<string, object?>
        {
            ["tags"] = SelectedTags.Length > 0 ? SelectedTags : null,
            ["from"] = DateFrom?.ToString("yyyy-MM-dd"),
            ["to"] = DateTo?.ToString("yyyy-MM-dd")
        };
    }
}
```

---

### DateRangePreset (UI Model)

```csharp
namespace TechHub.Web.Models;

/// <summary>
/// Preset date range options for quick selection
/// </summary>
public record DateRangePreset
{
    public required string Label { get; init; }
    public required int? Days { get; init; }  // Null = All time

    public static readonly DateRangePreset[] Defaults = new[]
    {
        new DateRangePreset { Label = "Last 7 days", Days = 7 },
        new DateRangePreset { Label = "Last 30 days", Days = 30 },
        new DateRangePreset { Label = "Last 90 days", Days = 90 },
        new DateRangePreset { Label = "All time", Days = null }
    };

    public (DateTimeOffset? From, DateTimeOffset To) GetDateRange()
    {
        var to = DateTimeOffset.UtcNow;
        var from = Days.HasValue ? to.AddDays(-Days.Value) : (DateTimeOffset?)null;
        return (from, to);
    }
}
```

---

## Service Interfaces

### ITagCloudService

```csharp
namespace TechHub.Core.Interfaces;

/// <summary>
/// Service for calculating tag clouds with scoping and sizing
/// </summary>
public interface ITagCloudService
{
    /// <summary>
    /// Calculate tag cloud for given scope
    /// </summary>
    Task<IReadOnlyList<TagCloudItem>> CalculateTagCloudAsync(
        TagCloudRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all available tags in scope (for dropdown)
    /// </summary>
    Task<AllTagsResponse> GetAllTagsAsync(
        string? sectionName = null,
        string? collectionName = null,
        CancellationToken cancellationToken = default);
}
```

---

### ITagMatchingService

```csharp
namespace TechHub.Core.Interfaces;

/// <summary>
/// Service for tag normalization and subset matching
/// </summary>
public interface ITagMatchingService
{
    /// <summary>
    /// Normalize tag for comparison (lowercase, trim, remove punctuation)
    /// </summary>
    string NormalizeTag(string tag);

    /// <summary>
    /// Check if content tag matches selected tag using word boundaries
    /// </summary>
    bool IsMatch(string contentTag, string selectedTag);

    /// <summary>
    /// Filter items by tags using OR logic
    /// </summary>
    IEnumerable<T> FilterByTags<T>(
        IEnumerable<T> items,
        string[] selectedTags,
        Func<T, IEnumerable<string>> getItemTags);
}
```

---

### IContentRepository Extensions

```csharp
namespace TechHub.Core.Interfaces;

public partial interface IContentRepository
{
    /// <summary>
    /// Filter content by tags, date range, and scope
    /// </summary>
    Task<IReadOnlyList<ContentItem>> FilterAsync(
        FilterRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get content items within date range
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetByDateRangeAsync(
        DateTimeOffset? from,
        DateTimeOffset? to,
        string? sectionName = null,
        string? collectionName = null,
        CancellationToken cancellationToken = default);
}
```

---

## Component Hierarchy

### Blazor Component Tree

```text
Section.razor / SectionCollection.razor (Pages)
├── @inject FilterStateService FilterState
├── @inject NavigationManager Navigation
├── @inject TechHubApiClient ApiClient
│
├── FilterSidebar.razor (NEW - Container Component)
│   ├── Props:
│   │   ├── SectionName (string)
│   │   ├── CollectionName (string?)
│   │   └── OnFilterChanged (EventCallback<FilterState>)
│   │
│   ├── SidebarTagCloud.razor (NEW - Tag Cloud)
│   │   ├── Props:
│   │   │   ├── Tags (TagCloudItem[])
│   │   │   ├── SelectedTags (string[])
│   │   │   └── OnTagToggled (EventCallback<string>)
│   │   └── Features:
│   │       ├── Quantile-based sizing (3 tiers)
│   │       ├── Click to toggle selection
│   │       ├── Tooltip with count on hover
│   │       └── Keyboard navigation (Tab, Space, Enter)
│   │
│   ├── DateRangeSelector.razor (NEW - Date Filter)
│   │   ├── Props:
│   │   │   ├── DateFrom (DateTimeOffset?)
│   │   │   ├── DateTo (DateTimeOffset?)
│   │   │   └── OnDateRangeChanged (EventCallback<(DateTimeOffset?, DateTimeOffset?)>)
│   │   ├── Features:
│   │   │   ├── Preset buttons (7/30/90 days, All time)
│   │   │   ├── Optional custom date pickers
│   │   │   └── Default: Last 90 days
│   │   └── State:
│   │       ├── ActivePreset (int?)
│   │       └── ShowCustomRange (bool)
│   │
│   ├── TagDropdownFilter.razor (NEW - Excel-Style Dropdown)
│   │   ├── Props:
│   │   │   ├── AllTags (TagWithCount[])
│   │   │   ├── SelectedTags (string[])
│   │   │   └── OnTagsChanged (EventCallback<string[]>)
│   │   ├── Features:
│   │   │   ├── Search box (debounced 300ms)
│   │   │   ├── Checkboxes for multi-select
│   │   │   ├── Virtual scrolling (50+ tags)
│   │   │   ├── Select All / Clear All
│   │   │   └── Tag counts display
│   │   └── State:
│   │       ├── SearchQuery (string)
│   │       ├── FilteredTags (TagWithCount[])
│   │       └── IsOpen (bool)
│   │
│   └── ClearFiltersButton.razor (NEW - Reset)
│       ├── Props: OnClearFilters (EventCallback)
│       ├── Features: Single click to clear all
│       └── Display: Only when HasActiveFilters
│
└── ContentItemsGrid.razor (MODIFIED - Display Filtered Results)
    ├── Props: FilteredItems (ContentItemDto[])
    ├── Features:
    │   ├── Virtualize for large result sets
    │   └── Loading skeleton during filtering
    │
    └── ContentItemCard.razor (MODIFIED - Add Tag Badges)
        └── ContentItemTagBadges.razor (NEW - Tag Display)
            ├── Props: Tags (string[])
            ├── Features:
            │   ├── Clickable badges
            │   ├── Navigate to filtered view
            │   └── Horizontal scroll/wrap
            └── OnTagClicked: Navigate with tag in URL
```

---

## State Flow Diagram

```text
┌─────────────────────────────────────────────────────────────────┐
│                    User Interaction                              │
│  (Click tag, Select date, Search dropdown)                      │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│               Component Event Handler                            │
│  - SidebarTagCloud.OnTagClicked()                               │
│  - DateRangeSelector.OnPresetSelected()                         │
│  - TagDropdownFilter.OnCheckboxChanged()                        │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│               FilterStateService.SetFilters()                    │
│  - Update internal state (tags, dates)                          │
│  - Emit OnChange event                                          │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│               NavigationManager.NavigateTo()                     │
│  - Build URL with query parameters                              │
│  - Navigate (triggers page re-render)                           │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│          Section.razor OnParametersSet()                         │
│  - Read [SupplyParameterFromQuery] parameters                   │
│  - Update FilterStateService from URL                           │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│          TechHubApiClient.FilterContentAsync()                   │
│  - Build FilterRequest DTO                                      │
│  - Call GET /api/content/filter                                 │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│          API FilterEndpoint                                      │
│  - Validate request                                             │
│  - Call IContentRepository.FilterAsync()                        │
│  - Return FilterResponse DTO                                    │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│          FileBasedContentRepository.FilterAsync()                │
│  - Load content items from markdown files                       │
│  - Apply tag filter (via TagMatchingService)                    │
│  - Apply date filter (DateTimeOffset comparison)                │
│  - Return filtered ContentItem[]                                │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│          Section.razor Render Update                             │
│  - Update filteredItems property                                │
│  - StateHasChanged() triggers re-render                         │
│  - ContentItemsGrid displays filtered results                   │
│  - Update FilterSidebar with current state                      │
└─────────────────────────────────────────────────────────────────┘
```

---

## Validation Rules Summary

| Field | Rules | Error Message |
|-------|-------|---------------|
| `SelectedTags[]` | Max 50 chars per tag, alphanumeric + spaces/hyphens | "Invalid tag format" |
| `DateFrom` | Not in future, ≤ DateTo if both present | "Date cannot be in the future" |
| `DateTo` | Not in future, ≥ DateFrom if both present | "End date must be after start date" |
| `SectionName` | Must exist in appsettings.json | "Section '{name}' not found" |
| `CollectionName` | Must exist for section | "Collection '{name}' not found in section" |
| `MaxTags` | Range 1-100 | "Max tags must be between 1 and 100" |
| `MinUses` | Range 1-1000 | "Min uses must be between 1 and 1000" |
| `LastDays` | Range 1-365 or null | "Last days must be between 1 and 365" |

---

## Database Schema (N/A - File-Based)

**Note**: Tech Hub uses file-based markdown content storage. No database schema required. Content filtering is performed in-memory from parsed markdown files.

**Content Source**: `collections/` directory with YAML frontmatter

---

## Performance Considerations

### Tag Cloud Calculation

```csharp
// Cache tag cloud results per scope
private readonly MemoryCache _tagCloudCache = new(new MemoryCacheOptions());

public async Task<IReadOnlyList<TagCloudItem>> CalculateTagCloudAsync(TagCloudRequest request, CancellationToken ct)
{
    var cacheKey = $"tagcloud:{request.Scope}:{request.SectionName}:{request.CollectionName}:{request.LastDays}";
    
    if (_tagCloudCache.TryGetValue(cacheKey, out IReadOnlyList<TagCloudItem>? cached))
        return cached!;

    var items = await GetContentInScopeAsync(request, ct);
    var tagCloud = CalculateCloud(items, request.MaxTags, request.MinUses);

    _tagCloudCache.Set(cacheKey, tagCloud, TimeSpan.FromMinutes(15));
    return tagCloud;
}
```

**Cache Strategy**:

- Tag clouds: 15-minute TTL (infrequent content changes)
- All tags list: 15-minute TTL
- Filtered results: No cache (user-specific, URL-driven)

---

## Next Steps

1. ✅ Data models defined
2. ⏭️ Generate API contracts (OpenAPI schemas)
3. ⏭️ Generate quickstart guide
4. ⏭️ Update agent context
5. ⏭️ Re-evaluate Constitution Check
