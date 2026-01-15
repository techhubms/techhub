---
layout: post
title: 'Public Preview: Sharing Capacity Reservation Groups Across Azure Subscriptions'
author: Tarannum91
canonical_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-for-sharing-capacity-reservation-groups-now/ba-p/4461834
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-15 22:47:39 +00:00
permalink: /azure/community/Public-Preview-Sharing-Capacity-Reservation-Groups-Across-Azure-Subscriptions
tags:
- Azure
- Azure API
- Azure Capacity Reservation Groups
- Azure Reserved Instances
- Azure Virtual Machines
- Centralized Capacity
- Cloud Scalability
- Community
- Cost Optimization
- CRG
- Disaster Recovery
- Public Preview
- Resource Management
- Subscription Management
- Virtual Machine Scale Sets
section_names:
- azure
---
Tarannum91 details the new public preview for sharing Azure Capacity Reservation Groups (CRGs) between subscriptions. This feature enables resource reuse, simplified management, and more efficient scaling for Azure Virtual Machines.<!--excerpt_end-->

# Public Preview: Sharing Capacity Reservation Groups Across Azure Subscriptions

Azure has announced the public preview of the ability to share Capacity Reservation Groups (CRGs) across subscriptions. Previously, CRGs could only be used within a single subscription, limiting resource reuse and centralized management. With this release, organizations can take advantage of shared capacity scenarios to streamline compute management and reduce costs.

## Key Benefits

- **Resource Reuse**: Disaster Recovery Capacity Reservation Groups can now be repurposed for tasks like development testing across multiple subscriptions.
- **Centralized Capacity Management**: Allows organizations to manage capacity centrally but grant access to various teams or departments through separate subscriptions.
- **Cost-Efficient Scaling**: Aligns with scale-out architectures and enables separating security from capacity management.

## How It Works

A minimum of two subscriptions are required:

- **Provider Subscription**: Creates and manages the CRG and its reservations.
- **Consumer Subscription**: Is granted access to the shared capacity, enabling deployment of VMs under the same SLA conditions as the provider.

### Example Steps

1. **Share the CRG**: An administrator in Subscription B (consumer) grants User “A” share permissions on the CRG. User “A”, who owns the CRG, updates the sharing profile to include Subscription B. Each CRG can be shared with up to 100 subscriptions.
2. **Grant Deployment Rights**: An administrator in Subscription A (provider) grants read/deploy permissions to User “B” (VM owner) for the CRG.
3. **Deploy VMs**: User B can deploy VMs in the consumer subscription by specifying the `capacityReservationGroup` property on VMs or Virtual Machine Scale Sets. Deployed VMs must match the CRG on SKU, region, and availability zone.

## Public Preview Limitations

- No Azure Portal support; only API and other Azure clients are supported.
- Re-provisioning VMs in a VMSS within a shared CRG during zone outages is not supported.

## Background

On-demand capacity reservations enable organizations to reserve compute capacity for Azure VMs without commitment. This ensures resource availability for business-critical applications and can be used in combination with Azure Reserved VM Instances to lower costs. The feature covers most VM sizes and supports various Azure regions and availability zones.

## Resources

- [Capacity Reservation Sharing Quickstart](https://aka.ms/computereservationsharing)
- [Documentation and Sample Code](https://aka.ms/on-demand-capacity-reservations-docs)
- [Public Preview Blog (specialty SKUs)](https://aka.ms/on-demand-capacity-reservations-GA-ACOM)
- [Video Overview](https://www.youtube.com/watch?v=9dd65rTbbWY)
- [Detailed Documentation](https://docs.microsoft.com/en-us/azure/virtual-machines/capacity-reservation-overview)

*Posted by Tarannum91 on the Azure Compute Blog (October 15, 2025)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/public-preview-for-sharing-capacity-reservation-groups-now/ba-p/4461834)
