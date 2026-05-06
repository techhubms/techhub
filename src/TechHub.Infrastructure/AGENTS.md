# TechHub.Infrastructure Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Infrastructure/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).

## Overview

Data access using PostgreSQL with tsvector full-text search, markdown processing, content sync, and other infrastructure concerns.

**Testing**: See [tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md) for testing patterns.

## Project Structure

```text
TechHub.Infrastructure/
├── Data/                    # Connection factory, SQL dialect, migrations runner, SQL migration scripts
├── Repositories/            # Content repository implementations (PostgreSQL + Dapper)
├── Services/                # Markdown, RSS, content sync, AI processing, roundup generation
│   ├── ContentProcessing/   # Pipeline: feed ingestion, AI categorization, article enrichment
│   └── RoundupGeneration/   # Weekly roundup AI generation pipeline
└── TechHub.Infrastructure.csproj
```

## Database Schema

**Main Tables**:

- `content_items` — Denormalized section boolean flags + bitmask for fast filtering
- `content_tags_expanded` — Word-level tag matching (multi-word tags split into rows)
- `sync_metadata` — Last sync time and content hashes
- `content_fts` — tsvector full-text search index (synced via triggers)

**Key Design**: Section filtering via bitmask (`WHERE sections_bitmask & @mask > 0`), tag filtering via subquery on `content_tags_expanded`, pagination via keyset (cursor-based).

See [Data/Migrations/postgres/](Data/Migrations/postgres/) for complete schema.

## Repository Pattern

`ContentRepository` uses `ISqlDialect` abstraction for database-specific SQL (FTS matching, list filtering, boolean literals). Uses Dapper for lightweight ORM. Optional query logging via `DapperExtensions`.

## Markdown Processing

### Frontmatter Parsing

`FrontMatterParser` extracts YAML between `---` delimiters. Uses `UnderscoredNamingConvention` to map `snake_case` YAML to `PascalCase` C#.

### Markdown to HTML

`MarkdownService` with Markdig pipeline: `UseAdvancedExtensions()`, `UseAutoIdentifiers()`, `UseEmojiAndSmiley()`, `UsePipeTables()`. Excerpt: content before `<!--excerpt_end-->` or first 200 words. Date handling: converts to Unix epoch using `Europe/Brussels` timezone.

## Service Lifetimes

| Lifetime | Services |
|---|---|
| Singleton | `ISqlDialect`, `IDbConnectionFactory` |
| Scoped | `IDbConnection` (per-request) |
| Transient | All other services |

## Tag Cloud

`TagCloudService` queries tags with section/collection title exclusion (repository filters these BEFORE counting). Quantile sizing: top 25% Large, middle 50% Medium, bottom 25% Small. Size group normalization: 1 group → all Medium, 2 → Medium + Small.
