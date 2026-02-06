---
external_url: https://blog.fabric.microsoft.com/en-US/blog/unlocking-enterprise-ready-sql-database-in-microsoft-fabric-auditing-backup-copilot-more/
title: 'Unlocking Enterprise-Ready SQL Database Features in Microsoft Fabric: ALM, Backups, and Copilot Enhancements'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-23 13:00:00 +00:00
tags:
- ALM
- Application Lifecycle Management
- Copilot
- Data Platform
- Database Recovery
- Developer Productivity
- Microsoft Entra ID
- Microsoft Fabric
- Performance Dashboard
- Point in Time Restore
- Query Management
- Replication
- REST API
- SQL Auditing
- SQL Database
- VS Code Integration
- AI
- Azure
- ML
- Security
- News
- .NET
- Machine Learning
section_names:
- ai
- azure
- dotnet
- ml
- security
primary_section: ai
---
The Microsoft Fabric Blog, with Joanne Wong as coauthor, details critical updates to SQL Database in Microsoft Fabric, highlighting Copilot features, ALM APIs, advanced security options, VS Code integrations, and new enterprise capabilities for developers.<!--excerpt_end-->

# Unlocking Enterprise-Ready SQL Database Features in Microsoft Fabric: ALM, Backups, and Copilot Enhancements

Coauthored by: Joanne Wong

## Introduction

Microsoft Fabric continues to evolve as a unified data platform. The most recent updates to SQL Database in Fabric transform it into a fully managed SaaS offering, introducing features that prioritize security, developer flexibility, and productivity. This article summarizes the most impactful enhancements.

---

## Copilot & Query Editor Enhancements

- **SSMS One-Click Connection**: Instantly connect to SQL Database in Fabric via both VS Code integration and SQL Server Management Studio, reducing setup friction for developers and data professionals.
- **Bulk Query Management**: Easily manage and delete multiple queries within the workspace using intuitive selection and actions.
- **Shared Queries**: Support for collaborative query editing and sharing across workspace teams.
- **Copilot Mode Selector**: Toggle quickly between 'read-only' mode (Copilot suggests SQL but does not execute) and 'read/write' mode (Copilot can execute code after user approval), enhancing safe automation and governance.

---

## Application Lifecycle Management (ALM)

- **Database Definition Import/Export via REST API**: Developers can now export database object definitions as portable SQL projects and import compiled definitions (e.g., dacpacs) with automated diff detection and application of changes.

---

## VS Code Integration

- **MSSQL Extension for VS Code**: Seamless connectivity between VS Code and Fabric SQL databases via the official MSSQL extension. Browse, authenticate, and connect with Microsoft Entra ID-based persistent sign-in.
- **Provisioning Workflows**: Create and connect Fabric SQL databases directly from VS Code in under three minutes, supporting rapid development and prototyping.

**Additional Resources**:

- [MSSQL extension for VS Code: Fabric integration (preview)](https://aka.ms/vscode-mssql-fabric)
- [Fabric Integration in Visual Studio Code with MSSQL – SQL Server | Microsoft Learn](https://learn.microsoft.com/en-us/sql/tools/visual-studio-code-extensions/mssql/mssql-fabric-integration?view=sql-server-ver17)

---

## Point-in-Time Restore (PITR)

- **Granular Database Recovery**: Restore any Fabric SQL database to a specific time within either a 7-day or (now) 35-day retention window. Supports recovery from errors, bugs, or security incidents.

---

## Performance Dashboard Improvements

- **Memory Consumption Metrics**: Real-time monitoring of memory usage by individual queries, alongside existing metrics like CPU utilization, user connections, requests per second, blocked queries, database size, automatic index info, and overall performance.
- [Performance Dashboard documentation](https://learn.microsoft.com/fabric/database/sql/performance-dashboard)

---

## Mirroring/Replication Configurability

- **Programmatic Replication Control**: Teams can start or stop data replication/mirroring to Fabric OneLake via REST APIs, supporting automated pipeline management and operational resilience.

---

## Upcoming: SQL Auditing

- **Enterprise Compliance and Security**: Built-in auditing logs all critical events by default (logins, schema changes, permissions) in an immutable OneLake folder.
- **Custom Auditing Configurations**: Fine-grained audit options allow organizations to tailor events, retention, and predicates for compliance.

---

## Getting Started and Community Resources

- [Microsoft Fabric SQL database documentation](https://learn.microsoft.com/en-us/sql/sql-server/fabric-database/sql-database-in-fabric?view=azuresqldb-current)
- [Video: Deploy SQL databases in Fabric](https://youtu.be/ycq7r-ngOBI)
- [Join the Fabric community](http://ideas.fabric.microsoft.com)
- [Microsoft Fabric Roadmap](https://aka.ms/fabricsqlroadmap)
- [End-to-end tutorials](http://aka.ms/Fabric-tutorials)

## Conclusion

The capabilities highlighted in this update make SQL Database in Microsoft Fabric a robust, secure, and modern platform for developers and enterprises. Whether focusing on application lifecycle management, security, performance, or developer experience, these features support mission-critical workloads and compliance at scale.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/unlocking-enterprise-ready-sql-database-in-microsoft-fabric-auditing-backup-copilot-more/)
