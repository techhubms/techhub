---
layout: "post"
title: "Azure SDK Release Highlights – March 2025"
description: "This post from Hector Norzagaray details the Azure SDK releases for March 2025, featuring the beta launch of the official Azure SDK for Rust, updates on Node.js 18 end of life, new Python (Conda) packages, and initial stable and beta releases for various languages. Release notes and feedback channels are included."
author: "Hector Norzagaray"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-march-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-03-27 17:00:03 +00:00
permalink: "/news/2025-03-27-Azure-SDK-Release-Highlights-March-2025.html"
categories: ["Azure", "Coding"]
tags: [".NET", "Azure", "Azure SDK", "C++", "Coding", "Conda", "Cosmos DB", "Database Watcher", "Device Registry", "Event Hubs", "Go", "Identity", "Java", "JavaScript", "Key Vault", "Language Authoring", "Management Library", "News", "Node.js 18", "OpenTelemetry", "Python", "Rust", "SDK", "SDK Release"]
tags_normalized: ["dotnet", "azure", "azure sdk", "cplusplus", "coding", "conda", "cosmos db", "database watcher", "device registry", "event hubs", "go", "identity", "java", "javascript", "key vault", "language authoring", "management library", "news", "nodedotjs 18", "opentelemetry", "python", "rust", "sdk", "sdk release"]
---

Authored by Hector Norzagaray, this March 2025 Azure SDK release post highlights a major beta launch for Rust developers, updates across numerous programming languages, guidance regarding Node.js 18, and more.<!--excerpt_end-->

# Azure SDK Release Highlights – March 2025

*Author: Hector Norzagaray*

Thank you for your interest in the new Azure SDKs! Each month, Azure releases new features, improvements, and bug fixes across its SDK offerings for developers. If you’d like to stay updated on these enhancements, you can subscribe to the [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/).

All package links, code, and documentation are available on the [Azure SDK Releases page](https://aka.ms/azsdk/releases).

## Feedback Invitation

If you’re actively using the Azure SDK, the team encourages feedback via their [Developer Survey](https://aka.ms/azsdk/blog-survey).

## Release Highlights

### 🚀 Beta Launch: Azure SDK for Rust

- **First official beta releases**: Azure now offers SDKs for the Rust programming language.
- **Supported libraries** (in beta):
  - [Identity](https://crates.io/crates/azure_identity)
  - [Key Vault Secrets](https://crates.io/crates/azure_security_keyvault_secrets)
  - [Key Vault Keys](https://crates.io/crates/azure_security_keyvault_keys)
  - [Event Hubs](https://crates.io/crates/azure_messaging_eventhubs)
  - [Cosmos DB](https://crates.io/crates/azure_data_cosmos)
- **Purpose & Goals**:
  - Provide a seamless, idiomatic Rust experience for Azure service integration.
  - Leverage Rust’s performance and safety features for robust, system-level applications.
  - Expand and refine Azure SDK support for Rust as developer needs evolve.
- **Get Involved**: Feedback, issues, and contributions are welcomed via the [GitHub issues page](https://github.com/Azure/azure-sdk-for-rust/issues).
- For more detail, read the [dedicated blog post](https://devblogs.microsoft.com/azure-sdk/rust-in-time-announcing-the-azure-sdk-for-rust-beta/).

### Node.js 18 End of Life

- **Timeline**: Node.js 18 reaches official end of life on April 30, 2025.
- **Impact**: After this date, Node.js 18 will no longer receive security or critical updates.
- **Recommendation**:
  - Upgrade to a supported Node.js version.
  - Azure SDK libraries may drop support at any time for end-of-life platforms, even without a major version change.
  - Ensure migration to supported dependencies to maintain technical support eligibility.
- See the [Node.js release timeline](https://nodejs.org/about/previous-releases) for details.

### New Conda Releases

- New Python (Conda) packages are available via the [Microsoft channel on Anaconda](https://anaconda.org/microsoft).

### Stable Releases

- **Client Library for Java**
  - OpenTelemetry AutoConfigure
  - Device Registry
- **Management Library releases** (stable):
  - .NET
  - Go
  - JavaScript
  - All include Device Registry functionality

### Beta Releases (Initial)

- **Client Library for .NET**
  - Language Conversation Authoring
  - Language Text Authoring
- **Client Library for Java**
  - System Events
- **Client Library for Rust**
  - Identity
  - Key Vault Secrets
  - Key Vault Keys
  - Event Hubs
  - Cosmos DB
- **Management Libraries (Beta) for Go, Java, JavaScript, Python**
  - Database Watcher
  - Impact Reporting
  - Migration Assistant (Go/Java), Assessment (Python)
  - Timezone (Python)

### Release Notes and Documentation

Comprehensive release notes are available by language and platform:

- [All languages](https://azure.github.io/azure-sdk/releases/2025-03/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2025-03/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2025-03/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-03/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2025-03/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2025-03/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2025-03/rust.html)
- [C++](https://azure.github.io/azure-sdk/releases/2025-03/cpp.html)
- [Embedded C](https://azure.github.io/azure-sdk/releases/2025-03/c.html)
- [Android](https://azure.github.io/azure-sdk/releases/2025-03/android.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2025-03/ios.html)

---

For the latest updates and announcements, follow the [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-march-2025/)
