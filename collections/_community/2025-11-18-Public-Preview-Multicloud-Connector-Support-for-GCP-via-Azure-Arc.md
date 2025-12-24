---
layout: "post"
title: "Public Preview: Multicloud Connector Support for GCP via Azure Arc"
description: "This announcement details the public preview of the Multicloud connector for Google Cloud environments, enabled by Azure Arc. It outlines how users can connect GCP and AWS workloads to Azure, leverage centralized inventory, automate Azure Arc onboarding for VMs, and use Azure management services such as Azure Monitor and Microsoft Defender for Cloud. The post provides step-by-step setup instructions, technical documentation references, and further resources for deep dives."
author: "Meagan McCrory"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-multicloud-connector-support-for-google-cloud/ba-p/4470700"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 16:23:35 +00:00
permalink: "/community/2025-11-18-Public-Preview-Multicloud-Connector-Support-for-GCP-via-Azure-Arc.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Agentless Inventory", "Arc Enabled", "AWS", "Azure", "Azure Arc", "Azure Monitor", "Cloud Operations", "Community", "DevOps", "GCP", "Google Cloud", "Inventory", "Kubernetes", "Microsoft Defender For Cloud", "Microsoft Services", "Multicloud", "OIDC Federation", "Preview", "Resource Graph", "Security", "Security Monitoring", "VM Management"]
tags_normalized: ["agentless inventory", "arc enabled", "aws", "azure", "azure arc", "azure monitor", "cloud operations", "community", "devops", "gcp", "google cloud", "inventory", "kubernetes", "microsoft defender for cloud", "microsoft services", "multicloud", "oidc federation", "preview", "resource graph", "security", "security monitoring", "vm management"]
---

Meagan McCrory introduces the public preview of the Multicloud connector for GCP with Azure Arc, describing its features for integrated inventory and management across cloud providers.<!--excerpt_end-->

# Public Preview: Multicloud Connector Support for GCP via Azure Arc

Azure Arc now enables seamless management and visibility for workloads across Google Cloud (GCP), AWS, and Azure with its new Multicloud connector public preview. This solution allows organizations to:

- **Unified Inventory**: Use agentless, periodic scans of your cloud resources (including GCP Compute VM, GKE, Storage, Functions, and more) projected into Azure as native resources. All GCP metadata, such as labels, is retained for easy querying and tagging from one pane of glass.
- **Azure Arc Onboarding for GCP**: Automatically install the Azure Arc agent on eligible GCP VMs. This unlocks Azure management features like Azure Monitor and Microsoft Defender for Cloud, broadening observability and security across clouds.
- **Step-by-Step Setup**: [Follow the getting started guide](https://aka.ms/multicloud-connector-create) for connector creation and permissions configuration in GCP utilizing OIDC federation.

## Post-Setup Capabilities

- Query all GCP and Azure resources using Azure Resource Graph
- Apply Azure management services to Arc-enabled AWS EC2 and GCP instances ([Azure Arc Supported Operations](https://learn.microsoft.com/azure/azure-arc/servers/overview?toc=%2Fazure%2Fcloud-adoption-framework%2Ftoc.json&bc=%2Fazure%2Fcloud-adoption-framework%2F_bread%2Ftoc.json#supported-cloud-operations))
- Access [multicloud capabilities documentation](https://aka.ms/multi-cloud-connector) for technical deep dive
- Participate in discussions on the [Azure Arc forum](https://techcommunity.microsoft.com/t5/azure-arc/bd-p/azurearc)
- Learn more in the [Ignite session](https://ignite.microsoft.com/sessions/BRK183?source=sessions)

## Key Links

- [Inventory solution](https://aka.ms/multi-cloud-inventory)
- [Azure Arc onboarding steps](https://aka.ms/multicloud-arc-onboarding)
- [Onboarding prerequisites](https://learn.microsoft.com/en-us/azure/azure-arc/multicloud-connector/onboard-multicloud-vms-arc#prerequisites)

Multicloud connector is currently free in preview. For questions, reach out on the forum or via Microsoft support.

*Published by Meagan McCrory, November 18, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/public-preview-multicloud-connector-support-for-google-cloud/ba-p/4470700)
