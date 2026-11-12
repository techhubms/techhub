-- Per-feed polling survives restarts through the recorded last check.
ALTER TABLE rss_feed_configs
    ADD COLUMN polling_interval_minutes INTEGER NOT NULL DEFAULT 60
    CHECK (polling_interval_minutes > 0);

-- Category and community feeds aggregate many boards; individual boards and DevBlogs stay hourly.
UPDATE rss_feed_configs
SET polling_interval_minutes = 15,
    updated_at = NOW()
WHERE url ~* '^https://techcommunity\.microsoft\.com/.*rss/(Category|Community)([?/#]|$)';
