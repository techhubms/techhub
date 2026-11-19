-- Promote the existing Aspire alias without changing the product ID or its links.
DELETE FROM product_aliases a
USING products p
WHERE a.product_id = p.id AND lower(a.alias) = 'aspire' AND lower(p.name) = '.net aspire'
  AND NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = 'aspire');

UPDATE products p
SET name = 'Aspire', updated_at = NOW()
WHERE lower(p.name) = '.net aspire'
  AND NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = 'aspire')
  AND NOT EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = 'aspire');

INSERT INTO products (name, parent_product_id)
SELECT v.name, p.id
FROM (VALUES
    ('Azure AD Graph API', 'Microsoft Entra ID'),
    ('Model Router Auto Evaluation toolkit', 'Microsoft Foundry')
) AS v (name, parent)
JOIN products p ON lower(p.name) = lower(v.parent)
WHERE NOT EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(v.name))
  AND NOT EXISTS (SELECT 1 FROM ignored_product_names i WHERE lower(i.name) = lower(v.name))
ON CONFLICT (lower(name)) DO NOTHING;

INSERT INTO product_aliases (product_id, alias)
SELECT p.id, v.alias
FROM (VALUES
    ('Aspire', '.NET Aspire'),
    ('Azure AD Graph API', 'Azure AD Graph'),
    ('Model Router Auto Evaluation toolkit', 'Model Router Auto Evaluation')
) AS v (product, alias)
JOIN products p ON lower(p.name) = lower(v.product)
WHERE NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = lower(v.alias))
  AND NOT EXISTS (SELECT 1 FROM ignored_product_names i WHERE lower(i.name) = lower(v.alias))
ON CONFLICT (lower(alias)) DO NOTHING;

INSERT INTO product_features (product_id, name)
SELECT p.id, v.feature
FROM (VALUES
    ('Aspire', 'Aspire Dashboard'),
    ('Azure Functions', 'Durable Functions'),
    ('Microsoft Fabric', 'Database Agents'),
    ('Microsoft Fabric', 'Fabric HorizonDB'),
    ('Microsoft Foundry', 'Insights in Foundry')
) AS v (product, feature)
JOIN products p ON lower(p.name) = lower(v.product)
WHERE NOT EXISTS (
    SELECT 1 FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
    WHERE f.product_id = p.id AND lower(a.alias) = lower(v.feature)
)
ON CONFLICT (product_id, lower(name)) DO NOTHING;

-- Bare "Insights" and "Toolbox" would also match as product names outside Foundry.
INSERT INTO product_feature_aliases (feature_id, alias)
SELECT f.id, v.alias
FROM (VALUES
    ('GitHub Copilot', 'Token-based billing', 'Usage-based billing'),
    ('Microsoft Fabric', 'Fabric HorizonDB', 'Microsoft Fabric HorizonDB'),
    ('Microsoft Foundry', 'Insights in Foundry', 'Foundry Insights'),
    ('Microsoft Foundry', 'Foundry Toolbox', 'Foundry Toolboxes')
) AS v (product, feature, alias)
JOIN products p ON lower(p.name) = lower(v.product)
JOIN product_features f ON f.product_id = p.id AND lower(f.name) = lower(v.feature)
WHERE NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = lower(v.alias))
  AND NOT EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(v.alias))
  AND NOT EXISTS (SELECT 1 FROM ignored_product_names i WHERE lower(i.name) = lower(v.alias))
  AND NOT EXISTS (SELECT 1 FROM product_features x WHERE x.id <> f.id AND lower(x.name) = lower(v.alias))
  AND NOT EXISTS (SELECT 1 FROM product_feature_aliases a WHERE a.feature_id <> f.id AND lower(a.alias) = lower(v.alias))
ON CONFLICT (feature_id, lower(alias)) DO NOTHING;

SELECT relink_item_products();

WITH resolved AS (
    SELECT e.id, r.out_product_id, r.out_feature_id
    FROM content_timeline_events e
    CROSS JOIN LATERAL resolve_event_catalog(e.product, e.feature) r
    WHERE e.extracted_from <> 'catalog'
)
UPDATE content_timeline_events e
SET product_id = r.out_product_id, feature_id = r.out_feature_id
FROM resolved r
WHERE e.id = r.id
  AND (e.product_id, e.feature_id) IS DISTINCT FROM (r.out_product_id, r.out_feature_id);
