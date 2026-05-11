-- Migration 018: Link related content items to GHC features via full-text search
-- Date: 2026-05-11
-- Purpose: For each feature in ghc_features, find the top-4 most relevant
--          content items (videos) using PostgreSQL full-text search (search_vector),
--          then insert them as non-thumbnail links in ghc_feature_content.
--
-- Matching criteria:
--   - Same collection as existing links ('videos')
--   - ts_rank(search_vector, plainto_tsquery(feature.title)) >= 0.70
--   - is_github_copilot = TRUE (ensures topicality)
--   - Not the feature's own slug (already the thumbnail)
--   - Not already linked
--   - At most 4 extra links per feature

INSERT INTO ghc_feature_content (feature_slug, collection_name, item_slug, is_thumbnail, sort_order)
SELECT feature_slug, collection_name, item_slug, FALSE, rn::integer
FROM (
    SELECT
        f.slug                 AS feature_slug,
        ci.collection_name,
        ci.slug                AS item_slug,
        ROW_NUMBER() OVER (
            PARTITION BY f.slug
            ORDER BY ts_rank(ci.search_vector, plainto_tsquery('english', f.title)) DESC
        ) AS rn
    FROM ghc_features f
    JOIN content_items ci ON
        ci.collection_name = 'videos'
        AND ci.search_vector @@ plainto_tsquery('english', f.title)
        AND ci.slug != f.slug
        AND ci.is_github_copilot = TRUE
        AND ts_rank(ci.search_vector, plainto_tsquery('english', f.title)) >= 0.70
        AND NOT EXISTS (
            SELECT 1
            FROM ghc_feature_content fc
            WHERE fc.feature_slug = f.slug
              AND fc.item_slug    = ci.slug
        )
) ranked
WHERE rn <= 4
ON CONFLICT (feature_slug, collection_name, item_slug) DO NOTHING;
