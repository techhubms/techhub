---
external_url: https://andrewlock.net/splitting-the-netescapades-enumgenerators-packages-the-road-to-a-stable-release/
title: 'Splitting the NetEscapades.EnumGenerators Packages: Architecture and Stable Release Path'
author: Andrew Lock
primary_section: dotnet
feed_name: Andrew Lock's Blog
date: 2026-03-10 09:00:00 +00:00
tags:
- .NET
- .NET 6
- .NET 8
- .NET Core
- Blogs
- C#
- Code Generation
- Dependency Management
- Enum Performance
- EnumParseOptions
- Incremental Generators
- Library Design
- NuGet
- Package Architecture
- Roslyn
- SerializationOptions
- Source Generators
section_names:
- dotnet
---
Andrew Lock explains recent architectural changes to the NetEscapades.EnumGenerators package family, guiding .NET developers on choosing the right flavor for their scenario and sharing lessons from evolving the popular source generator library.<!--excerpt_end-->

# Splitting the NetEscapades.EnumGenerators Packages: The Road to a Stable Release

*By Andrew Lock*

## Introduction

This post describes recent significant structural changes to the [NetEscapades.EnumGenerators](https://github.com/andrewlock/NetEscapades.EnumGenerators) source generator NuGet package. The goal: to support a wider range of usage scenarios for .NET developers and library authors and to set the path toward a stable 1.0.0 release.

## Why Use an Enum Source Generator?

Enum operations in .NET, such as `ToString()` or parsing from strings, can be inefficient—especially in older runtimes. NetEscapades.EnumGenerators addresses this by generating fast methods like `ToStringFast()` for enums. This source generator leverages features introduced in .NET 6 (incremental generators) and provides significant speedup, as proven by [BenchmarkDotNet](https://benchmarkdotnet.org/) benchmarks:

| Method        | Framework | Mean     | Ratio  |
|--------------|-----------|----------|--------|
| ToString     | net48     | 578 ns   | 1.000  |
| ToStringFast | net48     | 3 ns     | 0.005  |
| ToString     | net6.0    | 18 ns    | 1.000  |
| ToStringFast | net6.0    | 0.12 ns  | 0.007  |
| ToString     | net10.0   | 6.4 ns   | 1.000  |
| ToStringFast | net10.0   | 0.005 ns | 0.001  |

Even as .NET's built-in enum support has improved, generated code remains significantly faster in most scenarios.

## Feature Evolution and Backward Compatibility Pitfalls

Version `1.0.0-beta19` introduced major changes such as `EnumParseOptions` and `SerializationOptions`. These enabled overloads for parsing and serializing enums with fine-grained control, such as case sensitivity and number parsing. These new API capabilities, however, inadvertently broke some users due to transitive runtime dependency requirements.

### Real-World Build System Patterns

Developers commonly add source generators as dependencies with `PrivateAssets=All` and/or `ExcludeAssets=runtime` to prevent those assets from polluting downstream consumers or output folders. When new types necessary for runtime became part of the API and lived inside `NetEscapades.EnumGenerators.Attributes.dll`, these patterns resulted in missing type errors at runtime (e.g., error CS0012 involving 'EnumParseOptions').

## The Architectural Solution: Splitting the Packages

To better support diverse usage patterns, the package was refactored as follows:

- **NetEscapades.EnumGenerators**: Metapackage for easy install, references the subpackages.
- **NetEscapades.EnumGenerators.Generators**: Contains only the source generator logic.
- **NetEscapades.EnumGenerators.RuntimeDependencies**: Holds types needed by generated code at runtime (`EnumParseOptions`, `SerializationOptions`, `SerializationTransform`).

### Recommended Usage Patterns

- **Typical app**: Reference the metapackage (`NetEscapades.EnumGenerators`) and get all features with the best onboarding experience.
- **Internal library, avoid runtime dependencies for consumers**:
    - Reference `NetEscapades.EnumGenerators.Generators` directly using `PrivateAssets=All` and optionally `ExcludeAssets=runtime`.
    - Optionally reference `RuntimeDependencies` for less verbose generated code.
- **Optimized, decoupled consumption**: If you avoid referencing `RuntimeDependencies`, the generator emits necessary types as nested types inside extension classes, so usage is more verbose but avoids introducing transitive dependencies.

### Example Project File Configurations

**Standard app integration:**

```xml
<PackageReference Include="NetEscapades.EnumGenerators" Version="1.0.0-beta21" />
```

**Reusable library (no runtime dependencies):**

```xml
<PackageReference Include="NetEscapades.EnumGenerators.Generators" Version="1.0.0-beta21" PrivateAssets="All" ExcludeAssets="runtime" />
```

**Hybrid (library with explicit runtime deps):**

```xml
<PackageReference Include="NetEscapades.EnumGenerators.Generators" Version="1.0.0-beta21" PrivateAssets="All" ExcludeAssets="runtime"/>
<PackageReference Include="NetEscapades.EnumGenerators.RuntimeDependencies" Version="1.0.0-beta21" />
```

If you omit the runtime dependency package, you'll need to use verbose type names (e.g., `ColourExtensions.EnumParseOptions`) in your code.

## Moving Toward a Stable 1.0.0 Release

The post discusses remaining API, usability, and documentation questions before finalizing a stable version, such as:

- Should usage analyzers be enabled by default?
- Should nested dependency types be hidden/made private?
- Managing method overloads to reduce ambiguity and improve the developer experience.

Feedback from the community is welcome. Try out the [latest beta](https://www.nuget.org/packages/NetEscapades.EnumGenerators/1.0.0-beta21), read the [project's README](https://github.com/andrewlock/NetEscapades.EnumGenerators), and share your experiences or issues on [GitHub](https://github.com/andrewlock/NetEscapades.EnumGenerators/issues/67).

## Summary

If you're working on an app, stick to the metapackage for simplicity. Library authors with stricter dependency requirements should pick a minimal, explicit package set. This architectural split offers flexibility but introduces extra decisions for maintainers. Andrew Lock is seeking real-world feedback as part of the final push toward a stable, reliable v1.0.0.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/splitting-the-netescapades-enumgenerators-packages-the-road-to-a-stable-release/)
