---
layout: "post"
title: "Azure Developer CLI (azd) July 2025 Release: New Features, Enhancements, and Community Updates"
description: "This post by Kristen Womack details the July 2025 release (versions 1.17.1 and 1.18.0) of the Azure Developer CLI (azd). It covers new features, reliability improvements, bug fixes, workflow enhancements, new documentation, and contributions from the developer community."
author: "Kristen Womack"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-july-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-07-28 20:07:42 +00:00
permalink: "/news/2025-07-28-Azure-Developer-CLI-azd-July-2025-Release-New-Features-Enhancements-and-Community-Updates.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", ".NET Aspire", "Azd", "Azure", "Azure Container Apps", "Azure Developer CLI", "Azure Functions", "Azure SDK", "Bicep", "CI/CD", "Codespaces", "Coding", "Deployment Packaging", "DevOps", "Docker", "Environment Management", "Event Driven Architecture", "Extension Framework", "GitHub Codespaces", "Java", "JavaScript", "Kubernetes", "News", "OIDC Authentication", "Project Validation", "Python", "Remote Build", "Template Gallery", "Terraform", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "dotnet aspire", "azd", "azure", "azure container apps", "azure developer cli", "azure functions", "azure sdk", "bicep", "cislashcd", "codespaces", "coding", "deployment packaging", "devops", "docker", "environment management", "event driven architecture", "extension framework", "github codespaces", "java", "javascript", "kubernetes", "news", "oidc authentication", "project validation", "python", "remote build", "template gallery", "terraform", "typescript", "vs code"]
---

Kristen Womack presents an in-depth overview of the Azure Developer CLI (azd) July 2025 release, highlighting major new features, key improvements, bug fixes, updated documentation, and community-driven templates for Azure developers.<!--excerpt_end-->

# Azure Developer CLI (azd) July 2025 Release

**Author:** Kristen Womack

The July 2025 release of the Azure Developer CLI (azd) brings a range of new features, reliability improvements, security enhancements, and community-driven updates focused on enhancing the developer experience.

## Release Overview

- **Versions released:** 1.17.1, 1.18.0
- **Main themes:** Developer experience, project validation, AI integration, authentication enhancements, secu­rity, and documentation expansion.

[Join the release discussion on GitHub](https://github.com/Azure/azure-dev/discussions/5524)

---

## New Features

### Enhanced Project Validation

- Improved project name validation prevents packaging failures caused by invalid characters, reducing deployment errors for developers. ([PR #5380](https://github.com/Azure/azure-dev/pull/5380))

### Generic OIDC Authentication Support

- Expanded authentication to include generic OIDC with any CI provider, enabling broader integration with various pipelines beyond default providers. ([PR #5397](https://github.com/Azure/azure-dev/pull/5397))

### Improved User Experience

- New confirmation prompt before setting a new environment as default avoids accidental environment switches. ([PR #4832](https://github.com/Azure/azure-dev/pull/4832))

### Enhanced Metadata Support

- Boolean and integer default values now supported in azd metadata fields, increasing configuration flexibility. ([PR #5384](https://github.com/Azure/azure-dev/pull/5384))

### Metadata Field Regression Fix

- Fixed an issue where prompting for boolean or integer fields without defaults was not handled correctly. ([PR #5386](https://github.com/Azure/azure-dev/pull/5386))

### Deployment Packaging Improvements

- Support for `.webappignore` and `.funcignore` files allows selective exclusion of files from deployment packages, optimizing deployment performance. ([PR #5383](https://github.com/Azure/azure-dev/pull/5383))

### Interactive Hooks Enhancement

- `azd hooks run` now always runs in interactive mode with improved schema validation for easier debugging and development. ([PR #5430](https://github.com/Azure/azure-dev/pull/5430))

### Static Web Apps CLI Update

- Ensures the latest static-web-apps CLI npm package is always used for current deployment features. ([PR #5203](https://github.com/Azure/azure-dev/pull/5203))

### Terraform OIDC Support

- Adds support for Terraform with OIDC authentication in pipeline configurations, improving secure infrastructure automation. ([PR #5270](https://github.com/Azure/azure-dev/pull/5270))

### Streamlined Project Initialization

- Combines “Create a minimal project” with “Scan current directory” and introduces a `--minimal` flag for faster project creation. ([PR #5280](https://github.com/Azure/azure-dev/pull/5280))

### Enhanced Environment Management

- `azd env set` accepts multiple key-value pairs for more efficient environment variable configuration. ([PR #4942](https://github.com/Azure/azure-dev/pull/4942))

### Compose CI/CD Improvements

- Can conditionally set user-only role assignments in compose scenarios, controlled by a new `AZURE_PRINCIPAL_TYPE` environment variable. ([PR #5285](https://github.com/Azure/azure-dev/pull/5285))

### Subscription Management Enhancement

- Subscriptions are now sorted in a case-insensitive manner for easier navigation. ([PR #4969](https://github.com/Azure/azure-dev/pull/4969))

---

## Bug Fixes

### Error Handling Improvements

- Bicep secure output parameter handling crash fixed ([PR #5478](https://github.com/Azure/azure-dev/pull/5478)).
- Fixed PowerShell 7 suggestion text for service-level hooks ([PR #5468](https://github.com/Azure/azure-dev/pull/5468)).
- Preflight error handling improved for deployment stack validation ([PR #5470](https://github.com/Azure/azure-dev/pull/5470)).

### Environment and Authentication Fixes

- Proper injection of environment variables for Bicep parameter evaluation ([PR #5446](https://github.com/Azure/azure-dev/pull/5446)).
- Fixed nil pointer error for Azure DevOps authentication credential checks ([PR #5459](https://github.com/Azure/azure-dev/pull/5459)).

### .NET Aspire Improvements

- Improved error handling of unrecognized expressions in manifest files ([PR #5434](https://github.com/Azure/azure-dev/pull/5434)).

---

## Other Changes

### Development Workflow

- CI workflow optimizations: obsolete runs cancelled when new commits are pushed ([PR #5471](https://github.com/Azure/azure-dev/pull/5471)).
- Improved documentation for `azd provision --no-state` ([PR #5420](https://github.com/Azure/azure-dev/pull/5420)).
- Fixed null tag handling in deployment stack operations ([PR #5372](https://github.com/Azure/azure-dev/pull/5372)).

### Infrastructure and Tooling

- Added Microsoft.Automation/automationAccounts to output resources ([PR #5378](https://github.com/Azure/azure-dev/pull/5378)).
- Updated GitHub CLI to v2.75.1 ([PR #5461](https://github.com/Azure/azure-dev/pull/5461)).
- Updated Bicep CLI to v0.36.177 ([PR #5443](https://github.com/Azure/azure-dev/pull/5443)).

---

## New Documentation

Several new articles have been published on Microsoft Learn:

- **[Azure Container Apps deployment strategies](https://learn.microsoft.com/azure/developer/azure-developer-cli/container-apps-workflows):** Blue/green deployments, rolling updates, and advanced patterns for Azure Container Apps.
- **[Remote build support](https://learn.microsoft.com/azure/developer/azure-developer-cli/remote-builds):** Use remote builds with Azure Container Registry for consistent, high-performance builds.
- **[Docker as a language in azd](https://learn.microsoft.com/azure/developer/azure-developer-cli/docker-language-support):** Use Docker for containerized applications, especially when native language support is lacking.
- **[azd Extension Framework](https://devblogs.microsoft.com/azure-sdk/azd-extension-framework/):** Extend azd with custom workflows and third-party integrations.
- **[Dev to Production workflow with azd](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-one-click/):** Guidance for end-to-end, one-command deployments.

---

## New Templates

**Community Highlight:**

- New Azure Functions quickstart template (by Paul Yuknewicz): Demonstrates event-driven integration with Azure SQL Database using SQL Triggers and Bindings in .NET.
- Suitable for data integration, audit trails, and real-time analytics scenarios.
- The template gallery now surpasses 248 templates, prompting addition of pagination support to manage the growing resource.

---

## Getting Started with azd

- Works on Windows, Linux, macOS via your preferred terminal.
- Use directly in Visual Studio Code or GitHub Codespaces with the [azd extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.azure-dev).
- Preview feature available in Visual Studio.

**Resources:**

- [Official documentation](https://aka.ms/azd)
- [Azure Developer CLI repository](https://github.com/Azure/azure-dev)
- [Troubleshooting documentation](https://aka.ms/azd-troubleshoot)

Community contributors are highlighted and thanked for their ongoing input to this rapidly evolving tool.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-july-2025/)
