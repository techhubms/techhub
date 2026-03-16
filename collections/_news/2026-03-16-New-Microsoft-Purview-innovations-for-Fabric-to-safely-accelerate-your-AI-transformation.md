---
date: 2026-03-16 17:10:00 +00:00
tags:
- Agents
- AI
- Audit
- Copilots
- Data Exfiltration
- Data Governance
- Data Loss Prevention (dlp)
- Data Quality
- Data Security Posture Management (dspm)
- Ediscovery
- Fabric Warehouse
- Information Protection
- Insider Risk Management (irm)
- KQL
- Lakehouse
- Microsoft Fabric
- Microsoft Purview
- ML
- News
- OneLake
- Purview Unified Catalog
- Retention Policies
- Security
- Sensitivity Labels
- SQL
external_url: https://techcommunity.microsoft.com/blog/microsoft-security-blog/new-microsoft-purview-innovations-for-fabric-to-safely-accelerate-your-ai-transf/4502156
title: New Microsoft Purview innovations for Fabric to safely accelerate your AI transformation
feed_name: Microsoft Security Blog
primary_section: ai
section_names:
- ai
- ml
- security
author: Darren Portillo
---

Darren Portillo outlines Microsoft Purview updates for Microsoft Fabric focused on preventing data oversharing and improving governance and data quality, with new DLP, Insider Risk Management, DSPM, and Unified Catalog capabilities aimed at supporting safer AI adoption.<!--excerpt_end-->

## Overview

As organizations adopt AI, security and governance become core requirements—especially around preventing sensitive data oversharing and ensuring data quality.

Microsoft announced new **Microsoft Purview** innovations for **Microsoft Fabric** (shared at FabCon Atlanta) spanning:

1. Discovering risks and preventing data oversharing in Fabric
2. Improving governance processes and data quality across the data estate

## 1. Discover risks and prevent data oversharing in Fabric

As data volume increases with AI usage, Purview positions several capabilities together for Fabric data estates:

- Information Protection
- Data Loss Prevention (DLP)
- Insider Risk Management (IRM)
- Data Security Posture Management (DSPM)

### Purview DLP policies for Fabric Warehouse and KQL/SQL DBs

- **Generally available**: Purview DLP policies to help Fabric admins prevent data oversharing via **policy tips** when sensitive data is detected in assets uploaded to **Fabric Warehouses**.
- **Preview**: Purview DLP to **restrict access** to assets containing sensitive data in **KQL/SQL DBs** and **Fabric Warehouses**.
  - Goal: limit access to sensitive data to asset owners and allowed collaborators.

### Purview Insider Risk Management (IRM) updates for Fabric

- **Generally available**: Purview IRM now supports **Microsoft Fabric lakehouses** (in addition to existing Power BI support) with **ready-to-use risk indicators** based on risky user activities, such as sharing lakehouse data externally.
- **Generally available**: An **IRM data theft policy** for security admins to detect Fabric data exfiltration (example given: exporting Power BI reports).
- **Generally available**: An **IRM pay-as-you-go usage report** for Fabric, providing a dashboard to track consumption and support cost predictability.

### Purview controls for Fabric Copilots and Agents (preview)

Purview capabilities (in preview) apply to **all Copilots and Agents in Fabric**, including:

- **Discover data risks** (for example, sensitive data in prompts and responses) and receive recommended actions.
- **Detect and remediate oversharing risks** using **Data Risk Assessments** in **DSPM**, to identify potentially overshared/unprotected/sensitive Fabric assets and take targeted actions (such as applying labels or policies).
- **Identify risky AI usage** using **Purview Insider Risk Management**, such as users sharing sensitive data with AI.
- **Govern AI usage** using:
  - Microsoft Purview Audit
  - Microsoft Purview eDiscovery
  - Retention policies
  - Non-compliant usage detection

## 2. Improve governance processes and data quality across the data estate

Once data is secured for AI usage, the next challenge is enabling users to find and trust data.

### Purview Unified Catalog

Purview’s **Unified Catalog** is positioned as the foundation for enterprise data governance:

- **Estate-wide data discovery** for a holistic view of the data landscape (reducing underutilization).
- **Built-in data quality tools** to measure, monitor, and remediate issues such as:
  - incomplete records
  - inconsistencies
  - redundancies

Purview governance capabilities are described as supplementing teams using the **Fabric OneLake catalog**.

### Publication workflows for data products and glossary terms

- **Generally available**: Data owners can use **Workflows** in the Purview Unified Catalog to manage publishing of **data products** and **glossary terms**.
- Workflows are described as customizable to help governance teams curate catalogs faster while ensuring responsible publishing and governance.
- Data consumers can request access to data products with more assurance around governance standards.

### Data quality for ungoverned assets (including Fabric data)

- In the Unified Catalog, **Data Quality for ungoverned assets** allows organizations to run data quality checks on assets (including Fabric assets) **without linking them to data products**.
- This is positioned as enabling data quality stewards to work faster and at greater scale to support “high quality data” for AI use cases.

## Looking forward

The post emphasizes that as AI ambitions scale, **data security and governance** become essential. Purview + Fabric are positioned as an integrated foundation to keep data protected, governed, and trusted for responsible AI activation.

## References

- 2025 AI Security Gap: 83% of Organizations Flying Blind: https://www.kiteworks.com/cybersecurity-risk-management/ai-security-gap-2025-organizations-flying-blind/
- The Importance Of Data Quality: Metrics That Drive Business Success: https://www.forbes.com/councils/forbestechcouncil/2024/10/21/the-importance-of-data-quality-metrics-that-drive-business-success/

## Source

- New Microsoft Purview innovations for Fabric to safely accelerate your AI transformation: https://techcommunity.microsoft.com/blog/microsoft-security-blog/new-microsoft-purview-innovations-for-fabric-to-safely-accelerate-your-ai-transf/4502156

[Read the entire article](https://techcommunity.microsoft.com/blog/microsoft-security-blog/new-microsoft-purview-innovations-for-fabric-to-safely-accelerate-your-ai-transf/4502156)

