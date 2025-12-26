---
layout: "post"
title: "Exploring Developer Tier APIM with Self-hosted Gateway for Greater Flexibility"
description: "This post examines the strengths and trade-offs of using the Developer tier of Azure API Management (APIM) in conjunction with a self-hosted gateway. It compares APIM tiers, discusses premium features available in the Developer tier, and explains how organizations can increase flexibility and control by managing their own gateway infrastructure via Azure VMs. Readers will learn the practical considerations and typical use cases for this architecture."
author: "reve"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-paas-blog/developer-tier-apim-self-hosted-gateway/ba-p/4457556"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-27 16:45:43 +00:00
permalink: "/community/2025-09-27-Exploring-Developer-Tier-APIM-with-Self-hosted-Gateway-for-Greater-Flexibility.html"
categories: ["Azure"]
tags: ["API Analytics", "API Gateway", "APIM", "Azure", "Azure API Management", "Azure Monitor", "Azure VM", "Cloud Architecture", "Community", "Developer Tier", "Entra Integration", "Feature Comparison", "Self Hosted Gateway", "Service Availability", "Virtual Network"]
tags_normalized: ["api analytics", "api gateway", "apim", "azure", "azure api management", "azure monitor", "azure vm", "cloud architecture", "community", "developer tier", "entra integration", "feature comparison", "self hosted gateway", "service availability", "virtual network"]
---

reve outlines the use of Azure API Management's Developer tier alongside a self-hosted gateway, highlighting its premium features, limitations, and scenarios where this setup can improve control and minimize service interruptions.<!--excerpt_end-->

# Developer Tier APIM + Self-hosted Gateway: Architecture and Considerations

Azure API Management (APIM) offers multiple service tiers tailored to different use cases. The Developer tier, while typically intended for non-production scenarios, includes access to many features usually reserved for premium plans—such as Virtual Network (VNet) injection, Microsoft Entra integration, private endpoint support, and a self-hosted gateway option.

## Feature Comparison of APIM Tiers

Here is an overview of selected features across APIM tiers:

| Feature                                    | Consumption | Developer | Basic | Basic v2 | Standard | Standard v2 | Premium | Premium v2 (preview) |
|--------------------------------------------|-------------|-----------|-------|----------|----------|-------------|---------|----------------------|
| Microsoft Entra integration                | No          | Yes       | No    | Yes      | Yes      | Yes         | Yes     | Yes                  |
| VNet injection support                     | No          | Yes       | No    | No       | No       | No          | Yes     | Yes                  |
| Private endpoint support                   | No          | Yes       | Yes   | No       | Yes      | Yes         | Yes     | No                   |
| Self-hosted gateway                        | No          | Yes       | No    | No       | No       | No          | Yes     | No                   |
| API analytics                              | No          | Yes       | Yes   | Yes      | Yes      | Yes         | Yes     | Yes                  |
| Built-in cache                             | No          | Yes       | Yes   | Yes      | Yes      | Yes         | Yes     | Yes                  |
| Management over Git                        | No          | Yes       | Yes   | No       | Yes      | No          | Yes     | No                   |
| Azure Monitor & Log Analytics request logs | No          | Yes       | Yes   | Yes      | Yes      | Yes         | Yes     | Yes                  |
| And more...                               |             |           |       |          |          |             |         |                      |

See the [official APIM feature comparison](https://learn.microsoft.com/en-us/azure/api-management/api-management-features) for a full list.

## Limitations of Developer Tier

- **No SLA Guarantee:** The Developer tier is designed for non-production/evaluation
- **Service Interruptions:** It may be disrupted by maintenance events (such as OS upgrades)
- **Intended for Testing:** Not recommended for mission-critical workloads

## Combining with Self-hosted Gateway

To increase control over API gateway availability while leveraging the Developer tier's rich features, you can deploy a self-hosted gateway on your own Azure VM or infrastructure. This approach allows:

- **Direct management of underlying infrastructure** (e.g., schedule your own VM updates)
- **Greater flexibility over maintenance schedules**—helpful in avoiding outages during business hours
- **Continued access to Developer tier "premium" features**

### Example Use Case

- **Set up APIM Developer tier in Azure**
- **Provision an Azure VM**
- **Install and configure the self-hosted APIM gateway on the VM**
- **Manage VM updates according to business needs**

This hybrid setup gives teams improved infrastructure control compared to using Developer tier APIM alone, at a lower cost than Premium, although still without a formal SLA.

## Conclusion

Combining Azure API Management Developer tier with a self-hosted gateway can provide more flexibility and control for non-production or cost-sensitive environments, especially when direct infrastructure management is needed. Be mindful, however, that this architecture is not covered by Microsoft's production SLA, and is best suited for dev/test workloads or evaluation scenarios.

---
Author: reve

Original post: [Feature-based comparison of Azure API Management tiers | Microsoft Learn](https://learn.microsoft.com/en-us/azure/api-management/api-management-features)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-paas-blog/developer-tier-apim-self-hosted-gateway/ba-p/4457556)
