---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/announcing-cobalt-200-azure-s-next-cloud-native-cpu/ba-p/4469807
title: 'Announcing Azure Cobalt 200: Next-Generation Arm-Based CPU for Cloud Workloads'
author: sebilgin
feed_name: Microsoft Tech Community
date: 2025-11-18 16:00:05 +00:00
tags:
- ARM Neoverse CSS V3
- Azure Boost
- Azure Cobalt 200
- Azure Infrastructure
- Azure Integrated HSM
- Cloud Native Compute
- Confidential Compute Architecture
- Cryptography Accelerator
- Data Analytics
- Dynamic Voltage Frequency Scaling
- Energy Efficiency
- FIPS 140 3
- Key Vault
- Memory Encryption
- Performance Benchmarking
- Server Architecture
- System On Chip
- TSMC 3nm
- Virtual Machines
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
Selim Bilgin and Pat Stemen present the technical foundations and innovations behind Azure Cobalt 200, detailing its cloud-native capabilities, hardware acceleration, security features, and impact on Microsoft’s cloud infrastructure.<!--excerpt_end-->

# Announcing Azure Cobalt 200: Next-Generation Arm-Based CPU for Cloud Workloads

**By Selim Bilgin, Corporate Vice President, Silicon Engineering, and Pat Stemen, Vice President, Azure Cobalt**

Microsoft has announced Azure Cobalt 200, the next evolution of its custom Arm-based CPU for cloud-native workloads. Cobalt 200 is engineered to deliver up to 50% performance gains over its predecessor, Cobalt 100, and integrates advanced security, networking, and storage features for robust data center solutions.

## Key Innovations in Cobalt 200

- **Full Compatibility**: Supports workloads previously running on Cobalt 100, enabling seamless migration and upgrade.
- **Performance**: 132 active Arm Neoverse V3 cores, each with 3MB of L2 cache and a total of 192MB L3 system cache, boosting compute for complex cloud workloads.
- **Energy Efficiency**: Utilizes individual core-level Dynamic Voltage and Frequency Scaling (DVFS) and is built on TSMC's 3nm process to optimize power consumption.

## Benchmarking for Real Workloads

- Microsoft used telemetry from diverse Azure workloads—including databases, web servers, storage, network transactions, and analytics—to tune Cobalt 200 beyond traditional hardware benchmarks.
- Over 140 benchmarks evaluated across 2,800 system design parameters and 350,000 system configurations ensured optimal performance for real-world scenarios.

## Advanced Security Architecture

- **Memory Encryption**: On by default, enabled by a custom memory controller, ensuring data protection with minimal impact to performance.
- **Confidential Compute Architecture (CCA)**: Provides hardware-based isolation of VM memory from hypervisors and host operating systems.
- **Custom Accelerators**: Dedicated hardware for compression, cryptography, and encryption tasks offloads CPU cycles, benefiting workloads like Azure SQL.
- **Integrated HSM**: Built-in Azure Integrated Hardware Security Module works with Azure Key Vault for cryptographic key management, supporting FIPS 140-3 Level 3 compliance.

## Enhanced Infrastructure Capabilities

- **Azure Boost**: Increases network bandwidth and efficiently offloads storage/networking tasks to specialized hardware, reducing workload latency and maximizing throughput.
- **Scalability**: Deployment across new datacenter regions, supporting both Microsoft cloud services and customer workloads.

## Real-World Impact

- Early adopters (Databricks, Snowflake, Microsoft Teams) experience significantly improved performance, energy efficiency, and cost-effectiveness.
- Teams media processing reduced core usage by 35%, while attaining up to 45% better performance compared to previous platforms.
- Security and compliance features are now more deeply integrated into cloud hardware.

## Availability and Outlook

- Production Cobalt 200 servers are operating in select Microsoft datacenters, with wider customer access targeted for 2026.
- Microsoft is preparing for global rollout, anticipating new customer solutions and service advancements.

## Learn More

- [Microsoft Ignite keynote](https://ignite.microsoft.com/en-US/sessions/KEY01?source=sessions)
- [What's new in Azure at Ignite](http://aka.ms/AzureAtIgnite)
- [Microsoft's global datacenter infrastructure](https://datacenters.microsoft.com/)

---

For infrastructure architects and cloud engineers, Cobalt 200 represents Microsoft’s commitment to optimizing cloud compute from silicon to software, delivering performance, energy efficiency, and strong security features to meet real-world workload demands.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/announcing-cobalt-200-azure-s-next-cloud-native-cpu/ba-p/4469807)
