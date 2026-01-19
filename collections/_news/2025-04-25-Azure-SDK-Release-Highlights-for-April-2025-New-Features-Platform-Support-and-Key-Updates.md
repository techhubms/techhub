---
layout: post
title: 'Azure SDK Release Highlights for April 2025: New Features, Platform Support, and Key Updates'
author: Hector Norzagaray
canonical_url: https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-april-2025/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/azure-sdk/feed/
date: 2025-04-25 17:55:05 +00:00
permalink: /coding/news/Azure-SDK-Release-Highlights-for-April-2025-New-Features-Platform-Support-and-Key-Updates
tags:
- .NET
- Azure SDK
- C++
- Embedded C
- End Of Life
- Go
- Java
- JavaScript
- Management Libraries
- Node.js
- Python
- Release Notes
- Rust
- SDK
- TypeScript
section_names:
- azure
- coding
---
Hector Norzagaray presents the April 2025 Azure SDK release update, highlighting new stable and beta libraries, essential platform advisories, and resources for further reading.<!--excerpt_end-->

# Azure SDK Release (April 2025)

**Author:** Hector Norzagaray

Thank you for your interest in the new Azure SDKs! The Azure team releases new features, improvements, and bug fixes every month. To stay updated on future releases, you can subscribe to the [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/).

For access to packages, code, and documentation, visit the [Azure SDK Releases page](https://aka.ms/azsdk/releases).

---

## Release Highlights

### Node.js 18 End of Life

According to the official [Node.js release timeline](https://nodejs.org/about/previous-releases), Node.js 18 is scheduled to reach its end-of-life on **April 30, 2025**. After this date, it will no longer receive security updates and critical bug fixes.

**Recommendations:**

- Upgrade to a supported Node.js version to continue receiving support and updates.
- The Azure SDK libraries are not guaranteed to function on platforms or with dependencies that have reached end-of-life. Dropping support for deprecated dependencies may occur without increasing the Azure SDK's major version.
- Migrate to supported platforms and dependencies for ongoing technical support.

For extended guidance, read the [Azure SDK Blog post on Node.js 18 end of support](https://devblogs.microsoft.com/azure-sdk/announcing-the-end-of-support-for-node-js-18-x-in-the-azure-sdk-for-javascript/).

---

### Initial Stable Releases

- **Management Library for .NET**
- **Recovery Services Data Replication**

### Initial Beta Releases

- **Client Library for Rust:**
  - [Storage Blob](https://crates.io/crates/azure_storage_blob)

- **Management Library for .NET:**
  - Agriculture Platform
  - Database Watcher
  - Migration Assessment

- **Management Library for Go:**
  - Arize AI Observability
  - Durable Task
  - Weights and Biases

- **Management Library for Java:**
  - Arize AI Observability
  - Durable Task
  - Weights and Biases

- **Management Library for JavaScript:**
  - Agriculture Platform
  - Arize AI Observability
  - Durable Task
  - Migration Assessment
  - Weights and Biases

- **Management Library for Python:**
  - Arize AI Observability
  - Durable Task
  - Weights and Biases

---

## Release Notes

Release notes are available for every supported language and platform:

- [All Languages Overview](https://azure.github.io/azure-sdk/releases/2025-04/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2025-04/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2025-04/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-04/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2025-04/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2025-04/go.html)
- [Rust](https://azure.github.io/azure-sdk/releases/2025-04/rust.html)
- [C++](https://azure.github.io/azure-sdk/releases/2025-04/cpp.html)
- [Embedded C](https://azure.github.io/azure-sdk/releases/2025-04/c.html)
- [Android](https://azure.github.io/azure-sdk/releases/2025-04/android.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2025-04/ios.html)

For in-depth coverage and ongoing updates, readers are encouraged to browse the relevant release notes for their platform and language of interest.

---

## Additional Resources

- [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk) for announcements, best practices, and technical guidance.
- [Azure SDK Releases](https://aka.ms/azsdk/releases) for the latest packages and documentation.

---

Stay tuned for next month's Azure SDK updates!

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-april-2025/)
