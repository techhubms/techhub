-- Product hierarchy. Azure products are separate services, but a product like GitHub Copilot is a family: the CLI,
-- the coding agent and code review are products of their own that belong to GitHub Copilot. A product can have a
-- parent; the parent shows the articles and dates of its children. Articles and events keep linking to the most
-- specific product that matches their name.
ALTER TABLE products ADD COLUMN IF NOT EXISTS parent_product_id BIGINT
    REFERENCES products (id) ON DELETE SET NULL
    CHECK (parent_product_id IS NULL OR parent_product_id <> id);

CREATE INDEX IF NOT EXISTS idx_products_parent ON products (parent_product_id);

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

-- The products of the GitHub Copilot family that are named in articles.
INSERT INTO products (name) VALUES
    ('GitHub Copilot Chat'),
    ('GitHub Copilot CLI'),
    ('GitHub Copilot code review'),
    ('GitHub Copilot coding agent'),
    ('GitHub Copilot Fusion'),
    ('GitHub Copilot SDK')
ON CONFLICT (lower(name)) DO NOTHING;

UPDATE products c SET parent_product_id = p.id
FROM (VALUES
    ('GitHub Copilot Chat', 'GitHub Copilot'),
    ('GitHub Copilot CLI', 'GitHub Copilot'),
    ('GitHub Copilot code review', 'GitHub Copilot'),
    ('GitHub Copilot coding agent', 'GitHub Copilot'),
    ('GitHub Copilot Fusion', 'GitHub Copilot'),
    ('GitHub Copilot SDK', 'GitHub Copilot'),
    ('Microsoft Foundry Agent Service', 'Microsoft Foundry'),
    ('ASP.NET Core Blazor', 'ASP.NET Core'),
    ('GitHub Enterprise Cloud', 'GitHub Enterprise'),
    ('GitHub Enterprise Server', 'GitHub Enterprise'),
    ('Microsoft Defender for Cloud', 'Microsoft Defender'),
    ('Microsoft Defender for Endpoint', 'Microsoft Defender'),
    ('Microsoft Defender for Identity', 'Microsoft Defender'),
    ('Microsoft Defender for Office 365', 'Microsoft Defender'),
    ('Microsoft Defender Vulnerability Management', 'Microsoft Defender'),
    ('Microsoft Defender XDR', 'Microsoft Defender'),
    ('.NET MAUI CLI', '.NET MAUI')
) AS v (child, parent)
JOIN products p ON lower(p.name) = lower(v.parent)
WHERE lower(c.name) = lower(v.child) AND c.parent_product_id IS NULL;

-- The new products may match names that were not in the catalog yet.
UPDATE content_item_products SET product_id = find_product_id(product_name) WHERE product_id IS NULL;
UPDATE content_timeline_events SET product_id = find_product_id(product) WHERE product_id IS NULL;
