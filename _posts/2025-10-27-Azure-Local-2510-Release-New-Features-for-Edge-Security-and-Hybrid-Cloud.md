---
layout: "post"
title: "Azure Local 2510 Release: New Features for Edge, Security, and Hybrid Cloud"
description: "This blog post by Thomas Maurer highlights significant new capabilities in the Azure Local 2510 release, including enhancements around edge resilience, security, deployment flexibility, and hybrid cloud operations. It provides an overview of General Availability (GA) and preview features such as Software Defined Networking (SDN) with NSG, rack-aware clusters, local identity deployment, trusted VM guest attestation, and the Well-Architected Review Assessment, with references to official documentation for further detail."
author: "Thomas Maurer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.thomasmaurer.ch/2025/10/azure-local-2510-release-and-new-preview-features/"
viewing_mode: "external"
feed_name: "Thomas Maurer's Blog"
feed_url: "https://www.thomasmaurer.ch/feed/"
date: 2025-10-27 09:05:14 +00:00
permalink: "/2025-10-27-Azure-Local-2510-Release-New-Features-for-Edge-Security-and-Hybrid-Cloud.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Arc", "Azure Local", "Cloud", "Deployment", "Edge Computing", "Hybrid Cloud", "Microsoft", "Microsoft Azure", "Network Security Groups", "Posts", "Rack Aware Cluster", "SDN", "Security", "Security Updates", "Sovereign Cloud", "Trusted VM", "Virtualization", "Well Architected Framework"]
tags_normalized: ["azure", "azure arc", "azure local", "cloud", "deployment", "edge computing", "hybrid cloud", "microsoft", "microsoft azure", "network security groups", "posts", "rack aware cluster", "sdn", "security", "security updates", "sovereign cloud", "trusted vm", "virtualization", "well architected framework"]
---

Thomas Maurer details the Azure Local 2510 release, covering new security and deployment features for edge and hybrid cloud environments. The post is especially useful for architects and IT professionals working with Microsoft Azure Local.<!--excerpt_end-->

# Azure Local 2510 Release: New Features for Edge, Security, and Hybrid Cloud

**Author:** Thomas Maurer  
**Date:** October 27, 2025

Microsoft has released the Azure Local 2510 update, introducing a wave of new and enhanced features focused on edge resiliency, security, and deployment flexibility for hybrid and sovereign cloud scenarios.

## Release Highlights

- **Software Defined Network (SDN) with Network Security Groups (NSG) [GA]**  
  - Enables creation and management of network security groups and rules for Azure Local VMs  
  - Improves segmentation and network security on-premises  
  - [Learn more](https://learn.microsoft.com/en-us/azure/azure-local/concepts/sdn-overview?view=azloc-2510)

- **Rack Aware Cluster (Public Preview)**  
  - Allows you to define local availability zones based on physical racks  
  - Enhances cluster resilience and protects critical workloads from rack-level failures  
  - [Learn more](https://learn.microsoft.com/en-us/azure/azure-local/concepts/rack-aware-cluster-overview?view=azloc-2510)

- **Deployment using Local Identity (Public Preview)**  
  - Also known as AD-Less Deployment  
  - Simplifies deployment by reducing dependency on external identities  
  - Useful for streamlining edge infrastructure rollouts  
  - [Learn more](https://learn.microsoft.com/en-us/azure/azure-local/deploy/deployment-local-identity-with-key-vault?view=azloc-2510)

- **Trusted Virtual Machine Guest Attestation (Public Preview)**  
  - Verifies the VM boot chain integrity (firmware, OS loader, drivers)  
  - Helps ensure VMs start securely and without unauthorized modifications  
  - [Learn more](https://learn.microsoft.com/en-us/azure/azure-local/manage/trusted-launch-vm-overview?view=azloc-2510)

- **Azure Local Well-Architected Review Assessment**  
  - A framework-based self-assessment for operational excellence, reliability, security, cost optimization, and performance efficiency  
  - [Read more](https://techcommunity.microsoft.com/blog/AzureArchitectureBlog/optimize-azure-local-using-insights-from-a-well-architected-review-assessment/4458433)

## Security Focus

Several features in this release address network and platform security concerns:

- **NSG and SDN support:** Centralizes security policy management for internal Azure Local workloads.
- **Trusted Launch attestation:** Ensures confidence in VM start-up integrity.
- **Examples:** For regulated or sovereign environments, these features reduce risk and ease compliance.

## Who Should Read This?

- IT architects and admins designing hybrid or edge solutions
- Azure Local users looking for up-to-date features
- Organizations with requirements around sovereign cloud, compliance, or edge deployment

## Additional Resources

- [Features and improvements in 2510](https://learn.microsoft.com/en-us/azure/azure-local/whats-new?view=azloc-2510)
- [Security updates for Azure Local](https://learn.microsoft.com/en-us/azure/azure-local/security-update/security-update?view=azloc-2510&viewFallbackFrom=azloc-25010&preserve-view=true&tabs=os-build-25398-xxxx)

---

_Thomas Maurer is the EMEA Global Black Belt (GBB) for Sovereign Cloud at Microsoft, specializing in scalable architecture and hybrid cloud strategies._

[Thomas Maurer Cloud and Datacenter Blog](https://www.thomasmaurer.ch/)

This post appeared first on "Thomas Maurer's Blog". [Read the entire article here](https://www.thomasmaurer.ch/2025/10/azure-local-2510-release-and-new-preview-features/)
