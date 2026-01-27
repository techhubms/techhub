---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-10-rc-1/
title: Announcing .NET 10 Release Candidate 1
author: .NET Team
feed_name: Microsoft .NET Blog
date: 2025-09-09 18:45:00 +00:00
tags:
- .NET
- .NET 10
- ASP.NET Core
- Blazor
- C#
- C# Dev Kit
- Community Standup
- Cosmos DB
- Diagnostics
- EF Core
- Featured
- HybridWebView
- MAUI
- Metrics
- Minimal APIs
- OpenAPI
- Post Quantum Cryptography
- SQL Server
- Tensor
- Visual Studio Insiders
- VS Code
- Windows Forms
section_names:
- coding
primary_section: coding
---
.NET Team presents the first release candidate of .NET 10, with new features and improvements for libraries, ASP.NET Core, Blazor, .NET MAUI, and more. Learn what's new for developers in this broad platform update.<!--excerpt_end-->

# Announcing .NET 10 Release Candidate 1

.NET 10 Release Candidate 1 is now available and includes significant improvements across multiple areas of the .NET ecosystem. This release is production-ready with a go-live support license, allowing developers to utilize it confidently for their applications. It's fully supported in the new Visual Studio 2026 Insiders and Visual Studio Code with the C# Dev Kit extension.

## Key Highlights

### Libraries

- **Cryptography:** ML-DSA External Mu and Post Quantum Cryptography “API Complete”
- **Performance:** UTF-8 support for hex-string conversion
- **Numerics:** New Tensor, TensorSpan, and ReadOnlyTensorSpan types

[Read full library release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc1/libraries.md)

### Runtime, SDK, C#, F#, VB

- This release focuses on quality improvements for the runtime, SDK, and core languages (C#, F#, Visual Basic), with no new features introduced in this RC.

### ASP.NET Core & Blazor

- **New features:**
  - Persistent component state support for enhanced navigation
  - New ASP.NET Core Identity metrics
  - Validation improvements for Minimal APIs and Blazor
  - OpenAPI schema generation improvements

[Read full ASP.NET Core release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc1/aspnetcore.md)

### .NET MAUI

- Diagnostics and metrics tracking
- HybridWebView events
- Enhanced RefreshView capabilities
- Experimental CoreCLR runtime support for Android

[Read full .NET MAUI release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc1/dotnetmaui.md)

### Windows Forms

- Full dark mode integration
- Improved theming controls
- Renderer, async, and state management improvements

[Read full Windows Forms release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc1/winforms.md)

### Entity Framework Core

- SQL Server vector search
- SQL Server JSON type support
- Cosmos DB full-text and hybrid search
- Complex types and other query improvements

[Read full EF Core release notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc1/efcore.md)

### Other Areas

- Quality-focused improvements for WPF and container images

## Getting Started

- [Download .NET 10 RC1](https://get.dot.net/10)
- Use with Visual Studio 2026 Insiders, Visual Studio Code, and the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

## Stay Involved

- Join weekly [community standups](https://dotnet.microsoft.com/live/community-standup) to engage with the developers and product managers behind .NET
- Keep up-to-date through [release notes](https://github.com/dotnet/core/tree/dotnet10-rc1/release-notes/10.0), blogs, and [GitHub discussions](https://github.com/dotnet/core/discussions/categories/news)

## Learn More

- [What’s new in .NET 10](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview)
- [C# 14 features](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14)
- [.NET MAUI, ASP.NET Core, EF Core, Windows Forms, and WPF updates](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview)
- [Breaking changes in .NET 10](https://learn.microsoft.com/dotnet/core/compatibility/10.0)

Developers are encouraged to try out .NET 10 RC1, provide feedback, and prepare for production deployments before the final release.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-10-rc-1/)
