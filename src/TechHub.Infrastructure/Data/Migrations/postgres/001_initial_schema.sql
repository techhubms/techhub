-- PostgreSQL Initial Schema
-- Feature: PostgreSQL Storage & Search Architecture
-- Date: 2026-02-02
-- Note: PostgreSQL variant uses tsvector for full-text search
--
-- Nullability constraints match ContentItem constructor validation.
-- All content items MUST have: slug, title, author, date, collection, feed, section, excerpt, external_url, tags
-- Optional: subcollection_name (only ghc-features), plans (only ghc-features)

-- ========================================
-- Main Content Table
-- ========================================
CREATE TABLE IF NOT EXISTS content_items (
    -- Primary identifiers
    slug TEXT NOT NULL,
    collection_name TEXT NOT NULL,
    
    -- Content fields (all required per ContentItem constructor)
    title TEXT NOT NULL,
    content TEXT NOT NULL,
    excerpt TEXT NOT NULL,                -- Required: ArgumentNullException.ThrowIfNull
    
    -- Metadata (all required per constructor validation)
    date_epoch BIGINT NOT NULL,
    subcollection_name TEXT,              -- Optional: only used by ghc-features
    primary_section_name TEXT NOT NULL,
    external_url TEXT NOT NULL,           -- Required: validated for all collections
    author TEXT NOT NULL,                 -- Required: "Content author cannot be empty"
    feed_name TEXT NOT NULL,              -- Required: "Feed name cannot be empty"
    ghes_support BOOLEAN NOT NULL DEFAULT FALSE,
    draft BOOLEAN NOT NULL DEFAULT FALSE,
    plans TEXT,                           -- Optional: only used by ghc-features
    content_hash TEXT NOT NULL,
    
    -- Denormalized tags (required - SetTags requires at least one tag)
    tags_csv TEXT NOT NULL,               -- Format: ",AI,GitHub Copilot,DevOps,"
    
    -- Denormalized section booleans (zero-join filtering)
    is_ai BOOLEAN NOT NULL DEFAULT FALSE,
    is_azure BOOLEAN NOT NULL DEFAULT FALSE,
    is_dotnet BOOLEAN NOT NULL DEFAULT FALSE,
    is_devops BOOLEAN NOT NULL DEFAULT FALSE,
    is_github_copilot BOOLEAN NOT NULL DEFAULT FALSE,
    is_ml BOOLEAN NOT NULL DEFAULT FALSE,
    is_security BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- Bitmask for sections (optimized filtering)
    -- Bit 0 (1): AI, Bit 1 (2): Azure, Bit 2 (4): .NET, Bit 3 (8): DevOps
    -- Bit 4 (16): GitHub Copilot, Bit 5 (32): ML, Bit 6 (64): Security
    sections_bitmask INTEGER NOT NULL DEFAULT 0,
    
    -- Timestamps
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    
    -- Full-text search vector (PostgreSQL-specific: generated column)
    search_vector TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C')
    ) STORED,
    
    PRIMARY KEY (collection_name, slug)
);

-- ========================================
-- Expanded Tags Table (Denormalized)
-- ========================================
-- Purpose: Word-level tag matching with denormalized filters
-- Example: "GitHub Copilot" tag creates rows:
--   - tag_word="github copilot", tag_display="GitHub Copilot", is_full_tag=true (actual tag)
--   - tag_word="github", tag_display=NULL, is_full_tag=false (word expansion)
--   - tag_word="copilot", tag_display=NULL, is_full_tag=false (word expansion)
-- tag_word is always lowercase for efficient querying (no LOWER() needed)
-- tag_display preserves original case only for full tags (is_full_tag=true)
CREATE TABLE IF NOT EXISTS content_tags_expanded (
    collection_name TEXT NOT NULL,
    slug TEXT NOT NULL,
    tag_word TEXT NOT NULL,              -- Lowercase for efficient querying (e.g., "github copilot")
    tag_display TEXT NOT NULL,           -- Original cased version of the word (e.g., "GitHub Copilot" for full tag, "GitHub" and "Copilot" for word expansions)
    is_full_tag BOOLEAN NOT NULL DEFAULT FALSE, -- true = actual tag, false = word expansion
    
    -- Denormalized from content_items (eliminates joins)
    date_epoch BIGINT NOT NULL,
    is_ai BOOLEAN NOT NULL DEFAULT FALSE,
    is_azure BOOLEAN NOT NULL DEFAULT FALSE,
    is_dotnet BOOLEAN NOT NULL DEFAULT FALSE,
    is_devops BOOLEAN NOT NULL DEFAULT FALSE,
    is_github_copilot BOOLEAN NOT NULL DEFAULT FALSE,
    is_ml BOOLEAN NOT NULL DEFAULT FALSE,
    is_security BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- Bitmask for sections (optimized filtering)
    -- Bit 0 (1): AI, Bit 1 (2): Azure, Bit 2 (4): .NET, Bit 3 (8): DevOps
    -- Bit 4 (16): GitHub Copilot, Bit 5 (32): ML, Bit 6 (64): Security
    sections_bitmask INTEGER NOT NULL DEFAULT 0,
    
    PRIMARY KEY (collection_name, slug, tag_word)
);

-- ========================================
-- Sync Metadata Table
-- ========================================
CREATE TABLE IF NOT EXISTS sync_metadata (
    key TEXT PRIMARY KEY,
    value TEXT NOT NULL,
    updated_at TIMESTAMPTZ DEFAULT NOW()
);

-- ========================================
-- Indexes
-- ========================================

CREATE INDEX IF NOT EXISTS idx_content_hash ON content_items(content_hash);

-- content_items indexes - Support various query patterns
-- PostgreSQL-specific: Can use partial indexes with WHERE clause
-- 1. For queries with collection + section + date (e.g., section pages with collection filter)
CREATE INDEX IF NOT EXISTS idx_items_collection_date_sections ON content_items(
    collection_name,
    date_epoch DESC,
    sections_bitmask,
    tags_csv,
    slug
) WHERE draft = FALSE;

-- 2. For queries with section + date only (e.g., GetBySectionAsync without collection filter)
CREATE INDEX IF NOT EXISTS idx_items_sections_date ON content_items(
    sections_bitmask,
    date_epoch DESC,
    tags_csv,
    slug
) WHERE draft = FALSE;

-- 3. For queries with date only (e.g., GetAllAsync, no section or collection filter)
CREATE INDEX IF NOT EXISTS idx_items_date ON content_items(
    date_epoch DESC,
    tags_csv,
    slug
) WHERE draft = FALSE;

-- GIN index for full-text search using tsvector
CREATE INDEX IF NOT EXISTS idx_content_search ON content_items USING GIN(search_vector);

-- content_tags_expanded indexes - Support various tag query patterns
-- tag_word is always lowercase, so no LOWER() function needed in indexes or queries
-- All indexes include collection_name and slug for covering index optimization
-- 1. For queries with tag + collection + section + date (e.g., tag search with all filters)
CREATE INDEX IF NOT EXISTS idx_tags_word_collection_date_sections ON content_tags_expanded(
    tag_word,
    collection_name,
    date_epoch DESC,
    sections_bitmask,
    slug
);

-- 2. For queries with tag + section + date only (e.g., tag search without collection filter)
CREATE INDEX IF NOT EXISTS idx_tags_word_sections_date ON content_tags_expanded(
    tag_word,
    sections_bitmask,
    date_epoch DESC,
    collection_name,
    slug
);

-- 3. For queries with tag + date (e.g., tag search without collection filter)
-- Includes sections_bitmask so bitmask filters don't require table lookups
CREATE INDEX IF NOT EXISTS idx_tags_word_date ON content_tags_expanded(
    tag_word,
    date_epoch DESC,
    sections_bitmask,
    collection_name,
    slug
);

-- 4. Covering index for tag cloud queries filtered by collection.
-- The single-pass tag cloud query groups by tag_word with collection_name equality filter.
-- Leading collection_name enables index seek; tag_word second enables GroupAggregate.
-- Including sections_bitmask and tag_display makes it a covering index (Index Only Scan,
-- 0 heap fetches) — all data comes from the index without touching the table.
CREATE INDEX IF NOT EXISTS idx_tags_collection_tagword ON content_tags_expanded(
    collection_name, tag_word, sections_bitmask, tag_display
);

-- 5. Covering index for tag cloud queries WITHOUT collection filter (section-only, homepage).
-- Leading tag_word enables GroupAggregate/sorted access for GROUP BY tag_word.
-- Including sections_bitmask and tag_display makes it a covering index (Index Only Scan).
CREATE INDEX IF NOT EXISTS idx_tags_tagword_display ON content_tags_expanded(
    tag_word, sections_bitmask, tag_display
);

-- 6. Covering partial index for valid tag_words (actual tags, not word expansions).
-- Used by queries that need to find which tag_words are real tags.
-- Partial index keeps only is_full_tag=true rows (~45% of table).
-- Includes date_epoch for date range filters and tag_display for covering scans.
CREATE INDEX IF NOT EXISTS idx_tags_valid_tagwords ON content_tags_expanded(
    tag_word, sections_bitmask, collection_name, date_epoch, tag_display
) WHERE is_full_tag = TRUE;
