---
external_url: https://blog.fabric.microsoft.com/en-US/blog/onelake-security-on-the-sql-analytics-endpoint/
title: OneLake Security on the SQL Analytics Endpoint
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-06 12:00:00 +00:00
tags:
- Access Control
- Column Level Security
- Data Governance
- Data Lakehouse
- Data Protection
- Microsoft Fabric
- OneLake Security
- Preview Feature
- RBAC
- Row Level Security
- Security Sync
- Shortcuts
- SQL Analytics Endpoint
- SQL Permissions
- Workspace Roles
- ML
- Security
- News
section_names:
- ml
- security
primary_section: ml
---
Microsoft Fabric Blog outlines how OneLake Security centralizes granular data access policies for Fabric via the SQL Analytics Endpoint, including RLS, CLS, and access mode configuration for secure enterprise analytics.<!--excerpt_end-->

# OneLake Security on the SQL Analytics Endpoint

## Overview

OneLake Security brings centralized, fine-grained access control to Microsoft Fabric's data architecture. It acts as a unified RBAC (Role-Based Access Control) and policy layer, applying security consistently across different computation engines.

Currently in preview and opt-in per Fabric data item, OneLake Security enables you to define roles at the table or folder level. You can then layer on policies such as Row-Level Security (RLS) and Column-Level Security (CLS) to control what users see across various Fabric experiences.

## SQL Analytics Endpoint Integration

When OneLake Security is enabled for a Lakehouse, policies are enforced on all data access—including the SQL Analytics Endpoint. This endpoint provides a read-only SQL surface on Lakehouse data. Policies defined in OneLake (roles, RLS, CLS) determine access either via the user's identity or the delegated (owner's) identity, depending on your configuration.

### Access Modes

- **User identity mode**: The signed-in user's identity is checked against OneLake policies. SQL table GRANT/REVOKE are ignored for tables; SQL permissions only apply to objects like views and procedures. This ensures consistent data access governance across Power BI, notebooks, Lakehouse, and SQL.
- **Delegated identity mode**: Here, the endpoint authenticates as the Lakehouse or workspace owner, and SQL-managed permissions (GRANT/REVOKE, RLS, CLS, DDM) are authoritative. Use this for more traditional DBA workflows or to manage access at scale outside OneLake role limits.

### Role Nuances

- Admin/Member/Contributor roles can bypass OneLake restrictions; to enforce policies, use Viewer or share read-only.
- DefaultReader role preserves legacy access and can be refined for least-privilege.

## Managing Shortcuts and Remote Data Ownership

Shortcuts allow inclusion of data from other Lakehouses or external sources. Security policies always follow the data source—shortcut queries enforce the originating OneLake roles and policies. Access mappings must be aligned between source and consumer items, and changes in shortcut targets require validation.

Tips:

- Keep documentation of role mappings for easier troubleshooting.
- Expect some propagation delay when shortcut targets change names/locations.

## Configuration Steps: Enabling and Managing Security

1. **Opt-in to OneLake Security** on each relevant Lakehouse. Adjust the DefaultReader and create more granular roles as needed.
2. In the **SQL Analytics Endpoint's Security** section, select the desired access mode (User identity or Delegated identity).
3. Plan for access model transitions carefully, as switching modes can drop existing SQL table roles or stop applying OneLake policies to table access.

## Differences Between User Identity and Delegated Identity Modes

- **Tables**: User identity mode — enforcement by OneLake; SQL table permissions are disabled. Delegated — SQL is authoritative.
- **Other objects (views, procs, functions)**: SQL permissions work in both modes.
- **RLS/CLS/DDM**: User identity mode enforces RLS/CLS from OneLake; Delegated handles these in SQL.
- **Workspace roles**: Policy application depends on assigned roles in User identity mode. In Delegated, SQL governs access directly.

## Under the Hood: Security Sync and Validation

- In User identity mode, a security sync service ensures SQL's security state matches OneLake's dynamic policies, translating RLS/CLS to SQL objects and validating shortcut targets.
- In Delegated mode, access depends on SQL-side permissions, and any misalignment between SQL grants and the owner's Lakehouse access will cause query failures.

## Key Takeaways

- Choose User identity mode for unified governance—define once, enforce everywhere.
- Use Delegated identity to retain traditional SQL control or leverage DBA tooling at scale.
- Comprehensive OneLake Security for Fabric Warehouse is coming soon.

## Resources

- [OneLake Security for SQL analytics endpoints (Preview)](https://learn.microsoft.com/fabric/onelake/sql-analytics-endpoint-onelake-security)
- [OneLake security overview](https://learn.microsoft.com/fabric/onelake/security/get-started-security)
- [The next evolution of OneLake security (Preview)](https://blog.fabric.microsoft.com/blog/the-next-evolution-of-onelake-security-enters-early-preview/)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/onelake-security-on-the-sql-analytics-endpoint/)
