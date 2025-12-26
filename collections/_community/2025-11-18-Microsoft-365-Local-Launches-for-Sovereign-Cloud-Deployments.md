---
layout: "post"
title: "Microsoft 365 Local Launches for Sovereign Cloud Deployments"
description: "This announcement details the general availability of Microsoft 365 Local, a deployment framework that enables Exchange Server, SharePoint Server, and Skype for Business Server on Azure Local within sovereign environments. It outlines the technical architecture, security, infrastructure management via Azure as the control plane (including Azure Arc, Azure Monitor, Microsoft Defender for Cloud), and compliance capabilities for organizations with stringent data sovereignty requirements, covering both connected and upcoming disconnected scenarios."
author: "daniel-lee-msft"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/microsoft-365-local-is-generally-available/ba-p/4470170"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:00:00 +00:00
permalink: "/community/2025-11-18-Microsoft-365-Local-Launches-for-Sovereign-Cloud-Deployments.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Arc", "Azure Local", "Azure Monitor", "Community", "Compliance", "Data Sovereignty", "Exchange Server", "Identity Management", "Infrastructure Management", "Microsoft 365 Local", "Microsoft Defender For Cloud", "Network Security Groups", "Privileged Access", "Security", "Security Baseline", "SharePoint Server", "Skype For Business Server", "Software Defined Networking", "Sovereign Cloud"]
tags_normalized: ["azure", "azure arc", "azure local", "azure monitor", "community", "compliance", "data sovereignty", "exchange server", "identity management", "infrastructure management", "microsoft 365 local", "microsoft defender for cloud", "network security groups", "privileged access", "security", "security baseline", "sharepoint server", "skype for business server", "software defined networking", "sovereign cloud"]
---

daniel-lee-msft announces Microsoft 365 Local's general availability, bringing Exchange, SharePoint, and Skype for Business to Azure Local with enhanced jurisdictional control, security, and compliance capabilities for sovereign workloads.<!--excerpt_end-->

# Microsoft 365 Local Launches for Sovereign Cloud Deployments

Microsoft 365 Local is now generally available, delivering a new deployment framework that allows organizations and governments to run core collaboration tools—Exchange Server, SharePoint Server, Skype for Business Server—on Azure Local for robust data sovereignty and compliance.

## Key Features and Architecture

- Based on [Microsoft Sovereign Cloud](https://www.microsoft.com/industry/sovereignty/cloud) and [Azure Local Premier Solutions](https://azurelocalsolutions.azure.microsoft.com/)
- Validated reference architecture supporting mission-critical sovereign and regulatory requirements
- Supports deployment sizing, configuration, and best practices via partner-led services

## Infrastructure Management

- Azure acts as the central control plane for managing wide-ranging workloads
- Full-stack deployment visibility in Azure portal, spanning servers and clusters
- All hosts and VMs are Arc-enabled by default for built-in monitoring (connectivity, health, security updates)

## Security and Compliance Controls

- Network Security Groups managed with SDN (software defined networking) via Azure Arc
- Security baseline of over 300 settings enforced across host/VMs, covering:
  - Network security
  - Identity and access management
  - Privileged access
  - Data protection
- Built-in compliance features help organizations meet local regulations and reduce risk

## Integration with Azure Services

- Unified management with Azure Monitor and Microsoft Defender for Cloud
- Security alerts, health checks, and recommendations integrated for proactive governance

## Disconnected Operations Coming

- Azure Local disconnected operation support planned for early 2026—enabling fully offline sovereign deployments for sensitive workloads

## Additional Resources

- Learn more: [Microsoft 365 Local Documentation](https://aka.ms/M365LocalDocs)
- Partner consultation: [Sign up for support](https://aka.ms/M365LocalSignup)

**Version:** 1.0 (Updated Nov 17, 2025)

**Author:** daniel-lee-msft

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/microsoft-365-local-is-generally-available/ba-p/4470170)
