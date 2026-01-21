---
external_url: https://andrewlock.net/using-and-authoring-dotnet-tools/
title: 'Using and Authoring .NET Tools: Multi-Targeting, CI, and Best Practices'
author: Andrew Lock
feed_name: Andrew Lock's Blog
date: 2025-09-02 10:00:00 +00:00
tags:
- .NET
- .NET 10
- .NET SDK
- .NET Tool
- .NET Tools
- Allow Downgrade
- CI
- Continuous Integration
- Global Tools
- Local Tools
- Multi Targeting
- NuGet
- PackAsTool
- Pre Release
- Rollforward
- Testing
- Tool Manifest
section_names:
- coding
- devops
---
Andrew Lock explores the complexities of authoring and managing .NET tools, offering practical advice on runtime targeting, manifest management, and CI testing for developers.<!--excerpt_end-->

# Using and Authoring .NET Tools: Multi-Targeting, CI, and Best Practices

*By Andrew Lock*

.NET tools provide a flexible way to distribute and use command-line and GUI utilities via NuGet, with options to install them either globally or locally for a specific project. This guide explores the practical issues around authoring, targeting different .NET runtimes, and testing tools in continuous integration pipelines.

## What are .NET Tools?

.NET tools are applications packaged for distribution on NuGet and installable via the .NET SDK. They can be global tools, available system-wide, or local tools, associated with a project's codebase through a tool manifest (a JSON file checked into version control).

### Example: Creating a Local Tool Manifest

```bash
dotnet new tool-manifest
```

This command creates `.config/dotnet-tools.json`, where you can list required tools for your project.

### Installing and Running Tools

Install a tool for your project:

```bash
dotnet tool install Cake.Tool
```

Run the installed tool:

```bash
dotnet tool run dotnet-cake      # or
 dotnet dotnet-cake
 dotnet cake
```

## Supporting Multiple Runtimes: Multi-Targeting

.NET tools are dependent on the runtime they target. To ensure wide compatibility, consider multi-targeting several frameworks in your *.csproj*:

```xml
<PropertyGroup>
  <TargetFrameworks>netcoreapp2.1;netcoreapp3.0;netcoreapp3.1;net5.0;net6.0;net7.0;net8.0;net9.0</TargetFrameworks>
</PropertyGroup>
```

- **Limitation:** You must stick to APIs available in the lowest targeted framework.
- **Downside:** Package size increases, which can slow down package restore and tool startup operations.

## Simplifying Runtime Forwarding with RollForward

Instead of targeting every potential runtime, use the `<RollForward>` option to let your tool run on newer compatible runtimes:

```xml
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
  <RollForward>Major</RollForward>
</PropertyGroup>
```

This tells the .NET host to use newer available runtimes. This technique is especially helpful for future .NET versions.

- **Caveat:** This assumes .NET's backward compatibility holds, but it isn't strictly guaranteed.

## CI and Testing Tips for .NET Tools

### Local Package Testing

- Use `--source` to specify where to install tools from (e.g. a local folder of .nupkg files)
- Use `--tool-path` to install to a specific directory without affecting the global tool cache.

Example:

```bash
dotnet tool install dd-trace \
  --add-source /app/install/. \
  --tool-path /tool \
  --version 1.2.3
```

### Installing Pre-release Versions

If your tool has a version suffix (e.g. `1.0.0-beta`), you must supply `--prerelease`:

```bash
dotnet tool install dd-trace \
  --add-source /app/install/. \
  --tool-path /tool \
  --version 1.2.3-preview \
  --prerelease
```

### Downgrading Tool Versions

By default, downgrades are blocked. Add `--allow-downgrade` to permit installing lower versions:

```bash
dotnet tool update -g dotnet-serve --version 1.10.175 --allow-downgrade
```

Both `install` and `update` support this flag in recent .NET SDKs.

## Key Takeaways

- Use tool manifests for local tool management and reproducible CI environments.
- Multi-target frameworks for the best compatibility at the cost of bigger packages and lowest-common-denominator APIs.
- RollForward is a powerful alternative to multi-targeting, especially for future .NET versions.
- Be aware of pre-release installation and version downgrade behaviors.
- CI testing and local installs are simplified by using `--source` and `--tool-path`.

This comprehensive guide ensures your .NET tools are robust, flexible, and CI-friendly, serving diverse runtime environments and development workflows.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/using-and-authoring-dotnet-tools/)
