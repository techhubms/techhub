---
date: 2026-03-16 17:00:00 +00:00
external_url: https://azure.microsoft.com/en-us/blog/from-legacy-to-leadership-how-postgresql-on-azure-powers-enterprise-agility-and-innovation/
feed_name: The Azure Blog
author: Affan Dar
section_names:
- ai
- azure
- github-copilot
- security
primary_section: github-copilot
tags:
- AI
- AI Assisted Migration
- Azure
- Azure Database For PostgreSQL
- Azure HorizonDB
- Azure Kubernetes Service (aks)
- Azure Monitor
- Azure Open Source
- Citus
- Confidential Compute
- Database Migration
- Databases
- DiskANN
- Elastic Clusters
- Encryption
- GitHub Copilot
- Hybrid + Multicloud
- Microsoft Defender For Cloud
- Microsoft Entra ID
- Microsoft Fabric
- Multi Agent System
- News
- Oracle To PostgreSQL Migration
- PostgreSQL
- PostgreSQL Extension For VS Code
- Private Endpoints
- Security
- SSD V2 Storage
- V6 Series Compute SKUs
- VS Code
title: 'From legacy to leadership: How PostgreSQL on Azure powers enterprise agility and innovation'
---

Affan Dar outlines how Azure Database for PostgreSQL (and the new Azure HorizonDB) targets enterprise migration pain from Oracle, including an AI-assisted Oracle-to-PostgreSQL tooling workflow in VS Code powered by GitHub Copilot, plus performance, scaling, and security capabilities on Azure.<!--excerpt_end-->

# From legacy to leadership: How PostgreSQL on Azure powers enterprise agility and innovation

Our mission is to make PostgreSQL the most performant, scalable, and enterprise-ready open database platform available. With Azure Database for PostgreSQL and the newly introduced Azure HorizonDB, Microsoft is positioning PostgreSQL on Azure as a path away from legacy on-prem databases (notably Oracle) and toward more scalable, resilient cloud architectures.

## The cost of standing still

Legacy, on-premises databases can become a bottleneck due to:

- Rising hardware refresh and maintenance costs
- Increasing licensing fees and support costs
- Scalability limits
- Dependence on niche expertise

The post cites customer-reported drivers for moving away from Oracle, including licensing costs, performance bottlenecks, and scalability limits.

## Apollo Hospitals: a migration case study

Apollo Hospitals (a large healthcare provider in Asia) migrated from Oracle to **Azure Database for PostgreSQL** with support from Microsoft and a cloud partner.

Reported outcomes after migration:

- **90% of transactions** complete within **five seconds**
- **99.95% uptime**
- **40% reduction** in deployment timelines
- **60% reduction** in operational costs
- **3x improvement** in overall system performance

Reference:

- Apollo Hospitals customer story: https://www.microsoft.com/en/customers/story/25629-apollo-hospitals-azure-database-for-postgresql

## Smarter Oracle-to-PostgreSQL migrations with AI-assisted tooling

A key challenge in Oracle migrations is converting:

- Schemas
- Stored procedures and functions
- Application code tied to Oracle syntax and drivers (examples mentioned: **Java** and **.NET**)

Microsoft describes an **AI-assisted Oracle-to-PostgreSQL migration tool** (preview) delivered as part of the **PostgreSQL extension for Visual Studio Code**.

### How the tool works (as described)

The workflow described includes:

- Analyzing Oracle schemas and stored procedures
- Converting to PostgreSQL-compatible formats
- Scanning application code (Java/.NET) and updating:
  - Database drivers
  - SQL queries
  - Stored procedure calls
- Generating automated unit tests for validation
- Running post-conversion validation in a scratch PostgreSQL environment

It’s described as using a **multi-agent AI system** and being **powered by GitHub Copilot**.

Related link:

- GitHub Copilot: https://azure.microsoft.com/en-gb/products/github/Copilot

## Post-migration: performance, scale, and security on Azure

The post highlights enterprise capabilities for PostgreSQL on Azure across compute, storage, monitoring, and security.

### Azure Database for PostgreSQL: performance and scaling

- **v6-series compute SKUs**: vertical scaling up to **192 vCores**
- **Elastic clusters** using the open-source **Citus** extension for horizontal scaling (distributed PostgreSQL)
  - Example workload types mentioned:
    - Multi-tenant SaaS
    - IoT platforms
    - Large-scale analytics
- Storage improvements:
  - **SSD v2 storage** for high IOPS and low latency
- Monitoring and tuning:
  - **Azure Monitor** for real-time insights and automated optimization

Links:

- Azure Database for PostgreSQL: https://azure.microsoft.com/en-us/products/postgresql
- Azure Monitor: https://azure.microsoft.com/en-us/products/monitor

### Security features called out

Azure Database for PostgreSQL security capabilities mentioned include:

- **Microsoft Defender for Cloud**
- **Microsoft Entra ID** integration
- Private endpoints
- Confidential compute SKUs
- End-to-end encryption

Link:

- Microsoft Defender for Cloud: https://www.microsoft.com/en-us/security/business/cloud-security/microsoft-defender-cloud

## Azure HorizonDB: PostgreSQL at extreme scale (private preview)

Azure HorizonDB is introduced as a new cloud-native PostgreSQL service (private preview) for “extreme performance and scale requirements.” Claims/capabilities mentioned:

- Up to **3,072 vCores**
- Up to **128 TB** auto-scaling storage
- **Sub-millisecond** multi-zone commit latencies
- Up to **3x higher throughput** than self-managed PostgreSQL
- Built-in “AI and agentic capabilities” including:
  - Built-in AI model management
  - **DiskANN** advanced filtering capabilities

Compatibility and upgrade path:

- PostgreSQL-compatible
- Positioned as a path to start on Azure Database for PostgreSQL and move to HorizonDB later without replatforming/rewrite

Link:

- Azure HorizonDB: https://azure.microsoft.com/en-us/products/horizondb

## Open source and enterprise readiness

The post emphasizes Microsoft’s participation in the PostgreSQL open-source project and claims ongoing investment in:

- Performance
- Security
- Developer experience
- Ecosystem integration

## What “modernize” looks like after migration

In the Apollo Hospitals example, possible next steps mentioned include:

- AI-powered clinical dashboards
- Real-time analytics with **Microsoft Fabric**
- Containerized workloads with **Azure Kubernetes Service (AKS)**

Links:

- Microsoft Fabric: https://www.microsoft.com/en-us/microsoft-fabric
- AKS: https://azure.microsoft.com/en-us/products/kubernetes-service

## Further resources

- Download e-book: https://aka.ms/migrate-to-postgres-on-Azure
- Oracle migration concepts for Azure Database for PostgreSQL: https://learn.microsoft.com/en-us/azure/postgresql/migrate/concepts-oracle-migration


[Read the entire article](https://azure.microsoft.com/en-us/blog/from-legacy-to-leadership-how-postgresql-on-azure-powers-enterprise-agility-and-innovation/)

