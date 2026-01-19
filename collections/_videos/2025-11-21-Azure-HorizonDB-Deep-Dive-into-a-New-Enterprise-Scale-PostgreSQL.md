---
external_url: https://www.youtube.com/watch?v=BJxrSKAvCis
title: 'Azure HorizonDB: Deep Dive into a New Enterprise-Scale PostgreSQL'
author: Microsoft Events
viewing_mode: internal
feed_name: Microsoft Events YouTube
date: 2025-11-21 17:11:56 +00:00
tags:
- AI Features
- Azure HorizonDB
- Cloud Database
- Cloud Native
- Database Architecture
- Elastic Scaling
- High Availability
- InnovatewithAzureAIappsandagents
- KNN Vector Indexing
- Microsoft Azure
- Microsoft Ignite
- MSIgnite
- Performance Optimization
- PostgreSQL
- Read Replicas
- Rust Runtime
- Write Ahead Log
section_names:
- ai
- azure
---
Microsoft Events hosts a technical session on Azure HorizonDB, offering a deep dive into its cloud-native PostgreSQL architecture, performance optimizations, high-availability features, and built-in AI innovations.<!--excerpt_end-->

{% youtube BJxrSKAvCis %}

# Azure HorizonDB: Deep Dive into a New Enterprise-Scale PostgreSQL

**Speakers:** Adam Prout, Denzil Ribeiro  
**Event:** Microsoft Ignite 2025 (BRK127)  
**Level:** Advanced (300)

## Overview

Azure HorizonDB is Microsoft’s new fully managed PostgreSQL database, engineered for mission-critical workloads on Azure. This session explores how HorizonDB blends open-source Postgres with advanced Azure infrastructure, enabling:

- Enhanced scalability without performance tradeoffs
- Ultra-fast failover and always-on read replicas
- Architectural innovations for reliability and high throughput

## Key Technical Highlights

### Architecture and Engineering

- **Built on PostgreSQL:** Leverages open-source Postgres for compatibility and extensibility
- **Custom WAL Storage:** Technical details on Write Ahead Log (WAL) storage components for data durability and performance
- **Rust-based Runtime (Kimo Geo):** Open-sourced asynchronous runtime designed for efficient parallel operations and improved resource management

### High Availability and Elasticity

- **Fast Failover Mechanisms:** Innovations enabling rapid recovery and minimizing downtime
- **Read Replicas:** Architected for constant availability and scalability for read-heavy workloads
- **Elastic Scaling:** Supports up to 100TB with automated resizing to meet enterprise demands

### Performance & Innovation

- **Small Commit Workload Optimization:** Demonstrations show improved performance for high-frequency transactions
- **Comparison with Standard Postgres HA:** Outlines limitations in traditional setups versus HorizonDB’s approach

### Advanced AI Features

- Integrated AI features to power next-generation applications
- Development of KNN (K-Nearest Neighbor) vector indexing for advanced search capabilities and AI workloads

## Takeaways

- HorizonDB combines familiar PostgreSQL workflows with enterprise-grade Azure reliability and performance
- Advanced engineering, including a custom Rust runtime and optimized WAL storage
- Scaling, failover, and AI-powered indexing unlock new application scenarios on Azure

## Further Resources

- [Plan for Azure SQL and AI Development (Microsoft link)](https://aka.ms/ignite25-plans-AzureSQLAIDevelopment)
- [Microsoft Ignite Sessions On-Demand](https://ignite.microsoft.com)

---
*Session hashtags: #MSIgnite #InnovatewithAzureAIappsandagents*
