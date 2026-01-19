---
layout: post
title: 'Enhancing Resiliency in Azure Compute Gallery: Soft Delete and Zonal Redundant Storage'
author: Sandeep-Raichura
canonical_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/enhancing-resiliency-in-azure-compute-gallery/ba-p/4470082
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 17:24:09 +00:00
permalink: /azure/community/Enhancing-Resiliency-in-Azure-Compute-Gallery-Soft-Delete-and-Zonal-Redundant-Storage
tags:
- Access Control
- Azure Compute Gallery
- Azure Portal
- Business Continuity
- Cloud Resiliency
- Cloud Storage
- Disaster Recovery
- Image Management
- Image Recovery
- REST API
- Soft Delete
- Virtual Machines
- VM Images
- Zonal Redundant Storage
- ZRS
section_names:
- azure
---
Sandeep-Raichura reviews Azure Compute Gallery's latest features—Soft Delete and default Zonal Redundant Storage—demonstrating how they protect VM images and enhance operational resiliency for Azure users.<!--excerpt_end-->

# Enhancing Resiliency in Azure Compute Gallery: Soft Delete and Zonal Redundant Storage

Author: Sandeep-Raichura

## Introduction

Ensuring your cloud resources are resilient and recoverable is crucial for modern organizations. Azure Compute Gallery (ACG) is advancing resource protection through two important features: Soft Delete (in public preview) and Zonal Redundant Storage (ZRS) as the default storage type for image versions. These enhancements significantly reduce the risk of VM image loss and support improved business continuity.

---

## Soft Delete Feature (Public Preview)

Accidental deletion of VM images used to cause major disruptions, requiring time-intensive rebuilds. The new Soft Delete feature acts as a safety net:

- **Grace Period**: Deleted images enter a "soft-deleted" state and are recoverable for up to 7 days.
- **Recovery**: Restore images via Azure Portal or REST API.
- **Role-Based Access**: Only gallery owners or users with the Compute Gallery Sharing Admin role can manage soft-deleted images.
- **No Additional Cost**: Only storage charges apply; the feature itself is free.
- **Broad Support**: Available for Private, Direct Shared, and Community Galleries.

**How to Enable:**

- Update your gallery settings in the Azure Portal or via Azure CLI.
- Soft-deleted images can be listed, restored, or permanently deleted as needed.
- Learn more: [Soft Delete specification](https://aka.ms/sigsoftdelete).

---

## Zonal Redundant Storage (ZRS) By Default

ZRS now serves as the default storage type for new image versions, automatically replicating images across multiple availability zones within a region:

- **Automatic Redundancy**: No manual configuration needed.
- **High Availability**: VM images are protected against a single zone failure.
- **Simplified Management**: Users benefit from resilient storage without managing storage account redundancy settings.
- **Availability**: ZRS as default starts with API version 2025-03-03; Portal and SDK support are coming soon.

---

## Benefits of These Features

- **Reduced Operational Risk**: Protects against accidental deletions and zone outages.
- **Faster Recovery**: Minimized downtime and manual recovery efforts.
- **Secure Operations**: Advanced access controls for recovery and deletion.
- **Compliance Support**: Transparent processes for audit and recovery.

---

## Getting Started

- **Soft Delete**: Register for the preview, activate on galleries via Portal or REST API. Recommended for test environments during preview.
- **ZRS**: Available by default for new image versions (starting API version 2025-03-03).

Detailed, step-by-step instructions are available in the [public specification document](https://aka.ms/sigsoftdelete).

---

## Conclusion

Azure Compute Gallery’s new features—Soft Delete and default ZRS—raise the standard for VM image resiliency and recoverability. These enhancements let users mitigate common risks and ensure reliable VM lifecycle management in the Azure cloud. Stay tuned for further updates as these capabilities progress toward general availability.

*Last updated: Nov 18, 2025 (Version 1.0)*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/enhancing-resiliency-in-azure-compute-gallery/ba-p/4470082)
