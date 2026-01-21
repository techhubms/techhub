---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/generally-available-high-scale-mode-in-azure-monitor-container/ba-p/4452199
title: General Availability of High Scale Mode in Azure Monitor Container Insights
author: DalanMendonca
feed_name: Microsoft Tech Community
date: 2025-09-08 20:32:11 +00:00
tags:
- Agent Configuration
- AKS
- Azure Monitor
- Cloud Infrastructure
- Cloud Operations
- Container Insights
- Data Collection
- DCR
- High Scale Mode
- Kubernetes
- Log Collection
- Monitoring
- Observability
- Scalability
section_names:
- azure
- devops
---
Dalan Mendonca explains the general availability of High Scale mode in Azure Monitor Container Insights, offering new capabilities for AKS cluster log collection at scale.<!--excerpt_end-->

# General Availability of High Scale Mode in Azure Monitor - Container Insights

**Author: Dalan Mendonca**

## Overview

Azure Monitor's Container Insights now supports High Scale mode, which is generally available to all customers. This feature is designed for Azure Kubernetes Service (AKS) users who need to collect a much larger volume of logs — up to 10 times more than before — to support large-scale workloads.

## Key Features

- **High Volume Log Collection**: High Scale mode addresses growing customer needs by allowing up to and beyond 10,000 logs/sec from a single AKS node.
- **Technical Enhancements**:
  - More powerful agent configuration
  - Different data pipeline for improved throughput
  - Greater resource allocation (e.g., RAM) for log collection agents
  - New Data Collection Rule (DCR) implementation with minimal manual configuration
- **Operational Consistency**:
  - The new mode impacts only the data collection pipeline. Querying, alerting, and user experience remain the same.
  - High Scale mode is currently off by default but may become the default in the future to prevent log loss as workloads grow.

## Benefits

- **Scalability**: Accommodates the increasing adoption of AKS and higher observability demands.
- **Transparency and Ease-of-Use**: All upgrades in the data collection layer are handled automatically, requiring no manual customer intervention.
- **Data Integrity**: Reduces the chance of log loss during workload scaling.

## How to Enable High Scale Mode

To start using High Scale mode in Container Insights, refer to the official documentation: [https://aka.ms/cihsmode](https://aka.ms/cihsmode)

## Additional Information

- **Release Date**: General availability announced September 8, 2025 (Version 2.0)
- **Relevant Services**: Azure Monitor, Azure Kubernetes Service, Container Insights

For news and technical updates, follow the [Azure Observability Blog](https://techcommunity.microsoft.com/t5/azure-observability-blog/bg-p/AzureObservabilityBlog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/generally-available-high-scale-mode-in-azure-monitor-container/ba-p/4452199)
