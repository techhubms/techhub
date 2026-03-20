---
title: On-premises data gateway auto-update (admin triggered) (Generally Available)
primary_section: ml
tags:
- Admin Triggered Updates
- Automation Scripts
- Change Management
- Compliance
- Data Factory
- DevOps
- Gateway Auto Update
- Gateway Clusters
- Lifecycle Management
- Maintenance Windows
- Microsoft Fabric
- ML
- News
- November Release
- On Premises Data Gateway
- PowerShell
- Staged Deployment
- Update DataGatewayClusterMember
date: 2026-03-18 05:46:50 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-auto-update-admin-triggered-generally-available/
section_names:
- devops
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces general availability of admin-triggered auto-update for the on-premises data gateway, explaining how admins can schedule and automate upgrades (including a PowerShell option) to better fit maintenance windows, change control, and compliance needs.<!--excerpt_end-->

# On-premises data gateway auto-update (admin triggered) (Generally Available)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

The **on-premises data gateway auto-update (admin triggered)** capability gives administrators more control over **how and when** gateway updates are applied. The goal is to better align upgrades with:

- Organizational maintenance windows
- Change management processes
- Compliance requirements

With admin-triggered auto-update, gateway administrators can **trigger upgrades on demand**, helping organizations adopt:

- New features
- Performance improvements
- Security patches

## What’s new with auto-update (admin triggered)

With this release, administrators can:

- **Trigger gateway updates on demand** instead of installing/uninstalling on local machines.
- **Control update timing** to match internal maintenance windows and policies.
- **Reduce operational risk** by planning and validating updates before deployment.
- **Apply updates programmatically** using PowerShell or scripts for automation scenarios.
- **Standardize update processes** across multiple servers or clusters.

This is positioned as particularly useful for enterprises that need:

- Staged deployment
- Validation in test environments
- Strict change control procedures

## Baseline version requirement

The **November 2025 release** is the baseline required for this feature.

- Gateways must be updated to **November 2025 (or later)** to use admin-triggered auto-update.

## Getting started

1. **Update to the baseline version**
   - Ensure your gateway is running the **November 2025** release or newer.

2. **Check for available updates in the gateway UI**
   - Open the on-premises data gateway UI.
   - When a newer version is available, you’ll see an update indicator showing version availability details.

![Screenshot of the gateway manual update from the UI.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-gateway-manual-update-from-the-u.png)

*Figure: Administrators can trigger on-premises data gateway auto-updates to quickly apply the latest improvements, security fixes, and reliability enhancements.*

3. **Trigger the update manually**
   - Select the update icon to start the manual upgrade process when you’re ready.

4. **Use PowerShell for programmatic updates (optional)**
   - You can initiate updates through PowerShell or automation scripts.

### PowerShell example

```powershell
Update-DataGatewayClusterMember -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77
```

This is intended to enable centralized and repeatable update workflows across environments.

## Looking ahead

Microsoft states this is part of a broader push toward more flexible, enterprise-ready gateway lifecycle management, with planned investment in:

- Additional automation
- Additional management options

## Further reading

- Update an on-premises data gateway: https://learn.microsoft.com/data-integration/gateway/service-gateway-update?toc=%2Ffabric%2Fdata-factory%2Ftoc.json&bc=fabric%2Fdata-factory%2Fbreadcrumb%2Ftoc.json#update-gateways-from-the-ui-preview


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/on-premises-data-gateway-auto-update-admin-triggered-generally-available/)

