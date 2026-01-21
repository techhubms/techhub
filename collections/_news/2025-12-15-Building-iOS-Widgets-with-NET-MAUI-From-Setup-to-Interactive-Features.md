---
external_url: https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/
title: 'Building iOS Widgets with .NET MAUI: From Setup to Interactive Features'
author: Toine de Boer
feed_name: Microsoft .NET Blog
date: 2025-12-15 18:05:00 +00:00
tags:
- .NET
- .NET For Ios
- App Groups
- AppIntents
- Apple Developer Console
- Build Pipelines
- C#
- Deep Linking
- Entitlements
- Interactive Widgets
- Ios
- Ios Widgets
- MAUI
- Mobile Development
- Preferences
- Release Build
- Swift
- UserDefaults
- VS
- VS Code
- Widget Extension
- WidgetKit
- Widgets
- Xcode
- XReleases
section_names:
- coding
---
Toine de Boer’s detailed post guides developers through the process of creating iOS widgets with .NET MAUI, from project setup to adding interactive features, focusing on core technical steps for seamless integration.<!--excerpt_end-->

# Building iOS Widgets with .NET MAUI: From Setup to Interactive Features

## Introduction

In this guide, Toine de Boer demonstrates how developers can build complete, interactive iOS widgets using .NET MAUI, tackling common obstacles around documentation and toolchain integration. The post walks through essential technical steps, enabling professional widget development comparable to native approaches, while highlighting integration points between .NET MAUI, Xcode, and Swift.

## Prerequisites

- **Apple Developer Console setup:**
  - Obtain Bundle IDs for both your main app and widget extension.
  - Enable App Groups capability for shared data.
  - Create a Group ID for secure cross-app data communication.

## Project Structure: Xcode and Swift Essentials

- Create an iOS and Android-targeted .NET MAUI project.
- Build your widget extension in Xcode using Swift:
  - Start with an app template, attach the Widget Extension, and configure Bundle IDs.
  - Include 'Configuration App Intent' for sample data and easier initial testing.
- Version alignment: Ensure all targets match iOS versions to avoid deployment issues.
- Refactor generated Swift files to improve maintainability.
  - Key objects include WidgetBundle, Widget, AppIntentTimelineProvider, TimelineEntry, View, and WidgetConfigurationIntent.

## Widget Assets and App Icons

- Ensure icons are properly set in both the asset catalog and referenced in the widget’s info.plist.
- Use online generators for bulk icon formatting and confirm correct display after updates.
- Insert specific XML entries in info.plist to link assets and handle iOS icon caching.

## Building and Integrating the Widget Extension

- Use Xcode scripts to automate release build collection in a dedicated folder, which integrates well with CI pipelines.
- Place build output under `Platforms/iOS/`, using `CopyToOutput` in your .NET MAUI `.csproj` for seamless inclusion.
- Ensure the item group in your `.csproj` reflects the correct paths, filenames, and platform targets for the widget extension.

## Data Sharing Between App and Widget

- Treat widgets as standalone apps; direct communication is limited.
- Use .NET MAUI Preferences (maps to UserDefaults on iOS) for data exchange.
- Both projects need identical Entitlements with the same Group ID.
- Example usage:
  - C# (.NET MAUI):

    ```csharp
    Preferences.Set("MyDataKey", "my data to share", "group.com.enbyin.WidgetExample");
    ```

  - Swift:

    ```swift
    UserDefaults(suiteName: "group.com.enbyin.WidgetExample")?.set("my data to share", forKey: "MyDataKey")
    let data = UserDefaults(suiteName: "group.com.enbyin.WidgetExample")?.string(forKey: "MyDataKey")
    ```

## Communication Strategies

- **App to Widget:**
  - Use WidgetKit’s API (via a NuGet binding package) to signal data refresh:

    ```csharp
    var widgetCenterProxy = new WidgetKit.WidgetCenterProxy();
    widgetCenterProxy.ReloadTimeLinesOfKind("MyWidget");
    ```

- **Widget to App:**
  - Tap interactions use `widgetUrl()` to open the app with optional deep link data.
  - AppIntents enable basic interactivity within widgets, such as updating values and forcing timeline reloads:

    ```swift
    struct IncrementCounterIntent: AppIntent {
        // ...increment logic, save new data, reload timeline
    }
    ```

## Practical Development Tips

- Pair-program in VS Code with Copilot for Swift code iteration, but expect to make manual fixes.
- Use Xcode Canvas view for preview data and immediate feedback on layout changes.
- Core widget logic should reside in your .NET MAUI project for cross-platform reuse; widget-specific tasks (storage, view, lightweight backend communication) require Swift.

## Wrapping Up

- Once interactive widgets are live, refine design, layout, and backend logic for production readiness.
- Stay tuned for further guidance on Android widget integration with .NET MAUI.

## References

- [Example code on GitHub: Maui.WidgetExample](https://github.com/Toine-db/Maui.WidgetExample)
- [Original Post](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/)

---

This guide offers a step-by-step technical walkthrough of building iOS widgets with .NET MAUI, balancing cross-platform logic reuse with the realities of Swift and Xcode integration. Final tips help developers streamline workflow and avoid common pitfalls during the transition from C# to Swift.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/how-to-build-ios-widgets-with-dotnet-maui/)
