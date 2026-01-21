---
external_url: https://blog.fabric.microsoft.com/en-US/blog/extending-outbound-access-protection-to-fabric-warehouse-and-sql-analytics-endpoint/
title: Extending Outbound Access Protection to Fabric Warehouse and SQL Analytics Endpoint
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-13 09:00:00 +00:00
tags:
- Analytics Security
- Cloud Analytics
- COPY INTO
- Data Exfiltration Prevention
- Data Governance
- Data Security
- Fabric Warehouse
- Governance
- Microsoft Fabric
- OPENROWSET
- Outbound Access Protection
- Role Based Access Control
- SQL Analytics Endpoint
- Workspace Protection
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog explains how Outbound Access Protection is now available for Fabric Warehouse and SQL Analytics Endpoint, enabling organizations to secure analytics workloads by restricting outbound connections to only trusted destinations.<!--excerpt_end-->

# Extending Outbound Access Protection to Fabric Warehouse and SQL Analytics Endpoint

With the release of Outbound Access Protection (OAP) for Fabric workspaces, customers now have a robust tool for limiting where outbound connections can be established. This helps ensure that data loads and queries in a workspace only target trusted endpoints, reducing potential exposure and supporting better governance.

## OAP Now Covers Fabric Warehouse and SQL Analytics Endpoint

- **Consistency**: Outbound protections applied to Spark workloads are now extended to Fabric Warehouse (Preview) and SQL Analytics Endpoint (GA), guaranteeing all workspace analytics workloads are secured equally.
- **Governance at Scale**: OAP is enforced at the workspace level, so every process and artifact abides by the same access rules.
- **Stronger Security Posture**: Operations like `COPY INTO` and `OPENROWSET` are restricted to trusted sources, locking down possible routes for unintentional or malicious data exfiltration.

## Outbound Connection Boundaries

OAP works like a firewall for your Fabric workspace's outbound data connections. Connections are only allowed to approved destinations; unapproved destinations are blocked automatically. Once enabled at the workspace level, all workloads adhere to these boundaries.

## Addressing Security Gaps in Data Loads

Data loads are sensitive operations; without protection, operations like `COPY INTO` or `OPENROWSET` could expose your environment to untrusted or compromised data sources. OAP directly addresses this risk by ensuring only pre-approved endpoints can be used, preventing data movement to unauthorized locations in real time.

## How to Enable OAP

1. Enable OAP at the workspace level within Microsoft Fabric.
2. Upon activation, Fabric enforces outbound connection boundaries automatically for all covered workloads.

Currently, outbound connections for Warehouse and SQL Endpoint are limited to their own workspace. Upcoming releases will introduce allow lists for trusted external destinations outside the workspace.

## Benefits for Fabric Warehouse and SQL Endpoint

- **Fabric Warehouse**: Ingestion pipelines only accept data from trusted sources, supporting enterprise governance policies.
- **SQL Endpoint**: Additional protection is applied to queries pulling data from external systems.

This provides confidence that any data movement or query in your analytics estate only draws from, or pushes to, authorized locations.

## Advancing Secure Analytics in Fabric

OAP is one part of a comprehensive security foundation in Fabric, working together with features such as encryption, auditing, and role-based access control. Built directly into the service, it's designed for simplicity and effectiveness.

## Additional Resources

- [OneLake as a Source for COPY INTO and OPENROWSET (Preview)](https://blog.fabric.microsoft.com/blog/announcing-public-preview-onelake-as-a-source-for-copy-into-and-openrowset/)
- [COPY INTO (Transact-SQL) Documentation](https://learn.microsoft.com/sql/t-sql/statements/copy-into-transact-sql?view=fabric)
- [Browse File Content Before Ingestion with the OPENROWSET function](https://learn.microsoft.com/fabric/data-warehouse/browse-file-content-with-openrowset)
- [Workspace Outbound Access Protection Overview](https://learn.microsoft.com/fabric/security/workspace-outbound-access-protection-overview)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/extending-outbound-access-protection-to-fabric-warehouse-and-sql-analytics-endpoint/)
