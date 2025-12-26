---
layout: "post"
title: "Outbound Access Protection for Spark Now Generally Available in Microsoft Fabric"
description: "This news update introduces Outbound Access Protection (OAP) for Spark workloads in Microsoft Fabric workspaces, a security feature designed to restrict outbound connectivity and help prevent data exfiltration. Organizations can now govern how Spark jobs connect to external destinations and other Fabric workspaces within the same tenant. The announcement also shares plans to extend OAP support to Data Factory artifacts in future updates, emphasizing Microsoft's focus on granular security controls."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/workspace-outbound-access-protection-for-spark-is-now-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-01 14:00:00 +00:00
permalink: "/news/2025-10-01-Outbound-Access-Protection-for-Spark-Now-Generally-Available-in-Microsoft-Fabric.html"
categories: ["Azure", "ML", "Security"]
tags: ["Azure", "Cloud Security", "Connectivity Control", "Data Exfiltration", "Data Factory", "Fabric Workspaces", "General Availability", "Microsoft Fabric", "ML", "ML Workloads", "News", "Outbound Access Protection", "Security", "Spark", "Tenant Isolation", "Workspace Governance"]
tags_normalized: ["azure", "cloud security", "connectivity control", "data exfiltration", "data factory", "fabric workspaces", "general availability", "microsoft fabric", "ml", "ml workloads", "news", "outbound access protection", "security", "spark", "tenant isolation", "workspace governance"]
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
