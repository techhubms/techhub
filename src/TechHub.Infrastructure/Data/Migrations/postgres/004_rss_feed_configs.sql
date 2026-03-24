-- Migration 004: RSS Feed Configurations
-- Moves RSS feed configuration from a static JSON file to the database,
-- allowing admin users to manage feeds from the admin UI.

CREATE TABLE IF NOT EXISTS rss_feed_configs (
    id              BIGSERIAL PRIMARY KEY,
    name            TEXT NOT NULL,                          -- Display name (e.g. "The GitHub Blog")
    url             TEXT NOT NULL,                          -- RSS/Atom feed URL
    output_dir      TEXT NOT NULL,                          -- Target collection (e.g. "_news", "_blogs")
    enabled         BOOLEAN NOT NULL DEFAULT TRUE,          -- Whether this feed is active
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    UNIQUE (url)
);

CREATE INDEX IF NOT EXISTS idx_rss_feed_configs_enabled
    ON rss_feed_configs (enabled) WHERE enabled = TRUE;
