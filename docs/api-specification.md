# Tech Hub API Specification

## Overview

The Tech Hub API provides RESTful access to content organized by sections and collections. It uses a nested route structure that mirrors the site's hierarchical organization.

**Key Design Principles**:

- **Resource hierarchy**: Routes mirror domain model (`/sections/{sectionName}/collections/{collectionName}/items`)
- **Consistent naming**: Both sections and collections use human-readable slugs (no arbitrary IDs)
- **Self-documenting**: URL structure explains what data is returned
- **Flexible filtering**: Combine multiple criteria via `/api/content/filter` endpoint

## Base URL

```http
https://localhost:5001/api
```

## Authentication

Currently not required (development phase).

## Data Model

### Section

Represents a thematic grouping of content (e.g., AI, GitHub Copilot, Azure).

```json
{
  "name": "ai",
  "title": "AI",
  "description": "Your gateway to the AI revolution...",
  "url": "/ai",
  "backgroundImage": "/assets/section-backgrounds/ai.jpg",
  "collections": [
    {
      "title": "News",
      "name": "news",
      "url": "/ai/news",
      "description": "News articles from official product sources.",
      "isCustom": false
    }
  ]
}
```

### Content Item (ContentItemDto)

Represents individual content (articles, videos, blogs, etc.) in API responses.

```json
{
  "slug": "2024-01-15-example-post",
  "title": "Example Post",
  "description": "Brief description of the content",
  "author": "Jane Doe",
  "dateEpoch": 1705276800,
  "dateIso": "2024-01-15",
  "collectionName": "news",
  "sectionNames": ["ai", "github-copilot"],
  "primarySectionName": "ai",
  "tags": ["copilot", "ai", "productivity"],
  "excerpt": "First paragraph excerpt...",
  "externalUrl": "https://techcommunity.microsoft.com/example-post",
  "videoId": null,
  "url": "/ai/news/2024-01-15-example-post"
}
```

**Key Fields**:

- `slug`: Unique identifier derived from filename (e.g., `"2024-01-15-example-post"`)
- `title`, `description`, `author`, `excerpt`: Content metadata
- `dateEpoch`, `dateIso`: Publication date in both formats
- `collectionName`: Collection this item belongs to (`"news"`, `"blogs"`, `"videos"`, `"community"`, `"roundups"`)
- `sectionNames`: Array of lowercase section identifiers this content belongs to (e.g., `["ai", "github-copilot"]`) - mapped from frontmatter `categories` field (which contains Section Titles like "AI", "GitHub Copilot")
- `primarySectionName`: Highest-priority section name (lowercase identifier) used for URL routing (e.g., `"ai"`). Calculated using section priority: github-copilot > ai > ml > azure > coding > devops > security
- `tags`: Content tags for filtering
- `url`: Item detail page URL (format: `/{primarySectionName}/{collectionName}/{slug}`)
- `externalUrl`: Original source URL (e.g., `"https://techcommunity.microsoft.com/..."`) - present for content that links to external sources
- `videoId`: YouTube video ID from frontmatter `youtube_id` field (for video embeds)

**Important Behaviors**:

- **Section Mapping**: Frontmatter `categories` field contains Section Titles (e.g., "AI", "GitHub Copilot") which are automatically mapped to Section Names (lowercase identifiers like "ai", "github-copilot") and stored in `sectionNames` property
- **Primary Section**: Calculated using section priority: github-copilot > ai > ml > azure > coding > devops > security. The highest-priority section name is used for URL routing
- **URL Routing**: URLs always use `primarySectionName` for consistent navigation (e.g., item with `["ai", "ml"]` sectionNames → `/ai/videos/slug`)
- **Collection-Based Links**: Content rendering determined by collection type:
  - External collections (news, blogs, community) - link to original source in new tab
  - Internal collections (videos, roundups, custom) - navigate to detail page on site
- **ExternalUrl**: Contains original source URL for external collections (used for attribution and external links)

## Endpoints

### Sections

#### GET /api/sections

Get all sections.

**Response**: `200 OK`

```json
[
  {
    "name": "ai",
    "title": "AI",
    "description": "...",
    "url": "/ai",
    "backgroundImage": "/assets/section-backgrounds/ai.jpg",
    "collections": [...]
  }
]
```

**Example**:

```bash
curl -k https://localhost:5001/api/sections
```

---

#### GET /api/sections/{sectionName}

Get a specific section by name.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`, `ml`)

**Response**: `200 OK` or `404 Not Found`

**Example**:

```bash
curl -k https://localhost:5001/api/sections/ai
```

---

#### GET /api/sections/{sectionName}/items

Get all content items in a section (across all collections).

**Parameters**:

- `sectionName` (path): Section identifier

**Response**: `200 OK` or `404 Not Found`

**Example**:

```bash
curl -k https://localhost:5001/api/sections/ai/items

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
curl -k https://localhost:5001/api/sections/github-copilot/collections

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
curl -k https://localhost:5001/api/sections/ai/collections/news
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
curl -k https://localhost:5001/api/sections/ml/collections/videos

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
curl -k "https://localhost:5001/api/content/filter?sections=ai,ml"

# Returns 1519+ items from AI or ML sections
```

Filter by collections only:

```bash
curl -k "https://localhost:5001/api/content/filter?collections=news,blogs"

# Returns 1142+ news and blog items
```

Combine section and collection:

```bash
curl -k "https://localhost:5001/api/content/filter?sections=ai&collections=news"

# Returns 528+ AI news items
```

Complex multi-criteria filter:

```bash
curl -k "https://localhost:5001/api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot"

# Returns 144+ items with copilot tag in AI/ML news/blogs
```

Search within filtered results:

```bash
curl -k "https://localhost:5001/api/content/filter?sections=github-copilot&q=vscode"

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
curl -k https://localhost:5001/api/content/tags

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

### Section Names

- `all` - Everything
- `github-copilot` - GitHub Copilot
- `ai` - AI
- `ml` - ML (Machine Learning)
- `devops` - DevOps
- `azure` - Azure
- `coding` - .NET
- `security` - Security

### Collection Names

- `news` - Official announcements and product updates
- `videos` - Video content and tutorials
- `community` - Community posts and discussions
- `blogs` - Blogs and technical articles
- `roundups` - Weekly content summaries

## RESTful Design Principles

The API follows RESTful conventions:

1. **Resource hierarchy**: `/sections/{sectionName}/collections/{collectionName}/items`
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
GET /api/content/collection/{collectionName}  # Filter by collection
GET /api/content/category/{category}      # Filter by category
GET /api/content/{collectionName}/{id}        # Single item
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
- ❌ `GET /api/content/collection/{collectionName}` → Use `/api/sections/{sectionName}/collections/{collectionName}/items`
- ❌ `GET /api/content/category/{category}` → Use `/api/sections/{sectionName}/items`
- ❌ `GET /api/content/search?q={query}` → Use `/api/content/filter?q={query}`

**New** (current):

- ✅ `GET /api/sections/{sectionName}/items` - All items in a section
- ✅ `GET /api/sections/{sectionName}/collections/{collectionName}/items` - Items in specific collection
- ✅ `GET /api/content/filter` - Advanced multi-criteria filtering with query parameters

## Example Use Cases

### 1. Build a section landing page

```bash
# Get section details

curl -k https://localhost:5001/api/sections/ai

# Get all items for the section

curl -k https://localhost:5001/api/sections/ai/items
```

### 2. Display collection-specific content

```bash
# Get GitHub Copilot news

curl -k https://localhost:5001/api/sections/github-copilot/collections/news/items
```

### 3. Complex content discovery

```bash
# Find all copilot-related content in AI and ML blogs

curl -k "https://localhost:5001/api/content/filter?sections=ai,ml&collections=blogs&tags=copilot"
```

### 4. Build tag-based navigation

```bash
# Get all tags

curl -k https://localhost:5001/api/content/tags

# Filter by specific tags

curl -k "https://localhost:5001/api/content/filter?tags=azure,copilot"
```

### 5. Search functionality

```bash
# Search across all content

curl -k "https://localhost:5001/api/content/filter?q=blazor"

# Search within specific section

curl -k "https://localhost:5001/api/content/filter?sections=coding&q=blazor"
```

---

### Health

#### GET /health

Health check endpoint for monitoring and load balancers.

**Response**: `200 OK` with `text/plain`

```text
Healthy
```

**Example**:

```bash
curl -k https://localhost:5001/health
```

**Use Case**: Kubernetes liveness/readiness probes, load balancer health checks

---

### Content Detail

#### GET /api/content/{sectionName}/{collectionName}/{slug}

Get detailed content item by section, collection, and content slug.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`)
- `collectionName` (path): Collection name (e.g., `news`, `blogs`, `videos`)
- `slug` (path): Content slug (URL-friendly identifier from filename)

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `ContentItemDetailDto` with full markdown content, metadata, and related items

**Example**:

```bash
curl -k "https://localhost:5001/api/content/ai/news/2024-06-introducing-copilot-extensions"
```

**Use Case**: Single content page, detail view, full article rendering

---

### RSS Feeds (API Direct)

These are the backend RSS feed endpoints. For public-facing RSS feeds, use the Blazor Web proxy endpoints (see "RSS Feed Proxy Endpoints" section below).

#### GET /api/rss/all

Get RSS feed for all content across all sections.

**Response**: `200 OK` with `application/rss+xml; charset=utf-8`

**Example**:

```bash
curl -k https://localhost:5001/api/rss/all
```

---

#### GET /api/rss/{sectionName}

Get RSS feed for a specific section.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`)

**Response**: `200 OK` or `404 Not Found` with `application/rss+xml; charset=utf-8`

**Example**:

```bash
curl -k https://localhost:5001/api/rss/ai
```

---

#### GET /api/rss/collection/{collectionName}

Get RSS feed for a specific collection.

**Parameters**:

- `collectionName` (path): Collection name (e.g., `roundups`, `news`, `blogs`)

**Response**: `200 OK` or `404 Not Found` with `application/rss+xml; charset=utf-8`

**Example**:

```bash
curl -k https://localhost:5001/api/rss/collection/roundups
```

---

### Custom Pages

Endpoints for custom standalone pages with structured data.

#### GET /api/custom-pages/dx-space

Get Developer Experience Space page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `DXSpacePageData` with page content and related items

---

#### GET /api/custom-pages/handbook

Get GitHub Copilot Handbook page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `HandbookPageData` with page content and related items

---

#### GET /api/custom-pages/levels

Get Levels of Enlightenment page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `LevelsPageData` with page content and related items

---

#### GET /api/custom-pages/vscode-updates

Get VS Code Updates page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `VSCodeUpdatesPageData` with page content and related items

---

#### GET /api/custom-pages/features

Get GitHub Copilot Features page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `FeaturesPageData` with page content and related items

---

#### GET /api/custom-pages/genai-basics

Get GenAI Basics page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `GenAIBasicsPageData` with page content and related items

---

#### GET /api/custom-pages/genai-advanced

Get GenAI Advanced page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `GenAIAdvancedPageData` with page content and related items

---

#### GET /api/custom-pages/genai-applied

Get GenAI Applied page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `GenAIAppliedPageData` with page content and related items

---

#### GET /api/custom-pages/sdlc

Get SDLC page data.

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `SDLCPageData` with page content and related items

---

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

See [tests/AGENTS.md](../tests/AGENTS.md) and [tests/TechHub.Api.Tests/AGENTS.md](../tests/TechHub.Api.Tests/AGENTS.md) for complete testing documentation.

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

## RSS Feed Proxy Endpoints (Blazor Web)

The Blazor Web application provides proxy endpoints that serve RSS feeds from the same domain as the website, avoiding cross-origin issues and exposing the API publicly.

**Architecture**: Web proxy endpoints call the API internally via `TechHubApiClient`, then serve RSS XML to end users from the same domain (`tech.hub.ms`).

### GET /feed.xml

Get RSS feed for all content across all sections.

**Response**: `200 OK` with `application/rss+xml; charset=utf-8`

**Example**:

```bash
curl -k https://localhost:5003/feed.xml
```

**Discovery Link**: Available on Home page (`<link rel="alternate" type="application/rss+xml" href="/feed.xml">`)

---

### GET /{sectionName}/feed.xml

Get RSS feed for a specific section.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`, `ml`)

**Response**: `200 OK` or `404 Not Found` with `application/rss+xml; charset=utf-8`

**Examples**:

```bash
curl -k https://localhost:5003/ai/feed.xml
curl -k https://localhost:5003/github-copilot/feed.xml
curl -k https://localhost:5003/azure/feed.xml
```

**Discovery Link**: Available on Section pages (`<link rel="alternate" type="application/rss+xml" href="/ai/feed.xml">`)

---

### GET /collection/{collectionName}/feed.xml

Get RSS feed for a specific collection.

**Parameters**:

- `collectionName` (path): Collection name (e.g., `roundups`, `news`, `blogs`)

**Response**: `200 OK` or `404 Not Found` with `application/rss+xml; charset=utf-8`

**Example**:

```bash
curl -k https://localhost:5003/collection/roundups/feed.xml
```

**Usage**: Available for content collections (roundups is most common)

---

**Why Proxy Endpoints?**

- **Same Domain**: Feeds served from `tech.hub.ms`, not `api.tech.hub.ms`
- **Security**: API not exposed publicly - Web app calls API internally
- **No CORS**: Avoids cross-origin issues for RSS readers
- **Consistent URLs**: User-facing URLs match site structure
- **Rate Limiting**: Can implement rate limiting at proxy layer
- **Monitoring**: Track RSS usage separately from API calls

**Implementation**: See `TechHubApiClient.cs` for HTTP client methods and `Program.cs` for proxy endpoint routing.

## Tag Endpoints

Tag endpoints provide access to tag information, tag clouds with quantile-based sizing, and tag usage statistics.

### GET /api/tags/all

Get all tags with their usage counts across all content or within a specific scope.

**Parameters** (all optional):

- `sectionName` (query): Filter to specific section (e.g., `ai`, `github-copilot`)
- `collectionName` (query): Filter to specific collection (e.g., `news`, `blogs`)

**Response**:

```json
{
  "tags": [
    { "tag": "ai", "count": 152 },
    { "tag": "github-copilot", "count": 89 },
    { "tag": "azure", "count": 67 }
  ]
}
```

**Ordering**: Tags are sorted by count (descending), then alphabetically

**Example**:

```bash
# All tags across entire site
curl -k https://localhost:5001/api/tags/all

# Tags only from AI section
curl -k https://localhost:5001/api/tags/all?sectionName=ai

# Tags from blogs collection in GitHub Copilot section
curl -k https://localhost:5001/api/tags/all?sectionName=github-copilot&collectionName=blogs
```

---

### GET /api/tags/cloud

Get a tag cloud with quantile-based sizing for visual representation.

**Parameters**:

- `scope` (query, **required**): Scoping level - `Homepage`, `Section`, `Collection`, or `Content`
- `sectionName` (query): Required when `scope=Section`, `Content`, or `Collection` + `Section`
- `collectionName` (query): Required when `scope=Collection`
- `maxTags` (query, optional): Maximum number of tags (default: 20)
- `minUses` (query, optional): Minimum tag usage count (default: 5)
- `lastDays` (query, optional): Filter to content from last N days (default: 90)

**Response**:

```json
[
  { "tag": "ai", "count": 152, "size": 2 },
  { "tag": "github-copilot", "count": 89, "size": 2 },
  { "tag": "azure", "count": 67, "size": 1 },
  { "tag": "devops", "count": 45, "size": 1 },
  { "tag": "security", "count": 23, "size": 0 }
]
```

**Tag Sizes** (quantile-based):

- `2` (Large): Top 25% of tags by usage
- `1` (Medium): Middle 50% of tags
- `0` (Small): Bottom 25% of tags

**Examples**:

```bash
# Homepage tag cloud (top tags across all content)
curl -k https://localhost:5001/api/tags/cloud?scope=Homepage

# Section-specific tag cloud
curl -k https://localhost:5001/api/tags/cloud?scope=Section&sectionName=ai

# Collection-specific tag cloud
curl -k https://localhost:5001/api/tags/cloud?scope=Collection&collectionName=blogs

# Content-specific tag cloud (section + collection)
curl -k https://localhost:5001/api/tags/cloud?scope=Content&sectionName=github-copilot&collectionName=videos

# Limit to top 10 tags from last 30 days
curl -k "https://localhost:5001/api/tags/cloud?scope=Homepage&maxTags=10&lastDays=30"
```

**Validation**:

- `400 Bad Request` if scope is invalid or required parameters are missing
- `400 Bad Request` if `scope=Section` without `sectionName`
- `400 Bad Request` if `scope=Collection` without `collectionName`
- `400 Bad Request` if `scope=Content` without both parameters

---

## Future Enhancements

Planned features (not yet implemented):

- **Pagination**: `?page=1&pageSize=50` for large result sets
- **Sorting**: `?sortBy=date&order=desc` for custom ordering
- **Tag filtering modes**: `?tagMatch=any` for OR logic (currently AND only)
- **Caching headers**: ETag and Last-Modified for efficient caching
- **Individual items**: `/api/items/{collectionName}/{id}` for direct item access
- **Rate limiting**: Protect against abuse
- **Authentication/Authorization**: Secure access control
