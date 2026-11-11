-- Product catalog: products, their aliases, parents and features, and the links from articles and timeline events to them.
--
-- The AI returns product and feature names as free text (ai_metadata.products and content_timeline_events.product/feature). That text
-- stays as it is. The catalog is a curated layer on top: names are linked to it by SQL (never by an AI call, and the AI never sees
-- the catalog), and the links are rebuilt when an article is written and whenever the catalog changes (relink_item_products and
-- relink_timeline_events).
--
-- What is a product and what is a feature (the same rule as in content-catalog-rules.md, which the prompts use):
--
--   A product is something you can use, buy, install or deploy by itself ("can I use this by itself?"). A feature is something you only
--   get as part of a product: a capability, a mode, a model you pick inside the product, a part of a bundle, a billing concept. You do not
--   buy secret scanning, you buy GitHub Advanced Security and secret scanning is one of its features; you do not say that a pipeline is
--   built with GitHub-hosted runners, it is built with GitHub Actions. A product can belong to a parent product (GitHub Actions to
--   GitHub) when it is a separate product of the same family. Plans, surfaces (IDEs, clients) and editions are neither: they are not
--   catalogued, they belong in the description of an event.
--
-- This migration builds the starting catalog. Later numbered migrations seed reviewed names and correct classifications.
-- Validate them in isolated databases before restoring production and letting application startup apply pending migrations.
--
--   1. Tables
--   2. Matching functions
--   3. Starting catalog: products, parents, aliases and names that are not products
--   4. The GitHub Copilot features page: the dated features become events of the timeline, the rest become features
--   5. Features of the other products and the names of the features
--   6. Links of the existing articles and events

-- ── 1. Tables ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────

CREATE TABLE IF NOT EXISTS products (
    id                BIGSERIAL PRIMARY KEY,
    name              TEXT NOT NULL,
    parent_product_id BIGINT REFERENCES products (id) ON DELETE SET NULL CHECK (parent_product_id IS NULL OR parent_product_id <> id),
    created_at        TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at        TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_products_name ON products (lower(name));
CREATE INDEX IF NOT EXISTS idx_products_parent ON products (parent_product_id);

-- Other names for a product (old names, short forms). A name belongs to one product.
CREATE TABLE IF NOT EXISTS product_aliases (
    id         BIGSERIAL PRIMARY KEY,
    product_id BIGINT NOT NULL REFERENCES products (id) ON DELETE CASCADE,
    alias      TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_aliases_alias ON product_aliases (lower(alias));
CREATE INDEX IF NOT EXISTS idx_product_aliases_product ON product_aliases (product_id);

-- Names the AI returns that are not products (an event, a program, another vendor's model), so they stay out of the review queue.
CREATE TABLE IF NOT EXISTS ignored_product_names (
    id         BIGSERIAL PRIMARY KEY,
    name       TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_ignored_product_names_name ON ignored_product_names (lower(name));

-- The product names an article lists, in classifier order (0 is the main product), with the product they match. product_id is NULL
-- while the name is not in the catalog: that is the review queue.
CREATE TABLE IF NOT EXISTS content_item_products (
    collection_name TEXT NOT NULL,
    item_slug       TEXT NOT NULL,
    product_name    TEXT NOT NULL,
    position        INTEGER NOT NULL,
    product_id      BIGINT REFERENCES products (id) ON DELETE SET NULL,
    PRIMARY KEY (collection_name, item_slug, product_name),
    FOREIGN KEY (collection_name, item_slug)
        REFERENCES content_items (collection_name, slug) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_content_item_products_product ON content_item_products (product_id);
CREATE INDEX IF NOT EXISTS idx_content_item_products_name ON content_item_products (lower(product_name));

-- The capabilities of a product. A slug is the readable address of a feature (generated from the name), release_date is epoch
-- seconds and optional.
CREATE TABLE IF NOT EXISTS product_features (
    id           BIGSERIAL PRIMARY KEY,
    product_id   BIGINT NOT NULL REFERENCES products (id) ON DELETE CASCADE,
    name         TEXT NOT NULL,
    slug         TEXT NOT NULL,
    description  TEXT NOT NULL DEFAULT '',
    release_date BIGINT,
    created_at   TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at   TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_features_name ON product_features (product_id, lower(name));
CREATE UNIQUE INDEX IF NOT EXISTS ux_product_features_slug ON product_features (slug);

CREATE TABLE IF NOT EXISTS product_feature_aliases (
    id         BIGSERIAL PRIMARY KEY,
    feature_id BIGINT NOT NULL REFERENCES product_features (id) ON DELETE CASCADE,
    alias      TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_feature_aliases_alias ON product_feature_aliases (feature_id, lower(alias));

-- The content (videos, posts) a feature links to. One link can be the thumbnail.
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

ALTER TABLE content_timeline_events ADD COLUMN IF NOT EXISTS product_id BIGINT REFERENCES products (id) ON DELETE SET NULL;
ALTER TABLE content_timeline_events ADD COLUMN IF NOT EXISTS feature_id BIGINT REFERENCES product_features (id) ON DELETE SET NULL;
CREATE INDEX IF NOT EXISTS idx_timeline_events_product_id ON content_timeline_events (product_id);
CREATE INDEX IF NOT EXISTS idx_timeline_events_feature_id ON content_timeline_events (feature_id);

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

-- Features created without a slug get one from the name.
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

-- ── 2. Matching functions ────────────────────────────────────────────────────────────────────────────────────────────────────────
-- A name is matched in tiers, each one stricter in what it forgives: exact name or alias, the name of a feature (the product of the
-- feature), the normalized name (case, punctuation, a trailing 'Service', a leading 'Microsoft'), and phrases (missing or extra words
-- around a name). A name that fits several unrelated products links to none of them.

-- A name without surrounding or repeated whitespace, in lower case.
CREATE OR REPLACE FUNCTION catalog_key(value TEXT) RETURNS TEXT
LANGUAGE sql IMMUTABLE AS $$
    SELECT lower(regexp_replace(btrim(value), '\s+', ' ', 'g'))
$$;

-- A name in lower case, without parentheses, trailing 'Service(s)' and punctuation (the characters of names like '.NET', 'C#' and
-- 'Oracle AI Database@Azure' are kept).
CREATE OR REPLACE FUNCTION catalog_match_key(value TEXT) RETURNS TEXT
LANGUAGE sql IMMUTABLE AS $$
    SELECT btrim(regexp_replace(
        regexp_replace(
            regexp_replace(
                regexp_replace(lower(COALESCE(value, '')), '\([^)]*\)', ' ', 'g'),
                '&', ' and ', 'g'),
            '[^[:alnum:].#+@]+', ' ', 'g'),
        '\s+services?$|[.\s]+$', '', 'g'))
$$;

-- The number of words of a name once normalized.
CREATE OR REPLACE FUNCTION catalog_phrase_size(value TEXT) RETURNS INT
LANGUAGE sql IMMUTABLE AS $$
    SELECT COALESCE(cardinality(string_to_array(NULLIF(catalog_match_key(value), ''), ' ')), 0)
$$;

-- The product and all of its descendants. UNION (not UNION ALL) keeps the walk finite even for bad data.
CREATE OR REPLACE FUNCTION product_tree_ids(root_id BIGINT) RETURNS SETOF BIGINT
LANGUAGE sql STABLE AS $$
    WITH RECURSIVE tree (id) AS (
        SELECT root_id
        UNION
        SELECT p.id FROM products p JOIN tree t ON p.parent_product_id = t.id
    )
    SELECT id FROM tree
$$;

-- The one product that stands for a set of products: the only one, or the one that is the parent (or the same) of all the others.
-- NULL when there are several unrelated products, so a name that fits many products does not link to any of them.
CREATE OR REPLACE FUNCTION pick_product(p_ids BIGINT[]) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    SELECT c
    FROM unnest(p_ids) AS c
    WHERE NOT EXISTS (SELECT 1 FROM unnest(p_ids) AS d WHERE d NOT IN (SELECT product_tree_ids(c)))
    LIMIT 1
$$;

-- The product text and the feature text as one normalized text. When the product text ends with the words that the feature text
-- starts with ('GitHub Copilot' and 'Copilot Fusion') they are written once.
CREATE OR REPLACE FUNCTION catalog_flatten(p_product TEXT, p_feature TEXT) RETURNS TEXT
LANGUAGE sql IMMUTABLE AS $$
    WITH t AS (
        SELECT COALESCE(string_to_array(NULLIF(catalog_match_key(p_product), ''), ' '), '{}'::text[]) AS a,
               COALESCE(string_to_array(NULLIF(catalog_match_key(p_feature), ''), ' '), '{}'::text[]) AS b
    ),
    k AS (
        SELECT COALESCE(max(g), 0) AS overlap
        FROM t, generate_series(1, LEAST(cardinality(t.a), cardinality(t.b))) AS g
        WHERE t.a[cardinality(t.a) - g + 1 : cardinality(t.a)] = t.b[1 : g]
    )
    SELECT array_to_string(t.a || t.b[k.overlap + 1 : cardinality(t.b)], ' ') FROM t, k
$$;

CREATE OR REPLACE FUNCTION find_product_id_normalized(product_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    WITH k AS (
        SELECT catalog_match_key(product_name) AS key,
               regexp_replace(catalog_match_key(product_name), '^(the|microsoft)\s+(?=\S)', '') AS short_key
    ),
    matches AS (
        SELECT p.id AS product_id FROM products p, k WHERE k.key <> '' AND catalog_match_key(p.name) IN (k.key, k.short_key)
        UNION
        SELECT a.product_id FROM product_aliases a, k WHERE k.key <> '' AND catalog_match_key(a.alias) IN (k.key, k.short_key)
        UNION
        SELECT f.product_id FROM product_features f, k WHERE k.key <> '' AND catalog_match_key(f.name) IN (k.key, k.short_key)
        UNION
        SELECT f.product_id FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id, k
        WHERE k.key <> '' AND catalog_match_key(a.alias) IN (k.key, k.short_key)
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

    -- The name without its release version.
    IF v_stem <> v_key THEN
        SELECT array_agg(DISTINCT x.product_id) INTO v_ids
        FROM (
            SELECT p.id AS product_id, catalog_match_key(p.name) AS k FROM products p
            UNION ALL
            SELECT a.product_id, catalog_match_key(a.alias) FROM product_aliases a
        ) x
        WHERE x.k = v_stem;

        IF v_ids IS NOT NULL THEN
            RETURN pick_product(v_ids);
        END IF;
    END IF;

    -- Words are missing at the start: the name is the end of the name or alias of a product, the part without the vendor
    -- ('Surface Laptop Ultra' of 'Microsoft Surface Laptop Ultra'). A phrase in the middle or at the start of a longer name is
    -- not enough: 'Active Directory' is a product of its own, not the start of 'Microsoft Active Directory Certificate Services'.
    SELECT array_agg(DISTINCT x.product_id) INTO v_ids
    FROM (
        SELECT p.id AS product_id, catalog_match_key(p.name) AS k FROM products p
        UNION ALL
        SELECT a.product_id, catalog_match_key(a.alias) FROM product_aliases a
    ) x
    WHERE x.k <> v_stem AND right(' ' || x.k, length(v_stem) + 1) = ' ' || v_stem;

    IF v_ids IS NOT NULL THEN
        RETURN pick_product(v_ids);
    END IF;

    -- Extra words: a name or alias of a product is a phrase inside the name and covers most of it.
    IF v_n >= 2 THEN
        WITH hits AS (
            SELECT x.product_id, cardinality(string_to_array(x.k, ' ')) AS n
            FROM (
                SELECT p.id AS product_id, catalog_match_key(p.name) AS k FROM products p
                UNION ALL
                SELECT a.product_id, catalog_match_key(a.alias) FROM product_aliases a
            ) x
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

-- The product a name belongs to: by product name or alias, else by the name or alias of a feature (the product of the feature,
-- only when the name belongs to the features of one product), else by the normalized name, else by the phrase.
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
        find_product_id_normalized(product_name),
        find_product_id_by_phrase(product_name))
$$;

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

-- The feature a product name belongs to ('GitHub Copilot coding agent' is the feature Coding agent), or NULL when no feature or
-- several features have that name.
CREATE OR REPLACE FUNCTION find_feature_id_by_name(feature_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    SELECT CASE WHEN count(DISTINCT m.feature_id) = 1 THEN min(m.feature_id) END
    FROM (
        SELECT f.id AS feature_id FROM product_features f WHERE lower(f.name) = catalog_key(feature_name)
        UNION ALL
        SELECT a.feature_id FROM product_feature_aliases a WHERE lower(a.alias) = catalog_key(feature_name)
    ) m
$$;

-- The one child product or feature, inside the product, whose name or alias occurs as a phrase in the product text and the feature
-- text together. NULL when there is none or when several are equally long.
CREATE OR REPLACE FUNCTION find_catalog_entry_in_product(p_product_id BIGINT, p_product_text TEXT, p_feature_text TEXT)
RETURNS TABLE (entry_kind TEXT, entry_product_id BIGINT, entry_feature_id BIGINT)
LANGUAGE sql STABLE AS $$
    WITH txt AS (
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
    entries AS (
        SELECT 'product'::text AS kind, p.id AS product_id, NULL::bigint AS feature_id, catalog_match_key(p.name) AS k
        FROM products p
        WHERE p.id IN (SELECT id FROM below) AND p.id <> p_product_id
        UNION ALL
        SELECT 'product', a.product_id, NULL, catalog_match_key(a.alias)
        FROM product_aliases a
        WHERE a.product_id IN (SELECT id FROM below) AND a.product_id <> p_product_id
        UNION ALL
        SELECT 'feature', f.product_id, f.id, catalog_match_key(f.name)
        FROM product_features f
        WHERE f.product_id IN (SELECT id FROM below UNION SELECT id FROM above)
        UNION ALL
        SELECT 'feature', f.product_id, f.id, catalog_match_key(a.alias)
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

-- The product and the feature that the product text and the feature text of an event belong to.
CREATE OR REPLACE FUNCTION resolve_event_catalog(p_product TEXT, p_feature TEXT)
RETURNS TABLE (out_product_id BIGINT, out_feature_id BIGINT)
LANGUAGE plpgsql STABLE AS $$
DECLARE
    v_product_id BIGINT := find_product_id(p_product);
    v_feature_id BIGINT;
    v_hit        RECORD;
BEGIN
    -- An event without a feature text gets the feature that its product name belongs to, when the name is not a product.
    IF btrim(COALESCE(p_feature, '')) = '' THEN
        IF EXISTS (SELECT 1 FROM products p WHERE lower(p.name) = catalog_key(p_product))
           OR EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = catalog_key(p_product)) THEN
            v_feature_id := NULL;
        ELSE
            v_feature_id := find_feature_id_by_name(p_product);
        END IF;

        RETURN QUERY SELECT v_product_id, v_feature_id;
        RETURN;
    END IF;

    v_feature_id := find_feature_id(v_product_id, p_feature);

    IF v_feature_id IS NULL AND v_product_id IS NOT NULL THEN
        SELECT h.entry_kind, h.entry_product_id, h.entry_feature_id INTO v_hit
        FROM find_catalog_entry_in_product(v_product_id, p_product, p_feature) h;

        IF v_hit.entry_kind = 'product' THEN
            v_product_id := v_hit.entry_product_id;

            -- The words that follow the child product may name a feature of it ('Data Warehouse' + 'result caching').
            SELECT h.entry_kind, h.entry_product_id, h.entry_feature_id INTO v_hit
            FROM find_catalog_entry_in_product(v_product_id, (SELECT p.name FROM products p WHERE p.id = v_product_id), p_feature) h;

            IF v_hit.entry_kind = 'feature' THEN
                v_feature_id := v_hit.entry_feature_id;
                IF v_hit.entry_product_id IN (SELECT product_tree_ids(v_product_id)) THEN
                    v_product_id := v_hit.entry_product_id;
                END IF;
            END IF;
        ELSIF v_hit.entry_kind = 'feature' THEN
            v_feature_id := v_hit.entry_feature_id;
            IF v_hit.entry_product_id IN (SELECT product_tree_ids(v_product_id)) THEN
                v_product_id := v_hit.entry_product_id;
            END IF;
        END IF;
    END IF;

    RETURN QUERY SELECT v_product_id, v_feature_id;
END;
$$;

CREATE OR REPLACE FUNCTION relink_item_products(p_collection TEXT DEFAULT NULL, p_slug TEXT DEFAULT NULL) RETURNS VOID
LANGUAGE sql AS $$
    UPDATE content_item_products l
    SET product_id = find_product_id(l.product_name)
    WHERE (p_collection IS NULL OR l.collection_name = p_collection)
      AND (p_slug IS NULL OR l.item_slug = p_slug)
      AND l.product_id IS DISTINCT FROM find_product_id(l.product_name);
$$;

-- Links timeline events to the catalog: the product and its feature. Without arguments it covers all events.
CREATE OR REPLACE FUNCTION relink_timeline_events(p_collection TEXT DEFAULT NULL, p_slug TEXT DEFAULT NULL) RETURNS VOID
LANGUAGE sql AS $$
    WITH resolved AS (
        SELECT e.id, r.out_product_id, r.out_feature_id
        FROM content_timeline_events e
        CROSS JOIN LATERAL resolve_event_catalog(e.product, e.feature) r
        WHERE (p_collection IS NULL OR e.collection_name = p_collection)
          AND (p_slug IS NULL OR e.item_slug = p_slug)
    )
    UPDATE content_timeline_events e
    SET product_id = r.out_product_id, feature_id = r.out_feature_id
    FROM resolved r
    WHERE e.id = r.id
      AND (e.product_id IS DISTINCT FROM r.out_product_id OR e.feature_id IS DISTINCT FROM r.out_feature_id);
$$;

-- Turns a child product into a feature of its parent and returns the id of the feature, or NULL when the product does not
-- exist or has no parent. The feature is named like the product without the name of the parent ('GitHub Copilot coding agent'
-- becomes 'Coding agent'); a feature of the parent with that name or alias is reused. The name and the aliases of the product
-- become aliases of the feature, and its own children and features move to the parent. The caller relinks the articles and events.
CREATE OR REPLACE FUNCTION convert_product_to_feature(p_product_id BIGINT) RETURNS BIGINT
LANGUAGE plpgsql AS $$
DECLARE
    v_parent_id    BIGINT;
    v_name         TEXT;
    v_parent_name  TEXT;
    v_feature_name TEXT;
    v_feature_id   BIGINT;
BEGIN
    SELECT c.parent_product_id, c.name, p.name
    INTO v_parent_id, v_name, v_parent_name
    FROM products c JOIN products p ON p.id = c.parent_product_id
    WHERE c.id = p_product_id;

    IF v_parent_id IS NULL THEN
        RETURN NULL;
    END IF;

    v_feature_name := btrim(v_name);
    IF lower(v_feature_name) LIKE lower(v_parent_name) || ' %' THEN
        v_feature_name := btrim(substr(v_feature_name, length(v_parent_name) + 1));
    END IF;
    v_feature_name := upper(left(v_feature_name, 1)) || substr(v_feature_name, 2);

    SELECT m.id INTO v_feature_id
    FROM (
        SELECT f.id FROM product_features f
        WHERE f.product_id = v_parent_id AND lower(f.name) = catalog_key(v_feature_name)
        UNION ALL
        SELECT f.id FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
        WHERE f.product_id = v_parent_id AND lower(a.alias) = catalog_key(v_feature_name)
    ) m
    LIMIT 1;

    IF v_feature_id IS NULL THEN
        INSERT INTO product_features (product_id, name) VALUES (v_parent_id, v_feature_name) RETURNING id INTO v_feature_id;
    END IF;

    -- The names of the product are names of the feature now. A name that is a feature of the parent already is skipped.
    INSERT INTO product_feature_aliases (feature_id, alias)
    SELECT v_feature_id, n.alias
    FROM (
        SELECT v_name AS alias
        UNION
        SELECT a.alias FROM product_aliases a WHERE a.product_id = p_product_id
    ) n
    WHERE lower(n.alias) <> lower((SELECT name FROM product_features WHERE id = v_feature_id))
      AND NOT EXISTS (
          SELECT 1 FROM product_features x WHERE x.product_id = v_parent_id AND lower(x.name) = lower(n.alias))
    ON CONFLICT DO NOTHING;

    -- Features and children that the parent does not have by name move to the parent.
    DELETE FROM product_aliases WHERE product_id = p_product_id;

    UPDATE product_features s
    SET product_id = v_parent_id
    WHERE s.product_id = p_product_id
      AND s.id <> v_feature_id
      AND NOT EXISTS (
          SELECT 1 FROM product_features t
          WHERE t.product_id = v_parent_id AND lower(t.name) = lower(s.name));

    UPDATE products SET parent_product_id = v_parent_id WHERE parent_product_id = p_product_id;

    DELETE FROM products WHERE id = p_product_id;

    RETURN v_feature_id;
END;
$$;

-- ── 3. Starting catalog ─────────────────────────────────────────────────────────────────────────────────────────────────────────
-- The products of the Microsoft developer ecosystem that the articles are about: the products named in the content and the families
-- they belong to. Every name is skipped when it exists already, so this can be repeated.
--
-- Hierarchy: a product belongs to a parent when it is a separate product of the same family (GitHub Actions to GitHub, Azure Functions to
-- Microsoft Azure). Everything that is or was called Azure is part of Microsoft Azure. A parent shows the articles and dates of its children.
CREATE TEMP TABLE catalog_seed_products (name TEXT NOT NULL, parent TEXT);

INSERT INTO catalog_seed_products (name, parent) VALUES
    ('.NET', NULL),
    ('.NET Aspire', '.NET'),
    ('.NET MAUI', '.NET'),
    ('.NET MAUI CLI', '.NET MAUI'),
    ('ASP.NET Core', '.NET'),
    ('BenchmarkDotNet', '.NET'),
    ('C#', '.NET'),
    ('Entity Framework Core', '.NET'),
    ('F#', '.NET'),
    ('MSBuild', '.NET'),
    ('NuGet', '.NET'),
    ('NuGet MCP Server', 'NuGet'),
    ('Orleans', '.NET'),
    ('Roslyn', '.NET'),
    ('Visual Basic', '.NET'),
    ('WPF', '.NET'),
    ('Windows Forms', '.NET'),
    ('Xamarin', '.NET'),
    ('Agent Host Protocol', NULL),
    ('Ansys CFX', NULL),
    ('Apache Artemis', NULL),
    ('Apache Hadoop', NULL),
    ('Apache Iceberg', NULL),
    ('Apache Kafka', NULL),
    ('Apache Spark', NULL),
    ('Arnis', NULL),
    ('AutoGen', NULL),
    ('Co-op Translator', NULL),
    ('Citus', 'PostgreSQL'),
    ('ComfyUI', NULL),
    ('Cursor', NULL),
    ('Dapr', NULL),
    ('Delta Lake', NULL),
    ('Dev Containers', NULL),
    ('Dev Proxy', NULL),
    ('Docker', NULL),
    ('Docker Desktop', 'Docker'),
    ('Dynamics 365', NULL),
    ('Flint', NULL),
    ('Git', NULL),
    ('GitHub', NULL),
    ('GitHub Actions', 'GitHub'),
    ('GitHub Advanced Security', 'GitHub'),
    ('GitHub Advanced Security for Azure DevOps', 'GitHub Advanced Security'),
    ('GitHub Code Security', 'GitHub Advanced Security'),
    ('CodeQL CLI', 'GitHub CodeQL'),
    ('GitHub CodeQL', 'GitHub Advanced Security'),
    ('GitHub Secret Protection', 'GitHub Advanced Security'),
    ('GitHub Agentic Workflows', 'GitHub'),
    ('GitHub CLI', 'GitHub'),
    ('GitHub Codespaces', 'GitHub'),
    ('GitHub Copilot', 'GitHub'),
    ('GitHub Copilot app', 'GitHub Copilot'),
    ('GitHub Copilot CLI', 'GitHub Copilot'),
    ('GitHub Copilot Workspace', 'GitHub Copilot'),
    ('GitHub Copilot for Azure', 'GitHub Copilot'),
    ('GitHub Dependabot', 'GitHub'),
    ('GitHub Desktop', 'GitHub'),
    ('GitHub Enterprise', 'GitHub'),
    ('GitHub Enterprise Cloud', 'GitHub Enterprise'),
    ('GitHub Enterprise Importer', 'GitHub Enterprise Cloud'),
    ('GitHub Enterprise Local', 'GitHub Enterprise'),
    ('GitHub Enterprise Server', 'GitHub Enterprise'),
    ('GitHub MCP Server', 'GitHub'),
    ('GitHub Marketplace', 'GitHub'),
    ('GitHub Mobile', 'GitHub'),
    ('GitHub Models', 'GitHub'),
    ('GitHub Packages', 'GitHub'),
    ('GitHub Pages', 'GitHub'),
    ('GitHub Spark', 'GitHub'),
    ('GitHub Sponsors', 'GitHub'),
    ('Go', NULL),
    ('Grafana', NULL),
    ('Grafana Pyroscope', 'Grafana'),
    ('HashiCorp Terraform', NULL),
    ('Hugging Face', NULL),
    ('IBM MQ', NULL),
    ('Infragistics Ignite UI for Blazor', NULL),
    ('Java', NULL),
    ('JavaScript', NULL),
    ('JetBrains Air', NULL),
    ('JetBrains IntelliJ IDEA', NULL),
    ('Kiota', NULL),
    ('Kubernetes', NULL),
    ('Helm', 'Kubernetes'),
    ('KEDA', 'Kubernetes'),
    ('LakeSail Cloud', NULL),
    ('LangChain', NULL),
    ('Lean', NULL),
    ('M5Stack StackChan', NULL),
    ('Microsoft 365', NULL),
    ('Exchange Online', 'Microsoft 365'),
    ('Microsoft 365 Copilot', 'Microsoft 365'),
    ('Microsoft Teams', 'Microsoft 365'),
    ('OneDrive', 'Microsoft 365'),
    ('Outlook', 'Microsoft 365'),
    ('SharePoint', 'Microsoft 365'),
    ('SharePoint Framework', 'SharePoint'),
    ('Microsoft AI Diagnostic Orchestrator', NULL),
    ('Microsoft Agent 365', NULL),
    ('Microsoft Agent Framework', NULL),
    ('Microsoft Azure', NULL),
    ('Azure AI Search', 'Microsoft Azure'),
    ('Azure AI Services', 'Microsoft Azure'),
    ('Azure AI Content Safety', 'Azure AI Services'),
    ('Azure AI Document Intelligence', 'Azure AI Services'),
    ('Azure AI Language', 'Azure AI Services'),
    ('Azure AI Speech', 'Azure AI Services'),
    ('Azure AI Translator', 'Azure AI Services'),
    ('Azure AI Vision', 'Azure AI Services'),
    ('Azure API Center', 'Microsoft Azure'),
    ('Azure API Management', 'Microsoft Azure'),
    ('Azure Advisor', 'Microsoft Azure'),
    ('Azure App Configuration', 'Microsoft Azure'),
    ('Azure App Service', 'Microsoft Azure'),
    ('Azure Application Gateway', 'Microsoft Azure'),
    ('Azure Arc', 'Microsoft Azure'),
    ('Azure Automation', 'Microsoft Azure'),
    ('Azure Backup', 'Microsoft Azure'),
    ('Azure Bastion', 'Microsoft Azure'),
    ('Azure Batch', 'Microsoft Azure'),
    ('Azure Bicep', 'Microsoft Azure'),
    ('Azure CDN', 'Microsoft Azure'),
    ('Azure CLI', 'Microsoft Azure'),
    ('Azure Cache for Redis', 'Microsoft Azure'),
    ('Azure Chaos Studio', 'Microsoft Azure'),
    ('Azure Cloud HSM', 'Microsoft Azure'),
    ('Azure Cloud Shell', 'Microsoft Azure'),
    ('Azure Communication Services', 'Microsoft Azure'),
    ('Azure Confidential Clean Rooms', 'Microsoft Azure'),
    ('Azure Confidential Computing', 'Microsoft Azure'),
    ('Azure Confidential Ledger', 'Microsoft Azure'),
    ('Azure Container Apps', 'Microsoft Azure'),
    ('Azure Container Instances', 'Microsoft Azure'),
    ('Azure Container Registry', 'Microsoft Azure'),
    ('Azure Copilot', 'Microsoft Azure'),
    ('Azure Cosmos DB', 'Microsoft Azure'),
    ('Azure CycleCloud', 'Microsoft Azure'),
    ('Azure DDoS Protection', 'Microsoft Azure'),
    ('Azure DNS', 'Microsoft Azure'),
    ('Azure Data Explorer', 'Microsoft Azure'),
    ('Azure Data Factory', 'Microsoft Azure'),
    ('Azure Data Manager for Energy', 'Microsoft Azure'),
    ('Azure Data Studio', 'Microsoft Azure'),
    ('Azure Database for MySQL', 'Microsoft Azure'),
    ('Azure Database for PostgreSQL', 'Microsoft Azure'),
    ('Azure HorizonDB', 'Azure Database for PostgreSQL'),
    ('Azure Databricks', 'Microsoft Azure'),
    ('Azure DNS Private Resolver', 'Microsoft Azure'),
    ('Azure DevOps', 'Microsoft Azure'),
    ('Azure Artifacts', 'Azure DevOps'),
    ('Azure Boards', 'Azure DevOps'),
    ('Azure DevOps Server', 'Azure DevOps'),
    ('Azure DevOps Services', 'Azure DevOps'),
    ('Azure Pipelines', 'Azure DevOps'),
    ('Azure Repos', 'Azure DevOps'),
    ('Azure Test Plans', 'Azure DevOps'),
    ('Azure Developer CLI', 'Microsoft Azure'),
    ('Azure Device Registry', 'Microsoft Azure'),
    ('Azure Digital Twins', 'Microsoft Azure'),
    ('Azure Event Grid', 'Microsoft Azure'),
    ('Azure Event Hubs', 'Microsoft Azure'),
    ('Azure ExpressRoute', 'Microsoft Azure'),
    ('Azure Firewall', 'Microsoft Azure'),
    ('Azure Front Door', 'Microsoft Azure'),
    ('Azure Functions', 'Microsoft Azure'),
    ('Azure Functions Core Tools', 'Azure Functions'),
    ('Azure Government', 'Microsoft Azure'),
    ('Azure IoT Hub', 'Microsoft Azure'),
    ('Azure IoT Operations', 'Microsoft Azure'),
    ('Azure Key Vault', 'Microsoft Azure'),
    ('Azure Key Vault Managed HSM', 'Azure Key Vault'),
    ('Azure Kubernetes Service', 'Microsoft Azure'),
    ('Azure Kubernetes Fleet Manager', 'Azure Kubernetes Service'),
    ('Azure Landing Zones', 'Microsoft Azure'),
    ('Azure Lighthouse', 'Microsoft Azure'),
    ('Azure Linux', 'Microsoft Azure'),
    ('Azure Load Balancer', 'Microsoft Azure'),
    ('Azure Load Testing', 'Microsoft Azure'),
    ('Azure Local', 'Microsoft Azure'),
    ('Azure Logic Apps', 'Microsoft Azure'),
    ('Azure MCP Server', 'Microsoft Azure'),
    ('Azure Machine Learning', 'Microsoft Azure'),
    ('Azure Managed Grafana', 'Microsoft Azure'),
    ('Azure Managed Redis', 'Microsoft Azure'),
    ('Azure Maps', 'Microsoft Azure'),
    ('Azure Migrate', 'Microsoft Azure'),
    ('Azure Monitor', 'Microsoft Azure'),
    ('Azure Application Insights', 'Azure Monitor'),
    ('Azure NetApp Files', 'Microsoft Azure'),
    ('Azure Notification Hubs', 'Microsoft Azure'),
    ('Azure OpenAI Service', 'Microsoft Azure'),
    ('Azure Policy', 'Microsoft Azure'),
    ('Azure Portal', 'Microsoft Azure'),
    ('Azure PowerShell', 'Microsoft Azure'),
    ('Azure Quantum', 'Microsoft Azure'),
    ('Azure Red Hat OpenShift', 'Microsoft Azure'),
    ('Azure Reservations', 'Microsoft Azure'),
    ('Azure Resource Graph', 'Microsoft Azure'),
    ('Azure Resource Health', 'Microsoft Azure'),
    ('Azure Route Server', 'Microsoft Azure'),
    ('Azure Resource Manager', 'Microsoft Azure'),
    ('Azure SDK', 'Microsoft Azure'),
    ('Azure Identity', 'Azure SDK'),
    ('Azure SDK for .NET', 'Azure SDK'),
    ('Azure SDK for C++', 'Azure SDK'),
    ('Azure SDK for Go', 'Azure SDK'),
    ('Azure SDK for Java', 'Azure SDK'),
    ('Azure SDK for JavaScript', 'Azure SDK'),
    ('Azure SDK for Python', 'Azure SDK'),
    ('Azure SQL', 'Microsoft Azure'),
    ('Azure SQL Database', 'Azure SQL'),
    ('Azure SQL Managed Instance', 'Azure SQL'),
    ('Azure SRE Agent', 'Microsoft Azure'),
    ('Azure Service Bus', 'Microsoft Azure'),
    ('Azure Service Fabric', 'Microsoft Azure'),
    ('Azure Service Health', 'Microsoft Azure'),
    ('Azure SignalR Service', 'Microsoft Azure'),
    ('Azure Site Recovery', 'Microsoft Azure'),
    ('Azure Skills', 'Microsoft Azure'),
    ('Azure Stack Hub', 'Microsoft Azure'),
    ('Azure Static Web Apps', 'Microsoft Azure'),
    ('Azure Storage', 'Microsoft Azure'),
    ('Azure Blob Storage', 'Azure Storage'),
    ('Azure Container Storage', 'Azure Storage'),
    ('Azure Data Box', 'Azure Storage'),
    ('Azure Data Lake Storage', 'Azure Storage'),
    ('Azure Disk Storage', 'Azure Storage'),
    ('Azure Elastic SAN', 'Azure Storage'),
    ('Azure Files', 'Azure Storage'),
    ('Azure Managed Lustre', 'Azure Storage'),
    ('Azure Queue Storage', 'Azure Storage'),
    ('Azure Storage Discovery', 'Azure Storage'),
    ('Azure Storage Explorer', 'Azure Storage'),
    ('Azure Storage Mover', 'Azure Storage'),
    ('Azure Table Storage', 'Azure Storage'),
    ('Azurite', 'Azure Storage'),
    ('Azure Stream Analytics', 'Microsoft Azure'),
    ('Azure Synapse Analytics', 'Microsoft Azure'),
    ('Azure Traffic Manager', 'Microsoft Azure'),
    ('Azure Update Manager', 'Microsoft Azure'),
    ('Azure VMware Solution', 'Microsoft Azure'),
    ('Azure Verified Modules', 'Microsoft Azure'),
    ('Azure Virtual Desktop', 'Microsoft Azure'),
    ('Azure Virtual Machines', 'Microsoft Azure'),
    ('Azure Virtual Machine Scale Sets', 'Azure Virtual Machines'),
    ('Azure Virtual Network', 'Microsoft Azure'),
    ('Azure NAT Gateway', 'Azure Virtual Network'),
    ('Azure Network Watcher', 'Azure Virtual Network'),
    ('Azure Private Link', 'Azure Virtual Network'),
    ('Azure VPN Gateway', 'Azure Virtual Network'),
    ('Azure Virtual WAN', 'Microsoft Azure'),
    ('Azure Web Application Firewall', 'Microsoft Azure'),
    ('Azure Web PubSub', 'Microsoft Azure'),
    ('Microsoft Azure Attestation', 'Microsoft Azure'),
    ('Microsoft Cost Management', 'Microsoft Azure'),
    ('Microsoft Entra ID', 'Microsoft Azure'),
    ('Azure Active Directory B2C', 'Microsoft Entra ID'),
    ('Microsoft Entra External ID', 'Microsoft Entra ID'),
    ('Microsoft Foundry', 'Microsoft Azure'),
    ('Foundry Local', 'Microsoft Foundry'),
    ('Microsoft Foundry Agent Service', 'Microsoft Foundry'),
    ('Microsoft Foundry Toolkit', 'Microsoft Foundry'),
    ('Microsoft Purview', 'Microsoft Azure'),
    ('Microsoft Sentinel', 'Microsoft Azure'),
    ('Oracle AI Database@Azure', 'Microsoft Azure'),
    ('Microsoft Build of OpenJDK', NULL),
    ('Microsoft Copilot', NULL),
    ('Microsoft Defender', NULL),
    ('Microsoft Defender Antivirus', 'Microsoft Defender'),
    ('Microsoft Defender Experts', 'Microsoft Defender'),
    ('Microsoft Defender External Attack Surface Management', 'Microsoft Defender'),
    ('Microsoft Defender Vulnerability Management', 'Microsoft Defender'),
    ('Microsoft Defender XDR', 'Microsoft Defender'),
    ('Microsoft Defender for Cloud', 'Microsoft Defender'),
    ('Microsoft Defender for Cloud Apps', 'Microsoft Defender'),
    ('Microsoft Defender for Endpoint', 'Microsoft Defender'),
    ('Microsoft Defender for Identity', 'Microsoft Defender'),
    ('Microsoft Defender for Office 365', 'Microsoft Defender'),
    ('Microsoft Dev Box', NULL),
    ('Microsoft Discovery', NULL),
    ('Microsoft Edge', NULL),
    ('Microsoft Execution Container', NULL),
    ('Microsoft Fabric', NULL),
    ('Fabric Notebook Toolkit', 'Microsoft Fabric'),
    ('Microsoft Graph', NULL),
    ('Microsoft Intelligent Terminal', NULL),
    ('Microsoft Intune', NULL),
    ('Microsoft Power BI', NULL),
    ('Power BI Desktop', 'Microsoft Power BI'),
    ('Microsoft Power Platform', NULL),
    ('Microsoft Copilot Studio', 'Microsoft Power Platform'),
    ('Microsoft Dataverse', 'Microsoft Power Platform'),
    ('Microsoft Power Apps', 'Microsoft Power Platform'),
    ('Microsoft Power Automate', 'Microsoft Power Platform'),
    ('Power Query', 'Microsoft Power Platform'),
    ('Microsoft Premonition', NULL),
    ('Microsoft Project Perception', NULL),
    ('Microsoft SQL Server', NULL),
    ('SQL Server Management Studio', 'Microsoft SQL Server'),
    ('Microsoft Security Copilot', NULL),
    ('Microsoft Security Exposure Management', NULL),
    ('Microsoft Sovereign Public Cloud', NULL),
    ('Microsoft Surface', NULL),
    ('Microsoft Surface Laptop Ultra', 'Microsoft Surface'),
    ('Microsoft Surface RTX Spark Dev Box', 'Microsoft Surface'),
    ('Microsoft Team Foundation Server', NULL),
    ('Microsoft Threat Intelligence', NULL),
    ('Microsoft Windows', NULL),
    ('Microsoft PowerToys', 'Microsoft Windows'),
    ('Microsoft Store', 'Microsoft Windows'),
    ('Microsoft Windows Server', 'Microsoft Windows'),
    ('Active Directory', 'Microsoft Windows Server'),
    ('Microsoft Active Directory Certificate Services', 'Active Directory'),
    ('Hyper-V', 'Microsoft Windows Server'),
    ('Windows Admin Center', 'Microsoft Windows Server'),
    ('Windows App SDK', 'Microsoft Windows'),
    ('WinUI', 'Windows App SDK'),
    ('Windows ML', 'Microsoft Windows'),
    ('Windows Package Manager', 'Microsoft Windows'),
    ('Windows Subsystem for Linux', 'Microsoft Windows'),
    ('Windows Terminal', 'Microsoft Windows'),
    ('Microsoft on-premises data gateway', NULL),
    ('Minecraft', NULL),
    ('Model Context Protocol', NULL),
    ('Node.js', NULL),
    ('ONNX Runtime', NULL),
    ('OpenTelemetry', NULL),
    ('Playwright', NULL),
    ('Playwright Workspaces', 'Playwright'),
    ('PHP', NULL),
    ('PostgreSQL', NULL),
    ('Pgpool-II', 'PostgreSQL'),
    ('PowerShell', NULL),
    ('Prometheus', NULL),
    ('PyTorch', NULL),
    ('Python', NULL),
    ('Red Hat OpenShift', NULL),
    ('Rust', NULL),
    ('SLB Agile Reservoir Modeling', NULL),
    ('Sail', NULL),
    ('Semantic Kernel', NULL),
    ('Syncfusion Toolkit for Blazor', NULL),
    ('TypeScript', NULL),
    ('TypeSpec', NULL),
    ('Traefik', NULL),
    ('Verus', NULL),
    ('Visual Studio', NULL),
    ('Visual Studio Code', NULL),
    ('Visual Studio Marketplace', NULL),
    ('Windows 365', NULL),
    ('Xbox', NULL),
    ('lakeFS', NULL),
    ('npm', NULL),
    ('vLLM', NULL)
;

INSERT INTO products (name)
SELECT s.name
FROM catalog_seed_products s
WHERE NOT EXISTS (SELECT 1 FROM products p WHERE lower(p.name) = lower(s.name))
ON CONFLICT DO NOTHING;

-- A parent is only set while the product has none, so a parent chosen in the admin is never overwritten.
UPDATE products c
SET parent_product_id = p.id, updated_at = NOW()
FROM catalog_seed_products s
JOIN products p ON lower(p.name) = lower(s.parent)
WHERE lower(c.name) = lower(s.name)
  AND c.parent_product_id IS NULL
  AND c.id <> p.id
  AND p.id NOT IN (SELECT product_tree_ids(c.id));

DROP TABLE catalog_seed_products;

-- Other names of a product: the old names, the short forms and the names with a word more or less that the matching cannot guess.
INSERT INTO product_aliases (product_id, alias)
SELECT p.id, v.alias
FROM (VALUES
    ('.NET Aspire', 'Aspire'),
    ('.NET MAUI', 'MAUI'),
    ('ASP.NET Core', 'Microsoft ASP.NET Core'),
    ('Agent Host Protocol', 'AHP'),
    ('Azure AI Document Intelligence', 'Azure Document Intelligence'),
    ('Azure AI Document Intelligence', 'Form Recognizer'),
    ('Azure AI Services', 'Azure Cognitive Services'),
    ('Azure AI Services', 'Cognitive Services'),
    ('Azure Active Directory B2C', 'Azure AD B2C'),
    ('Azure Application Insights', 'Application Insights'),
    ('Azure Bicep', 'Bicep'),
    ('Azure Cosmos DB', 'Azure Cosmos DB for NoSQL'),
    ('Azure Cosmos DB', 'Cosmos DB'),
    ('Azure Data Lake Storage', 'Azure Data Lake'),
    ('Azure Data Lake Storage', 'Azure Data Lake Storage Gen2'),
    ('Azure Kubernetes Service', 'AKS'),
    ('Azure Kubernetes Service', 'Azure Kubernetes Service (AKS)'),
    ('Azure Local', 'Azure Stack HCI'),
    ('Microsoft Execution Container', 'Microsoft Execution Containers'),
    ('Azure OpenAI Service', 'Azure OpenAI'),
    ('Azure Pipelines', 'Azure DevOps Pipelines'),
    ('Entity Framework Core', 'EF Core'),
    ('Entity Framework Core', 'Entity Framework'),
    ('Flint', 'Flint-interactive'),
    ('GitHub Advanced Security', 'GHAS'),
    ('GitHub CodeQL', 'CodeQL'),
    ('GitHub Copilot app', 'Copilot app'),
    ('GitHub Copilot CLI', 'Copilot CLI'),
    ('GitHub Copilot Workspace', 'Copilot Workspace'),
    ('GitHub Dependabot', 'Dependabot'),
    ('GitHub Enterprise Server', 'GHES'),
    ('GitHub Spark', 'Spark'),
    ('HashiCorp Terraform', 'Terraform'),
    ('Microsoft Azure', 'Azure'),
    ('Microsoft Copilot Studio', 'Copilot Studio'),
    ('Microsoft Cost Management', 'Azure Cost Management'),
    ('Microsoft Defender Antivirus', 'Windows Defender'),
    ('Microsoft Defender External Attack Surface Management', 'Defender EASM'),
    ('Microsoft Defender XDR', 'Microsoft 365 Defender'),
    ('Microsoft Defender for Cloud', 'Azure Defender'),
    ('Microsoft Defender for Cloud', 'Azure Security Center'),
    ('Microsoft Defender for Cloud Apps', 'Microsoft Cloud App Security'),
    ('Microsoft Entra ID', 'AAD'),
    ('Microsoft Entra ID', 'Azure AD'),
    ('Microsoft Entra ID', 'Azure Active Directory'),
    ('Microsoft Entra ID', 'Entra ID'),
    ('Microsoft Entra ID', 'Microsoft Entra'),
    ('Microsoft Fabric', 'Fabric'),
    ('Microsoft Foundry', 'Azure AI Foundry'),
    ('Microsoft Foundry', 'Azure AI Studio'),
    ('Microsoft Foundry', 'Foundry'),
    ('Microsoft Foundry Agent Service', 'Azure AI Foundry Agent Service'),
    ('Microsoft Foundry Agent Service', 'Foundry Agent Service'),
    ('Microsoft Foundry Toolkit', 'AI Toolkit'),
    ('Microsoft Foundry Toolkit', 'Foundry Toolkit'),
    ('Microsoft Intune', 'Microsoft Endpoint Manager'),
    ('Microsoft Power BI', 'Power BI'),
    ('Microsoft Project Perception', 'Project Perception'),
    ('Microsoft Purview', 'Azure Purview'),
    ('Microsoft SQL Server', 'Microsoft SQL'),
    ('Microsoft SQL Server', 'SQL Server'),
    ('Microsoft Sentinel', 'Azure Sentinel'),
    ('Microsoft Team Foundation Server', 'Team Foundation Server'),
    ('Microsoft Teams', 'Teams'),
    ('Microsoft Threat Intelligence', 'Microsoft Defender Threat Intelligence'),
    ('Model Context Protocol', 'MCP'),
    ('SQL Server Management Studio', 'SSMS'),
    ('Visual Studio Code', 'VS Code'),
    ('Visual Studio Code', 'VSCode'),
    ('Windows Package Manager', 'winget'),
    ('Windows Subsystem for Linux', 'WSL')
) AS v (product, alias)
JOIN products p ON lower(p.name) = lower(v.product)
WHERE NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = lower(v.alias))
ON CONFLICT (lower(alias)) DO NOTHING;

-- Names that are not products: an event, a program, a vendor or model that is not part of this ecosystem.
INSERT INTO ignored_product_names (name)
SELECT v.name
FROM (VALUES
    ('Anthropic'),
    ('Cohere Rerank'),
    ('GitHub Secure Open Source Fund'),
    ('GitHub Security Lab'),
    ('GitHub Universe'),
    ('Microsoft Build'),
    ('Microsoft Ignite'),
    ('Microsoft-Decision-1'),
    ('Microsoft Research'),
    ('OpenAI')
) AS v (name)
WHERE NOT EXISTS (SELECT 1 FROM products p WHERE lower(p.name) = lower(v.name))
  AND NOT EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(v.name))
ON CONFLICT (lower(name)) DO NOTHING;


-- ── 4. The GitHub Copilot features page ─────────────────────────────────────────────────────────────────────────────────────────
-- The features page of GitHub Copilot (ghc_features and ghc_feature_content) is replaced by the timeline of the section, and its
-- features live in the catalog. The two old tables are read once and dropped.
--
--   * Every feature with a release date and linked content becomes an event per linked item (launch of the feature, dated on its
--     release date, with the description of the feature). Events of the same date, kind and feature are one card on the timeline with
--     all items as sources. They are stored with extracted_from = 'catalog', which the AI extraction and the edit of an article leave alone.
--   * Features that describe the same capability are merged ('Chat in IDE', 'Chat on Mobile' and 'Ice Breakers' are Copilot Chat):
--     the old name becomes an alias, so the names the AI uses keep matching.
--   * What is a product (GitHub Spark, Copilot Workspace, the CLI extension) becomes an event of that product.
--   * What is no feature (a policy, user management) loses its feature; a model release stays an event of the product.
--   * Token-based billing, AI credits and premium requests are features of GitHub Copilot: no capability, but a lot of articles are about them.
--
-- The tables exist when the old features page existed. They are created empty when they are missing, so the section can run again.
CREATE TABLE IF NOT EXISTS ghc_features (
    slug         TEXT PRIMARY KEY,
    title        TEXT NOT NULL,
    description  TEXT NOT NULL DEFAULT '',
    release_date BIGINT,
    plans        TEXT NOT NULL DEFAULT '',
    ghes_support BOOLEAN NOT NULL DEFAULT FALSE,
    created_at   TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at   TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS ghc_feature_content (
    feature_slug    TEXT    NOT NULL REFERENCES ghc_features (slug) ON DELETE CASCADE,
    collection_name TEXT    NOT NULL,
    item_slug       TEXT    NOT NULL,
    is_thumbnail    BOOLEAN NOT NULL DEFAULT FALSE,
    sort_order      INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (feature_slug, collection_name, item_slug),
    FOREIGN KEY (collection_name, item_slug) REFERENCES content_items (collection_name, slug) ON DELETE CASCADE
);

-- ── The features and their links ─────────────────────────────────────────────────────────────────────────────────────────────────
-- When two old features share a title, the first slug wins.
INSERT INTO product_features (product_id, name, slug, description, release_date)
SELECT p.id, g.title, g.slug, COALESCE(g.description, ''), g.release_date
FROM (
        SELECT DISTINCT ON (lower(title)) slug, title, description, release_date
        FROM ghc_features
        ORDER BY lower(title), slug
     ) g
JOIN products p ON lower(p.name) = 'github copilot'
ON CONFLICT (product_id, lower(name)) DO NOTHING;

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

-- A release date that was entered as midnight Brussels time is an epoch in the evening of the day before in UTC. The dates are read and
-- written as midnight UTC of the date, like the dates of the timeline events. Dates that are midnight UTC already do not change.
UPDATE product_features
SET release_date = EXTRACT(EPOCH FROM (to_timestamp(release_date) AT TIME ZONE 'Europe/Brussels')::date)::bigint
WHERE release_date IS NOT NULL;

-- ── The plan ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
--   action rename   the feature gets the new name (the old name becomes an alias)
--   action merge    the feature is merged into the feature with the target name; its events keep the old name in the description
--   action product  the feature is a product (target): its events become events of that product and the feature is deleted
--   action drop     the feature is deleted (it has no content, so no events)
--   no row          the feature stays as it is
CREATE TEMP TABLE ghc_feature_plan (old_name TEXT NOT NULL, action TEXT NOT NULL, target TEXT, event_kind TEXT NOT NULL);

INSERT INTO ghc_feature_plan (old_name, action, target, event_kind) VALUES
    -- Names in the style of the vendor, and the one name that all of them use
    ('Agent Mode', 'rename', 'Agent mode', 'launch'),
    ('Bring Your Own LLM', 'rename', 'Bring your own model', 'launch'),
    ('Code Completion', 'rename', 'Code completions', 'launch'),
    ('Next Edit Suggestions', 'rename', 'Next edit suggestions', 'launch'),
    ('Copilot Coding Agent in Pull Requests', 'rename', 'Coding agent', 'launch'),
    ('MCP with Azure and GitHub', 'rename', 'MCP support', 'launch'),
    ('Premium Requests', 'rename', 'Premium requests', 'pricing-change'),
    ('Fine Tuned Models', 'rename', 'Fine-tuned models', 'launch'),
    ('Fetch Webpage', 'rename', 'Fetch webpage', 'launch'),
    ('Commit Message Suggestions', 'rename', 'Commit message generation', 'launch'),
    ('Code Scanning AI Autofix', 'rename', 'Copilot Autofix', 'launch'),
    ('Multi-file Edits', 'rename', 'Copilot Edits', 'launch'),
    ('Code Referencing', 'rename', 'Code referencing', 'launch'),
    ('Content Exclusions', 'rename', 'Content exclusions', 'launch'),
    ('Web Search with Bing', 'rename', 'Web search', 'launch'),
    ('Explain Failed Action Jobs', 'rename', 'Explain failed Actions jobs', 'launch'),
    ('Token-based billing: from premium request units to AI credits and tokens', 'rename', 'Token-based billing', 'pricing-change'),
    ('Usage Metrics', 'rename', 'Usage metrics', 'launch'),
    ('Slash Commands', 'rename', 'Slash commands', 'launch'),
    ('User Instructions', 'rename', 'Custom instructions', 'launch'),
    ('PR Body Generation in WebUI', 'rename', 'Pull request summaries', 'launch'),
    ('Chat in IDE', 'rename', 'Copilot Chat', 'launch'),
    -- One capability on several surfaces or in several steps
    ('Chat on Mobile', 'merge', 'Copilot Chat', 'launch'),
    ('@github Chat Participant', 'merge', 'Copilot Chat', 'launch'),
    ('Chat with your Pull Request', 'merge', 'Copilot Chat', 'launch'),
    ('Integration in Web UI', 'merge', 'Copilot Chat', 'launch'),
    ('Ice Breakers', 'merge', 'Copilot Chat', 'launch'),
    ('PR Body Generation in VS Code', 'merge', 'Pull request summaries', 'launch'),
    ('Local In-Editor Code Reviews', 'merge', 'Code review', 'launch'),
    ('Set Coding Guidelines for Code Review', 'merge', 'Code review', 'launch'),
    ('Multi-Models', 'merge', 'Model selection', 'launch'),
    ('Chat with Knowledge Bases', 'merge', 'Copilot Spaces', 'launch'),
    ('Multi-Repository Context', 'merge', 'Copilot Spaces', 'launch'),
    ('Copilot Metrics API', 'merge', 'Usage metrics', 'launch'),
    ('Path-specific Custom Instructions', 'merge', 'Custom instructions', 'launch'),
    ('Organization-wide Custom Instructions', 'merge', 'Custom instructions', 'launch'),
    ('Commit Message Custom Instructions', 'merge', 'Custom instructions', 'launch'),
    ('Playwright Test Generation', 'merge', 'Agent mode', 'launch'),
    -- The slash commands of Copilot Chat
    ('Code Docs', 'merge', 'Slash commands', 'launch'),
    ('Code Explanation', 'merge', 'Slash commands', 'launch'),
    ('Code Fixing', 'merge', 'Slash commands', 'launch'),
    ('Code Debugging', 'merge', 'Slash commands', 'launch'),
    ('Code Translation', 'merge', 'Slash commands', 'launch'),
    ('Unit Tests', 'merge', 'Slash commands', 'launch'),
    ('Smart Actions', 'merge', 'Slash commands', 'launch'),
    -- Products and releases of a model
    ('GitHub Spark', 'product', 'GitHub Spark', 'launch'),
    ('Preview for Copilot Workspace', 'product', 'GitHub Copilot Workspace', 'preview'),
    ('CLI Assistance', 'product', 'GitHub Copilot CLI', 'launch'),
    ('GPT-4o Copilot Suggestions Model', 'product', 'GitHub Copilot', 'launch'),
    -- Not features
    ('User Management', 'drop', NULL, 'launch'),
    ('Data Excluded From Training by Default', 'drop', NULL, 'launch');

-- Folds a feature into another one: its aliases and content move to the kept feature, its own name becomes an alias, and the kept
-- feature gets the earliest release date of the two.
CREATE OR REPLACE FUNCTION merge_features(p_drop_id BIGINT, p_keep_id BIGINT) RETURNS VOID
LANGUAGE plpgsql AS $$
DECLARE
    v_drop_name TEXT;
BEGIN
    IF p_drop_id IS NULL OR p_keep_id IS NULL OR p_drop_id = p_keep_id THEN
        RETURN;
    END IF;

    SELECT name INTO v_drop_name FROM product_features WHERE id = p_drop_id;

    INSERT INTO product_feature_content (feature_id, collection_name, item_slug, is_thumbnail, sort_order)
    SELECT p_keep_id, c.collection_name, c.item_slug, FALSE, c.sort_order
    FROM product_feature_content c
    WHERE c.feature_id = p_drop_id
    ON CONFLICT DO NOTHING;

    UPDATE product_feature_aliases a SET feature_id = p_keep_id
    WHERE a.feature_id = p_drop_id
      AND NOT EXISTS (SELECT 1 FROM product_feature_aliases x WHERE x.feature_id = p_keep_id AND lower(x.alias) = lower(a.alias))
      AND NOT EXISTS (SELECT 1 FROM product_features k WHERE k.id = p_keep_id AND lower(k.name) = lower(a.alias));

    UPDATE product_features k
    SET release_date = LEAST(k.release_date, d.release_date),
        updated_at = NOW()
    FROM product_features d
    WHERE k.id = p_keep_id AND d.id = p_drop_id;

    UPDATE content_timeline_events SET feature_id = p_keep_id WHERE feature_id = p_drop_id;

    DELETE FROM product_features WHERE id = p_drop_id;

    INSERT INTO product_feature_aliases (feature_id, alias)
    SELECT p_keep_id, v_drop_name
    WHERE NOT EXISTS (SELECT 1 FROM product_features k WHERE k.id = p_keep_id AND lower(k.name) = lower(v_drop_name))
    ON CONFLICT DO NOTHING;
END;
$$;

-- ── The dated features become events ───────────────────────────────────────────────────────────────────────────────────────────
-- One event per linked item, so every video that came with a feature is a source of the card. A release date on the first of a month
-- was entered as a month. The description of a merged feature starts with its own name, so the card says what it is about.
INSERT INTO content_timeline_events
    (collection_name, item_slug, kind, product, feature, description, event_date, date_precision, date_source, quote, extracted_from)
SELECT l.collection_name,
       l.item_slug,
       COALESCE(m.event_kind, 'launch'),
       CASE WHEN m.action = 'product' THEN m.target ELSE p.name END,
       CASE WHEN m.action = 'product' THEN NULL ELSE COALESCE(CASE WHEN m.action IN ('rename', 'merge') THEN m.target END, f.name) END,
       CASE WHEN m.action IN ('merge', 'product')
            THEN f.name || COALESCE(': ' || NULLIF(btrim(f.description), ''), '')
            ELSE COALESCE(NULLIF(btrim(f.description), ''), f.name) END,
       d.event_date,
       CASE WHEN EXTRACT(DAY FROM d.event_date) = 1 THEN 'month' ELSE 'day' END,
       'explicit',
       NULL,
       'catalog'
FROM product_features f
JOIN products p ON p.id = f.product_id
LEFT JOIN ghc_feature_plan m ON lower(m.old_name) = lower(f.name)
JOIN product_feature_content l ON l.feature_id = f.id
CROSS JOIN LATERAL (SELECT (to_timestamp(f.release_date) AT TIME ZONE 'UTC')::date AS event_date) d
WHERE f.release_date IS NOT NULL
  AND f.product_id IN (SELECT product_tree_ids(c.id) FROM products c WHERE lower(c.name) = 'github copilot')
  AND COALESCE(m.action, 'keep') <> 'drop'
  AND NOT EXISTS (
      SELECT 1 FROM content_timeline_events e
      WHERE e.collection_name = l.collection_name
        AND e.item_slug = l.item_slug
        AND e.kind = COALESCE(m.event_kind, 'launch')
        AND lower(e.product) = lower(CASE WHEN m.action = 'product' THEN m.target ELSE p.name END)
        AND lower(COALESCE(e.feature, '')) = lower(CASE WHEN m.action = 'product' THEN '' ELSE COALESCE(CASE WHEN m.action IN ('rename', 'merge') THEN m.target END, f.name) END)
        AND e.event_date = d.event_date);

-- ── Names ──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
WITH renamed AS (
    UPDATE product_features f
    SET name = m.target, updated_at = NOW()
    FROM ghc_feature_plan m
    WHERE m.action = 'rename'
      AND lower(f.name) = lower(m.old_name)
      AND f.name <> m.target
      AND f.product_id IN (SELECT product_tree_ids(c.id) FROM products c WHERE lower(c.name) = 'github copilot')
      AND NOT EXISTS (
          SELECT 1 FROM product_features x
          WHERE x.product_id = f.product_id AND lower(x.name) = lower(m.target) AND x.id <> f.id)
    RETURNING f.id, m.old_name, m.target
)
INSERT INTO product_feature_aliases (feature_id, alias)
SELECT r.id, r.old_name
FROM renamed r
WHERE lower(r.old_name) <> lower(r.target)
ON CONFLICT DO NOTHING;

-- The feature that others are merged into exists, also when the page did not have it.
INSERT INTO product_features (product_id, name)
SELECT DISTINCT p.id, m.target
FROM ghc_feature_plan m
JOIN product_features d ON lower(d.name) = lower(m.old_name)
JOIN products p ON p.id = d.product_id
WHERE m.action = 'merge'
  AND NOT EXISTS (SELECT 1 FROM product_features k WHERE k.product_id = p.id AND lower(k.name) = lower(m.target))
ON CONFLICT DO NOTHING;

-- ── Features that describe the same capability ──────────────────────────────────────────────────────────────────────────────────
-- A feature that could not be renamed because a feature with the new name exists already is merged into it.
SELECT merge_features(d.id, k.id)
FROM ghc_feature_plan m
JOIN product_features d ON lower(d.name) = lower(m.old_name)
JOIN product_features k ON k.product_id = d.product_id AND lower(k.name) = lower(m.target)
WHERE m.action IN ('merge', 'rename')
  AND d.id <> k.id
  AND d.product_id IN (SELECT product_tree_ids(c.id) FROM products c WHERE lower(c.name) = 'github copilot');

-- ── Products and things that are not features ──────────────────────────────────────────────────────────────────────────────────
DELETE FROM product_features f
USING ghc_feature_plan m
WHERE m.action IN ('product', 'drop')
  AND lower(f.name) = lower(m.old_name)
  AND f.product_id IN (SELECT product_tree_ids(c.id) FROM products c WHERE lower(c.name) = 'github copilot');

DROP FUNCTION merge_features(BIGINT, BIGINT);
DROP TABLE ghc_feature_plan;

-- ── Billing ──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
-- Not capabilities, but many articles are about them, so the timeline and the catalog should be able to say so.
INSERT INTO product_features (product_id, name, description)
SELECT p.id, v.name, v.description
FROM products p
CROSS JOIN (VALUES
    ('Premium requests', 'The allowance of requests to the most capable models that a GitHub Copilot plan includes, and what extra requests cost.'),
    ('Token-based billing', 'GitHub Copilot usage is billed by the tokens that the models use, instead of by premium request units.'),
    ('AI credits', 'The credits that pay for GitHub Copilot usage under token-based billing, replacing the premium request units.')
) AS v (name, description)
WHERE lower(p.name) = 'github copilot'
  AND NOT EXISTS (SELECT 1 FROM product_features x WHERE x.product_id = p.id AND lower(x.name) = lower(v.name))
ON CONFLICT DO NOTHING;

-- The old tables are not used any more.
DROP TABLE ghc_feature_content;
DROP TABLE ghc_features;


-- ── 5. Features ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
-- A feature is part of one product and is only used through it: a capability, a mode, a model you pick inside the product, a part of
-- a bundle, a billing concept. A date can be about a feature, and the name of a feature that the AI returns as a product still links to
-- the product of the feature. A product also has the features of its parents.
INSERT INTO product_features (product_id, name)
SELECT p.id, v.feature
FROM (VALUES
    ('ASP.NET Core', 'Blazor'),
    ('ASP.NET Core', 'SignalR'),
    ('Azure Kubernetes Service', 'Automatic'),
    ('Azure Monitor', 'Agent'),
    ('Azure Monitor', 'Logs'),
    ('Azure Monitor', 'Managed Prometheus'),
    ('Azure Virtual Network', 'Accelerated Networking'),
    ('Azure Virtual Network', 'Microsoft Azure Network Adapter'),
    ('GitHub', 'Discussions'),
    ('GitHub', 'Environments'),
    ('GitHub', 'GitHub Apps'),
    ('GitHub', 'GraphQL API'),
    ('GitHub', 'Issues'),
    ('GitHub', 'Merge queue'),
    ('GitHub', 'Projects'),
    ('GitHub', 'Pull requests'),
    ('GitHub', 'REST API'),
    ('GitHub', 'Releases'),
    ('GitHub', 'Rulesets'),
    ('GitHub', 'Webhooks'),
    ('GitHub Actions', 'Hosted runners'),
    ('GitHub Code Security', 'Code scanning'),
    ('GitHub Copilot', 'AI credits'),
    ('GitHub Copilot', 'Agent mode'),
    ('GitHub Copilot', 'App modernization'),
    ('GitHub Copilot', 'Bring your own model'),
    ('GitHub Copilot', 'Code completions'),
    ('GitHub Copilot', 'Code referencing'),
    ('GitHub Copilot', 'Code review'),
    ('GitHub Copilot', 'Coding agent'),
    ('GitHub Copilot', 'Commit message generation'),
    ('GitHub Copilot', 'Content exclusions'),
    ('GitHub Copilot', 'Copilot Autofix'),
    ('GitHub Copilot', 'Copilot Chat'),
    ('GitHub Copilot', 'Copilot Edits'),
    ('GitHub Copilot', 'Copilot Extensions Marketplace'),
    ('GitHub Copilot', 'Copilot Fusion'),
    ('GitHub Copilot', 'Copilot Memory'),
    ('GitHub Copilot', 'Copilot Spaces'),
    ('GitHub Copilot', 'Custom instructions'),
    ('GitHub Copilot', 'Explain failed Actions jobs'),
    ('GitHub Copilot', 'Fetch webpage'),
    ('GitHub Copilot', 'Fine-tuned models'),
    ('GitHub Copilot', 'MCP support'),
    ('GitHub Copilot', 'Model selection'),
    ('GitHub Copilot', 'Next edit suggestions'),
    ('GitHub Copilot', 'Premium requests'),
    ('GitHub Copilot', 'Pull request summaries'),
    ('GitHub Copilot', 'SDK'),
    ('GitHub Copilot', 'Security Advisory Summaries'),
    ('GitHub Copilot', 'Slash commands'),
    ('GitHub Copilot', 'Token-based billing'),
    ('GitHub Copilot', 'Usage metrics'),
    ('GitHub Copilot', 'Web search'),
    ('GitHub Secret Protection', 'Secret scanning'),
    ('Microsoft Azure', 'Azure role-based access control'),
    ('Microsoft Defender for Cloud', 'Defender for Containers'),
    ('Microsoft Defender for Cloud', 'Defender for Storage'),
    ('Microsoft Discovery', 'Discovery app'),
    ('Microsoft Entra ID', 'Agent ID'),
    ('Microsoft Entra ID', 'Conditional Access'),
    ('Microsoft Entra ID', 'Managed identities'),
    ('Microsoft Entra ID', 'Privileged Identity Management'),
    ('Microsoft Fabric', 'Data Engineering'),
    ('Microsoft Fabric', 'Data Factory'),
    ('Microsoft Fabric', 'Data Warehouse'),
    ('Microsoft Fabric', 'Data agent'),
    ('Microsoft Fabric', 'Eventhouse'),
    ('Microsoft Fabric', 'Fabric IQ'),
    ('Microsoft Fabric', 'Mirroring'),
    ('Microsoft Fabric', 'OneLake'),
    ('Microsoft Fabric', 'Real-Time Intelligence'),
    ('Microsoft Fabric', 'SQL database'),
    ('Microsoft Fabric', 'User Data Functions'),
    ('Microsoft Foundry', 'Models'),
    ('Microsoft Windows', 'BitLocker'),
    ('Microsoft Windows', 'Windows Hello')
) AS v (product, feature)
JOIN products p ON lower(p.name) = lower(v.product)
ON CONFLICT (product_id, lower(name)) DO NOTHING;

-- Other names of a feature. A name that is a feature of the product already, or an alias of one of its features, is skipped.
INSERT INTO product_feature_aliases (feature_id, alias)
SELECT f.id, v.alias
FROM (VALUES
    ('ASP.NET Core', 'Blazor', 'ASP.NET Core Blazor'),
    ('ASP.NET Core', 'SignalR', 'ASP.NET Core SignalR'),
    ('Azure Kubernetes Service', 'Automatic', 'AKS Automatic'),
    ('Azure Kubernetes Service', 'Automatic', 'Azure Kubernetes Service Automatic'),
    ('Azure Monitor', 'Agent', 'Azure Monitor Agent'),
    ('Azure Monitor', 'Logs', 'Azure Log Analytics'),
    ('Azure Monitor', 'Logs', 'Azure Monitor Logs'),
    ('Azure Monitor', 'Logs', 'Log Analytics'),
    ('Azure Monitor', 'Managed Prometheus', 'Azure Managed Prometheus'),
    ('Azure Monitor', 'Managed Prometheus', 'Azure Monitor managed service for Prometheus'),
    ('Azure Virtual Network', 'Accelerated Networking', 'Azure Accelerated Networking'),
    ('GitHub', 'Discussions', 'GitHub Discussions'),
    ('GitHub', 'GraphQL API', 'GitHub GraphQL API'),
    ('GitHub', 'Issues', 'GitHub Issues'),
    ('GitHub', 'Projects', 'GitHub Projects'),
    ('GitHub', 'REST API', 'GitHub REST API'),
    ('GitHub', 'Rulesets', 'Repository rulesets'),
    ('GitHub Actions', 'Hosted runners', 'GitHub Hosted Runners'),
    ('GitHub Code Security', 'Code scanning', 'GitHub Code Scanning'),
    ('GitHub Copilot', 'AI credits', 'AI credit'),
    ('GitHub Copilot', 'AI credits', 'GitHub AI credits'),
    ('GitHub Copilot', 'Agent mode', 'Playwright Test Generation'),
    ('GitHub Copilot', 'App modernization', 'GitHub Copilot app modernization'),
    ('GitHub Copilot', 'Bring your own model', 'BYOK'),
    ('GitHub Copilot', 'Bring your own model', 'Bring Your Own Key'),
    ('GitHub Copilot', 'Bring your own model', 'Bring Your Own LLM'),
    ('GitHub Copilot', 'Bring your own model', 'Bring Your Own Model'),
    ('GitHub Copilot', 'Code completions', 'Code Completion'),
    ('GitHub Copilot', 'Code completions', 'Code completions'),
    ('GitHub Copilot', 'Code completions', 'Inline suggestions'),
    ('GitHub Copilot', 'Code review', 'Automated code review'),
    ('GitHub Copilot', 'Code review', 'Copilot code review'),
    ('GitHub Copilot', 'Code review', 'GitHub Copilot code review'),
    ('GitHub Copilot', 'Code review', 'Local In-Editor Code Reviews'),
    ('GitHub Copilot', 'Code review', 'Set Coding Guidelines for Code Review'),
    ('GitHub Copilot', 'Coding agent', 'Coding agent'),
    ('GitHub Copilot', 'Coding agent', 'Copilot Coding Agent in Pull Requests'),
    ('GitHub Copilot', 'Coding agent', 'Copilot coding agent'),
    ('GitHub Copilot', 'Coding agent', 'GitHub Copilot coding agent'),
    ('GitHub Copilot', 'Commit message generation', 'Commit Message Suggestions'),
    ('GitHub Copilot', 'Commit message generation', 'Commit message generation'),
    ('GitHub Copilot', 'Content exclusions', 'Content exclusion'),
    ('GitHub Copilot', 'Copilot Autofix', 'Code Scanning AI Autofix'),
    ('GitHub Copilot', 'Copilot Chat', '@github Chat Participant'),
    ('GitHub Copilot', 'Copilot Chat', 'Chat'),
    ('GitHub Copilot', 'Copilot Chat', 'Chat in IDE'),
    ('GitHub Copilot', 'Copilot Chat', 'Chat on Mobile'),
    ('GitHub Copilot', 'Copilot Chat', 'Chat with your Pull Request'),
    ('GitHub Copilot', 'Copilot Chat', 'Copilot Chat'),
    ('GitHub Copilot', 'Copilot Chat', 'GitHub Copilot Chat'),
    ('GitHub Copilot', 'Copilot Chat', 'Ice Breakers'),
    ('GitHub Copilot', 'Copilot Chat', 'Integration in Web UI'),
    ('GitHub Copilot', 'Copilot Edits', 'Multi-file Edits'),
    ('GitHub Copilot', 'Copilot Extensions Marketplace', 'Copilot Extensions'),
    ('GitHub Copilot', 'Copilot Extensions Marketplace', 'GitHub Copilot Extensions'),
    ('GitHub Copilot', 'Copilot Fusion', 'GitHub Copilot Fusion'),
    ('GitHub Copilot', 'Copilot Memory', 'GitHub Copilot Memory'),
    ('GitHub Copilot', 'Copilot Spaces', 'Chat with Knowledge Bases'),
    ('GitHub Copilot', 'Copilot Spaces', 'GitHub Copilot Spaces'),
    ('GitHub Copilot', 'Copilot Spaces', 'Multi-Repository Context'),
    ('GitHub Copilot', 'Copilot Spaces', 'Spaces'),
    ('GitHub Copilot', 'Custom instructions', 'Commit Message Custom Instructions'),
    ('GitHub Copilot', 'Custom instructions', 'Organization-wide Custom Instructions'),
    ('GitHub Copilot', 'Custom instructions', 'Path-specific Custom Instructions'),
    ('GitHub Copilot', 'Custom instructions', 'User Instructions'),
    ('GitHub Copilot', 'Explain failed Actions jobs', 'Explain Failed Action Jobs'),
    ('GitHub Copilot', 'Fine-tuned models', 'Custom models'),
    ('GitHub Copilot', 'Fine-tuned models', 'Fine Tuned Models'),
    ('GitHub Copilot', 'Fine-tuned models', 'Fine-tuned models'),
    ('GitHub Copilot', 'MCP support', 'MCP servers'),
    ('GitHub Copilot', 'MCP support', 'MCP support'),
    ('GitHub Copilot', 'MCP support', 'MCP with Azure and GitHub'),
    ('GitHub Copilot', 'MCP support', 'Model Context Protocol'),
    ('GitHub Copilot', 'Model selection', 'Model picker'),
    ('GitHub Copilot', 'Model selection', 'Multi-Models'),
    ('GitHub Copilot', 'Model selection', 'Multiple models'),
    ('GitHub Copilot', 'Next edit suggestions', 'NES'),
    ('GitHub Copilot', 'Next edit suggestions', 'Next edit suggestion'),
    ('GitHub Copilot', 'Premium requests', 'Premium request'),
    ('GitHub Copilot', 'Premium requests', 'Premium request units'),
    ('GitHub Copilot', 'Pull request summaries', 'PR Body Generation in VS Code'),
    ('GitHub Copilot', 'Pull request summaries', 'PR Body Generation in WebUI'),
    ('GitHub Copilot', 'SDK', 'Copilot SDK'),
    ('GitHub Copilot', 'SDK', 'GitHub Copilot SDK'),
    ('GitHub Copilot', 'Slash commands', 'Code Debugging'),
    ('GitHub Copilot', 'Slash commands', 'Code Docs'),
    ('GitHub Copilot', 'Slash commands', 'Code Explanation'),
    ('GitHub Copilot', 'Slash commands', 'Code Fixing'),
    ('GitHub Copilot', 'Slash commands', 'Code Translation'),
    ('GitHub Copilot', 'Slash commands', 'Smart Actions'),
    ('GitHub Copilot', 'Slash commands', 'Unit Tests'),
    ('GitHub Copilot', 'Token-based billing', 'Token based billing'),
    ('GitHub Copilot', 'Token-based billing', 'Token-based billing: from premium request units to AI credits and tokens'),
    ('GitHub Copilot', 'Usage metrics', 'Copilot Metrics API'),
    ('GitHub Copilot', 'Usage metrics', 'Copilot usage metrics'),
    ('GitHub Copilot', 'Usage metrics', 'Metrics API'),
    ('GitHub Copilot', 'Web search', 'Web Search with Bing'),
    ('GitHub Copilot', 'Web search', 'Web search'),
    ('GitHub Secret Protection', 'Secret scanning', 'GitHub Secret Scanning'),
    ('Microsoft Azure', 'Azure role-based access control', 'Azure RBAC'),
    ('Microsoft Defender for Cloud', 'Defender for Containers', 'Microsoft Defender for Containers'),
    ('Microsoft Defender for Cloud', 'Defender for Storage', 'Microsoft Defender for Storage'),
    ('Microsoft Discovery', 'Discovery app', 'Microsoft Discovery app'),
    ('Microsoft Entra ID', 'Agent ID', 'Entra Agent ID'),
    ('Microsoft Entra ID', 'Agent ID', 'Microsoft Entra Agent ID'),
    ('Microsoft Entra ID', 'Conditional Access', 'Azure AD Conditional Access'),
    ('Microsoft Entra ID', 'Conditional Access', 'Microsoft Entra Conditional Access'),
    ('Microsoft Entra ID', 'Managed identities', 'Managed Identity'),
    ('Microsoft Entra ID', 'Managed identities', 'Managed identities for Azure resources'),
    ('Microsoft Entra ID', 'Privileged Identity Management', 'Microsoft Entra Privileged Identity Management'),
    ('Microsoft Entra ID', 'Privileged Identity Management', 'PIM'),
    ('Microsoft Fabric', 'Data Engineering', 'Fabric Data Engineering'),
    ('Microsoft Fabric', 'Data Engineering', 'Microsoft Fabric Data Engineering'),
    ('Microsoft Fabric', 'Data Factory', 'Fabric Data Factory'),
    ('Microsoft Fabric', 'Data Factory', 'Microsoft Fabric Data Factory'),
    ('Microsoft Fabric', 'Data Warehouse', 'Fabric Data Warehouse'),
    ('Microsoft Fabric', 'Data Warehouse', 'Microsoft Fabric Data Warehouse'),
    ('Microsoft Fabric', 'Data agent', 'Fabric data agent'),
    ('Microsoft Fabric', 'Data agent', 'Microsoft Fabric Data Agent'),
    ('Microsoft Fabric', 'Eventhouse', 'Microsoft Fabric Eventhouse'),
    ('Microsoft Fabric', 'Mirroring', 'Fabric Mirroring'),
    ('Microsoft Fabric', 'Mirroring', 'Microsoft Fabric Mirroring'),
    ('Microsoft Fabric', 'OneLake', 'Microsoft OneLake'),
    ('Microsoft Fabric', 'Real-Time Intelligence', 'Fabric Real-Time Intelligence'),
    ('Microsoft Fabric', 'Real-Time Intelligence', 'Microsoft Fabric Real-Time Intelligence'),
    ('Microsoft Fabric', 'SQL database', 'Microsoft Fabric SQL database'),
    ('Microsoft Fabric', 'SQL database', 'SQL database in Fabric'),
    ('Microsoft Fabric', 'User Data Functions', 'Fabric User Data Functions'),
    ('Microsoft Fabric', 'User Data Functions', 'Microsoft Fabric User Data Functions'),
    ('Microsoft Foundry', 'Models', 'Azure AI Foundry Models'),
    ('Microsoft Foundry', 'Models', 'Foundry Models'),
    ('Microsoft Foundry', 'Models', 'Microsoft Foundry Models')
) AS v (product, feature, alias)
JOIN products p ON lower(p.name) = lower(v.product)
JOIN product_features f ON f.product_id = p.id AND lower(f.name) = lower(v.feature)
WHERE NOT EXISTS (SELECT 1 FROM product_features x WHERE x.product_id = p.id AND lower(x.name) = lower(v.alias))
  AND NOT EXISTS (
      SELECT 1 FROM product_feature_aliases a JOIN product_features af ON af.id = a.feature_id
      WHERE af.product_id = p.id AND lower(a.alias) = lower(v.alias))
ON CONFLICT DO NOTHING;

-- Features found in the review queue. These are capabilities of an existing product, not standalone products.
CREATE TEMP TABLE catalog_seed_review_features (product TEXT NOT NULL, feature TEXT NOT NULL);

INSERT INTO catalog_seed_review_features (product, feature) VALUES
    ('.NET', 'Closed hierarchies'),
    ('.NET', 'Lazy<T>'),
    ('.NET', 'LazyInitializer'),
    ('.NET', 'Union types'),
    ('ASP.NET Core', 'In-browser form validation'),
    ('ASP.NET Core', 'Interactive Auto rendering'),
    ('ASP.NET Core', 'Interactive Server rendering'),
    ('ASP.NET Core', 'Interactive WebAssembly rendering'),
    ('ASP.NET Core', 'QuickGrid'),
    ('ASP.NET Core', 'Static server-side rendering'),
    ('Arnis', 'World generation'),
    ('Azure AI Speech', 'LLM Speech'),
    ('Azure AI Speech', 'Voice Live API'),
    ('Azure API Management', 'A365 integration'),
    ('Azure API Management', 'Token limit policy'),
    ('Azure App Configuration', 'Enhanced Feature Flags'),
    ('Azure Application Gateway', 'JSON Web Token validation'),
    ('Azure Application Gateway', 'Model Context Protocol gateway'),
    ('Azure Application Gateway', 'Private ingress'),
    ('Azure Application Gateway', 'Web Application Firewall insights'),
    ('Azure Arc', 'Automatic Agent Upgrade'),
    ('Azure Arc', 'Azure Arc-enabled SQL Server'),
    ('Azure Cloud HSM', 'Hardware Audit Logging'),
    ('Azure Container Apps', 'Jobs'),
    ('Azure Container Registry', 'ABAC repository permissions'),
    ('Azure Container Registry', 'ACR Tasks'),
    ('Azure Container Registry', 'Connected registry'),
    ('Azure Container Registry', 'Data transfer and transactions metrics'),
    ('Azure Container Registry', 'Managed identity authentication'),
    ('Azure Container Registry', 'Retention policy'),
    ('Azure Container Registry', 'Soft delete'),
    ('Azure Cosmos DB', 'Rust SDK'),
    ('Azure Data Manager for Energy', 'Agile reservoir modeling integration'),
    ('Azure Database for PostgreSQL', 'PgBouncer'),
    ('Azure Database for PostgreSQL', 'Private endpoints'),
    ('Azure Database for PostgreSQL', 'Read replicas'),
    ('Azure Database for PostgreSQL', 'REST API'),
    ('Azure Database for PostgreSQL', 'Virtual endpoints'),
    ('Azure Database for PostgreSQL', 'Zstd WAL compression'),
    ('Azure Databricks', 'Back-end Private Link'),
    ('Azure Databricks', 'Environment variables for serverless jobs'),
    ('Azure Databricks', 'Front-end Private Link'),
    ('Azure Databricks', 'Lakeflow Jobs'),
    ('Azure Databricks', 'Secure Cluster Connectivity'),
    ('Azure Databricks', 'Serverless outbound connectivity'),
    ('Azure Databricks', 'Unity Catalog'),
    ('Azure Databricks', 'VNet injection'),
    ('Azure Developer CLI', 'Evaluations extension'),
    ('Azure Developer CLI', 'Microsoft Foundry extension'),
    ('Azure Event Grid', 'Health event notifications'),
    ('Azure Files', 'Revert Snapshot'),
    ('Azure Files', 'Vaulted backup'),
    ('Azure Firewall', 'IPv6 application rules'),
    ('Azure Front Door', 'Post-quantum TLS key exchange'),
    ('Azure Front Door', 'Private Link origins'),
    ('Azure Key Vault Managed HSM', 'External Key Management'),
    ('Azure Kubernetes Service', 'Azure portal support'),
    ('Azure Kubernetes Service', 'Flex nodes'),
    ('Azure Kubernetes Service', 'Kubernetes workload placement'),
    ('Azure Local', 'Arc gateway'),
    ('Azure Local', 'Automatic recovery key backup'),
    ('Azure Local', 'Azure Local VMs'),
    ('Azure Local', 'Confidential virtual machines'),
    ('Azure Local', 'Connected-to-disconnected rehoming'),
    ('Azure Local', 'Deployment without Active Directory'),
    ('Azure Local', 'Infrastructure logical network'),
    ('Azure Local', 'Proxy settings'),
    ('Azure Local', 'Scheduled backups'),
    ('Azure Local', 'SDN enabled by Arc'),
    ('Azure Local', 'Solution updates'),
    ('Azure Logic Apps', 'Custom code'),
    ('Azure Logic Apps', 'Hybrid Logic Apps'),
    ('Azure Logic Apps', 'Local CLI'),
    ('Azure Logic Apps', 'Logic Apps Automation'),
    ('Azure Logic Apps', 'Logic Apps Standard extension for Azure CLI'),
    ('Azure Migrate', 'Business case'),
    ('Azure Migrate', 'NAS share assessment'),
    ('Azure Migrate', 'NAS share discovery'),
    ('Azure Migrate', 'NAS share discovery and assessment'),
    ('Azure Monitor', 'Advanced platform metrics'),
    ('Azure Monitor', 'Data collection rules'),
    ('Azure Monitor', 'DCE private IP endpoint compression'),
    ('Azure Monitor', 'IIS log collection'),
    ('Azure Monitor', 'Metrics usage insights'),
    ('Azure Monitor', 'Performance counter collection'),
    ('Azure Monitor', 'Windows event collection'),
    ('Azure OpenAI Service', 'Decisions API'),
    ('Azure Pipelines', 'Microsoft-hosted agents'),
    ('Azure Pipelines', 'NodeTool task'),
    ('Azure Pipelines', 'Self-hosted agents'),
    ('Azure Pipelines', 'UseNode task'),
    ('Azure Policy', 'Agent ID support'),
    ('Azure Policy', 'denyAction effect'),
    ('Azure Policy', 'Enforcement mode'),
    ('Azure Policy', 'Identity-aware policy evaluation'),
    ('Azure Policy', 'Identity-based exemptions'),
    ('Azure Policy', 'IP address expressions'),
    ('Azure Policy', 'Policy effect overrides'),
    ('Azure Policy', 'Resource selectors'),
    ('Azure Private Link', 'Private endpoints'),
    ('Azure Resource Health', 'VM health events'),
    ('Azure SQL Database', 'Audit logging'),
    ('Azure SQL Database', 'Cross-region recovery'),
    ('Azure SQL Database', 'Database watcher'),
    ('Azure SQL Database', 'Database-level managed identity'),
    ('Azure SQL Database', 'Deleted resource recovery'),
    ('Azure SQL Database', 'Hyperscale'),
    ('Azure SQL Database', 'Long-term retention backups'),
    ('Azure SQL Database', 'Modern Query Editor'),
    ('Azure SQL Database', 'Point-in-time restore'),
    ('Azure SQL Database', 'Point-in-time restore backup immutability'),
    ('Azure SQL Database', 'Portal restore'),
    ('Azure SQL Managed Instance', 'Audit logging'),
    ('Azure SQL Managed Instance', 'Cross-region recovery'),
    ('Azure SQL Managed Instance', 'Deleted resource recovery'),
    ('Azure SQL Managed Instance', 'Long-term retention backups'),
    ('Azure SQL Managed Instance', 'Point-in-time restore'),
    ('Azure SQL Managed Instance', 'Point-in-time restore backup immutability'),
    ('Azure SRE Agent', 'Skills'),
    ('Azure Service Bus', 'Geo-Replication'),
    ('Azure Service Bus', 'Partitioning'),
    ('Azure Site Recovery', 'Disaster recovery for Azure Local'),
    ('Azure Site Recovery', 'Flexible orchestration support'),
    ('Azure Skills', 'AKS Skills'),
    ('Azure Skills', 'Azure Kusto Graph Skills'),
    ('Azure Skills', 'Azure Local Skills'),
    ('Azure Skills', 'Azure plugin'),
    ('Azure Skills', 'Foundry IQ Skills'),
    ('Azure Storage Discovery', 'Advanced capacity insights'),
    ('Azure Storage Mover', 'Azure Files-to-Azure Files data copy'),
    ('Azure Virtual Machines', 'Azure Failure Prediction and Detection'),
    ('Azure Virtual Machines', 'Live migration'),
    ('Azure Virtual Machines', 'Multi-disk crash-consistent restore points'),
    ('Azure Virtual Machines', 'Scheduled Events'),
    ('Azure Virtual Network', 'Out-of-tree driver'),
    ('Foundry Local', 'Agentic AI scenarios on Azure Local'),
    ('Foundry Local', 'Continuous batching'),
    ('Foundry Local', 'Foundry Local on Azure Local'),
    ('Foundry Local', 'Local inference'),
    ('Foundry Local', 'Prefix caching'),
    ('GitHub', '@GitHub in Slack and Teams'),
    ('GitHub', 'Accessibility statements'),
    ('GitHub', 'Activity summary'),
    ('GitHub', 'Community Standards'),
    ('GitHub', 'Conversation filters'),
    ('GitHub', 'Enterprise installations'),
    ('GitHub', 'Git storage architecture'),
    ('GitHub', 'Issue Artifacts'),
    ('GitHub', 'Legacy GitHub app for Microsoft Teams'),
    ('GitHub', 'Pull request access controls'),
    ('GitHub', 'Pull request limits'),
    ('GitHub', 'Pull request overview'),
    ('GitHub', 'Secret risk assessment'),
    ('GitHub Actions', 'Actions event policies'),
    ('GitHub Actions', 'Control flow graph'),
    ('GitHub Actions', 'Manual workflow runs'),
    ('GitHub Actions', 'OpenID Connect'),
    ('GitHub Actions', 'Scheduled workflows'),
    ('GitHub Actions', 'Sparse checkout'),
    ('GitHub Actions', 'Workflow execution protections'),
    ('GitHub Advanced Security', 'AI push protection'),
    ('GitHub Advanced Security', 'AI Scan'),
    ('GitHub Advanced Security', 'AI secret detection'),
    ('GitHub Advanced Security', 'AI-powered PR scanning'),
    ('GitHub Advanced Security', 'GitHub Code Quality'),
    ('GitHub Advanced Security', 'License compliance'),
    ('GitHub Advanced Security', 'Package firewall'),
    ('GitHub Advanced Security', 'Push protection'),
    ('GitHub Advanced Security', 'Reachability analysis'),
    ('GitHub Advanced Security', 'Whole-repository AI scanning'),
    ('GitHub Advanced Security for Azure DevOps', 'Agentic Autofix'),
    ('GitHub CLI', 'Expiring, automatically refreshing OAuth credentials'),
    ('GitHub Code Security', 'Code security risk assessment'),
    ('GitHub CodeQL', 'AI threat model'),
    ('GitHub CodeQL', 'Intel-based Mac support'),
    ('GitHub CodeQL', 'JSON data extensions'),
    ('GitHub Copilot', 'Agent Merge'),
    ('GitHub Copilot', 'Agentic Autofix'),
    ('GitHub Copilot', 'AI models'),
    ('GitHub Copilot', 'AI Scan'),
    ('GitHub Copilot', 'Assisted approval instructions'),
    ('GitHub Copilot', 'Assisted permissions'),
    ('GitHub Copilot', 'Cloud sandboxes'),
    ('GitHub Copilot', 'Computer Use'),
    ('GitHub Copilot', 'Copilot Automations'),
    ('GitHub Copilot', 'Copilot Connectors'),
    ('GitHub Copilot', 'Copilot Dreaming'),
    ('GitHub Copilot', 'Copilot impact dashboard'),
    ('GitHub Copilot', 'Copilot sub-agents'),
    ('GitHub Copilot', 'Custom agents'),
    ('GitHub Copilot', 'GitHub Canvas'),
    ('GitHub Copilot', 'GitHub Copilot in Teams'),
    ('GitHub Copilot', 'GitHub security extension'),
    ('GitHub Copilot', 'Hooks'),
    ('GitHub Copilot', 'Image generation'),
    ('GitHub Copilot', 'Interactive canvases'),
    ('GitHub Copilot', 'JetBrains Air integration'),
    ('GitHub Copilot', 'Local automations'),
    ('GitHub Copilot', 'Model policy API'),
    ('GitHub Copilot', 'Model providers'),
    ('GitHub Copilot', 'Multi-repository support'),
    ('GitHub Copilot', 'Natural-language controls'),
    ('GitHub Copilot', 'Plan Mode'),
    ('GitHub Copilot', 'Server-to-server authentication'),
    ('GitHub Copilot', 'Session continuity'),
    ('GitHub Copilot', 'Shared memory'),
    ('GitHub Copilot', 'Upgrade agent'),
    ('GitHub Copilot', 'Verified proficiency'),
    ('GitHub Copilot', 'Xcode integration'),
    ('GitHub Copilot CLI', 'Allow-all permissions'),
    ('GitHub Copilot CLI', 'Plugins'),
    ('GitHub Copilot CLI', 'Settings dashboard'),
    ('GitHub Copilot CLI', 'Skills'),
    ('GitHub Copilot app', 'Copilot app Security canvas'),
    ('GitHub Dependabot', 'Agent Package Manager support'),
    ('GitHub Dependabot', 'Production context'),
    ('GitHub Dependabot', 'Python ecosystem support'),
    ('GitHub Dependabot', 'Reachability analysis'),
    ('GitHub Dependabot', 'Version updates'),
    ('GitHub Desktop', 'Short-lived authentication tokens'),
    ('GitHub Enterprise Cloud', 'Cost center membership lookup'),
    ('GitHub Enterprise Cloud', 'GitHub package firewall'),
    ('GitHub Enterprise Cloud', 'Public monitoring'),
    ('GitHub Enterprise Cloud', 'Unaffiliated users'),
    ('GitHub Secret Protection', 'Audit log events'),
    ('KEDA', 'GitHub Runner Scaler'),
    ('Microsoft 365 Copilot', 'Copilot Cowork'),
    ('Microsoft Agent Framework', 'Builder skills'),
    ('Microsoft Copilot', 'Agent ID support'),
    ('Microsoft Copilot', 'Browser automation'),
    ('Microsoft Copilot', 'Business process memory'),
    ('Microsoft Copilot', 'Copilot Projects'),
    ('Microsoft Copilot', 'Copilot Workspaces'),
    ('Microsoft Copilot', 'Insights agent'),
    ('Microsoft Copilot', 'Referencing previous conversations'),
    ('Microsoft Copilot', 'Role plugins'),
    ('Microsoft Copilot', 'Teams recap'),
    ('Microsoft Copilot', 'Triggers'),
    ('Microsoft Copilot', 'Work IQ MCP retrieve'),
    ('Microsoft Copilot Studio', 'Agent evaluations'),
    ('Microsoft Copilot Studio', 'Agent handoffs'),
    ('Microsoft Copilot Studio', 'Agent lifecycle management'),
    ('Microsoft Copilot Studio', 'Automation flow evaluations'),
    ('Microsoft Copilot Studio', 'Connected agents'),
    ('Microsoft Copilot Studio', 'Context passing'),
    ('Microsoft Copilot Studio', 'Conversation Harness'),
    ('Microsoft Copilot Studio', 'Evaluation warnings'),
    ('Microsoft Copilot Studio', 'Evaluations'),
    ('Microsoft Copilot Studio', 'Human escalation'),
    ('Microsoft Copilot Studio', 'Multi-agent orchestration'),
    ('Microsoft Copilot Studio', 'Tool accuracy grader'),
    ('Microsoft Copilot Studio', 'Topics'),
    ('Microsoft Defender', 'Local AI agent discovery'),
    ('Microsoft Defender Antivirus', 'Attack surface reduction rules'),
    ('Microsoft Defender Antivirus', 'Cloud-delivered protection'),
    ('Microsoft Defender Antivirus', 'Potentially unwanted application protection'),
    ('Microsoft Defender Antivirus', 'Reputation-based protection'),
    ('Microsoft Defender Antivirus', 'Tamper protection'),
    ('Microsoft Defender Experts', 'Defender Experts investigations'),
    ('Microsoft Defender Vulnerability Management', 'Exposure Score'),
    ('Microsoft Defender Vulnerability Management', 'Security recommendations'),
    ('Microsoft Defender XDR', 'Advanced hunting'),
    ('Microsoft Defender XDR', 'Automatic attack disruption'),
    ('Microsoft Defender XDR', 'Threat analytics'),
    ('Microsoft Defender for Cloud', 'AI threat protection'),
    ('Microsoft Defender for Cloud', 'Defender for Key Vault'),
    ('Microsoft Defender for Office 365', 'Attack simulation training'),
    ('Microsoft Defender for Office 365', 'Move to Quarantine'),
    ('Microsoft Defender for Office 365', 'Safe Attachments'),
    ('Microsoft Defender for Office 365', 'Safe Links'),
    ('Microsoft Defender for Office 365', 'Zero-hour auto purge'),
    ('Microsoft Entra ID', 'Workload identity risk protection'),
    ('Microsoft Fabric', 'Additional table metadata API'),
    ('Microsoft Fabric', 'Admin connection APIs'),
    ('Microsoft Fabric', 'Advanced DAX generation'),
    ('Microsoft Fabric', 'Advanced NL2SQL'),
    ('Microsoft Fabric', 'Authentication allowlist'),
    ('Microsoft Fabric', 'Business Actions'),
    ('Microsoft Fabric', 'Capacity Metrics'),
    ('Microsoft Fabric', 'Connectors'),
    ('Microsoft Fabric', 'Copilot dashboard generation'),
    ('Microsoft Fabric', 'Credential vending'),
    ('Microsoft Fabric', 'Custom visuals with Copilot'),
    ('Microsoft Fabric', 'Database Hub'),
    ('Microsoft Fabric', 'Direct Lake mode'),
    ('Microsoft Fabric', 'Endpoint discovery API'),
    ('Microsoft Fabric', 'Fabric Actions'),
    ('Microsoft Fabric', 'Fabric Apps'),
    ('Microsoft Fabric', 'Fabric Data Warehouse integration with Fabric User Data Functions'),
    ('Microsoft Fabric', 'Fabric Migration Assistant Teradata support'),
    ('Microsoft Fabric', 'Govern Skills'),
    ('Microsoft Fabric', 'GPU-accelerated query acceleration'),
    ('Microsoft Fabric', 'Iceberg writes'),
    ('Microsoft Fabric', 'Internal shortcuts'),
    ('Microsoft Fabric', 'Mirrored Azure Databricks catalog'),
    ('Microsoft Fabric', 'OneLake catalog Govern experience'),
    ('Microsoft Fabric', 'OneLake external-location support'),
    ('Microsoft Fabric', 'OneLake hosted MCP server'),
    ('Microsoft Fabric', 'OneLake security for Eventhouse external tables'),
    ('Microsoft Fabric', 'Query history'),
    ('Microsoft Fabric', 'Real-Time dashboards'),
    ('Microsoft Fabric', 'Reference Data Join'),
    ('Microsoft Fabric', 'Regular expression functions'),
    ('Microsoft Fabric', 'Salesforce Data 360 integration'),
    ('Microsoft Fabric', 'Schema-aware Eventstreams'),
    ('Microsoft Fabric', 'Semantic model assistance'),
    ('Microsoft Fabric', 'Shortcut transformations'),
    ('Microsoft Fabric', 'Skills for Fabric'),
    ('Microsoft Fabric', 'Snowflake Iceberg in OneLake'),
    ('Microsoft Fabric', 'SQL analytics endpoint'),
    ('Microsoft Fabric', 'Tenant allowlist'),
    ('Microsoft Fabric', 'Variable library integration with Eventstream'),
    ('Microsoft Fabric', 'Warehouse Monitor'),
    ('Microsoft Fabric', 'Workspace-level private connectivity for Eventstream custom endpoint sources'),
    ('Microsoft Foundry', 'Browser automation'),
    ('Microsoft Foundry', 'Cloud evaluations'),
    ('Microsoft Foundry', 'Continuous evaluation'),
    ('Microsoft Foundry', 'Conversation sandbox'),
    ('Microsoft Foundry', 'Copilot harness'),
    ('Microsoft Foundry', 'Custom code evaluators'),
    ('Microsoft Foundry', 'Decisions API'),
    ('Microsoft Foundry', 'Direct SIP'),
    ('Microsoft Foundry', 'Evaluations'),
    ('Microsoft Foundry', 'Fabric data agent custom-tool integration'),
    ('Microsoft Foundry', 'Foundry workflows'),
    ('Microsoft Foundry', 'Grounding with Bing Custom Search'),
    ('Microsoft Foundry', 'Guardrails'),
    ('Microsoft Foundry', 'Hosted Agents'),
    ('Microsoft Foundry', 'Image generation'),
    ('Microsoft Foundry', 'Long-running hosted-agent resilience'),
    ('Microsoft Foundry', 'Managed Fine-Tuning'),
    ('Microsoft Foundry', 'Microsoft Entra ID authentication'),
    ('Microsoft Foundry', 'Model catalog'),
    ('Microsoft Foundry', 'Model evaluation'),
    ('Microsoft Foundry', 'Model Router'),
    ('Microsoft Foundry', 'Multi-LoRA adapter deployment on Managed Compute'),
    ('Microsoft Foundry', 'Native subagents'),
    ('Microsoft Foundry', 'Priority pool'),
    ('Microsoft Foundry', 'Prompt Agents'),
    ('Microsoft Foundry', 'Trace dataset generation'),
    ('Microsoft Foundry', 'Video generation'),
    ('Microsoft Foundry Agent Service', 'Agent evaluators'),
    ('Microsoft Foundry Agent Service', 'Agent Inspector'),
    ('Microsoft Foundry Agent Service', 'Agent Optimizer'),
    ('Microsoft Graph', 'Create channel API'),
    ('Microsoft Intune', 'Endpoint Privilege Management'),
    ('Microsoft Purview', 'Case access diagnostics'),
    ('Microsoft Purview', 'eDiscovery diagnostics'),
    ('Microsoft Purview', 'Hold-policy distribution diagnostics'),
    ('Microsoft Purview', 'Security filter diagnostics'),
    ('Microsoft Security Copilot', 'Security Copilot agents'),
    ('Microsoft Security Exposure Management', 'Attack Path Analysis'),
    ('Microsoft Sentinel', 'Deception'),
    ('Microsoft Teams', 'AI-powered phishing detection'),
    ('Microsoft Teams', 'External access'),
    ('Microsoft Windows', 'Hotpatching'),
    ('Microsoft Windows', 'Optional non-security preview updates'),
    ('Microsoft Windows', 'Power mode'),
    ('Microsoft Windows', 'Security updates'),
    ('Microsoft Windows', 'Startup apps'),
    ('Microsoft Windows', 'Virtual desktops'),
    ('Microsoft Windows', 'Windows Security'),
    ('Microsoft Windows', 'Windows security baselines'),
    ('Microsoft Windows', 'Windows Update'),
    ('Microsoft Windows Server', 'Hotpatching'),
    ('Node.js', 'Permission Model'),
    ('Node.js', 'Single executable applications'),
    ('Node.js', 'Startup snapshots'),
    ('Oracle AI Database@Azure', 'Azure proximity placement group integration'),
    ('Visual Studio Code', 'MCP server configuration'),
    ('Visual Studio Marketplace', 'Trusted Publishing');

INSERT INTO product_features (product_id, name)
SELECT p.id, s.feature
FROM catalog_seed_review_features s
JOIN products p ON lower(p.name) = lower(s.product)
ON CONFLICT (product_id, lower(name)) DO NOTHING;

DROP TABLE catalog_seed_review_features;

-- These names describe the same capability, so keep one feature and resolve the other spellings as aliases.
INSERT INTO product_feature_aliases (feature_id, alias)
SELECT f.id, v.alias
FROM (VALUES
    ('Azure Container Registry', 'Data transfer and transactions metrics', 'Data Transfer metric'),
    ('Azure Container Registry', 'Data transfer and transactions metrics', 'Transactions and Data Transfer metrics'),
    ('Azure Cosmos DB', 'Rust SDK', 'Rust client'),
    ('GitHub Advanced Security', 'Package firewall', 'GitHub package firewall'),
    ('GitHub Advanced Security', 'Package firewall', 'Package Firewall'),
    ('GitHub Copilot', 'Copilot Automations', 'Shared Copilot Automations'),
    ('Microsoft Fabric', 'Admin connection APIs', 'Admin Connections REST APIs'),
    ('Microsoft Fabric', 'Capacity Metrics', 'Fabric Capacity Metrics'),
    ('Microsoft Fabric', 'Real-Time dashboards', 'Real-Time Dashboards multi-KPI'),
    ('Microsoft Fabric', 'Shortcut transformations', 'Shortcut transforms'),
    ('Microsoft Fabric', 'Warehouse Monitor', 'Monitor in warehouse'),
    ('Microsoft Foundry', 'Browser automation', 'Browser Automation Tool'),
    ('Microsoft Foundry', 'Copilot harness', 'Microsoft Copilot Harness'),
    ('Microsoft Foundry', 'Copilot harness', 'Unified Copilot harness'),
    ('Microsoft Foundry', 'Long-running hosted-agent resilience', 'Long-running resilience'),
    ('Microsoft Foundry', 'Long-running hosted-agent resilience', 'Long-running resilience for hosted agents'),
    ('Microsoft Foundry', 'Multi-LoRA adapter deployment on Managed Compute', 'Multi-LoRA Managed Compute'),
    ('Microsoft Intune', 'Endpoint Privilege Management', 'Microsoft Intune Endpoint Privilege Management')
) AS v (product, feature, alias)
JOIN products p ON lower(p.name) = lower(v.product)
JOIN product_features f ON f.product_id = p.id AND lower(f.name) = lower(v.feature)
WHERE NOT EXISTS (SELECT 1 FROM product_features x WHERE x.product_id = p.id AND lower(x.name) = lower(v.alias))
  AND NOT EXISTS (
      SELECT 1 FROM product_feature_aliases a
      JOIN product_features af ON af.id = a.feature_id
      WHERE af.product_id = p.id AND lower(a.alias) = lower(v.alias))
ON CONFLICT DO NOTHING;

-- What the features that others were merged into are about (the page shows these descriptions).
UPDATE product_features f
SET description = v.description, updated_at = NOW()
FROM (VALUES
    ('Copilot Chat', 'Ask Copilot about your code in a conversation: in the IDE, on GitHub.com and in GitHub Mobile. Explain, generate, fix and refactor without leaving where you work.'),
    ('Custom instructions', 'Tell Copilot how your team works: instructions for a user, a path in a repository, a repository or a whole organization, and for specific tasks such as commit messages.'),
    ('Slash commands', 'Short commands in Copilot Chat for common tasks: explain code, fix it, write tests or documentation.'),
    ('Pull request summaries', 'Copilot writes the description of a pull request from its changes, in Visual Studio Code and on GitHub.com.'),
    ('Code review', 'Ask Copilot to review a pull request or a code change. It leaves inline comments with suggested fixes that can be applied directly.'),
    ('Model selection', 'Choose which model Copilot uses for chat and for agents.'),
    ('Copilot Spaces', 'Collections of repositories, files and guidelines that ground Copilot answers in the context of a project or a team.'),
    ('Usage metrics', 'Usage data of Copilot, in the dashboards and through an API: acceptance rates, active users and model usage.'),
    ('SDK', 'A software development kit for building agents on top of Copilot and bringing its agent capabilities into your own applications.')
) AS v (name, description)
JOIN products p ON lower(p.name) = 'github copilot'
WHERE f.product_id = p.id AND lower(f.name) = lower(v.name);

-- ── 6. Links of the existing articles and events ─────────────────────────────────────────────────────────────────────────────────
-- A name that is a product or an alias is not ignored.
DELETE FROM ignored_product_names i
WHERE EXISTS (SELECT 1 FROM products p WHERE lower(p.name) = lower(i.name))
   OR EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(i.name));

-- The articles: the metadata of schema version 2 lists the products of an article.
WITH src AS (
    SELECT ci.collection_name,
           ci.slug,
           regexp_replace(btrim(n.name), '\s+', ' ', 'g') AS name,
           n.ordinal
    FROM content_items ci
    CROSS JOIN LATERAL jsonb_array_elements_text(
        CASE WHEN COALESCE((ci.ai_metadata ->> 'schema_version') ~ '^\d+$', FALSE)
                  AND (ci.ai_metadata ->> 'schema_version')::int >= 2
                  AND jsonb_typeof(ci.ai_metadata -> 'products') = 'array'
             THEN ci.ai_metadata -> 'products'
             ELSE '[]'::jsonb END) WITH ORDINALITY AS n (name, ordinal)
),
firsts AS (
    SELECT DISTINCT ON (collection_name, slug, lower(name)) collection_name, slug, name, ordinal
    FROM src
    WHERE name <> ''
    ORDER BY collection_name, slug, lower(name), ordinal
)
INSERT INTO content_item_products (collection_name, item_slug, product_name, position, product_id)
SELECT collection_name, slug, name,
       (ROW_NUMBER() OVER (PARTITION BY collection_name, slug ORDER BY ordinal))::int - 1,
       find_product_id(name)
FROM firsts
ON CONFLICT DO NOTHING;

SELECT relink_item_products();
SELECT relink_timeline_events();
