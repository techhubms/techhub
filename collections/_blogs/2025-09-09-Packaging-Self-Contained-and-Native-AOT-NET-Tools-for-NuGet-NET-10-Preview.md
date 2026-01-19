---
layout: post
title: 'Packaging Self-Contained and Native AOT .NET Tools for NuGet: .NET 10 Preview'
author: Andrew Lock
canonical_url: https://andrewlock.net/exploring-dotnet-10-preview-features-7-packaging-self-contained-and-native-aot-dotnet-tools-for-nuget/
viewing_mode: external
feed_name: Andrew Lock's Blog
feed_url: https://andrewlock.net/rss.xml
date: 2025-09-09 10:00:00 +00:00
permalink: /coding/blogs/Packaging-Self-Contained-and-Native-AOT-NET-Tools-for-NuGet-NET-10-Preview
tags:
- .NET 10
- .NET Tools
- AOT
- C#
- Cross Platform Deployment
- Microsoft.Data.Sqlite
- Microsoft.NET.Sdk
- Native AOT
- Native Dependencies
- NuGet
- Package Size Optimization
- PublishTrimmed
- SDK Limitations
- SDK Preview
- Self Contained Packaging
- Spectre.Console
section_names:
- coding
---
Andrew Lock details how .NET 10 preview improves .NET tool packaging for NuGet with new self-contained and native AOT deployment options, uncovering benefits, limitations, and practical tips for developers.<!--excerpt_end-->

# Packaging Self-Contained and Native AOT .NET Tools for NuGet: Exploring the .NET 10 Preview

*Author: Andrew Lock*

This article dives deep into new features introduced with the .NET 10 SDK for .NET tool authors, focusing on support for packaging tools as self-contained or Native Ahead-Of-Time (AOT) NuGet packages. These new capabilities allow tool developers to control package size, platform specificity, and eliminate runtime dependencies on target machines. The post covers:

## Key Deployment Models in .NET 10

- **Framework-dependent Executable:** Needs .NET runtime on target machine. Small app size; does not include the runtime itself.
- **Self-contained Executable:** Embeds the .NET runtime. Larger size but independent of installed runtime.
- **Trimmed Executable:** Self-contained but with unused code removed for a smaller footprint.
- **Native AOT Executable:** AOT compiles to native binaries per platform, offering fast startup and minimal overhead, but restricts platform portability and disables some features like certain reflection APIs.

The post also distinguishes between **platform-agnostic** and **platform-specific** deployments, impacting how native dependencies and runtime libraries are managed.

## New .NET Tool Packaging Options

With .NET 10 preview 6 and later:

- Packages can be built as framework-dependent or self-contained
- Targeting platform-specific runtime IDs allows for leaner packages
- Native AOT and trimming are now supported for tool packaging

Sample scenarios explore how changes to `<RuntimeIdentifiers>`, `<PublishSelfContained>`, `<PublishTrimmed>`, and `<PublishAot>` in the project file affect the build outputs.

## Example Project Setup

The walkthrough uses a simple tool called `sayhello`, multi-targeting several frameworks and including dependencies like `Spectre.Console` and `Microsoft.Data.Sqlite` to illustrate handling of native code and library trimming.

Code excerpt:

```xml
<PropertyGroup>
  <OutputType>Exe</OutputType>
  <TargetFrameworks>netcoreapp3.1;net5.0;net6.0;net7.0;net8.0;net9.0</TargetFrameworks>
  <PackAsTool>true</PackAsTool>
  <ToolCommandName>sayhello</ToolCommandName>
  ...
</PropertyGroup>
```

## Publishing and Packaging Results

- **Framework-Dependent, Platform-Agnostic:** Single, large package supporting many frameworks and platforms. Contains redundant files, suitable for maximum compatibility.
- **Framework-Dependent, Platform-Specific:** Produces individual packages for each runtime ID, minimizing package size and native library redundancy. Requires .NET 10 SDK support.
- **Self-Contained, Platform-Specific:** No runtime dependency; one framework per package. Greatly increases size but eliminates compatibility checks.
- **Trimmed, Self-Contained:** Leverages code trimming to reduce package size dramatically, works well if dependencies support trimming.
- **Native AOT:** Smallest and fastest executables, but limited to compatible platforms and disables features requiring dynamic runtime support.

The article visually demonstrates package contents and compares size savings across approaches.

## Limitations and Bugs

- New features only work with .NET 10 SDK, limiting backward compatibility.
- Bugs in preview releases affect fallback package creation and multi-targeting scenarios.
- Native AOT requires building on matching host OS.

## Recommendations and Takeaways

- Platform-specific/self-contained packaging is ideal for tool authors needing native dependencies or runtime-free deployment.
- Stick with agnostic packages for maximum compatibility until .NET 10 adoption increases.
- Monitor SDK bug fixes and improvements in upcoming RCs and GA releases.

## References

- [Official .NET 10 Preview Docs](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/sdk.md#platform-specific-net-tools)
- [Deploying .NET Applications](https://learn.microsoft.com/en-us/dotnet/core/deploying/)
- [Native AOT in .NET](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)

Andrew Lock's detailed walkthrough empowers .NET tool authors to leverage new .NET 10 SDK capabilities for optimized, platform-specific packaging while maintaining awareness of compatibility and current limitations.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-7-packaging-self-contained-and-native-aot-dotnet-tools-for-nuget/)
