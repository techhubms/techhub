-- Migration 002: Feature Tables
-- Creates all tables and columns introduced after the initial schema (001).
-- Consolidates what were previously migrations 002–023.
-- Pure DDL — no data manipulation. See 003_data_and_constraints.sql for data work.

-- ============================================================
-- content_items additions
-- ============================================================

ALTER TABLE content_items
    ADD COLUMN IF NOT EXISTS ai_metadata JSONB;

-- ============================================================
-- content_processing_jobs
-- ============================================================

CREATE TABLE IF NOT EXISTS content_processing_jobs (
    id                    BIGSERIAL    PRIMARY KEY,
    started_at            TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    completed_at          TIMESTAMPTZ,
    duration_ms           BIGINT,
    status                TEXT         NOT NULL DEFAULT 'running',       -- running | completed | failed
    trigger_type          TEXT         NOT NULL DEFAULT 'scheduled',     -- scheduled | manual
    job_type              TEXT         NOT NULL DEFAULT 'content-processing',
    feeds_processed       INTEGER      NOT NULL DEFAULT 0,
    items_added           INTEGER      NOT NULL DEFAULT 0,
    items_skipped         INTEGER      NOT NULL DEFAULT 0,
    items_fixed           INTEGER      NOT NULL DEFAULT 0,
    error_count           INTEGER      NOT NULL DEFAULT 0,
    transcripts_succeeded INTEGER      NOT NULL DEFAULT 0,
    transcripts_failed    INTEGER      NOT NULL DEFAULT 0,
    log_output            TEXT
);

CREATE INDEX IF NOT EXISTS idx_processing_jobs_started_at
    ON content_processing_jobs (started_at DESC);

-- ============================================================
-- rss_feed_configs
-- ============================================================

CREATE TABLE IF NOT EXISTS rss_feed_configs (
    id                   BIGSERIAL    PRIMARY KEY,
    name                 TEXT         NOT NULL,
    url                  TEXT         NOT NULL,
    output_dir           TEXT         NOT NULL,
    enabled              BOOLEAN      NOT NULL DEFAULT TRUE,
    transcript_mandatory BOOLEAN      NOT NULL DEFAULT FALSE,
    created_at           TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    UNIQUE (url)
);

CREATE INDEX IF NOT EXISTS idx_rss_feed_configs_enabled
    ON rss_feed_configs (enabled) WHERE enabled = TRUE;

-- ============================================================
-- processed_urls (final form — all columns included from the start)
-- ============================================================

CREATE TABLE IF NOT EXISTS processed_urls (
    external_url    TEXT         PRIMARY KEY,
    status          TEXT         NOT NULL,                          -- succeeded | skipped | failed
    error_message   TEXT,
    youtube_tags    TEXT[],
    feed_name       TEXT,
    collection_name TEXT,
    reason          TEXT,                                           -- AI explanation or error detail
    has_transcript  BOOLEAN,                                        -- NULL for non-YouTube items
    job_id          BIGINT,
    slug            TEXT,
    processed_at    TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    updated_at      TIMESTAMPTZ  NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_processed_urls_status_processed_at
    ON processed_urls (status, processed_at);

CREATE INDEX IF NOT EXISTS idx_processed_urls_job_id
    ON processed_urls (job_id) WHERE job_id IS NOT NULL;

-- ============================================================
-- custom_page_data
-- ============================================================

CREATE TABLE IF NOT EXISTS custom_page_data (
    key         TEXT         PRIMARY KEY,
    description TEXT         NOT NULL DEFAULT '',
    json_data   TEXT         NOT NULL,
    updated_at  TIMESTAMPTZ  NOT NULL DEFAULT NOW()
);

-- ============================================================
-- background_job_settings
-- ============================================================

CREATE TABLE IF NOT EXISTS background_job_settings (
    job_name    TEXT         PRIMARY KEY,
    enabled     BOOLEAN      NOT NULL DEFAULT TRUE,
    description TEXT         NOT NULL DEFAULT ''
);

-- ============================================================
-- content_reviews
-- ============================================================

CREATE TABLE IF NOT EXISTS content_reviews (
    id              BIGSERIAL    PRIMARY KEY,
    slug            TEXT         NOT NULL,
    collection_name TEXT         NOT NULL,
    change_type     TEXT         NOT NULL,                          -- 'tags' | 'markdown' | 'author'
    original_value  TEXT         NOT NULL DEFAULT '',
    fixed_value     TEXT         NOT NULL DEFAULT '',
    status          TEXT         NOT NULL DEFAULT 'pending',        -- pending | approved | rejected
    job_id          BIGINT       REFERENCES content_processing_jobs (id) ON DELETE SET NULL,
    created_at      TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    reviewed_at     TIMESTAMPTZ,

    CONSTRAINT fk_content_reviews_item
        FOREIGN KEY (collection_name, slug)
        REFERENCES content_items (collection_name, slug)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_content_reviews_status_created
    ON content_reviews (status, created_at DESC);

CREATE INDEX IF NOT EXISTS idx_content_reviews_item
    ON content_reviews (collection_name, slug);
