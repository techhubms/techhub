---
layout: post
title: 'Windows Server 2019 Retirement on AKS enabled by Azure Arc: Migration Guidance'
author: ssarwa
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/windows-server-2019-retirement-on-aks-enabled-by-azure-arc/ba-p/4470184
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 05:18:55 +00:00
permalink: /azure/community/Windows-Server-2019-Retirement-on-AKS-enabled-by-Azure-Arc-Migration-Guidance
tags:
- AKS
- Azure
- Azure Arc
- Cloud Operations
- Cluster Management
- Community
- Container Migration
- DevOps
- Kubernetes
- Microsoft Azure
- Node Pools
- OS Upgrade
- Support Lifecycle
- Windows Server
- Workload Migration
section_names:
- azure
- devops
---
ssarwa highlights the required migration from Windows Server 2019 to Windows Server 2022 for AKS enabled by Azure Arc, outlining timelines, migration steps, and operational impacts of the upcoming retirement.<!--excerpt_end-->

# Action Required: Migrate to Windows Server 2022 by April 2026

Microsoft will retire support for Windows Server 2019 on Azure Kubernetes Service (AKS) enabled by Azure Arc (formerly AKS Hybrid) after April 1, 2026. To ensure continued support, access to security updates, and the ability to upgrade Kubernetes versions, all customers must migrate workloads to Windows Server 2022 before this date.

## Migration Approach

Migration from Windows Server 2019 node pools involves:

1. **Creating new node pools** using Windows Server 2022
2. **Updating and rebuilding container images** with WS2022 base images
3. **Migrating workloads** to the new node pools
4. **Decommissioning old Windows Server 2019 node pools**

Allocate at least 2-4 weeks for testing, aiming to complete migrations by March 2026.

## Key Milestones

| Milestone                                | Date                | Details                                                                      |
|-------------------------------------------|---------------------|------------------------------------------------------------------------------|
| Windows Server 2019 Support Ends         | Post April 1, 2026  | No support or security updates after this date                               |
| Windows Server 2019 Retirement           | Post April 1, 2026  | Cannot create new node pools with WS2019 in new releases                     |
| Default Image Changes to WS2022          | Early 2026          | New deployments default to Windows Server 2022                               |

## Reason for Change

- **End of Support**: Mainstream support for Windows Server 2019 ended January 9, 2024
- **Streamlined Deployments**: Windows Server 2022 will be the default OS SKU for new deployments from early 2026, providing a more seamless experience

## Post-Retirement Impacts

Starting April 1, 2026, clusters with Windows Server 2019 node pools will face:

- **Kubernetes upgrade blocking**: Upgrades will be blocked with specific error messages if WS2019 node pools are present
- **Node pool creation restrictions**: Cannot create new Windows node pools with WS2019 OS SKU
- **Loss of support**: Microsoft support cases involving clusters running WS2019 will require upgrades before help is provided. No security patches or updates will be available
- **Compliance**: Clusters will be out of compliance without migration

The default OS image for node pools becomes Windows Server 2022 in early 2026 for all new deployments.

## Step-by-Step Migration Guidance

Visit the official [Migration How-to Guide](https://learn.microsoft.com/en-us/azure/aks/aksarc/windows-server-migration-guide) for:

- Identifying clusters that need migration
- Procedures for updating container images
- Creating and migrating node pools
- Troubleshooting and validation best practices

## Frequently Asked Questions

**Q: What if I don't migrate before April 2026?**

- Your clusters will lose support and cannot receive help from Microsoft nor upgrade Kubernetes versions.

**Q: Can I upgrade node pools in place?**

- No, you must create new pools with Windows Server 2022 and migrate workloads.

**Q: Will existing container images work on Windows Server 2022?**

- Most WS2019 images will work, but rebuilding on the WS2022 base is recommended.

**Q: Can I run both WS2019 and WS2022 node pools during migration?**

- Yes, multiple OS SKUs can coexist during migration for phased cutover.

**Q: Will migration cause downtime?**

- Stateless apps using rolling updates may have minimal downtime; stateful workloads may require planned windows.

## Additional Resources

- [AKS enabled by Azure Arc Overview](https://learn.microsoft.com/azure/aks/hybrid/aks-overview)
- [Windows Container Upgrade Guide](https://learn.microsoft.com/azure/aks/upgrade-windows-2019-2022)
- [Original Retirement Announcement](https://techcommunity.microsoft.com/blog/containers/announcing-the-3-year-retirement-of-windows-server-2019-on-azure-kubernetes-serv/3777341)

_Last updated Nov 18, 2025 by ssarwa._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/windows-server-2019-retirement-on-aks-enabled-by-azure-arc/ba-p/4470184)
