---
tags:
- Audit Visibility
- Compliance
- Connection Inventory
- Connection Lifecycle Management
- Connection Recency
- Connections
- Credential Rotation
- Data Source Management
- Fabric Data Factory
- Governance
- Microsoft Fabric
- ML
- News
- Observability
- Operational Risk
- Preview Feature
- Workspace Administration
date: 2026-03-23 09:00:48 +00:00
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/connection-recency-for-improved-audit-and-management-preview/
title: Connection Recency for improved audit and management (Preview)
author: Microsoft Fabric Blog
section_names:
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces a preview feature called Connection Recency that adds usage signals to Fabric connections, helping admins and workspace owners audit connection usage and manage cleanup, credential rotation, and governance more safely.<!--excerpt_end-->

# Connection Recency for improved audit and management (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

Introducing a new **connection recency** capability for connections **(Preview)**, designed to improve audit visibility and connection lifecycle management across **Microsoft Fabric**. This feature adds two new usage-based properties to connections, helping administrators and workspace owners better understand how and when a connection is used.

Over time, connections may be reused, replaced, or left idle, making it difficult to determine whether they are still active or safe to modify or remove. With these new usage signals, you can more confidently manage connection inventory and reduce operational risk.

## What’s new in preview

Two new properties are now added to connections:

### Last linked to items

- Shows the most recent time the connection was linked to a Fabric artifact.
- Reflects configuration activity (when the connection was associated with an item).
- Helps identify recently created or newly reused connections.

### Last credentials used

- Shows the most recent time the connection’s credentials were used at runtime.
- Indicates actual execution usage, not just configuration.
- Helps distinguish between defined connections and actively used ones.

![Screenshot showing the recency of connection.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-the-recency-of-connection-.png)

*Figure: Connection recency displays the last used time for each connection, enabling administrators to quickly identify stale or unused connections for better management.*

Together, these properties provide both **linkage signals** and **runtime usage signals** for better operational insight.

## Why this matters

This feature helps improve governance and operational decision-making by enabling teams to:

- Improve **audit and compliance visibility**.
- Identify **inactive or stale connections**.
- Make safer **cleanup and decommissioning** decisions.
- Validate usage before **credential rotation**.
- Differentiate between configured vs. executed connections.
- Better manage connection sprawl in large environments.

## Getting started

No additional setup is required. The new properties are automatically populated based on system activity and will appear in the connection metadata where supported.

Learn more from: [Data source management – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/data-factory/data-source-management#connection-recency-preview)

## Looking ahead

Microsoft will continue enhancing connection observability and governance capabilities, adding richer usage signals and management insights over time. Feedback is welcomed during the preview period.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/connection-recency-for-improved-audit-and-management-preview/)

