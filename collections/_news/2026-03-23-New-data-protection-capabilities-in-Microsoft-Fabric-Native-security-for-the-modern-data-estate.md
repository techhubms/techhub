---
author: Microsoft Fabric Blog
title: 'New data protection capabilities in Microsoft Fabric: Native security for the modern data estate'
section_names:
- ai
- ml
- security
tags:
- AI
- Audit Logs
- Bulk Remove Labels API
- Bulk Set Labels API
- Create Item API
- Data Agents
- Data Exfiltration
- Data Loss Prevention (dlp)
- Data Theft Policy
- DSPM For AI
- Ediscovery
- Fabric Copilot
- Get Item API
- Insider Risk Management (irm)
- KQL Database
- Lakehouse
- List Items API
- Microsoft Fabric
- Microsoft Fabric REST API
- Microsoft Purview
- ML
- News
- OneLake
- Pay as You Go (payg)
- Purview Audit
- Restrict Access
- Retention Policies
- Security
- Semantic Models
- Sensitivity Labels
- SQL Database
- Update Item API
- Warehouse
external_url: https://blog.fabric.microsoft.com/en-US/blog/new-data-protection-capabilities-in-microsoft-fabric-native-security-for-the-modern-data-estate/
date: 2026-03-23 04:09:27 +00:00
feed_name: Microsoft Fabric Blog
primary_section: ai
---

Microsoft Fabric Blog outlines new Microsoft Fabric data protection features powered by Microsoft Purview, including expanded DLP “restrict access” in OneLake, sensitivity labels in public APIs, Insider Risk Management indicators for Lakehouses, and DSPM for AI controls covering Fabric Copilots and data agents.<!--excerpt_end-->

# New data protection capabilities in Microsoft Fabric: Native security for the modern data estate

As organizations scale analytics and AI initiatives, Microsoft Fabric is adding data security capabilities directly into the platform. These updates are powered by deep integration with **Microsoft Purview**, aiming to reduce oversharing risk, improve visibility into sensitive data usage, and speed up response to potential data theft.

## Expanded DLP Restrict Access for Structured Data in OneLake (Preview)

**DLP restrict access** has been expanded to cover **all structured data in OneLake** once sensitive information is detected.

With this release, restrict access now also applies to:

- SQL databases
- KQL databases
- Warehouses

This builds on support that already existed for:

- Lakehouses
- Semantic models

These controls are described as policy-driven and automatically enforced to reduce accidental or intentional exposure while still enabling broad data access for analytics and AI scenarios.

![In the OneLake catalog we can see the restrict access indication on the Warehouse, informing us that there is sensitive info in it and a restriction was applied. A hover card also appears to allow us to view more details](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/in-the-onelake-catalog-we-can-see-the-restrict-acc-5.png)

*Figure: DLP restrict access indication on Warehouse*

\* Support for c is coming soon.

## Sensitivity Labels Available Through Public APIs (Generally Available)

**Sensitivity labels** are now accessible through **public APIs**, enabling programmatic discovery, application, and management of classification across Fabric assets.

New/updated API behavior includes:

- [List Items](https://learn.microsoft.com/rest/api/fabric/core/items/list-items?tabs=HTTP): response includes the **Sensitivity Label ID** for each item
- [Get Item](https://learn.microsoft.com/rest/api/fabric/core/items/get-item?tabs=HTTP): response includes the item’s **Sensitivity Label ID**
- [Create Item](https://learn.microsoft.com/rest/api/fabric/core/items/create-item?tabs=HTTP): you can include a label ID to create an item with a sensitivity label
- [Update Item](https://learn.microsoft.com/rest/api/fabric/core/items/update-item?tabs=HTTP): response includes the **Sensitivity Label ID**

> Note: labels can be retrieved programmatically, but updating labels via this API is not supported.

These additions complement the existing label management APIs:

- [Bulk Set Labels](https://learn.microsoft.com/rest/api/fabric/admin/labels/bulk-set-labels?tabs=HTTP): admins can apply sensitivity labels to items
- [Bulk Remove Labels](https://learn.microsoft.com/rest/api/fabric/admin/labels/bulk-remove-labels?tabs=HTTP): admins can remove sensitivity labels from items

## Insider Risk Management Support for Lakehouse Indicators (Generally Available)

**Microsoft Purview Insider Risk Management (IRM)** now supports **Lakehouse indicators** in Fabric.

By ingesting Fabric audit signals into IRM, security teams can detect, investigate, and respond to risky user activities involving sensitive data in Fabric Lakehouses, using centralized investigation capabilities already used across Microsoft 365 and other sources.

![In Microsoft Purview you can see the Lakehouse signals alongside other signals in the Insider Risk Management section](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/in-microsoft-purview-you-can-see-the-lakehouse-sig-5.png)

*Figure: Lakehouse indicators used within the IRM tool*

## New Quick Data Theft Policy in IRM for Fabric (Generally Available)

A streamlined experience is available for **quick policy creation** of the **Data Theft rule for Fabric**, intended to speed up response to data exfiltration scenarios.

## IRM Pay-as-you-go Usage Report (Generally Available)

A **pay-as-you-go (PAYG) usage report** for Purview Insider Risk Management is available to help with transparency, budget planning, and policy tuning.

Admins can review billed PAYG processing units across:

- Workloads (Fabric)
- Sub-workloads (for example, Power BI, Lakehouse)
- Indicators (for example, downloading Power BI reports)

## Purview Data Security Posture Management (DSPM) for AI for Fabric Copilots and data agents (Preview)

**DSPM for AI for Fabric** adds controls for monitoring and governing AI interactions involving **Fabric Copilots and data agents**.

Capabilities called out include:

- Surfacing data security risks by detecting sensitive info in AI prompts/responses, with recommendations in the DSPM dashboard
- Detecting and investigating risky AI behavior with Insider Risk Management
- Applying governance/oversight through Purview Audit, eDiscovery, retention policies, and detection of non-compliant usage

![Purview DSPM for AI provides admins with comprehensive reports on Fabric Copilot’s user activities, as well as data entered and shared within Fabric Copilots and Agents](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/purview-dspm-for-ai-provides-admins-with-comprehen-5.png)

*Figure: Purview DSPM for AI report showing Data Agent interaction in Fabric*

## Bringing it all together

Microsoft frames these updates around built-in, consistent, end-to-end protection across analytics and AI workloads, emphasizing:

- Sensitivity labeling and automated access restriction in OneLake
- Insider risk detection and investigation for Fabric activity
- Faster remediation workflows for potential data theft

## Links

- [Learn more about DLP restrict access](https://learn.microsoft.com/purview/dlp-powerbi-get-started#supported-actions)
- [Learn more about IRM for Fabric](https://learn.microsoft.com/purview/insider-risk-management-settings-policy-indicators#microsoft-fabric-indicators)
- [Learn more about DSPM for AI in Fabric](https://learn.microsoft.com/purview/ai-copilot-fabric)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/new-data-protection-capabilities-in-microsoft-fabric-native-security-for-the-modern-data-estate/)

