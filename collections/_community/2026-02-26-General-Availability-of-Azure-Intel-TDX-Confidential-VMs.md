---
layout: "post"
title: "General Availability of Azure Intel® TDX Confidential VMs"
description: "This announcement details the release of Azure's next generation of confidential virtual machines powered by 5th Gen Intel Xeon processors and Intel Trust Domain Extensions (TDX). It explains key features, supported scenarios (including confidential AI and security), hardware advancements, customer testimonials, and current availability for organizations running sensitive workloads on Azure."
author: "simranparkhe"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-confidential-computing/announcing-general-availability-of-azure-intel-tdx-confidential/ba-p/4495693"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-26 17:00:00 +00:00
permalink: "/2026-02-26-General-Availability-of-Azure-Intel-TDX-Confidential-VMs.html"
categories: ["AI", "Azure", "Security"]
tags: ["5th Gen Intel Xeon", "AI", "AI Workloads", "Attestation", "Azure", "Azure Boost", "Azure Confidential VMs", "Cloud Security", "Community", "Compliance", "Confidential AI", "Confidential Computing", "Confidential Data", "Cryptographic Attestation", "Data Protection", "Hardware Security", "Intel TDX", "NVMe SSD", "OpenHCL Paravisor", "Security", "Virtual Machines", "Virtualization Security", "VM Performance"]
tags_normalized: ["5th gen intel xeon", "ai", "ai workloads", "attestation", "azure", "azure boost", "azure confidential vms", "cloud security", "community", "compliance", "confidential ai", "confidential computing", "confidential data", "cryptographic attestation", "data protection", "hardware security", "intel tdx", "nvme ssd", "openhcl paravisor", "security", "virtual machines", "virtualization security", "vm performance"]
---

Simran Parkhe introduces Azure's next-gen Intel® TDX Confidential Virtual Machines, highlighting security, performance upgrades, hardware-based data protection, and industry use cases.<!--excerpt_end-->

# General Availability of Azure Intel® TDX Confidential VMs

**Author:** Simran Parkhe  
**Posted:** Feb 26, 2026

## Overview

Azure's newest generation of confidential virtual machines (VMs), powered by 5th Gen Intel® Xeon® processors and Intel® Trust Domain Extensions (TDX), is now generally available. These confidential VMs are designed for customers who need enhanced hardware-enforced isolation, cryptographic attestation, and robust performance for their most sensitive workloads — all with no required application code changes.

### Key Features

- **Hardware-enforced isolation:** Protects data and models while in use, even from the cloud operator.
- **Cryptographic attestation:** Verifies the integrity of workloads and infrastructure for compliance and trust.
- **Confidential AI support:** Accelerated confidential AI scenarios via Intel® Advanced Matrix Extensions (AMX), supporting efficient protection of models, weights, and data.
- **Performance improvements:** NVMe local SSD support for the DCedsv6 and ECedsv6 series delivers 5× more throughput and 16% lower latency than previous SCSI-based VMs, with up to 205k IOPS and 4 GB/s remote storage throughput (Azure Boost).
- **Open-source paravisor (OpenHCL):** Increases transparency, allowing customers to validate the security model.
- **No application changes required:** Existing workloads can be migrated without modification.

## Customer Use Cases

**Bosch Trustworthy Collaboration Services** enrolled their collaboration platform to benefit from improved transparency, performance, and robust verification.  
**Thales Cyber & Digital Identity** leverages Intel® TDX for comprehensive end-to-end encryption, uniting protection for data at rest, in transit, and in use, with CipherTrust Data Security Platform as the trust anchor.  
**Nuuday (TDC Erhverv)** enables secure and compliant confidential AI in regulated industries.  
**Arqit** partners to demonstrate provable protection of sensitive AI workloads processed in multi-region public cloud setups.

## VM Series and Specifications

- **DCesv6 and DCedsv6:** General-purpose with up to 128 vCPUs and 512 GiB memory.
- **ECesv6 and ECedsv6:** Memory-optimized, up to 64 vCPUs and 512 GiB memory.
- **Local NVMe SSDs:** Available for DCedsv6-series and ECedsv6-series for storage-intensive scenarios.

## Supported Scenarios

- Sensitive or regulated workloads
- Confidential AI pipelines
- Protecting intellectual property
- High-performance storage requirements
- Compliance-driven industry use cases (public sector, regulated industries, multi-org collaboration)

## Regions & Access

- Generally available in West US and West US 3 regions.
- Accessible via Azure Portal, Azure CLI, and Azure PowerShell.
- Supported OS: Windows Server 2025, Ubuntu 22.04/24.04
- Preview requests open for additional regions soon.

## Technical Innovations

- **NVMe performance:** Average 5× throughput increase and ~16% latency decrease vs SCSI.
- **Open-source paravisor:** [openvmm repo](https://github.com/microsoft/openvmm), [OpenHCL announcement](https://techcommunity.microsoft.com/blog/windowsosplatform/openhcl-the-new-open-source-paravisor/4273172)
- **Cryptographic attestation:** Open infrastructure for verifiable integrity.
- **Azure Boost:** High disk/network throughput for storage workloads.

## References & Further Reading

- [Intel Trust Domain Extensions](https://www.intel.com/content/www/us/en/products/docs/accelerator-engines/trust-domain-extensions.html)
- [OpenHCL Paravisor Announcement](https://techcommunity.microsoft.com/blog/windowsosplatform/openhcl-the-new-open-source-paravisor/4273172)
- [Azure Confidential Computing Preview](https://aka.ms/acc/v6preview)
- [OpenVMM on GitHub](https://github.com/microsoft/openvmm)
- [Thales End-to-End Data Encryption](https://cpl.thalesgroup.com/encryption/end-to-end-data-protection)
- [Thales CipherTrust Data Security Platform](https://cpl.thalesgroup.com/encryption/data-security-platform)

## Conclusion

Azure’s next-gen Intel® TDX confidential VMs unlock new possibilities for secure, high-performance cloud deployments. Organizations can trust Azure to safeguard their sensitive data in use, benefit from new hardware and open-source innovations, and accelerate digital transformation without compromising security or compliance.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-confidential-computing/announcing-general-availability-of-azure-intel-tdx-confidential/ba-p/4495693)
