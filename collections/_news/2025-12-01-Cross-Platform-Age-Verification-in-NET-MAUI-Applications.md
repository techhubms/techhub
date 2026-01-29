---
external_url: https://devblogs.microsoft.com/dotnet/cross-platform-age-verification-dotnet-maui/
title: Cross-Platform Age Verification in .NET MAUI Applications
author: Gerald Versluis
feed_name: Microsoft .NET Blog
date: 2025-12-01 18:05:00 +00:00
tags:
- .NET
- Age Verification
- Android
- API Integration
- Apple Declared Age Range
- Compliance
- Cross Platform
- Family Sharing
- Google Play Age Signals
- IAgeSignalService
- Ios
- MAUI
- NuGet
- Platform Specific Code
- Sample App
- Swift Interop
- UserAccountInformation
- Windows
- Windows Age Consent
- Coding
- News
section_names:
- coding
primary_section: coding
---
Gerald Versluis presents a comprehensive guide to implementing age verification across Android, iOS, and Windows in .NET MAUI apps, with details on meeting upcoming legal compliance requirements.<!--excerpt_end-->

# Cross-Platform Age Verification in .NET MAUI Applications

## Overview

New legislation in Texas (Jan 1, 2026), Utah (May 7, 2026), and Louisiana (July 1, 2026) requires apps to verify user ages. This guide walks you through building age verification in your .NET MAUI apps using platform-specific APIs for Android, iOS, and Windows, supported by a sample repository to accelerate development.

## Legal Compliance Deadlines

- Texas: January 1, 2026
- Utah: May 7, 2026
- Louisiana: July 1, 2026

Non-compliance may lead to fines and app store removal.

## Platforms & APIs

- **Android**: [Google Play Age Signals API](https://developer.android.com/google/play/age-signals/overview) — returns age status (VERIFIED, SUPERVISED, etc.) based on jurisdiction.
- **iOS**: [Apple Declared Age Range API](https://developer.apple.com/documentation/declaredagerange/) — provides age range and verification status (works only on iOS 26.0+ and requires bridging between Swift and .NET).
- **Windows**: [Windows Age Consent API](https://learn.microsoft.com/uwp/api/windows.system.user.checkuserageconsentgroupasync) — categorizes users as Child, Minor, or Adult.

## Implementation in .NET MAUI

### Unified Interface

Define a `IAgeSignalService` to abstract platform APIs:

```csharp
public interface IAgeSignalService {
    Task<AgeVerificationResult> RequestAgeVerificationAsync(int minimumAge = 13, object? platformContext = null);
    Task<AgeVerificationResult> RequestAgeVerificationAsync(AgeVerificationRequest request);
    string GetPlatformName();
}
```

### Platform-Specific Services

Use conditional files like `AgeSignalService.Android.cs`, `AgeSignalService.iOS.cs`, and `AgeSignalService.Windows.cs` to handle API integration. The build system selects the right implementation for each target platform.

### Dependency Registration

Register your service in `MauiProgram.cs`:

```csharp
builder.Services.AddSingleton<IAgeSignalService, AgeSignalService>();
builder.Services.AddSingleton<MainPage>();
```

## Platform Requirements

- **Android**: Device with Google Play Store, Android 6.0+, `Xamarin.Google.Android.Play.Age.Signals` NuGet package. Works only in necessary jurisdictions.
- **iOS**: iOS 26.0+, real device, `com.apple.developer.declared-age-range` entitlement, Family Sharing, XCFramework bindings for Swift-to-.NET interop.
- **Windows**: Windows 11+ (Build 22000+), `UserAccountInformation` capability in your manifest.

## Usage Considerations

- Know the legal requirements for your app users' locations.
- Clearly explain why age verification is needed to end users.
- Design for graceful failure if age verification is unavailable.
- Use collected data solely for compliance, not for analytics or ads.
- Test on actual devices, as simulators do not support all platform-specific APIs (especially for iOS).

## Links & Resources

- [Sample Repository](https://github.com/dotnet/maui-samples/tree/main/10.0/AgeSignals)
- [Google Play Age Signals API](https://developer.android.com/google/play/age-signals/overview)
- [Google Play Compliance Guide](https://support.google.com/googleplay/android-developer/answer/16569691)
- [Apple Declared Age Range API](https://developer.apple.com/documentation/declaredagerange/)
- [WWDC 2024 Session](https://developer.apple.com/videos/play/wwdc2024/10178/)
- [Windows Age Consent API](https://learn.microsoft.com/uwp/api/windows.system.user.checkuserageconsentgroupasync)
- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)

## Final Notes

The sample provides guidance and ready-to-use code for your projects. Pay attention to regional laws and deadlines. For questions, consult the provided documentation and reach out in community forums.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/cross-platform-age-verification-dotnet-maui/)
