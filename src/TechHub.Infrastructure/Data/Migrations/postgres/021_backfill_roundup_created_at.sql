-- Migration 021: Backfill roundup created_at from slug date
-- Date: 2026-05-24
-- Purpose: Roundups that were imported via backfill migration all received
--          created_at = the migration run time instead of their original publish date.
--          Each roundup slug encodes the publish date as the final YYYY-MM-DD segment,
--          e.g. 'weekly-ai-roundup-2026-05-18' was published on 2026-05-18.
--          The roundup generator runs at 08:00 UTC on Mondays, so created_at is set
--          to that date at 08:00:00+00.
--
-- This migration is idempotent: re-running it produces the same result.

BEGIN;

UPDATE content_items
SET    created_at = to_timestamp(
                       (regexp_match(slug, '-(\d{4}-\d{2}-\d{2})$'))[1],
                       'YYYY-MM-DD'
                   ) + INTERVAL '8 hours',
       updated_at = NOW()
WHERE  collection_name = 'roundups'
  AND  slug ~ '^weekly-.+-roundup-\d{4}-\d{2}-\d{2}$';

COMMIT;
