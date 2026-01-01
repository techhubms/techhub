---
layout: "post"
title: "Building sleep-pc: A .NET Native AOT Tool for Automating Windows Sleep"
description: "This in-depth post by Andrew Lock walks through creating 'sleep-pc,' a small, native AOT-compiled .NET tool that puts a Windows PC to sleep after a specified timeout. The guide covers Win32 API usage, command-line parsing with ConsoleAppFramework, Native AOT support, project packaging, and optimizing for small binary size, providing a practical look at modern .NET 8+ capabilities and tool development."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/sleep-pc-a-dotnet-tool-to-make-windows-sleep-after-a-timeout/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-09-23 10:00:00 +00:00
permalink: "/2025-09-23-Building-sleep-pc-A-NET-Native-AOT-Tool-for-Automating-Windows-Sleep.html"
categories: ["Coding"]
tags: [".NET", ".NET 10", ".NET 8", ".NET Core", "AOT", "Blogs", "C#", "Coding", "Command Line Parsing", "Command Line Tool", "ConsoleAppFramework", "Csproj", "DllImport", "Native AOT", "NuGet", "P/Invoke", "PackAsTool", "SetSuspendState", "Source Generators", "Task.Delay", "Win32 API", "Windows"]
tags_normalized: ["dotnet", "dotnet 10", "dotnet 8", "dotnet core", "aot", "blogs", "csharp", "coding", "command line parsing", "command line tool", "consoleappframework", "csproj", "dllimport", "native aot", "nuget", "pslashinvoke", "packastool", "setsuspendstate", "source generators", "taskdotdelay", "win32 api", "windows"]
---

Andrew Lock details the development of 'sleep-pc,' a native AOT .NET tool for putting Windows PCs to sleep after a timeout, highlighting his approach, toolchain choices, and packaging for NuGet.<!--excerpt_end-->

# sleep-pc: A .NET Native AOT Tool to Make Windows Sleep After a Timeout

**Author:** Andrew Lock  
**GitHub:** [andrewlock/sleep-pc](https://github.com/andrewlock/sleep-pc)  
**NuGet:** [sleep-pc](https://www.nuget.org/packages/sleep-pc)

## Overview

This post describes the creation of `sleep-pc`, a small command-line tool for Windows, written in C# and built with the latest .NET (8/10) Native AOT features. The tool puts your Windows PC to sleep after a configurable timeout, using the Win32 `SetSuspendState` API. Andrew Lock walks through the design and the lessons learned packaging the tool for NuGet as a .NET CLI tool.

---

## Background: Why Automate Sleep?

Like many, Andrew found it difficult to make his Windows laptop reliably sleep after certain actions (such as finishing a playlist in Windows Media Player Legacy). Tired of power management struggles, he decided to automate the process: _a tiny tool that sleeps the laptop after a specified period._

## First Attempt: Simple Win32 Sleep Via P/Invoke

A proof-of-concept version simply waits, then calls the Win32 API:

```csharp
using System.Runtime.InteropServices;

var wait = TimeSpan.FromSeconds(60 * 60); // 1 hour
Console.WriteLine($"Waiting for {wait}");
Thread.Sleep(wait);
Console.WriteLine("Sleeping!");
SetSuspendState(false, false, false);

[DllImport("PowrProf.dll", SetLastError = true)]
static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
```

This uses `SetSuspendState` from `PowrProf.dll` to trigger sleep. Tweaks around force/hibernate state were also explored.

## Improving The Tool: CLI Arguments & Native AOT

To make the tool user-friendly and robust:

- Added argument parsing & help text, leveraging [ConsoleAppFramework](https://github.com/Cysharp/ConsoleAppFramework) (fast, AOT-safe, zero-reflection CLI toolkit).
- Compiled using Native AOT features enabled by .NET 8/10.
- Published as a .NET global tool (installable via NuGet).

**Sample argument parsing:**

```csharp
await ConsoleApp.RunAsync(args, App.Countdown);

public static async Task Countdown(
  [Range(1, 99 * 60 * 60)] uint sleepDelaySeconds = 3600,
  bool dryRun = false,
  CancellationToken ct = default)
{
  // timer logic...
}
```

## Native AOT & Trimming

- Added `<PublishAot>true</PublishAot>` in the `.csproj` to publish a natively compiled, small executable (3.3MB).
- Used `.NET 10` tools features for packaging, including a compromise NuGet pack for maximum compatibility.
- Explored further trimming and analyzed binary size with [Sizoscope](https://github.com/MichalStrehovsky/sizoscope).

**csproj NuGet packing highlights:**

```xml
<PackAsTool>true</PackAsTool>
<ToolCommandName>sleep-pc</ToolCommandName>
<PackageId>sleep-pc</PackageId>
<PackageVersion>0.1.0</PackageVersion>
<Authors>Andrew Lock</Authors>
```

## Console Output Polish

- Chose not to use Spectre.Console (not fully Native AOT compatible).
- Implemented an in-place countdown timer in the console for better user feedback via simple backspace tricks.

## Packaging and Installation

- Packaged for NuGet with dual support: framework-dependent (`net8.0`) and platform-specific Native AOT (`net10.0` win-x64).
- Users can install globally via: `dotnet tool install -g sleep-pc`.

## Summary

Andrew Lock provides a practical, well-documented guide for small utility development in modern .NET. The post covers:

- P/Invoke interop
- Command-line app design
- Native AOT compilation and trimming
- NuGet tool packaging with compatibility for .NET 8/10
- Lessons for minimizing binary size and maximizing performance

**Reference links:**  

- [Full sleep-pc code on GitHub](https://github.com/andrewlock/sleep-pc)  
- [NuGet package page](https://www.nuget.org/packages/sleep-pc)

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/sleep-pc-a-dotnet-tool-to-make-windows-sleep-after-a-timeout/)
