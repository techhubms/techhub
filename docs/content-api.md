# Content API

This document describes the core Content API endpoints for retrieving sections, collections, content items, and custom pages.

**Related Documentation**:

- [Filtering](filtering.md) - Tag filtering behavior and tag cloud API
- [Custom Pages](custom-pages.md) - Custom page endpoints for specialized content
- [RSS Feeds](rss-feeds.md) - RSS feed endpoints for content syndication
- [Newsletter](newsletter.md) - Newsletter subscription API, schedule and email delivery
- [Admin](admin.md) - Admin API (`/api/admin/*`) and admin UI pages
- [Database Configuration](database.md) - Supported database providers and sync behavior
- [Terminology](terminology.md) - Section and collection definitions

## Overview

The Tech Hub Content API provides RESTful access to content organized by sections and collections. It uses a nested route structure that mirrors the site's hierarchical organization.

**Content Storage**: The API serves content exclusively from a PostgreSQL database backend. All content is populated via background services and administrative tools.

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
  "tag": "AI",
  "hideCollectionPages": false,
  "showHeroBanner": false,
  "collections": [
    {
      "title": "News",
      "name": "news",
      "url": "/ai/news",
      "description": "News articles from official product sources.",
      "displayName": "News",
      "isCustom": false,
      "order": 0
    }
  ]
}
```

`tag` is the display tag of the section. `hideCollectionPages` replaces the per-collection sub-navigation with a unified Browse page, and `showHeroBanner` renders the collapsible hero banner. On a collection, `order` only matters for custom pages (lower values appear first).

### Content Item (ContentItem)

Represents individual content (articles, videos, blogs, etc.) in API responses.

```json
{
  "slug": "2024-01-15-example-post",
  "title": "Example Post",
  "author": "Jane Doe",
  "dateEpoch": 1705276800,
  "collectionName": "news",
  "feedName": "Microsoft AI Blog",
  "primarySectionName": "ai",
  "sections": ["ai", "github-copilot"],
  "tags": ["copilot", "ai", "productivity"],
  "excerpt": "First paragraph excerpt...",
  "externalUrl": "https://techcommunity.microsoft.com/example-post"
}
```

**Key Fields**:

- `slug`: Unique identifier derived from filename
- `title`, `author`, `excerpt`: Content metadata
- `dateEpoch`: Publication date as Unix timestamp
- `collectionName`: Collection this item belongs to (`news`, `blogs`, `videos`, `community`, `roundups`)
- `feedName`: Original RSS feed source name
- `primarySectionName`: Highest-priority section name used for URL routing
- `sections`: Array of lowercase section identifiers this content belongs to
- `tags`: Content tags for filtering
- `externalUrl`: Source URL - external link for aggregated content, internal path for generated content (present in all collections)

### Linking Strategy (Internal vs. External)

Content linking behavior is determined by **collection name**, not by the `externalUrl` property. All collections have an `externalUrl` field, but it's only used for actual external linking:

| Collection | Links To | Detail Endpoint | `externalUrl` Usage |
|------------|----------|-----------------|---------------------|
| `news` | External URL | Returns metadata without `renderedHtml` | Used for linking |
| `blogs` | External URL | Returns metadata without `renderedHtml` | Used for linking |
| `community` | External URL | Returns metadata without `renderedHtml` | Used for linking |
| `videos` | Internal page | Returns content | Stored but not used for linking |
| `roundups` | Internal page | Returns content | Internal path, not used |

- **External Collections** (news, blogs, community): UI cards link directly to `externalUrl`. The canonical internal URL (`/section/collection/slug`) still renders a summary (excerpt + a link to the original source) so shared links always land on Tech Hub first.
- **Internal Collections** (videos, roundups): Content is displayed on the internal detail page at `/section/collection/slug`.

**Note**: Tech Hub aggregates external content but presents videos and roundups internally. The `externalUrl` field stores the source URL for all content (used for RSS feeds and attribution), but the linking behavior is controlled by collection type.

### ContentItem Navigation Methods

The `ContentItem` domain model provides helper methods for navigation and URL generation:

| Method | Purpose | Returns |
|--------|---------|---------|
| `LinksExternally()` | Checks if item links to external source | `bool` |
| `GetHref(sectionOverride?)` | Gets the navigation URL (external or internal) | `string` |
| `GetCanonicalHref(sectionOverride?)` | Gets the internal Tech Hub URL, always `/section/collection/slug`, even for external items | `string` |
| `GetTarget()` | Gets link target attribute | `"_blank"` or `null` |
| `GetRel()` | Gets link rel attribute | `"noopener noreferrer"` or `null` |
| `GetAriaLabel()` | Gets accessibility label for screen readers | `string` |

`GetAriaLabel()` always returns a value: `"{Title} - opens in new tab"` for external items, or just `"{Title}"` for internal ones.

**Usage in Blazor**:

```razor
<a href="@item.GetHref(SectionName)"
   target="@item.GetTarget()"
   rel="@item.GetRel()"
   aria-label="@item.GetAriaLabel()">
    @item.Title
</a>
```

`GetCanonicalHref()` powers the card's share button: it always copies the Tech Hub internal URL to
the clipboard — regardless of whether the card itself opens externally — so recipients always land
on Tech Hub first.

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

**Response**: `200 OK`, `204 No Content` (section not found), or `400 Bad Request` (invalid `sectionName` format)

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
- `q` (optional): Search query, maximum 200 characters (searches title, excerpt, content, feed name, collection name and section name; tags are filtered with `tags`, not searched)
- `tags` (optional): Comma-separated tags (AND logic - items must have all tags). Maximum 20 tags of up to 100 characters each
- `types` (optional): Comma-separated collection types to include (e.g., `news,blogs`). Only applies when `collectionName` is `all`. Unknown types are ignored
- `exact` (optional): `true` switches to an exact title phrase match (see [filtering.md](filtering.md#exact-phrase-match)). Ignored without `q`. Tags, dates and `types` are ignored in exact mode
- `lastDays` (optional): Filter to content from last N days (0-3650). **Default behavior**: When no `lastDays`, `from`, or `to` parameters are provided, a default 90-day filter is automatically applied (`AppSettings:TagCloud:DefaultDateRangeDays`). Pass `lastDays=0` to explicitly disable date filtering and return all content regardless of date.
- `from` (optional): Start date for custom range (ISO 8601 format, e.g., `2024-01-15`). Takes precedence over `lastDays`
- `to` (optional): End date for custom range (ISO 8601 format, e.g., `2024-06-15`). Takes precedence over `lastDays`

**Response**: `200 OK`, `404 Not Found` (section or collection not found), or `400 Bad Request` (invalid name format, `q` over 200 characters, more than 20 tags, a tag over 100 characters, `lastDays` outside 0-3650, or an unparseable `from`/`to`)

**Response Body**: `{ "items": [ContentItem, ...], "totalCount": 123 }`. `totalCount` is the number of matches before `take`/`skip` is applied.

```bash
curl -k https://localhost:5001/api/sections/ml/collections/videos/items
```

#### GET /api/sections/{sectionName}/collections

Get all collections available in a section.

**Parameters**:

- `sectionName` (path): Section identifier

**Response**: `200 OK`, `204 No Content` (section not found), or `400 Bad Request` (invalid `sectionName` format)

```bash
curl -k https://localhost:5001/api/sections/github-copilot/collections
```

#### GET /api/sections/{sectionName}/collections/{collectionName}

Get details about a specific collection within a section.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name (e.g., `news`, `blogs`, `videos`, `community`, `roundups`)

**Response**: `200 OK`, `204 No Content` (section or collection not found), or `400 Bad Request` (invalid `sectionName` or `collectionName` format)

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

**Response**: `200 OK`, `204 No Content` (section, collection, or slug not found), or `400 Bad Request` (invalid `sectionName`, `collectionName`, or `slug` format)

**Response Body**: `ContentItemDetail` with metadata, TOC and the rendered HTML

```bash
curl -k "https://localhost:5001/api/sections/ai/collections/videos/2024-06-ai-overview"
```

**Note**: Every valid slug returns `200 OK` with a `ContentItemDetail`. For items in externally-linking collections (`news`, `blogs`, `community`), `renderedHtml` is null because the Web app renders a summary (excerpt + a link to the original source) at the canonical URL instead. Other collections include the rendered HTML.

### Authors

#### GET /api/authors

Get all authors with their published content item counts.

**Response**: `200 OK`

Returns authors sorted alphabetically (case-insensitive).

```bash
curl -k https://localhost:5001/api/authors
```

**Response Body**:

```json
[
  { "name": "Alice Example", "itemCount": 12 },
  { "name": "Bob Smith", "itemCount": 5 }
]
```

#### GET /api/authors/{authorName}/items

Get paginated content items attributed to a specific author across all sections and collections.

**Parameters**:

- `authorName` (path): Author display name (URL-encoded if contains spaces, e.g., `Alice%20Example`)

**Query Parameters**:

- `take` (optional): Number of items to return (default: 20, max: 50)
- `skip` (optional): Number of items to skip for pagination

**Response**: `200 OK`, `400 Bad Request` (invalid author name), or `204 No Content` (author not found)

```bash
curl -k "https://localhost:5001/api/authors/Alice%20Example/items"
```

**Notes**:

- Author name matching is case-insensitive (URL parameter) but the stored name casing is used for the database query.
- Only published items are returned.
- The author name is URL-decoded first and must not contain path separators or null bytes (maximum 200 characters).
- The response body has the same `{ items, totalCount }` shape as the items endpoint.

### Tag Cloud

#### GET /api/sections/{sectionName}/collections/{collectionName}/tags

Get tag cloud with quantile-based sizing for visual representation.

**Parameters**:

- `sectionName` (path): Section identifier
- `collectionName` (path): Collection name (use `all` for all collections)

**Query Parameters**:

- `maxTags` (optional): Maximum number of tags to return (default: 20)
- `minUses` (optional): Minimum tag usage count (default: 1)
- `lastDays` (optional): Filter to content from last N days (0-3650, default: 90 days via `AppSettings:TagCloud:DefaultDateRangeDays`)
- `tags` (optional): Comma-separated list of currently selected tags for dynamic count calculation. Maximum 20 tags of up to 100 characters each
- `tagsToCount` (optional): Comma-separated list of specific tags to get counts for. When provided, returns counts only for these tags (used by content item pages to show real section counts for the item's tags). Display names in the response match the casing provided in this parameter.
- `q` (optional): Text search query (maximum 200 characters). Counts only reflect items matching the search
- `types` (optional): Comma-separated collection types to include. Only applies when `collectionName` is `all`
- `from` (optional): Start date for custom range (ISO 8601 format, e.g., `2024-01-15`). Takes precedence over `lastDays`
- `to` (optional): End date for custom range (ISO 8601 format, e.g., `2024-06-15`). Takes precedence over `lastDays`

**Response**: `200 OK`, `404 Not Found` (section or collection not found), or `400 Bad Request` (invalid date format, `q` too long, `lastDays` out of range, or too many/too long `tags`)

```bash
curl -k "https://localhost:5001/api/sections/all/collections/all/tags?maxTags=30"
```

## GHC Features API

Endpoints for GitHub Copilot feature metadata stored in the `ghc_features` table.
See [content-schema.md](content-schema.md#github-copilot-features) for the schema.

### GET /api/ghc-features

Returns all GHC features with their content links. Public endpoint (no authentication required).

**Response**: `200 OK`

```json
[
  {
    "slug": "copilot-free-plan",
    "title": "GitHub Copilot Free Plan",
    "description": "Free tier for individual developers.",
    "releaseDate": 1700000000,
    "plans": ["Free", "Pro"],
    "ghesSupport": false,
    "contentLinks": [
      {
        "featureSlug": "copilot-free-plan",
        "collectionName": "videos",
        "itemSlug": "intro-to-copilot-free",
        "isThumbnail": true,
        "sortOrder": 1,
        "itemTitle": "Introduction to GitHub Copilot Free",
        "itemExternalUrl": "https://youtube.com/watch?v=..."
      }
    ],
    "thumbnailLink": { ... }
  }
]
```

### PUT /api/ghc-features/{slug}

Create or update a GHC feature. **Requires AdminOnly authorization.**

**Body**: `GhcFeature` JSON (same schema as GET response item). `slug` in the body must match the URL, `title` is required and `plans` must be an array (use `[]` when no plan applies).

**Response**: `204 No Content` or `400 Bad Request` (slug mismatch, empty title or null plans)

### DELETE /api/ghc-features/{slug}

Delete a GHC feature and its content links. **Requires AdminOnly authorization.**

**Response**: `204 No Content` or `404 Not Found`

### POST /api/ghc-features/{slug}/content-links

Link a content item (usually a video) to a feature. **Requires AdminOnly authorization.**

**Body**: `{ "collectionName": "videos", "itemSlug": "intro-to-copilot-free", "isThumbnail": false, "sortOrder": 0 }`. `collectionName` and `itemSlug` are required. Linking an item again updates its `isThumbnail` and `sortOrder`, and setting `isThumbnail` clears the previous thumbnail.

**Response**: `204 No Content`, `400 Bad Request` (missing fields), or `404 Not Found` (feature or content item does not exist)

### DELETE /api/ghc-features/{slug}/content-links

Remove a content link. **Requires AdminOnly authorization.**

**Query Parameters**: `collection` and `itemSlug` (both required)

**Response**: `204 No Content`, `400 Bad Request` (missing parameters), or `404 Not Found`

### PUT /api/ghc-features/{slug}/thumbnail

Mark one of the feature's existing content links as its thumbnail. A feature has at most one thumbnail. **Requires AdminOnly authorization.**

**Query Parameters**: `collection` and `itemSlug` (both required)

**Response**: `204 No Content`, `400 Bad Request` (missing parameters), or `404 Not Found` (the link does not exist)

## VS Code Updates API

### GET /api/vscode-updates

Returns the content items shown on the VS Code Updates page (`/github-copilot/vscode-updates`). Public endpoint (no authentication required). The endpoint has no pagination parameters and returns at most 100 items.

**Response**: `200 OK` with an array of `{ collectionName, slug, title, externalUrl, primarySectionName, dateEpoch, createdAt }`

Admins manage the membership through the admin API, see [admin.md](admin.md).

## Legacy Redirect API

### GET /api/legacy-redirect

Resolves a slug from the old website to the canonical URL. The Web app calls it from URL normalization when a request ends in `.html` (see [url-routing.md](url-routing.md#url-normalization)).

**Query Parameters**:

- `slug` (required): Legacy slug, with or without a `YYYY-MM-DD-` prefix and `.html` extension
- `section` (optional): Section hint. When several items match, the item from this section wins

**Response**: `200 OK` with `{ "url": "..." }`, `204 No Content` (no matching item), or `400 Bad Request` (invalid `slug` or `section` format)

`url` is the original source URL for items in externally-linking collections and the internal path (`/{section}/{collection}/{slug}`) for everything else. The old weekly roundup permalink `weekly-ai-and-tech-news-roundup` resolves to the latest roundup instead of an exact match.

## Other APIs

- **Newsletter**: Public subscribe, confirm, manage and unsubscribe endpoints. See [newsletter.md](newsletter.md).
- **Admin**: `/api/admin/*` (processing triggers, feeds, reviews, content editing, job settings, cache invalidation and more). See [admin.md](admin.md) and [admin-authentication.md](admin-authentication.md).
- **Custom pages**: See [custom-pages.md](custom-pages.md).
- **RSS and sitemap**: See [rss-feeds.md](rss-feeds.md) and [seo.md](seo.md).

## Error Responses

### 400 Bad Request

Returned when route parameters have invalid format. All path parameters are validated:

- **sectionName / collectionName**: Must start with a letter and contain only letters and hyphens, case-insensitive (e.g., `ai`, `github-copilot`, `AI`). Rejects dots, slashes, encoded characters and leading hyphens. Maximum 200 characters.
- **slug**: Must start with a letter or digit and contain only letters, digits and hyphens, case-insensitive. Rejects file extensions (`.php`), encoded HTML, path traversal. Maximum 200 characters.

Validation is enforced via an endpoint filter on the `/api/sections` group and the `/api/rss` group. See `RouteParameterValidator` in `TechHub.Core/Validation/`. Query parameters have their own limits, listed per endpoint above and in [input-validation-and-sanitization.md](input-validation-and-sanitization.md).

Every `400` returned by an endpoint handler is logged as a warning (method, path and reason) by `RejectedRequestLogging`. The query string is never logged because it can carry emails and tokens, and handlers must not echo untrusted request values in their 400 reason, and framework-generated binding failures such as malformed JSON bodies never reach endpoint filters, so they are not covered.

### 204 No Content

Lookup endpoints answer a missing resource with `204` instead of `404`, because they are data lookups and not page requests: section, collections, collection and content detail endpoints, authors and legacy redirect. Clients treat `204` as "not found".

### 404 Not Found

Returned by the items and tag cloud endpoints when the section or collection does not exist, by the RSS endpoints for unknown sections or collections, and by the GHC feature endpoints for unknown features.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404
}
```

## MCP Server

The Web frontend exposes the latest content to AI clients through a Model Context Protocol (MCP) server at `/mcp` over stateless Streamable HTTP. The server does not create sessions, emit an `Mcp-Session-Id` header, or expose MCP `GET`/`DELETE` transport operations.

- **Landing page**: Opening `/mcp` in a browser on `tech.hub.ms` or `tech.xebia.ms` shows an overview, the branded public server URL (never `localhost`), connection snippets, and the available tools and options. The homepage-style navigation shows an `MCP` button next to `Newsletter` on the homepage, newsletter pages, and the MCP page itself. An MCP client requesting `GET /mcp` with `Accept: text/event-stream` and no acceptable HTML representation receives `405` with `Allow: POST` instead of the landing page.

- **Latest-content tools**: one `get_latest_<section>_content` tool per section (hyphens in the section name become underscores, e.g. `get_latest_github_copilot_content`), plus `get_latest_content` for the aggregate `all` section. Items can belong to several sections, so `get_latest_content` lists each item once and is the tool to prefer when no single section is needed. Tools reflect the site's configured content sections. Each tool has a readable title and is declared read-only.
- **Filters**: every latest-content tool accepts the optional arguments below; the tool descriptions and server instructions explain them to the client.

  | Argument | Meaning |
  |---|---|
  | `page` | Page number, 1-100 (default 1). |
  | `days` | Only items from the last N days (0-3650). Without `days` and `since` the content API's default window of 90 days applies; `0` removes it. |
  | `since` | Only items on or after this date (`yyyy-MM-dd`). Takes precedence over `days`. |
  | `contentType` | Comma-separated collections: `news`, `blogs`, `videos`, `community`, `roundups`. `roundups` returns the weekly summaries. |
  | `tags` | Comma-separated tags (maximum 20 of up to 100 characters); items must have all of them. |
  | `query` | Keyword search (maximum 200 characters), the same search as `q` on the items endpoint. |
  | `includeTags` | Include the tags of every item. Off by default to keep results small. |

  Invalid values return a readable tool error instead of a generic failure.
- **Paging**: Newest first, 10 items per page. The response contains `items`, `page`, `pageSize`, `totalPages` (capped at 100), `totalCount` and `generatedAt` (the server's current time in Europe/Brussels, so clients can judge how fresh the data is). Every item has `title`, `author`, `date` (ISO 8601, Europe/Brussels), `collection`, `excerpt` and an absolute `url` to its Tech Hub page, plus `tags` when requested. The original article's URL is deliberately not exposed: the Tech Hub page of a news, blog or community item shows the summary and a button to open the original, so linking to `url` keeps Tech Hub in the loop and brings traffic.
- **`get_roundup_content`**: takes the `url` of a roundup (an absolute URL on the configured server origin or a path such as `/ai/roundups/some-roundup`, found with a latest-content tool and `contentType=roundups`) and returns `title`, `date`, `section`, `url` and the rendered HTML `content` (the AI-generated editorial summary of the week). Urls of other collections are rejected with a readable tool error. This is the only tool that returns body content: for every other item the `excerpt` is the summary and clients link users to the item's Tech Hub `url`.
- **Item links**: Absolute URLs use the configured public server origin (`https://tech.hub.ms` by default, `https://localhost:5003` in Development), never the request `Host` header.
- **Origin validation**: Requests with an `Origin` header are rejected with `403` unless the origin uses HTTPS and matches a configured public site host or the server's public base URL. In Development, loopback origins are also allowed. This prevents DNS-rebinding attacks as required by the MCP Streamable HTTP transport. Requests without `Origin` (non-browser clients) are allowed.
- **Caching**: Unfiltered requests may return recently cached results. Filtered requests are not cached so arbitrary filter combinations do not create stale cached variants.
- **Throttling**: Requests are limited to 60 per minute per IP (loopback exempt in Development only). Excess requests receive `429` with `Retry-After`.
- **Instrumentation**: See [MCP Server Telemetry](telemetry.md#mcp-server-telemetry).

## Performance Characteristics

- **Sections**: ~25ms response time
- **Content (first load)**: ~500ms (database query)
- **Content (cached)**: < 100ms
- **Filtering**: Varies based on criteria (typically < 2 seconds)

## Testing

See [tests/TechHub.Api.Tests/AGENTS.md](../tests/TechHub.Api.Tests/AGENTS.md) for comprehensive test documentation.
The API has 100% integration test coverage for all endpoints including error scenarios.
