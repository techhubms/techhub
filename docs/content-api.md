# Content API

This document describes the core Content API endpoints for retrieving sections, collections, content items, and custom pages.

**Related Documentation**:

- [Filtering](filtering.md) - Tag filtering behavior and tag cloud API
- [Custom Pages](custom-pages.md) - Custom page endpoints for specialized content
- [RSS Feeds](rss-feeds.md) - RSS feed endpoints for content syndication
- [Database Configuration](database.md) - Supported database providers and sync behavior
- [Terminology](terminology.md) - Section and collection definitions

## Overview

The Tech Hub Content API provides RESTful access to content organized by sections and collections. It uses a nested route structure that mirrors the site's hierarchical organization.

**Content Storage**: The API serves content from a database backend (PostgreSQL or FileSystem) configured via appsettings.json. The database syncs from markdown files in the `collections/` folder on startup.

**Key Design Principles**:

- **Resource hierarchy**: Routes mirror domain model (`/sections/{sectionName}/collections/{collectionName}/items`)
- **Consistent naming**: Both sections and collections use human-readable slugs (no arbitrary IDs)
- **Self-documenting**: URL structure explains what data is returned

## Data Model

### Section

Represents a thematic grouping of content (e.g., AI, GitHub Copilot, Azure).

```json
{
  "name": "ai",
  "title": "AI",
  "description": "Your gateway to the AI revolution...",
  "url": "/ai",
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

### Content Item (ContentItem)

Represents individual content (articles, videos, blogs, etc.) in API responses.

```json
{
  "slug": "2024-01-15-example-post",
  "title": "Example Post",
  "author": "Jane Doe",
  "dateEpoch": 1705276800,
  "collectionName": "news",
  "subcollectionName": null,
  "feedName": "Microsoft AI Blog",
  "primarySectionName": "ai",
  "sections": ["ai", "github-copilot"],
  "tags": ["copilot", "ai", "productivity"],
  "excerpt": "First paragraph excerpt...",
  "externalUrl": "https://techcommunity.microsoft.com/example-post",
  "draft": false,
  "plans": [],
  "ghesSupport": false
}
```

**Key Fields**:

- `slug`: Unique identifier derived from filename
- `title`, `author`, `excerpt`: Content metadata
- `dateEpoch`: Publication date as Unix timestamp
- `collectionName`: Collection this item belongs to (`news`, `blogs`, `videos`, `community`, `roundups`)
- `subcollectionName`: Optional subcollection (e.g., `ghc-features`, `vscode-updates`)
- `feedName`: Original RSS feed source name
- `primarySectionName`: Highest-priority section name used for URL routing
- `sections`: Array of lowercase section identifiers this content belongs to
- `tags`: Content tags for filtering
- `externalUrl`: Source URL - external link for aggregated content, internal path for generated content (present in all collections)
- `draft`: Whether content is a draft (used for Features page "Coming Soon" items)
- `plans`: GitHub Copilot subscription plans (only for ghc-features subcollection)
- `ghesSupport`: Whether feature supports GitHub Enterprise Server (only for ghc-features)

### Linking Strategy (Internal vs. External)

Content linking behavior is determined by **collection name**, not by the `externalUrl` property. All collections have an `externalUrl` field, but it's only used for actual external linking:

| Collection | Links To | Detail Endpoint | `externalUrl` Usage |
|------------|----------|-----------------|---------------------|
| `news` | External URL | Returns `404` | Used for linking |
| `blogs` | External URL | Returns `404` | Used for linking |
| `community` | External URL | Returns `404` | Used for linking |
| `videos` | Internal page | Returns content | Stored but not used for linking |
| `roundups` | Internal page | Returns content | Internal path, not used |

- **External Collections** (news, blogs, community): UI cards link directly to `externalUrl`. The detail endpoint returns `404 Not Found`.
- **Internal Collections** (videos, roundups): Content is displayed on the internal detail page at `/section/collection/slug`.

**Note**: Tech Hub aggregates external content but presents videos and roundups internally. The `externalUrl` field stores the source URL for all content (used for RSS feeds and attribution), but the linking behavior is controlled by collection type.

### ContentItem Navigation Methods

The `ContentItem` domain model provides helper methods for navigation and URL generation:

| Method | Purpose | Returns |
|--------|---------|---------|
| `LinksExternally()` | Checks if item links to external source | `bool` |
| `GetHref()` | Gets the navigation URL (external or internal) | `string` |
| `GetTarget()` | Gets link target attribute | `"_blank"` or `null` |
| `GetRel()` | Gets link rel attribute | `"noopener noreferrer"` or `null` |
| `GetAriaLabel()` | Gets accessibility label | `"Opens in new tab"` or `null` |
| `GetDataEnhanceNav()` | Gets Blazor enhance attribute | `"false"` or `null` |
| `GetUrlInSection(section)` | Gets URL for specific section context | `string` |
| `GetPrimarySectionUrl()` | Gets URL using primary section | `string` |

**Usage in Blazor**:

```razor
<a href="@item.GetHref()"
   target="@item.GetTarget()"
   rel="@item.GetRel()"
   aria-label="@item.GetAriaLabel()"
   data-enhance-nav="@item.GetDataEnhanceNav()">
    @item.Title
</a>
```

For detailed information about filtering content by tags, see [filtering.md](filtering.md).

## Endpoints

### Sections

#### GET /api/sections

Get all sections.

**Response**: `200 OK`

```bash
curl -k https://localhost:5001/api/sections
```

#### GET /api/sections/{sectionName}

Get a specific section by name.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`, `ml`)

**Response**: `200 OK` or `404 Not Found`

```bash
curl -k https://localhost:5001/api/sections/ai
```

#### GET /api/sections/{sectionName}/collections/{collectionName}/items

Get items in a specific collection within a section. Use `all` as collectionName to get all items in the section across all collections.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name (use `all` for all collections)

**Query Parameters**:

- `take` (optional): Number of items to return (default: 20, max: 50)
- `skip` (optional): Number of items to skip for pagination
- `q` (optional): Search query (searches title, description, tags)
- `tags` (optional): Comma-separated tags (AND logic - items must have all tags)
- `subcollection` (optional): Filter by subcollection
- `lastDays` (optional): Filter to content from last N days
- `from` (optional): Start date for custom range (ISO 8601 format, e.g., `2024-01-15`). Takes precedence over `lastDays`
- `to` (optional): End date for custom range (ISO 8601 format, e.g., `2024-06-15`). Takes precedence over `lastDays`
- `includeDraft` (optional): Include draft content (default: false)

**Response**: `200 OK` or `404 Not Found`

```bash
curl -k https://localhost:5001/api/sections/ml/collections/videos/items
```

#### GET /api/sections/{sectionName}/collections

Get all collections available in a section.

**Parameters**:

- `sectionName` (path): Section identifier

**Response**: `200 OK` or `404 Not Found`

```bash
curl -k https://localhost:5001/api/sections/github-copilot/collections
```

#### GET /api/sections/{sectionName}/collections/{collectionName}

Get details about a specific collection within a section.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name (e.g., `news`, `blogs`, `videos`, `community`, `roundups`)

**Response**: `200 OK` or `404 Not Found`

```bash
curl -k https://localhost:5001/api/sections/ai/collections/news
```

### Content Items

### Content Detail

#### GET /api/sections/{sectionName}/collections/{collectionName}/{slug}

Get detailed content item by section, collection, and content slug.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name
- `slug` (path): Content slug

**Response**: `200 OK` or `404 Not Found`

**Response Body**: `ContentItemDetail` with full rendered HTML, metadata, and TOC

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/videos/2024-06-ai-overview"
```

**Note**: Only `videos` and `roundups` collections return content. External collections (`news`, `blogs`, `community`) return `404 Not Found`.

### Tag Cloud

#### GET /api/sections/{sectionName}/collections/{collectionName}/tags

Get tag cloud with quantile-based sizing for visual representation.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name (use `all` for all collections)

**Query Parameters**:

- `maxTags` (optional): Maximum number of tags to return (default: 20)
- `minUses` (optional): Minimum tag usage count (default: 5)
- `lastDays` (optional): Filter to content from last N days (default: 90 days via `AppSettings:Filtering:TagCloud:DefaultDateRangeDays`)
- `tags` (optional): Comma-separated list of currently selected tags for dynamic count calculation
- `tagsToCount` (optional): Comma-separated list of specific tags to get counts for. When provided, returns counts only for these tags (used by content item pages to show real section counts for the item's tags). Display names in the response match the casing provided in this parameter.
- `from` (optional): Start date for custom range (ISO 8601 format, e.g., `2024-01-15`). Takes precedence over `lastDays`
- `to` (optional): End date for custom range (ISO 8601 format, e.g., `2024-06-15`). Takes precedence over `lastDays`

**Response**: `200 OK`, `404 Not Found`, or `400 Bad Request` (invalid date format)

```bash
curl -k "https://localhost:5001/api/sections/all/collections/all/tags?maxTags=30"
```

### Error Responses

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

## Testing

See [tests/TechHub.Api.Tests/AGENTS.md](../tests/TechHub.Api.Tests/AGENTS.md) for comprehensive test documentation.
The API has 100% integration test coverage for all endpoints including error scenarios.
