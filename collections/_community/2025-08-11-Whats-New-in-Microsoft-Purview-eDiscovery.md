---
external_url: https://techcommunity.microsoft.com/t5/microsoft-security-community/what-s-new-in-microsoft-purview-ediscovery/ba-p/4441676
title: What’s New in Microsoft Purview eDiscovery
author: Aaron_Thorp
feed_name: Microsoft Tech Community
date: 2025-08-11 16:00:00 +00:00
tags:
- Audit Readiness
- Case Management
- Compliance
- Compliance Boundary
- Condition Builder
- Data Sources
- Decryption Settings
- Ediscovery
- Export Controls
- Legal Hold
- Logical Operators
- Microsoft 365 Compliance
- Microsoft Purview
- Modern Ediscovery
- Premium Features
- Reporting
- Search Duplication
section_names:
- security
---
Aaron_Thorp introduces the newest features and workflow enhancements in Microsoft Purview eDiscovery, aimed at streamlining case management and strengthening compliance and security for organizations.<!--excerpt_end-->

# What’s New in Microsoft Purview eDiscovery

## Overview

Microsoft Purview eDiscovery continues to evolve to support the complex needs of organizations facing compliance and legal challenges. The latest updates bring significant improvements in case management, workflow efficiency, and security, helping legal and compliance teams handle cases with greater precision and less friction.

## Enhanced Reporting in Modern eDiscovery

- **Comprehensive Audit Trails**: All case actions are now recorded, supporting defensibility and audit readiness.
- **Compliance Boundary Visibility**: Summary.csv includes boundary settings, clarifying search context based on user permissions.
- **Decryption Settings Reporting**: Settings.csv now indicates if Exchange or SharePoint decryption capabilities are enabled.
- **Premium Feature Usage**: Reporting on Premium features usage is now available for compliance checks.
- **Item Identification Methods**: Items.csv distinguishes between direct, partial, and advanced indexing.
- **Export Context**: Summary.csv offers explanations of data volume discrepancies, accounting for cloud attachments or document versions.

## Workflow Enhancements

### Copy Search to Hold

Users can quickly create a hold from an existing search, reducing workflow duplication and supporting legal holds in scenarios such as litigation.

### Retry Failed Locations

Automatically retry search locations that were temporarily inaccessible without having to restart the entire job. The results aggregate with the original job for continuity.

### Duplicate Search

Easily duplicate existing searches, carrying over up all parameters and conditions, to save time and maintain consistency throughout investigations.

### Case-Level Data Source Management

A new Data Sources tab permits centralized management of all data sources for a case (mailboxes, SharePoint, Teams), streamlining frequent searches and legal hold workflows.

### Delete Searches and Exports

Clean up outdated or redundant searches and exports, keeping case data organized and secure. This also aids in removing unneeded sensitive information, minimizing clutter and risk.

## Advanced Search Tools

- **Condition Builder Enhancement**: Logical operators (AND, OR, NEAR) are now supported in the same line, enabling richer and more targeted search conditions.

## Premium and Export Controls

- **Tenant-Level Control**: Admins can set default behaviors for Premium features by tenant, offering better management in mixed-license environments.
- **Export Naming and Sizing**: Export packages can now include user-defined names, with administrators able to define maximum export package sizes for performance optimization and easier data tracking.

## Conclusion

These updates represent Microsoft Purview's commitment to a modernized, intuitive user experience in eDiscovery. Enhanced reporting, streamlined search and hold workflows, better data source management, and new export controls provide compliance teams with the tools needed for more effective case management and risk mitigation.

For detailed documentation and advanced guides:

- [eDiscovery Documentation](https://aka.ms/ediscoverydocsnew)
- [eDiscovery Product Roadmap](https://aka.ms/ediscoveryroadmap)
- [Purview eDiscovery Ninja Guide](https://aka.ms/ediscoveryninja)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-security-community/what-s-new-in-microsoft-purview-ediscovery/ba-p/4441676)
