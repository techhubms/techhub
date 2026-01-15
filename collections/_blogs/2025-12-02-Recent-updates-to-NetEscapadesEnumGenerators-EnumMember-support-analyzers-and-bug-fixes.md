---
layout: post
title: 'Recent updates to NetEscapades.EnumGenerators: [EnumMember] support, analyzers, and bug fixes'
author: Andrew Lock
canonical_url: https://andrewlock.net/recent-updates-to-netescapaades-enumgenerators/
viewing_mode: external
feed_name: Andrew Lock's Blog
feed_url: https://andrewlock.net/rss.xml
date: 2025-12-02 09:00:00 +00:00
permalink: /coding/blogs/Recent-updates-to-NetEscapadesEnumGenerators-EnumMember-support-analyzers-and-bug-fixes
tags:
- .NET
- .NET Core
- Analyzers
- BenchmarkDotNet
- Blogs
- C#
- Coding
- DescriptionAttribute
- Diagnostics
- DisplayAttribute
- EnumGenerators
- EnumMember
- Extension Methods
- Metadata Attributes
- NuGet
- Performance Optimization
- Roslyn
- Source Generator
- Source Generators
section_names:
- coding
---
Andrew Lock explains the recent improvements to NetEscapades.EnumGenerators, a .NET source generator package, including support for [EnumMember], additional analyzers, and bug fixes. Developers gain new metadata options and guidance on edge cases.<!--excerpt_end-->

# Recent updates to NetEscapades.EnumGenerators: [EnumMember] support, analyzers, and bug fixes

**Author**: Andrew Lock

NetEscapades.EnumGenerators is a source generator NuGet package designed to create efficient extension methods for working with enums in .NET. This article covers the main updates in version 1.0.0-beta16, including new metadata attribute support, added analyzers, and several bug fixes.

## Why Use NetEscapades.EnumGenerators?

- Generates fast `ToStringFast()` and parsing methods for enums
- Addresses performance issues in regular enum operations, as shown via `BenchmarkDotNet` comparisons
- Supports methods that return metadata, such as display names or descriptions

### Example: Faster ToString Methods

```csharp
public static string ToStringFast(this Colour colour) => colour switch {
    Colour.Red => nameof(Colour.Red),
    Colour.Blue => nameof(Colour.Blue),
    _ => colour.ToString(),
};
```

Benchmarks demonstrate significant speed improvements over default implementations in both .NET Framework and .NET.

## Main Updates in 1.0.0-beta16

### 1. Updated Metadata Attribute and `[EnumMember]` Support

- You can now select one metadata attribute source (`Display`, `Description`, or `EnumMember`) per enum.
- Set the source with the `MetadataSource` property on the `[EnumExtensions]` attribute, e.g.:

```csharp
[EnumExtensions(MetadataSource = MetadataSource.DisplayAttribute)]
public enum MyEnum { ... }
```

- The default metadata source is now `[EnumMember]`. Change defaults project-wide via MSBuild property:

```xml
<PropertyGroup>
  <EnumGenerator_EnumMetadataSource>DisplayAttribute</EnumGenerator_EnumMetadataSource>
</PropertyGroup>
```

### 2. New Roslyn Analyzers

- **NEEG001**: Flags duplicate extension class names (caused by naming clashes between enums).
- **NEEG002**: Warns when enums are nested inside generic types (unsupported scenario).
- **NEEG003**: Detects duplicate enum values, highlighting potentially unexpected `ToStringFast()` results.

Code examples illustrate each case and the relevant diagnostic. These analyzers improve developer awareness of subtle edge cases.

### 3. Bug Fixes

- Extension members generation now restricts by actual language version (C#14+), not just `LangVersion=Preview`.
- Introduced explicit opt-in via `EnumGenerator_ForceExtensionMembers` property when using previews.
- Corrected handling of reserved word enum member names using explicit escaping:

```csharp
private static string EscapeIdentifier(string identifier) {
    return SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None ? "@" + identifier : identifier;
}
```

- Removed the now-redundant `NETESCAPADES_ENUMGENERATORS_EMBED_ATTRIBUTES` option, reducing configuration complexity.

## Summary

The updates to NetEscapades.EnumGenerators include improved metadata handling, helpful analyzers for source generator edge cases, and fixes for extension method and reserved word scenarios. These changes increase the usability and reliability of enum utilities for .NET developers. For feedback and issues, contributions and bug reports are welcomed via the project's GitHub repository.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/recent-updates-to-netescapaades-enumgenerators/)
