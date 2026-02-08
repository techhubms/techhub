---
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-outbound-access-protection-for-spark-preview/
title: Introducing Workspace Outbound Access Protection for Spark
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-03 10:04:16 +00:00
tags:
- Compliance
- Data Engineering
- Data Exfiltration
- Managed Private Endpoints
- Microsoft Fabric
- Network Security
- OAP
- Private Links
- Spark
- Trusted Workspace Access
- Workspace Outbound Access Protection
- Azure
- ML
- Security
- News
section_names:
- azure
- ml
- security
primary_section: ml
---
Microsoft Fabric Blog introduces Workspace Outbound Access Protection (OAP) for Spark, empowering admins to restrict outbound connectivity and prevent data exfiltration in managed workspaces.<!--excerpt_end-->

# Introducing Workspace Outbound Access Protection (OAP) for Spark in Microsoft Fabric

In today’s hyper-connected digital landscape, safeguarding sensitive data is more critical than ever. Microsoft Fabric provides a comprehensive suite of network security features, supporting both inbound and outbound connectivity for your data assets.

## Key New Release

**Workspace Outbound Access Protection (OAP)** is now available in public preview for Spark in Microsoft Fabric. OAP allows Workspace Admins to govern and restrict which external destinations Fabric workspaces can connect to, significantly enhancing data exfiltration prevention capabilities.

## Summary of Security Features

- **Private Links:** Enable secure, private connectivity between Fabric and your resources.
- **Trusted Workspace Access:** Manage which workspaces can trust or connect with each other.
- **Managed Private Endpoints (MPE):** Restrict connections from Fabric artifacts to only approved destinations.
- **Outbound Access Protection (OAP):** Granular outbound governance at the workspace level, preventing unauthorized data transfer.

## Why Outbound Access Protection?

Organizations depend on Microsoft Fabric to handle vast amounts of sensitive data. Exfiltration protection is increasingly essential. OAP puts admins “in the driver’s seat,” providing the ability to:

- Explicitly permit or deny outbound workspace traffic
- Enforce network boundaries for Spark jobs and notebooks
- Combine with inbound protection for comprehensive workspace security

## How OAP Works for Spark

- **Restricts outbound connections:** Only destinations with Managed Private Endpoints may be accessed from Spark Notebooks and Jobs.
- **Workspace-level controls:** Allow differential policies for different workspaces or environments (dev/test/prod).
- **Artifact creation restrictions:** When OAP is enabled, workspaces accept only supported data engineering artifacts (Spark Notebooks, Jobs, Lakehouse); others are blocked to ensure rule enforcement.

## Example Workflow

1. Admin enables OAP for a Fabric workspace.
2. Admin configures Managed Private Endpoints to specify permitted destinations.
3. Spark notebooks can only connect to those pre-approved endpoints.
4. Attempts to access public internet or unauthorized endpoints are blocked.
5. For additional security, inbound workspace protection (e.g., workspace-level private links) may also be enabled.

For detailed setup, supported artifacts, and limitations, see the [Workspace Outbound Access Protection overview](https://learn.microsoft.com/en-us/fabric/security/workspace-outbound-access-protection-overview).

## Compliance and Future Plans

- **Regulatory compliance:** Enforce strong boundaries for sensitive data, helping meet requirements for privacy and data residency.
- **Continuous improvements:** Expansion to other Fabric experiences is planned.

> *Your feedback helps shape Microsoft Fabric’s security capabilities. Share your input in the blog comments.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/introducing-workspace-outbound-access-protection-for-spark-preview/)
