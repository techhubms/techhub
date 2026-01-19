---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-logic-apps-mcp-servers-public-preview/ba-p/4450419
title: Build Modular Agents with Logic Apps MCP Servers (Public Preview)
author: KentWeareMSFT
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-09-08 20:37:35 +00:00
tags:
- Agent Solutions
- API Integration
- Azure API Center
- Azure Logic Apps
- Cloud Integration
- Connectors
- Easy Auth
- Enterprise Integration
- MCP Server
- Microsoft Entra
- Modular Tools
- Public Preview
- Workflow Automation
section_names:
- ai
- azure
---
KentWeareMSFT announces the public preview of MCP server support in Azure Logic Apps, describing new ways for developers to build and manage modular agents with improved integration and governance.<!--excerpt_end-->

# Build Modular Agents with Logic Apps MCP Servers (Public Preview)

Azure Logic Apps (Standard) can now function as Model Context Protocol (MCP) servers, giving organizations a new way to build, manage, and reuse agent capabilities. By transforming Logic Apps connectors into modular MCP tools, developers can:

- Rapidly construct scalable, adaptable agents using reusable connectors
- Integrate data access, messaging, and orchestration as specialized capabilities
- Reduce development overhead and improve reusability
- Integrate and manage these tools within the Azure ecosystem

## Two Approaches to Creating Logic Apps MCP Servers

### 1. Register Connectors as MCP Servers Using Azure API Center

- **Streamlined experience** for building MCP servers directly from managed connectors
- Select connectors and their actions to generate MCP servers and related tools
- Automated Logic Apps workflow creation with Easy Auth authentication setup
- MCP servers are registered in the API Center enterprise catalog for discoverability and management
- Benefits both admins (central management) and developers (easy onboarding of agent solutions)

#### Getting Started

- Refer to the [product documentation](https://learn.microsoft.com/azure/logic-apps/create-mcp-server-api-center)
- Watch [demo videos](https://techcommunity.microsoft.com/blog/integrationsonazureblog/logic-apps---mcp-demos/4452175)

### 2. Enable Logic Apps as Remote MCP Servers

- Ideal for customers with existing investments or needing advanced control
- Requirements for a Logic App to act as an MCP server:
  - One or more workflows with HTTP Request triggers and HTTP Response actions
  - Meaningful schema and descriptions in triggers
  - `host.json` configured for MCP capabilities
  - App registration setup in Microsoft Entra with Easy Auth configured

#### Getting Started

- See the [product documentation](https://learn.microsoft.com/azure/logic-apps/set-up-model-context-protocol-server-standard)
- Watch [demo videos](https://techcommunity.microsoft.com/blog/integrationsonazureblog/logic-apps---mcp-demos/4452175)

## Feedback and Availability

Both approaches are available worldwide in public preview. For feedback or questions, fill out the [official form](https://aka.ms/MCP-Servers-LogicApps).

---
**Author:** KentWeareMSFT

_Last updated Sep 08, 2025 — Version 1.0_

For more information and official updates, follow the [Azure Integration Services Blog](https://techcommunity.microsoft.com/category/azure/blog/integrationsonazureblog).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-logic-apps-mcp-servers-public-preview/ba-p/4450419)
