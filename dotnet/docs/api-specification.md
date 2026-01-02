# Tech Hub API Specification

## Overview

The Tech Hub API provides RESTful access to content organized by sections and collections. It uses a nested route structure that mirrors the site's hierarchical organization.

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

## Migration Notes

### From Old Flat Structure

The previous flat structure has been replaced:

**Old** (deprecated):
- ❌ `GET /api/content` - Use `/api/content/filter` instead
- ❌ `GET /api/content/collection/{collection}` - Use `/api/sections/{section}/collections/{collection}/items`
- ❌ `GET /api/content/category/{category}` - Use `/api/sections/{section}/items`

**New** (current):
- ✅ `GET /api/sections/{section}/items` - All items in a section
- ✅ `GET /api/sections/{section}/collections/{collection}/items` - Items in specific collection
- ✅ `GET /api/content/filter` - Advanced multi-criteria filtering

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

## Future Enhancements

Planned features (not yet implemented):

- Pagination support for large result sets
- Sorting options (by date, title, etc.)
- OR logic for tags (currently AND only)
- RSS feed endpoints
- Individual item endpoints (`/api/items/{collection}/{id}`)
- Caching headers (ETag, Last-Modified)
- Rate limiting
- Authentication/Authorization

