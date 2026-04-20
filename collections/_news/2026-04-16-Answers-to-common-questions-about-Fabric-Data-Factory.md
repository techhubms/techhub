---
primary_section: ai
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/answers-to-common-questions-about-fabric-data-factory/
feed_name: Microsoft Fabric Blog
title: Answers to common questions about Fabric Data Factory
tags:
- AI
- Airflow DAGs
- Apache Airflow Jobs
- Azure
- Azure Data Factory
- Business Data Cloud
- Change Data Capture
- CI/CD
- Connector Library
- Copilot
- Copy Jobs
- Data Factory MCP Server
- Data Integration
- Dataflows Gen2
- Dbt Jobs
- DevOps
- Fabric Data Factory
- Git Backed Version Control
- MCP
- MCP Server
- Microsoft Fabric
- Migration
- Mirroring
- ML
- Modern Query Evaluator
- News
- OneLake
- OneLake Shortcuts
- Power Query
- SAP S/4HANA
- SCD Type 2
date: 2026-04-16 09:00:00 +00:00
section_names:
- ai
- azure
- devops
- ml
---

Microsoft Fabric Blog explains what customers are asking about Fabric Data Factory vs Azure Data Factory (ADF): when to migrate, how mature it is, and what new capabilities you gain—like Mirroring to OneLake, Copy Jobs with CDC, Dataflows Gen2, Copilot assistance, and managed Airflow/dbt execution.<!--excerpt_end-->

## Answers to common questions about Fabric Data Factory

As the Data Integration Customer Advisory Team (CAT) lead, the author spent time talking with customers at [FabCon/SQLCon](https://fabriccon.com/) about Fabric Data Factory and collected recurring questions about the future of data integration.

## What customers asked

- I’m already on ADF. Should I move to Fabric Data Factory?
- Fabric Data Factory is newer. How do I know it’s mature enough for production?
- What does Fabric Data Factory give me that ADF doesn’t?
- How do I migrate? Where do I start?

## I’m already on ADF. Should I move to Fabric Data Factory?

Microsoft’s position in the post:

- **You don’t have to move immediately**, because Microsoft continues to fully support Azure Data Factory (ADF).
- **You’ll likely want to move over time**, because Microsoft says new data integration innovation is landing in **Fabric Data Factory** (for example: new connectors, new capabilities, and AI-assisted data integration with Copilot).

Why migrate (per the post):

- Less engineering overhead
- AI-assisted development
- Pro-developer tooling *and* low-code tooling in one place
- A more unified platform instead of “stitching multiple services together”

The post also claims existing ADF pipelines can come over **without rewrites**, preserving:

- Connector knowledge
- Pipeline logic
- Operational patterns

And it highlights Fabric-only capabilities such as:

- Copilot-assisted development
- Mirroring
- Copy Jobs
- Apache Airflow
- dbt
- Unified CI/CD
- OneLake integration

## Fabric Data Factory is newer. How do I know it’s mature enough for production workloads?

The post argues Fabric Data Factory is production-ready because it is built on the same foundation as ADF:

- Same execution engine
- Same connector library
- Same reliability model

What changes, according to the post, is the “surface area” around that core:

- A modern SaaS experience
- Unified governance
- AI assistance
- Additional tools enabled by Fabric’s SaaS model

![Diagram highlighting benefits of upgrading to Fabric Data Factory (ADF foundation, multi-cloud orchestration, low-code and pro-code capabilities, transformation options)](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/the-image-is-a-diagram-that-highlights-the-benefit.png)

*Figure: Reasons for upgrading to Fabric Data Factory.*

## What does Fabric Data Factory give me that ADF doesn’t?

### Unify data across clouds, on-premises, and more

**Mirroring**

- Described as a low-friction on-ramp.
- Provides continuous, low-latency replication from sources like **SQL Server, Snowflake, Google BigQuery, Cosmos DB**, and others into **OneLake**.
- Emphasizes “no pipelines to build, configure, or maintain.”
- Notes cost detail: [Fabric compute and OneLake storage for mirroring are free up to a capacity-based limit](https://learn.microsoft.com/fabric/mirroring/overview#cost-of-mirroring).

**Copy Jobs**

- Configuration-first bulk copy and incremental sync.
- Supports watermark-based incremental sync and native CDC scenarios.
- Includes **SCD Type 2** support for full history tracking with built-in audit columns.

**SAP integration**

- Mentions Microsoft’s partnership with SAP via the Business Data Cloud (BDC) ecosystem.
- Links: [Microsoft’s strategic partnership with SAP](https://blog.fabric.microsoft.com/blog/29410).
- Describes “configuration-driven integration patterns” for SAP-sourced operational data (e.g., **SAP S/4HANA**) into OneLake.

**Connector breadth and destinations**

- Links: [Fabric Data Factory connector library](https://learn.microsoft.com/fabric/data-factory/connector-overview).
- Claims Fabric Data Factory has Microsoft’s broadest connector set to date.
- Highlights moving data to destinations like **Snowflake, Oracle, Google BigQuery, Amazon S3, Databricks**.
- Notes Dataflows Gen2 can write transformed data to platforms such as Snowflake and BigQuery.

**OneLake Shortcuts** (data-in-place)

- Links: [OneLake Shortcuts](https://learn.microsoft.com/fabric/onelake/onelake-shortcuts).
- Lets you connect to data in Azure/AWS/GCP without copying.
- Centralizes credentials/permissions through OneLake.

### Change data made easy

The post calls out typical pain points in batch change processing:

- Late-arriving data
- Out-of-order events
- Schema drift

Fabric features positioned as simplifications:

- Mirroring handles inserts/updates/deletes without custom pipeline logic.
- Copy Jobs provide [native CDC](https://learn.microsoft.com/fabric/data-factory/cdc-copy-job) and SCD Type 2 support.

### AI-accelerated development

Fabric Data Factory includes **Copilot** in the authoring experience. The post lists capabilities:

- Generate pipelines from natural language
- Expression suggestions and explanations
- Inline troubleshooting (identify errors and recommend fixes)
- Expose Copy Jobs as **MCP endpoints**

It also points to the **Data Factory MCP server**:

- GitHub: [microsoft/DataFactory.MCP](https://github.com/microsoft/DataFactory.MCP)
- Described as enabling pipelines/dataflows/data movement to be built and operationalized through AI.

### Low code transformation at scale

**Dataflows Gen2**

- Cloud Power Query experience for visual (no-code) transformations.
- Mentions “hundreds of data sources” and “300+ built-in transformations.”
- Multiple output destinations (examples listed): Lakehouse, Warehouse, Azure SQL, Snowflake, ADLS Gen2.
- Performance/cost improvements via “Partitioned execution” and “Modern Query Evaluator.”
- Mentions variable library integration for environment promotion (dev/test/prod), positioning it as CI/CD-ready.

### Pro-developer tooling

Fabric Data Factory adds managed capabilities (no separate infra) alongside pipelines:

**Apache Airflow Jobs**

- Docs: [Apache Airflow Jobs concepts](https://learn.microsoft.com/fabric/data-factory/apache-airflow-jobs-concepts).
- SaaS-native Airflow experience inside Fabric.
- No cluster/scheduler provisioning.
- Runs in the same workspace as pipelines, Dataflows Gen2, and Copy Jobs.

**dbt Jobs**

- Docs: [dbt job overview](https://learn.microsoft.com/fabric/data-factory/dbt-job-overview).
- “Operationalize dbt” without separate compute/storage/auth layers.
- Git-backed version control.
- Environment-specific execution without custom scripting.
- Integrated with Lakehouse and Data Warehouse.

## How do I migrate? Where do I start?

The post says migration is incremental and starts inside the ADF authoring UX:

- **Assess** inside ADF to identify which pipelines are ready and which need review.
- **Migrate selectively and in phases** based on priority.
- **No rewrites required** (pipelines brought over as-is) while unlocking Fabric features.
- **Continue running ADF during transition** to validate parity and move at your own pace.

It also states the migration experience:

- Maps ADF linked services to Fabric connections
- Automates the assessment
- Produces a plan before moving anything

## Next steps (links from the post)

- [Differences between Azure Data Factory and Fabric Data Factory](https://learn.microsoft.com/fabric/data-factory/compare-fabric-data-factory-and-azure-data-factory)
- [Training on Fabric Data Factory](https://learn.microsoft.com/training/browse/?terms=data%20factory%20fabric)
- [Migration assessment tool (ADF to Fabric)](https://learn.microsoft.com/azure/data-factory/how-to-assess-your-azure-data-factory-to-fabric-data-factory-migration)
- [Plan migration to Fabric Data Factory](https://learn.microsoft.com/fabric/data-factory/migrate-planning-azure-data-factory?toc=/azure/data-factory/TOC.json&bc=/azure/data-factory/breadcrumb/toc.json)
- [2-minute migration demo video](https://www.youtube.com/watch?v=M2TS1U9bE54)
- [Microsoft Fabric Ideas portal (Data Factory)](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/answers-to-common-questions-about-fabric-data-factory/)

