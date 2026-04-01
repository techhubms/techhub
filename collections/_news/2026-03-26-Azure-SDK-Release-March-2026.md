---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-march-2026/
author: Ronnie Geraghty
feed_name: Microsoft Azure SDK Blog
section_names:
- ai
- azure
- dotnet
date: 2026-03-26 20:41:06 +00:00
primary_section: ai
title: Azure SDK Release (March 2026)
tags:
- .NET
- AI
- Artifact Signing
- Audio Analysis
- Azure
- Azure AI Content Understanding
- Azure Cosmos DB
- Azure Identity
- Azure Resource Manager
- Azure SDK
- Azure.Identity
- ClientCertificateCredential
- ContentUnderstandingClient
- CosmosClientBuilder
- Document Analysis
- Fault Injection
- General Availability
- Go SDK
- JavaScript SDK
- Macos Keychain
- Multi Region Writes
- News
- npm
- NuGet
- Provisioning Libraries
- PyPI
- Python SDK
- Rust
- SDK
- Service Fabric Managed Clusters
- Transactional Batch
- Video Analysis
- Windows Certificate Store
---

Ronnie Geraghty summarizes the March 2026 Azure SDK updates, highlighting Azure Identity 1.19.0 for .NET, Cosmos DB 0.31.0 for Rust, and the GA release of Azure AI Content Understanding 1.0.0, plus a roundup of new stable and beta client/management libraries across .NET, JavaScript, Python, Go, and more.<!--excerpt_end-->

# Azure SDK Release (March 2026)

Thank you for your interest in the new Azure SDKs. New features, improvements, and bug fixes are released every month.

- RSS feed: [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/)
- Release hub: [Azure SDK Releases page](https://aka.ms/azsdk/releases)

## Release highlights

## Azure Identity 1.19.0 for .NET

Package: [Azure.Identity 1.19.0](https://www.nuget.org/packages/Azure.Identity/1.19.0)

The Azure Identity library for .NET now supports specifying a certificate path in the form:

- `cert:/StoreLocation/StoreName/Thumbprint`

This is for use with `ClientCertificateCredential` and allows referencing a certificate directly from the platform certificate store (for example, the Windows Certificate Store or the macOS KeyChain) instead of requiring a certificate file on disk.

Example path (certificate from the “My” store in the “CurrentUser” location):

- `cert:/CurrentUser/My/E661583E8FABEF4C0BEF694CBC41C28FB81CD870`

## Cosmos DB 0.31.0 for Rust

Crate: [azure_data_cosmos 0.31.0](https://crates.io/crates/azure_data_cosmos/0.31.0)

The Azure Cosmos DB client library for Rust adds several significant features and includes breaking changes.

New capabilities:

- Basic multi-region writes support
- Transactional batch support (execute multiple operations atomically within the same partition key)
- Fault injection support for testing in disaster scenarios

API updates:

- Client construction API redesigned around a new `CosmosClientBuilder`
- Query methods now return a `FeedItemIterator<T>` that implements `Stream<Item = Result<T>>`

Platform support note:

- `wasm32-unknown-unknown` support was removed across the Rust SDK

## Content Understanding 1.0.0 for .NET

Package: [Azure.AI.ContentUnderstanding 1.0.0](https://www.nuget.org/packages/Azure.AI.ContentUnderstanding/1.0.0)

Azure AI Content Understanding reaches general availability for .NET, JavaScript, and Python.

This library provides a `ContentUnderstandingClient` for:

- Analyzing documents, audio, and video content
- Creating, managing, and configuring analyzers

.NET-specific updates called out in this release:

- Strongly typed `Value` properties on `ContentField` subclasses
- A `ContentSource` hierarchy for strongly typed parsing of grounding source strings
- `ContentRange` value type with static factory methods for specifying content ranges

## Initial stable releases

- Client Libraries for .NET
  - [Content Understanding 1.0.0](https://www.nuget.org/packages/Azure.AI.ContentUnderstanding/1.0.0)
  - [Voice Live 1.0.0](https://www.nuget.org/packages/Azure.AI.VoiceLive/1.0.0)
  - [Provisioning – Network 1.0.0](https://www.nuget.org/packages/Azure.Provisioning.Network/1.0.0)
- Client Library for JavaScript
  - [Content Understanding 1.0.0](https://www.npmjs.com/package/@azure/ai-content-understanding/v/1.0.0)
- Client Library for Python
  - [Azure Content Understanding in Foundry Tools 1.0.0](https://pypi.org/project/azure-ai-contentunderstanding/1.0.0/)
- Management Library for .NET
  - [Resource Management – Disconnected Operations 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.DisconnectedOperations/1.0.0)
- Management Libraries for Go
  - [Resource Management – Disconnected Operations 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/disconnectedoperations/armdisconnectedoperations@v1.0.0)
  - [Resource Management – Service Fabric Managed Clusters 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/servicefabricmanagedclusters/armservicefabricmanagedclusters@v1.0.0)
- Management Libraries for JavaScript
  - [Resource Management – Artifact Signing 1.0.0](https://www.npmjs.com/package/@azure/arm-artifactsigning/v/1.0.0)
  - [Resource Management – Disconnected Operations 1.0.0](https://www.npmjs.com/package/@azure/arm-disconnectedoperations/v/1.0.0)
  - [Resource Management – Service Fabric Managed Clusters 1.0.0](https://www.npmjs.com/package/@azure/arm-servicefabricmanagedclusters/v/1.0.0)
- Management Libraries for Python
  - [Resource Management – Artifact Signing 1.0.0](https://pypi.org/project/azure-mgmt-artifactsigning/1.0.0/)
  - [Resource Management – Disconnected Operations 1.0.0](https://pypi.org/project/azure-mgmt-disconnectedoperations/1.0.0/)

## Initial beta releases

- Client Library for .NET
  - [Provisioning – CDN 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Provisioning.Cdn/1.0.0-beta.1)
- Management Libraries for .NET
  - [Resource Management – Certificate Registration 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.CertificateRegistration/1.0.0-beta.1)
  - [Resource Management – Managed Ops 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.ManagedOps/1.0.0-beta.1)
  - [Resource Management – Container Registry.Tasks 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.ContainerRegistry.Tasks/1.0.0-beta.1)
- Management Libraries for Go
  - [Resource Management – Artifact Signing 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/artifactsigning/armartifactsigning@v0.1.0)
  - [Resource Management – Domain Registration 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/domainregistration/armdomainregistration@v0.1.0)
  - [Resource Management – Certificate Registration 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/certificateregistration/armcertificateregistration@v0.1.0)
- Management Libraries for JavaScript
  - [Resource Management – Managed Ops 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-managedops/v/1.0.0-beta.1)
  - [Resource Management – Compute Bulk Actions 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-computebulkactions/v/1.0.0-beta.1)
  - [Resource Management – Disconnected Operations 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-disconnectedoperations/v/1.0.0-beta.1)
- Management Libraries for Python
  - [Resource Management – Managed Ops 1.0.0b1](https://pypi.org/project/azure-mgmt-managedops/1.0.0b1/)
  - [Resource Management – Compute Bulk Actions 1.0.0b1](https://pypi.org/project/azure-mgmt-computebulkactions/1.0.0b1/)

## Release notes

- [All languages](https://azure.github.io/azure-sdk/releases/2026-03/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2026-03/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2026-03/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2026-03/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2026-03/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2026-03/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2026-03/rust.html)
- [C++](https://azure.github.io/azure-sdk/releases/2026-03/cpp.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2026-03/ios.html)


[Read the entire article](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-march-2026/)

