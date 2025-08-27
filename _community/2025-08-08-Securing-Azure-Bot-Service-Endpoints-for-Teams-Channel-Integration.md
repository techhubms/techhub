---
layout: "post"
title: "Securing Azure Bot Service Endpoints for Teams Channel Integration"
description: "This in-depth article by daisami explores the security challenges and practical strategies for protecting Azure Bot Service endpoints used with Microsoft Teams. It covers endpoint types, network limitations, application-level controls, token validation, tenant restrictions, and best practices for developers deploying bots in production environments using ASP.NET Core. The discussion includes code walkthroughs and references to relevant Microsoft documentation and community resources."
author: "daisami"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-secure-azure-bot-service-endpoints-with-teams-channel/ba-p/4440495"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-08-08 01:10:31 +00:00
permalink: "/2025-08-08-Securing-Azure-Bot-Service-Endpoints-for-Teams-Channel-Integration.html"
categories: ["Azure", "Coding", "Security"]
tags: ["Application Level Security", "ASP.NET Core", "Azure", "Azure App Service", "Azure Bot Service", "Azure Firewall", "Best Practices", "Bot Framework", "C#", "Cloud Security", "Coding", "Community", "Endpoint Security", "Entra ID", "JWT Token Validation", "Microsoft 365 Copilot", "Microsoft Teams", "Multi Tenant Security", "Network Isolation", "Network Security Groups", "Security", "Service Principal", "Token Authentication"]
tags_normalized: ["application level security", "aspdotnet core", "azure", "azure app service", "azure bot service", "azure firewall", "best practices", "bot framework", "csharp", "cloud security", "coding", "community", "endpoint security", "entra id", "jwt token validation", "microsoft 365 copilot", "microsoft teams", "multi tenant security", "network isolation", "network security groups", "security", "service principal", "token authentication"]
---

daisami guides developers through securing Azure Bot Service endpoints for Microsoft Teams, focusing on application-level protections, token validation, and tenant restrictions to safeguard accessible endpoints in ASP.NET Core bots.<!--excerpt_end-->

# Securing Azure Bot Service Endpoints for Teams Channel Integration

By daisami

## Prerequisite

Before proceeding, review the foundational article on [Developing a Custom Engine Agent for Microsoft 365 Copilot Chat Using Pro-Code](https://techcommunity.microsoft.com/blog/appsonazureblog/develop-custom-engine-agent-to-microsoft-365-copilot-chat-with-pro-code/4435612), which details how to publish a Custom Engine Agent using C# or pro-code platforms.

## Overview

This article shifts the focus from development to security, addressing the critical question of how to secure your bot endpoints when integrating Azure Bot Service with the Microsoft Teams channel. Due to the architectural characteristics of Teams and Azure Bot Service, there are unique security considerations and limitations that teams must account for in production environments.

## Endpoints in Scope

You must consider THREE primary endpoints:

1. **Teams Endpoint** - Where users access the bot through the Teams app. Access is managed via Teams Admin Center, with restrictions set for users/groups. There is no endpoint-level isolation possible, and app packages can be copied by external organizations.

2. **Azure Bot Service Endpoint** - Publicly accessible bridge between Teams and your backend. Limited configuration—mainly the Service Principal. No granular access controls on this relay.

3. **ASP.NET Core Endpoint** - The actual backend, often exposed publicly (via devtunnel for development or Azure App Service in production).

   - **Network isolation is NOT available** for Teams channel bots (unlike Direct Line channel). Therefore, you must implement **application-level security controls** like token validation, IP filtering, and mutual TLS.
   - See [Azure Bot Service networking documentation](https://learn.microsoft.com/en-us/azure/bot-service/dl-network-isolation-concept?view=azure-bot-service-4.0) for limitations.

## Current Security Approaches and Limitations

- **Teams app management** can restrict user/group access inside your tenant but cannot stop external sharing or copying of app packages.
- **Azure Bot Service** enforces identity via Service Principal, but the endpoint itself is public and acts as a pass-through.
- **Backend endpoint (ASP.NET Core)** must be exposed publicly for Teams to reach it. Network-level isolation (Private Endpoints, VNET) is not supported for the Teams channel, so all security must be enforced at the application code level.

## Recommended Application-Level Security Controls

1. **JWT Token Validation**
   - Validate issuer, audience, and signature of incoming tokens.
   - Only accept tokens for your Azure Bot Service's Service Principal.
   - Validate that the required claims (audience/client and tenant IDs) match your configuration.

2. **Reject Requests without Tokens**
   - Modify authentication logic to fail requests missing authorization headers. (This is a security improvement over default behavior where missing tokens may be ignored.)
   - Example code (simplified):

     ```csharp
     if (string.IsNullOrEmpty(authorizationHeader)) {
        context.Fail("Authorization header is missing.");
        logger?.LogWarning("Authorization header is missing.");
        return;
     }
     ```

3. **Deny Access from Unknown Entra ID Tenants**
   - Compare the tenant ID from the incoming token and activity to your allowed tenant IDs.
   - Example logic:

     ```csharp
     if (!string.IsNullOrEmpty(_tenantId) && !string.Equals(activity.Conversation?.TenantId, _tenantId, StringComparison.OrdinalIgnoreCase)) {
         _logger.LogWarning("Unauthorized serviceUrl detected: {ServiceUrl}. Expected TenantId: {TenantId}", activity?.ServiceUrl, _tenantId);
         await turnContext.SendActivityAsync("Unauthorized service URL.", cancellationToken: cancellationToken);
         return;
     }
     ```

4. **Additional Protections**
   - Implement IP filtering, network security groups (NSG), and Azure Firewall where possible.
   - Always use HTTPS and consider mutual TLS if practical.

## Code Walkthrough

- The author includes code samples for ASP.NET Core `Program.cs` and `AspNetExtensions.cs` to show how to add and configure token validation for bot requests.
- Highlights how to decode and inspect JWTs to confirm required claims (audience and tenant).
- Demonstrates defense-in-depth by layering configuration-level and runtime application controls.

## Key Takeaways

- **Full network isolation is not feasible for Teams-integrated bots** due to SaaS constraints.
- **Application-level security (JWT validation, tenant filtering)** is essential.
- Service Principals must have proper API permissions (e.g., `User.Read.All`) to ensure tokens are issued and can be validated.
- Developers must ensure authentication logic is strict—do not let requests without tokens through.
- Continuous monitoring, code inspection, and reviewing latest best practices are advised.

## Further Reading and References

- [Azure Bot Service, Microsoft Teams architecture, and message flow](https://moimhossain.com/2025/05/22/azure-bot-service-microsoft-teams-architecture-and-message-flow/?utm_source=chatgpt.com)
- [Is it possible to integrate Azure Bot with Teams without public access?](https://learn.microsoft.com/en-us/answers/questions/2263616/is-it-possible-to-integrate-azure-bot-with-teams-w)
- [How to create Azure Bot Service in a private network?](https://learn.microsoft.com/en-us/answers/questions/2153606/how-to-create-azure-bot-service-in-a-private-netwo)
- [GitHub: Company Communicator Bot Filter Middleware Example](https://github.com/OfficeDev/microsoft-teams-apps-company-communicator/blob/dcf3b169084d3fff7c1e4c5b68718fb33c3391dd/Source/CompanyCommunicator/Bot/CompanyCommunicatorBotFilterMiddleware.cs#L44)

---

For feedback and discussion, connect with daisami on the Microsoft Tech Community.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-secure-azure-bot-service-endpoints-with-teams-channel/ba-p/4440495)
