# Content Filtering System

This document describes how content filtering works in Tech Hub, covering tag filtering, date range filtering, and text search — the most crucial features of the website for content discovery.

**Related Documentation**:

- [Content API](content-api.md) - REST API endpoints for content retrieval
- [Database Configuration](database.md) - Database providers that support filtering

## Text Search

### Search Behavior

The search box allows users to filter content by typing text queries. Search is performed across:

- **Title**: Article/post titles
- **Excerpt/Description**: Brief descriptions or excerpts
- **Content**: Full article body text (when using full-text search providers)
- **Tags**: All tags associated with the content

**Prefix Matching**: Search supports prefix matching, allowing users to find content by typing partial words:

- Search `rein` → Matches "**Rein**ier", "**rein**deer", "**rein**force"
- Search `cop` → Matches "**Cop**ilot", "**cop**y", "**cop**per"
- Search `tech` → Matches "**Tech**Hub", "**tech**nology", "**tech**nical"

This enables auto-complete-style search behavior where users don't need to type complete words.

### Search Implementation

**Frontend**: The `SidebarSearch` component provides a search input with:

- 300ms debounce delay to reduce API calls while typing
- Clear button (X icon) to remove the search query
- Keyboard support (Escape key clears the search)
- URL parameter synchronization (`?search=query`)

**Backend**: Full-text search is implemented at the database level:

- **PostgreSQL**: Uses `tsvector` with `ts_rank` relevance scoring and `to_tsquery` for prefix support
- Supports weighted search (title > excerpt > content)
- Automatically appends wildcards for prefix matching (`copilot` → `copilot:*` for PostgreSQL)

### Search + Filter Combination

Search combines with other filters using **AND logic**:

- Search "copilot" + tag "ai" → Items containing "copilot" AND tagged with "ai"
- Search "blazor" + last 30 days → Items containing "blazor" AND published in last 30 days
- All filters (search + tags + date range) work together

### URL Parameters

Search state is preserved in the URL for sharing and bookmarking:

```text
?search=copilot                          # Search only
?search=copilot&tags=ai                  # Search + tag filter
?search=copilot&tags=ai&from=2024-01-01  # Search + tag + date filter
```

**Implementation Details**:

- Query parameter: `search` (URL-encoded)
- Empty/removed when search is cleared
- Combined with `tags`, `from`, `to` parameters
- Supports browser back/forward navigation

## Tag Storage and Expansion

### How Tags Are Stored

When content is loaded into the database, tags are processed as follows:

**Original Tag**: `"Azure AI Foundry"`

**Storage in `content_items.tags_csv`**: `,Azure AI Foundry,` (original tag preserved)

**Expansion in `content_tags_expanded.tag_word`**: Each tag is split into individual words AND the full tag is stored:

- `azure ai foundry` (full tag, lowercase)
- `azure` (individual word)
- `ai` (individual word)
- `foundry` (individual word)

### Why Tag Expansion?

Tag expansion enables **word-level matching** without requiring exact multi-word matches. This allows users to find content by typing partial tag names.

## Tag Filtering Behavior

### Single Tag Search

When searching for a single tag, the system performs **exact match** on the `tag_word` column:

**Search**: `tags=ai`
**Query**: `WHERE tag_word = 'ai'`
**Matches**: Articles with tags like "AI", "Azure AI Foundry", "AI Agent", etc.

**Search**: `tags=azure ai foundry`
**Query**: `WHERE tag_word = 'azure ai foundry'`
**Matches**: Only articles with the exact tag "Azure AI Foundry"

**Search**: `tags=azure ai foundry bla`
**Query**: `WHERE tag_word = 'azure ai foundry bla'`
**Matches**: Nothing (tag doesn't exist)

### Multiple Tag Search (AND Logic)

When searching for multiple tags (comma-separated), the system finds items that have **ALL** tags:

**Search**: `tags=ai,azure`
**Query**: `WHERE tag_word IN ('ai', 'azure') GROUP BY slug HAVING COUNT(DISTINCT tag_word) = 2`
**Matches**: Only articles that have BOTH "ai" AND "azure" as tag words

**Search**: `tags=copilot,azure,devops`
**Matches**: Only articles with all three tag words

### Examples

#### Example 1: Article with Tag "Azure AI Foundry"

**Stored tags**:

- Full tag: `Azure AI Foundry`
- Expanded words: `azure ai foundry`, `azure`, `ai`, `foundry`

**Search Results**:

- `tags=ai` → ✅ Match (has "ai")
- `tags=azure` → ✅ Match (has "azure")
- `tags=foundry` → ✅ Match (has "foundry")
- `tags=azure ai foundry` → ✅ Match (has full tag)
- `tags=ai,azure` → ✅ Match (has both)
- `tags=ai foundry` → ❌ No match ("ai foundry" is not a stored tag_word - we only store the full "azure ai foundry" and individual words)
- `tags=ai,blockchain` → ❌ No match (missing "blockchain")

#### Example 2: Article with Tags "AI" and "GitHub Copilot"

**Stored tags**:

- Tag 1 expanded: `ai`
- Tag 2 expanded: `github copilot`, `github`, `copilot`

**Search Results**:

- `tags=ai` → ✅ Match
- `tags=copilot` → ✅ Match
- `tags=github` → ✅ Match
- `tags=github copilot` → ✅ Match
- `tags=ai,copilot` → ✅ Match (has both)
- `tags=ai,azure` → ❌ No match (missing "azure")

## Implementation Details

### Query Structure

```sql
SELECT collection_name, slug 
FROM content_tags_expanded
WHERE tag_word IN @tags
  AND sections_bitmask & @sectionMask > 0
  AND collection_name IN @collections
GROUP BY collection_name, slug
HAVING COUNT(DISTINCT tag_word) = @tagCount
ORDER BY MAX(date_epoch) DESC
```

**Key Points**:

- `tag_word IN @tags` - exact match on tag words (no substring matching)
- `GROUP BY` - prevents duplicate results when item has multiple matching tags
- `HAVING COUNT(DISTINCT tag_word) = @tagCount` - enforces AND logic for multiple tags
- `MAX(date_epoch)` - orders by newest date when item appears in multiple collections

### Duplicate Prevention

The `GROUP BY collection_name, slug` clause ensures each content item appears only once in results, even if it matches multiple tag words.

**Without GROUP BY**:

```text
Search: tags=ai,azure
Results: article-1 (matched "ai"), article-1 (matched "azure") ← DUPLICATE
```

**With GROUP BY**:

```text
Search: tags=ai,azure
Results: article-1 ← Single result
```

## Content Filtering API

### GET /api/sections/{sectionName}/collections/{collectionName}/items

This is the primary endpoint for content retrieval and filtering. Using `all` for both section and collection (`/api/sections/all/collections/all/items`) allows filtering content across the entire site.

**Query Parameters**:

- `q` (optional): Text search query (searches title, description, tags)
- `tags` (optional): Comma-separated tags - content must have ALL tags (AND logic)
- `subcollection` (optional): Filter by subcollection
- `lastDays` (optional): Filter to content from last N days. **Default behavior**: When no `lastDays`, `from`, or `to` parameters are provided, a default 90-day filter is automatically applied (configured via `AppSettings:Filtering:TagCloud:DefaultDateRangeDays`). Pass `lastDays=0` to explicitly disable date filtering and return all content regardless of date.
- `take` (optional): Number of items to return (default: 20, max: 50)
- `skip` (optional): Number of items to skip (for pagination)

**Response**: `200 OK`

**Examples**:

Filter by sections (use specific section instead of 'all'):

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/all/items"
```

Filter by collections (use specific collection instead of 'all'):

```bash
curl -k "https://localhost:5001/api/sections/all/collections/news/items"
```

Global text search:

```bash
curl -k "https://localhost:5001/api/sections/all/collections/all/items?q=blazor"
```

Combine search with tags:

```bash
curl -k "https://localhost:5001/api/sections/all/collections/all/items?q=copilot&tags=ai,azure"
```

Filter by tags (AND logic):

```bash
curl -k "https://localhost:5001/api/sections/all/collections/all/items?tags=copilot,azure"
```

Complex multi-criteria filter (search + tags + date):

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/videos/items?q=blazor&tags=copilot&lastDays=30"
```

## Tag Statistics API

### GET /api/sections/{sectionName}/collections/{collectionName}/tags

Get a tag cloud with quantile-based sizing for visual representation. Using `all` allows retrieving tag stats for the entire site.

**Supports dynamic tag counts**: When filter parameters are provided (`tags`, `from`, `to`), the counts update to show how many items would remain if that tag is selected.

**Parameters**:

- `maxTags` (query, optional): Maximum number of tags (default: 20)
- `minUses` (query, optional): Minimum tag usage count (default: 5)
- `lastDays` (query, optional): Filter to content from last N days (default: 90 days via `AppSettings:Filtering:TagCloud:DefaultDateRangeDays`)
- `tags` (query, optional): Comma-separated list of currently selected tags for dynamic count calculation
- `tagsToCount` (query, optional): Comma-separated list of specific tags to get counts for. Returns counts only for these tags instead of the top N popular tags. Display names in the response preserve the casing from this parameter.
- `from` (query, optional): Start date for custom range (ISO 8601 format, e.g., `2024-01-15`)
- `to` (query, optional): End date for custom range (ISO 8601 format, e.g., `2024-06-15`)

**Static Counts (No Filter)**:

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/all/tags?maxTags=20"
```

Response shows total items with each tag:

```json
[
  { "tag": "AI", "count": 901, "size": 2 },
  { "tag": "GitHub Copilot", "count": 567, "size": 2 },
  { "tag": "Azure", "count": 423, "size": 1 }
]
```

**Dynamic Counts (With Tags Filter)**:

When tags are selected, counts show items matching **ALL selected tags AND this tag** (intersection):

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/all/tags?tags=ai&maxTags=20"
```

Response shows intersection counts:

```json
[
  { "tag": "GitHub Copilot", "count": 245, "size": 2 },  // Items with BOTH AI AND Copilot
  { "tag": "Azure", "count": 178, "size": 1 },           // Items with BOTH AI AND Azure
  { "tag": "Machine Learning", "count": 0, "size": 0 }   // No items with BOTH (would be disabled in UI)
]
```

**Dynamic Counts (With Date Range)**:

Date range filters affect tag counts:

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/all/tags?from=2024-01-01&to=2024-06-30&maxTags=20"
```

Counts reflect only items within the date range.

**Combined Filters**:

All filters can be combined:

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/all/tags?tags=ai,copilot&from=2024-01-01&to=2024-06-30&maxTags=20"
```

Counts show items with **AI AND Copilot AND this tag** within the date range.

**Error Handling**:

Invalid date formats return `400 Bad Request`:

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/all/tags?from=invalid-date"
# Returns: 400 Bad Request with error message
```

**Response**:

```json
[
  { "tag": "ai", "count": 152, "size": 2 },
  { "tag": "github-copilot", "count": 89, "size": 2 },
  { "tag": "azure", "count": 67, "size": 1 }
]
```

### Tag Size Algorithm (Quantile-Based)

Tag sizes are calculated using quantile distribution based on actual count values to ensure appropriate visual representation:

| Size | Criteria | CSS Class | Visual Weight |
|------|----------|-----------|---------------|
| Large | Count >= 75th percentile value | `tag-size-large` | Bold, larger font |
| Medium | Count >= 25th percentile value | `tag-size-medium` | Normal weight |
| Small | Count < 25th percentile value | `tag-size-small` | Lighter weight |

**How It Works:**

1. Tags are sorted by count (descending: highest first)
2. The count VALUE at the 25th percentile position becomes the "high threshold"
3. The count VALUE at the 75th percentile position becomes the "low threshold"  
4. Each tag is assigned a size based on these threshold values:
   - Count >= high threshold → Large
   - Count >= low threshold → Medium
   - Count < low threshold → Small

**Example:** Tags with counts [100, 95, 90, 50, 48, 45, 10, 8, 5, 3]

- High threshold (at 25% position) = 90
- Low threshold (at 75% position) = 8
- Result: 100, 95, 90 are Large | 50, 48, 45, 10, 8 are Medium | 5, 3 are Small

**Why Value-Based Quantiles?**

- **Similar counts = similar sizes**: Tags with nearly identical counts (e.g., 3, 3, 2, 2, 2) will have uniform sizing instead of artificially spanning all three size categories
- **Adapts to data distribution**: When all tags have similar counts, most will have the same size (appropriate). When counts vary widely, sizes distribute across all three categories (also appropriate)
- **Prevents misleading emphasis**: A tag with count 3 won't appear much larger than a tag with count 2 just because of its position in the sorted list

**Edge Cases:**

- 1-2 tags: All get Medium size
- Very uniform counts (all within 2x range): Most/all tags will have same size

**Size Group Normalization:**

After the quantile algorithm assigns sizes, the number of distinct size groups is checked and normalized to prevent misleading visual emphasis. The percentile thresholds shift based on the number of groups:

| Distinct Groups | Threshold | Normalization | Rationale |
|-----------------|-----------|--------------|-----------|
| 1 group | None | All → Medium | All tags have equal weight; no emphasis needed |
| 2 groups | 50th percentile (median) | Above median → Medium, Below → Small | Balanced split; avoid exaggerated Large sizing |
| 3 groups | 25th / 75th percentile | Large, Medium, Small (unchanged) | Full range of counts; all sizes appropriate |

**Example:** A collection where every tag appears once (count=1) produces 1 size group → all tags display as Medium (regular size) instead of all Large.

### Section/Collection Title Exclusion

The tag cloud excludes section and collection titles from the **popular fill** portion of results. For example, when viewing the "AI" section without any tag filters active, the "AI" tag won't appear in the sidebar since it's redundant with the section context.

**Exception for selected tags**: When a user has actively selected a tag that matches a section/collection title (e.g., `?tags=news`), that tag is **always included** in the tag cloud results. This ensures users can deselect any tag they've selected, even if it matches a structural title. Only the popular fill (tags loaded to fill remaining slots) applies the structural exclusion.

## Tag Cloud UI Behavior

The `SidebarTagCloud` component provides interactive tag filtering with toggle behavior.

### Visual Active State

Selected tags are highlighted with the `.selected` CSS class:

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

### Toggle Behavior

- Clicking a tag toggles it on/off (add to filter or remove from filter)
- Multiple tags can be selected (AND logic applied)
- Tags are stored in URL query parameter `?tags=tag1,tag2,tag3`
- Tags are automatically deduplicated and normalized (lowercased) when parsing from URL
- Tag comparison uses `StringComparer.OrdinalIgnoreCase`
- **Selected tags with 0 count remain clickable**: Even when a selected tag has no matching content (count = 0) for the current filters, it remains clickable so users can deselect it. Only non-selected tags with 0 count are disabled.

### URL State Management

- Store selected tags in `HashSet<string>` with `StringComparer.OrdinalIgnoreCase`
- Initialize from URL with deduplication and normalization (`.ToLowerInvariant()`)
- Toggle adds/removes tags from set
- Update URL after each toggle using `NavigationManager`

### Page Integration

- Use `[SupplyParameterFromQuery(Name = "tags")]` for URL binding
- Parse comma-separated tags with `Uri.UnescapeDataString()`
- Normalize and deduplicate on parse
- Use `Distinct(StringComparer.OrdinalIgnoreCase)`

### Content Item Tag Cloud

When viewing a content item detail page, the tag cloud shows the item's tags with real section-level counts from the default 90-day date range. This way, clicking a tag navigates to filtered results that match the count displayed.

### Card Tag Badge Active State

On collection/section pages, tag badges on content item cards are aware of the active filter tags from the URL. When a tag on a card matches one of the active filter tags, the badge is highlighted with the `.badge-tag-active` CSS class (same purple styling as the sidebar tag cloud `.selected` state).

**Behavior**:

- **Visual highlight**: Active filter tags on cards use `--color-purple-dark` background and `--color-purple-bright` border, matching the sidebar tag cloud selected state
- **Toggle (deselect)**: Clicking a highlighted badge removes that tag from the URL filter (deselects it), instead of adding it again
- **Toggle (select)**: Clicking a non-highlighted badge adds the tag to the URL filter (existing behavior)
- **Case-insensitive matching**: Tag comparison uses `ToLowerInvariant()` for both display tags and active filter tags
- **Aria labels**: Active badges say "Remove filter: {tag}", inactive badges say "Filter by {tag}"

**Data flow**: `SectionCollection.razor` passes `selectedTags` as `FilterTags` to `ContentItemsGrid`, which passes them as `ActiveFilterTags` to each `ContentItemCard`. The card checks each tag against the active filters to determine styling and click behavior.

- `SidebarTagCloud` receives both `Tags` (the content item's tags) and `SectionName`
- When both are provided, it calls the tag cloud API with `tagsToCount` to fetch real counts
- If `SectionName` is not available, falls back to displaying each tag with a count of 1

### Filter Interaction Behavior

When search queries or date ranges change, the tag cloud dynamically updates to show relevant tags while preserving user selections:

**Popular Tags Adapt to Filters**: The tag cloud displays the most popular tags based on current filters (search query, date range). This provides better UX by showing contextually relevant tags.

**Selected Tags Always Visible**: Even if a selected tag is not among the popular tags for the current filters, it remains visible in the tag cloud. This ensures users never lose sight of their active selections.

**Selected Tags Appear First**: Selected tags are always displayed at the top of the tag cloud, followed by other popular tags. This improves visibility and makes it easier for users to see their active filters.

**Implementation Pattern**:

1. Fetch popular tags with all active filters applied (search, date range, etc.)
2. Check if any selected tags are missing from the popular tags results
3. If missing selected tags are found, make a second API call to fetch counts for those specific tags
4. Merge and reorder: selected tags first, then remaining popular tags

**Example Scenario**:

- User selects tag "Security" from the initial tag cloud
- User adds date filter (January 2024)
- Tag cloud updates to show popular tags for January 2024
- "Security" is not in the top 20 popular tags for January 2024
- Tag cloud displays: **"Security" (selected, shown first)**, then "AI", "Azure", and other popular tags for January 2024

This ensures the tag cloud balances contextual relevance with selection visibility while keeping active filters prominently displayed.

## Date Range Filtering

### Overview

The `DateRangeSlider` component provides interactive date range filtering with a dual-handle slider and preset buttons. It defaults to showing the last 90 days of content and integrates with the tag cloud so tag counts recalculate when the date range changes.

### Default Behavior

- **Default range**: Last 90 days (configurable via `DefaultLastDays` parameter)
- **Slider range**: Earliest content date to today
- **Presets**: 7d, 30d, 90d, 1y, All — clicking applies the range immediately

### Date Range URL State Management

Date range is stored as query parameters:

- `from=2024-10-16` — Start date (ISO 8601 format)
- `to=2026-01-16` — End date (ISO 8601 format)

**Parameter handling**:

- If no date parameters in URL: defaults to last 90 days
- URL is updated via `history.replaceState` (no full page reload)
- When combined with tags: `?tags=ai,azure&from=2024-01-01&to=2024-06-30`
- Existing query parameters (tags) are preserved when dates change

### Render Mode Architecture

The `DateRangeSlider` is an **interactive island** (`InteractiveServerRenderMode` with prerender) hosted inside static SSR parent pages (`Section.razor`, `SectionCollection.razor`). Because the parent pages are static SSR, they cannot call `NavigationManager.NavigateTo()` during the interactive phase. The slider handles URL updates directly by injecting its own `NavigationManager` within the interactive context.

### Integration with Tag Cloud

When the date range changes:

1. `DateRangeSlider` fires `OnDateRangeChanged` event
2. Parent page passes new dates to `SidebarTagCloud` as `FromDate`/`ToDate` parameters
3. Tag cloud reloads counts using the new date range
4. Tags with zero items within the range become disabled

Date parameters are passed through to the tag cloud API: `GET /api/sections/{sectionName}/collections/{collectionName}/tags?from=...&to=...`

### Integration with Content Grid

The parent page passes `FromDate`/`ToDate` as formatted strings to `ContentItemsGrid`, which includes them in API requests. Infinite scroll respects the date range — subsequent batches load only content within the selected range.

### Content Filtering API with Dates

#### Items Endpoint

```bash
# Content from a specific date range
curl -k "https://localhost:5001/api/sections/ai/collections/all/items?from=2024-01-01&to=2024-06-30"

# Combined with tags
curl -k "https://localhost:5001/api/sections/ai/collections/all/items?tags=copilot&from=2024-01-01&to=2024-12-31"
```

When `from`/`to` are provided, they take precedence over `lastDays`. Invalid date formats return `400 Bad Request`.

#### Tags Endpoint

```bash
# Tag counts within a date range
curl -k "https://localhost:5001/api/sections/ai/collections/all/tags?from=2024-01-01&to=2024-06-30"

# Combined with tag selection
curl -k "https://localhost:5001/api/sections/ai/collections/all/tags?tags=ai&from=2024-01-01&to=2024-06-30"
```

### Accessibility

- Slider inputs have `aria-label` attributes ("From date" / "To date")
- Date display uses `aria-live="polite"` for screen reader announcements
- Preset buttons indicate active state
- Component wrapped in `<nav aria-label="Filter by date range">` landmark

## Testing

See [tests/TechHub.Infrastructure.Tests/Repositories/TagFilteringTests.cs](../tests/TechHub.Infrastructure.Tests/Repositories/TagFilteringTests.cs) for repository level tests and [ContentEndpointsTests.cs](../tests/TechHub.Api.Tests/Endpoints/ContentEndpointsTests.cs) for integration tests.

For tag cloud UI behavior tests, see [tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs](../tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs).

For date range slider component tests, see [tests/TechHub.Web.Tests/Components/DateRangeSliderTests.cs](../tests/TechHub.Web.Tests/Components/DateRangeSliderTests.cs). For date range E2E tests, see [tests/TechHub.E2E.Tests/Web/DateRangeSliderTests.cs](../tests/TechHub.E2E.Tests/Web/DateRangeSliderTests.cs).

## Future Enhancements (Not Currently Implemented)

1. **Partial tag word storage**: Store "azure ai" and "ai foundry" as separate tag words for better partial matching
2. **Fuzzy matching**: Allow typo tolerance in tag searches
3. **Tag synonyms**: Map related tags (e.g., "ML" → "Machine Learning")
4. **Tag hierarchy**: Support parent-child tag relationships
5. **Pagination**: `?page=1&pageSize=50` for large result sets
6. **Sorting**: `?sortBy=date&order=desc` for custom ordering
7. **Tag filtering modes**: `?tagMatch=any` for OR logic (currently AND only)
