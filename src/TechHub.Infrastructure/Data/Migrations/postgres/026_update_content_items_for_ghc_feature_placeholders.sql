-- Migration 026: Fix content_items for GHC feature placeholder items
-- Date: 2026-05-13
-- Purpose: For content_items that correspond to a GHC feature (same slug) AND still have
--          auto-generated placeholder content ("This content demonstrates..."):
--
--            Group A — item has a real YouTube URL (11-char video ID):
--              Update excerpt and content to use the proper ghc_features.description.
--              content = <first sentence><!--excerpt_end-->
--
--                        {% youtube VIDEO_ID %}
--
--                        ## Overview
--
--                        <full description>
--              excerpt = first sentence of ghc_features.description (plain text)
--
--            Group B — item has a placeholder/no real YouTube URL:
--              Delete the content_items row entirely — a video item with no watchable
--              video has no value. The feature is represented in ghc_features already.
--              content_tags_expanded, ghc_feature_content, and vscode_update_items all
--              have ON DELETE CASCADE and are cleaned up automatically.
--
--          content_items.search_vector is a GENERATED column and updates automatically.
--          content_items.ai_metadata is left untouched (not regenerated here).
--          content_hash matches the AiCategorizationService formula: SHA-256 of
--          title || new_content || new_excerpt, stored as lowercase hex.
--
-- Preview (run these SELECTs before applying to confirm scope):
--
-- Group A (will be updated):
-- SELECT ci.slug, ci.title, ci.external_url
-- FROM content_items ci
-- JOIN ghc_features f ON f.slug = ci.slug
-- WHERE ci.content LIKE 'This content demonstrates%'
--   AND (   ci.external_url ~ 'youtu\.be/[A-Za-z0-9_-]{11}(\?|$)'
--        OR ci.external_url ~ 'youtube\.com/watch\?[^ ]*v=[A-Za-z0-9_-]{11}(&|$)'
--        OR ci.external_url ~ 'youtube\.com/embed/[A-Za-z0-9_-]{11}')
-- ORDER BY ci.slug;
--
-- Group B (will be deleted):
-- SELECT ci.slug, ci.title, ci.external_url
-- FROM content_items ci
-- JOIN ghc_features f ON f.slug = ci.slug
-- WHERE ci.content LIKE 'This content demonstrates%'
--   AND NOT (   ci.external_url ~ 'youtu\.be/[A-Za-z0-9_-]{11}(\?|$)'
--            OR ci.external_url ~ 'youtube\.com/watch\?[^ ]*v=[A-Za-z0-9_-]{11}(&|$)'
--            OR ci.external_url ~ 'youtube\.com/embed/[A-Za-z0-9_-]{11}')
-- ORDER BY ci.slug;

-- ============================================================
-- Step 1: Delete Group B — content_items
--         (content_tags_expanded, ghc_feature_content, and
--          vscode_update_items all cascade automatically via
--          the FK constraints added in migration 003)
-- ============================================================

DELETE FROM content_items ci
USING ghc_features f
WHERE ci.slug = f.slug
  AND ci.content LIKE 'This content demonstrates%'
  AND NOT (   ci.external_url ~ 'youtu\.be/[A-Za-z0-9_-]{11}(\?|$)'
           OR ci.external_url ~ 'youtube\.com/watch\?[^ ]*v=[A-Za-z0-9_-]{11}(&|$)'
           OR ci.external_url ~ 'youtube\.com/embed/[A-Za-z0-9_-]{11}');

-- ============================================================
-- Step 2: Update Group A — real YouTube video items
-- ============================================================

WITH feature_items AS (
    SELECT
        ci.slug,
        ci.collection_name,
        ci.title,
        f.description,
        -- First sentence as excerpt: text up to and including the first period
        CASE
            WHEN POSITION('.' IN f.description) > 0
                THEN LEFT(f.description, POSITION('.' IN f.description))
            ELSE f.description
        END AS short_excerpt,
        -- Extract the 11-char YouTube video ID
        CASE
            WHEN ci.external_url ~ 'youtu\.be/([A-Za-z0-9_-]{11})(\?|$)'
                THEN substring(ci.external_url FROM 'youtu\.be/([A-Za-z0-9_-]{11})')
            WHEN ci.external_url ~ 'youtube\.com/watch\?[^ ]*v=([A-Za-z0-9_-]{11})(&|$)'
                THEN substring(ci.external_url FROM 'v=([A-Za-z0-9_-]{11})')
            WHEN ci.external_url ~ 'youtube\.com/embed/([A-Za-z0-9_-]{11})'
                THEN substring(ci.external_url FROM 'embed/([A-Za-z0-9_-]{11})')
        END AS video_id
    FROM content_items ci
    JOIN ghc_features f ON f.slug = ci.slug
    WHERE ci.content LIKE 'This content demonstrates%'
),
feature_items_with_content AS (
    SELECT
        slug,
        collection_name,
        title,
        short_excerpt,
        short_excerpt
            || '<!--excerpt_end-->'
            || E'\n\n{% youtube ' || video_id || E' %}\n\n'
            || '## Overview'
            || E'\n\n' || description
        AS new_content
    FROM feature_items
)
UPDATE content_items ci
SET
    excerpt      = fi.short_excerpt,
    content      = fi.new_content,
    -- SHA-256(title || content || excerpt) as lowercase hex — matches AiCategorizationService
    content_hash = encode(sha256(convert_to(fi.title || fi.new_content || fi.short_excerpt, 'UTF8')), 'hex'),
    updated_at   = NOW()
FROM feature_items_with_content fi
WHERE ci.slug            = fi.slug
  AND ci.collection_name = fi.collection_name;
