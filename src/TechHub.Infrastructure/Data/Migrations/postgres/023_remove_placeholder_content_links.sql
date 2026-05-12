-- Migration 023: Remove placeholder content links from ghc_feature_content
-- Date: 2026-05-12
-- Purpose: Migration 016 created a ghc_feature_content row for every migrated
--          ghc-features content item, linking each feature back to itself as the
--          thumbnail. Many of those linked content_items have placeholder or
--          draft YouTube URLs (youtu.be/placeholder, youtu.be/draft-*, etc.)
--          that are not real watchable videos.
--
--          This migration removes ghc_feature_content rows where the linked
--          content item does not have a real YouTube video URL — i.e. the URL
--          does not match the standard youtu.be/<video-id> or
--          youtube.com/watch?v=<video-id> patterns with an actual video ID.
--
-- A "real" YouTube video URL is defined as:
--   - youtu.be/<id>  where <id> is 11 alphanumeric/dash/underscore chars
--   - youtube.com/watch?v=<id>  with the same ID constraints
-- Placeholder patterns like youtu.be/placeholder, youtu.be/draft-*, etc. are removed.

DELETE FROM ghc_feature_content
WHERE (feature_slug, collection_name, item_slug) IN (
    SELECT fc.feature_slug, fc.collection_name, fc.item_slug
    FROM ghc_feature_content fc
    JOIN content_items ci
      ON ci.collection_name = fc.collection_name
     AND ci.slug             = fc.item_slug
    WHERE ci.external_url IS NULL
       OR (
            -- Not a youtu.be/<11-char-id> URL
            ci.external_url !~ 'youtu\.be/[A-Za-z0-9_-]{11}(\?|$)'
            AND
            -- Not a youtube.com/watch?v=<11-char-id> URL
            ci.external_url !~ 'youtube\.com/watch\?.*v=[A-Za-z0-9_-]{11}(&|$)'
          )
);
