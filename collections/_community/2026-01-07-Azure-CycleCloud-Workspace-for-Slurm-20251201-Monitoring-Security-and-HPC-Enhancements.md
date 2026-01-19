---
layout: post
title: 'Azure CycleCloud Workspace for Slurm 2025.12.01: Monitoring, Security, and HPC Enhancements'
author: xpillons
canonical_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-azure-cyclecloud-workspace-for-slurm-version-2025-12/ba-p/4481953
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2026-01-07 09:22:19 +00:00
permalink: /azure/community/Azure-CycleCloud-Workspace-for-Slurm-20251201-Monitoring-Security-and-HPC-Enhancements
tags:
- AlmaLinux 9
- ARM64
- Authentication
- Azure CycleCloud
- Azure Managed Grafana
- Azure Monitor
- Cluster Management
- Entra ID
- Grafana
- High Performance Computing
- HPC
- Linux
- Monitoring
- Open OnDemand
- Prometheus
- Resource Management
- Single Sign On
- Slurm
- Ubuntu 24.04
- User Managed Identity
section_names:
- azure
- security
---
xpillons introduces key improvements in the Azure CycleCloud Workspace for Slurm 2025.12.01 release, highlighting new monitoring capabilities, security enhancements via Entra ID SSO, and expanded platform support for HPC deployments.<!--excerpt_end-->

# Azure CycleCloud Workspace for Slurm 2025.12.01: Major Enhancements

## Overview

The 2025.12.01 release of Azure CycleCloud Workspace for Slurm brings several pivotal features, focusing on monitoring, security, and platform flexibility to better support technical and scientific HPC communities.

## Major Feature Updates

- **Integrated Monitoring with Prometheus and Managed Grafana:**
  - Prometheus self-agent now automates metric collection from compute nodes and Slurm jobs for real-time cluster insights.
  - Azure Managed Grafana offers customizable dashboards for easy visualization of performance data.
  - Monitoring covers GPUs, Infiniband interconnects, CPUs, network, disk, and NFS.

#### How to Set Up Managed Monitoring

1. **Create a Resource Group** for monitoring resources in Azure.
2. **Deploy Infrastructure:**

   ```bash
   git clone https://github.com/Azure/cyclecloud-monitoring.git
   cd cyclecloud-monitoring
   ./infra/deploy.sh <monitoring_resource_group>
   ```

3. **Access Dashboards:**
   - Use the Azure Monitor Workspace (e.g., `ccw-mon-xxx`) and Azure Managed Grafana (`ccw-graf-xxx`).
   - View and customize dashboards in the Grafana portal.

4. **Monitoring Details:**
   - **GPUs:** Utilization, temperatures, clock speeds, ECC errors, NVLink stats.
   - **Infiniband:** Throughput and error monitoring.
   - **Others:** CPU & memory usage, disk, network, file system, NFS operations.

5. **Enable Monitoring During Deployment:**
   - Available in the Marketplace UI for Slurm clusters starting with CycleCloud 8.8.1 via the cluster template.
   - Set the correct Client ID (user managed identity) for node metric permissions, usually `ccwLockerManagedIdentity`.

## Security Upgrade: Entra ID Single Sign-On (SSO)

- **Integrated SSO:** Entra ID SSO centralizes authentication for both CycleCloud UI and Open OnDemand via OpenID Connect.
- **Benefits:**
  - Secure, frictionless login experience.
  - Supports MFA and compliance requirements.
  - Simplified user and access management.

#### How to Enable Entra ID SSO

1. **Pre-deployment:**
   - Register an Entra ID app for authentication.
   - Create a User Managed Identity (UMI) for federated credential management.
   - Assign UMI to Open OnDemand VM as the trusted source.
   - Refer to [entra_instructions](https://github.com/Azure/cyclecloud-slurm-workspace/blob/main/entra_instructions.md) for details.

2. **Deployment:**
   - Enable SSO from the Basics tab in the Azure Marketplace UI during workspace setup.
   - Use output from pre-deployment scripts for required values.

3. **Post-deployment:**
   - Update CycleCloud and Open OnDemand IPs in the Entra ID app (manually or with [entra_postdeploy.sh](https://github.com/Azure/cyclecloud-slurm-workspace/blob/main/util/entra_postdeploy.sh)).
   - Assign users to roles (User, Node Admin, SuperUser, Administrator) via the Enterprise Applications portal.
   - First-time users must log in via CycleCloud UI to provision local cluster accounts.

## Additional Upgrades

- Support for ARM64 compute nodes.
- Compatibility with Ubuntu 24.04 and AlmaLinux 9.
- Improved performance, resource tracking, and user access management.

## Conclusion

This update to Azure CycleCloud Workspace for Slurm streamlines monitoring, strengthens user authentication, and broadens support for modern hardware and operating systems. Organizations can more easily deploy and manage secure, efficient, and highly visible HPC clusters on Azure.

---

**References:**

- [CycleCloud Monitoring Tools](https://github.com/Azure/cyclecloud-monitoring?tab=readme-ov-file#build-the-managed-monitoring-infrastructure)
- [CycleCloud SSO Entra ID Guide](https://github.com/Azure/cyclecloud-slurm-workspace/blob/main/entra_instructions.md)
- [Entra Post-Deploy Script](https://github.com/Azure/cyclecloud-slurm-workspace/blob/main/util/entra_postdeploy.sh)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/announcing-azure-cyclecloud-workspace-for-slurm-version-2025-12/ba-p/4481953)
