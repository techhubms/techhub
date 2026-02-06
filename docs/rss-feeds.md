# RSS Feed System

This document describes the RSS feed system that provides content syndication for Tech Hub.

## Overview

Tech Hub provides RSS 2.0 feeds for all content, allowing users to subscribe to updates via RSS readers. Feeds are served from the web frontend domain and are generated from the content repository with proper caching for performance.

## Available RSS Feeds

### Everything Feed

**URL**: `/all/feed.xml`

Returns all content from all sections, sorted by publication date (newest first), limited to 50 most recent items.

**Use Case**: Subscribe to all Tech Hub content in a single feed.

### Roundups Feed

**URL**: `/all/roundups/feed.xml`

Returns only weekly content roundups, sorted by publication date (newest first), limited to 50 most recent items.

**Use Case**: Subscribe to weekly curated content summaries.

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

- **Title**: Section or collection name (e.g., "Tech Hub - AI")
- **Description**: Section or collection description
- **Link**: URL to the section or collection page on Tech Hub
- **Language**: `en-us`
- **Last Build Date**: Timestamp when feed was generated

### Feed Items

Each item in the feed includes:

- **Title**: Content item title
- **Link**: URL to the full content (external source or internal detail page)
- **Description**: Content excerpt (up to 200 words)
- **Author**: Content author name
- **Publication Date**: Date content was published (Europe/Brussels timezone)
- **GUID**: Unique identifier for the item (based on slug)

### Content Rendering

- **Markdown to HTML**: Markdown content is converted to HTML using Markdig
- **Absolute URLs**: All relative URLs (images, links) are converted to absolute URLs using `https://tech.hub.ms` domain
- **Proper Escaping**: HTML content is properly escaped for XML

## Feed Behavior

### Sorting and Limiting

- Feeds are sorted by publication date, **newest first**
- Feeds are limited to the **50 most recent items** for performance
- Future-dated content (beyond current date in Europe/Brussels timezone) is excluded
- Draft content is excluded from all RSS feeds

### Content-Type

All feeds are served with:

```http
Content-Type: application/rss+xml; charset=utf-8
```

## Integration with Site

### UI Integration

- Section pages include an RSS icon link in the page header
- Footer includes "Subscribe via RSS" link to the everything feed (`/all/feed.xml`)
- RSS links navigate to the frontend feed URLs (e.g., `/{sectionName}/feed.xml`)

### Feed Architecture

- **Frontend Proxy**: RSS feeds are served from the web frontend at `https://tech.hub.ms/{sectionName}/feed.xml`
- **Backend API**: The frontend proxies requests to the secured API backend (`/api/rss/*` endpoints)
- **Same-Origin**: Feeds are served from the same domain as the website for security and simplicity

## RSS API Direct Endpoints

These are the backend RSS feed endpoints. For public-facing RSS feeds, use the Blazor Web proxy endpoints described below.

### GET /api/rss/all

Get RSS feed for all content across all sections.

**Response**: `200 OK` with `application/rss+xml; charset=utf-8`

### GET /api/rss/{sectionName}

Get RSS feed for a specific section.

**Parameters**:

- `sectionName` (path): Section identifier (e.g., `ai`, `github-copilot`)

**Response**: `200 OK` or `404 Not Found`

### GET /api/rss/{sectionName}/{collectionName}

Get RSS feed for a specific collection within a section (use `all` for sectionName to get a collection across all sections).

**Parameters**:

- `sectionName` (path): Section identifier or `all` for cross-section
- `collectionName` (path): Collection name (e.g., `roundups`, `news`, `blogs`)

**Response**: `200 OK` or `404 Not Found`

## RSS Proxy Endpoints (Blazor Web)

The Blazor Web application provides proxy endpoints that serve RSS feeds from the same domain as the website, avoiding cross-origin issues.

### GET /all/feed.xml

Get RSS feed for all content across all sections.
**Discovery Link**: Available on Home page

### GET /all/roundups/feed.xml

Get RSS feed specifically for weekly roundups collection.
**Discovery Link**: Roundups page or Footer

### GET /{sectionName}/feed.xml

Get RSS feed for a specific section (e.g., `/ai/feed.xml`).
**Discovery Link**: Available on Section pages

## Implementation & Testing

**For implementation details**, see:

- [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) - Frontend RSS proxy endpoints
- [src/TechHub.Api/AGENTS.md](../src/TechHub.Api/AGENTS.md) - Backend RSS API endpoints
- [src/TechHub.Infrastructure/AGENTS.md](../src/TechHub.Infrastructure/AGENTS.md) - RssService implementation

**Testing**: See [tests/TechHub.E2E.Tests/AGENTS.md](../tests/TechHub.E2E.Tests/AGENTS.md) for RSS feed tests.
