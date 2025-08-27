---
layout: "post"
title: "Azure Developer CLI (azd) – April 2025 Release: New Features, Templates, and Enhancements"
description: "The April 2025 release of Azure Developer CLI (azd) introduces new features, improved AI integration, updated templates, and several bug fixes. The update offers enhanced environment setup, AI Search support, Key Vault integration, and new templates for AI and web development in the Azure ecosystem."
author: "Kristen Womack"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-april-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-04-03 18:38:43 +00:00
permalink: "/2025-04-03-Azure-Developer-CLI-azd-April-2025-Release-New-Features-Templates-and-Enhancements.html"
categories: ["AI", "Azure", "DevOps"]
tags: [".NET", ".NET Aspire", "AI", "Azd", "Azure", "Azure AI Search", "Azure Container Apps", "Azure Developer CLI", "Azure OpenAI", "Azure SDK", "Bicep", "Codespaces", "DevOps", "Docker", "GitHub Codespaces", "Java", "JavaScript", "Key Vault", "Kubernetes", "News", "Python", "Templates", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "dotnet aspire", "ai", "azd", "azure", "azure ai search", "azure container apps", "azure developer cli", "azure openai", "azure sdk", "bicep", "codespaces", "devops", "docker", "github codespaces", "java", "javascript", "key vault", "kubernetes", "news", "python", "templates", "typescript", "vs code"]
---

Authored by Kristen Womack, this release note summarizes the April 2025 updates to the Azure Developer CLI (azd). Explore new features, improved Azure AI integrations, new templates, and vital enhancements for cloud development workflows.<!--excerpt_end-->

## Azure Developer CLI (azd) – April 2025 Release Overview

**Author:** Kristen Womack

The April 2025 update for the Azure Developer CLI (azd) marks version 1.14.0 and introduces a range of new features, improvements, bug fixes, and community-contributed templates to advance developer efficiency in the Azure ecosystem.

### Notable New Features

- **Environment Setup Improvements**: `azd init` can now automatically load environment values from a root `.env` file, enhancing environment initialization. Users can also bypass non-empty folder warnings using the new `AZD_ALLOW_NON_EMPTY_FOLDER` variable.
- **Streamlined Template Initialization**: The new `azd init --up` flag initializes and then automatically runs `azd up`, reducing setup steps.
- **AI Service Integration**:
  - *Azure AI Search Support*: The `azd add` command now includes Azure AI Search as an option under the AI group.
  - *Existing Resource Support*: Templates can now add existing resources—including AI models, AI Search, Key Vault, Event Hubs, Service Bus, and Storage—using `azd add`.
  - *Location and Quota Validation*: Enhanced validation for Azure AI Services, including quota checks and location metadata.
- **Improved Secrets Management**: Shortcuts added for `azd env set-secrets` enable direct use of Key Vaults created with `azd add`.
- **Resource Visibility Enhancements**: The `azd show` command now lists all resources supported by `azd add`, displaying fully evaluated connection variables.
- **Model Usage Validation**: Arrays of `usageName` for quota validation and support for usage name metadata in main Bicep parameters are now available.
- **Azure Container Apps Support**: Includes environment domain and registry outputs.
- **.NET Aspire 9.1 Updates**: Enhanced deployment, endpoint fetching for Aspire services, better migration mode detection, and support for new Bicep secret handling.

### Bug Fixes

- Resolved issues with Azure OpenAI endpoint display and preview in `azd add`.
- Corrected environment targeting for `azd up -e <env>`.
- Fixed failures in simplified init when provisioning detected databases and handling alpha.compose states.
- Enhanced quota checks for Azure Cognitive Services to require minimum capacity units.

### Other Enhancements

- Upgraded internal dependencies to Go 1.24.
- Switched Azure Service Bus to Standard SKU and refined parameter passing.
- Cleaned up unused core Bicep modules and updated tags in the awesome-azd gallery, adding .NET Aspire as a tool.

### New Community Templates

Several new templates have been added, supporting a broad spectrum of development scenarios:

- **[AZD-LoadTest](https://github.com/maartenvandiemen/azd-loadtest/)** – By Peter De Tender and Maarten van Diemen
- **[LiteLLM in Azure Container Apps](https://github.com/Build5Nines/azd-litellm)** – By Build5Nines
- **[Deploy Your AI Application in Production](https://github.com/microsoft/Deploy-Your-AI-Application-In-Production)** – By Mike Swantek, Daniel Schmidt, and Seth Steenken
- **[Getting Started with Agents Using Azure AI Foundry](https://github.com/Azure-Samples/get-started-with-ai-agents)** – By Howie Leung, Nikolay Rovinskiy, and Sophia Ramsey
- **[Get Started with Chat Using Azure AI Foundry](https://github.com/Azure-Samples/get-started-with-ai-chat)** – By Howie Leung, Nikolay Rovinskiy, and Sophia Ramsey
- **[Conversation Knowledge Mining Solution Accelerator](https://github.com/microsoft/Conversation-Knowledge-Mining-Solution-Accelerator)** – By Malory Rose, Blessing Sanusi, Mark Taylor, and Anish Arora
- **[Azure Language OpenAI Conversational Agent Accelerator](https://github.com/Azure-Samples/Azure-Language-OpenAI-Conversational-Agent-Accelerator)** – By Sean Murray and Yanling Xiong
- **[.NET 9 Web Application with Redis Output Caching and Azure OpenAI](https://github.com/Azure-Samples/azure-redis-dalle-semantic-caching)** – By Catherine Wang

### Getting Started with azd

- **Platforms Supported**: azd can be used on Windows, Linux, or macOS terminals.
- **IDE Integration**: Available as an extension for Visual Studio Code and GitHub Codespaces. Also, can be enabled in Visual Studio via a preview feature flag.
- **Documentation**: Official documentation is available at [aka.ms/azd](https://aka.ms/azd).
- **Community Support**: Join the April release discussion on GitHub, file issues, and contribute templates via the contributor guide or troubleshooting docs.

---

For more details, visit the official [Azure Developer CLI documentation](https://aka.ms/azd) and join the community conversations or contribute your own templates. Thanks to all template authors and contributors for moving the Azure developer experience forward.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-april-2025/)
