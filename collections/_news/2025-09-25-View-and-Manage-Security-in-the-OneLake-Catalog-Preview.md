---
external_url: https://blog.fabric.microsoft.com/en-US/blog/view-and-manage-security-in-the-onelake-catalog-now-in-preview/
title: View and Manage Security in the OneLake Catalog (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-25 15:00:00 +00:00
tags:
- Access Control
- Azure Data Platform
- Column Level Security
- Data Governance
- Fabric Governance
- Least Privilege
- Microsoft Fabric
- OneLake Catalog
- Permissions Management
- Role Management
- Row Level Security
- Secure Tab
- Security Roles
- Workspace Roles
section_names:
- azure
- security
---
The Microsoft Fabric Blog details how workspace owners and governance teams can use the Secure tab in the OneLake catalog to efficiently manage access and security roles. Author: Microsoft Fabric Blog.<!--excerpt_end-->

# View and Manage Security in the OneLake Catalog (Preview)

Microsoft Fabric has introduced the Secure tab in the OneLake catalog, a centralized platform for understanding and managing permissions across Fabric items. This feature streamlines access management for governance teams and workspace owners by consolidating user and role views to enable effective least-privilege enforcement.

## Key Features

### View Users

- Access the View users page in the Secure tab to investigate workspace roles and permissions.
- Use workspace filtering to focus on specific business domains or workspaces.
- Additional filters allow you to drill down to particular roles or user types for targeted reviews.
- The interface displays each user’s name along with a counter of their assigned roles across selected workspaces, assisting with quarterly access reviews and rapid response to permission queries.

### View Security Roles

- Explore OneLake security role definitions for different workspaces and item types in a single location.
- Example: Opening a test role for a lakehouse shows its permission settings (e.g., Read permission on a specific table).
- Adjust the scope by adding or removing paths, and update the role’s member list directly from the catalog.
- Support for Row-Level Security (RLS) and Column-Level Security (CLS) provides fine-grained access control within tables, enabling inline authoring and validation for consistent protection.
- All role management tasks, including creating or deleting security roles, are consolidated in the Secure tab’s interface.

## Get Started

- Navigate to the OneLake catalog in Microsoft Fabric.
- Select the Secure tab and begin reviewing or managing user access and security roles for your data estate.

By centralizing these controls, Microsoft Fabric empowers teams to tighten permissions and promote data security standards efficiently.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/view-and-manage-security-in-the-onelake-catalog-now-in-preview/)
