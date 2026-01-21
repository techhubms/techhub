---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-june-2025/
title: Azure SDK Release Highlights for June 2025
author: Ronnie Geraghty
feed_name: Microsoft DevBlog
date: 2025-07-02 15:00:43 +00:00
tags:
- .NET
- AI Agents
- Analytics
- Azure AI Search
- Azure Identity
- Azure SDK
- Bing Search
- Bulk API
- Client Library
- Cloud Services
- Cosmos DB
- DefaultAzureCredential
- Go
- Hybrid Search
- Java
- JavaScript
- Online Experimentation
- Python
- Release Notes
- Resource Management
- Resource Manager
- SDK
section_names:
- ai
- azure
- coding
- ml
---
In this comprehensive roundup, Ronnie Geraghty reviews the Azure SDK releases for June 2025, spotlighting advancements in AI, identity, Cosmos DB, and resource management libraries, with details for .NET, Python, Java, JavaScript, and Go developers.<!--excerpt_end-->

# Azure SDK Release Highlights for June 2025

**Author:** Ronnie Geraghty

Stay up to date with the latest from the Azure SDK team! The June 2025 release introduces a suite of new features and improvements targeting developers and integrators of Azure cloud services. Below are this month’s key highlights, updates, and resources.

> Subscribe to the [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/) for monthly notifications.

Quick access: Visit the [Azure SDK Releases page](https://aka.ms/azsdk/releases) for links to packages, code repositories, and documentation.

---

## Release Highlights

### Azure AI Agents Persistent 1.0.0 for .NET (General Availability)

- The [Azure AI Agents Persistent](https://www.nuget.org/packages/Azure.AI.Agents.Persistent/1.0.0) client library is now generally available for .NET developers.
- Provides a toolset for creating, managing, and running AI agents that persist state between sessions.
- Supports integration with grounding tools, such as Azure AI Search and Bing Search.

### Azure Identity: DefaultAzureCredential Customization

- Azure Identity libraries have added support for the `AZURE_TOKEN_CREDENTIALS` environment variable in `DefaultAzureCredential`.
  - Allows selection between ‘Deployed service’ or ‘Developer tool’ credentials via `prod` or `dev` values.
- Minimum version requirements for various languages:
  - [.NET 1.14.0](https://aka.ms/azsdk/net/identity/credential-chains#exclude-a-credential-type-category)
  - [C++ 1.12.0](https://aka.ms/azsdk/cpp/identity/credential-chains#how-to-customize-defaultazurecredential)
  - [Go 1.10.0](https://aka.ms/azsdk/go/identity/credential-chains#how-to-customize-defaultazurecredential)
  - [Java 1.16.1](https://aka.ms/azsdk/java/identity/credential-chains#how-to-customize-defaultazurecredential)
  - [JavaScript 4.10.0](https://aka.ms/azsdk/js/identity/credential-chains#how-to-customize-defaultazurecredential)
  - [Python 1.23.0](https://aka.ms/azsdk/python/identity/credential-chains#exclude-a-credential-type-category)

### Cosmos DB 4.4.0 for JavaScript

- [Cosmos DB JavaScript library (4.4.0)](https://www.npmjs.com/package/@azure/cosmos/v/4.4.0) brings a new Bulk API with these enhancements:
  - Removes the prior 100-operation cap.
  - Introduces per-operation retries for resilience.
  - Implements dynamic congestion control for real-time system adaptation.
  - Adds ‘WeightedRankFusion’ in Hybrid Search for ranked query results.

## Major Initial Stable Releases

**.NET Client Libraries:**

- [AI Agents Persistent 1.0.0](https://www.nuget.org/packages/Azure.AI.Agents.Persistent/1.0.0)

**Python (PyPI):**

- [AI Agents 1.0.0](https://pypi.org/project/azure-ai-agents/1.0.0)
- [Health Deidentification 1.0.0](https://pypi.org/project/azure-health-deidentification/1.0.0)

**Python (Conda):**

- [Azure AI Agents](https://anaconda.org/Microsoft/azure-ai-agents)
- [Azure Health Data Services De-identification](https://anaconda.org/microsoft/azure-health-deidentification)

**.NET Management Libraries:**

- [Resource Management – Neon Postgres 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.NeonPostgres/1.0.0)
- [Resource Management – API Center 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.ApiCenter/1.0.0)

**Go Management:**

- [Resource Management – Workloads SAP Virtual Instance 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/workloadssapvirtualinstance/armworkloadssapvirtualinstance)

**JavaScript Management:**

- [Resource Management – Arize AI Observability Eval 1.0.0](https://www.npmjs.com/package/@azure/arm-arizeaiobservabilityeval)

## Beta Releases

Selected highlights among initial beta libraries:

- **.NET:**
  - [Analytics – Online Experimentation 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Analytics.OnlineExperimentation/1.0.0-beta.1)
  - Various Resource Management betas: Dependency Map, Carbon Optimization, Secrets Store Extension, Pure Storage Block, and more.
- **Java:**
  - [AI Agents Persistent 1.0.0-beta.1](https://mvnrepository.com/artifact/com.azure/azure-ai-agents-persistent/1.0.0-beta.1)
  - [AI Projects 1.0.0-beta.1](https://mvnrepository.com/artifact/com.azure/azure-ai-projects/1.0.0-beta.1)
  - Analytics, Resource Management, Kubernetes and more.
- **JavaScript:**
  - [AI Agents 1.0.0-beta.1](https://www.npmjs.com/package/@azure/ai-agents/v/1.0.0-beta.1)
  - [Online Experimentation 1.0.0-beta.1](https://www.npmjs.com/package/@azure-rest/onlineexperimentation/v/1.0.0-beta.1)
  - Resource Manager: Carbon Optimization, Extensions, Site Manager.
- **Python:**
  - [Online Experimentation 1.0.0b1](https://pypi.org/project/azure-onlineexperimentation/1.0.0b1)
  - Resource Management: Kubernetes Configuration, Site Manager, Secrets Store, MongoDB Atlas, and others.
- **Go:**
  - Resource manager betas for Lambda Test Hyper Execute, Programmable Connectivity, Kubernetes, and more.

  For detailed links, see the release notes below.

## Release Notes by Language

- [All languages](https://azure.github.io/azure-sdk/releases/2025-06/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2025-06/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2025-06/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-06/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2025-06/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2025-06/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2025-06/rust.html)
- [C++](https://azure.github.io/azure-sdk/releases/2025-06/cpp.html)
- [Embedded C](https://azure.github.io/azure-sdk/releases/2025-06/c.html)
- [Android](https://azure.github.io/azure-sdk/releases/2025-06/android.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2025-06/ios.html)

## Additional Resources

- For all Azure SDK release info and downloads: [Azure SDK Releases](https://aka.ms/azsdk/releases)
- [Subscribe to Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk/feed/)

Stay tuned for more updates each month as Azure evolves with new capabilities and developer-centric enhancements.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-june-2025/)
