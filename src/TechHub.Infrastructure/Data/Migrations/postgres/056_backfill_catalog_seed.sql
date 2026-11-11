-- Reviewed against the 7 product names and 151 feature names in the 10 November 2026 backfill.
-- Insert catalog data only; preserve existing products, parents, features and descriptions.
-- Models, runtime versions, regions, whole-product feature texts and incorrectly attributed capabilities
-- are not made into features just to empty the review queue.

INSERT INTO products (name, parent_product_id)
SELECT v.name, p.id
FROM (VALUES
    ('Claude Code', NULL),
    ('PIX', NULL),
    ('dotcraft', NULL),
    ('Microsoft Foundry SDK', 'Microsoft Foundry'),
    ('PostgreSQL extension for Visual Studio Code', 'Visual Studio Code'),
    ('WSL extension for Visual Studio Code', 'Visual Studio Code'),
    ('Azure Advisor plugin', 'Azure Skills'),
    ('Azure Cost plugin', 'Azure Skills')
) AS v (name, parent)
LEFT JOIN products p ON p.name = v.parent
WHERE v.parent IS NULL OR p.id IS NOT NULL
ON CONFLICT (lower(name)) DO NOTHING;

INSERT INTO product_aliases (product_id, alias)
SELECT p.id, v.alias
FROM (VALUES
    ('Azure Private Link', 'Private Link'),
    ('Microsoft Foundry SDK', 'Foundry SDK'),
    ('PostgreSQL extension for Visual Studio Code', 'PostgreSQL extension'),
    ('WSL extension for Visual Studio Code', 'WSL extension'),
    ('Azure Cost plugin', 'Azure Cost'),
    ('Azure Cost plugin', 'azure-cost')
) AS v (product, alias)
JOIN products p ON p.name = v.product
WHERE NOT EXISTS (SELECT 1 FROM products x WHERE lower(x.name) = lower(v.alias))
ON CONFLICT (lower(alias)) DO NOTHING;

-- A conference and model identifiers are not products. Keep the extracted text, but exclude these names from review.
INSERT INTO ignored_product_names (name)
SELECT v.name
FROM (VALUES
    ('.NET Conf'),
    ('MAI-Transcribe-2-Streaming'),
    ('MAI-Voice-2.1'),
    ('MAI-Voice-2.1-Flash')
) AS v (name)
WHERE NOT EXISTS (SELECT 1 FROM products p WHERE lower(p.name) = lower(v.name))
  AND NOT EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(v.name))
ON CONFLICT (lower(name)) DO NOTHING;

INSERT INTO product_features (product_id, name)
SELECT p.id, v.feature
FROM (VALUES
    ('.NET MAUI', 'Blazor Hybrid'),
    ('Azure AI Speech', 'SIP'),
    ('Azure API Management', 'Managed identity authentication'),
    ('Azure API Management', 'Private networking'),
    ('Azure Arc', 'Arc gateway'),
    ('Azure Arc', 'Database migration'),
    ('Azure Backup', 'Vaulted backup'),
    ('Azure Databricks', 'External locations'),
    ('Azure Databricks', 'Managed storage'),
    ('Azure Databricks', 'Materialized views'),
    ('Azure Databricks', 'Streaming tables'),
    ('Azure Pipelines', 'Job containers'),
    ('Azure Route Server', 'Downstream policy enforcement'),
    ('Azure Route Server', 'Network isolation'),
    ('Azure Route Server', 'Path selection'),
    ('Azure Route Server', 'Route filtering'),
    ('Azure Route Server', 'Route maps'),
    ('Azure Route Server', 'Route summarization'),
    ('Azure Route Server', 'SD-WAN interoperability'),
    ('Azure SDK', 'Authentication'),
    ('Azure SRE Agent', 'Alerts'),
    ('Azure SRE Agent', 'Live Reports'),
    ('Azure Virtual Machines', 'Run Command'),
    ('CodeQL CLI', 'JVM diagnostics'),
    ('CodeQL CLI', 'Pack publishing checks'),
    ('CodeQL CLI', 'Python support'),
    ('ComfyUI', 'Custom nodes'),
    ('Dev Proxy', 'Dev Proxy API'),
    ('Dev Proxy', 'System proxy'),
    ('Docker Desktop', 'WSL 2 integration'),
    ('Foundry Local', 'Speculative decoding'),
    ('GitHub', 'Contributor exemptions'),
    ('GitHub', 'Garbage collection'),
    ('GitHub', 'Merge readiness summary'),
    ('GitHub', 'Native Citation support'),
    ('GitHub', 'Newest-first comment sorting'),
    ('GitHub', 'ORCID ID integration'),
    ('GitHub', 'Ref-CAS'),
    ('GitHub', 'Repository compaction'),
    ('GitHub', 'Repository pull request views'),
    ('GitHub', 'Virtual replicas'),
    ('GitHub Actions', 'Ephemeral runners'),
    ('GitHub CodeQL', 'Security queries'),
    ('GitHub Copilot', 'Conversation context'),
    ('GitHub Copilot', 'Enterprise managed settings'),
    ('GitHub Copilot', 'Global default policy'),
    ('GitHub Copilot', 'Plugin management'),
    ('GitHub Enterprise Cloud', 'Cost centers'),
    ('Microsoft Defender for Cloud', 'SQL Server agent'),
    ('Microsoft Execution Container', 'Agentic process flagging'),
    ('Microsoft Execution Container', 'Containment'),
    ('Microsoft Execution Container', 'Granular policies'),
    ('Microsoft Execution Container', 'Learn modes'),
    ('Microsoft Fabric', 'Automatic synchronization'),
    ('Microsoft Fabric', 'Copilot capacity billing'),
    ('Microsoft Fabric', 'Copilot in Microsoft Fabric'),
    ('Microsoft Fabric', 'Data connection policies'),
    ('Microsoft Fabric', 'Data engineering agent'),
    ('Microsoft Fabric', 'Database Hub alerts'),
    ('Microsoft Fabric', 'Delta shortcuts'),
    ('Microsoft Fabric', 'Fabric Policies'),
    ('Microsoft Fabric', 'Iceberg shortcuts'),
    ('Microsoft Fabric', 'Materialized views'),
    ('Microsoft Fabric', 'OneLake File APIs'),
    ('Microsoft Fabric', 'OneLake Table APIs'),
    ('Microsoft Fabric', 'OneLake shortcuts'),
    ('Microsoft Fabric', 'Query Insights'),
    ('Microsoft Fabric', 'Streaming tables'),
    ('Microsoft Foundry', 'Microsoft Foundry Skill'),
    ('Microsoft Foundry', 'Model deployment'),
    ('Microsoft Foundry', 'Sensitive-content protection for traces'),
    ('Microsoft Foundry Agent Service', 'Agent identity'),
    ('Microsoft Power BI', 'DirectQuery'),
    ('Oracle AI Database@Azure', 'EU Data Boundary support'),
    ('Pgpool-II', 'Connection pooling'),
    ('Pgpool-II', 'Health checks'),
    ('Pgpool-II', 'Load balancing'),
    ('Pgpool-II', 'Pgpool Control Protocol'),
    ('PIX', 'Crash postmortems'),
    ('Traefik', 'Hostname routing'),
    ('Traefik', 'Middleware'),
    ('Traefik', 'Path routing'),
    ('Traefik', 'TLS termination'),
    ('Visual Studio Code', 'Add Context'),
    ('Visual Studio Code', 'Agents Window'),
    ('Visual Studio Code', 'Canvases'),
    ('Visual Studio Code', 'Chat view modes'),
    ('Visual Studio Code', 'Local automations'),
    ('Visual Studio Code', 'Remote browser support')
) AS v (product, feature)
JOIN products p ON p.name = v.product
WHERE NOT EXISTS (
    SELECT 1 FROM product_feature_aliases a JOIN product_features f ON f.id = a.feature_id
    WHERE f.product_id = p.id AND lower(a.alias) = lower(v.feature))
ON CONFLICT (product_id, lower(name)) DO NOTHING;

-- Reuse the capability already in the catalog instead of creating one feature per extracted spelling.
-- Do not alias bare "Copilot" to Fabric: that would hijack the ambiguous global product name.
-- "Azure Advisor" remains the Azure service, not an alias of the separately installed Advisor plugin.
INSERT INTO product_feature_aliases (feature_id, alias)
SELECT f.id, v.alias
FROM (VALUES
    ('ASP.NET Core', 'Interactive Auto rendering', 'Interactive Auto render mode'),
    ('ASP.NET Core', 'Interactive Server rendering', 'Interactive Server render mode'),
    ('ASP.NET Core', 'Interactive WebAssembly rendering', 'Interactive WebAssembly render mode'),
    ('ASP.NET Core', 'Static server-side rendering', 'Static Server rendering'),
    ('Azure API Management', 'Managed identity authentication', 'Managed identity'),
    ('Azure Container Registry', 'ABAC repository permissions', 'Repository-level permissions'),
    ('Azure Front Door', 'Private Link origins', 'Private Link'),
    ('Azure Policy', 'IP address expressions', 'IP address and CIDR expressions'),
    ('Azure Policy', 'Policy effect overrides', 'Policy overrides'),
    ('Azure Policy', 'Identity-aware policy evaluation', 'Request context identity'),
    ('Azure SQL Database', 'Audit logging', 'Auditing'),
    ('Azure SQL Database', 'Long-term retention backups', 'Long-term retention'),
    ('Azure SQL Managed Instance', 'Audit logging', 'Auditing'),
    ('Azure SQL Managed Instance', 'Long-term retention backups', 'Long-term retention'),
    ('GitHub', '@GitHub in Slack and Teams', 'GitHub app for Microsoft Teams'),
    ('GitHub', '@GitHub in Slack and Teams', 'Slack and Teams integration'),
    ('GitHub Actions', 'Manual workflow runs', 'Manually triggered workflows'),
    ('GitHub Copilot', 'Assisted permissions', 'Assisted approvals'),
    ('GitHub Copilot', 'GitHub Canvas', 'Canvas'),
    ('GitHub Copilot', 'Hooks', 'Copilot CLI hooks'),
    ('GitHub Copilot', 'Plugin management', 'Repository plugin defaults and enterprise-managed plugin controls'),
    ('GitHub Copilot', 'Session continuity', 'Session persistence'),
    ('GitHub Copilot app', 'Copilot app Security canvas', 'Security canvas'),
    ('GitHub Dependabot', 'Version updates', 'Dependency updates'),
    ('Microsoft Defender', 'Local AI agent discovery', 'Local agent discovery'),
    ('Microsoft Defender Experts', 'Defender Experts investigations', 'Microsoft Defender Experts investigations'),
    ('Microsoft Execution Container', 'Containment', 'AI agent containment'),
    ('Microsoft Fabric', 'Data agent', 'Data agents'),
    ('Microsoft Fabric', 'Data engineering agent', 'Project Osmos'),
    ('Microsoft Fabric', 'Database Hub alerts', 'Database Hub alerts for Azure SQL Database'),
    ('Microsoft Fabric', 'OneLake external-location support', 'External locations'),
    ('Microsoft Fabric', 'OneLake hosted MCP server', 'Hosted MCP server'),
    ('Microsoft Fabric', 'Iceberg writes', 'Iceberg REST Catalog writes'),
    ('Microsoft Fabric', 'Internal shortcuts', 'Internal OneLake shortcuts'),
    ('Microsoft Fabric', 'Salesforce Data 360 integration', 'Salesforce Data 360 shortcuts'),
    ('Microsoft Fabric', 'OneLake shortcuts', 'Shortcuts'),
    ('Microsoft Foundry', 'Microsoft Entra ID authentication', 'Keyless authentication'),
    ('Microsoft Foundry', 'Model deployment', 'Model deployments'),
    ('Microsoft Foundry', 'Native subagents', 'Subagents'),
    ('Microsoft Windows', 'Hotpatching', 'Hotpatch updates'),
    ('Microsoft Windows Server', 'Hotpatching', 'Hotpatch updates'),
    ('Visual Studio Code', 'MCP server configuration', 'MCP servers')
) AS v (product, feature, alias)
JOIN products p ON p.name = v.product
JOIN product_features f ON f.product_id = p.id AND f.name = v.feature
WHERE NOT EXISTS (SELECT 1 FROM product_features x WHERE x.product_id = p.id AND lower(x.name) = lower(v.alias))
  AND NOT EXISTS (
      SELECT 1 FROM product_feature_aliases a JOIN product_features af ON af.id = a.feature_id
      WHERE af.product_id = p.id AND lower(a.alias) = lower(v.alias))
ON CONFLICT DO NOTHING;

-- Refresh derived links when the application runs this migration on startup; extracted source text is unchanged.
SELECT relink_item_products();
SELECT relink_timeline_events();
