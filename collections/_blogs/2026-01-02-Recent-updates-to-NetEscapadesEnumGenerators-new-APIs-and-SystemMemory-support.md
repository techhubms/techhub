---
layout: "post"
title: "Recent updates to NetEscapades.EnumGenerators: new APIs and System.Memory support"
description: "This post by Andrew Lock details important updates to the NetEscapades.EnumGenerators source generator, focusing on version 1.0.0-beta19. Key changes include options for disabling enum number parsing, support for automatic case transformation, and expanded APIs via the System.Memory NuGet package, improving performance and usability for .NET developers."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/updates-to-netescapaades-enumgenerators-new-apis-and-system-memory-support/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2026-01-02 09:00:00 +00:00
permalink: "/2026-01-02-Recent-updates-to-NetEscapadesEnumGenerators-new-APIs-and-SystemMemory-support.html"
categories: ["Coding"]
tags: [".NET Core", "API Design", "Blogs", "C#", "Coding", "Enum", "NetEscapades.EnumGenerators", "NuGet", "Parse Methods", "Performance Optimization", "ReadOnlySpan", "Roslyn", "SerializationOptions", "SerializationTransform", "Source Generators", "StringComparison", "System.Memory"]
tags_normalized: ["dotnet core", "api design", "blogs", "csharp", "coding", "enum", "netescapadesdotenumgenerators", "nuget", "parse methods", "performance optimization", "readonlyspan", "roslyn", "serializationoptions", "serializationtransform", "source generators", "stringcomparison", "systemdotmemory"]
---

Andrew Lock summarizes recent improvements to the NetEscapades.EnumGenerators package, including new APIs, advanced parsing options, and System.Memory support, offering insights for .NET developers seeking faster enum operations.<!--excerpt_end-->

# Recent Updates to NetEscapades.EnumGenerators: New APIs and System.Memory Support

**Author:** Andrew Lock

## Overview

This blog post covers several feature additions and improvements to the [NetEscapades.EnumGenerators](https://github.com/andrewlock/NetEscapades.EnumGenerators) source generator as of version 1.0.0-beta19. These changes aim to boost performance, provide greater API flexibility, and accommodate broader platform support for .NET development targeting enums.

## Why Use an Enum Source Generator?

Working with enums in C# can sometimes incur performance costs, especially when using standard methods like `ToString()`. NetEscapades.EnumGenerators addresses this by generating fast extension methods for enum value serialization and parsing, such as:

```csharp
public static class ColourExtensions {
    public string ToStringFast(this Colour colour) => colour switch {
        Colour.Red => nameof(Colour.Red),
        Colour.Blue => nameof(Colour.Blue),
        _ => colour.ToString(),
    };
}
```

Benchmarks show that these generated methods can dramatically outperform the default implementations:

| Method         | Mean      | Allocated |
| -------------- | --------- | --------- |
| ToString       | 6.44 ns   | 24 B      |
| ToStringFast   | 0.0050 ns | -         |

For details on all features, check the [README](https://github.com/andrewlock/NetEscapades.EnumGenerators) or previous blog posts.

## What’s New in 1.0.0-beta19

### 1. Disabling Number Parsing with EnumParseOptions

A longstanding request from users has been the option to disable fallback number parsing in `Parse` and `TryParse` methods. By default, `Enum.Parse` allows any integer value to be parsed into an enum, but this is rarely desirable.

A new `EnumParseOptions` readonly struct now lets you explicitly control this behavior:

```csharp
public readonly struct EnumParseOptions {
    public EnumParseOptions(
        StringComparison comparisonType = StringComparison.Ordinal,
        bool allowMatchingMetadataAttribute = false,
        bool enableNumberParsing = true
    ) {...}

    public bool EnableNumberParsing => ...;
}
```

Usage:

```csharp
ColourExtensions.Parse(someNumber, new EnumParseOptions(enableNumberParsing: false));
// throws ArgumentException if input is a number
```

This approach keeps the API clear by encapsulating all relevant options.

### 2. Automatic ToLowerInvariant()/ToUpperInvariant() Support

Another significant addition is the ability to control serialization casing using a `SerializationOptions` struct. You can now generate methods that serialize enum names in lower or upper invariant formats—useful for interacting with APIs requiring specific casing.

```csharp
public readonly struct SerializationOptions {
    public SerializationOptions(..., SerializationTransform transform = SerializationTransform.None) {...}
}

public enum SerializationTransform { None, LowerInvariant, UpperInvariant }
```

Example usage:

```csharp
var colour = Colour.Red;
Console.WriteLine(colour.ToStringFast(new(transform: SerializationTransform.LowerInvariant))); // red
```

### 3. System.Memory NuGet Package Support

To improve compatibility for libraries targeting .NET Standard 2.0 or .NET Framework, the generator now supports `ReadOnlySpan<char>` APIs when the *System.Memory* package is referenced. This offers performance improvements and access to modern parsing methods even on older runtimes.

**Setup example:**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>netstandard2.0</TargetFrameworks>
    <EnumGenerator_UseSystemMemory>true</EnumGenerator_UseSystemMemory>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="NetEscapades.EnumGenerators" Version="1.0.0-beta19" />
    <PackageReference Include="System.Memory" Version="4.6.3" />
  </ItemGroup>
</Project>
```

**Note:** There are caveats regarding allocations when `int.TryParse()` is unavailable for spans; see the XML documentation for warnings.

### Additional Notes

- Options structs make the API extensible without overload explosion.
- There’s experimental automatic detection of *System.Memory* package references.
- Feedback from users is especially appreciated for the System.Memory and options features.

## Summary

These updates make NetEscapades.EnumGenerators more versatile and performant for .NET developers. With new options for parsing, casing, and broader runtime support, the package continues to evolve in response to community feedback. Try the latest release, and report any issues or suggestions via [GitHub](https://github.com/andrewlock/NetEscapades.EnumGenerators/issues).

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/updates-to-netescapaades-enumgenerators-new-apis-and-system-memory-support/)
