---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-cni-overlay-for-application-gateway-for-containers-and/ba-p/4449350
title: 'General Availability: Azure CNI Overlay for Application Gateway for Containers and AGIC'
author: jonw
feed_name: Microsoft Tech Community
date: 2025-08-29 16:12:02 +00:00
tags:
- AKS
- Application Gateway For Containers
- Application Gateway Ingress Controller
- Azure CNI Overlay
- Azure Networking
- Azure VNet
- Container Networking
- Ingress Controller
- IPAM
- Kubenet
- L7 Load Balancing
- Pod Routing
- Scalability
section_names:
- azure
- devops
---
jonw announces the general availability of Azure CNI Overlay support for Application Gateway for Containers and AGIC, explaining how this benefits AKS users through enhanced IP scalability and high-performance ingress.<!--excerpt_end-->

# General Availability: Azure CNI Overlay for Application Gateway for Containers and AGIC

**Author:** jonw

## Overview

The Azure team has announced the general availability (GA) of Azure CNI Overlay support for both Application Gateway for Containers and Application Gateway Ingress Controller (AGIC) on Azure Kubernetes Service (AKS). This update is aimed at improving network IP scalability and offering a performant layer 7 (L7) load-balancing solution for containerized workloads.

## What Are Azure CNI Overlay and Application Gateway?

- **Azure CNI Overlay:** Utilizes logical network spaces for pod IP assignment (IPAM), minimizing management overhead and boosting IP scalability. In this mode, nodes receive IPs from the Azure VNet, while pods are assigned IPs from an overlay range, supporting direct communication and efficient IP reuse across clusters.
- **Application Gateway for Containers:** Modern L7 load balancer optimized for AKS, providing scalable control and data planes tailored for high-performance container workloads.

## Why Is This Feature Important?

- **Solves Network Scalability Challenges:** Large-scale container environments often face IP exhaustion and require efficient ingress management. The overlay approach enables pod-to-pod communication at scale, with over a million IP addresses available, and allows IP reuse across clusters.
- **Supports Transition from Kubenet:** As kubenet is being retired, Azure CNI Overlay is now the default for AKS. This ensures feature parity and business continuity for customers upgrading their networking model.
- **Enterprise-Level Ingress:** By combining Azure CNI Overlay with Application Gateway for Containers or AGIC, customers receive a highly scalable and reliable ingress system, eliminating the need for custom or third-party networking solutions.

## Key Benefits

- **High Scale Networking:** Direct Azure-native routing for pods, no encapsulation overhead, reduced IP management complexity.
- **Integrated Ingress:** Modern, scalable L7 load balancing for sophisticated app deployment scenarios.
- **Operational Continuity:** Smooth transition for users moving from kubenet with feature parity and strong support.

## Getting Started & Resources

- [Azure CNI Overlay Documentation](https://learn.microsoft.com/en-us/azure/aks/azure-cni-overlay?tabs=kubectl)
- [Application Gateway for Containers Overview](https://learn.microsoft.com/en-us/azure/application-gateway/for-containers/overview)
- [How to Upgrade AKS Cluster IPAM](https://learn.microsoft.com/en-us/azure/aks/upgrade-azure-cni?tabs=azure-cni)
- [AKS Product Page](https://azure.microsoft.com/en-us/products/kubernetes-service/)
- [Application Gateway Product Page](https://azure.microsoft.com/en-us/products/application-gateway/#overview)

*Version 2.0 — Updated August 29, 2025*

---
For more from jonw and the Azure Networking Blog, follow the [Azure Networking Blog](/category/azure/blog/azurenetworkingblog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/azure-cni-overlay-for-application-gateway-for-containers-and/ba-p/4449350)
