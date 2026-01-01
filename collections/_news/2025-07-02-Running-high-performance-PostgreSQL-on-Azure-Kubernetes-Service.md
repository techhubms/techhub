---
layout: "post"
title: "Running High-Performance PostgreSQL on Azure Kubernetes Service"
description: "This article by Qi Ke explores strategies for deploying high-performance PostgreSQL workloads on Azure Kubernetes Service (AKS), focusing on advanced storage options like Azure Container Storage with local NVMe and Premium SSD v2, as well as leveraging CloudNativePG for high availability and operational efficiency."
author: "Qi Ke"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://azure.microsoft.com/en-us/blog/running-high-performance-postgresql-on-azure-kubernetes-service/"
viewing_mode: "external"
feed_name: "The Azure Blog"
feed_url: "https://azure.microsoft.com/en-us/blog/feed/"
date: 2025-07-02 15:00:00 +00:00
permalink: "/2025-07-02-Running-high-performance-PostgreSQL-on-Azure-Kubernetes-Service.html"
categories: ["Azure", "ML"]
tags: ["AKS", "Azure", "Azure Blob Storage", "Azure Container Storage", "CloudNativePG", "Compute", "Containers", "Data Durability", "Database Performance", "High Availability", "Kubernetes", "ML", "News", "NVMe", "Persistent Storage", "PostgreSQL", "Premium SSD V2", "SQL", "Storage", "Storage Benchmarking"]
tags_normalized: ["aks", "azure", "azure blob storage", "azure container storage", "cloudnativepg", "compute", "containers", "data durability", "database performance", "high availability", "kubernetes", "ml", "news", "nvme", "persistent storage", "postgresql", "premium ssd v2", "sql", "storage", "storage benchmarking"]
---

Qi Ke examines the deployment of high-performance PostgreSQL databases on Azure Kubernetes Service, detailing solutions for optimizing performance, availability, and cost-efficiency using Azure-native storage technologies and operators.<!--excerpt_end-->

## Introduction

PostgreSQL is increasingly positioned as a leading database choice for Kubernetes workloads, now comprising 36% of all database deployments on Kubernetes, according to the "Kubernetes in the Wild 2025 report." As its popularity rises, so do challenges related to deploying and managing PostgreSQL for demanding, data-intensive scenarios, particularly when balancing Kubernetes primitives and tuning for high performance.

## Storage Options for PostgreSQL on AKS

Azure offers two primary options for hosting stateful PostgreSQL workloads on Azure Kubernetes Service (AKS):

1. **Azure Container Storage with Local NVMe**
   - Designed for strict latency and high transaction workloads
   - Orchestrates Kubernetes volumes on local NVMe storage attached to AKS nodes
   - Enables exceptional IOPS and sub-millisecond latency
   - Ideal for performance-critical environments (e.g., payment systems, gaming, real-time personalization)
   - Supports other storage backends (Azure Disk, Elastic SAN [preview]) under a Kubernetes-native control plane

2. **Premium SSD v2**
   - Suited for scenarios emphasizing price-performance scalability
   - Offers independent configuration of IOPS, throughput, and size
   - Accommodates flexible scaling without excessive up-front cost or overprovisioning
   - Supports up to 80,000 IOPS and 1,200 MB/s per volume
   - Appropriate for SaaS platforms, variable production workloads, and multi-tenant environments

## Benchmark Comparisons

Azure conducted benchmarking using CloudNativePG operator setups on AKS, with both storage options as the only variable:

- **Local NVMe (Standard_L16s_v3)**: Achieved ~14,812 transactions per second (TPS) with an average latency of 4.321 ms
- **Premium SSD v2 (Standard_D16ds_v5)**: Reached 8,600 TPS with a 7.417 ms latency

### Cost Considerations

- Local NVMe delivers best performance, but at a higher cost and less flexible scaling
- Premium SSD v2 emphasizes cost-efficiency with the ability to adjust capacity and performance dynamically
- Data durability is higher with Premium SSD v2 (local redundancy). Local NVMe should be combined with replica-based architectures via CloudNativePG and regular backups to Azure Blob Storage

## High Availability with CloudNativePG Operator

For production deployments, the CloudNativePG operator delivers:

- Built-in replication and automated failover
- Application-consistent backups with Azure Blob Storage integration
- Seamless compatibility with both Azure Container Storage and Premium SSD v2
- Simplifies high-availability architectures without custom logic or manual workflows

Reference: [How to deploy PostgreSQL HA on AKS](https://learn.microsoft.com/en-us/azure/aks/deploy-postgresql-ha?tabs=azuredisk)

## AKS for Future-Proof Stateful Workloads

AKS is evolving to host a wide range of stateful workloads—databases, message queues, AI inference, enterprise applications—with reliability and managed persistence.
Whether deploying PostgreSQL, Redis, MongoDB, Kafka, or GPU-powered ML pipelines, AKS supports operational ease, consistent management, and leading storage innovations (like Azure Container Storage for NVMe and Premium SSD v2 for persistent storage).

Explore proven stateful deployment patterns in the [AKS Stateful Workloads Overview](https://learn.microsoft.com/en-us/azure/aks/stateful-workloads-overview).

## Conclusion

Modernizing your data layer on Kubernetes is made easier with Azure. PostgreSQL, whether for high-performance or cost-efficient deployments, is fully supported on AKS with managed storage, high availability, and scalable architectures. By selecting the appropriate storage backend and leveraging operators like CloudNativePG, organizations can confidently run mission-critical databases on AKS.

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/running-high-performance-postgresql-on-azure-kubernetes-service/)
