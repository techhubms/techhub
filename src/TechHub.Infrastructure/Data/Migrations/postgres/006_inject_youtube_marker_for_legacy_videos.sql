-- Migration 006: Inject YouTube embed marker for legacy video items
-- Inserts {% youtube VIDEO_ID %} immediately after the <!--excerpt_end--> comment.
-- All legacy video items follow the pattern: intro paragraph<!--excerpt_end-->\n\n## ...
-- Newer items already have the YouTube marker.
-- This enables the YouTube iframe embed renderer (MarkdownService.ProcessYouTubeEmbeds).
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
             WHEN external_url ~ 'youtube\.com/shorts/([A-Za-z0-9_-]{11})'
               THEN substring(external_url FROM 'shorts/([A-Za-z0-9_-]{11})')
             ELSE NULL
           END AS video_id
    FROM   content_items
    WHERE  collection_name = 'videos'
      AND  external_url IS NOT NULL
      AND  external_url ~ 'youtu(\.be|be\.com)'
      AND  content IS NOT NULL
      AND  content !~ '\[YouTube:'
      AND  content !~ '\{%\s*youtube'
      AND  content ~ '<!--excerpt_end-->'
)
UPDATE content_items ci
SET    content    = regexp_replace(
                      ci.content,
                      '<!--excerpt_end-->',
                      '<!--excerpt_end-->' || E'\n\n' || '{% youtube ' || t.video_id || ' %}'
                    ),
       updated_at = NOW()
FROM   targets t
WHERE  ci.collection_name = t.collection_name
  AND  ci.slug = t.slug
  AND  t.video_id IS NOT NULL;

COMMIT;
