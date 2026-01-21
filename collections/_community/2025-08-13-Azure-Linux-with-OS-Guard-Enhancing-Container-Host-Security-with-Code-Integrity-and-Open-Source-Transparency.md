---
external_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/azure-linux-with-os-guard-immutable-container-host-with-code/ba-p/4437473
title: 'Azure Linux with OS Guard: Enhancing Container Host Security with Code Integrity and Open Source Transparency'
author: Sudhanva
feed_name: Microsoft Tech Community
date: 2025-08-13 16:01:17 +00:00
tags:
- AKS
- Azure Linux
- Code Integrity
- Container Security
- Dm Verity
- FedRAMP
- FIPS 140 3
- Immutability
- IPE
- Microsoft Azure
- Open Source
- OS Guard
- Rootkit Protection
- Secure Boot
- Security Compliance
- SELinux
- Supply Chain Security
- Trusted Launch
- Unified Kernel Images
- Vtpm
section_names:
- azure
- security
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
