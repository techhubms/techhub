-- Processed URLs Tracking
-- Feature: Track every URL the content processing pipeline has attempted.
-- Date: 2026-03-24
-- Purpose: Prevent re-processing URLs that have already been attempted (success or failure).
--          Also stores YouTube tags fetched from the external API for reuse.

-- ========================================
-- Processed URLs Table
-- ========================================
CREATE TABLE IF NOT EXISTS processed_urls (
    -- The external URL being tracked (unique key)
    external_url TEXT PRIMARY KEY,

    -- Processing outcome: 'succeeded' or 'failed'
    status TEXT NOT NULL,

    -- Optional error message for failed items
    error_message TEXT,

    -- YouTube tags fetched from external API (NULL for non-YouTube or when fetch failed)
    youtube_tags TEXT[],

    -- Timestamps
    processed_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Index for cleanup of old failed entries
CREATE INDEX IF NOT EXISTS idx_processed_urls_status_processed_at
    ON processed_urls (status, processed_at);
