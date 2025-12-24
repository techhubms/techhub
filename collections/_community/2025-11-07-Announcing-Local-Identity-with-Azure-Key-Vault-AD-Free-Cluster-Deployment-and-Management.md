---
layout: "post"
title: "Announcing Local Identity with Azure Key Vault: AD-Free Cluster Deployment and Management"
description: "This post introduces the public preview of Local Identity integration with Azure Key Vault, enabling organizations to deploy Azure local clusters without Active Directory dependency. Learn about streamlined key management, compatibility with partner ecosystems, and new private preview features such as the Management Toolkit and Internal DNS, aimed at simplifying cluster administration and internal name resolution for Microsoft environments."
author: "ShireenIsab"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-architecture-blog/introducing-local-identity-with-azure-key-vault-in-build-2510/ba-p/4467939"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-07 23:55:58 +00:00
permalink: "/community/2025-11-07-Announcing-Local-Identity-with-Azure-Key-Vault-AD-Free-Cluster-Deployment-and-Management.html"
categories: ["Azure", "Security"]
tags: ["Active Directory", "Azure", "Azure Key Vault", "Azure Local Portal", "Backup Integration", "Cluster Deployment", "Community", "Commvault", "Dell", "Internal DNS", "Key Management", "Lenovo", "Local Identity", "Management Toolkit", "Partner Ecosystem", "Security", "Veeam"]
tags_normalized: ["active directory", "azure", "azure key vault", "azure local portal", "backup integration", "cluster deployment", "community", "commvault", "dell", "internal dns", "key management", "lenovo", "local identity", "management toolkit", "partner ecosystem", "security", "veeam"]
---

ShireenIsab introduces Local Identity with Azure Key Vault, detailing how it enables simplified, AD-free cluster deployments and discusses new management and DNS features now in preview.<!--excerpt_end-->

# Announcing Local Identity with Azure Key Vault: AD-Free Cluster Deployment and Management

**Author:** ShireenIsab

## Overview

Microsoft has launched a public preview for Local Identity integrated with **Azure Key Vault**, enabling users to deploy Azure local clusters without dependency on Active Directory (AD). This new approach streamlines key management, boosts partner compatibility, and simplifies cluster deployment and management in Microsoft environments.

## Key Features

### 1. Active Directory Independence

- Deploy clusters without needing Active Directory, removing dependencies on existing infrastructure.
- Simplifies initial setup and reduces management complexity for environments where AD is unnecessary or undesired.

### 2. Azure Key Vault Integration

- Seamlessly back up and manage keys using Azure Key Vault regardless of AD presence.
- Strengthens security posture by centralizing key management in a trusted Microsoft cloud service.

### 3. Partner Ecosystem Compatibility

- Designed to work with backup solutions such as **Veeam** and **Commvault**.
- Collaboration with hardware vendors like **Dell** and **Lenovo** to ensure broad support and integration in various environments.

## Private Preview Features

Two new features are now available in private preview:

### 1. Management Toolkit

- Provides a guided setup for configuring admin workstations to manage clusters securely, even without AD.
- **Highlights:**
  - Aligns local admin accounts and installs required certificates
  - Applies safe defaults for protocols, authentication, and firewall settings
  - Offers helpers for MMC tools (e.g., Failover Cluster Manager, Hyper-V Manager)
- Augments existing tools rather than replacing them, enabling seamless administration.

### 2. Internal DNS

- Enables internal name resolution within the cluster, negating the need for AD.
- **Overview:**
  - Simplifies hostname resolution for nodes, virtual machines, and services
  - Flexible resource consumption adjustments for constrained environments
  - Can be enabled via the Azure Local portal during deployment

## How to Get Started

- Ensure deployment on **build 2510 or later**
- Explore Local Identity with Azure Key Vault integration
- Participate in the private preview for Management Toolkit and Internal DNS by contacting [azurelocalidentity@microsoft.com](mailto:azurelocalidentity@microsoft.com)
- Provide feedback and suggestions via the same contact

## Additional Information

- The Management Toolkit and Internal DNS are in private preview and require explicit sign-up.
- Full documentation and deployment guides are available at the [Azure documentation portal](https://learn.microsoft.com/en-us/azure/azure-local/deploy/deployment-local-identity-with-key-vault).

---

*Published by ShireenIsab on November 7, 2025. For further updates and related discussions, follow the Azure Architecture Blog.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/introducing-local-identity-with-azure-key-vault-in-build-2510/ba-p/4467939)
