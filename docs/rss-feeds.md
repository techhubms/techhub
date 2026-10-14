# RSS Feed System

This document describes the RSS feed system that provides content syndication for Tech Hub.

## Overview

Tech Hub provides RSS 2.0 feeds for all content, allowing users to subscribe to updates via RSS readers. Feeds are served from the web frontend domain and are generated from the content repository with proper caching for performance.

## Available RSS Feeds

### Everything Feed

**URL**: `/all/feed.xml`

Returns all content from all sections, sorted by publication date (newest first), limited to 50 most recent items.

**Use Case**: Subscribe to all Tech Hub content in a single feed.

### Section Roundups Feed

**URL**: `/{sectionName}/roundups/feed.xml`

Returns weekly roundups for a specific section, sorted by publication date (newest first), limited to 50 most recent items.

**Use Case**: Subscribe to weekly curated summaries for one section.

### Section Feeds

Section feeds contain all content tagged with that specific section, regardless of collection type (news, blogs, videos, community, roundups).

| Section         | URL                              | Description                           |
| --------------- | -------------------------------- | ------------------------------------- |
| AI              | `/ai/feed.xml`                   | Artificial Intelligence content       |
| GitHub Copilot  | `/github-copilot/feed.xml`       | GitHub Copilot content                |
| ML              | `/ml/feed.xml`                   | Machine Learning content              |
| Azure           | `/azure/feed.xml`                | Azure cloud platform content          |
| .NET            | `/dotnet/feed.xml`               | .NET development content              |
| DevOps          | `/devops/feed.xml`               | DevOps and automation content         |
| Security        | `/security/feed.xml`             | Security content                      |

**Use Case**: Subscribe to a specific topic area (e.g., only AI-related content).

## Feed Format

All feeds are generated in **RSS 2.0** format with the following characteristics:

### Feed Metadata

Each feed includes:

- **Title**: `Tech Hub - {section title}` (e.g., "Tech Hub - AI", "Tech Hub - Everything" for the everything feed)
- **Description**: Section or collection description
- **Link**: URL to the section or collection page on Tech Hub
- **Language**: `en-us`
- **Last Build Date**: Publication date of the newest item in the feed (the current time when the feed is empty)
- **Self link**: An `atom:link rel="self"` element with the canonical feed URL

### Feed Items

Each item in the feed includes:

- **Title**: Content item title
- **Link**: URL to the full content. External source for `news`, `blogs` and `community`, otherwise the absolute Tech Hub page URL
- **Description**: The plain-text excerpt of the item (no rendered body)
- **Creator**: Author name in a `dc:creator` element, because RSS `author` requires an email address
- **Publication Date**: Publication date in RFC 1123 format, expressed in GMT
- **GUID**: The item link, marked `isPermaLink="true"`
- **Categories**: The item's tags, one `category` element each

## Feed Behavior

### Sorting and Limiting

- Feeds are sorted by publication date, **newest first**
- Feeds are limited to the **50 most recent items** for performance (`AppSettings:Rss:MaxItemsInFeed`)
- The feed itself does not filter by date. Items with a future publication date never appear because the content pipeline caps them to the processing date at ingestion (see [content-processing.md](content-processing.md#content-processor-configuration))

### Content-Type

All feeds are served with:

```http
Content-Type: application/rss+xml; charset=utf-8
```

## Integration with Site

### UI Integration

- Section and home pages render an RSS link in the banner (mobile) and an RSS button in the sub-navigation
- The sidebar can list RSS links (`SidebarRssLinks`)
- Pages include a `<link rel="alternate" type="application/rss+xml">` tag in the head for feed autodiscovery
- RSS links navigate to the frontend feed URLs (e.g., `/{sectionName}/feed.xml`)

### Legacy Feed URLs

The URL normalization middleware redirects old feed URLs with a `301`: `/feed.xml` goes to `/all/feed.xml` and `/{sectionName}.xml` goes to `/{sectionName}/feed.xml` (known sections only). See [url-routing.md](url-routing.md#url-normalization).

### Feed Architecture

- **Frontend Proxy**: RSS feeds are served from the web frontend at `https://tech.hub.ms/{sectionName}/feed.xml`
- **Backend API**: The frontend proxies requests to the secured API backend (`/api/rss/*` endpoints)
- **Same-Origin**: Feeds are served from the same domain as the website for security and simplicity
- **Rate limiting**: The proxy endpoints use the `web-rss` policy, 20 requests per minute per IP (see [url-routing.md](url-routing.md#rate-limiting))

## RSS API Direct Endpoints

These are the backend RSS feed endpoints. For public-facing RSS feeds, use the Blazor Web proxy endpoints described below.

### GET /api/rss/all

Get RSS feed for all content across all sections.

**Response**: `200 OK` with `application/rss+xml; charset=utf-8`

### GET /api/rss/{sectionName}

Get RSS feed for a specific section.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`)

**Response**: `200 OK`, `400 Bad Request` (invalid `sectionName` format), or `404 Not Found`

### GET /api/rss/{sectionName}/{collectionName}

Get RSS feed for a specific collection within a section (use `all` for sectionName to get a collection across all sections).

**Parameters**:

- `sectionName` (path): Section identifier or `all` for cross-section
- `collectionName` (path): Collection name (e.g., `roundups`, `news`, `blogs`)

**Response**: `200 OK`, `400 Bad Request` (invalid name format), or `404 Not Found`

## RSS Proxy Endpoints (Blazor Web)

The Blazor Web application provides proxy endpoints that serve RSS feeds from the same domain as the website, avoiding cross-origin issues.

### GET /all/feed.xml

Get RSS feed for all content across all sections.
**Discovery Link**: Available on Home page

### GET /{sectionName}/roundups/feed.xml

Get RSS feed specifically for weekly roundups in a section (e.g. `/ai/roundups/feed.xml`).
**Discovery Link**: Section roundup pages

### GET /{sectionName}/feed.xml

Get RSS feed for a specific section (e.g., `/ai/feed.xml`).
**Discovery Link**: Available on Section pages

**Unknown sections**: both section-based proxy endpoints return `400 Bad Request` for an invalid section name format and `404 Not Found` when `{sectionName}` is not a known section. When the in-memory section cache is ready, the Web app answers 404 directly without calling the API. When the cache is not ready yet, the request is forwarded to the API and a downstream `404` is translated to `404 Not Found` instead of surfacing as a `500`. Crawlers regularly append `/feed.xml` to arbitrary article slugs; answering 404 here keeps those requests from becoming 500s and failed API calls that trip the failed-requests alert.

## Implementation & Testing

**For implementation details**, see:

- [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) - Frontend RSS proxy endpoints
- [src/TechHub.Api/AGENTS.md](../src/TechHub.Api/AGENTS.md) - Backend RSS API endpoints
- [src/TechHub.Infrastructure/AGENTS.md](../src/TechHub.Infrastructure/AGENTS.md) - RssService implementation

**Testing**: See [tests/TechHub.E2E.Tests/AGENTS.md](../tests/TechHub.E2E.Tests/AGENTS.md) for RSS feed tests.
