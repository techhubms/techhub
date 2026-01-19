---
layout: post
title: Build Multi-Agent AI Systems on Azure App Service
author: jordanselig
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-multi-agent-ai-systems-on-azure-app-service/ba-p/4451373
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-05 16:37:30 +00:00
permalink: /ai/community/Build-Multi-Agent-AI-Systems-on-Azure-App-Service
tags:
- .NET Aspire
- Agent Orchestration
- App Deployment
- App Monitoring
- Azure AI Foundry
- Azure App Service
- Cloud Native
- Connected Agents
- E Commerce
- MCP
- Multi Agent Systems
- Observability
- OpenAPI Tools
- Premium V4
- Python
section_names:
- ai
- azure
- coding
---
jordanselig illustrates how to build sophisticated multi-agent AI applications on Azure App Service, combining Azure AI Foundry, .NET Aspire, and MCP tooling for a cloud-native, scalable, and observable solution.<!--excerpt_end-->

# Build Multi-Agent AI Systems on Azure App Service

## Introduction

Discover how to enhance your Azure App Service applications by integrating multi-agent AI architectures with technologies like Azure AI Foundry, Model Context Protocol (MCP), OpenAPI, and .NET Aspire. This guide walks through building a fashion e-commerce demo with multiple specialized AI agents, increased observability, and seamless cloud deployment.

## Key Technologies and Architecture

### Multi-Agent System Components

- **Main Orchestrator** manages workflow and inventory queries via MCP tools
- **Cart Manager** handles shopping cart operations through OpenAPI integrations
- **Fashion Advisor** provides personalized styling recommendations
- **Content Moderator** ensures safe, compliant user interactions

### Tooling and Integrations

- **MCP Tools**: Real-time external inventory connections using the Model Context Protocol
- **OpenAPI Tools**: Direct API links to App Service, enabling seamless agent actions
- **Connected Agent Tools**: Enable automatic, orchestrated communication between agents for more complex workflows

### .NET Aspire and Premium v4 App Service

- **.NET Aspire**: Enhances developer experience by providing built-in observability and cloud-native patterns, plus real-time telemetry in local development
- **App Service Premium v4**: Offers the most performant, scalable hosting for modern, AI-powered workloads

## Implementation Patterns

- **Incremental Enhancement**: Extend existing App Service infrastructure with multi-agent capabilities
- **Simple Integration**: Use familiar tools like `azd up` for environment and agent deployment
- **Production-Readiness**: Build on mature Azure services; easily extendable as new features emerge

## Step-By-Step Getting Started

1. **Clone the Sample**: Download code and resources from the [GitHub repository](https://github.com/seligj95/app-service-aspire-eshop-agents)
2. **Quick Deploy**: Use `azd up` for one-command infrastructure setup
3. **Configure Agents**: Run the provided Python setup script to stand up your multi-agent environment
4. **Environment Linking**: Add a single environment variable to connect your agents
5. **Explore and Test**: Experiment with the provided sample conversations and observe agent orchestration in action

## Upcoming Features and Roadmap

- **MCP Authentication Integration**: Enhanced security with Azure Entra ID
- **Azure AI Foundry Updates**: New agent features, integration improvements, and model support
- **Advanced Analytics**: Deeper Azure Monitor and business intelligence integrations
- **Multi-Language Sample Expansion**: Support for more programming languages beyond .NET and Python

## Learning Resources

- [Integrate AI into your Azure App Service applications](https://learn.microsoft.com/azure/app-service/overview-ai-integration?tabs=dotnet)
- [Supercharge Your App Service Apps with AI Foundry Agents](https://techcommunity.microsoft.com/blog/appsonazureblog/supercharge-your-app-service-apps-with-ai-foundry-agents-connected-to-mcp-server/4444310)
- [Host Remote MCP Servers on App Service](https://techcommunity.microsoft.com/blog/appsonazureblog/host-remote-mcp-servers-on-app-service-updated-samples-now-with-new-languages-an/4420607)
- [Azure AI Foundry Documentation: Connected Agents Guide](https://learn.microsoft.com/azure/ai-foundry/agents/how-to/connected-agents)
- [.NET Aspire on App Service Deployment Guide](https://azure.github.io/AppService/2025/05/19/Aspire-on-App-Service.html)
- [Premium v4 Announcement](https://techcommunity.microsoft.com/blog/appsonazureblog/announcing-general-availability-of-premium-v4-for-azure-app-service/4446204)

## Conclusion

This sample and guide showcase how to incrementally evolve existing applications into modern, AI-powered, cloud-native systems using the Azure ecosystem. With clear deployment patterns, advanced observability, and flexible integration, developers can confidently embrace the future of multi-agent systems on Azure App Service. For full technical deep-dives and the latest updates, visit the GitHub repository and official documentation linked above.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/build-multi-agent-ai-systems-on-azure-app-service/ba-p/4451373)
