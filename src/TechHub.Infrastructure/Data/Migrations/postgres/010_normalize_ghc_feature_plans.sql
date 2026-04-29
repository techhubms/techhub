-- Migration 010: Normalize ghc-features plan assignments to full tier groups
-- Date: 2026-04-29
-- Purpose: The admin UI and features page use three plan groups:
--   Group 0: Free
--   Group 1: Student, Pro, Business
--   Group 2: Pro+, Enterprise
-- Existing records may have incomplete plan sets (e.g. only "Pro" instead of
-- "Student,Pro,Business"). This migration expands each record to include all
-- plans in its group, matching the frontend display logic.

-- ============================================================
-- 1. Group 1: Any record containing Student, Pro, or Business
--    should have all three: Student,Pro,Business
-- ============================================================
UPDATE content_items
SET plans      = 'Student,Pro,Business',
    updated_at = NOW()
WHERE subcollection_name = 'ghc-features'
  AND plans IS NOT NULL
  AND (
      plans LIKE '%Student%'
      OR plans LIKE '%Pro%'
      OR plans LIKE '%Business%'
  )
  AND plans NOT LIKE '%Pro+%'
  AND plans NOT LIKE '%Enterprise%'
  AND plans != 'Student,Pro,Business';

-- ============================================================
-- 2. Group 2: Any record containing Pro+ or Enterprise
--    should have both: Pro+,Enterprise
-- ============================================================
UPDATE content_items
SET plans      = 'Pro+,Enterprise',
    updated_at = NOW()
WHERE subcollection_name = 'ghc-features'
  AND plans IS NOT NULL
  AND (
      plans LIKE '%Pro+%'
      OR plans LIKE '%Enterprise%'
  )
  AND plans != 'Pro+,Enterprise';
