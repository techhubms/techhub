---
layout: post
title: Announcing Azure MCP Server 1.0.0 Stable Release – A New Era for Agentic Workflows
author: Sandeep Sen
canonical_url: https://devblogs.microsoft.com/azure-sdk/announcing-azure-mcp-server-stable-release/
viewing_mode: external
feed_name: Microsoft Azure SDK Blog
feed_url: https://devblogs.microsoft.com/azure-sdk/feed/
date: 2025-10-28 18:00:57 +00:00
permalink: /ai/news/Announcing-Azure-MCP-Server-100-Stable-Release-A-New-Era-for-Agentic-Workflows
tags:
- .NET AOT
- Agentic Automation
- Agents
- AI Agents
- App Configuration
- Azure AI Foundry
- Azure AI Search
- Azure Developer CLI
- Azure Functions
- Azure MCP Server
- Azure Postgres
- Azure SDK
- Cloud Workflows
- Docker
- Event Hubs
- IntelliJ
- Kusto
- Log Analytics
- MCP
- Open Source
- Resource Groups
- Service Bus
- VS
- VS Code
section_names:
- ai
- azure
- coding
- devops
---
Sandeep Sen announces Azure MCP Server 1.0, a stable and open-source release empowering developers to connect AI agents with Azure services through the Model Context Protocol. The update offers extensive integration, agentic workflow support, and robust DevOps compatibility.<!--excerpt_end-->

# Announcing Azure MCP Server 1.0.0 Stable Release – A New Era for Agentic Workflows

**Author:** Sandeep Sen

Azure MCP Server 1.0 is now officially live, representing a game-changing leap in agentic development and cloud automation. This open-source implementation of the Model Context Protocol (MCP) enables AI agents and developers to access and manage Azure services using both natural language and code, via a standard protocol for interoperability and automation.

## What is Azure MCP Server?

Azure MCP Server is an open-source platform designed to connect AI agents with Azure's broad service ecosystem. It supports a "write once" approach for automation across:

- AI services: Azure AI Foundry, Azure AI Search
- Databases: Postgres, Kusto
- Messaging: Event Hubs, Service Bus
- Compute: Azure Function Apps
- Infrastructure: Resource Groups, App Configuration, Azure Storage, Log Analytics
- CLI integrations: Azure CLI, Azure Developer CLI (azd)

Agents can query, manage, and automate cloud workflows securely, with actions triggered by both natural language and code. Common agent interfaces like GitHub Copilot Agent Mode, custom MCP clients, and frameworks such as Agent Framework are supported.

## Why This Matters

Azure MCP Server offers a universal, open protocol (MCP), enabling:

- Cross-service automation through a common interface
- Natural language interaction for querying databases, managing storage/logs, running CLI commands
- Seamless integration with existing developer IDEs and tools
- Secure, scalable, and reliable operations backed by Microsoft security standards

## 1.0 Stable Release Highlights

- **Support for 47+ Azure services** out of the box
- Native integration with popular IDEs: Visual Studio Code, Visual Studio, IntelliJ
- Available as a Docker container for CI/CD and DevOps workflows
- Over 170 tools consolidated and refined for discoverability and usability
- Flexible operational modes: `namespace`, `all`, and `single` tool modes for onboarding, debugging, or scaling
- Security-first design: User confirmations required for sensitive actions, rigorous threat modeling and Responsible AI reviews
- Optimized with .NET AOT compilation for improved startup and resource efficiency

## Ecosystem and Tooling

- IDE Extensions: VS Code, Visual Studio, IntelliJ (with ongoing platform expansion)
- Docker Image: Pull from Microsoft Container Registry (MCR) for containers and pipelines
- Marketplace Exposure: Available in IDE marketplaces and listed in platforms like Cline
- Continuous integration with DevOps workflows for agentic automation

## Getting Started

Access the ecosystem and resources:

- [GitHub Repo](https://aka.ms/azmcp)
- [Documentation](https://aka.ms/azmcp/docs)
- [VS Code Extension](https://aka.ms/azmcp/download/vscode)
- [Visual Studio Extension](https://aka.ms/azmcp/download/vs)
- [IntelliJ Plugin](https://aka.ms/azmcp/download/intellij)
- [Docker Image](https://aka.ms/azmcp/download/docker)
- [Create an Issue](https://aka.ms/azmcp/issues)
- [Demo Video](https://youtu.be/HPvXjrxOPcI)

## Security and Performance

- User confirmations for sensitive operations
- Full compliance with Microsoft SDL and Responsible AI
- .NET AOT ensures efficient container performance

## Conclusion

Azure MCP Server 1.0 is a collaborative, developer-driven platform that opens the door to agentic automation, cloud integration, and AI-powered development on Azure. Join the community, contribute feedback, and help define the next wave of intelligent cloud solutions.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/announcing-azure-mcp-server-stable-release/)
