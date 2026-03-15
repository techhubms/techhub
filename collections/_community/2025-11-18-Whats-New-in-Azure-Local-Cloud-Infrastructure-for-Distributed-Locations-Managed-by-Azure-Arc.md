---
external_url: https://techcommunity.microsoft.com/t5/azure-arc-blog/what-s-new-in-azure-local-cloud-infrastructure-for-distributed/ba-p/4469773
title: 'What’s New in Azure Local: Cloud Infrastructure for Distributed Locations Managed by Azure Arc'
author: meenagowdar
feed_name: Microsoft Tech Community
date: 2025-11-18 16:05:17 +00:00
tags:
- AKS
- Azure Arc
- Azure Key Vault
- Azure Local
- Cloud Managed Services
- Containers
- Data Sovereignty
- Distributed Infrastructure
- Edge Computing
- Hybrid Cloud
- Infrastructure Modernization
- Microsoft Defender For Cloud
- Network Security Groups
- NVIDIA RTX Pro 6000
- Rack Aware Clustering
- SAN Storage
- Sovereign Private Cloud
- Virtual Machines
- VM Migration
- VMware Migration
- Azure
- DevOps
- Security
- Community
section_names:
- azure
- devops
- security
primary_section: azure
---
meenagowdar presents a comprehensive overview of Azure Local, detailing its new features for distributed cloud infrastructure, enhanced security, and management via Azure Arc.<!--excerpt_end-->

# What’s New in Azure Local: Cloud Infrastructure for Distributed Locations Managed by Azure Arc

meenagowdar shares the latest updates on Azure Local, Microsoft’s solution for running Azure services and workloads at distributed and sovereign locations, managed through Azure Arc.

## Key Highlights

- **Distributed Solutions**: Azure Local enables organizations—such as hospitals and government agencies—to run cloud-native and traditional apps on their own infrastructure with centralized Azure Portal management.
- **Sovereignty and Compliance**: Built as the foundation of Microsoft’s Sovereign Private Cloud, Azure Local helps customers meet strict regulatory and data residency requirements, extending Azure services to customer-controlled environments.

## New Capabilities

### Infrastructure Flexibility & Scale

- **External SAN Storage**: Preview support for integrating Fiber Channel-based SAN storage from vendors like Dell, NetApp, Lenovo, HPE, and Hitachi, bringing performance and resilience to Azure Local clusters.
- **Rack Aware Clustering**: Preview release allowing intelligent placement and workload resiliency across multi-rack setups, improving fault tolerance for larger deployments.
- **Large Scale Deployments**: Azure Local now supports up to 10,000+ cores across 100+ nodes, enabling massive scale with consistent management and disaggregated storage.

### Application Enablement

- **Versatile Application Hosting**: Supports virtual machines, containers, and Azure services. AKS is built-in for modern workload orchestration. Azure Virtual Desktop, SQL Managed Instance, and Azure IoT Operations are available directly on Azure Local.
- **GPU Acceleration**: Now supporting NVIDIA RTX PRO 6000 Blackwell Server Edition GPUs for advanced AI, simulation, and visualization workloads.

### Migration & Modernization

- **VMware Migration**: Azure Migrate enables seamless, agentless migration of VMware VMs into Azure Local, minimizing downtime and streamlining onboarding with cloud-consistent management.
- **Microsoft 365 Local**: Brings productivity apps like Exchange Server, SharePoint Server, and Skype for Business Server to private sovereign clouds leveraging Azure Arc and Azure Local.

### Identity, Management & Security

- **Local Identity with Azure Key Vault**: Preview option for deployments without Active Directory, simplifying setups and providing secure access/secret management via Azure services.
- **Network Security Groups (NSGs)**: Policy-driven access control supporting detailed segmentation, filtering traffic between VMs/logical networks for edge workload isolation.
- **Built-in Microsoft Defender for Cloud**: Hardened infrastructure stack and centralized monitoring for security baselines, threat response, and policy enforcement. Trusted launch VMs further enhance baseline protections.
- **Operation at the Edge**: Enables consistent management and updates through Azure Arc, with support for disconnected or partially connected deployments, edge sites, and regulated environments.

## Getting Started

- **Production Deployments**: Browse validated hardware in the [solutions catalog](https://azurelocalsolutions.azure.microsoft.com/) and view the [deployment overview](https://learn.microsoft.com/en-us/azure/azure-local/deploy/deployment-introduction).
- **Evaluation**: Use [Azure Arc Jumpstart](https://jumpstart.azure.com/azure_jumpstart_localbox) for sandbox experiences.

## Additional Resources & Links

- [Azure Local announcements and features](https://azure.microsoft.com/en-us/blog/microsoft-strengthens-sovereign-cloud-capabilities-with-new-services/?msockid=261e425b08b06a1526f35632095e6b67)
- [VMware Migration capabilities](https://techcommunity.microsoft.com/blog/azurearcblog/azure-migrate-expands-capabilities-to-accelerate-migration-to-azure-local/4464789)
- [Network Security Group general availability](https://techcommunity.microsoft.com/blog/azurearcblog/announcing-general-availability-of-software-defined-networking-sdn-on-azure-loca/4467579)
- [Rack aware cluster preview](https://aka.ms/blog/AzureLocalRACpreview)
- [NVIDIA GPU support announcement](https://azure.microsoft.com/en-us/blog/building-the-future-together-microsoft-and-nvidia-announce-ai-advancements-at-gtc-dc/)
- [Microsoft 365 Local update](https://aka.ms/blog/M365Local)
- [Adaptive Cloud at Ignite](https://thankful-ground-0531db91e.3.azurestaticapps.net/)

## FAQ

1. **What is Azure Local?**
   - Azure’s infrastructure software for running cloud and edge workloads in distributed or sovereign environments.
2. **Relation to Sovereign Private Cloud?**
   - Azure Local is the core platform for customer-controlled, compliant private clouds.
3. **When to use Azure Local?**
   - For locations needing low-latency compute, data sovereignty, and cloud-managed infrastructure.

## Summary

Azure Local offers flexible, secure, and scalable infrastructure options anchored in Azure’s consistent management, supporting complex enterprise requirements from edge to cloud.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/what-s-new-in-azure-local-cloud-infrastructure-for-distributed/ba-p/4469773)
