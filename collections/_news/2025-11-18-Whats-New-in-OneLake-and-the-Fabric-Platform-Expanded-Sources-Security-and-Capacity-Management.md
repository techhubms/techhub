---
layout: post
title: 'What’s New in OneLake and the Fabric Platform: Expanded Sources, Security, and Capacity Management'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-onelake-and-the-fabric-platform-more-sources-security-and-capacity-tooling/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-18 08:00:00 +00:00
permalink: /ai/news/Whats-New-in-OneLake-and-the-Fabric-Platform-Expanded-Sources-Security-and-Capacity-Management
tags:
- AI
- AI Readiness
- Azure
- Capacity Management
- Cosmos DB
- Customer Managed Keys
- Data Governance
- Data Lake
- Data Mirroring
- Data Shortcuts
- Fabric IQ
- Foundry IQ
- Microsoft 365
- Microsoft Fabric
- ML
- News
- OneLake
- Outbound Access Protection
- Power BI
- Real Time Analytics
- SAP Integration
- Security
- SQL Server
- Zero ETL
section_names:
- ai
- azure
- ml
- security
---
Microsoft Fabric Blog introduces a major update to OneLake and the Fabric platform, revealing new mirroring sources, security controls, and AI-ready data governance features for organizations seeking unified, intelligent data estates.<!--excerpt_end-->

# What’s New in OneLake and the Fabric Platform: Expanded Sources, Security, and Capacity Management

Organizations today face increasing pressure to unify data across clouds, systems, and formats, while upholding stringent requirements for data governance, security, and AI readiness. Microsoft Fabric addresses these challenges, with thousands of customers already adopting its capabilities for analytics, AI, and operational workloads.

## Unified Data Estate with Microsoft OneLake

OneLake offers centralized management for multi-cloud and on-premises data, accessible via an organization-wide catalog. New capabilities enable easier data unification:

- **Mirroring Sources**: General availability for PostgreSQL, Cosmos DB, and SQL Server (2016–2022, 2025). Preview for SAP mirroring powered by SAP Datasphere and bidirectional integration with SAP BDC. Iceberg support for Snowflake mirroring is now generally available.
- **Zero-ETL Approach**: Streamlines data integration, eliminating complex ETL and providing analytics-optimized Delta tables.
- **Shortcuts**: Preview features for SharePoint and OneDrive shortcuts allow unstructured productivity data to reside in OneLake without manual file copying or custom ETL flows. This supports agent training and keeps files updated in real time.

## Agents and AI Integration with Foundry IQ

Microsoft announced Foundry IQ by Azure AI Search, advancing retrieval-augmented generation (RAG). Agents can now use OneLake as a knowledge source, connecting to multi-cloud sources such as AWS S3 and diverse enterprise data, without unnecessary duplication. AI developers benefit from curated, governed knowledge bases for building intelligent agents grounded in accurate data.

## Governance and Security Enhancements

The OneLake catalog is being expanded as a strategic control plane for data governance and security:

- **Admin Experience**: A new Govern tab offers domain/capacity inventory, workspace insights, protection status, and detailed Power BI reports. Copilot integration provides automated model summaries and actionable insights for admins.
- **ReadWrite Permissions**: Teams can configure folder-level write access within lakehouses for controlled data contributions, improving collaboration without overgranting workspace roles.

## Network Security and Capacity Management

Reliability and protection remain central to new platform features:

- **Outbound Access Protection (OAP)**: Restricts outbound connections on dataflows, pipelines, and shortcuts to approved endpoints. OAP support for Spark and SQL Analytics Endpoints is generally available.
- **Customer Managed Keys**: Now generally available and supporting Azure Key Vault integration with firewall security, extends to Cosmos DB and SQL Databases in Fabric.
- **Surge Protection and Capacity Overage**: New controls enable workload protection, automatic overage payment for periods of high demand, budget limits, and seamless throttling management. Real-time capacity events are available in preview, offering granular metrics and instant state updates for admins.

## Platform and Ecosystem Updates

- **Fabric IQ Workload**: Unifies data with operational systems under a semantic model, providing a live, connected business view and supporting modern analytics applications.
- **Expanded Interoperability**: Bi-directional, zero-copy sharing with SAP, Salesforce, Azure Databricks, and Snowflake.
- **AI Infusion**: AI and Copilot features are available in Power BI and Fabric operation agents for embedded intelligence.

## Community, Certification, and Further Learning

- Dedicated Fabric Community Conference and SQLCon 2026 announced.
- Skills Challenges for DP-600 and DP-700 certifications are live.
- Resources include free trials, roadmap updates, community forums, partner blogs, and technical blogs for deeper exploration.

---

For full details, links to relevant documentation, and community engagement, see: [What's new in OneLake and the Fabric platform](https://blog.fabric.microsoft.com/en-us/blog/whats-new-in-onelake-and-the-fabric-platform-more-sources-security-and-capacity-tooling/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/whats-new-in-onelake-and-the-fabric-platform-more-sources-security-and-capacity-tooling/)
