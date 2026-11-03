-- Plans and surfaces. A product like GitHub Copilot is available in different subscription plans (Free, Pro, Business,
-- Enterprise) and on different surfaces (VS Code, JetBrains, github.com), and a feature can be in preview on one and
-- generally available on another. Those are not products of their own, so they are a controlled vocabulary of
-- qualifiers that products, aliases and timeline events can carry:
--
--   product_qualifiers          the plans and surfaces ('kind'), unique per kind ignoring case.
--   product_qualifier_aliases   other names for a qualifier ('VS Code' for 'Visual Studio Code'), unique ignoring case.
--   product_aliases.qualifier_id  an alias can imply a qualifier: 'GitHub Copilot Business' is the product GitHub Copilot
--                               in the plan Business. Simplifying the name to the product loses nothing.
--   content_item_products.qualifier_id  the qualifier an article's product name implies.
--   content_timeline_events.plans/surfaces  the raw names the AI returned for an event, as text.
--   content_timeline_event_qualifiers  the qualifiers of an event: from its plans and surfaces and from its product name.
--
-- The links are kept in sync by relink_item_products and relink_timeline_events, which run when an article or event is
-- written and again whenever the catalog changes.

-- A name without surrounding or repeated whitespace, in lower case.
CREATE OR REPLACE FUNCTION catalog_key(value TEXT) RETURNS TEXT
LANGUAGE sql IMMUTABLE AS $$
    SELECT lower(regexp_replace(btrim(value), '\s+', ' ', 'g'))
$$;

CREATE TABLE IF NOT EXISTS product_qualifiers (
    id         BIGSERIAL PRIMARY KEY,
    kind       TEXT NOT NULL CHECK (kind IN ('plan', 'surface')),
    name       TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_qualifiers_name ON product_qualifiers (kind, lower(name));

CREATE TABLE IF NOT EXISTS product_qualifier_aliases (
    id           BIGSERIAL PRIMARY KEY,
    qualifier_id BIGINT NOT NULL REFERENCES product_qualifiers (id) ON DELETE CASCADE,
    alias        TEXT NOT NULL,
    created_at   TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_qualifier_aliases_alias ON product_qualifier_aliases (lower(alias));
CREATE INDEX IF NOT EXISTS idx_product_qualifier_aliases_qualifier ON product_qualifier_aliases (qualifier_id);

ALTER TABLE product_aliases ADD COLUMN IF NOT EXISTS qualifier_id BIGINT REFERENCES product_qualifiers (id) ON DELETE SET NULL;
ALTER TABLE content_item_products ADD COLUMN IF NOT EXISTS qualifier_id BIGINT REFERENCES product_qualifiers (id) ON DELETE SET NULL;

ALTER TABLE content_timeline_events ADD COLUMN IF NOT EXISTS plans TEXT[] NOT NULL DEFAULT '{}';
ALTER TABLE content_timeline_events ADD COLUMN IF NOT EXISTS surfaces TEXT[] NOT NULL DEFAULT '{}';

CREATE TABLE IF NOT EXISTS content_timeline_event_qualifiers (
    event_id     BIGINT NOT NULL REFERENCES content_timeline_events (id) ON DELETE CASCADE,
    qualifier_id BIGINT NOT NULL REFERENCES product_qualifiers (id) ON DELETE CASCADE,
    PRIMARY KEY (event_id, qualifier_id)
);

CREATE INDEX IF NOT EXISTS idx_timeline_event_qualifiers_qualifier ON content_timeline_event_qualifiers (qualifier_id);

-- The qualifier of a kind that a name belongs to (by name or alias), or NULL when it is not in the vocabulary.
CREATE OR REPLACE FUNCTION find_qualifier_id(qualifier_kind TEXT, qualifier_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    SELECT COALESCE(
        (SELECT q.id FROM product_qualifiers q
          WHERE q.kind = qualifier_kind AND lower(q.name) = catalog_key(qualifier_name)),
        (SELECT q.id FROM product_qualifier_aliases a JOIN product_qualifiers q ON q.id = a.qualifier_id
          WHERE q.kind = qualifier_kind AND lower(a.alias) = catalog_key(qualifier_name)))
$$;

-- The qualifier a product name implies (the product alias 'GitHub Copilot Business' implies the plan Business).
CREATE OR REPLACE FUNCTION find_product_qualifier_id(product_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    SELECT a.qualifier_id FROM product_aliases a WHERE lower(a.alias) = catalog_key(product_name)
$$;

-- Links the product names of articles to the catalog. Without arguments it covers all articles.
CREATE OR REPLACE FUNCTION relink_item_products(p_collection TEXT DEFAULT NULL, p_slug TEXT DEFAULT NULL) RETURNS VOID
LANGUAGE sql AS $$
    UPDATE content_item_products l
    SET product_id = find_product_id(l.product_name),
        qualifier_id = find_product_qualifier_id(l.product_name)
    WHERE (p_collection IS NULL OR l.collection_name = p_collection)
      AND (p_slug IS NULL OR l.item_slug = p_slug)
      AND (l.product_id IS DISTINCT FROM find_product_id(l.product_name)
           OR l.qualifier_id IS DISTINCT FROM find_product_qualifier_id(l.product_name));
$$;

-- Links timeline events to the catalog: the product and the qualifiers. Without arguments it covers all events.
CREATE OR REPLACE FUNCTION relink_timeline_events(p_collection TEXT DEFAULT NULL, p_slug TEXT DEFAULT NULL) RETURNS VOID
LANGUAGE sql AS $$
    UPDATE content_timeline_events e
    SET product_id = find_product_id(e.product)
    WHERE (p_collection IS NULL OR e.collection_name = p_collection)
      AND (p_slug IS NULL OR e.item_slug = p_slug)
      AND e.product_id IS DISTINCT FROM find_product_id(e.product);

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

-- Starting vocabulary: the plans and surfaces of GitHub Copilot.
INSERT INTO product_qualifiers (kind, name) VALUES
    ('plan', 'Free'),
    ('plan', 'Pro'),
    ('plan', 'Pro+'),
    ('plan', 'Business'),
    ('plan', 'Enterprise'),
    ('surface', 'Visual Studio Code'),
    ('surface', 'Visual Studio'),
    ('surface', 'JetBrains IDEs'),
    ('surface', 'Xcode'),
    ('surface', 'Eclipse'),
    ('surface', 'Neovim'),
    ('surface', 'GitHub.com'),
    ('surface', 'GitHub Mobile')
ON CONFLICT (kind, lower(name)) DO NOTHING;

INSERT INTO product_qualifier_aliases (qualifier_id, alias)
SELECT q.id, v.alias
FROM (VALUES
    ('plan', 'Pro+', 'Pro Plus'),
    ('plan', 'Free', 'Copilot Free'),
    ('plan', 'Pro', 'Copilot Pro'),
    ('plan', 'Pro+', 'Copilot Pro+'),
    ('plan', 'Business', 'Copilot Business'),
    ('plan', 'Enterprise', 'Copilot Enterprise'),
    ('surface', 'Visual Studio Code', 'VS Code'),
    ('surface', 'Visual Studio Code', 'VSCode'),
    ('surface', 'JetBrains IDEs', 'JetBrains'),
    ('surface', 'JetBrains IDEs', 'JetBrains IDE'),
    ('surface', 'JetBrains IDEs', 'IntelliJ IDEA'),
    ('surface', 'Neovim', 'Vim'),
    ('surface', 'Neovim', 'Vim/Neovim'),
    ('surface', 'GitHub.com', 'github.com'),
    ('surface', 'GitHub.com', 'GitHub web'),
    ('surface', 'GitHub Mobile', 'Mobile')
) AS v (kind, qualifier, alias)
JOIN product_qualifiers q ON q.kind = v.kind AND lower(q.name) = lower(v.qualifier)
ON CONFLICT (lower(alias)) DO NOTHING;

-- 'GitHub Copilot Business' is GitHub Copilot in the plan Business. A bare 'Copilot Pro' is not mapped: Copilot also
-- names Microsoft 365 Copilot, Microsoft Copilot and others.
INSERT INTO product_aliases (product_id, alias, qualifier_id)
SELECT p.id, v.alias, q.id
FROM (VALUES
    ('GitHub Copilot Free', 'Free'),
    ('GitHub Copilot Pro', 'Pro'),
    ('GitHub Copilot Pro+', 'Pro+'),
    ('GitHub Copilot Business', 'Business'),
    ('GitHub Copilot Enterprise', 'Enterprise')
) AS v (alias, plan)
JOIN products p ON lower(p.name) = 'github copilot'
JOIN product_qualifiers q ON q.kind = 'plan' AND lower(q.name) = lower(v.plan)
ON CONFLICT (lower(alias)) DO NOTHING;

SELECT relink_item_products();
SELECT relink_timeline_events();
