---
layout: "post"
title: "Azure Developer CLI (azd) May 2025 Release: New Features, Templates, and Docs"
description: "This blog post from Kristen Womack outlines the May 2025 release (v1.15.0) of the Azure Developer CLI (azd), highlighting new features such as enhanced CI/CD support, expanded App Service deployment, new documentation, and a rich set of new starter templates across .NET, Python, Node.js, and AI-powered solutions."
author: "Kristen Womack"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-may-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-05-07 17:30:53 +00:00
permalink: "/2025-05-07-Azure-Developer-CLI-azd-May-2025-Release-New-Features-Templates-and-Docs.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: [".NET", "AI", "App Service", "Azd", "Azure", "Azure API Management", "Azure Container Apps", "Azure Developer CLI", "Azure SDK", "Azure SQL", "Blazor", "CI/CD", "Codespaces", "Coding", "DevOps", "Docker", "GitHub Actions", "GitHub Codespaces", "Java", "JavaScript", "Kubernetes", "News", "Node.js", "OpenAI", "Python", "Semantic Kernel", "Templates", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "ai", "app service", "azd", "azure", "azure api management", "azure container apps", "azure developer cli", "azure sdk", "azure sql", "blazor", "cislashcd", "codespaces", "coding", "devops", "docker", "github actions", "github codespaces", "java", "javascript", "kubernetes", "news", "nodedotjs", "openai", "python", "semantic kernel", "templates", "typescript", "vs code"]
---

In this post, Kristen Womack presents the May 2025 release of Azure Developer CLI (azd), covering new features, updated templates, and improved documentation for enhanced Azure application development.<!--excerpt_end-->

## Azure Developer CLI (azd) – May 2025 Release Overview

**Author:** Kristen Womack  
**Source:** Azure SDK Blog

The May 2025 release (v1.15.0) of the Azure Developer CLI (azd) introduces significant updates, new documentation, and a broad selection of templates for building and deploying applications on Azure.

### Key Highlights

#### New Features

- **CI/CD Compose Support (Alpha Composability Mode):**  
  Users can now generate GitHub Actions workflows for composed applications, ensuring service dependencies are correctly understood and deployed in order. This is enabled through the `azd pipeline config` command within the alpha composability mode.

- **Azure App Service Support for Node.js and Python:**  
  The `azd add` command now allows adding Node.js and Python apps to projects, offering additional flexibility alongside containerized app hosting.

#### Other Updates

- **Multi-module Project Build Support:**  
  Enhanced build system (via community contribution) now supports multi-module projects.
- **Dependency Updates:**  
  Key libraries have been updated to resolve component governance alerts.

### New Documentation

Several new Microsoft Learn resources have been published to help users maximize azd’s capabilities:

- [Quickstart: Explore and customize a template](https://learn.microsoft.com/azure/developer/azure-developer-cli/quickstart-explore-templates): Step-by-step guidance on customizing azd templates.
- [azd up workflow overview](https://learn.microsoft.com/azure/developer/azure-developer-cli/azd-up-workflow): Understand the end-to-end process of local setup and cloud deployment using `azd up`.
- [Azure Developer CLI vs. Azure CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/azure-developer-cli-vs-azure-cli): Comparison of azd and traditional Azure CLI, highlighting scenarios for each tool.

### New Templates

A wide variety of templates are now available, covering modern frameworks, AI functionality, and different language stacks:

- **AI, Data, and Conversational Apps:**  
  - Remote MCP Servers via Azure API Management ([Python](https://github.com/Azure-Samples/remote-mcp-apim-functions-python), [Node.js/TypeScript](https://github.com/Azure-Samples/remote-mcp-functions-typescript)).
  - [Multi Agents Banking Assistant with .NET and Semantic Kernel](https://github.com/dminkovski/agent-openai-banking-assistant-csharp).
  - [YouTube Summarizer with OpenAI and Blazor .NET](https://github.com/petender/azd-youtubesummarizer-openai).
  - [Vite + Lit AI Chat UI Starter](https://github.com/Azure-Samples/vite-chat-interface).
  - [Azure SQL with Azure OpenAI Blazor Chat App](https://github.com/Azure-Samples/blazor-azure-sql-vector-search).
  - [Build Your Own Copilot Solution Accelerator](https://github.com/microsoft/Build-your-own-copilot-Solution-Accelerator).
- **Backend & API Apps:**
  - [C# API App with Swagger UI](https://github.com/Build5Nines/azd-csharp-api-swagger).
  - [C# API with Swagger Template](https://github.com/powergentic/azd-mcp-csharp).
- **Data & Storage Solutions:**
  - [Neon Serverless Postgres on Azure](https://github.com/neondatabase-labs/rag-neon-postgres-openai-azure-python).
  - [.NET Aspire Azure Storage Demo](https://github.com/FBoucher/AspireAzStorage).
  - [Azure Storage Account with Blobs and File Share](https://github.com/petender/azd-storaccnt/).
  - [Azure Secure Data Solutions](https://github.com/true-while/secure-data-solutions/).
- **Container & Serverless Orchestration:**
  - [LiteLLM in Azure Container Apps with PostgreSQL](https://github.com/Build5Nines/azd-litellm).
  - [Deploy DeepSeek-R1 on Azure Container Apps](https://github.com/daverendon/azd-deepseek-r1-on-azure-container-apps).
- **Web Apps and Static Sites:**
  - [Static Website to Azure Web App (Vue.js)](https://github.com/erudinsky/azd-static-web-vuejs).
- **Agent Frameworks:**
  - [Chainlit Agent](https://github.com/zhenbzha/chainlit-agent).

Many templates are AI-integrated or leverage capabilities such as Semantic Kernel, Azure OpenAI, and copilot-like assistants. Contributors from across the community, including Julia Kasper, Paul Yuknewicz, David Minkovski, Brittneé Keller, and others, are recognized for their efforts.

### Getting Started with azd

- azd is cross-platform and can run from the terminal on Windows, Linux, or macOS.
- Integration available for [Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.azure-dev) and GitHub Codespaces via extensions.
- Visual Studio users can enable azd through a feature flag.
- Official docs, troubleshooting guides, and community discussions are available:
  - [Official docs](https://aka.ms/azd)
  - [Troubleshooting](https://aka.ms/azd-troubleshoot)
  - [azd GitHub repo](https://github.com/Azure/azure-dev)

### Contributing

Interested contributors can submit and share new templates using the [contributor guide](https://azure.github.io/awesome-azd/docs/intro).

---

For full release notes or to join the discussion, see the [May release discussion on GitHub](https://github.com/Azure/azure-dev/discussions/5160).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-may-2025/)
