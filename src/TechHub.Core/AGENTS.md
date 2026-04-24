# TechHub.Core Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Core/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).

## Overview

Domain models and interfaces with **zero external dependencies** (pure .NET). Defines core business entities and contracts.

**Testing**: See [tests/TechHub.Core.Tests/AGENTS.md](../../tests/TechHub.Core.Tests/AGENTS.md) for unit testing patterns.

## Project Structure

```text
TechHub.Core/
├── Models/                       # Domain models organized by feature
│   ├── Core/                    # Section, ContentItem, Collection
│   ├── Facets/                  # FacetRequest, FacetResults, FacetValue
│   ├── Filter/                  # FilterRequest, FilterResponse, FilterSummary
│   ├── PageData/                # Custom page data models (DXSpace, Features, GenAI, etc.)
│   ├── Rss/                     # RssChannel, RssItem
│   ├── Search/                  # SearchRequest, SearchResults
│   └── Tags/                    # AllTagsResponse, TagCloudItem, TagWithCount
├── Interfaces/                   # IContentRepository, IDbConnectionFactory, IMarkdownService, IRssService, ISqlDialect
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

Single model serves both list and detail views. **ContentItem** ([Models/Core/ContentItem.cs](Models/Core/ContentItem.cs)):

- List views: `RenderedHtml` is null
- Detail views: `RenderedHtml` getter throws if accessed when null (fail-fast)
- Same pattern for custom page data models in `Models/PageData/`

### Repository Contract

All `IContentRepository` methods **MUST** return content sorted by `DateEpoch` descending (newest first). This sorting happens at the repository layer, before caching. Clients should never sort themselves.

All methods async with `CancellationToken ct = default`. Return `IReadOnlyList<T>` for collections, `T?` for single items.

## Frontmatter Mapping

📖 See [docs/content-schema.md](../../docs/content-schema.md#metadata-to-domain-model-mapping) for complete field definitions.

Key mappings: `date` → `DateEpoch` (Unix timestamp, Europe/Brussels), `section_names` → `SectionNames` (array), `tags` → `Tags` (normalized lowercase), filename → `Slug`.

## Collection Custom Page Ordering

Custom pages (`IsCustom = true`) support ordering via `Order` property: lower values first, then alphabetically by `Title`. Applied in `SectionCard.razor` and `SubNav.razor`.

## URL Generation

`ContentItem.GetUrlInSection(sectionName)` → `/{sectionName}/{collectionName}/{slug}`. Validates section name exists in item's `SectionNames`.
