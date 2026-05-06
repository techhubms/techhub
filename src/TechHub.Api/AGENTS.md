# TechHub.Api Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Api/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).

## Overview

REST API backend using ASP.NET Core Minimal APIs. Exposes endpoints for sections, content, filtering, RSS feeds, and structured data.

**Testing**: See [tests/TechHub.Api.Tests/AGENTS.md](../../tests/TechHub.Api.Tests/AGENTS.md) for integration testing patterns.

## Project Structure

```text
TechHub.Api/
├── Program.cs                    # Entry point, DI registration, middleware pipeline
├── Endpoints/                    # Minimal API endpoint definitions (content, RSS, authors, admin, custom pages, sitemap)
├── appsettings.json             # Configuration (sections, collections, paths)
└── appsettings.Development.json # Development-specific settings
```

## Input Sanitization

**RULE**: Sanitize user-controlled string parameters at each endpoint handler entry using `.Sanitize()` (from `TechHub.Core.Logging`). Overwrite the parameter (not a new variable) so there is zero risk of using the unsanitized value. Validate query parameters (search length, tag count/length, lastDays range) before processing — return `400 Bad Request` for invalid input.

See [docs/input-validation-and-sanitization.md](../../docs/input-validation-and-sanitization.md) for the full 6-layer strategy.

## Minimal API Patterns

**Key Pattern**: Static extension methods on `WebApplication` with endpoint groups. Separate files in `Endpoints/`. Register in Program.cs: `app.MapContentEndpoints();`

**RESTful Design**: Resource hierarchy (`/sections/{sectionName}/collections/{collectionName}/items`), human-readable slugs, GET for retrieval, 200/404 status codes.

**Always add OpenAPI metadata**: `WithName()`, `WithSummary()`, `WithDescription()`, `WithTags()`, `WithOpenApi()`, `Produces<T>()`.

**Always include `CancellationToken` parameter** in endpoint handlers.

**Always handle exceptions and log them** with full context (exception, parameters, operation).

## RSS Endpoints (Internal API)

| Endpoint                                   | Description                       |
| ------------------------------------------ | --------------------------------- |
| `GET /api/rss/all`                         | All content across all sections   |
| `GET /api/rss/{sectionName}`               | Content for a specific section    |
| `GET /api/rss/{sectionName}/{collectionName}` | Content for a specific collection |

All return `application/xml; charset=utf-8`. These are internal — user-facing feeds via [Web frontend proxies](../TechHub.Web/AGENTS.md).

See [docs/rss-feeds.md](../../docs/rss-feeds.md) for functional documentation.
