# Content Filtering System

This document describes how tag filtering works in Tech Hub, which is one of the most crucial features of the website.

**Related Documentation**:

- [Content API](content-api.md) - REST API endpoints for content retrieval
- [Database Configuration](database.md) - Database providers that support filtering

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
- `lastDays` (optional): Filter to content from last N days
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

Filter by tags (AND logic):

```bash
curl -k "https://localhost:5001/api/sections/all/collections/all/items?tags=copilot,azure"
```

Complex multi-criteria filter:

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/videos/items?tags=copilot&lastDays=30"
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

Tag sizes are calculated using quantile distribution to ensure consistent visual representation across varying tag counts:

| Quantile | Size Value | CSS Class | Description |
|----------|------------|-----------|-------------|
| Top 25% | 2 | `tag-size-large` | Most frequently used tags |
| Middle 50% | 1 | `tag-size-medium` | Moderately used tags |
| Bottom 25% | 0 | `tag-size-small` | Less frequently used tags |

**Why Quantile Sizing?**

- Ensures even distribution of sizes regardless of actual count values
- Prevents a few high-count tags from dominating the visual
- Adapts automatically to different sections/collections with varying tag frequencies

### Section/Collection Title Exclusion

The tag cloud automatically excludes section and collection titles from the tag list. For example, when viewing the "AI" section, the "AI" tag is filtered out since it's redundant with the section context.

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

## Testing

See [tests/TechHub.Infrastructure.Tests/Repositories/TagFilteringTests.cs](../tests/TechHub.Infrastructure.Tests/Repositories/TagFilteringTests.cs) for repository level tests and [ContentEndpointsTests.cs](../tests/TechHub.Api.Tests/Endpoints/ContentEndpointsTests.cs) for integration tests.

For UI behavior tests, see [tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs](../tests/TechHub.E2E.Tests/Web/TagFilteringTests.cs).

## Future Enhancements (Not Currently Implemented)

1. **Partial tag word storage**: Store "azure ai" and "ai foundry" as separate tag words for better partial matching
2. **Fuzzy matching**: Allow typo tolerance in tag searches
3. **Tag synonyms**: Map related tags (e.g., "ML" → "Machine Learning")
4. **Tag hierarchy**: Support parent-child tag relationships
5. **Pagination**: `?page=1&pageSize=50` for large result sets
6. **Sorting**: `?sortBy=date&order=desc` for custom ordering
7. **Tag filtering modes**: `?tagMatch=any` for OR logic (currently AND only)
