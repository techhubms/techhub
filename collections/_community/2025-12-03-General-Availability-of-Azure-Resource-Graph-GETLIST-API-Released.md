---
external_url: https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-general-availability-for-azure-resource-graph-arg-get/ba-p/4474188
title: General Availability of Azure Resource Graph GET/LIST API Released
author: JaspreetKaur
feed_name: Microsoft Tech Community
date: 2025-12-03 04:39:28 +00:00
tags:
- API Contract
- API Throttling
- ARG GET/LIST API
- ARM API
- Azure Governance
- Azure Resource Graph
- Azure Subscription
- Cloud Operations
- Cloud Scalability
- Control Plane Routing
- Feedback
- InstanceView
- Kusto Query Language
- Microsoft Learn
- Performance Optimization
- Quota Management
- Resilient Architecture
- Resource Group
- Resource Management
- Virtual Machines
section_names:
- azure
primary_section: azure
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

*Last updated: Dec 03, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-governance-and-management/announcing-general-availability-for-azure-resource-graph-arg-get/ba-p/4474188)
