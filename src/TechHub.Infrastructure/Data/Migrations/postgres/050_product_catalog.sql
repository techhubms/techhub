-- Product catalog. The AI returns product names as free text (ai_metadata.products and the timeline events); that text
-- stays as it is. This catalog is a curated layer on top: products with aliases, plus link tables that connect the raw
-- names to a product, so an article can be about several products and a product has many articles and dates.
--
--   products                 curated, canonical product names.
--   product_aliases          other names for a product (old names, spelling variants). The same name can only
--                            belong to one product.
--   ignored_product_names    names the AI returns that are not products (an event, a feature, an umbrella brand),
--                            so they stay out of the review queue.
--   content_item_products    the product names an article lists, in classifier order, with the product they match.
--                            product_id is NULL while the name is not in the catalog (the review queue).
--   content_timeline_events  gets the same product_id; the raw product text stays on the event.
--
-- A name matches a product by name or alias, case-insensitive (find_product_id). The links are kept in sync when an
-- article is written and again whenever the catalog changes.
CREATE TABLE IF NOT EXISTS products (
    id         BIGSERIAL PRIMARY KEY,
    name       TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_products_name ON products (lower(name));

CREATE TABLE IF NOT EXISTS product_aliases (
    id         BIGSERIAL PRIMARY KEY,
    product_id BIGINT NOT NULL REFERENCES products (id) ON DELETE CASCADE,
    alias      TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_product_aliases_alias ON product_aliases (lower(alias));
CREATE INDEX IF NOT EXISTS idx_product_aliases_product ON product_aliases (product_id);

CREATE TABLE IF NOT EXISTS ignored_product_names (
    id         BIGSERIAL PRIMARY KEY,
    name       TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_ignored_product_names_name ON ignored_product_names (lower(name));

CREATE TABLE IF NOT EXISTS content_item_products (
    collection_name TEXT NOT NULL,
    item_slug       TEXT NOT NULL,
    product_name    TEXT NOT NULL,           -- as the AI returned it
    position        INTEGER NOT NULL,        -- classifier order, 0 is the main product
    product_id      BIGINT REFERENCES products (id) ON DELETE SET NULL,
    PRIMARY KEY (collection_name, item_slug, product_name),
    FOREIGN KEY (collection_name, item_slug)
        REFERENCES content_items (collection_name, slug) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_content_item_products_product ON content_item_products (product_id);
CREATE INDEX IF NOT EXISTS idx_content_item_products_name ON content_item_products (lower(product_name));

ALTER TABLE content_timeline_events ADD COLUMN IF NOT EXISTS product_id BIGINT REFERENCES products (id) ON DELETE SET NULL;
CREATE INDEX IF NOT EXISTS idx_timeline_events_product_id ON content_timeline_events (product_id);

-- The product a name belongs to, or NULL when it is not in the catalog.
CREATE OR REPLACE FUNCTION find_product_id(product_name TEXT) RETURNS BIGINT
LANGUAGE sql STABLE AS $$
    SELECT COALESCE(
        (SELECT p.id FROM products p
          WHERE lower(p.name) = lower(regexp_replace(btrim(product_name), '\s+', ' ', 'g'))),
        (SELECT a.product_id FROM product_aliases a
          WHERE lower(a.alias) = lower(regexp_replace(btrim(product_name), '\s+', ' ', 'g'))))
$$;
