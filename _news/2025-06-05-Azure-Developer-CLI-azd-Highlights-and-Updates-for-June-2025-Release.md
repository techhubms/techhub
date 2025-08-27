---
layout: "post"
title: "Azure Developer CLI (azd): Highlights and Updates for June 2025 Release"
description: "This post details the latest updates to the Azure Developer CLI, including new alpha and beta features, CI/CD improvements, breaking changes, bug fixes, fresh templates, and new documentation. The June 2025 release emphasizes extensibility, improved workflow support, and enhanced support for AI and DevOps integrations."
author: "Kristen Womack"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-june-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-06-05 20:20:50 +00:00
permalink: "/2025-06-05-Azure-Developer-CLI-azd-Highlights-and-Updates-for-June-2025-Release.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: [".NET", "AI", "AI Agents", "Aspire", "Azd", "Azure", "Azure AI Foundry", "Azure Developer CLI", "Azure DevOps", "Azure Functions", "Azure SDK", "Bicep", "CI/CD", "Codespaces", "Coding", "Cosmos DB", "DevOps", "Docker", "Extensions", "GenAI", "GitHub Actions", "Java", "JavaScript", "Kubernetes", "Managed Identities", "MCP", "News", "Pipeline Configuration", "Python", "Templates", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "ai", "ai agents", "aspire", "azd", "azure", "azure ai foundry", "azure developer cli", "azure devops", "azure functions", "azure sdk", "bicep", "cislashcd", "codespaces", "coding", "cosmos db", "devops", "docker", "extensions", "genai", "github actions", "java", "javascript", "kubernetes", "managed identities", "mcp", "news", "pipeline configuration", "python", "templates", "typescript", "vs code"]
---

In this detailed release post, Kristen Womack explores the June 2025 updates for the Azure Developer CLI (`azd`). Learn about new features, infrastructure changes, extension support, AI-powered templates, DevOps integrations, and enhanced developer experiences.<!--excerpt_end-->

## Azure Developer CLI (azd): June 2025 Release Summary

**Author:** Kristen Womack

### Overview

This post provides a comprehensive rundown of the June 2025 updates for the Azure Developer CLI (`azd`), covering releases 1.16.0, 1.16.1, and 1.17.0. The updates focus on new alpha and beta features, extension capabilities, improvements in CI/CD integration, enhanced AI support, new and updated documents, and fresh templates to boost productivity for Azure developers.

### Community Engagement & Feedback

Readers are encouraged to participate in the [June release discussion on GitHub](https://github.com/Azure/azure-dev/discussions/5269) and provide input via a [Terraform and azd survey](https://forms.office.com/r/bg6XPYLEaS), reflecting growing community interest in enhancing Terraform support within `azd`.

---

## What's New

### Main Menu Reorganization

- Updated `azd` menu offers improved feature organization.
- New sections for beta and alpha commands are introduced to facilitate early experimentation.

![Updated AZD CLI Menu](https://devblogs.microsoft.com/azure-sdk/wp-content/uploads/sites/58/2025/06/06-05-azd-menu.png)

### Extensions (Alpha Feature)

- New extension system available as an alpha feature, unlockable with `azd config set alpha.extensions on`.
- Build and integrate custom extensions tailored to specific developer workflows.
- Initial documentation:
  - [Overview of extensions](https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/overview)
  - [Quickstart for creating extensions](https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/quickstart-ai-extension)

### Compose Feature Enhancements (Beta)

- The `azd add` and composability features are promoted to beta for new projects.
- Deprecated the `alpha.compose` feature.
- The new `azd infra generate` (alias: `azd infra gen`) command replaces `azd infra synth` for infrastructure code generation, now in beta.
- Users are encouraged to adopt new command patterns as older aliases will be phased out.

### Other Notable Features

- Support for Azure App Service in resource display commands.
- Azure AI Foundry hub now utilizes identity-based storage access for increased security.
- Enhanced pipeline config features:
  - Auto-detection of necessary variables and secrets from infrastructure parameters.
  - Interactive control for GitHub Actions variables/secrets during pipeline setup.
- Support for extension namespaces (hierarchical structuring).
- Aspire project single-service deployment via vs-server integrated.
- AI Foundry 1RP setup simplifies model management under AI Services.
- Homebrew formulas updated for Linux availability.
- Autoprompt/login guard middleware enhances CLI usability.
- CI/CD configuration improvements with Managed Identities.
- Bicep parameter prompting supports default selection with `--no-prompt`.

### Breaking Changes

- When initializing from app code/minimal projects, infra folders aren't generated by default—`azd` manages infra in-memory unless code is explicitly generated.
- Environment initialization deferred to provision-time unless otherwise specified.
- Updates to AI Services models in `azd add` require a newer Azure AI Foundry SDK client library configuration for API/project endpoint usage.

### Bug Fixes

- Prevents duplicate resource entries with `azd add`.
- UX enhancements in resource previews and error messaging.
- Improvements in Bicep generation, Aspire project handling, container registry logins, and more.
- Vulnerability mitigation in Bicep CLI dependencies.

### Additional Improvements

- Enhanced `azd help` text organization.
- Refined environment name prompt messaging.
- Template initialization and project detection messaging is clarified.
- Internal registry extension deprecated in favor of developer extension.
- Dependency updates for CVE mitigation and overall code health.

---

## Documentation Updates

**New and updated Microsoft Learn documents include:**

- [Initialization workflows for azd](https://learn.microsoft.com/azure/developer/azure-developer-cli/azd-init-workflow)
- [Extensions overview for azd](https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/overview)
- [Extension quickstart](https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/quickstart-ai-extension)
- [CI/CD pipeline configuration updates](https://learn.microsoft.com/azure/developer/azure-developer-cli/configure-devops-pipeline)
- [GitHub Actions and Azure DevOps pipeline setup](https://learn.microsoft.com/azure/developer/azure-developer-cli/pipeline-github-actions)
- [Custom and advanced pipeline features](https://learn.microsoft.com/azure/developer/azure-developer-cli/pipeline-advanced-features)

---

## New Templates

Several new project templates, including those leveraging AI and advanced architecture patterns:

- **Azure VM Win2022 as ADDS Domain Controller with Bastion:** Secure connectivity, Active Directory setup; DSC VM Extensions. ([GitHub](https://github.com/petender/azd-addsvm))
- **SQL Server 2019 AlwaysOn Cluster with ADDS VM and Management VM, Bastion-protected:** Robust, highly available SQL deployments. ([GitHub](https://github.com/petender/azd-sqlao))
- **AI-Powered Code Snippet Manager with Azure Functions and MCP:** Integrates Cosmos DB vector search, AI Agents, and Copilot for discovery. ([GitHub](https://github.com/Azure-Samples/snippy))
- **Vite + Lit AI Chat UI Starter:** Ready-to-integrate chat UI for AI-powered conversations. ([GitHub](https://github.com/Azure-Samples/vite-chat-interface))
- **.NET OpenAI MCP Agent:** .NET agent app using Azure OpenAI and TypeScript-based MCP server. ([GitHub](https://github.com/Azure-Samples/openai-mcp-agent-dotnet))
- **GPT-RAG: Enterprise GenAI Accelerator:** Reference for enterprise GenAI solutions featuring security, auditability, and responsible AI. ([GitHub](https://github.com/Azure/GPT-RAG))
- New "MCP" tag helps surface Model Context Protocol-based templates.

**Thanks to contributors:** Heath Stewart, Eric Erhardt, Peter De Tender, Govind Kamtamneni, Julia Muiruri, Justin Yoo, Paulo Lacerda, Gonzalo Becerra.

---

## Workshop Highlight

A Microsoft Build workshop demonstrates how to use `azd`, GitHub Copilot for Azure, AI Search, and AI Chat, deploying to Azure Container Apps. The [tutorial is publicly available](https://github.com/microsoft/build25-LAB309).

---

## Getting Started & Support

- Use `azd` on Windows, Linux, or macOS terminals, VSCode, Codespaces, or Visual Studio.
- [Official documentation](https://aka.ms/azd).
- Report issues or discuss features via the [GitHub repo](https://github.com/Azure/azure-dev).
- Check out [troubleshooting docs](https://aka.ms/azd-troubleshoot).

---

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-june-2025/)
