---
layout: post
title: Easier Reflection with [UnsafeAccessorType] in .NET 10
author: Andrew Lock
canonical_url: https://andrewlock.net/exploring-dotnet-10-preview-features-9-easier-reflection-with-unsafeaccessortype/
viewing_mode: external
feed_name: Andrew Lock's Blog
feed_url: https://andrewlock.net/rss.xml
date: 2025-11-04 10:00:00 +00:00
permalink: /coding/blogs/Easier-Reflection-with-UnsafeAccessorType-in-NET-10
tags:
- .NET 10
- .NET 8
- .NET 9
- Access Modifiers
- APIs
- C#
- Field Access
- Generics
- Internal Classes
- Method Access
- Performance
- Private Members
- Reflection
- System.Reflection
- UnsafeAccessor
- UnsafeAccessorType
section_names:
- coding
---
Andrew Lock demonstrates improvements to the [UnsafeAccessor] mechanism in .NET 10, focusing on how [UnsafeAccessorType] enables accessing private fields and methods for types without direct references.<!--excerpt_end-->

# Easier Reflection with `[UnsafeAccessorType]` in .NET 10

Andrew Lock explores the new `[UnsafeAccessorType]` attribute in .NET 10 and compares it with earlier .NET versions, showing how it simplifies and speeds up access to private members—even when you cannot reference their types at compile time.

## Background: `[UnsafeAccessor]` in .NET 8 and 9

- `[UnsafeAccessor]` was introduced in .NET 8 to simplify accessing private members versus using the old reflection APIs.
- .NET 9 added support for generics but required all types in signatures to be visible and accessible at compile time.
- Standard usage involves defining extern methods with `[UnsafeAccessor]` to get or set fields, or invoke methods/constructors based on the member type.

**Example: Accessing a Private Field**

```csharp
static class Accessors<T>
{
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
    public static extern ref T[] GetItems(List<T> list);
}
```

## Limitations in .NET 9

- You cannot use `[UnsafeAccessor]` if you can't reference the required types (such as internal/private classes in third-party libraries).
- Real-world scenarios (like instrumentation libraries or framework cross-dependencies) force you to revert to slower classic reflection or more complex alternatives.

## What’s New in .NET 10: `[UnsafeAccessorType]`

- `[UnsafeAccessorType]` lets you specify required types as fully-qualified strings, so types need not be visible/referenced in your assembly.
- Works in combination with `[UnsafeAccessor]`, allowing non-referenced types for method parameters, returns, and more (as long as the signature uses `object` plus an attribute specifying the real type).

**Example:**

```csharp
[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "GetPrivate")]
[return: UnsafeAccessorType("PrivateClass")]
static extern object GetByMethod(PublicClass instance);

[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_SomeValue")]
static extern string GetSomeValue([UnsafeAccessorType("PrivateClass")] object instance);
```

- Attribute values must use fully qualified type names (namespace, assembly, generic notation).
- See [the runtime's unit tests](https://github.com/dotnet/runtime/blob/main/src/tests/baseservices/compilerservices/UnsafeAccessors/UnsafeAccessorsTests.cs) for extensive samples.

## Use Cases and Complex Scenarios

- Works for calling constructors, static methods, instance methods, and even handling some generic scenarios.
- Examples are provided from synthetic, library, and real-world contexts.
- Useful for code instrumentation, tests, or cross-assembly hacks where full type information isn’t directly available.

## Remaining Limitations

- You cannot use `[UnsafeAccessorType]` for fields or methods that return a `ref` where the type isn’t directly referenceable.
- It is not possible to represent type arguments if you can't reference them, making some scenarios impossible without classic reflection.
- Attempting to use `[UnsafeAccessorType]` incorrectly (e.g., for inaccessible `ref` fields) results in runtime errors (`NotSupportedException`).

## Summary

`.NET 10`’s `[UnsafeAccessorType]` builds on `[UnsafeAccessor]` to allow easier, more performant access to private class members without direct references—but some hard reflection-only cases remain. Previous restrictions have been eased, especially for method and constructor invocations, making it a powerful addition for advanced .NET hackers, library authors, and toolmakers.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-9-easier-reflection-with-unsafeaccessortype/)
