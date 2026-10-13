# TechHub.Api Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Api/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).

## Overview

REST API backend using ASP.NET Core Minimal APIs. Exposes endpoints for sections, collections, content and filtering, authors, RSS feeds, sitemap, custom page data, GitHub Copilot features, VS Code updates, legacy slug redirects, newsletter, and the admin API (`/api/admin/*`). Also hosts the background services (content processing, content fixer, roundup generation, newsletter, startup).

## Background Services

Background services inherit from `BackgroundService` and are registered in `Program.cs` via `AddHostedService<T>()`. The outer execution loop MUST catch all non-cancellation exceptions to prevent the entire application from crashing:

```csharp
catch (Exception ex) when (ex is not OperationCanceledException)
{
    _logger.LogError(ex, "Unhandled exception in MyBackgroundService — service will continue");
}
```

See [src/AGENTS.md](../AGENTS.md#accepted-broad-catch-pattern) for the accepted broad-catch pattern and rationale.

## Project Structure

```text
TechHub.Api/
├── Program.cs                    # Entry point, DI registration, middleware pipeline
├── Authorization/                # AdminAuthorization (the AdminOnly policy; fails closed in Production without Azure AD)
├── Endpoints/                    # Minimal API endpoints (content, RSS, authors, admin, custom pages, sitemap, GHC features, newsletter)
├── HealthChecks/                 # StartupHealthCheck, DatabaseHealthCheck
├── Middleware/                   # ExceptionHandlerMiddleware
├── Services/                     # Background services (content processing, content fixer, roundup, newsletter, startup) and StartupStateService
├── appsettings.json              # Configuration (sections, collections, paths)
└── appsettings.{Environment}.json # Per-environment overrides (Development, IntegrationTest, Staging, Production)
```

## Input Sanitization

**RULE**: Sanitize user-controlled string parameters at each endpoint handler entry using `.Sanitize()` (from `TechHub.Core.Logging`). Overwrite the parameter (not a new variable) so there is zero risk of using the unsanitized value. Validate query parameters (search length, tag count/length, lastDays range) before processing — return `400 Bad Request` for invalid input.

See [docs/input-validation-and-sanitization.md](../../docs/input-validation-and-sanitization.md) for the full 6-layer strategy.

## Minimal API Patterns

**Key Pattern**: Static extension methods on `IEndpointRouteBuilder` (`MapXxxEndpoints`) with endpoint groups, one file per area in `Endpoints/`. Register on the root group in Program.cs, which applies `WithRejectedRequestLogging()`: `api.MapContentEndpoints();`

**RESTful Design**: Resource hierarchy (`/sections/{sectionName}/collections/{collectionName}/items`), human-readable slugs, GET for retrieval.

**Status codes for missing resources**: Match the sibling endpoints. Single-resource lookups (section, collection, content detail by slug, author items, legacy redirect) return `204 No Content`. Collection items and tag cloud, RSS feeds, custom page data and GHC feature mutations return `404 Not Found`. Invalid input returns `400`.

**Always add OpenAPI metadata**: `WithName()`, `WithSummary()`, `WithDescription()`, `WithTags()`, `Produces<T>()`. OpenAPI is provided by `AddOpenApi()` plus Swashbuckle (`AddSwaggerGen`), so there is no `WithOpenApi()`.

**Always include `CancellationToken` parameter** in endpoint handlers.

**Always handle exceptions and log them** with full context (exception, parameters, operation).

## RSS Endpoints (Internal API)

| Endpoint                                   | Description                       |
| ------------------------------------------ | --------------------------------- |
| `GET /api/rss/all`                         | All content across all sections   |
| `GET /api/rss/{sectionName}`               | Content for a specific section    |
| `GET /api/rss/{sectionName}/{collectionName}` | Content for a specific collection |

All return `application/rss+xml; charset=utf-8`. These are internal — user-facing feeds via [Web frontend proxies](../TechHub.Web/AGENTS.md).

See [docs/rss-feeds.md](../../docs/rss-feeds.md) for functional documentation.
