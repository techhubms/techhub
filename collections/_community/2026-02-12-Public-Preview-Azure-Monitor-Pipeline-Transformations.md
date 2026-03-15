---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/public-preview-azure-monitor-pipeline-transformations/ba-p/4491980
title: 'Public Preview: Azure Monitor Pipeline Transformations'
author: susaraswat4
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-12 00:16:01 +00:00
tags:
- Aggregation
- Azure
- Azure Monitor
- Cloud Monitoring
- Community
- Cost Optimization
- Data Collection
- Data Routing
- Edge Computing
- Filtering
- KQL
- Log Analytics
- Multi Cloud
- Pipeline Transformations
- Schema Validation
- Syslog
- Telemetry
section_names:
- azure
---
susaraswat4 outlines the capabilities and features of Azure Monitor pipeline transformations, explaining how this new Azure feature enables granular data shaping, schema validation, KQL templates, and significant telemetry ingestion cost savings for cloud and edge scenarios.<!--excerpt_end-->

# Public Preview: Azure Monitor Pipeline Transformations

**Author: susaraswat4**

Azure has introduced pipeline transformations in Azure Monitor, currently in public preview, to help organizations gain granular control over telemetry data before ingestion—especially for edge and multi-cloud scenarios.

## Overview

The [Azure Monitor pipeline](https://learn.microsoft.com/en-us/azure/azure-monitor/data-collection/data-collection-rule-overview#azure-monitor-pipeline) now lets users collect massive telemetry volumes (over 100k EPS), route data flexibly (including edge-to-cloud and intermittent connections), and cache locally until cloud connectivity is restored. This enables robust hybrid data collection and optimized transmission.

Key public docs:

- [Configure Azure Monitor pipeline - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/data-collection/edge-pipeline-configure?tabs=Portal)

## Why Transformations Matter

**Transformations before ingestion:**

- **Lower costs:** Aggregate/filter data before it enters Azure Monitor to reduce ingestion volume and costs.
- **Improved analytics:** Enforce schema standardization for cleaner dashboards and more efficient query performance.
- **Future-proofing:** Built-in schema validation safeguards data structures at deployment time.

By transforming data at the pipeline level, users can avoid complex downstream processing and get high-quality, analytics-ready datasets directly in their Log Analytics Workspace.

## Key Features in Public Preview

### 1. Schema Change Detection

- Built-in validation (exposed via the "Check KQL Syntax" button in the Strato UI) inspects transformations for schema compatibility against standard tables (Syslog, CEF).
- If a transformation introduces changes (such as new columns), actionable guidance is provided:
  - Option 1: Remove schema-changing operations, e.g. aggregations.
  - Option 2: Route data to custom tables supporting such schemas.
- Ensures pipelines remain compatible and robust for analytics.

### 2. Pre-Built KQL Templates

- Ready-made KQL templates can be applied for common data transformation scenarios, minimizing query-writing effort and mistakes.

### 3. Automatic Schema Standardization for Syslog and CEF

- Automates schematization for these log types, ensuring compatibility with standard tables without extra transformation code.

### 4. Advanced Filtering

- Allows dropping events by specific Syslog (Facility, ProcessName, SeverityLevel) and CEF (DeviceVendor, DestinationPort) attributes.
- Filters out noise and lowers ingest/storage costs.

### 5. Log Aggregation for High-Volume Sources

- Enables grouping by key fields (e.g., DestinationIP, DeviceVendor) into fixed intervals (1 min), summarizing large log streams efficiently.

### 6. Drop Unnecessary Fields

- Remove redundant columns to further reduce storage and improve clarity of ingested datasets.

## Supported KQL Functions

Common supported categories:

- **Aggregation:** summarize(by), sum, max, min, avg, count, bin
- **Filtering:** where, contains, has, in, and, or, equality (==, !=), comparison operators
- **Schematization:** extend, project, project-away, project-rename, project-keep, iif, case, coalesce, parse_json
- **Variables:** let
- **String/Conversion operations:** strlen, replace_string, substring, strcat, extract, tostring, toint, tobool, tofloat, todatetime, etc.

## Getting Started

- Access the new pipeline transformations UI from the [Azure Portal](https://ms.portal.azure.com/?feature.canmodifystamps=true&Microsoft_Azure_Monitoring=stratopreview#home).
- Apply sample templates, validate your KQL, and test all transformation features in public preview.
- For full technical walkthroughs, visit: [Configure Azure Monitor pipeline transformations - Azure Monitor | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-monitor/data-collection/pipeline-transformations?tabs=portal)

## Summary

Azure Monitor pipeline transformations bring flexible, powerful data shaping capabilities directly to your telemetry ingestion layer. This upgrade enables practitioners to control cost, ensure data quality, comply with analytics schemas, and adapt to edge/multi-cloud environments efficiently.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/public-preview-azure-monitor-pipeline-transformations/ba-p/4491980)
