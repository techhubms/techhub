---
layout: post
title: Strategies for Migrating Backup Data from Datto Cloud to Azure
author: Armlessbastard
canonical_url: https://www.reddit.com/r/AZURE/comments/1mccydp/moving_backup_data_from_datto_cloud_to_azure/
viewing_mode: external
feed_name: Reddit Azure
feed_url: https://www.reddit.com/r/azure/.rss
date: 2025-07-29 14:29:22 +00:00
permalink: /azure/community/Strategies-for-Migrating-Backup-Data-from-Datto-Cloud-to-Azure
tags:
- Azcopy
- Azure
- Backup Data
- Backup Provider Change
- Cloud Migration
- Community
- Containers
- Data Transfer
- Datto
- Restore Points
- VHDX Compatibility
section_names:
- azure
---
Armlessbastard outlines the challenges and considerations in moving backup data from Datto cloud or on-prem devices to Azure, seeking community advice and sharing preliminary solutions.<!--excerpt_end-->

## Overview

The article by Armlessbastard discusses a real-world scenario where an organization needs to rapidly move backup data from Datto cloud/on-prem devices to Microsoft Azure. The author addresses the complexity of this requirement, noting insufficient time for comprehensive planning and the lack of readily available solutions tailored to this situation.

## Challenge Description

- The company has requested a quick migration of backup data from Datto to Azure.
- The author has communicated the difficulty of meeting the project's timeframe but is still expected to provide a rough migration approach.
- There is limited guidance available for this specific scenario, with the author unable to find directly comparable cases or ready-made community advice.

## Proposed Solution

- The suggested approach involves creating Azure containers as target storage locations.
- The author considers mounting each Datto restore point to a server in Azure, or possibly a local service point, and then using `azcopy` to transfer the data from Datto to Azure.
- A complication arises because Datto's VHDX snapshots are not compatible with Azure VMs in a way that allows easy mounting, making direct migration more challenging.

## Vendor Support and Alternatives

- The author speculates that when changing backup providers, arrangements can often be made with the original vendor to facilitate data transfer—either directly or via third-party products.
- They note that another provider, Veeam, hypothetically could handle the data transfer from Datto, suggesting vendor-vendor transitions sometimes offer support for such migrations.

## Request for Community Feedback

- The post concludes with an appeal for advice or shared experiences from others who have migrated backup data from Datto to Azure, especially regarding accepted tools, best practices, or negotiation tips with current providers.

## Key Considerations

- Data transfer approach: Using Azure Storage containers and azcopy for manual migration.
- Compatibility: Issues with Datto’s VHDX snapshots and Azure VM mounting.
- Vendor involvement: Importance of negotiating migration terms with existing and new backup vendors.
- Community engagement: Seeking peer input on effective and common patterns for such migrations.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mccydp/moving_backup_data_from_datto_cloud_to_azure/)
