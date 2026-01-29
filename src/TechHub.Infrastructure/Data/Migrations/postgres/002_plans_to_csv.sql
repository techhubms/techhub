-- Migration: Convert plans from junction table to CSV field
-- Date: 2026-01-29
-- Reason: Plans only used on one page (ghc-features), simpler as CSV

-- Add plans column to content_items (with existence check)
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'content_items' AND column_name = 'plans'
    ) THEN
        ALTER TABLE content_items ADD COLUMN plans TEXT;
    END IF;
END $$;

-- Migrate existing data from content_plans to CSV
UPDATE content_items
SET plans = (
    SELECT STRING_AGG(plan_name, ',')
    FROM content_plans
    WHERE content_plans.collection_name = content_items.collection_name
      AND content_plans.slug = content_items.slug
);

-- Drop the content_plans table and its indexes
DROP INDEX IF EXISTS idx_plans_name;
DROP INDEX IF EXISTS idx_plans_content;
DROP TABLE IF EXISTS content_plans CASCADE;
