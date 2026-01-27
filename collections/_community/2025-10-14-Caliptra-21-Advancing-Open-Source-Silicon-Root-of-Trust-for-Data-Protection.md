---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/caliptra-2-1-an-open-source-silicon-root-of-trust-with-enhanced/ba-p/4460758
title: 'Caliptra 2.1: Advancing Open-Source Silicon Root of Trust for Data Protection'
author: Bryankel
feed_name: Microsoft Tech Community
date: 2025-10-14 00:00:00 +00:00
tags:
- Caliptra 2.1
- Confidential Computing
- Cryptographic Hardware
- FPV Verification
- Hardware Security
- Key Management
- Microsoft Security
- OCP LOCK
- Open Compute Project
- Quantum Cryptography
- Root Of Trust
- Silicon Security
- Trusted Execution Environments
- Zero Trust
section_names:
- azure
- security
primary_section: azure
---
Bryankel presents Caliptra 2.1, Microsoft’s open-source silicon Root of Trust subsystem, detailing its quantum-resilient cryptography and integration with Azure’s security architecture, aiming to advance industry-wide device security.<!--excerpt_end-->

# Caliptra 2.1: Advancing Open-Source Silicon Root of Trust for Data Protection

**Author:** Bryankel

## Overview

Caliptra 2.1 is a next-generation, open-source silicon Root of Trust (RoT) subsystem introduced at the Open Compute Project Global Summit. Developed by Microsoft and industry collaborators, Caliptra 2.1 expands upon Caliptra 1.0 by delivering:

- A full RoT security subsystem for secure device integration
- Quantum-resilient cryptography and hardware-enforced key management
- Comprehensive defense-in-depth features, including secure boot, attested secure erase, and multi-layered key hierarchies

## Integration with Azure Security

Microsoft highlights how Caliptra 2.1 aligns with Azure's hardware security architecture, securing data throughout its lifecycle:

- **Data in-transit**: Encryption during transfer across networks
- **Data in-use**: Isolation and protection through Confidential Computing and hardware-based Trusted Execution Environments (TEEs)
- **Data at-rest**: Encryption on storage media

## Collaborative Key Management: OCP L.O.C.K

Microsoft, with partners like Google, Samsung, Kioxia, and Solidigm, developed OCP L.O.C.K (Layered Open-source Cryptographic Key-management), implemented in Caliptra 2.1. Its features include:

- Hardware-enforced key isolation
- Layered key hierarchies
- Attested secure erase in self-encrypting storage devices

## Quantum-Resilient Capabilities

Caliptra 2.1 integrates [Adams Bridge 2.0](https://github.com/chipsalliance/adams-bridge), providing:

- ML-DSA and ML-KEM cryptographic algorithms
- Side channel countermeasures
- Protection against "harvest-now, decrypt-later" and future quantum attacks

## Root-of-Trust Enhancements

Additional modular features include:

- Ownership transfer supporting owner-endorsed code integrity
- Streaming boot for resilient system recovery
- Broader adoption suitable for modern SoCs

## Design, Validation, and Open Source Approach

- The full Caliptra subsystem totals 1,640,145 gates; 62% devoted to cryptographic accelerators and key management
- Stringent verification—including third-party Formal Property Verification—is performed on every release
- All code, validation artifacts, and test coverage are published open-source for community review

## Industry Philosophy and Community Involvement

Caliptra’s open-source strategy is founded on:

- **Security transparency**: Enabling trust through verifiable implementations and independent reviews
- **Consistency**: Allowing for focused, robust security hardening and predictable security posture

Participation is encouraged via [https://Caliptra.io](https://Caliptra.io), including documentation, GitHub repositories, and open community meetings (CHIPS Alliance and OCP Security calls).

## Useful Links

- [Caliptra.io](https://Caliptra.io)
- [CHIPS Alliance](https://www.chipsalliance.org)
- [Open Compute Project](https://www.opencompute.org/projects/security)
- [Protecting Azure Infrastructure Blog](https://azure.microsoft.com/en-us/blog/protecting-azure-infrastructure-from-silicon-to-systems/)

---
*For more about Azure Infrastructure and Security practices, visit the [Azure Infrastructure Blog](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/bg-p/AzureInfrastructureBlog).*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/caliptra-2-1-an-open-source-silicon-root-of-trust-with-enhanced/ba-p/4460758)
