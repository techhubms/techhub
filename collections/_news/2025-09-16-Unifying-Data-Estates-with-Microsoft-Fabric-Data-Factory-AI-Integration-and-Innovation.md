---
external_url: https://blog.fabric.microsoft.com/en-US/blog/unify-your-data-estate-for-the-era-of-ai-with-fabric-data-factory/
title: 'Unifying Data Estates with Microsoft Fabric Data Factory: AI, Integration, and Innovation'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-16 01:00:00 +00:00
tags:
- AI Copilot
- Azure Key Vault
- Azure SQL
- Change Data Capture
- CI/CD
- Copy Job
- Data Integration
- Data Mirroring
- Data Orchestration
- Database Replication
- Dataflow Gen2
- Fabric Data Factory
- Lakehouse
- Managed Airflow
- Microsoft Fabric
- Natural Language Transformation
- On Premises Gateway
- OneLake
- Pipelines
- Power Query
- Semantic Models
- VNet Gateway
- Zero ETL
- AI
- Azure
- ML
- News
section_names:
- ai
- azure
- ml
primary_section: ai
---
The Microsoft Fabric Blog team highlights recent innovations in Fabric Data Factory, focusing on enterprise-scale data integration, AI-driven features, and new performance updates to empower data engineers and analysts.<!--excerpt_end-->

# Unifying Data Estates with Microsoft Fabric Data Factory: AI, Integration, and Innovation

## Overview

Microsoft Fabric Data Factory now brings together industry-leading, cloud-first data integration capabilities built on OneLake. By fusing Azure Data Factory’s pro-grade integration with Power Query’s citizen data transformation features, Fabric Data Factory offers a unified approach to connect, prepare, and manage data from multicloud and on-premises sources.

## Key Feature Updates

### Dataflow Gen2: Pricing and Performance Improvements

- Lower base rate (12 CU, down from 16 CU)
- Tiered pricing for long-running jobs (reduced cost after 10 minutes)
- Improved performance via modern PQ evaluator and partitioned query execution
- New design-time experience with fast ‘Preview only Steps’ for editing queries

### Petabyte-Scale Data Movement

- 170+ connectors for diverse data sources/destinations
- Copy job allows high-throughput, cross-cloud movement, including AWS, Oracle, PostgreSQL, and more
- Latest features: Copy job orchestration in pipelines, variable library for environment parameterization, support for new formats like Iceberg/JSON, improved scheduling, and integration with Lakehouse, SharePoint, Snowflake
- Enhanced incremental data ingestion (Change Data Feeds, CDC)

### Pro-Code & Low-Code Orchestration

- Low-code pipelines for robust scheduling and automation
- CI/CD enhancements: ADF pipeline migration utility, variable libraries, monitoring workspace
- Pro-code integration with managed Airflow for serverless DAG orchestration
- New activities: Email, Teams, SPN/Workspace identity, Dataflow parameterization

### Database Mirroring

- Zero-copy, zero-ETL data replication into OneLake from major sources (Google BigQuery, Oracle, Azure SQL, Snowflake, etc.)
- Secure access via Private Link, on-prem gateway support, and semantic modeling direct from mirrored databases

### AI-Powered Data Integration (Copilot)

- Author, debug, and monitor pipelines using AI copilots
- New features: Natural language to define columns and transformations, automate documentation, and chat-driven pipeline creation
- Upcoming: AI-powered transformation prompts for sentiment analysis, summarization, and categorization

### Security and Mission-Critical Enhancements

- Workspace identity support and Private Link for secure, credential-free connections
- Azure Key Vault integration for managing secrets
- VNet Gateway, API support, and enhanced administration via PowerShell
- Planned: Snowflake key-pair authentication for automation

## Practical Impact

- Simplifies complex data estate unification, enabling data engineers and analysts to build intelligence-driven workflows
- Reduces operational costs while increasing performance and scalability
- Empowers users of all skill levels (pro-code, low-code) to automate and transform data across clouds
- Accelerates AI and analytics adoption with integrated copilots and efficient dataflows

## Resources

- [Data Factory Documentation](https://learn.microsoft.com/fabric/data-factory/data-factory-overview)
- [Dataflow Gen2 Pricing](https://aka.ms/dfg2pricing)
- [What is Copy job in Data Factory?](https://aka.ms/FabConEU2025_DataFactory_DataMovement)
- [Security in Data Factory](https://aka.ms/FabConEU2025_DataFactory_Platform)

---
This update underlines Microsoft's ongoing commitment to community-driven innovation in the Fabric ecosystem. For more in-depth feature guides, the Fabric Blog will publish detailed follow-ups.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/unify-your-data-estate-for-the-era-of-ai-with-fabric-data-factory/)
