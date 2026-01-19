---
external_url: https://blog.fabric.microsoft.com/en-US/blog/onelake-file-explorer-smarter-more-reliable-and-seamlessly-integrated/
title: 'OneLake File Explorer 1.0.14.0: Enhanced Integration with Microsoft Fabric'
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-09-10 09:00:00 +00:00
tags:
- .NET 8
- Data Engineering
- Data Upload
- Data Workflow
- Diagnostic Logging
- File Management
- Lakehouse
- Lakehouse Integration
- Microsoft Ecosystem
- Microsoft Fabric
- OneLake File Explorer
- Sync Reliability
- Windows Integration
- Workspace Management
section_names:
- azure
- ml
---
Microsoft Fabric Blog announces OneLake File Explorer version 1.0.14.0, featuring smarter sync, better stability, and .NET 8 migration for enhanced data workflow integration.<!--excerpt_end-->

# OneLake File Explorer 1.0.14.0: Enhanced Integration with Microsoft Fabric

**Version 1.0.14.0** of OneLake File Explorer brings users powerful updates for managing data workflows seamlessly within the Microsoft Fabric environment.

## What is OneLake File Explorer?

OneLake File Explorer is designed to connect your local Windows file system with Microsoft Fabric, enabling users to manage OneLake workspaces and artifacts directly from Windows Explorer. This extension bridges the gap between on-premises file management and cloud data analytics, making complex data workflows more accessible.

## Key Capabilities

- **Directly browse and manage OneLake workspaces/items** from Windows file explorer
- **Upload data** with drag-and-drop simplicity to Lakehouses, Warehouses, and other Fabric resources
- **Right-click integration** for instant access to Fabric web portal features, including launching Lakehouses in Fabric Notebooks
- **Multi-account sign-in** support for flexibility in authentication
- **Diagnostic logging** for easier troubleshooting and understanding sync behaviors

## What’s New in Version 1.0.14.0?

### Smarter Sync for Temporary Files

Temporary files (such as those with a `.tmp` extension, often created during edits in Excel) will no longer linger in a sync-pending state. The new version cleans up these files automatically once connectivity is restored, eliminating persistent sync issues and server conflicts.

### Enhanced Stability

A memory access violation bug, which caused the app to crash during some sync processes (especially with files that hadn't been previously opened), has been resolved. This improves the reliability of background operations.

### .NET 8 Migration

The application now runs on .NET 8, ensuring long-term support, better security, and compatibility with current Microsoft frameworks.

## Why Update?

With these improvements, OneLake File Explorer offers a smoother, smarter, and more secure environment for Microsoft Fabric and Lakehouse data workflows.

- More stable data synchronization
- Simplified experience for data engineers and analysts
- Ongoing compatibility with Microsoft’s evolving ecosystem

[Get the latest OneLake File Explorer version](https://www.microsoft.com/download/details.aspx?id=105222)

For further details, visit the [original announcement](https://blog.fabric.microsoft.com/en-us/blog/onelake-file-explorer-smarter-more-reliable-and-seamlessly-integrated/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/onelake-file-explorer-smarter-more-reliable-and-seamlessly-integrated/)
