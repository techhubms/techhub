---
tags:
- Apache Iceberg
- API Integration
- Authorized Engines
- Column Level Security (cls)
- Data Governance
- Data Lake
- Delta Lake
- Distributed Enforcement
- Metadata APIs
- Microsoft Fabric
- ML
- News
- OneLake
- OneLake Security
- Policy Definition
- Query Engine Integration
- Role Based Access Control (rbac)
- Row Level Security (rls)
- Security
- Security Interoperability
- Table Permissions
section_names:
- ml
- security
title: Third-party support for OneLake security
external_url: https://blog.fabric.microsoft.com/en-US/blog/third-party-support-for-onelake-security/
date: 2026-03-19 14:00:00 +00:00
feed_name: Microsoft Fabric Blog
author: Microsoft Fabric Blog
primary_section: ml
---

Microsoft Fabric Blog announces new OneLake security APIs that let third-party query engines enforce Fabric/OneLake permissions (including RLS and CLS) at query time, so security can be defined once and applied consistently wherever the data is read.<!--excerpt_end-->

# Third-party support for OneLake security

As modern data lakes standardize on open formats and table layers like Delta and Iceberg, organizations want to use different analytics engines without copying data or re-implementing security in each tool. The core requirement is **define security once and enforce it consistently everywhere data is consumed**.

This update introduces **API support for third-party enforcement** of OneLake security through an **authorized engine model**, extending the same enforcement approach used by Microsoft Fabric workloads to external engines.

## Authorized engines

OneLake security is built on **centralized policy definition** with **controlled, distributed enforcement**:

- Security policies are authored and stored once in OneLake, including:
  - Table permissions
  - **Row-level security (RLS)**
  - **Column-level security (CLS)**
- Enforcement happens **at query time** inside the engine reading the data.

### How the authorized engine approach works

- An authorized engine is granted **scoped access** to OneLake data and security definitions via **OneLake APIs**.
- When the engine receives a query, it fetches the required data and security definitions from OneLake and **enforces the security rules during query execution**.
- **OneLake remains the source of truth** for access control.
- The engine retains control over query optimization and execution.

This model already exists for Microsoft Fabric engines and now extends to third-party engines, with the same guarantee: users querying via an authorized third-party engine should only see the rows/columns permitted by OneLake policies.

## Implementation guidance

Two docs were published alongside the APIs:

- Implementation guide for building an integration:
  - [OneLake Security Integration Docs](https://aka.ms/OneLakeSecurityIntegrationDocs)
- User-facing setup guide for configuring an authorized engine in Fabric:
  - [Getting started guide](https://aka.ms/OneLakeSecurity3PDocsGettingStarted)

### API design notes

- The APIs are intended to be **engine-agnostic**.
- OneLake **pre-computes a user's effective access** across their OneLake security roles.
- Engines receive a **ready-to-apply security definition**.

The post also notes upcoming evolution, including potential support for using **bitmaps** to implement RLS filtering.

## Next steps

With authorized engine support:

- Data vendors can integrate directly with OneLake security for consistent enforcement.
- Fabric administrators can keep a single source of truth for RLS, CLS, and role-based permissions.
- Business users can use their preferred query engine against OneLake while keeping permissions consistent.

If you build or operate a query engine that reads from OneLake, the post recommends reviewing the new APIs and starting integration planning. For more background, see the OneLake security whitepaper:

- [The future of data security is interoperability (OneLake security whitepaper)](https://blog.fabric.microsoft.com/blog/the-future-of-data-security-is-interoperability-a-technical-look-at-onelake-security?ft=All)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/third-party-support-for-onelake-security/)

