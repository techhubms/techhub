---
layout: "post"
title: "Optimizing Linux Deployments: Performance and Security on Azure"
description: "This advanced session from Microsoft Ignite 2025 presents techniques for deploying, securing, and optimizing Linux environments—Ubuntu, Red Hat, SLES, and Rocky—on Azure. It covers image creation, workload hardening, performance monitoring, and compliance using Azure-native solutions including Azure Monitor, Defender for Linux, and PostgreSQL. The session features demos on Red Hat-based image deployment, license integration, network inspection with eBPF, and system-level performance tuning. Speakers highlight open source partnerships and the value of Microsoft’s cloud for Linux workloads."
author: "Microsoft Events"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=scrkmm03K88"
viewing_mode: "internal"
feed_name: "Microsoft Events YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCrhJmfAGQ5K81XQ8_od1iTg"
date: 2025-11-24 12:01:44 +00:00
permalink: "/2025-11-24-Optimizing-Linux-Deployments-Performance-and-Security-on-Azure.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure Best Practices", "Azure Monitor", "Cloud Security", "Compliance", "Defender For Linux", "DevOps", "Ebpf", "Inspector Gadget", "Linux On Azure", "Microsoft Ignite", "Migrateandmodernizeyourestate", "MSIgnite", "Open Source", "Performance Tuning", "PostgreSQL On Azure", "Red Hat", "Rocky Linux", "Security", "Security Hardened Images", "SLES", "Ubuntu", "Videos", "VM Deployment"]
tags_normalized: ["azure", "azure best practices", "azure monitor", "cloud security", "compliance", "defender for linux", "devops", "ebpf", "inspector gadget", "linux on azure", "microsoft ignite", "migrateandmodernizeyourestate", "msignite", "open source", "performance tuning", "postgresql on azure", "red hat", "rocky linux", "security", "security hardened images", "sles", "ubuntu", "videos", "vm deployment"]
---

Microsoft Events' Ignite session with Karl Abbott and Lachlan Evenson explores deploying, securing, and optimizing Linux workloads on Azure using native tools and partner integrations.<!--excerpt_end-->

{% youtube scrkmm03K88 %}

# Optimizing Linux Deployments: Performance and Security on Azure

## Overview

This Microsoft Ignite 2025 breakout session, led by Karl Abbott and Lachlan Evenson, focuses on advanced techniques for managing Linux environments on Azure—ranging from Ubuntu to Rocky Linux. It highlights Microsoft's open source contributions and the technical strategies for creating robust, performant, and secure cloud-native Linux architectures.

## Key Topics

- **Azure-Native Tools for Linux**: Utilizing Azure Monitor and Defender for Linux to stay compliant, monitor, and secure workloads.
- **Streamlining Image Creation**: How to deploy and customize Red Hat-based images and integrate existing licenses.
- **Workload Hardening & Compliance**: Implementing secure baselines for VM images and maintaining compliance.
- **Performance Monitoring & Tuning**:
  - Using eBPF and Inspector Gadget for deep packet and system inspection.
  - Network performance tuning (TCP optimizations, configuration changes), with practical demos of applying tuning files and measuring throughput improvements (up to 710 Mbps).
- **Operational Demos**:
  - Deploying a Linux VM via Azure portal and SSH verification.
  - Tuning and testing system-level performance.
- **Open Source and Cloud Native Ecosystem**: Microsoft’s engagement and integration across Linux and cloud native tools.

## Step-by-Step Highlights

1. **Open Source in Microsoft Ecosystem**: Discussion on cloud native and AI ecosystem contributions.
2. **Image Deployment**:
    - Demo: Customized Red Hat-based image creation
    - License integration
    - VM provisioning and connection via SSH
3. **Advanced System Inspection**:
    - eBPF and Inspector Gadget for live workload analysis
    - Performance metrics and diagnostic insights
4. **Network and Resource Tuning**:
    - Cross-region performance tuning
    - Practical steps for TCP configuration
    - Impact assessment through before/after tests
5. **Security and Compliance Tools**:
    - Defender for Linux configuration
    - Secure baseline enforcement

## Resources and Next Steps

- Microsoft documentation: https://aka.ms/ignite25-plans-migrateLinuxPostgreSQL
- Explore more on Microsoft Ignite: https://ignite.microsoft.com

## Conclusion

This session provides actionable strategies for architects, sysadmins, and anyone deploying Linux on Azure to maximize performance, manage security, and ensure compliance—leveraging both native Azure capabilities and open source integrations.
