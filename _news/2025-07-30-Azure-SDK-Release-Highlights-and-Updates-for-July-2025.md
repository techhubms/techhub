---
layout: "post"
title: "Azure SDK Release Highlights and Updates for July 2025"
description: "This post by Ronnie Geraghty shares the highlights and detailed release notes for the Azure SDK updates in July 2025. It covers updates to Azure AI Agents, Azure Storage, Key Vault, initial stable and beta releases across major languages, and provides links to comprehensive release notes."
author: "Ronnie Geraghty"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-july-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-07-30 22:41:00 +00:00
permalink: "/2025-07-30-Azure-SDK-Release-Highlights-and-Updates-for-July-2025.html"
categories: ["AI", "Azure", "Coding", "Security"]
tags: [".NET", "AI", "AI Agents", "Azure", "Azure SDK", "Azure Storage", "Certificate Management", "Cloud Health", "Coding", "Event Grid", "Go", "Java", "JavaScript", "Key Vault", "Lambda Test", "Management Libraries", "MongoDB Atlas", "News", "Playwright", "Python", "Release Notes", "Resource Management", "SDK", "Security", "System Events"]
tags_normalized: ["net", "ai", "ai agents", "azure", "azure sdk", "azure storage", "certificate management", "cloud health", "coding", "event grid", "go", "java", "javascript", "key vault", "lambda test", "management libraries", "mongodb atlas", "news", "playwright", "python", "release notes", "resource management", "sdk", "security", "system events"]
---

Ronnie Geraghty summarizes the key updates in the July 2025 Azure SDK release, detailing new features, bug fixes, stable and beta releases for languages like Python, .NET, and more.<!--excerpt_end-->

# Azure SDK Release (July 2025)

*By Ronnie Geraghty*

Thank you for your interest in the new Azure SDKs! Azure SDKs are updated every month with new features, improvements, and bug fixes. Subscribe to the [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/) to receive notifications about the latest releases.

You can always find links to packages, source code, and documentation on the [Azure SDK Releases page](https://aka.ms/azsdk/releases).

## Release Highlights

### Azure AI Agents 1.0.2 for Python

- **Update**: Addresses a critical tracing issue during process termination when messages or run steps were not fully iterated.
- **Impact**: Enhances reliability for AI agents running in production.
- [Release on PyPI](https://pypi.org/project/azure-ai-agents/1.0.2/)

### Azure Storage Libraries for Python

- **Libraries Updated**:
    - [Storage Blobs 12.26.0](https://pypi.org/project/azure-storage-blob/12.26.0/)
    - [Storage Queues 12.13.0](https://pypi.org/project/azure-storage-queue/12.13.0/)
    - [Storage Files Data Lake 12.21.0](https://pypi.org/project/azure-storage-file-datalake/12.21.0/)
    - [Storage Files Share 12.22.0](https://pypi.org/project/azure-storage-file-share/12.22.0/)
- **Highlights**: Stable releases, feature enhancements, and bug fixes.

### Azure Key Vault Libraries for Python

- **API Version Support**: Now supports API version 7.6.
- **Key Updates**:
    - [Key Vault Keys 4.11.0](https://pypi.org/project/azure-keyvault-keys/4.11.0/) includes `get_key_attestation` for retrieving key attestation blobs from managed HSMs.
    - [Key Vault Certificates 4.10.0](https://pypi.org/project/azure-keyvault-certificates/4.10.0/) introduces `preserve_order` to maintain certificate chain ordering during creation/import.

## Initial Stable Releases

### Client Libraries

- **.NET**
    - [System Events 1.0.0](https://www.nuget.org/packages/Azure.Messaging.EventGrid.SystemEvents/1.0.0)
- **Java**
    - [System Events 1.0.0](https://central.sonatype.com/artifact/com.azure/azure-messaging-eventgrid-systemevents/1.0.0)
- **JavaScript**
    - [System Events 1.0.0](https://www.npmjs.com/package/@azure/eventgrid-systemevents/v/1.0.0)
    - [AI Agents 1.0.0](https://www.npmjs.com/package/@azure/ai-agents/v/1.0.0)
    - [Health Deidentification 1.0.0](https://www.npmjs.com/package/@azure-rest/health-deidentification/v/1.0.0)
- **Go**
    - [System Events 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/messaging/eventgrid/azsystemevents@v1.0.0)

### Management Libraries

#### .NET

- [Resource Management – Lambda Test Hyper Execute 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.LambdaTestHyperExecute/1.0.0)
- [Resource Management – Storage Actions 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.StorageActions/1.0.0)
- [Resource Management – Pure Storage Block 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.PureStorageBlock/1.0.0)
- [Resource Management – Arize AI Observability Eval 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.ArizeAIObservabilityEval/1.0.0)
- [Resource Management – Carbon Optimization 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.CarbonOptimization/1.0.0)
- [Resource Management – MongoDB Atlas 1.0.0](https://www.nuget.org/packages/Azure.ResourceManager.MongoDBAtlas/1.0.0)

#### Java

- [Resource Management – Lambda Test Hyper Execute 1.0.0](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-lambdatesthyperexecute/1.0.0)
- [Resource Management – Arize AI Observability Eval 1.0.0](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-arizeaiobservabilityeval/1.0.0)
- [Resource Management – Pure Storage Block 1.0.0](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-purestorageblock/1.0.0)
- [Resource Management – MongoDB Atlas 1.0.0](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-mongodbatlas/1.0.0)
- [Resource Management – Resources – Deployment Stacks 1.0.0](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-resources-deploymentstacks/1.0.0)
- [Resource Management – Storage Actions 1.0.0](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-storageactions/1.0.0)

#### JavaScript

- [Resource Management – Pure Storage Block 1.0.0](https://www.npmjs.com/package/@azure/arm-purestorageblock/v/1.0.0)
- [Resource Management – Lambda Test Hyper Execute 1.0.0](https://www.npmjs.com/package/@azure/arm-lambdatesthyperexecute/v/1.0.0)
- [Resource Management – Storage Actions 1.0.0](https://www.npmjs.com/package/@azure/arm-storageactions/v/1.0.0)
- [Resource Management – MongoDB Atlas 1.0.0](https://www.npmjs.com/package/@azure/arm-mongodbatlas/v/1.0.0)

#### Python

- [Resource Management – Lambda Test Hyper Execute 1.0.0](https://pypi.org/project/azure-mgmt-lambdatesthyperexecute/1.0.0/)
- [Resource Management – Pure Storage Block 1.0.0](https://pypi.org/project/azure-mgmt-purestorageblock/1.0.0/)
- [Resource Management – Storage Actions 1.0.0](https://pypi.org/project/azure-mgmt-storageactions/1.0.0/)
- [Resource Management – Arize AI Observability Eval 1.0.0](https://pypi.org/project/azure-mgmt-arizeaiobservabilityeval/1.0.0/)
- [Resource Management – MongoDB Atlas 1.0.0](https://pypi.org/project/azure-mgmt-mongodbatlas/1.0.0/)
- [Resource Management – Hardware Security Modules 1.0.0](https://pypi.org/project/azure-mgmt-hardwaresecuritymodules/1.0.0/)

#### Go

- [Resource Management – Lambda Test Hyper Execute 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/lambdatesthyperexecute/armlambdatesthyperexecute@v1.0.0)
- [Resource Management – Arize AI Observability Eval 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/arizeaiobservabilityeval/armarizeaiobservabilityeval@v1.0.0)
- [Resource Management – Storage Actions 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/storageactions/armstorageactions@v1.0.0)
- [Resource Management – Pure Storage Block 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/purestorageblock/armpurestorageblock@v1.0.0)
- [Resource Management – MongoDB Atlas 1.0.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/mongodbatlas/armmongodbatlas@v1.0.0)

## Initial Beta Releases

### Client Libraries

- **.NET**: [Playwright 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Developer.Playwright/1.0.0-beta.1), [Playwright NUnit 1.0.0-beta.1](https://www.nuget.org/packages/Azure.Developer.Playwright.NUnit/1.0.0-beta.1)
- **JavaScript**: [Playwright 1.0.0-beta.1](https://www.npmjs.com/package/@azure/playwright/v/1.0.0-beta.1), [Create Playwright 1.0.0-beta.1](https://www.npmjs.com/package/@azure/create-playwright/v/1.0.0-beta.1)

### Management Libraries

- **.NET**:
    - [Connected Cache 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.ConnectedCache/1.0.0-beta.1)
    - [Cloud Health 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.CloudHealth/1.0.0-beta.1)
    - [Planetary Computer 1.0.0-beta.1](https://www.nuget.org/packages/Azure.ResourceManager.PlanetaryComputer/1.0.0-beta.1)
- **Java**:
    - [Kubernetes Configuration – Private Link Scopes 1.0.0-beta.1](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-kubernetesconfiguration-privatelinkscopes/1.0.0-beta.1)
- **JavaScript**:
    - [Planetary Computer 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-planetarycomputer/v/1.0.0-beta.1)
    - [Kubernetes Configuration – Private Link Scopes 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-kubernetesconfiguration-privatelinkscopes/v/1.0.0-beta.1)
    - [Playwright 1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-playwright/v/1.0.0-beta.1)
- **Python**:
    - [Kubernetes Configuration – Private Link Scopes 1.0.0b1](https://pypi.org/project/azure-mgmt-kubernetesconfiguration-privatelinkscopes/1.0.0b1/)
    - [Resource-Deployments 1.0.0b1](https://pypi.org/project/azure-mgmt-resource-deployments/1.0.0b1/)
    - [Container Service Safeguards 1.0.0b1](https://pypi.org/project/azure-mgmt-containerservicesafeguards/1.0.0b1/)
    - [Cloud Health 1.0.0b1](https://pypi.org/project/azure-mgmt-cloudhealth/1.0.0b1/)
    - [Resource-Bicep 1.0.0b1](https://pypi.org/project/azure-mgmt-resource-bicep/1.0.0b1/)
    - [Playwright 1.0.0b1](https://pypi.org/project/azure-mgmt-playwright/1.0.0b1/)
- **Go**:
    - [Agriculture Platform 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/agricultureplatform/armagricultureplatform@v0.1.0)
    - [Kubernetes Configuration – Private Link Scopes 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/kubernetesconfiguration/armprivatelinkscopes@v0.1.0)
    - [Deployment Safeguards 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/containerservice/armdeploymentsafeguards@v0.1.0)
    - [Playwright 0.1.0](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/playwright/armplaywright@v0.1.0)

## Release Notes by Language

- [All languages](https://azure.github.io/azure-sdk/releases/2025-07/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2025-07/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2025-07/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-07/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2025-07/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2025-07/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2025-07/rust.html)
- [C++](https://azure.github.io/azure-sdk/releases/2025-07/cpp.html)
- [Android](https://azure.github.io/azure-sdk/releases/2025-07/android.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2025-07/ios.html)

For comprehensive release notes, visit the links above for each language.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-july-2025/)
