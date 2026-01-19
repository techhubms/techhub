---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-september-2025/
title: Azure SDK Release Highlights – September 2025
author: Ronnie Geraghty
viewing_mode: external
feed_name: Microsoft Azure SDK Blog
date: 2025-10-02 15:00:17 +00:00
tags:
- .NET
- API Changes
- Azure Batch
- Azure Developer Tools
- Azure Identity
- Azure Playwright
- Azure SDK
- Beta Release
- Client Libraries
- Go
- Java
- JavaScript
- Long Running Operations
- Management Libraries
- Python
- Resource Management
- Rust
- SDK
- SDK Release
- Stable Release
section_names:
- azure
- coding
---
Ronnie Geraghty provides an overview of the September 2025 Azure SDK releases, highlighting key updates, new libraries, and where to find detailed release notes for developers across multiple languages.<!--excerpt_end-->

# Azure SDK Release Highlights – September 2025

_Author: Ronnie Geraghty_

Each month, Microsoft releases updates for the Azure SDKs to provide developers with new features, platform support, and performance improvements. The September 2025 update includes both stable and beta releases across .NET, Java, JavaScript/TypeScript, Python, Go, and Rust.

## Key Release Highlights

### Azure Batch 1.0.0-beta.5 for Java

- Major model renaming for clarity: `-Content` → `-Parameters`.
- Updated core classes for Azure SDK naming consistency.
- Introduces support for Long-Running Operations (LROs), enabling extended operations like `beginDeleteJob`, `beginDeallocateNode`, and `beginStopPoolResize`.

### Azure Identity 1.18.0 for Java

- Adds claims challenge support to `AzureDeveloperCliCredential`.
- Enhanced error handling for `AzurePowerShellCredential` and `AzureCliCredential`.
- Introduces `requireEnvVars()` to enforce environment variable presence at build time.
- Fixes issues with XML header parsing and hanging timeout scenarios.

### Azure Playwright 1.0.0 Initial Stable Release

- Provides browser automation testing in Azure environments.
- Available as client libraries for .NET and JavaScript.
- Management libraries released for .NET, Go, Java, JavaScript, and Python.
- Includes Playwright NUnit for .NET projects.
- Example package links:
    - [Azure.Developer.Playwright for .NET](https://www.nuget.org/packages/Azure.Developer.Playwright/1.0.0)
    - [@azure/playwright for JavaScript](https://www.npmjs.com/package/@azure/playwright/v/1.0.0)
    - [Azure.ResourceManager.Playwright for .NET](https://www.nuget.org/packages/Azure.ResourceManager.Playwright/1.0.0)
    - [azure-mgmt-playwright for Python](https://pypi.org/project/azure-mgmt-playwright/1.0.0/)

## Notable Initial Stable Releases

- Playwright client/management libraries across major languages.
- Resource manager libraries for Playwright, Fabric, and Workload Orchestration.

## Notable Beta Releases

- **AI VoiceLive** 1.0.0-beta.1 (.NET, Python): SDKs for voice-based AI scenarios.
- **AI Language Conversations Authoring** 1.0.0b1 (Python): Conversational AI authoring.
- **Resource Management Libraries**:
    - Workload Orchestration, Cloud Health, Azure Stack HCI VM, Bicep, Compute Recommender in Go, Java, JavaScript, and Python.
    - Container Service Safeguards and Resources Bicep for Java and JavaScript.
    - EventHubs Checkpoint Store for Rust.

## Release Notes and Further Information

- [All languages overview](https://azure.github.io/azure-sdk/releases/2025-09/index.html)
- [Detailed .NET release notes](https://azure.github.io/azure-sdk/releases/2025-09/dotnet.html)
- [Detailed Java release notes](https://azure.github.io/azure-sdk/releases/2025-09/java.html)
- [JavaScript/TypeScript release notes](https://azure.github.io/azure-sdk/releases/2025-09/js.html)
- [Python release notes](https://azure.github.io/azure-sdk/releases/2025-09/python.html)
- [Go release notes](https://azure.github.io/azure-sdk/releases/2025-09/go.html)
- [Rust release notes](https://azure.github.io/azure-sdk/releases/2025-09/rust.html)
- [C++ release notes](https://azure.github.io/azure-sdk/releases/2025-09/cpp.html)

## How to Stay Updated

- Subscribe to the [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/) for the latest updates.
- Explore all available packages and documentation on the [Azure SDK Releases page](https://aka.ms/azsdk/releases).

---

For detailed API references, migration details, and upgrade strategies, visit the respective language-specific release notes above.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-september-2025/)
