---
section_names:
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/unifying-analyze-data-with-analytics-across-fabric-preview/
author: Microsoft Fabric Blog
date: 2026-04-14 09:00:00 +00:00
tags:
- Analytics UX
- Data Warehouse
- Eventhouse
- Eventhouse Endpoint
- KQL Database
- Kusto Query Language (kql)
- Lakehouse
- Microsoft Fabric
- ML
- News
- Notebooks
- OneLake
- Preview Feature
- Real Time Intelligence
- Schema Synchronization
- Spark Notebooks
- SQL Endpoint
feed_name: Microsoft Fabric Blog
title: Unifying “Analyze data with” analytics across Fabric (Preview)
primary_section: ml
---

Microsoft Fabric Blog explains how Fabric’s “Analyze data with” entry points are being unified across Lakehouse, Data Warehouse, and Eventhouse (Preview), embedding Eventhouse Endpoint alongside SQL Endpoint and Notebooks to make analysis more discoverable and consistent.<!--excerpt_end-->

## Overview

Microsoft Fabric is updating its **“Analyze data with”** entry points (Preview) to provide a more consistent way to analyze data across **Lakehouse**, **Data Warehouse**, and **Eventhouse**. The key change is deeper integration of the **Eventhouse Endpoint** into these experiences, alongside **SQL Endpoint** and **Notebooks**.

![Lakehouse with the updated "Analyze data with" entry point](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/lakehouse-with-the-newe-analyze-data-with.png)

*Figure: Lakehouse with the updated Analyze data with entry point, providing consistent access to analytics experiences.*

![Analyze data with menu showing Notebook and Eventhouse Endpoint options](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/analyze-data-with-has-new-options-of-notebook-an.png)

*Figure: The Analyze data with menu now includes Notebook and Eventhouse Endpoint, enabling quick SQL and Spark-based analysis.*

## Why this integration matters

Fabric users historically had multiple analysis options spread across different menus and experiences. That fragmentation increased onboarding friction, especially for new users who needed to learn both:

- which analytics engines were available, and
- where each option lived in the UI.

Making **“Analyze data with”** a first-class action across workloads is intended to provide a **single, predictable starting point** for analysis while keeping flexibility for advanced users.

## Eventhouse Endpoint on Lakehouse and Data Warehouse main pages

**Eventhouse Endpoint** provides an **Eventhouse-powered query experience** directly on top of **Lakehouse** and **Data Warehouse** data, without requiring data duplication or manual sync.

When enabled:

- An **Eventhouse** and a **KQL Database** are created automatically as child artifacts of the source Lakehouse or Warehouse.
- **Schema synchronization** is handled automatically by the backend.

With the new integration, users can:

- Analyze data using **SQL Endpoint**.
- Analyze data using **Notebooks** (both **new** and **existing** notebooks).
- Access **Eventhouse Endpoint** directly from Lakehouse and Data Warehouse via **“Analyze data with.”**
- Open Eventhouse from the source data experience without switching to another workload first.
- Rely on the endpoint reflecting the **current schema** of the source data for near-real-time analytical access.

## “Analyze data with” inside Eventhouse

The same unified **“Analyze data with”** menu is now available within **Eventhouse** itself (at the database level), grouping supported analytics tools together.

From Eventhouse, users can:

- Analyze data using **SQL Endpoint** (when **OneLake availability and sync** are enabled).
- Analyze data using **Notebooks** (new and existing).
- Launch analysis actions from a single predictable location (placed next to Share), instead of searching through the ribbon.

The intent is consistency: whether starting from Lakehouse, Warehouse, or Eventhouse, the analysis entry point should look and behave similarly.

## Notebook support across workloads (new and existing)

Notebook integration is part of the unified experience across workloads.

Notable capabilities:

- Open a notebook from a **KQL Database** or **Eventhouse**, with the database automatically added to the notebook environment.
- Keep consistent behavior for **Spark notebooks**, regardless of starting point (Lakehouse, Warehouse, Eventhouse).
- Use a single integration model to avoid special-case flows.

## What Fabric is aiming for

By embedding Eventhouse Endpoint into “Analyze data with” across relevant workloads, Fabric is targeting:

- A unified entry point for analytics
- Clear discoverability for **Eventhouse**, **SQL Endpoint**, and **Notebooks**
- Reduced onboarding friction
- A scalable model for plugging additional workloads/tools into the same pattern over time

Learn more: Eventhouse Endpoint for Lakehouse and Data Warehouse (Preview) (https://learn.microsoft.com/fabric/real-time-intelligence/eventhouse-as-endpoint)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/unifying-analyze-data-with-analytics-across-fabric-preview/)

