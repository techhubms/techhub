-- Migration 024: Expand ghc_feature plans to include all accessible tiers
-- Date: 2026-05-12
-- Purpose: The GitHub Copilot Features page uses a tier hierarchy where higher-tier
--          subscribers can access all features available to lower tiers. A feature
--          with minimum tier "Free" is accessible to all 6 tiers; a feature with
--          minimum tier "Student" is accessible to Student, Pro, Business, Pro+, Enterprise.
--
--          This migration directly expands the plans column in ghc_features so that
--          each record explicitly lists every tier that can access the feature:
--            Free        → Free,Student,Pro,Business,Pro+,Enterprise
--            Student     → Student,Pro,Business,Pro+,Enterprise
--            Pro         → Pro,Business,Pro+,Enterprise
--            Business    → Business,Pro+,Enterprise
--            Pro+        → Pro+,Enterprise
--            Enterprise  → Enterprise
--
--          The expansion rule: find the minimum tier index among the feature's current
--          plans, then include all tiers at that index or higher (more expensive).
--
--          After this migration the ContentSyncService no longer performs this expansion
--          in code — the data is stored expanded, and the admin UI allows further edits.

WITH tier_order(tier, pos) AS (
    VALUES
        ('Free',       1),
        ('Student',    2),
        ('Pro',        3),
        ('Business',   4),
        ('Pro+',       5),
        ('Enterprise', 6)
),
feature_min_tier AS (
    -- For each feature, find the lowest-tier position among its current plans
    SELECT
        f.slug,
        MIN(COALESCE(t.pos, 9999)) AS min_pos
    FROM ghc_features f
    CROSS JOIN LATERAL unnest(string_to_array(f.plans, ',')) AS plan(name)
    LEFT JOIN tier_order t ON trim(plan.name) = t.tier
    WHERE f.plans IS NOT NULL AND f.plans != ''
    GROUP BY f.slug
),
feature_expanded AS (
    -- Build the expanded plan list: all tiers at or above the minimum
    SELECT
        m.slug,
        string_agg(t.tier, ',' ORDER BY t.pos) AS new_plans
    FROM feature_min_tier m
    JOIN tier_order t ON t.pos >= m.min_pos AND m.min_pos < 9999
    GROUP BY m.slug
)
UPDATE ghc_features f
SET
    plans      = fe.new_plans,
    updated_at = NOW()
FROM feature_expanded fe
WHERE f.slug = fe.slug
  AND f.plans != fe.new_plans;
