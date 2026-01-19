---
layout: post
title: Connecting Azure Logic Apps MCP Server to Copilot Studio Securely
author: KentWeareMSFT
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/calling-logic-apps-mcp-server-from-copilot-studio/ba-p/4456277
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-23 15:31:29 +00:00
permalink: /ai/community/Connecting-Azure-Logic-Apps-MCP-Server-to-Copilot-Studio-Securely
tags:
- API Center
- Azure Connectors
- Azure Logic Apps
- Azure Security
- Copilot Studio
- EasyAuth
- Enterprise Authentication
- Logic Apps Standard
- Managed Identity
- MCP Server
- Microsoft Entra ID
- Secure Integration
- Workflow Automation
section_names:
- ai
- azure
- security
---
KentWeareMSFT demonstrates how to set up a secure connection from Copilot Studio to a Logic Apps MCP Server, using Azure Logic Apps and API Center to unlock enterprise data for conversational AI agents.<!--excerpt_end-->

# Connecting Azure Logic Apps MCP Server to Copilot Studio Securely

In this post, KentWeareMSFT details the process for configuring a secure and efficient integration between Azure Logic Apps MCP Server and Copilot Studio. This integration is designed to unlock enterprise data for agentic conversational solutions using Microsoft's latest platform capabilities.

## Why Azure Logic Apps?

Azure Logic Apps Standard is recommended for building MCP Servers due to its:

- **Strong Security Posture:** Supports Entra ID authentication (EasyAuth), enabling enterprise-level authorization and authentication.
- **Single Tenant Offering:** Dedicated compute, storage, and networking per customer.
- **Managed Identity Integration:** Secure connections to Azure resources without requiring keys or secrets.
- **Rich Connector Library:** Over 1400 supported connectors for both cloud SaaS and high-throughput on-premises scenarios, avoiding the need for an on-premises gateway.

## Key Services Used

1. **API Center:** A utility for building and governing MCP servers with discoverability features for scaling enterprise integrations. The free tier is sufficient for basic setups.
2. **Logic Apps Standard:** Provides MCP functionality by exposing workflows and connectors as tools.
3. **Copilot Studio:** Enables conversational AI scenarios, acting as a consumer of the Logic Apps MCP Server.

## Step-by-Step Setup

1. **Build the Logic Apps MCP Server:**
   - Use API Center for a wizard-driven setup and organizational governance.
   - Note: API Center is optional; direct setup is also possible.
2. **App Registration:**
   - An Azure App Registration will be created during the process.
   - Retrieve necessary registration details and apply required changes for security and connectivity.
3. **Integrate with Copilot Studio:**
   - Register the MCP Server within Copilot Studio.
   - Configure your Copilot Studio agent to call the MCP Server endpoint securely.

## Security Highlights

- Use **Entra ID authentication (EasyAuth)** for enterprise security and SSO capabilities.
- Prefer **Managed Identities** for service-to-service authentication to minimize secrets management.
- Ensure all connections leverage single-tenant Logic Apps for maximum isolation and control.

## Useful References

- [Introducing Logic Apps MCP Servers (Public Preview)](https://techcommunity.microsoft.com/blog/integrationsonazureblog/introducing-logic-apps-mcp-servers-public-preview/4450419)
- [Azure Logic Apps Documentation](https://docs.microsoft.com/azure/logic-apps/)

---

By following this guide, organizations can enable Copilot Studio agents to access and utilize enterprise data securely via Logic Apps MCP servers, improving the power and compliance of conversational AI solutions on Azure.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/calling-logic-apps-mcp-server-from-copilot-studio/ba-p/4456277)
