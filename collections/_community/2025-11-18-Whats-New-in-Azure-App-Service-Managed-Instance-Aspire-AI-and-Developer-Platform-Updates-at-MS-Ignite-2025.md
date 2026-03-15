---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-app-service-at-msignite-2025/ba-p/4468207
title: "What's New in Azure App Service: Managed Instance, Aspire, AI, and Developer Platform Updates at MS Ignite 2025"
author: Stefan_Schackow
feed_name: Microsoft Tech Community
date: 2025-11-18 16:27:30 +00:00
tags:
- .NET
- AI Integration
- App Service Environment V3
- ASP.NET
- Aspire
- Azure App Service
- Azure Monitor
- Azure Pipelines
- CI/CD
- Custom Error Pages
- Elastic APM
- Foundry Agent Service
- GitHub Actions
- Hyper V
- Linux
- Log Stream
- Managed Instance
- Microsoft Agent Framework
- Microsoft Ignite
- Network Segmentation
- Node.js
- Open Telemetry
- PHP
- Premium V4 SKU
- Private Endpoints
- Python
- Root Certificate
- Scaling
- SCM Advanced Tools
- Small Language Models
- Ubuntu
- AI
- Azure
- DevOps
- Community
section_names:
- ai
- azure
- dotnet
- devops
primary_section: ai
---
Stefan Schackow delivers a deep dive into the latest innovations in Azure App Service from Microsoft Ignite 2025, highlighting modernization with Managed Instance, Aspire enhancements, AI agent integration, and developer-centric platform updates.<!--excerpt_end-->

# What's New in Azure App Service at MS Ignite 2025

## Modernize ASP.NET and .NET Web Apps with Managed Instance on Azure App Service

Azure App Service now features Managed Instance, a new public preview enabling organizations to migrate classic ASP.NET and .NET apps (including those with custom Windows dependencies) to Azure's managed PaaS with minimal code changes. This service supports scenarios such as COM/COM+ integration, Windows registry access, custom server-side PDF rendering (GDI+), and installation of custom software.

**Premium v4 SKU and Hyper-V Nested Virtualization:** Managed Instance leverages Hyper-V nested virtualization to run custom scripts and dependencies during VM startup. Developers can maintain and update both .NET and Windows OS components automatically, access IIS Manager, and securely connect via RDP.

- **Related Sessions** at Ignite 2025:
  - Technical deep dive BRK102 ([details](https://ignite.microsoft.com/sessions/BRK102))
  - Hands-on migration labs LAB501
  - Conversation Corners for Q&A
  - [Public preview of Managed Instance](https://aka.ms/AppService/ManagedInstance) is now open in select regions

## Enhanced Aspire Support for .NET Developers on Linux

App Service continues to support and evolve the [Aspire](https://github.com/dotnet/aspire) platform, now offering deep integration for the Aspire developer dashboard on Azure App Service for Linux. Developers can:

- Access application logs and metrics
- Visualize project topology and drill down into Aspire web components
- Watch demos from [.NET Conf 2025](https://youtu.be/WSHMfrCHD0c?si=eHJBe5sWs5PTSY0c) and explore [Aspire samples on GitHub](https://github.com/Azure-Samples/appservice-aspire-samples/)

## Building AI-Integrated Web Applications

Azure App Service is positioned as a premier platform for creating intelligent applications and integrating AI agents. Recent advancements include:

- [AI Integration landing page](https://learn.microsoft.com/azure/app-service/overview-ai-integration) with usage samples
- Direct integration examples using Foundry Agent Service and [Microsoft Agent Framework](https://azure.microsoft.com/blog/introducing-microsoft-agent-framework/)
- Orchestrating multi-agent workflows ([sample](https://techcommunity.microsoft.com/blog/appsonazureblog/part-2-build-long-running-ai-agents-on-azure-app-service-with-microsoft-agent-fr/4465825)) both on Foundry Agent Service and directly inside applications
- Utilizing web/API apps as external agent tools via [OpenAPI endpoints](https://learn.microsoft.com/azure/app-service/overview-ai-integration?tabs=dotnet#app-service-as-openapi-tool-in-azure-ai-foundry-agent) and [Model Context Protocol (MCP)](https://learn.microsoft.com/azure/app-service/overview-ai-integration?tabs=dotnet#app-service-as-model-context-protocol-mcp-servers)
- Explore related samples ([e-commerce scenario](https://github.com/Azure-Samples/app-service-aspire-eshop-agents)) and [Azure Friday deep dives](https://docs.microsoft.com/shows/azure-friday/create-intelligent-ai-agents-and-resilient-apps-with-azure-app-service/)

## Platform and Language Runtime Updates

Recent improvements across Azure App Service include:

- Support for LTS releases like Python 3.14, Node.js 24, PHP 8.4, and .NET 10 GA (on Ubuntu images)
- New developer features: deployment via pyproject+uv+Poetry, enhanced bash shell, ever-growing [Python AI sample collection](https://github.com/Azure-Samples/appservice-ai-samples/#sample-projects)
- General Availability of [Custom Error Pages](https://learn.microsoft.com/azure/app-service/configure-error-pages) and major [Log Stream](https://learn.microsoft.com/azure/app-service/configure-error-pages) updates for granular observability
- SCM site UX revamp with a new [AI Playground](https://techcommunity.microsoft.com/blog/AppsonAzureBlog/introducing-ai-playground-on-azure-app-service-for-linux/4469497) for small language models (SLMs)
- Sidecar support for CI/CD ([GitHub Actions/Azure Pipelines](https://techcommunity.microsoft.com/blog/appsonazureblog/add-sidecars-to-azure-app-service-for-linux%E2%80%94via-github-actions-or-azure-pipeline/4465419)), and pre-built Open Telemetry extensions for telemetry streaming to [Azure Monitor](https://techcommunity.microsoft.com/blog/appsonazureblog/part-i-otel-sidecar-extension-on-azure-app-service-for-linux---intro--php-walkth/4469514) and [Elastic APM](https://techcommunity.microsoft.com/blog/appsonazureblog/part-ii-otel-sidecar-extension-on-azure-app-service-for-linux---elastic-apm-setu/4469576)

## Scaling, Networking, and ASE (App Service Environment) Updates

- **Async scaling** (public preview): enables reliable, repeatable resource scaling and rapid app service plan creation ([docs](https://aka.ms/appserviceplanasyncscaling))
- **IPv6 support:** inbound traffic is GA; outbound traffic support rolling out
- **Private endpoints & custom domain binding**: private DNS zone option entering preview
- **ASE v3 improvements:** new [root certificate API](https://aka.ms/asev3-root-cert-api) (GA), outbound network segmentation, and future Premium v4 hardware support

## Next Steps and Resources

- [Getting started with Azure App Service](https://learn.microsoft.com/azure/app-service/getting-started)
- Stay updated via [Azure Updates](https://azure.microsoft.com/updates), [Apps on Azure Blog](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/bg-p/appsonazureblog/), [@AzAppService Twitter](https://x.com/AzAppService), and deep-dive technical [article searches](https://aka.ms/AppService/TechCommunityArticlesSearchLink)
- Explore event session references and hands-on labs from Ignite 2025 for deeper learning opportunities

---

## Session Reference: Microsoft Ignite 2025

- [BRK150](https://ignite.microsoft.com/sessions/BRK150): Modernizing .NET on Azure
- [BRK102](https://ignite.microsoft.com/sessions/BRK102): Managed Instance technical deep dive
- [LAB501](https://ignite.microsoft.com/sessions/LAB501): ASP.NET migration hands-on
- [BRK116](https://ignite.microsoft.com/sessions/BRK116): Apps, agents, and MCP AI innovation

*Author: Stefan Schackow, Azure App Service Product Group*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/what-s-new-in-azure-app-service-at-msignite-2025/ba-p/4468207)
