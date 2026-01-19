---
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-fabric-mcp-public-preview/
title: 'Introducing Fabric MCP (Preview): Developer-Focused AI Integration for Microsoft Fabric'
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-10-02 15:00:00 +00:00
tags:
- .NET 9
- AI Assisted Development
- API Integration
- Data Analytics
- Dev Box
- Fabric API
- KQL
- Lakehouse
- MCP
- Microsoft Fabric
- Open Source
- OpenAI
- Pipelines
- Python
- REST API
- Semantic Models
- SQL Server
- VS Code
- Workflow Automation
section_names:
- ai
- azure
- coding
- ml
---
This article by Microsoft Fabric Blog introduces the Fabric MCP (Preview), a developer-centric, open-source protocol empowering AI agents and coders with secure access to Microsoft Fabric APIs, schemas, and best practices for streamlined data analytics development.<!--excerpt_end-->

# Introducing Fabric MCP (Preview): Developer-Focused AI Integration for Microsoft Fabric

The Microsoft Fabric Blog presents the new Model Context Protocol (MCP) for Fabric, a framework purpose-built for developers seeking AI-assisted integration with the Microsoft Fabric platform. This open-source initiative offers a unified context layer combining Fabric’s public APIs, item definitions, and best-practice guidance, enabling local, secure code and item generation powered by AI agents such as GitHub Copilot.

## What Is the Fabric MCP?

The Fabric MCP is designed for local deployment, allowing AI agents to interact with APIs and schemas securely, without direct access to your environment. It serves as a universal interface—much like a USB-C port for AI—that standardizes access to Fabric’s capabilities and enforces best practices in generated code.

**Key features include:**

- **Comprehensive API catalog:** Browse and access detailed schemas, authentication requirements, parameters, and data types for all supported Microsoft Fabric workloads.
- **Rich item definitions:** JSON schemas for Lakehouses, pipelines, semantic models, notebooks, and real-time analytics workloads, detailing item structures and constraints.
- **Built-in best practices:** Guidance on pagination, error handling, and recommended patterns to standardize code quality and maintainability.
- **Local-first security:** Runs entirely on your own infrastructure, generating code offline; no connection to live Fabric environments, keeping credentials and data secure.
- **Open-source and extensible:** Fork, contribute, and expand MCP servers for other services or add your own templates and schemas.

## Why Does MCP Matter for Data Analytics?

Large language models often struggle with real-world data application when they lack context from target environments. MCP bridges this gap by acting as a standardized protocol between AI systems and external tools/datasets. This enables agents to:

- Discover API schemas and authentication requirements
- Learn relationships between services
- Generate optimized queries (e.g., KQL queries for Eventhouse or Azure Data Explorer)
- Maintain context across services for multi-platform workflows

Microsoft provides MCP servers for other services, including Dev Box and SQL Server, all following a shared lightweight architecture.

## What Fabric MCP Unlocks

- **AI-assisted item authoring:** Agents generate or update Fabric artifacts aligned with API specifications and schemas, streamlining development.
- **Intelligent integration code:** Scaffold scripts to call Fabric APIs, manage authentication, handle responses, and automate workflows.
- **Cross-platform orchestration:** Leverage open MCP standards to coordinate scenarios across Dev Box, SQL Server, and Fabric within a consistent architecture.
- **Faster onboarding:** AI agents deliver real-time context to new developers, reducing the need to memorize extensive documentation.
- **Consistent, secure architecture:** Templates ensure scalability, security, and maintainability by default.
- **Focus on innovation:** Reduces manual integration, allowing more attention to business logic and analytics.

## Getting Started with Fabric MCP

1. **Clone the repository:**

   ```
   git clone https://github.com/microsoft/mcp
   ```

2. **Build and run the Fabric MCP server (requires .NET 9):**

   ```
   cd mcp
   dotnet build servers/Fabric.Mcp.Server/src/Fabric.Mcp.Server.csproj
   dotnet run --project servers/Fabric.Mcp.Server/src/Fabric.Mcp.Server.csproj
   ```

   The server provides offline, read-only access to API definitions and item schemas; it does not perform live actions or access private tenant data.

3. **Connect your AI agent or script:**
   Use your preferred tool (e.g., GitHub Copilot in VSCode) to interact with the MCP server, generate code, or explore available workloads.

4. **Share feedback and contribute:**
   The MCP project welcomes contributions and feedback via GitHub. Suggest templates, raise issues, and join discussions to help shape the protocol’s evolution.

## Looking Ahead

Fabric MCP is in preview, with future plans to expand templates, guidance, and hosted/remote-execution patterns. Compatibility with the wider MCP ecosystem across Microsoft is a prioritized goal. Community feedback will directly influence future enhancements.

## Conclusion

The Fabric MCP lays the groundwork for AI-powered data analytics, giving both developers and AI agents the context needed to automate and streamline workflows in a secure, consistent manner. By open-sourcing the platform, Microsoft invites the community to accelerate innovation and shape the next era of AI-integrated analytics on Microsoft Fabric.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/introducing-fabric-mcp-public-preview/)
