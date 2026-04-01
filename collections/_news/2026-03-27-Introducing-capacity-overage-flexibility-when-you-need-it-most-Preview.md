---
author: Microsoft Fabric Blog
date: 2026-03-27 09:30:00 +00:00
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-capacity-overage-preview-flexibility-when-you-need-it-most/
feed_name: Microsoft Fabric Blog
title: Introducing capacity overage flexibility when you need it most (Preview)
primary_section: ml
tags:
- 24 Hour Billing Window
- Billing Transparency
- Capacity Admin
- Capacity Bursting
- Capacity Debt
- Capacity Events
- Capacity Metrics App
- Capacity Overage
- Capacity Smoothing
- Compute Spikes
- Fabric Capacity
- Microsoft Fabric
- ML
- News
- Overage Limit
- Pay as You Go
- Preview Feature
- Real Time Hub
- Throttling
- Workload Availability
section_names:
- ml
---

Microsoft Fabric Blog announces Capacity overage (preview), an opt-in Microsoft Fabric capability that keeps workloads running during unexpected compute spikes by automatically billing for excess usage within admin-set limits, instead of throttling operations.<!--excerpt_end-->

## Overview

**Capacity overage** is a new **opt-in** capability in **Microsoft Fabric** (now in **preview**) intended to keep Fabric workloads running during unexpected compute spikes. Instead of throttling operations when the purchased capacity limit is exceeded, Fabric can automatically bill for the excess usage—within limits that capacity admins configure.

The post also points readers to Arun Ulag’s summary of broader event announcements: “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform”.

## Why capacity overage?

Fabric capacities include mechanisms intended to manage load:

- **Bursting**
- **Smoothing**
- **Throttling**

Under sustained high load, capacities can enter throttling phases, which can result in:

- Delays
- Rejected operations

**Capacity overage** is positioned as a way to avoid those disruptions by:

- Automatically “paying off” compute overages in real time
- Clearing accumulated **capacity debt**
- Lifting workloads back out of throttling

The post states that:

- **No jobs are paused or terminated**
- Operations continue normally

### Cost and when to use it

- Overage usage is billed at **3× the pay-as-you-go rate**.
- The post frames this as best suited for **occasional spikes** to ensure availability.
- For **prolonged** excess usage, it recommends either:
  - Further **optimizations**, or
  - Moving to the **next higher sized capacity**

## How capacity overage billing works

Fabric capacity admins can configure capacity overage **per capacity**:

- Opt-in on a per-capacity basis
- Set a limit for how much overage can be incurred within a **24-hour period**

If billed overage reaches the configured limit:

- Fabric reverts to normal **throttling** behavior
- Interactive delays or job rejections may occur until:
  - The window resets, or
  - The admin increases the limit

## Monitoring and transparency

The post says Microsoft will be rolling out updates to the **capacity metrics app** so customers can view overage billing utilization.

It also notes overage information is available via **capacity events** in **Real-Time Hub**.

## Learn more

- Capacity overage documentation: https://learn.microsoft.com/fabric/enterprise/capacity-overage-overview


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/introducing-capacity-overage-preview-flexibility-when-you-need-it-most/)

