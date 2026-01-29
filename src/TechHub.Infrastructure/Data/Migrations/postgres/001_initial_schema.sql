-- PostgreSQL Initial Schema
-- Feature: PostgreSQL Storage & Search Architecture
-- Date: 2026-01-27
-- Note: PostgreSQL variant uses tsvector for full-text search

-- Main content table
CREATE TABLE IF NOT EXISTS content_items (
    slug TEXT NOT NULL,
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
    plans TEXT,
    content_hash TEXT NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW(),
    
    -- Full-text search vector (generated column)
    search_vector TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C')
    ) STORED,
    
    PRIMARY KEY (collection_name, slug)
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
    collection_name TEXT NOT NULL,
    slug TEXT NOT NULL,
    tag TEXT NOT NULL,
    tag_normalized TEXT NOT NULL,
    PRIMARY KEY (collection_name, slug, tag),
    FOREIGN KEY (collection_name, slug) REFERENCES content_items(collection_name, slug) ON DELETE CASCADE
);

-- Expanded tags for subset matching
CREATE TABLE IF NOT EXISTS content_tags_expanded (
    collection_name TEXT NOT NULL,
    slug TEXT NOT NULL,
    tag_word TEXT NOT NULL,
    PRIMARY KEY (collection_name, slug, tag_word),
    FOREIGN KEY (collection_name, slug) REFERENCES content_items(collection_name, slug) ON DELETE CASCADE
);

-- Section names junction table
CREATE TABLE IF NOT EXISTS content_sections (
    collection_name TEXT NOT NULL,
    slug TEXT NOT NULL,
    section_name TEXT NOT NULL,
    PRIMARY KEY (collection_name, slug, section_name),
    FOREIGN KEY (collection_name, slug) REFERENCES content_items(collection_name, slug) ON DELETE CASCADE
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
CREATE INDEX IF NOT EXISTS idx_tags_content ON content_tags(collection_name, slug);
CREATE INDEX IF NOT EXISTS idx_tags_tag_covering ON content_tags(tag, collection_name, slug);

CREATE INDEX IF NOT EXISTS idx_tags_expanded_word ON content_tags_expanded(tag_word);
CREATE INDEX IF NOT EXISTS idx_tags_expanded_content ON content_tags_expanded(collection_name, slug);
CREATE INDEX IF NOT EXISTS idx_tags_expanded_word_covering ON content_tags_expanded(tag_word, collection_name, slug);

CREATE INDEX IF NOT EXISTS idx_sections_name ON content_sections(section_name);
CREATE INDEX IF NOT EXISTS idx_sections_content ON content_sections(collection_name, slug);
CREATE INDEX IF NOT EXISTS idx_sections_name_covering ON content_sections(section_name, collection_name, slug);
