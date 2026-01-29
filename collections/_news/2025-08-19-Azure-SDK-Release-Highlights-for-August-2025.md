---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-august-2025/
title: Azure SDK Release Highlights for August 2025
author: Ronnie Geraghty
feed_name: Microsoft DevBlog
date: 2025-08-19 18:43:38 +00:00
tags:
- .NET
- AI Projects
- Azure AI Foundry
- Azure OpenAI Service
- Azure SDK
- Carbon Optimization
- Client Libraries
- Data Movement
- Go
- JavaScript
- Management Libraries
- Monitor Query Logs
- Monitor Query Metrics
- Python
- Recovery Services
- Release Notes
- Resource Management
- Rust
- SDK
- Storage Discovery
- AI
- Azure
- Coding
- News
section_names:
- ai
- azure
- coding
primary_section: ai
---
Ronnie Geraghty announces the latest Azure SDK updates, highlighting new AI Projects libraries, data movement improvements for .NET, and resource management tools for multiple programming languages. This release is focused on practical features for Azure developers.<!--excerpt_end-->

# Azure SDK Release Highlights for August 2025

Each month, Azure SDK publishes new features, enhancements, and fixes across its core client and management libraries, supporting a range of use cases in AI, data movement, monitoring, and resource management.

## Release Highlights

### AI Projects Libraries 1.0.0 (Stable)

- **Languages:** JavaScript ([npm](https://www.npmjs.com/package/@azure/ai-projects/v/1.0.0)), Python ([PyPI](https://pypi.org/project/azure-ai-projects/1.0.0/))
- **Overview:** General availability for AI Projects libraries supporting Azure AI Foundry workflows. Features include:
  - Project management for Azure AI solutions
  - Authentication support for Azure services
  - Integration with Azure OpenAI Service
  - Separation of preview features (Evaluations, Red Team operations) into beta APIs for enhanced stability

### Storage Data Movement 12.2.0 (.NET)

- [NuGet Package](https://www.nuget.org/packages/Azure.Storage.DataMovement/12.2.0)
- Improved file and directory transfers with robust handling of special and URL-encoded characters in file names, addressing bugs and improving reliability for operations across Azure Storage services.

## Initial Stable Releases by Language

**Java**

- [Monitor Query Logs](https://central.sonatype.com/artifact/com.azure/azure-monitor-query-logs/1.0.0)
- [Monitor Query Metrics](https://central.sonatype.com/artifact/com.azure/azure-monitor-query-metrics/1.0.0)
- Resource Management: [Carbon Optimization](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-carbonoptimization/1.0.0), [Recovery Services Data Replication](https://central.sonatype.com/artifact/com.azure.resourcemanager/azure-resourcemanager-recoveryservicesdatareplication/1.0.0)

**JavaScript**

- [Monitor Query Logs](https://www.npmjs.com/package/@azure/monitor-query-logs/v/1.0.0)
- [Monitor Query Metrics](https://www.npmjs.com/package/@azure/monitor-query-metrics/v/1.0.0)
- [AI Projects](https://www.npmjs.com/package/@azure/ai-projects/v/1.0.0)
- Resource Management: [Carbon Optimization](https://www.npmjs.com/package/@azure/arm-carbonoptimization/v/1.0.0), [Recovery Services Data Replication](https://www.npmjs.com/package/@azure/arm-recoveryservicesdatareplication/v/1.0.0)

**Python**

- [Monitor Query Metrics](https://pypi.org/project/azure-monitor-querymetrics/1.0.0/)
- [AI Projects](https://pypi.org/project/azure-ai-projects/1.0.0/)
- Resource Management: [Carbon Optimization](https://pypi.org/project/azure-mgmt-carbonoptimization/1.0.0/), [Recovery Services Data Replication](https://pypi.org/project/azure-mgmt-recoveryservicesdatareplication/1.0.0/)

**Go**

- Resource Management: [Carbon Optimization](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/carbonoptimization/armcarbonoptimization@v1.0.0), [Recovery Services Data Replication](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/recoveryservicesdatareplication/armrecoveryservicesdatareplication@v1.0.0)

## Beta Releases

- **Rust:** [Core – Macros](https://crates.io/crates/azure_core_macros/0.1.0), [Core – OpenTelemetry](https://crates.io/crates/azure_core_opentelemetry/0.1.0)
- **.NET:** [Resource Management – Playwright](https://www.nuget.org/packages/Azure.ResourceManager.Playwright/1.0.0-beta.1), [Resource Management – Storage Discovery](https://www.nuget.org/packages/Azure.ResourceManager.StorageDiscovery/1.0.0-beta.1)
- **JavaScript:** [Resource Management – Storage Discovery](https://www.npmjs.com/package/@azure/arm-storagediscovery/v/1.0.0-beta.1)
- **Python:** [Resource Management – Storage Discovery](https://pypi.org/project/azure-mgmt-storagediscovery/1.0.0b1/)
- **Go:** [Resource Management – Storage Discovery](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/storagediscovery/armstoragediscovery@v0.1.0), [Deployments](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/resourcemanager/resources/armdeployments@v0.1.0)

## Release Notes and Documentation

- [All languages](https://azure.github.io/azure-sdk/releases/2025-08/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2025-08/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2025-08/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-08/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2025-08/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2025-08/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2025-08/rust.html)
- [C++](https://azure.github.io/azure-sdk/releases/2025-08/cpp.html)

## Resources

- [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/)
- [Azure SDK Releases page](https://aka.ms/azsdk/releases)

For the latest information, ongoing release notes, and code samples, check the [Azure SDK Release (August 2025) blog post](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-august-2025/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-august-2025/)
