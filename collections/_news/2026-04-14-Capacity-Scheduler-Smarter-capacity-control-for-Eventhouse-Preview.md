---
section_names:
- ml
date: 2026-04-14 10:00:00 +00:00
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
tags:
- Autoscale
- Baseline Capacity
- Capacity Planning
- Capacity Scheduling
- Elastic Scaling
- Eventhouse
- Ingestion Workloads
- Microsoft Fabric
- Minimum Capacity
- ML
- News
- Preview Feature
- Query Workloads
- Real Time Analytics
- Real Time Intelligence
- Workload Scheduling
- Workspace Monitoring
title: 'Capacity Scheduler: Smarter capacity control for Eventhouse (Preview)'
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/capacity-scheduler-smarter-capacity-control-for-eventhouse-preview/
---

The Microsoft Fabric Blog introduces Capacity Scheduler (preview) for Eventhouse, explaining how teams can schedule minimum capacity across the week to better match predictable real-time analytics patterns while keeping autoscale enabled.<!--excerpt_end-->

## Capacity Scheduler: Smarter capacity control for Eventhouse (Preview)

Modern real-time analytics workloads often follow predictable patterns (for example: heavy ingestion during business hours, lighter query traffic overnight, quiet weekends, and short but critical pipeline windows). In Eventhouse, this can make a single “always-on” minimum capacity setting either wasteful (paying for guaranteed resources when they’re not needed) or risky (insufficient performance during peak hours).

To address this, Eventhouse introduces **Capacity Scheduler** (preview), which aligns guaranteed baseline capacity with when workloads actually run.

![Minimum Consumption Scheduler allows customers to define different minimum capacity levels across the week](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/minimum-consumption-scheduler-allows-customers-to.png)

*Figure: Minimum Consumption Scheduler allows customers to define different minimum capacity levels across the week.*

## What Capacity Scheduler enables

Capacity Scheduler lets you define **different minimum capacity levels across the week** instead of relying on one static minimum.

Capabilities:

- Configure a **7-day recurring schedule**.
- Split each day into **60-minute time blocks**.
- Define a **minimum capacity per block** or explicitly mark it as **no minimum**.
- Turn guaranteed capacity **on only when needed**, **without disabling autoscale**.

The intent is to keep a baseline only during the time windows that matter, while staying fully elastic outside of those windows.

## Designed for operational clarity

A design goal is **predictability** and reducing configuration mistakes:

- The UI provides a **clear weekly view** of minimum-capacity behavior.
- Validation occurs **at the cell level** (spreadsheet-style) to make errors easy to spot.
- Warnings appear when scheduled minimums exceed available capacity.
- The Eventhouse overview banner evaluates the **total minimum capacity for the next 24 hours**, so you can see upcoming commitments before saving.

This design reflects feedback from internal UX reviews and Teams discussions aimed at reducing surprises and misconfiguration.

## Next steps and documentation

Feedback is requested on scenarios the scheduler helps with today and where it should evolve toward GA.

- Eventhouse documentation: https://learn.microsoft.com/fabric/real-time-intelligence/eventhouse
- Workspace monitoring resources: https://learn.microsoft.com/fabric/real-time-intelligence/monitor-eventhouse


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/capacity-scheduler-smarter-capacity-control-for-eventhouse-preview/)

