---
layout: post
title: Fully Managed Cloud-to-Cloud Transfers with Azure Storage Mover
author: Aung Oo
canonical_url: https://azure.microsoft.com/en-us/blog/fully-managed-cloud-to-cloud-transfers-with-azure-storage-mover/
viewing_mode: external
feed_name: The Azure Blog
feed_url: https://azure.microsoft.com/en-us/blog/feed/
date: 2025-10-23 16:00:00 +00:00
permalink: /ai/news/Fully-Managed-Cloud-to-Cloud-Transfers-with-Azure-Storage-Mover
tags:
- AI Workloads
- AWS S3
- Azure Active Directory
- Azure Blob Storage
- Azure Monitor
- Azure Portal
- Azure Storage Mover
- Cloud Migration
- Cloud To Cloud
- Data Analytics
- Data Transfer
- Incremental Sync
- Log Analytics
- Multicloud
- NFS
- RBAC
- SMB
- Storage
- Storage Migration
section_names:
- ai
- azure
---
Aung Oo outlines the newly available cloud-to-cloud migration support in Azure Storage Mover, enabling effortless transfers from AWS S3 to Azure Blob Storage and unlocking Azure’s analytics and AI features.<!--excerpt_end-->

# Fully Managed Cloud-to-Cloud Transfers with Azure Storage Mover

## Overview

Azure Storage Mover now supports direct cloud-to-cloud migration from AWS S3 to Azure Blob Storage, offering a fully managed solution that eliminates the need for agents or scripts. This capability simplifies the migration process for organizations moving data from File Shares and NAS Storage into Azure Object and File storage with minimal disruption.

## Key Capabilities

- **Direct, Parallel Transfers:** High-speed, server-to-server parallel transfers maximize migration performance, especially for large datasets.
- **Integrated Automation:** Azure portal and CLI provide workflow automation and tracking, without the need for custom scripts or third-party solutions.
- **Secure Migration:** Data transfers are encrypted in transit and integrate with Azure’s security frameworks, including Azure Active Directory and RBAC.
- **Incremental Sync:** After initial migration, only changed files are moved, minimizing operational downtime.
- **Monitoring and Observability:** Progress can be tracked via the Azure portal, CLI, or REST API, with integration into Azure Monitor and Log Analytics for granular telemetry and diagnostics.

## Real-World Impact

Organizations have already migrated petabytes of data during public preview. A noteworthy case is Syncro, with partner SOUTHWORKS, which migrated hundreds of terabytes from AWS S3 to Azure Blob Storage using Storage Mover. The phased migration maintained data integrity and provided immediate access to Azure’s analytics and AI resources.

> Syncro, a leading IT management provider, used Azure Storage Mover for a seamless and secure multi-stage migration from AWS S3 to Azure Blob. The process enabled ongoing phased migrations, ensuring data integrity and enabling rapid adoption of Azure’s advanced features.

## Benefits for AI and Analytics

Migrating data into Azure Blob Storage allows teams to rapidly leverage Azure’s AI and machine learning services. Once migrated, data can be used immediately for model training, advanced analytics, and real-time insights.

## Additional Updates

- Support for on-premises SMB shares to Azure Object Storage
- Migration from on-premises NFS shares to Azure Files NFS 4.1
- Integration with Azure Copilot for storage migration planning
- Upcoming support for Azure US Government regions

## Resources

- [Azure Storage Mover](https://azure.microsoft.com/en-us/products/storage-mover)
- [Azure Blob Storage](https://azure.microsoft.com/en-us/products/storage/blobs/)
- [Storage Migration solutions using Azure Copilot](https://learn.microsoft.com/en-us/azure/copilot/improve-storage-accounts#discover-storage-migration-solutions)
- [Product Update for SMB shares](https://azure.microsoft.com/en-us/updates?id=495546)
- [NFS Update](https://aka.ms/MoverNFSUpdate)
- [Get started with cloud-to-cloud migration](https://aka.ms/cloud2cloud-migration)

Azure Storage Mover empowers organizations to accelerate digital transformation by simplifying secure cloud and on-premises data transfers and unlocking Microsoft’s data, analytics, and AI ecosystem.

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/fully-managed-cloud-to-cloud-transfers-with-azure-storage-mover/)
