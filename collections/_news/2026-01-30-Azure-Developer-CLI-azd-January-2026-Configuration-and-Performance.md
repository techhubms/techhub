---
layout: "post"
title: "Azure Developer CLI (azd) – January 2026: Configuration & Performance"
description: "An in-depth overview of the January 2026 updates to the Azure Developer CLI (azd), detailing major enhancements in configuration management, multitenant authentication, developer experience, performance improvements, and new community-contributed templates across AI, infrastructure, and DevOps scenarios."
author: "PuiChee (PC) Chan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-january-2026/"
viewing_mode: "external"
feed_name: "Microsoft Azure SDK Blog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2026-01-30 10:30:09 +00:00
permalink: "/2026-01-30-Azure-Developer-CLI-azd-January-2026-Configuration-and-Performance.html"
categories: ["AI", "Azure", "Coding", "DevOps", "Security"]
tags: [".NET", "AI", "Authentication", "Azd", "Azure", "Azure Developer CLI", "Azure SDK", "Azure Spring Apps", "Bicep", "Codespaces", "Coding", "Configuration Management", "DevOps", "Docker", "GitHub CLI", "Java", "JavaScript", "Kubernetes", "Managed Identity", "MCP", "Microsoft Agent Framework", "Multitenant", "News", "Performance", "Podman", "Python", "Security", "Templates", "Terraform", "Typescript", "VS", "VS Code"]
tags_normalized: ["dotnet", "ai", "authentication", "azd", "azure", "azure developer cli", "azure sdk", "azure spring apps", "bicep", "codespaces", "coding", "configuration management", "devops", "docker", "github cli", "java", "javascript", "kubernetes", "managed identity", "mcp", "microsoft agent framework", "multitenant", "news", "performance", "podman", "python", "security", "templates", "terraform", "typescript", "vs", "vs code"]
---

PuiChee (PC) Chan summarizes the January 2026 release of the Azure Developer CLI, highlighting critical improvements in configuration, authentication, performance, and new AI-powered and security-focused templates contributed by the community.<!--excerpt_end-->

# Azure Developer CLI (azd) – January 2026: Configuration & Performance

The January 2026 update for the Azure Developer CLI (`azd`) brings a suite of significant enhancements aimed at improving configuration management, authentication, performance, developer experience, and community template contributions.

## Release Highlights

- **Configuration Management Enhancements:**
  - New `azd config options` command for easier discovery and understanding of global/settings.
  - New environment-specific config management: `azd env config` commands allow tailored settings for dev, staging, production, etc.
  - Clean removal of environment configuration files with `azd env remove`.
- **Authentication & Security:**
  - Cross-tenant authentication for remote state on Azure Blob Storage expands multitenant support.
  - `azd auth status` command aids credential verification and troubleshooting.
- **Performance Improvements:**
  - File-based caching in `azd show` delivers up to 60x faster resource inspection.
  - Podman added as fallback container runtime when Docker is unavailable, broadening support for more development environments.
  - Auto-detection of infrastructure providers (Bicep/Terraform) for streamlined setup.
- **Developer Experience:**
  - Automated extension update checks when running extension commands, with upgrade reminders and parallel registry caching.
  - Fine-grained property-level previews for Bicep infrastructure changes.
  - Support for non-Aspire projects in Visual Studio connected services.
- **Breaking Changes:**
  - Deprecated commands (`azd login`, `azd logout`) removed—users should use the new `azd auth` subcommands.
  - Azure Spring Apps support discontinued; recommendations provided to migrate to Azure Container Apps or Azure App Service.

## Bug Fixes & Quality Improvements

- Enhanced extension configuration and installation reliability.
- Improved error messages and telemetry for better diagnostics.
- Fixes for Bicep CLI path issues, resource display, deployment state handling, and authentication edge cases.
- Updates GitHub CLI integration and resolves multiple context and workflow step errors.

## Documentation & Learning Resources

- **Proxy configuration guide** for using `azd` behind corporate proxies.
- **Device code authentication FAQ** for workflows without browser login.
- **azd publish command** documentation, separating registry publishing from deployment.

## New Community Templates

A record 23 new templates expand the ecosystem, addressing topics such as:

- Integration with Model Context Protocol (MCP), multi-agent AI systems, and secure Azure deployments
- Examples:
  - Azure MCP Server deployments for Copilot Studio and Foundry agents
  - AI chatbots and multi-agent orchestration using Microsoft Agent Framework and LangChain.js
  - Secure API Management using OAuth and Entra ID
  - Azure Functions quickstarts, Kubernetes, App Service MCP servers, Document Intelligence demos, and more

See the [Awesome azd template gallery](https://azure.github.io/awesome-azd/) and [AI App Templates](https://aka.ms/ai-gallery) for all new contributions.

## Getting Started & Community

`azd` streamlines the journey from code to cloud for Azure projects, offering best-practice commands and robust CI/CD integration. Install the latest release, explore updated docs, browse templates, or join community user research to help shape the project's future.

- [Official azd documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/)
- [Install azd](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)
- [Troubleshooting and support](https://learn.microsoft.com/azure/developer/azure-developer-cli/troubleshoot)
- [GitHub repo and discussions](https://github.com/Azure/azure-dev)
- [Community feedback & user research](https://aka.ms/azd-user-research-signup)

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-january-2026/)
