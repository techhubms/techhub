---
title: 'FabCon and SQLCon 2026: What’s new in Microsoft OneLake'
primary_section: ai
tags:
- AI
- Azure
- Azure Databricks
- Azure Monitor Mirroring
- Change Data Feed
- Column Level Security
- Customer Managed Keys
- Dataverse
- Delta Lake
- Dremio
- Eventhouse
- Fabric MCP Server
- Iceberg
- IP Firewall Rules
- KQL
- Lakehouse
- Microsoft Entra ID
- Microsoft Fabric
- Microsoft Foundry
- ML
- News
- OneLake
- OneLake Catalog
- OneLake Mirroring
- OneLake Shortcuts
- Oracle Mirroring
- Outbound Access Protection
- Power BI Semantic Model
- Private Link
- Row Level Security
- SAP Datasphere
- Security
- SharePoint Lists Mirroring
- Snowflake Interoperability
- Unity Catalog
- Zero Copy
- Zero ETL
date: 2026-03-18 05:47:58 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabcon-and-sqlcon-2026-whats-new-in-microsoft-onelake/
section_names:
- ai
- azure
- ml
- security
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog outlines OneLake updates announced at FabCon and SQLCon 2026, covering new mirroring/shortcut sources, shortcut transformations (including Delta Lake conversions), deeper Microsoft Foundry integration, and expanded governance controls like OneLake security, network access rules, and catalog APIs.<!--excerpt_end-->

# FabCon and SQLCon 2026: What’s new in Microsoft OneLake

*For a broader view across Fabric and Microsoft database announcements, see Arun Ulag’s post: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).* 

Microsoft is positioning **OneLake** as an “AI-ready” data lake for unifying access to data across clouds and platforms, aiming to reduce duplicated copies and brittle ETL pipelines.

## Connect to and transform your entire data estate

### Shortcuts and mirroring (zero-copy / zero-ETL)

OneLake shortcuts and mirroring are presented as a way to connect to data across:

- Azure, AWS, Google Cloud, Oracle
- On-premises sources
- Platforms like SAP, Dataverse, Snowflake, Azure Databricks

Updates announced:

- Mirroring expanded to include:
  - SharePoint lists (Preview)
  - Mirroring via shortcuts for Azure Monitor (Preview)
  - Dremio (Preview)
- Mirroring now **Generally Available** for:
  - Oracle
  - SAP Datasphere
- “Extended capabilities in mirroring” (paid option) introduced for operationalizing mirrored sources at scale:
  - **Change Data Feed (CDF)**
  - Ability to create **views** on top of mirrored data (starting with Snowflake)

### Shortcut transformations (Generally Available)

[Shortcut transformations](https://learn.microsoft.com/fabric/onelake/shortcuts-file-transformations/transformations) are now GA. They allow transformation when bringing data into OneLake or moving it between OneLake items, including:

- Converting formats to **Delta Lake**
- “AI-powered” transformations such as:
  - Summarization
  - Translation
  - Document classification

New preview transformation:

- **Excel to Delta table** transformation (Preview) to load Excel data directly into Fabric Lakehouse Delta tables.

### Delegated shortcuts (coming soon)

Delegated shortcuts will let you create a shortcut to OneLake data using a **delegated Microsoft Entra identity**, including data in another tenant.

## Building interoperability between OneLake and other industry platforms

- Azure Databricks can natively [read OneLake data through Unity Catalog](https://aka.ms/Databricks-Read-FabCon-2026), moving from public beta to public preview. The post states this is supported by production workloads.
- Microsoft and Databricks are also working on enabling Databricks to write to and store data directly in OneLake (two-way interoperability).
- OneLake interoperability with Snowflake is highlighted as GA: [Microsoft OneLake and Snowflake interoperability is now Generally Available](https://blog.fabric.microsoft.com/blog/microsoft-onelake-and-snowflake-interoperability-is-now-generally-available?ft=All)
  - Bidirectional read of **Iceberg** data managed by Snowflake or Fabric
  - Ability to store Snowflake-managed Iceberg tables in OneLake
- Example customer architecture: Auger’s supply chain platform built on Fabric with data stored natively in OneLake, surfaced through shortcuts in customer Fabric environments.

## Accessing your OneLake data from Windows and Microsoft Foundry

### Microsoft Foundry integration

OneLake is integrating with [Microsoft Foundry](https://azure.microsoft.com/products/ai-foundry/?msockid=053354d6877e66602b7b41a286ec67a0):

- Access the OneLake catalog in Foundry
- Discover data via metadata, endorsements, sensitivity labels, descriptions
- One-click connect OneLake items to add data to “Knowledge”

### OneLake file explorer for Windows

A OneLake file explorer for Windows is expected to be **Generally Available in the coming weeks**, enabling browsing workspaces and data assets and uploading/downloading/editing with an experience similar to OneDrive.

- Download: [OneLake file explorer for Windows](https://www.microsoft.com/download/details.aspx?id=105222&msockid=053354d6877e66602b7b41a286ec67a0)

## Share data securely with OneLake security (Generally Available)

[OneLake security](https://learn.microsoft.com/fabric/onelake/security/get-started-security#onelake-security-preview) is expected to be **Generally Available in the coming weeks**, with a unified permission model intended to follow the data.

Capabilities mentioned:

- Define roles
- Enforce **row-level** and **column-level** controls
- Permissions enforced across analytics experiences, including:
  - Spark notebook queries
  - Power BI report viewing
  - Fabric data agent exploration

Image from the post:

- ![GIF of OneLake security and the OneLake catalog Secure tab experience](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/gif-of-onelake-security-and-the-onelake-catalog-se.gif)

Additional security-related updates:

- Eventhouse supports OneLake security for data queried through **KQL**
- New APIs for third-party query engines to integrate with OneLake security
- Improved role management UX, including defining row-level and column-level security for new roles

## Additional network security in OneLake

New network security controls:

- **Resource Instance Rules** (Preview soon): control inbound access using trusted **Azure resource identities** (approve specific Azure resource instances rather than relying only on public IP allowlists).
  - Works alongside **Private Link** and IP filtering.
- **Workspace-level IP firewall rules** (Generally Available): enforce access controls based on request IP addresses.
- **Outbound access protection (OAP)** extended to more Fabric items, including OneLake shortcuts and mirrored databases (Generally Available):
  - See: [Outbound access protection overview](https://learn.microsoft.com/fabric/security/workspace-outbound-access-protection-overview)
- **Customer managed keys (CMK)** support expanded for workspaces in a capacity already protected by Power BI **bring your own key (BYOK)**:
  - [Customer-managed keys](https://learn.microsoft.com/fabric/security/workspace-customer-managed-keys)

## Take control of your data estate in the OneLake catalog

The [OneLake catalog](https://learn.microsoft.com/fabric/governance/onelake-catalog-overview) is positioned as the governance/control plane for Fabric data.

New/updated capabilities:

- Admin-focused insights in the Govern tab (GA): [Insights for Fabric admins](https://learn.microsoft.com/fabric/governance/onelake-catalog-govern#insights-for-fabric-admins)
  - Visibility into domains, capacity utilization, workspace activity, protection coverage, curation
  - Drill into Power BI reports and recommendations
  - Access the underlying semantic model for custom Power BI reporting
- Workspace tags to improve discoverability for users and AI agents
- Copilot can generate descriptions for semantic models to improve reuse
- Public APIs for OneLake catalog search and discovery:
  - Metadata access
  - Relevancy-ranked search
  - Integration with the **Fabric MCP server** for AI-driven content discovery
- Storage size breakdown per item, including soft-deleted sizes, via workspace settings

Image from the post:

- ![GIF showing the new insights for Admins in the OneLake catalog Govern tab](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/a-gif-showing-the-new-insights-for-admins-in-the-o.gif)

## Explore more Microsoft Fabric innovation

Additional links called out in the post:

- [Fabric March 2026 Feature summary blog](https://aka.ms/March-2026-Feature-Summary-Blog)
- [Power BI March 2026 feature summary blog](https://aka.ms/March-2026-PowerBI-Summary-Blog)
- [Fabric Updates channel](https://blog.fabric.microsoft.com/blog/)
- [Database announcement blog](https://aka.ms/FabCon-SQLCon-2026-Shireesh)
- [Fabric Platform announcement blog](https://aka.ms/FabCon-SQLCon-2026-Kim)
- [Fabric Data Factory announcement blog](https://aka.ms/FabCon-SQLCon-2026-Faisal)
- [Fabric Analytics announcement blog](https://aka.ms/FabCon-SQLCon-2026-Bogdan)
- [Real-Time Intelligence announcement blog](https://aka.ms/FabCon-SQLCon-2026-Yitzhak)
- [Fabric IQ announcement blog](https://aka.ms/FabCon-SQLCon-2026-Yitzhak-FabricIQ)
- [Power BI announcement blog](https://aka.ms/FabCon-SQLCon-2026-Mo)
- [Planning in Fabric IQ blog](https://aka.ms/FabCon-SQLCon-2026-Planning)
- [Fabric AI announcement blog](https://aka.ms/FabCon-SQLCon-2026-Nellie)
- [Fabric ISV announcement blog](https://aka.ms/FabCon-SQLCon-2026-Dipti)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/fabcon-and-sqlcon-2026-whats-new-in-microsoft-onelake/)

