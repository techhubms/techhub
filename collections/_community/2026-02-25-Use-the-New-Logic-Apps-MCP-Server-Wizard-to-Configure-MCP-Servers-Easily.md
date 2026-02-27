---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/stop-writing-plumbing-use-the-new-logic-apps-mcp-server-wizard/ba-p/4496702
title: Use the New Logic Apps MCP Server Wizard to Configure MCP Servers Easily
author: KentWeareMSFT
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-25 14:19:26 +00:00
tags:
- AI
- API Center
- API Key Authentication
- API Management
- API Security
- Automation
- Azure
- Azure Logic Apps
- Community
- Connectors
- Copilot Studio
- DevOps
- Logic Apps Standard
- MCP Server
- Microsoft Foundry
- OAuth
- VS Code Integration
- Workflow Automation
section_names:
- ai
- azure
- devops
---
KentWeareMSFT demonstrates the streamlined MCP server configuration wizard within Azure Logic Apps Standard, guiding developers through setup, workflow exposure, and secure authentication for agentic integration.<!--excerpt_end-->

# Use the New Logic Apps MCP Server Wizard to Configure MCP Servers Easily

Azure Logic Apps Standard introduces a guided wizard for configuring MCP (Multi-Channel Protocol) servers, designed to simplify the process of exposing workflows as MCP tools for AI agents. This enables robust and secure connectivity, with quick setup and a focus on agentic capabilities rather than manual protocol wiring.

## Key Capabilities

- Turn any Logic Apps Standard instance into a fully functional MCP server using an in-portal guided workflow
- Manage setup, authentication, and workflow exposure within a single management surface
- Leverage previous integrations with API Center and Microsoft Foundry tools for dynamic publishing of MCP servers

## Creating MCP Servers

Developers can:

- **Create New Workflows:** Define the MCP server name and description, select connectors (such as Salesforce), and configure workflow actions (e.g., create, update, fetch records). Adjust input fields and descriptions to increase predictability for AI agent calls. Register workflows and expose them securely as MCP tools.
- **Use Existing Workflows:** Expose already-built workflows as MCP tools if they include HTTP triggers and HTTP response actions. Add detailed descriptions and JSON schema for maximum reliability.

## Workflow and Tool Management

- Edit MCP server names, descriptions, and attached workflows
- Copy the unique MCP server URL
- Remove MCP servers (with underlying workflows preserved)
- Navigate to, customize, and enhance individual workflows with further connectors or custom code

## Authentication Options

- **API Key-Based:** Generate, download, and manage API keys for MCP server access. Previous keys remain valid until expiration or manual regeneration.
- **OAuth Authentication:** Use Azure Easy Auth by registering a Microsoft identity provider, configuring app registrations, and managing audience and issuer values. Additional checks and server authentication settings can be tuned to organization security requirements.

## Testing and Integration

- Test MCP servers with agent platforms such as VS Code, Copilot Studio, Microsoft Foundry, or directly from Logic Apps.
- Known issue: Some OAuth configurations may require reapplying Easy Auth settings via Azure CLI for seamless authentication.

## Additional Resources

- [API Center introduction](https://techcommunity.microsoft.com/blog/integrationsonazureblog/introducing-logic-apps-mcp-servers-public-preview/4450419)
- [Microsoft Foundry tools catalog](https://techcommunity.microsoft.com/blog/integrationsonazureblog/%F0%9F%8E%99%EF%B8%8Fpublic-preview-azure-logic-apps-connectors-as-mcp-tools-in-microsoft-foundry/4473062)
- Video guide available in the original blog

---

*Author: KentWeareMSFT | Published Feb 24, 2026*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/stop-writing-plumbing-use-the-new-logic-apps-mcp-server-wizard/ba-p/4496702)
