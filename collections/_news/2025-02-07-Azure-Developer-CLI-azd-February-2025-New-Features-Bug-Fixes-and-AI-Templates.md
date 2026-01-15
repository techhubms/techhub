---
layout: post
title: 'Azure Developer CLI (azd) – February 2025: New Features, Bug Fixes, and AI Templates'
author: Kristen Womack
canonical_url: https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-february-2025/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/azure-sdk/feed/
date: 2025-02-07 20:43:29 +00:00
permalink: /ai/news/Azure-Developer-CLI-azd-February-2025-New-Features-Bug-Fixes-and-AI-Templates
tags:
- .NET
- AI
- AI App Deployment
- AI Templates
- Azd
- Azure
- Azure Developer CLI
- Azure SDK
- Bicep
- CLI Tools
- Cloud Development
- Codespaces
- Coding
- DevOps
- Docker
- GitHub Codespaces
- Java
- JavaScript
- Key Vault
- Kubernetes
- News
- OpenAI
- Python
- Semantic Kernel
- Typescript
- VS Code
section_names:
- ai
- azure
- coding
- devops
---
Authored by Kristen Womack, this post summarizes the Azure Developer CLI (azd) February 2025 update, featuring new CLI capabilities, bug fixes, expanded Bicep and secret management, and an array of new AI-integrated application templates to accelerate cloud app development on Azure.<!--excerpt_end-->

# Azure Developer CLI (azd) – February 2025: New Features, Bug Fixes, and AI Templates

*Authored by Kristen Womack*

This post announces the February 2025 release (version 1.12.0) of the Azure Developer CLI (`azd`). Join the [February release discussion on GitHub](https://github.com/Azure/azure-dev/discussions/4772) for more insights and feedback.

## What's New in azd 1.12.0

### New Features

- **Improved Maven Project Detection:** Enhanced detection using effective POM. Thanks to contributor [Rujun Chen](https://github.com/rujche).
- **Environment Secret Management:** Introduced `azd env set-secret` command and support for using Azure Key Vault secrets in an `azd` environment. [Learn more](https://github.com/Azure/azure-dev/blob/main/cli/azd/docs/using-environment-secrets.md).
- **Authentication Enhancements:** Displays identity info post-`azd auth login`; introduces the `azd auth login --check-status` command. Thanks to [John Aziz](https://github.com/Azure/azure-dev/pull/2856).

### Bug Fixes

- **PowerShell Custom Command Format:** Enhanced support for `pwsh` in hook scripts, now including `-NoProfile` and additional arguments ([details](https://github.com/Azure/azure-dev/pull/4595), by [YTiancheng Zhang](https://github.com/Yionse)).
- **Database Addition:** Fixed issues with `azd add` for databases lacking a host ([#4692](https://github.com/Azure/azure-dev/pull/4692)).
- **Bicep Improvements:**
    - Full support for Bicep `@sealed()` decorator on user-defined types ([#4684](https://github.com/Azure/azure-dev/pull/4684)).
    - Reliable deployment using provision with `sealed()` decorators.
    - Support for nullable Bicep parameters ([#4722](https://github.com/Azure/azure-dev/pull/4722)).
    - Key Vault references in parameter files for Bicep ([#4744](https://github.com/Azure/azure-dev/pull/4744)).
    - Respect for location picker options using `@allowed` and `@metadata` decorators ([#4752](https://github.com/Azure/azure-dev/pull/4752)).

### Other Updates

- Updated Bicep CLI to v0.33.93.
- Redis Azure Verified Modules now use native secrets export.
- Default pipeline definitions now use .NET SDK without the Aspire workload.
- Azure DevOps extension now uses updated install scripts.
- Support for `bicep.v1` resource for .NET Aspire.

## New Application Templates

This release showcases several new templates accelerating AI and cloud application development, including:

- [Azure Agents Travel Assistant](https://github.com/Azure-Samples/azureai-travel-agent-python) (by Marco Aurélio Bigélli Cardoso)
- [VoiceRAG: An Application Pattern for RAG + Voice Using Azure AI Search and the GPT-4o Realtime API for Audio](https://github.com/Azure-Samples/aisearch-openai-rag-audio) (by Pamela Fox and Pablo Castro)
- [Customer Assistant](https://github.com/Azure-Samples/semantic-kernel-customer-assistant-demo-java) (by John Oliver)
- [GenAI App with Keyless Deployment | Go](https://github.com/Azure-Samples/azure-openai-keyless-go) (by Richard Park)
- [Entity Extraction with Azure OpenAI Structured Outputs](https://github.com/Azure-Samples/azure-openai-entity-extraction) (by Pamela Fox)
- [Creative Writing Assistant: Working with Agents using Prompty](https://github.com/Azure-Samples/contoso-creative-writer) (by Cassie Breviu and Seth Juarez)
- [Creative Writing Assistant: Agents using Semantic Kernel](https://github.com/Azure-Samples/aspire-semantic-kernel-creative-writer) (by Ricardo Niepel)
- [Azure AI Basic App Sample](https://github.com/Azure-Samples/azureai-basic-python) (by Dan Taylor and Pamela Fox)

*Thank you to all contributors for expanding the template catalog.*

> If you have an `azd` template to contribute, consult the [contributor guide](https://azure.github.io/awesome-azd/docs/intro).

## Community Calls & Demo Videos

If you participate in monthly community calls, note the move to the Azure Developers group community call, where azd updates and demos are given live on the [Azure Community Standup](https://www.youtube.com/live/yGgtCvXg_oE?si=-wJGWNVRmMqDTVSY).

New video resources on AI app templates using azd:

- [January Community Call](https://youtube.com/playlist?list=PLI7iePan8aH6idG01JPEZoF7ob307EH1I&si=zgsojn4JRSuEt7fK)
- [Open at Microsoft episode](https://www.youtube.com/watch?v=NNy6qPMAg60)
- [Deploy AI Apps in Seconds with AI App Template Gallery](https://www.youtube.com/watch?v=YfP2kZ2z8HE)

Additionally, Java developers can learn more about streamlined code-to-cloud deployment in [this Azure blog post](https://techcommunity.microsoft.com/blog/appsonazureblog/from-code-to-cloud-deploy-your-java-apps-to-azure-in-just-2-steps/4351147) by [Ken Tao](https://github.com/taoxu0903).

## Getting Started with Azure Developer CLI

You can get started or enhance your usage of `azd` through several methods:

- **Terminal users** (Windows, Linux, macOS): Directly via command line.
- **Visual Studio Code or GitHub Codespaces:** Install the extension from the [Marketplace](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.azure-dev) or via the extension view (`Ctrl+Shift+X` on Windows or `Cmd+Shift+X` on macOS).
- **Visual Studio:** Enable the relevant preview feature flag as [described here](https://devblogs.microsoft.com/azure-sdk/introducing-the-azure-developer-cli-a-faster-way-to-build-apps-for-the-cloud/#visual-studio).
- **Official documentation**: [Learn more about azd](https://aka.ms/azd).

For issues, questions, or suggestions:

- File an issue or join a discussion in the [Azure Developer CLI repo](https://github.com/Azure/azure-dev)
- Consult the [troubleshooting documentation](https://aka.ms/azd-troubleshoot)

---

For further updates and details, reference the [official documentation](https://aka.ms/azd) and stay tuned to the [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-february-2025/)
