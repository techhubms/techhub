---
external_url: https://blog.fabric.microsoft.com/en-US/blog/surge-protection-gets-smarter-introducing-workspace-level-controls-preview/
title: Workspace-Level Surge Protection in Microsoft Fabric (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2026-01-26 09:00:00 +00:00
tags:
- Admin Controls
- Capacity Planning
- Compute Units
- Data Platform
- Metrics App
- Microsoft Fabric
- Mission Critical Mode
- Performance Optimization
- Resource Allocation
- Surge Protection
- Throttling
- Workspace Management
- ML
- News
section_names:
- ml
primary_section: ml
---
Microsoft Fabric Blog details new workspace-level surge protection controls, enabling administrators to set compute thresholds, enforce blocking, and prioritize mission-critical workspaces for improved resource management.<!--excerpt_end-->

# Workspace-Level Surge Protection in Microsoft Fabric (Preview)

Surge protection in Microsoft Fabric has long protected organizations from overloading capacity and ensured stable performance. Traditionally, surge protection worked solely at the capacity level, applying universal thresholds to all workspaces.

## What's New: More Granular Workspace Controls

With this update, administrators can now set surge protection at the workspace level. These enhancements mean:

- **Per-workspace CU % Limits:** Now, you can define a compute unit (CU) consumption threshold as a percentage of overall capacity usage for each workspace on a rolling 24-hour basis. This enforces fair usage and prevents accidental monopolization of shared resources.
- **Automatic Blocking:** If a workspace exceeds its CU threshold, it is automatically placed into a blocked state. During this state, new operations for that workspace are rejected until usage falls below the threshold or the block expires, helping prevent unintentional performance degradation for other users.
- **Mission Critical Mode:** Certain high-priority workspaces can be designated as mission critical. These are exempted from surge protection rules, ensuring that vital solutions remain available regardless of overall usage patterns. Mission critical status can also be used to override a block on an essential workspace.

![Surge Protection Settings View](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/01/image-7.png)

## Benefits of Workspace-Level Surge Protection

- **Improved Resource Allocation:** Lower-priority workspaces no longer risk exhausting shared resources, while important projects get the bandwidth they need.
- **Operational Continuity:** Mission critical workspaces are less likely to face interruptions, leading to more predictable performance.
- **Transparency:** The Capacity Metrics App now includes dedicated views to monitor and understand workspace-level surge protection thresholds and status.

## Learn More

- [Surge protection documentation](https://learn.microsoft.com/fabric/enterprise/surge-protection)
- [Metrics app compute page](https://learn.microsoft.com/fabric/enterprise/metrics-app-compute-page)

These new workspace-level controls allow for more sophisticated management strategies, helping organizations optimize their Microsoft Fabric environments for both stability and efficiency.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/surge-protection-gets-smarter-introducing-workspace-level-controls-preview/)
