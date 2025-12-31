---
layout: "post"
title: "Solving Native Library Loading Issues for .NET Core on Alpine Linux"
description: "This article by Andrew Lock details a practical debugging journey for resolving a 'native library not found' error when running .NET Core 3.1 and .NET 5 applications on Alpine Linux 3.17. It explores the underlying cause related to .NET runtime ID resolution, documents the discovery steps using diagnostic tools, and presents environmental fixes for legacy .NET runtimes on modern Alpine versions."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/fixing-an-old-dotnet-core-native-library-loading-issue-on-alpine/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-08-26 10:00:00 +00:00
permalink: "/blogs/2025-08-26-Solving-Native-Library-Loading-Issues-for-NET-Core-on-Alpine-Linux.html"
categories: ["Coding"]
tags: [".NET 3.1", ".NET 5", ".NET Core", "Alpine Linux", "CI/CD", "Coding", "Containerization", "Cross Platform Development", "Datadog .NET Tracer", "Environment Variables", "LD DEBUG", "LD LIBRARY PATH", "Libe Sqlite3", "Linux Musl X64", "Microsoft.Data.Sqlite", "Native Library Loading", "Blogs", "Runtime ID", "Strace", "System.DllNotFoundException", "Troubleshooting"]
tags_normalized: ["dotnet 3dot1", "dotnet 5", "dotnet core", "alpine linux", "cislashcd", "coding", "containerization", "cross platform development", "datadog dotnet tracer", "environment variables", "ld debug", "ld library path", "libe sqlite3", "linux musl x64", "microsoftdotdatadotsqlite", "native library loading", "blogs", "runtime id", "strace", "systemdotdllnotfoundexception", "troubleshooting"]
---

Andrew Lock walks through the real-world debugging process for a library loading failure on Alpine Linux when running older .NET Core apps, clearly explaining the steps and ultimate resolution.<!--excerpt_end-->

# Solving Native Library Loading Issues for .NET Core on Alpine Linux

**Author: Andrew Lock**

## Introduction

Running .NET Core applications on different Linux distributions can sometimes lead to challenging native library loading issues. In this post, Andrew Lock details how he and his team investigated and resolved a problem with `Microsoft.Data.Sqlite` failing to load `libe_sqlite3` on Alpine Linux 3.17 with .NET Core 3.1 and .NET 5, by methodically debugging the runtime and system configuration.

## The Problem

After upgrading their CI/testing images to `alpine:3.17` to support .NET 10, the team encountered the following exception:

```
System.DllNotFoundException: Unable to load shared library 'e_sqlite3' or one of its dependencies
```

This only affected samples targeting `netcoreapp3.1` and `net5.0` using `Microsoft.Data.Sqlite`.

## Initial Investigation

- The same application worked correctly on `alpine:3.14`. The upgrade to `alpine:3.17` caused the failure.
- Running `ldd` on the library in question (`libe_sqlite3.so`) confirmed the dependencies were present and not missing.
- Tools such as `LD_DEBUG` and `strace` were used to gain more insight, but didn't provide clear clues since the library was loaded by .NET at runtime, not as a dynamic dependency.

## Working Solution

Manually setting `LD_LIBRARY_PATH` to include the directory with `libe_sqlite3.so` allowed the app to run successfully. This suggested the problem was not the library itself, but how .NET was resolving runtime IDs and searching for the correct native assets.

## Root Cause Analysis

- Old versions of .NET (3.1, 5.0) do not recognize newer versions of Alpine Linux (like 3.17) for runtime identifier (RID) mapping.
- As a consequence, the runtime uses a fallback RID (e.g., `linux-x64`), failing to find native libraries built for `linux-musl-x64`.
- Support for Alpine 3.17+ RIDs was only officially added in .NET 6 and above.
- The root issue was a missing mapping from modern Alpine versions to `linux-musl-x64` in old .NET Core runtime code.

## Permanent Fix

Set the environment variable `DOTNET_RUNTIME_ID=linux-musl-x64` before launching the application. Example:

```bash
DOTNET_RUNTIME_ID=linux-musl-x64 dotnet Samples.Microsoft.Data.Sqlite.dll
```

Alternatively, ensure `LD_LIBRARY_PATH` includes the directory containing the needed native library. For future-proofing, upgrade to .NET 7+ where these issues are resolved by default.

## Summary

- The error arises from lack of support for new Alpine Linux RIDs in legacy .NET runtimes.
- Diagnostic commands like `ldd`, `LD_DEBUG`, and `strace` help to isolate native library loading problems.
- Setting `DOTNET_RUNTIME_ID` explicitly guides the .NET runtime to use the correct assets.
- .NET 7 and newer include these fixes, but for older runtimes, setting the environment variable is essential.

---

For full details and troubleshooting steps, see the original post by Andrew Lock: [Fixing an old .NET Core native library loading issue on Alpine](https://www.andrewlock.net/).

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/fixing-an-old-dotnet-core-native-library-loading-issue-on-alpine/)
