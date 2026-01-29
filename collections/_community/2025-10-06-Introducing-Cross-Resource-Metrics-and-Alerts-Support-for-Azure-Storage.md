---
external_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/introducing-cross-resource-metrics-and-alerts-support-for-azure/ba-p/4459193
title: Introducing Cross Resource Metrics and Alerts Support for Azure Storage
author: dafalkne
feed_name: Microsoft Tech Community
date: 2025-10-06 12:38:16 +00:00
tags:
- Action Groups
- Azure Monitor
- Azure Storage
- Blob Storage
- Cloud Operations
- Cross Resource Metrics
- Dashboard
- File Storage
- Fleet Management
- Operational Monitoring
- Queue Storage
- ResourceId
- Storage Metrics
- Table Storage
- Azure
- DevOps
- Community
section_names:
- azure
- devops
primary_section: azure
---
dafalkne announces a new Azure Storage feature: Cross Resource Metrics and Alerts. This post guides users through aggregating metrics and configuring alerts across multiple storage accounts for more effective monitoring and fleet management.<!--excerpt_end-->

# Introducing Cross Resource Metrics and Alerts Support for Azure Storage

## Overview

Azure Storage now supports Cross Resource Metrics and Alerts, allowing you to monitor and analyze storage metrics across multiple storage accounts from a single chart and to configure alerts that span multiple accounts within the same subscription and region. This streamlines the operational workflow for teams managing large Azure Storage fleets.

## What’s New

- **Cross Resource Metrics Support**: Visualize aggregated and per-account breakdowns for metrics across multiple storage accounts.
- **Cross Resource Alerting Support**: Create a single alert rule that responds to metric thresholds breached by any storage account in the group.
- **Full Namespace Support**: Works with Blob, File, Table, and Queue storage services.

## Why This Matters

- **Centralized Monitoring**: Monitor large numbers of storage accounts efficiently via a unified view.
- **Fleet-wide Alerting**: Rapidly detect and respond when any account shows issues, such as unexpected performance drops.
- **Operational Efficiency**: Set up, manage, and scale dashboards and alerts across many storage accounts without individual configuration.

## Getting Started

### 1. Create a Cross Resource Metrics Chart

- Go to **Azure Monitor → Metrics**.
- Under **Select a scope**, choose the target metric namespace (e.g., blob, file, queue, table) for multiple storage accounts in the same subscription and region, then click **Apply**.
- Select a metric (such as Blob Capacity, Transactions, Ingress).
- By default, the chart will have a **Split by** clause on `ResourceId` to enable per-account breakdown.
- To see aggregated metrics for all accounts, remove the **Split by** clause.

#### Example

- Monitor total transactions across storage accounts on the **Hot** tier by selecting the Transactions metric.
- Use the **Add Filter** clause to filter by `ResourceId` and by tier (e.g., Hot).
- Applying splitting by `ResourceId` shows an ordered breakdown per account; removing it shows an aggregate view.

### 2. Set Up Cross Resource Alert Rules

- From the metrics chart, click **New alert rule**.
- The alert will cover all selected accounts; configure the threshold, unit, and value (e.g., Transactions > 5000 over 5 minutes).
- Under **Split by dimensions**, ensure that `Microsoft.ResourceId` is not included if you want aggregation.
- Attach an **Action Group** (Email, Webhook, Logic App, etc.) to define alert actions.
- Review and create the alert.

## Final Thoughts

Cross Resource Metrics and Alerts transform Azure Storage monitoring, making it practical and efficient for operations teams to manage large numbers of storage accounts. This feature enables both broad and granular insights across all supported storage namespace types—including blobs, queues, files, and tables—helping teams visualize performance and rapidly respond to any issues.

For feedback or to learn more, comment below or visit [Azure Feedback](https://feedback.azure.com/).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/introducing-cross-resource-metrics-and-alerts-support-for-azure/ba-p/4459193)
