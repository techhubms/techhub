---
layout: "post"
title: ".NET 11 Preview 2: New Features and Improvements Across Runtime, SDK, and Libraries"
description: ".NET 11 Preview 2 introduces significant updates and performance enhancements across the entire .NET ecosystem. This release covers improvements in the runtime, SDK, libraries, ASP.NET Core, Blazor, .NET MAUI, Entity Framework Core, and more. Developers can expect better performance, new APIs, expanded support, and updates to popular tools and frameworks, enabling faster and more efficient application development for web, desktop, mobile, and cloud scenarios."
author: ".NET Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/dotnet-11-preview-2/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2026-03-10 20:35:00 +00:00
permalink: "/2026-03-10-NET-11-Preview-2-New-Features-and-Improvements-Across-Runtime-SDK-and-Libraries.html"
categories: ["Coding"]
tags: [".NET", ".NET 11", "ASP.NET Core", "Blazor", "C#", "Coding", "Container Images", "EF Core", "F#", "Featured", "JIT", "LINQ", "MAUI", "News", "OpenTelemetry", "Runtime", "SDK", "Visual Basic", "VS", "Windows Forms", "WPF"]
tags_normalized: ["dotnet", "dotnet 11", "aspdotnet core", "blazor", "csharp", "coding", "container images", "ef core", "fsharp", "featured", "jit", "linq", "maui", "news", "opentelemetry", "runtime", "sdk", "visual basic", "vs", "windows forms", "wpf"]
---

The .NET Team announces the availability of .NET 11 Preview 2, highlighting new features, performance improvements, and expanded support across libraries, runtime, SDK, ASP.NET Core, Blazor, .NET MAUI, and more for developers.<!--excerpt_end-->

# .NET 11 Preview 2: New Features and Improvements Across Runtime, SDK, and Libraries

Today, the .NET Team announced the release of .NET 11 Preview 2. This version introduces enhancements and new features in several areas of the .NET platform, including:

## Key Highlights

- **Libraries**
  - Generic `GetTypeInfo` support for `System.Text.Json`
  - Tar archive format selection
  - Matrix4x4.GetDeterminant performance increased by ~15%
  - [Full library release notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview2/libraries.md)

- **Runtime**
  - Async improvements (V2)
  - JIT (Just-In-Time compiler) enhancements
  - VM updates such as cached interface dispatch
  - [Full runtime release notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview2/runtime.md)

- **SDK**
  - Smaller SDK installers for Linux and macOS
  - Improved code analyzers
  - New warnings and build targets
  - [Full SDK release notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview2/sdk.md)

- **Languages**
  - No major new features in C# or Visual Basic for this preview
  - F# additions: simplified DIM interface hierarchies, overload resolution caching, new preprocessor directives, and collection functions
  - [C# what's new](https://learn.microsoft.com/dotnet/csharp/whats-new/)
  - [F# full release notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview2/fsharp.md)

- **ASP.NET Core & Blazor**
  - Native OpenTelemetry tracing for ASP.NET Core
  - TempData support in Blazor
  - OpenAPI 3.2.0 support
  - .NET Web Worker project template
  - Various performance improvements
  - [Full ASP.NET Core and Blazor release notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview2/aspnetcore.md)

- **.NET MAUI**
  - Map control and TypedBinding performance enhancements
  - Immutability annotations for `Color` and `Font`
  - VisualStateManager API consistency improvements
  - Platform support updates (Android fix updates, CoreCLR API changes)
  - [Full .NET MAUI release notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview2/dotnetmaui.md)

- **Entity Framework Core**
  - LINQ `MaxBy` and `MinBy` support
  - DiskANN vector indexes and `VECTOR_SEARCH()` for SQL Server
  - Support for full-text catalogs and indexes in SQL Server
  - JSON_CONTAINS() support
  - [Full EF Core release notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview2/efcore.md)

- **Other Platforms**
  - Quality and reliability improvements for Windows Forms and WPF
  - Better container images: SDK images up to 17% smaller

## Getting Started

To try out .NET 11 Preview 2:

- [Download .NET 11 Preview 2](https://dotnet.microsoft.com/download/dotnet/11.0)
- On Windows, install the latest [Visual Studio 2026 Insiders](https://visualstudio.microsoft.com/insiders)
- Use [Visual Studio Code](https://code.visualstudio.com/) with [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

For detailed documentation and all full release notes, see the linked sections throughout this announcement.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-2/)
