---
layout: "post"
title: "Announcing .NET 10 Release Candidate 2"
description: "This announcement covers the release of .NET 10 Release Candidate 2, highlighting stabilization and quality improvements across the .NET ecosystem. It details updates for .NET MAUI, Entity Framework Core, ASP.NET Core, Blazor, SDK integration, and tools like Visual Studio 2026 Insiders and Visual Studio Code. Developers are encouraged to validate and provide feedback ahead of general availability."
author: ".NET Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/dotnet-10-rc-2/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2025-10-14 22:15:00 +00:00
permalink: "/2025-10-14-Announcing-NET-10-Release-Candidate-2.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", "Android API 36.1", "ASP.NET Core", "Blazor", "C#", "Code Quality", "Coding", "Containers", "Dev Kit", "EF Core", "F#", "MAUI", "Migration", "News", "Quality", "Release Candidate", "SDK", "Source Generation", "Uno Platform", "Visual Basic", "VS", "WinForms", "WPF", "XAML", "Xcode 26"]
tags_normalized: ["dotnet", "dotnet 10", "android api 36dot1", "aspdotnet core", "blazor", "csharp", "code quality", "coding", "containers", "dev kit", "ef core", "fsharp", "maui", "migration", "news", "quality", "release candidate", "sdk", "source generation", "uno platform", "visual basic", "vs", "winforms", "wpf", "xaml", "xcode 26"]
---

The .NET Team introduces .NET 10 Release Candidate 2, emphasizing final quality and stabilization improvements for developers across platforms and frameworks.<!--excerpt_end-->

# Announcing .NET 10 Release Candidate 2

.NET 10 Release Candidate 2 is now available. This update represents the final release candidate before general availability, providing a go-live support license for production use and focused on quality and stabilization throughout the .NET ecosystem.

## Key Highlights

- **Production-Ready**: This release candidate comes with a go-live license, enabling production applications.
- **Visual Studio 2026 Insiders Support**: .NET 10 RC2 is supported in Visual Studio 2026 Insiders and Visual Studio Code with the C# Dev Kit.

## .NET MAUI

- New microphone permission handling
- SafeAreaEdges feature enhancements
- XAML Source Generation improvements
- Support for Android API 36.1 and Xcode 26 bindings
- Acknowledgement for Uno Platform's contributions, enabling support for Android 16 and API 36.1
- [Full MAUI Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc2/dotnetmaui.md)

## Entity Framework Core

- Background fixes and improvements for complex JSON support
- Improvement for transaction behavior during migrations
- Use of ExecutionStrategy for query retries
- Analyzer warnings for raw SQL string concatenation
- [Full EF Core Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc2/efcore.md)

## SDK

- Support for using .NET MSBuild tasks with .NET Framework MSBuild
- [Full SDK Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc2/sdk.md)

Additional areas refined in this release candidate include: Libraries, Runtime, C#, F#, Visual Basic, ASP.NET Core, Blazor, Windows Forms, WPF, and container images. Details are available in their respective release notes.

## Getting Started

- [Install .NET 10 SDK](https://get.dot.net/10)
- Download .NET 10 RC2 for installers and binaries [here](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc2/10.0.0-rc.2.md)
- For Windows, use the latest [Visual Studio 2026 Insiders](https://aka.ms/VS2026Insiders).
- Visual Studio Code integration is supported via [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit).

## Stay Up-to-Date

- [What’s new in .NET 10](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview)
- [What’s new in C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14)
- [ASP.NET Core Release Notes](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-10.0)
- [EF Core 10 Release Notes](https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/whatsnew)
- [Windows Forms Release Notes](https://learn.microsoft.com/dotnet/desktop/winforms/whats-new/net100)
- [WPF Release Notes](https://learn.microsoft.com/dotnet/desktop/wpf/whats-new/net100)
- [Breaking Changes in .NET 10](https://learn.microsoft.com/dotnet/core/compatibility/10.0)
- [.NET 10 Release Overview](https://github.com/dotnet/core/blob/main/release-notes/10.0/README.md)

## Feedback and Community

Stay informed through the GitHub Discussions [news feed](https://github.com/dotnet/core/discussions/categories/news.atom) and participate in the [.NET 10 RC2 discussion](https://aka.ms/dotnet/10/rc2) to provide feedback.

For further collaboration and partnership details (such as the Uno Platform’s work with MAUI), check the [Uno Platform blog](https://platform.uno/blog/Announcing-UnoPlatform-Microsoft-dotnet-collaboration).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-2/)
