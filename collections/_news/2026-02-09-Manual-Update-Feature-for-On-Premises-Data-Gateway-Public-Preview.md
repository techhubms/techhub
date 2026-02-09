---
layout: "post"
title: "Manual Update Feature for On-Premises Data Gateway (Public Preview)"
description: "This article introduces the public preview of the manual update feature for the On-premises Data Gateway, allowing administrators to control update timing via UI or scripts. It details requirements, update steps, and the future road map toward automated updates for improved flexibility, security, and compliance in Microsoft Fabric environments."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/manual-update-for-on-premises-data-gateway-public-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-09 10:00:00 +00:00
permalink: "/2026-02-09-Manual-Update-Feature-for-On-Premises-Data-Gateway-Public-Preview.html"
categories: ["Azure"]
tags: ["Automation", "Azure", "Cluster Updates", "Data Integration", "Gateway API", "Gateway Management", "IT Administration", "Manual Updates", "Microsoft Fabric", "News", "On Premises Data Gateway", "PowerShell", "Security Patching", "Version Control"]
tags_normalized: ["automation", "azure", "cluster updates", "data integration", "gateway api", "gateway management", "it administration", "manual updates", "microsoft fabric", "news", "on premises data gateway", "powershell", "security patching", "version control"]
---

Microsoft Fabric Blog outlines the new manual update capability for On-premises Data Gateway, giving administrators enhanced control over updates. Authored by the Microsoft Fabric Blog team, this feature preview guides secure and flexible gateway maintenance.<!--excerpt_end-->

# Manual Update Feature for On-Premises Data Gateway (Public Preview)

The Microsoft Fabric team has announced the public preview of a manual update feature for the On-premises Data Gateway. This feature empowers administrators to perform gateway updates on their terms, either through the gateway UI or via API and scripting options, aligning with organizational maintenance needs.

## Key Highlights

- **Manual Update Trigger**: Administrators can initiate updates directly from the UI or programmatically using scripts or APIs, providing flexibility in scheduling and compliance with internal policies.
- **Security and Compliance**: Timely updates enable faster adoption of features, performance enhancements, and security patches, supporting operational stability and compliance.
- **Baseline Version Requirement**: The November 2025 release acts as the baseline; organizations can utilize manual updates starting from December 2025.
- **Future Road Map**: This manual update preview serves as the first step toward a fully automated update process, with more management capabilities planned for upcoming releases.

## How to Get Started

1. Ensure your gateway is updated to the **November 2025 baseline version**.
2. Use the **on-premises data gateway UI** to check for available updates when the feature is released in December 2025.
3. To apply an update, select the update icon in the UI, or alternatively, use PowerShell or scripts:
   
   ```powershell
   Update-DataGatewayClusterMember -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77
   ```

4. Manual updates can be rolled out across servers or clusters, allowing for controlled deployment.

## Visual Guide

- The UI displays current and available versions, allowing administrators to monitor update readiness and initiate upgrades on demand.

![On-premises data gateway manual update screen - current and update versions shown.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/10/image-78.png)
![Manual update screen with 'Update' button.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/10/image-79.png)

## What’s Next

Microsoft is progressing toward a fully automated gateway update experience, planning to further streamline maintenance processes and minimize manual intervention in future versions of the product.

For more information on managing gateway updates, see the official documentation: [Update an on-premises data gateway | Microsoft Learn](https://learn.microsoft.com/data-integration/gateway/service-gateway-update?toc=%2Ffabric%2Fdata-factory%2Ftoc.json&bc=fabric%2Fdata-factory%2Fbreadcrumb%2Ftoc.json#update-gateways-from-the-ui-preview).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/manual-update-for-on-premises-data-gateway-public-preview/)
