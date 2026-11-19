-- Unambiguous catalog decisions from the November review. Preserve existing curation and extracted text.
INSERT INTO products (name, parent_product_id)
SELECT v.name, p.id
FROM (VALUES
    ('.NET Framework', NULL),
    ('Anyscale on Azure', 'Microsoft Azure'),
    ('Application Gateway for Containers', 'Microsoft Azure'),
    ('Aspire CLI', '.NET Aspire'),
    ('Aspire extension for Visual Studio Code', '.NET Aspire'),
    ('Azure AI Content Understanding', 'Azure AI Services'),
    ('Azure Blueprints', 'Microsoft Azure'),
    ('Azure Compute Fleet', 'Microsoft Azure'),
    ('Azure Compute Gallery', 'Microsoft Azure'),
    ('Azure Database Migration Service', 'Microsoft Azure'),
    ('Azure DocumentDB', 'Microsoft Azure'),
    ('Azure File Sync', 'Microsoft Azure'),
    ('Azure Observability plugin', 'Azure Skills'),
    ('Azure Tools extension pack', 'Visual Studio Code'),
    ('BizTalk Server', NULL),
    ('C# Dev Kit', 'Visual Studio Code'),
    ('C#/WinRT', 'Microsoft Windows'),
    ('Connector Namespace', 'Microsoft Azure'),
    ('Data API builder', NULL),
    ('DirectX', 'Microsoft Windows'),
    ('Durable Task Scheduler', 'Microsoft Azure'),
    ('EF Core Power Tools', 'Entity Framework Core'),
    ('FinOps toolkit', 'Microsoft Azure'),
    ('GitHub Actions Runner Controller', 'GitHub Actions'),
    ('GitHub Spec Kit', 'GitHub'),
    ('LangGraph', NULL),
    ('MLflow', NULL),
    ('MSSQL extension for Visual Studio Code', 'Visual Studio Code'),
    ('MSTest', '.NET'),
    ('Microsoft Entra Private Access', 'Microsoft Entra ID'),
    ('Microsoft C++ Language Server', 'GitHub Copilot CLI'),
    ('Microsoft JDBC Driver for SQL Server', 'Microsoft SQL Server'),
    ('Microsoft Learn MCP Server', 'Microsoft Learn'),
    ('Microsoft Olive', NULL),
    ('Microsoft Planetary Computer Pro', 'Microsoft Azure'),
    ('Microsoft Quantum Development Kit', NULL),
    ('Microsoft Web IQ', NULL),
    ('Microsoft.Data.SqlClient', 'Microsoft SQL Server'),
    ('Microsoft.Extensions.AI', '.NET'),
    ('Microsoft.Testing.Platform', '.NET'),
    ('Nerdio Compass', NULL),
    ('OpenTofu', NULL),
    ('PSScriptAnalyzer', 'PowerShell'),
    ('Pylance', 'Visual Studio Code'),
    ('Python Environments', 'Visual Studio Code'),
    ('Rayfin', 'Microsoft Fabric'),
    ('SqlPackage', 'Microsoft SQL Server'),
    ('ThinkingBox', NULL),
    ('dotnetup', '.NET'),
    ('mssql-python', 'Microsoft SQL Server')
) AS v (name, parent)
LEFT JOIN products p ON lower(p.name) = lower(v.parent)
WHERE (v.parent IS NULL OR p.id IS NOT NULL)
  AND NOT EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(v.name))
  AND NOT EXISTS (SELECT 1 FROM ignored_product_names i WHERE lower(i.name) = lower(v.name))
ON CONFLICT (lower(name)) DO NOTHING;

-- Insert children whose parent is created above.
INSERT INTO products (name, parent_product_id)
SELECT v.name, p.id
FROM (VALUES
    ('Azure DocumentDB MCP Toolkit', 'Azure DocumentDB'),
    ('Azure Optimization Engine', 'FinOps toolkit'),
    ('DocumentDB for VS Code', 'Visual Studio Code')
) AS v (name, parent)
JOIN products p ON lower(p.name) = lower(v.parent)
WHERE NOT EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(v.name))
  AND NOT EXISTS (SELECT 1 FROM ignored_product_names i WHERE lower(i.name) = lower(v.name))
ON CONFLICT (lower(name)) DO NOTHING;

INSERT INTO product_aliases (product_id, alias)
SELECT p.id, v.alias
FROM (VALUES
    ('Apache Artemis', 'Apache ActiveMQ Artemis'),
    ('Aspire extension for Visual Studio Code', 'Aspire VS Code extension'),
    ('Azure AI Content Understanding', 'Azure Content Understanding'),
    ('Azure AI Speech', 'Azure Speech'),
    ('Azure Application Insights', 'Azure Monitor Application Insights'),
    ('Azure Key Vault Managed HSM', 'Azure Managed HSM'),
    ('Azure Observability plugin', 'Azure Observability Skills Plugin'),
    ('Connector Namespace', 'Azure Connector Namespace'),
    ('DocumentDB for VS Code', 'DocumentDB extension for Visual Studio Code'),
    ('Durable Task Scheduler', 'Azure Durable Task Scheduler'),
    ('GitHub Spec Kit', 'Spec Kit'),
    ('Microsoft Web IQ', 'Web IQ'),
    ('NuGet', 'NuGet.org'),
    ('SharePoint', 'SharePoint Online')
) AS v (product, alias)
JOIN products p ON lower(p.name) = lower(v.product)
WHERE NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = lower(v.alias))
  AND NOT EXISTS (SELECT 1 FROM ignored_product_names i WHERE lower(i.name) = lower(v.alias))
  AND NOT EXISTS (SELECT 1 FROM product_features f WHERE lower(f.name) = lower(v.alias) AND f.product_id <> p.id)
  AND NOT EXISTS (
      SELECT 1 FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
      WHERE lower(a.alias) = lower(v.alias) AND f.product_id <> p.id
  )
ON CONFLICT (lower(alias)) DO NOTHING;

INSERT INTO product_features (product_id, name)
SELECT p.id, v.feature
FROM (VALUES
    ('.NET', 'Native AOT'),
    ('.NET', 'ReadyToRun'),
    ('.NET', 'Trimming'),
    ('.NET Aspire', 'AppHost'),
    ('Azure API Management', 'AI Gateway'),
    ('Azure Arc', 'Arc-enabled servers'),
    ('Azure Kubernetes Service', 'Kubernetes Center'),
    ('Azure Monitor', 'Trace Analytics'),
    ('Azure Policy', 'Machine Configuration'),
    ('Azure Service Bus', 'Duplicate detection'),
    ('Azure Virtual Machines', 'Confidential virtual machines'),
    ('GitHub Copilot', 'Local sandboxing'),
    ('Microsoft Entra External ID', 'Native authentication'),
    ('Microsoft Fabric', 'Eventstream'),
    ('Microsoft Fabric', 'Semantic views'),
    ('Microsoft Fabric', 'Workspace monitoring'),
    ('Microsoft Foundry', 'Foundry IQ'),
    ('Microsoft Foundry', 'Foundry Toolbox'),
    ('Microsoft Foundry', 'Managed Compute')
) AS v (product, feature)
JOIN products p ON lower(p.name) = lower(v.product)
WHERE NOT EXISTS (
    SELECT 1 FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
    WHERE f.product_id = p.id AND lower(a.alias) = lower(v.feature)
)
ON CONFLICT (product_id, lower(name)) DO NOTHING;

INSERT INTO product_feature_aliases (feature_id, alias)
SELECT f.id, v.alias
FROM (VALUES
    ('.NET', 'Native AOT', 'NativeAOT'),
    ('Azure Arc', 'Arc-enabled servers', 'Azure Arc-enabled servers'),
    ('Azure Cosmos DB', 'Rust SDK', 'Azure Cosmos DB SDK for Rust'),
    ('Azure Policy', 'Machine Configuration', 'Azure Machine Configuration'),
    ('Azure Virtual Machines', 'Confidential virtual machines', 'Azure confidential VMs'),
    ('GitHub Copilot', 'Copilot Fusion', 'HydraFusion'),
    ('Microsoft Fabric', 'Eventstream', 'Eventstreams'),
    ('Microsoft Fabric', 'SQL database', 'SQL database in Microsoft Fabric')
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

-- Catalog-authored events keep their manually selected links.
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
