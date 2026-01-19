---
layout: post
title: 'Microsoft Azure Introduces Azure Integrated HSM: Secure Hardware-Backed Cryptography for Virtual Machines'
author: simranparkhe
canonical_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/microsoft-azure-introduces-azure-integrated-hsm-a-key-cache-for/ba-p/4456283
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-25 17:10:09 +00:00
permalink: /azure/community/Microsoft-Azure-Introduces-Azure-Integrated-HSM-Secure-Hardware-Backed-Cryptography-for-Virtual-Machines
tags:
- AMD V7 Series
- Azure Integrated HSM
- Azure Key Vault
- Crypto Accelerator
- Cryptography
- Encryption
- FIPS 140 3
- Hardware Security Module
- Key Management
- Secure Compute
- Trusted Launch VMs
- Windows Virtual Machines
section_names:
- azure
- security
---
simranparkhe introduces the public preview of Azure Integrated HSM for AMD v7 Trusted Launch Virtual Machines, providing secure hardware-based cryptography directly within supported Azure VMs.<!--excerpt_end-->

# Microsoft Azure Introduces Azure Integrated HSM: Secure Hardware-Backed Cryptography for Virtual Machines

Azure has announced the public preview of Azure Integrated HSM (Hardware Security Module) support for AMD v7 Trusted Launch Virtual Machines, targeting customers with intensive cryptographic workloads and stringent security requirements.

## Key Features

- **Integrated Hardware Security Module (HSM):** Azure Integrated HSM delivers a hardware-backed cache and cryptographic offload inside virtual machines. It is designed for workloads that require quick, secure cryptographic key operations, such as encryption, decryption, signing, and verification.
- **FIPS 140-3 Level 3 Compliance:** Designed to meet strict security standards for cryptographic modules, protecting keys and sensitive assets while they are in-use.
- **Improved Performance and Security:** By avoiding reliance on remote HSM services (such as Azure Key Vault Managed HSM), Azure Integrated HSM eliminates network round-trip latency and never exposes keys outside of the secure hardware boundary. Authorized services gain fast oracle-style access within the VM.
- **Platform Support:** Available in preview for AMD Dasv7-, Dadsv7-, Easv7-, and Eadsv7-series VMs (8+ vCores) running Windows, with Linux support planned soon. The preview comes at no additional cost.

## Azure Integrated HSM vs. Azure Key Vault Managed HSM

- **Azure Managed HSM:** Offers robust, managed, single-tenant key protection, but network calls can introduce latency, and key extraction carries a risk of reduced security post-transfer.
- **Azure Integrated HSM:** Keys remain in the local hardware module, avoiding the complexity and potential risks involved in key release and import. Key usage remains within the VM, maintaining a high security standard.

## Security and Cryptographic Operations

Azure Integrated HSM utilizes specialized hardware accelerators for:

- Encryption and decryption
- Digital signing and verification
- Fast, secure, and local cryptographic key usage while maintaining FIPS 140-3 Level 3 compliance

## Getting Started

- **Public Preview Enrollment:** Interested customers can [sign up for the public preview](https://forms.office.com/pages/responsepage.aspx?id=v4j5cvGGr0GRqy180BHbRyMSy8VejZVEo6yZykiPSHpUQkI0VFlXTVVVUlhDMVg5SkRYSTFPNEJHQi4u&route=shorturl).
- **Resources & Samples:** [GitHub repository](https://github.com/microsoft/AziHSM-Guest) contains usage samples and set-up instructions.

## Availability

- **Supported VM Series:** General-purpose Dasv7-, Dadsv7-, Easv7-, and Eadsv7-series VMs (8 vCores and above).
- **Platform:** Currently Windows; Linux support coming soon.
- **Cost:** Feature offered at no extra cost during preview.

## Reference Links

- [Azure Integrated HSM Overview](https://techcommunity.microsoft.com/blog/AzureInfrastructureBlog/securing-azure-infrastructure-with-silicon-innovation/4293834)
- [Azure Key Vault Managed HSM documentation](https://learn.microsoft.com/en-us/azure/key-vault/managed-hsm/overview)
- [AMD v7 VM series announcement](https://techcommunity.microsoft.com/blog/azurecompute/announcing-preview-of-new-azure-dasv7-easv7-fasv7-series-vms-based-on-amd-epyc%E2%84%A2-/4448360?previewMessage=true)

---

*Posted by simranparkhe. For further information, reference the provided enrollment form and GitHub repository links.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/microsoft-azure-introduces-azure-integrated-hsm-a-key-cache-for/ba-p/4456283)
