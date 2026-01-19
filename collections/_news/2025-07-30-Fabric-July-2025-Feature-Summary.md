---
layout: post
title: Fabric July 2025 Feature Summary
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-july-2025-feature-summary/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-07-30 09:05:00 +00:00
permalink: /ai/news/Fabric-July-2025-Feature-Summary
tags:
- Activator
- Azure SQL Database
- Connectors
- Copilot Studio
- Cosmos DB
- Data Factory
- Data Governance
- Data Mesh
- Fabric Data Agent
- Microsoft Fabric
- Mirroring
- NoSQL
- OneLake Catalog
- Pipelines
- Power BI
- Real Time Intelligence
section_names:
- ai
- azure
- ml
- security
---
This article by Microsoft Fabric Blog offers a comprehensive summary of July 2025's new features in Microsoft Fabric, authored by Patrick LeBlanc. Key updates span data science, governance, Power BI, data connectors, and AI-driven integrations with Copilot Studio.<!--excerpt_end-->

# Fabric July 2025 Feature Summary

_Authored by Patrick LeBlanc, Microsoft Fabric Blog_

## Overview

The July 2025 update for Microsoft Fabric brings several enhancements across the platform, including feature launches, AI-driven integrations, improved real-time intelligence, enhanced data management, and upcoming community events.

---

## Contents

- Events and Announcements
- Fabric Platform Enhancements
- Data Science Features
- Real-Time Intelligence Updates
- Data Factory Enhancements
- Mirroring Improvements
- Database Updates (Cosmos DB)

---

## Events and Announcements

### Microsoft Fabric Community Conference

- The Fabric Community Conference returns, scheduled for September 15-18, 2025, in Vienna, Austria. The event will feature full-day tutorials, a partner pre-day, and over 120 sessions from both product teams and the community.
- Another edition will take place in Atlanta, USA, on March 16-20, 2026.

### Power BI 10th Anniversary

- Power BI’s 10th anniversary was celebrated on July 24th with community events, an on-demand birthday stream by Guy in a Cube, a Dataviz Contest, and 50% exam voucher offers for Fabric and Power BI certifications.

---

## Fabric Platform Enhancements

### Domain Tags

- Supports organizing data into domains and sub-domains, with new capabilities allowing each domain to define and apply its own tags for business-specific contexts. This aids discoverability and governance.
- Tags can be configured by tenants or admins and make searching and filtering content by domain more effective.

### Updated Default Category in OneLake Catalog

- OneLake catalog introduces dynamic default category assignment based on user roles:
    - Power BI users default to the 'Insights' category for analytics and visualization tools.
    - Fabric users default to the 'Data' category for raw and structured data assets.
- User selections are preserved across sessions for a smoother experience.

---

## Data Science Features

### Fabric Data Agent Integration with Microsoft Copilot Studio

- Fabric data agents, which are AI-powered assistants for enterprise data synthesis and policy enforcement, can now be integrated with Copilot Studio.
- This enables organizations to build, deploy, and scale intelligent agents that work across trusted data sources, fostering agent-to-agent collaboration and richer user interactions within business chat scenarios.

### Data Source Instructions

- Fabric Data Agents now support data source-specific instructions, allowing users to guide AI behavior regarding query formulation, filtering, and dataset joins.
- This increases precision and relevance in responses, especially in environments with complex schemas.

### Streaming Results in Fabric Data Agent

- Data Agent now supports streaming incremental query results, displaying live updates as queries are processed, thus reducing wait times and providing transparency for troubleshooting.

### Improved Run Execution Flow for Data Agent

- Enhanced visualizations and step nomenclature for each phase of a Data Agent operation, streamlining understanding and troubleshooting.

---

## Real-Time Intelligence

### Simplified Rule and Object Creation

- Improved workflows for creating and grouping rules, reducing complexity in tracking and alerting on data events at both event and object levels.

### Send Alerts to Teams Groups and Channels

- Activator extends the ability to send automated Teams alerts not just to individuals but to group chats and channels, enhancing workflow integration and collaboration.

### Pass Parameter Values to Fabric Items (Preview)

- Activator can now pass both static and dynamic parameter values to triggered pipelines and notebooks, making events-driven automation richer and more adaptable.

---

## Data Factory Enhancements

### Incremental Copy in Copy Jobs (Generally Available)

- Incremental copy functionality is now GA, supporting more resource-efficient data movement by syncing only changes between runs, reducing processing and costs.

### Upsert Data to More Destinations

- Copy jobs can now upsert (merge) directly into Fabric Lakehouse, Salesforce, Dataverse, Dynamics 365/CRM, and Azure Cosmos DB (NoSQL).

### Expanded Connectors

- New source/destination connectors include SFTP, FTP, IBM Db2, Oracle Cloud Storage, Azure AI Search, and many more.

### Copy from On-Premises to Data Warehouses

- Copy jobs now natively support copying data from on-premises data stores directly into Snowflake and Fabric Data Warehouse using staging in OneLake.

### Manual Control of Auto-Refresh in Pipelines

- Users can now disable or enable auto-refresh of output and monitoring views in pipeline activities, allowing easier exploration of lengthy activity lists.

---

## Mirroring Improvements

### Azure SQL Database Mirroring (Firewall Support, GA)

- Now generally available with support for network security via VNet Data Gateway and On-Premises Data Gateway. Enables secure mirroring of Azure SQL Databases behind firewalls.

### Improved Resume Process for Mirrored Databases

- When Fabric capacity is resumed, mirrored databases accurately show their 'Paused' status and allow manual resume, maintaining data integrity across interruptions.

### UI Option for Retention Period

- The UI now supports direct configuration of mirrored data retention periods, offering more flexibility over data lifecycle management.

---

## Database Updates

### Cosmos DB (NoSQL) in Fabric (Preview)

- Azure Cosmos DB (NoSQL) is now available in preview for Microsoft Fabric, enabling direct integration and real-time analytics on globally distributed NoSQL data within Lakehouses, Notebooks, and Power BI.
- This reduces ETL workload and supports development of AI-driven applications on a unified platform.

---

## Related Blog Highlights

- **OneLake as a Source for COPY INTO and OPENROWSET (Preview):** Simplifies data ingestion for Data Warehouses.
- **JSON Lines Support in OPENROWSET:** Adds parsing capabilities for semi-structured data in Data Warehouses and Lakehouse SQL endpoints.

---

## Further Reading and Documentation

- [Fabric Community Conference Registration](http://aka.ms/fce)
- [Fabric Data Agents Documentation](https://learn.microsoft.com/fabric/data-science/how-to-create-data-agent)
- [OneLake Catalog Documentation](https://learn.microsoft.com/fabric/governance/onelake-catalog-explore)
- [Cosmos DB in Fabric Overview](https://learn.microsoft.com/en-us/fabric/database/cosmos-db/overview)

---

For more detailed instructions on each new feature, view the referenced documentation or follow the embedded links for tutorials and community resources.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-july-2025-feature-summary/)
