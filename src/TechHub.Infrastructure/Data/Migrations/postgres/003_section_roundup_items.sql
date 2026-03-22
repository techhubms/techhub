-- Migration 003: Section Roundup Items
-- Accumulates content items into per-section weekly roundup drafts as they are processed.
-- Each new item with roundup_relevance "high" or "medium" is registered here automatically
-- by the content processing pipeline.
-- On Monday morning the roundup generator reads from this table to build the final roundups.

-- ========================================
-- Section Roundup Items Table
-- ========================================
-- One row per (section × week × content item).
-- week_start_date is always the Monday of the ISO week in Europe/Brussels time.
-- Items can belong to multiple sections (one row per section).
CREATE TABLE IF NOT EXISTS section_roundup_items (
    id                  BIGSERIAL PRIMARY KEY,
    section_name        TEXT NOT NULL,              -- e.g. "github-copilot", "ai", "azure"
    week_start_date     DATE NOT NULL,              -- Monday of the ISO week (Europe/Brussels)
    collection_name     TEXT NOT NULL,
    slug                TEXT NOT NULL,
    added_at            TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    FOREIGN KEY (collection_name, slug)
        REFERENCES content_items (collection_name, slug)
        ON DELETE CASCADE,

    -- Prevent duplicate registrations of the same item in the same section/week
    UNIQUE (section_name, week_start_date, collection_name, slug)
);

CREATE INDEX IF NOT EXISTS idx_roundup_items_section_week
    ON section_roundup_items (section_name, week_start_date DESC);

CREATE INDEX IF NOT EXISTS idx_roundup_items_week
    ON section_roundup_items (week_start_date DESC);
