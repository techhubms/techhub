---
layout: "post"
title: "Azure Linux with OS Guard: Enhancing Container Host Security with Code Integrity and Open Source Transparency"
description: "This in-depth post explores Azure Linux with OS Guard, a locked-down, immutable container host that builds on Azure Linux to deliver advanced code integrity, immutability, and mandatory access control for cloud-native workloads. It details how features like IPE, SELinux enforcement, Secure Boot, and trusted supply chain practices provide significant security benefits for Azure Kubernetes Service users and open source contributors. Readers will learn how OS Guard mitigates modern threats, supports compliance, and welcomes community exploration via public images and upstream contributions."
author: "Sudhanva"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-13 16:01:17 +00:00
permalink: "/2025-08-13-Azure-Linux-with-OS-Guard-Enhancing-Container-Host-Security-with-Code-Integrity-and-Open-Source-Transparency.html"
categories: ["Azure", "Security"]
tags: ["AKS", "Azure", "Azure Linux", "Code Integrity", "Community", "Container Security", "Dm Verity", "FedRAMP", "FIPS 140 3", "Immutability", "IPE", "Microsoft Azure", "Open Source", "OS Guard", "Rootkit Protection", "Secure Boot", "Security", "Security Compliance", "SELinux", "Supply Chain Security", "Trusted Launch", "Unified Kernel Images", "Vtpm"]
tags_normalized: ["aks", "azure", "azure linux", "code integrity", "community", "container security", "dm verity", "fedramp", "fips 140 3", "immutability", "ipe", "microsoft azure", "open source", "os guard", "rootkit protection", "secure boot", "security", "security compliance", "selinux", "supply chain security", "trusted launch", "unified kernel images", "vtpm"]
---

Sudhanva presents a comprehensive overview of Azure Linux with OS Guard, highlighting Microsoft's advancements in secure, immutable container hosting and the collaborative open source approach underlying these innovations.<!--excerpt_end-->

# Azure Linux with OS Guard: Immutable Container Host with Code Integrity and Open Source Transparency

Azure Linux, the trusted Linux OS powering Microsoft's massive fleet and AKS workloads, is now advancing secure container hosting with the introduction of **Azure Linux with OS Guard**. Announced at Microsoft Build 2025, OS Guard introduces a locked-down, immutable container host combining code integrity, advanced access controls, and transparent open source engineering.

## Key Features

### Code Integrity with IPE

- **Integrity Policy Enforcement (IPE):** Leveraging the IPE module recently upstreamed to the Linux 6.12 kernel, only trusted binaries from *dm-verity*-protected volumes can execute—including container images.
- **Customizable IPE Policies:** Flexibility to allow specific files by *fs-verity* digests or restrict execution to approved *dm-verity* volumes enhances security controls.

### Immutability

- **Read-Only */usr* Directory:** The user-space is locked down by mounting */usr* as a *dm-verity* protected and signed volume. Unauthorized changes are detected and blocked at runtime.

### Mandatory Access Control

- **SELinux Enforcement:** Ensures only trusted users and processes access sensitive system areas, in enforcing mode by default.

### Trusted Launch and Secure Boot

- **Trusted Launch:** Ensures measured boot integrity, with cryptographic keys stored in virtual TPM (vTPM).
- **Secure Boot:** Guards against rootkit threats and tampering from the earliest boot stages.

## Security Benefits and Threat Mitigation

Azure Linux with OS Guard directly addresses key cloud-native threats:

- **Rootkit and Boot Tampering:** Secure Boot and Trusted Launch safeguard boot components' integrity.
- **Container Escape and Tampering:** Read-only root filesystem and validated container images reduce risks of unauthorized changes and container escapes.
- **Unauthorized Code Execution:** IPE and SELinux together enforce that only verified, untampered code runs, with minimal attack surface for adversaries.

## Built on Proven Azure Linux Foundation

- **Sovereign Supply Chain:** Uses Azure Linux pipelines and signed Unified Kernel Images (UKIs), creating a transparent Software-Bill-of-Materials (SBOM) from firmware to user-space.
- **Compliance & Quantum-Safe Security:** Inherits robust compliance (FedRAMP, FIPS 140-3) and is post-quantum-ready as Azure Linux cryptography evolves.
- **Enterprise-Grade Support:** Follows strict patch SLAs and is regularly penetration tested by Microsoft security researchers.

## Open Source Transparency and Community Collaboration

OS Guard draws on open source components (**dm-verity**, SELinux, IPE) and upstreams Microsoft engineering enhancements:

- **Script Integrity for Interpreters:** Ongoing kernel changes improve Bash and Python script security.
- **SELinux Policy Hardening:** Moving configurations to immutable, best-practice paths.
- **Containerd Code Integrity:** Collaborating with upstream community to secure OCI container execution.
- **Open Build Tooling:** [azure-linux-image-tools on GitHub](https://github.com/microsoft/azure-linux-image-tools) publicly provides build tooling.

## Availability and How to Try

Azure Linux with OS Guard will soon be offered as an official OS SKU for AKS, enabling preview deployments via feature flags and Azure CLI. The initial community image—publicly available via Microsoft Container Registry—enables exploration of Secure Boot, immutable */usr*, and enforced IPE/SELinux on Azure VMs.

- **Community Edition Details:**
    - Trusted Launch VMs, ephemeral keys
    - *dm-verity* backed, read-only */usr*
    - IPE and SELinux in enforcing mode
- [Try OS Guard: Step-by-step instructions](https://aka.ms/azurelinux-osguard-howto)

## Conclusion

Azure Linux with OS Guard strengthens security for cloud-native Linux hosts by combining immutable infrastructure, advanced code integrity, and proven Microsoft-supported compliance—all while advancing open source transparency. Its integration with AKS and clear supply chain makes it a compelling choice for enterprises seeking secure and trustworthy container platforms.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473)
