---
layout: "post"
title: "Exploring Single-File .NET Apps with dotnet run app.cs in .NET 10 Preview"
description: "Andrew Lock details the new single-file run experience in .NET 10, enabling developers to execute C# files directly without a project file. The post explores its features, configuration, limitations, target use-cases, and upcoming enhancements, making .NET more accessible for newcomers and simplifying script and sample development."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/exploring-dotnet-10-preview-features-1-exploring-the-dotnet-run-app.cs/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-07-01 10:00:00 +00:00
permalink: "/2025-07-01-Exploring-Single-File-NET-Apps-with-dotnet-run-appcs-in-NET-10-Preview.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", ".NET Publish", ".NET Run", ".NET Script", "Aspire AppHost", "C#", "C# Scripting", "CLI", "Coding", "MSBuild", "NuGet Packages", "Posts", "Project SDK", "Shebang", "Single File Programs", "Visual Studio Code"]
tags_normalized: ["net", "net 10", "net publish", "net run", "net script", "aspire apphost", "c", "c sharp scripting", "cli", "coding", "msbuild", "nuget packages", "posts", "project sdk", "shebang", "single file programs", "visual studio code"]
---

Andrew Lock introduces and examines the new single-file .NET run experience in .NET 10, highlighting its features, scenarios, and emerging enhancements that simplify C# development.<!--excerpt_end-->

# Exploring Single-File .NET Apps with dotnet run app.cs in .NET 10 Preview

**Author: Andrew Lock**

.NET 10 brings a major convenience to C# developers: the ability to build and run a complete application from a single `.cs` file—no project or `.csproj` file needed. In this post, Andrew Lock explains how this new feature works, explores its capabilities, and outlines use cases and limitations, targeted especially at new .NET users or those wanting light-weight scripting with C#.

---

## What is `dotnet run app.cs`?

Historically, running even a simple C# program required:

- a `.csproj` project file
- a `.cs` file with your C# code

With .NET 10 (preview), the requirement for the `.csproj` file is optional. You can now create a C# source file (e.g., `app.cs`) and run it directly:

```csharp
Console.WriteLine("Hello, world");
```

```bash
dotnet run app.cs
Hello, world
```

This feature has been referred to as "file-based programs," "runfile," or described by the invocation `dotnet run app.cs`. The .NET team showcased it during a Microsoft Build session and through blog announcements.

---

## Key Features of the Single-File Experience

### Directives for Configuration

Single-file apps in .NET 10 support several special directives you can include at the top of your file (all starting with `#:`):

- **Shebang**:
  - Make scripts executable in Unix-like systems with `#!/usr/bin/dotnet run`.
  - Allows running with `chmod +x app.cs` and then `./app.cs`.
- **SDK References**:
  - Default SDK is `Microsoft.NET.Sdk`, but you can specify others (e.g., `Microsoft.NET.Sdk.Web`) with `#:sdk`.
  - Multiple `#:sdk` entries are supported, e.g. for using Aspire AppHost:

    ```csharp
    #:sdk Microsoft.NET.Sdk
    #:sdk Aspire.AppHost.Sdk 9.3.0
    ```

- **NuGet Package References**:
  - Add dependencies inline with `#:package`, e.g. `#:package Aspire.Hosting.AppHost@9.3.0`.
  - Wildcards are supported for versions.
- **MSBuild Properties**:
  - Configure properties as you would in a `.csproj`: `#:property UserSecretsId 2eec9746-c21a-4933-90af-c22431f35459`
  - Syntax likely changing from space to `=` separators in future previews.

#### Example

```csharp
# !/usr/bin/dotnet run

# :sdk Microsoft.NET.Sdk

# :sdk Aspire.AppHost.Sdk 9.3.0

# :package Aspire.Hosting.AppHost@9.3.0

# :property UserSecretsId 2eec9746-c21a-4933-90af-c22431f35459

using Microsoft.Extensions.Configuration;
var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> {
  { "ASPIRE_DASHBOARD_OTLP_ENDPOINT_URL", "https://localhost:21049" },
  { "ASPIRE_RESOURCE_SERVICE_ENDPOINT_URL", "https://localhost:22001" },
  { "ASPNETCORE_URLS", "https://localhost:17246" },
});
builder.Build().Run();
```

---

### Upcoming Features (in .NET 10 Preview Roadmap)

- **Project References**: Use `#:project` to reference other projects by path or directory (coming in preview 6).
- **Publishing**: Ability to publish single-file apps with `dotnet publish app.cs`, defaulting to NativeAOT publishing.
- **Direct Execution**: Use `dotnet app.cs` directly, enhancing shebang support and Unix compatibility.
- **Stdin Code Execution**: Pipe C# code direct from stdin (e.g., via shell or curl) for quick scripts or experimentation.

---

## Who Benefits from Single-File C# Programs?

This feature is primarily designed to improve the onboarding experience for newcomers to .NET and C#: you need only a single file to get started—no XML-heavy project files. However, there are other handy scenarios:

- **Scripting/Utility scripts**: Use C# instead of bash or PowerShell for quick utilities.
- **Samples & Training**: Library/framework examples can be organized as a collection of single file scripts, rather than separate projects.
- **Experimentation**: Try out new language or SDK features quickly, with minimal setup.

The ability to convert a single-file app to a conventional project is built in:

```bash
dotnet project convert app.cs
```

This will generate a `.csproj` file and transfer all configuration, allowing continued growth of your application.

---

## Current Limitations and “Escape Hatches”

- **No Multi-file Support Yet**: Direct support for including multiple source files is postponed to .NET 11, though workarounds exist using `Directory.Build.props` and similar MSBuild files.
- **Visual Studio Support**: Only Visual Studio Code and CLI are supported. Visual Studio itself will not support single-file runs, though third-party support in editors like JetBrains Rider is being discussed.
- **Language Limitation**: Only C# is supported for now—not Visual Basic or F#.

#### Supported Escape Hatch Files

For advanced configuration, the single-file runner will implicitly use certain files (if present) from the working directory:

- `global.json`
- `NuGet.config`
- `Directory.Build.props` / `Directory.Build.targets`
- `Directory.Packages.props`
- *.rsp MSBuild response files

These can be used to set shared properties or dependencies across multiple single-file apps in the same directory.

---

## What’s Not Coming (Yet)?

- **Multi-file Support**: Inclusion of multiple or nested files will only be added in .NET 11 or beyond.
- **Visual Studio Integration**: No support within the full Visual Studio IDE, only in VS Code and the CLI.
- **Non-C# Languages**: No announced plans for similar experience in VB or F#.

---

## Summary

.NET 10 introduces a featureful and flexible way to execute C# code instantly from a single file, tailored for onboarding, demos, scripting, and rapid prototyping. Andrew Lock’s post covers the main configuration options, showcases practical usage, and previews likely future expansions—while also clarifying what developers should not expect in the first releases.

For further details, see the official [announcement post](https://devblogs.microsoft.com/dotnet/announcing-dotnet-run-app/) and check out Damian Edwards' demos and the GitHub issues labeled `Area-run-file` for tracking ongoing work in this area.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-1-exploring-the-dotnet-run-app.cs/)
