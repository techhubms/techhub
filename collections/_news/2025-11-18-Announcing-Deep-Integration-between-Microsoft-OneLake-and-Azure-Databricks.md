---
external_url: https://blog.fabric.microsoft.com/en-US/blog/microsoft-and-databricks-advancing-openness-and-interoperability-with-onelake/
title: Announcing Deep Integration between Microsoft OneLake and Azure Databricks
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-18 08:00:00 +00:00
tags:
- Azure AI Foundry
- Azure Databricks
- Catalog API
- Cloud Data Platform
- Copilot Studio
- Data Analytics
- Data Engineering
- Data Governance
- Data Integration
- Data Interoperability
- Data Lake
- Data Mirroring
- Data Sharing
- Delta Lake
- Lakehouse Architecture
- Microsoft Fabric
- Microsoft OneLake
- Open Data
- Power BI
- Unity Catalog
- AI
- Azure
- Machine Learning
- News
section_names:
- ai
- azure
- ml
primary_section: ai
---
Microsoft Fabric Blog announces deeper Azure Databricks and OneLake integration, detailed by Adam Conway and Arun Ulag, empowering organizations with open lakehouse architecture, integrated analytics, and AI.<!--excerpt_end-->

# Announcing Deep Integration between Microsoft OneLake and Azure Databricks

*Co-authored by [Adam Conway](https://www.linkedin.com/in/adammconway/), SVP Products at Databricks, and [Arun Ulag](https://www.linkedin.com/in/arunulag/), President of Microsoft Azure Data*

## Delivering an Open Data Lakehouse

Microsoft and Databricks continue their multi-year partnership by launching new integrations between Azure Databricks and Microsoft OneLake, forming the backbone of an open, interoperable lakehouse architecture. The collaboration aims to eliminate data silos and simplify access to data for analytics and AI solutions across the Microsoft Fabric platform.

### Key Features and Roadmap

- **Databricks Mirroring to OneLake (Generally Available):**
  - Azure Databricks users can mirror data into OneLake through Unity Catalog, making performance-optimized Databricks tables available instantly across Fabric workloads. Both platforms operate on Delta Lake format data with no movement required.
  - [Integration Guide](https://learn.microsoft.com/fabric/mirroring/azure-databricks)

- **OneLake Data Reading from Databricks (Preview by end of 2025):**
  - Azure Databricks will enable native reading from OneLake via a new catalog API through Unity Catalog. This removes the need for data duplication or complex pipelines and supports analytics acceleration and cost reduction.
  - Data from any Fabric workload will be accessible.

- **Native OneLake Writing from Databricks (Upcoming):**
  - Future developments will allow writing and storing data directly in OneLake, offering significant operational simplicity and improved interoperability.
  - Timeline announcements expected at FabCon March 2026.

### Customer Benefits

- **Tool and Engine Flexibility:** Freely choose the analytics engine, tools, and platform for each task without data movement barriers or silo concerns.
- **Productivity App Integration:** OneLake catalog now integrates with Microsoft 365 experiences, including Teams, Excel, and Copilot Studio, enabling business users to leverage governed data natively within productivity workflows.
- **Resource Efficiency:** A unified copy of data across Microsoft Fabric and Azure Databricks prevents duplication, streamlines governance, and enables teams to focus on innovation.
- **Empowered AI and Analytics:** Seamless data integration powers advanced AI (Copilot Studio, AI Foundry), Databricks Agents, and visualization (Power BI) scenarios, so analytics and AI projects benefit from unified datasets without extra data movement.

## Looking Ahead

The collaboration highlights Microsoft and Databricks’ ongoing dedication to openness, flexibility, and innovation through cloud-native data architectures. Customers gain the freedom to build modern data platforms while accelerating AI and analytics outcomes.

> "By bringing Azure Databricks and OneLake closer together, we’re giving customers the freedom to build modern data architectures without compromise."

Stay tuned for updates from Microsoft Fabric and Databricks as these features roll out and evolve.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/microsoft-and-databricks-advancing-openness-and-interoperability-with-onelake/)
