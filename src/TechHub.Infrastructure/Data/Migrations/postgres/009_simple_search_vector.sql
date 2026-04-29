-- Migration: Add simple-config search vector for stop word preservation
-- Date: 2026-04-28
-- Purpose: The english tsvector config strips stop words like "vs" (from "VS Code"),
--          making searches for "vs code" ineffective. Adding a simple-config component
--          preserves all words including stop words, improving search recall.
--          Combined with OR-based query logic and ts_rank ordering.

-- Drop and recreate the search_vector generated column to include simple config
-- The simple config preserves all words (no stemming, no stop word removal)
-- Weight D is used for simple-config terms so they don't overpower english-stemmed matches
ALTER TABLE content_items
    DROP COLUMN search_vector;

ALTER TABLE content_items
    ADD COLUMN search_vector TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C') ||
        setweight(to_tsvector('simple', coalesce(title, '')), 'D')
    ) STORED;

-- Recreate the GIN index on the updated search_vector
CREATE INDEX IF NOT EXISTS idx_content_search ON content_items USING GIN(search_vector);
