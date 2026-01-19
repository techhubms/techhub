---
external_url: https://devblogs.microsoft.com/azure-sdk/azure-toolkit-for-intellij-introducing-the-enhanced-java-code-quality-analyzer/
title: Azure Toolkit for IntelliJ Brings Enhanced Java Code Quality Analyzer for Azure SDK Development
author: Sameeksha Vaity
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-04-18 17:49:56 +00:00
tags:
- Asynchronous Programming
- Authentication
- Azure SDK
- Azure Toolkit
- Best Practices
- Code Quality
- Event Hub
- IntelliJ
- Java
- Microsoft Entra ID
- OpenAI
- Performance
- Reactive Streams
- Service Bus
- Storage
section_names:
- azure
- coding
- security
---
Sameeksha Vaity details the latest enhancements to the Azure Toolkit for IntelliJ, highlighting a powerful Java Code Quality Analyzer aimed at improving code quality, security, and performance for developers working with the Azure SDK.<!--excerpt_end-->

# Azure Toolkit for IntelliJ: Enhanced Java Code Quality Analyzer

**Author:** Sameeksha Vaity

## Overview

A significant update to the Azure Toolkit for IntelliJ has been announced, featuring a comprehensive enhancement to the Java Code Quality Analyzer. This update introduces robust, categorized rule sets and best practices advice for developers working with Azure SDK for Java client libraries within IntelliJ IDEA. These enhancements aim to help developers write cleaner, safer, and more efficient Java code that aligns with modern development standards and Azure service requirements.

---

## What's New?

### Azure SDK for Java Integration

- **Rule Sets & Real-Time Suggestions:** The toolkit now delivers context-sensitive suggestions as you write code, focusing on secure authentication, performance, and modern API conventions.
- **Simplified Authentication Flows:** Guides developers towards using secure credentials such as `DefaultAzureCredential` and `ManagedIdentityCredential`, integrating seamlessly with the Azure Identity library (`com.azure:azure-identity`).
- **Performance & Reliability Improvements:** Recommends performant APIs like `SyncPoller` and `EventProcessorClient` to optimize library usage by reducing latency and resource consumption.
- **API Pattern Recommendations:** Suggests modern practices for API calls, efficient async operations, and helps avoid complex or inefficient reactive chains.

**Availability:** Features roll out starting with Azure Toolkit for IntelliJ version 3.95.0.

### Storage Improvements

- **Memory Usage Detection:** Flags improper Storage Upload API usage—specifically, missing length parameters—which can result in memory leaks or performance bottlenecks. Explicit specification of the length parameter or using `BlockBlobClient` for large uploads is recommended.

### Identity & Security Enhancements

- **Hardcoded API Key Detection:** Identifies hardcoded API keys and recommends transitioning to `DefaultAzureCredential` for secure Microsoft Entra ID authentication.
- **Connection String Warnings:** Warns against using connection strings for authentication, suggesting migration to Entra ID credentials for better security.

### Asynchronous Programming Best Practices

- **Polling Optimization:** Suggests using `SyncPoller` for synchronous polling instead of more complex alternatives like `PollerFlux#getSyncPoller()`.
- **Blocking Call Detection:** Flags blocking calls on asynchronous methods, urging the adoption of non-blocking programming paradigms for scalability and performance.
- **Reactive Streams Optimization:** Highlights areas to improve performance by efficiently chaining operators (e.g., `block`, `subscribe`).
- **Timeout Best Practices:** Identifies async calls missing timeout configurations and recommends APIs like `timeout(Duration)` to prevent hanging operations and improve responsiveness.

### Messaging (AMQP & Service Bus) Recommendations

- **Service Bus Client Advisory:** Recommends migrating from `ServiceBusReceiverAsyncClient` to `ServiceBusProcessorClient` for better concurrency and error handling.
- **Event Hub Client Advisory:** Encourages using `EventProcessorClient` over `EventHubConsumerAsyncClient` for enhanced load balancing and reliability.
- **Autocomplete Cautions:** Flags cases where message autocomplete is enabled by default and suggests disabling it (using `disableAutoComplete()`) to ensure safe, explicit message acknowledgment and to prevent accidental data loss.
- **Checkpoint Management:** Promotes the use of `EventProcessorClient` for easier checkpoint management and discourages suboptimal `updateCheckpointAsync` patterns.

### General Best Practices

- **Batch Operation Promotion:** Advises batching requests over single operations in loops for better overall system performance.
- **Optimized Azure OpenAI Usage:** Suggests using `getChatCompletions` instead of `getCompletions` when leveraging Azure OpenAI services for improved accuracy and efficiency.
- **Client Object Management:** Recommends creating and reusing client objects via `buildClient` or `buildAsyncClient`, instead of repeatedly creating new instances, to conserve resources.

---

## Why It Matters

- **Immediate Feedback:** Developers receive instant suggestions and warnings as they write code, enabling early issue detection and remediation.
- **Improved Code Quality:** Adoption of best practices leads to safer, more maintainable, and high-performing Java codebases.
- **Tailored Azure SDK Guidance:** Recommendations are specifically designed for Azure services and libraries, ensuring developers follow up-to-date guidelines.

---

## Getting Started

1. **Update the Toolkit:** Ensure you are using the latest Azure Toolkit for IntelliJ.
2. **Open Your Project:** Start IntelliJ IDEA and load your Java project.
3. **Start Coding:** Receive real-time feedback on best practices and optimizations as you develop.

---

## Further Resources

- [Azure Toolkit for IntelliJ Documentation](https://learn.microsoft.com/azure/developer/java/toolkit-for-intellij/)
- [Azure SDK for Java Best Practices](https://aka.ms/azsdk/java/bestpractices)

The Azure SDK team is dedicated to continually improving the developer experience. Developers are encouraged to try out the updated analyzer and provide feedback to further shape future enhancements.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/azure-toolkit-for-intellij-introducing-the-enhanced-java-code-quality-analyzer/)
