---
layout: "post"
title: "Azure SDK Release Highlights and Notes for February 2025"
description: "This post covers the February 2025 Azure SDK release highlights, including notable version updates, platform support notes, initial stable and beta releases for multiple languages, and essential migration recommendations. It provides release notes and resources for developers using the Azure SDK."
author: "Hector Norzagaray"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-february-2025/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-02-26 19:09:17 +00:00
permalink: "/2025-02-26-Azure-SDK-Release-Highlights-and-Notes-for-February-2025.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", "Android", "Azure", "Azure SDK", "C++", "Coding", "Compute Schedule", "Dependency Management", "DevOps", "Embedded C", "Go", "Ios", "Java", "JavaScript", "Library", "News", "Node.js", "Open Telemetry", "Pinecone Vector DB", "Platform Support", "Python", "SDK"]
tags_normalized: ["dotnet", "android", "azure", "azure sdk", "cplusplus", "coding", "compute schedule", "dependency management", "devops", "embedded c", "go", "ios", "java", "javascript", "library", "news", "nodedotjs", "open telemetry", "pinecone vector db", "platform support", "python", "sdk"]
---

In this news update, Hector Norzagaray summarizes the key highlights and notes from the February 2025 Azure SDK release, including support advisories, new stable and beta library releases, and links to in-depth documentation.<!--excerpt_end-->

# Azure SDK Release – February 2025

_Author: Hector Norzagaray_

Thank you for your interest in the new Azure SDKs! We release new features, improvements, and bug fixes every month. To stay up to date with the latest releases:

- **Subscribe:** [Azure SDK Blog RSS Feed](https://devblogs.microsoft.com/azure-sdk/feed/)
- **Documentation & Downloads:** Find all packages, code, and docs on the [Azure SDK Releases page](https://aka.ms/azsdk/releases).

## Share Your Feedback

If you’ve been using the Azure SDK, feedback is appreciated. Take a moment to [complete our feedback survey](https://aka.ms/azsdk/blog-survey).

## Release Highlights

- **Node.js 18 End-of-Life**: Per the [official Node.js release timeline](https://nodejs.org/about/previous-releases), Node.js 18 will reach its official end-of-life on April 30, 2025. After this date, security updates and critical bug fixes will cease. **Upgrading to a supported Node.js version is strongly recommended to maintain platform support.**

- **Support Policy:** Azure SDK libraries are not guaranteed to work on platforms or dependencies which are end-of-life. Dropping support for those may occur without a major version increment. Migrate to supported platforms and dependencies to ensure continued technical support.

### Initial Stable Releases

- **Management Library for .NET**
  - Compute Schedule
  - IoT Operations
- **Management Library for Go**
  - Compute Schedule
- **Management Library for Java**
  - Compute Schedule
- **Client Library for Java**
  - HTTPClient – JDK
  - HTTPClient – VertX
- **Management Library for JavaScript**
  - Compute Schedule
- **Management Library for Python**
  - Compute Schedule

### Initial Beta Releases

- **Client Library for Java**
  - Open Telemetry Auto Configure
- **Management Library for Go, Java, JavaScript, Python**
  - Pinecone Vector DB

## Release Notes

- [All languages](https://azure.github.io/azure-sdk/releases/2025-02/index.html)
- [.NET](https://azure.github.io/azure-sdk/releases/2025-02/dotnet.html)
- [Java](https://azure.github.io/azure-sdk/releases/2025-02/java.html)
- [JavaScript/TypeScript](https://azure.github.io/azure-sdk/releases/2025-02/js.html)
- [Python](https://azure.github.io/azure-sdk/releases/2025-02/python.html)
- [Go](https://azure.github.io/azure-sdk/releases/2025-02/go.html)
- [C++](https://azure.github.io/azure-sdk/releases/2025-02/cpp.html)
- [Embedded C](https://azure.github.io/azure-sdk/releases/2025-02/c.html)
- [Android](https://azure.github.io/azure-sdk/releases/2025-02/android.html)
- [iOS](https://azure.github.io/azure-sdk/releases/2025-02/ios.html)

For more granular information on each language-specific release, please refer to the links above. These resources contain detailed change logs, breaking changes, added features, and bug fixes for the February 2025 cycle.

---

_Keep your SDKs and platform dependencies up to date to benefit from improvements and continued support. For questions or issues, please use the official feedback channels._

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-sdk-release-february-2025/)
