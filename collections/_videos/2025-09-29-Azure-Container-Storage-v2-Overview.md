---
layout: "post"
title: "Azure Container Storage v2 Overview"
description: "This video by John Savill's Technical Training provides a comprehensive overview of Azure Container Storage (ACStor) v2, focusing on new features, architectural updates, performance improvements, and practical guidance for Azure Kubernetes Service (AKS) environments. It covers the use of local NVMe storage, VM SKUs, local disks, durability, and workloads best suited for ACStor v2. The content also addresses migration, cost considerations, and technical prerequisites for deploying and managing container storage in the cloud."
author: "John Savill's Technical Training"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=v6j0lJYdPU4"
viewing_mode: "internal"
feed_name: "John Savill's Technical Training"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCpIn7ox7j7bH_OFj7tYouOQ"
date: 2025-09-29 13:13:51 +00:00
permalink: "/videos/2025-09-29-Azure-Container-Storage-v2-Overview.html"
categories: ["Azure"]
tags: ["ACStor", "AKS", "Azure", "Azure Cloud", "Azure Container Storage", "Cloud", "Cloud Storage", "Container Storage Interface", "Containers", "CSI", "Durability", "Kubernetes", "Local", "Local Disks", "Microsoft", "Microsoft Azure", "NVMe", "Performance", "Storage Architecture", "Videos", "VM SKUs"]
tags_normalized: ["acstor", "aks", "azure", "azure cloud", "azure container storage", "cloud", "cloud storage", "container storage interface", "containers", "csi", "durability", "kubernetes", "local", "local disks", "microsoft", "microsoft azure", "nvme", "performance", "storage architecture", "videos", "vm skus"]
---

John Savill's Technical Training presents an in-depth look at Azure Container Storage v2, highlighting new features and best practices for AKS and Kubernetes environments.<!--excerpt_end-->

{% youtube v6j0lJYdPU4 %}

# Azure Container Storage v2 Overview

**Presented by John Savill's Technical Training**

## Introduction

This session explores the latest enhancements in Azure Container Storage (ACStor) v2, designed for modern container workloads running on Azure Kubernetes Service (AKS).

## Key Topics Covered

- **AKS and CSI (Container Storage Interface):** Context around storage integration for Kubernetes clusters in Azure
- **ACStor v1 vs v2:** Architectural differences, new features
- **Local NVMe Storage:** Benefits and use cases for leveraging fast local storage on VMs
- **VM SKUs:** Guidance on VM size selection for optimal ACStor v2 performance
- **Local Disks and Striping:** Techniques for improving IOPS and throughput by using disk striping
- **Best-fit Workloads:** Identifying the scenarios and workloads that benefit most from ACStor v2
- **Durability Considerations:** Recommendations around data protection, durability, and best practices
- **Performance Improvements:** Benchmarks and comparisons to previous versions
- **Demo:** Step-by-step walkthrough demonstrating ACStor v2 configuration and usage
- **Local CSI Driver:** Updates to the storage interface driver to support new features
- **Operational Changes:** Reduced node minimums, no migration paths from v1, and zero-cost implications
- **Post-GA Features:** Future roadmap and general availability plans

## Resources and Further Learning

- [Whiteboard Overview](https://github.com/johnthebrit/RandomStuff/raw/master/Whiteboards/ACStorv2.png)
- [Recommended AKS & Azure Learning Path](https://learn.onboardtoazure.com)
- [Certification Content](https://github.com/johnthebrit/CertificationMaterials)
- [Weekly Azure Update Playlist](https://youtube.com/playlist?list=PLlVtbbG169nEv7jSfOVmQGRp9wAoAM0Ks)
- [DevOps Master Class](https://youtube.com/playlist?list=PLlVtbbG169nFr8RzQ4GIxUEznpNR53ERq)
- [PowerShell Master Class](https://youtube.com/playlist?list=PLlVtbbG169nFq_hR7FcMYg32xsSAObuq8)
- [FAQ](https://savilltech.com/faq)

## Conclusion

Azure Container Storage v2 provides major improvements for container environments in Azure, especially for AKS users needing high-performance, scalable, and cost-effective storage solutions. Watch the demo for hands-on configuration guidance and consult the linked resources for more advanced learning.
