---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-mcp-server-support-for-logic-apps-agent-loop/ba-p/4470778
title: Announcing MCP Server Support for Logic Apps Agent Loop
author: KentWeareMSFT
feed_name: Microsoft Tech Community
date: 2025-11-18 18:35:43 +00:00
tags:
- Agent Loop
- Azure Logic Apps Standard
- Connector Driven Architecture
- Custom Connectors
- Enterprise Integration
- Intelligent Workflows
- Managed Connectors
- MCP
- MCP Server
- On Behalf Of Authentication
- OpenAPI
- Secure Authentication
- Tool Extensibility
- AI
- Azure
- Community
section_names:
- ai
- azure
primary_section: ai
---
KentWeareMSFT introduces MCP support for Agent Loop in Azure Logic Apps Standard, explaining how agents can securely interact with enterprise tools using managed and custom MCP connectors.<!--excerpt_end-->

# Announcing MCP Server Support for Logic Apps Agent Loop

At Ignite, Azure Logic Apps Standard’s Agent Loop received Model Context Protocol (MCP) integration. This update empowers agents to discover and call external tools through secure, standardized interfaces, benefitting both conversational and autonomous scenarios.

## Key Features

- **MCP-Based Extensibility:** Agents gain a common protocol for interacting with enterprise systems. MCP servers, integrated via Logic Apps connectors, ensure consistent and governable interactions.
- **Connector Model:**
  - **Bring Your Own MCP Connector:** Point to an external MCP server by URL and credentials to surface its tools dynamically.
    - *Example:* [Logic Apps as an MCP Server](https://learn.microsoft.com/en-us/azure/logic-apps/create-mcp-server-api-center), built dynamically using Logic Apps connectors.
  - **Managed MCP Connector:** Utilize Azure-hosted MCP servers through Logic Apps' managed connector experience (e.g., Office 365 Email, Office 365 Calendar, Salesforce, Microsoft Learn, Atlassian Jira, GitHub). These are preconfigured and ready for use.
  - **Custom MCP Connector:** Build and publish OpenAPI-based MCP connectors for private or tenant-scoped MCP servers, improving organizational reusability.

- **On-Behalf-Of Authentication:** Managed and custom MCP connectors support OBO authentication, allowing tool access based on the end user’s identity, enhancing security and permissions.

## Getting Started / Demo

A video walkthrough is available for setting up MCP servers inside Agent Loop:

- [Agent Loop Calling MCP Servers - Logic Apps Labs](https://aka.ms/agentloop/mcp)
- [Logic Apps MCP Demos](https://aka.ms/mcp-demos)

## Summary

MCP server support extends agent-driven workflows in Azure Logic Apps, offering flexible, governable, and secure enterprise tool integration via connector-driven protocols.

---

*Author: KentWeareMSFT, Azure Integration Services Blog*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-mcp-server-support-for-logic-apps-agent-loop/ba-p/4470778)
