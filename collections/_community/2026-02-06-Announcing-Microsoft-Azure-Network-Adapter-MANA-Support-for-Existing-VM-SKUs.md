---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/announcing-microsoft-azure-network-adapter-mana-support-for/ba-p/4493279
title: Announcing Microsoft Azure Network Adapter (MANA) Support for Existing VM SKUs
author: ali_sheriff
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-06 23:45:42 +00:00
tags:
- Azure
- Azure Boost
- Azure Network Adapter
- Cloud Infrastructure
- Community
- Hardware Upgrades
- IaaS
- Latency
- MANA
- Microsoft Azure
- Migration
- NIC Firmware
- OS Compatibility
- Performance
- Throughput
- Virtual Machines
- VM SKUs
section_names:
- azure
---
ali_sheriff explains Microsoft's upcoming deployment of the Microsoft Azure Network Adapter (MANA) for existing Azure VM SKUs, highlighting what users need to know about new hardware support, improved performance, and next steps for IaaS customers.<!--excerpt_end-->

# Announcing Microsoft Azure Network Adapter (MANA) Support for Existing VM SKUs

Microsoft is delivering new hardware options for its Azure IaaS customers by deploying the Microsoft Azure Network Adapter (MANA) across existing VM Size Families starting February 2026. This initiative provides customers with the opportunity to access the latest server hardware without waiting to migrate to newer VM SKUs, thereby enhancing workload performance, reliability, security, and efficiency.

## Key Benefits of MANA

- **Sub-second NIC firmware upgrades** for supported operating systems
- **Higher network throughput and lower latency** for workloads using MANA-enabled SKUs
- **Improved security** and support for accelerated datapath using Azure Boost
- **Business continuity**: customers stay current with hardware innovation even on existing VM families

## Availability

- MANA will be deployed to existing VM sizes based on capacity needs
- Deployments are **not region-restricted**; once available in a region, VMs can use MANA hardware

## Compatibility & Performance

- Operating systems that fully support MANA will see the **maximum performance improvements**
- Workloads on non-MANA aware OSs will run with performance similar to prior non-MANA hardware

## Resources

- [Azure Boost Overview](https://learn.microsoft.com/en-us/azure/azure-boost/overview)
- [Microsoft Azure Network Adapter (MANA) Overview](https://learn.microsoft.com/en-us/azure/virtual-network/accelerated-networking-mana-overview)
- [MANA Support for Existing VM SKUs](https://aka.ms/manasupportforexistingvmfamilynva) (for guidance on VM eligibility and actions)

## Next Steps

- Review the compatibility of your VM operating systems with MANA
- Check whether your VMs are eligible for deployment on MANA-enabled hardware via the resources above
- Take any recommended actions to ensure optimal workload performance and security

*Content updated February 6, 2026 by ali_sheriff*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/announcing-microsoft-azure-network-adapter-mana-support-for/ba-p/4493279)
