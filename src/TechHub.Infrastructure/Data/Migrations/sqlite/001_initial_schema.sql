-- SQLite Initial Schema
-- Feature: PostgreSQL Storage & Search Architecture
-- Date: 2026-01-27
-- Note: SQLite variant uses FTS5 for full-text search instead of tsvector

-- Main content table
CREATE TABLE IF NOT EXISTS content_items (
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
    ghes_support INTEGER NOT NULL DEFAULT 0,
    draft INTEGER NOT NULL DEFAULT 0,
    content_hash TEXT NOT NULL,
    created_at TEXT DEFAULT (datetime('now')),
    updated_at TEXT DEFAULT (datetime('now'))
);

-- Collections lookup table
CREATE TABLE IF NOT EXISTS collections (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL UNIQUE,
    is_internal INTEGER NOT NULL DEFAULT 0,
    parent_name TEXT
);

-- Pre-populate collections
INSERT OR IGNORE INTO collections (name, is_internal, parent_name) VALUES
    ('news', 0, NULL),
    ('blogs', 0, NULL),
    ('community', 0, NULL),
    ('videos', 1, NULL),
    ('ghc-features', 1, 'videos'),
    ('vscode-updates', 1, 'videos'),
    ('roundups', 1, NULL),
    ('custom', 1, NULL);

-- Tags junction table
CREATE TABLE IF NOT EXISTS content_tags (
    content_id TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    tag TEXT NOT NULL,
    tag_normalized TEXT NOT NULL,
    PRIMARY KEY (content_id, tag)
);

-- Expanded tags for subset matching
CREATE TABLE IF NOT EXISTS content_tags_expanded (
    content_id TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    tag_word TEXT NOT NULL,
    PRIMARY KEY (content_id, tag_word)
);

-- Section names junction table
CREATE TABLE IF NOT EXISTS content_sections (
    content_id TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    section_name TEXT NOT NULL,
    PRIMARY KEY (content_id, section_name)
);

-- Plans junction table
CREATE TABLE IF NOT EXISTS content_plans (
    content_id TEXT NOT NULL REFERENCES content_items(id) ON DELETE CASCADE,
    plan_name TEXT NOT NULL,
    PRIMARY KEY (content_id, plan_name)
);

-- Sync metadata
CREATE TABLE IF NOT EXISTS sync_metadata (
    key TEXT PRIMARY KEY,
    value TEXT NOT NULL,
    updated_at TEXT DEFAULT (datetime('now'))
);

-- FTS5 virtual table for full-text search
CREATE VIRTUAL TABLE IF NOT EXISTS content_fts USING fts5(
    id, title, excerpt, content,
    content='content_items',
    content_rowid='rowid'
);

-- Triggers to maintain FTS index
CREATE TRIGGER IF NOT EXISTS content_ai AFTER INSERT ON content_items BEGIN
    INSERT INTO content_fts(rowid, id, title, excerpt, content)
    VALUES (new.rowid, new.id, new.title, new.excerpt, new.content);
END;

CREATE TRIGGER IF NOT EXISTS content_ad AFTER DELETE ON content_items BEGIN
    INSERT INTO content_fts(content_fts, rowid, id, title, excerpt, content)
    VALUES ('delete', old.rowid, old.id, old.title, old.excerpt, old.content);
END;

CREATE TRIGGER IF NOT EXISTS content_au AFTER UPDATE ON content_items BEGIN
    INSERT INTO content_fts(content_fts, rowid, id, title, excerpt, content)
    VALUES ('delete', old.rowid, old.id, old.title, old.excerpt, old.content);
    INSERT INTO content_fts(rowid, id, title, excerpt, content)
    VALUES (new.rowid, new.id, new.title, new.excerpt, new.content);
END;

-- Indexes
CREATE INDEX IF NOT EXISTS idx_content_date ON content_items(date_epoch DESC);
CREATE INDEX IF NOT EXISTS idx_content_collection ON content_items(collection_name);
CREATE INDEX IF NOT EXISTS idx_content_draft ON content_items(draft) WHERE draft = 1;
CREATE INDEX IF NOT EXISTS idx_content_hash ON content_items(content_hash);

CREATE INDEX IF NOT EXISTS idx_collections_name ON collections(name);
CREATE INDEX IF NOT EXISTS idx_collections_parent ON collections(parent_name);

CREATE INDEX IF NOT EXISTS idx_tags_normalized ON content_tags(tag_normalized);
CREATE INDEX IF NOT EXISTS idx_tags_content ON content_tags(content_id);

CREATE INDEX IF NOT EXISTS idx_tags_expanded_word ON content_tags_expanded(tag_word);
CREATE INDEX IF NOT EXISTS idx_tags_expanded_content ON content_tags_expanded(content_id);

CREATE INDEX IF NOT EXISTS idx_sections_name ON content_sections(section_name);
CREATE INDEX IF NOT EXISTS idx_sections_content ON content_sections(content_id);

CREATE INDEX IF NOT EXISTS idx_plans_name ON content_plans(plan_name);
CREATE INDEX IF NOT EXISTS idx_plans_content ON content_plans(content_id);
