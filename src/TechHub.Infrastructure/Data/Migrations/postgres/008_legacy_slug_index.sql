-- Migration 008: Partial index for legacy-slug lookups
-- The FindByLegacySlugAsync query matches on slug equality with pre-lowercased parameters.
-- All slugs in content_items are stored in lowercase (from markdown filenames).
-- The PK is (collection_name, slug) — collection_name is the leading key, so the PK
-- cannot efficiently serve slug-only equality seeks across all collections.
-- No existing index has slug as the leading column, so this new partial index is required.
-- Partial (WHERE draft = FALSE) keeps it small — draft items are never queried here.

CREATE INDEX IF NOT EXISTS idx_items_slug_nondraft
    ON content_items (slug)
    WHERE draft = FALSE;
