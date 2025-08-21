---
layout: "post"
title: "Azure API Management Adds MCP Support in v2 SKUs and External Server Integration"
description: "This news update outlines two major enhancements to Azure API Management: public preview MCP support in v2 SKUs and the ability to govern existing MCP-compliant servers. Learn how these features enable secure, scalable connections between AI agents, tools, and APIs—all with enterprise-grade governance and integration with Microsoft Entra ID, API Center, and monitoring tools."
author: "anishta"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/new-in-azure-api-management-mcp-in-v2-skus-external-mcp/ba-p/4440294"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-05 16:37:59 +00:00
permalink: "/2025-08-05-Azure-API-Management-Adds-MCP-Support-in-v2-SKUs-and-External-Server-Integration.html"
categories: ["AI", "Azure", "Security"]
tags: ["Agent Integration", "AI", "API Center", "API Governance", "API Security", "Application Insights", "Azure", "Azure API Management", "Azure Functions", "Azure Monitor", "Credential Manager", "External MCP Server", "LangChain", "Logic Apps", "MCP", "Microsoft Entra ID", "News", "OAuth 2.1", "Policy Engine", "Rate Limiting", "REST APIs", "Security", "V2 SKUs"]
tags_normalized: ["agent integration", "ai", "api center", "api governance", "api security", "application insights", "azure", "azure api management", "azure functions", "azure monitor", "credential manager", "external mcp server", "langchain", "logic apps", "mcp", "microsoft entra id", "news", "oauth 2dot1", "policy engine", "rate limiting", "rest apis", "security", "v2 skus"]
---

anishta reports on key Azure API Management updates, including public preview MCP support in v2 SKUs and streamlined governance for external MCP-compliant servers, helping organizations connect APIs and AI agents securely.<!--excerpt_end-->

# Azure API Management: MCP in v2 SKUs and External Server Support

## Overview

Azure API Management now offers two major new capabilities to help organizations connect APIs, agents, and tools securely and at scale:

1. **MCP (Model Context Protocol) support in v2 SKUs** (now in public preview)
2. **Governance for existing MCP-compliant servers via API Management**

These updates provide a robust, scalable foundation for enterprises building agent-powered applications with strong security and governance requirements.

---

## What Is MCP?

Model Context Protocol (MCP) is an open protocol designed for AI agent ecosystems, such as GitHub Copilot, ChatGPT, and Azure OpenAI. It enables these agents to discover, invoke, and connect with APIs as tools, facilitating structured and secure workflows in real time.

## Why Use Azure API Management for MCP?

Azure API Management acts as a unified, secure control plane for managing your APIs as MCP-enabled tools. Key features include:

- **Security**: OAuth 2.1, Microsoft Entra ID, API keys, IP filtering, and rate limiting
- **Outbound Token Injection**: Credential Manager and policy-based routing
- **Monitoring**: Integration with Azure Monitor, Logs, and Application Insights
- **Discovery and Reuse**: Via Azure API Center
- **Policy Engine**: Flexible request/response transformation, caching, header manipulation, validation, and throttling

All this allows end-to-end governance for both inbound and outbound agent or tool interactions—without requiring infrastructure changes or code rewrites.

## What’s New

### 1. MCP Support in v2 SKUs

- MCP features, previously limited to classic API Management tiers, are now in public preview for v2 SKUs: Basic v2, Standard v2, and Premium v2
- **No prerequisites or manual enablement** required
- Expose any REST API as an MCP server and protect it using Microsoft Entra ID, tokens, or API keys
- Register and discover tools in Azure API Center

### 2. Expose Existing MCP-Compliant Servers

- You can now manage and secure external tools or runtimes (e.g., Logic Apps, Azure Functions, LangChain, custom backends) as MCP servers through API Management
- Apply security (OAuth, rate limiting, Credential Manager), monitor/log usage, and unify discovery alongside internal Azure tools

## What's Next

- Introducing tool-level access policies for more granular governance
- Support for MCP resources and prompt management, expanding beyond tool exposure

## Get Started

- [Expose APIs as MCP servers](https://learn.microsoft.com/en-us/azure/api-management/export-rest-mcp-server)
- [Connect external MCP servers](https://learn.microsoft.com/en-us/azure/api-management/expose-existing-mcp-server)
- [Secure access to MCP servers](https://learn.microsoft.com/en-us/azure/api-management/secure-mcp-servers)
- [Discover tools in API Center](https://learn.microsoft.com/en-us/azure/api-center/register-discover-mcp-server)

---

## Summary

Azure API Management is evolving as a central hub for securely integrating agents, tools, and APIs in both internal and external environments. With MCP support now available in v2 SKUs and enhanced governance for external servers, organizations can rapidly build and scale agent-powered workflows on enterprise-proven Microsoft infrastructure.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/new-in-azure-api-management-mcp-in-v2-skus-external-mcp/ba-p/4440294)
