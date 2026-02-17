---
layout: "post"
title: "How Azure SRE Agent Can Investigate Resources in a Private Network"
description: "This technical guide explains how to securely enable Azure SRE Agent to query Log Analytics Workspaces that are protected by Azure Monitor Private Link Scope (AMPLS) using an Azure Function proxy within a VNet, secured with Microsoft Entra ID (Easy Auth). The solution covers the necessary Azure networking components, authentication with Managed Identity, and step-by-step deployment to maintain data privacy while allowing troubleshooting by the SRE Agent."
author: "dbandaru"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-azure-sre-agent-can-investigate-resources-in-a-private/ba-p/4494911"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-17 23:01:58 +00:00
permalink: "/2026-02-17-How-Azure-SRE-Agent-Can-Investigate-Resources-in-a-Private-Network.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Application Insights", "Authentication", "Azure", "Azure CLI", "Azure Function", "Azure Monitor", "Azure SRE Agent", "Community", "DevOps", "Easy Auth", "Log Analytics Workspace", "Managed Identity", "Microsoft Entra ID", "Network Security", "Private Endpoint", "Private Link", "Python", "RBAC", "Resource Group", "Security", "Serverless", "Virtual Network", "VNet Integration"]
tags_normalized: ["application insights", "authentication", "azure", "azure cli", "azure function", "azure monitor", "azure sre agent", "community", "devops", "easy auth", "log analytics workspace", "managed identity", "microsoft entra id", "network security", "private endpoint", "private link", "python", "rbac", "resource group", "security", "serverless", "virtual network", "vnet integration"]
---

dbandaru from Microsoft details a secure architecture for enabling Azure SRE Agent to query Log Analytics Workspaces in private networks, utilizing Azure Functions, Private Link, and Entra ID authentication.<!--excerpt_end-->

# How Azure SRE Agent Can Investigate Resources in a Private Network

When organizations protect their Log Analytics Workspaces (LAW) with Azure Monitor Private Link Scope (AMPLS) and disable public queries, external access, such as from Azure SRE Agent, is blocked for security reasons. This guide provides a step-by-step architecture to enable safe and compliant querying through a VNet-integrated Azure Function acting as a proxy while keeping your data private.

## Why Public Queries Get Blocked

- **AMPLS with Private Endpoint** ensures LAW can only be accessed from trusted resources within a private network.
- `publicNetworkAccessForQuery: Disabled` blocks all queries that do not originate from a Private Endpoint.
- SRE Agent, running outside the VNet, cannot directly access LAW under these constraints.

## The Solution Architecture

**Core Elements:**

- **Azure Functions** deployed inside the workload VNet act as a query proxy.
- **Managed Identity** on the Function provides secure authentication to LAW.
- **Easy Auth (Microsoft Entra ID)** on the Function authenticates and authorizes SRE Agent requests — no secrets or function keys are needed.
- **Private Endpoint** connects the Function to LAW over the private network.

**Pattern Overview:**

- SRE Agent calls publicly exposed (but secured) Azure Function HTTPS endpoints.
- The Function executes KQL queries against LAW via Private Endpoint and returns results to the SRE Agent.
- All access is tightly controlled by RBAC and Entra ID authentication.

## Step-by-Step Deployment

### 1. Deploy and Configure Log Analytics and AMPLS

```sh
az monitor log-analytics workspace create \
  --resource-group originations-rg \
  --workspace-name originations-law \
  --location eastus
az monitor log-analytics workspace update \
  --resource-group originations-rg \
  --workspace-name originations-law \
  --set properties.publicNetworkAccessForQuery=Disabled
az monitor private-link-scope create \
  --name originations-ampls \
  --resource-group originations-rg
```

### 2. Create the Private Endpoint in the VNet

```sh
az network private-endpoint create \
  --name pe-ampls \
  --resource-group rg-workload-ampls-demo \
  --vnet-name vnet-workload-ampls-demo \
  --subnet endpoints \
  --private-connection-resource-id "/subscriptions/.../resourceGroups/rg-originations-ampls-demo/providers/Microsoft.Insights/privateLinkScopes/ampls-originations-ampls-demo" \
  --group-id azuremonitor \
  --connection-name ampls-connection
```

### 3. Deploy the Azure Function with VNet Integration

```sh
az functionapp plan create --name plan-law-query --resource-group workload-rg --location eastus --sku EP1 --is-linux true
az functionapp create --name func-law-query --resource-group workload-rg --plan plan-law-query --runtime python --runtime-version 3.11 --functions-version 4 --assign-identity '[system]'
```

### 4. Secure the Function with Easy Auth (Microsoft Entra ID)

- Set function `authLevel` to "anonymous" (as Easy Auth handles authentication).
- Enable authentication in Azure Portal: Settings > Authentication > Add identity provider > Microsoft.
- Configure to require Entra ID authentication, register SRE Agent's Managed Identity as allowed client.

### 5. SRE Agent Authentication Flow

- SRE Agent PythonTool reads environment variables for identity endpoint and header.
- Acquires Bearer token for Function App using its Managed Identity.
- Makes secure API calls to Function endpoints, which validate via Entra ID.
- Azure Function queries LAW over the private endpoint and returns results.

## Security Considerations

- **End-to-end Security**: Only authenticated, authorized calls through Easy Auth are allowed.
- **No Key Secrets**: Managed Identity replaces key management.
- **All Activity Audited**: Application Insights logs all requests.
- **NSG Rules & VNet Routing**: Ensure Function traffic is tightly controlled.

## Investigation Flow Example

1. Incident initiator asks SRE Agent to investigate VM errors.
2. SRE Agent calls the Function App endpoint.
3. Azure Function queries LAW over private endpoint and returns logs.
4. SRE Agent analyzes data and reports root cause.

## Resources

- [Sample Repository](https://github.com/BandaruDheeraj/private-law-query-sample)
- [Azure Monitor Private Link Documentation](https://docs.microsoft.com/azure/azure-monitor/logs/private-link-security)
- [Azure Functions VNet Integration](https://docs.microsoft.com/azure/azure-functions/functions-networking-options)
- [Easy Auth / Managed Identity Guidance](https://docs.microsoft.com/azure/app-service/overview-managed-identity)
- [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli)

## Key Takeaways

- AMPLS and Private Endpoints harden Log Analytics Workspace from public queries.
- Azure Functions with Managed Identity and VNet integration enable secure SRE automation.
- Easy Auth with Microsoft Entra ID provides robust authentication and eliminates the need for secrets.

## Author

This solution and guide are provided by dbandaru (Microsoft). For full deployment samples, see the linked GitHub repository.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-azure-sre-agent-can-investigate-resources-in-a-private/ba-p/4494911)
