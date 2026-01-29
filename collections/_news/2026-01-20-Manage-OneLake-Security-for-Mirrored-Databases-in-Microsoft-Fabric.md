---
external_url: https://blog.fabric.microsoft.com/en-US/blog/manage-onelake-security-for-mirrored-databases-preview/
title: Manage OneLake Security for Mirrored Databases in Microsoft Fabric
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2026-01-20 11:00:00 +00:00
tags:
- Access Management
- Analytics Platform
- Azure SQL
- Data Access Roles
- Data Governance
- Data Lake
- Data Replication
- Data Security
- Enterprise Security
- Microsoft Fabric
- Microsoft Learn
- Mirrored Databases
- OneLake
- Role Based Access Control
- Shortcuts
- Azure
- Machine Learning
- Security
- News
section_names:
- azure
- ml
- security
primary_section: ml
---
Microsoft Fabric Blog explains how OneLake now allows fine-grained data access roles for Mirrored Databases. This upgrade helps organizations define roles at the table or folder level, enabling secure, efficient collaboration and governance across teams.<!--excerpt_end-->

# Manage OneLake Security for Mirrored Databases in Microsoft Fabric

Microsoft Fabric's OneLake now supports defining data access roles on all Mirrored item types, introducing granular, role-based access control for replicated data from transactional sources.

## Key Highlights

- **Granular Access Control**: Fine-grained roles can be set at the table or folder level for Mirrored items in OneLake, expanding security beyond traditional lakehouses.
- **Consistent Security Model**: The same OneLake access experience is extended, ensuring that permissions assigned are honored everywhere the mirrored data exists—even when shared as shortcuts.
- **Organization-wide Collaboration**: Teams can now safely reuse and share mirrored data without duplicating or re-securing it, reducing governance overhead and data sprawl.
- **Supported Artifacts**: All Mirrored item types are eligible for security role assignment as part of this release.
- **Enforcement at Time of Access**: Security roles are evaluated dynamically as users query data, so users see only the data they are authorized for.

![Screenshot of OneLake security management UI for a Mirrored Database in Azure SQL](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/01/outrostill-1024x576.png)

## How it Works

1. From the Mirrored item interface in Fabric, enable OneLake data access roles.
2. Define custom roles to grant access to specific tables or folders.
3. Assign users or groups to these roles to control access.
4. Shortcuts and data consumption patterns respect these role definitions automatically.

## Benefits

- **Minimized Data Duplication**: Teams mirror once, control access precisely, and avoid redundant datasets.
- **Simplified Governance**: Security is set at the OneLake layer, providing consistent enforcement regardless of how the data is used.
- **Smooth Transition**: Opt-in model ensures existing mirrored workloads are not interrupted while moving to the new access paradigm.

For full details and setup instructions, visit the [OneLake security documentation on Microsoft Learn.](https://learn.microsoft.com/fabric/onelake/security/get-started-onelake-security)

## Learn More

- [Mirroring in Fabric Overview](https://learn.microsoft.com/fabric/mirroring/overview)
- [Manage OneLake Security for Mirrored Databases (Preview)](https://blog.fabric.microsoft.com/en-us/blog/manage-onelake-security-for-mirrored-databases-preview/)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/manage-onelake-security-for-mirrored-databases-preview/)
