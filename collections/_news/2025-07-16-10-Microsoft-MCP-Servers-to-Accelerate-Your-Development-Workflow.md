---
external_url: https://devblogs.microsoft.com/blog/10-microsoft-mcp-servers-to-accelerate-your-development-workflow
title: 10 Microsoft MCP Servers to Accelerate Your Development Workflow
author: Jon Galloway
feed_name: Microsoft DevBlog
date: 2025-07-16 19:30:32 +00:00
tags:
- AI Integration
- Automation
- Azure AI Foundry
- Azure DevOps
- Developer Productivity
- GitHub
- MCP
- MCP Server
- Microsoft 365
- Playwright
- SQL Server
- VS
- VS Code
section_names:
- ai
- azure
- coding
- devops
---
In this detailed blog post, Jon Galloway presents ten Microsoft MCP servers that can greatly improve your developer workflow, offering actionable tips, configuration guidance, and real-world use cases for integrating AI and automation across popular Microsoft development tools.<!--excerpt_end-->

# 10 Microsoft MCP Servers to Accelerate Your Development Workflow

*Author: Jon Galloway*

## Introduction

Microsoft has been investing heavily in the Model Context Protocol (MCP), positioning it as a new open standard for enhancing developer productivity through AI-powered integrations. This article highlights ten Microsoft-built MCP servers, detailing how each one can be used to streamline workflows directly from within popular environments like Visual Studio and VS Code.

MCP servers give AI assistants real-time access to external tools and data sources, transforming traditional code generators into multifaceted productivity assistants. Whether you need to query Azure resources, automate GitHub tasks, or manage cloud environments, these servers help you accomplish more with less manual overhead.

## Why MCP Servers Matter

- **Stay in the Flow:** Run commands and queries inside your development environment, delegating chores like resource lookups or issue creation without losing focus.
- **Natural Language Commands:** Skip memorizing syntax for CLI tools—just describe your intent and let MCP + AI handle the rest.
- **Workflow Chaining:** Combine multiple MCP servers to create powerful, multi-tool workflows (e.g., create issues from database rows, automate test generation, etc.).
- **Vendor Interoperability:** MCP enables mixing Microsoft and third-party tools through a single protocol, supporting integrations beyond just Microsoft technologies.
- **Custom Extensibility:** Easily build and integrate your own MCP servers using provided SDKs, such as the C# MCP SDK.
- **Supercharge with Customizations:** Use repositories like Awesome GitHub Copilot Customizations for tailored chat modes and workflow enhancements.

## The 10 Key Microsoft MCP Servers

### 1. Microsoft Learn Docs MCP Server

- **What it does:** Provides AI assistants with semantic, real-time access to Microsoft Learn and technical documentation via `https://learn.microsoft.com/api/mcp`.
- **Why it’s useful:** Ensures AI-generated code, patterns, and answers use the most current .NET, Azure, and Microsoft 365 documentation.
- **Real-world uses:** Retrieve official CLI commands, documentation-based recommendations, and up-to-date solutions for evolving APIs.
- **Pro Tip:** Add custom system prompts to encourage assistants to use this server for Microsoft-related questions.

### 2. Azure MCP Server

- **What it does:** Offers 15+ Azure service connectors, including for resource management, databases, monitoring, containers, authentication, and more.
- **Why it’s useful:** Automates Azure operations (such as listing resources, querying logs, or deploying applications), returning code that follows latest best practices.
- **Real-world uses:** Ask for storage account listings, generate scripts, or analyze metrics—all from within your IDE.
- **Setup:** Comprehensive guides are available in the Azure MCP repo.

### 3. GitHub MCP Server

- **What it does:** Provides complete GitHub integration for repository management, Actions, pull requests, issues, security, notifications, and more.
- **Why it’s useful:** Centralizes GitHub workflows in VS Code, removing the need to switch to the browser.
- **Real-world use:** Manage CI/CD workflows, automate PR reviews, handle issue tracking, and security analysis via natural language.
- **Authentication:** Supports OAuth and personal access tokens.

### 4. Azure DevOps MCP Server

- **What it does:** Connects with Azure DevOps for work item tracking, build pipeline management, and repository operations.
- **Why it’s useful:** Consolidates team and project management directly inside development tools.
- **Real-world use:** Query sprints, create bugs, or monitor builds without context switching.

### 5. MarkItDown MCP Server

- **What it does:** Converts documents (PDF, Office, images, audio, web formats, emails, etc.) into structured Markdown.
- **Why it’s useful:** Facilitates documentation workflows, AI text ingestion, and content formatting optimized for LLMs.
- **Real-world use:** Convert slides, extract structured tables, generate documentation from presentations.
- **Advanced:** Handles OCR, audio transcription, and integrates with Azure Document Intelligence.

### 6. SQL Server MCP Server

- **What it does:** Enables conversational, AI-driven access to SQL Server databases (on-premises, Azure SQL, or Fabric).
- **Why it’s useful:** Translate natural language queries into SQL, perform operations, and receive formatted results instantly.
- **Real-world use:** Run queries, discover schema, and manage data through chat—no SQL knowledge needed.

### 7. Playwright MCP Server

- **What it does:** Powers AI-driven web interactions and test generation using Playwright.
- **Special Use:** Underpins GitHub Copilot’s Coding Agent for web browsing and black-box test creation.
- **Real-world use:** Automate UI testing, validate flows, generate test code from natural language instructions.

### 8. Dev Box MCP Server

- **What it does:** Manages Microsoft Dev Box environments through natural language queries.
- **Why it’s useful:** Simplifies creation, configuration, and monitoring of cloud-based development environments, ‑ ideal for demos, onboarding, and standardization.
- **Real-world use:** Set up environments, check statuses, or automate routine setups.

### 9. Azure AI Foundry MCP Server (Experimental)

- **What it does:** Integrates Azure AI model catalogs, deployment management, search indexing, and AI evaluation tools into development flows.
- **Why it’s useful:** Discover, deploy, and manage LLMs and knowledge bases for AI-powered applications directly from your IDE.
- **Real-world use:** Model discovery, RAG system setup, agent evaluation, and performance analysis.

### 10. Microsoft 365 Agents Toolkit MCP Server

- **What it does:** Supports building and validating AI agents/apps for Microsoft 365 and Copilot with schema checking, sample code, and troubleshooting.
- **Why it’s useful:** Reduces errors and ramps up development for new M365/Copilot agents or integrations.
- **Real-world use:** Validate manifests, find coding patterns, solve Teams integrations.

## Getting Started with MCP Servers

### Visual Studio Code

1. **Enable Agent Mode** in Copilot Chat
2. **Configure MCP Servers** in `settings.json`
3. **Start Servers** from the UI
4. **Select Tools** for your session

### Visual Studio 2022

1. **Enable Agent Mode** in Copilot Chat
2. **Create `.mcp.json`** file in your solution directory
3. **Configure Servers** with connection/auth details
4. **Approve Tools** as prompted

### Tips & Resources

- Manage your MCP servers directly from the Extensions UI in VS Code.
- Each server may require its own authentication/configuration.
- For more detail, consult:
  - [VS Code MCP Documentation](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
  - [Visual Studio MCP Documentation](https://learn.microsoft.com/visualstudio/ide/mcp-servers)

## Looking Ahead

- The MCP ecosystem is growing rapidly with contributions from Microsoft and third parties, enhancing interoperability and expanding AI capabilities in development pipelines.
- Events like [MCP Dev Days](https://aka.ms/mcpdevdays) and resource hubs (official [MCP repo](https://github.com/microsoft/mcp)) provide additional learning and community engagement opportunities.

Developers are encouraged to build custom MCP servers for integrating their own tools and services, further customizing their workflow productivity.

---

*The content above is a detailed adaptation of Jon Galloway's post, summarizing the features, benefits, and setup instructions for ten key Microsoft MCP servers aimed at developer productivity.*

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/blog/10-microsoft-mcp-servers-to-accelerate-your-development-workflow)
