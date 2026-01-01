---
layout: "post"
title: "Ignite 2025: Fabric’s Transformation into an Intelligence Platform"
description: "This article by Markus Lehtola summarizes Microsoft Ignite 2025 announcements, focusing on how Microsoft Fabric is evolving from a traditional data warehouse to an enterprise intelligence platform. Key highlights include new features for data engineers, the strategic expansion to AI agent infrastructure, and enhanced data operations and monitoring capabilities."
author: "markus.lehtola@zure.com (Markus Lehtola)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://zure.com/blog/ignite-2025-fabrics-evolution-from-data-platform-to-intelligence-platform"
viewing_mode: "external"
feed_name: "Zure Data & AI Blog"
feed_url: "https://zure.com/blog/rss.xml"
date: 2025-11-19 08:32:01 +00:00
permalink: "/2025-11-19-Ignite-2025-Fabrics-Transformation-into-an-Intelligence-Platform.html"
categories: ["AI", "Azure", "ML"]
tags: ["Agent 365", "AI", "AI Agents", "Analytics Engineering", "Azure", "Blogs", "Capacity Planning", "Data", "Data Architecture", "Data Clustering", "Data Warehouse", "Dbt", "Eventhouse", "Fabric IQ", "KQL Database", "Microsoft", "Microsoft Fabric", "Microsoft Foundry", "ML", "Operations Agent", "Power BI", "Real Time Intelligence", "SAP Mirroring"]
tags_normalized: ["agent 365", "ai", "ai agents", "analytics engineering", "azure", "blogs", "capacity planning", "data", "data architecture", "data clustering", "data warehouse", "dbt", "eventhouse", "fabric iq", "kql database", "microsoft", "microsoft fabric", "microsoft foundry", "ml", "operations agent", "power bi", "real time intelligence", "sap mirroring"]
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
