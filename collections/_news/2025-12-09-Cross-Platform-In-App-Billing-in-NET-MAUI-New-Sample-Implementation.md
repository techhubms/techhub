---
layout: post
title: 'Cross-Platform In-App Billing in .NET MAUI: New Sample Implementation'
author: Gerald Versluis
canonical_url: https://devblogs.microsoft.com/dotnet/cross-platform-billing-dotnet-maui/
viewing_mode: external
feed_name: Microsoft .NET Blog
feed_url: https://devblogs.microsoft.com/dotnet/feed/
date: 2025-12-09 18:05:00 +00:00
permalink: /coding/news/Cross-Platform-In-App-Billing-in-NET-MAUI-New-Sample-Implementation
tags:
- .NET
- Android
- App Monetization
- Billing
- BillingService
- C#
- Coding
- Conditional Compilation
- Cross Platform
- Dependency Injection
- Google Play Billing
- in App Purchases
- Ios
- Mac Catalyst
- MAUI
- Microsoft Store
- MVVM
- News
- Sample Code
- StoreKit
- StoreKit 1
- StoreKit 2
- Windows
section_names:
- coding
---
Gerald Versluis introduces a new .NET MAUI sample for integrating cross-platform in-app billing, guiding developers through platform-specific APIs and architectural best practices.<!--excerpt_end-->

# Implementing Cross-Platform In-App Billing in .NET MAUI Applications

**Author:** Gerald Versluis

With the discontinuation of the InAppBillingPlugin, .NET MAUI developers now have a new resource for implementing in-app purchases in their applications. This new BillingService sample demonstrates how to integrate billing on Android, iOS, Mac Catalyst, and Windows using platform-specific APIs behind a unified interface.

## BillingService Sample Overview

- **Platforms Supported:**
  - **Android:** Google Play Billing Client v7
  - **iOS/Mac Catalyst:** StoreKit 1
  - **Windows:** Microsoft Store APIs

The BillingService sample helps developers:

- Maintain clean architectures (dependency injection, MVVM)
- Use a unified IBillingService interface for all platforms
- Replace now-discontinued plugins with comprehensive code examples

## Architecture

### Unified Interface

A central `IBillingService` interface allows your app to access billing features on any supported platform:

```csharp
public interface IBillingService {
    Task<bool> InitializeAsync();
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<PurchaseResult> PurchaseAsync(string productId);
    Task<bool> RestorePurchasesAsync();
    bool IsProductOwned(string productId);
}
```

### Platform-Specific Implementations

- Android, iOS (Mac Catalyst), and Windows have dedicated implementation files selected via conditional compilation.
- iOS/Mac Catalyst share a StoreKit-based implementation.

### Dependency Injection

BillingService is registered in `MauiProgram.cs`:

```csharp
builder.Services.AddSingleton<IBillingService, Services.BillingService>();
builder.Services.AddTransient<ProductsViewModel>();
```

### Patterns and Best Practices

- Unified code via interfaces and conditional compilation
- Dependency injection integration
- MVVM architecture for maintainability
- Platform-specific error handling and user feedback

## StoreKit Migration Note

- StoreKit 1 is currently used for iOS/Mac Catalyst, with Apple recommending StoreKit 2 for the future.
- The sample will be updated once .NET for iOS supports StoreKit 2.
- Apps using StoreKit 1 will keep working, but future improvements will require migration.

## Security Reminder

For production apps, implement server-side receipt verification and purchase validation for fraud prevention. This sample demonstrates client-side integration patterns only.

## Getting Started

1. [View the sample source on GitHub](https://github.com/dotnet/maui-samples/tree/main/10.0/BillingService)
2. [Browse live samples and docs](https://learn.microsoft.com/samples/dotnet/maui-samples/cross-platform-billing-service/)
3. Clone, configure, and run on your local setup

## Platform-Specific Product Setup Guides

- **Android:** [Google Play Console Setup](https://developer.android.com/google/play/billing/getting-ready)
- **iOS/Mac Catalyst:** [App Store Connect & Sandbox Accounts](https://developer.apple.com/help/app-store-connect/manage-in-app-purchases/create-in-app-purchases)
- **Windows:** [Microsoft Partner Center Add-on Setup](https://learn.microsoft.com/windows/uwp/publish/add-on-submissions)

## Key Takeaways for Developers

- Implement in-app purchases consistently across all .NET MAUI-supported platforms
- Use modern architectural patterns for maintainable, scalable code
- Prepare for StoreKit 2 migration on Apple platforms
- Reference detailed documentation for setup and testing

## Resources

- [BillingService Sample Code](https://github.com/dotnet/maui-samples/tree/main/10.0/BillingService)
- [Google Play Billing Docs](https://developer.android.com/google/play/billing)
- [Apple StoreKit Docs](https://developer.apple.com/storekit/)
- [Microsoft Store In-App Purchase Docs](https://learn.microsoft.com/windows/uwp/monetize/in-app-purchases-and-trials)
- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)

By following this sample, you can implement robust in-app billing for your cross-platform .NET MAUI apps and stay current with API changes across major app stores.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/cross-platform-billing-dotnet-maui/)
