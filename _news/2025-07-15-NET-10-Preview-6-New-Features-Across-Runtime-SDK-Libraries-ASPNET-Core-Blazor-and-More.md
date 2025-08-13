---
layout: "post"
title: ".NET 10 Preview 6: New Features Across Runtime, SDK, Libraries, ASP.NET Core, Blazor, and More"
description: "The .NET Team has released .NET 10 Preview 6, introducing enhancements in the runtime, SDK, libraries, ASP.NET Core, Blazor, .NET MAUI, and more. Key updates include Post-Quantum Cryptography, improved code generation, Blazor diagnostics, MAUI enhancements, and early support for Android API 35/36. Full release notes are available."
author: ".NET Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/dotnet-10-preview-6/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-07-15 17:22:00 +00:00
permalink: "/2025-07-15-NET-10-Preview-6-New-Features-Across-Runtime-SDK-Libraries-ASPNET-Core-Blazor-and-More.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", ".NET MAUI", "Android", "ASP.NET Core", "Blazor", "C#", "Coding", "Containers", "Entity Framework Core", "Featured", "Ios", "JSON Serialization", "Mac Catalyst", "News", "Post Quantum Cryptography", "Visual Studio", "WinForms", "WPF"]
tags_normalized: ["net", "net 10", "net maui", "android", "asp dot net core", "blazor", "c", "coding", "containers", "entity framework core", "featured", "ios", "json serialization", "mac catalyst", "news", "post quantum cryptography", "visual studio", "winforms", "wpf"]
---

In this announcement, the .NET Team details the latest features and improvements in .NET 10 Preview 6, covering updates to the core runtime, SDK, ASP.NET Core, Blazor, .NET MAUI, and more.<!--excerpt_end-->

# .NET 10 Preview 6 is Now Available

**Author:** .NET Team

---

The .NET Team is pleased to announce the sixth preview release of .NET 10, which brings a range of enhancements and quality improvements across the .NET ecosystem. This update delivers new capabilities and features in the .NET runtime, SDK, libraries, ASP.NET Core, Blazor, .NET MAUI, Entity Framework Core, and additional platforms.

This blog provides an overview of what's new in this preview. For comprehensive details, consult the [full release notes](https://github.com/dotnet/core/tree/main/release-notes/10.0/).

## 📚 Libraries

- **Option to disallow duplicate JSON properties**: Strengthen data integrity during serialization.
- **Strict JSON serialization options**: Ensure stricter compliance during JSON serialization.
- **Post-Quantum Cryptography (PQC)**: Early support for cryptographic algorithms resistant to quantum attacks.
- [Full Libraries Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/libraries.md)

## ⏱️ Runtime

- **Improved Code Generation for Struct Arguments**: Performance optimizations in method calls involving struct arguments.
- **Improved Loop Inversion**: Faster execution due to enhanced JIT compiler loop inversion.
- [Full Runtime Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/runtime.md)

## 🛠️ SDK

- **Platform-specific .NET Tools**: Improved ability to specify tooling for different platforms.
- **One-shot tool execution**: Run .NET tools for one-time actions.
- **New `dnx` execution script**: Streamlines tool execution experiences.
- **New `--cli-schema` option**: Enhances CLI introspection capabilities.
- **File-based apps improvements**: Expands platform and deployment support for file-based apps.
- [Full SDK Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/sdk.md)

## C#

- No new language features were added in this preview.
- [Full C# Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/csharp.md)

## F#

- No new features in this preview.
- [F# Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/fsharp.md)

## Visual Basic

- No new features in this preview.
- [Visual Basic Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/visualbasic.md)

## 🌐 ASP.NET Core & Blazor

- **Automatic Eviction from Memory Pool**: Enhanced memory management for ASP.NET Core.
- **Blazor WebAssembly Preloading**: Faster Blazor app startup.
- **Bundler Friendly Output for Blazor Builds**: Simplifies integration with JavaScript ecosystems.
- **Improved Form Validation**: Strengthens data entry reliability.
- **Blazor Diagnostics Improvements**: Eases monitoring and troubleshooting.
- **Blazor Server State Persistence**: Robust user experience in Blazor Server-side apps.
- **Passkey Support for ASP.NET Core Identity**: Next-gen authentication support.
- **Minimal API Validation with IProblemDetailsService**: Better error reporting in APIs.
- [ASP.NET Core Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/aspnetcore.md)

## 📱 .NET MAUI

- **MediaPicker Enhancements**: Smoother media selection experiences on mobile.
- **WebView Request Interception**: Greater flexibility for handling web requests in apps.
- **Control and Layout Fixes**: Numerous improvements in UI stability.
- [MAUI Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/dotnetmaui.md)
- Full quality improvements list at [dotnet/maui GitHub releases](https://github.com/dotnet/maui/releases/)

### 🤖 .NET for Android

- Support for **Android API levels 35 and 36**.
- Improvements in interop performance, binary size, and diagnostics.
- [Android Release Notes](https://github.com/dotnet/android/releases/)

### 🍎 .NET for iOS, Mac Catalyst, macOS, tvOS

- Platform SDK updates for Xcode 16.4 support.
- Improved binding generation, build reliability, and runtime behavior.
- [macios Release Notes](https://github.com/dotnet/macios/releases/)
- [Known issues](https://github.com/dotnet/macios/wiki/Known-issues-in-.NET10)

## 🖥️ Windows Forms

- Focused on **quality improvements** and **build performance**.
- [WinForms Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/winforms.md)

## 🖥️ Windows Presentation Foundation (WPF)

- Prioritized **quality improvements** and **build performance**.
- [WPF Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/wpf.md)

## 🎁 Entity Framework Core

- Ongoing **quality improvements** and **build performance**.
- [EF Core Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/efcore.md)

## 📦 Container Images

- Continued **quality and build improvements** for containerized environments.
- [Containers Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/containers.md)

---

## 🚀 Getting Started

- [Download the .NET 10 Preview 6 SDK](https://get.dot.net/10)
- For Windows users: Install the latest [Visual Studio 2022 preview](https://visualstudio.microsoft.com/vs/preview/). This now includes GitHub Copilot agent mode and MCP server support.
- Use [Visual Studio Code](https://code.visualstudio.com/) and the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) with .NET 10.

## 💬 Community & Feedback

- Join weekly [community standups](https://dotnet.microsoft.com/live/community-standup) to discuss developments directly with the .NET team.
- Participate in the live stream for .NET 10 Preview 6 Unboxed, featuring demos and in-depth discussions.
- Engage in [GitHub Discussions](https://github.com/dotnet/core/discussions/categories/news) for announcements and feedback.
- [Give feedback and discuss this release](https://aka.ms/dotnet/10/preview6).

## 📰 Stay Up-to-Date

Explore the latest features and documentation:

- [What’s new in .NET 10](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview)
- [What’s new in C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14)
- [What’s new in .NET MAUI](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-10)
- [What’s new in ASP.NET Core](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-10.0)
- [What’s new in Entity Framework Core](https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/whatsnew)
- [What’s new in Windows Forms](https://learn.microsoft.com/dotnet/desktop/winforms/whats-new/net100)
- [What’s new in WPF](https://learn.microsoft.com/dotnet/desktop/wpf/whats-new/net100)
- [Breaking Changes in .NET 10](https://learn.microsoft.com/dotnet/core/compatibility/10.0)
- [.NET 10 Releases](https://github.com/dotnet/core/blob/main/release-notes/10.0/README.md)

Subscribe to the [GitHub Discussions RSS news feed](https://github.com/dotnet/core/discussions/categories/news.atom) for the latest updates.

---

**Feedback from the community is encouraged. Join the discussion for .NET 10 Preview 6 and help shape the next version of .NET!**

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-6/)
