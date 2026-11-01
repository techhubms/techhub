-- Curated features per product. The AI returns the feature of a timeline event as free text (content_timeline_events.feature,
-- which stays as it is); a feature belongs to a product and has aliases, so "Agent mode" and "agent-mode" are one feature and
-- the events of a feature across surfaces and plans can be compared.
--
--   product_features          the curated features of a product, unique per product ignoring case.
--   product_feature_aliases   other names for a feature, unique per feature ignoring case.
--   content_timeline_events.feature_id  the feature an event matches. The feature of the event's product is used, then
--                             the features of its parents (a feature defined on GitHub Copilot matches events of GitHub Copilot CLI).
CREATE TABLE IF NOT EXISTS product_features (
    id         BIGSERIAL PRIMARY KEY,
    product_id BIGINT NOT NULL REFERENCES products (id) ON DELETE CASCADE,
    name       TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_features_name ON product_features (product_id, lower(name));

CREATE TABLE IF NOT EXISTS product_feature_aliases (
    id         BIGSERIAL PRIMARY KEY,
    feature_id BIGINT NOT NULL REFERENCES product_features (id) ON DELETE CASCADE,
    alias      TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_feature_aliases_alias ON product_feature_aliases (feature_id, lower(alias));

ALTER TABLE content_timeline_events ADD COLUMN IF NOT EXISTS feature_id BIGINT REFERENCES product_features (id) ON DELETE SET NULL;
CREATE INDEX IF NOT EXISTS idx_timeline_events_feature_id ON content_timeline_events (feature_id);

-- The feature of a product (or of one of its parents, nearest first) that a name belongs to, or NULL.
CREATE OR REPLACE FUNCTION find_feature_id(p_product_id BIGINT, feature_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    WITH RECURSIVE chain (id, depth) AS (
        SELECT p_product_id, 0
        UNION ALL
        SELECT p.parent_product_id, c.depth + 1
        FROM products p JOIN chain c ON p.id = c.id
        WHERE p.parent_product_id IS NOT NULL AND c.depth < 10
    )
    SELECT m.feature_id
    FROM (
        SELECT f.id AS feature_id, c.depth
        FROM chain c JOIN product_features f ON f.product_id = c.id
        WHERE lower(f.name) = catalog_key(feature_name)
        UNION ALL
        SELECT f.id, c.depth
        FROM chain c
        JOIN product_features f ON f.product_id = c.id
        JOIN product_feature_aliases a ON a.feature_id = f.id
        WHERE lower(a.alias) = catalog_key(feature_name)
    ) m
    ORDER BY m.depth
    LIMIT 1
$$;

-- Links timeline events to the catalog: the product, its feature and the qualifiers. Without arguments it covers all events.
CREATE OR REPLACE FUNCTION relink_timeline_events(p_collection TEXT DEFAULT NULL, p_slug TEXT DEFAULT NULL) RETURNS VOID
LANGUAGE sql AS $$
    UPDATE content_timeline_events e
    SET product_id = find_product_id(e.product)
    WHERE (p_collection IS NULL OR e.collection_name = p_collection)
      AND (p_slug IS NULL OR e.item_slug = p_slug)
      AND e.product_id IS DISTINCT FROM find_product_id(e.product);

    UPDATE content_timeline_events e
    SET feature_id = find_feature_id(e.product_id, e.feature)
    WHERE (p_collection IS NULL OR e.collection_name = p_collection)
      AND (p_slug IS NULL OR e.item_slug = p_slug)
      AND e.feature_id IS DISTINCT FROM find_feature_id(e.product_id, e.feature);

    DELETE FROM content_timeline_event_qualifiers eq
    USING content_timeline_events e
    WHERE eq.event_id = e.id
      AND (p_collection IS NULL OR e.collection_name = p_collection)
      AND (p_slug IS NULL OR e.item_slug = p_slug);

    INSERT INTO content_timeline_event_qualifiers (event_id, qualifier_id)
    SELECT DISTINCT e.id, q.qualifier_id
    FROM content_timeline_events e
    CROSS JOIN LATERAL (
        SELECT find_product_qualifier_id(e.product) AS qualifier_id
        UNION ALL
        SELECT find_qualifier_id('plan', x) FROM unnest(e.plans) AS x
        UNION ALL
        SELECT find_qualifier_id('surface', x) FROM unnest(e.surfaces) AS x
    ) q
    WHERE q.qualifier_id IS NOT NULL
      AND (p_collection IS NULL OR e.collection_name = p_collection)
      AND (p_slug IS NULL OR e.item_slug = p_slug);
$$;

-- Starting set: the features on the curated GitHub Copilot features page.
INSERT INTO product_features (product_id, name)
SELECT p.id, f.title
FROM ghc_features f
JOIN products p ON lower(p.name) = 'github copilot'
ON CONFLICT (product_id, lower(name)) DO NOTHING;

SELECT relink_timeline_events();
