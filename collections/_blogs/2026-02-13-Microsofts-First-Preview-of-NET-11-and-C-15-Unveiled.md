---
layout: "post"
title: "Microsoft's First Preview of .NET 11 and C# 15 Unveiled"
description: "This article covers Microsoft's announcement of the .NET 11 and C# 15 preview for the 2026 release, detailing major runtime changes, language feature updates, and the ongoing deprecation of Mono in favor of CoreCLR. The preview illustrates the evolution of the .NET ecosystem and highlights the opportunities and complexities brought by new collection expression argument features in C# 15."
author: "DevClass.com"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.devclass.com/development/2026/02/13/net-train-keeps-rolling-with-first-showing-of-2026-release/4090277"
viewing_mode: "external"
feed_name: "DevClass"
feed_url: "https://devclass.com/feed/"
date: 2026-02-13 11:58:16 +00:00
permalink: "/2026-02-13-Microsofts-First-Preview-of-NET-11-and-C-15-Unveiled.html"
categories: ["Coding"]
tags: [".NET 11", ".NET WebAssembly SDK", "Android Development", "Backward Compatibility", "Blazor", "Blogs", "C# 15", "Coding", "Collection Expression Arguments", "CoreCLR", "Cross Platform Development", "Language Features", "MAUI", "Microsoft", "Microsoft Developer Tools", "Mono Runtime", "Xamarin", "Zstandard Compression"]
tags_normalized: ["dotnet 11", "dotnet webassembly sdk", "android development", "backward compatibility", "blazor", "blogs", "csharp 15", "coding", "collection expression arguments", "coreclr", "cross platform development", "language features", "maui", "microsoft", "microsoft developer tools", "mono runtime", "xamarin", "zstandard compression"]
---

DevClass.com reports on Microsoft's first preview of .NET 11 and C# 15, outlining the shift from Mono, CoreCLR enhancements, and the new C# collection expression arguments feature for developers.<!--excerpt_end-->

# Microsoft's First Preview of .NET 11 and C# 15 Unveiled

**Published by DevClass.com**

Microsoft has announced the first preview releases of both .NET 11 and C# 15, kicking off the 2026 release cycle. This preview provides a look at significant runtime and language features that developers can begin evaluating ahead of the November general availability target.

## Key Platform Updates

- **Support Model**: .NET 11 will be supported for two years under Standard Term Support (STS).
- **Mono Runtime Reduction**: Microsoft continues its effort to lessen use of the Mono runtime, a cross-platform implementation of .NET acquired with Xamarin. Mono still underpins certain mobile (MAUI) and WebAssembly projects, but with .NET 11, CoreCLR—the main .NET runtime—becomes:
  - A target in .NET WebAssembly SDK (impacting some Blazor variants)
  - The default runtime for Android builds (migrating from Mono in .NET 10, where CoreCLR was an experimental feature)
- **Mono Maintenance**: Mono is maintained by Microsoft as part of the .NET codebase, even after the core project was transferred to WineHQ.
- **Library Improvements**:
  - Native support for Zstandard compression brings better performance to .NET libraries.

## What's New in C# 15

- C# 15 ships alongside every major .NET release. The preview includes support for **collection expression arguments**:
  - Enables developers to customize collections at instantiation time (e.g., setting capacity or using a custom comparer).
  - Syntax example:

    ```csharp
    List names = [with(capacity: values.Count * 2), .. values];
    ```

  - Maintains backward compatibility: any conflicts with pre-existing `with` methods can be resolved using `@with`.
- The new feature is subject to ongoing debate due to concerns about language complexity, but improvements were finalized in a recent Language Design Meeting (LDM).

## Community Discussion

The preview sparked discussions among the developer community, particularly regarding whether the new collection syntax maintains C#'s traditional simplicity or leans toward excessive complexity. Microsoft developers responded by addressing core motivations and defending the implementation.

For further details or to try out the preview:

- [Microsoft's official preview announcement](https://devblogs.microsoft.com/dotnet/dotnet-11-preview-1/)
- [C# collection expression arguments proposal](https://github.com/dotnet/csharplang/blob/main/proposals/collection-expression-arguments.md)
- [Community discussion on the API](https://github.com/dotnet/csharplang/discussions/8886)

## Summary

.NET 11 and C# 15 continue the platform's evolution with advances aimed at performance, cross-platform compatibility, and richer language constructs. As with any language modernization, new features bring both opportunities and added complexity, requiring developers to assess the trade-offs as they adopt or migrate to the latest tools.

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/development/2026/02/13/net-train-keeps-rolling-with-first-showing-of-2026-release/4090277)
