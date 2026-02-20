---
external_url: https://blog.fabric.microsoft.com/en-US/blog/governance-on-autopilot-the-power-of-default-domain-labels-in-fabric-generally-available/
title: Automating Data Governance in Microsoft Fabric with Default Domain Labels
author: Microsoft Fabric Blog
primary_section: azure
feed_name: Microsoft Fabric Blog
date: 2026-02-09 09:00:00 +00:00
tags:
- Admin Portal
- Azure
- Compliance
- Data Classification
- Data Governance
- Data Security
- Default Domain Labels
- Delegated Authority
- Enterprise Data Management
- Information Protection
- Microsoft Fabric
- Microsoft Purview
- News
- Security
- Security Automation
- Sensitivity Labels
section_names:
- azure
- security
---
Microsoft Fabric Blog explains how to automate data security governance using default domain sensitivity labels, with actionable steps for Fabric admins to enhance compliance and protection across large data estates.<!--excerpt_end-->

# Automating Data Governance in Microsoft Fabric with Default Domain Labels

Managing data security and compliance across a large organization can be challenging, especially as your system grows. The Microsoft Fabric Blog introduces a major improvement: **default domain labels**. This feature allows domain admins to automate the assignment of sensitivity labels when new items such as Lakehouses or Warehouses are created, reducing human error and streamlining compliance.

## Why Use Default Domain Labels?

Default domain labels ensure that every new item in Microsoft Fabric is automatically assigned an appropriate sensitivity label, based on the domain’s default settings. This means security is consistently applied from creation, reducing the risk of accidental data exposure and making governance scalable.

### Key Benefits

- **Security by default:** New items inherit the domain's default sensitivity label, reducing manual steps and mistakes.
- **Delegated authority:** Each business unit can set their own default, suiting varying risk or compliance needs per domain.
- **Reduced compliance overhead:** Engineers and data scientists can focus on building, free from repetitive governance tasks.
- **Scalability:** Apply governance at the container level rather than per workspace or item.

## How to Set a Default Sensitivity Label in Fabric

### Prerequisites

- **Admin Rights:** You need Domain Admin or Fabric Admin permissions.
- **Configured Labels:** Sensitivity labels must be defined and published in your organization's **Microsoft Purview compliance portal**.

### Step-by-Step Instructions

1. **Open the Admin Portal:** In Fabric, click the gear icon (Settings) and select **Admin portal**.
2. **Navigate to Domains:** Click the **Domains** tab on the sidebar.
3. **Pick a Domain:** Select the domain you want to configure (such as Sales or Engineering).
4. **Domain Settings:** Go to **Domain settings** and then **Delegated Settings**.
5. **Information Protection:** Under Information protection, choose the proper sensitivity label from your organization's list (e.g., *Internal*, *Confidential*).
6. **Save:** Click **Apply**. New items created in this domain will now inherit the default label automatically.

![Setting a default domain label is done through the Information protection section in Delegated Settings in the Domain settings](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/02/setting-a-default-domain-label-is-done-through-the.png)

### Pro-Tips for Admins

- **Existing Items:** Only new items are affected. Existing items remain unchanged.
- **Manual Changes Still Possible:** Users can upgrade a label’s sensitivity level if required.
- **Workspace Association:** Make sure workspaces are correctly assigned to domains for the labels to apply.

## Learn More

- [Domain-level default sensitivity labels in Microsoft Fabric documentation](https://learn.microsoft.com/fabric/governance/domain-default-sensitivity-label)
- [Contact Microsoft Fabric team](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR7yKBJI-cjNBipoBowXgYptURjQxMkJNNVg1M0U0R01VVDlQQjRJNFcxUC4u) for feedback.

By leveraging default domain labels, organizations can turn manual governance tasks into invisible infrastructure, increasing security and compliance at scale while minimizing user friction.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/governance-on-autopilot-the-power-of-default-domain-labels-in-fabric-generally-available/)
