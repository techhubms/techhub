-- Keep the matching tiers unchanged, but evaluate normalization once per input or candidate.
-- Inlining these expressions repeats regular-expression work for every comparison in review queries.
ALTER TABLE products ADD COLUMN match_key TEXT GENERATED ALWAYS AS (catalog_match_key(name)) STORED;
ALTER TABLE product_aliases ADD COLUMN match_key TEXT GENERATED ALWAYS AS (catalog_match_key(alias)) STORED;
ALTER TABLE product_features ADD COLUMN match_key TEXT GENERATED ALWAYS AS (catalog_match_key(name)) STORED;
ALTER TABLE product_feature_aliases ADD COLUMN match_key TEXT GENERATED ALWAYS AS (catalog_match_key(alias)) STORED;

CREATE INDEX idx_products_catalog_match_key ON products (match_key);
CREATE INDEX idx_product_aliases_catalog_match_key ON product_aliases (match_key);
CREATE INDEX idx_product_features_catalog_match_key ON product_features (match_key);
CREATE INDEX idx_product_feature_aliases_catalog_match_key ON product_feature_aliases (match_key);
CREATE INDEX idx_product_features_name ON product_features (lower(name));
CREATE INDEX idx_product_feature_aliases_alias ON product_feature_aliases (lower(alias));

CREATE OR REPLACE FUNCTION find_product_id_normalized(product_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    WITH k AS MATERIALIZED (
        SELECT catalog_match_key(product_name) AS key,
               regexp_replace(catalog_match_key(product_name), '^(the|microsoft)\s+(?=\S)', '') AS short_key
    ),
    matches AS (
        SELECT p.id AS product_id FROM products p, k WHERE k.key <> '' AND p.match_key IN (k.key, k.short_key)
        UNION
        SELECT a.product_id FROM product_aliases a, k WHERE k.key <> '' AND a.match_key IN (k.key, k.short_key)
        UNION
        SELECT f.product_id FROM product_features f, k WHERE k.key <> '' AND f.match_key IN (k.key, k.short_key)
        UNION
        SELECT f.product_id FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id, k
        WHERE k.key <> '' AND a.match_key IN (k.key, k.short_key)
    )
    SELECT CASE WHEN count(*) = 1 THEN min(product_id) END FROM matches
$$;

CREATE OR REPLACE FUNCTION find_product_id_by_phrase(product_name TEXT) RETURNS BIGINT
LANGUAGE plpgsql STABLE AS $$
DECLARE
    v_key  TEXT := catalog_match_key(product_name);
    v_stem TEXT;
    v_n    INT;
    v_ids  BIGINT[];
BEGIN
    v_stem := btrim(regexp_replace(v_key, '(\s+v?\d+(\.\d+)*)+$', ''));
    IF v_stem = '' THEN
        v_stem := v_key;
    END IF;
    IF v_stem = '' THEN
        RETURN NULL;
    END IF;
    v_n := cardinality(string_to_array(v_stem, ' '));

    IF v_stem <> v_key THEN
        SELECT array_agg(DISTINCT x.product_id) INTO v_ids
        FROM (
            SELECT p.id AS product_id, p.match_key AS k FROM products p
            UNION ALL
            SELECT a.product_id, a.match_key FROM product_aliases a
        ) x
        WHERE x.k = v_stem;

        IF v_ids IS NOT NULL THEN
            RETURN pick_product(v_ids);
        END IF;
    END IF;

    WITH names AS MATERIALIZED (
        SELECT p.id AS product_id, p.match_key AS k FROM products p
        UNION ALL
        SELECT a.product_id, a.match_key FROM product_aliases a
    )
    SELECT array_agg(DISTINCT x.product_id) INTO v_ids
    FROM names x
    WHERE x.k <> v_stem AND right(' ' || x.k, length(v_stem) + 1) = ' ' || v_stem;

    IF v_ids IS NOT NULL THEN
        RETURN pick_product(v_ids);
    END IF;

    IF v_n >= 2 THEN
        WITH names AS MATERIALIZED (
            SELECT p.id AS product_id, p.match_key AS k FROM products p
            UNION ALL
            SELECT a.product_id, a.match_key FROM product_aliases a
        ),
        hits AS (
            SELECT x.product_id, cardinality(string_to_array(x.k, ' ')) AS n
            FROM names x
            WHERE x.k <> '' AND x.k <> v_stem
              AND position(' ' || x.k || ' ' IN ' ' || v_stem || ' ') > 0
              AND cardinality(string_to_array(x.k, ' '))::numeric / v_n >= 0.6
        )
        SELECT array_agg(DISTINCT h.product_id) INTO v_ids FROM hits h WHERE h.n = (SELECT max(n) FROM hits);

        RETURN pick_product(v_ids);
    END IF;

    RETURN NULL;
END;
$$;

CREATE OR REPLACE FUNCTION find_catalog_entry_in_product(p_product_id BIGINT, p_product_text TEXT, p_feature_text TEXT)
RETURNS TABLE (entry_kind TEXT, entry_product_id BIGINT, entry_feature_id BIGINT)
LANGUAGE sql STABLE AS $$
    WITH txt AS MATERIALIZED (
        SELECT ' ' || catalog_flatten(p_product_text, p_feature_text) || ' ' AS flat,
               ' ' || catalog_match_key(p_product_text) || ' ' AS own,
               GREATEST(catalog_phrase_size(p_feature_text), 1) AS feature_words
    ),
    below AS (SELECT product_tree_ids(p_product_id) AS id),
    above AS (
        WITH RECURSIVE up (id) AS (
            SELECT p_product_id
            UNION
            SELECT p.parent_product_id FROM products p JOIN up ON p.id = up.id WHERE p.parent_product_id IS NOT NULL
        )
        SELECT id FROM up
    ),
    entries AS MATERIALIZED (
        SELECT 'product'::text AS kind, p.id AS product_id, NULL::bigint AS feature_id, p.match_key AS k
        FROM products p
        WHERE p.id IN (SELECT id FROM below) AND p.id <> p_product_id
        UNION ALL
        SELECT 'product', a.product_id, NULL, a.match_key
        FROM product_aliases a
        WHERE a.product_id IN (SELECT id FROM below) AND a.product_id <> p_product_id
        UNION ALL
        SELECT 'feature', f.product_id, f.id, f.match_key
        FROM product_features f
        WHERE f.product_id IN (SELECT id FROM below UNION SELECT id FROM above)
        UNION ALL
        SELECT 'feature', f.product_id, f.id, a.match_key
        FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
        WHERE f.product_id IN (SELECT id FROM below UNION SELECT id FROM above)
    ),
    hits AS (
        SELECT e.kind, e.product_id, e.feature_id, cardinality(string_to_array(e.k, ' ')) AS n
        FROM entries e, txt
        WHERE e.k <> ''
          AND position(' ' || e.k || ' ' IN txt.flat) > 0
          AND position(' ' || e.k || ' ' IN txt.own) = 0
    ),
    eligible AS (
        SELECT h.* FROM hits h, txt
        WHERE h.n >= 2 OR h.n::numeric / txt.feature_words >= 0.5
    ),
    best AS (
        SELECT DISTINCT kind, product_id, feature_id FROM eligible WHERE n = (SELECT max(n) FROM eligible)
    )
    SELECT b.kind, b.product_id, b.feature_id FROM best b WHERE (SELECT count(*) FROM best) = 1
$$;
