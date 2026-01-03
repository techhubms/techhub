# ADR: RESTful API Structure with Nested Routes

## Status

Accepted

## Date

2025-02-28

## Context

The initial API implementation used a flat structure with separate endpoints for different filtering criteria:

- `GET /api/content` - All content
- `GET /api/content/collection/{collection}` - Filter by collection
- `GET /api/content/category/{category}` - Filter by category
- `GET /api/content/{collection}/{id}` - Single item
- `GET /api/content/search?q={query}` - Text search
- `GET /api/content/tags` - All tags

This structure had several issues:

1. **Inconsistent naming**: Section identifiers used "sectionId" while collections used "collectionName", even though both are human-readable slugs
2. **Flat hierarchy**: Did not reflect the domain model's natural hierarchy (sections → collections → items)
3. **Limited composability**: Difficult to filter by multiple criteria simultaneously
4. **Unclear semantics**: Not obvious how to get "all items in AI section" or "videos in ML section"

## Decision

We restructured the API to use **nested RESTful routes** that mirror the domain hierarchy:

### Section Endpoints

```http
GET /api/sections
GET /api/sections/{sectionName}
GET /api/sections/{sectionName}/items
GET /api/sections/{sectionName}/collections
GET /api/sections/{sectionName}/collections/{collectionName}
GET /api/sections/{sectionName}/collections/{collectionName}/items
```

### Advanced Filtering

```http
GET /api/content/filter?sections={s1,s2}&collections={c1,c2}&tags={t1,t2}&q={query}
GET /api/content/tags
```

### Removed Endpoints

The following flat structure endpoints were **removed**:

- ❌ `GET /api/content` - Replaced by `/api/content/filter` (no params = all)
- ❌ `GET /api/content/collection/{collection}` - Replaced by nested routes
- ❌ `GET /api/content/category/{category}` - Replaced by `/api/sections/{section}/items`
- ❌ `GET /api/content/search?q={query}` - Replaced by `/api/content/filter?q={query}`

### Kept Endpoints

- ✅ `GET /api/content/tags` - Moved to `/api/content/tags` (global resource)

## Rationale

### 1. Consistent Naming

Both sections and collections now use human-readable names consistently:

- ✅ `sections/{sectionName}` (e.g., `ai`, `github-copilot`)
- ✅ `collections/{collectionName}` (e.g., `news`, `blogs`)

No more confusion between "ID" and "name" - they're the same.

### 2. Hierarchical Structure

The nested routes reflect the actual domain model:

```text
Section (e.g., AI)
  ├── Collection (e.g., News)
  │   └── Items (e.g., articles)
  ├── Collection (e.g., Blogs)
  │   └── Items (e.g., blog posts)
  └── Collection (e.g., Videos)
      └── Items (e.g., videos)
```

This makes the API self-documenting and intuitive.

### 3. Clear Semantics

New routes make intent obvious:

- `/api/sections/ai/items` - "All items in AI section"
- `/api/sections/ml/collections/videos/items` - "Videos in ML section"
- `/api/sections/github-copilot/collections` - "What collections exist in GitHub Copilot?"

### 4. Advanced Filtering

The `/api/content/filter` endpoint supports complex multi-criteria queries:

```bash
# All items with copilot tag in AI/ML news/blogs
GET /api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot
```

Filtering logic:

- **Sections**: OR logic (ai OR ml)
- **Collections**: OR logic (news OR blogs)
- **Tags**: AND logic (must have ALL tags)
- **Query**: AND logic with other filters

### 5. Performance

Nested routes enable efficient querying:

- Section items: Filter by category (one repository call)
- Collection items: Filter by collection (one repository call)
- Section + collection: Filter by both (one call + in-memory filter)

## Consequences

### Positive

1. **Intuitive API**: Routes mirror domain model, easier to understand
2. **Self-documenting**: URL structure explains what data is returned
3. **Consistent naming**: No more ID vs name confusion
4. **Flexible filtering**: Combine criteria as needed
5. **RESTful design**: Follows standard REST conventions
6. **Scalable**: Easy to add new nested resources

### Negative

1. **Breaking changes**: Existing consumers need to update (none exist yet)
2. **More endpoints**: 6 section endpoints vs 1 flat endpoint
3. **Deeper nesting**: Some URLs are longer (`/sections/{s}/collections/{c}/items`)

### Neutral

1. **Documentation overhead**: More endpoints to document (mitigated by clear structure)
2. **Testing complexity**: More endpoint combinations to test (mitigated by systematic test suite)

## Implementation

### Changed Files

- `/src/TechHub.Api/Endpoints/SectionsEndpoints.cs`
  - Added 4 new endpoint handlers:
    - `GetSectionItems()` - All items in section
    - `GetSectionCollections()` - All collections in section
    - `GetSectionCollection()` - Specific collection details
    - `GetSectionCollectionItems()` - Items in collection within section

- `/src/TechHub.Api/Endpoints/ContentEndpoints.cs`
  - Removed: `GetAllContent()`, `GetContentByCollection()`, `GetContentByCategory()`, `GetContentById()`, `SearchContent()`
  - Added: `FilterContent()` with multi-criteria support
  - Kept: `GetAllTags()`

- Deleted: `/src/TechHub.Api/Endpoints/ItemsEndpoints.cs` (duplicate/obsolete)

### Documentation Updates

- Created `/docs/api-specification.md` with complete API reference
- This ADR documents the decision and rationale
- Test suite `/.tmp/test-restful-api.ps1` verifies all 14 endpoints

## Testing

All 14 endpoints tested and passing:

**Section Endpoints** (8 tests):

- ✅ GET /api/sections - 8 sections
- ✅ GET /api/sections/ai - AI section details
- ✅ GET /api/sections/ai/items - 1378 items
- ✅ GET /api/sections/github-copilot/collections - 4 collections
- ✅ GET /api/sections/ai/collections/news - News collection
- ✅ GET /api/sections/ml/collections/videos - 1 video item
- ✅ GET /api/sections/invalid - 404
- ✅ GET /api/sections/ai/collections/invalid - 404

**Advanced Filtering** (6 tests):

- ✅ GET /api/content/filter?sections=ai,ml - 1519 items
- ✅ GET /api/content/filter?collections=news,blogs - 1142 items
- ✅ GET /api/content/filter?sections=ai&collections=news - 528 items
- ✅ GET /api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot - 144 items
- ✅ GET /api/content/filter?sections=github-copilot&q=vscode - 4 items
- ✅ GET /api/content/tags - 12,524 tags

## Future Considerations

1. **Pagination**: Add `?page=1&pageSize=50` for large result sets
2. **Sorting**: Add `?sortBy=date&order=desc`
3. **Tag filtering modes**: Add `?tagMatch=any` for OR logic (currently AND only)
4. **Caching**: Add ETags and Last-Modified headers
5. **Individual items**: Add `/api/items/{collection}/{id}` for direct access
6. **RSS feeds**: Add `/api/sections/{section}/collections/{collection}/feed`

## References

- [API Specification](/docs/api-specification.md)
- [Test Suite](/.tmp/test-restful-api.ps1)
- [SectionsEndpoints.cs](/src/TechHub.Api/Endpoints/SectionsEndpoints.cs)
- [ContentEndpoints.cs](/src/TechHub.Api/Endpoints/ContentEndpoints.cs)
