-- Migration 014: Drop transcript_mandatory column from rss_feed_configs
-- The TranscriptMandatory feature has been removed. Automatic transcript fetching is
-- now disabled entirely in the batch pipeline; transcripts can be provided manually
-- via the admin UI for both new and existing video content items.

ALTER TABLE rss_feed_configs
    DROP COLUMN IF EXISTS transcript_mandatory;
