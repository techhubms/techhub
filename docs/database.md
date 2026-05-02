# Database Configuration

Tech Hub uses PostgreSQL as its exclusive content storage backend, configured via `appsettings.json`.

**Related Documentation**:

- [Content Processing](content-processing.md) - How content syncs to the database
- [Filtering](filtering.md) - Tag filtering that leverages database indexes
- [Running & Testing](running-and-testing.md) - How to run during development
- [Query Logging](query-logging.md) - Debug and performance monitoring with query logging

## Provider

> **Default provider**: Tech Hub.Api defaults to **PostgreSQL** with connection string `Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev`.

### PostgreSQL

Uses PostgreSQL with tsvector full-text search and GIN indexes.

**Configuration**:

```json
{
  "Database": {
    "Provider": "PostgreSQL",
    "ConnectionString": "Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev"
  }
}
```

**Pros**: Production-ready, best performance at scale, full-text search with tsvector, semantic search ready  
**Cons**: Requires Docker (`docker compose up -d postgres`) or cloud PostgreSQL instance

**Usage**:

- See [running-and-testing.md](running-and-testing.md) for instructions on running with Docker.

## Database Schema

### Main Tables

| Table | Purpose |
|-------|--------|
| `content_items` | All content with denormalized section flags and bitmask |
| `content_tags_expanded` | Word-level tag matching (denormalized for fast filtering) |
| `sync_metadata` | Tracks last sync time and content hashes |
| `content_fts` | Full-text search using tsvector/tsquery |
| `rss_feed_configs` | RSS feed configuration (URL, name, collection, enabled flag) |
| `processed_urls` | Tracks processed/skipped/failed URLs for the content pipeline |
| `content_processing_jobs` | Audit log of content processing runs |
| `custom_page_data` | JSON data for custom pages (editable from admin UI) |

### Design Decisions

1. **Denormalized section flags**: Each content item has `is_ai`, `is_azure`, `is_github_copilot`, etc. boolean columns for fast filtering without JOINs
2. **Section bitmask**: Single integer column for multi-section filtering with bitwise operations
3. **Expanded tags table**: Multi-word tags split into separate rows for word-level matching
4. **tsvector full-text index**: Full-text index synced via triggers
5. **tsvector prefix indexes**: Prefix matching support enables fast prefix matching for partial word searches (e.g., "cop" matches "copilot")

### Query Patterns

**Section filtering with bitmask** (very fast):

```sql
WHERE sections_bitmask & 17 > 0  -- AI (1) + GitHub Copilot (16)
```

**Tag filtering with pre-filter** (reduces FTS search scope):

```sql
WHERE (collection_name, slug) IN (
    SELECT collection_name, slug FROM content_tags_expanded
    WHERE tag_word IN ('copilot', 'ai')
)
```

### Schema Location

See [src/TechHub.Infrastructure/Data/Migrations/postgres/](../src/TechHub.Infrastructure/Data/Migrations/postgres/) for complete schema.

## Database Statistics

The admin UI includes a **Statistics** page (`/admin/statistics`) powered by `DatabaseStatisticsService`. It queries PostgreSQL system views to surface:

- **Content counts** — total items, broken down by collection and by section
- **Tag statistics** — unique tag count and total tag words
- **Table sizes** — disk usage for each table (`pg_total_relation_size`)
- **Processing stats** — recent content processing job history
- **Slow queries** — queries logged above the configured threshold (from `pg_stat_statements`)
- **Latest items** — most recently synced content items

All timestamps are reported in `Europe/Brussels` timezone.
