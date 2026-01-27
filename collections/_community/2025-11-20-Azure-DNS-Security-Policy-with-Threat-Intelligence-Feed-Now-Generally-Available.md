---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/announcing-azure-dns-security-policy-with-threat-intelligence/ba-p/4470183
title: Azure DNS Security Policy with Threat Intelligence Feed Now Generally Available
author: Sergio Figueiredo
feed_name: Microsoft Tech Community
date: 2025-11-20 20:22:48 +00:00
tags:
- ARM
- Azure DNS
- Azure Private DNS Zones
- Bicep
- DevOps Integration
- DNS Filtering
- DNS Security Policy
- DNS Traffic Monitoring
- Event Hubs
- High Availability
- IaaS Alternatives
- Log Analytics
- Microsoft Security Response Center
- Private Resolver
- SOC
- Terraform
- Threat Intelligence
- Virtual Network
- Zero Day Threats
- Zone Redundancy
section_names:
- azure
- devops
- security
primary_section: azure
---
Sergio Figueiredo announces the general availability of Azure DNS security policy with Threat Intelligence feed, highlighting its impact on DNS threat mitigation, logging, and integration for Azure environments.<!--excerpt_end-->

# Azure DNS Security Policy with Threat Intelligence Feed GA Announcement

**Author:** Sergio Figueiredo, Principal Product Manager

A successful DNS protection strategy should secure your entire Azure environment—across all virtual networks and regions—without introducing operational friction. Azure DNS security policy with Threat Intelligence addresses these needs with:

- **Consistent Protection:** Applies DNS security policies across all VNETs and regions.
- **Real-Time Threat Intelligence:** Blocks and alerts on DNS queries to known malicious domains using continuously updated lists from Microsoft's Security Response Center (MSRC).
- **Visibility & Logging:** Integrates with Log Analytics, Event Hubs, and storage accounts to log DNS activities and provide insight into blocked or risky queries.
- **Low Performance Impact:** Maintains high availability, low latency, and zone redundancy for name resolution services.

## Core Features

- **DNS Traffic Filtering:** Allow, block, or alert on DNS queries at the VNET level for both public and private DNS traffic.
- **Threat Intelligence Feed:** Managed domain lists that automatically update to prevent resolution of malicious domains, offering proactive defense.
- **Monitoring & Alerts:** Capture logs and configure alerts for suspicious DNS activities, supporting SOC teams in reducing incident volume and noise.
- **Infrastructure Integration:** Seamless configuration alongside Azure Private DNS Zones, Private Resolver, and existing Azure networking resources.
- **DevOps Friendly:** Deploy and manage policies via PowerShell, CLI, .NET, Java, Python, REST, Typescript, Go, ARM, and Terraform. Pipeline support with ARM, Bicep, and Terraform for automated workflows.

## Key Use Cases

- Early detection and mitigation of DNS-based security threats, including zero day and ongoing attacks.
- Visibility into DNS traffic and compromised hosts within Azure VNETs.
- Reducing operational costs compared to traditional VM-based DNS appliances.
- Integration with SOC workflows and cloud-native log management solutions.

## Getting Started

- For technical documentation, guidance, and policy templates, visit the [Azure DNS Security Policy docs](https://learn.microsoft.com/en-us/azure/dns/dns-security-policy).
- Community feedback and suggestions are welcome on the [Azure networking community page](https://feedback.azure.com/d365community/forum/8ae9bf04-8326-ec11-b6e6-000d3a4f0789).

## Additional Information

- **Maintained by Microsoft:** Feed is continually refreshed to address new threats as they arise.
- **Flexible Deployment:** Policies can be created, updated, and managed using familiar IaC tooling and Azure management interfaces.

---

Azure DNS security policy with Threat Intelligence feed now empowers organizations to defend against DNS-based attacks with automated, cloud-native threat intelligence feeds, minimizing operational complexity and enhancing overall cloud security.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/announcing-azure-dns-security-policy-with-threat-intelligence/ba-p/4470183)
