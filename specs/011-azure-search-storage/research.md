# Research: PostgreSQL Storage & Search Architecture

**Feature**: PostgreSQL Storage & Search Architecture  
**Branch**: `011-azure-search-storage`  
**Date**: 2026-01-27  
**Status**: Complete

## Purpose

This research document captures technical decisions and best practices for migrating from filesystem-based content storage to PostgreSQL. All technical unknowns from the spec have been resolved through this research.

## Research Areas

### 1. PostgreSQL Full-Text Search (tsvector)

**Question**: How to implement effective full-text search with PostgreSQL tsvector?

**Research Findings**:

- **tsvector** is PostgreSQL's built-in full-text search type, optimized for text search operations
- **ts_rank** provides relevance scoring for search results
- **ts_headline** generates highlighted excerpts with matched terms
- **Generated columns** (STORED) automatically maintain search vectors when content changes
- **GIN indexes** on tsvector columns enable fast full-text queries

**Decision**: Use generated tsvector column with weighted fields (title > excerpt > content)

**Example Implementation**:

```sql
-- In content_items table
search_vector TSVECTOR GENERATED ALWAYS AS (
    setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
    setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
    setweight(to_tsvector('english', coalesce(content, '')), 'C')
) STORED

-- Index for performance
CREATE INDEX idx_content_search ON content_items USING GIN(search_vector);

-- Query pattern
SELECT c.*, ts_rank(c.search_vector, query) AS rank
FROM content_items c,
     plainto_tsquery('english', 'search terms') AS query
WHERE c.search_vector @@ query
ORDER BY rank DESC;
```

**References**:

- PostgreSQL Documentation: [Full Text Search](https://www.postgresql.org/docs/current/textsearch.html)
- PostgreSQL Documentation: [GIN Indexes](https://www.postgresql.org/docs/current/gin.html)

---

### 2. Dapper for .NET Database Access

**Question**: How to use Dapper effectively for read-heavy workloads?

**Research Findings**:

- **Dapper** is a lightweight micro-ORM optimized for performance
- Maps database rows to C# objects with minimal overhead
- Supports async/await for non-blocking database operations
- Query methods: `Query<T>`, `QuerySingleOrDefault<T>`, `QueryMultiple`
- Parameter binding prevents SQL injection: `new { paramName = value }`

**Decision**: Use Dapper for all database operations (no EF Core entity tracking overhead)

**Example Implementation**:

```csharp
public class PostgresContentRepository : IContentRepository
{
    private readonly IDbConnection _connection;
    
    public async Task<ContentItem?> GetBySlugAsync(string slug, CancellationToken ct)
    {
        const string sql = @"
            SELECT id, title, content, excerpt, date_epoch, collection_name
            FROM content_items
            WHERE id = @slug AND draft = false";
        
        return await _connection.QuerySingleOrDefaultAsync<ContentItem>(
            new CommandDefinition(sql, new { slug }, cancellationToken: ct));
    }
}
```

**References**:

- Dapper GitHub: [https://github.com/DapperLib/Dapper](https://github.com/DapperLib/Dapper)
- Best practices: Use `CommandDefinition` for cancellation token support

---

### 3. SQLite FTS5 for Local Development

**Question**: How to implement full-text search in SQLite for local dev parity?

**Research Findings**:

- **FTS5** is SQLite's full-text search extension (enabled by default in .NET SQLite)
- Provides similar capabilities to PostgreSQL tsvector
- **bm25()** ranking function for relevance scoring
- Virtual table approach with triggers to keep FTS index in sync
- MATCH operator for full-text queries

**Decision**: Use FTS5 virtual table with triggers for local dev/integration tests

**Example Implementation**:

```sql
-- Create FTS5 virtual table
CREATE VIRTUAL TABLE content_fts USING fts5(
    id, title, excerpt, content,
    content='content_items',
    content_rowid='rowid'
);

-- Triggers to maintain sync
CREATE TRIGGER content_ai AFTER INSERT ON content_items BEGIN
    INSERT INTO content_fts(rowid, id, title, excerpt, content)
    VALUES (new.rowid, new.id, new.title, new.excerpt, new.content);
END;

-- Query pattern
SELECT c.*, bm25(content_fts) AS rank
FROM content_items c
JOIN content_fts ON c.id = content_fts.id
WHERE content_fts MATCH 'search terms'
ORDER BY rank;
```

**References**:

- SQLite FTS5 Documentation: [https://www.sqlite.org/fts5.html](https://www.sqlite.org/fts5.html)

---

### 4. Hash-Based Change Detection (SHA256)

**Question**: How to efficiently detect changed markdown files for incremental sync?

**Research Findings**:

- **SHA256** provides collision-resistant hashing (practically impossible to collide)
- .NET provides `SHA256.HashData()` for file content hashing
- Store hash in database for comparison on subsequent runs
- Compare file hash vs database hash to determine INSERT/UPDATE/DELETE operations

**Decision**: Use SHA256 hash of file content for change detection

**Example Implementation**:

```csharp
public static class FileHasher
{
    public static string ComputeHash(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        var hashBytes = SHA256.HashData(stream);
        return Convert.ToHexString(hashBytes);
    }
}

// In ContentSyncService
var fileHash = FileHasher.ComputeHash(filePath);
var dbHash = await GetStoredHashAsync(slug);

if (dbHash == null)
    await InsertContentAsync(content, fileHash);  // New file
else if (fileHash != dbHash)
    await UpdateContentAsync(content, fileHash);  // Changed file
// else: Skip unchanged file
```

**References**:

- .NET SHA256 Documentation: [System.Security.Cryptography.SHA256](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.sha256)

---

### 5. Keyset Pagination (Cursor-Based)

**Question**: How to implement efficient pagination for infinite scroll?

**Research Findings**:

- **Offset pagination** (`LIMIT x OFFSET y`) scans all skipped rows (O(n) performance)
- **Keyset pagination** (`WHERE (date, id) < cursor`) seeks directly (O(1) performance)
- Cursor encodes last item's sort keys (date_epoch, id for tiebreaking)
- Base64-encoded JSON provides safe URL transmission
- Stable across data changes (new items above cursor, deleted items below cursor skipped)

**Decision**: Use keyset pagination with Base64-encoded JSON cursor

**Example Implementation**:

```csharp
public record PaginationCursor(long DateEpoch, string Id);

public static string EncodeCursor(PaginationCursor cursor) =>
    Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(cursor));

public static PaginationCursor? DecodeCursor(string? token) =>
    string.IsNullOrEmpty(token) ? null :
    JsonSerializer.Deserialize<PaginationCursor>(Convert.FromBase64String(token));

// SQL query pattern
const string sql = cursor == null
    ? "SELECT * FROM content_items ORDER BY date_epoch DESC, id DESC LIMIT @take"
    : "SELECT * FROM content_items WHERE (date_epoch, id) < (@cursorDate, @cursorId) ORDER BY date_epoch DESC, id DESC LIMIT @take";
```

**References**:

- Pagination patterns: [Use the Index, Luke - Pagination](https://use-the-index-luke.com/no-offset)

---

### 6. Database Migrations (SQL Scripts)

**Question**: How to version and apply database schema changes?

**Research Findings**:

- **SQL migration scripts** provide explicit control over schema changes
- Numbered files ensure execution order (001_initial_schema.sql, 002_add_indexes.sql)
- Track applied migrations in database table
- Idempotent scripts support rerunning safely (IF NOT EXISTS, DROP IF EXISTS)

**Decision**: Use numbered SQL migration scripts with tracking table

**Example Implementation**:

```sql
-- migrations/__migrations_log.sql
CREATE TABLE IF NOT EXISTS __migrations (
    id SERIAL PRIMARY KEY,
    version TEXT NOT NULL UNIQUE,
    applied_at TIMESTAMPTZ DEFAULT NOW()
);

-- Check if migration already applied
SELECT EXISTS(SELECT 1 FROM __migrations WHERE version = '001_initial_schema');

-- migrations/001_initial_schema.sql
BEGIN;

CREATE TABLE IF NOT EXISTS content_items (...);
CREATE TABLE IF NOT EXISTS content_tags (...);
-- ... other tables

INSERT INTO __migrations (version) VALUES ('001_initial_schema')
ON CONFLICT (version) DO NOTHING;

COMMIT;
```

**References**:

- Database migration best practices: [Evolutionary Database Design](https://martinfowler.com/articles/evodb.html)

---

### 7. Docker Compose for Local PostgreSQL

**Question**: How to run PostgreSQL locally for development and E2E tests?

**Research Findings**:

- **Docker Compose** provides consistent local development environment
- PostgreSQL official image (postgres:16-alpine) includes all needed extensions
- Volume mounts persist data across container restarts
- DevContainer can use Docker Compose services

**Decision**: Add PostgreSQL service to docker-compose.yml

**Example Implementation**:

```yaml
# docker-compose.yml
version: '3.8'
services:
  postgres:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: techhub
      POSTGRES_USER: techhub
      POSTGRES_PASSWORD: localdev
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U techhub"]
      interval: 5s
      timeout: 5s
      retries: 5

volumes:
  pgdata:
```

**References**:

- Docker PostgreSQL Image: [https://hub.docker.com/_/postgres](https://hub.docker.com/_/postgres)

---

### 8. Azure PostgreSQL Flexible Server

**Question**: What Azure PostgreSQL tier for production hosting?

**Research Findings**:

- **Basic tier**: 2 vCores, 5GB storage, ~$15/month
- **General Purpose tier**: 4+ vCores, 128GB storage, ~$80/month
- Tech Hub working set: ~50MB database + ~15MB hot data
- Read-heavy workload with infrequent writes (sync only)

**Decision**: Use Basic tier (2 vCores, 5GB) - sufficient for 10,000+ articles

**Cost Projection**:

| Tier | vCores | Storage | Cost/Month | Sufficient For |
|------|--------|---------|------------|----------------|
| Basic | 2 | 5GB | $15 | Up to 10,000 articles |
| General Purpose | 4 | 32GB | $80 | 50,000+ articles |

**References**:

- Azure PostgreSQL Pricing: [https://azure.microsoft.com/pricing/details/postgresql/](https://azure.microsoft.com/pricing/details/postgresql/)

---

## Technology Decisions Summary

| Area | Technology | Rationale |
|------|------------|-----------|
| **ORM** | Dapper | Read-heavy workload, no change tracking, direct SQL control |
| **Full-Text Search** | PostgreSQL tsvector | Built-in, performant, no external dependencies |
| **Local Dev FTS** | SQLite FTS5 | Parity with PostgreSQL, enabled by default |
| **Change Detection** | SHA256 hashing | Collision-resistant, fast, reliable |
| **Pagination** | Keyset (cursor-based) | O(1) performance, stable across data changes |
| **Migrations** | SQL scripts | Explicit control, idempotent, versioned |
| **Local PostgreSQL** | Docker Compose | Consistent environment, easy setup |
| **Production Hosting** | Azure PostgreSQL Basic | Cost-effective ($15/mo), sufficient capacity |

## Unknowns Resolved

All technical unknowns from the spec have been resolved:

✅ PostgreSQL full-text search implementation (tsvector + GIN indexes)  
✅ Dapper usage patterns for read-heavy workloads  
✅ SQLite FTS5 for local development parity  
✅ Hash-based change detection algorithm (SHA256)  
✅ Keyset pagination implementation (Base64-encoded JSON cursor)  
✅ Database migration strategy (numbered SQL scripts)  
✅ Local PostgreSQL setup (Docker Compose)  
✅ Azure PostgreSQL tier selection (Basic tier sufficient)

## Next Steps

Proceed to **Phase 1: Design & Contracts** to generate:

- `data-model.md` - Complete database schema with all tables and relationships
- `contracts/` - Repository interface definitions with new search methods
- `quickstart.md` - Developer onboarding guide
