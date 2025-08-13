---
layout: "post"
title: "Understanding Costs and Storage for Azure Cloud Shell and File Persistence"
description: "The article addresses user questions about the costs and availability of cloud storage for files in Azure Cloud Shell, specifically for az & PowerShell usage in admin.microsoft.com. It explores license implications, storage costs for the Cloud Shell home directory, and whether E5 licenses include additional space or typically result in extra charges."
author: "smydsmith"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mbuxp4/what_is_cost_and_how_do_have_cloud_space_for/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-07-28 22:48:41 +00:00
permalink: "/2025-07-28-Understanding-Costs-and-Storage-for-Azure-Cloud-Shell-and-File-Persistence.html"
categories: ["Azure"]
tags: ["Admin.microsoft.com", "Azure", "Azure Cloud Shell", "Azure Storage", "Cloud Costs", "Cloud Storage Pricing", "Community", "E5 License", "Ephemeral Storage", "File Persistence", "PowerShell", "Storage Accounts"]
tags_normalized: ["admin dot microsoft dot com", "azure", "azure cloud shell", "azure storage", "cloud costs", "cloud storage pricing", "community", "e5 license", "ephemeral storage", "file persistence", "powershell", "storage accounts"]
---

Authored by smydsmith, this article discusses the costs and details of using Azure Cloud Shell's file storage, specifically addressing questions about licensing, pricing, and storage persistence within Azure environments.<!--excerpt_end-->

## Introduction

The post by smydsmith explores practical questions regarding storage costs and setup for using Azure Cloud Shell, specifically in the context of admin.microsoft.com and file persistence for az and PowerShell files.

## Key Points

- **Cloud Shell Storage:** When launching Azure Cloud Shell for the first time, users are given the option to make files ephemeral (temporary) or persistent (permanent). However, the prompt does not clarify the associated costs for this storage.
- **E5 License Implications:** The author wonders if an E5 license (a Microsoft 365 licensing tier) includes cloud storage for Azure Cloud Shell, or if users are charged separately based on the amount of space used.
- **Storage Pricing:** According to available documentation, Azure Cloud Shell as a service is free to use, but users incur costs for the Azure Storage account used to persist files. Typically, the main charge is for the 5GB home directory image and any additional files. Charges are minimal (a few cents per month for basic usage).
- **Lack of Clarity:** The author notes that search results and documentation commonly reference a 5GB home drive but do not make clear how this is billed (to the company, the user, or if a new payment method is required).

## Summary

- _Azure Cloud Shell is free to use as a service._
- _Persistent storage, where command-line files and scripts are saved, requires an Azure Storage account._
- _Storage account costs are based on how much storage is used (e.g., the 5GB home drive), not bundled with Microsoft 365 E5 licenses._
- _Payment for the storage account is typically linked to the company's Azure subscription and billing arrangements._

## Practical Implications for Users

- When first configuring Azure Cloud Shell, you must create or link an Azure Storage account for file persistence.
- The cost for the initial 5GB home directory is low, and typical usage incurs only a small monthly fee.
- Licensing such as E5 does not provide Azure Storage for Cloud Shell; a separate Azure subscription and storage account is required.
- Costs are billed to the Azure subscription associated with your organization's tenancy unless otherwise configured.

## Additional Notes

- Always review your Azure subscription’s billing and quota settings to monitor storage costs.
- Documentation may not always clarify these billing nuances, so consult Azure pricing calculators or your organization's Azure administrator as needed.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mbuxp4/what_is_cost_and_how_do_have_cloud_space_for/)
