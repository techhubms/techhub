---
layout: "post"
title: "Securing Your Pro-Code Custom Engine Agent for Microsoft 365 Copilot: Endpoint Protection Guide"
description: "This article by daisami examines security techniques for protecting the endpoints of a pro-code custom engine agent for Microsoft 365 Copilot, focusing on application-level security with Azure Bot Service and ASP.NET Core. It explains how to enforce JWT token validation, restrict unauthorized Entra ID tenants, and harden publicly accessible endpoints hosted on Azure App Service when integrating with Teams. Practical code samples and references to official Microsoft docs are provided."
author: "daisami"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-secure-your-pro-code-custom-engine-agent-of-microsoft-365/ba-p/4440495"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-08 17:44:53 +00:00
permalink: "/2025-08-08-Securing-Your-Pro-Code-Custom-Engine-Agent-for-Microsoft-365-Copilot-Endpoint-Protection-Guide.html"
categories: ["Azure", "Coding", "Security"]
tags: ["Application Security", "ASP.NET Core", "Authentication", "Azure", "Azure App Service", "Azure Bot Service", "Azure Firewall", "Bot Framework", "C#", "Coding", "Community", "Custom Engine Agent", "Entra ID", "JWT Token Validation", "Microsoft 365 Copilot", "Microsoft Teams", "Network Security", "NSG", "Security", "Service Principal", "Tenant Restriction"]
tags_normalized: ["application security", "aspdotnet core", "authentication", "azure", "azure app service", "azure bot service", "azure firewall", "bot framework", "csharp", "coding", "community", "custom engine agent", "entra id", "jwt token validation", "microsoft 365 copilot", "microsoft teams", "network security", "nsg", "security", "service principal", "tenant restriction"]
---

daisami presents a practical guide on securing pro-code custom engine agent endpoints for Microsoft 365 Copilot, detailing application-level controls, code samples, and Azure integration strategies.<!--excerpt_end-->

# Securing Pro-Code Custom Engine Agents for Microsoft 365 Copilot

Author: daisami

**Last updated: Aug 08, 2025, Version 5.0**

## Introduction

If you've built a custom engine agent for Microsoft 365 Copilot using pro-code approaches like C#, this guide will help you secure its endpoints—especially as your solution becomes publicly accessible via Azure App Service and integrates with Microsoft Teams through Azure Bot Service.

## Key Endpoints That Require Security

The architecture exposes three main endpoints:

1. **Teams Endpoint** (user entry via Microsoft Teams)
2. **Azure Bot Service Endpoint** (acts as a relay between Teams and bot backend)
3. **ASP.NET Core Endpoint** (your main app, likely hosted on Azure App Service in production)

Each requires different layers of protection.

### 1. Teams Endpoint Control

Control is established using Teams app management in your Microsoft 365 tenant. Admins upload your custom agent manifest and restrict access through Teams Admin Center at the user or group level. **Limitations:** You can't restrict access at the endpoint level or prevent external orgs from copying the manifest. Thus, endpoint-level network protection isn't feasible here.

### 2. Azure Bot Service Endpoint

This relay is managed by Azure and allows little configuration beyond specifying the Service Principal. The main goal is to ensure the connected backend endpoint is secured.

### 3. ASP.NET Core Endpoint (Critical!)

When exposing your custom engine agent (the bot backend), you often host it on Azure App Service, making it accessible from the public internet. **Note:** Microsoft network isolation is not supported for Teams channel bots, so you must use app-level controls (token validation, IP filtering, mTLS, etc.).

## Security Implementation: Application-Level Controls

### Enforcing JWT Token Validation in ASP.NET Core

In your `Program.cs`, ensure you register authentication and token validation services:

```csharp
builder.Services.AddBotAspNetAuthentication(builder.Configuration);
```

The `AddBotAspNetAuthentication` extension is usually defined in your project (e.g., `AspNetExtensions.cs`). This method reads issuer/audience settings from `appsettings.{env}.json` and configures JWT validation accordingly. The snippet below highlights key points:

```csharp
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidIssuers = validTokenIssuers,
        ValidAudiences = audiences,
        ValidateIssuerSigningKey = true,
        RequireSignedTokens = true,
    };
    // ...
});
```

- Ensure `aud` (audience) claim matches your Service Principal (bot registration)
- Handle different OpenID metadata URLs based on Azure/Gov cloud

**Critical Security Fixes:**

- **Reject requests with missing/empty token:** Update `OnMessageReceived` so any request without an Authorization header is explicitly failed (context.Fail("Authorization header is missing."))
- **Restrict access to authorized Entra ID tenants:** Load expected TenantId from configuration and compare at runtime. Reject requests originating from unknown or mismatched Tenant IDs.

### Example: Validating and Rejecting Unauthorized Requests

In your middleware or bot handler (`MessageActivityAsync`):

```csharp
if (activity.ChannelId != "msteams" ||
    activity.Conversation?.ConversationType?.ToLowerInvariant() != "personal" ||
    string.IsNullOrEmpty(activity.From?.AadObjectId) ||
    (!string.IsNullOrEmpty(_tenantId) && !string.Equals(activity.Conversation?.TenantId, _tenantId, StringComparison.OrdinalIgnoreCase)))
{
    _logger.LogWarning($"Unauthorized serviceUrl detected: {activity?.ServiceUrl}. Expected TenantId: {_tenantId}");
    await turnContext.SendActivityAsync("Unauthorized service URL.");
    return;
}
```

### Service Principal Permissions

Ensure your Service Principal (App Registration) includes `User.Read.All` and other necessary API permissions, or token issuance/validation will fail.

### Additional Resources

- [Azure Bot Service Teams Integration](https://learn.microsoft.com/en-us/azure/bot-service/dl-network-isolation-concept?view=azure-bot-service-4.0)
- [Network Isolation Limitations for Teams Channel](https://learn.microsoft.com/en-us/answers/questions/2263616/is-it-possible-to-integrate-azure-bot-with-teams-w)
- [Practical Bot Service Security Sample (GitHub)](https://github.com/OfficeDev/microsoft-teams-apps-company-communicator)

**Bottom line:** For Microsoft Teams integrated bots, network isolation isn't feasible; focus on hardened authentication, authorization, and robust token validation logic at the ASP.NET Core endpoint to minimize risk.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-secure-your-pro-code-custom-engine-agent-of-microsoft-365/ba-p/4440495)
