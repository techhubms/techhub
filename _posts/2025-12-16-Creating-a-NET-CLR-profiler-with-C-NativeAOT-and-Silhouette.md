---
layout: "post"
title: "Creating a .NET CLR profiler with C# NativeAOT and Silhouette"
description: "This article by Andrew Lock walks through building a basic .NET CLR profiler in C# using the Silhouette library and NativeAOT. It covers the essentials of unmanaged profiling APIs, project setup, and demonstrates how to hook events such as assembly loading in a test console app. Readers learn how NativeAOT enables profiling in pure C#, bypassing the usual need for unmanaged C++ code and lowering the barrier to custom CLR profiling."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/creating-a-dotnet-profiler-using-csharp-with-silhouette/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-12-16 10:00:00 +00:00
permalink: "/2025-12-16-Creating-a-NET-CLR-profiler-with-C-NativeAOT-and-Silhouette.html"
categories: ["Coding"]
tags: [".NET CLR Profiler", ".NET Core", "Assembly Loading", "C#", "Class Library", "Coding", "Console Application", "COR PRF MONITOR", "Custom Profiler", "Debugging APIs", "DotTrace", "Entry Points", "HResult", "ICorProfilerCallback", "Metadata APIs", "Native AOT", "NativeAOT", "Performance", "Performance Profiling", "PerfView", "Posts", "Profiling APIs", "Project Setup", "Silhouette Library", "Source Generators", "Unmanaged APIs", "Visual Studio Profilers", "Win32Exception"]
tags_normalized: ["dotnet clr profiler", "dotnet core", "assembly loading", "csharp", "class library", "coding", "console application", "cor prf monitor", "custom profiler", "debugging apis", "dottrace", "entry points", "hresult", "icorprofilercallback", "metadata apis", "native aot", "nativeaot", "performance", "performance profiling", "perfview", "posts", "profiling apis", "project setup", "silhouette library", "source generators", "unmanaged apis", "visual studio profilers", "win32exception"]
---

Andrew Lock demonstrates how to create a simple .NET CLR profiler using C# and NativeAOT with the Silhouette library, showing how to hook into assembly load events for custom profiling.<!--excerpt_end-->

# Creating a .NET CLR profiler with C# NativeAOT and Silhouette

Andrew Lock explores how to build a simple .NET profiler using C# rather than C++, leveraging the Silhouette library and NativeAOT compilation. The article starts by introducing profiling APIs within .NET, which are typically accessed via native C++ code, but here, Silhouette makes managed C# development practical.

## Key Concepts

- **.NET Profiling APIs**: Three families—debugging APIs, metadata APIs, and profiling APIs. Profiling APIs are central for monitoring and instrumenting application behavior at runtime.
- **NativeAOT**: Allows .NET apps to compile to native binaries, a requirement for attaching as a CLR profiler.
- **Silhouette Library**: Simplifies exposing managed types as C++ interfaces and handles the boilerplate for the profiler DLL interface.

## Project Setup

1. **Create Solution**:
   - Class library for the profiler (`SilhouetteProf`)
   - Console app as test target (`TestApp`)
   - Added Silhouette via NuGet to the profiler project
2. **Modify Project Properties**:
   - `PublishAot=true`, `AllowUnsafeBlocks=true` (NativeAOT requirements)
3. **Add Profiler Class**:
   - Implements `CorProfilerCallbackBase` derivatives
   - Decorated with `[Profiler(GUID)]` for registration

## Profiler Implementation

- **Initialize Method**: Checks interface compatibility and sets event mask for profiling events:

```csharp
protected override HResult Initialize(int iCorProfilerInfoVersion) {
  Console.WriteLine("[SilhouetteProf] Initialize");
  if (iCorProfilerInfoVersion < 5) {
    return HResult.E_FAIL;
  }
  return ICorProfilerInfo5.SetEventMask(COR_PRF_MONITOR.COR_PRF_MONITOR_ALL);
}
```

- **Assembly Load Hook**: Logs loaded assemblies using the profiling info API and catches exceptions:

```csharp
protected override HResult AssemblyLoadFinished(AssemblyId assemblyId, HResult hrStatus) {
  try {
    AssemblyInfoWithName assemblyInfo = ICorProfilerInfo5.GetAssemblyInfo(assemblyId).ThrowIfFailed();
    Console.WriteLine($"[SilhouetteProf] AssemblyLoadFinished: {assemblyInfo.AssemblyName}");
    return HResult.S_OK;
  } catch (Win32Exception ex) {
    Console.WriteLine($"[SilhouetteProf] AssemblyLoadFinished failed: {ex}");
    return ex.NativeErrorCode;
  }
}
```

- **Other Event Overrides**: Additional events like shutdown, class load, etc., are implemented similarly.

## Publishing and Testing

1. **Publish Projects**:
   - Test app published using `dotnet publish` for correct isolation
   - Profiler published with NativeAOT for target runtime (e.g., win-x64)
2. **Set Environment Variables**:
   - Example for .NET Core:
     - `CORECLR_ENABLE_PROFILING=1`
     - `CORECLR_PROFILER={GUID}`
     - `CORECLR_PROFILER_PATH=abs/path/to/profiler.dll`
3. **Run Application**:
   - Profiler logs assembly load events as .NET runtime executes the test app

## Insights and Trade-Offs

- Silhouette makes prototyping and custom CLR profiling accessible in C#.
- Understanding profiling APIs is still necessary for advanced scenarios; library helps manage entrypoints and event hooks.
- NativeAOT and managed profilers are practical for proof-of-concept and developer-focused tools but may not yet be optimal for production-level diagnostics.

## Summary

This guide demonstrates that CLR profiling can be approachable for C# developers. Silhouette streamlines interop with unmanaged APIs, and NativeAOT removes the typical C++ barrier. While this implementation is basic, it lays groundwork for deeper exploration and custom profilers tailored to .NET apps.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/creating-a-dotnet-profiler-using-csharp-with-silhouette/)
