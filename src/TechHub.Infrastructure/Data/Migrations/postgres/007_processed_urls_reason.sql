-- Processed URLs Reason
-- Feature: Store AI explanation/reason for why a URL was included, excluded, or failed
-- Date: 2026-03-29
-- Purpose: The AI categorization returns an 'explanation' field explaining why content
--          was included or excluded. This column stores that explanation, as well as
--          error messages for failed items, providing transparency in the admin UI.

ALTER TABLE processed_urls
    ADD COLUMN IF NOT EXISTS reason TEXT;
