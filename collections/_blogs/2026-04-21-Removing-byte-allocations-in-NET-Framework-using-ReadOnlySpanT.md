---
external_url: https://andrewlock.net/removingbyte-array-allocations-in-dotnet-framework-using-readonlyspan-t/
primary_section: dotnet
date: 2026-04-21 10:00:00 +00:00
feed_name: Andrew Lock's Blog
title: Removing byte[] allocations in .NET Framework using ReadOnlySpan<T>
section_names:
- dotnet
author: Andrew Lock
tags:
- .NET
- .NET Framework
- Allocations
- Blogs
- C#
- C# 11
- C# 8
- Collection Expressions
- Dotpeek
- Garbage Collection
- IL
- ILSpy
- JIT
- MSIL
- Net48
- NuGet
- Performance
- ReadOnlySpan<T>
- Rider
- RuntimeHelpers.CreateSpan
- RuntimeHelpers.InitializeArray
- SharpLab
- Span<T>
- Stackalloc
- System.Memory
- UTF 8 String Literals
---

Andrew Lock explains how to avoid some static byte[] allocations—even on .NET Framework—by returning ReadOnlySpan<byte> backed by embedded assembly data, and validates the behavior by inspecting generated IL, with a clear rundown of the sharp edges that can accidentally reintroduce allocations.<!--excerpt_end-->

# Removing byte[] allocations in .NET Framework using ReadOnlySpan<T>

In this post, Andrew Lock shows a simple pattern that can eliminate certain `byte[]` allocations across target frameworks (including .NET Framework), explains what the C# compiler generates under the hood, and highlights the cases where the pattern *doesn’t* work (or becomes actively allocation-heavy).

## Span<T> / ReadOnlySpan<T> refresher

`Span<T>` and `ReadOnlySpan<T>` let you work with a *view* over existing memory without copying it.

A common example is slicing strings without allocating substrings:

- Instead of `Substring()` (allocates new strings)
- Use `AsSpan()` to work with `ReadOnlySpan<char>` segments

They’re also useful for temporary buffers. With `stackalloc`, you can keep small buffers off the heap:

```csharp
Span<byte> buffer = requiredSize <= 256
    ? stackalloc byte[requiredSize]
    : new byte[requiredSize];
```

Even on older runtimes like .NET Framework, you can use spans via the `System.Memory` NuGet package:

- https://www.nuget.org/packages/System.Memory

## The optimization: replace static byte[] fields with ReadOnlySpan<byte> properties

A typical approach to store static data is a `static readonly byte[]`:

```csharp
public static class MyStaticData
{
    private static readonly byte[] ByteField = new byte[] { 1, 2, 3, 4 };
}
```

That allocates and initializes the array the first time the type is used.

Starting in C# 8, if you only need a read-only view, you can expose a `ReadOnlySpan<byte>` property:

```csharp
public static class MyStaticData
{
    // Before
    private static readonly byte[] ByteField = new byte[] { 1, 2, 3, 4 };

    // After
    private static ReadOnlySpan<byte> ReadOnlySpanProp => new byte[] { 1, 2, 3, 4 };
}
```

Even though it *looks* like it would allocate a new array on every call, the compiler recognizes this pattern (for specific element types and constant data) and instead:

- embeds the bytes into the assembly
- returns a `ReadOnlySpan<byte>` that points directly to embedded data

No heap allocation, no GC pressure, and no “first use” array initialization cost.

This also applies to certain locals:

```csharp
public static void TestData()
{
    // Looks like it allocates, but it doesn't (for supported cases)
    ReadOnlySpan<byte> arr = new byte[] { 0, 1, 2 };
}
```

### Also works with UTF-8 string literals

UTF-8 string literals in C# 11 are represented as bytes, so this can also be allocation-free:

```csharp
public static class MyStaticData
{
    private static ReadOnlySpan<byte> ReadOnlySpanUtf8 => "Hello world"u8;
}
```

## Verifying it on .NET Framework by inspecting IL

Lock demonstrates this on .NET Framework by creating a project targeting `net48`, using the latest C# version, and adding `System.Memory`.

### Example project file

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net48</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="System.Memory" Version="4.6.3" />
  </ItemGroup>
</Project>
```

### Sample code

```csharp
public static class MyStaticData
{
    private static ReadOnlySpan<byte> ReadOnlySpanProp => new byte[] { 1, 2, 3, 4 };
    private static ReadOnlySpan<byte> ReadOnlySpanUtf8 => "Hello world"u8;
}
```

### Tooling options

- SharpLab: https://sharplab.io/
- ILSpy: https://github.com/icsharpcode/ilspy
- dotPeek: https://www.jetbrains.com/decompiler/
- Rider: https://www.jetbrains.com/rider/

### What the IL shows (key point)

For the optimized `byte[]` case, the IL does **not** contain `newarr`, `InitializeArray()`, or `ToArray()`.

Instead it:

- loads the address of embedded data
- loads the length
- calls the `ReadOnlySpan<T>(void*, int)` constructor

Lock includes an IL example showing `ldsflda` + length + `newobj` for `ReadOnlySpan<byte>`.

## Sharp edges (ways to fall off a performance cliff)

Lock emphasizes the pattern is only safe in a narrow set of cases:

- the element type is `byte`, `sbyte`, or `bool`
- all values are constants
- the data must be immutable (`ReadOnlySpan<T>`, not `Span<T>`)

### 1) Only byte-sized primitives (byte/sbyte/bool) get the compiler trick

For other element types (e.g., `int`) on .NET Framework and older runtimes, the compiler typically emits code that allocates an array and initializes it.

```csharp
public static class MyStaticData
{
    // Allocates on .NET Framework and < .NET 7
    private static ReadOnlySpan<int> ReadOnlySpanPropInt => new int[] { 1, 2, 3, 4 };
}
```

He shows IL containing `newarr` and `RuntimeHelpers.InitializeArray`, plus a caching approach (create once, store in a static field).

On .NET 7+, a runtime API enables a true no-allocation approach for broader types via `RuntimeHelpers.CreateSpan`.

### 2) Values must be constants

If one element comes from a non-const value (even a `static readonly`), the compiler can fall back to “allocate a new array every time” behavior:

```csharp
public static class MyStaticData
{
    private static readonly byte One = 1;

    // Compiles, but can allocate on every access
    private static ReadOnlySpan<byte> ReadOnlySpanPropNonConstant => new byte[] { One, 2, 3, 4 };
}
```

The IL shown includes:

- `newarr`
- `InitializeArray`
- then a `stelem` to patch the non-constant element

### 3) Don’t return Span<T> (mutable)

Returning mutable `Span<T>` prevents the compiler from safely returning a view of embedded read-only data, so it allocates:

```csharp
public static class MyStaticData
{
    private static Span<byte> SpanProp => new byte[] { 1, 2, 3, 4 };
}
```

The IL shown includes `newarr` + `InitializeArray`.

## Using collection expressions as guardrails (sometimes)

Lock notes that collection expressions can help catch some of the problematic patterns at compile time for **static properties**:

```csharp
public static class MyStaticData
{
    // Doesn't compile (good): non-constant element
    private static ReadOnlySpan<byte> ReadOnlySpanPruopNonConstantCollectionExpression => [One, 2, 3, 4];

    // Doesn't compile (good): mutable Span<byte>
    private static Span<byte> SpanPropCollectionExpression => [1, 2, 3, 4];
}
```

This can produce `CS9203` errors about the collection expression potentially being exposed outside the current scope.

However, for **locals**, collection expressions don’t necessarily protect you on older targets (e.g., .NET Framework / < .NET 8):

```csharp
public static class MyStaticData
{
    private static readonly byte One = 1;

    public static void TestData()
    {
        // On .NET Framework:
        ReadOnlySpan<int> intArray = [1, 2, 3, 4];
        ReadOnlySpan<byte> nonConstantArray = [One, 2, 3, 4];
        Span<byte> spanArray = [1, 2, 3, 4];
    }
}
```

Lock includes IL showing:

- the `int` case uses cached static array initialization
- the `nonConstantArray` and `Span<byte>` cases allocate a new array each time

## Conclusion

- Replacing `static readonly byte[]` with `static ReadOnlySpan<byte>` properties can remove initialization cost and allocations, even on .NET Framework, because it’s largely a **compiler feature** (assuming `ReadOnlySpan<T>` exists via `System.Memory`).
- It’s easy to accidentally reintroduce allocations:
  - wrong element type (`int` etc., unless .NET 7+)
  - any non-constant element
  - returning `Span<T>` instead of `ReadOnlySpan<T>`
- Collection expressions can provide some protection for static properties, but not all cases (especially locals on older targets).

Related discussions/issues linked by the author:

- [Analyzer: Unexpected allocation when converting arrays to spans](https://github.com/dotnet/runtime/issues/69577)
- [Proposal: ReadOnlySpan initialization from static data](https://github.com/dotnet/csharplang/issues/5295)
- [compile-time readonly arrays, const T[]](https://github.com/dotnet/csharplang/discussions/955)
- [ReadOnlySpan initialization from static data – language design notes](https://github.com/dotnet/csharplang/blob/main/meetings/2023/LDM-2023-10-09.md#readonlyspan-initialization-from-static-data)
- [API Proposal: ReadOnlySpan CreateSpan(RuntimeFieldHandle)](https://github.com/dotnet/runtime/issues/60948)

## Author

Andrew Lock | .NET Escapades

[Read the entire article](https://andrewlock.net/removingbyte-array-allocations-in-dotnet-framework-using-readonlyspan-t/)

