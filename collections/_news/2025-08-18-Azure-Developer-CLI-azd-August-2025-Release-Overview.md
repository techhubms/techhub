---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-august-2025/
title: Azure Developer CLI (azd) August 2025 Release Overview
author: PuiChee (PC) Chan
feed_name: Microsoft DevBlog
date: 2025-08-18 18:44:14 +00:00
tags:
- .NET
- API Management
- Azd
- Azure AI
- Azure Aspire
- Azure Developer CLI
- Azure DevOps
- Azure Monitor
- Azure SDK
- CI/CD
- Codespaces
- Container Apps
- Docker
- Environment Variables
- Java
- JavaScript
- Kubernetes
- LangChain.js
- Log Analytics
- Microsoft Entra ID
- Microsoft Sentinel
- OAuth2
- PostgreSQL
- PowerShell
- Purview
- Python
- Resource Group Deployment
- Threat Detection
- Typescript
- VS Code
section_names:
- ai
- azure
- coding
- devops
- ml
- security
---
PuiChee (PC) Chan shares the August 2025 Azure Developer CLI (azd) release summary, including important bug fixes, new documentation, improved templates for common developer scenarios, and enhanced security and AI integrations.<!--excerpt_end-->

# Azure Developer CLI (azd) August 2025 Release Overview

Author: PuiChee (PC) Chan

## Release Highlights

The August 2025 edition of Azure Developer CLI (azd) introduces releases 1.18.1 and 1.18.2. This update focuses on bug fixes, improved documentation, and new community-driven deployment templates.

### Bug Fixes and Improvements

- **PowerShell fallback support:** The azd hook engine now automatically falls back to Windows PowerShell 5.1 if PowerShell 7 is unavailable, ensuring event hook scripts operate across more environments. Scripts relying on PowerShell 7 features may still fail in 5.1.
- **Aspire project detection:** azd checks for .NET Aspire projects before running, preventing errors.
- **Enhanced Visual Studio deployment:** Nonproject.v0 items are skipped, so only valid services are published.
- **Improved error handling:** Clear suggestions now shown when resource group or Container App errors occur during deployment.
- **CI/CD automation:** Interactive login prompts are now avoided in CI/CD environments, improving automation workflows.
- **Credential management:** Invalid branch name characters are fixed for federated credential name generation.
- **Package deployment stability:** Packages remain protected when using the `--from-package` flag with `azd deploy`.
- **Linux support:** Extension installation issues have been resolved for Linux systems.
- **Containerd/Docker error messaging:** More helpful error messages for containerd-enabled Docker package failures.
- **Aspire project warnings:** Warnings now display if a legacy or limited .NET Aspire project is detected.

### Updated Documentation

- _Azure Developer CLI environment overview_: Detailed guide to environment variables, their usage, and troubleshooting.
- _Working with environments_: Guidance for creating, managing, and switching azd environments.
- _Resource Group Scoped Deployments_: Steps to enable scoped deployments for resource groups.
- _Managing environment variables_: Expanded insights on handling environment variables for multi-environment workflows.
- _Reference documents_: CLI reference improvements, with current environment and variable management commands.
- “Dev to Production” series: See how azd workflows can be implemented with Azure DevOps and Azure Pipelines.

### New Templates from the Community

- **Trainer-Demo-Deploy scenarios**: Rapid prototyping and demo templates.
- **Azure Monitor with custom logs**: Deploy Log Analytics workspaces with custom log tables and external telemetry collection.
- **API Management + ConferenceAPI (OAuth2, Entra ID)**: Secure API deployment integrating ConferenceAPI with OAuth2 and Microsoft Entra ID.
- **Microsoft Sentinel threat detection and response**: Demo template aligned with AZ-500 and AZ-104 certification paths.
- **Azure PostgreSQL & AI integration**: Step-by-step guidance for connecting, configuring, and integrating AI with PostgreSQL in Azure.
- **Purview templates for secure AI prompts**: Purview API integration in TypeScript and Python for auditing and securing AI prompts/responses:
  - Serverless AI chat with LangChain.js and Purview.
  - RAG chat app with Azure OpenAI and Azure AI Search.

### Contributor Recognition

A special thank you to petender, kareldewinter, rob-foulkrod, daveRendon, true-while, PramodKumarHK89, and the Purview P4AI Team for their valuable template contributions.

### How to Use Azure Developer CLI

- CLI available on Windows, Linux, and macOS terminals.
- Extension for Visual Studio Code and GitHub Codespaces via Marketplace or VSCode extension manager.
- Preview feature available for Visual Studio users.
- Refer to [official documentation](https://aka.ms/azd) and [troubleshooting guide](https://aka.ms/azd-troubleshoot).

### Feedback & Community

- Share your feedback and questions in the [August release discussion on GitHub](https://github.com/Azure/azure-dev/discussions/5581).
- Start discussions or file issues in the [Azure Developer CLI repository](https://github.com/Azure/azure-dev).

Enjoy the new features and improvements, try the updated templates, and let the team know your thoughts. Your feedback drives future releases.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-august-2025/)
