-- ============================================================
-- 055: Indexes for the content of an RSS feed
-- The admin feeds page asks per feed whether it has content (content_items or processed_urls) and when the newest
-- article arrived. Without these indexes every feed scans both tables.
-- ============================================================

CREATE INDEX IF NOT EXISTS idx_content_items_feed_name_created_at
    ON content_items (feed_name, created_at);

CREATE INDEX IF NOT EXISTS idx_processed_urls_feed_name
    ON processed_urls (feed_name);
