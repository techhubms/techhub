---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/public-preview-announcing-public-preview-of-dynamic-thresholds/ba-p/4467363
title: 'Dynamic Thresholds for Log Search Alerts in Azure Monitor: Public Preview'
author: Efrat_Ben_Porat
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-18 16:00:00 +00:00
tags:
- AKS
- AKS Monitoring
- Alerting Automation
- Anomaly Detection
- Azure Monitor
- Cloud Operations
- DevOps Monitoring
- Dynamic Thresholds
- Kusto Query Language
- Log Search Alerts
- Machine Learning
- Metric Alerts
- Microsoft
- Resource Graph
- Subscription Based Alerting
section_names:
- ai
- azure
- devops
---
Efrat Ben-Porat presents the public preview of dynamic thresholds for log search alerts in Azure Monitor, showing how machine learning can simplify alerting and improve anomaly detection for cloud resources.<!--excerpt_end-->

# Dynamic Thresholds for Log Search Alerts in Azure Monitor: Public Preview

Azure Monitor's new dynamic thresholds feature for log search alerts automates the process of defining alerting conditions. Leveraging advanced machine learning, this capability intelligently calculates thresholds based on historical log data and observed patterns, both hourly and weekly.

## What Are Dynamic Thresholds?

- **Dynamic thresholds** use machine learning to:
  - Learn from the historical results of your log queries
  - Detect seasonal patterns (hourly, daily, weekly)
  - Automatically select optimal thresholds per alert rule
  - Adjust in response to changes in data patterns

## Benefits

- **Simplified configuration** – Eliminates manual tuning of alert conditions
- **Adaptive monitoring** – Alerts evolve with changing workloads
- **Multi-dimensional support** – Automatic calculation per dimension combination for at-scale use cases

## Example Use Cases

### 1. AKS Pod Restart Spike Anomaly Detection

- **Scenario:** Monitor Kubernetes pod logs for spikes in restarts across clusters.
- **Machine learning advantage:** Dynamic thresholds account for autoscale patterns, aiding detection of true anomalies without false positives from normal workload fluctuations.
- **Kusto Query Example:**

  ```kusto
  KubePodInventory | summarize restartCount = sum(PodRestartCount) by bin(TimeGenerated, 10m), ClusterName, Namespace, Name
  ```

- **Dynamic threshold settings:**
  - Measure: `restartCount`
  - Split by: `Namespace`, `Name` (for workload or pod granularity)

### 2. Resource Inventory Drift Detection (Azure Resource Graph)

- **Scenario:** Track sudden changes in resource creation/deletion across subscriptions or management groups, helping identify abnormal deployment events.
- **Machine learning advantage:** Adapts thresholds by resource type and subscription, minimizing false alerts during routine scaling events.
- **Kusto Query Example:**

  ```kusto
  arg("").Resources | summarize resourceCount = count() by type, subscriptionId
  ```

- **Dynamic threshold settings:**
  - Measure: `resourceCount`
  - Split by: `type`, `subscriptionId`

## Getting Started

To learn more and begin configuring log search alerts with dynamic thresholds, visit the [official Microsoft documentation](https://aka.ms/alerts-dynamic-thresholds).

---

**Author:** Efrat Ben-Porat

- Microsoft Azure Observability Blog
- Published: November 12, 2025

---

This feature helps DevOps engineers and cloud practitioners reduce operational overhead and improve detection accuracy in their cloud monitoring workflows.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/public-preview-announcing-public-preview-of-dynamic-thresholds/ba-p/4467363)
