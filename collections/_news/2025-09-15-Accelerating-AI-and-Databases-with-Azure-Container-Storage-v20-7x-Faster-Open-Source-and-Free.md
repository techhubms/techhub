---
layout: post
title: 'Accelerating AI and Databases with Azure Container Storage v2.0: 7x Faster, Open Source, and Free'
author: Aung Oo
canonical_url: https://azure.microsoft.com/en-us/blog/accelerating-ai-and-databases-with-azure-container-storage-now-7-times-faster-and-open-source/
viewing_mode: external
feed_name: The Azure Blog
feed_url: https://azure.microsoft.com/en-us/blog/feed/
date: 2025-09-15 15:00:00 +00:00
permalink: /ai/news/Accelerating-AI-and-Databases-with-Azure-Container-Storage-v20-7x-Faster-Open-Source-and-Free
tags:
- AI
- AKS
- Azure
- Azure Container Storage
- Cloud Storage
- Compute
- Containers
- CSI
- Databases
- IOPS
- KAITO
- Kubernetes
- Latency
- Model Deployment
- News
- NVMe
- Open Source
- Performance Optimization
- Persistent Volumes
- PostgreSQL
- Stateful Workloads
- Storage Orchestration
section_names:
- ai
- azure
---
Aung Oo shares significant updates to Azure Container Storage, detailing version 2.0's 7x IOPS gains, open-source release, and enhanced AI and database workload support for developers on Azure.<!--excerpt_end-->

# Accelerating AI and Databases with Azure Container Storage v2.0: 7x Faster, Open Source, and Free

Azure Container Storage v2.0.0 delivers a substantial leap in storage performance and developer flexibility for Kubernetes stateful workloads. This update offers:

- **Up to 7x higher IOPS and 4x lower latency** for containers versus prior releases
- **Support for local NVMe drives** on Azure VMs, unlocking high-throughput, low-latency applications
- **No per-GB management fees**—all features now available for free (pay only for the underlying storage)
- **Open-source version** for easy installation on any Kubernetes cluster, not just AKS
- **Flexible scaling**, including single-node clusters

## What is Azure Container Storage?

Azure Container Storage is a cloud-native storage orchestration service for Kubernetes, enabling the provisioning and management of persistent volumes for AKS and other clusters. Version 2.0.0 refactors the architecture for even better speed and efficiency, especially on NVMe-backed nodes, and positions Azure Container Storage as a unified block storage solution for Kubernetes workloads on Azure.

Key features include:

- Seamless integration with Azure Kubernetes Service (AKS)
- Support for multiple storage backends via Kubernetes APIs
- Automation of storage pools, volume lifecycles, snapshots, and scaling
- Elimination of context switching and management of individual CSI drivers

## Highlights in Version 2.0.0

### 1. Free Storage Orchestration (Pricing Changes)

- The service removes monthly storage pool fees (beyond 5 TiB) for both Azure-managed and open-source versions.
- Users pay only for the backend storage they consume.

### 2. Record Performance Improvements

- Optimized for Azure VMs with local NVMe drives.
- The new version delivers 7x higher IOPS and 4x less latency, verified by *fio* benchmarks.
- Real-world applications like PostgreSQL on AKS observed a 60% boost in transaction throughput and over 30% lower latency.
- Performance enhancements are achieved with lower CPU use: v2.0.0 combines past performance tiers into one, providing more speed with reduced resource impact.

### 3. Enhanced Support for AI and ML

- Substantial reduction in model load times for AI/ML workloads, especially beneficial on NVMe-equipped GPU VMs.
- Integration with KAITO, an open-source tool for Kubernetes-native AI model deployment, accelerates time-to-inference for large models such as Llama-3.1-8B-Instruct.
- Loading speeds for large AI models improved fivefold versus using ephemeral OS disks.

### 4. Flexible Cluster Scaling

- Removal of the 3-node minimum for ephemeral drive clusters.
- Now supports clusters of any size—including single-node/dev and edge scenarios.

### 5. Fully Open Source and Community-Oriented

- Find the open-source code at [local-csi-driver](https://github.com/Azure/local-csi-driver).
- Azure Container Storage is available for Azure-managed and self-hosted Kubernetes clusters.
- Community contribution, transparency, and easy installation are core priorities.

### 6. Use Cases and Supported Scenarios

- Supports high-throughput databases (PostgreSQL), AI model deployment and inferencing (KAITO), dev/test environments, and flexible scaling for edge deployments.
- Delivers enterprise-grade storage orchestration to all Kubernetes users on Azure.

## Getting Started

- **New to Azure Container Storage?** [Introductory docs](https://learn.microsoft.com/en-us/azure/storage/container-storage/container-storage-introduction).
- **Deploying Databases?** [PostgreSQL guide](https://learn.microsoft.com/en-us/azure/aks/postgresql-ha-overview).
- **Open-Source Usage?** [GitHub repository](https://github.com/Azure/local-csi-driver).
- **More information:** Find out about [AKS storage integration](https://learn.microsoft.com/en-us/azure/aks/azure-disk-csi) and [Azure Arc support](https://learn.microsoft.com/en-us/azure/azure-arc/container-storage/).

For questions or feedback, contact the Azure team at AskContainerStorage@microsoft.com.

## Visual Benchmarks

- *fio* tests: 7x more IOPS, 4x lower latency
- PostgreSQL on AKS: 60% higher transactions/sec, 30% lower latency
- Llama-3.1-8B-Instruct: Model load time improved 5x

![IOPS and latency benchmarks](https://azure.microsoft.com/en-us/blog/wp-content/uploads/2025/09/image-11.webp)

## Summary

Azure Container Storage v2.0.0 introduces exceptional speed, flexibility, and open community support for stateful workloads on Kubernetes. Its free, open-source distribution and advanced NVMe optimizations position it as a leading solution for running demanding AI and database applications at scale on Azure.

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/accelerating-ai-and-databases-with-azure-container-storage-now-7-times-faster-and-open-source/)
