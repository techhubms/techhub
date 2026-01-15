---
layout: post
title: Comprehensive Methods for Integrating Azure Databricks with Microsoft Fabric
author: Rafia_Aqil
canonical_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/approaches-to-integrating-azure-databricks-with-microsoft-fabric/ba-p/4453643
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-12 21:43:27 +00:00
permalink: /ml/community/Comprehensive-Methods-for-Integrating-Azure-Databricks-with-Microsoft-Fabric
tags:
- Azure
- Azure Databricks
- Community
- Data Engineering
- Data Governance
- Data Integration
- Data Lake
- Data Orchestration
- Data Pipelines
- Data Warehouse
- Delta Sharing
- ETL
- Lakehouse
- Microsoft Fabric
- ML
- OneLake
- Power BI
- Trusted Workspace Access
- Unity Catalog
section_names:
- azure
- ml
---
Rafia_Aqil provides an in-depth guide comparing approaches to integrate Azure Databricks with Microsoft Fabric, helping technical teams select the best method for unified data analytics and governance.<!--excerpt_end-->

# Approaches to Integrating Azure Databricks with Microsoft Fabric: The Better Together Story!

**Author:** Rafia_Aqil  
**Peer Reviewers:** ArvindPeriyasamy, Hamood_Aleem, jbarry15

This guide explores multiple approaches to integrate Azure Databricks with Microsoft Fabric, providing detailed steps, considerations, and decision points for each method. The aim is to help architects and engineers select the most suitable technique for unified analytics, governance, and data pipeline automation across the Microsoft cloud ecosystem.

## Direct Publish from DBSQL to Fabric

- **Overview:** Enables users to connect Databricks SQL Warehouses to Power BI using the native connector, making it possible to build live reports and dashboards on Databricks data from within Fabric.
- **Key Steps:**
  - Obtain SQL Warehouse connection details from Databricks (hostname, HTTP Path, JDBC URL, OAuth URL).
  - Create pipelines or dataflows in Microsoft Fabric to source from Databricks and publish to Power BI (Lakehouse or Fabric SQL Database as destination).
  - Optionally, use the Catalog UI in Databricks for one-click publish to Power BI workspace in Fabric.
- **Considerations:** Choose between Import and DirectQuery modes in Power BI based on performance and dataset size.

## Mirroring Azure Databricks Unity Catalog

- **Overview:** Automates the creation of mirrored catalogs in Fabric from Databricks Unity Catalog (or selected schemas), exposing tables in real-time via OneLake shortcuts with zero data duplication.
- **Key Steps:**
  - Enable Unity Catalog and 'External Data Access' in Databricks.
  - Assign appropriate permissions to the mirroring service principal.
  - In Fabric, create a new Mirrored Azure Databricks Catalog and select schemas/tables to sync.
  - Fabric creates OneLake shortcuts for each Databricks table and auto-generates Power BI datasets.
- **Considerations:** Data remains read-only in Fabric; row-level security does not transfer. Not currently supported when Databricks is behind a private endpoint.

## Delta Sharing for Cross-Platform Data Exchange

- **Overview:** Leverages the open Delta Sharing protocol to securely exchange Delta Lake data across platforms, even in scenarios lacking direct Fabric-to-Databricks connectivity.
- **Key Steps:**
  - Set up a share in Databricks Unity Catalog, adding tables and recipients (with Entra ID or token-based authentication).
  - In Fabric, configure Dataflow Gen2 or Data Factory pipeline to consume the share via provided endpoint/token.
  - Load shared tables into OneLake, Lakehouse, or other Fabric destinations.
- **Considerations:** This method materializes data in Fabric (ETL); schema changes require manual syncs. Suited for partner or multi-tenant collaborations.

## Azure Databricks Activity in Fabric Pipelines

- **Overview:** Allows Data Factory in Fabric to orchestrate Databricks jobs, notebooks, and scripts, providing automated, hybrid ETL workflows.
- **Key Steps:**
  - In Fabric Data Factory, add an Azure Databricks activity (configure linked service, authentication).
  - Define and parameterize the Databricks task (notebook, Python script, JAR file, or job).
  - Chain Databricks activities with other ETL steps, including error handling and monitoring.
- **Considerations:** Great for batch-oriented, complex pipelines. Incorporate cluster auto-termination to control costs.

## Automatic Publishing to Power BI from Databricks

- **Overview:** Use Databricks Workflow Jobs to trigger Power BI dataset creation or refresh in Fabric on job completion, enabling near real-time reporting.
- **Key Steps:**
  - Add a Power BI task to your Databricks job after processing.
  - Map tables/views from Unity Catalog, set up Power BI connection and workspace.
  - Define dataset mode (Import or DirectQuery) and run the job to publish or update in Power BI.
- **Considerations:** Provides Databricks-driven BI refreshes; best for active pipelines requiring up-to-date reporting.

## Integrate Databricks External Tables with OneLake

- **Overview:** Create OneLake shortcuts in Fabric Lakehouse using Databricks API and Unity Catalog external tables.
- **Key Steps:**
  - Generate Databricks personal access token and collect workspace URL.
  - Register workspace and lakehouse info in Fabric.
  - Run provided Python notebook to sync external tables as OneLake shortcuts.
- **Considerations:** Recommended to use Databricks OAuth and store secrets in Azure Key Vault. Supports governance-first, large dataset access scenarios.

## Directly Write into OneLake from Databricks Notebook

- **Overview:** Allows Databricks notebooks to write directly to Fabric Lakehouse via ABFS paths, supporting custom integrations, ETL, and cross-cloud scenarios.
- **Key Steps:**
  - Retrieve the Fabric Lakehouse ABFS path.
  - Configure appropriate credentials in Databricks (using secret scopes or Azure Key Vault).
  - Use Spark APIs to write data directly to OneLake.
- **Considerations:** Enables custom pipeline integration; schema management requires diligence.

## OneLake Shortcuts with Trusted Workspace Access

- **Overview:** Facilitates secure shortcuts from Databricks data in ADLS Gen2 to Fabric via workspace identity and resource access rules, bypassing Unity Catalog’s governance in scenarios with private endpoints.
- **Key Steps:**
  - Prepare ADLS Gen2 storage, set up Fabric workspace identity, and assign limited permissions.
  - Disable public access, create a resource instance rule for the Fabric workspace.
  - In Fabric, connect using 'Workspace identity' authentication and establish the shortcut.
- **Considerations:** Maintains high performance with no data duplication; ensure adherence to governance models and compatibility by keeping Delta as storage format.

## Comparison Table

| Approach                                 | Pros                                   | Cons                                  | Use Cases                           |
|------------------------------------------|----------------------------------------|---------------------------------------|--------------------------------------|
| Direct Publish from DBSQL to Power BI    | Simple, quick to set up                | Not for complex ETL or large datasets | Ad-hoc dashboards, quick reports     |
| Azure Databricks Mirroring in Fabric     | No data duplication, real-time access  | Read-only, feature limitations        | Enterprise data governance           |
| Delta Sharing                            | Secure cross-org/tenant sharing        | Manual sync, storage duplication      | Partner/vendor collaboration         |
| Databricks Activity in Fabric Pipelines  | Centralized, native orchestration      | More setup, batch-oriented            | Automated hybrid ETL                 |
| Power BI Tasks in Databricks             | Databricks-driven refreshes            | Requires orchestration logic          | BI tightly coupled with pipelines    |
| External Tables via OneLake Shortcuts    | No duplication, governance-friendly    | Unity Catalog dependency              | Large, governed datasets             |
| Write Directly into OneLake              | Full control and customization         | Needs code, risk of schema drift      | Custom ETL pipelines                 |
| Trusted Workspace Access Shortcuts       | Secure, no duplication, private access | Strong config and security required   | Private/secure environments          |

## Conclusion

Selecting an integration method depends on project priorities—whether favoring zero-copy analytics, governance, cross-tenant sharing, custom ETL, or hybrid orchestration. Many teams will combine techniques for maximum flexibility. Explore the official documentation for detailed, up-to-date guidance on each approach.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/approaches-to-integrating-azure-databricks-with-microsoft-fabric/ba-p/4453643)
