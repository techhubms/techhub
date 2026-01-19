---
external_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/from-policy-to-practice-built-in-cis-benchmarks-on-azure/ba-p/4467884
title: 'Built-In CIS Benchmarks for Linux Security on Azure: Flexible and Hybrid-Ready Compliance'
author: pallakatos
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-18 08:00:00 +00:00
tags:
- Audit Automation
- Azure Arc
- Azure Osconfig
- CIS Benchmarks
- Cloud Security
- Compliance as Code
- Custom Configuration
- Ebpf
- GuestConfig
- HIPAA
- Hybrid Cloud
- Linux Security
- Machine Configuration
- Microsoft Certification
- NIST
- OVAL/XCCDF
- PCI DSS
- Policy Management
- Regulatory Compliance
- STIG
section_names:
- azure
- security
---
pallakatos showcases the launch of built-in CIS Benchmarks for Linux workloads on Azure, detailing secure configuration, hybrid cloud support, and Azure Arc integration for scalable compliance and security management.<!--excerpt_end-->

# Built-In CIS Benchmarks for Linux Security on Azure: Flexible and Hybrid-Ready Compliance

## Overview

Security and compliance remain at the forefront of cloud operations. Azure now offers industry-standard Center for Internet Security (CIS) Benchmarks as a built-in capability for all Azure-endorsed Linux distributions and Azure Arc-connected machines, allowing organizations to apply trusted security controls at scale and meet major regulatory requirements (NIST, HIPAA, PCI DSS).

## Key Features

- **Zero-cost CIS Benchmarks** for all Azure endorsed Linux distros and Azure Arc customers ([see supported distros](https://learn.microsoft.com/en-us/azure/virtual-machines/linux/endorsed-distros)).
- **Full parity with CIS specifications**: Azure ingests machine-readable benchmark content (OVAL/XCCDF files) to enforce controls matching official CIS rules.
- **Compliance as Code**: Parameters and exceptions can be customized. Manage policy as code, enabling consistent controls across cloud, hybrid, and on-prem environments.
- **Dynamic compliance engine** ([azure-osconfig](https://github.com/Azure/azure-osconfig/tree/dev/src/modules/complianceengine)): Lightweight open-source module for Linux server audit and future auto-remediation.
- **Near real-time compliance**: Machine Configuration agent evaluates every 15-30 minutes, supporting thousands of VMs and Arc-enabled servers without resource strain.
- **Audit-only launch**: Auto-remediation and enforcement (e.g., with eBPF) are planned for future releases.
- **Extensible framework**: Designed to handle multiple standards (CIS, STIG) and support workflows for benchmark version upgrade and migration.
- **Certified by CIS** ([official certification](https://www.cisecurity.org/partner/microsoft)): Azure compliance results are authoritative and benchmark updates are managed automatically.
- **Integration with reporting and workbooks**: Compatible with GuestConfig workbook ([link](https://github.com/Azure/Microsoft-Defender-for-Cloud/tree/main/Workbooks/GuestConfiguration%20Result)).

## Architecture Highlights

- **Azure Control Plane**: Orchestrates deployment and updates, instructing machines to fetch policy packages.
- **Machine Configuration Agent**: Interfaces with azure-osconfig for audit, remediation, and reporting.
- **Scalable for fleets**: Designed to work reliably across massive hybrid and multi-cloud infrastructure.
- **Configurable automation**: Supports logic operations and Lua scripting to express complex checks, suitable for CIS Critical Security Controls.

## Supported Use Cases

- **CIS Benchmarks for all Azure-endorsed distros** (audit only, L1/L2 profiles)
- **Hybrid and on-prem compliance management** with Azure Arc
- **Custom configuration & policy exception handling**
- **Regulatory compliance automation** mapped to frameworks like NIST, HIPAA, PCI DSS
- **Cloud-native workflow migration**: Seamless upgrade across Benchmark versions without manual intervention

## How to Get Started

1. See [documentation](https://aka.ms/cisonazure) for instructions.
2. Enable CIS Benchmarks in Azure Machine Configuration by selecting “Official Center for Internet Security (CIS) Benchmarks for Linux Workloads” and the relevant distribution for your assignment.
3. Customize parameters and exceptions as needed.
4. For extended distro support or feedback, open an Azure support case or [submit a Github issue](https://github.com/Azure/azure-osconfig/issues).
5. Attend the relevant Ignite 2025 sessions for technical deep-dives:
   - [Hybrid workload compliance from policy to practice on Azure](https://ignite.microsoft.com/en-US/sessions/THR712)
   - [Optimizing performance, deployments, and security for Linux on Azure](https://ignite.microsoft.com/en-US/sessions/BRK143)
   - [Build, modernize, and secure AKS workloads with Azure Linux](https://ignite.microsoft.com/en-US/sessions/BRK144)
   - [From VMs and containers to AI apps with Azure Red Hat OpenShift](https://ignite.microsoft.com/en-US/sessions/BRK104)
   - [From Container to Node: Building Minimal-CVE Solutions with Azure Linux](https://ignite.microsoft.com/en-US/sessions/THR701)
   - [Fast track your Linux and PostgreSQL migration with Azure Migrate](https://ignite.microsoft.com/en-US/sessions/LAB505)

## Collaboration and Roadmap

Microsoft and CIS have worked closely to certify this feature. Future plans include adding auto-remediation, releasing new distributions, and evolving workflows for even more seamless compliance management.

## Learn More & Community Engagement

- [Azure Linux Community Board](https://techcommunity.microsoft.com/category/azure/blog/linuxandopensourceblog)
- [azure-osconfig GitHub](https://github.com/Azure/azure-osconfig)
- [CIS Benchmarks official site](https://www.cisecurity.org/cis-benchmarks/)

For further details and to connect with the Linux security team, visit the Ignite sessions or the Azure booth for demonstrations and Q&A.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/from-policy-to-practice-built-in-cis-benchmarks-on-azure/ba-p/4467884)
