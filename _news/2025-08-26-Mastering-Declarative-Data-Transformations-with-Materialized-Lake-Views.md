---
layout: "post"
title: "Mastering Declarative Data Transformations with Materialized Lake Views"
description: "This article demonstrates how enterprises can leverage Microsoft Fabric's Lakehouse architecture and materialized lake views to streamline the transformation of mirrored SQL data for analytics. It covers mirroring Azure SQL databases to OneLake, building medallion architectures (Bronze, Silver, Gold), and automating data enrichment using declarative views for better governance, performance, and AI-driven insight generation."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/mastering-declarative-data-transformations-with-materialized-lake-views/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-26 16:00:00 +00:00
permalink: "/2025-08-26-Mastering-Declarative-Data-Transformations-with-Materialized-Lake-Views.html"
categories: ["Azure", "ML"]
tags: ["Analytics", "Azure", "Azure SQL Database", "Data Engineering", "Data Governance", "Data Monitoring", "Data Orchestration", "Data Pipeline", "Data Quality", "Declarative Transformation", "ETL", "Lakehouse", "Materialized Lake Views", "Medallion Architecture", "Microsoft Fabric", "ML", "News", "OneLake", "SQL Mirroring"]
tags_normalized: ["analytics", "azure", "azure sql database", "data engineering", "data governance", "data monitoring", "data orchestration", "data pipeline", "data quality", "declarative transformation", "etl", "lakehouse", "materialized lake views", "medallion architecture", "microsoft fabric", "ml", "news", "onelake", "sql mirroring"]
---

Microsoft Fabric Blog explains how organizations can use materialized lake views in Fabric's Lakehouse architecture to automate, monitor, and scale data transformation from Azure SQL Databases. The article covers step-by-step methods for improved analytics and governance.<!--excerpt_end-->

# Mastering Declarative Data Transformations with Materialized Lake Views

Organizations increasingly need scalable analytics for large, centralized SQL data stores. Traditional reporting can become a bottleneck as business teams demand faster and more customized insights. Microsoft Fabric’s Lakehouse architecture addresses this by allowing teams to mirror SQL data into OneLake, leveraging a Medallion architecture—Bronze, Silver, and Gold layers—to organize and process data efficiently.

Materialized lake views are key to this setup, enabling:

- Automated, declarative data transformations
- Streamlined enrichment and cleansing (Silver layer)
- Reliable input for dashboards and AI-driven analytics
- Scalable governance and performance monitoring

## Step-by-Step Implementation

**Step 1:** Mirror your Azure SQL Database to Microsoft Fabric using [this tutorial](https://learn.microsoft.com/en-us/fabric/database/mirrored-database/azure-sql-database) within your chosen Workspace (Workspace A).

**Step 2:** Create a shortcut from the Bronze tables (applying filters as needed) into another Workspace (Workspace B).

**Step 3:** Build Silver and Gold materialized lake views. Use the [MLV syntax](https://learn.microsoft.com/fabric/data-engineering/materialized-lake-views/create-materialized-lake-view) within your Fabric notebook, filtering the Bronze layer and defining transformations.

**Step 4:** After building Silver and Gold views, access the Lakehouse to manage and [view materialized lake view lineage](https://learn.microsoft.com/fabric/data-engineering/materialized-lake-views/view-lineage), aiding transparency and auditability.

**Step 5:** Schedule materialized lake view runs according to your business and analytics needs to automate ongoing data transformation.

With these steps, organizations can continuously ingest SQL Server data into Fabric, transform and curate it within the Lakehouse, and produce actionable analytics and AI-ready datasets.

## Learn More

- [Materialized Lake Views documentation](https://learn.microsoft.com/fabric/data-engineering/materialized-lake-views/overview-materialized-lake-view)
- Stay tuned for upcoming features, such as improved lineage viewing across layers
- Provide feedback or suggest ideas via the [Fabric Ideas portal](https://community.fabric.microsoft.com/t5/Fabric-Ideas/ct-p/fbc_ideas)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mastering-declarative-data-transformations-with-materialized-lake-views/)
