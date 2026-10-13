# TechHub.Infrastructure Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Infrastructure/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).

## Overview

Data access using PostgreSQL with generated tsvector full-text search, markdown processing, RSS generation, content processing (RSS ingestion, AI categorization), AI roundup generation, newsletter email, and other infrastructure concerns.

**Testing**: See [tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md) for testing patterns.

## Project Structure

```text
TechHub.Infrastructure/
├── Data/                    # Connection factory, SQL dialect, DapperExtensions (query logging), MigrationRunner
│   ├── Migrations/          # Embedded SQL migration scripts (postgres/)
│   ├── Resources/           # Embedded AI prompts (system-message.md, roundup-*-system.md) and newsletter HTML templates
│   └── Roundups/            # SQL scripts for roundup content (per section and all-roundups)
├── Repositories/            # Repository implementations (PostgreSQL + Dapper): content, write, jobs, processed URLs, reviews, feeds, roundups, GHC features, custom pages, newsletter subscribers, background job settings
├── Services/                # Markdown, RSS, ContentFixerService, DatabaseStatisticsService, AiCompletionClient, JsonEscapeSanitizer, ContentSyncService (test-only)
│   ├── ContentProcessing/   # Pipeline: feed ingestion, AI categorization, article enrichment
│   ├── RoundupGeneration/   # Weekly roundup AI generation pipeline
│   └── Newsletter/          # NewsletterService, ACS email sender, Handlebars NewsletterTemplateProvider
└── TechHub.Infrastructure.csproj
```

## Database Schema

**Main Tables**:

- `content_items` — Denormalized section boolean flags + bitmask for fast filtering. Full-text search uses the generated `search_vector` column (`GENERATED ALWAYS ... STORED`, no triggers) with the GIN index `idx_content_search`
- `content_tags_expanded` — Word-level tag matching (multi-word tags split into rows)
- `sync_metadata` — Sync hashes, only written by the test-only `ContentSyncService`

**Key Design**: Section filtering via bitmask (`WHERE sections_bitmask & @mask > 0`), tag filtering via subquery on `content_tags_expanded`, pagination via `LIMIT @take OFFSET @skip` (not keyset/cursor-based).

See [Data/Migrations/postgres/](Data/Migrations/postgres/) for complete schema and [docs/database.md](../../docs/database.md) for the functional overview.

## Repository Pattern

`ContentRepository` uses `ISqlDialect` abstraction for database-specific SQL (FTS matching, list filtering, boolean literals). Uses Dapper for lightweight ORM. Optional query logging via `DapperExtensions`.

## Markdown Processing

### Frontmatter Parsing

`FrontMatterParser` extracts YAML between `---` delimiters (`UnderscoredNamingConvention` maps `snake_case` to `PascalCase`). It is only reached through `MarkdownService.ParseFrontMatterAsync`, which serves the test-only file sync (`ContentSyncService`). Production content comes from the content processing pipeline, not from markdown files.

### Markdown to HTML

`MarkdownService` with Markdig pipeline: `UseAdvancedExtensions()`, `UseAutoIdentifiers()`, `UseEmojiAndSmiley()`, `UsePipeTables()`. Excerpt: content before `<!--excerpt_end-->` or first 200 words. Date handling: converts to Unix epoch using `Europe/Brussels` timezone.

## Service Lifetimes

Registrations live in [Api/Program.cs](../TechHub.Api/Program.cs) and the `AddRoundupGeneration()` / `AddAcsEmailClient()` extensions.

| Lifetime | Services |
|---|---|
| Singleton | `ISqlDialect`, `IDbConnectionFactory`, `NpgsqlDataSource`, `TimeProvider`, `StartupStateService`, background services (content processing, content fixer, roundup, newsletter, startup), `NewsletterTemplateProvider`, ACS `EmailClient` |
| Scoped | `IDbConnection` (per-request), repositories that reuse it (RssFeedConfig, CustomPageData, BackgroundJobSetting, ContentProcessingJob, ProcessedUrl, ContentReview, ContentItemWrite, SectionRoundup, NewsletterSubscriber), `ContentFixerService`, `ContentProcessingService`, `NewsletterService`, `AcsEmailSender`, roundup generation pipeline (`IRoundupGeneratorService` and helpers) |
| Transient | `IContentRepository`, `IGhcFeatureRepository`, `IMarkdownService`, `IRssService`, `IMigrationRunner`, feed ingestion / article content / YouTube tag / AI categorization services, `DatabaseStatisticsService`, typed HTTP clients (`AddHttpClient`) |

Scoped services are not injectable into singletons. Background services create their own scope per run.

## Per-Item Processing Loops

Processing pipelines (content ingestion, newsletter sending, roundup generation) iterate over collections and MUST NOT abort the entire run when one item fails. Use the broad-catch pattern with `#pragma warning disable CA1031`:

```csharp
#pragma warning disable CA1031 // Best-effort: continue with other items if one fails
catch (Exception ex) when (ex is not OperationCanceledException)
{
    errorCount++;
    _logger.LogWarning(ex, "Failed to process {Item} — skipping", item);
}
#pragma warning restore CA1031
```

See [src/AGENTS.md](../AGENTS.md#accepted-broad-catch-pattern) for the full rationale.

## Tag Cloud

There is no `TagCloudService`. `ContentRepository.GetTagCountsAsync` counts tags (filtering out section/collection title tags BEFORE counting), and `ContentEndpoints.ApplyQuantileSizing` in the API assigns sizes. Quantile sizing: top 25% Large, middle 50% Medium, bottom 25% Small. Size group normalization: 1 group → all Medium, 2 → Medium + Small.
