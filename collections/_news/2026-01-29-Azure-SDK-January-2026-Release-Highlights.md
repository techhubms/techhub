---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-january-2026/
title: Azure SDK January 2026 Release Highlights
author: Ronnie Geraghty
primary_section: ai
feed_name: Microsoft Azure SDK Blog
date: 2026-01-29 22:00:41 +00:00
tags:
- .NET
- AgentServer
- AI
- Azure
- Azure AI Foundry
- Azure AI Search
- Azure Functions
- Azure OpenAI
- Azure SDK
- Beta Release
- Client Libraries
- Coding
- DNS
- Durable Task
- Front Door
- Go
- GPT 5
- Java
- Management Libraries
- Microsoft Azure
- Mongo Cluster
- News
- Python
- Resource Management
- SDK
- SocketIO
- Web PubSub
section_names:
- ai
- azure
- coding
---
Ronnie Geraghty summarizes the January 2026 Azure SDK release, spotlighting significant improvements in AI, real-time communication tools, and expanded platform support for developers.<!--excerpt_end-->

# Azure SDK January 2026 Release Highlights

By Ronnie Geraghty

## Overview

The Azure SDK January 2026 release brings a wealth of new features, enhancements, and bug fixes for developers building on Microsoft Azure. Packages span several languages including .NET, Go, Java, and Python, with a focus on AI integration, data services, real-time communications, and resource management.

Subscribe to the [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/) for future updates.

Full release details, links to packages, code, and documentation are available on the [Azure SDK Releases page](https://aka.ms/azsdk/releases).

## Release Highlights

### AI Foundry 1.2.0-beta.1 (.NET)

- Major feature update enabling the Microsoft Foundry Agents Service.
- Integrates with the new `Azure.AI.Projects.OpenAI` package.
- Expands evaluation, insights, red teaming, scheduling, and more.
- Designed for .NET developers working with Azure AI services.

### Azure AI Search 11.8.0-beta.1 (.NET)

- Adds support for the `2025-11-01-preview` service version.
- New features: multiple facet aggregation types (avg, min, max, cardinality).
- New knowledge sources: web, remoteSharePoint, indexedSharePoint, indexedOneLake.
- Support for Azure OpenAI models: gpt-5, gpt-5-mini, gpt-5-nano.
- Breaking change: Knowledge Agent renamed to Knowledge Base across APIs.

### Functions Extension for WebPubSub for SocketIO 1.0.0 (.NET)

- Enables developers to build real-time, bidirectional applications with Socket.IO using Azure Web PubSub as backend.
- First stable release for this extension.

### Initial Stable Releases

- Functions extension for Web PubSub for SocketIO 1.0.0 ([NuGet](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO/1.0.0))
- Resource Management Libraries for .NET: Storage Discovery, Mongo Cluster, Durable Task
- Management Library for Go: Resource Deployment

### Initial Beta Releases

- .NET: Provisioning Libraries for Front Door and DNS
- AI AgentServer Contract, Core, and AgentFramework for .NET
- Java: azure-ai-voicelive, azure-ai-agents
- Python: confidential ledger certificate, planetary computer, AI agent server core
- Python: Recoveryservicesbackup-Passivestamp Management Library

## Release Notes

- [All languages](https://azure.github.io/azure-sdk/releases/2026-01/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2026-01/dotnet.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2026-01/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2026-01/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2026-01/go.html)
- [C++](https://azure.github.io/azure-sdk/releases/2026-01/cpp.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2026-01/ios.html)

## Useful Links

- [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk/)
- [Azure SDK Releases](https://aka.ms/azsdk/releases)

## Conclusion

January 2026's Azure SDK release advances support for AI workloads, improves real-time communication solutions, and extends cross-language tooling, helping developers build trusted, modern applications on Azure.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-january-2026/)
