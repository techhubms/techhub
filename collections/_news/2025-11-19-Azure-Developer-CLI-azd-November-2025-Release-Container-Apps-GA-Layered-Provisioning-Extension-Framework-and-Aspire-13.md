---
layout: "post"
title: "Azure Developer CLI (azd) November 2025 Release: Container Apps GA, Layered Provisioning, Extension Framework, and Aspire 13"
description: "The November 2025 release of Azure Developer CLI (azd) introduces major features such as general availability for Azure Container Apps, layered provisioning in beta, expanded extension framework capabilities, and Aspire 13 compatibility. The update adds new prepublish and postpublish hooks, enhanced support for community templates—including Copilot Studio scenarios—plus improvements to pipeline configuration, documentation, and bug fixes. Developers gain increased flexibility and extensibility, especially around workflows, programming language support, and integration with GitHub Copilot, Azure AI, and other cloud-native technologies."
author: "PuiChee (PC) Chan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-november-2025/"
viewing_mode: "external"
feed_name: "Microsoft Azure SDK Blog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-11-19 16:00:44 +00:00
permalink: "/2025-11-19-Azure-Developer-CLI-azd-November-2025-Release-Container-Apps-GA-Layered-Provisioning-Extension-Framework-and-Aspire-13.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: [".NET", ".NET 10", "AI", "Aspire 13", "Azd", "Azure", "Azure AI Search", "Azure Communication Services", "Azure Container Apps", "Azure Developer CLI", "Azure Functions", "Azure SDK", "CI/CD", "Codespaces", "Coding", "Copilot Studio", "DevOps", "Docker", "Event Grid", "Extension Framework", "gRPC API", "Java", "JavaScript", "Kubernetes", "Layered Provisioning", "News", "Python", "Semantic Kernel", "Typescript", "VS Code"]
tags_normalized: ["dotnet", "dotnet 10", "ai", "aspire 13", "azd", "azure", "azure ai search", "azure communication services", "azure container apps", "azure developer cli", "azure functions", "azure sdk", "cislashcd", "codespaces", "coding", "copilot studio", "devops", "docker", "event grid", "extension framework", "grpc api", "java", "javascript", "kubernetes", "layered provisioning", "news", "python", "semantic kernel", "typescript", "vs code"]
---

PuiChee (PC) Chan shares insights into the latest Azure Developer CLI (azd) release, covering new features like container apps GA, layered provisioning beta, extension enhancements, template updates, and tighter integration with AI and DevOps workflows.<!--excerpt_end-->

# Azure Developer CLI (azd) November 2025 Release Overview

**Authored by PuiChee (PC) Chan**

## Release Highlights

- Azure Container Apps support **GA** (general availability)
- Layered provisioning feature now in **Beta**
- Extension framework adds:
  - Custom ServiceConfig settings
  - Enhanced CI/CD and no-prompt support
  - ServiceContext lifecycle events
  - Additional programming language/framework support
  - Custom service target endpoints for azd show
  - New AccountService gRPC API
- **Aspire 13** compatibility guarantees smooth .NET 10 and cloud integration
- Updated pipeline configs support the latest .NET runtimes
- Azure App Service deployments now display Aspire dashboard URLs
- `azure.yaml` gains prepublish and postpublish hooks for workflow customization
- Container Apps deployments now support runtime env variable management via `azure.yaml`

## Bug Fixes & Improvements

- Container App resource check accuracy
- Error messaging for missing `--environment` flag
- Output formatting fixes for hyperlinks
- Progress bar stability during agent deployments
- Improved lifetime management and reliability within extensions
- Environment reload improvements for bicep provider
- Remote build stability for agent extension
- Project-level events and workflow command registration fixes
- Developer experience improvements (logged hook warnings, completion support for Visual Studio Code)

## Documentation Updates

- [Container App deployment docs](https://learn.microsoft.com/azure/developer/azure-developer-cli/container-apps-workflows)
- [GitHub Copilot coding agent extension guide](https://learn.microsoft.com/azure/developer/azure-developer-cli/extensions/copilot-coding-agent-extension)
- [Custom workflow hooks documentation](https://learn.microsoft.com/azure/developer/azure-developer-cli/azd-extensibility)
- “Dev to Production” blog covering solutions for image publishing and layered infrastructure: [Azure Container Apps Dev-to-Prod Deployment](https://devblogs.microsoft.com/devops/azure-developer-cli-azure-container-apps-dev-to-prod-deployment-with-layered-infrastructure/)

## Community Templates

- **Copilot Studio with Azure AI Search**: Build AI-based agents using Copilot Studio and Azure AI Search ([template](https://github.com/Azure-Samples/Copilot-Studio-with-Azure-AI-Search))
- **Release Manager Assistant**: AI solution accelerator for release lifecycle management ([template](https://github.com/microsoft/release-manager-assistant))
- **Call Center Voice Agent Accelerator**: Scalable voice agents via Azure Communication Services ([template](https://github.com/Azure-Samples/call-center-voice-agent-accelerator))
- **AI applications with SharePoint knowledge and actions**: Use Azure AI Foundry SDK and Copilot Retrieval API ([template](https://github.com/microsoft/app-with-sharepoint-knowledge))
- **Functions Event Grid Blob Trigger quickstarts**: for Python, JavaScript, .NET, Java, TypeScript ([gallery](https://github.com/Azure/awesome-azd/pull/661))
- **Semantic Kernel Function App**: Deploy Semantic Kernel-powered function apps ([template](https://github.com/JayChase/semantic-kernel-function-app))
- **Logic App AI Agents and AI Foundry** ([template](https://github.com/marnixcox/logicapp-ai-agent))
- **Azure Container Apps dynamic sessions w/ Python interpreter** ([template](https://github.com/Azure-Samples/aca-python-code-interpreter-session))
- **Label Studio on Container Apps** ([template](https://github.com/bderusha/azd-label-studio))

## Contributor Acknowledgments

Many community contributors advanced support for Azure, AI, and DevOps workflows.

- HadwaAbdelhalem (Copilot Studio template with AI Search)
- JayChase (Semantic Kernel Function App)
- marnixcox (Logic Apps AI Agents, Azure AI Foundry)
- Jeff Martinez (Python dynamic sessions)
- Bill DeRusha (Label Studio deployment)

## Getting Started with azd

- [Install the Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd?tabs=winget-windows%2Cbrew-mac%2Cscript-linux&amp;pivots=os-windows)
- Explore curated [template galleries](https://azure.github.io/awesome-azd/) and [AI App Templates](https://aka.ms/ai-gallery)
- Read the [official docs](https://learn.microsoft.com/azure/developer/azure-developer-cli/)
- Visit the [troubleshooting guide](https://learn.microsoft.com/azure/developer/azure-developer-cli/troubleshoot)
- Raise issues or join discussions via [GitHub](https://github.com/Azure/azure-dev)

---

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-developer-cli-azd-november-2025/)
