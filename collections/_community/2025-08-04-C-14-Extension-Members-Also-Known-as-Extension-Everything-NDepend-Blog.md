---
external_url: https://www.reddit.com/r/dotnet/comments/1mhbukq/c_14_extension_members_also_known_as_extension/
title: 'C# 14 Extension Members: Also Known as Extension Everything - NDepend Blog'
author: PatrickSmacchia
feed_name: Reddit DotNet
date: 2025-08-04 12:18:27 +00:00
tags:
- .NET
- C# 14
- C# Features
- Developer Productivity
- Extension Members
- Extension Methods
- Language Design
- NDepend
- Operators
- Properties
- Community
section_names:
- dotnet
primary_section: dotnet
---
PatrickSmacchia delves into the upcoming C# 14 feature of Extension Members, offering insights on how it expands the language's extensibility.<!--excerpt_end-->

# Overview

PatrickSmacchia's blog post on the NDepend Blog explores the anticipated C# 14 feature known as Extension Members, also informally referred to as 'Extension Everything.' This new capability builds upon the well-established concept of extension methods in C#, expanding it to include properties and operators. The article aims to inform developers about how these enhancements will improve the expressiveness and modularity of their code.

## What Are Extension Members?

Traditionally, C# supported extension methods, allowing developers to 'extend' existing types with additional methods without modifying their source code. Extension Members generalize this concept, making it possible to also add extension properties and extension operators to existing types, further increasing the language's flexibility.

## Motivation and Benefits

The primary motivation behind Extension Members is to empower developers to adapt types in a non-intrusive manner, ensuring cleaner and more maintainable code. This feature will be especially valuable for:

- Adapting third-party or system types with custom behaviors.
- Creating fluent APIs or adding cross-cutting features.
- Improving code separation and modularity.

## Usage Scenarios

### Extension Properties

Developers will be able to define computed properties on existing types, much like extension methods. This provides a convenient, idiomatic way of exposing cross-type data or functionality.

### Extension Operators

By defining operators as extensions, user code can implement custom operator logic for types that do not already support them, enhancing expressiveness in certain domains (e.g., mathematical or domain-driven code).

## Language Syntax and Examples

While the article highlights conceptual usage and benefits, concrete syntax is expected in the official C# 14 documentation. Sample pseudo-code might look like:

```csharp
public static class StringExtensions {
    public static int WordCount(this string s) => ...;
    public static string Capitalize(this string s) => ...;
    public static operator +(this string s1, string s2) => ...;
}
```

## Impact on Codebases

Extension Members are set to have a significant impact where:

- Enhancements to legacy types are necessary.
- Extension methods do not sufficiently express intent.
- Operator overloading is desired without altering original types.

## Conclusion

Extension Members ('Extension Everything') represent a major evolution for C# developers seeking greater extensibility and code reuse. The feature will soon be available in C# 14, and the blog encourages developers to explore its possibilities for cleaner, more adaptive code.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mhbukq/c_14_extension_members_also_known_as_extension/)
