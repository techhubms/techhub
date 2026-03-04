---
layout: "post"
title: "Azure Arc Gateway for Kubernetes Now Generally Available"
description: "This announcement introduces the general availability of the Azure Arc Gateway for Arc-enabled Kubernetes. The new gateway simplifies network configuration and onboarding processes by reducing the number of required endpoints, benefiting organizations with strong network controls. Key new features, supported scenarios, operational details, and guidance for getting started are included for prospective users and technical implementers."
author: "jalenmcg"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-the-azure-arc-gateway-for/ba-p/4498561"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-03 23:12:14 +00:00
permalink: "/2026-03-03-Azure-Arc-Gateway-for-Kubernetes-Now-Generally-Available.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Arc Enabled Kubernetes", "Arc Gateway", "Arc Proxy", "Azure", "Azure Arc", "Azure CLI", "Azure Monitor", "Azure Policy", "Cloud Networking", "Cluster Management", "Community", "Custom Locations", "DevOps", "Enterprise Security", "Firewall Configuration", "Kubernetes", "Microsoft Defender For Containers", "Onboarding Automation", "PowerShell", "Security"]
tags_normalized: ["arc enabled kubernetes", "arc gateway", "arc proxy", "azure", "azure arc", "azure cli", "azure monitor", "azure policy", "cloud networking", "cluster management", "community", "custom locations", "devops", "enterprise security", "firewall configuration", "kubernetes", "microsoft defender for containers", "onboarding automation", "powershell", "security"]
---

jalenmcg announces the general availability of Azure Arc Gateway for Arc-enabled Kubernetes, detailing its simplified network configuration and improved onboarding processes for enterprise customers.<!--excerpt_end-->

# Azure Arc Gateway for Kubernetes Now Generally Available

_Author: jalenmcg_

## Overview

The Azure Arc Gateway is now generally available for Arc-enabled Kubernetes clusters. This feature was developed to address the challenges organizations face when configuring network access for Azure Arc integration, especially in environments with strict outbound proxy or firewall rules.

## Key Features and Improvements

- **Endpoint Reduction:** Enables Arc-enabling of Kubernetes clusters with just **9** endpoint approvals instead of 18, cutting required firewall/proxy changes by 50%.
- **Simplified Onboarding:** Consolidates outbound traffic, allowing faster onboarding for organizations operating behind enterprise network restrictions.
- **Consistent Operations:** Streamlines and standardizes how agent and extension traffic is routed to Azure.

## How the Arc Gateway Works

- **Arc Gateway (Azure Resource):** A unique Azure-side endpoint that acts as a single ingress for all Arc-related traffic from the organization’s environment.
- **Azure Arc Proxy:** Resides on every Arc-enabled Kubernetes cluster, routing all agent and extension traffic to Azure via the Arc Gateway endpoint. No separate installation required—it’s part of the Arc agent.
- **Typical traffic flow:** `Arc-enabled Kubernetes agent → Arc Proxy → Enterprise Proxy → Arc Gateway → Target Azure service`

## Supported Scenarios

Arc Gateway improves a number of common Azure Arc for Kubernetes scenarios, including:

- [Arc-enabled Kubernetes Cluster Connect](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/conceptual-cluster-connect)
- [Arc-enabled Kubernetes Resource View](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/kubernetes-resource-view)
- [Custom Location](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/custom-locations)
- [Azure Policy's Extension for Azure Arc](https://learn.microsoft.com/en-us/azure/governance/policy/concepts/policy-for-kubernetes)

Other scenarios, such as Microsoft Defender for Containers, Azure Key Vault, and Container Insights in Azure Monitor, may still require separate allow-listing of certain customer-specific data plane destinations (e.g., Log Analytics workspaces, Storage Accounts, Key Vault URLs). For latest coverage, consult the [Arc gateway documentation](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/arc-gateway-simplify-networking?tabs=azure-cli).

## Steps to Get Started

1. **Create an Arc Gateway Resource:** Via Azure portal, CLI, or PowerShell.
2. **Allow Arc Gateway Endpoints:** Approve the gateway endpoint and core endpoints in your enterprise proxy or firewall.
3. **Onboard or Update Clusters:** Connect your clusters to the Arc gateway resource.

Refer to the [Arc gateway documentation](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/arc-gateway-simplify-networking?tabs=azure-cli) for step-by-step instructions.

## Frequently Asked Questions

- **Do I need to install new software on clusters?** No. The Arc Proxy is included in the Arc-enabled Kubernetes agent.
- **Will all scenarios use the gateway?** Most onboarding scenarios are supported at GA, with some customer-specific endpoints requiring separate allow-listing.
- **Other Infrastructure Types?** Arc gateway is also GA for [Arc-enabled Servers](https://learn.microsoft.com/en-us/azure/azure-arc/servers/arc-gateway?tabs=portal) and [Azure Local](https://learn.microsoft.com/en-us/azure/azure-local/deploy/deployment-azure-arc-gateway-overview?view=azloc-2601&tabs=portal).

## Feedback

Feedback is encouraged—use the [Arc gateway feedback form](https://microsoft-my.sharepoint.com/personal/jalenmcgee_microsoft_com/Documents/aka.ms/ArcGWFeedback) to reach the product team.

_Last updated: March 2, 2026_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-the-azure-arc-gateway-for/ba-p/4498561)
