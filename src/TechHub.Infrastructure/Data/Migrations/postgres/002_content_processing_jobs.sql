-- Migration 002: Content Processing Jobs + AI Metadata
-- Adds job tracking for the content processing pipeline integrated into the API,
-- and stores AI-extracted roundup metadata per content item.

-- ========================================
-- Content Processing Jobs Table
-- ========================================
-- Tracks each run of the content processing pipeline (scheduled or manual).
-- Rows are append-only: one row per run with final statistics on completion.
CREATE TABLE IF NOT EXISTS content_processing_jobs (
    id             BIGSERIAL PRIMARY KEY,
    started_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    completed_at   TIMESTAMPTZ,
    duration_ms    BIGINT,
    status         TEXT NOT NULL DEFAULT 'running', -- running | completed | failed
    trigger_type   TEXT NOT NULL DEFAULT 'scheduled', -- scheduled | manual
    feeds_processed INTEGER NOT NULL DEFAULT 0,
    items_added    INTEGER NOT NULL DEFAULT 0,
    items_skipped  INTEGER NOT NULL DEFAULT 0,
    error_count    INTEGER NOT NULL DEFAULT 0,
    log_output     TEXT
);

CREATE INDEX IF NOT EXISTS idx_processing_jobs_started_at
    ON content_processing_jobs (started_at DESC);

-- ========================================
-- AI Metadata Column on content_items
-- ========================================
-- Stores AI-extracted roundup metadata as JSONB:
--   {
--     "roundup_summary": "...",
--     "key_topics": ["..."],
--     "roundup_relevance": "high|medium|low"
--   }
-- NULL for items processed before this migration or items without AI metadata.
ALTER TABLE content_items
    ADD COLUMN IF NOT EXISTS ai_metadata JSONB;
