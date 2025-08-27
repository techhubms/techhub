---
layout: "post"
title: "Evaluating Azure Application Gateway and WAF Placement in API Architectures"
description: "A practical discussion led by TheCitrixGuy and the Azure community on whether to place Azure Application Gateway with Web Application Firewall (WAF) in front of API Management Services (APIM), weighing costs, scalability, and alternative approaches like Azure Front Door and Cloudflare. The conversation includes real-world deployment experiences, trade-offs in multi-region/multi-environment setups, integration with AKS, and cost-saving tips."
author: "TheCitrixGuy"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mjvyr1/application_gateway_thoughts/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-08-07 10:00:00 +00:00
permalink: "/2025-08-07-Evaluating-Azure-Application-Gateway-and-WAF-Placement-in-API-Architectures.html"
categories: ["Azure", "Security"]
tags: ["AKS", "API Management", "API Ops", "API Security", "APIM", "Azure", "Azure Application Gateway", "Azure Front Door", "Cloudflare", "Community", "Cost Optimization", "Firewall", "Landing Zone", "Networking", "Private Link", "Security", "WAF Policy", "Web Application Firewall"]
tags_normalized: ["aks", "api management", "api ops", "api security", "apim", "azure", "azure application gateway", "azure front door", "cloudflare", "community", "cost optimization", "firewall", "landing zone", "networking", "private link", "security", "waf policy", "web application firewall"]
---

TheCitrixGuy and members of the Azure community explore the architectural and cost considerations of using Azure Application Gateway with WAF in front of APIM. They share alternative solutions including Azure Front Door and Cloudflare, offering deployment insights and practical recommendations.<!--excerpt_end-->

# Evaluating Azure Application Gateway - Community Thoughts

**Discussion led by:** TheCitrixGuy

## Context

The community reviews architectural strategies for routing internal and external APIs through Azure API Management Services (APIM). A key question is whether to position Azure Application Gateway (AGW) with Web Application Firewall (WAF) in front of APIM, following Microsoft's recommended patterns, and how this impacts manageability and costs at scale.

## Key Discussion Points

### 1. Microsoft's Recommended Architecture

- Microsoft often suggests putting an AGW with WAF in front of each APIM instance for centralized security and routing.
- In complex organizations, this could multiply AGW and APIM resources across environments and regions, increasing cost and complexity.

### 2. Real-World Deployments

- Many clients typically use a single AGW shared across multiple apps or environments, except in multi-region or low-latency scenarios.
- AGW is recognized as costly with a steep learning curve, but is deeply embedded in Azure connectivity patterns.
- Using one AGW per landing zone is considered viable, routing all inbound traffic and relying on additional layers like Azure Firewall for segmentation.

### 3. Alternatives to AGW + WAF

- **Azure Front Door (FD):**
  - Can be used with WAF as an alternative, often seen as more globally distributed and cost effective.
  - Internal mode APIM still requires AGW, since FD can't privatelink directly to APIM.
  - FD provides SSL certificate management and consolidated WAF policy.
  - Some users have experienced outages impacting trust; client example: migrated to Cloudflare from FD to reduce costs and increase reliability.
- **Cloudflare Zero Trust/Tunnels:**
  - Used for secure, cloud-based routing with cost advantages – for some, this replaced AGW/FD entirely, saving thousands annually.

### 4. Best Practices and Cost Management

- AGW with WAF is among the most expensive Azure resources—always model costs before scaling up architectures.
- Consider deploying AGW and APIM as shared services for capacity and scaling efficiencies.
- Managing WAF policies and APIM endpoints as code (API Ops) enables consistency and automation.
- FD instances can be split by domains or endpoints for environmental separation.
- Not strictly necessary to use AGW fronting APIM—it's only one of several approaches to add WAF and routing.

### 5. Kubernetes and API Traffic

- Query raised: Do teams use dedicated AGW per AKS cluster, or share one AGW across clusters?
- Common practice is to use a shared AGW per landing zone to centralize inbound routing.

## Community Recommendations

- Use AGW or FD based on required features (private link, SSL, global reach, etc.) and trust/history with services.
- Model costs carefully—AGW WAF is expensive, FD is typically more cost friendly.
- Where possible, consolidate routing and WAF to minimize resource sprawl and operational complexity.
- Organize configuration using DevOps practices for repeatability.

## Summary

Azure’s recommended reference architectures provide a solid baseline, but can lead to high costs and added complexity when strictly followed. The community emphasizes balancing security, manageability, and cost, considering alternatives like Azure Front Door and Cloudflare when appropriate. Real-world deployment patterns often deviate from “one AGW per service/environment,” preferring shared and automated solutions.

---
*This summary is based on practical experiences and advice from the Azure community, led by TheCitrixGuy.*

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mjvyr1/application_gateway_thoughts/)
