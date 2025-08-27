---
layout: "post"
title: "Behind the Scenes of dotnet run app.cs: Deep Dive into .NET 10 Single-File Run Experience"
description: "Andrew Lock provides a detailed exploration of how the new single-file run feature in .NET 10 works under the hood. He investigates the implementation in the .NET SDK, focusing on virtual project file construction, build caching, directive handling, and the overall execution model for running C# files without a .csproj."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/exploring-dotnet-10-preview-features-2-behind-the-scenes-of-dotnet-run-app.cs/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-07-08 10:00:00 +00:00
permalink: "/2025-07-08-Behind-the-Scenes-of-dotnet-run-appcs-Deep-Dive-into-NET-10-Single-File-Run-Experience.html"
categories: ["Coding"]
tags: [".NET 10", ".NET Run", "Build Caching", "C#", "Coding", "MSBuild", "Posts", "Roslyn", "SDK Directives", "Single File Application", "Source Code Dive", "Top Level Statements", "Virtual Project File"]
tags_normalized: ["dotnet 10", "dotnet run", "build caching", "csharp", "coding", "msbuild", "posts", "roslyn", "sdk directives", "single file application", "source code dive", "top level statements", "virtual project file"]
---

In this article, Andrew Lock uncovers the internal workings of the upcoming .NET 10 feature that allows developers to run single C# files without a separate project file, detailing how the SDK constructs virtual projects and streamlines the build process.<!--excerpt_end-->

# Behind the Scenes of dotnet run app.cs: Exploring the .NET 10 Preview - Part 2

*By Andrew Lock*

---

In this article, I look at how the new .NET 10 feature that enables building and running a single C# file without a *.csproj* project is implemented inside the .NET SDK. This post focuses especially on how the virtual project file is constructed and how various supporting mechanisms work to create a streamlined developer experience.

## Recap: The Single-File App Feature

Previously, building a "hello world" C# app involved at minimum a *.csproj* project file and a *.cs* source file. .NET 10 introduces a preview feature where you can simply have a single C# file:

```csharp
Console.WriteLine("Hello, world");
```

and run it as:

```bash
dotnet run app.cs

# Output

Hello, world
```

The new feature removes the need for an explicit *.csproj*, applying a similar approach to script-based, rapid prototyping seen in other language ecosystems.

## Supported Directives and Examples

The feature supports several directives directly in the C# file:

- Shebang (e.g. `#!/usr/bin/dotnet run`)
- `#:sdk` for explicit SDK selection
- `#:package` and `#:project` for dependencies
- `#:property` for custom MSBuild properties

> Example:
>
> ```csharp
> #!/usr/bin/dotnet run
> #:sdk Microsoft.NET.Sdk.Web
> #:package [email protected]
> #:property UserSecretsId 2eec9746-c21a-4933-90af-c22431f35459
> var builder = WebApplication.CreateBuilder(args);
> var app = builder.Build();
> app.MapGet("/", () => JsonConvert.SerializeObject(new { Greeting = "Hello, World!" }));
> app.Run();
> ```

## The SDK Implementation — High-Level Overview

When executing `dotnet run app.cs`, the SDK:

1. Identifies whether the argument is a valid single-file C# program.
2. If valid, creates a `VirtualProjectBuildingCommand`.
3. Computes a build cache object to determine if a rebuild is needed.
4. Extracts directives from the file.
5. Assembles a virtual *.csproj* in memory.
6. Invokes MSBuild to restore and build.
7. Runs the compiled output.

## Identifying a Single-File Program

The SDK checks if the filename ends with `.cs`, or begins with a shebang (`#!`). If the file is piped from stdin (argument is `-`), it reads the file from standard input into a temporary file for processing.

Example:

```bash
'Console.WriteLine("Hello, World!");' | dotnet run -

# Output

Hello, World!
```

## Build Cache and Rebuild Logic

The SDK uses a cache mechanism to avoid unnecessary rebuilds. It checks for a `build-success.cache` file, comparing MSBuild properties and file changes (including implicit files like `global.json`, `NuGet.config`, etc.) to the previous build. If nothing relevant has changed, the build is skipped.

## Building the Virtual Project In-Memory

Key steps include:

- Parsing directives from the C# file using the Roslyn `SyntaxTokenParser` API.
- Constructing a virtual project file as a string (using methods such as `WriteProjectFile()`), then parsing it into an MSBuild project instance.

### Directive Handling Example

The SDK processes directives as follows:

- First `#:sdk` directive sets the primary SDK; further ones are imported with or without version specification.
- `#:property` directives are added as MSBuild properties.
- `#:package` and `#:project` become `<PackageReference>` and `<ProjectReference>` respectively.
- The single entry-point C# file is added explicitly, since `EnableDefaultItems` is disabled to prevent sibling file inclusion.

### Example Virtual Project Structure

A minimal produced project looks like:

```xml
<Project>
  <PropertyGroup>
    <IncludeProjectNameInArtifactsPaths>false</IncludeProjectNameInArtifactsPaths>
    <ArtifactsPath>/artifacts</ArtifactsPath>
  </PropertyGroup>
  <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <PublishAot>true</PublishAot>
  </PropertyGroup>
  <PropertyGroup>
    <EnableDefaultItems>false</EnableDefaultItems>
  </PropertyGroup>
  <PropertyGroup>
    <Features>$(Features);FileBasedProgram</Features>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="System.CommandLine" Version="2.0.0-beta4.22272.1" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="/path/to/Program.cs" />
  </ItemGroup>
  <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
</Project>
```

Workarounds for certain NuGet issues are also baked in as MSBuild target overrides.

## Restore and Build Steps

The SDK invokes MSBuild Restore and Build using the virtual project assembled in memory. Successful builds update the cache file to facilitate rapid future runs.

## Summary

The single-file run feature in .NET 10 leverages an ingenious implementation inside the SDK that essentially reverse-engineers a project structure for direct `.cs` execution. It parses special directives for SDK selection, packages, and properties—generating an in-memory project file which feeds MSBuild as usual. The system includes practical caching strategies and clever build optimizations to provide a smooth, fast developer experience.

---

> For a deeper code-level dive, see the references to actual .NET SDK and Roslyn source files provided throughout the article.

---

*Author: [Andrew Lock](https://www.andrewlock.net/)*

---

Stay up to the date with the latest posts!

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-2-behind-the-scenes-of-dotnet-run-app.cs/)
