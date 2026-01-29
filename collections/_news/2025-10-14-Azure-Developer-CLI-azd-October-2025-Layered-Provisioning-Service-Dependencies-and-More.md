---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-october-2025/
title: 'Azure Developer CLI (azd) October 2025: Layered Provisioning, Service Dependencies, and More'
author: PuiChee (PC) Chan
feed_name: Microsoft Azure SDK Blog
date: 2025-10-14 21:54:15 +00:00
tags:
- .NET
- AKS
- Authentication
- Azd
- Azure App Service
- Azure Developer CLI
- Azure SDK
- Bicep
- CI/CD
- Cloud Development
- Codespaces
- Community Templates
- Container Apps
- Docker
- Extension Management
- GitHub
- IaC
- Java
- JavaScript
- Kubernetes
- Layered Provisioning
- Managed Identity
- Python
- Service Dependencies
- Templates
- Typescript
- VS Code
- Azure
- Coding
- DevOps
- News
section_names:
- azure
- coding
- devops
primary_section: coding
---
PuiChee (PC) Chan details the October 2025 Azure Developer CLI (azd) releases, introducing layered provisioning, service dependencies, the new azd publish command, and significant improvements to extension management and development features.<!--excerpt_end-->

# Azure Developer CLI (azd) – October 2025 Release Notes

_**Author:** PuiChee (PC) Chan_

The October 2025 update for the Azure Developer CLI (`azd`) brings two major releases—1.19.0 and 1.20.0—packed with new features, community-powered templates, and important enhancements for Azure-centric application development and DevOps workflows.

## 🎉 What’s New

### Layered Provisioning Support (Alpha)

- **Layered provisioning:** Define infrastructure dependencies using layers in `azure.yaml` for granular control over sequential or parallel deployments.
- Provision, refresh, or clean up individual infrastructure layers using azd commands. This shift ensures you can use Bicep end-to-end as IaC, reducing reliance on custom scripts.
- [See PR #5492](https://github.com/Azure/azure-dev/pull/5492)

### Service Dependencies and Automatic Deployment Ordering

- **Service dependencies:** Use the new `uses` property in `azure.yaml` (1.20.0) to declare and orchestrate deployment order across services, automating complex dependency graphs.
- azd manages ordering and validation for you. [See PR #5856](https://github.com/Azure/azure-dev/pull/5856)

### New `azd publish` Command

- Separates container image publishing from app deployment, offering more flexible workflows.
- Supports Azure Container Registry, third-party registries, and both local/remote builds.
- Enhanced support for pre/post-publish hooks and integration with Azure Container Apps/AKS.
- `azd deploy` now supports more deployment scenarios with `--from-package` for direct image deployments.
- Customizable workflows are possible by redefining steps in `azure.yaml`.
- [See PR #5663](https://github.com/Azure/azure-dev/pull/5663)

### Extension Management Improvements

- Auto-install, beta APIs, custom service targets, and UX upgrades for system extensions.
- MCP server capability enables better AI integration scenarios.

### Advanced Development Features

- Container App Jobs for .NET Aspire, Bicep container app revisions, and custom language support extend developer toolchains.
- Optional shell attributes streamline workflow hooks across platforms.

### Authentication Enhancements

- Device-code flow improvements with enhanced claims support.
- Better guidance for service principal creation.

## 🛠️ Bug Fixes

- **Deployment logging readability**
- **CloudShell telemetry tracking**
- **Warning message clarity**
- **Extension registry streamlining**
- **Preflight error handling and Aspire improvements**

## ⚡ Dependency and Ecosystem Updates

- Go 1.25.0, Bicep CLI 0.38.33, and the latest GitHub CLI for up-to-date DevOps tooling.
- Schema improvements and clearer Redis provisioning signals.

## 📚 New Documentation

- Updated metadata documentation: [Azure Developer CLI Metadata Docs](https://learn.microsoft.com/azure/developer/azure-developer-cli/metadata)

## 🚀 New Community Templates

- **A2A Translation Service:** Azure Container Apps, Storage Queues, AI Translator
- **FastMCP with Azure OpenAI:** Minimal AI integration experience
- **Architecture Document Generator:** AI Foundry agents, MCP service validation
- **Durable Functions Fan-Out/Fan-In:** Secure, scalable sample with Managed Identity
- **Network-hardened Web App:** Alignment with AZ-500/AZ-104 certifications
- **AI Foundry Agent Service:** Deep Research using Azure AI and Bing
- **Agentic/Data Foundation Solutions:** Microsoft Fabric, Semantic Kernel, unified data architecture
- **Medallion Architecture in Fabric:** PySpark and Power BI dashboards
- **Container Migration Accelerator:** AI-driven multi-cloud container service migration

## 🙌 Contributor Acknowledgements

- Special thanks to [@Saipriya-1144](https://github.com/Saipriya-1144) for deployment log improvements.

## 🏁 Getting Started

- Azure Developer CLI (`azd`) is a cross-platform tool optimized for Azure resource provisioning and app deployment.
- Available for terminal (Windows, Linux, macOS), VS Code, GitHub Codespaces, and Visual Studio.
- Start with `azd init`, or explore a template with `azd init --template [template name]`.
- Full docs: [Azure Developer CLI Dev Center](https://aka.ms/azd). Report issues and get help on [GitHub](https://github.com/Azure/azure-dev).

Happy coding!

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-october-2025/)
