-- Corrections after checking the ignored names against official vendor documentation.
-- A separately installed tool, deployable appliance or independently consumed API is a product.
-- Sources and the decisions for all 23 reviewed names are recorded in docs/product-catalog.md.

INSERT INTO products (name, parent_product_id)
SELECT v.name, parent.id
FROM (VALUES
    ('Agent Package Manager', NULL),
    ('Azure Arc resource bridge', 'Azure Arc'),
    ('Work IQ', 'Microsoft 365'),
    ('Windows Autopatch', 'Microsoft Windows'),
    ('ChatGPT', NULL),
    ('Claude', NULL),
    ('Google Gemini', NULL),
    ('Salesforce Data 360', NULL),
    ('Snowflake', NULL),
    ('Microsoft Learn', NULL)
) AS v (name, parent)
LEFT JOIN products parent ON parent.name = v.parent
WHERE v.parent IS NULL OR parent.id IS NOT NULL
ON CONFLICT (lower(name)) DO NOTHING;

INSERT INTO product_aliases (product_id, alias)
SELECT p.id, v.alias
FROM (VALUES
    ('Work IQ', 'Microsoft 365 Work IQ'),
    ('Work IQ', 'Microsoft 365 Copilot Work IQ')
) AS v (product, alias)
JOIN products p ON p.name = v.product
WHERE NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = lower(v.alias))
ON CONFLICT (lower(alias)) DO NOTHING;

-- These capabilities remain features, and a product-field spelling must resolve to their owner.
INSERT INTO product_features (product_id, name)
SELECT p.id, v.feature
FROM (VALUES
    ('Azure AI Speech', 'Voice Live API'),
    ('Microsoft Intune', 'Endpoint Privilege Management'),
    ('Azure Arc', 'Azure Arc-enabled SQL Server')
) AS v (product, feature)
JOIN products p ON p.name = v.product
ON CONFLICT (product_id, lower(name)) DO NOTHING;

INSERT INTO product_feature_aliases (feature_id, alias)
SELECT f.id, 'Microsoft Intune Endpoint Privilege Management'
FROM product_features f JOIN products p ON p.id = f.product_id
WHERE p.name = 'Microsoft Intune' AND f.name = 'Endpoint Privilege Management'
ON CONFLICT DO NOTHING;

-- Carry legacy feature aliases over to the promoted products before retiring the old feature rows.
INSERT INTO product_aliases (product_id, alias)
SELECT target.id, a.alias
FROM (VALUES
    ('Azure Arc', 'Azure Arc resource bridge', 'Azure Arc resource bridge'),
    ('Microsoft 365 Copilot', 'Work IQ', 'Work IQ'),
    ('Microsoft Intune', 'Windows Autopatch', 'Windows Autopatch')
) AS v (parent, feature, product)
JOIN products parent ON parent.name = v.parent
JOIN product_features f ON f.product_id = parent.id AND f.name = v.feature
JOIN product_feature_aliases a ON a.feature_id = f.id
JOIN products target ON target.name = v.product
WHERE NOT EXISTS (SELECT 1 FROM products p WHERE lower(p.name) = lower(a.alias))
ON CONFLICT (lower(alias)) DO NOTHING;

-- Earlier unreleased seeds made these products into features. Fresh restores no longer seed those rows.
-- On an already seeded database, refuse to discard manual feature curation.
DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM product_features f JOIN products p ON p.id = f.product_id
        WHERE (p.name, f.name) IN (
            ('Azure Arc', 'Azure Arc resource bridge'),
            ('Microsoft 365 Copilot', 'Work IQ'),
            ('Microsoft Intune', 'Windows Autopatch'))
          AND (f.description <> '' OR f.release_date IS NOT NULL
               OR EXISTS (SELECT 1 FROM product_feature_content c WHERE c.feature_id = f.id)
               OR EXISTS (SELECT 1 FROM content_timeline_events e WHERE e.feature_id = f.id AND e.extracted_from = 'catalog'))
    ) THEN
        RAISE EXCEPTION 'Catalog promotion needs manual review: a former resource bridge, Work IQ or Windows Autopatch feature has curated data.';
    END IF;
END;
$$;

DELETE FROM product_features f
USING products p
WHERE p.id = f.product_id AND (p.name, f.name) IN (
    ('Azure Arc', 'Azure Arc resource bridge'),
    ('Microsoft 365 Copilot', 'Work IQ'),
    ('Microsoft Intune', 'Windows Autopatch'));

-- Remove only the reviewed exclusions. Companies, models, conferences and research programs stay ignored.
DELETE FROM ignored_product_names
WHERE lower(name) IN (
    'agent package manager', 'azure arc resource bridge', 'work iq',
    'microsoft 365 work iq', 'microsoft 365 copilot work iq', 'windows autopatch',
    'chatgpt', 'claude', 'google gemini', 'salesforce data 360', 'snowflake', 'microsoft learn',
    'voice live api', 'microsoft intune endpoint privilege management', 'azure arc-enabled sql server');

SELECT relink_item_products();
SELECT relink_timeline_events();
