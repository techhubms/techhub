# API Contracts: Tech Hub REST API

**Generated**: 2026-01-02  
**Base URL**: `https://api.tech.hub.ms` (production) | `https://localhost:5001` (local)  
**Format**: REST with JSON responses  
**Authentication**: None required for MVP (public API)

## Overview

This directory contains REST API endpoint specifications for the Tech Hub backend. All endpoints return JSON and follow REST conventions.

---

## Endpoints

### 1. Get All Sections

**GET** `/api/sections`

Returns all configured sections with their collections.

**Response**: `200 OK`

```json
{
  "sections": [
    {
      "id": "ai",
      "title": "AI",
      "description": "Artificial Intelligence news and resources",
      "url": "/ai",
      "category": "ai",
      "backgroundImage": "/assets/section-backgrounds/ai.jpg",
      "collections": [
        {
          "title": "Latest News",
          "collection": "news",
          "url": "/ai/news",
          "description": "AI news and announcements",
          "isCustom": false
        }
      ]
    }
  ]
}
```

**Caching**: 1 hour absolute expiration

---

### 2. Get Section Content

**GET** `/api/sections/{sectionId}`

Returns section metadata with all content items for that section.

**Parameters**:
- `sectionId` (path) - Section identifier (e.g., "ai", "github-copilot")

**Query Parameters**:
- `collection` (optional) - Filter by collection (e.g., "news", "blogs")
- `limit` (optional) - Max items to return (default: all)

**Response**: `200 OK`

```json
{
  "section": {
    "id": "ai",
    "title": "AI",
    "description": "Artificial Intelligence news and resources",
    "url": "/ai",
    "category": "ai",
    "backgroundImage": "/assets/section-backgrounds/ai.jpg",
    "collections": [ /* collection references */ ]
  },
  "items": [
    {
      "id": "2026-01-02-example-article",
      "title": "Example Article Title",
      "description": "Brief summary of the article",
      "author": "Author Name",
      "dateIso": "2026-01-02",
      "dateEpoch": 1735776000,
      "collection": "news",
      "altCollection": null,
      "canonicalUrl": "/ai/news/2026-01-02-example-article.html",
      "categories": ["ai", "machine-learning"],
      "tags": ["azure-openai", "gpt-4"],
      "excerpt": "Short excerpt...",
      "externalUrl": null,
      "videoId": null
    }
  ],
  "totalCount": 150
}
```

**Error Responses**:
- `404 Not Found` - Section does not exist

**Caching**: 30 minutes sliding expiration

---

### 3. Get Content Item

**GET** `/api/content/{sectionId}/{collection}/{itemId}`

Returns full content item with rendered HTML.

**Parameters**:
- `sectionId` (path) - Section identifier
- `collection` (path) - Collection name (e.g., "news", "blogs")
- `itemId` (path) - Content item ID (slug)

**Response**: `200 OK`

```json
{
  "id": "2026-01-02-example-article",
  "title": "Example Article Title",
  "description": "Brief summary of the article",
  "author": "Author Name",
  "dateIso": "2026-01-02",
  "dateEpoch": 1735776000,
  "collection": "news",
  "altCollection": null,
  "canonicalUrl": "/ai/news/2026-01-02-example-article.html",
  "categories": ["ai", "machine-learning"],
  "tags": ["azure-openai", "gpt-4"],
  "renderedHtml": "<h1>Article Title</h1><p>Full content...</p>",
  "excerpt": "Short excerpt...",
  "externalUrl": null,
  "videoId": null
}
```

**Error Responses**:
- `404 Not Found` - Content item does not exist

**Caching**: 1 hour absolute expiration

---

### 4. Get Collection Items

**GET** `/api/collections/{collection}`

Returns all items from a specific collection across all sections.

**Parameters**:
- `collection` (path) - Collection name (e.g., "news", "roundups")

**Query Parameters**:
- `page` (optional) - Page number (default: 1)
- `pageSize` (optional) - Items per page (default: 20, max: 100)

**Response**: `200 OK`

```json
{
  "items": [ /* array of ContentItemDto */ ],
  "totalCount": 250,
  "pageSize": 20,
  "currentPage": 1,
  "hasNextPage": true
}
```

**Error Responses**:
- `404 Not Found` - Collection does not exist

**Caching**: 30 minutes sliding expiration

---

### 5. Get RSS Feed

**GET** `/api/rss/{sectionId}`

Returns RSS 2.0 feed for a section.

**Parameters**:
- `sectionId` (path) - Section identifier (or "all" for combined feed)

**Response**: `200 OK` (Content-Type: `application/rss+xml`)

```xml
<?xml version="1.0" encoding="UTF-8"?>
<rss version="2.0">
  <channel>
    <title>Tech Hub - AI</title>
    <description>Artificial Intelligence news and resources</description>
    <link>https://tech.hub.ms/ai</link>
    <language>en-us</language>
    <lastBuildDate>Thu, 02 Jan 2026 12:00:00 GMT</lastBuildDate>
    <item>
      <title>Example Article</title>
      <description>Article summary...</description>
      <link>https://tech.hub.ms/ai/news/2026-01-02-example-article.html</link>
      <guid>https://tech.hub.ms/ai/news/2026-01-02-example-article.html</guid>
      <pubDate>Thu, 02 Jan 2026 10:00:00 GMT</pubDate>
      <author>author@example.com (Author Name)</author>
      <category>AI</category>
      <category>Machine Learning</category>
    </item>
  </channel>
</rss>
```

**Error Responses**:
- `404 Not Found` - Section does not exist

**Caching**: 30 minutes absolute expiration

---

### 6. Search Content

**GET** `/api/search`

Search across all content (returns matching items).

**Query Parameters**:
- `q` (required) - Search query
- `section` (optional) - Filter by section
- `collection` (optional) - Filter by collection
- `page` (optional) - Page number (default: 1)
- `pageSize` (optional) - Items per page (default: 20, max: 100)

**Response**: `200 OK`

```json
{
  "query": "azure openai",
  "items": [ /* array of ContentItemDto */ ],
  "totalCount": 42,
  "pageSize": 20,
  "currentPage": 1,
  "hasNextPage": true
}
```

**Error Responses**:
- `400 Bad Request` - Missing or invalid query parameter

**Caching**: 5 minutes sliding expiration (search results change frequently)

---

### 7. Get Available Tags

**GET** `/api/tags`

Returns all unique tags across all content (for filter UI).

**Query Parameters**:
- `section` (optional) - Filter tags by section

**Response**: `200 OK`

```json
{
  "tags": [
    { "name": "azure-openai", "count": 45 },
    { "name": "gpt-4", "count": 32 },
    { "name": "copilot", "count": 128 }
  ]
}
```

**Caching**: 1 hour absolute expiration

---

### 8. Health Check

**GET** `/health`

Returns API health status.

**Response**: `200 OK`

```json
{
  "status": "Healthy",
  "timestamp": "2026-01-02T12:00:00Z",
  "version": "1.0.0",
  "uptime": "2d 3h 15m"
}
```

**No caching**

---

## Error Response Format

All error responses follow this structure:

```json
{
  "error": {
    "code": "NOT_FOUND",
    "message": "Section 'invalid-section' not found",
    "timestamp": "2026-01-02T12:00:00Z",
    "requestId": "abc123"
  }
}
```

**HTTP Status Codes**:
- `200 OK` - Success
- `400 Bad Request` - Invalid parameters
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error
- `503 Service Unavailable` - Service temporarily unavailable

---

## Rate Limiting

**Current**: No rate limiting (public API, low traffic)

**Future**: Consider adding rate limiting if abuse detected:
- 100 requests/minute per IP
- 1000 requests/hour per IP

---

## CORS Policy

**Allowed Origins**: `https://tech.hub.ms`, `http://localhost:*` (dev only)  
**Allowed Methods**: GET, OPTIONS  
**Allowed Headers**: Content-Type, Accept  
**Credentials**: Not allowed (no authentication)

---

## Versioning Strategy

**Current**: No versioning (v1 implicit in all endpoints)

**Future**: Add version prefix when breaking changes needed:
- `/api/v2/sections`
- Maintain v1 for backwards compatibility (6 months deprecation period)

---

## Performance SLAs

- **TTFB**: < 200ms (p95)
- **Response Time**: < 50ms (p95, cached requests)
- **Availability**: 99.9% uptime target (best effort, no formal SLA)

---

**Status**: ✅ API contracts complete - Ready for implementation
