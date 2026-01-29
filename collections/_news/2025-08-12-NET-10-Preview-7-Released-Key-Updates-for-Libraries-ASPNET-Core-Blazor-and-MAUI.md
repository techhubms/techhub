---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/
title: '.NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI'
author: .NET Team
feed_name: Microsoft .NET Blog
date: 2025-08-12 19:20:00 +00:00
tags:
- .NET
- .NET 10
- .NET Runtime
- AES KeyWrap
- ASP.NET Core
- Blazor
- C#
- C# Dev Kit
- Community Standup
- EF Core
- Featured
- Libraries
- MAUI
- PipeReader
- Preview Release
- Process Group
- Release Notes
- SDK
- TLS 1.3
- VS
- WebSocketStream
- Windows Forms
- WPF
- Coding
- News
section_names:
- coding
primary_section: coding
---
.NET Team presents a summary of features and improvements in .NET 10 Preview 7, highlighting updates across the ecosystem and providing links to detailed resources. Authors from the .NET Team offer guidance on how developers can get started with this release.<!--excerpt_end-->

# .NET 10 Preview 7 Released: Key Updates for Libraries, ASP.NET Core, Blazor, and MAUI

The .NET Team has announced the seventh preview of .NET 10, delivering a broad set of improvements across the .NET ecosystem. This release focuses on enhancements to the .NET runtime, SDK, core libraries, and developer tools, together with major updates in frameworks like ASP.NET Core, Blazor, and .NET MAUI.

## Download and Get Started

- [Download .NET 10 Preview 7](https://get.dot.net/10)
- Recommended: Use the latest [Visual Studio 2022 preview](https://visualstudio.microsoft.com/vs/preview/) (with GitHub Copilot agent mode & MCP server support), or [Visual Studio Code](https://code.visualstudio.com/) with the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

## What's New in .NET 10 Preview 7?

### Libraries

- Launch Windows processes in new process group
- AES KeyWrap with Padding (IETF RFC 5649)
- ML-DSA & Composite ML-DSA support
- PipeReader support for JSON serializer
- WebSocketStream improvements
- TLS 1.3 for macOS (client)
- [Detailed library release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/libraries.md)

### Runtime, SDK, and Tooling

- Support for `any` RuntimeIdentifier with platform-specific .NET Tools
- Various performance and compatibility updates
- See [runtime](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/runtime.md) and [SDK](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/sdk.md) release notes for full details

### C#, F#, and Visual Basic

- No new feature updates in this preview
- [C#](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/csharp.md), [F#](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/fsharp.md), [Visual Basic](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/visualbasic.md) release notes available

### ASP.NET Core & Blazor

- Configurable suppression of exception handler diagnostics
- Avoid cookie login redirects on API endpoints
- Passkey authentication improvements
- Support for `.localhost` TLD in development
- PipeReader support in System.Text.Json
- Enhanced validation for classes and records
- Blazor and OpenAPI.NET upgrades
- [ASP.NET Core release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/aspnetcore.md)

### .NET MAUI

- XAML Source Generator
- MediaPicker EXIF support
- SafeArea and Secondary Toolbar improvements
- New Control APIs, deprecated API removals
- [MAUI release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/dotnetmaui.md) | [maui GitHub releases](https://github.com/dotnet/maui/releases/)

### Entity Framework Core

- Improved translation for parameterized collections
- Miscellaneous bug fixes & improvements
- [EF Core release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/efcore.md)

### Windows UI Frameworks

- **Windows Forms**: Dark mode rendering improvements (ComboBox, RichTextBox, PropertyGrid)
- **WPF**: Bug fixes and Fluent Theme improvements
- [WinForms release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/winforms.md), [WPF release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/wpf.md)

### Containers

- `dnx` added to `PATH` in SDK images
- [Container release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/containers.md)

## Stay Up-to-Date

- Explore what's new in [C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14), [MAUI](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-10), [ASP.NET Core](https://learn.microsoft.com/aspnet/core/release-notes/aspnetcore-10.0), [Entity Framework Core](https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/whatsnew), [Windows Forms](https://learn.microsoft.com/dotnet/desktop/winforms/whats-new/net100), and [WPF](https://learn.microsoft.com/dotnet/desktop/wpf/whats-new/net100)
- Review the [full .NET 10 release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/README.md) and [breaking changes](https://learn.microsoft.com/dotnet/core/compatibility/10.0)

## Community Involvement

- Join [community standups](https://dotnet.microsoft.com/live/community-standup) to connect with the team
- Engage on [.NET Discussions](https://github.com/dotnet/core/discussions/categories/news)
- [Give feedback on Preview 7](https://aka.ms/dotnet/10/preview7)

For further details and deep dive updates, see the [official .NET Blog post](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/).

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-10-preview-7/)
