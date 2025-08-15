---
layout: "post"
title: "Update to Azure API Management Workspaces: Built-in Gateway and Tier Support Changes"
description: "This update discusses recent changes and reversals affecting workspaces within Azure API Management, specifically around built-in gateway support and tier-based workspace limits. It clarifies what is changing, what remains the same, and provides guidance for users relying on different tiers, with resources for further reading."
author: "budzynski"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/update-to-api-management-workspaces-breaking-changes-built-in/ba-p/4443668"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-14 20:52:17 +00:00
permalink: "/2025-08-14-Update-to-Azure-API-Management-Workspaces-Built-in-Gateway-and-Tier-Support-Changes.html"
categories: ["Azure"]
tags: ["API Management Tiers", "API Management Update", "Azure", "Azure API Management", "Basic Tier", "Breaking Changes", "Built in Gateway", "Cloud APIs", "Community", "Developer Tier", "Premium Tier", "Service Level Managed Identity", "Standard Tier", "Workspace Gateways", "Workspaces"]
tags_normalized: ["api management tiers", "api management update", "azure", "azure api management", "basic tier", "breaking changes", "built in gateway", "cloud apis", "community", "developer tier", "premium tier", "service level managed identity", "standard tier", "workspace gateways", "workspaces"]
---

budzynski announces changes to Azure API Management workspaces, focusing on updates to built-in gateway and tier support, and how these reversals affect existing and future deployment strategies.<!--excerpt_end-->

# Update to API Management Workspaces: Built-in Gateway & Tiers Support

**Author:** budzynski  
**Date:** August 14, 2025

## What’s Changing?

Azure is reintroducing support for workspaces on the built-in gateway in Azure API Management. This effectively rescinds some of the previously announced breaking changes. If your API Management service uses preview workspaces on the built-in gateway and meets the following tier-based limits, those workspaces will continue to function as-is and will automatically transition to general availability when the feature is announced.

**Current workspace limits by API Management tier:**

| API Management Tier     | Max Workspaces on Built-in Gateway |
|------------------------|------------------------------------|
| Premium, Premium v2    | 30                                 |
| Standard, Standard v2  | 5                                  |
| Basic, Basic v2        | 1                                  |
| Developer              | 1                                  |

## Why This Change?

The workspace gateway requirement was introduced to increase reliability and scalability in large federated API environments. However, Microsoft recognizes that many customers have workflows depending on the preview workspaces model or need workspaces in non-Premium tiers. As a result, built-in gateway support is being kept available for these scenarios, within the stated limits.

## What’s Not Changing?

Other workspace-related breaking changes remain. For example, service-level managed identities are still not available within workspaces. Premium and Premium v2 services continue to support deploying workspaces with dedicated workspace gateways.

## Resources

- [Workspaces in Azure API Management](https://aka.ms/apimdocs/workspaces)
- [Reduced tier availability](https://learn.microsoft.com/azure/api-management/breaking-changes/workspaces-breaking-changes-june-2024)
- [Requirement for workspace gateways](https://learn.microsoft.com/azure/api-management/breaking-changes/workspaces-breaking-changes-march-2025)

For further discussion and official updates, follow the [Azure Integration Services Blog](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/bg-p/IntegrationsonAzureBlog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/update-to-api-management-workspaces-breaking-changes-built-in/ba-p/4443668)
