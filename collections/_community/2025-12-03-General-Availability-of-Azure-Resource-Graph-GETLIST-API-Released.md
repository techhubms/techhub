---
layout: "post"
title: "General Availability of Azure Resource Graph GET/LIST API Released"
description: "This announcement details the release of the Azure Resource Graph (ARG) GET/LIST API and its practical impact on Azure management and governance. It highlights the API’s ability to deliver significantly higher throttling quotas, reduce read throttling, and improve performance for resource queries in the cloud. The post provides guidance on usage scenarios, covers API request structure and routing mechanics, compares ARG GET/LIST with traditional ARM and ARG queries, and links to supporting resources for implementation, limitations, and feedback."
author: "JaspreetKaur"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-general-availability-for-azure-resource-graph-arg-get/ba-p/4474188"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-12-03 04:39:28 +00:00
permalink: "/2025-12-03-General-Availability-of-Azure-Resource-Graph-GETLIST-API-Released.html"
categories: ["Azure"]
tags: ["API Contract", "API Throttling", "ARG GET/LIST API", "ARM API", "Azure", "Azure Governance", "Azure Resource Graph", "Azure Subscription", "Cloud Operations", "Cloud Scalability", "Community", "Control Plane Routing", "Feedback", "InstanceView", "Kusto Query Language", "Microsoft Learn", "Performance Optimization", "Quota Management", "Resilient Architecture", "Resource Group", "Resource Management", "Virtual Machines"]
tags_normalized: ["api contract", "api throttling", "arg getslashlist api", "arm api", "azure", "azure governance", "azure resource graph", "azure subscription", "cloud operations", "cloud scalability", "community", "control plane routing", "feedback", "instanceview", "kusto query language", "microsoft learn", "performance optimization", "quota management", "resilient architecture", "resource group", "resource management", "virtual machines"]
---

JaspreetKaur announces the general availability of Azure Resource Graph GET/LIST API, explaining how this new feature provides improved throttling quotas and optimized resource query performance for Azure workloads.<!--excerpt_end-->

# General Availability of Azure Resource Graph GET/LIST API Released

Azure Resource Graph (ARG) GET/LIST API is now generally available, unlocking significant improvements in how users query and manage resources in Azure at scale. This feature delivers 10X higher throttling quotas compared to traditional ARG queries, allowing for more scalable and resilient resource lookups across subscriptions, resource groups, and parent resources.

## Key Advantages

- **Higher Throttling Quotas:** Easily manage bursty traffic and high-volume GET requests with 10X the previous limits, minimizing throttling incidents.
- **Intelligent Control Plane Routing:** By appending the parameter `useResourceGraph=true` to eligible GET/LIST API requests, callers can route through the optimized backend for faster, more reliable performance.
- **Seamless Integration:** The API contract follows the ARM control plane standards, ensuring minimal changes to current workflows while delivering improved results.

## When to Use ARG GET/LIST API

Use ARG GET/LIST API in scenarios such as:

- High volume of resource GETs within a single subscription or resource group.
- Susceptibility to throttling from traffic spikes or quota competition.
- Need for highly available and low-latency resource lookups.

**Supported Tables:** Use only with `resources` and `computeresources` tables at this time. More details are available in Microsoft Learn documentation.

## How to Use the API

1. Confirm your scenario aligns with supported call patterns and throttling needs.
2. Add `&useResourceGraph=true` to the query string of your GET/LIST API call.
3. Requests are routed to the optimized backend only when this parameter is present; otherwise, calls proceed as usual to the resource provider.

**Learn more:**

- [Azure Resource Graph GET/LIST API Guidance - Azure Resource Graph | Microsoft Learn](https://learn.microsoft.com/en-us/azure/governance/resource-graph/concepts/azure-resource-graph-get-list-api#arg-getlist-api-contract)

## Example Scenarios and Usage

- **ARG Query via ARG Explorer:** Use KQL to query resources:

  ```kusto
  Resources | where type =~ 'microsoft.compute/virtualmachines' | where id =~ '/subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/microsoft.compute/virtualmachines/{vm}'
  ```

- **ARM (Compute RP) API:**

  ```http
  GET https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/microsoft.compute/virtualmachines/{vm}?api-version=2024-07-01&$expand=instanceView
  ```

- **ARG GET/LIST API:**

  ```http
  GET https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/microsoft.compute/virtualmachines/{vm}?api-version=2024-07-01&$expand=instanceView&useResourceGraph=true
  ```

For more samples and details, consult [Documentation](https://learn.microsoft.com/en-us/azure/governance/resource-graph/concepts/azure-resource-graph-get-list-api#some-frequently-used-examples).

## Additional Resources

- [Overview](https://learn.microsoft.com/en-us/azure/governance/resource-graph/concepts/azure-resource-graph-get-list-api)
- [Known Limitations](https://learn.microsoft.com/en-us/azure/governance/resource-graph/concepts/azure-resource-graph-get-list-api#known-limitations)
- [FAQ](https://learn.microsoft.com/en-us/azure/governance/resource-graph/concepts/azure-resource-graph-get-list-api#frequently-asked-questions)
- [Video Walkthrough](https://www.youtube.com/watch?v=h6ieZqCO_90)

## Feedback and Community

- Reach the Azure Resource Graph team at [argpms@microsoft.com](mailto:argpms@microsoft.com).
- Product feedback and ideas: [Azure Governance · Community](https://feedback.azure.com/d365community/forum/675ae472-f324-ec11-b6e6-000d3a4f0da0)

---

*Author: JaspreetKaur*

**Happy Querying!**

_Last updated: Dec 03, 2025_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-general-availability-for-azure-resource-graph-arg-get/ba-p/4474188)
