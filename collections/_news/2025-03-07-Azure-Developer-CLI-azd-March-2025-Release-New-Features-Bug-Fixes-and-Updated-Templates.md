---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-march-2025/
title: 'Azure Developer CLI (azd) – March 2025 Release: New Features, Bug Fixes, and Updated Templates'
author: Kristen Womack
feed_name: Microsoft DevBlog
date: 2025-03-07 19:14:23 +00:00
tags:
- .NET
- Azd
- Azure Cosmos DB
- Azure Developer CLI
- Azure Event Hubs
- Azure Functions
- Azure Key Vault
- Azure Logic Apps
- Azure OpenAI
- Azure SDK
- Azure Service Bus
- Azure Storage
- Bicep
- Codespaces
- Docker
- Java
- JavaScript
- Kubernetes
- LangChain4j
- Pipelines
- PowerShell
- Python
- Quarkus
- Templates
- Typescript
- VS Code
- AI
- Azure
- DevOps
- Security
- News
section_names:
- ai
- azure
- devops
- security
- dotnet
primary_section: ai
---
Authored by Kristen Womack, this news post details the March 2025 Azure Developer CLI (azd) release, covering notable new features, bug fixes, contributions on templates, and enhanced support for AI and Azure services.<!--excerpt_end-->

# Azure Developer CLI (azd) – March 2025 Release

*Author: Kristen Womack*

The March 2025 release (version 1.13.0) of the Azure Developer CLI (azd) introduces a host of new features, expanded support for Azure resources, vital bug fixes, and fresh templates—making it easier than ever for developers to leverage the Microsoft Azure ecosystem.

For more details and discussion, visit the [March release discussion on GitHub](https://github.com/Azure/azure-dev/discussions/4902).

## What’s New in This Release?

### New Features

- **Expanded Resource Support in `azd add`:**
  - **Azure AI Services models & Azure AI Foundry resources:** Adds intelligent location filtering in Bicep based on AI model quota and usage.
  - **Azure Cosmos DB** ([#4780](https://github.com/Azure/azure-dev/pull/4780)) – Contributed by @saragluna
  - **Azure Database for MySQL** ([#4783](https://github.com/Azure/azure-dev/pull/4783)) – Contributed by @saragluna
  - **Azure Key Vault** ([#4842](https://github.com/Azure/azure-dev/pull/4842))
  - **Azure Service Bus & Azure Event Hubs** ([#4743](https://github.com/Azure/azure-dev/pull/4743))
  - **Azure Storage (Blob Service)** ([#4765](https://github.com/Azure/azure-dev/pull/4765))
- **Docker language type support:** ([#4859](https://github.com/Azure/azure-dev/pull/4859))
  - Enables containerized apps in languages without native azd support (e.g., Go).
- **Pipeline Environment Secrets:** ([#4770](https://github.com/Azure/azure-dev/pull/4770))
  - Add environment secrets support in pipeline configuration.
- **Other Notable Features:**
  - Warning when updating an existing environment key with different casing ([#4547](https://github.com/Azure/azure-dev/pull/4547)).
  - Improved error messaging for `pwsh` hooks when PowerShell 7 isn’t installed ([#4872](https://github.com/Azure/azure-dev/pull/4872)).

### Bug Fixes

- **Maven Support:** Fix error retrieving the effective POM for multi-module Maven projects ([#4806](https://github.com/Azure/azure-dev/pull/4806)).
- **Symlinks:** Correct error when packaging app code with directory symlinks ([#4773](https://github.com/Azure/azure-dev/pull/4773)).
- **YAML Parsing:** Better handling of nested structures ([#4807](https://github.com/Azure/azure-dev/pull/4807)).
- **UI Consistency:** Enhanced color consistency ([#4847](https://github.com/Azure/azure-dev/pull/4847)).
- **Aspire Platform:**
  - Fix bind mounts outside of C: drive and single file binding ([#4801](https://github.com/Azure/azure-dev/pull/4801), [#4782](https://github.com/Azure/azure-dev/pull/4782)).
  - Ensure Aspire container entrypoints are respected ([#4789](https://github.com/Azure/azure-dev/pull/4789)).
  - Restrict password generation charset to be Aspire-compatible ([#4849](https://github.com/Azure/azure-dev/pull/4849)).
  - Support CI initialization/deployment for Aspire apps with no prompt ([#4850](https://github.com/Azure/azure-dev/pull/4850)).

### Other Changes

- **Template Framework:** Switches azd templates to use AVM rather than infra/core ([#3976](https://github.com/Azure/azure-dev/pull/3976)).
- **Telemetry:** Add GitHub Copilot for Azure telemetry user agent ([#4797](https://github.com/Azure/azure-dev/pull/4797)).
- **Documentation:** Corrected contributing links ([#4784](https://github.com/Azure/azure-dev/pull/4784)).

## New Templates Available

A range of new templates further enhances developer productivity and enterprise readiness:

- [**Azure Functions – TextCompletion using OpenAI Input Binding (Java)**](https://github.com/Azure-Samples/azure-functions-completion-openai-java) by [Thiago Almeida](https://github.com/nzthiago)
- [**Agentic Voice Assistant**](https://github.com/Azure-Samples/agentic-voice-assistant): Built with Azure Container Apps, Azure OpenAI, and Azure Logic Apps by [Evgeny Minkevich](https://github.com/evmin)
- [**Azure OpenAI RAG with Java, LangChain4j, Quarkus**](https://github.com/Azure-Samples/azure-openai-rag-workshop-java) by [Sandra Ahlgrimm](https://github.com/SandraAhlgrimm)
- [**Azure Integration Services Quickstart**](https://github.com/ronaldbosma/azure-integration-services-quickstart) by [Ronald Bosma](https://github.com/ronaldbosma)

**Thank you to all template authors and contributors!**

If you’d like to contribute your own azd template to the community, see the [contributor guide](https://azure.github.io/awesome-azd/docs/intro).

## Getting Started with `azd`

- Use `azd` from your terminal on Windows, Linux, or macOS.
- Supported in Visual Studio Code and GitHub Codespaces via [the VSCode Marketplace extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.azure-dev).
- Enable preview feature in Visual Studio via [this guide](https://devblogs.microsoft.com/azure-sdk/introducing-the-azure-developer-cli-a-faster-way-to-build-apps-for-the-cloud/#visual-studio).
- Documentation: [https://aka.ms/azd](https://aka.ms/azd)
- Need help? File an issue or discuss in the [GitHub repository](https://github.com/Azure/azure-dev) or visit the [troubleshooting guide](https://aka.ms/azd-troubleshoot).

---

For more updates, visit the [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-march-2025/)
