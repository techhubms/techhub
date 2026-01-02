# API Contracts: Tech Hub REST API

**Updated**: 2025-02-28 (RESTful restructuring completed)  
**Base URL**: `http://localhost:5029` (local) | `https://api.tech.hub.ms` (production - future)  
**Format**: REST with JSON responses  
**Authentication**: None required for MVP (public API)

## Overview

This directory contains REST API endpoint specifications for the Tech Hub backend. The API uses a **nested RESTful structure** that mirrors the domain hierarchy: sections → collections → items.

**Key Changes** (2025-02-28):

- ✅ Implemented nested routes for better REST semantics
- ✅ Added advanced filtering endpoint for complex queries
- ✅ Consistent naming (both sections and collections use human-readable names)
- ❌ Removed flat structure endpoints (see Migration Notes below)

See [/dotnet/docs/api-specification.md](../../../dotnet/docs/api-specification.md) for complete API reference.

## Section Endpoints

### 1. Get All Sections

**GET** `/api/sections`

Returns all configured sections with their collections.

**Response**: `200 OK`

**Example**: <http://localhost:5029/api/sections>

**Tested**: ✅ Returns 8 sections

### 2. Get Section by Name

**GET** `/api/sections/{sectionName}`

Returns section metadata with collection references.

**Parameters**:

- `sectionName` (path) - Section identifier (e.g., "ai", "github-copilot", "ml")

**Response**: `200 OK` or `404 Not Found`

**Example**: <http://localhost:5029/api/sections/ai>

**Tested**: ✅ AI section with 4 collections

### 3. Get All Items in Section

**GET** `/api/sections/{sectionName}/items`

Returns all content items across all collections in a section.

**Parameters**:

- `sectionName` (path) - Section identifier

**Response**: `200 OK` or `404 Not Found`

**Example**: <http://localhost:5029/api/sections/ai/items>

**Tested**: ✅ Returns 1378 AI items

### 4. Get Collections in Section

**GET** `/api/sections/{sectionName}/collections`

Returns all collection references for a section.

**Parameters**:

- `sectionName` (path) - Section identifier

**Response**: `200 OK` or `404 Not Found`

**Example**: <http://localhost:5029/api/sections/github-copilot/collections>

**Tested**: ✅ Returns 4 collections

### 5. Get Specific Collection in Section

**GET** `/api/sections/{sectionName}/collections/{collectionName}`

Returns collection metadata within a section.

**Parameters**:

- `sectionName` (path) - Section identifier
- `collectionName` (path) - Collection name (news, blogs, videos, community, roundups)

**Response**: `200 OK` or `404 Not Found`

**Example**: <http://localhost:5029/api/sections/ai/collections/news>

**Tested**: ✅ Returns news collection details

### 6. Get Items in Collection within Section

**GET** `/api/sections/{sectionName}/collections/{collectionName}/items`

Returns all items in a specific collection within a section.

**Parameters**:

- `sectionName` (path) - Section identifier
- `collectionName` (path) - Collection name

**Response**: `200 OK` or `404 Not Found`

**Example**: <http://localhost:5029/api/sections/ml/collections/videos/items>

**Tested**: ✅ Returns 1 ML video item

## Content Filtering Endpoints

### 7. Advanced Content Filtering

**GET** `/api/content/filter`

Filter content by multiple criteria with AND logic.

**Query Parameters** (all optional):

- `sections` - Comma-separated section names (e.g., "ai,ml")
- `collections` - Comma-separated collection names (e.g., "news,blogs")
- `tags` - Comma-separated tags (content must have ALL tags - AND logic)
- `q` - Text search query (searches title, description, tags)

**Response**: `200 OK`

**Examples**:

Filter by sections: `GET /api/content/filter?sections=ai,ml` (Returns 1519+ items)

Combine section and collection: `GET /api/content/filter?sections=ai&collections=news` (Returns 528+ items)

Complex filter: `GET /api/content/filter?sections=ai,ml&collections=news,blogs&tags=copilot` (Returns 144+ items)

Search: `GET /api/content/filter?sections=github-copilot&q=vscode` (Returns 4+ items)

**Tested**: ✅ All filtering combinations working

### 8. Get All Tags

**GET** `/api/content/tags`

Returns all unique tags across all content.

**Response**: `200 OK`

**Example**: <http://localhost:5029/api/content/tags>

**Tested**: ✅ Returns 12,524 unique tags

## Error Responses

All error responses follow ASP.NET Core Problem Details format with HTTP status codes:

- `200 OK` - Success
- `404 Not Found` - Resource not found
- `400 Bad Request` - Invalid parameters (not yet implemented)
- `500 Internal Server Error` - Server error

## Migration Notes

### Removed Endpoints (Breaking Changes)

The following flat structure endpoints were removed on 2025-02-28:

- `GET /api/content` - Replaced by `/api/content/filter`
- `GET /api/content/collection/{collection}` - Replaced by nested routes
- `GET /api/content/category/{category}` - Replaced by `/api/sections/{section}/items`
- `GET /api/content/{collection}/{id}` - Future: `/api/items/{collection}/{id}`
- `GET /api/content/search?q={query}` - Replaced by `/api/content/filter?q={query}`

### Migration Guide

**Old → New**:

- `/api/content` → `/api/content/filter`
- `/api/content/collection/news` → `/api/content/filter?collections=news`
- `/api/content/category/ai` → `/api/sections/ai/items`
- `/api/content/search?q=copilot` → `/api/content/filter?q=copilot`

## Performance Characteristics

- **Sections**: ~25ms response time
- **Content (first load)**: ~5-9 seconds (2251+ markdown files)
- **Content (cached)**: < 100ms
- **Filtering**: Varies (typically < 2 seconds)

## Valid Values

### Sections

`all`, `github-copilot`, `ai`, `ml`, `devops`, `azure`, `coding`, `security`

### Collections

`news`, `videos`, `community`, `blogs`, `roundups`

## RESTful Design Principles

1. **Resource hierarchy**: `/sections/{section}/collections/{collection}/items`
2. **Consistent naming**: Both sections and collections use human-readable names
3. **HTTP methods**: GET for retrieval (POST/PUT/DELETE not yet implemented)
4. **Status codes**: 200 OK for success, 404 Not Found for missing resources
5. **Nested routes**: Resources organized hierarchically
6. **Advanced filtering**: Separate `/filter` endpoint for complex queries

## Future Enhancements

Planned but not yet implemented:

- Pagination support
- Sorting options
- OR logic for tags
- RSS feed endpoints
- Individual item endpoints
- Caching headers
- Rate limiting
- Authentication/Authorization
- Health check endpoint

## Status

✅ **Phase 3 API Implementation Complete** (14/14 endpoints tested and working)

**Test Results**: 14/14 tests passing (100% pass rate)

**Documentation**:

- [API Specification](../../../dotnet/docs/api-specification.md) - Complete endpoint reference
- [ADR: RESTful Structure](../../../dotnet/docs/decisions/restful-api-structure.md) - Design decision
- [Test Suite](../../../dotnet/.tmp/test-restful-api.ps1) - Comprehensive tests
