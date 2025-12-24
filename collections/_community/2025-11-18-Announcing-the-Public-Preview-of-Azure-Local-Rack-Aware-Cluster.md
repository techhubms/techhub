---
layout: "post"
title: "Announcing the Public Preview of Azure Local Rack Aware Cluster"
description: "This announcement details the public preview release of Azure Local rack aware cluster, describing its advanced architecture for fault tolerance and high availability in Azure Local instances. It highlights architecture concepts, cluster deployment options, native Azure Arc integration, unified storage capabilities, rack-level zone resiliency, and streamlined operational upgrades for hybrid workloads."
author: "mindydiep"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-preview-of-azure-local-rack-aware-cluster/ba-p/4469435"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:33:51 +00:00
permalink: "/community/2025-11-18-Announcing-the-Public-Preview-of-Azure-Local-Rack-Aware-Cluster.html"
categories: ["Azure", "DevOps"]
tags: ["ARM Templates", "Availability Zones", "Azure", "Azure Arc", "Azure Local", "Azure Portal", "Cluster Deployment", "Community", "Data Resiliency", "DevOps", "Fault Tolerance", "High Availability", "Hybrid Cloud", "Preview Release", "Rack Aware Cluster", "Scale Out", "Storage Spaces Direct", "Version 2510", "VM Provisioning"]
tags_normalized: ["arm templates", "availability zones", "azure", "azure arc", "azure local", "azure portal", "cluster deployment", "community", "data resiliency", "devops", "fault tolerance", "high availability", "hybrid cloud", "preview release", "rack aware cluster", "scale out", "storage spaces direct", "version 2510", "vm provisioning"]
---

mindydiep introduces the public preview of Azure Local rack aware cluster, highlighting its architecture and integration for fault-tolerant and scalable hybrid cloud deployments.<!--excerpt_end-->

# Announcing the Public Preview of Azure Local Rack Aware Cluster

Azure has released the public preview of Azure Local rack aware clusters, bringing robust fault tolerance and enhanced data distribution for on-premises Azure Local environments. These clusters span two physical racks, each acting as a local availability zone, located in separate rooms or buildings and connected using high bandwidth, low latency networking. This design ensures that, even if one rack experiences a failure (due to fire or power outage), the other rack maintains data integrity and operational continuity.

## Architecture and Features

- **Rack-Level Fault Tolerance**: Distributes data evenly between racks using unified storage pools and Storage Spaces Direct (S2D) for volume replication.
- **Flexible Configurations**: Supports cluster setups from 2 to 8 machines, allowing scalable deployments tailored to different workload sizes.
- **Scale-Out Expansion**: Easily grow the cluster by adding machines, accommodating dynamic workload needs without requiring a full redeployment.
- **Unified Storage Pool**: Automatically balances data distribution, improving failover performance and reducing risk of rack-level data loss.
- **Native Azure Arc Integration**: Enables seamless management and monitoring across hybrid environments, facilitating consistent Azure Local VM and AKS operations.
- **Deployment Options**: Provision clusters via the Azure Portal or ARM templates, offering familiar Azure deployment experiences, with new portal inputs specific to rack aware clusters.
- **VM Provisioning in Local Zones**: Place Azure Local virtual machines directly into specific local availability zones via the portal for granular workload allocation and improved resilience.
- **Upgrade from Preview to GA**: Update clusters deployed with the preview build (version 2510) to General Availability releases without redeployment, preserving investments and operational continuity.

## Getting Started

The public preview is now available to all Azure customers. For setup and documentation,
see: [Overview of Azure Local rack aware clustering (Preview) - Azure Local | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-local/concepts/rack-aware-cluster-overview)

Feedback from early adopters will shape further enhancements before general availability in 2026.

---

**Author:** mindydiep  
**Published:** November 12, 2025

## Additional Details

- Azure Arc integration enables unified management for hybrid deployments.
- High availability design supports mission-critical workloads in edge and on-premises environments.
- Storage Spaces Direct offers enterprise-grade failover and recovery features.
- Customers can use familiar tooling (Azure Portal, ARM templates) for smooth deployment.

Stay tuned for more updates and share your feedback as you explore Azure Local rack aware cluster’s capabilities for your next-generation edge workloads.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-preview-of-azure-local-rack-aware-cluster/ba-p/4469435)
