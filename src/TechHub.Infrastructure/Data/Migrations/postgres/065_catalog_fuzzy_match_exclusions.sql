-- Ambiguous names must stay reviewable, unlike ignored names that are not products.
CREATE TABLE catalog_fuzzy_match_exclusions (
    match_key TEXT PRIMARY KEY
);

INSERT INTO catalog_fuzzy_match_exclusions (match_key)
VALUES (catalog_match_key('DocumentDB'));

CREATE OR REPLACE FUNCTION find_product_id(product_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    SELECT COALESCE(
        (SELECT p.id FROM products p WHERE lower(p.name) = catalog_key(product_name)),
        (SELECT a.product_id FROM product_aliases a WHERE lower(a.alias) = catalog_key(product_name)),
        (SELECT CASE WHEN count(DISTINCT m.product_id) = 1 THEN min(m.product_id) END
         FROM (
             SELECT f.product_id FROM product_features f WHERE lower(f.name) = catalog_key(product_name)
             UNION ALL
             SELECT f.product_id FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
             WHERE lower(a.alias) = catalog_key(product_name)
         ) m),
        CASE WHEN NOT EXISTS (
            SELECT 1 FROM catalog_fuzzy_match_exclusions x WHERE x.match_key = catalog_match_key(product_name)
        ) THEN COALESCE(find_product_id_normalized(product_name), find_product_id_by_phrase(product_name)) END)
$$;

-- Repair links created by earlier migrations without touching unrelated or catalog-authored entries.
WITH resolved AS (
    SELECT l.collection_name, l.item_slug, l.product_name, l.feature_name,
           CASE WHEN l.feature_name = '' THEN find_product_id(l.product_name)
                ELSE (SELECT r.out_product_id FROM resolve_event_catalog(l.product_name, l.feature_name) r)
           END AS product_id
    FROM content_item_products l
    WHERE catalog_match_key(l.product_name) IN (SELECT match_key FROM catalog_fuzzy_match_exclusions)
)
UPDATE content_item_products l
SET product_id = r.product_id
FROM resolved r
WHERE (l.collection_name, l.item_slug, l.product_name, l.feature_name)
    = (r.collection_name, r.item_slug, r.product_name, r.feature_name)
  AND l.product_id IS DISTINCT FROM r.product_id;

WITH resolved AS (
    SELECT e.id, r.out_product_id, r.out_feature_id
    FROM content_timeline_events e
    CROSS JOIN LATERAL resolve_event_catalog(e.product, e.feature) r
    WHERE e.extracted_from <> 'catalog'
      AND catalog_match_key(e.product) IN (SELECT match_key FROM catalog_fuzzy_match_exclusions)
)
UPDATE content_timeline_events e
SET product_id = r.out_product_id, feature_id = r.out_feature_id
FROM resolved r
WHERE e.id = r.id
  AND (e.product_id, e.feature_id) IS DISTINCT FROM (r.out_product_id, r.out_feature_id);
