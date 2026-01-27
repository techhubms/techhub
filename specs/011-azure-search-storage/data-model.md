# Data Model: PostgreSQL Storage & Search Architecture

**Feature**: PostgreSQL Storage & Search Architecture  
**Branch**: `011-azure-search-storage`  
**Date**: 2026-01-27

## Overview

This document defines the complete database schema for Tech Hub's PostgreSQL-based content storage and search system. The schema directly maps to `ContentItemDto` fields and supports fast faceted navigation, full-text search, tag subset matching, and efficient querying.

## Core Principles

1. **Markdown as source of truth** - Database is a derived store, rebuilt from Git
2. **Junction tables for many-to-many** - Tags, sections, and plans use junction pattern
3. **Hash-based change detection** - SHA256 for incremental sync
4. **Generated search vectors** - Automatic tsvector maintenance
5. **Proper indexing** - All filter columns and foreign keys indexed

## Entity Relationship Diagram

```text
┌─────────────────────────────────────────────────────────────────────┐
│                         CONTENT STORAGE                              │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌────────────────┐                                                 │
│  │ content_items  │◄────────┐                                       │
│  ├────────────────┤          │                                       │
│  │ id (PK)        │          │                                       │
│  │ title          │          │ Many-to-Many Relationships:          │
│  │ content        │          │                                       │
│  │ excerpt        │          ├──────┐ content_tags                  │
│  │ date_epoch     │──────────┤      │ - content_id (FK)             │
│  │ collection_name│          │      │ - tag                         │
│  │ external_url   │          │      │ - tag_normalized              │
│  │ content_hash   │          │                                       │
│  │ search_vector  │          ├──────┐ content_tags_expanded         │
│  └────────────────┘          │      │ - content_id (FK)             │
│                              │      │ - tag_word                    │
│                              │                                       │
│  ┌────────────────┐          ├──────┐ content_sections              │
│  │ collections    │          │      │ - content_id (FK)             │
│  ├────────────────┤          │      │ - section_name                │
│  │ id (PK)        │          │                                       │
│  │ name (UNIQUE)  │          ├──────┐ content_plans                 │
│  │ is_internal    │          │      │ - content_id (FK)             │
│  │ parent_name    │          │      │ - plan_name                   │
│  └────────────────┘          │                                       │
│                              │                                       │
│  ┌────────────────┐          │                                       │
│  │ sync_metadata  │          │                                       │
│  ├────────────────┤          │                                       │
│  │ key (PK)       │          │                                       │
│  │ value          │          │                                       │
│  │ updated_at     │          │                                       │
│  └────────────────┘          │                                       │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

## Table Definitions

### content_items

Primary content storage table. Maps directly to `ContentItemDto`.

```sql
CREATE TABLE content_items (
    -- Primary identification
    id                      TEXT PRIMARY KEY,           -- Slug (e.g., "2024-06-19-copilot-tips")
    
    -- Content fields (stored as markdown)
    title                   TEXT NOT NULL,
    content                 TEXT NOT NULL,              -- Full markdown (rendered server-side)
    excerpt                 TEXT,                       -- Markdown summary (rendered server-side)
    
    -- Metadata
    date_epoch              BIGINT NOT NULL,            -- Unix timestamp
    collection_name         TEXT NOT NULL,              -- "videos", "ghc-features", "blogs", etc.
    subcollection_name      TEXT,                       -- Optional subfolder identifier
    primary_section_name    TEXT NOT NULL,              -- First section for navigation URLs
    external_url            TEXT,                       -- Original source (NULL only for roundups)
    author                  TEXT,                       -- Single string (multiple authors deferred)
    feed_name               TEXT,                       -- RSS source attribution
    
    -- Flags
    ghes_support            BOOLEAN NOT NULL DEFAULT FALSE,
    draft                   BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- Sync tracking
    content_hash            TEXT NOT NULL,              -- SHA256 for change detection
    created_at              TIMESTAMPTZ DEFAULT NOW(),
    updated_at              TIMESTAMPTZ DEFAULT NOW(),
    
    -- Full-text search (PostgreSQL only, omitted in SQLite)
    search_vector           TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C')
    ) STORED
);
```

**Field Mapping to ContentItemDto**:

| ContentItemDto Field | Database Storage | Notes |
|---------------------|------------------|-------|
| `Slug` | `id` | Primary key |
| `Title` | `title` | Full-text searchable |
| `Content` | `content` | Full markdown |
| `Excerpt` | `excerpt` | Markdown summary |
| `RenderedHtml` | *(computed)* | Rendered server-side, not stored |
| `DateEpoch` | `date_epoch` | Unix timestamp |
| `DateIso` | *(computed)* | Derived from `date_epoch` |
| `CollectionName` | `collection_name` | "videos", "ghc-features", etc. |
| `SubcollectionName` | `subcollection_name` | Optional |
| `FeedName` | `feed_name` | RSS attribution |
| `SectionNames` | `content_sections` junction | Many-to-many |
| `PrimarySectionName` | `primary_section_name` | First section |
| `Tags` | `content_tags` junction | Many-to-many |
| `ExternalUrl` | `external_url` | NULL for roundups only |
| `Url` | *(computed)* | Built from collection/section/slug |
| `Plans` | `content_plans` junction | Many-to-many |
| `GhesSupport` | `ghes_support` | Boolean flag |
| `Draft` | `draft` | Boolean flag |
| `GhcFeature` | *(derived)* | `collection_name = 'ghc-features'` |

---

### collections

Lookup table for collection metadata and query optimization.

```sql
CREATE TABLE collections (
    id              SERIAL PRIMARY KEY,
    name            TEXT NOT NULL UNIQUE,               -- e.g., "videos", "ghc-features"
    is_internal     BOOLEAN NOT NULL DEFAULT FALSE,    -- true for videos/roundups
    parent_name     TEXT                                -- "videos" for ghc-features/vscode-updates
);

-- Pre-populated data
INSERT INTO collections (name, is_internal, parent_name) VALUES
    ('news', false, NULL),
    ('blogs', false, NULL),
    ('community', false, NULL),
    ('videos', true, NULL),
    ('ghc-features', true, 'videos'),
    ('vscode-updates', true, 'videos'),
    ('roundups', true, NULL),
    ('custom', true, NULL);
```

**Navigation Logic**:

- `is_internal = true` → Use internal URL (`/{section}/{collection}/{slug}`)
- `is_internal = false` → Use `external_url` with `target="_blank"`

---

### content_tags

Junction table for content-to-tag relationships (many-to-many).

```sql
CREATE TABLE content_tags (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    tag             TEXT NOT NULL,                      -- Original tag (e.g., "Azure AI")
    tag_normalized  TEXT NOT NULL,                      -- Lowercase, trimmed
    PRIMARY KEY (content_id, tag)
);
```

**Example Data**:

```text
content_id               | tag               | tag_normalized
-------------------------+-------------------+------------------
2024-06-19-copilot-tips | Azure AI          | azure ai
2024-06-19-copilot-tips | GitHub Copilot    | github copilot
```

---

### content_tags_expanded

Expanded tags for subset matching (e.g., "Azure AI" → "azure", "ai").

```sql
CREATE TABLE content_tags_expanded (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    tag_word        TEXT NOT NULL,                      -- Individual word (e.g., "azure")
    PRIMARY KEY (content_id, tag_word)
);
```

**Example Data** (from "Azure AI", "GitHub Copilot"):

```text
content_id               | tag_word
-------------------------+----------
2024-06-19-copilot-tips | azure
2024-06-19-copilot-tips | ai
2024-06-19-copilot-tips | github
2024-06-19-copilot-tips | copilot
```

**Subset Matching Query**:

```sql
-- Find content with "ai" anywhere in tags
SELECT DISTINCT c.*
FROM content_items c
JOIN content_tags_expanded e ON c.id = e.content_id
WHERE e.tag_word = 'ai';  -- Matches "AI", "Azure AI", "Generative AI"
```

---

### content_sections

Junction table for content-to-section relationships (many-to-many).

```sql
CREATE TABLE content_sections (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    section_name    TEXT NOT NULL,                      -- e.g., "ai", "github-copilot"
    PRIMARY KEY (content_id, section_name)
);
```

---

### content_plans

Junction table for GitHub Copilot plan tiers (many-to-many).

```sql
CREATE TABLE content_plans (
    content_id      TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    plan_name       TEXT NOT NULL,                      -- "Free", "Pro", "Business", "Enterprise"
    PRIMARY KEY (content_id, plan_name)
);
```

---

### sync_metadata

Tracks sync state for audit and debugging.

```sql
CREATE TABLE sync_metadata (
    key             TEXT PRIMARY KEY,
    value           TEXT NOT NULL,
    updated_at      TIMESTAMPTZ DEFAULT NOW()
);

-- Example data
INSERT INTO sync_metadata (key, value) VALUES
    ('last_sync', '2026-01-27T10:30:00Z'),
    ('total_items', '4013'),
    ('sync_duration_ms', '45230');
```

---

## Indexes

Performance-critical indexes for all filter columns and foreign keys.

```sql
-- Content items indexes
CREATE INDEX idx_content_date ON content_items(date_epoch DESC);
CREATE INDEX idx_content_collection ON content_items(collection_name);
CREATE INDEX idx_content_draft ON content_items(draft) WHERE draft = true;
CREATE INDEX idx_content_search ON content_items USING GIN(search_vector);
CREATE INDEX idx_content_hash ON content_items(content_hash);

-- Collections indexes
CREATE INDEX idx_collections_name ON collections(name);
CREATE INDEX idx_collections_parent ON collections(parent_name);

-- Tags indexes
CREATE INDEX idx_tags_normalized ON content_tags(tag_normalized);
CREATE INDEX idx_tags_content ON content_tags(content_id);

-- Expanded tags indexes
CREATE INDEX idx_tags_expanded_word ON content_tags_expanded(tag_word);
CREATE INDEX idx_tags_expanded_content ON content_tags_expanded(content_id);

-- Sections indexes
CREATE INDEX idx_sections_name ON content_sections(section_name);
CREATE INDEX idx_sections_content ON content_sections(content_id);

-- Plans indexes
CREATE INDEX idx_plans_name ON content_plans(plan_name);
CREATE INDEX idx_plans_content ON content_plans(content_id);
```

---

## SQLite Variant (Local Development)

SQLite schema differs in full-text search implementation (uses FTS5).

```sql
-- Main table (same structure, minus search_vector)
CREATE TABLE content_items (
    id TEXT PRIMARY KEY,
    title TEXT NOT NULL,
    content TEXT NOT NULL,
    excerpt TEXT,
    date_epoch INTEGER NOT NULL,
    collection_name TEXT NOT NULL,
    subcollection_name TEXT,
    primary_section_name TEXT NOT NULL,
    external_url TEXT,
    author TEXT,
    feed_name TEXT,
    ghes_support INTEGER NOT NULL DEFAULT 0,  -- SQLite uses INTEGER for boolean
    draft INTEGER NOT NULL DEFAULT 0,
    content_hash TEXT NOT NULL,
    created_at TEXT DEFAULT (datetime('now')),
    updated_at TEXT DEFAULT (datetime('now'))
);

-- FTS5 virtual table for full-text search
CREATE VIRTUAL TABLE content_fts USING fts5(
    id, title, excerpt, content,
    content='content_items',
    content_rowid='rowid'
);

-- Triggers to maintain FTS index
CREATE TRIGGER content_ai AFTER INSERT ON content_items BEGIN
    INSERT INTO content_fts(rowid, id, title, excerpt, content)
    VALUES (new.rowid, new.id, new.title, new.excerpt, new.content);
END;

CREATE TRIGGER content_ad AFTER DELETE ON content_items BEGIN
    INSERT INTO content_fts(content_fts, rowid, id, title, excerpt, content)
    VALUES ('delete', old.rowid, old.id, old.title, old.excerpt, old.content);
END;

CREATE TRIGGER content_au AFTER UPDATE ON content_items BEGIN
    INSERT INTO content_fts(content_fts, rowid, id, title, excerpt, content)
    VALUES ('delete', old.rowid, old.id, old.title, old.excerpt, old.content);
    INSERT INTO content_fts(rowid, id, title, excerpt, content)
    VALUES (new.rowid, new.id, new.title, new.excerpt, new.content);
END;
```

---

## Query Patterns

### Faceted Navigation

Get tag counts for filtered content:

```sql
SELECT t.tag, COUNT(DISTINCT t.content_id) as count
FROM content_tags t
JOIN content_sections s ON t.content_id = s.content_id
WHERE s.section_name = 'ai'
  AND t.content_id IN (
      SELECT content_id FROM content_items WHERE draft = false
  )
GROUP BY t.tag
ORDER BY count DESC
LIMIT 50;
```

### Tag AND Logic

Find content matching ALL selected tags:

```sql
SELECT c.*
FROM content_items c
WHERE c.id IN (
    SELECT content_id
    FROM content_tags_expanded
    WHERE tag_word IN ('ai', 'azure', 'copilot')
    GROUP BY content_id
    HAVING COUNT(DISTINCT tag_word) = 3  -- Must match all 3 tags
);
```

### Full-Text Search with Ranking

```sql
SELECT 
    c.*,
    ts_rank(c.search_vector, query) AS rank,
    ts_headline('english', c.excerpt, query, 'StartSel=<mark>, StopSel=</mark>') AS highlighted
FROM content_items c,
     plainto_tsquery('english', 'agent framework') AS query
WHERE c.search_vector @@ query
  AND c.draft = false
ORDER BY rank DESC
LIMIT 20;
```

### Keyset Pagination

```sql
-- First page
SELECT * FROM content_items
WHERE section_name = 'ai' AND draft = false
ORDER BY date_epoch DESC, id DESC
LIMIT 20;

-- Subsequent pages
SELECT * FROM content_items
WHERE section_name = 'ai' 
  AND draft = false
  AND (date_epoch, id) < (1706371200, 'cursor-id-here')
ORDER BY date_epoch DESC, id DESC
LIMIT 20;
```

---

## Size Estimates

| Component | 4K Items | 6K Items | 10K Items |
|-----------|----------|----------|-----------|
| Content table | ~20 MB | ~30 MB | ~50 MB |
| Tag junctions | ~3 MB | ~4.5 MB | ~7.5 MB |
| Expanded tags | ~2.5 MB | ~4 MB | ~6 MB |
| Section junctions | ~0.3 MB | ~0.5 MB | ~0.8 MB |
| Full-text index | ~6 MB | ~9 MB | ~15 MB |
| B-tree indexes | ~2 MB | ~3 MB | ~5 MB |
| **Total** | **~33 MB** | **~50 MB** | **~84 MB** |

**Azure PostgreSQL Basic tier (5GB storage)** is sufficient for 50,000+ items.
