-- Migration 035: Fix primary_section_name for legacy all-section roundups
-- Purpose: The weekly-ai-and-tech-news-roundup-* entries are pre-PR all-section roundups
--          that covered every section. Their external_url and sections_bitmask are already
--          correct (/all/roundups/... and 127 respectively), but primary_section_name was
--          incorrectly set to 'github-copilot'. Update it to 'all' so they are properly
--          isolated from per-section roundup generation and listing.

UPDATE content_items
SET primary_section_name = 'all',
    updated_at = NOW()
WHERE collection_name = 'roundups'
  AND slug LIKE 'weekly-ai-and-tech-news-roundup-%';
