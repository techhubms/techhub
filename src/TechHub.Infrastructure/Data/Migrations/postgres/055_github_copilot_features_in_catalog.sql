-- The GitHub Copilot features page reads its features from the product catalog instead of its own tables.
--
-- A feature in the catalog (product_features) now also carries what the page shows: a slug (the anchor and the address of the
-- feature in the admin), a description, whether GitHub Enterprise Server supports it, a release date (epoch seconds, optional:
-- the page falls back on the first launch, preview or ga date of the events of the feature) and the content it links to
-- (product_feature_content, with one optional thumbnail).
--
-- The features and links of ghc_features and ghc_feature_content are copied here. Those two tables stay in place, unused, so the
-- copy can be checked or repeated; they are dropped in a later migration. The subscription plans of ghc_features are not copied:
-- the page no longer filters on plans.

ALTER TABLE product_features
    ADD COLUMN IF NOT EXISTS slug         TEXT,
    ADD COLUMN IF NOT EXISTS description  TEXT NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS ghes_support BOOLEAN NOT NULL DEFAULT FALSE,
    ADD COLUMN IF NOT EXISTS release_date BIGINT,
    ADD COLUMN IF NOT EXISTS updated_at   TIMESTAMPTZ NOT NULL DEFAULT NOW();

-- A readable, unique slug for a feature name: lowercase words joined by dashes, with a number when the slug is taken.
CREATE OR REPLACE FUNCTION generate_feature_slug(feature_name TEXT) RETURNS TEXT
LANGUAGE plpgsql AS $$
DECLARE
    v_base      TEXT := left(trim(both '-' from regexp_replace(lower(COALESCE(feature_name, '')), '[^a-z0-9]+', '-', 'g')), 80);
    v_candidate TEXT;
    v_suffix    INT := 1;
BEGIN
    IF v_base = '' THEN
        v_base := 'feature';
    END IF;

    v_candidate := v_base;
    WHILE EXISTS (SELECT 1 FROM product_features WHERE slug = v_candidate) LOOP
        v_suffix := v_suffix + 1;
        v_candidate := v_base || '-' || v_suffix;
    END LOOP;

    RETURN v_candidate;
END
$$;

-- Features of the curated page that the catalog does not have yet (054 copied the titles that existed then).
INSERT INTO product_features (product_id, name)
SELECT p.id, g.title
FROM (SELECT DISTINCT ON (lower(title)) title FROM ghc_features ORDER BY lower(title), slug) g
JOIN products p ON lower(p.name) = 'github copilot'
ON CONFLICT (product_id, lower(name)) DO NOTHING;

-- The page data of each feature, matched by title. When two old features share a title, the first slug wins.
UPDATE product_features f
SET slug         = g.slug,
    description  = COALESCE(g.description, ''),
    ghes_support = g.ghes_support,
    release_date = g.release_date,
    updated_at   = NOW()
FROM (
        SELECT DISTINCT ON (lower(title)) slug, title, description, ghes_support, release_date
        FROM ghc_features
        ORDER BY lower(title), slug
     ) g,
     products p
WHERE p.id = f.product_id
  AND lower(p.name) = 'github copilot'
  AND lower(f.name) = lower(g.title);

DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN SELECT id, name FROM product_features WHERE slug IS NULL ORDER BY id LOOP
        UPDATE product_features SET slug = generate_feature_slug(r.name) WHERE id = r.id;
    END LOOP;
END
$$;

ALTER TABLE product_features ALTER COLUMN slug SET NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_product_features_slug ON product_features (slug);

-- Features created later get their slug from the name.
CREATE OR REPLACE FUNCTION product_features_default_slug() RETURNS TRIGGER
LANGUAGE plpgsql AS $$
BEGIN
    IF NEW.slug IS NULL OR btrim(NEW.slug) = '' THEN
        NEW.slug := generate_feature_slug(NEW.name);
    END IF;

    RETURN NEW;
END
$$;

DROP TRIGGER IF EXISTS trg_product_features_slug ON product_features;
CREATE TRIGGER trg_product_features_slug
    BEFORE INSERT ON product_features
    FOR EACH ROW EXECUTE FUNCTION product_features_default_slug();

-- The content (videos, posts) a feature links to. One link can be the thumbnail shown when the feature card is expanded.
CREATE TABLE IF NOT EXISTS product_feature_content (
    feature_id      BIGINT  NOT NULL REFERENCES product_features (id) ON DELETE CASCADE,
    collection_name TEXT    NOT NULL,
    item_slug       TEXT    NOT NULL,
    is_thumbnail    BOOLEAN NOT NULL DEFAULT FALSE,
    sort_order      INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (feature_id, collection_name, item_slug),
    FOREIGN KEY (collection_name, item_slug) REFERENCES content_items (collection_name, slug) ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_feature_content_thumbnail
    ON product_feature_content (feature_id) WHERE is_thumbnail;

INSERT INTO product_feature_content (feature_id, collection_name, item_slug, is_thumbnail, sort_order)
SELECT f.id, l.collection_name, l.item_slug, FALSE, l.sort_order
FROM ghc_feature_content l
JOIN ghc_features g ON g.slug = l.feature_slug
JOIN product_features f ON lower(f.name) = lower(g.title)
JOIN products p ON p.id = f.product_id AND lower(p.name) = 'github copilot'
ON CONFLICT DO NOTHING;

UPDATE product_feature_content c
SET is_thumbnail = TRUE
FROM (
        SELECT DISTINCT ON (f.id) f.id AS feature_id, l.collection_name, l.item_slug
        FROM ghc_feature_content l
        JOIN ghc_features g ON g.slug = l.feature_slug
        JOIN product_features f ON lower(f.name) = lower(g.title)
        JOIN products p ON p.id = f.product_id AND lower(p.name) = 'github copilot'
        WHERE l.is_thumbnail
        ORDER BY f.id, g.slug
     ) t
WHERE c.feature_id = t.feature_id
  AND c.collection_name = t.collection_name
  AND c.item_slug = t.item_slug;
