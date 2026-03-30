-- Processed URLs Feed Metadata
-- Feature: Store feed_name and collection_name directly in processed_urls
-- Date: 2026-03-29
-- Purpose: Previously we relied on a LEFT JOIN to content_items to show feed/collection,
--          but many processed URLs (AI-skipped or failed items) have no matching content_item.
--          Storing feed metadata directly ensures it's always available for the admin page.

ALTER TABLE processed_urls
    ADD COLUMN IF NOT EXISTS feed_name TEXT,
    ADD COLUMN IF NOT EXISTS collection_name TEXT;

-- Backfill from content_items for existing data
UPDATE processed_urls p
SET feed_name = c.feed_name,
    collection_name = c.collection_name
FROM content_items c
WHERE p.external_url = c.external_url
  AND p.feed_name IS NULL;
