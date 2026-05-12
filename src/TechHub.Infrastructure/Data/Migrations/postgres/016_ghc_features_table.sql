-- Migration 016: Dedicated tables for GHC Features and VS Code Updates
-- Date: 2026-05-11
-- Purpose: Replace the subcollection_name / plans / ghes_support / draft columns on
--          content_items with proper relational tables:
--
--   ghc_features         — the feature entity (slug, title, excerpt, plans, ghes_support, release_date)
--   ghc_feature_content  — many-to-many: one feature can link to multiple content items,
--                          one content item can belong to multiple features.
--                          is_thumbnail marks the single item whose YouTube thumbnail
--                          is shown when a feature card is expanded on the features page.
--   vscode_update_items  — membership set: content items that appear on /github-copilot/vscode-updates.
--
-- Plans are now stored as ALL applicable tiers (Free, Student, Pro, Business, Pro+, Enterprise)
-- rather than a minimum-tier indicator.

-- ============================================================
-- 1. Create new tables
-- ============================================================

CREATE TABLE IF NOT EXISTS ghc_features (
    slug         TEXT        PRIMARY KEY,
    title        TEXT        NOT NULL,
    excerpt      TEXT        NOT NULL DEFAULT '',
    release_date BIGINT,                          -- nullable: set by admin when feature ships
    plans        TEXT        NOT NULL DEFAULT '',  -- comma-separated, ALL applicable plans
    ghes_support BOOLEAN     NOT NULL DEFAULT FALSE,
    created_at   TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at   TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS ghc_feature_content (
    feature_slug    TEXT    NOT NULL REFERENCES ghc_features(slug) ON DELETE CASCADE,
    collection_name TEXT    NOT NULL,
    item_slug       TEXT    NOT NULL,
    is_thumbnail    BOOLEAN NOT NULL DEFAULT FALSE,
    sort_order      INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (feature_slug, collection_name, item_slug),
    FOREIGN KEY (collection_name, item_slug) REFERENCES content_items(collection_name, slug) ON DELETE CASCADE
);

-- Enforce at most one thumbnail per feature
CREATE UNIQUE INDEX IF NOT EXISTS idx_ghc_feature_content_thumbnail
    ON ghc_feature_content (feature_slug)
    WHERE is_thumbnail = TRUE;

CREATE TABLE IF NOT EXISTS vscode_update_items (
    collection_name TEXT NOT NULL DEFAULT 'videos',
    slug            TEXT NOT NULL,
    PRIMARY KEY (collection_name, slug),
    FOREIGN KEY (collection_name, slug) REFERENCES content_items(collection_name, slug) ON DELETE CASCADE
);

-- ============================================================
-- 2. Migrate existing data
-- ============================================================

-- 2a. Create ghc_features rows for every existing ghc-features content item.
--     release_date inherits the item's date_epoch (video publication date).
INSERT INTO ghc_features (slug, title, excerpt, release_date, plans, ghes_support)
SELECT
    slug,
    title,
    COALESCE(excerpt, ''),
    date_epoch,
    COALESCE(plans, ''),
    ghes_support
FROM content_items
WHERE subcollection_name = 'ghc-features'
ON CONFLICT (slug) DO NOTHING;

-- 2b. Link each migrated feature to its content_item as the thumbnail.
--     All existing items were 1-to-1, so every link is the thumbnail.
INSERT INTO ghc_feature_content (feature_slug, collection_name, item_slug, is_thumbnail, sort_order)
SELECT
    slug AS feature_slug,
    collection_name,
    slug AS item_slug,
    TRUE,
    0
FROM content_items
WHERE subcollection_name = 'ghc-features'
ON CONFLICT DO NOTHING;

-- 2c. Migrate vscode-updates items into the membership table.
INSERT INTO vscode_update_items (collection_name, slug)
SELECT collection_name, slug
FROM content_items
WHERE subcollection_name = 'vscode-updates'
ON CONFLICT DO NOTHING;

-- ============================================================
-- 3. Rebuild search_vector without subcollection_name
--    (Migration 012 added it; dropping the column requires rebuilding.)
-- ============================================================

ALTER TABLE content_items
    DROP COLUMN IF EXISTS search_vector;

ALTER TABLE content_items
    ADD COLUMN search_vector TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C') ||
        setweight(to_tsvector('simple', coalesce(title, '')), 'D') ||
        setweight(to_tsvector('simple', coalesce(feed_name, '')), 'D') ||
        setweight(to_tsvector('simple', coalesce(collection_name, '')), 'D') ||
        setweight(to_tsvector('simple', coalesce(primary_section_name, '')), 'D')
    ) STORED;

CREATE INDEX IF NOT EXISTS idx_content_search ON content_items USING GIN(search_vector);

-- ============================================================
-- 4. Drop the now-redundant columns from content_items
-- ============================================================

ALTER TABLE content_items
    DROP COLUMN IF EXISTS subcollection_name,
    DROP COLUMN IF EXISTS plans,
    DROP COLUMN IF EXISTS ghes_support,
    DROP COLUMN IF EXISTS draft;
