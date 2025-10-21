---
layout: "post"
title: "SSMS 22 Meets Fabric Data Warehouse: Evolving the Developer Experiences"
description: "This article details the new integration between Microsoft Fabric Data Warehouse and SQL Server Management Studio (SSMS) 22, focusing on enhanced developer workflows. It introduces features such as schema-based object grouping, warehouse-centric views, improved context menus, and direct querying of snapshots. The update aligns web and desktop experiences, aiming for productivity and continuity for SQL developers."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/ssms-22-meets-fabric-data-warehouse-evolving-the-developer-experiences/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-21 10:00:00 +00:00
permalink: "/2025-10-21-SSMS-22-Meets-Fabric-Data-Warehouse-Evolving-the-Developer-Experiences.html"
categories: ["Azure", "Coding", "ML"]
tags: ["Azure", "Azure Data Platform", "Coding", "Data Warehouse", "Database Projects", "Developer Tools", "Fabric Web Editor", "IntelliSense", "Microsoft Fabric", "ML", "News", "Query Management", "Schema Grouping", "SQL Analytics Endpoint", "SQL Server Management Studio", "SSMS 22", "T SQL", "Warehouse Snapshots"]
tags_normalized: ["azure", "azure data platform", "coding", "data warehouse", "database projects", "developer tools", "fabric web editor", "intellisense", "microsoft fabric", "ml", "news", "query management", "schema grouping", "sql analytics endpoint", "sql server management studio", "ssms 22", "t sql", "warehouse snapshots"]
---

Microsoft Fabric Blog presents a comprehensive overview of the SSMS 22 integration with Fabric Data Warehouse, highlighting new features that enhance SQL development workflows and align cross-platform experiences for developers.<!--excerpt_end-->

# SSMS 22 Meets Fabric Data Warehouse: Evolving the Developer Experiences

## Why this Investment Matters

SQL Server Management Studio (SSMS) has long been the standard for SQL developers, with millions of users worldwide. However, integration with Fabric Data Warehouse was limited—creating friction between web and desktop workflows. With SSMS 22, Fabric Warehouse delivers deep integration, aiming for a unified and consistent experience for developers.

## What’s New in SSMS 22 for Fabric Warehouse?

- **Friendly Connection Names**: Instead of cryptic server names, SSMS 22 displays readable workspace names in the object explorer, making environment identification easier, especially for those working with multiple workspaces.

- **Schema-Based Object Grouping**: Tables, views, and stored procedures are now grouped by schema, mirroring the Fabric Web editor. This logical organization improves navigation and accelerates development for complex data models.

- **Warehouse-Centric Views**: Warehouses and SQL Analytics Endpoints are now primary objects in SSMS, with warehouse snapshots visible and directly queryable—enabling point-in-time recovery and audits.

- **Full T-SQL Experience**: The T-SQL editor continues to support familiar features like IntelliSense, query execution plans, and scripting, preserving productivity while introducing enhancements.

- **Cleaner Context Menus**: Context-aware menus now present only relevant actions for Fabric Warehouse, reducing menu clutter and minimizing confusion.

## Why this is Important

These updates reflect a developer-first commitment:

- Reduced workflow friction between Fabric and SSMS
- Consistent query and schema management across tools
- Foundations laid for future features like version control and database projects within SSMS

## What’s Next?

Upcoming investments include:

- Direct discovery and connection to Warehouses and SQL Analytics Endpoints without manual searching
- Expanded context menu actions (e.g., Extract DacPac, Generate Scripts, Refresh Snapshot)
- Script generation tailored to the Fabric engine edition
- Full warehouse lifecycle management (creation/deletion) from SSMS

These changes are part of an ongoing strategy to empower developers with seamless, cloud-powered analytics development inside their trusted tools.

## Conclusion

The collaboration between Fabric and the SSMS team is closing the gap between cloud innovation and classic developer tools. With continuous feedback from the community, Fabric Warehouse in SSMS 22 represents only the first step toward a more powerful, developer-centric analytics platform.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/ssms-22-meets-fabric-data-warehouse-evolving-the-developer-experiences/)
