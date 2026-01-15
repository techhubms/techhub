---
layout: post
title: 'ZaString: A Zero-Allocation Span-Based String Builder for .NET'
author: typicalyume
canonical_url: https://www.reddit.com/r/dotnet/comments/1mkqa37/stop_allocating_strings_i_built_a_spanpowered/
viewing_mode: external
feed_name: Reddit DotNet
feed_url: https://www.reddit.com/r/dotnet/.rss
date: 2025-08-08 09:05:50 +00:00
permalink: /coding/community/ZaString-A-Zero-Allocation-Span-Based-String-Builder-for-NET
tags:
- .NET
- API Design
- ArrayPool
- Benchmarks
- Buffer Management
- C#
- Coding
- Community
- Garbage Collection
- ISpanFormattable
- Logging
- NuGet
- Performance
- ReadOnlySpan
- Serialization
- Span
- Stackalloc
- StringBuilder
- ZaString
- Zero Allocation
- ZString
section_names:
- coding
---
typicalyume presents ZaString, a Span-powered zero-allocation string helper for .NET, and invites detailed feedback from the developer community on its API, safety, and performance.<!--excerpt_end-->

# ZaString: A Zero-Allocation Span-Based String Builder for .NET

**Author:** typicalyume

## Overview

ZaString is a newly introduced .NET library designed for building strings with zero heap allocations. It leverages `Span<char>`, `ReadOnlySpan<char>`, and `ISpanFormattable` to enable efficient string creation, particularly for scenarios where garbage collector (GC) pressure needs to be minimized, such as logging, serialization, and processing tight loops.

NuGet package: [ZaString 0.1.1](https://www.nuget.org/packages/ZaString/0.1.1)

---

## What is ZaString?

- Fluent API for composing text into a caller-provided buffer (array or `stackalloc`), strictly avoiding intermediate string allocations.
- Multiple `Append` overloads supporting spans, primitives, and any type implementing `ISpanFormattable` (e.g., numbers with format specifiers).
- Best suited for performance-critical .NET code where minimizing garbage collection is a priority.

### Developer Experience (DX) Focus

- Chainable `Append(...)` calls minimize ceremony.
- Compatible with `stackalloc` or pooled buffers the developer manages directly.
- Users control when (or whether) to produce an actual `string` instance; results can be consumed as spans where possible.

---

## Example Usage

```csharp
Span<char> buf = stackalloc char[256];
var z = ZaSpanString.CreateString(buf)
            .Append("order=")
            .Append(orderId)
            .Append("; total=")
            .Append(total, "F2")
            .Append("; ok=")
            .Append(true);
// Consume z as a span, or materialize only when needed
// var s = z.ToString(); // Call if/when you need an actual string
```

---

## Community Feedback Sought

- **API Surface:** Suggestions for naming consistency, ergonomics, and missing methods.
- **Safety:** Questions about best practices for bounds checking, format/culture awareness, and safe operation of low-level buffer manipulation.
- **Interop:** Possibilities for working with `String.Create`, `Rune` (UTF-8 pipelines), and `ArrayPool<char>` patterns.
- **Benchmarks:** Methods and scenarios for evaluating performance and GC impact.

---

## Discussion Highlights

- **Comparison with StringBuilder:** ZaString targets zero heap allocation, whereas StringBuilder, though optimized, can cause GC pressure due to allocations of intermediate and final string segments.
- **Benchmarks:** Reference to [Performance Section](https://github.com/CorentinGS/ZaString?tab=readme-ov-file#-performance) with reported timings and memory usage, demonstrating benefits relative to StringBuilder.
- **ZString Comparison:** Community notes similarities and key differences, suggesting deeper exploration for those familiar with ZString.
- **stackalloc Limitations:** Discussion around stack size limits (typically ~1MB), and consequences like potential StackOverflowException if very large spans are used.
- **Structural Design:** Essentially, ZaString wraps a `Span<char>` and an `int` tracking the span's active usage, abstracting away repetitive boilerplate for end-users.
- **Pooled Buffers:** Library supports interoperability with buffers from `ArrayPool<char>`, offering additional options for scenarios requiring larger or heap-based buffer pools.

---

## Open Questions

- How much real-world performance does switching to span-powered APIs offer over traditional string manipulation?
- What safety checks are appropriate for boundary and formatting logic?
- Where is the line between stack-allocated and heap-allocated buffer usage—especially for very large data?
- What other real-world scenarios or APIs should ZaString support to broaden its application?

---

## Further Reading & Resources

- [ZaString on NuGet](https://www.nuget.org/packages/ZaString/0.1.1)
- [ZaString Performance Section in GitHub README](https://github.com/CorentinGS/ZaString?tab=readme-ov-file#-performance)
- [Comparison: ZString](https://github.com/Cysharp/ZString)

---

## Community Invitation

The author, typicalyume, welcomes suggestions, reviews, and critical feedback, particularly from developers experienced in Span-heavy utilities and high-performance .NET codebases. Input on benchmarks, interop, and real-world use cases is highly encouraged.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mkqa37/stop_allocating_strings_i_built_a_spanpowered/)
