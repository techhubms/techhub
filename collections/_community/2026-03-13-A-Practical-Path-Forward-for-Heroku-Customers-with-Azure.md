---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/a-practical-path-forward-for-heroku-customers-with-azure/ba-p/4501797
title: A Practical Path Forward for Heroku Customers with Azure
author: NagaSurendran
primary_section: github-copilot
feed_name: Microsoft Tech Community
date: 2026-03-13 15:00:00 +00:00
tags:
- .NET
- AI
- AI Platform
- App Modernization
- Application Monitoring
- Azure
- Azure App Service
- Azure Container Apps
- Azure Monitor
- Cloud Native
- Community
- Dapr
- DevOps
- GitHub Copilot
- GitHub Integration
- Heroku Migration
- Java
- KEDA
- Microservices
- Microsoft Foundry
- Node.js
- PostgreSQL
- Python
- Serverless
section_names:
- ai
- azure
- devops
- github-copilot
---
NagaSurendran details practical strategies for organizations migrating from Heroku, focusing on how Azure and its integrated tools—including GitHub Copilot and Microsoft Foundry—enable modern, secure, and intelligent cloud-native applications.<!--excerpt_end-->

# A Practical Path Forward for Heroku Customers with Azure

Heroku recently announced a transition to a sustaining engineering model, prompting many organizations to reassess their application hosting and modernization strategies. Microsoft Azure positions itself as a comprehensive, developer-friendly platform—ideal for migrating and evolving workloads previously running on Heroku.

## Comprehensive Application Platform

Azure provides:

- **Web Apps, APIs, Event-driven Systems, and Containers**: Use [Azure App Service](https://azure.microsoft.com/en-us/products/app-service) for managed web app hosting or [Azure Container Apps](https://azure.microsoft.com/en-us/products/container-apps) for running serverless, containerized microservices with features like scale-to-zero and integrated security.
- **Developer Ecosystem**: Supports .NET, Java, Node.js, Python, Docker containers, and more. Begin with familiar frameworks, then evolve toward microservices, event-driven architectures (with Dapr and KEDA), and background processing as needed.

## Databases and State Management

- **PostgreSQL Compatibility**: [Azure Database for PostgreSQL](https://azure.microsoft.com/en-us/products/postgresql) provides a seamless transition for Heroku apps reliant on PostgreSQL, offering high availability, automated scaling, security integration, and predictable performance.

## GitHub Integration and Agentic DevOps

- **Native GitHub Integration**: Azure seamlessly connects with [GitHub](https://learn.microsoft.com/en-us/azure/developer/github/), enabling source control, CI/CD, and collaboration.
- **GitHub Copilot and Modernization Agent**: [GitHub Copilot](https://learn.microsoft.com/en-us/dotnet/core/porting/github-copilot-app-modernization/overview) enhances modernization by helping developers understand legacy code, refactor components, update dependencies, and migrate workloads to Azure. GitHub Actions automates CI/CD workflows.
- **Agentic Tooling**: Modernization agents assist migration tasks and reduce manual effort for common refactoring patterns.

## Monitoring, Reliability, and Operations

- **Observability**: [Azure Monitor](https://azure.microsoft.com/en-us/products/monitor) and AI-assisted diagnostics simplify production monitoring and troubleshooting.
- **SRE Tools**: [Azure SRE Agent](https://azure.microsoft.com/en-us/products/sre-agent) automates operations to improve uptime, accelerate diagnosis, and reduce incident impact.

## AI Platform: Models, Agents, and Tools

- **Microsoft Foundry**: [Microsoft Foundry](https://azure.microsoft.com/en-us/products/ai-foundry) offers unified access to 11,000+ AI models—including options from OpenAI, Meta, Anthropic, and Microsoft—integrated with orchestration, monitoring, evaluation, and governance. Foundry enables secure, enterprise-ready AI features, agentic app patterns, and event-driven workflows. MCP support enables robust tool-calling and sandboxed execution.

## Path Forward and Migration Strategies

- **Incremental Modernization**: Start by [moving select services](https://learn.microsoft.com/en-us/azure/container-apps/migrate-heroku) to Azure Container Apps or Azure App Service, running existing apps in parallel.
- **AI-Native and Event-Driven Scenarios**: Leverage Microsoft Foundry for fast experimentation and production deployment of AI-driven features.
- **DevOps and Automation**: Integrate GitHub Copilot and agentic tooling to automate code migration and align with Azure architectures.

## Resources

- [Learn how customers build modern AI Apps on Azure](https://www.microsoft.com/en-us/customers/search?q=azure+container+apps+azure+app+service)
- [Modernize your applications faster with GitHub Copilot](https://github.com/solutions/use-case/app-modernization)
- [Build, deploy, and scale microservices with Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/overview)
- [Azure App Service Overview](https://learn.microsoft.com/en-us/azure/app-service/overview)
- [Run PostgreSQL at enterprise scale](https://learn.microsoft.com/en-us/azure/postgresql/)

Azure brings together global-class infrastructure, rich developer tooling, and deep AI integration to empower Heroku customers as they evolve toward cloud-native and AI-native architectures.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/a-practical-path-forward-for-heroku-customers-with-azure/ba-p/4501797)
