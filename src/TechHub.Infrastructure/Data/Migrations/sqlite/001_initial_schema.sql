-- SQLite Schema
-- TechHub Content Database
-- Last Updated: 2026-01-30
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
    date_epoch INTEGER NOT NULL,
    subcollection_name TEXT,              -- Optional: only used by ghc-features
    primary_section_name TEXT NOT NULL,
    external_url TEXT NOT NULL,           -- Required: validated for all collections
    author TEXT NOT NULL,                 -- Required: "Content author cannot be empty"
    feed_name TEXT NOT NULL,              -- Required: "Feed name cannot be empty"
    ghes_support INTEGER NOT NULL DEFAULT 0,
    draft INTEGER NOT NULL DEFAULT 0,
    plans TEXT,                           -- Optional: only used by ghc-features
    content_hash TEXT NOT NULL,
    
    -- Denormalized tags (required - SetTags requires at least one tag)
    tags_csv TEXT NOT NULL,               -- Format: ",AI,GitHub Copilot,DevOps,"
    
    -- Denormalized section booleans (zero-join filtering)
    is_ai INTEGER NOT NULL DEFAULT 0,
    is_azure INTEGER NOT NULL DEFAULT 0,
    is_dotnet INTEGER NOT NULL DEFAULT 0,
    is_devops INTEGER NOT NULL DEFAULT 0,
    is_github_copilot INTEGER NOT NULL DEFAULT 0,
    is_ml INTEGER NOT NULL DEFAULT 0,
    is_security INTEGER NOT NULL DEFAULT 0,
    
    -- Bitmask for sections (optimized filtering)
    -- Bit 0 (1): AI, Bit 1 (2): Azure, Bit 2 (4): .NET, Bit 3 (8): DevOps
    -- Bit 4 (16): GitHub Copilot, Bit 5 (32): ML, Bit 6 (64): Security
    sections_bitmask INTEGER NOT NULL DEFAULT 0,
    
    -- Timestamps
    created_at TEXT DEFAULT (datetime('now')),
    updated_at TEXT DEFAULT (datetime('now')),
    
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
    date_epoch INTEGER NOT NULL,
    is_ai INTEGER NOT NULL DEFAULT 0,
    is_azure INTEGER NOT NULL DEFAULT 0,
    is_dotnet INTEGER NOT NULL DEFAULT 0,
    is_devops INTEGER NOT NULL DEFAULT 0,
    is_github_copilot INTEGER NOT NULL DEFAULT 0,
    is_ml INTEGER NOT NULL DEFAULT 0,
    is_security INTEGER NOT NULL DEFAULT 0,
    
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
    updated_at TEXT DEFAULT (datetime('now'))
);

-- ========================================
-- Full-Text Search (FTS5)
-- ========================================
CREATE VIRTUAL TABLE IF NOT EXISTS content_fts USING fts5(
    slug, title, excerpt, content,
    content='content_items',
    content_rowid='rowid'
);

-- ========================================
-- FTS Triggers
-- ========================================
CREATE TRIGGER IF NOT EXISTS content_ai AFTER INSERT ON content_items BEGIN
    INSERT INTO content_fts(rowid, slug, title, excerpt, content)
    VALUES (new.rowid, new.slug, new.title, new.excerpt, new.content);
END;

CREATE TRIGGER IF NOT EXISTS content_ad AFTER DELETE ON content_items BEGIN
    INSERT INTO content_fts(content_fts, rowid, slug, title, excerpt, content)
    VALUES ('delete', old.rowid, old.slug, old.title, old.excerpt, old.content);
END;

CREATE TRIGGER IF NOT EXISTS content_au AFTER UPDATE ON content_items BEGIN
    INSERT INTO content_fts(content_fts, rowid, slug, title, excerpt, content)
    VALUES ('delete', old.rowid, old.slug, old.title, old.excerpt, old.content);
    INSERT INTO content_fts(rowid, slug, title, excerpt, content)
    VALUES (new.rowid, new.slug, new.title, new.excerpt, new.content);
END;

-- ========================================
-- Indexes
-- ========================================

CREATE INDEX IF NOT EXISTS idx_content_hash ON content_items(content_hash);

-- content_items indexes - Support various query patterns
-- 1. For queries with collection + section + date (e.g., section pages with collection filter)
CREATE INDEX IF NOT EXISTS idx_items_collection_date_sections ON content_items(
    collection_name,
    date_epoch DESC,
    sections_bitmask,
    tags_csv,
    slug
) WHERE draft = 0;

-- 2. For queries with section + date only (e.g., GetBySectionAsync without collection filter)
CREATE INDEX IF NOT EXISTS idx_items_sections_date ON content_items(
    sections_bitmask,
    date_epoch DESC,
    tags_csv,
    slug
) WHERE draft = 0;

-- 3. For queries with date only (e.g., GetAllAsync, no section or collection filter)
CREATE INDEX IF NOT EXISTS idx_items_date ON content_items(
    date_epoch DESC,
    tags_csv,
    slug
) WHERE draft = 0;

-- content_tags_expanded indexes - Support various tag query patterns
-- All indexes include collection_name and slug to eliminate temp B-trees for DISTINCT operations
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