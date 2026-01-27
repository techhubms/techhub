-- PostgreSQL Initial Schema
-- Feature: PostgreSQL Storage & Search Architecture
-- Date: 2026-01-27
-- Note: PostgreSQL variant uses tsvector for full-text search

-- Main content table
CREATE TABLE IF NOT EXISTS content_items (
    id TEXT PRIMARY KEY,
    title TEXT NOT NULL,
    content TEXT NOT NULL,
    excerpt TEXT,
    date_epoch BIGINT NOT NULL,
    collection_name TEXT NOT NULL,
    subcollection_name TEXT,
    primary_section_name TEXT NOT NULL,
    external_url TEXT,
    author TEXT,
    feed_name TEXT,
    ghes_support BOOLEAN NOT NULL DEFAULT FALSE,
    draft BOOLEAN NOT NULL DEFAULT FALSE,
    content_hash TEXT NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    
    -- Full-text search vector (generated column)
    search_vector TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C')
    ) STORED
);

-- Collections lookup table
CREATE TABLE IF NOT EXISTS collections (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    is_internal BOOLEAN NOT NULL DEFAULT FALSE,
    parent_name TEXT
);

-- Pre-populate collections
INSERT INTO collections (name, is_internal, parent_name) VALUES
    ('news', false, NULL),
    ('blogs', false, NULL),
    ('community', false, NULL),
    ('videos', true, NULL),
    ('ghc-features', true, 'videos'),
    ('vscode-updates', true, 'videos'),
    ('roundups', true, NULL),
    ('custom', true, NULL)
ON CONFLICT (name) DO NOTHING;

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
    updated_at TIMESTAMPTZ DEFAULT NOW()
);

-- Indexes
CREATE INDEX IF NOT EXISTS idx_content_date ON content_items(date_epoch DESC);
CREATE INDEX IF NOT EXISTS idx_content_collection ON content_items(collection_name);
CREATE INDEX IF NOT EXISTS idx_content_draft ON content_items(draft) WHERE draft = true;
CREATE INDEX IF NOT EXISTS idx_content_search ON content_items USING GIN(search_vector);
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
