---
tags:
- Azure
- Azure Managed Grafana
- Azure Monitor
- Azure Monitor Managed Service For Prometheus
- Azure Portal
- Community
- Copy To Managed Grafana
- Dashboard Migration
- Dashboards With Grafana
- Data Sources
- Grafana
- Grafana Plugins
- Grafana Workspaces
- Managed Identity
- Managed Prometheus
- Monitoring
- Observability
- Private Endpoints
- RBAC
- Reporting
- Visualization
section_names:
- azure
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/copy-dashboards-from-dashboards-with-grafana-to-azure-managed/ba-p/4505710
author: aayodeji
primary_section: azure
feed_name: Microsoft Tech Community
title: Copy dashboards from Dashboards with Grafana to Azure Managed Grafana
date: 2026-03-26 17:28:43 +00:00
---

aayodeji explains the new “Copy to Managed Grafana” flow in the Azure portal, which lets teams move custom Azure Monitor dashboards from the in-portal Grafana experience into Azure Managed Grafana without exporting JSON or recreating dashboards.<!--excerpt_end-->

## Overview

Azure Monitor **Dashboards with Grafana** is an in-portal Grafana experience optimized for:

- **Azure Monitor** data
- **Managed Prometheus** data

As teams’ observability needs grow (more data sources, tighter security, and more advanced workflows), they may want to move dashboards into **Azure Managed Grafana**.

This release adds a new **Copy to Managed Grafana** experience to reduce friction when making that transition.

## Why a copy experience was needed

**Dashboards with Grafana** is positioned as the “fast, zero-setup” option:

- Fast, zero-setup visualization
- Tight integration with Azure Monitor and Prometheus
- Embedded directly in the Azure portal

**Azure Managed Grafana** extends that with broader capabilities:

- Full Grafana workflows (alerts, reporting, automation)
- Additional data sources and plugin support
- Enterprise security features:
  - Private endpoints
  - Managed identity
- Reuse and governance features:
  - Folders
  - APIs
  - Role Based Access Controls (RBAC)

Previously, moving dashboards often meant manual recreation or exporting/importing JSON.

## Introducing “Copy to Managed Grafana”

You can now copy dashboards from **Dashboards with Grafana** into **Azure Managed Grafana** directly from the Azure portal **without changing the original dashboard**.

The experience is designed to be:

- **In-context**: start from your dashboard inside Dashboards with Grafana
- **Seamless**: no exports or re-creation
- **Non-disruptive**: keep using the source dashboard while adopting Managed Grafana

## How the flow works

1. In your dashboard (in **Dashboards with Grafana**), select **Copy to Managed Grafana**.
   - Note: this **doesn’t work on built-in dashboards**. You must first save a copy of the built-in dashboard.
2. Choose an existing **Azure Managed Grafana** workspace, or create a new one.
3. Complete the copy.
4. In Azure Managed Grafana, configure advanced capabilities **after copying** (as needed), such as:
   - Additional data sources
   - Alerts
   - Folder organization

## Intended “observability journey”

The post frames this as an incremental path:

- Start quickly with **Dashboards with Grafana**
- Copy dashboards into **Azure Managed Grafana** when you need more capabilities
- Keep end-to-end observability in the Azure ecosystem as requirements evolve

## Reference

- Microsoft Learn: Copy an Azure Monitor dashboard to Azure Managed Grafana - Azure Monitor | Microsoft Learn
  - https://learn.microsoft.com/en-us/azure/azure-monitor/visualize/visualize-copy-to-managed-grafana

## Metadata from the source

- Published: Mar 26, 2026
- Version: 1.0

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-observability-blog/copy-dashboards-from-dashboards-with-grafana-to-azure-managed/ba-p/4505710)

