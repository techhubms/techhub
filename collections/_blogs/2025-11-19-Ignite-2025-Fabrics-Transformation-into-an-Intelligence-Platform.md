---
external_url: https://zure.com/blog/ignite-2025-fabrics-evolution-from-data-platform-to-intelligence-platform
title: 'Ignite 2025: Fabric’s Transformation into an Intelligence Platform'
author: markus.lehtola@zure.com (Markus Lehtola)
feed_name: Zure Data & AI Blog
date: 2025-11-19 08:32:01 +00:00
tags:
- Agent 365
- AI Agents
- Analytics Engineering
- Capacity Planning
- Data
- Data Architecture
- Data Clustering
- Data Warehouse
- Dbt
- Eventhouse
- Fabric IQ
- KQL Database
- Microsoft
- Microsoft Fabric
- Microsoft Foundry
- Operations Agent
- Power BI
- Real Time Intelligence
- SAP Mirroring
- AI
- Azure
- Machine Learning
- Blogs
section_names:
- ai
- azure
- ml
primary_section: ai
---
Markus Lehtola reviews key Microsoft Ignite 2025 updates, detailing Fabric’s shift to an intelligence platform and what it means for data and AI teams leveraging Microsoft’s cloud ecosystem.<!--excerpt_end-->

# Ignite 2025: Fabric’s Evolution from Data Platform to Intelligence Platform

**Author: Markus Lehtola**  
Date: 19.11.2025

Microsoft Ignite 2025 highlighted a major shift: Microsoft Fabric is no longer just a data warehouse but a central intelligence platform driving next-generation AI agents and analytics.

## Key Announcements

### 1. Fabric IQ, Foundry, and Agent 365 – The Intelligence Stack

- **Fabric IQ:** Serves as the enterprise-wide semantic intelligence layer, extending the Power BI model to all business entities and processes. For example, definitions like "Customer" or "Churn Rate" are maintained and used consistently across applications.
- **Microsoft Foundry:** The platform for building AI agents. Agents built here leverage Fabric IQ for grounded, trustworthy data access, minimizing risks like "hallucinations."
- **Agent 365:** Acts as the registry and governance layer, enforcing security and access controls for data even in agent-mediated scenarios.

### 2. Data Warehouse Enhancements

- **IDENTITY Columns (Preview):** Enables automatic surrogate key generation, currently supporting only BIGINT types without custom seed/increment.
- **Data Clustering (Preview):** Offers manual data clustering to boost query performance, comparable to "CLUSTER BY" in other modern platforms. Best for optimizing large fact tables by frequently filtered columns.

### 3. Fabric Data Factory Joins the dbt Era

- **dbt Job Support:** Analytics engineers can now orchestrate and trigger dbt models directly in Fabric pipelines, supporting code-first analytics patterns. Initial support is for dbt core, with a high-performance Rust-based dbt Fusion Engine integration planned for 2026.

### 4. SAP Mirroring and Real-Time Intelligence

- **SAP Mirroring:** Low-latency replication from SAP (via Datasphere) into Fabric simplifies unifying SAP and other data sources.
- **Operations Agent and Capacity Events:** Real-Time Intelligence updates include a new Operations Agent for monitoring data freshness/quality and proposing remediation, as well as granular capacity monitoring and historical event retention for capacity planning.

## What This Means for Data Teams

1. **Fabric IQ** becomes the semantic standard for enabling AI agent and analytics scenarios.
2. **dbt and Clustering** enable robust, code-driven analytics engineering on Fabric.
3. **Enhanced Operations:** Real-time monitoring and long-term capacity analytics are improving platform reliability and efficiency.

## Conclusion

Fabric is advancing well beyond a unified data platform—it's now positioned as the "nervous system" of enterprise intelligence, tightly connecting data, analytics, and AI automation in Microsoft’s cloud ecosystem.

---

*Markus Lehtola is a data architect with nearly 10 years of experience in analytics, data, and business intelligence, helping organizations deliver value through data solutions on Azure.*

This post appeared first on "Zure Data & AI Blog". [Read the entire article here](https://zure.com/blog/ignite-2025-fabrics-evolution-from-data-platform-to-intelligence-platform)
