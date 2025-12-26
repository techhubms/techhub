---
layout: "post"
title: "Preview: Govern, Secure, and Observe A2A APIs with Azure API Management"
description: "This announcement introduces preview support for Agent2Agent (A2A) APIs in Azure API Management, enabling organizations to manage and govern agent APIs along with AI model APIs, Model Context Protocol (MCP) tools, and traditional APIs. The new capability extends governance, security, and observability to agentic and AI-powered systems, supporting policy management, OpenTelemetry GenAI semantic conventions, and integration with Application Insights for monitoring and debugging."
author: "budzynski"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/preview-govern-secure-and-observe-a2a-apis-with-azure-api/ba-p/4469800"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-19 19:21:35 +00:00
permalink: "/community/2025-11-19-Preview-Govern-Secure-and-Observe-A2A-APIs-with-Azure-API-Management.html"
categories: ["AI", "Azure", "Security"]
tags: ["A2A API", "Agent2Agent", "AI", "AI Agent", "API Gateway", "API Governance", "API Integration", "API Observability", "API Policy", "Application Insights", "Azure", "Azure API Management", "Community", "Enterprise Integration", "GenAI", "JSON RPC", "MCP Tools", "OpenTelemetry", "Security"]
tags_normalized: ["a2a api", "agent2agent", "ai", "ai agent", "api gateway", "api governance", "api integration", "api observability", "api policy", "application insights", "azure", "azure api management", "community", "enterprise integration", "genai", "json rpc", "mcp tools", "opentelemetry", "security"]
---

budzynski announces the preview of A2A API support in Azure API Management, giving organizations new ways to govern, secure, and monitor agent and AI APIs within a unified management plane.<!--excerpt_end-->

# Govern, Secure, and Observe A2A APIs with Azure API Management (Preview)

Azure API Management now provides preview support for Agent2Agent (A2A) APIs, letting organizations manage agent APIs, AI model APIs, Model Context Protocol (MCP) tools, and traditional APIs (REST, SOAP, GraphQL, WebSocket, gRPC) from one API management platform.

## Extending API Governance to the Agentic Ecosystem

As agentic architectures and AI-powered systems become central to enterprise solutions, it’s crucial to ensure consistent API governance, security enforcement, and observability. With A2A APIs, Azure API Management brings established API practices to the agentic world:

- Mediate JSON-RPC runtime operations with policy enforcement
- Expose and manage "agent cards" for users, clients, or agents
- Support OpenTelemetry GenAI semantic conventions, enriching traces in Application Insights (e.g., `gen_ai.agent.id`, `gen_ai.agent.name`)

## How It Works

- **Importing an A2A API:**
  - API Management mediates calls to agent backends (JSON-RPC only).
  - The agent card becomes an operation in the managed API.
  - API Management replaces the hostname with its gateway address, adapts agent card security schemes to platform authentication, and removes unsupported interfaces.

- **Telemetry & Observability:**
  - Integrated with Application Insights, traces are enriched with GenAI-compliant attributes.
  - Enables identification, monitoring, and debugging of agent/API operations together.

## Quick Start

1. Go to "APIs" in the Azure portal and pick "A2A Agent".
2. Input your agent card URL; the portal auto-populates settings if it’s accessible.
3. Set up other properties, like the API path for API Management.

_Currently, A2A support is available in v2 API Management tiers, with broader rollout planned._

## Unified API Management

A2A support allows organizations to manage agent APIs with the same governance, security policies, and observability tools as their existing APIs, supporting both classic and AI-powered services for stronger control.

[Learn more about A2A API support in Azure API Management](https://aka.ms/apimdocs/a2a-agent-api)

**Version:** 1.0
**Published:** Nov 19, 2025
**Author:** budzynski

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/preview-govern-secure-and-observe-a2a-apis-with-azure-api/ba-p/4469800)
