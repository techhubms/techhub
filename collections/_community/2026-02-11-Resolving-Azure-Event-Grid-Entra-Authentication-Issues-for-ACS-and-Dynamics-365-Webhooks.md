---
external_url: https://techcommunity.microsoft.com/t5/azure/how-to-fix-azure-event-grid-entra-authentication-issue-for-acs/m-p/4494308#M22430
title: Resolving Azure Event Grid Entra Authentication Issues for ACS and Dynamics 365 Webhooks
author: ani_ms_emea
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-11 11:41:04 +00:00
tags:
- App Role Assignment
- Azure
- Azure Active Directory
- Azure Communication Services
- Azure Event Grid
- Azure Portal
- Community
- Dynamics 365
- Event Driven Architecture
- Event Subscription
- Identity Management
- Microsoft Entra ID
- Microsoft Graph
- PowerShell
- Role Based Access Control
- Security
- Service Principal
- Webhook Security
section_names:
- azure
- security
---
ani_ms_emea provides a comprehensive guide to fixing Azure Event Grid webhook authentication issues for Azure Communication Services and Dynamics 365 through proper Microsoft Entra ID configuration.<!--excerpt_end-->

# How to Fix Azure Event Grid Entra Authentication issue for ACS and Dynamics 365 integrated Webhooks

## Introduction

Azure Event Grid enables event-driven architectures in Azure by routing events to various endpoints, including webhooks. Securing webhook delivery is vital, and Azure supports this with Microsoft Entra ID (previously Azure AD) authentication via the `AzureEventGridSecureWebhookSubscriber` app role.

## Problem Statement

When integrating Azure Communication Services (ACS) with Dynamics 365 Contact Center using Entra ID-authenticated Event Grid webhooks, you may encounter the following error during event subscription deployment:

> **HTTP POST request failed with unknown error code**

This usually happens when the necessary role assignments are missing or misconfigured.

## Prerequisite Verification

- Ensure you have the **Owner** role on the app to create event subscriptions.
- Review Microsoft’s [official guidance](https://learn.microsoft.com/en-us/dynamics365/customer-service/administer/voice-channel-configure-services).

## Root Cause

The AzureEventGridSecureWebhookSubscriber app role has not been correctly created and/or assigned to required service principals and users. This prevents authenticated Event Grid delivery.

## Role Explanation

**AzureEventGridSecureWebhookSubscriber**

- An Azure Entra application role.
- Enables webhook authentication and authorization.
- Must be assigned to:
  - The Microsoft Event Grid service principal.
  - The user (or app) creating the subscription.

## How It Works

1. **App Role Creation:** Add the app role to your webhook application’s Azure Entra registration.
2. **Role Assignment:** Assign the role to Event Grid’s service principal and to subscription creators.
3. **Token Validation:** Event Grid adds a role claim when sending events.
4. **Authorization:** The webhook validates the Entra token and checks for the role.

## Key Participants

- **Webhook Application:** Receives and validates events.
- **Microsoft Event Grid Service Principal:** Delivers events to webhooks (App ID: `4962773b-9cdb-44cf-a8bf-237846a00ab7`).
- **Event Subscription Creator:** Needs the app role to create subscriptions.

## Solution Steps

### 1. Verify/Create Microsoft.EventGrid Service Principal

- Go to Azure Portal → Microsoft Entra ID → Enterprise Applications
- Filter by Application Type: `Microsoft Applications`
- Search for `Microsoft.EventGrid`
- Check for App ID: `4962773b-9cdb-44cf-a8bf-237846a00ab7`
- If missing, contact your Azure administrator.

### 2. Create the App Role

- Go to your Webhook App Registration → App roles → + Create app role
- Display name: `AzureEventGridSecureWebhookSubscriber`
- Allowed member types: Both
- Value: `AzureEventGridSecureWebhookSubscriber`
- Description: Azure Event Grid Role
- Enable the role

### 3. Assign Your User

- Go to Enterprise Applications → Your webhook app → Users and groups → + Add user/group
- Select yourself
- Assign the AzureEventGridSecureWebhookSubscriber role

### 4. Assign Event Grid Service Principal (PowerShell Required)

```powershell
Connect-MgGraph -Scopes "AppRoleAssignment.ReadWrite.All"
$webhookAppId = "YOUR-WEBHOOK-APP-ID-HERE"
$webhookSP = Get-MgServicePrincipal -Filter "appId eq '$webhookAppId'"
$eventGridSP = Get-MgServicePrincipal -Filter "appId eq '4962773b-9cdb-44cf-a8bf-237846a00ab7'"
$appRole = $webhookSP.AppRoles | Where-Object {---
layout: "post"
title: "Resolving Azure Event Grid Entra Authentication Issues for ACS and Dynamics 365 Webhooks"
description: "This detailed guide walks through diagnosing and resolving Azure Event Grid authentication failures when integrating Azure Communication Services and Dynamics 365 Contact Center using Microsoft Entra ID. It provides step-by-step instructions to correctly configure the AzureEventGridSecureWebhookSubscriber role, complete with PowerShell scripts, portal methods, and troubleshooting steps."
author: "ani_ms_emea"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure/how-to-fix-azure-event-grid-entra-authentication-issue-for-acs/m-p/4494308#M22430"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2026-02-11 11:41:04 +00:00
permalink: "2026-02-11-Resolving-Azure-Event-Grid-Entra-Authentication-Issues-for-ACS-and-Dynamics-365-Webhooks.html"
categories: ["Azure", "Security"]
tags: ["App Role Assignment", "Azure", "Azure Active Directory", "Azure Communication Services", "Azure Event Grid", "Azure Portal", "Community", "Dynamics 365", "Event Driven Architecture", "Event Subscription", "Identity Management", "Microsoft Entra ID", "Microsoft Graph", "PowerShell", "Role Based Access Control", "Security", "Service Principal", "Webhook Security"]
tags_normalized: [["app role assignment", "azure", "azure active directory", "azure communication services", "azure event grid", "azure portal", "community", "dynamics 365", "event driven architecture", "event subscription", "identity management", "microsoft entra id", "microsoft graph", "powershell", "role based access control", "security", "service principal", "webhook security"]]
---

ani_ms_emea provides a comprehensive guide to fixing Azure Event Grid webhook authentication issues for Azure Communication Services and Dynamics 365 through proper Microsoft Entra ID configuration.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/how-to-fix-azure-event-grid-entra-authentication-issue-for-acs/m-p/4494308#M22430)
.Value -eq "AzureEventGridSecureWebhookSubscriber"}
New-MgServicePrincipalAppRoleAssignment `
  -ServicePrincipalId $eventGridSP.Id `
  -PrincipalId $eventGridSP.Id `
  -ResourceId $webhookSP.Id `
  -AppRoleId $appRole.Id
```

### 5. Verification

- **App Registration:** App roles should show `AzureEventGridSecureWebhookSubscriber`.
- **User Assignment:** Your user is listed with the new role.
- **Event Grid Assignment:** Microsoft.EventGrid is listed with the role.

## Workflow Analogy

Imagine your webhook app is a secure building. The `AzureEventGridSecureWebhookSubscriber` role acts as a GOLD badge needed by Event Grid and users/deployment tools to interact securely with your system.

- Badge created and stored in the building (app registration)
- Badge assigned to Event Grid (so it can deliver messages)
- Badge assigned to users/deployment tools (so they can create subscriptions)

## Disclaimer

Scripts are provided AS IS without warranty. Test in a non-production environment and validate against current Azure documentation.

## Conclusion

By creating and properly assigning the `AzureEventGridSecureWebhookSubscriber` app role to both the Microsoft Event Grid service principal and the required users or applications, you will resolve the authentication error with Event Grid webhooks for ACS and Dynamics 365 integrations.

---
*For more detailed guidance, refer to Microsoft Learn or consult your Azure administrator as needed.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/how-to-fix-azure-event-grid-entra-authentication-issue-for-acs/m-p/4494308#M22430)
