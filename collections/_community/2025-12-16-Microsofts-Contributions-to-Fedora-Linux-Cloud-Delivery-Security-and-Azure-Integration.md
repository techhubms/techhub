---
layout: post
title: 'Microsoft’s Contributions to Fedora Linux: Cloud Delivery, Security, and Azure Integration'
author: bexelbie
canonical_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/building-bridges-microsoft-s-participation-in-the-fedora-linux/ba-p/4478461
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-16 19:01:32 +00:00
permalink: /azure/community/Microsofts-Contributions-to-Fedora-Linux-Cloud-Delivery-Security-and-Azure-Integration
tags:
- ARM64
- Azure Community Gallery
- Azure Linux Team
- Azure VM Utils
- Cloud Image Delivery
- Cloud Infrastructure
- Ebpf
- Fedora Linux
- Fedora SIGs
- Inspektor Gadget
- Kernel Instrumentation
- LISA Framework
- Microsoft Linux Engineering
- Observability
- OpenShift Automation
- Rust Client
- Secure Boot
- Sigul
- UEFI Signing
- WSL
section_names:
- azure
- devops
- security
---
bexelbie shares how Microsoft's Linux engineering teams are collaborating with Fedora to modernize cloud image delivery, security infrastructure, and Azure compatibility, advancing open source development.<!--excerpt_end-->

# Microsoft’s Contributions to Fedora Linux: Cloud Delivery, Security, and Azure Integration

Microsoft has deepened its technical engagement in the Fedora Linux community through tangible engineering contributions and strategic collaboration. This article highlights several key efforts undertaken by the Community Linux Engineering team over the past year.

## 1. Modernizing Fedora Cloud Image Delivery

- Expanded support for publishing Fedora Cloud images on major platforms: **Azure Community Gallery**, Google Cloud Platform, and AWS.
- Migrated AWS image publishing to an OpenShift-hosted automation framework, streamlining multi-cloud delivery and improving scalability.
- Enhanced nightly Fedora builds for Azure, including image validation and functional testing, to guarantee reliable cloud workloads.

## 2. Improving Secure Boot on ARM with Sigul

- Focused on enabling Secure Boot for ARM systems to meet trusted cloud workload needs.
- Led modernization of **Sigul** (artifact signing) with a new Rust client and re-architected codebase for better maintenance and cross-architecture support.
- Working toward Sigul-based UEFI application signing, moving beyond x86_64-only workflows.

## 3. Kernel Observability Tooling: Inspektor Gadget

- Collaborated with the Inspektor Gadget team to package and maintain this **eBPF-based kernel instrumentation toolkit** directly in Fedora.
- Enables advanced performance profiling and syscall tracing for developers and system engineers.
- Microsoft encourages direct package maintenance and active upstream contribution.

## 4. Azure VM Utils: Simplified Fedora Enablement

- Introduced the **azure-vm-utils** package, aggregating Udev rules and utilities that optimize Fedora performance on Azure, especially for NVMe devices.
- Provides transparency, maintainability, and a template for other cloud providers.

## 5. Fedora WSL Integration

- Fedora added to the official Windows Subsystem for Linux catalog after tackling both complex engineering and governance hurdles.
- Achieved through partnership among Fedora, Red Hat, and Microsoft teams.

## 6. Strategic Roadmap: What’s Next

- Replace Sigul with modern signing infrastructure.
- Expand participation in Fedora SIGs (Cloud, Go, Rust) leveraging Microsoft’s expertise.
- Enhance automated testing for Fedora images at cloud scale using Microsoft’s **LISA Open Source Framework**.
- Improve Fedora-on-Azure experience—exploring mirrored repositories within Azure and additional agent support.
- Azure Linux team aligning its upstream model closer to Fedora, especially for the 4.0 release.

## 7. Commitment to Open Source Collaboration

Microsoft aims for long-term, bi-directional collaboration in Fedora, prioritizing strategic engineering investments, package maintenance, and community listening. The company encourages anyone with ideas for boosting Fedora's technical future to engage directly in discussions, meetings, and conferences.

---

(This post was adapted from a Flock to Fedora 2025 conference talk. For more details, see [original YouTube recording](https://www.youtube.com/live/YhoFPG7Ack0?si=v5KH_0nRXl_bKtBD&t=4290).)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/building-bridges-microsoft-s-participation-in-the-fedora-linux/ba-p/4478461)
