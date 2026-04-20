---
section_names:
- azure
- security
primary_section: azure
title: 'Sovereignty in Azure Belgium Central: A Three-Layer Technical Deep Dive'
feed_name: Microsoft Tech Community
external_url: https://techcommunity.microsoft.com/t5/azure-confidential-computing/sovereignty-in-azure-belgium-central-a-three-layer-technical/ba-p/4506936
author: wesback
date: 2026-04-09 16:00:00 +00:00
tags:
- AES 256
- AKS Confidential Node Pools
- AMD SEV SNP
- Availability Zones
- Azure
- Azure Attestation
- Azure Belgium Central
- Azure Confidential Computing
- Azure Encryption
- Azure Key Vault
- Azure Kubernetes Service (aks)
- Azure Managed HSM
- Azure Regions
- Community
- Confidential VMs
- Customer Managed Keys (cmk)
- Data Locality
- Data Residency
- Disaster Recovery
- Double Encryption
- FIPS 140 2
- FIPS 140 3
- Geo Replication
- Hardware Security Module (hsm)
- Intel TDX
- Key Vault Premium
- Key Vault Standard
- MACsec
- Non Paired Regions
- Secure Boot
- Security
- TLS 1.2
- Trusted Execution Environment (tee)
- Trusted Launch
- Vtpm
- Zone Redundant Storage (zrs)
---

wesback breaks down what “sovereignty” can mean in Azure Belgium Central by mapping it to three practical technical layers: data residency/locality, encryption (including CMK with Key Vault or Managed HSM), and confidential computing with attestation for in-use protection.<!--excerpt_end-->

# Sovereignty in Azure Belgium Central: A Three-Layer Technical Deep Dive

When **Azure Belgium Central** went live (November 2025), it enabled Belgian and EU organizations to deploy workloads in-country and apply technical controls that can support sovereignty requirements.

The post breaks “sovereignty” into three technical layers (concentric rings of protection):

- **Layer 1: Data Residency & Locality** — where data lives and what happens during failures
- **Layer 2: Encryption at Rest & In Transit** — how data is protected and who controls encryption keys
- **Layer 3: Confidential Computing** — how data is protected while processed in memory

## Layer 1: Data Residency & Locality

### In-country storage

For **regionally deployed** Azure services, customer data at rest is stored in the selected Azure region. In **Belgium Central**, that means data at rest for supported services is stored in Belgium. Microsoft indicates the region’s datacenters are located in the **Brussels area**.

Selecting the region in IaC examples:

- Terraform: `location = "belgiumcentral"`
- Bicep: `location: 'belgiumcentral'`

This matters for:

- Belgian or EU data residency requirements
- Public sector workloads needing assurance data doesn’t cross borders without explicit configuration

Source: [Microsoft Digital AmBEtion](https://www.microsoft.com/en-be/digitalambetion/datacenter)

### Three Availability Zones

Belgium Central supports **Availability Zones** (physically separate locations within the region with independent power/cooling/networking). This supports **zone-redundant architectures** (VMs, databases, storage across zones) while keeping resources in the same region.

Zone latency depends on architecture and placement and should be validated per scenario.

Source: [The ABC of Azure Belgium Central](https://techcommunity.microsoft.com/discussions/beluxpartnerzone/the-abc-of-azure-belgium-central/3808027)

### Non-paired region

Azure Belgium Central is a **non-paired region**. For services that rely on region pairing for automatic geo-replication, behavior/options can differ.

Sovereignty angle: customers may prefer explicit choice of **cross-region DR** targets rather than automatic pairing behavior.

Notes called out in the post:

- Some geo-redundant features (example: **GRS** for Azure Storage) may not be available in non-paired regions.
- Many designs use **ZRS** for in-region redundancy across Availability Zones.
- Cross-region replication can be configured explicitly (for example, **object replication** where supported), with the destination region chosen by the customer.

Source: [Azure region pairs and nonpaired regions](https://learn.microsoft.com/en-us/azure/reliability/regions-paired)

### What this means architecturally

When designing for Belgium Central:

- Prefer **intra-region redundancy** via Availability Zones (zone-redundant deployments, **ZRS**) where supported
- Implement **cross-region DR explicitly** (customer-chosen secondary region)
- Validate **service-specific replication behavior** (in-zone, across zones, or cross-region) and what configuration is required

## Layer 2: Encryption at Rest & In Transit

### Encryption at rest: platform-managed by default

Azure encrypts stored data at rest by default (examples cited: storage accounts, managed disks, databases) using **AES-256** with **Microsoft-managed keys**.

For sovereignty scenarios where “Microsoft holds the keys” is not sufficient, the post points to **double encryption** and **customer-managed keys (CMK)**.

Source: [Double encryption in Azure](https://learn.microsoft.com/en-us/azure/security/fundamentals/double-encryption)

### Customer-managed keys (CMK) via Key Vault

Azure services in Belgium Central support **CMK** using **Azure Key Vault**, shifting key ownership to the customer (generate/rotate/revoke keys). Examples mentioned include:

- VM disk encryption
- Storage account encryption
- Azure SQL Transparent Data Encryption

Source: [Azure encryption overview](https://learn.microsoft.com/en-us/azure/security/fundamentals/encryption-overview)

### Key management tiers in Belgium Central

#### Key Vault Standard (software-protected keys)

- Keys stored and protected in software (not dedicated HSM hardware)
- Positioned as cost-effective for scenarios not requiring hardware key protection

#### Key Vault Premium (HSM-backed keys, multi-tenant)

- Adds **HSM-protected keys**
- Key material lives in Microsoft-managed HSMs (shared, multi-tenant, logically isolated)
- Compliance/validation depends on hardware generation, region, and configuration; customers should check current docs/listings

#### Managed HSM (single-tenant, maximum isolation)

- Dedicated, single-tenant key management service
- Described as backed by **FIPS 140-3 Level 3 validated hardware**
- Includes security-domain concepts (customer downloads/controls the security domain) and RBAC for key operations
- Different pricing/ops model (pool-based billing, extra operational steps)

Sources:

- [Azure Key Vault Managed HSM overview](https://learn.microsoft.com/en-us/azure/key-vault/managed-hsm/overview)
- [Managed HSM technical details](https://learn.microsoft.com/en-us/azure/key-vault/managed-hsm/managed-hsm-technical-details)
- [About keys](https://learn.microsoft.com/en-us/azure/key-vault/keys/about-keys)

### Encryption in transit: MACsec + TLS

The post describes two layers:

1. **MACsec (IEEE 802.1AE)** on portions of the Azure backbone (coverage varies; check current documentation)
2. **TLS** for client-to-service connections (supported TLS versions/config vary by service; validate endpoints)

### Layer 2 summary table

| Concern | Mechanism | Key detail |
| --- | --- | --- |
| Data at rest (default) | AES-256, platform-managed keys | Automatic, no config needed |
| CMK: software keys | Key Vault Standard | FIPS 140-2 L1, multi-tenant, lowest cost |
| CMK: HSM-backed keys | Key Vault Premium | FIPS 140-3 L3 (new hardware), multi-tenant |
| CMK: dedicated HSM | Managed HSM | FIPS 140-3 L3, single-tenant, security domain |
| Data in transit (infra) | MACsec (IEEE 802.1AE) | Coverage varies by link/scenario; refer to current documentation |
| Data in transit (client) | TLS 1.2+ | Supported versions vary by service and configuration |

### Trusted Launch and data-at-rest protection

**Trusted Launch** for Azure VMs:

- Enables **Secure Boot** and **virtual TPM (vTPM)** on supported VM sizes
- Helps protect against bootkits/rootkits and supports integrity of the boot process
- Adds data-at-rest protection by isolating encryption keys in a platform-managed vTPM and binding key release to verified boot integrity

Source: [Trusted Launch for Azure virtual machines](https://learn.microsoft.com/en-us/azure/virtual-machines/trusted-launch)

## Layer 3: Confidential Computing

Layer 3 focuses on protecting data **in use** (in memory) using hardware-backed isolation.

### Confidential Virtual Machines (CVM)

Azure Confidential VMs create a VM-level **Trusted Execution Environment (TEE)**.

#### AMD SEV-SNP (DCasv6 / DCadsv6 / ECasv6 / ECadsv6)

Properties described:

- VM memory encrypted with CPU-generated keys designed to remain within the CPU boundary
- Designed to protect VM memory/state from the hypervisor/host management code
- Supports Confidential OS disk encryption with **PMK or CMK**, binding encryption to vTPM on supported configurations
- Uses **vTPM** for key sealing and integrity measurement

#### Intel TDX (DCesv6 / DCedsv6)

Properties described:

- Full VM memory encryption and integrity protection enforced by the Intel CPU
- Dedicated per-Trust-Domain encryption keys
- Supports Confidential OS disk encryption (PMK/CMK) and vTPM integration on supported configurations
- Performance and details vary by VM size/generation

Availability notes in the post:

- **AMD SEV-SNP VM families** are available in **Preview** in Azure Belgium Central (GA planned).
- **Intel TDX** is **not currently available** in Azure Belgium Central.

Sources:

- [About Azure confidential VMs](https://learn.microsoft.com/en-us/azure/confidential-computing/confidential-vm-overview)
- [DC family VM sizes](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/general-purpose/dc-family)
- [Intel TDX confidential VMs GA announcement](https://techcommunity.microsoft.com/blog/azureconfidentialcomputingblog/announcing-general-availability-of-azure-intel%C2%AE-tdx-confidential-vms/4495693)

### Azure Attestation

Azure Attestation validates hardware/firmware integrity before workloads run.

Platform attestation flow (as described):

1. CVM boots and hardware generates an **attestation report** (SNP report for AMD, TDX quote for Intel)
2. Azure Attestation evaluates the report against expected values
3. If it passes, decryption keys can be released from **Key Vault** or **Managed HSM**
4. Keys unlock vTPM state and the encrypted OS disk; VM starts

If the platform fails policy checks, key release can be blocked and the VM may not start (depending on configuration).

The post also mentions **guest-initiated attestation** (from inside the CVM) so applications can obtain an attestation token at runtime to prove execution in a genuine TEE.

Region availability caveat: Azure Attestation availability is region-dependent; validate for Belgium Central.

Sources:

- [Azure Attestation overview](https://learn.microsoft.com/en-us/azure/attestation/overview)
- [Attestation types and scenarios](https://learn.microsoft.com/en-us/azure/confidential-computing/attestation-solutions)

### Confidential computing on AKS

For container workloads, **AKS** supports confidential computing using **confidential node pools**:

- Add node pools backed by confidential VMs alongside regular node pools
- Protection is at the VM/node level; containers can run without app refactoring
- Validate region/SKU availability for the node sizes you plan to deploy

The post notes AMD SEV-SNP support today, with Intel TDX “on the roadmap”, and that attestation integration can be part of architectures but is not automatically enforced for all pods.

Sources:

- [Confidential VM node pools on AKS](https://learn.microsoft.com/en-us/azure/confidential-computing/confidential-node-pool-aks)
- [Use CVM in AKS](https://learn.microsoft.com/en-us/azure/aks/use-cvm)

## The full data protection chain (as described)

When combining the layers with confidential VMs in Belgium Central:

- Confidential VM boots
- Hardware TEE encrypts VM memory (SEV-SNP or TDX)
- Azure Attestation validates the platform report
- Key Vault (Premium) or Managed HSM conditionally releases disk decryption keys
- vTPM state unlocked, OS disk decrypted
- VM starts
- Data in memory protected by TEE (Layer 3)
- Data at rest protected by CMK (Layer 2)
- Data in transit protected by TLS (and MACsec on selected backbone links) (Layer 2)
- Data stored/processed in Belgium Central where supported and as configured (Layer 1)

## Bringing it all together (summary table)

| Layer | What it protects | Key technologies | Availability in Belgium Central |
| --- | --- | --- | --- |
| 1: Data residency | Where data lives | 3 AZs, non-paired region, ZRS | GA. No cross-border replication by default. |
| 2: Encryption | Data at rest + in transit | CMK, Key Vault (Std/Premium), Managed HSM, MACsec, TLS | GA. All three Key Vault tiers available in-region. |
| 3: Confidential computing | Data in use (memory) | SEV-SNP / TDX VMs, Attestation, AKS | Availability varies by SKU and region; validate for the exact sizes you plan to use. |

## Honest caveats (from the post)

1. **Check region availability for specific SKUs** (confidential VM sizes, attestation, Managed HSM, AKS node pool sizes).
2. **Sovereignty isn’t just technical** (legal/jurisdictional concerns are separate).
3. **Managed HSM has different pricing and operational characteristics** than Key Vault.
4. **Confidential VM limitations vary** by size/generation/features (backup/DR options, live migration behavior, accelerated networking, resize paths, etc.); validate for your exact configuration.

Disclosure highlighted: DR design/configuration remains a customer responsibility (secondary region selection, replication/failover/testing/runbooks). Replication scope is service-specific and depends on redundancy options; validate per service.

## References

- [Microsoft Digital AmBEtion](https://www.microsoft.com/en-be/digitalambetion/datacenter)
- [The ABC of Azure Belgium Central](https://techcommunity.microsoft.com/discussions/beluxpartnerzone/the-abc-of-azure-belgium-central/3808027)
- [Azure region pairs and nonpaired regions](https://learn.microsoft.com/en-us/azure/reliability/regions-paired)
- [Azure encryption overview](https://learn.microsoft.com/en-us/azure/security/fundamentals/encryption-overview)
- [Double encryption in Azure](https://learn.microsoft.com/en-us/azure/security/fundamentals/double-encryption)
- [Azure Key Vault Managed HSM overview](https://learn.microsoft.com/en-us/azure/key-vault/managed-hsm/overview)
- [Managed HSM technical details](https://learn.microsoft.com/en-us/azure/key-vault/managed-hsm/managed-hsm-technical-details)
- [About keys](https://learn.microsoft.com/en-us/azure/key-vault/keys/about-keys)
- [About Azure confidential VMs](https://learn.microsoft.com/en-us/azure/confidential-computing/confidential-vm-overview)
- [DC family VM sizes](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/general-purpose/dc-family)
- [Confidential VM FAQ](https://learn.microsoft.com/en-us/azure/confidential-computing/confidential-vm-faq)
- [Intel TDX confidential VMs GA announcement](https://techcommunity.microsoft.com/blog/azureconfidentialcomputingblog/announcing-general-availability-of-azure-intel%C2%AE-tdx-confidential-vms/4495693)
- [Confidential VM node pools on AKS](https://learn.microsoft.com/en-us/azure/confidential-computing/confidential-node-pool-aks)
- [Use CVM in AKS](https://learn.microsoft.com/en-us/azure/aks/use-cvm)
- [Azure Attestation overview](https://learn.microsoft.com/en-us/azure/attestation/overview)
- [Attestation types and scenarios](https://learn.microsoft.com/en-us/azure/confidential-computing/attestation-solutions)
- [Azure products by region](https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/)
- [Trusted Launch for Azure virtual machines](https://learn.microsoft.com/en-us/azure/virtual-machines/trusted-launch)


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-confidential-computing/sovereignty-in-azure-belgium-central-a-three-layer-technical/ba-p/4506936)

