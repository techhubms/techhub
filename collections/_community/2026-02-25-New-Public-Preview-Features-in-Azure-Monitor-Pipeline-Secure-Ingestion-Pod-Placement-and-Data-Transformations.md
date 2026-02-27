---
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-new-public-preview-capabilities-in-azure-monitor/ba-p/4488904
title: 'New Public Preview Features in Azure Monitor Pipeline: Secure Ingestion, Pod Placement, and Data Transformations'
author: XemaPathak
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-25 23:40:09 +00:00
tags:
- Azure
- Azure Monitor
- Certificate Management
- Certified Authentication
- Cluster Scheduling
- Community
- Data Aggregation
- Data Pipeline
- Data Transformation
- DevOps
- Edge Computing
- ETL
- KQL
- Kubernetes
- Mtls
- Pod Placement
- Public Preview
- Schema Standardization
- Secure Ingestion
- Security
- Telemetry
- TLS
section_names:
- azure
- devops
- security
---
XemaPathak outlines new public preview features in Azure Monitor pipeline, including secure TLS/mTLS ingestion, Kubernetes pod placement controls, and automated data transformations, aimed at enhancing scalability and security.<!--excerpt_end-->

# New Public Preview Features in Azure Monitor Pipeline: Secure Ingestion, Pod Placement, and Data Transformations

**Author:** XemaPathak  

Azure Monitor pipeline continues to evolve as a flexible, scalable platform for unified telemetry collection. In this update, several new public preview features are introduced to address the challenges of secure, large-scale data collection, especially for complex environments such as on-premises, edge sites, and large Kubernetes clusters.

## What's New and Why It Matters

The newly announced capabilities focus on three critical areas:

- **Secure ingestion with TLS and mutual TLS (mTLS)** for enhanced data protection.
- **Pod placement controls** to provide finer control over where pipeline instances run in Kubernetes environments.
- **Transformations and automated schema standardization** to clean, filter, and shape data before it reaches Azure Monitor.

---

## 1. Secure Ingestion with TLS and Mutual TLS (mTLS)

### Why is this important?

Moving telemetry ingestion beyond Azure, especially toward the edge, raises the bar for data security. Security-conscious environments, including regulated industries, require encrypted data flows and robust authentication.

### Feature Details

- **TLS and mTLS support**: Enables both server- and mutual client-server authentication.
- **Certificate integration**: Bring your own certificates and work seamlessly with your enterprise PKI.
- **Ingestion-time enforcement**: Security standards are enforced when data first enters the pipeline.

These controls allow teams to securely ingest telemetry from devices, appliances, and on-premises workloads—removing the need for external proxies or gateways.

[Learn more about TLS/mTLS for Azure Monitor pipeline](https://aka.ms/AzMonTLS)

---

## 2. Pod Placement Controls for Kubernetes

### Why is this necessary?

Kubernetes-based deployments demand operational flexibility for performance, cost, and compliance:

- **Workload isolation** in multi-tenant clusters
- Running pipelines on specific, high-resource nodes
- Limiting instances per node to prevent port exhaustion
- Enforcing data residency, security zones, and zone-aware deployment

### Feature Details

- **executionPlacement configuration**: Directly control where Azure Monitor pipeline pods schedule in your cluster.
- **Node targeting** via labels (by team, resource capability, or zone)
- **Strict isolation**: Restrict pipelines to single nodes as needed
- **Validated at deployment**: Ensures that unsatisfied placement constraints prevent deployment, avoiding silent failures

[Learn more about pod placement controls](http://aka.ms/AzMonpipelinePod)

---

## 3. Transformations and Automated Schema Standardization

### Why is this needed?

Telemetry data can be inconsistent and voluminous. Pre-processing incoming data allows you to filter, normalize, and shape information before storage, reducing cost and increasing analysis quality.

### Feature Details

- **Data transformation rules**: Filter, aggregate, or reshape incoming data.
- **Schema standardization**: Automatically convert logs (e.g., syslog, CEF) to standardized schemas.
- **Sample KQL templates**: Use pre-built queries or customize as needed.
- **Direct routing**: Send transformed data directly to standard Azure tables.

[Read the official blog for more details](https://techcommunity.microsoft.com/blog/azureobservabilityblog/public-preview-azure-monitor-pipeline-transformations/4491980) and [Microsoft Learn documentation](https://learn.microsoft.com/en-us/azure/azure-monitor/data-collection/pipeline-transformations?tabs=portal).

---

## Getting Started

All these capabilities are available today in **public preview**. If you're already using Azure Monitor pipeline, you can start experimenting with secure ingestion, pod placement, and data transformations now. Feedback is encouraged as the platform evolves toward general availability.

- [Azure Monitor pipeline overview](https://aka.ms/AzMonEdgePipeline)

---

*Updated Feb 25, 2026*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/announcing-new-public-preview-capabilities-in-azure-monitor/ba-p/4488904)
