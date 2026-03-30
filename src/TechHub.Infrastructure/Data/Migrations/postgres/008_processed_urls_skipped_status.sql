-- Add 'skipped' status for AI-excluded items.
-- Previously both AI-included and AI-excluded items had status='succeeded'.
-- Now: 'succeeded' = AI included + written to content_items,
--      'skipped'   = AI excluded (not relevant enough),
--      'failed'    = error during processing.

-- Backfill: 'succeeded' entries without a matching content_items row are 'skipped'.
UPDATE processed_urls
SET status = 'skipped'
WHERE status = 'succeeded'
  AND NOT EXISTS (
    SELECT 1 FROM content_items ci
    WHERE ci.external_url = processed_urls.external_url
  );
