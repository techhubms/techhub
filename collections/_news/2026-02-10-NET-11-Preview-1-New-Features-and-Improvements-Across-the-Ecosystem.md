---
layout: "post"
title: ".NET 11 Preview 1: New Features and Improvements Across the Ecosystem"
description: "This overview summarizes the major enhancements introduced in .NET 11 Preview 1, covering updates to the runtime, SDK, core libraries, ASP.NET Core, Blazor, C#, .NET MAUI, Entity Framework Core, and supporting tools for developers. It highlights key features, performance improvements, and new capabilities for building applications using the latest Microsoft technologies."
author: ".NET Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/dotnet/dotnet-11-preview-1/"
viewing_mode: "external"
feed_name: "Microsoft .NET Blog"
feed_url: "https://devblogs.microsoft.com/dotnet/feed/"
date: 2026-02-10 22:38:20 +00:00
permalink: "/2026-02-10-NET-11-Preview-1-New-Features-and-Improvements-Across-the-Ecosystem.html"
categories: ["Coding"]
tags: [".NET", ".NET 11", "ASP.NET Core", "Blazor", "C#", "Coding", "CoreCLR", "EF Core", "F#", "Featured", "Libaries", "MAUI", "MSBuild", "News", "OpenAPI", "Performance Improvements", "Runtime", "SDK", "VS", "Windows Forms", "WPF", "XAML"]
tags_normalized: ["dotnet", "dotnet 11", "aspdotnet core", "blazor", "csharp", "coding", "coreclr", "ef core", "fsharp", "featured", "libaries", "maui", "msbuild", "news", "openapi", "performance improvements", "runtime", "sdk", "vs", "windows forms", "wpf", "xaml"]
---

This summary by the .NET Team highlights what's new in .NET 11 Preview 1, focusing on updates for developers across runtime, SDK, C#, MAUI, ASP.NET Core, and more.<!--excerpt_end-->

# .NET 11 Preview 1: What's New for Developers

The .NET Team has announced the release of .NET 11 Preview 1, bringing a suite of enhancements for .NET developers working across platforms and workloads. This preview introduces important changes in libraries, runtime, the SDK, and key frameworks like ASP.NET Core, Blazor, .NET MAUI, and Entity Framework Core.

---

## Libraries

- **Zstandard Compression Support**: New support for Zstandard improves data compression scenarios.
- **BFloat16 Floating-Point Type**: Offers improved performance for workloads requiring this type.
- **ZipArchiveEntry Improvements, Hard Link APIs, DivisionRounding**: Better file and math functionality.
- **HMAC and KMAC Verification APIs**: Expands cryptographic capabilities.
- **FrozenDictionary Collection Expression Support**: Enhanced collection handling and performance.
- [More details here](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/libraries.md).

## Runtime

- **Runtime Async**: Improved async support for more responsive applications.
- **WebAssembly CoreCLR**: Expanded support for running .NET via WebAssembly.
- **Interpreter Expansion & JIT Performance**: Quicker start and improved runtime efficiency.
- **Architecture Enablement**: New hardware compatibility (RISC-V, s390x).
- [Full runtime notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/runtime.md).

## SDK and Tooling

- **`dotnet run` Enhancements**: Interactive target and device selection.
- **`dotnet test` Positional Arguments**: Simplifies test execution.
- **`dotnet watch` Hot Reload & Configurable Ports**: Streamlined hot-reload experiences.
- **New Analyzers**: Improved static analytics and guidance for development.
- [SDK details](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/sdk.md).

## MSBuild

- **Terminal Logger Improvements**, Language Fixes, and New APIs for richer project experiences.
- [MSBuild changes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/msbuild.md).

## C# and F#

- **C#**: Collection Expression Arguments and Expanded Layout.
- **F#**: Parallel compilation, new language features, compiler performance improvements.
- [C# and F# full notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/csharp.md), [F# notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/fsharp.md).

## ASP.NET Core and Blazor

- **New Components**: EnvironmentBoundary, Label, DisplayName for more flexible UI.
- **SignalR and Interactive Components**: Improved real-time and interactive features.
- **WSL Development**: Better certificate management.
- [ASP.NET Core details](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/aspnetcore.md).

## .NET MAUI and .NET for Android

- **XAML Source Generation by Default**: Faster, more efficient UI builds.
- **CoreCLR by default for Android**.
- [MAUI details](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/dotnetmaui.md).

## Entity Framework Core

- **Complex Types with JSON Columns**
- **Azure Cosmos DB Improvements**: Transactional batches, bulk execution, session token management.
- **One-step Migrations** for database changes.
- [EF Core notes](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/efcore.md).

## Windows Application Frameworks

- **Windows Forms**: Quality improvements.
- **WPF**: Fluent window backdrops and quality updates.

---

## Getting Started

- [Download .NET 11 Preview 1](https://dotnet.microsoft.com/download/dotnet/11.0)
- [Visual Studio 2026 Insiders](https://visualstudio.microsoft.com/insiders) or [VS Code C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) support is available.

For more information, refer to the full release notes linked for each section.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-1/)
