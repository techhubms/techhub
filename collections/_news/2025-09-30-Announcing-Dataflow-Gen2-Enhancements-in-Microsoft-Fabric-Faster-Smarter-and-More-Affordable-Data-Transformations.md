---
layout: post
title: 'Announcing Dataflow Gen2 Enhancements in Microsoft Fabric: Faster, Smarter, and More Affordable Data Transformations'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/unlocking-the-next-generation-of-data-transformations-with-dataflow-gen2-fabcon-europe-2025-announcements/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-09-30 09:00:00 +00:00
permalink: /ai/news/Announcing-Dataflow-Gen2-Enhancements-in-Microsoft-Fabric-Faster-Smarter-and-More-Affordable-Data-Transformations
tags:
- AI
- AI Powered Transformation
- Azure
- Azure Data Lake Storage Gen2
- CI/CD
- Copilot
- Data Transformation
- Dataflow Gen2
- Lakehouse
- Microsoft Fabric
- Migration
- ML
- Modern Query Evaluation
- Natural Language Data Ingestion
- News
- OneLake Catalog
- Parameterized Dataflows
- Performance Optimization
- Pricing Model
- SharePoint Integration
- Snowflake
- Variable Libraries
section_names:
- ai
- azure
- ml
---
Microsoft Fabric Blog details the latest Dataflow Gen2 enhancements for Microsoft Fabric, offering faster runtimes, better pricing, AI-driven transformations, and expanded integration options for enterprise data engineers and architects.<!--excerpt_end-->

# Unlocking the Next Generation of Data Transformations with Dataflow Gen2 – FabCon Europe 2025 Announcements

Microsoft has announced a comprehensive set of enhancements for Dataflow Gen2 within Microsoft Fabric, targeting enterprise data prep and transformation workflows. These improvements address customer feedback and focus on usability, performance, and integration in modern data environments.

## Key Enhancements

### Smarter, Cost-Effective Pricing

- New 2-tier pricing: first 10 minutes of each query billed at 12 CU (25% reduction), time beyond 10 minutes at 1.5 CU (90% reduction)
- Designed to enable cost-effective scaling for dataflows of any size
- Effective immediately for CI/CD operations; migration for non-CI/CD via 'Save as Dataflow Gen2 (CI/CD)'
- [More on pricing](https://learn.microsoft.com/fabric/data-factory/pricing-dataflows-gen2)

### Faster Dataflow Runs

- Introduction of Modern Query Evaluation Service for accelerated data processing
- Supports parallel query execution through partitioning
- Together, these yield significantly reduced run times and lower operational costs

### Enhanced Design-Time Productivity

- 'Preview only steps' feature accelerates design-time data sampling and iteration
- Reduces preview wait times for iterative design and testing
- [Preview only steps documentation](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-preview-only-step)

### Expanded Data Destinations

- **Lakehouse CSV Output:** Direct export to CSV files in Fabric Lakehouse, helping teams using Spark, Python, and open-source tools
- **Azure Data Lake Storage Gen2:** Write support for CSV outputs; centralized data ingestion for large-scale projects
- **Snowflake Integration:** Sneak peek of support for Snowflake databases, promising multi-cloud flexibility
- **SharePoint & Excel:** General availability of SharePoint CSV destination, with Excel XLSX support coming soon
- **OneLake Catalog Integration:** Unified metadata and data destination discovery
- Enhanced configuration for default output destinations
- [Learn about data destinations](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-data-destinations-and-managed-settings)

### Advanced Parameterization and Variable Libraries

- Parameterized Dataflows (GA): Reusable, environment-independent dataflows
- Support for parameterizing output destinations
- Native support for referencing Fabric Variable Libraries within dataflows
- [Using Fabric variable libraries](https://learn.microsoft.com/fabric/data-factory/dataflow-gen2-variable-library-integration)

### AI-Powered Features with Copilot

- Enhanced Get Data experience with Copilot integration allows natural language-driven data ingestion and transformation
- Upcoming AI-powered transformation prompts for sentiment analysis, summarization, and categorization
- [More about Copilot integration](https://aka.ms/diaifabric)

### Migration and Compatibility

- General availability of the 'Save Dataflow Gen1 as Dataflow Gen2' migration tool
- Simplifies transition to Gen2 with enhanced features and compatibility
- [Migration documentation](https://learn.microsoft.com/fabric/data-factory/migrate-to-dataflow-gen2-using-save-as)

## Why This Matters

- **For developers and architects:** Faster iterations, lower costs, and unified data integration improve end-to-end data engineering workflows.
- **For data engineers:** More destinations and automation options support hybrid and multi-cloud architectures with strong governance and access controls.
- **For organizations:** AI-powered capabilities (Copilot) and natural language prompts make complex data transformation more accessible and scalable.

## Get Started

Read the official Microsoft documentation and try out the new features in your own dataflows. Community feedback remains central to shaping these improvements—share your experience to help further enhance Microsoft Fabric’s data platform capabilities.

For further information, visit the [Microsoft Fabric blog post](https://blog.fabric.microsoft.com/en-us/blog/unlocking-the-next-generation-of-data-transformations-with-dataflow-gen2-fabcon-europe-2025-announcements/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/unlocking-the-next-generation-of-data-transformations-with-dataflow-gen2-fabcon-europe-2025-announcements/)
