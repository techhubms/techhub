---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-february-2026/
title: 'Azure Developer CLI (azd) – February 2026 Release: JMESPath, Deployment Slots & More'
author: PuiChee (PC) Chan
primary_section: ai
feed_name: Microsoft Azure SDK Blog
date: 2026-02-25 16:00:38 +00:00
tags:
- .NET
- AI
- AI Coding Agent
- Automation
- Azd
- Azure
- Azure App Service
- Azure Developer CLI
- Azure Functions
- Azure SDK
- CI/CD
- Codespaces
- Deployment Slots
- Dev Containers
- DevOps
- Docker
- Extensions
- Java
- JavaScript
- JMESPath
- Kubernetes
- News
- Provisioning
- Python
- Remote Build
- Templates
- Typescript
- VS Code
section_names:
- ai
- azure
- dotnet
- devops
---
PuiChee (PC) Chan details the February 2026 Azure Developer CLI release, featuring JMESPath queries, App Service slot deployments, dev container extensions, new AI automation and scripting improvements, plus templates and guides for Azure developers.<!--excerpt_end-->

# Azure Developer CLI (azd) – February 2026 Release: JMESPath Queries & Deployment Slots

**Author:** PuiChee (PC) Chan  
**Published on Azure SDK Blog**

## What's New in azd (February 2026)

This release covers azd versions [1.23.3](https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.3) to [1.23.6](https://github.com/Azure/azure-dev/releases/tag/azure-dev-cli_1.23.6).

### Highlights

- **JMESPath query support:** Filter and transform JSON output directly in the CLI with the new `--query` flag, empowering automation and scripting.
- **Direct deployment to Azure App Service slots:** Deploy to App Service deployment slots without extra scripting.
- **Automatic extension installation in dev containers:** Define required azd extensions to be installed at dev container build time.
- **Enhanced provisioning controls:** New `--subscription` and `--location` flags for fine-tuned provisioning.
- **Remote build improvements:** Support for remote builds of Azure Functions on Flex Consumption plans.
- **Extension compatibility:** Extensions can require a minimum azd version, helping avoid compatibility issues.
- **AI coding agent detection:** azd now recognizes AI agent environments and eliminates hanging interactive prompts in automation scenarios.

---

## Key Improvements & Features

### Query and Output Enhancements

- **JMESPath Query Support:** Use `--query` to apply JMESPath expressions to azd outputs for fine-grained filtering and data extraction.
  - [Details](https://devblogs.microsoft.com/azure-sdk/azd-jmespath-query-support/)

### Deployment & Infrastructure

- **App Service Deployment Slots:** Deploy directly to Azure App Service slots using azd.
  - [Details](https://devblogs.microsoft.com/azure-sdk/azd-app-service-slot/)
- **Subscription & Location Flags:** Override default provisioning settings per command.
- **Remote Build for Functions Flex:** New `remoteBuild` option for Azure Functions Flex deployments avoids local build requirements.

### Extensions and Dev Containers

- **Required azd Version for Extensions:** Extensions can define a minimum azd version (via `requiredAzdVersion`).
- **Dev Container Extensions:** azd now supports specifying extensions in devcontainer configs; these install automatically when the dev container is built.
  - [Details](https://devblogs.microsoft.com/azure-sdk/azd-devcontainer-extensions/)

### AI and Automation

- **AI Coding Agent Detection:** azd detects AI agent-driven environments (for example, when used inside an AI coding workflow) and skips interactive prompts to enhance automation and scripting reliability.

## Bug Fixes

- Resolved terminal UI issues in Ghostty emulator.
- Improved handling of configuration keys and missing options.
- Addressed issues with remote build, subscription caching, provision errors, and extension conflicts.
- Enhanced environment variable handling and telemetry event bundling.

## Other Improvements

- Better error messages for provisioning, authentication, and resource creation issues (including soft-delete conflict hints).
- Refactored internal container helper code for maintainability.

## New Documentation

- [Layered provisioning patterns](https://learn.microsoft.com/azure/developer/azure-developer-cli/layered-provisioning) for shared and environment-specific infrastructure (Feb 12 release).
- [Full-stack deployment guide](https://learn.microsoft.com/azure/developer/azure-developer-cli/full-stack-deployment) covering frontend, backend, and databases (Jan 28).
- [Multi-tenant authentication guidance](https://learn.microsoft.com/azure/developer/azure-developer-cli/faq#how-do-i-authenticate-across-multiple-tenants) (Jan 29 update).

## New Community Templates

10 new templates in the [Awesome azd template gallery](https://azure.github.io/awesome-azd/) enable:

- Blazor Server + Azure Foundry deployments
- Event Hubs triggers on Azure Functions (TypeScript, Python, .NET)
- MCP server with OAuth 2.1
- AI-powered agents with Microsoft Agent Framework
- KEDA autoscaling on AKS
- Unified AI Gateway with Azure API Management
- Data/agent governance (Purview, Fabric, Copilot, Defender for AI)

## Getting Started / Additional Resources

- [Install azd](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)
- [Browse templates](https://azure.github.io/awesome-azd/)
- [AI App Templates](https://aka.ms/ai-gallery)
- [Official docs & troubleshooting](https://learn.microsoft.com/azure/developer/azure-developer-cli/)
- [GitHub repo](https://github.com/Azure/azure-dev) for issues or discussions
- [User research signup](https://aka.ms/azd-user-research-signup)

**Are you new to azd?**
azd accelerates application deployment to Azure with developer-focused commands for terminal, editor, and CI/CD workflows.

**MVP Summit (March 24):** Come chat with the team in person!

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-february-2026/)
