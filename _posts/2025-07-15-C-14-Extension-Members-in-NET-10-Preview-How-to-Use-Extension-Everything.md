---
layout: "post"
title: "C# 14 Extension Members in .NET 10 Preview: How to Use Extension Everything"
description: "Andrew Lock explores the upcoming C# 14 'extension members' feature available in the .NET 10 preview. This post covers the syntax changes, conversion from extension methods, new extension member types, and practical adoption in the NetEscapades.EnumGenerators NuGet package."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/exploring-dotnet-10-preview-features-3-csharp-14-extensions-members/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-07-15 10:00:00 +00:00
permalink: "/2025-07-15-C-14-Extension-Members-in-NET-10-Preview-How-to-Use-Extension-Everything.html"
categories: ["Coding"]
tags: [".NET 10", "C#", "C# 14", "Coding", "Extension Members", "Extension Methods", "Extension Properties", "Instance Extension Properties", "Language Features", "NetEscapades.EnumGenerators", "Posts", "Static Extension Methods", "Syntax Changes", "VS"]
tags_normalized: ["dotnet 10", "csharp", "csharp 14", "coding", "extension members", "extension methods", "extension properties", "instance extension properties", "language features", "netescapadesdotenumgenerators", "posts", "static extension methods", "syntax changes", "vs"]
---

In this post, Andrew Lock provides a detailed walkthrough of C# 14’s new extension members feature in .NET 10, including how to convert existing extension methods, the new syntax, and updates to his NetEscapades.EnumGenerators package.<!--excerpt_end-->

## C# 14 Extension Members: Extension Everything in .NET 10 Preview

**Author: Andrew Lock**

### Introduction

C# 14, currently available in the .NET 10 preview, introduces a significant language feature called *extension members*, also referred to as "extension everything." Andrew Lock examines what this feature is, how it builds upon the existing extension methods, and the steps for implementing extension members, with a practical focus on his NetEscapades.EnumGenerators NuGet package.

> This post references .NET 10 preview 5 and may not capture final release changes.

---

### Background: Extension Methods

Extension methods, introduced in .NET Framework 3.5 (2007), allow developers to emulate adding instance methods to types via static methods using the `this` modifier. For example:

```csharp
public static class EnumerableExtensions {
    public static bool IsEmpty<T>(this IEnumerable<T> target) => !target.Any();
}
```

You can invoke such a method statically:

```csharp
if (EnumerableExtensions.IsEmpty(values)) { ... }
```

Or via the more idiomatic extension syntax:

```csharp
if (values.IsEmpty()) { ... }
```

Extension methods have become foundational in .NET, widely used throughout the base class libraries and user projects.

---

### What Are Extension Members?

Soon after extension methods debuted, the community requested support for other member types: properties, static methods, etc. Efforts to design this ("extension everything") continually arose but failed until now. C# 14 introduces *extension members*, making it possible to extend existing types with not just methods, but also properties and (eventually) operators.

> For details on design decisions, see the [official announcement](https://devblogs.microsoft.com/dotnet/csharp-exploring-extension-members/).

---

### How to Convert to Extension Member Syntax

Existing extension methods work unchanged, but the new syntax offers additional flexibility and types. To opt into extension members, your project must use C# 14 (currently set `<LangVersion>preview</LangVersion>` and .NET 10 SDK).

**Example:** Converting an extension method to extension member syntax:

Old extension method:

```csharp
public static class EnumerableExtensions {
    public static bool IsEmpty<T>(this IEnumerable<T> target) => !target.Any();
}
```

New extension member syntax:

```csharp
public static class EnumerableExtensions {
    extension<T>(IEnumerable<T> target) {
        public bool IsEmpty() => !target.Any();
    }
}
```

#### Migration steps

1. Add `extension(){ }` block.
2. Move the receiver parameter as a parameter to the `extension` block.
3. Move type arguments from method to `extension` block.
4. Remove the `static` keyword from the method.

Not all extension methods can migrate; [see the detailed guide](https://devblogs.microsoft.com/dotnet/csharp-exploring-extension-members/).

**IDE Support:** Visual Studio preview supports this syntax with code fixes; other tooling (like Rider) may not yet.

There’s no functional reason to convert unless you intend to leverage new extension member types.

---

### Adding New Types of Extension Members

The key innovation is the support for additional member types in extension form:

- **Static extension methods**
- **Static extension properties**
- **Instance extension properties**

Operator extensions will arrive in .NET 10 preview 7.

#### Static Extension Methods

Syntax closely resembles instance methods, but:

- The method is `static`.
- The receiver parameter in the `extension` block has no accessible variable.

**Example:**

```csharp
public static class StringExtensions {
    extension(string) {
        public static bool HasValue(string value) => !string.IsNullOrEmpty(value);
    }
}
```

Used as:

```csharp
if (string.HasValue(someValue)) { ... }
```

---

#### Extension Properties

Add properties as well as methods. Example:

```csharp
public static class StringExtensions {
    extension(string target) {
        public bool IsAscii => target.All(x => char.IsAscii(x));
    }
}
```

Used as:

```csharp
bool isAscii = someValue.IsAscii;
```

**Operator Extensions:**
Operators can be extended (available preview 7):

```csharp
static class PathExtensions {
    extension(string) {
        public static string operator /(string left, string right) => Path.Combine(left, right);
    }
}
```

Usage:

```csharp
var fullPath = "part1" / "part2" / "test.txt";
```

---

### Disambiguating Extension Members

Overloading and ambiguity are handled similarly to current extension methods:

- Instance extension methods: invoke statically, passing the instance.
- Static extension methods: call as static zero-parameter.
- Instance extension properties: call with `get_` prefix method, passing the instance.
- Static extension properties: call with `get_` prefix statically.

Example invocations:

```csharp
bool empty = EnumerableExtensions.IsEmpty([]);
bool hasValue = StringExtensions.HasValue("something");
bool isAscii = StringExtensions.get_IsAscii("something");
```

Behind the scenes, properties are implemented as `get_`/`set_` methods, as observable in disassemblers.

---

### Case Study: NetEscapades.EnumGenerators NuGet Package

NetEscapades.EnumGenerators is a package for generating extension methods and helpers for enums, providing fast reflection-style helpers.

Example definition:

```csharp
[EnumExtensions]
public enum MyColours { Red, Green, Blue }
```

Default generated methods:

```csharp
public static partial class MyColoursExtensions {
    public static string ToStringFast(this global::MyColours value) { }
    public static int AsUnderlyingType(this global::MyColours value) { }
    public static bool IsDefined(global::MyColours value) { }
    public static global::MyColours Parse(string? name) { }
}
```

Prior to C# 14, static methods (e.g., Parse, IsDefined) were called via the type:

```csharp
var colour = MyColoursExtensions.Parse("Red");
```

With C# 14’s extension syntax:

```csharp
public static partial class MyColoursExtensions {
    public static string ToStringFast(this global::MyColours value) { }
    public static int AsUnderlyingType(this global::MyColours value) { }
    extension(global::MyColours) {
        public static bool IsDefined(global::MyColours value) { }
        public static global::MyColours Parse(string? name) { }
    }
}
```

Now, you can access these directly from the enum:

```csharp
var colour = MyColours.Parse("Red");
```

---

### Summary

C# 14 brings extension members to .NET, broadening the types of members you can add to existing types via extension syntax. Conversion from existing extension methods is optional but straightforward. New possibilities include static extension methods, static/instance extension properties, and eventually operator extensions. Andrew’s NetEscapades.EnumGenerators illustrates practical adoption and advantages, especially for library authors aiming for more idiomatic APIs.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-3-csharp-14-extensions-members/)
