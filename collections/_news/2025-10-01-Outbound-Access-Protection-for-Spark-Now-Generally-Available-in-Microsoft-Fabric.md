---
external_url: https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-spark-is-now-generally-available/
title: Outbound Access Protection for Spark Now Generally Available in Microsoft Fabric
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-01 14:00:00 +00:00
tags:
- Cloud Security
- Connectivity Control
- Data Exfiltration
- Data Factory
- Fabric Workspaces
- General Availability
- Microsoft Fabric
- ML Workloads
- Outbound Access Protection
- Spark
- Tenant Isolation
- Workspace Governance
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog announces the general availability of Outbound Access Protection (OAP) for Spark workloads, providing workspace-level controls to restrict outbound connectivity and strengthen security.<!--excerpt_end-->

# Outbound Access Protection for Spark Now Generally Available in Microsoft Fabric

**Author:** Microsoft Fabric Blog

Outbound Access Protection (OAP) for Spark is now generally available in Microsoft Fabric workspaces. This new security feature is designed to restrict outbound connectivity, helping organizations prevent data exfiltration from their Spark workloads.

## Key Features

- **Restricts Outbound Connectivity:** Control outbound connections from Spark workloads within Fabric workspaces, limiting exposure to external destinations and other Fabric workspaces in the same tenant.
- **Granular Workspace Controls:** Organizations gain fine-grained governance over where data can be sent, adding a new layer of security for sensitive workloads and compliance requirements.
- **Centralized Management:** OAP functionality can be managed at the workspace level, integrating with existing Fabric security and governance features.
- **Expansion Plans:** Microsoft is actively working to expand OAP coverage to include Data Factory artifacts and additional experiences, further strengthening Fabric's overall security posture.

## How It Works

- When enabled, OAP restricts Spark jobs from making outbound connections that haven't been approved by workspace administrators.
- Administrators can specify permitted external endpoints and regulate peer-to-peer connectivity within a tenant.
- OAP helps prevent unintentional data leakage and supports regulatory compliance strategies.

## Resources and Further Reading

- [Workspace Outbound Access Protection Overview (Microsoft Docs)](https://learn.microsoft.com/fabric/security/workspace-outbound-access-protection-overview)
- [Introducing Workspace Outbound Access Protection for Spark (Preview)](https://blog.fabric.microsoft.com/blog/introducing-workspace-outbound-access-protection-for-spark-preview/)
- [Fabric Ideas – Microsoft Fabric Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/fabric%20platform%20%7C%20security)

## What’s Next?

Microsoft plans to expand OAP support to Data Factory artifacts and encourages user feedback through the Fabric Community.

---

For feedback, visit the Fabric Ideas portal to share your suggestions on securing and extending Fabric workspace controls.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-spark-is-now-generally-available/)
