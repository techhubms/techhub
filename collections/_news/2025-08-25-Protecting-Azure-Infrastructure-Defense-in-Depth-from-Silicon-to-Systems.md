---
layout: post
title: 'Protecting Azure Infrastructure: Defense-in-Depth from Silicon to Systems'
author: Mark Russinovich, Omar Khan and Bryan Kelly
canonical_url: https://azure.microsoft.com/en-us/blog/protecting-azure-infrastructure-from-silicon-to-systems/
viewing_mode: external
feed_name: The Azure Blog
feed_url: https://azure.microsoft.com/en-us/blog/feed/
date: 2025-08-25 15:00:00 +00:00
permalink: /azure/news/Protecting-Azure-Infrastructure-Defense-in-Depth-from-Silicon-to-Systems
tags:
- AI
- AI Infrastructure Security
- Azure
- Azure Boost
- Azure Integrated HSM
- Caliptra
- Code Transparency Services
- Confidential Computing
- Confidential Virtual Machines
- FIPS 140 3
- Hardware Security
- Microsoft Azure
- News
- OCP SAFE
- Root Of Trust
- SCITT
- Secure Cloud
- Secure Future Initiative
- Security
- Supply Chain Integrity
- Trusted Execution Environments
- Uncategorized
section_names:
- azure
- security
---
Authored by Mark Russinovich, Omar Khan, and Bryan Kelly, this article examines Microsoft’s holistic approach to securing Azure, covering innovations like Azure Boost, confidential computing, and root-of-trust hardware for end-to-end infrastructure protection.<!--excerpt_end-->

# Protecting Azure Infrastructure: Defense-in-Depth from Silicon to Systems

**Authors:** Mark Russinovich, Omar Khan, and Bryan Kelly  
**Source:** [Microsoft Azure Blog](https://azure.microsoft.com/en-us/blog/protecting-azure-infrastructure-from-silicon-to-systems/)

Microsoft’s strategy for Azure security starts at the hardware (silicon) level and extends up the entire cloud stack. This article delves into the Secure Future Initiative (SFI) and how dedicated solutions are implemented to create a secure-by-design, secure-by-default, and secure-in-operation environment for Azure customers.

## Secure Hardware Foundations

- **Azure Boost:** Hardware-accelerated system controller that isolates control and data planes for enhanced isolation of virtual machines. It ensures only verified security configurations have access to Azure systems.
- **Azure Integrated HSM:** Local hardware security module (HSM) with FIPS 140-3 Level 3 compliance for high-assurance workloads, offering tamper resistance and identity-based authentication. Integrated HSMs protect keys in-use, reduce network latency, and ensure keys stay within a hardware-trusted boundary.
- **Azure Datacenter Secure Control Module (DC-SCM):** Incorporates Hydra, a security-centric Board Management Controller (BMC) with built-in root-of-trust to prevent unauthorized firmware access and ensure authenticated, cryptographically measured updates.

## Confidential Computing: Securing Data in Use

- **Confidential Computing:** Uses hardware-backed Trusted Execution Environments (TEEs) to isolate workloads such as VMs from underlying system software. Azure offers confidential VMs, containers, AI services, and ledgers, all providing hardware-backed data protection.
- **Confidential Computing Consortium:** Microsoft is a founding member and continues to drive hardware-integrated confidential guarantees and transparent platform operations.
- **Levels of Confidential Computing:** The post discusses migrating existing applications, designing new services with deep hardware protections, and leveraging insights from transparent computing environments.

## Hardware Security Transparency & Root-of-Trust

- **Caliptra:** An open-source, silicon-based root-of-trust created by Microsoft and industry partners, anchoring the chain of trust directly into cloud hardware. The release of Caliptra 2.0 added support for post-quantum resilient cryptography.
- **Code Transparency Services (CTS):** Immutable ledger for supply chain security and integrity, running within confidential computing environments. CTS enables cryptographically auditable firmware and hardware provenance, aligned with industry standards such as SCITT.
- **OCP SAFE:** Open Compute Project’s Systematic Appraisal Framework for Evaluations coordinates systematic, independent hardware security reviews.

## Supply Chain Security & Systematic Reviews

- Azure’s approach extends to practices like continuous hardware/firmware scanning, robust configuration management, and external auditing
- Industry collaboration extends trust beyond Microsoft and enables cross-vendor verifiable supply chain transparency.

## Conclusion

Security in Azure is layered and continuously evolving. By integrating secure silicon, dedicated hardware modules, confidential computing, open-source roots-of-trust, and supply chain transparency solutions, Microsoft aims to build a future-proof, resilient cloud infrastructure ready to defend against evolving threats.

For more details, related resources, and technical documentation, see the original [Microsoft Azure Blog post](https://azure.microsoft.com/en-us/blog/protecting-azure-infrastructure-from-silicon-to-systems/).

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/protecting-azure-infrastructure-from-silicon-to-systems/)
