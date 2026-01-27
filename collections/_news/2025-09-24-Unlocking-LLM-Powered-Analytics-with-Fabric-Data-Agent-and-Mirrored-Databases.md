---
external_url: https://blog.fabric.microsoft.com/en-US/blog/unlocking-llm-powered-through-data-agent-from-your-mirrored-databases-in-microsoft-fabric/
title: Unlocking LLM-Powered Analytics with Fabric Data Agent and Mirrored Databases
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-24 17:00:00 +00:00
tags:
- AI Integration
- Azure Cosmos DB
- Azure SQL Database
- Cloud Data
- Data Agent
- Data Analytics
- Data Pipeline
- Database Mirroring
- Delta Parquet
- LLM
- Microsoft Fabric
- Mirrored Database
- Natural Language Queries
- NL2SQL
- Operational Data
section_names:
- ai
- azure
- ml
primary_section: ai
---
Microsoft Fabric Blog showcases how the new Data Agent feature supports mirrored databases, allowing LLM-powered analytics and instant natural language insights across various operational data sources.<!--excerpt_end-->

# Unlocking LLM-Powered Analytics with Fabric Data Agent and Mirrored Databases

**Author:** Microsoft Fabric Blog

Microsoft Fabric has expanded its Data Agent capabilities to directly support mirrored databases, making it easier for organizations to harness their operational data for AI-driven analytics.

## What Are Mirrored Database Artifacts?

Mirrored database artifacts represent external databases (like Azure Cosmos DB, Azure SQL Database, Oracle, Snowflake, and Databricks) within Microsoft Fabric. This process syncs operational data into the Delta Parquet format in Fabric, bringing formerly siloed data into a platform ready for advanced analytics and Large Language Model (LLM) applications.

## Key Supported Mirrored Artifacts

- Azure Cosmos DB
- Azure Database for PostgreSQL
- Azure Databricks catalog
- Azure SQL Database
- Azure SQL Managed Instance
- Oracle
- Snowflake
- SQL Server Database

## How It Works

1. **Mirror Your Database:** Bring your Azure or external databases into Microsoft Fabric as mirrored artifacts using open mirroring.
2. **Add to Data Agent:** Within the Fabric Data Agent interface, directly add these mirrored artifacts as data sources—no need for intermediary Lakehouses.
3. **Curate and Query:** Customize the Data Agent with instructions, provide sample queries, and use natural language (NL2SQL) to generate instant analytics powered by LLMs.

## Highlighted Features

- **Direct Integration:** Seamlessly connect mirrored databases to Fabric Data Agent.
- **AI-Driven Analytics:** Use NL2SQL to ask questions in natural language and get actionable insights in real time.
- **Broad Compatibility:** Integrate data from a range of databases, simplifying analytics over mixed data sources.
- **Customizable Knowledge Scope:** Curate and focus the Data Agent on specific tables for precise results.
- **Seamless AI Application:** Bridge operational data and AI workloads without manual pipeline setup.

## Getting Started

- Mirror databases (Azure, Oracle, Snowflake, Databricks, and more) using open mirroring.
- Use the Data Agent interface to add mirrored artifacts.
- Curate the agent and begin querying with natural language.

## Additional Resources

- [Fabric data agent concepts (preview)](https://learn.microsoft.com/fabric/data-science/concept-data-agent)
- [Open Mirroring documentation](https://learn.microsoft.com/fabric/database/mirrored-database/open-mirroring)

## Summary

Connecting mirrored artifacts directly to Fabric Data Agent empowers you to conduct LLM-powered analytics across all operational data, speeding up insight generation and enhancing decision-making capabilities within the Microsoft ecosystem.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/unlocking-llm-powered-through-data-agent-from-your-mirrored-databases-in-microsoft-fabric/)
