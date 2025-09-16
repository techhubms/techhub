---
layout: "post"
title: "AKS Automatic with Azure Linux: Streamlining Kubernetes Operations"
description: "This community post introduces the general availability of AKS Automatic, a new offering on Azure Kubernetes Service (AKS) that automates the operational overhead of cluster management. By default, AKS Automatic utilizes Azure Linux for all node pools, enhancing security, performance, and compliance. The post details features like secure-by-default configuration, CVE management, built-in tooling, and unified support. It also outlines the benefits of using AKS Automatic for efficient Kubernetes deployments and provides resources for getting started."
author: "FloraTaagen"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/aks-automatic-with-azure-linux/ba-p/4454284"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-16 17:37:20 +00:00
permalink: "/2025-09-16-AKS-Automatic-with-Azure-Linux-Streamlining-Kubernetes-Operations.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["AKS", "AKS Automatic", "Azure", "Azure Linux", "CIS Benchmarks", "Cloud Infrastructure", "Cluster Security", "Community", "Container Workloads", "CVE Management", "DevOps", "FedRAMP", "FIPS Compliance", "Kubernetes", "Monitoring Tools", "Node Management", "Operational Efficiency", "Performance Optimization", "Scaling", "Security"]
tags_normalized: ["aks", "aks automatic", "azure", "azure linux", "cis benchmarks", "cloud infrastructure", "cluster security", "community", "container workloads", "cve management", "devops", "fedramp", "fips compliance", "kubernetes", "monitoring tools", "node management", "operational efficiency", "performance optimization", "scaling", "security"]
---

FloraTaagen highlights the GA release of AKS Automatic for Azure Kubernetes Service, showcasing how Azure Linux integration enhances cluster security, performance, and operational simplicity.<!--excerpt_end-->

# AKS Automatic with Azure Linux: Streamlining Kubernetes Operations

**Author:** FloraTaagen  
**Published:** Sep 15, 2025

## Overview

Microsoft has announced the general availability (GA) of [AKS Automatic](https://aka.ms/AKSAutomatic/blog), an automated Kubernetes cluster management solution on Azure Kubernetes Service (AKS). With AKS Automatic, developers and platform teams can build, deploy, and scale applications on Kubernetes efficiently and with minimal operational burden. The default foundation for these clusters is [Azure Linux](https://learn.microsoft.com/en-us/azure/azure-linux/intro-azure-linux), a secure, Azure-optimized operating system for container workloads.

## What is AKS Automatic?

AKS Automatic simplifies the managed Kubernetes experience by handling all aspects of cluster setup, including:

- Node management
- Scaling
- Security
- Networking
- Preconfigured settings aligned with AKS well-architected recommendations

This approach ensures that developers can focus more on application development and less on infrastructure operations.

## Azure Linux: Enhancing Security and Performance

AKS Automatic uses Azure Linux for all user and system node pools, offering several operational benefits:

### CVE Management

- Minimal essential packages included, reducing the attack surface
- Fewer patches and updates required
- Built-in image cleaner to automatically remove unused images with known vulnerabilities

### Secure by Default

- Hardened node image and kernel optimized for Azure
- Default compliance certifications such as FIPS and FedRAMP
- Passes all CIS Level 1 benchmarks by default
- Security-hardened configuration out of the box

### Resiliency

- Automatic patching for nodes and cluster components according to maintenance schedules
- Extensive unit and end-to-end testing for every update
- Reduced package footprint minimizes disruption risk

### Performance

- Lower disk and memory usage due to streamlined operating system images
- Faster operations, including cluster creation, scaling, upgrades, and deletion

### Tooling and Extensibility

- Clusters come preconfigured with monitoring, scaling, security, and networking tools
- Supports all AKS extensions, add-ons, and open-source projects

### Unified Support

- End-to-end Microsoft support for Kubernetes stack, simplifying troubleshooting

## Benefits

- Accelerated innovation on Kubernetes
- Reduced operational complexity and overhead
- Secure, compliant, and high-performance clusters by default

## Getting Started

Getting started is streamlined—go from a container image to a fully deployed, best-practice-compliant application cluster within minutes. View the official [tutorial](https://learn.microsoft.com/en-us/azure/aks/automatic/quick-automatic-managed-network?pivots=azure-cli) for step-by-step instructions.

---

**For more technical information about AKS Automatic and Azure Linux:**

- [AKS Automatic Documentation](https://learn.microsoft.com/en-us/azure/aks/intro-aks-automatic)
- [Azure Linux Overview](https://learn.microsoft.com/en-us/azure/azure-linux/intro-azure-linux)

---

**About the Author:**  
FloraTaagen is a community contributor sharing the latest in Azure and Kubernetes advancements.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/aks-automatic-with-azure-linux/ba-p/4454284)
