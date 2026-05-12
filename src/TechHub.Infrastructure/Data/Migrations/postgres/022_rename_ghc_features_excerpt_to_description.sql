-- Migration 022: Rename ghc_features.excerpt to ghc_features.description
-- Date: 2026-05-12
-- Purpose: The column was named 'excerpt' to match the content_items pattern but
--          this table has no Excerpt concept — the field is a feature description.
--          Rename to avoid the aliasing hack in the repository (f.excerpt AS Description).

ALTER TABLE ghc_features RENAME COLUMN excerpt TO description;
