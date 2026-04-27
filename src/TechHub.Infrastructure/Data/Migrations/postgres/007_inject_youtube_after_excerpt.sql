-- Migration 007: Inject YouTube embed marker after <!--excerpt_end--> for videos without H1
-- Handles video items that do NOT have a duplicate H1 heading (covered by migration 006)
-- but DO have the <!--excerpt_end--> marker. Inserts {% youtube VIDEO_ID %} immediately
-- after the excerpt_end comment so the embed appears right after the intro paragraph.
-- Idempotent: skips items that already have a YouTube marker.

BEGIN;

WITH targets AS (
    SELECT slug,
           collection_name,
           CASE
             WHEN external_url ~ 'youtu\.be/([A-Za-z0-9_-]{11})'
               THEN substring(external_url FROM 'youtu\.be/([A-Za-z0-9_-]{11})')
             WHEN external_url ~ 'youtube\.com/watch\?[^ ]*v=([A-Za-z0-9_-]{11})'
               THEN substring(external_url FROM 'v=([A-Za-z0-9_-]{11})')
             WHEN external_url ~ 'youtube\.com/embed/([A-Za-z0-9_-]{11})'
               THEN substring(external_url FROM 'embed/([A-Za-z0-9_-]{11})')
             ELSE NULL
           END AS video_id
    FROM   content_items
    WHERE  collection_name = 'videos'
      AND  external_url IS NOT NULL
      AND  external_url ~ 'youtu(\.be|be\.com)'
      AND  content IS NOT NULL
      AND  content !~ '\[YouTube:'
      AND  content !~ '\{%\s*youtube'
      -- Only target items WITHOUT an H1 (those are handled by migration 006)
      AND  content !~ E'(^|\\n)# '
      -- Must have the excerpt_end marker
      AND  content ~ '<!--excerpt_end-->'
)
UPDATE content_items ci
SET    content    = regexp_replace(
                      ci.content,
                      '<!--excerpt_end-->',
                      '<!--excerpt_end-->' || E'\n\n' || '{% youtube ' || t.video_id || ' %}',
                      'n'
                    ),
       updated_at = NOW()
FROM   targets t
WHERE  ci.collection_name = t.collection_name
  AND  ci.slug = t.slug
  AND  t.video_id IS NOT NULL;

COMMIT;
