---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-november-2025/
title: 'Azure SDK November 2025 Release: Storage, Identity, Cosmos DB, and New Libraries'
author: Ronnie Geraghty
feed_name: Microsoft Azure SDK Blog
date: 2025-11-25 22:56:34 +00:00
tags:
- .NET
- Agent Server
- AI
- AI Agents
- AKS
- Azure Cosmos DB
- Azure Identity
- Azure SDK
- Azure Storage
- C#
- C++
- Certificate Management
- Confidential Ledger
- DNS
- Front Door
- Go
- Java
- JavaScript
- Managed Identity
- Python
- Resource Management
- SDK
- WebPubSub
section_names:
- azure
- coding
---
Ronnie Geraghty summarizes the new features and improvements in the Azure SDK November 2025 release, including updates for Storage, Identity, Cosmos DB, and a range of new and beta libraries for .NET, Java, Python, and more.<!--excerpt_end-->

# Azure SDK November 2025 Release Overview

**Author: Ronnie Geraghty**

Each month, Microsoft ships updates and new features to the Azure SDK, making it easier for developers to build and manage modern cloud applications. The November 2025 release introduces major enhancements across storage, identity, and data platforms. This summary highlights key changes and provides reference links for deeper exploration.

## Storage Library Updates

Significant stable updates have been shipped for Azure Storage libraries in .NET, Java, C++, Go, and other languages, all supporting service version 2025-11-05. Notable improvements include:

- Enhanced error reporting for blob copy operations in .NET (v12.26.0) and Java (v12.32.0)
- Improved message counting in Storage Queues for .NET and Java
- Files Shares libraries now support bearer token challenges and bug fixes
- C++ and Go Azure Storage libraries updated to the latest version, supporting new APIs across blobs, files, and queues

Full package and documentation links are available on the [Azure SDK Releases page](https://aka.ms/azsdk/releases).

## AKS Identity Binding Support in Azure Identity Libraries

To address scale limitations of federated identity credentials in Azure Kubernetes Service (AKS), the Azure Identity libraries now introduce support for [Identity Binding](https://learn.microsoft.com/azure/aks/identity-bindings-concepts). This makes it easier and more scalable to use user-assigned managed identities (UAMI) across clusters in .NET (from v1.18.0-beta.2), Go, Java, JavaScript, and Python, with new settings to enable the feature for each language SDK:

- .NET: `WorkloadIdentityCredentialOptions.IsAzureKubernetesTokenProxyEnabled`
- Go: `WorkloadIdentityCredentialOptions.EnableAzureTokenProxy`
- Java: `enableAzureTokenProxy()` on builder
- JavaScript: `enableAzureKubernetesTokenProxy` in options
- Python: `use_token_proxy=True`

This expands authentication options and operational simplicity for large-scale Kubernetes workloads.

## Cosmos DB SDK Enhancements

- **Java SDK (v4.75.0):** Dynamic per-partition failover, better exception handling, and Change Feed Processor improvements
- **Python SDK (v4.14.0):** Semantic reranking (preview), new `return_properties` parameter for database/container proxies

## New and Beta Client/Management Libraries

- **.NET: Client/Management Libraries** for Storage Discovery, Mongo Cluster, Durable Task, Recovery Services, SocketIO WebPubSub
- **.NET: Provisioning** for Front Door and DNS
- **AI/Agent Server:** Beta agent SDKs for .NET, Java, and Python
- **Java/Python:** New AI Agents, Voice Live (Java), Confidential Ledger Certificate, and Planetary Computer (Python)

## Reference: Release Notes and Language Guides

- [All languages](https://azure.github.io/azure-sdk/releases/2025-11/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2025-11/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2025-11/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-11/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2025-11/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2025-11/go.html)
- [C++](https://azure.github.io/azure-sdk/releases/2025-11/cpp.html)

For a detailed look at package versions, code, and doc links, visit the official [Azure SDK Releases page](https://aka.ms/azsdk/releases).

---

**Learn More**

- [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk)
- Microsoft Docs: [Azure SDK](https://docs.microsoft.com/azure/developer/)

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-november-2025/)
