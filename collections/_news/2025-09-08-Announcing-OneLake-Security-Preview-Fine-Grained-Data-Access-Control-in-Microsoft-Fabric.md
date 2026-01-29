---
external_url: https://blog.fabric.microsoft.com/en-US/blog/onelake-security-is-now-available-in-public-preview/
title: 'Announcing OneLake Security (Preview): Fine-Grained Data Access Control in Microsoft Fabric'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-08 08:00:00 +00:00
tags:
- Access Control
- API
- CLS
- Column Level Security
- Data Democratization
- Data Governance
- Data Lake Security
- Fabric Lakehouse
- Microsoft Fabric
- OneLake
- Power BI
- RLS
- Role Based Access
- Row Level Security
- Security Roles
- Shortcuts
- Spark
- SQL Analytics Endpoint
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
Microsoft Fabric Blog introduces the public preview of OneLake security, highlighting author-driven best practices for securing data lakes with advanced, unified access controls across Fabric workloads.<!--excerpt_end-->

# OneLake Security (Preview) in Microsoft Fabric

## Overview

OneLake security, now in public preview, delivers unified, fine-grained access control for data stored within the Microsoft Fabric platform. This enables organizations to manage permissions and protect sensitive information across multiple analytics engines (Power BI, Spark, Lakehouse, SQL Analytics) from a single, consistent security definition.

## Key Features

- **Fine-Grained Security:** Create security roles defining allowed folders, tables, rows, and columns. Assign permissions so users only access authorized data—ideal for controlling exposure of sensitive information (e.g., PII).
- **Unified Model:** Security definitions are managed once and enforced across all access points in Fabric (works outside Fabric as well). No need for duplicated definitions in each service.
- **Integration with Shortcuts:** Securely share data across workspaces and teams using shortcuts without copying data or weakening controls; access obeys security restrictions.
- **Flexible Management:** Manage security roles via user interface or APIs ([API documentation](https://learn.microsoft.com/rest/api/fabric/core/onelake-data-access-security)).
- **Automatic Migration:** Workspaces using former OneLake data access roles are automatically upgraded to OneLake security without required user action.

## Recent Improvements

### SQL Analytics Endpoint

- New UI for highlighting security sync errors and recommended resolution steps
- UI for identity/delegated mode clarity
- Faster, optimized backend sync of security changes

### Lakehouse and Spark Integration

- Automatic application of column-level security in object explorer
- Spark notebooks now support OneLake security for non-schema lakehouses
- Faster live pool startup for queries involving RLS (Row-Level Security) and CLS (Column-Level Security)
- Up to 4x performance improvement for security-sensitive queries

### Power BI

- Enhanced query performance for semantic models leveraging RLS

## Example Scenarios

- Data owners can grant access to business analysts for specific datasets while masking PII by configuring row or column security.
- Business analysts can consume data securely, even via shortcuts, without risking data leaks or needing duplicate copies.

## Getting Started

- OneLake security preview is available to all Fabric users (no sign-up required).
- [Documentation: OneLake Security](https://learn.microsoft.com/fabric/onelake/onelake-shortcut-security)
- [Watch the Demo](https://youtu.be/phFm4c7J8Ic)
- [Sign up for a free Microsoft Fabric trial](https://app.fabric.microsoft.com/)

---

OneLake security empowers Fabric users with transparent, modern controls for securing distributed data lakes while maintaining accessibility and data democratization across analytics environments.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/onelake-security-is-now-available-in-public-preview/)
