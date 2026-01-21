# Feature Specification: PostgreSQL Storage & Search Architecture

**Feature Branch**: `011-azure-search-storage`  
**Created**: 2026-01-20  
**Status**: Draft  
**Input**: Move from filesystem storage to PostgreSQL for fast filtering, faceted navigation with accurate counts, full-text search, and tag subset matching. Azure AI Search can be added later for semantic/vector search.

## Executive Summary

This specification defines the architectural foundation for migrating Tech Hub from filesystem-based storage to a **PostgreSQL database** as the primary storage and search engine. This enables:

1. **Blazing-fast faceted navigation** with accurate, real-time counts via SQL aggregations
2. **Full-text search** with PostgreSQL's built-in `tsvector` capabilities
3. **Tag subset matching** ("AI" matches "Azure AI", "Generative AI") via word tokenization
4. **Scalable storage** ready for 10,000+ articles
5. **Cost-effective hosting** (~$15/month on Azure PostgreSQL Basic vs $75-250/month for Azure AI Search)
6. **Easy local development** with SQLite or Docker PostgreSQL

**Future Enhancement**: Azure AI Search can be added as a Phase 2 enhancement for semantic/vector search and AI agent integration via MCP server.

**Important Clarification**: This is a **search-only** experience. Users search and browse results, then clicking any result directs them to the original source page (internal for videos/roundups, external for other collections). No detail pages are rendered within Tech Hub itself.

## Architectural Decisions

### Primary Storage & Search: PostgreSQL

**Rationale**: PostgreSQL is the optimal choice for Tech Hub's current needs:

| Requirement | PostgreSQL Capability |
| ----------- | --------------------- |
| Fast faceted counts | `GROUP BY` with proper indexes, single query returns counts |
| Tag subset matching | Junction table with tokenized tags, word-boundary matching |
| Multiple tag AND logic | SQL `INTERSECT` or `HAVING COUNT(DISTINCT tag) = N` |
| Full-text search | Built-in `tsvector` with ranking and highlighting |
| .NET/C# support | First-class `Npgsql` and EF Core support |
| Local development | SQLite (fallback) or Docker PostgreSQL |
| Cost | ~$15/month Azure PostgreSQL Basic vs $75-250/month AI Search |

**Future Enhancement**: Azure AI Search can be added later for:

- Semantic/vector search
- AI agent integration via MCP server
- Hybrid search (full-text + embeddings)

**Alternatives Considered**:

| Option | Pros | Cons | Decision |
| ------ | ---- | ---- | -------- |
| **Azure AI Search** | Excellent facets, vector search, semantic ranking | Expensive ($75-250/mo), no local emulator | ⏸️ Phase 2 for AI features |
| **Azure Cosmos DB** | Vector search, global distribution | No built-in facet counts, expensive | ❌ Rejected |
| **Graph Database (Neo4j)** | Great for relationships | Overkill, poor full-text search, expensive on Azure | ❌ Rejected |
| **SQLite** | Zero cost, embedded | Limited concurrency, no built-in full-text | ✅ Local dev fallback only |

### Content Source of Truth: Git Repository (Markdown Files)

**Critical Decision**: Markdown files in `collections/` remain the **source of truth**:

- Content authors continue editing markdown files
- Database is a **derived store**, rebuilt from markdown files
- **No cross-environment sync needed** - each environment rebuilds from Git
- Enables easy rollback, version control, and local development

### Sync Strategy: Incremental Hash-Based Updates

**Critical Requirement**: Database sync must be fast after first run:

1. **First startup**: Full import from markdown files (slow, ~30-60 seconds for 4000+ files)
2. **Subsequent startups**: Hash-based diff detection (fast, <1 second if no changes)
3. **Skip option**: `ContentSync:Enabled = false` in appsettings to skip entirely for faster local dev

**Hash-Based Change Detection**:

- Each content item has a `ContentHash` (SHA256 of file content)
- On startup, compare file hashes vs database hashes
- Only INSERT/UPDATE/DELETE changed items
- Store last sync timestamp for audit

**Configuration Options** (appsettings.json):

```json
{
  "ContentSync": {
    "Enabled": true,           // Set to false to skip sync entirely
    "ForceFullSync": false,    // Set to true to force full re-import
    "MaxParallelFiles": 10     // Parallel file processing
  }
}
```

### Architecture Overview

```text
┌─────────────────────────────────────────────────────────────────────────────┐
│                              CONTENT FLOW                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌──────────────┐      ┌──────────────┐      ┌────────────────────────────┐ │
│  │  Markdown    │      │   Startup /  │      │        PostgreSQL          │ │
│  │  Files       │──────│   CI/CD      │──────│     (Primary Store)        │ │
│  │  (Git Repo)  │      │  Sync        │      │                            │ │
│  │              │      │              │      │  • Full-text search        │ │
│  │  Source of   │      │  • Hash diff │      │  • Faceted navigation      │ │
│  │  Truth       │      │  • Upsert    │      │  • Tag junction table      │ │
│  │              │      │  • Skip opt  │      │  • Indexed queries         │ │
│  └──────────────┘      └──────────────┘      └────────────────────────────┘ │
│                                                        │                     │
│         LOCAL DEV                                      ▼                     │
│  ┌──────────────┐      ┌────────────────────────────────────────────────┐   │
│  │   SQLite or  │      │                APPLICATION LAYER              │   │
│  │   Docker     │      ├────────────────────────────────────────────────┤   │
│  │   PostgreSQL │      │                                                │   │
│  │              │      │  ┌──────────────┐    ┌──────────────┐          │   │
│  │  Same schema │      │  │  TechHub.Api │    │ TechHub.Web  │          │   │
│  │  Same sync   │      │  │  (REST API)  │    │ (Blazor SSR) │          │   │
│  └──────────────┘      │  │              │    │              │          │   │
│                        │  │  /api/search │    │  Tag Cloud   │          │   │
│  ┌──────────────┐      │  │  /api/filter │    │  Filters     │          │   │
│  │ Azure AI     │      │  │  /api/facets │    │  Pagination  │          │   │
│  │ Search       │      │  └──────────────┘    └──────────────┘          │   │
│  │ (Phase 2)    │      │                                                │   │
│  │              │      │  ┌──────────────────────────────────────────┐  │   │
│  │ • Semantic   │◄─────│  │  Phase 2: AI Agent / MCP Server          │  │   │
│  │ • Vectors    │      │  │  (Syncs from PostgreSQL to AI Search)    │  │   │
│  └──────────────┘      │  └──────────────────────────────────────────┘  │   │
│                        │                                                │   │
│                        └────────────────────────────────────────────────┘   │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

## User Scenarios & Testing

### User Story 1 - Fast Tag Filtering with Accurate Counts (Priority: P0)

Users can filter content by clicking tags and immediately see updated counts for remaining tags, enabling informed navigation decisions with AND logic.

**Why this priority**: Core feature for content discovery. Current filesystem-based filtering cannot provide real-time counts efficiently at scale.

**Independent Test**: Select "AI" tag, verify remaining tag counts update within 200ms, showing how many articles match if user clicks additional tags.

**Acceptance Scenarios**:

1. **Given** I'm on a section page with 1000+ articles, **When** I click the "AI" tag, **Then** the content filters within 200ms AND all remaining tag counts update to show intersection counts
2. **Given** I have "AI" tag selected, **When** I view other tags like "Azure", **Then** I see the count showing how many articles have BOTH "AI" AND "Azure" tags
3. **Given** I select multiple tags (AI + Azure + Videos), **When** filters apply, **Then** only articles matching ALL selected tags appear (AND logic)
4. **Given** I'm filtering by tags, **When** a tag would result in zero articles if clicked, **Then** that tag shows count "0" and is visually de-emphasized
5. **Given** I clear all tag filters, **When** the page reloads, **Then** all tag counts return to their full scope values

### User Story 2 - Tag Subset Matching (Priority: P0)

Users selecting "AI" see all content tagged with "AI" or tags containing "AI" as a complete word (e.g., "Azure AI", "Generative AI", "AI Agents").

**Why this priority**: Critical for user experience - prevents missing relevant content due to tag variations.

**Independent Test**: Search for "AI" tag, verify results include "AI", "Azure AI", "Generative AI" but NOT "AIR" or "FAIR".

**Acceptance Scenarios**:

1. **Given** I select "AI" tag, **When** results display, **Then** I see articles with tags: "AI", "Azure AI", "Generative AI", "AI Agents", "AI Engineering"
2. **Given** I select "Visual Studio" tag, **When** results display, **Then** I see articles with "Visual Studio", "Visual Studio Code", "Visual Studio 2022"
3. **Given** tag subset matching is active, **When** matching "AI", **Then** it uses word boundaries (does NOT match "AIR", "FAIR", "DAIRY")
4. **Given** I search for "Azure", **When** facet counts update, **Then** counts include both exact "Azure" matches and "Azure AI", "Azure DevOps", etc.

### User Story 3 - Full-Text Search with Highlighting (Priority: P1)

Users can search article content, titles, and descriptions with highlighted results and relevance ranking.

**Why this priority**: Essential search capability that enables content discovery beyond tag-based navigation.

**Independent Test**: Search "agent framework tutorial", verify results ranked by relevance with matching terms highlighted.

**Acceptance Scenarios**:

1. **Given** I enter "agent framework" in search, **When** results appear, **Then** articles mentioning "agent framework" in title/content rank highest
2. **Given** search results display, **When** I view article excerpts, **Then** matching terms are highlighted for easy scanning
3. **Given** I search "getting started Azure AI", **When** results appear, **Then** results include partial matches ranked by relevance
4. **Given** I combine search query with tag filters, **When** results update, **Then** AND logic applies (matches search AND has selected tags)
5. **Given** I search with typo "agnet framwork", **When** results display, **Then** fuzzy matching returns relevant "agent framework" results

### User Story 4 - Semantic AI Search (Priority: P2 - Phase 2)

**Deferred to Phase 2**: Semantic/vector search requires Azure AI Search integration.

Users can perform natural language queries that understand intent and context, returning semantically relevant results.

**Why deferred**: Requires Azure AI Search for vector embeddings and semantic ranking. PostgreSQL handles full-text search well, but semantic search needs dedicated AI infrastructure.

**Phase 2 Implementation**: Add Azure AI Search as a secondary index, sync from PostgreSQL, enable semantic queries.

**Independent Test**: Query "show me beginner-friendly videos explaining how to build AI agents", verify semantic relevance of results.

**Acceptance Scenarios** (Phase 2):

1. **Given** I enter semantic query "best videos for learning GitHub Copilot", **When** results appear, **Then** video content about GitHub Copilot tutorials ranks highest
2. **Given** I query "articles by John about Azure DevOps", **When** results display, **Then** articles by author "John" about Azure DevOps appear first
3. **Given** I ask "what's the difference between AI Search and Cosmos DB", **When** searching, **Then** comparative and explanatory articles rank highest
4. **Given** semantic search is combined with filters (section: AI, collection: videos), **When** results display, **Then** semantic relevance applies within the filtered scope
5. **Given** I query in conversational language, **When** results appear, **Then** system understands synonyms and related concepts

### User Story 5 - Date Range Filtering with Facets (Priority: P1)

Users can filter by date ranges with the date slider, with faceted counts updating to reflect temporal scope.

**Why this priority**: Complements tag filtering, essential for finding recent content.

**Independent Test**: Set date range to "Last 30 days", verify tag counts update to reflect only content from that period.

**Acceptance Scenarios**:

1. **Given** I select "Last 30 days", **When** tag counts update, **Then** they reflect only articles published in the last 30 days
2. **Given** I have date range + tag filters active, **When** I view results, **Then** AND logic applies (date range AND tags)
3. **Given** I use custom date slider, **When** I drag to a specific range, **Then** facet counts update in real-time (debounced)
4. **Given** a tag has zero articles in selected date range, **When** viewing tag cloud, **Then** tag shows count "0" and is de-emphasized

### User Story 6 - Related Articles Discovery (Priority: P2 - Phase 2)

**Phase 1 (PostgreSQL)**: Tag-based related articles using shared tags.
**Phase 2 (AI Search)**: Semantic similarity using vector embeddings.

Users viewing an article see a "Related Articles" section showing similar content.

**Why this priority**: Increases engagement and content discovery through contextual recommendations.

**Independent Test**: View an article about "Azure AI Search", verify related articles section shows topically similar content.

**Acceptance Scenarios**:

1. **Given** I'm viewing an article about "GitHub Copilot tips", **When** I scroll to related articles, **Then** I see other GitHub Copilot content ranked by tag overlap (Phase 1) or semantic similarity (Phase 2)
2. **Given** an article has specific tags, **When** viewing related content, **Then** articles sharing tags appear, weighted by tag overlap count
3. **Given** I click a related article, **When** navigating, **Then** URL preserves any active filters as context
4. **Given** an article is the only one on a niche topic, **When** viewing related content, **Then** broader related articles appear (same section/collection)

### User Story 7 - MCP Server for AI Agents (Priority: P2 - Phase 2)

**Deferred to Phase 2**: MCP server benefits most from semantic search capabilities.

External AI agents can query Tech Hub content via Model Context Protocol (MCP) for grounded responses.

**Why deferred**: While PostgreSQL can serve structured queries, MCP agents benefit significantly from semantic search for natural language queries. Best implemented alongside Azure AI Search in Phase 2.

**Phase 1 Alternative**: REST API endpoints can serve basic structured queries to agents.

**Independent Test**: MCP client queries "find Azure DevOps security best practices", receives structured response with relevant articles.

**Acceptance Scenarios** (Phase 2):

1. **Given** an MCP client connects to Tech Hub, **When** it queries content, **Then** structured search results are returned with metadata
2. **Given** agent queries "top 5 videos about AI agents", **When** processing, **Then** results include title, URL, author, date, and relevance score
3. **Given** agent uses MCP with context window, **When** querying, **Then** responses fit within token limits with summarized excerpts
4. **Given** MCP server is running, **When** external agents connect, **Then** authentication and rate limiting apply

### User Story 8 - Local Development Without Azure (Priority: P0)

Developers can run the full application locally without any Azure connectivity using SQLite or Docker PostgreSQL.

**Why this priority**: Essential for developer experience, CI/CD pipeline efficiency, and cost control.

**Independent Test**: Run `Run` without Azure credentials, verify all tests pass.

**Acceptance Scenarios**:

1. **Given** I'm developing locally, **When** I start the app, **Then** it uses SQLite or Docker PostgreSQL (configurable)
2. **Given** I set `ContentSync:Enabled = false`, **When** I start the app, **Then** sync is skipped entirely for fast startup
3. **Given** I run the app with sync enabled, **When** content hasn't changed, **Then** startup completes in <1 second (hash check only)
4. **Given** I run the app for the first time, **When** sync runs, **Then** full import completes with progress logging
5. **Given** I run integration tests, **When** executing, **Then** tests use in-memory SQLite with seeded test data
6. **Given** E2E tests run, **When** executing, **Then** tests use Docker PostgreSQL with real sync
7. **Given** Docker Compose is configured, **When** I run `docker-compose up`, **Then** local PostgreSQL starts and app syncs on first run

## Functional Requirements

### FR-1: PostgreSQL Database Schema

The database must support all content filtering and search scenarios with efficient indexing.

**Simplified Data Model Notes**:

- **No `description` field** - We use `excerpt` exclusively for summaries
- **No `viewingMode` field** - Frontend determines internal/external based on collection
- **Single date field** - `dateEpoch` (Unix timestamp); ISO format derived at query time
- **ContentHash** - SHA256 of file content for incremental sync detection
- **`external_url`** - Points to original source content; only NULL for roundups (we generate those)
- **Junction tables** - For tags, sections, collections, AND authors (proper normalization, efficient faceting)

**Navigation Logic** (handled by frontend):

- `collection_name` in `["videos", "roundups"]` → Internal URL: `/{section}/{collection}/{slug}`
- All other collections → External URL: `external_url` with `target="_blank"`

**Database Tables**:

```sql
-- Main content table
CREATE TABLE content_items (
    id              TEXT PRIMARY KEY,           -- Slug (e.g., "2024-06-19-copilot-tips")
    title           TEXT NOT NULL,              -- Full-text searchable
    content         TEXT NOT NULL,              -- Full article text
    excerpt         TEXT,                       -- Summary for snippets
    date_epoch      BIGINT NOT NULL,            -- Unix timestamp
    external_url    TEXT,                       -- Original source URL (NULL only for roundups)
    content_hash    TEXT NOT NULL,              -- SHA256 for change detection
    created_at      TIMESTAMPTZ DEFAULT NOW(),
    updated_at      TIMESTAMPTZ DEFAULT NOW(),
    
    -- Full-text search vector (PostgreSQL)
    search_vector   TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C')
    ) STORED
);

-- Authors lookup table (for normalization and faceting)
CREATE TABLE authors (
    id              SERIAL PRIMARY KEY,
    name            TEXT NOT NULL UNIQUE,       -- e.g., "John Doe"
    name_normalized TEXT NOT NULL               -- Lowercase for matching
);

-- Content-Authors junction table (many-to-many, supports multiple authors)
CREATE TABLE content_authors (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    author_id       INTEGER NOT NULL REFERENCES authors(id) ON DELETE CASCADE,
    PRIMARY KEY (content_id, author_id)
);

-- Collections lookup table (for normalization)
CREATE TABLE collections (
    id              SERIAL PRIMARY KEY,
    name            TEXT NOT NULL UNIQUE,       -- e.g., "videos", "blogs"
    is_internal     BOOLEAN NOT NULL DEFAULT FALSE  -- videos, roundups = true
);

-- Content-Collections junction table (content belongs to one collection, but normalized)
CREATE TABLE content_collections (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    collection_id   INTEGER NOT NULL REFERENCES collections(id) ON DELETE CASCADE,
    PRIMARY KEY (content_id)
);

-- Tags junction table (many-to-many)
CREATE TABLE content_tags (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    tag             TEXT NOT NULL,              -- Original tag (e.g., "Azure AI")
    tag_normalized  TEXT NOT NULL,              -- Lowercase, trimmed
    PRIMARY KEY (content_id, tag)
);

-- Expanded tags for subset matching (e.g., "Azure AI" -> "azure", "ai")
CREATE TABLE content_tags_expanded (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    tag_word        TEXT NOT NULL,              -- Individual word (e.g., "azure")
    PRIMARY KEY (content_id, tag_word)
);

-- Section names junction table (many-to-many)
CREATE TABLE content_sections (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    section_name    TEXT NOT NULL,              -- e.g., "ai", "github_copilot"
    PRIMARY KEY (content_id, section_name)
);

-- Sync metadata for tracking last sync state
CREATE TABLE sync_metadata (
    key             TEXT PRIMARY KEY,
    value           TEXT NOT NULL,
    updated_at      TIMESTAMPTZ DEFAULT NOW()
);

-- Indexes for performance
CREATE INDEX idx_content_date ON content_items(date_epoch DESC);
CREATE INDEX idx_content_search ON content_items USING GIN(search_vector);
CREATE INDEX idx_content_hash ON content_items(content_hash);

CREATE INDEX idx_authors_name ON authors(name_normalized);
CREATE INDEX idx_content_authors_author ON content_authors(author_id);
CREATE INDEX idx_content_authors_content ON content_authors(content_id);

CREATE INDEX idx_collections_name ON collections(name);
CREATE INDEX idx_content_collections_collection ON content_collections(collection_id);

CREATE INDEX idx_tags_normalized ON content_tags(tag_normalized);
CREATE INDEX idx_tags_content ON content_tags(content_id);

CREATE INDEX idx_tags_expanded_word ON content_tags_expanded(tag_word);
CREATE INDEX idx_tags_expanded_content ON content_tags_expanded(content_id);

CREATE INDEX idx_sections_name ON content_sections(section_name);
CREATE INDEX idx_sections_content ON content_sections(content_id);
```

**Benefits of Junction Tables for Collections/Authors**:

- **Facet counts**: Fast `GROUP BY` on normalized IDs
- **Data integrity**: Foreign key constraints
- **Future flexibility**: Easy to add author metadata (bio, avatar) or collection config
- **Query efficiency**: Integer joins faster than text comparisons

**SQLite Compatibility** (local dev fallback):

- Same schema structure, but without `TSVECTOR` (use `LIKE` for basic search)
- No `GENERATED ALWAYS` - compute search text in application layer
- Same junction tables work identically

### FR-2: Tag Subset Matching Implementation

**Approach**: Pre-compute expanded tag words during sync:

1. For each tag (e.g., "Azure AI"), extract words: ["azure", "ai"]
2. Store in `content_tags_expanded` table with word-boundary matching
3. When filtering by "AI", query `content_tags_expanded WHERE tag_word = 'ai'`
4. Word boundary matching via tokenization, not substring

**Example**:

- Original tags: `["Azure AI", "Machine Learning"]`
- Stored in `content_tags`: `(id, "Azure AI", "azure ai")`, `(id, "Machine Learning", "machine learning")`
- Stored in `content_tags_expanded`: `(id, "azure")`, `(id, "ai")`, `(id, "machine")`, `(id, "learning")`
- Query for "AI" matches via `WHERE tag_word = 'ai'`

**SQL Query Pattern**:

```sql
-- Find all content with tag containing word "ai"
SELECT DISTINCT c.*
FROM content_items c
JOIN content_tags_expanded e ON c.id = e.content_id
WHERE e.tag_word = 'ai';
```

### FR-3: Faceted Navigation with Dynamic Counts

**Implementation using PostgreSQL aggregations**:

1. Single query returns facet counts for filtered result set
2. Facets calculated using `GROUP BY` with proper indexes
3. Support for facets on: tags, collections, sections, authors
4. Configurable facet limits (top N by count)

**Query Pattern**:

```sql
-- Get tag counts for content in section 'ai'
SELECT t.tag, COUNT(DISTINCT t.content_id) as count
FROM content_tags t
JOIN content_sections s ON t.content_id = s.content_id
WHERE s.section_name = 'ai'
GROUP BY t.tag
ORDER BY count DESC
LIMIT 50;

-- Get author counts with proper join
SELECT a.name, COUNT(DISTINCT ca.content_id) as count
FROM authors a
JOIN content_authors ca ON a.id = ca.author_id
JOIN content_sections s ON ca.content_id = s.content_id
WHERE s.section_name = 'ai'
GROUP BY a.id, a.name
ORDER BY count DESC
LIMIT 20;

-- Get collection counts
SELECT c.name, COUNT(DISTINCT cc.content_id) as count
FROM collections c
JOIN content_collections cc ON c.id = cc.collection_id
JOIN content_sections s ON cc.content_id = s.content_id
WHERE s.section_name = 'ai'
GROUP BY c.id, c.name
ORDER BY count DESC;
```

**AND Logic for Multiple Tags**:

```sql
-- Find content with BOTH 'ai' AND 'azure' tags
SELECT c.*
FROM content_items c
WHERE c.id IN (
    SELECT content_id
    FROM content_tags_expanded
    WHERE tag_word IN ('ai', 'azure')
    GROUP BY content_id
    HAVING COUNT(DISTINCT tag_word) = 2
);
```

### FR-4: Full-Text Search with PostgreSQL

**Implementation using PostgreSQL tsvector**:

1. Full-text search using built-in `tsvector` with weighted fields (title > excerpt > content)
2. Ranking using `ts_rank` or `ts_rank_cd` for relevance scoring
3. Highlighting using `ts_headline` for result snippets
4. Fuzzy matching via `pg_trgm` extension (optional)

**Query Pattern**:

```sql
-- Full-text search with ranking
SELECT 
    c.*,
    ts_rank(c.search_vector, query) AS rank,
    ts_headline('english', c.excerpt, query, 'StartSel=<mark>, StopSel=</mark>') AS highlighted_excerpt
FROM content_items c,
     plainto_tsquery('english', 'agent framework') AS query
WHERE c.search_vector @@ query
ORDER BY rank DESC
LIMIT 20;

-- Combined with filters
SELECT c.*, ts_rank(c.search_vector, query) AS rank
FROM content_items c
JOIN content_sections s ON c.id = s.content_id,
     plainto_tsquery('english', 'getting started') AS query
WHERE c.search_vector @@ query
  AND s.section_name = 'ai'
ORDER BY rank DESC;
```

**SQLite Fallback** (local dev):

```sql
-- Basic LIKE search for SQLite (less sophisticated)
SELECT * FROM content_items
WHERE title LIKE '%agent%' OR content LIKE '%agent%'
ORDER BY date_epoch DESC;
```

### FR-4b: Semantic Search with Vector Embeddings (Phase 2)

**Deferred to Phase 2**: Requires Azure AI Search or PostgreSQL pgvector extension.

1. Generate embeddings for article content using Azure OpenAI
2. Store in Azure AI Search index (synced from PostgreSQL)
3. Hybrid search: combine PostgreSQL full-text with vector similarity
4. Semantic ranking for improved relevance

### FR-5: Content Repository Interface

Extend existing `IContentRepository` with search and facet operations:

```csharp
public interface IContentRepository
{
    // Existing methods
    Task<ContentItem?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<ContentItem>> GetAllAsync(CancellationToken ct = default);
    
    // New search methods
    Task<SearchResults<ContentItemDto>> SearchAsync(
        SearchRequest request,
        CancellationToken ct = default);
    
    Task<FacetResults> GetFacetsAsync(
        FacetRequest request,
        CancellationToken ct = default);
    
    Task<IReadOnlyList<ContentItemDto>> GetRelatedAsync(
        string articleId,
        int count = 5,
        CancellationToken ct = default);
}

// Implementations:
// - PostgresContentRepository (production, CI/CD)
// - SqliteContentRepository (local dev fallback)
// - InMemoryContentRepository (unit tests)
```

### FR-6: Incremental Content Sync Pipeline

**Sync Service Interface**:

```csharp
public interface IContentSyncService
{
    Task<SyncResult> SyncAsync(CancellationToken ct = default);
    Task<SyncResult> ForceSyncAsync(CancellationToken ct = default);
    Task<bool> IsContentChangedAsync(CancellationToken ct = default);
}

public record SyncResult(
    int Added,
    int Updated, 
    int Deleted,
    int Unchanged,
    TimeSpan Duration);
```

**Sync Algorithm**:

```text
1. IF ContentSync:Enabled = false
      SKIP sync, return immediately
      
2. IF ContentSync:ForceFullSync = true
      DELETE all content, re-import everything
      
3. ELSE (Incremental Sync):
      a. Load all markdown files, compute SHA256 hashes
      b. Load all content_hash values from database
      c. Compare:
         - New files (in filesystem, not in DB) → INSERT
         - Changed files (hash mismatch) → UPDATE
         - Deleted files (in DB, not in filesystem) → DELETE
         - Unchanged files (hash match) → SKIP
      d. Process changes in parallel batches (MaxParallelFiles)
      e. Update sync_metadata with timestamp
      
4. Log summary: "Sync complete: X added, Y updated, Z deleted, W unchanged (duration)"
```

**Error Handling** (Fail-Fast with Rollback):

- Entire sync runs in a **database transaction**
- If ANY file fails to parse or sync fails mid-way → **ROLLBACK entire transaction**
- App **fails to startup** with clear error message (no partial state)
- Developer must fix the issue and restart
- Rationale: Immediate visibility into problems, no inconsistent search results

**Startup Integration**:

```csharp
// In Program.cs
var syncService = app.Services.GetRequiredService<IContentSyncService>();
var syncResult = await syncService.SyncAsync();
logger.LogInformation("Content sync: {Added} added, {Updated} updated, {Deleted} deleted in {Duration:N0}ms",
    syncResult.Added, syncResult.Updated, syncResult.Deleted, syncResult.Duration.TotalMilliseconds);
```

**Performance Targets**:

- First sync (4000+ files): < 60 seconds
- Subsequent sync (no changes): < 1 second
- Subsequent sync (10 changes): < 5 seconds

### FR-7: Local Development Strategy

**Database Provider Selection** (via appsettings or environment):

| Environment | Provider | Connection | Sync |
| ----------- | -------- | ---------- | ---- |
| Local (default) | SQLite | `techhub.db` file | On startup (skippable) |
| Local (Docker) | PostgreSQL | `localhost:5432` | On startup (skippable) |
| CI/CD | PostgreSQL | Docker service | Always on startup |
| Dev/Prod | Azure PostgreSQL | Connection string | On deploy |

**Configuration**:

```json
{
  "Database": {
    "Provider": "SQLite",         // or "PostgreSQL"
    "ConnectionString": "Data Source=techhub.db"
  },
  "ContentSync": {
    "Enabled": true,
    "ForceFullSync": false,
    "MaxParallelFiles": 10
  }
}
```

**Docker Compose** (for local PostgreSQL):

```yaml
version: '3.8'
services:
  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: techhub
      POSTGRES_USER: techhub
      POSTGRES_PASSWORD: localdev
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

**Test Hierarchy**:

| Test Type | Database | Sync |
| --------- | -------- | ---- |
| Unit Tests | None (mocks) | N/A |
| Integration Tests | In-memory SQLite | Seeded test data |
| Component Tests (bUnit) | None (mocks) | N/A |
| E2E Tests | Docker PostgreSQL | Full sync |
| PowerShell Tests | None | N/A |

## Key Entities

### SearchRequest

```csharp
public record SearchRequest
{
    public string? Query { get; init; }
    public IReadOnlyList<string>? Tags { get; init; }
    public IReadOnlyList<string>? Sections { get; init; }
    public IReadOnlyList<string>? Collections { get; init; }
    public DateTimeOffset? DateFrom { get; init; }
    public DateTimeOffset? DateTo { get; init; }
    public bool UseSemanticSearch { get; init; }
    public int Take { get; init; } = 20;
    public string OrderBy { get; init; } = "date_desc";  // Default: newest first
    
    // Cursor-based pagination for infinite scroll (see 003-infinite-scroll spec)
    public string? ContinuationToken { get; init; }  // Encoded cursor for next batch
}
```

### FacetResults

```csharp
public record FacetResults
{
    public required IReadOnlyDictionary<string, IReadOnlyList<FacetValue>> Facets { get; init; }
    public required long TotalCount { get; init; }
}

public record FacetValue
{
    public required string Value { get; init; }
    public required long Count { get; init; }
}
```

### SearchResults

```csharp
public record SearchResults<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required long TotalCount { get; init; }
    public required FacetResults Facets { get; init; }
    public string? ContinuationToken { get; init; }
}
```

### SearchResultItem (Simplified)

```csharp
/// <summary>
/// Represents a single search result item.
/// Navigation: videos/roundups use internal URL, others use external_url.
/// </summary>
public record SearchResultItem
{
    public required string Id { get; init; }           // Slug
    public required string Title { get; init; }
    public required string Excerpt { get; init; }      // Summary snippet
    public required IReadOnlyList<string> Authors { get; init; }  // Can have multiple authors
    public required long DateEpoch { get; init; }      // Single date field
    public required string CollectionName { get; init; }
    public required bool IsInternalCollection { get; init; }  // true for videos, roundups
    public required IReadOnlyList<string> SectionNames { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }
    public string? ExternalUrl { get; init; }          // Original source (NULL only for roundups)
    public double? RelevanceScore { get; init; }       // Full-text search rank
    public string? HighlightedExcerpt { get; init; }   // With <mark> tags
}
```

**Navigation Logic** (handled by frontend):

```csharp
// In Blazor component or service
public string GetNavigationUrl(SearchResultItem item, string currentSection)
{
    if (item.IsInternalCollection)
    {
        // Internal: /ai/videos/my-video-slug
        return $"/{currentSection}/{item.CollectionName}/{item.Id}";
    }
    // External: original source URL
    return item.ExternalUrl ?? throw new InvalidOperationException(
        $"External URL required for collection '{item.CollectionName}'");
}

public string GetLinkTarget(SearchResultItem item) 
    => item.IsInternalCollection ? "_self" : "_blank";
```

## Success Criteria

1. **Tag filtering with counts completes in under 200ms** for 4000+ articles
2. **Facet counts are 100% accurate** using PostgreSQL GROUP BY aggregations
3. **Tag subset matching works correctly** with word-boundary rules via expanded tags table
4. **Full-text search returns relevant results** with highlighted excerpts using tsvector
5. **First-run sync completes in under 60 seconds** for 4000+ markdown files
6. **Subsequent startup with no changes completes in under 1 second** (hash check only)
7. **Sync can be skipped entirely** via `ContentSync:Enabled = false` for fast local dev
8. **All unit/integration tests pass without any database** (mocks/in-memory)
9. **E2E tests pass using Docker PostgreSQL** with full sync
10. **Related articles show tag-overlap similarity** (semantic similarity in Phase 2)

## Dependencies

**Phase 1 (PostgreSQL)**:

- **PostgreSQL 16+** - Primary database (Azure PostgreSQL Flexible Server in cloud)
- **Npgsql** - .NET PostgreSQL driver
- **EF Core** - ORM with PostgreSQL provider (optional, may use Dapper for performance)
- **SQLite** - Local development fallback
- **Docker** - Local PostgreSQL container (optional)

**Phase 2 (AI Search - Future)**:

- **Azure AI Search** - Semantic search service (synced from PostgreSQL)
- **Azure OpenAI** - Embedding generation for vector search
- **Azure.Search.Documents** - .NET SDK for Azure AI Search
- **Microsoft Agent Framework** - For AI agent integration (MCP server)

**Cost Estimates**:

| Environment | Phase 1 Cost | Phase 2 Additional |
| ----------- | ------------ | ------------------ |
| Local Dev | $0 (SQLite/Docker) | N/A |
| CI/CD | $0 (Docker) | N/A |
| Production | ~$15/mo (Azure PostgreSQL Basic) | +$75/mo (AI Search Basic) |

## Assumptions

1. PostgreSQL full-text search is sufficient for current search needs (semantic search deferred to Phase 2)
2. SQLite provides adequate local development experience for most scenarios
3. Hash-based change detection is reliable for incremental sync
4. Content updates are infrequent enough that startup sync is acceptable
5. 4000+ articles fit comfortably in PostgreSQL with proper indexing
6. Docker is available for local PostgreSQL when needed
7. Azure PostgreSQL Flexible Server Basic tier is sufficient for production workload

## Out of Scope

1. Semantic/vector search (deferred to Phase 2 with Azure AI Search)
2. Real-time content synchronization (startup sync is acceptable)
3. Graph-based relationship modeling (future feature)
4. Multi-language content support
5. Image/video content analysis (text metadata only)
6. User personalization / recommendation engine
7. MCP server implementation (deferred to Phase 2)

## Risks & Mitigations

| Risk | Impact | Mitigation |
| ---- | ------ | ---------- |
| PostgreSQL full-text search quality insufficient | Medium | Can upgrade to Phase 2 (Azure AI Search) for better relevance |
| First-run sync too slow | Medium | Progress logging, async background sync option |
| SQLite limitations for local dev | Low | Docker PostgreSQL available, feature flags for advanced queries |
| Hash collision in change detection | Very Low | Use SHA256 (collision practically impossible) |
| Breaking changes to existing filtering | High | Parallel implementation, feature flags |
| Database migration complexity | Medium | EF Core migrations, versioned schema |

## Implementation Phases

### Phase 1: PostgreSQL Foundation (This Spec)

- PostgreSQL database schema design
- EF Core / Dapper repository implementations
- Incremental content sync service with hash-based change detection
- Skip sync option via appsettings
- SQLite fallback for local development
- Full-text search with tsvector
- Faceted navigation with GROUP BY
- Tag subset matching with expanded tags table
- Docker Compose for local PostgreSQL

### Phase 2: AI Search Enhancement (Separate Spec)

- Azure AI Search as secondary index
- PostgreSQL → AI Search sync pipeline
- Semantic/vector search with embeddings
- Hybrid search (PostgreSQL + AI Search)
- MCP server for AI agents
- Related articles using vector similarity

## Clarifications

### Session 2026-01-20

- Q: When content sync fails mid-way (corrupted file, DB connection lost), what should happen? → A: Transactional rollback + fail-fast startup (app won't start, immediate visibility)
- Q: How should pagination work for search results? → A: Infinite scroll (see [003-infinite-scroll spec](../003-infinite-scroll/spec.md))
- Q: What should be the default sort order? → A: Date descending (newest first) for all lists
