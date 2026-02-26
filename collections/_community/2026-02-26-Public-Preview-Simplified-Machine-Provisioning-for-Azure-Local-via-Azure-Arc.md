---
layout: "post"
title: "Public Preview: Simplified Machine Provisioning for Azure Local via Azure Arc"
description: "This article introduces the public preview of simplified machine provisioning for Azure Local, designed to reduce on-site setup complexities for edge deployments. Leveraging Azure Arc, FIDO Device Onboarding, and centralized configuration, organizations can streamline provisioning, maintain security, and scale deployments across multiple sites with minimal IT staff intervention."
author: "PragyaDwivedi"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-public-preview-simplified-machine-provisioning-for/ba-p/4496811"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-26 18:02:37 +00:00
permalink: "/2026-02-26-Public-Preview-Simplified-Machine-Provisioning-for-Azure-Local-via-Azure-Arc.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["ARM Templates", "Azure", "Azure Arc", "Azure Local", "Azure Portal", "Centralized Configuration", "Cluster Creation", "Community", "Configurator App", "DevOps", "Edge Deployment", "FIDO Device Onboarding", "Hybrid Cloud", "Machine Provisioning", "Provisioning Automation", "Security", "Supply Chain Security", "USB Preparation Tool", "Zero Trust"]
tags_normalized: ["arm templates", "azure", "azure arc", "azure local", "azure portal", "centralized configuration", "cluster creation", "community", "configurator app", "devops", "edge deployment", "fido device onboarding", "hybrid cloud", "machine provisioning", "provisioning automation", "security", "supply chain security", "usb preparation tool", "zero trust"]
---

PragyaDwivedi presents an overview of the public preview for simplified machine provisioning on Azure Local, emphasizing streamlined, secure, and scalable infrastructure deployment with minimal on-site requirements.<!--excerpt_end-->

# Public Preview: Simplified Machine Provisioning for Azure Local via Azure Arc

**Author:** PragyaDwivedi  
**Published:** Feb 26, 2026  

Deploying infrastructure at the edge—across environments like retail stores, factories, branches, or remote sites—has long been complicated, often requiring skilled IT personnel on location. The new public preview of Simplified Machine Provisioning for Azure Local simplifies these deployments, allowing IT teams to centrally set up, configure, and monitor infrastructure with reduced on-site effort.

## Key Highlights

- **Centralized provisioning**: Define and manage machine configuration in Azure. On-site tasks are limited to racking hardware, powering on, and USB initialization; all other automation and configuration occurs remotely via Azure services.
- **Automation & Consistency**: Use Azure Resource Manager (ARM) templates to provision hardware, standardize setup, and reduce errors across multiple sites.
- **Security**: Built on the [FIDO Device Onboarding (FDO) specification](https://fidoalliance.org/device-onboarding-overview/), this process incorporates zero trust principles for device identity and supply chain security.
- **Azure Arc Integration**: The workflow relies on [Azure Arc Site](https://learn.microsoft.com/en-us/azure/azure-arc/site-manager/overview), allowing configuration and operational management for each physical location (site) with consistent, reusable provisioning artifacts.
- **Minimal Onsite Interaction**: The on-premises staff use a first-party Microsoft USB Preparation Tool to create bootable media using a maintenance environment package from the Azure portal. After basic setup, IT teams take over remotely from Azure.
- **Visibility & Monitoring**: Real-time deployment status via the Azure portal and [Configurator app](https://learn.microsoft.com/en-us/azure/azure-local/manage/troubleshoot-deployment-configurator-app?view=azloc-2601) to quickly identify and resolve issues.

## Simplified Provisioning Workflow

1. **Minimal Onsite Preparation:**
    - Rack and power on hardware.
    - Prepare a USB drive using the Azure-provided USB Preparation Tool.
    - Boot the machine using USB and export the ownership voucher for the IT team.

2. **Remote Provisioning by IT Teams:**
    - Create/define configuration in Azure Arc (site, network, subscription, deployment settings).
    - Download provisioning artifacts.
    - Kick off automation flows using ARM templates and Azure Portal interfaces.

3. **Cluster Creation & Workload Deployment:**
    - Upon provisioning completion, machines are ready for Azure Local cluster creation and production workload setup using standard Azure Arc procedures.

## Technical Features

- **Open Standards:** FIDO-based onboarding ensures consistent, secure deployment across device classes.
- **Maintenance Environment:** A tiny bootstrap OS initiates Azure Arc connections, installs required extensions, and downloads the Azure Local OS.
- **Reusable Configurations:** Apply the same configurations as you add new devices at a site, ensuring consistency and simplifying operations at scale.
- **Zero Trust & Supply Chain Security:** Secure machine identity, control, and monitoring practices by default.

## How to Get Started

- The public preview is live.
- Try the workflow: [Provisioning trial](https://aka.ms/provision/tryit)
- Learn more: [Full documentation](https://aka.ms/provision/doc)

Organizations are encouraged to review, try, and provide feedback as Microsoft shapes the next generation of edge infrastructure deployment. This new process empowers teams to deploy infrastructure rapidly and securely, even with limited on-site IT resources.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-public-preview-simplified-machine-provisioning-for/ba-p/4496811)
