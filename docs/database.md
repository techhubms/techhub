# Database Configuration

Tech Hub supports two content storage backends configured via `appsettings.json`.

**Related Documentation**:

- [Content Processing](content-processing.md) - How content syncs to the database
- [Filtering](filtering.md) - Tag filtering that leverages database indexes
- [Running & Testing](running-and-testing.md) - How to run with different database providers
- [Query Logging](query-logging.md) - Debug and performance monitoring with query logging

## Supported Providers

> **Default provider**: If `Database:Provider` is not specified, Tech Hub.Api defaults to **PostgreSQL** with connection string `Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev`.

### Option 1: FileSystem (No Database)

Reads markdown files directly from `collections/` folder.

```json
{
  "Database": {
    "Provider": "FileSystem"
  }
}
```

**Pros**: Fastest startup, no database needed, simplest setup  
**Cons**: No full-text search, slower filtering on large datasets

### Option 2: PostgreSQL (Recommended)

Uses PostgreSQL with tsvector full-text search and GIN indexes.

**Configuration**:

```json
{
  "Database": {
    "Provider": "PostgreSQL",
    "ConnectionString": "Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev"
  },
  "ContentSync": {
    "Enabled": true
  }
}
```

**Pros**: Production-ready, best performance at scale, full-text search with tsvector, semantic search ready  
**Cons**: Requires Docker (`docker compose up -d postgres`) or cloud PostgreSQL instance

**Sync Behavior**:

- **First Run**: Syncs from markdown files (~30-60s for 4000+ files)
- **Subsequent Runs**: Hash-based diff (<1s if no changes)

**Usage**:

- See [running-and-testing.md](running-and-testing.md) for instructions on running with Docker.

## Content Sync Configuration

Control database sync behavior via `appsettings.json`:

```json
{
  "ContentSync": {
    "Enabled": true,         // Set false to skip sync (faster startup)
    "ForceFullSync": false,  // Set true to force complete reimport
    "MaxParallelFiles": 10   // Parallel file processing during sync
  }
}
```

**Tip**: Set `ContentSync:Enabled = false` for rapid iteration when not testing search/filtering features.

## Database Schema

### Main Tables

| Table | Purpose |
|-------|--------|
| `content_items` | All content with denormalized section flags and bitmask |
| `content_tags_expanded` | Word-level tag matching (denormalized for fast filtering) |
| `sync_metadata` | Tracks last sync time and content hashes |
| `content_fts` | Full-text search using tsvector/tsquery |

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
