---
primary_section: ml
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
title: Integrating Dynamics 365 Business Central with Microsoft Fabric using Open Mirroring with BC2Fab workload (Generally Available)
date: 2026-03-24 09:30:00 +00:00
section_names:
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/integrating-dynamics-365-business-central-with-microsoft-fabric-using-open-mirroring-with-bc2fab-workload-generally-available/
tags:
- Data Engineering
- Data Replication
- Data Warehouse
- Dynamics 365 Business Central
- ERP Analytics
- Gen2 Dataflows
- Incremental Change Detection
- Metadata Driven Configuration
- Microsoft Fabric
- ML
- Navida
- News
- OneLake
- Open Mirroring
- Power BI
- Schema Evolution
---

Microsoft Fabric Blog explains how the BC2Fab Fabric Workload from Navida replicates Dynamics 365 Business Central data into Microsoft Fabric via an Open Mirroring architecture, aiming to keep ERP transactional workloads isolated while enabling scalable analytics on OneLake.<!--excerpt_end-->

# Integrating Dynamics 365 Business Central with Microsoft Fabric using Open Mirroring with BC2Fab workload (Generally Available)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of the FabCon and SQLCon announcements across both Fabric and Microsoft’s database offerings:* https://aka.ms/FabCon-SQLCon-2026-news

## Overview

Integrating **Dynamics 365 Business Central** with **Microsoft Fabric** is a common requirement when modernizing analytics platforms. The core challenge is usually not connectivity; it’s designing an architecture that:

- Scales predictably
- Protects the ERP system from analytical load
- Lets analytics teams focus on insights instead of maintaining ingestion pipelines

This article describes using the **BC2Fab Fabric Workload from Navida** to replicate Business Central data into Fabric using an **Open Mirroring** architecture.

- Product listing: https://app.fabric.microsoft.com/workloadhub/detail/Navida.BC2Fabric.Product

## Problem statement

Business Central and Fabric are optimized for different workloads:

- **Business Central**: transactional operations (accounting, purchasing, inventory, order management)
- **Fabric**: analytical workloads (reporting, data science, AI)

Common challenges with traditional ingestion patterns:

- Fabric capacity usage increases when ingestion pipelines do heavy transformations.
- Refresh times grow as data volumes and customizations increase.
- Production ERP environments experience extra load from analytical queries.
- Data teams spend time maintaining pipelines rather than building analytics solutions.

These issues often show up when moving from pilot to production, creating a need for an architecture that keeps ingestion predictable and isolates analytics workloads.

## Architecture overview

The **BC2Fab Fabric Workload** provides an integration layer between Business Central and Fabric by replicating Business Central data into **Microsoft OneLake** (Fabric’s unified storage layer). Once data is in OneLake, it can be used by Fabric experiences without additional copies.

Video: https://vimeo.com/1164034141

![Architecture diagram showing Business Central data replicated by the BC2Fab Fabric Workload into OneLake for use by Microsoft Fabric analytics engines.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/architecture-diagram-showing-business-central-data.png)

*Figure: BC2Fab Fabric Workload Architecture*

### Layers in the architecture

- **Business Central (source system)**
  - Stores operational and financial data.
  - Data is retrieved through APIs or read-only replicas to reduce impact on transactional workloads.
- **BC2Fab Fabric Workload (replication layer)**
  - Synchronizes Business Central data into Fabric using **incremental change detection**.
  - Manages **table replication**, **metadata configuration**, and **schema updates**.
- **OneLake storage (data layer)**
  - Stores replicated data in **open storage formats**.
  - Because OneLake is shared across Fabric workloads, the same dataset can support multiple analytical scenarios.
- **Fabric engines (analytics layer)**
  - Teams can use Fabric experiences like **Data Engineering** and **Data Warehouse**.
  - Reporting via **Microsoft Power BI**.
  - Mentions AI/Copilot scenarios built on ERP data (no implementation details provided).

## Using Business Central data in Fabric

Once Business Central data is available in Fabric, typical analytics scenarios include:

- Financial reporting and profitability analysis
- Sales and revenue performance tracking
- Inventory and supply chain monitoring
- Customer and product analytics

Because the data lands in **OneLake**, it can also be combined with other Fabric datasets to extend ERP reporting with additional operational or analytical sources.

## What clients say

Organizations using BC2Fab report reduced ingestion overhead and faster availability of Business Central tables in Fabric.

- Docs: https://docs.bc2fab.com/

> “BC2Fab deployed to a production workspace in Fabric—no more time spent wrangling data into tables or managing multiple Gen2 dataflows. I can now focus on using Fabric, trusting my Business Central data is already there. Even large Business Central tables—including custom extension fields and tables with 165M rows were available far earlier than planned thanks to metadata-driven configuration. Within the first week, this translated into real, measurable improvements across both daily operations and month-end processes.”
>
> **– Richard Swift, Finance Systems Manager, Enable Networks Limited (New Zealand)**
>
> “Getting Business Central data into Fabric used to be a headache. After the 2023 SQL Server Konferenz we searched everywhere for a reliable approach—until the BC2Fab Workload for Fabric finally solved it. Fast setup, no oversized capacity, and most importantly: it just works. Stable, low‑maintenance, and exactly what a critical data pipeline need. Now our engineers can focus on modeling, analytics, and insights instead of fighting ingestion and sync issues”
>
> – **Steffen Genz, Head of Business Analytics at AXRO GmbH (Germany)**
>
> “I have been using Business Central for years, but BC2Fab allows me to become data driven. I can get all the insights I need to make the right decisions.”
>
> – **Frank Glockzin, Managing Director, Glockzin (Germany)**

## Now available

The **BC2Fab Fabric Workload from Navida** is available for organizations that want to replicate Dynamics 365 Business Central data into Microsoft Fabric:

- https://app.fabric.microsoft.com/workloadhub/detail/Navida.BC2Fabric.Product

The workload is positioned for teams adopting Fabric for analytics who want ERP data replicated into **OneLake** to build reporting and analytics solutions on top of Business Central datasets.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/integrating-dynamics-365-business-central-with-microsoft-fabric-using-open-mirroring-with-bc2fab-workload-generally-available/)

