---
external_url: https://andrewlock.net/exploring-dotnet-10-preview-features-8-supporting-platform-specific-dotnet-tools-on-old-sdks/
title: 'Supporting Platform-Specific .NET Tools on Older SDKs: .NET 10 Preview Deep Dive'
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2025-09-16 10:00:00 +00:00
tags:
- .csproj
- .NET 10
- .NET Core
- .NET SDK
- Andrew Lock
- AOT
- Compatibility
- Datadog
- DotnetToolSettings.xml
- Framework Dependent
- Multi Targeting
- Native AOT
- Netcoreapp3.1
- NuGet
- Performance
- Platform Specific Tools
- Self Contained Deployment
- Tool Packaging
section_names:
- coding
primary_section: coding
---
Andrew Lock provides a thorough guide on leveraging the new .NET 10 platform-specific tool features, highlighting technical strategies and trade-offs for maintaining compatibility with older .NET SDKs.<!--excerpt_end-->

# Supporting Platform-Specific .NET Tools on Older SDKs: .NET 10 Preview Deep Dive

*By Andrew Lock*

In this article, Andrew Lock explores the recent advancements in .NET 10 for platform-specific (self-contained or Native AOT) NuGet package support for .NET tools. The post focuses on key aspects relevant to tool authors, balancing the benefits of these new features with the challenges of supporting earlier .NET SDKs.

## Introduction

.NET 10 introduces major changes for .NET tool authors, most notably:

- Platform-specific, self-contained, and Native AOT tool packages.
- Enhanced one-off tool execution (`dotnet tool exec`/`dnx`) without prior installation.

While these provide significant benefits like reduced startup times and simpler dependency matrices, they also create challenges for maintaining backwards compatibility with earlier SDK releases.

## New Features Breakdown

### .NET 10 SDK Enhancements

- **`dotnet tool exec` and `dnx`**: Allow running packaged tools on demand, streamlining usage for consumers.
- **Platform-Specific Packaging**: Tools can now be published as self-contained or Native AOT binaries for specific platforms, greatly potentially reducing package size and improving startup performance.

#### Implications for Tool Authors

- For one-off executions via `dnx`, smaller packages are better due to faster initial downloads for users.
- Creating platform-specific tools requires .NET 10 SDK, but this locks out users on earlier SDKs.
- Tools using Native AOT can benefit from much quicker startup, whereas those that can't may see limited value.

## Compatibility Trade-offs

- Pre-.NET 10 SDKs cannot install platform-specific packages due to changes in the *DotnetToolSettings.xml* schema (`Version=2` and `Runner=executable` are understood only by .NET 10+).
- Authors must decide whether to require .NET 10 SDK or maintain compatibility with older SDKs.
- Large matrix of supported frameworks can become complicated.

### The Compromise: Dual-packaging Approach

- **Root Package**: Target lowest supported framework (e.g., `netcoreapp3.1`) for maximum compatibility.
- **Platform-Specific Packages**: For .NET 10 SDK+, provide separate self-contained or Native AOT builds for each platform.

**Benefits:**

- .NET 10 users get all the latest improvements.
- Earlier SDK users remain supported.

**Drawbacks:**

- Root packages may become large.
- .NET 10 users may need to download both the root and platform package.
- Support matrix complexity remains unless all platforms are Native AOT compatible.

## Case Studies

### 1. Datadog `dd-trace` .NET Tool

- Supports a wide range of platforms and SDKs.
- Platform-specific packaging doesn't reduce maintenance or package size (no Native AOT); overall, not beneficial in its current setup.

### 2. `sleep-pc` .NET Tool

- Small, simple, privately used, and easily NativeAOT compiled.
- Dual packaging viable with insignificant size penalty; .NET 10 users get startup performance benefits.

## Technical Implementation

Andrew details how to structure your `.csproj` for conditional multi-targeting, balancing support for both modern and legacy SDKs, and tweaking the *DotnetToolSettings.xml* for compatibility. This includes:

- Conditional target frameworks.
- Using `dotnet pack` (and `dotnet pack -r <runtime>`) for generating appropriately structured NuGet packages.
- Two alternative approaches with code samples provided.

## Conclusion

Tool authors must weigh the platform-specific and NativeAOT benefits in .NET 10 against compatibility and package size trade-offs for users on older SDKs. A hybrid packaging approach can offer a practical middle-ground, but may or may not be worth the added complexity or size for a given tool.

# Key Takeaways

- .NET 10's features are powerful but require strategic packaging decisions.
- Backwards compatibility can be preserved, at the cost of larger and more complex packages.
- Evaluate whether NativeAOT and platform-specific features justify added support overhead for your audience and distribution model.

# Further Reading

- [Andrew Lock's original series](https://andrewlock.net/tag/dotnet/)
- [.NET 10 Official Documentation](https://learn.microsoft.com/en-us/dotnet/core/)

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-8-supporting-platform-specific-dotnet-tools-on-old-sdks/)
