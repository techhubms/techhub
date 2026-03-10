---
layout: "post"
title: "Migrating On-prem Windows & Linux VMs to Azure Confidential Virtual Machines via Azure Migrate"
description: "This in-depth whitepaper by SamhithaGurumurthy offers a hands-on framework for securely migrating on-premises Windows and Linux virtual machines to Azure Confidential Virtual Machines (CVMs) using Azure Migrate. It covers the end-to-end architecture, detailed migration phases, key Azure components, hardware prerequisites, disk encryption, operational governance, and critical security considerations. Readers will gain practical implementation knowledge focused on encryption, attestation, and secure workload lift-and-shift for regulated or mission-critical environments."
author: "SamhithaGurumurthy"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/migrating-on-prem-windows-linux-vms-to-azure-confidential/ba-p/4500898"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-10 14:12:02 +00:00
permalink: "/2026-03-10-Migrating-On-prem-Windows-and-Linux-VMs-to-Azure-Confidential-Virtual-Machines-via-Azure-Migrate.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Attestation", "Azure", "Azure Confidential Computing", "Azure Governance", "Azure Managed HSM", "Azure Migrate", "Cloud Security", "Community", "Confidential Virtual Machines", "DevOps", "Disk Encryption Sets", "ExpressRoute", "Intel TDX", "Linux Migration", "Private Endpoints", "RBAC", "Security", "SEV SNP", "Site To Site VPN", "Trusted Execution Environments", "Windows Migration", "Zero Trust"]
tags_normalized: ["attestation", "azure", "azure confidential computing", "azure governance", "azure managed hsm", "azure migrate", "cloud security", "community", "confidential virtual machines", "devops", "disk encryption sets", "expressroute", "intel tdx", "linux migration", "private endpoints", "rbac", "security", "sev snp", "site to site vpn", "trusted execution environments", "windows migration", "zero trust"]
---

SamhithaGurumurthy delivers a thorough step-by-step guide for migrating on-premises Windows and Linux VMs to Azure Confidential Virtual Machines with Azure Migrate, emphasizing secure architecture, disk encryption, attestation, and enterprise governance.<!--excerpt_end-->

# Migrating On-prem Windows & Linux VMs to Azure Confidential Virtual Machines via Azure Migrate

## Executive Summary

Enterprise adoption of cloud platforms now requires security boundaries beyond traditional isolation. While encryption at rest and in transit are foundational, protection of "data in use"—data processed in memory—has become a priority. Azure Confidential Computing (ACC) addresses this, offering hardware-backed Trusted Execution Environments (TEEs) to isolate VM memory, CPU, and I/O from the hypervisor and even privileged administrators. Azure Confidential Virtual Machines (CVMs) extend these protections to general-purpose workloads with minimal application changes, supporting:

- Memory encryption (unique keys per VM)
- Isolation from hypervisor and Azure's fabric
- Secure boot with platform attestation
- Managed HSM-driven cryptographic key release
- Seamless lift-and-shift migration via Azure Migrate

This guide details governance, implementation, and operational best practices for secure migration, aiming to enable regulated and mission-critical workloads in the cloud.

## Business Drivers & Compliance Alignment

- Enhanced data protection, even from cloud provider insiders
- Meets regulatory compliance by hardening against modern threat vectors
- Supports Zero Trust principles (assume breach, verify explicitly)
- Reduces risk of privileged-access and supply-chain attacks

### Threat Protection Matrix

| Category                | Traditional VM | CVM w/ ACC                                |
|-------------------------|---------------|-------------------------------------------|
| Hypervisor compromise   | Not isolated  | Isolated TEE (SEV-SNP/TDX)                |
| Insider admin threat    | Not protected | Memory/CPU state isolated with TEE        |
| DMA/memory scraping     | Vulnerable    | Hardware memory encryption                |
| Firmware/Supply chain   | At risk       | Attestation-gated secure boot             |
| Side-channel attacks    | Partial       | Strong isolation, mitigated risks         |

## Solution Architecture Overview

**On-Premises:**

- Windows and Linux servers to migrate
- Azure Migrate Appliance deployed in private network for secure discovery and replication

**Azure Landing Zone (Target):**

- Private Endpoints (Azure Migrate, Blob Storage, Managed HSM)
- Private DNS Zones (resolving only over private connections)
- Managed HSM for cryptographic keys and attestation-gated boot
- Confidential VMs for migrated workloads
- Disk Encryption Sets (DES) enforcing Customer-Managed Key (CMK) encryption
- Network isolation: ExpressRoute or Site-to-Site VPN, no public endpoints

**Workflow:**

- Discover and assess on-prem VMs
- Replicate data securely into Azure Blob Storage via private endpoints
- Test migration in an isolated Azure VNet
- Cutover to production with CVM security posture

## Azure Components Used

- **Azure Migrate Appliance**: Discovery, replication, and orchestration
- **Confidential VM (SEV-SNP/TDX)**: Secure VM execution environment
- **Managed HSM**: Secure, hardware-enforced cryptographic key storage and attestation
- **Disk Encryption Sets (DES)**: CMK-bound encryption for OS and data disks
- **Private Endpoints & DNS**: Enforces fully private traffic
- **Confidential VM Orchestrator**: Boot attestation and secure key release validation

## Confidential VM Prerequisites

### Hardware Requirements

- AMD SEV-SNP SKUs (DCasv6, ECasv6)
- Intel TDX SKUs (DCesv6, ECesv6)

### VM Configuration

- Generation 2 VM, UEFI + Secure Boot, vTPM enabled
- Confidential VM setting via Azure Migrate/ARM template

### Disk/Encryption

- OS disk must use Confidential Disk (<=128GB for full support)
- Data disks, if required, use DES with CMK keys (RSA-HSM)
- Managed HSM must have purge protection enabled
- Attestation required for disk key release
- All disks must use Premium SSD or SSD v2 (for CVM compatibility)

## End-to-End Migration Framework

**Nine Sequential Phases:**

1. **Azure Migrate Setup**: Connectivity, private endpoints, DNS setup, appliance deployment and registration
2. **OS Readiness**: Validate disk/boot format, driver support, and kernel/firmware for SEV-SNP/TDX
3. **Network Security**: Ensure all traffic uses private channels and the correct port matrix for replication
4. **CMK Encryption & HSM Governance**: Create and secure keys in Managed HSM, configure DES, set RBAC and attestation policies
5. **Confidential VM Orchestrator**: Set up service principal and assign minimum necessary permissions for key release
6. **Replication Enablement**: Use credential-less method for secure, audited migration; validate each configuration for CVM readiness
7. **Test Migration**: Boot isolation validation, verify attestation and application function in a sandboxed VNet
8. **Production Cutover**: Execute failover, boot integrity and encryption checks, switch to production endpoints
9. **Post-Migration Hardening**: Policy enforcement (CVM SKUs, CMK-only disks, deny public IPs), logging, monitoring, and regular key rotation

## Governance, Logging, and Monitoring

- Use Azure Policy to enforce CVM and encryption-only defaults
- Log all HSM, attestation, and migration activities
- Monitor with Azure Monitor, Defender for Cloud, and (optional) Microsoft Sentinel
- Regularly audit DES/HSM key lifecycle and rotate keys on schedule

## Limitations & Workarounds

- OS disk must be <=128 GB for full confidential disk encryption support; workaround involves post-migration update
- Only Windows Server 2019+, RHEL 9.4+, Ubuntu 22.04+ (specific SKUs) are supported
- Reference Microsoft documentation for the latest SKUs and OS support: [CVM OS Support Matrix](https://learn.microsoft.com/en-us/azure/confidential-computing/confidential-vm-overview#os-support)

## Conclusion

Azure Confidential Virtual Machines, alongside Managed HSM, Disk Encryption Sets, private networking, and rigorous governance, provide robust protection for enterprises migrating mission-critical workloads. The whitepaper’s framework illustrates a clear path to adopt hardware-enforced boundaries and encryption for cloud modernization, without the need to refactor existing applications.

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/migrating-on-prem-windows-linux-vms-to-azure-confidential/ba-p/4500898)
