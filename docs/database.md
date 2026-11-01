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
| `content_items` | All content with denormalized section flags and bitmask. Also holds the generated `search_vector` column used for full-text search and the optional `ai_metadata` JSON (roundup metadata, `schema_version` 2: key facts, significance, section roles, ...) |
| `content_tags_expanded` | Word-level tag matching (denormalized for fast filtering) |
| `rss_feed_configs` | RSS feed configuration (URL, name, collection, enabled flag) |
| `processed_urls` | Tracks processed/skipped/failed URLs for the content pipeline |
| `content_processing_jobs` | Audit log of background job runs (content processing, content fixer, roundup generation, timeline backfill) |
| `content_timeline_events` | Dated lifecycle events (preview, GA, deprecation, ...) that articles state, one row per event with a cascading foreign key to the source article and a link to the catalog product (see [content-processing.md](content-processing.md#timeline-events)) |
| `content_timeline_extractions` | Which articles have been scanned for timeline events, with the prompt version, so the backfill job is resumable |
| `products` | Curated product catalog: canonical product names, with an optional parent product (see [content-processing.md](content-processing.md#product-catalog)) |
| `product_aliases` | Other names for a product; a name the AI returns that matches an alias is linked to the product |
| `ignored_product_names` | Names the AI returns that are not products, kept out of the review queue |
| `product_qualifiers` | Vocabulary of plans and surfaces (`kind`), with `product_qualifier_aliases` for other names. Product aliases, article links and timeline events carry them (`content_timeline_event_qualifiers`), see [content-processing.md](content-processing.md#plans-and-surfaces) |
| `product_features` | Curated features of a product, with `product_feature_aliases` for other names. Timeline events link to one through `feature_id`, see [content-processing.md](content-processing.md#features). The features of GitHub Copilot also carry the data of the features page: `slug`, `description`, `release_date` |
| `product_feature_content` | Links features to content items (many-to-many) with a thumbnail flag and sort order. Used by the GitHub Copilot features page |
| `content_item_products` | The product names an article lists, linked to the catalog (`product_id` is empty for names that are not in the catalog), with a cascading foreign key to the article |
| `content_reviews` | Review queue for content fixer changes (tags, markdown, author) awaiting approval |
| `custom_page_data` | JSON data for custom pages (editable from admin UI) |
| `background_job_settings` | Per-job enabled flag that gates scheduled runs (see [content-processing.md](content-processing.md#scheduling)) |
| `vscode_update_items` | Membership set of content items shown on the VS Code Updates page |
| `newsletter_subscribers` | Newsletter subscribers with confirmation state and section preferences |
| `newsletter_send_log` | One row per newsletter send, used to prevent duplicate sends |
| `sync_metadata` | Key/value store written only by the test-only `ContentSyncService` (not used by the running application) |
| `_migrations` | Names of executed migration scripts (see [Migrations](#migrations)) |

See [content-schema.md](content-schema.md) for the content and GitHub Copilot feature columns.

### Design Decisions

1. **Denormalized section flags**: Each content item has `is_ai`, `is_azure`, `is_github_copilot`, etc. boolean columns for fast filtering without JOINs
2. **Section bitmask**: Single integer column for multi-section filtering with bitwise operations
3. **Expanded tags table**: Multi-word tags split into separate rows for word-level matching
4. **Generated tsvector column**: `content_items.search_vector` is a `GENERATED ALWAYS ... STORED` column indexed by the GIN index `idx_content_search`. The database keeps it current on every write, so there are no triggers or separate search tables. It covers title, excerpt, content, feed name, collection name and primary section name. Tags are not part of it and filter through `content_tags_expanded` instead
5. **Prefix matching**: Search queries append `:*` to each term, so partial words match (e.g., "cop" matches "copilot"). See [filtering.md](filtering.md#search-implementation)

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

## Migrations

The schema is managed by numbered SQL scripts (`001_`, `002_`, ...) that are embedded in the Infrastructure assembly.

- **Runner**: `MigrationRunner` executes the scripts in ordinal filename order, independent of the current culture, and records each executed filename in the `_migrations` table, so every script runs once. Legacy rows containing full embedded resource names are also recognized and left unchanged; only new executions use filenames.
- **When**: The API runs pending migrations on startup through `StartupBackgroundService`. Migrations are never applied by hand.
- **Locking**: A process-wide lock prevents concurrent runs.
- **Fail-fast**: A failing script is logged and the exception stops the application, so a half-migrated database never serves traffic. The API `/health` endpoint reports unhealthy until startup completes (see [health-checks.md](health-checks.md)).

## Database Statistics

The admin UI includes a **Statistics** page (`/admin/statistics`) powered by `DatabaseStatisticsService`. It queries PostgreSQL system views to surface:

- **Content counts** — total items, broken down by collection and by section
- **Tag statistics** — unique tag count and total tag words
- **Table sizes** — disk usage for each table (`pg_total_relation_size`)
- **Processing stats** — recent content processing job history
- **Slow queries** — queries logged above the configured threshold (from `pg_stat_statements`)
- **Latest items** — most recently synced content items

All timestamps are reported in `Europe/Brussels` timezone.
