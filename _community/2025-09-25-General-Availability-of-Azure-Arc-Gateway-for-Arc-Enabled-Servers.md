---
layout: "post"
title: "General Availability of Azure Arc Gateway for Arc-Enabled Servers"
description: "This post details the general availability of the Azure Arc Gateway for Arc-enabled servers, focusing on how it streamlines network configuration by reducing required endpoints and improving the onboarding process. Key scenarios, usage guidance, and technical FAQs are covered to help IT professionals efficiently adopt Azure Arc in enterprise environments."
author: "jalenmcg"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-the-azure-arc-gateway-for/ba-p/4456356"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-25 13:16:23 +00:00
permalink: "/2025-09-25-General-Availability-of-Azure-Arc-Gateway-for-Arc-Enabled-Servers.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Arc Enabled Servers", "Arc Gateway", "Azure", "Azure Arc", "Azure CLI", "Azure Extensions", "Azure Security", "Community", "DevOps", "Enterprise Proxy", "Firewall", "Hybrid Cloud", "Log Analytics", "Network Configuration", "Onboarding", "PowerShell", "Security", "Server Management", "Windows Admin Center"]
tags_normalized: ["arc enabled servers", "arc gateway", "azure", "azure arc", "azure cli", "azure extensions", "azure security", "community", "devops", "enterprise proxy", "firewall", "hybrid cloud", "log analytics", "network configuration", "onboarding", "powershell", "security", "server management", "windows admin center"]
---

jalenmcg introduces the general availability of Azure Arc Gateway, outlining its benefits for Arc-enabled server onboarding, enterprise networking, and security teams.<!--excerpt_end-->

# General Availability of Azure Arc Gateway for Arc-Enabled Servers

Azure has announced the general availability (GA) of the Arc Gateway for Arc-enabled servers. This feature aims to simplify the network configuration process required for Azure Arc by consolidating outbound connectivity through a small, predictable set of endpoints. For organizations operating behind enterprise proxies or firewalls, this means:

- Faster and smoother onboarding of servers to Azure Arc
- Fewer network change requests required
- Simplified network and security operations

## What's New: Reduced Endpoint Requirements

Previously, onboarding an Arc-enabled server required allowing 19 distinct endpoints through enterprise firewalls or proxies. With Arc Gateway GA, only 7 endpoints are required—a reduction of about 63%. This change removes friction for both security and networking teams, making Azure Arc more accessible for enterprises with strict outbound network controls.

## Key Benefits

- **Accelerated Onboarding:** Reduces network approvals and change requests for new servers
- **Simplified Operations:** Consistent traffic routing for all Arc agent and extension traffic

## How Arc Gateway Works

Arc Gateway operates through two main components:

1. **Arc Gateway (Azure Resource):** Acts as a single endpoint in an Azure tenant, receiving incoming traffic from on-premises servers and forwarding it to the required Azure service.
2. **Azure Arc Proxy (on each server):** Part of the core Arc agent, this routes agent and extension traffic through enterprise proxies/firewalls to the Arc Gateway. No separate installation is needed.

**Traffic Path:**

- Arc agent → Arc Proxy → Enterprise Proxy → Arc Gateway → Target Azure Service

## Supported Scenarios at GA

- Windows Admin Center
- SSH
- Extended Security Updates (ESU)
- Azure Extension for SQL Server

**Note:** Some customer-specific data destinations (like Log Analytics workspace or Key Vault URLs) may still need to be explicitly allowed in your enterprise network. For the latest supported scenarios, consult the official [Arc Gateway documentation](https://learn.microsoft.com/en-us/azure/azure-arc/servers/arc-gateway?tabs=portal).

## Getting Started With Arc Gateway

1. **Create an Arc Gateway resource** using the Azure Portal, Azure CLI, or PowerShell.
2. **Allow the Arc Gateway endpoint** (and other core endpoints) in your proxy or firewall.
3. **Onboard or update servers** to use the Arc Gateway and manage them via Azure Arc.

For detailed instructions, check the [step-by-step documentation](https://learn.microsoft.com/en-us/azure/azure-arc/servers/arc-gateway?tabs=portal) or watch the [Arc Gateway Jumpstart video demo](https://www.youtube.com/watch?v=rF1jC5Lmyu0).

## FAQs

**Q: Does Arc Gateway require new software on my servers?**  
A: No, Arc Proxy is included with the standard connected machine agent for Arc-enabled servers.

**Q: Will every Arc scenario route through the gateway now?**  
A: Many high-value scenarios are supported at GA; some customer-specific data endpoints may still require explicit network allowances. The documentation provides up-to-date coverage.

**Q: When will Arc Gateway for Azure Local be GA?**  
A: It is GA now. See the [official announcement](https://techcommunity.microsoft.com/blog/AzureArcBlog/announcing-the-general-availability-of-arc-gateway-for-azure-local/4456256) for more details.

**Q: When will Arc Gateway for Arc-enabled Kubernetes be GA?**  
A: It is still in public preview. For updates, check the [public preview docs](https://learn.microsoft.com/en-us/azure/azure-arc/kubernetes/arc-gateway-simplify-networking?tabs=azure-cli).

## Feedback

Azure encourages feedback on Arc Gateway for servers—share your experiences, suggestions, or requests for future scenarios through the [Arc Gateway feedback form](https://microsoft-my.sharepoint.com/personal/jalenmcgee_microsoft_com/Documents/aka.ms/ArcGWFeedback).

---

**References:**

- [Arc Gateway Documentation](https://learn.microsoft.com/en-us/azure/azure-arc/servers/arc-gateway?tabs=portal)
- [Arc Gateway Demo Video](https://www.youtube.com/watch?v=rF1jC5Lmyu0)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-arc-blog/announcing-the-general-availability-of-the-azure-arc-gateway-for/ba-p/4456356)
