---
external_url: https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/
title: Running .NET in the Browser Without Blazor Using WASM
author: Andrew Lock
viewing_mode: external
feed_name: Andrew Lock's Blog
date: 2025-08-12 10:00:00 +00:00
tags:
- .NET
- .NET 10
- .NET 9
- ASP.NET Core
- Blazor
- Browser Apps
- C#
- Client Side Development
- Fingerprinting
- Front End
- Globalization
- JavaScript Interop
- JSExport
- JSImport
- Performance Optimization
- Release Process
- Template
- WASM
- WebAssembly
section_names:
- coding
---
Andrew Lock explains how to run .NET code in the browser without using Blazor, focusing on browser-side WebAssembly and JavaScript interop techniques, with details relevant to .NET 9 and .NET 10.<!--excerpt_end-->

# Running .NET in the Browser Without Blazor Using WASM

**Author:** Andrew Lock

## Introduction

This article explains how developers can run .NET applications directly in the browser using WebAssembly (WASM) infrastructure, without relying on the Blazor framework. It provides background on .NET and WASM, covers required setup for experimenting with this lower-level approach, and discusses practical considerations for development and deployment.

## Background: WASM and .NET

While Blazor is the primary and most well-known .NET web-assembly based framework, .NET's WASM support goes deeper. Developers can:

- Run .NET on WASM _in the browser_ without Blazor.
- Run .NET on WASM within a Node.js process (server-side).
- Build WASI-compatible components to interop with other languages.
- Integrate Blazor with external JavaScript SPA frameworks.

The post focuses on running .NET directly in the browser without Blazor.

## Installing Experimental WASM Templates

The lower-level WASM browser templates are experimental. To use the .NET 10 preview templates:

```bash
dotnet new install Microsoft.NET.Runtime.WebAssembly.Templates.net10
```

Alternative installations include .NET 8 or 9 packages or installing the `wasm-experimental` workload for broader support.

For AOT compilation support (improved runtime speed at a cost of larger file size), also install the `wasm-tools` workload.

## Creating a .NET WASM Application

After installing the template, create a new browser WASM app with:

```bash
dotnet new wasmbrowser
```

This generates files for a sample app, which you can run locally via `dotnet run`. The sample is a stopwatch that starts on page load and provides pause and reset buttons.

## Key Concepts in the Template

- **Interop attributes:** `[JSImport]` allows .NET code to _call_ JavaScript, while `[JSExport]` allows JavaScript to _call_ into .NET code. Source generators automate the binding/marshalling process.
- **Program.cs:** Entry point, starts the sample, sets up rendering updates.
- **StopwatchSample:** Implements pause/reset logic and exposes methods for interop.

### Example Interop Methods

```csharp
[JSImport("dom.setInnerText", "main.js")]
internal static partial void SetInnerText(string selector, string content);

[JSExport]
internal static void Reset() { ... }
```

- **HTML & JavaScript:**
  - Minimal HTML, with script and link tags set up for WASM and asset preloading.
  - `main.js` sets up .NET runtime, wires up UI events to exported .NET methods, and boots the application.

## Asset Fingerprinting and Publishing

New in .NET 10 templates is support for client-side fingerprinting (cache busting) of static assets. Key config items include:

- Add `<script type="importmap"></script>` to HTML.
- Use `#[.{fingerprint}]` in script references.
- Set `<OverrideHtmlAssetPlaceholders>true</OverrideHtmlAssetPlaceholders>` and `<StaticWebAssetFingerprintPattern ... />` in the csproj.

A bug affecting JS file fingerprinting can be fixed by providing an `Expression="#[.{fingerprint}]!"` attribute.

## Reducing Published App Size

- Default build sizes: ~6.8MB (uncompressed), ~2MB (brotli).
- Enabling globalization invariant mode in the csproj reduces size further:

```xml
<InvariantGlobalization>true</InvariantGlobalization>
```

## Conclusion

Low-level WASM browser hosting for .NET is available, offering fine control without the abstraction of Blazor. This approach may suit use cases where Blazor is unnecessary or too heavy, with the flexibility to optimize startup, output size, and interop behavior. .NET 10 introduces handy improvements for asset handling and publishing.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/running-dotnet-in-the-browser-without-blazor/)
