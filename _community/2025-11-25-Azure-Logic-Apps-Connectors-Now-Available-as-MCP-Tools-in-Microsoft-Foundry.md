---
layout: "post"
title: "Azure Logic Apps Connectors Now Available as MCP Tools in Microsoft Foundry"
description: "This post announces the public preview of integrating Azure Logic Apps connectors as Model Context Protocol (MCP) tools inside Microsoft Foundry. It details how developers can leverage over 400 enterprise connectors—including SAP, Dynamics, and SQL—directly in Foundry agent workflows, eliminating boilerplate code. The guide covers step-by-step how to select, configure, and register Logic Apps connectors as tools, and explains new possibilities for enterprise integration, secure connectivity, and future roadmap enhancements."
author: "DivSwa"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/%EF%B8%8Fpublic-preview-azure-logic-apps-connectors-as-mcp-tools-in/ba-p/4473062"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-25 21:19:33 +00:00
permalink: "/2025-11-25-Azure-Logic-Apps-Connectors-Now-Available-as-MCP-Tools-in-Microsoft-Foundry.html"
categories: ["AI", "Azure", "Coding"]
tags: ["Agentic Apps", "AI", "API Integration", "Azure", "Azure Logic Apps", "Azure Portal", "Coding", "Community", "Connectors", "Dynamics 365", "Enterprise Integration", "MCP Servers", "Microsoft Foundry", "OAuth Connectors", "SAP Integration", "Secure Connectivity", "ServiceNow", "Workflow Automation", "Workflow Development"]
tags_normalized: ["agentic apps", "ai", "api integration", "azure", "azure logic apps", "azure portal", "coding", "community", "connectors", "dynamics 365", "enterprise integration", "mcp servers", "microsoft foundry", "oauth connectors", "sap integration", "secure connectivity", "servicenow", "workflow automation", "workflow development"]
---

DivSwa introduces the new capability to use Azure Logic Apps connectors as MCP tools within Microsoft Foundry, providing developers with secure, code-free ways to integrate enterprise systems in agentic apps.<!--excerpt_end-->

# Azure Logic Apps Connectors Now Available as MCP Tools in Microsoft Foundry

**Author:** DivSwa

## Introduction

At Ignite 2025, Microsoft Foundry introduced a public preview of Azure Logic Apps connectors as Model Context Protocol (MCP) tools. Developers can now provide their agents with secure, governed, and code-free access to enterprise systems via hundreds of available Logic Apps connectors.

---

## Overview

- Foundry is a unified catalog of tools, connectors, and MCP servers allowing for standardized agent creation.
- The new feature lets agents natively use Logic Apps connectors as first-class MCP tools, removing the need for manual authentication or API integration code.
- Supported connectors include a broad range: SAP, ServiceNow, Dynamics, Salesforce, SQL, GitHub, and many more.
- Both connectors and custom workflows can be exposed as MCP tools, letting agents operate over complex business processes securely.

---

## How It Works

1. **Select a Logic Apps Connector:**
   - In the Agent Tools catalog within Foundry, browse or filter for Logic Apps connectors (labeled *Custom*).
2. **Create or Select a Standard Logic App:**
   - Each connector requires a Standard Logic Apps resource as its host. Foundry facilitates either creating a new Logic App or selecting an existing one.
3. **Configure the Connector as an MCP Server:**
   - Select operations to expose as agent actions.
   - Optionally adjust parameter behavior and define whether parameters are supplied by the model or user.
   - Edit the tool description based on the connector’s OpenAPI definition for clarity and accuracy.
   - The UI clearly indicates which parameters require user input.
4. **Register the MCP Server in Foundry:**
   - Registration starts in the Azure portal and completes in the Foundry portal.
   - The new MCP tool appears in the Agent tool catalog, linked to the backing Logic Apps resource in the Connected Resources view.
   - Agents can now leverage these connectors as native tools instantly.

![Logic Apps Connectors in Microsoft Foundry](https://techcommunity.microsoft.com/blog/azure-ai-foundry-blog/build-smarter-connected-agentic-apps-with-foundry-tools/4470763)

---

## Additional Notes

- Foundry continues to support custom MCP servers. Workflows built in Logic Apps can also be registered as MCP tools using the same process ([detailed documentation](https://learn.microsoft.com/en-us/azure/logic-apps/set-up-model-context-protocol-server-standard)).

---

## Roadmap

- **Unified Experience:** While initial setup involves both the Azure and Foundry portals, an all-in-one Foundry workflow will be introduced in the future.
- **OAuth-based Connectors:** Some first-party connectors that use OAuth are not yet supported but will be enabled soon.

---

## Getting Started

- [Demo video](#) (link as referenced in the post)
- [Try the documentation](https://learn.microsoft.com/en-us/azure/logic-apps/add-agent-tools-connector-actions)

---

*Updated Nov 25, 2025 – Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/%EF%B8%8Fpublic-preview-azure-logic-apps-connectors-as-mcp-tools-in/ba-p/4473062)
