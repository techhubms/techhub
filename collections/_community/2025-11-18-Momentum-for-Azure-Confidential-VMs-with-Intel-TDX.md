---
layout: post
title: Momentum for Azure Confidential VMs with Intel® TDX
author: simranparkhe
canonical_url: https://techcommunity.microsoft.com/t5/azure-confidential-computing/azure-intel-tdx-confidential-vms-momentum/ba-p/4470736
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-18 17:00:00 +00:00
permalink: /ai/community/Momentum-for-Azure-Confidential-VMs-with-Intel-TDX
tags:
- 5th Gen Intel Xeon
- Azure Boost
- Azure Confidential VM
- Cloud Infrastructure
- Confidential AI
- Confidential Computing
- Data Security
- DCesv6 Series
- ECesv6 Series
- Encryption
- Hardware Security
- Intel TDX
- Intel Trust Domain Extensions
- Memory Encryption
- NVMe SSD
- Open Paravisor
- OpenHCL
- Performance Benchmark
- Remote Storage
- Trusted Execution
- VM Security
section_names:
- ai
- azure
- security
---
Simran Parkhe shares insights on Azure's next-gen Confidential VMs with Intel® TDX, detailing technical advancements, security boundaries, performance updates, and customer adoption stories.<!--excerpt_end-->

# Azure Confidential VMs Momentum with Intel® TDX

## Overview

Azure has announced the next generation of Confidential Virtual Machines, leveraging 5th Gen Intel® Xeon® processors (Emerald Rapids) and Intel® Trust Domain Extensions (TDX). These VMs are now available in preview and enable organizations to run confidential workloads in the cloud without modifying their application code.

Confidential VMs are suited for tenants with high security and confidentiality needs. They enforce strong, attestable hardware boundaries and ensure that code and data remain encrypted in memory during processing. This supports data privacy even while in use.

## Key Technical Enhancements

- **Supported SKUs:**
  - General-purpose: DCesv6-series
  - Memory-optimized: ECesv6-series
- **Intel® Advanced Matrix Extensions (AMX):**
  - Accelerate confidential AI workloads and scenarios
- **Local NVMe SSD Support (DCedsv6/ECedsv6):**
  - Designed for storage workloads demanding SSD capacity, compute, and memory balance
  - Achieves ~5x throughput, 16% lower latency vs previous SCSI gen
  - IO latency reduced by ~27 microseconds across varying block sizes and thread counts
- **Azure Boost:**
  - Up to 205k IOPS, 4 GB/s remote storage, 40 Gbps network bandwidth
- **Open-Source Paravisor and OpenHCL:**
  - Azure’s first use of open paravisor (see [GitHub](https://github.com/microsoft/openvmm) and [OpenHCL announcement](https://techcommunity.microsoft.com/blog/windowsosplatform/openhcl-the-new-open-source-paravisor/4273172))
  - Supports transparency through “trust but verify” approach

## Security and Confidentiality Highlights

- Hardware-enforced attestation and boundaries for sensitive workloads
- Memory encryption throughout data usage lifecycle
- Integration with customer-managed security platforms (e.g., Thales CipherTrust)
- Enables ecosystem approaches to end-to-end data protection (at rest, in transit, and in use)

## Customer Feedback

- **Bosch Trustworthy Collaboration Services:**
  - Uses Azure Confidential VMs for foundational secure collaboration with enhanced transparency and verification
- **Thales Cyber & Digital Identity:**
  - Achieves encryption for data-in-use, closing key gaps in end-to-end data protection
  - Relies on integration between Microsoft, Intel, and Thales CipherTrust
- **Nuuday (TDC Erhverv):**
  - Delivers a secure, compliant Confidential AI environment meeting privacy and sovereignty standards
- **Arqit:**
  - Demonstrates security-enhancing technologies for sovereign control over sensitive AI workloads, accelerating AI adoption

## Product Series Details

- **DCesv6-series / DCedsv6-series:**
  - Up to 128 vCPUs, up to 512 GiB memory
  - NVMe SSD options for storage workloads
- **ECesv6-series / ECedsv6-series:**
  - Higher memory/vCPU ratio, up to 64 vCPUs and 512 GiB memory

## Availability and Preview Sign-up

- General release expected in Q1 2026 (select US and EU regions)
- Preview sign-up for DCesv6 and ECesv6 VM: [forms.office.com](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR14xUOCZvvNNjxzop0-giQBUN1ZJRzhFSkdESVJOSkFQVE9RTEtRUVVFVi4u)

---

## Useful Links and References

- [Intel Trust Domain Extensions](https://www.intel.com/content/www/us/en/products/docs/accelerator-engines/trust-domain-extensions.html)
- [Intel Advanced Matrix Extensions](https://www.intel.com/content/www/us/en/products/docs/accelerator-engines/what-is-intel-amx.html)
- [Open-source paravisor (GitHub)](https://github.com/microsoft/openvmm)
- [OpenHCL Blog Announcement](https://techcommunity.microsoft.com/blog/windowsosplatform/openhcl-the-new-open-source-paravisor/4273172)
- [Azure Boost Info](https://techcommunity.microsoft.com/t5/aka.ms/acc/v6preview)
- [Thales Data Security](https://cpl.thalesgroup.com/encryption/data-security-platform)

---

_Last updated Nov 17, 2025 by Simran Parkhe_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-confidential-computing/azure-intel-tdx-confidential-vms-momentum/ba-p/4470736)
