# TechHub.Infrastructure Development Guide

> **AI CONTEXT**: This is a **LEAF** context file for `src/TechHub.Infrastructure/`. It complements [src/AGENTS.md](../AGENTS.md) and [Root AGENTS.md](../../AGENTS.md).
> **RULE**: All rules from parent AGENTS.md files apply. This file adds Infrastructure layer-specific patterns.

## Overview

This project implements data access using PostgreSQL database with tsvector full-text search, markdown processing, content synchronization from files to database, and other infrastructure concerns.

**When to read this file**: When implementing repositories, working with database queries, content sync, or understanding data access patterns.

**Testing this code**: See [tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md) for unit testing patterns.

## Project Structure

```text
TechHub.Infrastructure/
├── Data/                                      # Database infrastructure
│   ├── DbConnectionFactory.cs                 # PostgreSQL connection factory
│   ├── MigrationRunner.cs                     # Database schema migrations
│   ├── PostgresDialect.cs                     # PostgreSQL-specific SQL syntax
│   └── Migrations/                            # SQL migration scripts
│       └── postgres/                          # PostgreSQL migrations
├── Repositories/                              # Repository implementations
│   ├── ContentRepositoryBase.cs               # Abstract base for content repos
│   └── ContentRepository.cs                   # Database repository (PostgreSQL)
├── Services/                                  # Infrastructure services
│   ├── ContentSyncService.cs                  # Sync markdown files to database
│   ├── FrontMatterParser.cs                   # YAML frontmatter parsing
│   ├── MarkdownService.cs                     # Markdown to HTML conversion
│   └── RssService.cs                          # RSS feed generation
└── TechHub.Infrastructure.csproj              # Project file
```

## Database Architecture

### Provider Configuration

Database provider is configured in `appsettings.json`:

```json
{
  "Database": {
    "Provider": "PostgreSQL",
    "ConnectionString": "Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev"
  }
}
```

**Supported Provider**: PostgreSQL

### Schema Overview

**Main Tables**:

- `content_items` - All content with denormalized section flags and bitmask
- `content_tags_expanded` - Word-level tag matching (denormalized for fast filtering)
- `sync_metadata` - Tracks last sync time and content hashes
- `content_fts` - Full-text search using tsvector/tsquery

**Key Design Decisions**:

1. **Denormalized section flags**: Each content item has `is_ai`, `is_azure`, `is_github_copilot`, etc. boolean columns for fast filtering without JOINs
2. **Section bitmask**: Single integer column for multi-section filtering with bitwise operations
3. **Expanded tags table**: Multi-word tags split into separate rows for word-level matching
4. **tsvector full-text index**: Full-text index synced via triggers

**See**: [Data/Migrations/postgres/](Data/Migrations/postgres/) for complete schema

## Repository Patterns

### Database Content Repository

**Key Pattern**: Repository using `ISqlDialect` for PostgreSQL-specific SQL → Query with Dapper → Map to domain models → Use tsvector full-text search.

**Implementation**: `ContentRepository` uses `ISqlDialect` abstraction for database-specific SQL.

**Important Details**:

- **Dialect Abstraction**: `ISqlDialect` interface provides database-specific SQL fragments:
  - `GetFullTextWhereClause()` - FTS matching syntax
  - `GetFullTextOrderByClause()` - Relevance ranking
  - `GetListFilterClause()` - List filtering (`= ANY(@param)` for PostgreSQL)
  - `ConvertListParameter<T>()` - Convert lists to proper parameter type (Array for PostgreSQL)
  - `GetBooleanLiteral()` - Boolean literals (true/false for PostgreSQL)
- Uses Dapper for lightweight ORM
- tsvector for PostgreSQL full-text search
- Section filtering via bitmask: `WHERE sections_bitmask & @mask > 0`
- Tag filtering via subquery on `content_tags_expanded` table
- Pagination via keyset (cursor-based) for performance
- Optional query logging via `DapperExtensions` when `EnableQueryLogging` is enabled

**Query Optimization**:

```sql
-- Section filtering with bitmask (very fast)
WHERE sections_bitmask & 17 > 0  -- AI (1) + GitHub Copilot (16)

-- Tag filtering with pre-filter (reduces FTS search scope)
WHERE (collection_name, slug) IN (
    SELECT collection_name, slug FROM content_tags_expanded
    WHERE tag_word IN ('copilot', 'ai')
)
```

## Content Synchronization

### ContentSyncService

**Key Pattern**: Hash-based change detection → Incremental sync → Transaction safety.

**Implementation**: `ContentSyncService`

**Sync Process**:

1. Scan markdown files in `collections/` directory
2. Compute SHA-256 hash of each file
3. Compare with stored hashes in `sync_metadata`
4. INSERT new items, UPDATE changed items, DELETE removed items
5. Rebuild FTS index after bulk operations

**Performance Optimizations**:

- Bulk operations in single transaction
- Index/trigger disable during sync for speed
- FTS rebuild at end instead of per-row triggers

**Important Details**:

- Runs at API startup (see Program.cs)
- Entire sync is transactional (rollback on error)
- Logs detailed metrics for performance monitoring

**See**: [Services/ContentSyncService.cs](Services/ContentSyncService.cs) for implementation

## Markdown Processing

### Frontmatter Parsing

**Key Pattern**: Extract YAML between `---` delimiters → Parse with YamlDotNet → Return metadata + content.

**Implementation**: `FrontMatterParser`

**Important Details**:

- Uses `UnderscoredNamingConvention` to map `snake_case` YAML to `PascalCase` C# properties
- Returns tuple of (Dictionary, string) for flexibility
- See [src/TechHub.Core/AGENTS.md](../TechHub.Core/AGENTS.md) for field mappings

### Markdown to HTML Conversion

**Key Pattern**: Parse markdown → Apply extensions → Generate HTML with heading IDs.

**Implementation**: `MarkdownService`

**Important Details**:

- **Markdig pipeline**: Uses `UseAdvancedExtensions()`, `UseAutoIdentifiers()`, `UseEmojiAndSmiley()`, `UsePipeTables()`
- **Excerpt extraction**: Content before `<!--excerpt_end-->` marker, or first 200 words if no marker
- **Date handling**: Converts dates to Unix epoch using configured timezone (`Europe/Brussels`)

## Service Registration

**Registration in Program.cs**:

```csharp
// Database infrastructure
builder.Services.AddSingleton<ISqlDialect, PostgresDialect>();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));
builder.Services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());

// Repositories
builder.Services.AddTransient<IContentRepository, ContentRepository>();
builder.Services.AddSingleton<ISectionRepository, ConfigurationBasedSectionRepository>();

// Services
builder.Services.AddTransient<IMarkdownService, MarkdownService>();
builder.Services.AddTransient<IRssService, RssService>();
builder.Services.AddTransient<ITagCloudService, TagCloudService>();
builder.Services.AddTransient<IContentSyncService, ContentSyncService>();
builder.Services.AddTransient<MigrationRunner>();
```

**Service Lifetimes**:

- **Singleton**: `ISqlDialect`, `IDbConnectionFactory`, `ISectionRepository` (stateless or config-based)
- **Scoped**: `IDbConnection` (per-request database connection)
- **Transient**: All other services (lightweight, stateless)

## Tag Services

### Tag Cloud Service

**Key Pattern**: Query tags from repository (with section/collection title exclusion) → Count occurrences → Apply quantile sizing.

**Implementation**: `TagCloudService`

**Section/Collection Title Exclusion**: Repository layer (`ContentRepositoryBase`) filters out section and collection titles from tags BEFORE counting. This ensures tag clouds don't show "AI", "GitHub Copilot", etc. as tags when they are already shown as section/collection filters.

**Quantile Size Algorithm**:

- **Top 25%**: `TagSize.Large`
- **Middle 50%**: `TagSize.Medium`
- **Bottom 25%**: `TagSize.Small`

**Size Group Normalization**: After quantile assignment, the number of distinct size groups is normalized: 1 group → all Medium, 2 groups → Medium + Small, 3 groups → unchanged. This prevents misleading emphasis when all tags have similar counts.

### Tag Filtering

**Database**: Tag filtering is done directly in SQL queries via the `content_tags_expanded` table:

```sql
-- Tags are expanded to words during sync: "GitHub Copilot" → "github", "copilot"
SELECT collection_name, slug FROM content_tags_expanded
WHERE tag_word IN @tags
```

**File-based**: Uses simple substring matching:

```csharp
item.Tags.Any(t => t.Contains(normalizedTag, StringComparison.OrdinalIgnoreCase))
```

## Error Handling

**Always log errors and handle gracefully**:

- Wrap database operations in try-catch blocks
- Use transactions for multi-step operations
- Log with context (file path, error details)
- One bad file should not crash the entire system

## Testing

**See [tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md)** for comprehensive testing patterns including:

- Repository testing with PostgreSQL Testcontainers
- ContentSyncService integration tests
- Markdown parsing test cases
- Error handling scenarios

## Related Documentation

### Functional Documentation (docs/)

- **[Database](../../docs/database.md)** - Database configuration and sync settings
- **[Filtering](../../docs/filtering.md)** - Tag filtering and tag cloud algorithm
- **[Content Processing](../../docs/content-processing.md)** - Content sync workflow

### Implementation Guides (AGENTS.md)

- **[src/AGENTS.md](../AGENTS.md)** - Shared .NET patterns and code quality standards
- **[src/TechHub.Core/AGENTS.md](../TechHub.Core/AGENTS.md)** - Domain models and interfaces
- **[src/TechHub.Api/AGENTS.md](../TechHub.Api/AGENTS.md)** - API endpoints that use these repositories
- **[tests/TechHub.Infrastructure.Tests/AGENTS.md](../../tests/TechHub.Infrastructure.Tests/AGENTS.md)** - Infrastructure testing patterns
- **[Root AGENTS.md](../../AGENTS.md)** - Complete workflow and principles

---

**Remember**: Infrastructure layer handles the "how" - database access, file I/O, external services. Keep it separate from domain logic.
