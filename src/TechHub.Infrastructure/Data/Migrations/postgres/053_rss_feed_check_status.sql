-- ============================================================
-- 053: RSS feed check status
-- The content pipeline records when it last downloaded each feed and why that failed (for example "HTTP 404"),
-- so the admin feeds page can show broken feeds.
-- ============================================================

ALTER TABLE rss_feed_configs ADD COLUMN IF NOT EXISTS last_checked_at  TIMESTAMPTZ;
ALTER TABLE rss_feed_configs ADD COLUMN IF NOT EXISTS last_check_error TEXT;
