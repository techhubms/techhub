---
layout: "post"
title: "Streamlining Data Management: New Collation Settings in Microsoft Fabric Warehouse and SQL Endpoint"
description: "Microsoft Fabric introduces support for both Case Sensitive (CS) and Case Insensitive (CI) collations in Warehouses and SQL Endpoints. This update enables workspace-level and item-level collation configuration, enhancing data consistency and flexibility, with upcoming features aimed at further personalization and control of collation settings."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/streamlining-data-management-with-collation-settings-in-microsoft-fabric-warehouse-sql-analytics-endpoint/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-08-05 10:30:00 +00:00
permalink: "/news/2025-08-05-Streamlining-Data-Management-New-Collation-Settings-in-Microsoft-Fabric-Warehouse-and-SQL-Endpoint.html"
categories: ["Azure", "ML"]
tags: ["Azure", "Case Insensitive", "Case Sensitive", "Collation", "Data Management", "Data Workflows", "Item Level Configuration", "Lakehouse", "Microsoft Fabric", "Mirrored Database", "ML", "News", "SQL Endpoint", "Warehouse", "Workspace Level Settings"]
tags_normalized: ["azure", "case insensitive", "case sensitive", "collation", "data management", "data workflows", "item level configuration", "lakehouse", "microsoft fabric", "mirrored database", "ml", "news", "sql endpoint", "warehouse", "workspace level settings"]
---

Authored by the Microsoft Fabric Blog, this article explores the upcoming enhancements to collation settings in Microsoft Fabric Warehouse and SQL Endpoint, offering practical insights into new configuration options and future capabilities.<!--excerpt_end-->

## Streamlining Data Management with Collation Settings in Microsoft Fabric Warehouse and SQL Endpoint

*By Microsoft Fabric Blog*

Microsoft Fabric’s Warehouse and SQL Endpoint now support both **Case Sensitive (CS)** and **Case Insensitive (CI)** collations, providing users with essential tools for ensuring data consistency and operational flexibility. This update is significant for organizations that demand precise control over how textual data is stored, compared, and queried in their data platforms.

### Workspace-Level Collation: Driving Consistency

Workspace-level collation settings enable organizations to apply a standard collation configuration across all Warehouses and SQL Endpoints within a given Microsoft Fabric workspace. This standardization supports seamless integration and predictable query behavior.

#### Key Features

- **Default Settings**:
  - Newly created and existing workspaces default to **Case Sensitive (CS)** collation, remaining consistent with existing defaults.
  - Older workspaces retain their existing collations for backward compatibility, minimizing disruptions to current data environments.
- **User Control**:
  - Workspace admins, members, and contributors have the flexibility to modify collation settings through the workspace settings panel. Adjustments affect only new artifacts.
  - Viewers can observe, but not change, collation configurations.
  - During the creation of specific Warehouses or SQL Endpoints, users may override the workspace-default collation for particular scenarios.
- **SQL Endpoint Behavior**:
  - SQL Endpoints—associated with artifacts like Lakehouse, Mirrored Database, or SQL Database within Fabric—inherit the workspace collation unless specified otherwise.
  - For SQL Databases in existing workspaces, **Case Insensitive (CI)** collation applies until support is provided for independent collation settings.

> **Note:** Changing a workspace's collation does not retroactively affect existing artifacts, but may impact query performance and consistency across Warehouses and SQL Endpoints going forward.

### Enhancements: Upcoming Item-Level Collation Flexibility

Looking ahead, Microsoft Fabric will introduce more granular **item-level collation settings**, offering organizations and teams greater adaptability for specific use cases. These improvements aim to streamline migrations, cater to diverse data sources, and align with precise application requirements.

#### What’s Coming Next

- **Flexible Creation-Time Options**:
  - Users will continue to select **CS** or **CI** collation at the creation of individual Warehouses or SQL Endpoints (e.g., for Lakehouse or Mirrored Databases).
  - Enhanced visual indicators in the UI will make it easier to see and understand when workspace defaults are overridden.
- **Restoration and Snapshots**:
  - Restored Warehouses will keep their original collation settings, regardless of any changes at the workspace level.
  - Snapshots inherit the collation of the originating Warehouse.
- **Shortcut Handling**:
  - Lakehouse shortcuts will consistently inherit the SQL Endpoint's collation.
  - Future improvements will focus on increased transparency and management for shortcut collation, helping maintain pipeline and data flow consistency.

These features are designed to make configuring and managing collation settings more intuitive and powerful, enabling data professionals to tailor their environments with the right balance between consistency and point-in-time flexibility.

#### Further Resources

For details on configuring Warehouses with case-insensitive collation, consult the [official Microsoft documentation](https://learn.microsoft.com/fabric/data-warehouse/collation).

### Conclusion

The expanded collation options in Microsoft Fabric for Warehouses and SQL Endpoints create a more robust framework for data consistency and operational flexibility. With workspace-level defaults, item-level overrides, and upcoming UI enhancements, users can:

- Simplify complex data workflows
- Streamline migrations
- Optimize new data-driven applications
- Maintain alignment with organizational or project standards

These updates reinforce Microsoft Fabric’s commitment to providing a modern, adaptable platform for data management and analytics.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/streamlining-data-management-with-collation-settings-in-microsoft-fabric-warehouse-sql-analytics-endpoint/)
