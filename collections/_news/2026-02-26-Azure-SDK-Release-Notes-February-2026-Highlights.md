---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-february-2026/
title: Azure SDK Release Notes – February 2026 Highlights
author: Ronnie Geraghty
primary_section: ai
feed_name: Microsoft Azure SDK Blog
date: 2026-02-26 16:00:26 +00:00
tags:
- .NET
- AI
- Azure
- Azure AI Content Understanding
- Azure AI Foundry
- Azure SDK
- Azure.Core
- Beta Release
- Client Libraries
- Content Analysis
- Dependency Injection
- Go
- JavaScript
- News
- OpenTelemetry
- Python
- Resource Management
- SDK
- SDK Release
- Stable Release
- Tracing
section_names:
- ai
- azure
- dotnet
---
Ronnie Geraghty summarizes the February 2026 Azure SDK release, including major library updates, new tracing features, and the launch of Azure AI Content Understanding for developers.<!--excerpt_end-->

# Azure SDK Release (February 2026)

Stay current with the latest tools and features designed to enhance your experience when developing on Azure. Below you'll find highlights from this month's Azure SDK release, along with links to further details and packages.

## Highlights

### Azure.Core 1.51.0 for .NET

- **Better Integration**: Now supports `Microsoft.Extensions.Configuration` and `Microsoft.Extensions.DependencyInjection` for enhanced integration with ASP.NET Core and .NET hosts.
- **Certificate Rotation**: Enables client certificate rotation in the transport layer, allowing dynamic token binding without the need to rebuild the entire pipeline.
- **Bug Fixes**: Addresses a `NullReferenceException` when `GetHashCode()` is called on `default(AzureLocation)`.
- **Learn more:** [Azure.Core 1.51.0 NuGet](https://www.nuget.org/packages/Azure.Core/1.51.0)

### corehttp 1.0.0b7 for Python

- **OpenTelemetry Tracing**: Adds native tracing with OpenTelemetry for Python SDK operations via a new `OpenTelemetryTracer`.
- **Configuration**: SDK clients can now set an `_instrumentation_config` variable to customize tracing.
- **Improved Exception Handling**: Better granularity for timeouts, improved retry policies, and enhanced error chaining during claims challenge issues.
- **Learn more:** [corehttp 1.0.0b7 PyPI](https://pypi.org/project/corehttp/1.0.0b7/)

### Azure AI Content Understanding in Foundry Tools (Beta)

- **New Library**: The initial beta introduces `ContentUnderstandingClient` for document, audio, and video analysis using Azure AI Foundry. The unified API helps developers extract meaningful insights from diverse content types.
- **Learn more:** [azure-ai-contentunderstanding 1.0.0b1 PyPI](https://pypi.org/project/azure-ai-contentunderstanding/1.0.0b1/)

## Additional Releases

- **JavaScript:**
  - Management Library for Resource Management – Dell-Storage ([npm 1.0.0](https://www.npmjs.com/package/@azure/arm-dell-storage/v/1.0.0))
  - Edge Actions ([1.0.0-beta.1](https://www.npmjs.com/package/@azure/arm-edgeactions/v/1.0.0-beta.1))
- **Python:**
  - Management Libraries (Dell-Storage, Resource-Deployment Stacks, various Resource Management beta libraries)
- **Go:**
  - Management Libraries (Edge Actions, Disconnected Operations, Compute Bulk Actions)

## Release Notes by Language

- [All languages](https://azure.github.io/azure-sdk/releases/2026-02/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2026-02/dotnet.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2026-02/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2026-02/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2026-02/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2026-02/rust.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2026-02/ios.html)

## How to Stay Updated

Subscribe to the [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/) or visit the [Azure SDK Releases page](https://aka.ms/azsdk/releases) for details and notifications on future releases.

---

For complete release notes and in-depth documentation, follow the links above and review the official Azure SDK documentation.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-february-2026/)
