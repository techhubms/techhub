-- Drop section_roundup_items table
-- This table is no longer needed: roundup article selection now queries
-- content_items directly using section boolean columns and ai_metadata.
-- Date: 2026-03-31

DROP TABLE IF EXISTS section_roundup_items;
