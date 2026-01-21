---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-october-2025/
title: Azure SDK Release Highlights – October 2025
author: Ronnie Geraghty
feed_name: Microsoft Azure SDK Blog
date: 2025-10-27 18:59:41 +00:00
tags:
- .NET
- AI Agents
- AI Model Deployment
- API Improvements
- Azure AI Foundry
- Azure AI Search
- Azure Identity
- Azure SDK
- Document Intelligence
- Go
- Java
- JavaScript
- Microsoft Entra ID
- Python
- Resource Management
- SDK
- SDK Release
- Search Indexes
- Vector Search
section_names:
- ai
- azure
- coding
---
Ronnie Geraghty presents the October 2025 Azure SDK release highlights, detailing new and updated libraries for AI, search, and identity scenarios to help developers build and manage Azure-powered solutions.<!--excerpt_end-->

# Azure SDK Release Highlights – October 2025

By Ronnie Geraghty

Stay up to date with the latest Azure SDK releases and improvements rolled out in October 2025. This summary covers key updates in AI, search, identity, and resource management across multiple programming languages, providing developers robust tools to enhance their solutions.

## Release Highlights

### Azure AI Foundry 1.0.0 for .NET

- First stable release of Azure AI Foundry library for .NET
- Enables developers to:
  - Create and run AI Agents
  - Manage AI model deployments
  - Handle connections to Azure resources
  - Upload documents and create datasets
  - Manage search indexes
- Notable improvements:
  - Consistent `AIProject` prefix for key models (`AIProjectDeployment`, `AIProjectConnection`, `AIProjectIndex`)
  - Simplified client access and new `CreateOrUpdate` methods for index management
  - Uses v1 AI Foundry data plane REST APIs and integrates secure authentication with Microsoft Entra ID

### Azure AI Search for JavaScript (12.2.0) and Python (11.6.0)

- Adds support for the `2025-09-01` service version
- New features:
  - Vector queries against sub-fields of complex fields
  - Reranker boosted scores and expanded sorting options
  - Improved vector compression configs
  - LexicalNormalizers for better text analysis
  - New `DocumentIntelligenceLayoutSkill` for richer skillsets
  - OneLake data source connections for indexers
  - For Python: `QueryDebugMode` for debugging vector search

### Azure Identity Libraries – Performance Improvements

- Managed identity scenarios optimized for these languages:
  - C++ (1.13.2), Go (1.13.0), Java (1.18.1), JavaScript (4.13.0), Python (1.25.1)
- Skips IMDS endpoint probe when `AZURE_TOKEN_CREDENTIALS` is set to `ManagedIdentityCredential`, using direct token acquisition with robust retry logic and exponential backoff

## Initial Stable Releases

- .NET: AI Foundry 1.0.0, AI VoiceLive 1.0.0
- Java, Go, JavaScript, Python: Multiple new management libraries, notably for Data Migration, Site Manager, Storage Discovery, and Durable Task

## Beta Releases

- .NET, Go, Java: Beta versions of resource management libraries for Disconnected Operations and Compute.Recommender

## Where to Learn More

- [Azure SDK Release Notes – October 2025 (All languages)](https://azure.github.io/azure-sdk/releases/2025-10/index.html)
- Language-specific notes:
  - [.NET](https://azure.github.io/azure-sdk/releases/2025-10/dotnet.html)
  - [Java](https://azure.github.io/azure-sdk/releases/2025-10/java.html)
  - [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-10/js.html)
  - [Python](https://azure.github.io/azure-sdk/releases/2025-10/python.html)
  - [Go](https://azure.github.io/azure-sdk/releases/2025-10/go.html)
  - [Rust](https://azure.github.io/azure-sdk/releases/2025-10/rust.html)
  - [C++](https://azure.github.io/azure-sdk/releases/2025-10/cpp.html)

## Useful Links

- [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk/)
- [Azure SDK Releases Page](https://aka.ms/azsdk/releases)

---

For more details on package updates, enhancements, and code samples, visit the release notes for your preferred programming language.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-october-2025/)
