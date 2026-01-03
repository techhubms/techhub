# Tech Hub API Specification

## Overview

The Tech Hub API provides RESTful access to content organized by sections and collections. It uses a nested route structure that mirrors the site's hierarchical organization.

**Key Design Principles**:

- **Resource hierarchy**: Routes mirror domain model (`/sections/{section}/collections/{collection}/items`)
- **Consistent naming**: Both sections and collections use human-readable slugs (no arbitrary IDs)
- **Self-documenting**: URL structure explains what data is returned
- **Flexible filtering**: Combine multiple criteria via `/api/content/filter` endpoint

## Base URL

```
http://localhost:5029/api
```

## Authentication

Currently not required (development phase).

## Data Model

### Section

Represents a thematic grouping of content (e.g., AI, GitHub Copilot, Azure).

```json
{
  "id": "ai",
  "title": "AI",
  "description": "Your gateway to the AI revolution...",
  "url": "/ai",
  "category": "AI",
  "backgroundImage": "/assets/section-backgrounds/ai.jpg",
  "collections": [
    {
      "title": "News",
      "collection": "news",
      "url": "/ai/news.html",
      "description": "News articles from official product sources.",
      "isCustom": false
    }
  ]
}
```

### Content Item

Represents individual content (articles, videos, blogs, etc.).

```json
{
  "id": "2024-01-15-example-post",
  "title": "Example Post",
  "description": "Brief description of the content",
  "author": "Jane Doe",
  "dateEpoch": 1705276800,
  "dateIso": "2024-01-15",
  "collection": "news",
  "altCollection": null,
  "categories": ["AI", "GitHub Copilot"],
  "tags": ["copilot", "ai", "productivity"],
  "excerpt": "First paragraph excerpt...",
  "externalUrl": "https://example.com/post",
  "videoId": null,
  "url": "/news/2024-01-15-example-post"
}
```

## Endpoints

### Sections

#### GET /api/sections

Get all sections.

**Response**: `200 OK`

```json
[
  {
    "id": "ai",
    "title": "AI",
    "description": "...",
    "url": "/ai",
    "category": "AI",
    "backgroundImage": "/assets/section-backgrounds/ai.jpg",
    "collections": [...]
  }
]
```

**Example**:

```bash
curl http://localhost:5029/api/sections
```

---

#### GET /api/sections/{sectionName}

Get a specific section by name.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`, `ml`)

**Response**: `200 OK` or `404 Not Found`

**Example**:

```bash
curl http://localhost:5029/api/sections/ai
```

---

#### GET /api/sections/{sectionName}/items

Get all content items in a section (across all collections).

**Parameters**:

- `sectionName` (path): Section identifier

**Response**: `200 OK` or `404 Not Found`

**Example**:

```bash
curl http://localhost:5029/api/sections/ai/items

# Returns 1378+ AI-related items across all collections
```

---

#### GET /api/sections/{sectionName}/collections

Get all collections available in a section.

**Parameters**:

- `sectionName` (path): Section identifier

**Response**: `200 OK` or `404 Not Found`

**Example**:

```bash
curl http://localhost:5029/api/sections/github-copilot/collections

# Returns: news, blogs, videos, community
```

---

#### GET /api/sections/{sectionName}/collections/{collectionName}

Get details about a specific collection within a section.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name (e.g., `news`, `blogs`, `videos`, `community`, `roundups`)

**Response**: `200 OK` or `404 Not Found`

**Example**:

```bash
curl http://localhost:5029/api/sections/ai/collections/news
```

---

#### GET /api/sections/{sectionName}/collections/{collectionName}/items

Get all items in a specific collection within a section.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name

**Response**: `200 OK` or `404 Not Found`

**Example**:

```bash
curl http://localhost:5029/api/sections/ml/collections/videos

# Returns only videos tagged with ML category
```

---

### Advanced Filtering

#### GET /api/content/filter

Filter content across multiple criteria with AND logic.

**Query Parameters**:

- `sections` (optional): Comma-separated section names (e.g., `ai,ml`)
- `collections` (optional): Comma-separated collection names (e.g., `news,blogs`)
- `tags` (optional): Comma-separated tags - content must have ALL tags (AND logic)
- `q` (optional): Text search query (searches title, description, tags)

**Response**: `200 OK`

**Examples**:

Filter by sections only:

```bash
curl "http://localhost:5029/api/content/filter?sections=ai,ml"

# Returns 1519+ items from AI or ML sections
```

Filter by collections only:

```bash
curl "http://localhost:5029/api/content/filter?collections=news,blogs"

# Returns 1142+ news and blog items
```

Combine section and collection:

```bash
curl "http://localhost:5029/api/content/filter?sections=ai&collections=news"

# Returns 528+ AI news items
```

Complex multi-criteria filter:

```bash
curl "http://localhost:5029/api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot"

# Returns 144+ items with copilot tag in AI/ML news/blogs
```

Search within filtered results:

```bash
curl "http://localhost:5029/api/content/filter?sections=github-copilot&q=vscode"

# Returns 4+ GitHub Copilot items mentioning 'vscode'
```

---

#### GET /api/content/tags

Get all unique tags across all content.

**Response**: `200 OK`

```json
[
  "copilot",
  "ai",
  "azure",
  "github",
  ...
]
```

**Example**:

```bash
curl http://localhost:5029/api/content/tags

# Returns 12,524+ unique tags
```

## Error Responses

### 404 Not Found

Returned when a section, collection, or item does not exist.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404
}
```

## Performance Characteristics

- **Sections**: ~25ms response time
- **Content (first load)**: ~5-9 seconds (2251+ markdown files)
- **Content (cached)**: < 100ms
- **Filtering**: Varies based on criteria (typically < 2 seconds)

## Valid Values

### Sections

- `all` - Everything
- `github-copilot` - GitHub Copilot
- `ai` - AI
- `ml` - ML (Machine Learning)
- `devops` - DevOps
- `azure` - Azure
- `coding` - .NET
- `security` - Security

### Collections

- `news` - Official announcements and product updates
- `videos` - Video content and tutorials
- `community` - Community posts and discussions
- `blogs` - Blogs and technical articles
- `roundups` - Weekly content summaries

## RESTful Design Principles

The API follows RESTful conventions:

1. **Resource hierarchy**: `/sections/{section}/collections/{collection}/items`
2. **Consistent naming**: Both sections and collections use human-readable names (slugs), not arbitrary IDs
3. **HTTP methods**: GET for retrieval (POST/PUT/DELETE not yet implemented)
4. **Status codes**: 200 OK for success, 404 Not Found for missing resources
5. **Nested routes**: Resources are organized hierarchically to match domain model
6. **Advanced filtering**: Separate `/filter` endpoint for complex multi-criteria queries

### Why Nested Routes?

The nested route structure was chosen over a flat structure for several reasons:

**Previous Flat Structure (Replaced)**:

```http
GET /api/content                          # All content
GET /api/content/collection/{collection}  # Filter by collection
GET /api/content/category/{category}      # Filter by category
GET /api/content/{collection}/{id}        # Single item
GET /api/content/search?q={query}         # Text search
```

**Issues with Flat Structure**:

1. **Inconsistent naming**: Mixed use of "sectionId" vs "collectionName" despite both being slugs
2. **No hierarchy**: Didn't reflect the natural domain model (sections → collections → items)
3. **Limited composability**: Difficult to combine multiple filter criteria
4. **Unclear semantics**: Not obvious how to get "all items in AI section" or "videos in ML section"

**Benefits of Current Nested Structure**:

1. ✅ **Intuitive**: Routes mirror domain model, easier to understand
2. ✅ **Self-documenting**: URL structure explains what data is returned
3. ✅ **Consistent naming**: No more ID vs name confusion
4. ✅ **Flexible filtering**: Advanced `/filter` endpoint supports complex queries
5. ✅ **RESTful design**: Follows standard REST conventions
6. ✅ **Scalable**: Easy to add new nested resources

**Trade-offs**:

- More endpoints to document (6 section endpoints vs 1 flat endpoint)
- Deeper URL nesting for some routes
- Breaking changes for consumers (mitigated by this being pre-release)

See [Decision Record](#architecture-decision-record) below for complete rationale.

## Migration Notes

### From Old Flat Structure

The previous flat structure has been **completely removed**:

**Old** (deprecated):

- ❌ `GET /api/content` → Use `/api/content/filter` (no params = all content)
- ❌ `GET /api/content/collection/{collection}` → Use `/api/sections/{section}/collections/{collection}/items`
- ❌ `GET /api/content/category/{category}` → Use `/api/sections/{section}/items`
- ❌ `GET /api/content/search?q={query}` → Use `/api/content/filter?q={query}`

**New** (current):

- ✅ `GET /api/sections/{section}/items` - All items in a section
- ✅ `GET /api/sections/{section}/collections/{collection}/items` - Items in specific collection
- ✅ `GET /api/content/filter` - Advanced multi-criteria filtering with query parameters

## Example Use Cases

### 1. Build a section landing page

```bash
# Get section details

curl http://localhost:5029/api/sections/ai

# Get all items for the section

curl http://localhost:5029/api/sections/ai/items
```

### 2. Display collection-specific content

```bash
# Get GitHub Copilot news

curl http://localhost:5029/api/sections/github-copilot/collections/news/items
```

### 3. Complex content discovery

```bash
# Find all copilot-related content in AI and ML blogs

curl "http://localhost:5029/api/content/filter?sections=ai,ml&collections=blogs&tags=copilot"
```

### 4. Build tag-based navigation

```bash
# Get all tags

curl http://localhost:5029/api/content/tags

# Filter by specific tags

curl "http://localhost:5029/api/content/filter?tags=azure,copilot"
```

### 5. Search functionality

```bash
# Search across all content

curl "http://localhost:5029/api/content/filter?q=blazor"

# Search within specific section

curl "http://localhost:5029/api/content/filter?sections=coding&q=blazor"
```

## Testing

### Integration Tests

All 14 endpoints have comprehensive integration test coverage using `WebApplicationFactory<Program>`:

**Section Endpoints** (18 tests in `SectionsEndpointsTests.cs`):

- ✅ GET /api/sections - Returns all sections
- ✅ GET /api/sections/{sectionName} - Returns section by name, 404 for invalid
- ✅ GET /api/sections/{sectionName}/items - Returns section items, 404 for invalid
- ✅ GET /api/sections/{sectionName}/collections - Returns collections, 404 for invalid
- ✅ GET /api/sections/{sectionName}/collections/{collectionName} - Returns collection, 404 for invalid section/collection
- ✅ GET /api/sections/{sectionName}/collections/{collectionName}/items - Returns items with correct URLs, 404 for invalid

**Advanced Filtering** (22 tests in `ContentEndpointsTests.cs`):

- ✅ GET /api/content/filter - No params returns all, single/multiple sections, single/multiple collections
- ✅ Section AND collection filtering
- ✅ Tag filtering (single and multiple with AND logic)
- ✅ Complex multi-criteria filtering (sections + collections + tags)
- ✅ Text search across title/description/tags
- ✅ Text search with section filter
- ✅ Case-insensitive filtering
- ✅ Empty results handling
- ✅ GET /api/content/tags - Returns all unique tags

**Test Statistics**:

- **Total Tests**: 40 API integration tests
- **Pass Rate**: 100%
- **Execution Time**: ~1.4 seconds
- **Coverage**: All endpoints, all filter combinations, all error scenarios

See [tests/AGENTS.md](/tests/AGENTS.md) and [TechHub.Api.Tests](/tests/TechHub.Api.Tests/) for complete testing documentation.

## Architecture Decision Record

### Decision: RESTful API Structure with Nested Routes

**Status**: Accepted (2025-02-28)

**Context**:

The initial flat API structure used separate endpoints for different filtering criteria, which created inconsistencies in naming, didn't reflect the domain hierarchy, and made it difficult to combine multiple filters.

**Decision**:

We restructured the API to use nested RESTful routes that mirror the domain model:

- Section endpoints: `/api/sections/{sectionName}/...`
- Collection endpoints: `/api/sections/{sectionName}/collections/{collectionName}/...`
- Advanced filtering: `/api/content/filter` with query parameters

**Consequences**:

**Positive**:

1. Intuitive API that mirrors domain model
2. Self-documenting URL structure
3. Consistent naming (no ID vs name confusion)
4. Flexible multi-criteria filtering
5. Follows REST conventions
6. Scalable for future additions

**Negative**:

1. More endpoints to document (6 vs 1)
2. Deeper URL nesting for some routes
3. Breaking changes for consumers (mitigated by pre-release status)

**Implementation**:

- Created `SectionsEndpoints.cs` with 6 nested route handlers
- Refactored `ContentEndpoints.cs` to use `FilterContent()` with multi-criteria support
- Removed flat structure endpoints
- Added comprehensive integration test suite (40 tests)

See `/docs/decisions/restful-api-structure.md` for complete decision documentation.

## Future Enhancements

Planned features (not yet implemented):

- **Pagination**: `?page=1&pageSize=50` for large result sets
- **Sorting**: `?sortBy=date&order=desc` for custom ordering
- **Tag filtering modes**: `?tagMatch=any` for OR logic (currently AND only)
- **Caching headers**: ETag and Last-Modified for efficient caching
- **Individual items**: `/api/items/{collection}/{id}` for direct item access
- **RSS feeds**: `/api/sections/{section}/collections/{collection}/feed`
- **Rate limiting**: Protect against abuse
- **Authentication/Authorization**: Secure access control
