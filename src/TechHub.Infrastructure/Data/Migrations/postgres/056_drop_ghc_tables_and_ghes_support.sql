-- The GitHub Copilot features page now lives entirely on the catalog (migration 055), so the two tables it used before are dropped.
-- ghes_support was only meaningful for GitHub Copilot and does not belong in the catalog; the page no longer shows it.
ALTER TABLE product_features DROP COLUMN IF EXISTS ghes_support;

DROP TABLE IF EXISTS ghc_feature_content;
DROP TABLE IF EXISTS ghc_features;
