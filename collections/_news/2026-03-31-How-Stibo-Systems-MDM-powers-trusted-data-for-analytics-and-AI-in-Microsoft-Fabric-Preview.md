---
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
section_names:
- ai
- ml
title: How Stibo Systems’ MDM powers trusted data for analytics and AI in Microsoft Fabric (Preview)
date: 2026-03-31 09:00:00 +00:00
tags:
- AI
- Azure Marketplace
- Copilot Queries
- DaaS
- Data Agents
- Data as A Service
- Data Governance
- Delta Parquet
- Direct Lake Mode
- Golden Records
- Lakehouse
- Master Data Management
- MDM Workload
- Microsoft Fabric
- ML
- News
- OneLake
- Power BI
- Real Time Analytics
- Semantic Models
- Shortcuts
- Stibo Systems
primary_section: ai
external_url: https://blog.fabric.microsoft.com/en-US/blog/how-stibo-systems-mdm-powers-trusted-data-for-analytics-and-ai-in-microsoft-fabric-preview/
---

Microsoft Fabric Blog (with coauthor Simon Tuson) announces a preview integration that brings Stibo Systems Master Data Management into Microsoft Fabric, ingesting curated master data into OneLake/Lakehouse so teams can use Direct Lake, semantic models, and Fabric AI features for analytics and AI workloads with less ETL overhead.<!--excerpt_end-->

# How Stibo Systems’ MDM powers trusted data for analytics and AI in Microsoft Fabric (Preview)

Coauthor: Simon Tuson, Principal Product Manager, Stibo Systems

The preview of the **Stibo Systems Master Data Management (MDM) workload on Microsoft Fabric** is now available. It integrates enterprise master data and ingests it directly into **Fabric OneLake** through Stibo’s **DaaS (Data as a Service)** feature to support analytics and AI use cases.

![MDM workload detailed description page](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/mdm-workload-detailed-description-page-.png)

*Figure: Detailed description of the Stibo workload on the workload hub.*

## Why this matters

MDM and analytics platforms optimize for different goals:

- **MDM systems**: governance, stewardship, data quality.
- **Analytics/AI platforms**: speed, flexibility, delivering insights at scale.

Historically, bridging these often required rigid integrations, brittle exports, and ongoing maintenance.

Stibo’s **Data as a Service (DaaS)** approach aims to make master data available as **analytics-ready data in Microsoft Fabric**, reducing the need to build complex pipelines for transformation, schema rework, or duplication.

## What’s available in preview

The preview focuses on making governed master data easier to activate at scale within Fabric:

- Bring trusted data from upstream MDM into **OneLake**.
- Use **Power BI Direct Lake mode** for reporting and visualization.
- Use **Lakehouse semantic models** to build **Data Agents** and run **Copilot queries** on top of the data.
- Support faster iteration for changing business needs.

## Workload capabilities

Key capabilities described:

- Access curated data assets from MDM directly in Fabric.
- Group and transform master data (golden records, product data, etc.) into **analytics-ready datasets**.
- Store data in open format: **delta-parquet** in the customer’s **Lakehouse**.
- Deliver data into Fabric workloads “without programming and detailed MDM knowledge” (as stated in the post).

### OneLake-native integration options

- Materialize master data into **Lakehouse tables**.
- Or access it via **shortcuts**, enabling reuse across the Fabric ecosystem.
- Support self-service analytics with centralized governance: data teams configure exposed assets and refresh behavior, while MDM teams retain control over definitions and quality rules.

### Near real-time reporting path

- Use trusted Lakehouse data to power Power BI reporting through **Direct Lake mode**.
- With new data ingested into the Lakehouse, the semantic model powering the Power BI layer is described as staying up to date in near real time.

## Example use cases (as described)

- **Context-aware dynamic assortments**: combine product master data with contextual signals (weather, events, local demand) to optimize assortments.
- **Regulatory impact analysis**: use master data (supplier, material, country of origin, etc.) in Fabric to support scenario modeling and reporting as regulations change.

## Getting started

- Product overview and preview offer: [Azure Marketplace](https://aka.ms/AA10cmdi)
- Demo video: [YouTube](https://www.youtube.com/watch?v=v1mdsMK94E0)
- Support/contact: [Contact Stibo](https://aka.ms/AA10ct9k)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/how-stibo-systems-mdm-powers-trusted-data-for-analytics-and-ai-in-microsoft-fabric-preview/)

