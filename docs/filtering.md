# Content Filtering System

This document describes how tag filtering works in Tech Hub, which is one of the most crucial features of the website.

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

## API Examples

### Single Tag Filter

```bash
# Find all items tagged with "ai"
curl "https://localhost:5001/api/sections/all/collections/all/items?tags=ai"

# Find items with exact tag "azure ai foundry"
curl "https://localhost:5001/api/sections/github-copilot/collections/all/items?tags=azure%20ai%20foundry"
```

### Multiple Tag Filter (AND Logic)

```bash
# Find items with BOTH "ai" AND "azure"
curl "https://localhost:5001/api/sections/all/collections/all/items?tags=ai,azure"

# Find items with all three tags
curl "https://localhost:5001/api/sections/all/collections/all/items?tags=copilot,azure,devops"
```

### Combined with Other Filters

```bash
# Tags + Section + Collection
curl "https://localhost:5001/api/sections/github-copilot/collections/videos/items?tags=ai"

# Tags + Date Range
curl "https://localhost:5001/api/sections/all/collections/all/items?tags=ai&lastDays=30"

# Tags + Full-Text Search
curl "https://localhost:5001/api/sections/all/collections/all/items?tags=ai&q=copilot"
```

## Frontend Integration

The frontend tag filter allows users to search for tags by typing in a text box. The typed value is sent as-is to the API:

- User types: "ai" → API receives `tags=ai`
- User types: "azure ai foundry" → API receives `tags=azure%20ai%20foundry`
- User selects multiple tags: "ai", "azure" → API receives `tags=ai,azure`

## Performance Characteristics

- **Tag expansion**: Indexed for fast lookups
- **Bitmask filtering**: Very fast bitwise operations
- **GROUP BY**: Efficient deduplication
- **Date ordering**: Uses MAX(date_epoch) with index

**Expected Performance**:

- Tag-only queries: < 50ms
- Tag + Section + Collection: < 100ms
- Tag + FTS search: < 1000ms

## Testing

See [tests/TechHub.Infrastructure.Tests/Repositories/TagFilteringTests.cs](../tests/TechHub.Infrastructure.Tests/Repositories/TagFilteringTests.cs) for comprehensive test coverage of tag filtering behavior.

## Future Enhancements (Not Currently Implemented)

1. **Partial tag word storage**: Store "azure ai" and "ai foundry" as separate tag words for better partial matching
2. **Fuzzy matching**: Allow typo tolerance in tag searches
3. **Tag synonyms**: Map related tags (e.g., "ML" → "Machine Learning")
4. **Tag hierarchy**: Support parent-child tag relationships
