---
layout: "post"
title: "Azure Developer CLI (azd) December 2025: Extensions, Foundry, and Pipeline Updates"
description: "This news post covers the December 2025 release of the Azure Developer CLI (azd), highlighting major new features such as enhanced extension capabilities, Foundry rebranding updates, improvements to Azure Pipelines, bug fixes, and user experience upgrades. It also provides an overview for new users and details about the evolving azd ecosystem, including resources for installation and extention development."
author: "Kristen Womack"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-december-2025/"
viewing_mode: "external"
feed_name: "Microsoft Azure SDK Blog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-12-19 19:57:26 +00:00
permalink: "/2025-12-19-Azure-Developer-CLI-azd-December-2025-Extensions-Foundry-and-Pipeline-Updates.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: [".NET", "Agent Extensions", "AI", "Azd", "Azure", "Azure Developer CLI", "Azure Pipelines", "Azure SDK", "Bicep CLI", "CI/CD", "Codespaces", "Coding", "Command Line Tools", "Container Apps", "DevOps", "Docker", "Error Handling", "Extensions", "GitHub", "Interactive CLI", "Java", "JavaScript", "Kubernetes", "Microsoft Foundry", "News", "Python", "Static Web Apps", "Template Gallery", "Terraform", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "agent extensions", "ai", "azd", "azure", "azure developer cli", "azure pipelines", "azure sdk", "bicep cli", "cislashcd", "codespaces", "coding", "command line tools", "container apps", "devops", "docker", "error handling", "extensions", "github", "interactive cli", "java", "javascript", "kubernetes", "microsoft foundry", "news", "python", "static web apps", "template gallery", "terraform", "typescript", "vs code"]
---

Kristen Womack details the December 2025 Azure Developer CLI (azd) release, featuring extension enhancements, Foundry rebranding, Azure Pipeline improvements, bug fixes, and community highlights for developers.<!--excerpt_end-->

# Azure Developer CLI (azd) December 2025 Update

**Author:** Kristen Womack

## Overview

The December 2025 release of the Azure Developer CLI (`azd`) brings a robust set of new features and improvements. This release (versions 1.22.0–1.22.5) includes major updates to extension support, user experience enhancements, bug fixes, and the rebranding of Azure AI Foundry to Microsoft Foundry. It wraps up a year of steady advancements in developer tooling for cloud-based application development on Azure.

---

## Release Highlights

- Enhanced extensions with custom configurations and interactive mode
- Improved provisioning progress and error handling
- Significant bug fixes across Container Apps, Azure Pipelines, and deployment workflows
- Expanded Terraform and Static Web Apps support
- Microsoft Foundry naming and documentation updates

## New Features and Enhancements

### Extension Framework

- **Custom configuration properties** for both project and service levels now supported, empowering extension authors to fine-tune extension behavior.
- **Interactive mode** enables extensions to prompt users and display text-based user interfaces for richer interactions.
- Added **distributed tracing** and **structured error handling**, simplifying diagnosis and improving observability.

### User Experience

- `azd env select` now includes an interactive environment selector.
- Reduced infrastructure provisioning polling interval for quicker feedback.
- Enhanced error messaging for invalid `azure.yaml` files and improved guidance via upgraded error resolution prompts.

### Azure Configuration

- Added support for both `azure.yaml` and `azure.yml` files.
- Integrated template gallery links with `azd init` and `azd template list` for easier template discovery.
- Automatic prompts for directory creation when using `-C`/`--cwd` with nonexistent paths.

### Other Enhancements

- **Language-specific .gitignore templates** are now included in extension project scaffolding.
- Codebase and documentation updated for the new Microsoft Foundry name.

## Bug Fixes

- Extensions: Resolved installation and provisioning issues related to new features.
- Deployment: Fixed Azure Pipelines, Static Web Apps, and resource group deletion edge cases.
- Container Apps: Improved deployment and error messaging.
- Aspire: Loosened protocol validation and fixed revision-based deployments.
- Environment variables: Addressed JSON escaping issues in CI/CD syncing.
- Bicep CLI: Updated version to v0.39.26.
- Error handling and telemetry: Improved logging and diagnostics.

## Ecosystem Growth

Looking ahead, Microsoft aims to expand the extension ecosystem for azd, including new agent-style extensions like the azd AI agent for Microsoft Foundry and the GitHub Copilot coding agent. Developers are encouraged to explore the [azd extension framework](https://devblogs.microsoft.com/azure-sdk/azd-extension-framework/) to build tailored workflows or domain-specific integrations.

## Getting Started with azd

New to Azure Developer CLI? `azd` streamlines getting applications from local development to Azure with best practice workflows and integration into editors and CI/CD systems.

- **Install azd**: [Official guide](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd?tabs=winget-windows%2Cbrew-mac%2Cscript-linux&pivots=os-windows)
- **Template gallery**: [Awesome azd template gallery](https://azure.github.io/awesome-azd/) and [AI App Templates](https://aka.ms/ai-gallery)
- **Docs & troubleshooting**: [Official documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/) and [troubleshooting guide](https://learn.microsoft.com/azure/developer/azure-developer-cli/troubleshoot)
- **Community support**: [GitHub repository](https://github.com/Azure/azure-dev)

## Community Appreciation

Microsoft thanks all contributors, authors, and community members involved in 2025's ongoing azd development. Their code, templates, and insights are fundamental to the CLI's rapid evolution.

---

This post is essential reading for DevOps engineers, Azure developers, and anyone looking to automate and optimize cloud application development workflows using the latest tools from Microsoft.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-december-2025/)
