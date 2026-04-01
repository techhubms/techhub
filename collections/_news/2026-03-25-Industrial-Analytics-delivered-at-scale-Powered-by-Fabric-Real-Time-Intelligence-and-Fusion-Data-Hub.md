---
feed_name: Microsoft Fabric Blog
title: 'Industrial Analytics delivered at-scale: Powered by Fabric Real-Time Intelligence and Fusion Data Hub'
section_names:
- ml
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/industrial-analytics-delivered-at-scale-powered-by-fabric-real-time-intelligence-and-fusion-data-hub/
author: Microsoft Fabric Blog
tags:
- AspenTech IP.21
- Asset Hierarchy
- AVEVA PI System
- Data Fidelity
- Data Harmonization
- Data Normalization
- Deduplication
- Eventhouse
- Eventstream
- Geo SCADA
- Ignition
- Industrial IoT
- KQL
- KQL Database
- Kusto
- Late Arriving Data
- Manufacturing Analytics
- Microsoft Fabric
- ML
- News
- OPC UA
- OT Data
- Plant Historians
- Pre Aggregation
- Real Time Intelligence
- Rockwell Automation
- Schneider Electric
- Telemetry Ingestion
- Time Series Data
date: 2026-03-25 13:00:00 +00:00
---

Microsoft Fabric Blog explains how Fusion Data Hub brings industrial OT time-series data from plant historians and edge devices into Microsoft Fabric Real-Time Intelligence (Eventhouse and KQL databases) so teams can analyze governed, normalized telemetry at scale and near real time.<!--excerpt_end-->

## Overview

Industrial organizations produce continuous operational telemetry (temperature, pressure, flow, vibration, energy, etc.). This data is commonly stored in **plant historians** for long-term retention, compliance, and reporting.

The article argues that historians are optimized for running plants, not for modern **cloud-scale analytics and AI**, which creates a gap: operational data exists but is hard to **unify, govern, and operationalize** across teams.

Fusion addresses this by building on **Microsoft Fabric** to make OT (Operational Technology) time-series data **analytics-ready** in Fabric—securely, at scale, and without losing trust in the data.

## The challenge: Turning OT time-series into trusted, usable cloud data

Industrial data integration involves production realities beyond simply copying data:

- Multiple historian systems (on-prem and cloud) with different naming conventions
  - Including different representations of data quality (for example, “good/bad/unknown”)
- Real-world edge cases
  - Late-arriving data
  - Data gaps
  - Future timestamps
  - Intermittent connectivity
- Security and trust requirements
  - Data in the cloud must remain high-fidelity and match the historian source
  - Inconsistencies can impact downstream workflows and decision systems
- DIY approaches (custom connectors/pipelines) often reach POCs but take months to become enterprise-grade
  - Secure, scalable, resilient, maintainable
  - Additional complexity as sources and patterns evolve

## The solution: Fusion Data Hub on Microsoft Fabric

**Fusion Data Hub** is designed to ingest and harmonize OT data from industrial sources—**on-prem and cloud historians and edge devices**—and land it into **Fabric Real-Time Intelligence** foundations:

- **Eventhouse**
- **KQL databases**

This enables teams to analyze OT time-series data using Fabric-native experiences.

### Supported connectors (out of the box)

Fusion Data Hub lists these connectors for ingesting data into the real-time hub:

- OPC-UA
- AspenTech IP.21
- AVEVA PI System
- Schneider Electric Geo SCADA (formerly ClearSCADA)
- Inductive Automation Ignition
- Rockwell Automation

## What this enables in Fabric

### Near real-time visibility

- End-to-end data availability from the plant floor into the customer’s Fabric Eventhouse within **20–30 seconds** (using Fusion-provided connectivity).

![A visual of trendlines from time-series data ingested in Kusto database](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/a-visual-of-trendlines-from-time-series-data-inges.png)

*Figure: A visual display of trendlines ingesting real-time streaming data into an Eventstream.*

### Data fidelity and trust

- Normalization across major historian systems into a **common schema**
- Capabilities mentioned:
  - Deduplication
  - Handling late-arriving data
  - Pre-aggregation
  - Asset hierarchy management

![A view of the real-time normalized data in Kusto.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/a-view-of-the-real-time-normalized-data-in-kusto-.png)

*Figure: A view of aggregated datasets ingested from various historians into Kusto tables.*

### Scalable ingestion for high-volume telemetry

- Designed for large tag counts and high-frequency data
- Scaling claim: from a few events per second up to **millions of events per second**

### Faster time-to-value

- Emphasis on spending effort on operational insights rather than building custom IT ingestion solutions.

![Streaming device data with properties and tags ingested from plant historians](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/real-time-insights-generated-from-a-streaming-data-1.png)

*Figure: Streaming device data with properties and tags ingested from plant historians.*

## Why this matters

Fusion positions **Microsoft Fabric** as a foundation for **Real-Time Intelligence** and “AI-ready” decision systems by enabling OT time-series data to be:

- Ingested
- Organized
- Governed
- Used across teams (engineers validating signals through to operations leaders building dashboards/analytics)

## Getting started

- Add the **Fusion Data Hub workload** to a Fabric workspace to try a sample experience in a demo environment.
- Demo video: https://www.youtube.com/@fusion-data-hub
- Product documentation: https://aka.ms/AA107f0i
- Purchase/upgrade:
  - Upgrade in-app using **Upgrade**
  - Azure Marketplace: https://aka.ms/AA107tff
- Contact: https://aka.ms/AA107f0y

## Related announcements

The post references Arun Ulag’s FabCon/SQLCon 2026 announcement roundup:

- https://aka.ms/FabCon-SQLCon-2026-news

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/industrial-analytics-delivered-at-scale-powered-by-fabric-real-time-intelligence-and-fusion-data-hub/)

