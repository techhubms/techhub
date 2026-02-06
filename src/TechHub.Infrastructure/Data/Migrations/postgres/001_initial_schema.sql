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
-- Example: "GitHub Copilot" → rows for "github" and "copilot"
CREATE TABLE IF NOT EXISTS content_tags_expanded (
    collection_name TEXT NOT NULL,
    slug TEXT NOT NULL,
    tag_word TEXT NOT NULL,              -- Lowercase word from tag
    
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

-- PostgreSQL-specific: GIN index for full-text search (much faster than SQLite FTS5 for large datasets)
CREATE INDEX IF NOT EXISTS idx_content_search ON content_items USING GIN(search_vector);

-- content_tags_expanded indexes - Support various tag query patterns
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

-- 3. For queries with tag + date only (e.g., tag search without section or collection filter)
CREATE INDEX IF NOT EXISTS idx_tags_word_date ON content_tags_expanded(
    tag_word,
    date_epoch DESC,
    collection_name,
    slug
);
