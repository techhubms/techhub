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
    is_coding INTEGER NOT NULL DEFAULT 0,
    is_devops INTEGER NOT NULL DEFAULT 0,
    is_github_copilot INTEGER NOT NULL DEFAULT 0,
    is_ml INTEGER NOT NULL DEFAULT 0,
    is_security INTEGER NOT NULL DEFAULT 0,
    
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
    is_coding INTEGER NOT NULL DEFAULT 0,
    is_devops INTEGER NOT NULL DEFAULT 0,
    is_github_copilot INTEGER NOT NULL DEFAULT 0,
    is_ml INTEGER NOT NULL DEFAULT 0,
    is_security INTEGER NOT NULL DEFAULT 0,
    
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

-- Indexes for common query patterns
-- Note: primary_section_name is display-only, queries filter by is_* booleans
CREATE INDEX IF NOT EXISTS idx_content_hash ON content_items(content_hash);
CREATE INDEX IF NOT EXISTS idx_content_collection_date ON content_items(collection_name, draft, date_epoch DESC);
CREATE INDEX IF NOT EXISTS idx_content_draft_date ON content_items(draft, date_epoch DESC);

-- Covering index for homepage tag cloud (all sections)
-- Includes tags_csv to avoid table lookups - 5x performance improvement (350ms → 70ms)
-- Partial index WHERE draft = 0 (99% of data) - more selective than including draft as a column
-- Tradeoff: Slower writes (acceptable - content sync is infrequent), larger index (~1-2MB)
CREATE INDEX IF NOT EXISTS idx_draft_date_tags_covering ON content_items(date_epoch DESC, tags_csv)
    WHERE draft = 0;

-- Section partial indexes for browsing (no tags_csv needed - not used for tag clouds)
-- These support GetBySectionAsync and SearchAsync with section filters
CREATE INDEX IF NOT EXISTS idx_section_ai_date ON content_items(date_epoch DESC)
    WHERE is_ai = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_azure_date ON content_items(date_epoch DESC)
    WHERE is_azure = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_coding_date ON content_items(date_epoch DESC)
    WHERE is_coding = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_devops_date ON content_items(date_epoch DESC)
    WHERE is_devops = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_github_copilot_date ON content_items(date_epoch DESC)
    WHERE is_github_copilot = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_ml_date ON content_items(date_epoch DESC)
    WHERE is_ml = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_security_date ON content_items(date_epoch DESC)
    WHERE is_security = 1 AND draft = 0;

-- Section + collection partial indexes for tag cloud queries
-- Include tags_csv for covering index optimization (GetTagCountsAsync with section+collection)
CREATE INDEX IF NOT EXISTS idx_section_ai_collection_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE is_ai = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_azure_collection_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE is_azure = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_coding_collection_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE is_coding = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_devops_collection_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE is_devops = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_github_copilot_collection_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE is_github_copilot = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_ml_collection_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE is_ml = 1 AND draft = 0;
CREATE INDEX IF NOT EXISTS idx_section_security_collection_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE is_security = 1 AND draft = 0;

-- Collection index for tag cloud queries (no section filter)
-- Includes tags_csv for covering index optimization (GetTagCountsAsync with collection-only)
CREATE INDEX IF NOT EXISTS idx_collection_date_tags ON content_items(collection_name, date_epoch DESC, tags_csv)
    WHERE draft = 0;

-- Expanded tags indexes
-- General index for tags-only queries (no section filter, no collection filter)
CREATE INDEX IF NOT EXISTS idx_tags_date ON content_tags_expanded(tag_word, date_epoch DESC);

-- Collection-scoped tag indexes (tag filtering within specific collection, no section)
CREATE INDEX IF NOT EXISTS idx_tags_collection ON content_tags_expanded(tag_word, collection_name, date_epoch DESC);

-- Partial indexes for section-filtered tag queries (tag cloud, tag filtering)
CREATE INDEX IF NOT EXISTS idx_tags_section_ai ON content_tags_expanded(tag_word, date_epoch DESC) 
    WHERE is_ai = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_azure ON content_tags_expanded(tag_word, date_epoch DESC) 
    WHERE is_azure = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_coding ON content_tags_expanded(tag_word, date_epoch DESC) 
    WHERE is_coding = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_devops ON content_tags_expanded(tag_word, date_epoch DESC) 
    WHERE is_devops = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_github_copilot ON content_tags_expanded(tag_word, date_epoch DESC) 
    WHERE is_github_copilot = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_ml ON content_tags_expanded(tag_word, date_epoch DESC) 
    WHERE is_ml = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_security ON content_tags_expanded(tag_word, date_epoch DESC) 
    WHERE is_security = 1;

-- Section + collection tag indexes (most specific tag filtering)
CREATE INDEX IF NOT EXISTS idx_tags_section_collection_ai ON content_tags_expanded(tag_word, collection_name, date_epoch DESC)
    WHERE is_ai = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_collection_azure ON content_tags_expanded(tag_word, collection_name, date_epoch DESC)
    WHERE is_azure = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_collection_coding ON content_tags_expanded(tag_word, collection_name, date_epoch DESC)
    WHERE is_coding = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_collection_devops ON content_tags_expanded(tag_word, collection_name, date_epoch DESC)
    WHERE is_devops = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_collection_github_copilot ON content_tags_expanded(tag_word, collection_name, date_epoch DESC)
    WHERE is_github_copilot = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_collection_ml ON content_tags_expanded(tag_word, collection_name, date_epoch DESC)
    WHERE is_ml = 1;
CREATE INDEX IF NOT EXISTS idx_tags_section_collection_security ON content_tags_expanded(tag_word, collection_name, date_epoch DESC)
    WHERE is_security = 1;

-- Content lookup index for retrieving tags by content item
CREATE INDEX IF NOT EXISTS idx_tags_content_lookup ON content_tags_expanded(collection_name, slug, tag_word);
