---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/agent-loop-new-set-of-ai-features-arrive-in-public-preview/ba-p/4470764
title: Agent Loop Expands AI Automation in Azure Logic Apps with New Features
author: DivSwa
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-11-18 18:43:59 +00:00
tags:
- Agent Loop
- AI Automation
- APIM AI Gateway
- Automation
- Azure Logic Apps
- Bring Your Own Model
- Conversational Agents
- Document Level Permissions
- Enterprise Integration
- Identity Management
- MCP
- Microsoft Entra ID
- Observability
- Okta
- Preview Features
- RAG Security
- Serverless
- Teams Integration
- Tool Calling
- Workflow Designer
section_names:
- ai
- azure
- security
---
DivSwa shares an in-depth look at the latest AI-first updates for Agent Loop in Azure Logic Apps, including secure agentic automation, advanced model integration, tool extensibility, and new workflow designer enhancements.<!--excerpt_end-->

# Agent Loop Expands AI Automation in Azure Logic Apps with New Features

**Author:** DivSwa

## Overview

Agent Loop is now generally available in Logic Apps Standard, introducing robust agentic automation features for developers. This update also launches a public preview of several new AI-focused capabilities, responsible for making intelligent workflow automation scalable, secure, and more accessible within Azure, Teams, and Microsoft 365 environments.

## Key Highlights

### 1. Agentic Workflows on Consumption SKU

- Now available in Azure Logic Apps Consumption for fully serverless, pay-as-you-go deployments.
- Lets developers add conversational and autonomous AI agents to workflows without infrastructure management.
- Integrates with 1,400+ Logic Apps connectors for rapid tool calling and system integration.
- Limitations: Restricted regions, no VS Code local dev, no nested agents/MCP tools yet.

### 2. Bring Your Own Model (BYOM) Support

- Use custom AI models from Foundry, on-prem, or private cloud environments within Logic Apps Standard.
- Integrated with Azure API Management (APIM) AI Gateway, which provides authentication, API governance, monitoring, and consistent API shapes.
- Supports:
  - Direct external API model integration
  - Local/VNET private networking for sensitive data residency
- Enables safer, managed, and flexible integration of diverse models into workflows.

### 3. MCP Tool Extensibility

- Logic Apps Standard now supports Model Context Protocol (MCP), enabling agents to discover and interact with third-party tools through standardized connectors:
  - **Own MCP connector:** Connect any external MCP server using credentials and URL.
  - **Managed MCP connector:** Azure-hosted MCP connections with shared catalogs.
  - **Custom MCP connector:** OpenAPI-based connectors for organization-specific workflow tools.
- Supports on-behalf-of (OBO) authentication for context-aware tool access.

### 4. Teams and Microsoft 365 Deployment

- Deploy Logic Apps agents directly to Teams channels and chats for seamless user interactions.
- Agents can participate in stand-ups, answer internal queries, or execute business workflows—all within Teams.
- Azure Bot Service integration manages identity, tokens, and routing.
- Supports cross-platform “organization copilot” in Microsoft 365.

### 5. Secure RAG and Document-level Permissions

- Native support for document-level authorization via Azure AI Search ACLs.
- AI agents can only access documents the requesting user is authorized for, ensuring compliance and privacy in Retrieval Augmented Generation (RAG) workflows.
- Integrates principal ID and group memberships for fine-grained ACL enforcement.

### 6. Okta Identity Provider Integration

- Agent Loop now supports Okta as an identity source, alongside Microsoft Entra ID.
- Allows passing of authenticated user context, attributes, and group memberships to agents.
- Facilitates secure, personalized, and policy-aligned AI interactions without impacting existing identity architecture.

### 7. Redesigned Workflow Designer (Public Preview)

- New streamlined interface for Logic Apps developers: unified canvas, live code view, settings, and run history.
- Auto-saving Draft Mode for safe workflow editing.
- Enhanced visual documentation (sticky notes, markdown), rebuilt search, integrated monitoring, and hierarchical debugging timeline.
- Improves developer experience and workflow management across Standard and Consumption SKUs.

## How to Get Started

- [Agent Loop General Availability](https://aka.ms/agentloop/ga)
- [Agent Loop Consumption SKU](https://aka.ms/agentloop/consumption)
- [Bring Your Own Model Documentation](http://aka.ms/agentloop/byom)
- [MCP Integrations](https://aka.ms/AgentLoop-MCP)
- [Teams Deployment Guide](https://aka.ms/agentloop/teams)
- [Secure Knowledge Retrieval Blog](https://techcommunity.microsoft.com/blog/integrationsonazureblog/%F0%9F%94%90secure-ai-agent-knowledge-retrieval---introducing-security-filters-in-agent-lo/4467561)
- [Okta Integration Steps](https://aka.ms/agentloop/okta)
- [Designer Updates Blog](https://aka.ms/ladesignerv3)

## Preview Limitations

- Some features may have limited regional availability and development options in preview.
- Teams deployment is limited to conversational agents; additional M365 surfaces are in progress.
- Designer improvements are currently in public preview for Standard workflows only.

## Conclusion

Agent Loop delivers secure, flexible, and powerful AI automation for the Microsoft cloud ecosystem. With expanded agentic features, model integration, tool calling, document-level security, cross-identity support, and a redesigned developer experience, organizations can accelerate their intelligent workflow adoption within Azure and Microsoft 365.

Share your feedback and suggest prioritizations for future development.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/agent-loop-new-set-of-ai-features-arrive-in-public-preview/ba-p/4470764)
