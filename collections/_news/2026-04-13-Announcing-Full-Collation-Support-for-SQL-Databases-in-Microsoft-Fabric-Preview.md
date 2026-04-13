---
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/announcing-full-collation-support-for-sql-databases-in-microsoft-fabric-preview/
primary_section: ml
tags:
- Azure SQL Database Collations
- Collation
- Data Warehouse Collation
- Database Creation Payload
- Fabric CLI
- Fabric REST API
- Fabric SQL Database
- Globalization
- Microsoft Fabric
- Migration
- ML
- Multilingual Data
- News
- NewSQLDatabaseCreationPayload
- PowerShell
- Replication
- REST API
- SQL Analytics Endpoint
- SQL Database Collations
author: Microsoft Fabric Blog
date: 2026-04-13 10:51:08 +00:00
title: Announcing Full Collation Support for SQL Databases in Microsoft Fabric (Preview)
section_names:
- ml
---

Microsoft Fabric Blog announces (preview) full Azure SQL Database collation support for SQL databases in Microsoft Fabric, explaining what collations are and how to set them during database creation via the Fabric REST API and other deployment methods.<!--excerpt_end-->

# Announcing Full Collation Support for SQL Databases in Microsoft Fabric (Preview)

We heard from you and are introducing a major update for SQL database in Microsoft Fabric. Now, you can use **all Azure SQL database collations** when creating databases. This enhancement gives SQL database in Fabric users greater flexibility and compatibility for global data scenarios, reporting, and app development—no matter your language or regional requirements.

## What are collations?

Collations define how SQL database sorts and compare text data in your database. They affect everything from filtering results to running searches and managing multilingual content.

With full collation support, you can now tailor your database to match regional or application-specific needs in Fabric.

## How to use collations in Fabric

When creating a new SQL database in Fabric, specify your preferred collation in the creation payload through the REST API.

- No extra steps are needed.
- This capability is available across deployment methods and API versions.

Demo and samples:

- Demo video: https://www.youtube.com/watch?v=YiMmhcYm5mk
- Sample notebook: https://github.com/Microsoft/fabric-toolbox/tree/main/samples/notebook-create-sql-database

## Important behavior note: SQL analytics endpoint

The Fabric SQL Database collation does **not** affect the collation of the replicated data in the SQL analytics endpoint.

For how to change the collation of the SQL analytics endpoint, see:

- https://learn.microsoft.com/fabric/data-warehouse/collation

## Why it matters

- **Global readiness:** Adapt your database for any language or region.
- **Migration simplicity:** Move apps and workloads with unique collation needs more easily.
- **Compatibility:** Reduce friction when connecting external apps or services.

## Key resources and documentation

- Using PowerShell
- Create a SQL database with the REST API for Microsoft Fabric: https://learn.microsoft.com/fabric/database/sql/deploy-rest-api?tabs=7dot4
- REST API reference (creation payload): https://learn.microsoft.com/rest/api/fabric/sqldatabase/items/create-sql-database?tabs=HTTP#newsqldatabasecreationpayload
- Fabric CLI (database collation): https://learn.microsoft.com/fabric/database/sql/deploy-cli#database-collation

## Get started

Explore the documentation and demo to try out new deployments and make your next database project truly global.

**Have feedback?** Share your thoughts with the Fabric Community or in the comment section.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/announcing-full-collation-support-for-sql-databases-in-microsoft-fabric-preview/)

