---
layout: post
title: Zone Redundancy Now Default for Azure Container Registry in Availability Zone Regions
author: YiZha
canonical_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/zone-redundancy-is-now-enabled-by-default-in-azure-container/ba-p/4450618
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-08 07:00:00 +00:00
permalink: /azure/community/Zone-Redundancy-Now-Default-for-Azure-Container-Registry-in-Availability-Zone-Regions
tags:
- ACR
- Availability Zones
- Azure Container Registry
- Cloud Infrastructure
- Cloud Resilience
- Container Registry
- Disaster Recovery
- High Availability
- Microsoft Azure
- Zone Redundancy
section_names:
- azure
---
YiZha announces a major resiliency update to Azure Container Registry: zone redundancy is now enabled by default and at no extra cost on all ACR SKUs in supported regions.<!--excerpt_end-->

# Zone Redundancy Now Default for Azure Container Registry

Azure Container Registry (ACR) has introduced a significant resilience upgrade: **zone redundancy** is now enabled by default for all ACR SKUs in regions supporting Availability Zones. This means enhanced fault tolerance for your container images and artifacts without requiring any configuration changes or incurring additional costs.

## What Has Changed?

- **Previously**, zone redundancy was exclusive to the Premium SKU.
- **Now**, all SKUs—Basic, Standard, and Premium—benefit from zone redundancy if deployed in a region with Availability Zones.
- This update applies instantly and automatically to all new and existing registries in eligible regions, with **zero action required** from customers.

## What Does Zone Redundancy Provide?

- **Improved resilience:** Your registry's data is automatically replicated across multiple physical zones inside a region.
- **Protection against single-zone outages:** Minimizes downtime and data loss risk if a zone fails.
- **No configuration needed:** Zone redundancy is enabled by default; no manual setting or update required.
- **No extra cost:** This feature incurs no additional charges—it's now built into every supported ACR deployment.

## Current Portal and CLI Experience

- The Azure portal and CLI may not immediately reflect that zone redundancy is enabled (the property may still show as `false`).
- Despite the current portal/API visuals, **zone redundancy is active** for all registries in supported regions.
- Microsoft is working to update management interfaces to show accurate zone redundancy status.

## FAQ Highlights

**Q: What's the main benefit?**  
A: Your registry is automatically more resilient to single-zone failures at no additional cost or effort.

**Q: Do I need to change my SKU or settings?**  
A: No. All SKUs now benefit in regions with Availability Zone support.

**Q: Where can I see which regions are supported?**  
A: For the latest list, visit [Zone-Redundant Registry in Azure Container Registry - Microsoft Learn](https://aka.ms/acr/az).

## Additional Resources

- [Zone-Redundant Registry in Azure Container Registry - Microsoft Learn](https://aka.ms/acr/az)
- [Microsoft Developer Community Blog](/category/azure/blog/azuredevcommunityblog)

---
*Authored by YiZha, Microsoft Developer Community, updated Sep 3, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/zone-redundancy-is-now-enabled-by-default-in-azure-container/ba-p/4450618)
