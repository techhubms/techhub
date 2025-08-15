---
layout: "post"
title: "Azure Linux with OS Guard: Immutable, Secure Container Host for AKS"
description: "This blog post, authored by Sudhanva and David Weston of Microsoft, introduces Azure Linux with OS Guard—an immutable, secure Linux container host for Azure Kubernetes Service (AKS). The post details core security features like code integrity enforcement, immutability, mandatory access control (SELinux), support for Trusted Launch, and compliance with FedRAMP and FIPS standards. The authors highlight Microsoft’s commitment to open source, transparency, and ongoing upstream contributions, and explain how Azure Linux with OS Guard mitigates container escape, rootkits, unauthorized code execution, and user tampering. Instructions and links are provided for practitioners wanting to explore the preview image."
author: "Sudhanva"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-13 23:52:31 +00:00
permalink: "/2025-08-13-Azure-Linux-with-OS-Guard-Immutable-Secure-Container-Host-for-AKS.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Kubernetes Service", "Azure Linux", "Cloud Security", "Code Integrity", "Community", "Container Host Security", "Dm Verity", "Erofs Snapshotter", "FedRAMP", "FIPS 140 3", "Immutable OS", "IPE", "Kernel Security", "Linux", "Microsoft Azure", "Open Source", "OS Guard", "Post Quantum Cryptography", "SBOM", "Security", "SELinux", "Supply Chain Security", "Trusted Launch", "Unified Kernel Image", "Vtpm"]
tags_normalized: ["azure", "azure kubernetes service", "azure linux", "cloud security", "code integrity", "community", "container host security", "dm verity", "erofs snapshotter", "fedramp", "fips 140 3", "immutable os", "ipe", "kernel security", "linux", "microsoft azure", "open source", "os guard", "post quantum cryptography", "sbom", "security", "selinux", "supply chain security", "trusted launch", "unified kernel image", "vtpm"]
---

Sudhanva and David Weston present Azure Linux with OS Guard, a new immutable and secure container host for AKS. They explore its core security features and Microsoft’s open source and compliance initiatives.<!--excerpt_end-->

# Azure Linux with OS Guard: Immutable Container Host with Code Integrity and Open Source Transparency

**Authors:** [David Weston](https://techcommunity.microsoft.com/users/david%20weston%20%28dwizzzle%29/125192) (Corporate Vice President, Security @ Microsoft) and [Sudhanva Huruli](https://techcommunity.microsoft.com/users/sudhanva/567919) (Principal PM Core OS @ Microsoft)

## Overview

**Azure Linux** is Microsoft’s trusted Linux OS, built from the ground up for cloud-native workloads, powering essential infrastructure such as Azure Kubernetes Service (AKS). Over 80% of Microsoft’s own AKS workloads run on Azure Linux due to its performance, reliability, and strong security guarantees.

## Introducing Azure Linux with OS Guard

Azure Linux with OS Guard, announced at Microsoft Build 2025, is a container host OS designed for hardened cloud-native deployments. It builds on the FedRAMP-certified Azure Linux base (version 3.0) and enforces advanced protection mechanisms:

- **Immutability**: The */usr* directory is mounted via a *dm-verity* protected, signed root hash, making it read-only and guarding against tampering.
- **Code Integrity Enforcement (IPE)**: Integrity Policy Enforcement ensures only trusted binaries from *dm-verity* volumes run, even in container layers. Policies can be fine-tuned for specific volumes/files.
- **Mandatory Access Control (SELinux)**: Limits access to sensitive parts of the filesystem to only trusted users and processes.
- **Trusted Launch**: Ensures measured integrity of boot components, backed by vTPM-secured keys.

## Threat Mitigation

Azure Linux with OS Guard addresses threats such as:

- **Rootkits & Tampering**: Secure Boot and measured boot protect from the earliest boot stages.
- **Container Escapes & User Tampering**: Read-only root filesystem, signed container layers, and enforcement by *dm-verity* hashes prevent core system manipulation.
- **Unauthorized Code Execution**: IPE policy and SELinux enforcement only allow trusted code to execute, blocking tampered binaries—even inside a container image.

## Inherited Benefits from Azure Linux

- **Sovereign Supply Chain Security**: Signed Unified Kernel Images, official build pipelines, and a full SBOM provide transparency and trust.
- **Compliance**: FIPS 140-3 cryptographic modules, FedRAMP certification, with upcoming support for NIST-approved post-quantum algorithms.
- **Enterprise Security & SLAs**: Regular critical CVE patching and constant security assessments by Microsoft’s researchers.

## Open Source Commitment

Azure Linux with OS Guard uses open source technologies (dm-verity, SELinux, IPE), and Microsoft staff actively contribute upstream, including:

- Script integrity enforcement in interpreters (Bash, Python).
- SELinux policy hardening under immutable paths.
- Code integrity contributions to containerd via erofs-snapshotter.
- Public release of image tooling: [microsoft/azure-linux-image-tools](https://github.com/microsoft/azure-linux-image-tools).

## Availability & Try it Yourself

Azure Linux with OS Guard will soon be an official OS SKU on AKS, deployable with a feature flag and preview CLI. Practitioners can already explore the community image via the Microsoft Container Registry and deploy it to Azure VMs by following [these instructions](https://aka.ms/azurelinux-osguard-howto). The preview includes:

- Secure Boot with Trusted Launch VMs.
- Read-only */usr* via *dm-verity*.
- IPE and SELinux in enforcing mode.

---

*For further details and updates, follow the [Linux and Open Source Blog](https://techcommunity.microsoft.com/t5/Linux-and-Open-Source-Blog/bg-p/LinuxandOpenSourceBlog) on Microsoft Tech Community.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473)
