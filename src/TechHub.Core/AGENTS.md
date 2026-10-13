# TechHub.Core Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Core/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).

## Overview

Domain models and interfaces with **zero external dependencies** (pure .NET). Defines core business entities and contracts.

**Testing**: See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md) for unit testing patterns.

## Project Structure

```text
TechHub.Core/
├── Models/                       # Domain models organized by feature (plus root-level models such as GhcFeature, NewsletterSubscriber, PagedResult)
│   ├── Admin/                   # Admin DTOs: job settings, content reviews, edit/list items, statistics, newsletter send log
│   ├── ContentProcessing/       # Pipeline models: FeedConfig, RawFeedItem, ProcessedContentItem, ProcessedUrl, roundup models
│   ├── Core/                    # Section, Collection, ContentItem, ContentItemDetail, SectionCache, AuthorSummary, SitemapItem
│   ├── Facets/                  # FacetRequest, FacetResults, FacetValue
│   ├── PageData/                # Custom page data models (DXSpace, Features, GenAI, etc.)
│   ├── Rss/                     # RssChannel, RssItem
│   ├── Search/                  # SearchRequest, SearchResults
│   └── Tags/                    # AllTagsResponse, TagCloudItem, TagCountsRequest, TagWithCount
├── Configuration/                # Options classes (AppSettings, ContentProcessorOptions, NewsletterOptions, ...), TagNormalizer, TagExclusions
├── Interfaces/                   # Repository, service, client and dialect contracts (IContentRepository, ISqlDialect, IMarkdownService, ...)
├── Logging/                      # InputSanitizer (.Sanitize()), FileLoggerProvider
├── Security/                     # ProbeDetector
├── Validation/                   # RouteParameterValidator
└── TechHub.Core.csproj          # Project file (zero dependencies!)
```

## Core Principles

### No External Dependencies

**CRITICAL**:

- ✅ `System.*` namespaces, `Microsoft.Extensions.*.Abstractions`, standard .NET types, collections
- 🚫 **NEVER** add NuGet packages, ASP.NET Core, Entity Framework, or other framework references

### Immutability

Use `init` accessors and `required` properties. Use `IReadOnlyList<T>` for collections. Use `record` types for value objects.

### Unified Model Pattern (No Separate DTOs)

One model hierarchy serves list and detail views:

- **`ContentItem`** ([Models/Core/ContentItem.cs](Models/Core/ContentItem.cs)): list-view fields only. It has no `Content` or `RenderedHtml`.
- **`ContentItemDetail`** ([Models/Core/ContentItemDetail.cs](Models/Core/ContentItemDetail.cs)) extends it with `RenderedHtml` (nullable, set via `SetRenderedHtml`, does not throw) and the JSON-ignored raw `Content`. `RenderedHtml` is null for externally-linking collections (news, blogs, community).
- Custom page data models live in `Models/PageData/`.

### Repository Contract

All `IContentRepository` methods **MUST** return content sorted by `DateEpoch` descending (newest first). This sorting happens at the repository layer, before caching. Clients should never sort themselves.

All methods async with `CancellationToken ct = default`. Return `IReadOnlyList<T>` for collections, `T?` for single items.

## Frontmatter Mapping

📖 See [docs/content-schema.md](../../docs/content-schema.md#domain-model-mapping) for complete field definitions.

Key mappings: `date` → `DateEpoch` (Unix timestamp, Europe/Brussels), `section_names` → `Sections` (array), `tags` → `Tags` (casing normalized by `TagNormalizer` via a mapping, e.g. `vscode` → `VS Code`, not lowercased), filename → `Slug`.

## Collection Custom Page Ordering

Custom pages (`IsCustom = true`) support ordering via `Order` property: lower values first, then alphabetically by `Title`. Applied in `SectionCard.razor` and `SubNav.razor`.

## URL Generation

- `ContentItem.GetHref(sectionOverride)`: contextual link. Items in externally-linking collections (news, blogs, community) return their safe HTTP(S) `ExternalUrl`, all others return `/{section}/{collection}/{slug}` (section defaults to `PrimarySectionName`).
- `ContentItem.GetCanonicalHref(sectionOverride)`: always the internal `/{section}/{collection}/{slug}`. Use it for share links and MCP output.
