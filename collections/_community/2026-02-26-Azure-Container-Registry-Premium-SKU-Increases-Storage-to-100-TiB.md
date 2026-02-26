---
layout: "post"
title: "Azure Container Registry Premium SKU Increases Storage to 100 TiB"
description: "This announcement details the expansion of Azure Container Registry's Premium SKU storage limit from 40 TiB to 100 TiB. The update addresses enterprise-scale needs for container storage, speeds up geo-replication, improves portal monitoring, and supports large-scale AI and machine learning workloads. No migration is required for existing users; all Premium SKUs benefit automatically."
author: "johshmsft"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-container-registry-premium-sku-now-supports-100-tib/ba-p/4497651"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-26 20:46:56 +00:00
permalink: "/2026-02-26-Azure-Container-Registry-Premium-SKU-Increases-Storage-to-100-TiB.html"
categories: ["Azure"]
tags: ["ACR", "Azure", "Azure CLI", "Azure Container Registry", "Azure Portal", "Cloud Containers", "Community", "Container Management", "Containerization", "DevOps Tools", "Enterprise DevOps", "Geo Replication", "Monitoring", "Premium SKU", "Registry Capacity", "Registry Storage", "REST API", "Storage Limits", "VM Migration"]
tags_normalized: ["acr", "azure", "azure cli", "azure container registry", "azure portal", "cloud containers", "community", "container management", "containerization", "devops tools", "enterprise devops", "geo replication", "monitoring", "premium sku", "registry capacity", "registry storage", "rest api", "storage limits", "vm migration"]
---

johshmsft announces that Azure Container Registry Premium SKU now supports up to 100 TiB storage, addressing needs of large enterprise, AI/ML, and global DevOps teams with increased capacity and faster geo-replication.<!--excerpt_end-->

# Azure Container Registry Premium SKU Now Supports 100 TiB Storage

**Author:** johshmsft  
**Published:** Feb 26, 2026

## Overview

Azure Container Registry (ACR) Premium SKU has expanded its storage limit from 40 TiB to 100 TiB, a significant upgrade aimed at supporting the storage demands of large organizations, AI/ML workloads, and teams migrating from virtual machines to containers. In addition, geo-replication sync times are improved, and a new Azure Portal experience provides enhanced monitoring and visibility of storage capacity.

## Key Updates

- **Premium SKU storage:** Up to 100 TiB (previously 40 TiB)
- **Basic/Standard SKU:** Storage limits unchanged
- **Geo-replication:** Faster data sync speeds for new replicas
- **Monitoring:** Storage consumption visible in Azure Portal's Monitoring tab

## Why the Change?

Organizations are moving quickly toward container-based solutions for composability and operational efficiency. The rise of AI and ML means massive model artifacts and pipeline outputs are increasingly stored in registries, creating new storage needs. Many large organizations, especially those consolidating workloads, have hit or exceeded the previous 40 TiB cap and resorted to complex workarounds like using multiple registries or aggressive artifact cleanup strategies.

## Benefits

- **Simplified Operations:** Avoid multi-registry complexity and custom lifecycle policies
- **Seamless Expansion:** No action required—existing Premium registries now have 100 TiB capacity
- **Improved Geo-Replication:** Faster provisioning of new global replicas
- **Better Visibility:** Portal, CLI, and APIs now show usage vs. new limits

## Who Will Benefit?

- **Enterprise platform teams** managing centralized registries
- **AI/ML teams** storing large model artifacts, training outputs
- **Organizations consolidating workloads** as they migrate to containers
- **Global enterprises** using registry geo-replication

## How to Check Your Usage

- **Azure Portal:** Overview blade > Monitoring tab
- **Azure CLI:**  

  ```
  # View registry storage usage and limits
  az acr show-usage --name myregistry --output table
  ```

- **REST API:** [List Usages REST API](https://learn.microsoft.com/rest/api/container-registry/registries/list-usages)

## Upgrading to Premium

The 100 TiB storage limit is exclusive to Premium SKU. If you’re on Basic or Standard and need higher storage, upgrade using Azure CLI:

```
az acr update --name myregistry --sku Premium
```

## Additional Resources

- [Azure Container Registry service tiers and limits](https://aka.ms/acr/skus)
- [Geo-replication](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-geo-replication)
- [Best practices for ACR](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-best-practices)
- [Registry pricing](https://azure.microsoft.com/en-us/pricing/details/container-registry/)
- [List Usages REST API](https://learn.microsoft.com/rest/api/container-registry/registries/list-usages)

## Summary

The Premium SKU’s 100 TiB storage limit will help enterprises, especially in AI and global work, to scale confidently, reduce operational complexity, and improve registry management in Azure.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-container-registry-premium-sku-now-supports-100-tib/ba-p/4497651)
