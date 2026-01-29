---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/serverless-mcp-agent-with-langchain-js-v1-burgers-tools-and/ba-p/4463157
title: 'Serverless MCP Agent with LangChain.js v1: Full-Stack GenAI Deployment on Azure'
author: sinedied
feed_name: Microsoft Tech Community
date: 2025-10-22 07:29:14 +00:00
tags:
- Authentication
- Azure Developer CLI
- Azure Functions
- Azure Static Web Apps
- Cosmos DB
- Debugging
- GenAI
- IaC
- JavaScript
- LangChain.js
- LLM Agents
- MCP
- MCP Inspector
- Node.js
- OpenTelemetry
- Production Architecture
- Serverless
- Streaming APIs
- Tool Integration
- Web Components
- AI
- Azure
- Coding
- Community
section_names:
- ai
- azure
- coding
primary_section: ai
---
sinedied presents a step-by-step technical walkthrough of building a production-ready serverless AI agent using LangChain.js v1 and MCP, fully deployed and observable on Microsoft Azure.<!--excerpt_end-->

# Serverless MCP Agent with LangChain.js v1 — Burgers, Tools, and Traces 🍔

**Author:** sinedied

## Overview

This post introduces a complete, production-ready sample for building full-stack GenAI applications with the newly released LangChain.js v1 and the Model Context Protocol (MCP). Designed for developers working with Microsoft Azure, the sample demonstrates how to build an AI agent that interacts with real-world APIs (a burger ordering system), manages tool orchestration, provides observability, and supports serverless deployment—all optimized for extensibility.

## Key Takeaways

- Practical example: A Node.js AI agent using LangChain.js v1 and MCP tools.
- Architecture spans web app (Azure Static Web Apps), agent API (Azure Functions), MCP server, and business API (Cosmos DB backend).
- Streamed outputs with real-time reasoning and tool step visibility.
- One-command deployment (`azd up`) to Azure, suitable for real-world integration.
- Designed for forking, extending, and adapting to diverse domains and APIs.

## What You'll Learn

- The structure and features of the LangChain.js v1-based AI agent sample
- How MCP simplifies tool and API consumption for LLM agents
- Technical architecture and service layout
- Steps to deploy, run, and extend using Azure Developer CLI
- How to monitor, debug, and trace agent activity using OpenTelemetry and Azure Monitor
- How to adapt sample patterns for your own use cases (beyond burgers!)

## High-Level Architecture

| Service               | Role                                    | Technology                            |
|----------------------|------------------------------------------|----------------------------------------|
| Agent Web App        | UI, streaming output, session history    | Azure Static Web Apps, Lit Web Components |
| Agent API            | Agent orchestration, auth, history       | Azure Functions, Node.js               |
| Burger MCP Server    | MCP API exposure (tools, streaming)      | Azure Functions, Express, MCP SDK      |
| Burger API           | Business logic (burgers, toppings, orders lifecycle) | Azure Functions, Cosmos DB             |

![Architecture Diagram]

Additional infrastructure (like databases and storage) supports the overall solution.

## Sample Features

- **LangChain.js v1** delivers robust agent management with transparent streaming of reasoning and tool invocations.
- **MCP (Model Context Protocol):** open standard for exposing any business API as interoperable AI agent tools.
- **Streaming UI:** intermediate agent/tool reasoning steps are surfaced live to users.
- **Authentication:** supports sign-in via GitHub or Microsoft accounts.
- **Debug panel:** full reasoning and tool chain visibility for developers.
- **Serverless first:** designed to run on Azure with a single `azd up` command, or locally in VS Code/GitHub Codespaces.
- **OpenTelemetry tracing:** detailed agent telemetry for observability in Azure Monitor or local tools.
- **Infrastructure as code:** all Azure resources provisioned via Bicep templates.

## Quickstart

### Prerequisites

- GitHub account
- Azure account (free and student options available)
- [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd?tabs=winget-windows,brew-mac,script-linux&pivots=os-windows)

### Setup and Deployment

1. Open the [GitHub repo](https://github.com/Azure-Samples/mcp-agent-langchainjs) in Codespaces, or clone locally.
2. Install dependencies:

    ```sh
    npm install
    ```

3. Authenticate to Azure:

    ```sh
    azd auth login
    ```

4. Provision and deploy all services:

    ```sh
    azd up
    ```

5. After deployment, use `npm run env` to get service URLs.

### Running Locally

- For experimentation (in Codespaces), use `npm start` to run all services with in-memory storage.
- The MCP server runs at `http://localhost:3000/mcp`.
- Use [MCP Inspector](https://www.npmjs.com/package/@modelcontextprotocol/inspector) to test tool endpoints directly.

### Agent Web App

- Sign in with GitHub or Microsoft account.
- Access the chat interface and interact with the AI agent (e.g., "Recommend me an extra spicy burger").
- Observe streamed output, debug details, and real-time tool invocations.

## Extensibility and Beyond

- The sample is not burger-specific—swap business logic for inventory, bookings, or support tickets.
- Fork and extend using familiar frontends, tool APIs, or different domains.
- GitHub Copilot integration is supported for exploring and working with the codebase.

## Observability and Debugging

- Agent service emits tracing via OpenTelemetry.
- View detailed traces in Azure Monitor (production) or with local collectors.
- Expand the debug panel in the UI for full step-by-step agent reasoning.

## Resources

- [GitHub repo](https://github.com/Azure-Samples/mcp-agent-langchainjs)
- [LangChain.js Documentation](https://docs.langchain.com/oss/javascript/langchain/overview)
- [Model Context Protocol](https://modelcontextprotocol.io)
- [Azure Developer CLI Overview](https://learn.microsoft.com/azure/developer/azure-developer-cli/)

## Next Steps

- Extend the agent with your own business APIs via MCP.
- Experiment with streamable toolchains, new UI features, or LLM backends.
- Monitor agent performance and tracing data in Azure Monitor.
- Connect with the [Azure AI community on Discord](https://aka.ms/foundry/discord) for questions and collaboration.

## Final Notes

- Always remember to run `azd down` to clean up your Azure resources when done.
- Stay tuned for deeper-dive posts, new features, and tutorials based on this sample.

---

**Happy coding and burger ordering!**

*Version 3.0, Updated Oct 22, 2025*

*Author: sinedied*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/serverless-mcp-agent-with-langchain-js-v1-burgers-tools-and/ba-p/4463157)
