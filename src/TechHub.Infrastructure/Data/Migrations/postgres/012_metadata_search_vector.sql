-- Migration 012: Add metadata fields to search vector for improved discoverability
-- Date: 2026-04-30
-- Purpose: Index feed_name, subcollection_name, collection_name, and primary_section_name
--          in the search vector (all at weight D) so that searches from the "all" page
--          surface content by its structural identity, not only by body text.
--
--          Examples:
--            • Searching "vscode updates" now finds "VS Code Updates" videos via
--              subcollection_name = "vscode-updates" (tokenised to "vscode", "updates").
--            • Searching "Visual Studio Code YouTube" finds videos from that feed
--              even if the word "YouTube" never appears in their body text.
--            • Searching "videos" surfaces video-collection content.
--            • Searching "github-copilot" finds GitHub Copilot section content.
--
--          Weight D is used so metadata signals help rank relevant content higher
--          without overpowering title (A), excerpt (B), or body (C) matches.

ALTER TABLE content_items
    DROP COLUMN search_vector;

ALTER TABLE content_items
    ADD COLUMN search_vector TSVECTOR GENERATED ALWAYS AS (
        setweight(to_tsvector('english', coalesce(title, '')), 'A') ||
        setweight(to_tsvector('english', coalesce(excerpt, '')), 'B') ||
        setweight(to_tsvector('english', coalesce(content, '')), 'C') ||
        setweight(to_tsvector('simple', coalesce(title, '')), 'D') ||
        setweight(to_tsvector('simple', coalesce(feed_name, '')), 'D') ||
        setweight(to_tsvector('simple', coalesce(subcollection_name, '')), 'D') ||
        setweight(to_tsvector('simple', coalesce(collection_name, '')), 'D') ||
        setweight(to_tsvector('simple', coalesce(primary_section_name, '')), 'D')
    ) STORED;

-- Recreate the GIN index on the updated search_vector
CREATE INDEX IF NOT EXISTS idx_content_search ON content_items USING GIN(search_vector);
