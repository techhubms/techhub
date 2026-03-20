---
tags:
- AI
- Azure SQL
- Capacity Based Consumption
- Change Data Capture
- Curated Datasets
- Data Engineering
- Data Replication
- Delta Change Data Feed
- Delta Lake
- Fabric Capacity
- Fabric Mirroring
- Incremental Processing
- Microsoft Fabric
- ML
- News
- OneLake
- Oracle
- Power BI
- Preview Features
- Snowflake
- Spark
- SQL Analytics Endpoint
- Views Replication
- Zero ETL
title: 'Extended Capabilities in Mirroring in Microsoft Fabric: Optional Enhancements to Core Mirroring'
feed_name: Microsoft Fabric Blog
primary_section: ai
section_names:
- ai
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/extended-capabilities-in-mirroring-in-microsoft-fabric-optional-enhancements-to-core-mirroring/
date: 2026-03-19 11:00:00 +00:00
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces “Extended Capabilities” for Mirroring in Microsoft Fabric, adding paid, opt-in features like Delta Change Data Feed (CDF) and Mirroring Views to support incremental processing and curated, analytics-ready data in OneLake for analytics and AI workloads.<!--excerpt_end-->

## Overview

Mirroring in Microsoft Fabric is designed to reduce traditional data integration overhead by continuously replicating data from operational systems into **OneLake**, where it becomes available across Fabric workloads (analytics, AI, reporting).

As customers adopt Mirroring at scale, Microsoft is introducing **Extended Capabilities in Mirroring**: optional, paid enhancements that build on top of core Mirroring to support incremental analytics scenarios and curated datasets.

## Core Mirroring vs. Extended Capabilities

### Core Mirroring (free, unchanged)

Core Mirroring includes:

- Continuous replication into OneLake
- Automatic conversion to open **Delta** format
- Built-in **SQL analytics endpoints**
- Analytics-ready data accessible across **Spark**, **SQL**, **Power BI**, and other Fabric workloads

### Extended Capabilities (optional, paid)

Extended Capabilities target cases where replication alone isn’t sufficient, such as:

- Faster data freshness without full reloads
- Incremental processing for downstream pipelines
- Business-ready data that already reflects source-defined logic

These enhancements are described as “more advanced, real-world analytics scenarios” and “curated data experiences in OneLake.”

## Why Extended Capabilities Are Charged

Microsoft positions Extended Capabilities as additional continuous processing beyond basic replication.

- Features like **CDF** and **Views** require continuous change detection, transformation, and emission of data.
- Billing uses Fabric’s **capacity-based consumption model**.

The billing approach is described as:

- Charges apply only when extended features are enabled
- Billing is tied to actual processing activity (not idle state)
- A unified model across extended mirroring features
- Intended transparency and predictability at scale

Reference: Extended Capabilities overview documentation: https://aka.ms/extended-capabilties-mirroring

## Delta Change Data Feed (CDF): Incremental Changes, Continuously Applied

Many data engineering and analytics workflows don’t need full table refreshes; they need to identify **what changed and when**.

**Delta change data feed (CDF)** captures inserts, updates, and deletes at a granular level and applies them incrementally into OneLake. This allows downstream consumers to use **change-only data** rather than reprocessing entire datasets.

![Enabling delta change data feed in the MirroredDB settings pane](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/Screenshot-2026-03-03-at-10.54.47 AM-1024x603.png)

With CDF, customers can:

- Process incremental changes in near real-time
- Use change-only processing
- Enable downstream incremental analytics and AI workflows
- Eliminate custom change-tracking pipelines

CDF is described as working across mirroring sources including:

- Oracle
- Snowflake
- Azure SQL
- Open Mirroring partners

More details: https://aka.ms/cdf-extended-capabilities-mirroring

## Mirroring Views: Bringing Business Logic Directly into OneLake

The post argues analytics teams typically consume curated datasets, not raw tables.

**Mirroring views** allow replication of logical views from source systems into OneLake, preserving filters, joins, and transformations defined upstream. Fabric mirrors the results as **Delta tables**, intended to be ready for analytics and AI workloads.

With mirroring views, customers can:

- Replicate source-defined views instead of physical tables
- Reduce duplication and transformation complexity
- Keep business definitions consistent across systems
- Make curated data available immediately in Fabric

Current availability:

- Views are available in preview for **Mirroring for Snowflake**
- Additional sources are planned

More details: https://aka.ms/views-extended-capabilities-mirroring

## Get Started (Preview)

Extended Capabilities are available today in preview and can be enabled during mirror setup in a Fabric workspace.

Links:

- Extended Capabilities in Mirroring – Overview: https://aka.ms/extended-capabilties-mirroring
- Delta change data feed documentation: https://aka.ms/cdf-extended-capabilities-mirroring
- Views documentation: https://aka.ms/views-extended-capabilities-mirroring

Related announcement roundup referenced in the post:

- FabCon and SQLCon 2026 news: https://aka.ms/FabCon-SQLCon-2026-news

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/extended-capabilities-in-mirroring-in-microsoft-fabric-optional-enhancements-to-core-mirroring/)

