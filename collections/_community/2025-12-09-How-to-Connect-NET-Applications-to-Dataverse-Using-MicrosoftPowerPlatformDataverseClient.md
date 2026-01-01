---
layout: "post"
title: "How to Connect .NET Applications to Dataverse Using Microsoft.PowerPlatform.Dataverse.Client"
description: "This post asks for guidance on authenticating a .NET (4.6.2) application with Microsoft Dataverse using the Microsoft.PowerPlatform.Dataverse.Client library. It specifically covers confusion around App Registration in Entra ID (formerly Azure AD), where to obtain required credentials (App ID, Client Secret), and configuring permissions both in Entra ID and Power Platform. The author requests step-by-step instructions for this integration."
author: "AjayM"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/web-development/connect-net-4-6-2-to-dataverse-using-the-dataverse-plugin/m-p/4476310#M682"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=dotnet"
date: 2025-12-09 07:18:58 +00:00
permalink: "/2025-12-09-How-to-Connect-NET-Applications-to-Dataverse-Using-MicrosoftPowerPlatformDataverseClient.html"
categories: ["Azure", "Coding"]
tags: [".NET", "App Registration", "Application User", "Authentication", "Azure", "Azure Active Directory", "Client ID", "Client Secret", "Coding", "Community", "Dataverse", "IOrganizationService", "Microsoft Entra ID", "Microsoft.PowerPlatform.Dataverse.Client", "OAuth", "Permissions", "Power Platform"]
tags_normalized: ["dotnet", "app registration", "application user", "authentication", "azure", "azure active directory", "client id", "client secret", "coding", "community", "dataverse", "iorganizationservice", "microsoft entra id", "microsoftdotpowerplatformdotdataversedotclient", "oauth", "permissions", "power platform"]
---

AjayM asks how to connect a .NET application to Microsoft Dataverse using Microsoft.PowerPlatform.Dataverse.Client, focusing on App Registration, credentials, and necessary configuration in Entra ID and Power Platform.<!--excerpt_end-->

# How to Connect .NET Applications to Dataverse Using Microsoft.PowerPlatform.Dataverse.Client

**Author:** AjayM

## Background

AjayM is developing a .NET Framework 4.6.2 application and wants to authenticate with Microsoft Dataverse using the official client libraries. The main confusion is about how to obtain the necessary App ID (Client ID) and Client Secret, and where to set up the application registration so the .NET app can access Dataverse data securely.

## Example Problem Statement

- *"Do I need to create a separate app registration in Microsoft Entra ID (Azure AD), link it to the Dataverse environment (perhaps as an application user?), and then use that app's Client ID/Secret in my .NET code? If so, could someone outline the exact steps, including permissions and Power Platform admin center setup?"*

## Approach Summary

To connect your .NET application to Dataverse using OAuth and service principals (app registration), you must:

1. **Register an App in Entra ID (Azure Active Directory)**
2. **Grant appropriate API permissions to the app registration**
3. **Configure an application user in Power Platform admin center, tied to your app registration**
4. **Use the App (Client) ID and secret in your code's connection string**

## Step-by-Step Instructions

### 1. Register an Application in Microsoft Entra ID

- In Azure Portal, go to **Microsoft Entra ID** (formerly Azure AD).
- Navigate to **App registrations > New registration**.
    - Name: Choose a descriptive name (e.g., "Dataverse .NET Integration")
    - Supported account types: Typically, "Accounts in this organizational directory only"
    - Redirect URI: Use `app://58145B91-0C36-4500-8554-080854F2AC97` if you plan to use the sample code, or leave blank for now.
- After creation, **note the Application (client) ID** and **Directory (tenant) ID**.

### 2. Create a Client Secret

- In the App Registration, go to **Certificates & secrets > New client secret**.
- Add a description and select an expiry.
- After saving, **copy the secret value** (you will NOT be able to retrieve it again).

### 3. Assign API Permissions to the App Registration

- Go to **API permissions > Add a permission > APIs my organization uses > search for "Dataverse"** (or "Dynamics CRM")
    - Select the API and add "user_impersonation" (for delegated, if acting as a user), or "Application permissions" for app-only access.
- You may need to grant admin consent for the tenant.

### 4. Add the App Registration as an Application User in Dataverse/Power Platform

- Go to **Power Platform admin center** (https://admin.powerplatform.microsoft.com/)
- Select the correct environment > **Settings** > **Users + permissions > Application users**
- Click **+ New app user**, then search for your app registration
- Assign an appropriate security role (e.g., System Customizer, or a custom role for least privilege)

### 5. Build the Connection String for Your .NET Code

- Use the format as in your example, replacing the AppId, Client Secret, and URL with your values:

  ```
  AuthType=ClientSecret;
  Url=https://YOUR_ORG.crm.dynamics.com;
  ClientId=YOUR_APP_ID;
  ClientSecret=YOUR_SECRET;
  RedirectUri=YOUR_REDIRECT_URI; // often optional
  ````

- With **Microsoft.PowerPlatform.Dataverse.Client** (latest), you may use `ServiceClient` with this string.

### 6. Example .NET Connection

```csharp
using Microsoft.PowerPlatform.Dataverse.Client;
string connectionString = @"AuthType=ClientSecret; Url=https://YOUR_ORG.crm.dynamics.com; ClientId=YOUR_APP_ID; ClientSecret=YOUR_SECRET;";
ServiceClient serviceClient = new ServiceClient(connectionString);
```

- For classic code sample with username/password, note that this is being deprecated and not recommended. Modern best practice is app registration plus secret or certificate.

## References

- [Official Microsoft Docs: Connect to Microsoft Dataverse using C#](https://learn.microsoft.com/power-apps/developer/data-platform/authenticate-oauth)
- [Power Platform: Add application users](https://learn.microsoft.com/power-platform/admin/create-users)

## Further Notes

- Permissions granted in Entra ID must match the access needed by your app in Dataverse.
- Refer to the service principal and application user concept for non-interactive/background scenarios.
- Be cautious with client secret management (never store in source code).

## Common Pitfalls

- Not assigning the application user a security role in Dataverse
- Using an incorrect permission scope in API Permissions
- Failing to copy the client secret during creation

## Conclusion

By following these steps, your .NET application can securely authenticate and interact with Dataverse programmatically using modern, best-practice authentication patterns.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/web-development/connect-net-4-6-2-to-dataverse-using-the-dataverse-plugin/m-p/4476310#M682)
