---
external_url: https://techcommunity.microsoft.com/t5/azure-confidential-computing/dcasv6-and-ecasv6-confidential-vms-in-azure-government-cloud/ba-p/4494604
title: DCasv6 and ECasv6 Confidential Virtual Machines in Azure Government Cloud
author: Rakeshginjupalli
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-13 18:16:00 +00:00
tags:
- AMD SEV SNP
- Attestation
- Azure
- Azure Government
- Cloud Compliance
- Community
- Confidential Computing
- Confidential Virtual Machines
- Cryptographic Isolation
- DCasv6
- ECasv6
- Hardware Security
- Key Management
- Operational Security
- Security
- Sovereign Cloud
- Virtual Machines
- Vtpm
section_names:
- azure
- security
---
Rakeshginjupalli delivers an in-depth overview of the launch of DCasv6 and ECasv6 confidential VMs in Azure Government, outlining key security advancements and compliance benefits for organizations deploying sensitive federal workloads.<!--excerpt_end-->

# DCasv6 and ECasv6 Confidential Virtual Machines in Azure Government Cloud

Azure Government continues to evolve its secure cloud offerings, and the introduction of the DCasv6 and ECasv6-series confidential virtual machines (CVMs) brings a new standard for confidential computing in federal environments. Built on 4th Generation AMD EPYC™ processors, these CVMs are the first in Azure Government to support AMD SEV-SNP technology, which is designed to address long-standing barriers to multi-tenant cloud adoption in regulated sectors.

## Key Features

- **Hardware-Enforced Memory Isolation**: AMD SEV-SNP provides AES-128 encrypted memory, with unique keys generated and managed by the onboard AMD Secure Processor to protect against unauthorized memory access.
- **Online Key Rotation**: The Virtual Machine Metablob disk (VMMD) supports secure, online cryptographic key rotation, enhancing operational readiness and data protection.
- **Programmatic Attestation**: Before workloads are provisioned, customers can perform attestation—a cryptographic procedure that validates the integrity of hardware and software stacks. This produces a signed report verifying that a VM is truly a confidential instance, supporting zero-trust approaches and auditability.
- **Confidential OS Disk Encryption with Flexible Key Management**: Encryption extends to the operating system disk, with keys bound to the VM's virtual Trusted Platform Module (vTPM). Organizations can choose between platform-managed keys for simplicity or customer-managed keys for full control over key lifecycle—supporting stringent compliance requirements.

## Why It Matters

Azure Government's sovereign, hyperscale cloud delivers over 180 services tailored for federal agencies, ensuring operational assurance, regulatory compliance, and security innovation. The introduction of confidential VMs allows agencies to:

- Deploy sensitive workloads with enhanced protection from insider threats and multi-tenant risks
- Maintain sovereignty over key management and cryptographic boundaries
- Benefit from robust compliance frameworks and simplified service validation
- Modernize infrastructure quickly while keeping control over security and regulatory boundaries

## Additional Resources

- [Azure Government documentation | Microsoft Learn](https://learn.microsoft.com/en-us/azure/azure-government/)
- [Government Validation System](https://usgovintake.embark.microsoft.com/)

Updated: Feb 13, 2026

---

*Authored by Rakeshginjupalli*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-confidential-computing/dcasv6-and-ecasv6-confidential-vms-in-azure-government-cloud/ba-p/4494604)
