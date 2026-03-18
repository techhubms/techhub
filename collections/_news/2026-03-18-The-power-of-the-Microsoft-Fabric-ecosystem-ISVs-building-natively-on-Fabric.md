---
title: 'The power of the Microsoft Fabric ecosystem: ISVs building natively on Fabric'
primary_section: ai
tags:
- AI
- Apache Iceberg
- Azure Data Factory Migration
- Azure Synapse Analytics Migration
- Data Anonymization
- Data Quality Testing
- Data Replication
- Delta Lake
- Extensibility Toolkit
- Fabric Workloads
- Iceberg REST
- ISV Ecosystem
- Lakehouse
- Master Data Management
- Microsoft Fabric
- ML
- News
- OneLake
- Semantic Models
- SQL Translation
- Synthetic Data
- Table APIs
- Unity Catalog
- Workload Extensibility
date: 2026-03-18 05:48:42 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/the-power-of-the-microsoft-fabric-ecosystem-isvs-building-natively-on-fabric/
section_names:
- ai
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog rounds up new and updated ISV workloads and integrations that run natively in Microsoft Fabric and operate on OneLake data, covering GA and preview offerings (analytics/reporting, data quality, synthetic data, migrations) plus OneLake table-format and catalog integrations.<!--excerpt_end-->

# The power of the Microsoft Fabric ecosystem: ISVs building natively on Fabric

If you haven’t already, see Arun Ulag’s overview post: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

Microsoft positions **Fabric** as a connected platform where partner (ISV/SI) solutions run *inside* Fabric, operate on data in **OneLake**, and integrate with first-party services to reduce data movement, close integration gaps, and keep governance consistent.

Key platform building blocks called out:

- **Workload extensibility model**: [Fabric Extensibility Toolkit overview](https://learn.microsoft.com/fabric/extensibility-toolkit/extensibility-toolkit-overview)
- **Open standards / table APIs**: [OneLake table APIs overview](https://learn.microsoft.com/fabric/onelake/table-apis/table-apis-overview)
- **Shared data foundation**: [OneLake overview](https://learn.microsoft.com/fabric/onelake/onelake-overview)

## Example: Auger (supply chain intelligence) on Fabric

Auger is described as writing data natively to **Microsoft OneLake**, enabling customers to apply Fabric analytics, AI, and governance directly to supply chain decision-making (inventory optimization, supplier risk mitigation, scenario planning) without duplicating data or maintaining parallel systems.

- Deep dive: [Learn more about Auger](https://aka.ms/AugerBlog)

![List of Microsoft Fabric ISVs and their services](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-list-of-various-microsoft-fab.png)

*Figure: Microsoft Fabric ISV Ecosystem*

## Generally available Microsoft Fabric workloads

### Fusion Data Hub — Industrial Analytics (GA)

Marketplace: [Fusion Data Hub](https://azuremarketplace.microsoft.com/marketplace/apps/fusiondatahub1713978884000.history_transfer_basic?tab=Overview)

What it does:

- Unifies operational data from **historians, DCS, SCADA, and IoT sensors** into OneLake.
- Lets customers analyze operational/time-series data alongside enterprise data in Fabric.
- Supports applying analytics and AI to industrial operations **without moving data**.
- Targets scaling governed analytics for oil & gas and heavy industry.

Demo: [Fusion in Fabric video](https://vimeo.com/1116785072)

![Fusion Data Hub dashboard on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-dashboard-from-fusion-data-hu.png)

*Figure: Fusion Data Hub on Microsoft Fabric*

### Zebra AI — Advanced reporting for Fabric models (GA)

Marketplace: [Zebra AI](https://azuremarketplace.microsoft.com/marketplace/apps/zebrabi1634048186304.zebra_ai)

Highlights:

- Reporting integrated with **Fabric semantic models**.
- Builds financial/operational reports on OneLake data.
- Promotes consistent metrics across BI and analytics workflows.
- Adds visualization patterns on top of Fabric models.

Demo/Docs:

- Video: [Zebra AI running in Fabric](https://www.youtube.com/watch?v=XpME96phUxs)
- Quick start: [Setting up Zebra AI workload for Fabric](https://zebra-ai.com/knowledge-base/setting-up-zebra-ai-workload-for-fabric/)

![Financial comparison report created with Zebra AI on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-depicts-a-financial-comparison-of-cost-r.png)

*Figure: Zebra AI on Microsoft Fabric*

> “Zebra AI Workload for Fabric has been a true game changer for us. Critical, recurring analytical processes that once consumed significant time and resources across multiple platforms are now completed in a fraction of the time. The gains in efficiency, speed, and insight have been remarkable.”
> > >
> – **Pablo Trachsler, Head of Group Re Business Development & Innovation, Zurich Insurance**

### Tonic.ai — Redacts and synthesizes unstructured data (GA)

Marketplace: [Tonic.ai Textual](https://marketplace.microsoft.com/product/tonicaiinc1755101571967.tonicai-synthetic-data-solutions?tab=Overview)

Purpose:

- Anonymization and synthesis for **textual** (unstructured) data.
- Targets privacy/security/compliance while enabling analytics and AI.

Capabilities:

- Generate high-fidelity **synthetic data** directly from OneLake.
- Enable broader data access for analytics, testing, and AI development.
- Meet regulatory/privacy requirements without blocking usage.

Video: [How to use Tonic Textual in Fabric](https://www.youtube.com/watch?v=bX5ZrSwMiPs)

![Tonic.ai interface on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-the-interface-from-tonic-ai-wit.png)

*Figure: Tonic.ai on Microsoft Fabric*

### Navida BC2Fab — Replication from Business Central to Fabric (GA)

Marketplace: [BC2Fab](https://marketplace.microsoft.com/de-de/product/web-apps/navidainformationssysteme1611067488644.bc_2_fabric_saas)

What it does:

- Mirrors **Microsoft Business Central** data to Fabric in near real time.
- Uses a pull-based mechanism on a read-only replica to avoid extra load on production.

Noted features:

- Near-real-time mirroring with minimal configuration.
- Handles large tables (millions of rows).
- Automatic translation of fields and option values into supported languages.
- Can auto-generate **semantic models**, including support for custom objects.

Resources:

- Walkthrough video: [BC2Fab walkthrough](https://vimeo.com/1164034141?fl=pl&fe=sh)
- Quick start: [BC2Fab quickstart](https://docs.bc2fab.com/getting-started/quickstart.html)

![Sales dashboard created with Navida BC2Fab](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-sales-dashboard-with-various.png)

*Figure: Navida BC2Fab on Microsoft Fabric*

### 2Bit 2TEST — Data quality and testing for Fabric (GA)

Marketplace: [2TEST](https://marketplace.microsoft.com/product/2test.2test_fabric?tab=Overview)

Focus:

- Testing workload for validating reliability/quality of data and **Power BI semantic models**.
- Uses AI to analyze data/models and auto-generate tests.

Customer outcomes:

- Integrate data testing into Fabric workflows.
- Catch issues earlier across ingestion and transformation.
- Increase trust in analytics/AI outputs by validating semantic models.

Videos:

- [Testing data in your Lakehouse](https://www.youtube.com/watch?v=g3tIiDcpKOs)
- [Testing Power BI semantic models](https://www.youtube.com/watch?v=EctjHoSr3_w)

![2TEST workload visual on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/this-image-shows-a-visual-of-2test-on-microsoft-fa.png)

*Figure: 2TEST on Microsoft Fabric*

### Kanerika — Migration + Karl AI agent (GA)

Marketplace: [Kanerika workloads](https://marketplace.microsoft.com/product/saas/kanerikainc1719422858450.azure-to-fabric-migration-offer?tab=Overview)

Two workloads:

- **Workload Migration**: migrate **Azure Data Factory** and **Azure Synapse Analytics** pipelines to Fabric in real time.
  - Demo: [Azure to Fabric migration demo](https://youtu.be/wtcYLYZkG7k)
- **Karl AI Agent**: natural-language data analysis that translates questions to SQL, runs queries, generates visuals, and explains results.
  - Demos: [Karl for data insights](https://youtu.be/0vSj3-FzMkQ), [Karl the statistician](https://youtu.be/L5pIfqEDEsQ)

## Fabric workloads in preview

### Stibo Systems — MDM meets OneLake (Preview)

Stibo Systems’ MDM DaaS workload aims to expose governed master data as a cloud-native data service in OneLake, supporting analytics/AI-ready access and automation.

- Blog: [Stibo’s MDM DaaS on Fabric](https://www.stibosystems.com/blog/master-data-meets-microsoft-fabric)
- Demo: [Stibo running in Fabric](https://youtu.be/v1mdsMK94E0)

![Stibo Systems MDM UI on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-user-interface-for-stibo-syst.png)

*Figure: Stibo Systems on Microsoft Fabric*

### Intuigence.AI — Synthetic AI engineers for industrial ops (Preview)

Intuigence provides AI-based “engineers” (chemical, mechanical, plant support) intended to automate industrial workflow tasks with 24/7 operation, multilingual capabilities, and a UX layer.

- Demo: [Intuigence workload demo](https://www.youtube.com/watch?v=KurLBxz5Y-g)

![Intuigence.AI interface on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-shows-a-technical-interface-from-intuige.png)

*Figure: Intuigence.AI on Microsoft Fabric*

### Financial Fabric — Capital Markets Data Hub (Preview)

Provides a governed, analytics-ready foundation for institutional investors, including access to many providers/datasets and consumption via web apps, Excel, Power BI, APIs, and AI experiences.

- Video: [Capital Markets Data Hub for Fabric](https://youtu.be/a_HAxV1Kt9s)

![Financial Fabric dashboard on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-dashboard-of-financial-fabric.png)

*Figure: Financial Fabric on Microsoft Fabric*

### Spectral Core — Data warehouse migration to Fabric (Preview)

Aims to speed up data migration and SQL translation into Fabric-compatible syntax:

- **Omni Loader**: automated loading, schema mapping, bulk transfers
- **SQL Tran**: SQL translation for existing SQL code, stored procedures, functions

- Video: [Spectral Core in action](https://vimeo.com/1037513551?fl=pl&fe=sh)

![Spectral Core interface on Microsoft Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/image-shows-an-interface-of-spectral-core-on-ms-fa.png)

*Figure: Spectral Core on Microsoft Fabric*

## Microsoft OneLake integrations

OneLake is presented as a centralized data repository. Tables are stored in **Apache Iceberg** or **Delta Lake** open table formats. OneLake can automatically convert between formats via table virtualization, and supports widely adopted table APIs:

- **Apache Iceberg REST** compatibility: [Iceberg table APIs overview](https://learn.microsoft.com/fabric/onelake/table-apis/iceberg-table-apis-overview)
- **Databricks Unity Catalog-compatible APIs**: [Delta table APIs overview](https://learn.microsoft.com/fabric/onelake/table-apis/delta-table-apis-overview)

Announced integrations:

- **ClickHouse**: ClickHouse Cloud supports OneLake Iceberg-based Table APIs (Fast release channel first, Regular channel later).
  - Docs: [Configure OneLake catalog in ClickHouse](https://clickhouse.com/docs/use-cases/data-lake/onelake-catalog)
- **Dremio**: OneLake support reaches general availability in Dremio Enterprise and Cloud.
  - Docs: [Configure OneLake in Dremio](https://docs.dremio.com/current/data-sources/lakehouse-catalogs/onelake/)
  - Note: Mirrored Dremio Catalog in OneLake is “launching soon”.
- **Ryft**: preview lakehouse observability for OneLake Iceberg tables (sizes, ingestion, query behavior, schema changes).
  - Docs: [Connect Ryft to OneLake](https://docs.ryft.io/integrations/onelake)
- **Onehouse**: public preview of OneSync integration with **OneLake Security** (permission translation from AWS Glue/Lake Formation and Databricks Unity Catalog).
  - Azure note: Onehouse is described as [running natively in Azure](https://www.onehouse.ai/blog/bringing-onehouse-cloud-to-microsoft-azure)
  - Demo: [OneSync + OneLake Security demo](https://www.youtube.com/watch?v=ys81-Y9-7OE)
  - Setup: [Configure OneSync with OneLake Security](https://www.onehouse.ai/blog/announcing-onesync-permissions)

## What Microsoft is emphasizing

Across these announcements, Microsoft highlights partners:

- Building **native Fabric workloads** for analytics and AI projects
- Using **open standards** for interoperability and choice
- Shipping industry/domain-focused solutions intended to be production-ready and integrated by design


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/the-power-of-the-microsoft-fabric-ecosystem-isvs-building-natively-on-fabric/)

