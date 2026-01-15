---
layout: post
title: 'Azure Native Pure Storage Cloud: Integrating Enterprise Block Storage with Azure'
author: karautenMSFT
canonical_url: https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-native-pure-storage-cloud-brings-the-best-of-pure-and/ba-p/4456246
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-25 16:00:00 +00:00
permalink: /azure/community/Azure-Native-Pure-Storage-Cloud-Integrating-Enterprise-Block-Storage-with-Azure
tags:
- Azure
- Azure Native Integration
- Azure Storage
- Azure VMware Solution
- Cloud Infrastructure
- Cloud Storage
- Community
- Data Management
- Enterprise Block Storage
- Hybrid Cloud
- Premium Managed Disks
- Pure Storage Cloud
- Snapshot Replication
- Storage as A Service
- VMware Integration
- VNet Peering
section_names:
- azure
---
karautenMSFT presents an in-depth overview of the Azure Native Pure Storage Cloud, describing how Azure and Pure Storage engineering teams collaborated to provide an enterprise-grade, high-performance external storage solution natively integrated with Azure services.<!--excerpt_end-->

# Azure Native Pure Storage Cloud: Bringing Azure and Pure Storage Together

Azure Native Pure Storage Cloud is the result of a close integration effort between Microsoft Azure and Pure Storage teams. The goal was to bring enterprise-grade storage and advanced data management capabilities directly into the Azure ecosystem, leveraging Azure's infrastructure while maintaining Pure Storage's trusted data services.

## Tight Azure Integration for Partners

The [Azure Native Integrations](https://aka.ms/azurenativeisvs) program allows partner solutions like [Pure Storage Cloud](https://aka.ms/ani/purestorage) to operate within Azure management frameworks. This enables the deployment and operation of industry-leading storage with the same tools used to manage Azure VMs, storage accounts, and Kubernetes clusters.

## Meeting Diverse Storage Needs

Azure offers a wide range of storage options (block, file, object), but customers with hybrid cloud environments or specific needs might rely on advanced ISV solutions. Through collaborative engineering with partners, Microsoft enables joint solutions that run natively on Azure infrastructure. The collaboration with Pure Storage produced the first enterprise block storage service built for a public cloud, offering a viable, high-performing alternative for customers migrating from on-premises VMware solutions.

## Combining the Strengths of Azure and Pure Storage

Pure Storage Cloud is built to exploit the latest Azure infrastructure features. Pure worked closely with Azure’s Compute, Network, and Storage teams to validate and integrate new capabilities (such as Azure Boost, Shared Disks, and Premium Managed SSD v2), ensuring high performance and reliability. The two companies continue to align on future Azure roadmap items to optimize storage service outcomes.

## Simplifying VMware Storage Integration

The solution reduces operational overhead for adding and managing storage in Azure VMware Solution deployments. By making workflows and storage hierarchy more intuitive and minimizing the number of required steps, Pure Storage Cloud enables rapid and reliable provisioning of storage resources.

## Streamlined Networking and Native Data Management

Pure Storage Cloud, offered as Storage-as-a-Service, simplifies complex networking within Azure. Instead of difficult cross-tenant VNet peering, customers receive direct Pure Storage endpoints in their chosen VNets. Security controls allow the restriction of volume access to specific hosts. Features include:

- Full Pure Storage data management suite (snapshots, cross-zone/region replication, thin provisioning)
- Simplified volume growth management
- Secure, performant connections within Azure environments

## Support and Future Developments

While development was a joint project, **Pure Storage provides direct support** as the primary contact, with Azure Support involved as needed. The article notes Pure’s high customer satisfaction and offers links to support resources and documentation.

This release is the foundation for future integrations, with plans to address additional scenarios, showcasing Pure’s ongoing innovation in the storage space.

## Demos and Getting Started

Developers and architects can view end-to-end demos on this [YouTube Playlist](https://youtube.com/playlist?list=PLVr149Yb_wxGpN_Y6fEMkh_D_SYzWtl13&si=G2g7jqJPEQpS37UI) and try Pure Storage Cloud free for 30 days through the [Azure Marketplace](https://azuremarketplace.microsoft.com/marketplace/apps/purestoragemarketplaceadmin.krypton_3_plan?tab=Overview).

## Additional Resources

- [Pure Storage GA Blog](https://blog.purestorage.com/solutions/pure-storage-cloud-azure-native-ga/)
- [Azure Native Integration Documentation](https://aka.ms/ani/purestorage)
- [Pure Support Documentation](https://support.purestorage.com/bundle/m_azure_native_pure_storage_cloud/page/Pure_Cloud_Block_Store/Azure_Native_Pure_Storage_Cloud/topics/c_azure_native_pure_storage_cloud.html?uj=%5B%7B%22path%22%3A%22%2Fcategory%2Fm_pure_cloud_block_store%22%2C%22title%22%3A%22Pure+Cloud+Block+Store%22%7D%5D)
- [Capacity and Performance Overview](https://support.purestorage.com/bundle/m_azure_native_pure_storage_cloud/page/Pure_Cloud_Block_Store/Azure_Native_Pure_Storage_Cloud/design/c_performance_in_psc.html)
- [Azure Storage Partners](https://aka.ms/azurestoragepartners)
- [Azure VMware Solution Storage Options](https://learn.microsoft.com/en-us/azure/azure-vmware/ecosystem-external-storage-solutions)

---

**Author:** karautenMSFT

**Version:** 1.0 (Updated Sep 25, 2025)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-storage-blog/azure-native-pure-storage-cloud-brings-the-best-of-pure-and/ba-p/4456246)
