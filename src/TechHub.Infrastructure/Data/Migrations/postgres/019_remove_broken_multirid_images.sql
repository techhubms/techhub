-- Migration 019: Remove broken multirid image references from article content
--
-- The article 'ai-transforming-legal-workflows-insights-from-lawvu-at-microsoft-ignite-2024'
-- has 14 image references (multirid_01.png through multirid_14.png) in its markdown content
-- that point to images that were never uploaded to the CDN. Every crawler visiting this
-- page triggers 14 simultaneous 404 responses, which bursts past the 10/15min threshold
-- of the failed-requests alert.
--
-- This migration removes any content line that references a multirid image by filtering
-- the content line-by-line, keeping only lines that do not contain 'multirid'.
--
-- Idempotent: the WHERE clause short-circuits if multirid references are already absent.

BEGIN;

UPDATE content_items
SET    content = (
    SELECT string_agg(line, E'\n' ORDER BY rn)
    FROM (
        SELECT t.line, t.rn
        FROM unnest(string_to_array(content, E'\n')) WITH ORDINALITY AS t(line, rn)
    ) AS lines
    WHERE line NOT LIKE '%multirid%'
)
WHERE  slug    = 'ai-transforming-legal-workflows-insights-from-lawvu-at-microsoft-ignite-2024'
  AND  content LIKE '%multirid%';

COMMIT;
