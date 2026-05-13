-- Migration 025: Rename features custom page key and strip obsolete JSON fields
-- Date: 2026-05-13
-- Purpose: The 'features' custom_page_data entry originally stored the full
--          GitHub Copilot feature matrix including timelineFeatures, featureSections
--          and videoCollection. Those fields have been superseded by the dedicated
--          ghc_features / ghc_feature_content tables (migrations 016–024) and are
--          no longer read by the application. Only the subscriptionTiers, title,
--          description, intro, note and links remain in use.
--
-- Changes:
--   1. Rename key 'features' → 'githubcopilot-features' for clarity.
--   2. Update description to reflect the page's actual purpose.
--   3. Remove the now-unused JSON fields: timelineFeatures, featureSections, videoCollection.

UPDATE custom_page_data
SET
    key         = 'githubcopilot-features',
    description = 'GitHub Copilot Features page — subscription tier definitions and plan details used by /github-copilot/features',
    json_data   = ((json_data::jsonb - 'timelineFeatures' - 'featureSections' - 'videoCollection')::text),
    updated_at  = NOW()
WHERE key = 'features';
