---
layout: "post"
title: "Azure Developer CLI (azd) October 2025: Layered Provisioning, Service Dependencies, and More"
description: "Explore the major updates in Azure Developer CLI (azd) releases 1.19.0 and 1.20.0. This post highlights new features such as layered provisioning with service dependencies, the new 'azd publish' command, enhanced extension management, authentication improvements, and a wealth of community-driven app templates for Azure developers. Bug fixes, upgraded dependencies, and documentation updates are also covered, providing a comprehensive overview for anyone building or deploying apps on Azure."
author: "PuiChee (PC) Chan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-october-2025/"
viewing_mode: "external"
feed_name: "Microsoft Azure SDK Blog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-10-14 21:54:15 +00:00
permalink: "/news/2025-10-14-Azure-Developer-CLI-azd-October-2025-Layered-Provisioning-Service-Dependencies-and-More.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", "AKS", "Authentication", "Azd", "Azure", "Azure App Service", "Azure Developer CLI", "Azure SDK", "Bicep", "CI/CD", "Cloud Development", "Codespaces", "Coding", "Community Templates", "Container Apps", "DevOps", "Docker", "Extension Management", "GitHub", "IaC", "Java", "JavaScript", "Kubernetes", "Layered Provisioning", "Managed Identity", "News", "Python", "Service Dependencies", "Templates", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "aks", "authentication", "azd", "azure", "azure app service", "azure developer cli", "azure sdk", "bicep", "cislashcd", "cloud development", "codespaces", "coding", "community templates", "container apps", "devops", "docker", "extension management", "github", "iac", "java", "javascript", "kubernetes", "layered provisioning", "managed identity", "news", "python", "service dependencies", "templates", "typescript", "vs code"]
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
