---
external_url: https://techcommunity.microsoft.com/t5/azure-confidential-computing/ga-dcasv6-and-ecasv6-confidential-vms-based-on-4th-generation/ba-p/4451460
title: 'GA: DCasv6 and ECasv6 Confidential VMs with 4th Gen AMD EPYC for Azure'
author: Rakeshginjupalli
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-09-16 23:00:00 +00:00
tags:
- AES 256
- AKS
- AMD EPYC
- Azure Confidential Computing
- Cloud Security
- Confidential VMs
- Confidential Workloads
- DCasv6
- ECasv6
- Enterprise Performance
- Key Rotation
- Linux Kernel
- Memory Encryption
- Multi Tenant Security
- Secure Cloud
- SEV SNP
- Virtualization Based Security
- Windows VBS
section_names:
- azure
- security
---
Rakeshginjupalli details Azure's launch of the DCasv6 and ECasv6 confidential VM series with 4th Gen AMD EPYC processors, focusing on advanced security, AES-256 memory encryption, and AKS support for regulated and confidential workloads.<!--excerpt_end-->

# GA: DCasv6 and ECasv6 Confidential VMs with 4th Gen AMD EPYC for Azure

**Author:** Rakeshginjupalli

Azure has introduced the DCasv6 and ECasv6 confidential VM series, now generally available in the UAE North and Korea Central regions. These VMs leverage 4th Generation AMD EPYC™ processors and incorporate Secure Encrypted Virtualization-Secure Nested Paging (SEV-SNP) technology to enhance security and performance for sensitive workloads.

## Key Features

- **Hardware-rooted attestation**: Verifies VM integrity at the hardware level.
- **Memory encryption**: Protects confidential information in multi-tenant environments with AES-256 by default.
- **Enhanced data confidentiality**: Protects data from cloud operators, administrators, and insider threats.
- **Online key rotation**: Allows organizations to update encryption keys on-the-fly for dynamic defense.
- **High scalability**: Supports up to 96 vCPUs and 672 GiB RAM to meet demanding, memory-intensive workloads.
- **Regulated industry support**: Designed for organizations subject to stringent security and compliance requirements.

## Security Enhancements

- **SEV-SNP Technology**: Advanced memory protection mechanisms to defend against unauthorized access and side-channel attacks.
- **Virtualization-Based Security (VBS)**: In Windows (preview), enables secure processing of cryptographic keys, isolating them from the Guest OS and applications.
- **Integrated compliance and threat protection**: Assists in achieving and maintaining compliance standards while mitigating persistent security threats.

## Performance Improvements

- Benchmarks show up to **25% performance improvement** over previous confidential VM generations.
- Suitable for processing large confidential datasets, running joint analyses, medical research, and Confidential AI workloads.

## Kubernetes and OS Support

- **Azure Kubernetes Service (AKS) Integration**: Direct support for DCasv6 and ECasv6 families to deploy and manage confidential clusters with built-in CI/CD pipelines, monitoring, and scalability.
- **Operating System Support**: Azure Linux 3.0 (hardened and resource efficient) and Ubuntu 24.04 (preview) are supported for these VMs, enabling efficient orchestration and security.

## Usage and Documentation

- To get started, create confidential VMs via the [Azure portal](https://learn.microsoft.com/en-us/azure/confidential-computing/quick-create-confidential-vm-portal) or use ARM templates.
- Additional guidance:
  - [Quickstart: Create confidential VM with Azure portal](https://learn.microsoft.com/en-us/azure/confidential-computing/quick-create-confidential-vm-portal)
  - [Quickstart: Create confidential VM with ARM template](https://learn.microsoft.com/en-us/azure/confidential-computing/quick-create-confidential-vm-arm)
  - [Azure confidential virtual machines FAQ](https://learn.microsoft.com/en-us/azure/confidential-computing/confidential-vm-faq)

## Industry Perspectives

> *"The new v6 confidential VMs based on 4th Gen AMD EPYC processors deliver consistent, high performance for regulated workloads worldwide."*
> — Mohammed Retmi, Core42 (G42 company)
>
> *"KT uses Azure confidential computing to secure sensitive telco data and support the highest data protection for its customers in Korea."*
> — Woojin Jung, KT Corporation

## Summary Table

| Feature                 | DCasv6 Series           | ECasv6 Series           |
|------------------------|-------------------------|-------------------------|
| Max vCPUs              | 96                      | 96                      |
| Max RAM (GiB)          | 384                     | 672                     |
| Memory Encryption      | AES-256                 | AES-256                 |
| Key Rotation           | Supported               | Supported               |
| SEV-SNP                | Yes                     | Yes                     |
| OS Support             | Azure Linux 3.0, Ubuntu 24.04 | Azure Linux 3.0, Ubuntu 24.04 |
| AKS Support            | Yes                     | Yes                     |

## Workload Suitability

- **DCasv6**: General purpose with optimized memory-to-vCPU ratio, appropriate for most enterprise and confidential workloads.
- **ECasv6**: Memory-intensive workloads with a high memory-to-vCPU ratio, suitable for big-data analytics, genomics, and high-performance data science.

## Conclusion

Azure's DCasv6 and ECasv6 confidential VM series deliver leading-edge security, performance, and scalability for customers in regulated sectors and enterprises handling confidential information. Integration with AKS and support for the latest secure Linux distributions further enhance deployment options for modern cloud-native and sensitive workloads.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-confidential-computing/ga-dcasv6-and-ecasv6-confidential-vms-based-on-4th-generation/ba-p/4451460)
