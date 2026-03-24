---
date: 2026-03-24 15:30:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/34710/
tags:
- Authentication
- Azure
- Azure Private Link
- Block Public Internet Access
- Compliance
- Data APIs
- Fabric API For GraphQL
- Governance
- GraphQL
- Lakehouse
- Microsoft Backbone Network
- Microsoft Entra ID
- Microsoft Fabric
- Mirrored Database
- ML
- Network Isolation
- News
- Private Endpoints
- Saved Credentials
- Security
- Service Principal (spn)
- Single Sign On (sso)
- SQL Database
- Tenant Level Private Link
- Tenant Settings
- Warehouse
- Workspace Monitoring
title: Tenant level private link support for Microsoft Fabric API for GraphQL (Generally Available)
primary_section: ml
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
section_names:
- azure
- ml
- security
---

Microsoft Fabric Blog announces general availability of tenant-level Private Link for Microsoft Fabric API for GraphQL, explaining how private connectivity keeps GraphQL API traffic off the public internet, integrates with Entra ID authentication, and noting current limitations around workspace monitoring and SPN saved credentials.<!--excerpt_end-->

## Overview

Microsoft Fabric API for GraphQL now supports **tenant-level Private Link** (Generally Available). This allows organizations to access their Fabric GraphQL APIs over **private connectivity**, so traffic does **not** traverse the public internet.

> Note: The post also references Arun Ulag’s FabCon/SQLCon announcements roundup: “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform”.

## Secure your data access layer

Fabric API for GraphQL is positioned as a way for developers to query data across multiple Fabric-backed sources through a single API, including:

- Warehouses
- Lakehouses
- Mirrored databases
- SQL databases

With **Private Link** enabled, GraphQL API endpoints use private network connectivity similar to other Fabric services.

### What changes with Private Link

- API traffic flows through **Microsoft’s private backbone network** rather than the public internet.
- This adds network-level protection for sensitive data access.

## Why Private Link matters for API for GraphQL

- **Enhanced security**
  - Keeps GraphQL API requests on a secured network path.
  - Reduces exposure to public internet threats and lowers attack surface.
- **Regulatory compliance**
  - Supports industries requiring private connectivity for data access (healthcare, finance, government).
- **Simplified network architecture**
  - Reduces reliance on firewall rules, VPNs, or IP allowlists.
  - Enables governance via approved **private endpoints**.
- **Enterprise integration**
  - Fits environments already using **Azure Private Link** and virtual networks.

## Designed for enterprise workloads

The feature is intended for scenarios like:

- Internal dashboards
- Business intelligence applications
- Customer-facing analytics

When enabled at the tenant level, GraphQL APIs automatically participate in the secure network environment.

### Identity and access

The post calls out using **Microsoft Entra ID authentication**, along with existing security options such as:

- Single sign-on
- Saved credentials

## Getting started

- Enable Private Link via **Fabric tenant settings**.
- Once configured, **existing GraphQL APIs automatically** leverage private connectivity.
- You can keep using the same API endpoints and authentication patterns.

If your organization uses the tenant setting **“Block Public Internet Access”**, GraphQL APIs are described as working within that locked-down environment.

![Configuration page for enabling Azure Private Link within an organization, showing setup steps and required permissions.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-configuration-page-for-enabli.png)

*Figure: Enable tenant level setting for private link.*

## Current limitations

At launch, the post lists two limitations:

- **API monitoring dashboard and logging based on Workspace Monitoring is not currently supported.**
- **Service Principal (SPN) access with saved credentials is not currently supported.**
  - SPNs can be clients, but an SPN cannot create a saved credential for access between the API and the data source.

## Learn more

- [About Private Links for Secure Access to Fabric](https://learn.microsoft.com/fabric/security/security-private-links-overview)
- [What is Microsoft Fabric API for GraphQL?](https://learn.microsoft.com/fabric/data-engineering/api-graphql-overview)
- [Connect Applications to Fabric API for GraphQL](https://learn.microsoft.com/fabric/data-engineering/connect-apps-api-graphql)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/34710/)

