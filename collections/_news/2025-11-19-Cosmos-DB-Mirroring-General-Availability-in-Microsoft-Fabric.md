---
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-ga-of-cosmos-db-in-microsoft-fabric-and-cosmos-db-mirroring/
title: Cosmos DB Mirroring General Availability in Microsoft Fabric
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-19 09:30:00 +00:00
tags:
- BM25
- Cosmos DB
- Data Analytics
- Data Engineering
- DiskANN
- Enterprise Security
- General Availability
- High Velocity Data
- Microsoft Fabric
- NoSQL Database
- OneLake
- Power BI Integration
- Python
- Real Time Analytics
- Reciprocal Rank Fusion
- Reverse ETL
- Spark Notebooks
- T SQL
- User Data Functions
- Vector Indexing
- AI
- Azure
- ML
- News
- Machine Learning
section_names:
- ai
- azure
- ml
primary_section: ai
---
Microsoft Fabric Blog announces the GA of Cosmos DB Mirroring in Fabric, explained by the Fabric Blog team. The update details analytics, AI, and ML features—enabling live, unified data workflows for developers and data professionals.<!--excerpt_end-->

# Cosmos DB Mirroring General Availability in Microsoft Fabric

Cosmos DB in Microsoft Fabric and Cosmos DB Mirroring enable a unified experience for operational and analytical data. Now generally available, this feature lets users analyze live Cosmos DB data directly inside Fabric, eliminating the need for complex or costly ETL. Real-time sync with OneLake provides a single source of truth for both historical and current insights.

## Direct Analytics on Live Data

- Use T-SQL via SQL Endpoint, Python, or Spark Notebooks to query data
- Support for semi-structured and unstructured data powering analytics and ML
- Data remains in sync, available for dashboards, real-time analytics, and ML training

## AI-Optimized Vector Indexing

Cosmos DB is now AI-ready in Fabric:

- **DiskANN**: High-performance vector search, efficient similarity searches across large datasets
- **BM25**: Full-text indexing and search support
- **Hybrid Search**: Combines vector and text search, enabled by the Reciprocal Rank Fusion function
- **ML Scenarios**: Supports anomaly detection and recommendation systems (ALS)
- Retrieve real-time results without scanning entire datasets; enables ML-powered applications without separate vector stores

## Translytical Task Flows and User Data Functions

- Tight integration with Fabric User Data Functions enables custom business logic
- Write-back capabilities: Actions in Power BI can directly update Cosmos DB data
- Simplifies feedback loops between analytics and operational databases

## Reverse ETL Capabilities

- Zero-ETL data movement between Cosmos DB (operational) and OneLake (analytical)
- Fast, low-latency serving layer for real-time insights and applications
- Spark Notebooks can perform reverse ETL operations
- Cosmos DB Spark Connector enables management and throughput configuration

## Security, Performance, and Scalability

- Production-grade performance and enterprise security
- Instant autoscale elasticity, high concurrency, and high availability

## Who Benefits?

- Application developers
- Data engineers
- Analytics users looking for ML-ready, scalable environments

## Resources

- [Cosmos DB in Fabric documentation](https://aka.ms/CosmosFabricDocs)
- [Samples for Cosmos DB in Fabric](https://aka.ms/CosmosFabricSamples)
- [Video tutorials for Cosmos DB in Fabric](https://aka.ms/CosmosFabricVideos)
- Microsoft Ignite session — [BRK228: Real-time Analytics and AI Apps with Cosmos DB in Fabric](https://ignite.microsoft.com/sessions/BRK228?source=sessions)

## Notable Quotes

> “Cosmos DB in Fabric has been a home run for Kinectify...move data with zero ETL into Fabric has allowed us to experiment with Fabric...” — Mike Calvin, CTO at Kinectify

> “Integrating Cosmos DB in Microsoft Fabric...operationalize transactional data for near real-time analytics and GenAI use cases...enabling faster decisions...” — Enterprise Solution Architect, MAQ Software

> “By integrating Cosmos DB in Microsoft Fabric with G Agent’s multi-agent system...real-time agentic rig scheduling...” — Dr. Jamal Al-Enezi, KOC

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-ga-of-cosmos-db-in-microsoft-fabric-and-cosmos-db-mirroring/)
