---
external_url: https://techcommunity.microsoft.com/t5/azure-migration-and/discover-and-assess-postgresql-databases-for-migration-to-azure/ba-p/4456108
title: Discover and Assess PostgreSQL Databases for Azure Migration Using Azure Migrate
author: ShradhaKalra
feed_name: Microsoft Tech Community
date: 2025-09-23 08:09:24 +00:00
tags:
- Agentless Discovery
- Azure Compute SKU
- Azure Database For PostgreSQL
- Azure Migrate
- Cloud Migration
- Configuration Compatibility
- Cost Analysis
- Database Migration
- Dependency Mapping
- IaaS
- Microsoft Hyper V
- Migration Assessment
- PaaS
- PostgreSQL
- VMware
section_names:
- azure
primary_section: azure
---
ShradhaKalra provides a practical overview of Azure Migrate’s new capabilities to discover and assess PostgreSQL databases for migration and modernization to Azure, highlighting agentless assessment, dependency mapping, and migration planning features.<!--excerpt_end-->

# Discover and Assess PostgreSQL Databases for Azure Migration Using Azure Migrate

Cloud migration can be challenging, especially with a variety of database technologies and workloads to modernize. **Azure Migrate** now offers unified, agentless discovery and assessment at scale for PostgreSQL databases, helping organizations migrate and optimize with confidence.

## Why Use Azure Migrate for PostgreSQL?

Azure Migrate now supports:

- **Agentless Discovery**: Use a lightweight appliance to discover on-premises PostgreSQL installations without agents on every host.
- **Dependency Mapping**: Visualize application tiers and workload dependencies, including connections between your database and application servers.
- **Comprehensive Assessment**: Review configuration details (version, extensions, memory settings), support status, and readiness for specific Azure migration targets.
- **Migration Planning**: Get actionable guidance for modernizing to Azure Database for PostgreSQL (Flexible Server, PaaS) or migrating as-is to Azure Virtual Machines (IaaS). Includes cost estimates, SKU recommendations, and compatibility checks.

## Supported Scenarios

- **Platforms**: Discover PostgreSQL servers running on Windows, Linux, VMware, Hyper-V, Bare-metal, and cloud services (such as AWS EC2 or Azure VM).
- **Database Versions**: Currently supports PostgreSQL Community Edition (versions 6-18).
- **Workload Tagging**: Group related resources for assessment, migration, and modernization.

## End-to-End Workflow

1. **Start Agentless Discovery**: Set up the Azure Migrate appliance to scan your environment for PostgreSQL databases.
2. **Dependency Mapping**: Visualize which applications and workloads connect to each PostgreSQL instance, ensuring complete migration coverage.
3. **Assessment**: Review discovered inventory and verify readiness for Azure migration.
   - Review PostgreSQL version, installed extensions, configuration, and performance data (continuously updated).
   - Check compatibility with Azure Database for PostgreSQL (Flexible Server) or Azure VMs.
   - Get Azure compute/storage SKU recommendations and cost assessment per target service and region.
4. **Address Migration Blockers**: Azure Migrate flags unsupported extensions and incompatible configurations.
5. **Migration Execution**: Use [Azure Database Migration Service](https://azure.microsoft.com/services/database-migration/) or [Azure Migrate: Migration and Modernization](https://learn.microsoft.com/azure/migrate/server-migrate-overview?view=migrate) tools to move your PostgreSQL workloads.

## Key Benefits

- **Unified IT Estate View**: See Windows/Linux servers, ASP.NET/IIS, Java apps, SQL Server, MySQL, and PostgreSQL all in one place.
- **Cost Optimization**: Assess potential savings, select target SKUs, and compare service tiers.
- **Continuous Inventory Refresh**: Get up-to-date visibility as environments evolve.

## Getting Started

- [View the demo](https://aka.ms/migrate/postgreSQL/demo)
- [Learn more about Azure Migrate](https://learn.microsoft.com/azure/migrate/?view=migrate)
- [Explore Azure Database for PostgreSQL](https://azure.microsoft.com/products/postgresql/?msockid=09113ca016dc67900dbc2ace17306692)
- [Read migration documentation](https://docs.microsoft.com/data-migration/)
- [Azure Migration and Modernization Program](https://azure.microsoft.com/migration/migration-program/)

> *This feature is currently in public preview. Only PostgreSQL community edition is supported at this time; see documentation for complete details.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-migration-and/discover-and-assess-postgresql-databases-for-migration-to-azure/ba-p/4456108)
