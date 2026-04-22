---
tags:
- .NET
- AI
- API Management
- Azure
- Azure AI Agents
- Azure AI Agents Java SDK
- Azure AI Foundry
- Azure Batch
- Azure Compute
- Azure Cosmos Java SDK
- Azure Monitor
- Azure Provisioning
- Azure Resource Manager
- Azure SDK
- Azure Security Center
- Azure Stack HCI
- Azure.AI.Projects
- Azure.ResourceManager
- Breaking Changes
- Cosmos DB
- CWE 502
- ExpandableStringEnum
- General Availability
- Java Deserialization
- JavaScript SDK
- JSON Serialization
- News
- NuGet
- Release Notes
- Remote Code Execution
- SDK
- Security
- TypeScript SDK
title: Azure SDK Release (April 2026)
feed_name: Microsoft Azure SDK Blog
date: 2026-04-21 23:13:33 +00:00
author: Ronnie Geraghty
primary_section: ai
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-april-2026/
section_names:
- ai
- azure
- dotnet
- security
---

Ronnie Geraghty summarizes the April 2026 Azure SDK releases, highlighting key changes across Cosmos DB, Azure AI Foundry/Projects, and Azure AI Agents, plus links to full release notes for .NET, Java, JavaScript/TypeScript, Python, Go, and more.<!--excerpt_end-->

# Azure SDK Release (April 2026)

Thank you for your interest in the new Azure SDKs! We release new features, improvements, and bug fixes every month.

- Subscribe: [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/)
- Releases hub: [Azure SDK Releases page](https://aka.ms/azsdk/releases)

## Release highlights

### Cosmos DB 4.79.0 (Java)

Package link: [com.azure:azure-cosmos 4.79.0](https://central.sonatype.com/artifact/com.azure/azure-cosmos/4.79.0)

Key updates:

- **Critical security fix for Remote Code Execution (RCE)** vulnerability **CWE-502**.
  - **Java deserialization was replaced with JSON-based serialization** in:
    - `CosmosClientMetadataCachesSnapshot`
    - `AsyncCache`
    - `DocumentCollection`
  - This change is intended to **eliminate the entire class of Java deserialization attacks**.
- Adds support for:
  - **N-Region synchronous commit**
  - **Query Advisor** feature
  - `CosmosFullTextScoreScope` for controlling **BM25 statistics scope** in hybrid search queries

### AI Foundry 2.0.0 (.NET)

Package link: [Azure.AI.Projects 2.0.0](https://www.nuget.org/packages/Azure.AI.Projects/2.0.0)

The `Azure.AI.Projects` NuGet package ships its **2.0.0 stable release** with significant architectural changes:

- **Evaluations** and **memory operations** moved to separate namespaces:
  - `Azure.AI.Projects.Evaluation`
  - `Azure.AI.Projects.Memory`
- Renames for consistency:
  - `Insights` → `ProjectInsights`
  - `Schedules` → `ProjectSchedules`
  - `Evaluators` → `ProjectEvaluators`
  - `Trigger` → `ScheduleTrigger`
- Boolean properties now follow the `Is*` naming convention.
- Several internal types were made internal.

### AI Agents 2.0.0 (Java)

Package link: [com.azure:azure-ai-agents 2.0.0](https://central.sonatype.com/artifact/com.azure/azure-ai-agents/2.0.0)

The Java Azure AI Agents library reaches **general availability** with version 2.0.0, including breaking changes for API consistency:

- Several enum types were converted from standard Java `enum` to `ExpandableStringEnum`-based classes.
- `*Param` model classes were renamed to `*Parameter`.
- `MCPToolConnectorId` now uses consistent casing as `McpToolConnectorId`.
- Added a new convenience overload for `beginUpdateMemories`.

## Initial stable releases

- **Client Libraries for .NET**
  - [Provisioning – Network 1.0.0](https://www.nuget.org/packages/Azure.Provisioning.Network/1.0.0)
  - [Provisioning – Private DNS 1.0.0](https://www.nuget.org/packages/Azure.Provisioning.PrivateDns/1.0.0)
- **Management Library for Java**
  - [Resource Management – Azure Stack HCI 1.0.0](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-azurestackhci/1.0.0)
- **Management Library for Go**
  - [Resource Management – Policy 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/resources/armpolicy@v1.0.0)

## Initial beta releases

- **Client Libraries for .NET**
  - [Provisioning – API Management 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Provisioning.ApiManagement/1.0.0-beta.1)
  - [Provisioning – Batch 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Provisioning.Batch/1.0.0-beta.1)
  - [Provisioning – Compute 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Provisioning.Compute/1.0.0-beta.1)
  - [Provisioning – Monitor 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Provisioning.Monitor/1.0.0-beta.1)
  - [Provisioning – MySQL 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Provisioning.MySql/1.0.0-beta.1)
  - [Provisioning – Security Center 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Provisioning.SecurityCenter/1.0.0-beta.1)
- **Management Libraries for .NET**
  - [Resource Management – App Network 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.AppNetwork/1.0.0-beta.1)
  - [Resource Management – Domain Registration 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.DomainRegistration/1.0.0-beta.1)
  - [Resource Management – Resources.Policy 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.Resources.Policy/1.0.0-beta.1)
- **Management Libraries for Java**
  - [Resource Management – Container Registry Tasks 1.0.0-beta.1](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-containerregistry-tasks/1.0.0-beta.1)
  - [Resource Management – Service Groups 1.0.0-beta.1](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-servicegroups/1.0.0-beta.1)
- **Client Library for JavaScript**
  - [AI Speech Transcription 1.0.0-beta.1](https://www.npmjs.com/package/@azure/ai-speech-transcription/v/1.0.0-beta.1)
- **Management Libraries for JavaScript**
  - [Resource Management – Container Registry Tasks 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-containerregistrytasks/v/1.0.0-beta.1)
  - [Resource Management – Marketplace 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-marketplace/v/1.0.0-beta.1)
- **Management Libraries for Go**
  - [Resource Management – Alert Processing Rules 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/alertprocessingrules/armalertprocessingrules@v0.1.0)
  - [Resource Management – Alert Rule Recommendations 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/alertrulerecommendations/armalertrulerecommendations@v0.1.0)
  - [Resource Management – App Network 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/appnetwork/armappnetwork@v0.1.0)
  - [Resource Management – Preview Alert Rule 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/previewalertrule/armpreviewalertrule@v0.1.0)
  - [Resource Management – Prometheus Rule Groups 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/prometheusrulegroups/armprometheusrulegroups@v0.1.0)
  - [Resource Management – Tenant Activity Log Alerts 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/tenantactivitylogalerts/armtenantactivitylogalerts@v0.1.0)

## Release notes (full lists)

- [All languages](https://azure.github.io/azure-sdk/releases/2026-04/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2026-04/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2026-04/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2026-04/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2026-04/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2026-04/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2026-04/rust.html)
- [C++](https://azure.github.io/azure-sdk/releases/2026-04/cpp.html)
- [Embedded C](https://azure.github.io/azure-sdk/releases/2026-04/c.html)
- [Android](https://azure.github.io/azure-sdk/releases/2026-04/android.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2026-04/ios.html)


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-april-2026/)

