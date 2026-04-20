---
title: .NET 11 Preview 3 is now available!
external_url: https://devblogs.microsoft.com/dotnet/dotnet-11-preview-3/
author: .NET Team
feed_name: Microsoft .NET Blog
date: 2026-04-14 19:30:00 +00:00
section_names:
- dotnet
primary_section: dotnet
tags:
- .NET
- .NET 11
- .NET 11 Preview 3
- .NET CLI
- .NET Container Images
- .NET Libraries
- .NET Run
- .NET Runtime
- .NET SDK
- .NET Watch
- ASP.NET Core
- C#
- C# Dev Kit
- ChangeTracker
- DbContext Pooling
- EF
- EF Core
- EF Core Migrations
- Featured
- HTTP/3
- JIT
- LongPressGestureRecognizer
- MAUI
- News
- Regex
- Response Compression
- Signed Container Images
- Solution Filters
- System.IO.Compression
- System.Text.Json
- Union Types
- VS
- VS Code
- WebAssembly
- WebCIL
- XAML
- Zstandard
---

.NET Team announces .NET 11 Preview 3, summarizing what’s new across the runtime, SDK, libraries, C#, ASP.NET Core, .NET MAUI, Entity Framework Core, and official container images, with links to detailed release notes and downloads.<!--excerpt_end-->

## .NET 11 Preview 3 is now available

Today, the **.NET Team** announced the third preview release of **.NET 11**. It includes improvements across the **.NET Runtime**, **SDK**, **libraries**, **ASP.NET Core**, **.NET MAUI**, **C#**, **Entity Framework Core**, **container images**, and more.

- Download: [Download .NET 11 Preview 3](https://dotnet.microsoft.com/download/dotnet/11.0)

## 📚 Libraries

- [System.Text.Json offers more control over naming and ignore defaults](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/libraries.md#systemtextjson-offers-more-control-over-naming-and-ignore-defaults)
- [Zstandard moved to System.IO.Compression and ZIP reads validate CRC32](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/libraries.md#zstandard-moved-to-systemiocompression-and-zip-reads-validate-crc32)
- [SafeFileHandle and RandomAccess expand pipe support](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/libraries.md#safefilehandle-and-randomaccess-expand-pipe-support)
- [Regex recognizes all Unicode newline sequences](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/libraries.md#regex-recognizes-all-unicode-newline-sequences)
- [See all library updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/libraries.md)

## ⏱️ Runtime

- [Runtime async removes the preview-API opt-in requirement](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/runtime.md#runtime-async-removes-the-preview-api-opt-in-requirement)
- [JIT optimizations improve switches, bounds checks, and casts](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/runtime.md#jit-optimizations-improve-switches-bounds-checks-and-casts)
- [Browser and WebAssembly add WebCIL and debugging improvements](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/runtime.md#browser-and-webassembly-add-webcil-and-debugging-improvements)
- [See all runtime updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/runtime.md)

## 🛠️ SDK

- [Solution filters can now be edited from the CLI](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/sdk.md#solution-filters-can-now-be-edited-from-the-cli)
- [File-based apps can be split across files](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/sdk.md#file-based-apps-can-be-split-across-files)
- [`dotnet run -e` passes environment variables from the command line](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/sdk.md#dotnet-run--e-passes-environment-variables-from-the-command-line)
- [`dotnet watch` adds Aspire, crash recovery, and Windows desktop improvements](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/sdk.md#dotnet-watch-adds-aspire-crash-recovery-and-windows-desktop-improvements)
- [See all SDK updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/sdk.md)

## C#

- [`union` type support](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/csharp.md#union-type-support)
- [See all C# updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/csharp.md)

## 🌐 ASP.NET Core

- [Zstandard response compression and request decompression](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/aspnetcore.md#zstandard-response-compression-and-request-decompression)
- [Virtualize adapts to variable-height items at runtime](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/aspnetcore.md#virtualize-adapts-to-variable-height-items-at-runtime)
- [HTTP/3 starts processing requests earlier](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/aspnetcore.md#http3-starts-processing-requests-earlier)
- [See all ASP.NET Core updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/aspnetcore.md)

## 📱 .NET MAUI

- [Maps add clustering, styling, and richer interaction APIs](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/dotnetmaui.md#maps-add-clustering-styling-and-richer-interaction-apis)
- [XAML and styling improvements reduce startup work and speed up iteration](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/dotnetmaui.md#xaml-and-styling-improvements-reduce-startup-work-and-speed-up-iteration)
- [`LongPressGestureRecognizer` is now built into .NET MAUI](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/dotnetmaui.md#longpressgesturerecognizer-is-now-built-into-net-maui)
- [.NET for Android adds Android 17 / API 37 preview support](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/dotnetmaui.md#net-for-android-adds-android-17--api-37-preview-support)
- [See all .NET MAUI updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/dotnetmaui.md)

## 🎁 Entity Framework Core

- [`ChangeTracker.GetEntriesForState()` avoids extra change detection](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/efcore.md#changetrackergetentriesforstate-avoids-extra-change-detection)
- [DbContext configuration can remove providers and add pooled factories](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/efcore.md#dbcontext-configuration-can-remove-providers-and-add-pooled-factories)
- [Migrations get more control and clearer feedback](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/efcore.md#migrations-get-more-control-and-clearer-feedback)
- [SQL generation removes unnecessary joins and SQL Server adds JSON APIs](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/efcore.md#sql-generation-removes-unnecessary-joins-and-sql-server-adds-json-apis)
- [See all EF Core updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/efcore.md)

## 📦 Container images

- [.NET container images are now signed](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/containers.md#net-container-images-are-now-signed)
- [See all container updates](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/containers.md)

## 🚀 Get started

- Install the SDK: [install the .NET 11 SDK](https://dotnet.microsoft.com/download/dotnet/11.0)
- On Windows with Visual Studio: install the latest [Visual Studio 2026 Insiders](https://visualstudio.microsoft.com/insiders)
- Alternatively: use Visual Studio Code with the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension


[Read the entire article](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-3/)

