---
layout: "post"
title: "Public Preview: Self-Service Quota Management for Azure App Service"
description: "This post introduces the public preview of a self-service quota management experience for Azure App Service within the Azure portal. The new feature provides a dedicated interface to view, adjust, and manage quota limits for App Service plans, including automation for many requests and guidance for handling exceptions. Developers and IT admins gain more control for optimizing performance and avoiding disruptions."
author: "jordanselig"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-the-public-preview-of-the-new-app-service-quota-self/ba-p/4450415"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-04 21:49:24 +00:00
permalink: "/2025-09-04-Public-Preview-Self-Service-Quota-Management-for-Azure-App-Service.html"
categories: ["Azure"]
tags: ["App Service Plans", "Azure", "Azure App Service", "Azure Portal", "Capacity Management", "Cloud Platforms", "Community", "Deployment", "Portal Experience", "Public Preview", "Quota Management", "Resource Management", "Scaling", "Self Service", "Support Tickets", "VM SKUs"]
tags_normalized: ["app service plans", "azure", "azure app service", "azure portal", "capacity management", "cloud platforms", "community", "deployment", "portal experience", "public preview", "quota management", "resource management", "scaling", "self service", "support tickets", "vm skus"]
---

jordanselig details the new self-service quota management experience for Azure App Service, helping developers and IT admins proactively manage resource limits via an updated portal interface.<!--excerpt_end-->

# Public Preview: Self-Service Quota Management for Azure App Service

**Author:** jordanselig  

The Azure team has announced a public preview of a redesigned self-service experience for managing App Service quotas. This update introduces a dedicated App Service Quota blade in the Azure portal, enabling developers and IT admins to gain better visibility and control over resource allocations.

## What's New?

- **Dedicated Quota Blade:** Access a new portal area to view current quota usage and limits for all App Service SKUs (VM sizes).
- **Customizable Quotas:** Set quotas suited to your application's needs without filing a support ticket in many cases.
- **Improved Resource Management:** Proactively manage application infrastructure, prevent service disruptions, and optimize performance.

## Quick Reference

1. **Large/Multi-Subscription Deployments:** For quota changes involving ten or more subscriptions, or any requiring zone redundancy, open a support ticket (Problem type: Quota).
2. **Other Cases:** Use the new self-service portal feature for immediate quota adjustments.

## How to Adjust App Service Quotas

### 1. Navigate to the Quotas Resource Provider

- Use the Azure portal and find the Quotas resource provider.

### 2. Select App Service

- Each VM size/SKU appears as a separate entry in this interface.
- You can filter by region, subscription, provider, or usage.
- Group data by usage, quota (SKU), or region.
- Track current usage in App Service VMs and spot SKUs nearing their quota limits.

### 3. Request Quota Adjustments

- Use the pen icon to specify a new quota value for the chosen SKU (not incremental; enter the desired final value).
- Submit your request for automatic processing.

#### Quota Approval Workflow

- If the portal can fulfill your request automatically, you'll be notified within minutes.
- If your request exceeds limits permitted for automatic approval, you'll be prompted to file a support request with the relevant details prefilled.

### 4. Support Tickets (If Needed)

- For zone-redundant needs or bulk quota requests across subscriptions, create a support ticket (Technical > Quota).
- Remember to specify the region and App Service plan. You can track requests via the [Help + support dashboard](https://ms.portal.azure.com/#view/Microsoft_Azure_Support/HelpAndSupportBlade/~/overview).

## Known Issues (Public Preview Phase)

- Closing the quota request flyout window halts real-time notifications—leave it open until completion for status updates.
- Some SKUs may be missing from the dashboard initially (to be added over time).
- The Activity Log currently lacks detailed quota request histories (planned improvement).
- Zone-redundant deployments still require support ticket intervention; quota changes remain regional.
- Documentation in progress for supporting automated bulk requests via API.

## Feedback

The Azure team encourages users to provide feedback or report issues with the new quota management experience in the comments section.

---

*Updated: Sep 04, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/announcing-the-public-preview-of-the-new-app-service-quota-self/ba-p/4450415)
